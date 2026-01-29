using System;
using System.Diagnostics;
using System.Linq;
using MIDI_Monkey.Utilities;

namespace MIDI_Monkey.Services
{
    public class ProcessWindowService
    {
        public IntPtr GetWindowHandle(string processName, bool silent = false)
        {
            try
            {
                if (!silent)
                {
                    Logging.DebugLog($"Looking for process: {processName}");
                }

                if (string.IsNullOrEmpty(processName))
                {
                    if (!silent)
                    {
                        Logging.DebugLog("Process name is empty");
                    }
                    return IntPtr.Zero;
                }

                var process = Process.GetProcessesByName(processName).FirstOrDefault();
                if (process == null)
                {
                    if (!silent)
                    {
                        Logging.DebugLog($"Process '{processName}' not found");
                    }
                    return IntPtr.Zero;
                }

                IntPtr handle = process.MainWindowHandle;
                
                if (!silent && handle != IntPtr.Zero)
                {
                    Logging.DebugLog($"Found {processName} window handle: {handle}");
                }

                return handle;
            }
            catch (Exception ex)
            {
                if (!silent)
                {
                    Logging.DebugLog($"Error finding window handle: {ex.Message}");
                }
                return IntPtr.Zero;
            }
        }

        public bool IsProcessRunning(string processName)
        {
            try
            {
                return Process.GetProcessesByName(processName).Any();
            }
            catch
            {
                return false;
            }
        }
    }
}
