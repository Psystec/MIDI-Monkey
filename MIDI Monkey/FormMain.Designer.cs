using System.Drawing;
using System.Windows.Forms;

namespace MIDI_Monkey
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            panelTop = new Panel();
            pictureBoxSignalApp = new PictureBox();
            pictureBoxDiscord = new PictureBox();
            pictureBoxLogo = new PictureBox();
            buttonClose = new Button();
            labelAppNameLabel = new Label();
            groupBoxMIDIkeyMapProfiles = new GroupBox();
            listBoxMIDIKeyMaps = new ListBox();
            groupBoxMIDIControls = new GroupBox();
            comboBoxMIDIDevices = new ComboBox();
            labelMIDIDeviceLabel = new Label();
            buttonUseMidiDevice = new Button();
            buttonStopSong = new Button();
            buttonPlaySong = new Button();
            buttonSelectMIDIFolder = new Button();
            groupBoxSelectMIDIFolder = new GroupBox();
            listBoxMIDIFiles = new ListBox();
            splitContainerMain = new SplitContainer();
            splitContainerProfilesAndFiles = new SplitContainer();
            buttonEditKeyMaps = new Button();
            groupBoxDebugLog = new GroupBox();
            richTextBoxLog = new RichTextBox();
            groupBoxSettings = new GroupBox();
            labelModifiedDelay = new Label();
            labelModifierDelayLabel = new Label();
            trackBarModifierDelay = new TrackBar();
            labelTempo = new Label();
            trackBarTempo = new TrackBar();
            labelTempoLabel = new Label();
            checkBoxAlwaysOnTop = new CheckBox();
            checkBoxRepeatSong = new CheckBox();
            pictureBoxPayPal = new PictureBox();
            labelPayPalLabel = new Label();
            panelResizeHandle = new Panel();
            groupBoxKeyVisualizer = new GroupBox();
            panelMidiNoteVisualizer = new Panel();
            groupBoxMIDIChannels = new GroupBox();
            checkBoxMIDIChannel16 = new CheckBox();
            checkBoxMIDIChannel08 = new CheckBox();
            checkBoxMIDIChannel15 = new CheckBox();
            checkBoxMIDIChannel07 = new CheckBox();
            checkBoxMIDIChannel14 = new CheckBox();
            checkBoxMIDIChannel06 = new CheckBox();
            checkBoxMIDIChannel13 = new CheckBox();
            checkBoxMIDIChannel05 = new CheckBox();
            checkBoxMIDIChannel12 = new CheckBox();
            checkBoxMIDIChannel04 = new CheckBox();
            checkBoxMIDIChannel11 = new CheckBox();
            checkBoxMIDIChannel03 = new CheckBox();
            checkBoxMIDIChannel10 = new CheckBox();
            checkBoxMIDIChannel02 = new CheckBox();
            checkBoxMIDIChannel09 = new CheckBox();
            checkBoxMIDIChannel01 = new CheckBox();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSignalApp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDiscord).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            groupBoxMIDIkeyMapProfiles.SuspendLayout();
            groupBoxMIDIControls.SuspendLayout();
            groupBoxSelectMIDIFolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerProfilesAndFiles).BeginInit();
            splitContainerProfilesAndFiles.Panel1.SuspendLayout();
            splitContainerProfilesAndFiles.Panel2.SuspendLayout();
            splitContainerProfilesAndFiles.SuspendLayout();
            groupBoxDebugLog.SuspendLayout();
            groupBoxSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarModifierDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarTempo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPayPal).BeginInit();
            groupBoxKeyVisualizer.SuspendLayout();
            groupBoxMIDIChannels.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(50, 50, 50);
            panelTop.Controls.Add(pictureBoxSignalApp);
            panelTop.Controls.Add(pictureBoxDiscord);
            panelTop.Controls.Add(pictureBoxLogo);
            panelTop.Controls.Add(buttonClose);
            panelTop.Controls.Add(labelAppNameLabel);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(849, 35);
            panelTop.TabIndex = 0;
            panelTop.Paint += panelTop_Paint;
            // 
            // pictureBoxSignalApp
            // 
            pictureBoxSignalApp.Image = (Image)resources.GetObject("pictureBoxSignalApp.Image");
            pictureBoxSignalApp.Location = new Point(750, 3);
            pictureBoxSignalApp.Name = "pictureBoxSignalApp";
            pictureBoxSignalApp.Size = new Size(30, 29);
            pictureBoxSignalApp.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxSignalApp.TabIndex = 9;
            pictureBoxSignalApp.TabStop = false;
            pictureBoxSignalApp.MouseClick += pictureBoxSignalApp_Click;
            // 
            // pictureBoxDiscord
            // 
            pictureBoxDiscord.Image = (Image)resources.GetObject("pictureBoxDiscord.Image");
            pictureBoxDiscord.Location = new Point(786, 3);
            pictureBoxDiscord.Name = "pictureBoxDiscord";
            pictureBoxDiscord.Size = new Size(28, 29);
            pictureBoxDiscord.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxDiscord.TabIndex = 8;
            pictureBoxDiscord.TabStop = false;
            pictureBoxDiscord.MouseClick += pictureBoxDiscord_MouseClick;
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.BackgroundImage = (Image)resources.GetObject("pictureBoxLogo.BackgroundImage");
            pictureBoxLogo.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBoxLogo.Location = new Point(12, 0);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(35, 35);
            pictureBoxLogo.TabIndex = 0;
            pictureBoxLogo.TabStop = false;
            // 
            // buttonClose
            // 
            buttonClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonClose.BackgroundImage = (Image)resources.GetObject("buttonClose.BackgroundImage");
            buttonClose.BackgroundImageLayout = ImageLayout.Zoom;
            buttonClose.FlatAppearance.BorderSize = 0;
            buttonClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(255, 128, 128);
            buttonClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 192, 192);
            buttonClose.FlatStyle = FlatStyle.Flat;
            buttonClose.Location = new Point(820, 7);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(20, 20);
            buttonClose.TabIndex = 1;
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.MouseClick += buttonClose_MouseClick;
            // 
            // labelAppNameLabel
            // 
            labelAppNameLabel.AutoSize = true;
            labelAppNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelAppNameLabel.Location = new Point(53, 12);
            labelAppNameLabel.Name = "labelAppNameLabel";
            labelAppNameLabel.Size = new Size(83, 15);
            labelAppNameLabel.TabIndex = 0;
            labelAppNameLabel.Text = "MIDI Monkey";
            // 
            // groupBoxMIDIkeyMapProfiles
            // 
            groupBoxMIDIkeyMapProfiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxMIDIkeyMapProfiles.Controls.Add(listBoxMIDIKeyMaps);
            groupBoxMIDIkeyMapProfiles.FlatStyle = FlatStyle.Flat;
            groupBoxMIDIkeyMapProfiles.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxMIDIkeyMapProfiles.ForeColor = Color.LightGray;
            groupBoxMIDIkeyMapProfiles.Location = new Point(3, 3);
            groupBoxMIDIkeyMapProfiles.Name = "groupBoxMIDIkeyMapProfiles";
            groupBoxMIDIkeyMapProfiles.Size = new Size(156, 311);
            groupBoxMIDIkeyMapProfiles.TabIndex = 1;
            groupBoxMIDIkeyMapProfiles.TabStop = false;
            groupBoxMIDIkeyMapProfiles.Text = "MIDI KeyMap Profiles";
            // 
            // listBoxMIDIKeyMaps
            // 
            listBoxMIDIKeyMaps.BackColor = Color.FromArgb(64, 64, 64);
            listBoxMIDIKeyMaps.Dock = DockStyle.Fill;
            listBoxMIDIKeyMaps.ForeColor = Color.FromArgb(224, 224, 224);
            listBoxMIDIKeyMaps.FormattingEnabled = true;
            listBoxMIDIKeyMaps.IntegralHeight = false;
            listBoxMIDIKeyMaps.Location = new Point(3, 19);
            listBoxMIDIKeyMaps.Name = "listBoxMIDIKeyMaps";
            listBoxMIDIKeyMaps.Size = new Size(150, 289);
            listBoxMIDIKeyMaps.TabIndex = 6;
            listBoxMIDIKeyMaps.SelectedIndexChanged += listBoxMIDIKeyMaps_SelectedIndexChanged;
            // 
            // groupBoxMIDIControls
            // 
            groupBoxMIDIControls.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxMIDIControls.Controls.Add(comboBoxMIDIDevices);
            groupBoxMIDIControls.Controls.Add(labelMIDIDeviceLabel);
            groupBoxMIDIControls.Controls.Add(buttonUseMidiDevice);
            groupBoxMIDIControls.Controls.Add(buttonStopSong);
            groupBoxMIDIControls.Controls.Add(buttonPlaySong);
            groupBoxMIDIControls.Controls.Add(buttonSelectMIDIFolder);
            groupBoxMIDIControls.FlatStyle = FlatStyle.Flat;
            groupBoxMIDIControls.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxMIDIControls.ForeColor = Color.LightGray;
            groupBoxMIDIControls.Location = new Point(3, 3);
            groupBoxMIDIControls.Name = "groupBoxMIDIControls";
            groupBoxMIDIControls.Size = new Size(399, 105);
            groupBoxMIDIControls.TabIndex = 2;
            groupBoxMIDIControls.TabStop = false;
            groupBoxMIDIControls.Text = "MIDI Controls";
            // 
            // comboBoxMIDIDevices
            // 
            comboBoxMIDIDevices.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboBoxMIDIDevices.BackColor = Color.FromArgb(64, 64, 64);
            comboBoxMIDIDevices.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxMIDIDevices.FlatStyle = FlatStyle.Flat;
            comboBoxMIDIDevices.ForeColor = Color.LightGray;
            comboBoxMIDIDevices.Location = new Point(93, 63);
            comboBoxMIDIDevices.Name = "comboBoxMIDIDevices";
            comboBoxMIDIDevices.Size = new Size(178, 23);
            comboBoxMIDIDevices.TabIndex = 2;
            comboBoxMIDIDevices.TabStop = false;
            // 
            // labelMIDIDeviceLabel
            // 
            labelMIDIDeviceLabel.AutoSize = true;
            labelMIDIDeviceLabel.Location = new Point(7, 66);
            labelMIDIDeviceLabel.Name = "labelMIDIDeviceLabel";
            labelMIDIDeviceLabel.Size = new Size(80, 15);
            labelMIDIDeviceLabel.TabIndex = 1;
            labelMIDIDeviceLabel.Text = "MIDI Device:";
            // 
            // buttonUseMidiDevice
            // 
            buttonUseMidiDevice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonUseMidiDevice.BackColor = Color.DimGray;
            buttonUseMidiDevice.FlatAppearance.MouseDownBackColor = Color.Gray;
            buttonUseMidiDevice.FlatAppearance.MouseOverBackColor = Color.Silver;
            buttonUseMidiDevice.FlatStyle = FlatStyle.System;
            buttonUseMidiDevice.Location = new Point(276, 62);
            buttonUseMidiDevice.Name = "buttonUseMidiDevice";
            buttonUseMidiDevice.Size = new Size(115, 24);
            buttonUseMidiDevice.TabIndex = 0;
            buttonUseMidiDevice.Text = "Use MIDI Device";
            buttonUseMidiDevice.UseVisualStyleBackColor = false;
            buttonUseMidiDevice.Click += buttonUseMidiDevice_Click;
            // 
            // buttonStopSong
            // 
            buttonStopSong.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonStopSong.BackColor = Color.DimGray;
            buttonStopSong.FlatAppearance.MouseDownBackColor = Color.Gray;
            buttonStopSong.FlatAppearance.MouseOverBackColor = Color.Silver;
            buttonStopSong.FlatStyle = FlatStyle.System;
            buttonStopSong.Location = new Point(288, 22);
            buttonStopSong.Name = "buttonStopSong";
            buttonStopSong.Size = new Size(102, 24);
            buttonStopSong.TabIndex = 0;
            buttonStopSong.Text = "Stop Song (F6)";
            buttonStopSong.UseVisualStyleBackColor = false;
            buttonStopSong.Click += buttonStopSong_Click;
            // 
            // buttonPlaySong
            // 
            buttonPlaySong.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonPlaySong.BackColor = Color.DimGray;
            buttonPlaySong.FlatAppearance.MouseDownBackColor = Color.Gray;
            buttonPlaySong.FlatAppearance.MouseOverBackColor = Color.Silver;
            buttonPlaySong.FlatStyle = FlatStyle.System;
            buttonPlaySong.Location = new Point(184, 22);
            buttonPlaySong.Name = "buttonPlaySong";
            buttonPlaySong.Size = new Size(98, 24);
            buttonPlaySong.TabIndex = 0;
            buttonPlaySong.Text = "Play Song (F5)";
            buttonPlaySong.UseVisualStyleBackColor = false;
            buttonPlaySong.Click += buttonPlaySong_Click;
            // 
            // buttonSelectMIDIFolder
            // 
            buttonSelectMIDIFolder.BackColor = Color.DimGray;
            buttonSelectMIDIFolder.FlatAppearance.MouseDownBackColor = Color.Gray;
            buttonSelectMIDIFolder.FlatAppearance.MouseOverBackColor = Color.Silver;
            buttonSelectMIDIFolder.FlatStyle = FlatStyle.System;
            buttonSelectMIDIFolder.Location = new Point(6, 22);
            buttonSelectMIDIFolder.Name = "buttonSelectMIDIFolder";
            buttonSelectMIDIFolder.Size = new Size(139, 24);
            buttonSelectMIDIFolder.TabIndex = 0;
            buttonSelectMIDIFolder.Text = "Select MIDI Folder";
            buttonSelectMIDIFolder.UseVisualStyleBackColor = false;
            buttonSelectMIDIFolder.Click += buttonSelectMIDIFolder_Click;
            // 
            // groupBoxSelectMIDIFolder
            // 
            groupBoxSelectMIDIFolder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxSelectMIDIFolder.Controls.Add(listBoxMIDIFiles);
            groupBoxSelectMIDIFolder.FlatStyle = FlatStyle.Flat;
            groupBoxSelectMIDIFolder.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxSelectMIDIFolder.ForeColor = Color.LightGray;
            groupBoxSelectMIDIFolder.Location = new Point(3, 3);
            groupBoxSelectMIDIFolder.Name = "groupBoxSelectMIDIFolder";
            groupBoxSelectMIDIFolder.Size = new Size(242, 338);
            groupBoxSelectMIDIFolder.TabIndex = 3;
            groupBoxSelectMIDIFolder.TabStop = false;
            groupBoxSelectMIDIFolder.Text = "MIDI Files";
            // 
            // listBoxMIDIFiles
            // 
            listBoxMIDIFiles.BackColor = Color.FromArgb(64, 64, 64);
            listBoxMIDIFiles.Dock = DockStyle.Fill;
            listBoxMIDIFiles.ForeColor = Color.FromArgb(224, 224, 224);
            listBoxMIDIFiles.FormattingEnabled = true;
            listBoxMIDIFiles.IntegralHeight = false;
            listBoxMIDIFiles.Location = new Point(3, 19);
            listBoxMIDIFiles.Name = "listBoxMIDIFiles";
            listBoxMIDIFiles.Size = new Size(236, 316);
            listBoxMIDIFiles.TabIndex = 0;
            listBoxMIDIFiles.SelectedIndexChanged += listBoxMIDIFiles_SelectedIndexChanged;
            // 
            // splitContainerMain
            // 
            splitContainerMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainerMain.Location = new Point(12, 41);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(splitContainerProfilesAndFiles);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(groupBoxDebugLog);
            splitContainerMain.Panel2.Controls.Add(groupBoxSettings);
            splitContainerMain.Panel2.Controls.Add(groupBoxMIDIControls);
            splitContainerMain.Size = new Size(825, 347);
            splitContainerMain.SplitterDistance = 415;
            splitContainerMain.TabIndex = 4;
            // 
            // splitContainerProfilesAndFiles
            // 
            splitContainerProfilesAndFiles.Dock = DockStyle.Fill;
            splitContainerProfilesAndFiles.Location = new Point(0, 0);
            splitContainerProfilesAndFiles.Name = "splitContainerProfilesAndFiles";
            // 
            // splitContainerProfilesAndFiles.Panel1
            // 
            splitContainerProfilesAndFiles.Panel1.Controls.Add(buttonEditKeyMaps);
            splitContainerProfilesAndFiles.Panel1.Controls.Add(groupBoxMIDIkeyMapProfiles);
            splitContainerProfilesAndFiles.Panel1MinSize = 142;
            // 
            // splitContainerProfilesAndFiles.Panel2
            // 
            splitContainerProfilesAndFiles.Panel2.Controls.Add(groupBoxSelectMIDIFolder);
            splitContainerProfilesAndFiles.Panel2MinSize = 78;
            splitContainerProfilesAndFiles.Size = new Size(415, 347);
            splitContainerProfilesAndFiles.SplitterDistance = 161;
            splitContainerProfilesAndFiles.TabIndex = 4;
            // 
            // buttonEditKeyMaps
            // 
            buttonEditKeyMaps.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            buttonEditKeyMaps.BackColor = Color.DimGray;
            buttonEditKeyMaps.FlatAppearance.MouseDownBackColor = Color.Gray;
            buttonEditKeyMaps.FlatAppearance.MouseOverBackColor = Color.Silver;
            buttonEditKeyMaps.FlatStyle = FlatStyle.System;
            buttonEditKeyMaps.Location = new Point(3, 320);
            buttonEditKeyMaps.Name = "buttonEditKeyMaps";
            buttonEditKeyMaps.Size = new Size(156, 24);
            buttonEditKeyMaps.TabIndex = 3;
            buttonEditKeyMaps.Text = "MIDI KeyMap Editor";
            buttonEditKeyMaps.UseVisualStyleBackColor = false;
            buttonEditKeyMaps.Click += buttonEditKeyMaps_Click;
            // 
            // groupBoxDebugLog
            // 
            groupBoxDebugLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxDebugLog.Controls.Add(richTextBoxLog);
            groupBoxDebugLog.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxDebugLog.ForeColor = Color.LightGray;
            groupBoxDebugLog.Location = new Point(3, 251);
            groupBoxDebugLog.Name = "groupBoxDebugLog";
            groupBoxDebugLog.Size = new Size(400, 90);
            groupBoxDebugLog.TabIndex = 6;
            groupBoxDebugLog.TabStop = false;
            groupBoxDebugLog.Text = "Debug Log";
            // 
            // richTextBoxLog
            // 
            richTextBoxLog.BackColor = Color.FromArgb(64, 64, 64);
            richTextBoxLog.BorderStyle = BorderStyle.None;
            richTextBoxLog.Dock = DockStyle.Fill;
            richTextBoxLog.Font = new Font("Segoe UI", 8F);
            richTextBoxLog.ForeColor = Color.LightGray;
            richTextBoxLog.Location = new Point(3, 19);
            richTextBoxLog.Name = "richTextBoxLog";
            richTextBoxLog.Size = new Size(394, 68);
            richTextBoxLog.TabIndex = 0;
            richTextBoxLog.Text = "";
            // 
            // groupBoxSettings
            // 
            groupBoxSettings.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxSettings.Controls.Add(labelModifiedDelay);
            groupBoxSettings.Controls.Add(labelModifierDelayLabel);
            groupBoxSettings.Controls.Add(trackBarModifierDelay);
            groupBoxSettings.Controls.Add(labelTempo);
            groupBoxSettings.Controls.Add(trackBarTempo);
            groupBoxSettings.Controls.Add(labelTempoLabel);
            groupBoxSettings.Controls.Add(checkBoxAlwaysOnTop);
            groupBoxSettings.Controls.Add(checkBoxRepeatSong);
            groupBoxSettings.FlatStyle = FlatStyle.Flat;
            groupBoxSettings.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxSettings.ForeColor = Color.LightGray;
            groupBoxSettings.Location = new Point(3, 114);
            groupBoxSettings.Name = "groupBoxSettings";
            groupBoxSettings.Size = new Size(399, 131);
            groupBoxSettings.TabIndex = 5;
            groupBoxSettings.TabStop = false;
            groupBoxSettings.Text = "Settings";
            // 
            // labelModifiedDelay
            // 
            labelModifiedDelay.Anchor = AnchorStyles.Right;
            labelModifiedDelay.AutoSize = true;
            labelModifiedDelay.Font = new Font("Segoe UI", 9F);
            labelModifiedDelay.Location = new Point(353, 84);
            labelModifiedDelay.Name = "labelModifiedDelay";
            labelModifiedDelay.Size = new Size(13, 15);
            labelModifiedDelay.TabIndex = 13;
            labelModifiedDelay.Text = "0";
            // 
            // labelModifierDelayLabel
            // 
            labelModifierDelayLabel.Anchor = AnchorStyles.Right;
            labelModifierDelayLabel.AutoSize = true;
            labelModifierDelayLabel.Font = new Font("Segoe UI", 9F);
            labelModifierDelayLabel.Location = new Point(122, 84);
            labelModifierDelayLabel.Name = "labelModifierDelayLabel";
            labelModifierDelayLabel.Size = new Size(87, 15);
            labelModifierDelayLabel.TabIndex = 12;
            labelModifierDelayLabel.Text = "Modifier Delay:";
            // 
            // trackBarModifierDelay
            // 
            trackBarModifierDelay.Anchor = AnchorStyles.Right;
            trackBarModifierDelay.Location = new Point(215, 72);
            trackBarModifierDelay.Maximum = 100;
            trackBarModifierDelay.Name = "trackBarModifierDelay";
            trackBarModifierDelay.Size = new Size(131, 45);
            trackBarModifierDelay.TabIndex = 11;
            trackBarModifierDelay.TickStyle = TickStyle.Both;
            trackBarModifierDelay.Scroll += trackBarModifierDelay_Scroll;
            // 
            // labelTempo
            // 
            labelTempo.Anchor = AnchorStyles.Right;
            labelTempo.AutoSize = true;
            labelTempo.Font = new Font("Segoe UI", 9F);
            labelTempo.Location = new Point(353, 34);
            labelTempo.Name = "labelTempo";
            labelTempo.Size = new Size(13, 15);
            labelTempo.TabIndex = 6;
            labelTempo.Text = "0";
            // 
            // trackBarTempo
            // 
            trackBarTempo.Anchor = AnchorStyles.Right;
            trackBarTempo.Location = new Point(215, 21);
            trackBarTempo.Maximum = 20;
            trackBarTempo.Minimum = -10;
            trackBarTempo.Name = "trackBarTempo";
            trackBarTempo.Size = new Size(131, 45);
            trackBarTempo.TabIndex = 5;
            trackBarTempo.TickStyle = TickStyle.Both;
            trackBarTempo.Scroll += trackBarTempo_Scroll;
            // 
            // labelTempoLabel
            // 
            labelTempoLabel.Anchor = AnchorStyles.Right;
            labelTempoLabel.AutoSize = true;
            labelTempoLabel.Font = new Font("Segoe UI", 9F);
            labelTempoLabel.Location = new Point(163, 34);
            labelTempoLabel.Name = "labelTempoLabel";
            labelTempoLabel.Size = new Size(46, 15);
            labelTempoLabel.TabIndex = 4;
            labelTempoLabel.Text = "Tempo:";
            // 
            // checkBoxAlwaysOnTop
            // 
            checkBoxAlwaysOnTop.AutoSize = true;
            checkBoxAlwaysOnTop.Font = new Font("Segoe UI", 9F);
            checkBoxAlwaysOnTop.Location = new Point(6, 47);
            checkBoxAlwaysOnTop.Name = "checkBoxAlwaysOnTop";
            checkBoxAlwaysOnTop.Size = new Size(104, 19);
            checkBoxAlwaysOnTop.TabIndex = 0;
            checkBoxAlwaysOnTop.Text = "Always On Top";
            checkBoxAlwaysOnTop.UseVisualStyleBackColor = true;
            checkBoxAlwaysOnTop.CheckedChanged += checkBoxAlwaysOnTop_CheckedChanged;
            // 
            // checkBoxRepeatSong
            // 
            checkBoxRepeatSong.AutoSize = true;
            checkBoxRepeatSong.Font = new Font("Segoe UI", 9F);
            checkBoxRepeatSong.Location = new Point(7, 22);
            checkBoxRepeatSong.Name = "checkBoxRepeatSong";
            checkBoxRepeatSong.Size = new Size(92, 19);
            checkBoxRepeatSong.TabIndex = 0;
            checkBoxRepeatSong.Text = "Repeat Song";
            checkBoxRepeatSong.UseVisualStyleBackColor = true;
            checkBoxRepeatSong.CheckedChanged += checkBoxRepeatSong_CheckedChanged;
            // 
            // pictureBoxPayPal
            // 
            pictureBoxPayPal.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            pictureBoxPayPal.BackgroundImage = (Image)resources.GetObject("pictureBoxPayPal.BackgroundImage");
            pictureBoxPayPal.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBoxPayPal.Location = new Point(737, 480);
            pictureBoxPayPal.Name = "pictureBoxPayPal";
            pictureBoxPayPal.Size = new Size(97, 84);
            pictureBoxPayPal.TabIndex = 4;
            pictureBoxPayPal.TabStop = false;
            pictureBoxPayPal.Click += pictureBoxPayPal_Click;
            // 
            // labelPayPalLabel
            // 
            labelPayPalLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelPayPalLabel.Location = new Point(453, 477);
            labelPayPalLabel.Name = "labelPayPalLabel";
            labelPayPalLabel.Size = new Size(279, 87);
            labelPayPalLabel.TabIndex = 3;
            labelPayPalLabel.Text = "Support my work! Every donation helps me improve this app and keep it free (and ad free). Donate via PayPal: https://paypal.me/PsystecZA. Thank you! ❤️";
            labelPayPalLabel.TextAlign = ContentAlignment.MiddleCenter;
            labelPayPalLabel.Click += labelPayPalLabel_Click;
            // 
            // panelResizeHandle
            // 
            panelResizeHandle.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            panelResizeHandle.Cursor = Cursors.SizeNWSE;
            panelResizeHandle.Location = new Point(832, 559);
            panelResizeHandle.Name = "panelResizeHandle";
            panelResizeHandle.Size = new Size(25, 24);
            panelResizeHandle.TabIndex = 5;
            // 
            // groupBoxKeyVisualizer
            // 
            groupBoxKeyVisualizer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            groupBoxKeyVisualizer.Controls.Add(panelMidiNoteVisualizer);
            groupBoxKeyVisualizer.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxKeyVisualizer.ForeColor = Color.LightGray;
            groupBoxKeyVisualizer.Location = new Point(15, 394);
            groupBoxKeyVisualizer.Name = "groupBoxKeyVisualizer";
            groupBoxKeyVisualizer.Size = new Size(432, 170);
            groupBoxKeyVisualizer.TabIndex = 6;
            groupBoxKeyVisualizer.TabStop = false;
            groupBoxKeyVisualizer.Text = "MIDI Note Visualizer";
            // 
            // panelMidiNoteVisualizer
            // 
            panelMidiNoteVisualizer.Dock = DockStyle.Fill;
            panelMidiNoteVisualizer.Location = new Point(3, 19);
            panelMidiNoteVisualizer.Name = "panelMidiNoteVisualizer";
            panelMidiNoteVisualizer.Size = new Size(426, 148);
            panelMidiNoteVisualizer.TabIndex = 0;
            // 
            // groupBoxMIDIChannels
            // 
            groupBoxMIDIChannels.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel16);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel08);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel15);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel07);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel14);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel06);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel13);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel05);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel12);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel04);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel11);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel03);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel10);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel02);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel09);
            groupBoxMIDIChannels.Controls.Add(checkBoxMIDIChannel01);
            groupBoxMIDIChannels.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxMIDIChannels.ForeColor = Color.DarkGray;
            groupBoxMIDIChannels.Location = new Point(458, 394);
            groupBoxMIDIChannels.Name = "groupBoxMIDIChannels";
            groupBoxMIDIChannels.Size = new Size(376, 80);
            groupBoxMIDIChannels.TabIndex = 7;
            groupBoxMIDIChannels.TabStop = false;
            groupBoxMIDIChannels.Text = "MIDI Channels";
            // 
            // checkBoxMIDIChannel16
            // 
            checkBoxMIDIChannel16.AutoSize = true;
            checkBoxMIDIChannel16.Checked = true;
            checkBoxMIDIChannel16.CheckState = CheckState.Checked;
            checkBoxMIDIChannel16.ForeColor = Color.LightGray;
            checkBoxMIDIChannel16.Location = new Point(328, 47);
            checkBoxMIDIChannel16.Name = "checkBoxMIDIChannel16";
            checkBoxMIDIChannel16.Size = new Size(40, 19);
            checkBoxMIDIChannel16.TabIndex = 0;
            checkBoxMIDIChannel16.Tag = "16";
            checkBoxMIDIChannel16.Text = "16";
            checkBoxMIDIChannel16.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel16.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel08
            // 
            checkBoxMIDIChannel08.AutoSize = true;
            checkBoxMIDIChannel08.Checked = true;
            checkBoxMIDIChannel08.CheckState = CheckState.Checked;
            checkBoxMIDIChannel08.ForeColor = Color.LightGray;
            checkBoxMIDIChannel08.Location = new Point(328, 22);
            checkBoxMIDIChannel08.Name = "checkBoxMIDIChannel08";
            checkBoxMIDIChannel08.Size = new Size(40, 19);
            checkBoxMIDIChannel08.TabIndex = 0;
            checkBoxMIDIChannel08.Tag = "8";
            checkBoxMIDIChannel08.Text = "08";
            checkBoxMIDIChannel08.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel08.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel15
            // 
            checkBoxMIDIChannel15.AutoSize = true;
            checkBoxMIDIChannel15.Checked = true;
            checkBoxMIDIChannel15.CheckState = CheckState.Checked;
            checkBoxMIDIChannel15.ForeColor = Color.LightGray;
            checkBoxMIDIChannel15.Location = new Point(282, 47);
            checkBoxMIDIChannel15.Name = "checkBoxMIDIChannel15";
            checkBoxMIDIChannel15.Size = new Size(40, 19);
            checkBoxMIDIChannel15.TabIndex = 0;
            checkBoxMIDIChannel15.Tag = "15";
            checkBoxMIDIChannel15.Text = "15";
            checkBoxMIDIChannel15.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel15.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel07
            // 
            checkBoxMIDIChannel07.AutoSize = true;
            checkBoxMIDIChannel07.Checked = true;
            checkBoxMIDIChannel07.CheckState = CheckState.Checked;
            checkBoxMIDIChannel07.ForeColor = Color.LightGray;
            checkBoxMIDIChannel07.Location = new Point(282, 22);
            checkBoxMIDIChannel07.Name = "checkBoxMIDIChannel07";
            checkBoxMIDIChannel07.Size = new Size(40, 19);
            checkBoxMIDIChannel07.TabIndex = 0;
            checkBoxMIDIChannel07.Tag = "7";
            checkBoxMIDIChannel07.Text = "07";
            checkBoxMIDIChannel07.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel07.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel14
            // 
            checkBoxMIDIChannel14.AutoSize = true;
            checkBoxMIDIChannel14.Checked = true;
            checkBoxMIDIChannel14.CheckState = CheckState.Checked;
            checkBoxMIDIChannel14.ForeColor = Color.LightGray;
            checkBoxMIDIChannel14.Location = new Point(236, 47);
            checkBoxMIDIChannel14.Name = "checkBoxMIDIChannel14";
            checkBoxMIDIChannel14.Size = new Size(40, 19);
            checkBoxMIDIChannel14.TabIndex = 0;
            checkBoxMIDIChannel14.Tag = "14";
            checkBoxMIDIChannel14.Text = "14";
            checkBoxMIDIChannel14.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel14.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel06
            // 
            checkBoxMIDIChannel06.AutoSize = true;
            checkBoxMIDIChannel06.Checked = true;
            checkBoxMIDIChannel06.CheckState = CheckState.Checked;
            checkBoxMIDIChannel06.ForeColor = Color.LightGray;
            checkBoxMIDIChannel06.Location = new Point(236, 22);
            checkBoxMIDIChannel06.Name = "checkBoxMIDIChannel06";
            checkBoxMIDIChannel06.Size = new Size(40, 19);
            checkBoxMIDIChannel06.TabIndex = 0;
            checkBoxMIDIChannel06.Tag = "6";
            checkBoxMIDIChannel06.Text = "06";
            checkBoxMIDIChannel06.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel06.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel13
            // 
            checkBoxMIDIChannel13.AutoSize = true;
            checkBoxMIDIChannel13.Checked = true;
            checkBoxMIDIChannel13.CheckState = CheckState.Checked;
            checkBoxMIDIChannel13.ForeColor = Color.LightGray;
            checkBoxMIDIChannel13.Location = new Point(190, 47);
            checkBoxMIDIChannel13.Name = "checkBoxMIDIChannel13";
            checkBoxMIDIChannel13.Size = new Size(40, 19);
            checkBoxMIDIChannel13.TabIndex = 0;
            checkBoxMIDIChannel13.Tag = "13";
            checkBoxMIDIChannel13.Text = "13";
            checkBoxMIDIChannel13.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel13.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel05
            // 
            checkBoxMIDIChannel05.AutoSize = true;
            checkBoxMIDIChannel05.Checked = true;
            checkBoxMIDIChannel05.CheckState = CheckState.Checked;
            checkBoxMIDIChannel05.ForeColor = Color.LightGray;
            checkBoxMIDIChannel05.Location = new Point(190, 22);
            checkBoxMIDIChannel05.Name = "checkBoxMIDIChannel05";
            checkBoxMIDIChannel05.Size = new Size(40, 19);
            checkBoxMIDIChannel05.TabIndex = 0;
            checkBoxMIDIChannel05.Tag = "5";
            checkBoxMIDIChannel05.Text = "05";
            checkBoxMIDIChannel05.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel05.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel12
            // 
            checkBoxMIDIChannel12.AutoSize = true;
            checkBoxMIDIChannel12.Checked = true;
            checkBoxMIDIChannel12.CheckState = CheckState.Checked;
            checkBoxMIDIChannel12.ForeColor = Color.LightGray;
            checkBoxMIDIChannel12.Location = new Point(144, 47);
            checkBoxMIDIChannel12.Name = "checkBoxMIDIChannel12";
            checkBoxMIDIChannel12.Size = new Size(40, 19);
            checkBoxMIDIChannel12.TabIndex = 0;
            checkBoxMIDIChannel12.Tag = "12";
            checkBoxMIDIChannel12.Text = "12";
            checkBoxMIDIChannel12.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel12.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel04
            // 
            checkBoxMIDIChannel04.AutoSize = true;
            checkBoxMIDIChannel04.Checked = true;
            checkBoxMIDIChannel04.CheckState = CheckState.Checked;
            checkBoxMIDIChannel04.ForeColor = Color.LightGray;
            checkBoxMIDIChannel04.Location = new Point(144, 22);
            checkBoxMIDIChannel04.Name = "checkBoxMIDIChannel04";
            checkBoxMIDIChannel04.Size = new Size(40, 19);
            checkBoxMIDIChannel04.TabIndex = 0;
            checkBoxMIDIChannel04.Tag = "4";
            checkBoxMIDIChannel04.Text = "04";
            checkBoxMIDIChannel04.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel04.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel11
            // 
            checkBoxMIDIChannel11.AutoSize = true;
            checkBoxMIDIChannel11.Checked = true;
            checkBoxMIDIChannel11.CheckState = CheckState.Checked;
            checkBoxMIDIChannel11.ForeColor = Color.LightGray;
            checkBoxMIDIChannel11.Location = new Point(98, 47);
            checkBoxMIDIChannel11.Name = "checkBoxMIDIChannel11";
            checkBoxMIDIChannel11.Size = new Size(40, 19);
            checkBoxMIDIChannel11.TabIndex = 0;
            checkBoxMIDIChannel11.Tag = "11";
            checkBoxMIDIChannel11.Text = "11";
            checkBoxMIDIChannel11.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel11.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel03
            // 
            checkBoxMIDIChannel03.AutoSize = true;
            checkBoxMIDIChannel03.Checked = true;
            checkBoxMIDIChannel03.CheckState = CheckState.Checked;
            checkBoxMIDIChannel03.ForeColor = Color.LightGray;
            checkBoxMIDIChannel03.Location = new Point(98, 22);
            checkBoxMIDIChannel03.Name = "checkBoxMIDIChannel03";
            checkBoxMIDIChannel03.Size = new Size(40, 19);
            checkBoxMIDIChannel03.TabIndex = 0;
            checkBoxMIDIChannel03.Tag = "3";
            checkBoxMIDIChannel03.Text = "03";
            checkBoxMIDIChannel03.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel03.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel10
            // 
            checkBoxMIDIChannel10.AutoSize = true;
            checkBoxMIDIChannel10.Checked = true;
            checkBoxMIDIChannel10.CheckState = CheckState.Checked;
            checkBoxMIDIChannel10.ForeColor = Color.LightGray;
            checkBoxMIDIChannel10.Location = new Point(52, 47);
            checkBoxMIDIChannel10.Name = "checkBoxMIDIChannel10";
            checkBoxMIDIChannel10.Size = new Size(40, 19);
            checkBoxMIDIChannel10.TabIndex = 0;
            checkBoxMIDIChannel10.Tag = "10";
            checkBoxMIDIChannel10.Text = "10";
            checkBoxMIDIChannel10.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel10.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel02
            // 
            checkBoxMIDIChannel02.AutoSize = true;
            checkBoxMIDIChannel02.Checked = true;
            checkBoxMIDIChannel02.CheckState = CheckState.Checked;
            checkBoxMIDIChannel02.ForeColor = Color.LightGray;
            checkBoxMIDIChannel02.Location = new Point(52, 22);
            checkBoxMIDIChannel02.Name = "checkBoxMIDIChannel02";
            checkBoxMIDIChannel02.Size = new Size(40, 19);
            checkBoxMIDIChannel02.TabIndex = 0;
            checkBoxMIDIChannel02.Tag = "2";
            checkBoxMIDIChannel02.Text = "02";
            checkBoxMIDIChannel02.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel02.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel09
            // 
            checkBoxMIDIChannel09.AutoSize = true;
            checkBoxMIDIChannel09.Checked = true;
            checkBoxMIDIChannel09.CheckState = CheckState.Checked;
            checkBoxMIDIChannel09.ForeColor = Color.LightGray;
            checkBoxMIDIChannel09.Location = new Point(6, 47);
            checkBoxMIDIChannel09.Name = "checkBoxMIDIChannel09";
            checkBoxMIDIChannel09.Size = new Size(40, 19);
            checkBoxMIDIChannel09.TabIndex = 0;
            checkBoxMIDIChannel09.Tag = "9";
            checkBoxMIDIChannel09.Text = "09";
            checkBoxMIDIChannel09.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel09.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // checkBoxMIDIChannel01
            // 
            checkBoxMIDIChannel01.AutoSize = true;
            checkBoxMIDIChannel01.Checked = true;
            checkBoxMIDIChannel01.CheckState = CheckState.Checked;
            checkBoxMIDIChannel01.ForeColor = Color.LightGray;
            checkBoxMIDIChannel01.Location = new Point(6, 22);
            checkBoxMIDIChannel01.Name = "checkBoxMIDIChannel01";
            checkBoxMIDIChannel01.Size = new Size(40, 19);
            checkBoxMIDIChannel01.TabIndex = 0;
            checkBoxMIDIChannel01.Tag = "1";
            checkBoxMIDIChannel01.Text = "01";
            checkBoxMIDIChannel01.UseVisualStyleBackColor = true;
            checkBoxMIDIChannel01.CheckedChanged += checkBoxMidiChannel_CheckedChanged;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(849, 576);
            Controls.Add(groupBoxMIDIChannels);
            Controls.Add(groupBoxKeyVisualizer);
            Controls.Add(panelResizeHandle);
            Controls.Add(pictureBoxPayPal);
            Controls.Add(splitContainerMain);
            Controls.Add(labelPayPalLabel);
            Controls.Add(panelTop);
            ForeColor = Color.Silver;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(849, 576);
            Name = "FormMain";
            Text = "MIDI Monkey";
            Load += FormMain_Load;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSignalApp).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDiscord).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            groupBoxMIDIkeyMapProfiles.ResumeLayout(false);
            groupBoxMIDIControls.ResumeLayout(false);
            groupBoxMIDIControls.PerformLayout();
            groupBoxSelectMIDIFolder.ResumeLayout(false);
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            splitContainerProfilesAndFiles.Panel1.ResumeLayout(false);
            splitContainerProfilesAndFiles.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerProfilesAndFiles).EndInit();
            splitContainerProfilesAndFiles.ResumeLayout(false);
            groupBoxDebugLog.ResumeLayout(false);
            groupBoxSettings.ResumeLayout(false);
            groupBoxSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarModifierDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarTempo).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPayPal).EndInit();
            groupBoxKeyVisualizer.ResumeLayout(false);
            groupBoxMIDIChannels.ResumeLayout(false);
            groupBoxMIDIChannels.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private Label labelAppNameLabel;
        private Button buttonClose;
        private GroupBox groupBoxMIDIkeyMapProfiles;
        private GroupBox groupBoxMIDIControls;
        private GroupBox groupBoxSelectMIDIFolder;
        private SplitContainer splitContainerMain;
        private SplitContainer splitContainerProfilesAndFiles;
        private Button buttonSelectMIDIFolder;
        private Panel panelResizeHandle;
        private Button buttonStopSong;
        private Button buttonPlaySong;
        private ListBox listBoxMIDIKeyMaps;
        private ListBox listBoxMIDIFiles;
        private ComboBox comboBoxMIDIDevices;
        private Label labelMIDIDeviceLabel;
        private Button buttonUseMidiDevice;
        private Label labelPayPalLabel;
        private PictureBox pictureBoxPayPal;
        private GroupBox groupBoxSettings;
        private CheckBox checkBoxRepeatSong;
        private CheckBox checkBoxAlwaysOnTop;
        private Label labelModifiedDelay;
        private Label labelModifierDelayLabel;
        private TrackBar trackBarModifierDelay;
        private Label labelTempo;
        private TrackBar trackBarTempo;
        private Label labelTempoLabel;
        private GroupBox groupBoxKeyVisualizer;
        private Panel panelMidiNoteVisualizer;
        private PictureBox pictureBoxLogo;
        private GroupBox groupBoxDebugLog;
        private RichTextBox richTextBoxLog;
        private Button buttonEditKeyMaps;
        private GroupBox groupBoxMIDIChannels;
        private CheckBox checkBoxMIDIChannel16;
        private CheckBox checkBoxMIDIChannel08;
        private CheckBox checkBoxMIDIChannel15;
        private CheckBox checkBoxMIDIChannel07;
        private CheckBox checkBoxMIDIChannel14;
        private CheckBox checkBoxMIDIChannel06;
        private CheckBox checkBoxMIDIChannel13;
        private CheckBox checkBoxMIDIChannel05;
        private CheckBox checkBoxMIDIChannel12;
        private CheckBox checkBoxMIDIChannel04;
        private CheckBox checkBoxMIDIChannel11;
        private CheckBox checkBoxMIDIChannel03;
        private CheckBox checkBoxMIDIChannel10;
        private CheckBox checkBoxMIDIChannel02;
        private CheckBox checkBoxMIDIChannel09;
        private CheckBox checkBoxMIDIChannel01;
        private PictureBox pictureBoxSignalApp;
        private PictureBox pictureBoxDiscord;
    }
}
