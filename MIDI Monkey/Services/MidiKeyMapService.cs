using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MIDI_Monkey.Models;
using MIDI_Monkey.Utilities;

namespace MIDI_Monkey.Services
{
    public class MidiKeyMapService
    {
        private readonly string _midiKeyMapsDirectory;
        private Dictionary<int, List<VirtualKey>> _currentMapping = new Dictionary<int, List<VirtualKey>>();
        private string _currentApplicationName = string.Empty;

        public Dictionary<int, List<VirtualKey>> CurrentMapping => _currentMapping;
        public string CurrentApplicationName => _currentApplicationName;

        public MidiKeyMapService()
        {
            _midiKeyMapsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MIDIKeyMaps");
            EnsureDirectoryExists();
        }

        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(_midiKeyMapsDirectory))
            {
                Directory.CreateDirectory(_midiKeyMapsDirectory);
            }
        }

        public async Task<List<string>> GetAvailableKeyMapsAsync()
        {
            EnsureDirectoryExists();
            var files = Directory.GetFiles(_midiKeyMapsDirectory, "*.json");
            return files.Select(Path.GetFileNameWithoutExtension).ToList();
        }

        public async Task LoadKeyMapAsync(string keyMapName)
        {
            string filePath = Path.Combine(_midiKeyMapsDirectory, $"{keyMapName}.json");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"MidiKeyMap file not found: {filePath}");
            }

            try
            {
                string fileContent = await File.ReadAllTextAsync(filePath);
                var contentLines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(line => !line.TrimStart().StartsWith("//") && !line.TrimStart().StartsWith("#"))
                    .ToArray();

                string jsonContent = string.Join(Environment.NewLine, contentLines);
                var config = JsonConvert.DeserializeObject<MidiKeyMapConfig>(jsonContent);

                if (config == null || config.KeyMappings == null)
                {
                    throw new Exception("Failed to parse MidiKeyMap JSON or invalid format");
                }

                _currentApplicationName = config.ApplicationName ?? string.Empty;
                _currentMapping.Clear();

                foreach (var entry in config.KeyMappings)
                {
                    if (int.TryParse(entry.Key, out int midiKey))
                    {
                        var keyCodes = new List<VirtualKey>();
                        foreach (var keyName in entry.Value)
                        {
                            if (Enum.TryParse(keyName, out VirtualKey keyCode))
                            {
                                keyCodes.Add(keyCode);
                            }
                            else if (!string.IsNullOrWhiteSpace(keyName))
                            {
                                Logging.DebugLog($"Warning: {keyName} is not a valid VirtualKey.");
                            }
                        }

                        _currentMapping[midiKey] = keyCodes;
                    }
                }

                Logging.DebugLog($"Loaded MidiKeyMap: {keyMapName} for application: {_currentApplicationName}");
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error loading MidiKeyMap: {ex.Message}");
                throw;
            }
        }

        public async Task SaveKeyMapAsync(string keyMapName, MidiKeyMapConfig config)
        {
            string filePath = Path.Combine(_midiKeyMapsDirectory, $"{keyMapName}.json");

            var header = "# MIDI Monkey MidiKeyMap Config.\n" +
                         "# To create your MIDI Key Mapping, you need to assign each MIDI key to a corresponding keyboard key.\n" +
                         "# Each mapping can include a modifier key (such as Shift or Control) followed by the alphanumeric keyboard key.\n" +
                         "# If you do not want to use a modifier key, simply omit it and use only the alphanumeric key.\n" +
                         "# The configuration follows this structure:\n" +
                         "#   1. MIDI Key: The MIDI note you want to map (0 to 127)\n" +
                         "#   2. Modifier Key (Optional): The modifier key, if any (e.g., LeftShift, LeftControl)\n" +
                         "#   3. Alphanumeric Keyboard Key: The key on the keyboard (e.g., Q, W, Number1, Number2)\n" +
                         "#   4. ApplicationName: The process name (without .exe) that this mapping is for\n\n";

            var contents = JsonConvert.SerializeObject(config, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, header + contents);

            Logging.DebugLog($"Saved MidiKeyMap: {keyMapName}");
        }

        public async Task CreateBlankKeyMapAsync(string applicationName, string keyMapName)
        {
            var config = new MidiKeyMapConfig
            {
                ApplicationName = applicationName,
                KeyMappings = new Dictionary<string, List<string>>()
            };

            for (int i = 0; i <= 127; i++)
            {
                config.KeyMappings[i.ToString()] = new List<string>();
            }

            await SaveKeyMapAsync(keyMapName, config);
        }

        public bool ContainsKey(int midiKey) => _currentMapping.ContainsKey(midiKey);

        public bool TryGetValue(int midiKey, out List<VirtualKey> keys)
        {
            return _currentMapping.TryGetValue(midiKey, out keys);
        }
    }
}
