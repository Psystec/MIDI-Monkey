using WindowsInput.Native;
using Newtonsoft.Json;
using System.ComponentModel;

namespace MIDI_Monkey
{
    public partial class FormMidiKeyMapEditor : Form
    {
        private Dictionary<int, List<VirtualKeyCode>> currentMappings = new Dictionary<int, List<VirtualKeyCode>>();
        private string applicationName = string.Empty;
        private string currentFileName = string.Empty;
        private bool isNewFile = true;
        private bool hasUnsavedChanges = false;

        private readonly BindingList<MidiKeyMapEntry> mappingEntries = new BindingList<MidiKeyMapEntry>();

        public FormMidiKeyMapEditor()
        {
            InitializeComponent();
            SetupFormElements();
        }

        public FormMidiKeyMapEditor(string keyMapName) : this()
        {
            isNewFile = false;
            currentFileName = keyMapName;
            LoadKeyMap(keyMapName);
        }

        private void SetupFormElements()
        {
            dataGridViewKeyMappings.AutoGenerateColumns = false;

            var midiNoteColumn = new DataGridViewTextBoxColumn
            {
                Name = "MidiNote",
                HeaderText = "MIDI Note",
                DataPropertyName = "MidiNote",
                ReadOnly = true,
                Width = 80
            };

            var noteNameColumn = new DataGridViewTextBoxColumn
            {
                Name = "NoteName",
                HeaderText = "Note Name",
                DataPropertyName = "NoteName",
                ReadOnly = true,
                Width = 100
            };

            var modifierColumn = new DataGridViewComboBoxColumn
            {
                Name = "Modifier",
                HeaderText = "Modifier",
                DataPropertyName = "Modifier",
                Width = 120
            };
            modifierColumn.Items.AddRange(new object[] {
                "None", "LSHIFT", "LCONTROL", "LMENU", "RSHIFT", "RCONTROL", "RMENU"
            });

            var keyColumn = new DataGridViewComboBoxColumn
            {
                Name = "Key",
                HeaderText = "Key",
                DataPropertyName = "Key",
                Width = 120
            };

            var keyValues = Enum.GetValues(typeof(VirtualKeyCode))
                .Cast<VirtualKeyCode>()
                .Where(k => k != VirtualKeyCode.LSHIFT && k != VirtualKeyCode.LCONTROL &&
                            k != VirtualKeyCode.LMENU && k != VirtualKeyCode.RSHIFT &&
                            k != VirtualKeyCode.RCONTROL && k != VirtualKeyCode.RMENU)
                .Select(k => k.ToString())
                .ToArray();

            keyColumn.Items.AddRange(keyValues);

            dataGridViewKeyMappings.Columns.AddRange(new DataGridViewColumn[] {
                midiNoteColumn, noteNameColumn, modifierColumn, keyColumn
            });

            dataGridViewKeyMappings.DataSource = mappingEntries;

            dataGridViewKeyMappings.CellValueChanged += DataGridViewKeyMappings_CellValueChanged;
            dataGridViewKeyMappings.CurrentCellDirtyStateChanged += DataGridViewKeyMappings_CurrentCellDirtyStateChanged;

            InitializeEmptyMappings();
        }

        private void DataGridViewKeyMappings_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewKeyMappings.IsCurrentCellDirty)
            {
                dataGridViewKeyMappings.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DataGridViewKeyMappings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                hasUnsavedChanges = true;

                int midiNote = (int)dataGridViewKeyMappings.Rows[e.RowIndex].Cells["MidiNote"].Value;
                string modifier = dataGridViewKeyMappings.Rows[e.RowIndex].Cells["Modifier"].Value?.ToString() ?? "None";
                string key = dataGridViewKeyMappings.Rows[e.RowIndex].Cells["Key"].Value?.ToString() ?? "";

                currentMappings[midiNote] = new List<VirtualKeyCode>();

                if (modifier != "None" && Enum.TryParse(modifier, out VirtualKeyCode modifierKey))
                {
                    currentMappings[midiNote].Add(modifierKey);
                }

                if (!string.IsNullOrEmpty(key) && Enum.TryParse(key, out VirtualKeyCode virtualKey))
                {
                    currentMappings[midiNote].Add(virtualKey);
                }
            }
        }

        private void InitializeEmptyMappings()
        {
            mappingEntries.Clear();

            for (int i = 0; i < 128; i++)
            {
                string noteName = GetNoteName(i);
                mappingEntries.Add(new MidiKeyMapEntry
                {
                    MidiNote = i,
                    NoteName = noteName,
                    Modifier = "None",
                    Key = ""
                });

                currentMappings[i] = new List<VirtualKeyCode>();
            }
        }

        private string GetNoteName(int midiNote)
        {
            string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
            int octave = (midiNote / 12) - 1;
            int noteIndex = midiNote % 12;

            return $"{noteNames[noteIndex]}{octave}";
        }

        private async void LoadKeyMap(string keyMapName)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                string midiKeyMapsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MIDIKeyMaps");
                string filePath = Path.Combine(midiKeyMapsDirectory, $"{keyMapName}.json");

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"MidiKeyMap file not found: {filePath}");
                }

                string fileContent = await File.ReadAllTextAsync(filePath);
                var contentLines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(line => !line.TrimStart().StartsWith("//") && !line.TrimStart().StartsWith("#"))
                    .ToArray();

                string jsonContent = string.Join(Environment.NewLine, contentLines);

                var configObject = JsonConvert.DeserializeObject<MidiKeyMapConfig>(jsonContent);

                if (configObject == null || configObject.KeyMappings == null)
                {
                    throw new Exception("Failed to parse MidiKeyMap JSON or invalid format");
                }

                applicationName = configObject.ApplicationName ?? string.Empty;
                textBoxApplication.Text = applicationName;

                InitializeEmptyMappings();

                foreach (var entry in configObject.KeyMappings)
                {
                    if (int.TryParse(entry.Key, out int midiKey) && midiKey >= 0 && midiKey < 128)
                    {
                        var keyCodes = new List<VirtualKeyCode>();
                        string modifier = "None";
                        string mainKey = "";

                        foreach (var keyName in entry.Value)
                        {
                            if (Enum.TryParse(keyName, out VirtualKeyCode keyCode))
                            {
                                keyCodes.Add(keyCode);

                                if (keyCode == VirtualKeyCode.LSHIFT || keyCode == VirtualKeyCode.LCONTROL ||
                                    keyCode == VirtualKeyCode.LMENU || keyCode == VirtualKeyCode.RSHIFT ||
                                    keyCode == VirtualKeyCode.RCONTROL || keyCode == VirtualKeyCode.RMENU)
                                {
                                    modifier = keyCode.ToString();
                                }
                                else
                                {
                                    mainKey = keyCode.ToString();
                                }
                            }
                        }

                        var gridEntry = mappingEntries.FirstOrDefault(e => e.MidiNote == midiKey);
                        if (gridEntry != null)
                        {
                            gridEntry.Modifier = modifier;
                            gridEntry.Key = mainKey;
                        }

                        currentMappings[midiKey] = keyCodes;
                    }
                }

                Text = $"MIDI KeyMap Editor - {keyMapName}";
                hasUnsavedChanges = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading MidiKeyMap: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void SaveKeyMap()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                applicationName = textBoxApplication.Text.Trim();

                if (string.IsNullOrWhiteSpace(applicationName))
                {
                    MessageBox.Show("Please enter an application name.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxApplication.Focus();
                    return;
                }

                var config = new MidiKeyMapConfig
                {
                    ApplicationName = applicationName,
                    KeyMappings = new Dictionary<string, List<string>>()
                };

                foreach (var mapping in currentMappings)
                {
                    if (mapping.Value.Count > 0)
                    {
                        config.KeyMappings[mapping.Key.ToString()] = mapping.Value.Select(k => k.ToString()).ToList();
                    }
                    else
                    {
                        config.KeyMappings[mapping.Key.ToString()] = new List<string>();
                    }
                }

                if (isNewFile || string.IsNullOrWhiteSpace(currentFileName))
                {
                    using (SaveFileDialog saveDialog = new SaveFileDialog())
                    {
                        string midiKeyMapsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MIDIKeyMaps");

                        if (!Directory.Exists(midiKeyMapsDirectory))
                        {
                            Directory.CreateDirectory(midiKeyMapsDirectory);
                        }

                        saveDialog.Filter = "JSON files (*.json)|*.json";
                        saveDialog.InitialDirectory = midiKeyMapsDirectory;
                        saveDialog.FileName = applicationName;
                        saveDialog.DefaultExt = "json";
                        saveDialog.AddExtension = true;

                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            currentFileName = Path.GetFileNameWithoutExtension(saveDialog.FileName);
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                var header = "# MIDI Monkey MidiKeyMap Config.\n" +
                             "# To create your MIDI Key Mapping, you need to assign each MIDI key to a corresponding keyboard key.\n" +
                             "# Each mapping can include a modifier key (such as Shift or Control) followed by the alphanumeric keyboard key.\n" +
                             "# If you do not want to use a modifier key, simply omit it and use only the alphanumeric key.\n" +
                             "# The configuration follows this structure:\n" +
                             "#   1. MIDI Key: The MIDI note you want to map (0 to 127)\n" +
                             "#   2. Modifier Key (Optional): The modifier key, if any (e.g., LSHIFT, LCONTROL)\n" +
                             "#   3. Alphanumeric Keyboard Key: The key on the keyboard (e.g., VK_Q, VK_W)\n" +
                             "#   4. ApplicationName: The process name (without .exe) that this mapping is for\n\n";

                var contents = JsonConvert.SerializeObject(config, Formatting.Indented);
                string midiKeyMapsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MIDIKeyMaps");
                string outputPath = Path.Combine(midiKeyMapsDir, $"{currentFileName}.json");

                await File.WriteAllTextAsync(outputPath, header + contents);

                Text = $"MIDI KeyMap Editor - {currentFileName}";
                isNewFile = false;
                hasUnsavedChanges = false;

                MessageBox.Show($"KeyMap successfully saved as '{currentFileName}.json'", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving MidiKeyMap: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveKeyMap();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            if (hasUnsavedChanges)
            {
                DialogResult result = MessageBox.Show(
                    "You have unsaved changes. Would you like to save them before creating a new keymap?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    SaveKeyMap();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            isNewFile = true;
            currentFileName = string.Empty;
            applicationName = string.Empty;
            textBoxApplication.Text = string.Empty;
            InitializeEmptyMappings();
            Text = "MIDI KeyMap Editor - New KeyMap";
            hasUnsavedChanges = false;
        }

        private async void buttonLoad_Click(object sender, EventArgs e)
        {
            if (hasUnsavedChanges)
            {
                DialogResult result = MessageBox.Show(
                    "You have unsaved changes. Would you like to save them before loading another keymap?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    SaveKeyMap();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            try
            {
                var keyMaps = await MidiKeyMap.GetMidiKeyMapsAsync();

                if (keyMaps.Count == 0)
                {
                    MessageBox.Show("No KeyMap files found. Create a new one instead.", "No KeyMaps", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var form = new Form())
                {
                    form.Text = "Select KeyMap";
                    form.Size = new Size(400, 300);
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.FormBorderStyle = FormBorderStyle.FixedDialog;
                    form.MaximizeBox = false;
                    form.MinimizeBox = false;

                    var listBox = new ListBox
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(10),
                        Font = new Font("Segoe UI", 10F)
                    };

                    foreach (var keyMap in keyMaps)
                    {
                        listBox.Items.Add(keyMap);
                    }

                    if (listBox.Items.Count > 0)
                    {
                        listBox.SelectedIndex = 0;
                    }

                    var panelButtons = new Panel
                    {
                        Dock = DockStyle.Bottom,
                        Height = 50
                    };

                    var buttonOk = new Button
                    {
                        Text = "OK",
                        DialogResult = DialogResult.OK,
                        Size = new Size(80, 30),
                        Location = new Point(form.Width - 180, 10)
                    };

                    var buttonCancel = new Button
                    {
                        Text = "Cancel",
                        DialogResult = DialogResult.Cancel,
                        Size = new Size(80, 30),
                        Location = new Point(form.Width - 90, 10)
                    };

                    panelButtons.Controls.Add(buttonOk);
                    panelButtons.Controls.Add(buttonCancel);

                    form.Controls.Add(listBox);
                    form.Controls.Add(panelButtons);

                    form.AcceptButton = buttonOk;
                    form.CancelButton = buttonCancel;

                    if (form.ShowDialog() == DialogResult.OK && listBox.SelectedItem != null)
                    {
                        string selectedKeyMap = listBox.SelectedItem.ToString();
                        currentFileName = selectedKeyMap;
                        isNewFile = false;
                        LoadKeyMap(selectedKeyMap);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading keymaps: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxApplication_TextChanged(object sender, EventArgs e)
        {
            hasUnsavedChanges = true;
        }

        private void FormMidiKeyMapEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hasUnsavedChanges)
            {
                DialogResult result = MessageBox.Show(
                    "You have unsaved changes. Would you like to save before closing?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    SaveKeyMap();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private async void buttonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (isNewFile || string.IsNullOrEmpty(currentFileName))
                {
                    MessageBox.Show("Please save the keymap first before refreshing.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                await MidiKeyMap.LoadFromJsonAsync(currentFileName);
                MessageBox.Show("KeyMap has been refreshed in the application.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing keymap: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            dataGridViewKeyMappings = new DataGridView();
            panelButtons = new Panel();
            buttonRefresh = new Button();
            buttonLoad = new Button();
            buttonNew = new Button();
            buttonSave = new Button();
            panelTop = new Panel();
            label1 = new Label();
            textBoxApplication = new TextBox();
            ((ISupportInitialize)dataGridViewKeyMappings).BeginInit();
            panelButtons.SuspendLayout();
            panelTop.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewKeyMappings
            // 
            dataGridViewKeyMappings.AllowUserToAddRows = false;
            dataGridViewKeyMappings.AllowUserToDeleteRows = false;
            dataGridViewKeyMappings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewKeyMappings.BackgroundColor = SystemColors.Control;
            dataGridViewKeyMappings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewKeyMappings.Location = new Point(12, 51);
            dataGridViewKeyMappings.Name = "dataGridViewKeyMappings";
            dataGridViewKeyMappings.RowHeadersWidth = 40;
            dataGridViewKeyMappings.Size = new Size(560, 449);
            dataGridViewKeyMappings.TabIndex = 1;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(buttonRefresh);
            panelButtons.Controls.Add(buttonLoad);
            panelButtons.Controls.Add(buttonNew);
            panelButtons.Controls.Add(buttonSave);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(0, 512);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(584, 50);
            panelButtons.TabIndex = 2;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonRefresh.Location = new Point(283, 11);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(85, 28);
            buttonRefresh.TabIndex = 3;
            buttonRefresh.Text = "Refresh";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonLoad.Location = new Point(386, 11);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(85, 28);
            buttonLoad.TabIndex = 2;
            buttonLoad.Text = "Load";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += buttonLoad_Click;
            // 
            // buttonNew
            // 
            buttonNew.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonNew.Location = new Point(12, 11);
            buttonNew.Name = "buttonNew";
            buttonNew.Size = new Size(85, 28);
            buttonNew.TabIndex = 0;
            buttonNew.Text = "New";
            buttonNew.UseVisualStyleBackColor = true;
            buttonNew.Click += buttonNew_Click;
            // 
            // buttonSave
            // 
            buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonSave.Location = new Point(486, 11);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(85, 28);
            buttonSave.TabIndex = 1;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // panelTop
            // 
            panelTop.Controls.Add(label1);
            panelTop.Controls.Add(textBoxApplication);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(584, 45);
            panelTop.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(171, 15);
            label1.TabIndex = 0;
            label1.Text = "Application Name (Process ID):";
            // 
            // textBoxApplication
            // 
            textBoxApplication.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxApplication.Location = new Point(194, 12);
            textBoxApplication.Name = "textBoxApplication";
            textBoxApplication.Size = new Size(377, 23);
            textBoxApplication.TabIndex = 0;
            textBoxApplication.TextChanged += textBoxApplication_TextChanged;
            // 
            // FormMidiKeyMapEditor
            // 
            ClientSize = new Size(584, 562);
            Controls.Add(panelTop);
            Controls.Add(panelButtons);
            Controls.Add(dataGridViewKeyMappings);
            MinimumSize = new Size(600, 600);
            Name = "FormMidiKeyMapEditor";
            StartPosition = FormStartPosition.CenterParent;
            Text = "MIDI KeyMap Editor - New KeyMap";
            FormClosing += FormMidiKeyMapEditor_FormClosing;
            Load += FormMidiKeyMapEditor_Load;
            ((ISupportInitialize)dataGridViewKeyMappings).EndInit();
            panelButtons.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dataGridViewKeyMappings;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxApplication;
        private System.Windows.Forms.Button buttonRefresh;

        private void FormMidiKeyMapEditor_Load(object sender, EventArgs e)
        {

        }
    }

    public class MidiKeyMapEntry
    {
        public int MidiNote { get; set; }
        public string NoteName { get; set; }
        public string Modifier { get; set; }
        public string Key { get; set; }
    }
}
