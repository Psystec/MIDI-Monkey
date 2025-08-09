using NAudio.Midi;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowsInput.Native;

namespace MIDI_Monkey
{
    public partial class FormMain : Form
    {
        private const string _appVersion = "v6.4";
        private DraggablePanelHelper draggablePanelHelper;
        private ResizableFormHelper resizableFormHelper;

        private GlobalKeyboardHook _globalKeyboardHook;
        private MidiIn midiIn;
        private bool isMidiDeviceActive = false;
        private IntPtr gameWindowHandle = IntPtr.Zero;
        private CancellationTokenSource cancellationTokenSource;
        private bool isPlaying = false;

        private VisualBoard visualBoard;

        public FormMain()
        {
            this.AutoScaleMode = AutoScaleMode.None;
            InitializeComponent();
            labelAppNameLabel.Text = $"MIDI Monkey {_appVersion}";
            Logging.SetRichTextBox(richTextBoxLog);
            draggablePanelHelper = new DraggablePanelHelper(this, panelTop);
            resizableFormHelper = new ResizableFormHelper(this, panelResizeHandle);
            InitializeMidiInput();
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnGlobalKeyPressed;
            _globalKeyboardHook.HookKeyboard();
            Settings.Init();

            visualBoard = new VisualBoard(panelMidiNoteVisualizer);

            ToolTipLoader.LoadToolTips(this);
        }

        private async void FormMain_Load(object sender, EventArgs e)
        {
            Logging.DebugLog($"Starting MIDI Monkey.");

            GitHubVersionChecker ch = new GitHubVersionChecker();
            bool updateAvaialbe = await ch.Check(_appVersion);
            if (updateAvaialbe)
            {
                labelAppNameLabel.Text = $"MIDI Monkey {_appVersion} (UPDATE AVAILABLE)";
            }

            LoadSettings();

            await LoadMidiKeyMapsAsync();
            SelectLastUsedItem(listBoxMIDIKeyMaps, Settings.settings.LastKeyMapProfile);

            if (!string.IsNullOrEmpty(Settings.settings.LastMidiFolder) &&
                Directory.Exists(Settings.settings.LastMidiFolder))
            {
                await LoadMidiFilesAsync(Settings.settings.LastMidiFolder);
                SelectLastUsedItem(listBoxMIDIFiles, Path.GetFileName(Settings.settings.LastMidiFile));
            }
        }

        private void LoadSettings()
        {
            checkBoxRepeatSong.Checked = Settings.settings.RepeatSong;
            checkBoxAlwaysOnTop.Checked = Settings.settings.AlwaysOnTop;
            trackBarTempo.Value = Settings.settings.Speed;
            labelTempo.Text = GetTrackBarValueSafe(trackBarTempo).ToString();
            trackBarModifierDelay.Value = Settings.settings.ModifierDelay;
            labelModifiedDelay.Text = GetTrackBarValueSafe(trackBarModifierDelay).ToString();
            this.TopMost = Settings.settings.AlwaysOnTop;
        }

        private async Task SaveSettingsAsync()
        {
            Settings.settings.RepeatSong = checkBoxRepeatSong.Checked;
            Settings.settings.AlwaysOnTop = checkBoxAlwaysOnTop.Checked;
            Settings.settings.Speed = trackBarTempo.Value;
            Settings.settings.ModifierDelay = trackBarModifierDelay.Value;
            this.TopMost = Settings.settings.AlwaysOnTop;
            await Settings.SaveSettingsAsync();
        }

        private async Task LoadMidiKeyMapsAsync()
        {
            var midiKeyMapFiles = await MidiKeyMap.GetMidiKeyMapsAsync();

            listBoxMIDIKeyMaps.Items.Clear();
            foreach (var file in midiKeyMapFiles)
            {
                listBoxMIDIKeyMaps.Items.Add(file);
            }

            if (listBoxMIDIKeyMaps.Items.Count == 0)
            {
                Common.ShowErrorMessage("No MidiKeyMaps found. Please add some maps before launching the application.");
            }
        }

        private async Task LoadMidiFilesAsync(string folderPath)
        {
            try
            {
                string[] midiFiles = Directory.GetFiles(folderPath, "*.mid")
                    .Union(Directory.GetFiles(folderPath, "*.midi"))
                    .ToArray();

                listBoxMIDIFiles.Items.Clear();
                foreach (string file in midiFiles)
                {
                    listBoxMIDIFiles.Items.Add(Path.GetFileName(file));
                }

                if (listBoxMIDIFiles.Items.Count > 0)
                {
                    Settings.settings.LastMidiFolder = folderPath;
                    await SaveSettingsAsync();
                    Logging.DebugLog($"Loaded {midiFiles.Length} MIDI files from {folderPath}");
                }
                else
                {
                    Common.ShowErrorMessage("No MIDI files found in the selected folder.");
                }
            }
            catch (Exception ex)
            {
                Common.ShowErrorMessage($"Error loading MIDI files from last folder: {ex.Message}");
            }
        }

        private void SelectLastUsedItem(ListBox listBox, string lastUsedItem)
        {
            if (!string.IsNullOrEmpty(lastUsedItem))
            {
                int index = listBox.Items.IndexOf(lastUsedItem);
                if (index != -1)
                {
                    listBox.SelectedIndex = index;
                }
                else
                {
                    listBox.SelectedIndex = 0;
                }
            }
            else if (listBox.Items.Count > 0)
            {
                listBox.SelectedIndex = 0;
            }
        }

        private void pictureBoxPayPal_Click(object sender, EventArgs e)
        {
            Common.OpenDonationPage();
        }

        private void labelPayPalLabel_Click(object sender, EventArgs e)
        {
            Common.OpenDonationPage();
        }

        private void buttonClose_MouseClick(object sender, MouseEventArgs e)
        {
            ReleaseModifiers();
            _globalKeyboardHook.UnhookKeyboard();
            Application.Exit();
        }

        private void InitializeMidiInput()
        {
            int deviceCount = MidiIn.NumberOfDevices;
            if (deviceCount > 0)
            {
                comboBoxMIDIDevices.Enabled = true;
                buttonUseMidiDevice.Enabled = true;
                Logging.DebugLog($"{deviceCount} MIDI input devices found.");
                for (int i = 0; i < deviceCount; i++)
                {
                    string deviceName = MidiIn.DeviceInfo(i).ProductName;
                    comboBoxMIDIDevices.Items.Add(deviceName);
                    Logging.DebugLog($"Device {i + 1}: {deviceName}");
                }
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

        private async void listBoxMIDIKeyMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxMIDIKeyMaps.SelectedItem != null)
            {
                try
                {
                    if (isPlaying == true)
                    {
                        StopPlayback();
                    }

                    string selectedKeyMap = listBoxMIDIKeyMaps.SelectedItem.ToString();
                    Settings.settings.LastKeyMapProfile = selectedKeyMap;
                    await SaveSettingsAsync();
                    await MidiKeyMap.LoadFromJsonAsync(selectedKeyMap);
                    Logging.DebugLog($"Loaded MidiKeyMap: {selectedKeyMap}");
                }
                catch (Exception ex)
                {
                    Common.ShowErrorMessage($"Failed to load MidiKeyMap: {ex.Message}");
                }
            }
        }

        private void buttonSelectMIDIFolder_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "MIDI files (*.mid)|*.mid|All MIDI files (*.mid;*.midi)|*.mid;*.midi|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "Select MIDI File or Folder";
                openFileDialog.CheckFileExists = true;
                openFileDialog.ValidateNames = true;

                if (!string.IsNullOrEmpty(Settings.settings.LastMidiFolder) &&
                    Directory.Exists(Settings.settings.LastMidiFolder))
                {
                    openFileDialog.InitialDirectory = Settings.settings.LastMidiFolder;
                }

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFolder = Path.GetDirectoryName(openFileDialog.FileName);
                    _ = LoadMidiFilesAsync(selectedFolder);
                }
            }
        }

        private async void listBoxMIDIFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxMIDIFiles.SelectedItem != null)
            {
                try
                {
                    if (isPlaying == true)
                    {
                        StopPlayback();
                    }

                    string selectedMidiFile = listBoxMIDIFiles.SelectedItem.ToString();
                    Settings.settings.LastMidiFile = Path.Combine(Settings.settings.LastMidiFolder, selectedMidiFile);
                    await SaveSettingsAsync();
                    Logging.DebugLog($"Selected MIDI file: {selectedMidiFile}");
                }
                catch (Exception ex)
                {
                    Common.ShowErrorMessage($"Failed to select MIDI file: {ex.Message}");
                }
            }
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
            labelTempo.Text = GetTrackBarValueSafe(trackBarTempo).ToString();
            await SaveSettingsAsync();
        }

        private async void trackBarModifierDelay_Scroll(object sender, EventArgs e)
        {
            labelModifiedDelay.Text = GetTrackBarValueSafe(trackBarModifierDelay).ToString();
            await SaveSettingsAsync();
        }

        private bool TryGetGameWindowHandle()
        {
            try
            {
                string processName = MidiKeyMap.CurrentApplicationName;
                Logging.DebugLog($"Looking for process: {processName}");

                var process = Process.GetProcessesByName(processName).FirstOrDefault();
                if (process == null)
                {
                    Common.ShowErrorMessage($"Game process '{processName}' not found. Make sure the application is running.");
                    return false;
                }

                gameWindowHandle = process.MainWindowHandle;
                Logging.DebugLog($"Found {processName} window handle: {gameWindowHandle}");
                return true;
            }
            catch (Exception ex)
            {
                Common.ShowErrorMessage($"Error finding game window: {ex.Message}");
                return false;
            }
        }

        private void buttonPlaySong_Click(object sender, EventArgs e)
        {
            if (!TryGetGameWindowHandle())
                return;

            StartPlayback();
        }

        private void buttonStopSong_Click(object sender, EventArgs e)
        {
            StopPlayback();
        }

        private void OnGlobalKeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.Key == Keys.F5 && !isPlaying)
            {
                if (!TryGetGameWindowHandle())
                    return;

                StartPlayback();
            }

            if (e.Key == Keys.F6 && isPlaying)
            {
                StopPlayback();
            }
        }

        private void StartPlayback()
        {
            try
            {
                if (listBoxMIDIFiles.SelectedItem == null)
                {
                    Common.ShowErrorMessage("Please select a MIDI file first.");
                    return;
                }

                string selectedMidiFile = Path.Combine(Settings.settings.LastMidiFolder, listBoxMIDIFiles.SelectedItem.ToString());
                Logging.DebugLog($"Playing Song: {selectedMidiFile}");

                cancellationTokenSource?.Dispose();
                cancellationTokenSource = new CancellationTokenSource();

                isPlaying = true;

                _ = PlayMidiAsync(selectedMidiFile, cancellationTokenSource.Token)
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            Logging.DebugLog($"Playback error: {t.Exception?.InnerException?.Message}");
                        }

                        this.BeginInvoke(new Action(() =>
                        {
                            isPlaying = false;
                            Logging.DebugLog("Playback completed or was stopped.");
                        }));
                    });
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error starting playback: {ex.Message}");
                isPlaying = false;
            }
        }


        private void StopPlayback()
        {
            try
            {
                Logging.DebugLog("Stopping song...");
                if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
                {
                    cancellationTokenSource.Cancel();
                    Logging.DebugLog("Cancellation requested");
                }

                ReleaseModifiers();
                isPlaying = false;
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error stopping playback: {ex.Message}");
            }
        }

        private void ReleaseModifiers()
        {
            SendKey(VirtualKeyCode.LCONTROL, false);
            SendKey(VirtualKeyCode.LSHIFT, false);
        }

        private void SendKey(VirtualKeyCode key, bool keyDown)
        {
            Win32Api.SetForegroundWindow(gameWindowHandle);

            var inputs = new Win32Api.INPUT[]
            {
                new Win32Api.INPUT
                {
                    type = Win32Api.INPUT_KEYBOARD,
                    u = new Win32Api.InputUnion
                    {
                        ki = new Win32Api.KEYBDINPUT
                        {
                            wVk = (ushort)key,
                            dwFlags = keyDown ? 0 : Win32Api.KEYEVENTF_KEYUP
                        }
                    }
                }
            };

            Win32Api.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Win32Api.INPUT)));
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
            int num = (absoluteTime - lastTime) * (tempo / ticksPerQuarterNote) / 1000;
            num += this.GetTrackBarValueSafe(this.trackBarTempo) * -5;
            return (num < 0) ? 0 : num;
        }

        private int GetTrackBarValueSafe(TrackBar trackBar)
        {
            if (trackBar.InvokeRequired)
            {
                return (int)trackBar.Invoke(new Func<int>(() => trackBar.Value));
            }
            else
            {
                return trackBar.Value;
            }
        }

        private Dictionary<VirtualKeyCode, bool> _activeModifiers = new Dictionary<VirtualKeyCode, bool>();

        private bool HandleModifiers(List<VirtualKeyCode> keys)
        {
            bool modifiersChanged = false;

            if (keys == null || keys.Count <= 1)
            {
                foreach (var modifier in _activeModifiers.Keys.ToList())
                {
                    if (_activeModifiers[modifier])
                    {
                        SendKey(modifier, false);
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
                    SendKey(modifier, false);
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
                    SendKey(modifier, true);
                    _activeModifiers[modifier] = true;
                    modifiersChanged = true;
                }
            }

            return modifiersChanged;
        }

        private async Task PlayMidiAsync(string midiFilePath, CancellationToken token)
        {
            if (string.IsNullOrEmpty(midiFilePath))
            {
                Common.ShowErrorMessage("Please load a MIDI file first.");
                return;
            }

            try
            {
                MidiFile midiFile = new MidiFile(midiFilePath, false);
                int ticksPerQuarterNote = midiFile.DeltaTicksPerQuarterNote;
                int tempo = GetInitialTempo(midiFile);

                var allEvents = CollectMidiEvents(midiFile);
                Logging.DebugLog($"Total MIDI events to play: {allEvents.Count}");

                bool playOnce = true;

                while ((checkBoxRepeatSong.Checked || playOnce) && !token.IsCancellationRequested)
                {
                    int lastTime = 0;
                    var stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();

                    foreach (var (midiEvent, absoluteTime) in allEvents)
                    {
                        if (token.IsCancellationRequested)
                        {
                            Logging.DebugLog("Playback cancelled");
                            return;
                        }

                        if (midiEvent is NoteOnEvent noteOn && noteOn.Velocity > 0 &&
                            MidiKeyMap.MidiToKey.TryGetValue(noteOn.NoteNumber, out var keys))
                        {
                            int delay = CalculateDelay(absoluteTime, lastTime, tempo, ticksPerQuarterNote);

                            await Task.Delay(Math.Max(1, delay), token);

                            lastTime = absoluteTime;

                            _ = visualBoard.HighlightKey(noteOn.NoteNumber, Math.Max(100, delay));

                            bool modifiersChanged = HandleModifiers(keys);

                            if (modifiersChanged)
                            {
                                int modifierDelay = GetTrackBarValueSafe(trackBarModifierDelay);
                                if (modifierDelay > 0)
                                {
                                    Logging.DebugLog($"Modifier change detected - delaying {modifierDelay}ms");
                                    await Task.Delay(modifierDelay, token);
                                }
                            }

                            if (keys.Count > 0)
                            {
                                var actualKey = keys.Last();
                                SendKey(actualKey, true);
                                SendKey(actualKey, false);
                            }

                            Logging.DebugLog($"Key Down: {noteOn.NoteName} {noteOn.NoteNumber} ({string.Join(", ", keys)})");
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

                HandleModifiers(new List<VirtualKeyCode>());
                Logging.DebugLog("Song Ended");
            }
            catch (OperationCanceledException)
            {
                Logging.DebugLog("Playback was cancelled");
                HandleModifiers(new List<VirtualKeyCode>());
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error during MIDI playback: {ex.Message}");
                Common.ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        private void buttonUseMidiDevice_Click(object sender, EventArgs e)
        {
            try
            {
                if (isMidiDeviceActive)
                {
                    DisconnectMidiDevice();
                    buttonUseMidiDevice.Text = "Use MIDI Device";
                    return;
                }

                if (comboBoxMIDIDevices.SelectedIndex < 0 || comboBoxMIDIDevices.SelectedIndex >= MidiIn.NumberOfDevices)
                {
                    Common.ShowErrorMessage("Please select a valid MIDI device.");
                    return;
                }

                if (midiIn != null)
                {
                    DisconnectMidiDevice();
                }

                midiIn = new MidiIn(comboBoxMIDIDevices.SelectedIndex);
                string deviceName = MidiIn.DeviceInfo(comboBoxMIDIDevices.SelectedIndex).ProductName;

                midiIn.MessageReceived += OnMidiMessageReceived;
                midiIn.ErrorReceived += OnMidiErrorReceived;

                midiIn.Start();
                isMidiDeviceActive = true;

                buttonUseMidiDevice.Text = "Disconnect MIDI";
                comboBoxMIDIDevices.Enabled = false;

                Logging.DebugLog($"Connected to MIDI Device: {deviceName}");
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"ERROR connecting to MIDI device: {ex.Message}");
                Common.ShowErrorMessage($"Failed to connect to MIDI device: {ex.Message}");

                DisconnectMidiDevice();
            }
        }

        private void DisconnectMidiDevice()
        {
            if (midiIn != null)
            {
                try
                {
                    midiIn.Stop();
                    midiIn.MessageReceived -= OnMidiMessageReceived;
                    midiIn.ErrorReceived -= OnMidiErrorReceived;
                    midiIn.Dispose();
                    midiIn = null;

                    Logging.DebugLog("MIDI device disconnected");
                }
                catch (Exception ex)
                {
                    Logging.DebugLog($"Error disconnecting MIDI device: {ex.Message}");
                }
                finally
                {
                    isMidiDeviceActive = false;
                    comboBoxMIDIDevices.Enabled = true;
                    buttonUseMidiDevice.Text = "Use MIDI Device";
                }
            }
        }

        private void OnMidiErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            Logging.DebugLog($"MIDI Error: {e.RawMessage}");
        }

        private async void OnMidiMessageReceived(object sender, MidiInMessageEventArgs e)
        {
            try
            {
                if (e.MidiEvent is NoteEvent noteEvent)
                {
                    int noteNumber = noteEvent.NoteNumber;
                    bool isNoteOn = e.MidiEvent.CommandCode == MidiCommandCode.NoteOn && noteEvent.Velocity > 0;
                    bool isNoteOff = e.MidiEvent.CommandCode == MidiCommandCode.NoteOff ||
                                     (e.MidiEvent.CommandCode == MidiCommandCode.NoteOn && noteEvent.Velocity == 0);

                    if (isNoteOff)
                    {
                        visualBoard.UnhighlightKey(noteNumber);
                        return;
                    }

                    if (isNoteOn)
                    {
                        if (gameWindowHandle == IntPtr.Zero)
                        {
                            if (!TryGetGameWindowHandleQuiet())
                            {
                                _ = visualBoard.HighlightKey(noteNumber, 200);
                                return;
                            }
                        }

                        if (MidiKeyMap.MidiToKey.TryGetValue(noteNumber, out var keys) && keys.Count > 0)
                        {
                            _ = visualBoard.HighlightKey(noteNumber, 200);

                            bool modifiersChanged = HandleModifiers(keys);
                            if (modifiersChanged)
                            {
                                int modifierDelay = GetTrackBarValueSafe(trackBarModifierDelay);
                                if (modifierDelay > 0)
                                {
                                    await Task.Delay(modifierDelay);
                                }
                            }

                            var actualKey = keys.Last();
                            SendKey(actualKey, true);
                            SendKey(actualKey, false);

                            Logging.DebugLog($"MIDI Key: {noteEvent.NoteName} ({noteNumber}) -> Keys: {string.Join(", ", keys)}");
                        }
                        else
                        {
                            _ = visualBoard.HighlightKey(noteNumber, 100);
                            Logging.DebugLog($"MIDI Key not mapped: {noteEvent.NoteName} ({noteNumber})");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error processing MIDI message: {ex.Message}");
            }
        }

        private bool TryGetGameWindowHandleQuiet()
        {
            try
            {
                string processName = MidiKeyMap.CurrentApplicationName;

                if (string.IsNullOrEmpty(processName))
                {
                    return false;
                }

                var process = Process.GetProcessesByName(processName).FirstOrDefault();
                if (process == null)
                {
                    return false;
                }

                gameWindowHandle = process.MainWindowHandle;
                return gameWindowHandle != IntPtr.Zero;
            }
            catch
            {
                return false;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DisconnectMidiDevice();
            StopPlayback();

            if (_globalKeyboardHook != null)
            {
                _globalKeyboardHook.UnhookKeyboard();
            }

            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;

            base.OnFormClosing(e);
        }

        private void buttonEditKeyMaps_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedKeyMap = string.Empty;

                // If a keymap is already selected in the list, use that one
                if (listBoxMIDIKeyMaps.SelectedItem != null)
                {
                    selectedKeyMap = listBoxMIDIKeyMaps.SelectedItem.ToString();
                }

                // Create and show the editor form
                using (var editorForm = string.IsNullOrEmpty(selectedKeyMap)
                    ? new FormMidiKeyMapEditor()
                    : new FormMidiKeyMapEditor(selectedKeyMap))
                {
                    this.TopMost = false;
                    checkBoxAlwaysOnTop.Checked = false;

                    editorForm.ShowDialog();

                    // Reload the keymap list after editing
                    _ = LoadMidiKeyMapsAsync();
                }
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error opening KeyMap Editor: {ex.Message}");
                Common.ShowErrorMessage($"Error opening KeyMap Editor: {ex.Message}");
            }
        }

        private void panelTop_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
