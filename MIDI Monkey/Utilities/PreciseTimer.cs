using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace MIDI_Monkey.Utilities
{
    /// <summary>
    /// Waits until an exact point on a stopwatch timeline with ~1ms accuracy.
    /// Task.Delay / Thread.Sleep on Windows round up to the 15.6ms system tick, which made
    /// every note late and the whole song slower. This uses a high resolution waitable timer
    /// (Windows 10 1803+) for the coarse part of the wait and spins for the last millisecond.
    /// Create and use it on a single thread (the playback thread).
    /// </summary>
    public sealed class PreciseTimer : IDisposable
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateWaitableTimerExW(IntPtr lpTimerAttributes, string? lpTimerName, uint dwFlags, uint dwDesiredAccess);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWaitableTimer(IntPtr hTimer, ref long pDueTime, int lPeriod, IntPtr pfnCompletionRoutine, IntPtr lpArgToCompletionRoutine, [MarshalAs(UnmanagedType.Bool)] bool fResume);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("winmm.dll")]
        private static extern uint timeBeginPeriod(uint uPeriod);

        [DllImport("winmm.dll")]
        private static extern uint timeEndPeriod(uint uPeriod);

        private const uint CREATE_WAITABLE_TIMER_HIGH_RESOLUTION = 0x00000002;
        private const uint TIMER_ALL_ACCESS = 0x1F0003;
        private const uint INFINITE = 0xFFFFFFFF;

        // Longest single sleep, so cancellation (Stop) is noticed quickly.
        private const double MaxSleepChunkMs = 20.0;
        // Spin instead of sleeping for the last part of the wait.
        private const double SpinThresholdMs = 1.0;

        private IntPtr _timer;
        private readonly bool _usingTimeBeginPeriod;
        private bool _disposed;

        public PreciseTimer()
        {
            _timer = CreateWaitableTimerExW(IntPtr.Zero, null, CREATE_WAITABLE_TIMER_HIGH_RESOLUTION, TIMER_ALL_ACCESS);

            if (_timer == IntPtr.Zero)
            {
                // Older Windows: fall back to raising the system timer resolution for Thread.Sleep.
                timeBeginPeriod(1);
                _usingTimeBeginPeriod = true;
            }
        }

        /// <summary>
        /// Block until <paramref name="stopwatch"/> reaches <paramref name="targetMs"/>.
        /// Throws <see cref="OperationCanceledException"/> if the token is cancelled.
        /// </summary>
        public void WaitUntil(Stopwatch stopwatch, double targetMs, CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                double remaining = targetMs - stopwatch.Elapsed.TotalMilliseconds;
                if (remaining <= 0)
                    return;

                if (remaining > SpinThresholdMs)
                {
                    Sleep(Math.Min(remaining - SpinThresholdMs, MaxSleepChunkMs));
                }
                else
                {
                    Thread.SpinWait(50);
                }
            }
        }

        private void Sleep(double milliseconds)
        {
            if (_timer != IntPtr.Zero)
            {
                // Negative due time = relative, in 100ns units.
                long dueTime = -(long)(milliseconds * 10_000);
                if (SetWaitableTimer(_timer, ref dueTime, 0, IntPtr.Zero, IntPtr.Zero, false))
                {
                    WaitForSingleObject(_timer, INFINITE);
                    return;
                }
            }

            Thread.Sleep(Math.Max(1, (int)milliseconds));
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            if (_timer != IntPtr.Zero)
            {
                CloseHandle(_timer);
                _timer = IntPtr.Zero;
            }

            if (_usingTimeBeginPeriod)
            {
                timeEndPeriod(1);
            }
        }
    }
}
