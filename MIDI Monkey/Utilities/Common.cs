using System.Diagnostics;

namespace MIDI_Monkey.Utilities
{
    static class Common
    {
        public static void OpenDonationPage()
        {
            string url = "https://paypal.me/PsystecZA";
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        public static void OpenSignalMIDIPage()
        {
            string url = "https://signalmidi.app/";
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        public static void OpenDiscordPage()
        {
            string url = "https://discord.gg/s3vqJRCGht";
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}
