using Newtonsoft.Json;

namespace MIDI_Monkey
{
    public class Settings
    {
        public static Data settings = new Data();

        private static string settingsFile = "settings.json";

        public class Data
        {
            public string? LastMidiFile = "";
            public string? LastMidiFolder = "";
            public string? LastKeyMapProfile = "";
            public bool RepeatSong = false;
            public bool AlwaysOnTop = true;
            public int Speed = 0;
            public int ModifierDelay = 20;
        }

        public static async Task SaveSettingsAsync()
        {
            await AppSettings.SaveSettingsAsync();
        }

        public static void Init()
        {
            AppSettings.Init();
        }

        internal class AppSettings
        {
            internal static void Init()
            {
                if (!File.Exists(settingsFile))
                    File.WriteAllText(settingsFile, JsonConvert.SerializeObject(new Data(), Formatting.Indented));

                LoadSettings();
            }

            internal static void LoadSettings()
            {
                try
                {
                    string appSettings = File.ReadAllText(settingsFile);
                    settings = JsonConvert.DeserializeObject<Data>(appSettings);

                    if (!File.Exists(settings.LastMidiFile))
                        settings.LastMidiFile = "";

                }
                catch (Exception ex)
                {
                    File.Delete(settingsFile);
                }
            }

            internal static async Task SaveSettingsAsync()
            {
                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                await File.WriteAllTextAsync(settingsFile, json);
            }
        }
    }
}