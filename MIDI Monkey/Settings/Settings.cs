using MIDI_Monkey.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
            public VirtualKey StartPlaybackKey { get; set; } = VirtualKey.F5;
            public VirtualKey StopPlaybackKey { get; set; } = VirtualKey.F6;
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

        /// <summary>
        /// Save settings asynchronously (non-blocking)
        /// </summary>
        public static async Task SaveSettingsAsync()
        {
            if (!_isInitialized)
                Init();

            try
            {
                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);

                // Use async file writing with UTF8 encoding and buffering
                using (var fileStream = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    await writer.WriteAsync(json);
                    await writer.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                Utilities.Logging.DebugLog($"Error saving settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Save settings synchronously (blocking) - use sparingly
        /// </summary>
        public static void SaveSettingsSync()
        {
            if (!_isInitialized)
                Init();

            try
            {
                lock (_settingsLock)
                {
                    string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                    File.WriteAllText(settingsFile, json, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                Utilities.Logging.DebugLog($"Error saving settings (sync): {ex.Message}");
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
                        Path.GetDirectoryName(settingsFile),
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