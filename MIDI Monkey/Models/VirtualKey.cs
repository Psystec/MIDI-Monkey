namespace MIDI_Monkey.Models
{
    /// <summary>
    /// Virtual Key Codes for Windows keyboard input
    /// Based on Windows Virtual-Key Codes (winuser.h)
    /// These codes are hardware and layout independent
    /// </summary>
    public enum VirtualKey : ushort
    {
        // ============================================
        // MOUSE BUTTONS
        // ============================================
        LeftButton = 0x01,
        RightButton = 0x02,
        Cancel = 0x03,
        MiddleButton = 0x04,
        XButton1 = 0x05,
        XButton2 = 0x06,

        // ============================================
        // CONTROL KEYS
        // ============================================
        Back = 0x08,           // BACKSPACE
        Tab = 0x09,            // TAB
        Clear = 0x0C,          // CLEAR (Numpad 5 with NumLock off)
        Enter = 0x0D,          // ENTER / RETURN
        Shift = 0x10,          // SHIFT (either)
        Control = 0x11,        // CTRL (either)
        Alt = 0x12,            // ALT (either)
        Pause = 0x13,          // PAUSE
        CapsLock = 0x14,       // CAPS LOCK
        Escape = 0x1B,         // ESC
        Space = 0x20,          // SPACEBAR
        PageUp = 0x21,         // PAGE UP
        PageDown = 0x22,       // PAGE DOWN
        End = 0x23,            // END
        Home = 0x24,           // HOME
        Left = 0x25,           // LEFT ARROW
        Up = 0x26,             // UP ARROW
        Right = 0x27,          // RIGHT ARROW
        Down = 0x28,           // DOWN ARROW
        Select = 0x29,         // SELECT
        Print = 0x2A,          // PRINT
        Execute = 0x2B,        // EXECUTE
        PrintScreen = 0x2C,    // PRINT SCREEN
        Insert = 0x2D,         // INSERT
        Delete = 0x2E,         // DELETE
        Help = 0x2F,           // HELP

        // ============================================
        // NUMBER ROW (0-9)
        // ============================================
        Number0 = 0x30,        // 0
        Number1 = 0x31,        // 1
        Number2 = 0x32,        // 2
        Number3 = 0x33,        // 3
        Number4 = 0x34,        // 4
        Number5 = 0x35,        // 5
        Number6 = 0x36,        // 6
        Number7 = 0x37,        // 7
        Number8 = 0x38,        // 8
        Number9 = 0x39,        // 9

        // ============================================
        // LETTERS (A-Z)
        // ============================================
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,

        // ============================================
        // WINDOWS KEYS
        // ============================================
        LeftWindows = 0x5B,    // Left Windows key
        RightWindows = 0x5C,   // Right Windows key
        Apps = 0x5D,           // Application key (context menu)
        Sleep = 0x5F,          // Computer Sleep key

        // ============================================
        // NUMPAD
        // ============================================
        Numpad0 = 0x60,        // Numpad 0
        Numpad1 = 0x61,        // Numpad 1
        Numpad2 = 0x62,        // Numpad 2
        Numpad3 = 0x63,        // Numpad 3
        Numpad4 = 0x64,        // Numpad 4
        Numpad5 = 0x65,        // Numpad 5
        Numpad6 = 0x66,        // Numpad 6
        Numpad7 = 0x67,        // Numpad 7
        Numpad8 = 0x68,        // Numpad 8
        Numpad9 = 0x69,        // Numpad 9
        Multiply = 0x6A,       // Numpad *
        Add = 0x6B,            // Numpad +
        Separator = 0x6C,      // Numpad separator (locale-specific)
        Subtract = 0x6D,       // Numpad -
        Decimal = 0x6E,        // Numpad .
        Divide = 0x6F,         // Numpad /

        // ============================================
        // FUNCTION KEYS (F1-F24)
        // ============================================
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,

        // ============================================
        // LOCK KEYS
        // ============================================
        NumLock = 0x90,        // NUM LOCK
        ScrollLock = 0x91,     // SCROLL LOCK

        // ============================================
        // LEFT/RIGHT MODIFIER KEYS (Specific sides)
        // ============================================
        LeftShift = 0xA0,      // Left SHIFT
        RightShift = 0xA1,     // Right SHIFT
        LeftControl = 0xA2,    // Left CTRL
        RightControl = 0xA3,   // Right CTRL
        LeftAlt = 0xA4,        // Left ALT
        RightAlt = 0xA5,       // Right ALT (AltGr on international keyboards)

        // ============================================
        // BROWSER KEYS
        // ============================================
        BrowserBack = 0xA6,
        BrowserForward = 0xA7,
        BrowserRefresh = 0xA8,
        BrowserStop = 0xA9,
        BrowserSearch = 0xAA,
        BrowserFavorites = 0xAB,
        BrowserHome = 0xAC,

        // ============================================
        // VOLUME KEYS
        // ============================================
        VolumeMute = 0xAD,
        VolumeDown = 0xAE,
        VolumeUp = 0xAF,

        // ============================================
        // MEDIA KEYS
        // ============================================
        MediaNextTrack = 0xB0,
        MediaPreviousTrack = 0xB1,
        MediaStop = 0xB2,
        MediaPlayPause = 0xB3,

        // ============================================
        // LAUNCH KEYS
        // ============================================
        LaunchMail = 0xB4,
        LaunchMediaSelect = 0xB5,
        LaunchApp1 = 0xB6,
        LaunchApp2 = 0xB7,

        // ============================================
        // OEM KEYS (US QWERTY)
        // These are layout-specific and may differ on international keyboards
        // ============================================
        OEM1 = 0xBA,           // ;: (semicolon/colon) on US keyboard
        OEMPlus = 0xBB,        // =+ (equals/plus) on US keyboard
        OEMComma = 0xBC,       // ,< (comma/less than) on US keyboard
        OEMMinus = 0xBD,       // -_ (minus/underscore) on US keyboard
        OEMPeriod = 0xBE,      // .> (period/greater than) on US keyboard
        OEM2 = 0xBF,           // /? (forward slash/question mark) on US keyboard
        OEM3 = 0xC0,           // `~ (grave accent/tilde) on US keyboard
        OEM4 = 0xDB,           // [{ (left bracket/left brace) on US keyboard
        OEM5 = 0xDC,           // \| (backslash/pipe) on US keyboard
        OEM6 = 0xDD,           // ]} (right bracket/right brace) on US keyboard
        OEM7 = 0xDE,           // '" (single quote/double quote) on US keyboard
        OEM8 = 0xDF,           // Varies by keyboard

        // ============================================
        // ADDITIONAL OEM KEYS
        // ============================================
        OEM102 = 0xE2,         // <> or \| on RT 102-key keyboard (non-US)

        // ============================================
        // IME KEYS (for Asian language input)
        // ============================================
        ProcessKey = 0xE5,     // IME PROCESS key

        // ============================================
        // SPECIAL KEYS
        // ============================================
        Packet = 0xE7,         // Used to pass Unicode characters as keystrokes

        // ============================================
        // ATTN KEYS (mainframe-era keys, rarely used)
        // ============================================
        Attn = 0xF6,
        CrSel = 0xF7,
        ExSel = 0xF8,
        EraseEOF = 0xF9,
        Play = 0xFA,
        Zoom = 0xFB,
        NoName = 0xFC,
        PA1 = 0xFD,
        OEMClear = 0xFE,
    }

    
}