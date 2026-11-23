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
    /// <summary>
    /// Plays a MIDI file by sending key presses to the game.
    ///
    /// Timing design:
    /// - Playback runs on its own high priority thread, so UI work (scrolling, repainting,
    ///   logging) can no longer delay notes.
    /// - Every note's play time is calculated up front from the full tempo map, and the engine
    ///   waits until a stopwatch reaches that absolute time. Errors no longer add up note after note.
    /// - Notes that start at the same time (chords) are sent together in one SendInput call.
    /// - Keys are only sent while the game has focus.
    /// </summary>
    public class MidiPlaybackEngine
    {
        // If the engine falls this far behind (e.g. the PC stalled), skip ahead instead of
        // firing a burst of late notes.
        private const double MaxLatenessMs = 250.0;

        private readonly Dictionary<VirtualKey, bool> _activeModifiers = new Dictionary<VirtualKey, bool>();
        private readonly object _modifierLock = new object();

        private readonly MidiKeyMapService _midiKeyMapService;
        private readonly Func<bool> _isTargetFocused;
        private readonly Func<int> _getTempoAdjustment;
        private readonly Func<int> _getModifierDelay;
        private readonly Action<int, int> _highlightKeyAction;
        private readonly Func<bool> _getRepeatSetting;
        private readonly Func<MidiChannelService> _getChannelSettings;

        public MidiPlaybackEngine(
            MidiKeyMapService midiKeyMapService,
            Func<bool> isTargetFocused,
            Func<int> getTempoAdjustment,
            Func<int> getModifierDelay,
            Action<int, int> highlightKeyAction,
            Func<bool> getRepeatSetting,
            Func<MidiChannelService> getChannelSettings)
        {
            _midiKeyMapService = midiKeyMapService ?? throw new ArgumentNullException(nameof(midiKeyMapService));
            _isTargetFocused = isTargetFocused ?? throw new ArgumentNullException(nameof(isTargetFocused));
            _getTempoAdjustment = getTempoAdjustment;
            _getModifierDelay = getModifierDelay;
            _highlightKeyAction = highlightKeyAction;
            _getRepeatSetting = getRepeatSetting;
            _getChannelSettings = getChannelSettings;
        }

        /// <summary>
        /// One note-on with its exact play time in milliseconds from the start of the song.
        /// </summary>
        private readonly struct ScheduledNote
        {
            public ScheduledNote(long tick, double timeMs, int noteNumber, string noteName, int channel)
            {
                Tick = tick;
                TimeMs = timeMs;
                NoteNumber = noteNumber;
                NoteName = noteName;
                Channel = channel;
            }

            public long Tick { get; }
            public double TimeMs { get; }
            public int NoteNumber { get; }
            public string NoteName { get; }
            public int Channel { get; } // 1-16
        }

        /// <summary>
        /// Starts playback on a dedicated thread. The task completes when the song ends,
        /// and is cancelled when the token is cancelled.
        /// </summary>
        public Task PlayMidiAsync(string midiFilePath, CancellationToken token)
        {
            if (string.IsNullOrEmpty(midiFilePath))
            {
                throw new ArgumentException("MIDI file path cannot be empty", nameof(midiFilePath));
            }

            var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            var thread = new Thread(() =>
            {
                try
                {
                    PlayMidi(midiFilePath, token);
                    completion.TrySetResult();
                }
                catch (OperationCanceledException)
                {
                    completion.TrySetCanceled(token);
                }
                catch (Exception ex)
                {
                    completion.TrySetException(ex);
                }
            })
            {
                IsBackground = true,
                Priority = ThreadPriority.Highest,
                Name = "MIDI Monkey Playback"
            };

            thread.Start();
            return completion.Task;
        }

        private void PlayMidi(string midiFilePath, CancellationToken token)
        {
            using var timer = new PreciseTimer();

            try
            {
                var midiFile = new MidiFile(midiFilePath, false);
                var notes = BuildSchedule(midiFile);
                Logging.DebugLog($"Total MIDI notes to play: {notes.Count}");

                bool playOnce = true;

                while ((playOnce || _getRepeatSetting()) && !token.IsCancellationRequested)
                {
                    PlayOnce(notes, timer, token);
                    playOnce = false;
                }

                ReleaseAllModifiers();
                Logging.DebugLog("Song Ended");
            }
            catch (OperationCanceledException)
            {
                Logging.DebugLog("Playback was cancelled");
                ReleaseAllModifiers();
                throw;
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error during MIDI playback: {ex.Message}");
                ReleaseAllModifiers();
                throw;
            }
        }

        private void PlayOnce(List<ScheduledNote> notes, PreciseTimer timer, CancellationToken token)
        {
            var stopwatch = Stopwatch.StartNew();
            bool? wasFocused = null;

            // Position on our own timeline (ms). The Speed slider adjusts each gap, so this is
            // advanced group by group instead of using the raw note time directly.
            double timelineMs = 0;
            double previousNoteTimeMs = 0;
            bool first = true;

            int index = 0;
            while (index < notes.Count)
            {
                token.ThrowIfCancellationRequested();

                // Collect every note that starts on the same tick (a chord).
                long groupTick = notes[index].Tick;
                int groupEnd = index;
                while (groupEnd < notes.Count && notes[groupEnd].Tick == groupTick)
                {
                    groupEnd++;
                }

                double noteTimeMs = notes[index].TimeMs;
                double gapMs = first ? noteTimeMs : noteTimeMs - previousNoteTimeMs;
                first = false;
                previousNoteTimeMs = noteTimeMs;

                // Same behaviour as before: each Speed step shortens the gap between notes by 5ms.
                double adjustedGap = Math.Max(0, gapMs - _getTempoAdjustment() * 5.0);
                timelineMs += adjustedGap;

                // If we are badly behind (PC stall), move the timeline instead of bursting notes.
                double now = stopwatch.Elapsed.TotalMilliseconds;
                if (now - timelineMs > MaxLatenessMs)
                {
                    timelineMs = now;
                }

                timer.WaitUntil(stopwatch, timelineMs, token);

                // Only press keys while the game has focus, otherwise keys go to other windows.
                bool focused = _isTargetFocused();
                if (focused != wasFocused)
                {
                    if (!focused)
                    {
                        ReleaseAllModifiers();
                        Logging.DebugLog("Game window is not focused - not sending keys until it is focused again.");
                    }
                    else if (wasFocused == false)
                    {
                        Logging.DebugLog("Game window focused again - sending keys.");
                    }
                    wasFocused = focused;
                }

                PlayGroup(notes, index, groupEnd, focused, stopwatch, timer, ref timelineMs, token);

                index = groupEnd;
            }

            Logging.DebugLog($"Playback completed in {stopwatch.ElapsedMilliseconds}ms");
        }

        private void PlayGroup(List<ScheduledNote> notes, int start, int end, bool focused,
            Stopwatch stopwatch, PreciseTimer timer, ref double timelineMs, CancellationToken token)
        {
            var channelSettings = _getChannelSettings();

            // Keys that share the same modifiers can be pressed at the exact same moment.
            // Notes that need different modifiers must be pressed one after another.
            var pending = new List<(List<VirtualKey> modifiers, VirtualKey key)>();

            for (int i = start; i < end; i++)
            {
                var note = notes[i];

                if (!channelSettings.IsChannelEnabled(note.Channel))
                {
                    continue;
                }

                if (!_midiKeyMapService.TryGetValue(note.NoteNumber, out var keys) || keys.Count == 0)
                {
                    continue;
                }

                _highlightKeyAction(note.NoteNumber, 100);

                if (!focused)
                {
                    continue;
                }

                pending.Add((keys.Take(keys.Count - 1).ToList(), keys[^1]));
                Logging.DebugLog($"Key Down: {note.NoteName} ({note.NoteNumber}) CH:{note.Channel} ({string.Join(", ", keys)})");
            }

            if (pending.Count == 0)
            {
                return;
            }

            foreach (var modifierGroup in pending.GroupBy(p => string.Join(",", p.modifiers)))
            {
                var modifiers = modifierGroup.First().modifiers;

                if (HandleModifiers(modifiers))
                {
                    int modifierDelay = _getModifierDelay();
                    if (modifierDelay > 0)
                    {
                        // Give the game time to register the modifier, and push the timeline
                        // back by the same amount so the following notes keep their spacing.
                        double target = stopwatch.Elapsed.TotalMilliseconds + modifierDelay;
                        timer.WaitUntil(stopwatch, target, token);
                        timelineMs = Math.Max(timelineMs, target);
                    }
                }

                var inputs = new List<(VirtualKey key, bool keyDown)>();
                foreach (var (_, key) in modifierGroup)
                {
                    inputs.Add((key, true));
                }
                foreach (var (_, key) in modifierGroup)
                {
                    inputs.Add((key, false));
                }

                Win32Api.SendKeys(inputs);
            }
        }

        /// <summary>
        /// Releases any modifier keys the engine is holding down.
        /// Safe to call from any thread.
        /// </summary>
        public void ReleaseAllModifiers()
        {
            lock (_modifierLock)
            {
                foreach (var modifier in _activeModifiers.Keys.ToList())
                {
                    if (_activeModifiers[modifier])
                    {
                        Win32Api.SendKey(modifier, false);
                        _activeModifiers[modifier] = false;
                    }
                }
            }
        }

        /// <summary>
        /// Presses/releases modifiers so exactly <paramref name="currentModifiers"/> are held.
        /// Returns true if anything changed.
        /// </summary>
        private bool HandleModifiers(List<VirtualKey> currentModifiers)
        {
            lock (_modifierLock)
            {
                bool modifiersChanged = false;

                foreach (var modifier in _activeModifiers.Keys.ToList())
                {
                    if (_activeModifiers[modifier] && !currentModifiers.Contains(modifier))
                    {
                        Win32Api.SendKey(modifier, false);
                        _activeModifiers[modifier] = false;
                        modifiersChanged = true;
                    }
                }

                foreach (var modifier in currentModifiers)
                {
                    if (!_activeModifiers.TryGetValue(modifier, out bool isDown) || !isDown)
                    {
                        Win32Api.SendKey(modifier, true);
                        _activeModifiers[modifier] = true;
                        modifiersChanged = true;
                    }
                }

                return modifiersChanged;
            }
        }

        /// <summary>
        /// Builds the list of note-ons with exact millisecond times, honouring every tempo
        /// change in the file (previously only the first tempo was used).
        /// </summary>
        private static List<ScheduledNote> BuildSchedule(MidiFile midiFile)
        {
            int ticksPerQuarterNote = midiFile.DeltaTicksPerQuarterNote;
            if (ticksPerQuarterNote <= 0)
            {
                ticksPerQuarterNote = 480;
            }

            var tempoChanges = new List<(long tick, int microsecondsPerQuarter)>();
            var noteOns = new List<(long tick, NoteOnEvent noteOn)>();

            for (int trackIndex = 0; trackIndex < midiFile.Events.Tracks; trackIndex++)
            {
                long absoluteTick = 0;
                foreach (MidiEvent midiEvent in midiFile.Events[trackIndex])
                {
                    absoluteTick += midiEvent.DeltaTime;

                    if (midiEvent is TempoEvent tempoEvent)
                    {
                        tempoChanges.Add((absoluteTick, tempoEvent.MicrosecondsPerQuarterNote));
                    }
                    else if (midiEvent is NoteOnEvent noteOn && noteOn.Velocity > 0)
                    {
                        noteOns.Add((absoluteTick, noteOn));
                    }
                }
            }

            // Stable sort by tick keeps the original track order for notes on the same tick.
            tempoChanges = tempoChanges.OrderBy(t => t.tick).ToList();
            noteOns = noteOns.OrderBy(n => n.tick).ToList();

            var schedule = new List<ScheduledNote>(noteOns.Count);

            int tempo = 500000; // MIDI default: 120 BPM
            long segmentStartTick = 0;
            double segmentStartMs = 0;
            int tempoIndex = 0;

            foreach (var (tick, noteOn) in noteOns)
            {
                // Apply every tempo change at or before this note.
                while (tempoIndex < tempoChanges.Count && tempoChanges[tempoIndex].tick <= tick)
                {
                    long changeTick = tempoChanges[tempoIndex].tick;
                    segmentStartMs += TicksToMs(changeTick - segmentStartTick, tempo, ticksPerQuarterNote);
                    segmentStartTick = changeTick;
                    tempo = tempoChanges[tempoIndex].microsecondsPerQuarter;
                    tempoIndex++;
                }

                double timeMs = segmentStartMs + TicksToMs(tick - segmentStartTick, tempo, ticksPerQuarterNote);
                schedule.Add(new ScheduledNote(tick, timeMs, noteOn.NoteNumber, noteOn.NoteName, noteOn.Channel));
            }

            return schedule;
        }

        private static double TicksToMs(long ticks, int microsecondsPerQuarter, int ticksPerQuarterNote)
        {
            return ticks * (double)microsecondsPerQuarter / ticksPerQuarterNote / 1000.0;
        }
    }
}
