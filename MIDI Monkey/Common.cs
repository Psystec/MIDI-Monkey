using System.Diagnostics;

namespace MIDI_Monkey
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
        public static void ShowErrorMessage(string message, bool popup = true)
        {
            if (popup)
                MessageBox.Show(message, "MIDI Monkey", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Logging.DebugLog($"{message}");
        }
    }
}
