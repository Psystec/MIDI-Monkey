using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

namespace MIDI_Monkey
{
    internal static class Logging
    {
        private static readonly ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
        private static CancellationTokenSource _cts = new CancellationTokenSource();
        private static Task _logProcessorTask;
        private static readonly string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        private static RichTextBox richTextBoxLog;
        private static bool _isInitialized = false;

        public static void Initialize()
        {
            if (_isInitialized)
                return;

            if (!Directory.Exists(logDirectory))
            {
                try
                {
                    Directory.CreateDirectory(logDirectory);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to create log directory: {ex.Message}");
                }
            }

            _logProcessorTask = ProcessLogQueueAsync(_cts.Token);
            _isInitialized = true;

            DebugLog("Logging system initialized");
        }

        public static void SetRichTextBox(RichTextBox textBox)
        {
            richTextBoxLog = textBox;

            if (!_isInitialized)
                Initialize();
        }

        public static void DebugLog(string message)
        {
            if (!_isInitialized)
                Initialize();

            string formattedMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            _logQueue.Enqueue(formattedMessage);
        }

        private static async Task ProcessLogQueueAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (_logQueue.TryDequeue(out string logMessage))
                    {
                        await ProcessLogMessageAsync(logMessage);
                    }
                    else
                    {
                        await Task.Delay(50, token);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in log processor: {ex.Message}");
                    await Task.Delay(100, token);
                }
            }
        }

        private static async Task ProcessLogMessageAsync(string logMessage)
        {
            try
            {
                string logFilePath = Path.Combine(logDirectory, $"{DateTime.Now:yyyy-MM-dd}.log");

                using (StreamWriter writer = new StreamWriter(logFilePath, true, Encoding.UTF8))
                {
                    await writer.WriteLineAsync(logMessage);
                }

                UpdateRichTextBox(logMessage);

                Debug.WriteLine(logMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to process log message: {ex.Message}");
            }
        }

        private static void UpdateRichTextBox(string logMessage)
        {
            var textBox = richTextBoxLog;
            if (textBox == null)
                return;

            try
            {
                if (textBox.InvokeRequired)
                {
                    textBox.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            if (!textBox.IsDisposed && textBox.IsHandleCreated)
                            {
                                textBox.AppendText(logMessage + Environment.NewLine);

                                if (textBox.Lines.Length > 1000)
                                {
                                    string[] newLines = new string[900];
                                    Array.Copy(textBox.Lines, textBox.Lines.Length - 900, newLines, 0, 900);
                                    textBox.Lines = newLines;
                                }

                                textBox.ScrollToCaret();
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Failed to update RichTextBox: {ex.Message}");
                        }
                    }));
                }
                else if (!textBox.IsDisposed)
                {
                    textBox.AppendText(logMessage + Environment.NewLine);

                    if (textBox.Lines.Length > 1000)
                    {
                        string[] newLines = new string[900];
                        Array.Copy(textBox.Lines, textBox.Lines.Length - 900, newLines, 0, 900);
                        textBox.Lines = newLines;
                    }

                    textBox.ScrollToCaret();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to invoke RichTextBox: {ex.Message}");
            }
        }

        public static async Task ShutdownAsync()
        {
            if (!_isInitialized)
                return;

            try
            {
                _cts.Cancel();

                using (var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                {
                    try
                    {
                        while (_logQueue.TryDequeue(out string message))
                        {
                            await ProcessLogMessageAsync(message);
                        }

                        await Task.WhenAny(_logProcessorTask, Task.Delay(Timeout.Infinite, timeoutCts.Token));
                    }
                    catch (OperationCanceledException)
                    {
                        // Timeout occurred, that's fine
                    }
                }

                _isInitialized = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error shutting down logging: {ex.Message}");
            }
        }
    }
}