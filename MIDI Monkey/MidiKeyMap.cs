using Newtonsoft.Json;
using WindowsInput.Native;

namespace MIDI_Monkey
{
    public class MidiKeyMapConfig
    {
        public string ApplicationName { get; set; } = string.Empty;
        public Dictionary<string, List<string>> KeyMappings { get; set; } = new Dictionary<string, List<string>>();
    }

    public static class MidiKeyMap
    {
        private static Dictionary<int, List<VirtualKeyCode>> _midiToKeyMap = new Dictionary<int, List<VirtualKeyCode>>();
        private static string _currentApplicationName = string.Empty;
        public static string CurrentApplicationName => _currentApplicationName;

        public static async Task LoadFromJsonAsync(string selectedKeyMap)
        {
            string midiKeyMapsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MIDIKeyMaps");
            string filePath = Path.Combine(midiKeyMapsDirectory, $"{selectedKeyMap}.json");

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

                var configObject = JsonConvert.DeserializeObject<MidiKeyMapConfig>(jsonContent);

                if (configObject == null || configObject.KeyMappings == null)
                {
                    throw new Exception("Failed to parse MidiKeyMap JSON or invalid format");
                }

                _currentApplicationName = configObject.ApplicationName ?? string.Empty;

                _midiToKeyMap.Clear();
                foreach (var entry in configObject.KeyMappings)
                {
                    if (int.TryParse(entry.Key, out int midiKey))
                    {
                        var keyCodes = new List<VirtualKeyCode>();
                        foreach (var keyName in entry.Value)
                        {
                            if (Enum.TryParse(keyName, out VirtualKeyCode keyCode))
                            {
                                keyCodes.Add(keyCode);
                            }
                            else if (!string.IsNullOrWhiteSpace(keyName))
                            {
                                Logging.DebugLog($"Warning: {keyName} is not a valid VirtualKeyCode.");
                            }
                        }

                        _midiToKeyMap[midiKey] = keyCodes;
                    }
                }

                Settings.settings.LastKeyMapProfile = selectedKeyMap;
                await Settings.SaveSettingsAsync();

                Logging.DebugLog($"Loaded MidiKeyMap: {selectedKeyMap} for application: {_currentApplicationName}");
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"Error loading MidiKeyMap: {ex.Message}");
                throw;
            }
        }

        public static Dictionary<int, List<VirtualKeyCode>> MidiToKey => _midiToKeyMap;

        public static bool ContainsKey(int midiKey) => _midiToKeyMap.ContainsKey(midiKey);

        public static async Task CreateMidiKeyMapAsync(string applicationName, string outputPath)
        {
            var config = new MidiKeyMapConfig
            {
                ApplicationName = applicationName,
                KeyMappings = new Dictionary<string, List<string>>()
            };

            for (int i = 0; i <= 127; i++)
            {
                config.KeyMappings[i.ToString()] = new List<string> { "", "" };
            }

            var contents = JsonConvert.SerializeObject(config, Formatting.Indented);
            var header = "# MIDI Monkey MidiKeyMap Config.\n" +
                         "# To create your MIDI Key Mapping, you need to assign each MIDI key to a corresponding keyboard key.\n" +
                         "# Each mapping can include a modifier key (such as Shift or Control) followed by the alphanumeric keyboard key.\n" +
                         "# If you do not want to use a modifier key, simply omit it and use only the alphanumeric key.\n" +
                         "# The configuration follows this structure:\n" +
                         "#   1. MIDI Key: The MIDI note you want to map (0 to 127)\n" +
                         "#   2. Modifier Key (Optional): The modifier key, if any (e.g., LSHIFT, LCONTROL)\n" +
                         "#   3. Alphanumeric Keyboard Key: The key on the keyboard (e.g., VK_Q, VK_W)\n" +
                         "#   4. ApplicationName: The process name (without .exe) that this mapping is for\n\n";

            await File.WriteAllTextAsync(outputPath, header + contents);
            Logging.DebugLog($"Created new MidiKeyMap: {outputPath}");
        }

        public static async Task<List<string>> GetMidiKeyMapsAsync()
        {
            string midiKeyMapsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MIDIKeyMaps");

            if (!Directory.Exists(midiKeyMapsDirectory))
            {
                Directory.CreateDirectory(midiKeyMapsDirectory);
            }

            var midiKeyMapFiles = Directory.GetFiles(midiKeyMapsDirectory, "*.json");
            return midiKeyMapFiles.Select(Path.GetFileNameWithoutExtension).ToList();
        }
    }
}