namespace MIDI_Monkey.Models
{
    public class MidiKeyMapEntry
    {
        public int MidiNote { get; set; }
        public string NoteName { get; set; } = string.Empty;
        public string Modifier { get; set; } = "None";
        public string Key { get; set; } = string.Empty;
    }
}
