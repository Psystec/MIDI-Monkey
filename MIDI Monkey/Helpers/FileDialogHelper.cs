using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MIDI_Monkey.Helpers
{
    public static class FileDialogHelper
    {
        public static string? SelectMidiFolder(string initialDirectory = "")
        {
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Filter = "MIDI files (*.mid)|*.mid|All MIDI files (*.mid;*.midi)|*.mid;*.midi|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "Select MIDI File or Folder";
                openFileDialog.CheckFileExists = true;
                openFileDialog.ValidateNames = true;

                if (!string.IsNullOrEmpty(initialDirectory) && Directory.Exists(initialDirectory))
                {
                    openFileDialog.InitialDirectory = initialDirectory;
                }

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return Path.GetDirectoryName(openFileDialog.FileName);
                }

                return null;
            }
        }

        public static async Task<string[]> GetMidiFilesFromDirectoryAsync(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                return Array.Empty<string>();
            }

            return await Task.Run(() =>
            {
                return Directory.GetFiles(folderPath, "*.mid")
                    .Union(Directory.GetFiles(folderPath, "*.midi"))
                    .Select(Path.GetFileName)
                    .ToArray();
            });
        }
    }
}
