using MIDI_Monkey.Models;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIDI_Monkey
{
    public class Settings
    {
        public static Data settings = new Data();
        private static readonly string settingsFile = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "settings.json"
        );
        private static readonly object _settingsLock = new object();
        private static bool _isInitialized = false;

        public class Data
        {
            public string LastMidiFile { get; set; } = "";
            public string LastMidiFolder { get; set; } = "";
            public string LastKeyMapProfile { get; set; } = "";
            public bool RepeatSong { get; set; } = false;
            public bool AlwaysOnTop { get; set; } = true;
            public int Speed { get; set; } = 0;
            public int ModifierDelay { get; set; } = 0;
            public Keys StartPlaybackKey { get; set; } = Keys.F5;
            public Keys StopPlaybackKey { get; set; } = Keys.F6;
            public Point WindowSize { get;set; } = new Point(849, 576);
            public int SplitterMainDistance { get; set; } = 440;
        }

        /// <summary>
        /// Initialize settings - loads from file or creates default if doesn't exist
        /// </summary>
        public static void Init()
        {
            if (_isInitialized)
                return;

            try
            {
                // Set this FIRST to prevent infinite recursion when SaveSettingsSync is called
                _isInitialized = true;

                if (!File.Exists(settingsFile))
                {
                    // Create default settings file
                    settings = new Data();
                    SaveSettingsSync(); // Safe now - _isInitialized is already true
                }
                else
                {
                    LoadSettings();
                }

                Utilities.Logging.DebugLog("Settings initialized successfully");
            }
            catch (Exception ex)
            {
                Utilities.Logging.DebugLog($"Error initializing settings: {ex.Message}");
                // Start with default settings if initialization fails
                settings = new Data();
                _isInitialized = true; // Ensure this is set even on error
            }
        }

        // Only one save writes the file at a time. Settings can change many times in a row
        // (dragging a slider, moving the splitter), and overlapping saves used to fight over
        // settings.json ("being used by another process").
        private static readonly SemaphoreSlim _saveGate = new SemaphoreSlim(1, 1);
        private static int _saveRequested = 0;

        /// <summary>
        /// Save settings asynchronously (non-blocking).
        /// If a save is already running, this request is merged into it: the running save
        /// writes again afterwards with the latest values, so the file always ends up current.
        /// </summary>
        public static async Task SaveSettingsAsync()
        {
            if (!_isInitialized)
                Init();

            Interlocked.Exchange(ref _saveRequested, 1);

            while (true)
            {
                if (!await _saveGate.WaitAsync(0).ConfigureAwait(false))
                {
                    return; // The running save will pick up our request.
                }

                try
                {
                    while (Interlocked.Exchange(ref _saveRequested, 0) == 1)
                    {
                        string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                        await WriteSettingsFileAsync(json).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    Utilities.Logging.DebugLog($"Error saving settings: {ex.Message}");
                }
                finally
                {
                    _saveGate.Release();
                }

                // A request may have arrived between finishing the loop and releasing the gate.
                if (Volatile.Read(ref _saveRequested) == 0)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Writes to a temporary file and then replaces settings.json, so a crash or a
        /// second reader never sees a half written file. Retries briefly if another program
        /// (antivirus, cloud sync) is holding the file.
        /// </summary>
        private static async Task WriteSettingsFileAsync(string json)
        {
            string tempFile = settingsFile + ".tmp";
            const int maxAttempts = 5;

            for (int attempt = 1; ; attempt++)
            {
                try
                {
                    await File.WriteAllTextAsync(tempFile, json, Encoding.UTF8).ConfigureAwait(false);
                    File.Move(tempFile, settingsFile, overwrite: true);
                    return;
                }
                catch (IOException) when (attempt < maxAttempts)
                {
                    await Task.Delay(50 * attempt).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Save settings synchronously (blocking) - use sparingly
        /// </summary>
        public static void SaveSettingsSync()
        {
            if (!_isInitialized)
                Init();

            // The async save never needs the UI thread while holding the gate, so waiting here is safe.
            _saveGate.Wait();
            try
            {
                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                WriteSettingsFileAsync(json).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Utilities.Logging.DebugLog($"Error saving settings (sync): {ex.Message}");
            }
            finally
            {
                _saveGate.Release();
            }
        }

        /// <summary>
        /// Load settings from file
        /// </summary>
        private static void LoadSettings()
        {
            try
            {
                lock (_settingsLock)
                {
                    if (!File.Exists(settingsFile))
                    {
                        settings = new Data();
                        return;
                    }

                    string json = File.ReadAllText(settingsFile, Encoding.UTF8);

                    var loadedSettings = JsonConvert.DeserializeObject<Data>(json);

                    if (loadedSettings != null)
                    {
                        settings = loadedSettings;

                        // Validate file paths
                        if (!string.IsNullOrEmpty(settings.LastMidiFile) && !File.Exists(settings.LastMidiFile))
                        {
                            settings.LastMidiFile = "";
                        }

                        if (!string.IsNullOrEmpty(settings.LastMidiFolder) && !Directory.Exists(settings.LastMidiFolder))
                        {
                            settings.LastMidiFolder = "";
                        }

                        if (!string.IsNullOrEmpty(settings.LastKeyMapProfile))
                        {
                            var keyMapFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MIDIKeyMaps", $"{settings.LastKeyMapProfile}.json");
                            if (!File.Exists(keyMapFilePath))
                            {
                                settings.LastKeyMapProfile = "";
                            }
                        }

                        // Utilities.Logging.DebugLog("Settings loaded successfully");
                    }
                    else
                    {
                        Utilities.Logging.DebugLog("Settings file was null, using defaults");
                        settings = new Data();
                    }
                }
            }
            catch (JsonException jsonEx)
            {
                Utilities.Logging.DebugLog($"Invalid JSON in settings file: {jsonEx.Message}. Creating backup and using defaults.");
                BackupCorruptedSettings();
                settings = new Data();
            }
            catch (Exception ex)
            {
                Utilities.Logging.DebugLog($"Error loading settings: {ex.Message}. Using defaults.");
                settings = new Data();
            }
        }

        /// <summary>
        /// Reload settings from file (useful if settings changed externally)
        /// </summary>
        public static void Reload()
        {
            LoadSettings();
        }

        /// <summary>
        /// Reset settings to defaults
        /// </summary>
        public static async Task ResetToDefaultsAsync()
        {
            settings = new Data();
            await SaveSettingsAsync();
            Utilities.Logging.DebugLog("Settings reset to defaults");
        }

        /// <summary>
        /// Backup a corrupted settings file
        /// </summary>
        private static void BackupCorruptedSettings()
        {
            try
            {
                if (File.Exists(settingsFile))
                {
                    string backupFile = Path.Combine(
                        Path.GetDirectoryName(settingsFile) ?? AppDomain.CurrentDomain.BaseDirectory,
                        $"settings_corrupted_{DateTime.Now:yyyyMMdd_HHmmss}.json"
                    );
                    File.Copy(settingsFile, backupFile, true);
                    File.Delete(settingsFile);
                    Utilities.Logging.DebugLog($"Corrupted settings backed up to: {backupFile}");
                }
            }
            catch (Exception ex)
            {
                Utilities.Logging.DebugLog($"Could not backup corrupted settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Check if settings file exists
        /// </summary>
        public static bool SettingsFileExists() => File.Exists(settingsFile);

        /// <summary>
        /// Get the full path to the settings file
        /// </summary>
        public static string GetSettingsFilePath() => settingsFile;
    }
}