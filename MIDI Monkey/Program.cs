using System.Runtime.InteropServices;

namespace MIDI_Monkey
{
    internal static class Program
    {

        //[DllImport("user32.dll")]
        //private static extern bool SetProcessDPIAware();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //SetProcessDPIAware(); // Optional if manifest already covers it
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            ApplicationConfiguration.Initialize();
            Application.Run(new FormMain());
        }
    }
}