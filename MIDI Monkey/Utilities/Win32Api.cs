using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using MIDI_Monkey.Models;

namespace MIDI_Monkey.Utilities
{
    public static class Win32Api
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, [MarshalAs(UnmanagedType.Bool)] bool fAttach);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        private static extern ushort MapVirtualKey(uint uCode, uint uMapType);

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
            [FieldOffset(0)] public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        public const int INPUT_KEYBOARD = 1;
        public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const uint KEYEVENTF_KEYUP = 0x0002;
        public const uint KEYEVENTF_SCANCODE = 0x0008;
        public const uint MAPVK_VK_TO_VSC = 0;
        public const uint MAPVK_VK_TO_VSC_EX = 4;
        private const int SW_RESTORE = 9;

        private static readonly int InputSize = Marshal.SizeOf(typeof(INPUT));

        // MapVirtualKey is a syscall; cache scan codes so each key press costs only SendInput.
        private static readonly ushort[] _scanCodeCache = new ushort[256];
        private static readonly bool[] _scanCodeCached = new bool[256];

        private static ushort GetScanCode(VirtualKey key)
        {
            int index = (ushort)key & 0xFF;
            if (!_scanCodeCached[index])
            {
                _scanCodeCache[index] = MapVirtualKey((uint)key, MAPVK_VK_TO_VSC);
                _scanCodeCached[index] = true;
            }
            return _scanCodeCache[index];
        }

        private static INPUT CreateKeyInput(VirtualKey key, bool keyDown)
        {
            return new INPUT
            {
                type = INPUT_KEYBOARD,
                u = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = (ushort)key,
                        wScan = GetScanCode(key),
                        dwFlags = KEYEVENTF_SCANCODE | (keyDown ? 0 : KEYEVENTF_KEYUP),
                        time = 0,
                        dwExtraInfo = IntPtr.Zero,
                    }
                }
            };
        }

        /// <summary>
        /// Send a single key event to whatever window currently has focus.
        /// Does NOT change focus. Use <see cref="IsWindowFocused"/> to check the target first.
        /// </summary>
        public static void SendKey(VirtualKey key, bool keyDown)
        {
            var inputs = new[] { CreateKeyInput(key, keyDown) };
            SendInput(1, inputs, InputSize);
        }

        /// <summary>
        /// Send several key events in one SendInput call, so they arrive together (e.g. a chord).
        /// </summary>
        public static void SendKeys(IReadOnlyList<(VirtualKey key, bool keyDown)> keys)
        {
            if (keys.Count == 0)
                return;

            var inputs = new INPUT[keys.Count];
            for (int i = 0; i < keys.Count; i++)
            {
                inputs[i] = CreateKeyInput(keys[i].key, keys[i].keyDown);
            }

            SendInput((uint)inputs.Length, inputs, InputSize);
        }

        /// <summary>
        /// True if the handle still refers to an existing window.
        /// </summary>
        public static bool IsWindowValid(IntPtr hWnd)
        {
            return hWnd != IntPtr.Zero && IsWindow(hWnd);
        }

        /// <summary>
        /// Returns the process id that owns the window, or 0 if unknown.
        /// </summary>
        public static uint GetWindowProcessId(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return 0;

            GetWindowThreadProcessId(hWnd, out uint processId);
            return processId;
        }

        /// <summary>
        /// True when the foreground window belongs to the given process.
        /// Comparing process ids (not just handles) also covers games that use several windows.
        /// </summary>
        public static bool IsProcessFocused(uint processId)
        {
            if (processId == 0)
                return false;

            return GetWindowProcessId(GetForegroundWindow()) == processId;
        }

        /// <summary>
        /// True when the given window (or another window of the same process) has focus.
        /// </summary>
        public static bool IsWindowFocused(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return false;

            IntPtr foreground = GetForegroundWindow();
            return foreground == hWnd || IsProcessFocused(GetWindowProcessId(hWnd));
        }

        /// <summary>
        /// Bring the window to the foreground once. Returns true if it ends up focused.
        /// </summary>
        public static bool FocusWindow(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return false;

            if (IsWindowFocused(hWnd))
                return true;

            if (IsIconic(hWnd))
            {
                ShowWindow(hWnd, SW_RESTORE);
            }

            if (SetForegroundWindow(hWnd) && IsWindowFocused(hWnd))
                return true;

            // Windows may refuse SetForegroundWindow from a background process.
            // Attaching to the current foreground thread's input lets the request through.
            uint foregroundThread = GetWindowThreadProcessId(GetForegroundWindow(), out _);
            uint currentThread = GetCurrentThreadId();
            bool attached = foregroundThread != 0 && foregroundThread != currentThread &&
                            AttachThreadInput(currentThread, foregroundThread, true);
            try
            {
                BringWindowToTop(hWnd);
                SetForegroundWindow(hWnd);
            }
            finally
            {
                if (attached)
                {
                    AttachThreadInput(currentThread, foregroundThread, false);
                }
            }

            return IsWindowFocused(hWnd);
        }
    }
}
