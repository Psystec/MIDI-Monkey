using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIDI_Monkey.Utilities
{
    internal static class Logging
    {
        private static readonly ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
        private static CancellationTokenSource _cts = new CancellationTokenSource();
        private static Task _logProcessorTask;
        private static readonly string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static RichTextBox _richTextBoxLog;
        private static bool _isInitialized = false;

        // Configuration
        private const int MAX_QUEUE_SIZE = 10000;
        private const int BATCH_SIZE = 50;
        private const int PROCESSING_DELAY_MS = 50;
        private const int MAX_TEXTBOX_LINES = 1000;
        private const int TEXTBOX_TRIM_TO = 800;

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
            _richTextBoxLog = textBox;

            if (!_isInitialized)
                Initialize();
        }

        public static void DebugLog(string message)
        {
            if (!_isInitialized)
                Initialize();

            if (_logQueue.Count >= MAX_QUEUE_SIZE)
            {
                Debug.WriteLine($"Log queue full ({MAX_QUEUE_SIZE}), dropping message: {message}");
                return;
            }

            string formattedMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            _logQueue.Enqueue(formattedMessage);
        }

        private static async Task ProcessLogQueueAsync(CancellationToken token)
        {
            var batch = new List<string>(BATCH_SIZE);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    batch.Clear();

                    while (batch.Count < BATCH_SIZE && _logQueue.TryDequeue(out string logMessage))
                    {
                        batch.Add(logMessage);
                    }

                    if (batch.Count > 0)
                    {
                        await ProcessLogBatchAsync(batch);
                    }
                    else
                    {
                        await Task.Delay(PROCESSING_DELAY_MS, token);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in log processor: {ex.Message}");
                    await Task.Delay(1000, token);
                }
            }
        }

        private static async Task ProcessLogBatchAsync(List<string> batch)
        {
            if (batch.Count == 0)
                return;

            await WriteToFileAsync(batch);

            var textBox = _richTextBoxLog;
            if (textBox != null)
            {
                UpdateRichTextBox(textBox, batch);
            }

            foreach (var message in batch)
            {
                Debug.WriteLine(message);
            }
        }

        private static async Task WriteToFileAsync(List<string> messages)
        {
            try
            {
                string logFilePath = Path.Combine(logDirectory, $"{DateTime.Now:yyyy-MM-dd}.log");

                using (var fileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read, 4096, true))
                using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    foreach (var message in messages)
                    {
                        await writer.WriteLineAsync(message);
                    }
                    await writer.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }

        private static void UpdateRichTextBox(RichTextBox textBox, List<string> messages)
        {
            if (textBox == null || textBox.IsDisposed)
                return;

            var combinedText = string.Join(Environment.NewLine, messages) + Environment.NewLine;

            try
            {
                if (textBox.InvokeRequired)
                {
                    textBox.BeginInvoke(new Action(() => AppendToTextBox(textBox, combinedText)));
                }
                else
                {
                    AppendToTextBox(textBox, combinedText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to update RichTextBox: {ex.Message}");
            }
        }

        private static void AppendToTextBox(RichTextBox textBox, string text)
        {
            try
            {
                if (textBox.IsDisposed || !textBox.IsHandleCreated)
                    return;

                textBox.SuspendLayout();

                textBox.AppendText(text);

                if (textBox.Lines.Length > MAX_TEXTBOX_LINES)
                {
                    int linesToKeep = TEXTBOX_TRIM_TO;
                    string[] newLines = new string[linesToKeep];
                    Array.Copy(textBox.Lines, textBox.Lines.Length - linesToKeep, newLines, 0, linesToKeep);
                    textBox.Lines = newLines;
                }

                textBox.ScrollToCaret();
                textBox.ResumeLayout();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to append to RichTextBox: {ex.Message}");
                textBox.ResumeLayout();
            }
        }

        public static async Task ShutdownAsync()
        {
            if (!_isInitialized)
                return;

            try
            {
                DebugLog("Logging system shutting down...");

                _cts.Cancel();

                using (var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    try
                    {
                        var remainingMessages = new List<string>();

                        while (_logQueue.TryDequeue(out string message))
                        {
                            remainingMessages.Add(message);

                            if (remainingMessages.Count >= BATCH_SIZE)
                            {
                                await ProcessLogBatchAsync(remainingMessages);
                                remainingMessages.Clear();
                            }
                        }

                        if (remainingMessages.Count > 0)
                        {
                            await ProcessLogBatchAsync(remainingMessages);
                        }

                        await Task.WhenAny(_logProcessorTask, Task.Delay(Timeout.Infinite, timeoutCts.Token));
                    }
                    catch (OperationCanceledException)
                    {
                        Debug.WriteLine("Log shutdown timeout reached");
                    }
                }

                _isInitialized = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error shutting down logging: {ex.Message}");
            }
        }

        public static int GetQueueDepth() => _logQueue.Count;

        public static bool IsHealthy() => _isInitialized && _logQueue.Count < MAX_QUEUE_SIZE;

        /// <summary>
        /// Shows a non-blocking MessageBox that doesn't prevent interaction with the main form
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="title">The title of the message box</param>
        /// <param name="icon">The icon to display (default: Information)</param>
        /// <param name="buttons">The buttons to display (default: OK)</param>
        public static void ShowNonBlockingMessage(
            string message,
            string title = "Information",
            MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            DebugLog($"MessageBox shown - Title: {title}, Message: {message}");

            Task.Run(() =>
            {
                try
                {
                    MessageBox.Show(message, title, buttons, icon);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error showing non-blocking message: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// Shows a non-blocking MessageBox with a callback when the user responds
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="title">The title of the message box</param>
        /// <param name="onResult">Callback with the user's response (OK, Cancel, Yes, No, etc.)</param>
        /// <param name="icon">The icon to display (default: Information)</param>
        /// <param name="buttons">The buttons to display (default: OK)</param>
        public static void ShowNonBlockingMessage(
            string message,
            string title,
            Action<DialogResult> onResult,
            MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            DebugLog($"MessageBox shown - Title: {title}, Message: {message}");

            Task.Run(() =>
            {
                try
                {
                    DialogResult result = MessageBox.Show(message, title, buttons, icon);
                    onResult?.Invoke(result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error showing non-blocking message: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// Shows a blocking MessageBox that doesn't prevent interaction with the main form
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="title">The title of the message box</param>
        /// <param name="icon">The icon to display (default: Information)</param>
        /// <param name="buttons">The buttons to display (default: OK)</param>
        public static void ShowBlockingMessage(
            string message,
            string title = "Information",
            MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            DebugLog($"MessageBox shown - Title: {title}, Message: {message}");

            try
            {
                MessageBox.Show(message, title, buttons, icon);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error showing non-blocking message: {ex.Message}");
            }
        }

        /// <summary>
        /// Shows a blocking MessageBox with a callback when the user responds
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="title">The title of the message box</param>
        /// <param name="onResult">Callback with the user's response (OK, Cancel, Yes, No, etc.)</param>
        /// <param name="icon">The icon to display (default: Information)</param>
        /// <param name="buttons">The buttons to display (default: OK)</param>
        public static void ShowBlockingMessage(
            string message,
            string title,
            Action<DialogResult> onResult,
            MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            DebugLog($"MessageBox shown - Title: {title}, Message: {message}");

            try
            {
                DialogResult result = MessageBox.Show(message, title, buttons, icon);
                onResult?.Invoke(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error showing non-blocking message: {ex.Message}");
            }
        }
    }
}