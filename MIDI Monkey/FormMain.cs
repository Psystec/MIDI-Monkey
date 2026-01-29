using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MIDI_Monkey.Core;
using MIDI_Monkey.Services;
using MIDI_Monkey.Helpers;
using MIDI_Monkey.Utilities;
using MIDI_Monkey.UI;
using MIDI_Monkey.Models;

namespace MIDI_Monkey
{
    public partial class FormMain : Form
    {
        private const string AppVersion = "v7.0";

        // UI Helpers
        private DraggablePanelHelper _draggablePanelHelper;
        private ResizableFormHelper _resizableFormHelper;
        private VisualBoard _visualBoard;

        // Services
        private readonly MidiKeyMapService _keyMapService;
        private readonly MidiDeviceManager _midiDeviceManager;
        private readonly VersionCheckService _versionCheckService;
        private readonly ProcessWindowService _processWindowService;
        private MidiPlaybackEngine? _playbackEngine;
        private MidiChannelService _midiChannelService;

        // State
        private GlobalKeyboardHook? _globalKeyboardHook;
        private IntPtr _gameWindowHandle = IntPtr.Zero;
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _isPlaying = false;
        private bool _loading = true;

        public FormMain()
        {
            _loading = true;
            this.AutoScaleMode = AutoScaleMode.None;
            InitializeComponent();

            // Initialize services
            _keyMapService = new MidiKeyMapService();
            _midiDeviceManager = new MidiDeviceManager();
            _versionCheckService = new VersionCheckService();
            _processWindowService = new ProcessWindowService();
            _midiChannelService = new MidiChannelService();

            // Setup UI
            InitializeUIComponents();
            InitializeGlobalHotkeys();
            Settings.Init();

            // Load settings and data
            LoadSettings();
            LoadMidiKeyMapsAsync();
            LoadLastUsedFilesAsync();
        }

        private void InitializeUIComponents()
        {
            labelAppNameLabel.Text = $"MIDI Monkey {AppVersion}";
            Logging.SetRichTextBox(richTextBoxLog);

            _draggablePanelHelper = new DraggablePanelHelper(this, panelTop);
            _resizableFormHelper = new ResizableFormHelper(this, panelResizeHandle);
            _visualBoard = new VisualBoard(panelMidiNoteVisualizer);

            InitializeMidiDeviceList();
            ToolTipLoader.LoadToolTips(this);
        }

        private void InitializeGlobalHotkeys()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnGlobalKeyPressed;
            _globalKeyboardHook.HookKeyboard();
        }

        private async void FormMain_Load(object sender, EventArgs e)
        {
            Logging.DebugLog("Starting MIDI Monkey...");

            // Check for updates
            await CheckForUpdatesAsync();

            Logging.DebugLog("MIDI Monkey started successfully");
            _loading = false;
        }

        #region Version Check

        private async Task CheckForUpdatesAsync()
        {
            try
            {
                bool updateAvailable = await _versionCheckService.CheckForUpdatesAsync(AppVersion);
                if (updateAvailable)
                {
                    labelAppNameLabel.Text = $"MIDI Monkey {AppVersion} (UPDATE AVAILABLE)";
                }

            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error checking for updates: {ex.Message}");
            }
        }

        #endregion

        #region Settings Management

        private void LoadSettings()
        {
            checkBoxRepeatSong.Checked = Settings.settings.RepeatSong;
            checkBoxAlwaysOnTop.Checked = Settings.settings.AlwaysOnTop;
            trackBarTempo.Value = Settings.settings.Speed;
            labelTempo.Text = trackBarTempo.Value.ToString();
            trackBarModifierDelay.Value = Settings.settings.ModifierDelay;
            labelModifiedDelay.Text = trackBarModifierDelay.Value.ToString();
            this.TopMost = Settings.settings.AlwaysOnTop;
        }

        private async Task SaveSettingsAsync()
        {
            if (!_loading)
            {
                Settings.settings.RepeatSong = checkBoxRepeatSong.Checked;
                Settings.settings.AlwaysOnTop = checkBoxAlwaysOnTop.Checked;
                Settings.settings.Speed = trackBarTempo.Value;
                Settings.settings.ModifierDelay = trackBarModifierDelay.Value;
                this.TopMost = Settings.settings.AlwaysOnTop;
                await Settings.SaveSettingsAsync();
            }
        }

        #endregion

        #region MIDI KeyMap Management

        private async Task LoadMidiKeyMapsAsync()
        {
            var keyMapFiles = await _keyMapService.GetAvailableKeyMapsAsync();

            listBoxMIDIKeyMaps.Items.Clear();
            foreach (var file in keyMapFiles)
            {
                listBoxMIDIKeyMaps.Items.Add(file);
            }

            if (listBoxMIDIKeyMaps.Items.Count == 0)
            {
                Logging.ShowBlockingMessage("No MidiKeyMaps found. Please add some maps before launching the application.");
            }
            else if (!string.IsNullOrEmpty(Settings.settings.LastKeyMapProfile))
            {
                SelectListBoxItem(listBoxMIDIKeyMaps, Settings.settings.LastKeyMapProfile);
            }
        }

        private async void listBoxMIDIKeyMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxMIDIKeyMaps.SelectedItem == null) return;

            try
            {
                if (_isPlaying)
                {
                    StopPlayback();
                }

                string selectedKeyMap = listBoxMIDIKeyMaps.SelectedItem.ToString()!;
                await _keyMapService.LoadKeyMapAsync(selectedKeyMap);

                Settings.settings.LastKeyMapProfile = selectedKeyMap;
                await SaveSettingsAsync();

                //Logging.DebugLog($"Loaded MidiKeyMap: {selectedKeyMap}");
            }
            catch (Exception ex)
            {
                Logging.ShowBlockingMessage($"Failed to load MidiKeyMap: {ex.Message}");
            }
        }

        private void buttonEditKeyMaps_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedKeyMap = listBoxMIDIKeyMaps.SelectedItem?.ToString() ?? string.Empty;

                using (var editorForm = string.IsNullOrEmpty(selectedKeyMap)
                    ? new FormMidiKeyMapEditor()
                    : new FormMidiKeyMapEditor(selectedKeyMap))
                {
                    this.TopMost = false;
                    checkBoxAlwaysOnTop.Checked = false;

                    editorForm.ShowDialog();
                    _ = LoadMidiKeyMapsAsync();
                }
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error opening KeyMap Editor: {ex.Message}");
                Logging.ShowBlockingMessage($"Error opening KeyMap Editor: {ex.Message}");
            }
        }

        #endregion

        #region MIDI File Management

        private async Task LoadLastUsedFilesAsync()
        {
            if (!string.IsNullOrEmpty(Settings.settings.LastMidiFolder) &&
                System.IO.Directory.Exists(Settings.settings.LastMidiFolder))
            {
                await LoadMidiFilesAsync(Settings.settings.LastMidiFolder);
                SelectListBoxItem(listBoxMIDIFiles, System.IO.Path.GetFileName(Settings.settings.LastMidiFile));
            }
        }

        private async Task LoadMidiFilesAsync(string folderPath)
        {
            try
            {
                var midiFiles = await FileDialogHelper.GetMidiFilesFromDirectoryAsync(folderPath);

                listBoxMIDIFiles.Items.Clear();
                foreach (var file in midiFiles)
                {
                    listBoxMIDIFiles.Items.Add(file);
                }

                if (listBoxMIDIFiles.Items.Count > 0)
                {
                    Settings.settings.LastMidiFolder = folderPath;
                    await SaveSettingsAsync();
                    Logging.DebugLog($"Loaded {midiFiles.Length} MIDI files from {folderPath}");
                }
                else
                {
                    Logging.ShowBlockingMessage("No MIDI files found in the selected folder.");
                }
            }
            catch (Exception ex)
            {
                Logging.ShowBlockingMessage($"Error loading MIDI files: {ex.Message}");
            }
        }

        private void buttonSelectMIDIFolder_Click(object sender, EventArgs e)
        {
            var selectedFolder = FileDialogHelper.SelectMidiFolder(Settings.settings.LastMidiFolder);
            if (!string.IsNullOrEmpty(selectedFolder))
            {
                _ = LoadMidiFilesAsync(selectedFolder);
            }
        }

        private async void listBoxMIDIFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxMIDIFiles.SelectedItem == null) return;

            try
            {
                if (_isPlaying)
                {
                    StopPlayback();
                }

                string selectedMidiFile = listBoxMIDIFiles.SelectedItem.ToString()!;
                Settings.settings.LastMidiFile = System.IO.Path.Combine(Settings.settings.LastMidiFolder, selectedMidiFile);
                await SaveSettingsAsync();
                Logging.DebugLog($"Selected MIDI file: {selectedMidiFile}");
            }
            catch (Exception ex)
            {
                Logging.ShowBlockingMessage($"Failed to select MIDI file: {ex.Message}");
            }
        }

        #endregion

        #region MIDI Device Management

        private void InitializeMidiDeviceList()
        {
            var devices = _midiDeviceManager.GetAvailableDevices();

            if (devices.Count > 0)
            {
                comboBoxMIDIDevices.Enabled = true;
                buttonUseMidiDevice.Enabled = true;

                Logging.DebugLog($"{devices.Count} MIDI input devices found.");
                foreach (var deviceName in devices)
                {
                    comboBoxMIDIDevices.Items.Add(deviceName);
                    Logging.DebugLog($"Device: {deviceName}");
                }
                comboBoxMIDIDevices.SelectedIndex = 0;
            }
            else
            {
                comboBoxMIDIDevices.Items.Add("No MIDI Devices Found.");
                comboBoxMIDIDevices.SelectedIndex = 0;
                comboBoxMIDIDevices.Enabled = false;
                buttonUseMidiDevice.Enabled = false;
                Logging.DebugLog("No MIDI input devices found (You can use a MIDI Keyboard with MIDI Monkey).");
            }
        }

        private void buttonUseMidiDevice_Click(object sender, EventArgs e)
        {
            if (_midiDeviceManager.IsActive)
            {
                DisconnectMidiDevice();
                return;
            }

            if (comboBoxMIDIDevices.SelectedIndex < 0)
            {
                Logging.ShowBlockingMessage("Please select a valid MIDI device.");
                return;
            }

            if (_midiDeviceManager.Connect(comboBoxMIDIDevices.SelectedIndex))
            {
                _midiDeviceManager.MessageReceived += OnMidiMessageReceived;
                _midiDeviceManager.ErrorReceived += OnMidiErrorReceived;

                buttonUseMidiDevice.Text = "Disconnect MIDI";
                comboBoxMIDIDevices.Enabled = false;
            }
            else
            {
                Logging.ShowBlockingMessage("Failed to connect to MIDI device.");
            }
        }

        private void DisconnectMidiDevice()
        {
            _midiDeviceManager.MessageReceived -= OnMidiMessageReceived;
            _midiDeviceManager.ErrorReceived -= OnMidiErrorReceived;
            _midiDeviceManager.Disconnect();

            comboBoxMIDIDevices.Enabled = true;
            buttonUseMidiDevice.Text = "Use MIDI Device";
        }

        private void OnMidiErrorReceived(object? sender, MidiInMessageEventArgs e)
        {
            Logging.DebugLog($"MIDI Error: {e.RawMessage}");
        }

        private async void OnMidiMessageReceived(object? sender, MidiInMessageEventArgs e)
        {
            try
            {
                if (e.MidiEvent is not NoteEvent noteEvent) return;

                int noteNumber = noteEvent.NoteNumber;
                bool isNoteOn = e.MidiEvent.CommandCode == MidiCommandCode.NoteOn && noteEvent.Velocity > 0;
                bool isNoteOff = e.MidiEvent.CommandCode == MidiCommandCode.NoteOff ||
                                 (e.MidiEvent.CommandCode == MidiCommandCode.NoteOn && noteEvent.Velocity == 0);

                if (isNoteOff)
                {
                    _visualBoard.UnhighlightKey(noteNumber);
                    return;
                }

                if (isNoteOn)
                {
                    EnsureGameWindowHandle();

                    if (_keyMapService.TryGetValue(noteNumber, out var keys) && keys.Count > 0)
                    {
                        _ = _visualBoard.HighlightKey(noteNumber, 200);
                        await SendKeysAsync(keys);
                        Logging.DebugLog($"MIDI Key: {noteEvent.NoteName} ({noteNumber}) -> Keys: {string.Join(", ", keys)}");
                    }
                    else
                    {
                        _ = _visualBoard.HighlightKey(noteNumber, 100);
                        Logging.DebugLog($"MIDI Key not mapped: {noteEvent.NoteName} ({noteNumber})");
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error processing MIDI message: {ex.Message}");
            }
        }

        #endregion

        #region Playback Control

        private void OnGlobalKeyPressed(object? sender, KeyPressedEventArgs e)
        {
            if (e.Key == Keys.F5 && !_isPlaying)
            {
                if (TryGetGameWindowHandle())
                {
                    StartPlayback();
                }
            }
            else if (e.Key == Keys.F6 && _isPlaying)
            {
                StopPlayback();
            }
        }

        private void buttonPlaySong_Click(object sender, EventArgs e)
        {
            if (_isPlaying)
            {
                Logging.DebugLog("A song is already playing, please stop the current one first.");
            }
            else if (TryGetGameWindowHandle())
            {
                StartPlayback();
            }
        }

        private void buttonStopSong_Click(object sender, EventArgs e)
        {
            if (_isPlaying)
            {
                StopPlayback();
            }
            else
            {
                Logging.DebugLog("There is no song currently playing.");
            }
        }

        private void StartPlayback()
        {
            try
            {
                if (listBoxMIDIFiles.SelectedItem == null)
                {
                    Logging.ShowBlockingMessage("Please select a MIDI file first.");
                    return;
                }

                string selectedMidiFile = System.IO.Path.Combine(
                    Settings.settings.LastMidiFolder,
                    listBoxMIDIFiles.SelectedItem.ToString()!);

                Logging.DebugLog($"Playing Song: {selectedMidiFile}");

                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
                _isPlaying = true;

                InitializePlaybackEngine();

                _ = _playbackEngine!.PlayMidiAsync(selectedMidiFile, _cancellationTokenSource.Token)
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            Logging.DebugLog($"Playback error: {t.Exception?.InnerException?.Message}");
                        }

                        this.BeginInvoke(new Action(() =>
                        {
                            _isPlaying = false;
                            Logging.DebugLog("Playback completed or was stopped.");
                        }));
                    });
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error starting playback: {ex.Message}");
                _isPlaying = false;
            }
        }

        private void StopPlayback()
        {
            try
            {
                Logging.DebugLog("Stopping song...");
                _cancellationTokenSource?.Cancel();
                _playbackEngine?.ReleaseAllModifiers();
                _isPlaying = false;
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error stopping playback: {ex.Message}");
            }
        }

        private void InitializePlaybackEngine()
        {
            _playbackEngine = new MidiPlaybackEngine(
                midiKeyMapService: _keyMapService,
                sendKeyAction: Win32Api.SendKey,
                getTargetWindowHandle: () => _gameWindowHandle,
                getTempoAdjustment: () => GetTrackBarValueSafe(trackBarTempo),
                getModifierDelay: () => GetTrackBarValueSafe(trackBarModifierDelay),
                highlightKeyAction: (note, delay) => _visualBoard.HighlightKey(note, delay),
                getRepeatSetting: () => checkBoxRepeatSong.Checked,
                getChannelSettings: () => _midiChannelService
            );
        }

        #endregion

        #region Window and Input Management

        private bool TryGetGameWindowHandle()
        {
            _gameWindowHandle = _processWindowService.GetWindowHandle(_keyMapService.CurrentApplicationName);

            if (_gameWindowHandle == IntPtr.Zero)
            {
                Logging.ShowBlockingMessage($"Application '{_keyMapService.CurrentApplicationName}' not found. Make sure it is running.");
                return false;
            }

            return true;
        }

        private void EnsureGameWindowHandle()
        {
            if (_gameWindowHandle == IntPtr.Zero)
            {
                _gameWindowHandle = _processWindowService.GetWindowHandle(_keyMapService.CurrentApplicationName, silent: true);
            }
        }

        private async Task SendKeysAsync(List<VirtualKey> keys)
        {
            var modifierDelay = GetTrackBarValueSafe(trackBarModifierDelay);

            // Handle modifiers
            if (keys.Count > 1)
            {
                // Press modifiers
                for (int i = 0; i < keys.Count - 1; i++)
                {
                    Win32Api.SendKey(keys[i], true, _gameWindowHandle);
                }

                if (modifierDelay > 0)
                {
                    await Task.Delay(modifierDelay);
                }
            }

            // Press and release the main key
            var mainKey = keys[^1];
            Win32Api.SendKey(mainKey, true, _gameWindowHandle);
            Win32Api.SendKey(mainKey, false, _gameWindowHandle);

            // Release modifiers
            if (keys.Count > 1)
            {
                for (int i = keys.Count - 2; i >= 0; i--)
                {
                    Win32Api.SendKey(keys[i], false, _gameWindowHandle);
                }
            }
        }

        #endregion

        #region UI Event Handlers

        private void pictureBoxPayPal_Click(object sender, EventArgs e)
        {
            Common.OpenDonationPage();
        }

        private void labelPayPalLabel_Click(object sender, EventArgs e)
        {
            Common.OpenDonationPage();
        }

        private async void checkBoxRepeatSong_CheckedChanged(object sender, EventArgs e)
        {
            await SaveSettingsAsync();
        }

        private async void checkBoxAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            await SaveSettingsAsync();
        }

        private async void trackBarTempo_Scroll(object sender, EventArgs e)
        {
            labelTempo.Text = trackBarTempo.Value.ToString();
            await SaveSettingsAsync();
        }

        private async void trackBarModifierDelay_Scroll(object sender, EventArgs e)
        {
            labelModifiedDelay.Text = trackBarModifierDelay.Value.ToString();
            await SaveSettingsAsync();
        }

        private void buttonClose_MouseClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void pictureBoxDiscord_MouseClick(object sender, MouseEventArgs e)
        {
            Common.OpenDiscordPage();
        }

        private void pictureBoxSignalApp_Click(object sender, MouseEventArgs e)
        {
            Common.OpenSignalMIDIPage();
        }

        private void panelTop_Paint(object sender, PaintEventArgs e)
        {
            // Paint event for panel
        }

        #endregion

        #region Utility Methods

        private void SelectListBoxItem(ListBox listBox, string? itemName)
        {
            if (string.IsNullOrEmpty(itemName)) return;

            int index = listBox.Items.IndexOf(itemName);
            if (index != -1)
            {
                listBox.SelectedIndex = index;
            }
            else if (listBox.Items.Count > 0)
            {
                listBox.SelectedIndex = 0;
            }
        }

        private int GetTrackBarValueSafe(TrackBar trackBar)
        {
            if (trackBar.InvokeRequired)
            {
                return (int)trackBar.Invoke(new Func<int>(() => trackBar.Value));
            }
            return trackBar.Value;
        }

        #endregion

        #region Form Lifecycle

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DisconnectMidiDevice();
            StopPlayback();

            _globalKeyboardHook?.UnhookKeyboard();
            _cancellationTokenSource?.Dispose();
            _midiDeviceManager?.Dispose();

            base.OnFormClosing(e);
        }

        #endregion

        #region MIDI Channel Management

        private void checkBoxMidiChannel_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox cb)
            {
                int channel = Convert.ToInt32(cb.Tag);
                bool enabled = cb.Checked;

                _midiChannelService.SetChannel(channel, enabled);
            }
        }

        #endregion

    }
}