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
            panelTop.SuspendLayout();
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
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(50, 50, 50);
            panelTop.Controls.Add(pictureBoxLogo);
            panelTop.Controls.Add(buttonClose);
            panelTop.Controls.Add(labelAppNameLabel);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(830, 35);
            panelTop.TabIndex = 0;
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
            buttonClose.Location = new Point(801, 7);
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
            groupBoxMIDIkeyMapProfiles.Size = new Size(151, 341);
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
            listBoxMIDIKeyMaps.ItemHeight = 15;
            listBoxMIDIKeyMaps.Location = new Point(3, 19);
            listBoxMIDIKeyMaps.Name = "listBoxMIDIKeyMaps";
            listBoxMIDIKeyMaps.Size = new Size(145, 319);
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
            groupBoxMIDIControls.Size = new Size(387, 105);
            groupBoxMIDIControls.TabIndex = 2;
            groupBoxMIDIControls.TabStop = false;
            groupBoxMIDIControls.Text = "MIDI Controls";
            // 
            // comboBoxMIDIDevices
            // 
            comboBoxMIDIDevices.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboBoxMIDIDevices.DrawMode = DrawMode.OwnerDrawVariable;
            comboBoxMIDIDevices.FlatStyle = FlatStyle.System;
            comboBoxMIDIDevices.FormattingEnabled = true;
            comboBoxMIDIDevices.Location = new Point(93, 63);
            comboBoxMIDIDevices.Name = "comboBoxMIDIDevices";
            comboBoxMIDIDevices.Size = new Size(167, 24);
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
            buttonUseMidiDevice.Location = new Point(266, 62);
            buttonUseMidiDevice.Name = "buttonUseMidiDevice";
            buttonUseMidiDevice.Size = new Size(114, 23);
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
            buttonStopSong.Location = new Point(286, 22);
            buttonStopSong.Name = "buttonStopSong";
            buttonStopSong.Size = new Size(93, 23);
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
            buttonPlaySong.Location = new Point(187, 22);
            buttonPlaySong.Name = "buttonPlaySong";
            buttonPlaySong.Size = new Size(93, 23);
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
            buttonSelectMIDIFolder.Size = new Size(139, 23);
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
            groupBoxSelectMIDIFolder.Size = new Size(238, 341);
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
            listBoxMIDIFiles.ItemHeight = 15;
            listBoxMIDIFiles.Location = new Point(3, 19);
            listBoxMIDIFiles.Name = "listBoxMIDIFiles";
            listBoxMIDIFiles.Size = new Size(232, 319);
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
            splitContainerMain.Size = new Size(806, 347);
            splitContainerMain.SplitterDistance = 409;
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
            splitContainerProfilesAndFiles.Panel1.Controls.Add(groupBoxMIDIkeyMapProfiles);
            splitContainerProfilesAndFiles.Panel1MinSize = 142;
            // 
            // splitContainerProfilesAndFiles.Panel2
            // 
            splitContainerProfilesAndFiles.Panel2.Controls.Add(groupBoxSelectMIDIFolder);
            splitContainerProfilesAndFiles.Panel2MinSize = 78;
            splitContainerProfilesAndFiles.Size = new Size(409, 347);
            splitContainerProfilesAndFiles.SplitterDistance = 160;
            splitContainerProfilesAndFiles.TabIndex = 4;
            // 
            // groupBoxDebugLog
            // 
            groupBoxDebugLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxDebugLog.Controls.Add(richTextBoxLog);
            groupBoxDebugLog.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxDebugLog.ForeColor = Color.LightGray;
            groupBoxDebugLog.Location = new Point(3, 251);
            groupBoxDebugLog.Name = "groupBoxDebugLog";
            groupBoxDebugLog.Size = new Size(387, 90);
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
            richTextBoxLog.Size = new Size(381, 68);
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
            groupBoxSettings.Size = new Size(387, 131);
            groupBoxSettings.TabIndex = 5;
            groupBoxSettings.TabStop = false;
            groupBoxSettings.Text = "Settings";
            // 
            // labelModifiedDelay
            // 
            labelModifiedDelay.Anchor = AnchorStyles.Right;
            labelModifiedDelay.AutoSize = true;
            labelModifiedDelay.Font = new Font("Segoe UI", 9F);
            labelModifiedDelay.Location = new Point(343, 84);
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
            labelModifierDelayLabel.Location = new Point(113, 84);
            labelModifierDelayLabel.Name = "labelModifierDelayLabel";
            labelModifierDelayLabel.Size = new Size(87, 15);
            labelModifierDelayLabel.TabIndex = 12;
            labelModifierDelayLabel.Text = "Modifier Delay:";
            // 
            // trackBarModifierDelay
            // 
            trackBarModifierDelay.Anchor = AnchorStyles.Right;
            trackBarModifierDelay.Location = new Point(206, 72);
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
            labelTempo.Location = new Point(343, 34);
            labelTempo.Name = "labelTempo";
            labelTempo.Size = new Size(13, 15);
            labelTempo.TabIndex = 6;
            labelTempo.Text = "0";
            // 
            // trackBarTempo
            // 
            trackBarTempo.Anchor = AnchorStyles.Right;
            trackBarTempo.Location = new Point(206, 21);
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
            labelTempoLabel.Location = new Point(153, 34);
            labelTempoLabel.Name = "labelTempoLabel";
            labelTempoLabel.Size = new Size(47, 15);
            labelTempoLabel.TabIndex = 4;
            labelTempoLabel.Text = "Tempo:";
            // 
            // checkBoxAlwaysOnTop
            // 
            checkBoxAlwaysOnTop.AutoSize = true;
            checkBoxAlwaysOnTop.Font = new Font("Segoe UI", 9F);
            checkBoxAlwaysOnTop.Location = new Point(6, 47);
            checkBoxAlwaysOnTop.Name = "checkBoxAlwaysOnTop";
            checkBoxAlwaysOnTop.Size = new Size(105, 19);
            checkBoxAlwaysOnTop.TabIndex = 0;
            checkBoxAlwaysOnTop.Text = "Always On Top";
            checkBoxAlwaysOnTop.UseVisualStyleBackColor = true;
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
            // 
            // pictureBoxPayPal
            // 
            pictureBoxPayPal.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            pictureBoxPayPal.BackgroundImage = (Image)resources.GetObject("pictureBoxPayPal.BackgroundImage");
            pictureBoxPayPal.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBoxPayPal.Location = new Point(718, 394);
            pictureBoxPayPal.Name = "pictureBoxPayPal";
            pictureBoxPayPal.Size = new Size(97, 170);
            pictureBoxPayPal.TabIndex = 4;
            pictureBoxPayPal.TabStop = false;
            pictureBoxPayPal.Click += pictureBoxPayPal_Click;
            // 
            // labelPayPalLabel
            // 
            labelPayPalLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelPayPalLabel.Location = new Point(453, 394);
            labelPayPalLabel.Name = "labelPayPalLabel";
            labelPayPalLabel.Size = new Size(260, 170);
            labelPayPalLabel.TabIndex = 3;
            labelPayPalLabel.Text = "Support my work! Every donation helps me improve this app and keep it free (and ad free). Donate via PayPal: https://paypal.me/PsystecZA. Thank you! ❤️";
            labelPayPalLabel.TextAlign = ContentAlignment.MiddleCenter;
            labelPayPalLabel.Click += labelPayPalLabel_Click;
            // 
            // panelResizeHandle
            // 
            panelResizeHandle.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            panelResizeHandle.Cursor = Cursors.SizeNWSE;
            panelResizeHandle.Location = new Point(813, 559);
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
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(830, 576);
            Controls.Add(groupBoxKeyVisualizer);
            Controls.Add(panelResizeHandle);
            Controls.Add(pictureBoxPayPal);
            Controls.Add(splitContainerMain);
            Controls.Add(labelPayPalLabel);
            Controls.Add(panelTop);
            ForeColor = Color.Silver;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormMain";
            Text = "MIDI Monkey";
            Load += FormMain_Load;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
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
    }
}
