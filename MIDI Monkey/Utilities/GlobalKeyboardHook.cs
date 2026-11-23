using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MIDI_Monkey.Utilities
{
    public class KeyPressedEventArgs : EventArgs
    {
        public Keys Key { get; private set; }

        public KeyPressedEventArgs(Keys key)
        {
            Key = key;
        }
    }

    /// <summary>
    /// Low level keyboard hook for the Play/Stop hotkeys.
    ///
    /// The hook runs on its own thread with its own message loop. Windows silently removes a
    /// low level hook whose callback is slow, which happened when it shared the busy UI thread
    /// (the Stop hotkey then stopped working). Keys injected by MIDI Monkey itself are ignored.
    ///
    /// <see cref="KeyboardPressed"/> is raised on the hook thread; handlers must marshal to the UI thread.
    /// </summary>
    public class GlobalKeyboardHook
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_QUIT = 0x0012;
        private const uint LLKHF_INJECTED = 0x10;

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public int ptX;
            public int ptY;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string? lpModuleName);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        private static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PostThreadMessage(uint idThread, uint msg, IntPtr wParam, IntPtr lParam);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private readonly LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        private Thread? _hookThread;
        private uint _hookThreadId;

        public event EventHandler<KeyPressedEventArgs>? KeyboardPressed;

        public GlobalKeyboardHook()
        {
            _proc = HookCallback;
        }

        public void HookKeyboard()
        {
            if (_hookThread != null)
                return;

            // Not disposed: the hook thread may still signal it after a timed-out wait.
            var ready = new ManualResetEventSlim(false);

            _hookThread = new Thread(() =>
            {
                _hookThreadId = GetCurrentThreadId();
                // For low level hooks hMod can be any module of this process.
                _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, GetModuleHandle(null), 0);
                ready.Set();

                if (_hookID == IntPtr.Zero)
                {
                    Logging.DebugLog($"Failed to install keyboard hook (error {Marshal.GetLastWin32Error()}). Hotkeys will not work.");
                    return;
                }

                // Message loop: required for the hook callback to be called on this thread.
                while (GetMessage(out MSG msg, IntPtr.Zero, 0, 0) > 0)
                {
                }

                UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            })
            {
                IsBackground = true,
                Name = "MIDI Monkey Hotkeys",
                Priority = ThreadPriority.AboveNormal
            };

            _hookThread.Start();
            ready.Wait(2000);
        }

        public void UnhookKeyboard()
        {
            if (_hookThread == null)
                return;

            if (_hookThreadId != 0)
            {
                PostThreadMessage(_hookThreadId, WM_QUIT, IntPtr.Zero, IntPtr.Zero);
            }

            _hookThread.Join(1000);
            _hookThread = null;
            _hookThreadId = 0;
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                var data = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);

                // Ignore keys sent by SendInput (our own note keys), only react to the real keyboard.
                if ((data.flags & LLKHF_INJECTED) == 0)
                {
                    try
                    {
                        KeyboardPressed?.Invoke(this, new KeyPressedEventArgs((Keys)data.vkCode));
                    }
                    catch (Exception ex)
                    {
                        Logging.DebugLog($"Hotkey handler error: {ex.Message}");
                    }
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
