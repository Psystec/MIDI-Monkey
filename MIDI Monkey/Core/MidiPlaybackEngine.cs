using MIDI_Monkey.Models;
using MIDI_Monkey.Services;
using MIDI_Monkey.Utilities;
using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MIDI_Monkey.Core
{
    public class MidiPlaybackEngine
    {
        private Dictionary<VirtualKey, bool> _activeModifiers = new Dictionary<VirtualKey, bool>();
        private readonly MidiKeyMapService _midiKeyMapService;
        private readonly Action<VirtualKey, bool, IntPtr> _sendKeyAction;
        private readonly Func<IntPtr> _getTargetWindowHandle;
        private readonly Func<int> _getTempoAdjustment;
        private readonly Func<int> _getModifierDelay;
        private readonly Func<int, int, Task> _highlightKeyAction;
        private readonly Func<bool> _getRepeatSetting;
        private Func<MidiChannelService> _getChannelSettings;

        public MidiPlaybackEngine(
            MidiKeyMapService midiKeyMapService,
            Action<VirtualKey, bool, IntPtr> sendKeyAction,
            Func<IntPtr> getTargetWindowHandle,
            Func<int> getTempoAdjustment,
            Func<int> getModifierDelay,
            Func<int, int, Task> highlightKeyAction,
            Func<bool> getRepeatSetting,
            Func<MidiChannelService> getChannelSettings)
        {
            _midiKeyMapService = midiKeyMapService ?? throw new ArgumentNullException(nameof(midiKeyMapService));
            _sendKeyAction = sendKeyAction;
            _getTargetWindowHandle = getTargetWindowHandle ?? throw new ArgumentNullException(nameof(getTargetWindowHandle));
            _getTempoAdjustment = getTempoAdjustment;
            _getModifierDelay = getModifierDelay;
            _highlightKeyAction = highlightKeyAction;
            _getRepeatSetting = getRepeatSetting;
            _getChannelSettings = getChannelSettings;
        }

        public async Task PlayMidiAsync(string midiFilePath, CancellationToken token)
        {
            if (string.IsNullOrEmpty(midiFilePath))
            {
                throw new ArgumentException("MIDI file path cannot be empty", nameof(midiFilePath));
            }

            try
            {
                MidiFile midiFile = new MidiFile(midiFilePath, false);
                int ticksPerQuarterNote = midiFile.DeltaTicksPerQuarterNote;
                int tempo = GetInitialTempo(midiFile);

                var allEvents = CollectMidiEvents(midiFile);
                Logging.DebugLog($"Total MIDI events to play: {allEvents.Count}");

                bool playOnce = true;

                while ((_getRepeatSetting() || playOnce) && !token.IsCancellationRequested)
                {
                    int lastTime = 0;
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    foreach (var (midiEvent, absoluteTime) in allEvents)
                    {
                        if (token.IsCancellationRequested)
                        {
                            Logging.DebugLog("Playback cancelled");
                            return;
                        }

                        if (midiEvent is NoteOnEvent noteOn && noteOn.Velocity > 0)
                        {

                            // Check if this channel is enabled
                            var channelSettings = _getChannelSettings();
                            int midiChannel = noteOn.Channel + 1;

                            if (!channelSettings.IsChannelEnabled(midiChannel))
                            {
                                // Skip this note, channel is disabled
                                Logging.DebugLog($"Skipping {noteOn.NoteName} {noteOn.NoteNumber} on channel {midiChannel} (disabled)");
                                continue;
                            }

                            if (_midiKeyMapService.TryGetValue(noteOn.NoteNumber, out var keys))
                            {
                                int delay = CalculateDelay(absoluteTime, lastTime, tempo, ticksPerQuarterNote);
                                await Task.Delay(Math.Max(1, delay), token);

                                lastTime = absoluteTime;

                                _ = _highlightKeyAction(noteOn.NoteNumber, Math.Max(100, delay));

                                bool modifiersChanged = HandleModifiers(keys);

                                if (modifiersChanged)
                                {
                                    int modifierDelay = _getModifierDelay();
                                    if (modifierDelay > 0)
                                    {
                                        Logging.DebugLog($"Modifier change detected - delaying {modifierDelay}ms");
                                        await Task.Delay(modifierDelay, token);
                                    }
                                }

                                if (keys.Count > 0)
                                {
                                    var hwnd = _getTargetWindowHandle();
                                    var actualKey = keys.Last();
                                    _sendKeyAction(actualKey, true, hwnd);
                                    _sendKeyAction(actualKey, false, hwnd);
                                }

                                Logging.DebugLog($"Key Down: {noteOn.NoteName} {noteOn.NoteNumber} CH:{midiChannel} ({string.Join(", ", keys)})");
                            }
                        }

                        if (token.IsCancellationRequested)
                        {
                            Logging.DebugLog("Playback cancelled during note processing");
                            return;
                        }
                    }

                    stopwatch.Stop();
                    Logging.DebugLog($"Playback completed in {stopwatch.ElapsedMilliseconds}ms");

                    playOnce = false;
                }

                HandleModifiers(new List<VirtualKey>());
                Logging.DebugLog("Song Ended");
            }
            catch (OperationCanceledException)
            {
                Logging.DebugLog("Playback was cancelled");
                HandleModifiers(new List<VirtualKey>());
                throw;
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error during MIDI playback: {ex.Message}");
                throw;
            }
        }

        public void ReleaseAllModifiers()
        {
            var hwnd = _getTargetWindowHandle();
            foreach (var modifier in _activeModifiers.Keys.ToList())
            {
                if (_activeModifiers[modifier])
                {
                    _sendKeyAction(modifier, false, hwnd);
                    _activeModifiers[modifier] = false;
                }
            }
        }

        private int GetInitialTempo(MidiFile midiFile)
        {
            foreach (var track in midiFile.Events)
            {
                var tempoEvent = track.OfType<TempoEvent>().FirstOrDefault();
                if (tempoEvent != null)
                {
                    return tempoEvent.MicrosecondsPerQuarterNote;
                }
            }

            return 500000;
        }

        private List<(MidiEvent midiEvent, int absoluteTime)> CollectMidiEvents(MidiFile midiFile)
        {
            var allEvents = new List<(MidiEvent, int)>();
            int[] absoluteTimes = new int[midiFile.Tracks];

            for (int trackIndex = 0; trackIndex < midiFile.Events.Tracks; trackIndex++)
            {
                foreach (MidiEvent midiEvent in midiFile.Events[trackIndex])
                {
                    absoluteTimes[trackIndex] += midiEvent.DeltaTime;

                    if (midiEvent is NoteOnEvent noteOnEvent && noteOnEvent.Velocity > 0)
                    {
                        allEvents.Add((midiEvent, absoluteTimes[trackIndex]));
                    }
                }
            }

            allEvents.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            return allEvents;
        }

        private int CalculateDelay(int absoluteTime, int lastTime, int tempo, int ticksPerQuarterNote)
        {
            int delay = (absoluteTime - lastTime) * (tempo / ticksPerQuarterNote) / 1000;
            delay += _getTempoAdjustment() * -5;
            return Math.Max(0, delay);
        }

        private bool HandleModifiers(List<VirtualKey> keys)
        {
            bool modifiersChanged = false;
            var hwnd = _getTargetWindowHandle();

            if (keys == null || keys.Count <= 1)
            {
                foreach (var modifier in _activeModifiers.Keys.ToList())
                {
                    if (_activeModifiers[modifier])
                    {
                        _sendKeyAction(modifier, false, IntPtr.Zero);
                        _sendKeyAction(modifier, false, hwnd);
                        _activeModifiers[modifier] = false;
                        modifiersChanged = true;
                    }
                }
                return modifiersChanged;
            }

            var currentModifiers = keys.Take(keys.Count - 1).ToList();

            foreach (var modifier in _activeModifiers.Keys.ToList())
            {
                if (_activeModifiers[modifier] && !currentModifiers.Contains(modifier))
                {
                    _sendKeyAction(modifier, false, hwnd);
                    _activeModifiers[modifier] = false;
                    modifiersChanged = true;
                }
            }

            foreach (var modifier in currentModifiers)
            {
                if (!_activeModifiers.ContainsKey(modifier))
                {
                    _activeModifiers[modifier] = false;
                }

                if (!_activeModifiers[modifier])
                {
                    _sendKeyAction(modifier, true, hwnd);
                    _activeModifiers[modifier] = true;
                    modifiersChanged = true;
                }
            }

            return modifiersChanged;
        }
    }
}
