using System.Collections.Generic;

namespace MIDI_Monkey.Models
{
    public class MidiKeyMapConfig
    {
        public string ApplicationName { get; set; } = string.Empty;
        public Dictionary<string, List<string>> KeyMappings { get; set; } = new Dictionary<string, List<string>>();
    }
}
