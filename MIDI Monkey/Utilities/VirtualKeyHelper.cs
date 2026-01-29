using MIDI_Monkey.Models;

namespace MIDI_Monkey.Utilities
{
    /// <summary>
    /// Helper class for keyboard layout-specific information
    /// </summary>
    public static class VirtualKeyHelper
    {
        /// <summary>
        /// Get a friendly name for a virtual key
        /// </summary>
        public static string GetKeyName(VirtualKey key)
        {
            return key switch
            {
                // Mouse buttons
                VirtualKey.LeftButton => "Left Mouse Button",
                VirtualKey.RightButton => "Right Mouse Button",
                VirtualKey.Cancel => "Cancel",
                VirtualKey.MiddleButton => "Middle Mouse Button",
                VirtualKey.XButton1 => "Mouse X1",
                VirtualKey.XButton2 => "Mouse X2",

                // Control keys
                VirtualKey.Back => "Backspace",
                VirtualKey.Tab => "Tab",
                VirtualKey.Clear => "Clear",
                VirtualKey.Enter => "Enter",
                VirtualKey.Shift => "Shift",
                VirtualKey.Control => "Ctrl",
                VirtualKey.Alt => "Alt",
                VirtualKey.Pause => "Pause",
                VirtualKey.CapsLock => "Caps Lock",
                VirtualKey.Escape => "Esc",
                VirtualKey.Space => "Space",
                VirtualKey.PageUp => "Page Up",
                VirtualKey.PageDown => "Page Down",
                VirtualKey.End => "End",
                VirtualKey.Home => "Home",
                VirtualKey.Left => "Left Arrow",
                VirtualKey.Up => "Up Arrow",
                VirtualKey.Right => "Right Arrow",
                VirtualKey.Down => "Down Arrow",
                VirtualKey.Select => "Select",
                VirtualKey.Print => "Print",
                VirtualKey.Execute => "Execute",
                VirtualKey.PrintScreen => "Print Screen",
                VirtualKey.Insert => "Insert",
                VirtualKey.Delete => "Delete",
                VirtualKey.Help => "Help",

                // Number row
                VirtualKey.Number0 => "0",
                VirtualKey.Number1 => "1",
                VirtualKey.Number2 => "2",
                VirtualKey.Number3 => "3",
                VirtualKey.Number4 => "4",
                VirtualKey.Number5 => "5",
                VirtualKey.Number6 => "6",
                VirtualKey.Number7 => "7",
                VirtualKey.Number8 => "8",
                VirtualKey.Number9 => "9",

                // Letters
                VirtualKey.A => "A",
                VirtualKey.B => "B",
                VirtualKey.C => "C",
                VirtualKey.D => "D",
                VirtualKey.E => "E",
                VirtualKey.F => "F",
                VirtualKey.G => "G",
                VirtualKey.H => "H",
                VirtualKey.I => "I",
                VirtualKey.J => "J",
                VirtualKey.K => "K",
                VirtualKey.L => "L",
                VirtualKey.M => "M",
                VirtualKey.N => "N",
                VirtualKey.O => "O",
                VirtualKey.P => "P",
                VirtualKey.Q => "Q",
                VirtualKey.R => "R",
                VirtualKey.S => "S",
                VirtualKey.T => "T",
                VirtualKey.U => "U",
                VirtualKey.V => "V",
                VirtualKey.W => "W",
                VirtualKey.X => "X",
                VirtualKey.Y => "Y",
                VirtualKey.Z => "Z",

                // Windows keys
                VirtualKey.LeftWindows => "Left Windows",
                VirtualKey.RightWindows => "Right Windows",
                VirtualKey.Apps => "Menu",
                VirtualKey.Sleep => "Sleep",

                // Numpad
                VirtualKey.Numpad0 => "Numpad 0",
                VirtualKey.Numpad1 => "Numpad 1",
                VirtualKey.Numpad2 => "Numpad 2",
                VirtualKey.Numpad3 => "Numpad 3",
                VirtualKey.Numpad4 => "Numpad 4",
                VirtualKey.Numpad5 => "Numpad 5",
                VirtualKey.Numpad6 => "Numpad 6",
                VirtualKey.Numpad7 => "Numpad 7",
                VirtualKey.Numpad8 => "Numpad 8",
                VirtualKey.Numpad9 => "Numpad 9",
                VirtualKey.Multiply => "Numpad *",
                VirtualKey.Add => "Numpad +",
                VirtualKey.Separator => "Numpad Separator",
                VirtualKey.Subtract => "Numpad -",
                VirtualKey.Decimal => "Numpad .",
                VirtualKey.Divide => "Numpad /",

                // Function keys
                VirtualKey.F1 => "F1",
                VirtualKey.F2 => "F2",
                VirtualKey.F3 => "F3",
                VirtualKey.F4 => "F4",
                VirtualKey.F5 => "F5",
                VirtualKey.F6 => "F6",
                VirtualKey.F7 => "F7",
                VirtualKey.F8 => "F8",
                VirtualKey.F9 => "F9",
                VirtualKey.F10 => "F10",
                VirtualKey.F11 => "F11",
                VirtualKey.F12 => "F12",
                VirtualKey.F13 => "F13",
                VirtualKey.F14 => "F14",
                VirtualKey.F15 => "F15",
                VirtualKey.F16 => "F16",
                VirtualKey.F17 => "F17",
                VirtualKey.F18 => "F18",
                VirtualKey.F19 => "F19",
                VirtualKey.F20 => "F20",
                VirtualKey.F21 => "F21",
                VirtualKey.F22 => "F22",
                VirtualKey.F23 => "F23",
                VirtualKey.F24 => "F24",

                // Lock keys
                VirtualKey.NumLock => "Num Lock",
                VirtualKey.ScrollLock => "Scroll Lock",

                // Specific modifiers
                VirtualKey.LeftShift => "Left Shift",
                VirtualKey.RightShift => "Right Shift",
                VirtualKey.LeftControl => "Left Ctrl",
                VirtualKey.RightControl => "Right Ctrl",
                VirtualKey.LeftAlt => "Left Alt",
                VirtualKey.RightAlt => "Right Alt",

                // Browser keys
                VirtualKey.BrowserBack => "Browser Back",
                VirtualKey.BrowserForward => "Browser Forward",
                VirtualKey.BrowserRefresh => "Browser Refresh",
                VirtualKey.BrowserStop => "Browser Stop",
                VirtualKey.BrowserSearch => "Browser Search",
                VirtualKey.BrowserFavorites => "Browser Favorites",
                VirtualKey.BrowserHome => "Browser Home",

                // Volume keys
                VirtualKey.VolumeMute => "Volume Mute",
                VirtualKey.VolumeDown => "Volume Down",
                VirtualKey.VolumeUp => "Volume Up",

                // Media keys
                VirtualKey.MediaNextTrack => "Next Track",
                VirtualKey.MediaPreviousTrack => "Previous Track",
                VirtualKey.MediaStop => "Media Stop",
                VirtualKey.MediaPlayPause => "Play/Pause",

                // Launch keys
                VirtualKey.LaunchMail => "Launch Mail",
                VirtualKey.LaunchMediaSelect => "Launch Media",
                VirtualKey.LaunchApp1 => "Launch App 1",
                VirtualKey.LaunchApp2 => "Launch App 2",

                // OEM keys (US layout)
                VirtualKey.OEM1 => "; :",
                VirtualKey.OEMPlus => "= +",
                VirtualKey.OEMComma => ", <",
                VirtualKey.OEMMinus => "- _",
                VirtualKey.OEMPeriod => ". >",
                VirtualKey.OEM2 => "/ ?",
                VirtualKey.OEM3 => "` ~",
                VirtualKey.OEM4 => "[ {",
                VirtualKey.OEM5 => "\\ |",
                VirtualKey.OEM6 => "] }",
                VirtualKey.OEM7 => "' \"",
                VirtualKey.OEM8 => "OEM 8",
                VirtualKey.OEM102 => "< > \\ |",

                // IME and special keys
                VirtualKey.ProcessKey => "IME Process",
                VirtualKey.Packet => "Packet",

                // ATTN keys (rarely used)
                VirtualKey.Attn => "Attn",
                VirtualKey.CrSel => "CrSel",
                VirtualKey.ExSel => "ExSel",
                VirtualKey.EraseEOF => "Erase EOF",
                VirtualKey.Play => "Play",
                VirtualKey.Zoom => "Zoom",
                VirtualKey.NoName => "NoName",
                VirtualKey.PA1 => "PA1",
                VirtualKey.OEMClear => "OEM Clear",

                _ => key.ToString()
            };
        }

        /// <summary>
        /// Check if a key is a modifier key
        /// </summary>
        public static bool IsModifier(VirtualKey key)
        {
            return key == VirtualKey.Shift || key == VirtualKey.Control || key == VirtualKey.Alt ||
                   key == VirtualKey.LeftShift || key == VirtualKey.RightShift ||
                   key == VirtualKey.LeftControl || key == VirtualKey.RightControl ||
                   key == VirtualKey.LeftAlt || key == VirtualKey.RightAlt ||
                   key == VirtualKey.LeftWindows || key == VirtualKey.RightWindows;
        }

        /// <summary>
        /// Check if a key is a function key (F1-F24)
        /// </summary>
        public static bool IsFunctionKey(VirtualKey key)
        {
            return key >= VirtualKey.F1 && key <= VirtualKey.F24;
        }

        /// <summary>
        /// Check if a key is a numpad key
        /// </summary>
        public static bool IsNumpadKey(VirtualKey key)
        {
            return (key >= VirtualKey.Numpad0 && key <= VirtualKey.Divide);
        }

        /// <summary>
        /// Check if a key is a letter (A-Z)
        /// </summary>
        public static bool IsLetter(VirtualKey key)
        {
            return key >= VirtualKey.A && key <= VirtualKey.Z;
        }

        /// <summary>
        /// Check if a key is a number (0-9 on main keyboard)
        /// </summary>
        public static bool IsNumber(VirtualKey key)
        {
            return key >= VirtualKey.Number0 && key <= VirtualKey.Number9;
        }
    }
}
