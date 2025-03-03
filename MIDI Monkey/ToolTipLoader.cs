namespace MIDI_Monkey
{
    public static class ToolTipLoader
    {
        private static readonly Dictionary<string, string> ToolTips = new Dictionary<string, string>
        {
            [ControlNames.GroupBoxMIDIkeyMapProfiles] = "Manage and select different MIDI key mapping profiles.\nChoose the correct keymap that matches the game's input requirements for proper playback.",
            [ControlNames.ButtonSelectMIDIFolder] = "Select a folder containing MIDI files.\nAll MIDI files in the chosen folder will be automatically loaded into the selection menu for easy access.",
            [ControlNames.ButtonPlaySong] = "Start MIDI playback in-game.\nYou can also press **F5** as a shortcut to play the currently selected MIDI file.",
            [ControlNames.ButtonStopSong] = "Stop MIDI playback immediately.\nYou can also press **F6** as a shortcut to halt playback.",
            [ControlNames.LabelMIDIDeviceLabel] = "Select and connect a MIDI instrument for live input.\nPlay notes on your MIDI device, and they will be sent directly to the game or application in real time.",
            [ControlNames.TrackBarTempo] = "Adjust the playback speed of MIDI notes sent to the game or application.\nMove the slider to speed up or slow down note timing without altering the MIDI file.",
            [ControlNames.TrackBarModifierDelay] = "Set a delay between modifier keys (first key in the midikeymap.json) and key presses.\nUseful for fine-tuning input timing when using key modifiers.",
            [ControlNames.CheckBoxRepeatSong] = "Enable loop mode to continuously replay the selected song.\nPlayback will restart automatically when the song ends.",
            [ControlNames.CheckBoxAlwaysOnTop] = "Keep the MIDI Monkey window always visible above other applications.\nParticularly useful for single-monitor setups where quick access is needed.",
            [ControlNames.GroupBoxSelectMIDIFolder] = "Browse and select a MIDI file for playback.\nEnsure the correct MIDI file is loaded before starting playback."

        };

        public static void LoadToolTips(Form form)
        {
            ToolTip toolTip = new ToolTip
            {
                AutoPopDelay = 10000,
                InitialDelay = 500,
                ReshowDelay = 500,
                ShowAlways = true,
                ToolTipIcon = ToolTipIcon.Info,
                ToolTipTitle = "MIDI Monkey - Information"
            };

            ApplyToolTipsRecursively(form.Controls, toolTip);
        }

        private static void ApplyToolTipsRecursively(Control.ControlCollection controls, ToolTip toolTip)
        {
            foreach (Control control in controls)
            {
                if (ToolTips.TryGetValue(control.Name, out string tooltipText))
                {
                    toolTip.SetToolTip(control, tooltipText);
                }

                if (control.HasChildren)
                {
                    ApplyToolTipsRecursively(control.Controls, toolTip);
                }
            }
        }
    }

    public static class ControlNames
    {
        public const string ButtonSelectMIDIFolder = "buttonSelectMIDIFolder";
        public const string ButtonPlaySong = "buttonPlaySong";
        public const string ButtonStopSong = "buttonStopSong";
        public const string LabelMIDIDeviceLabel = "labelMIDIDeviceLabel";
        public const string TrackBarTempo = "trackBarTempo";
        public const string TrackBarModifierDelay = "trackBarModifierDelay";
        public const string CheckBoxRepeatSong = "checkBoxRepeatSong";
        public const string CheckBoxAlwaysOnTop = "checkBoxAlwaysOnTop";
        public const string GroupBoxMIDIkeyMapProfiles = "groupBoxMIDIkeyMapProfiles";
        public const string GroupBoxSelectMIDIFolder = "groupBoxSelectMIDIFolder";
    }
}
