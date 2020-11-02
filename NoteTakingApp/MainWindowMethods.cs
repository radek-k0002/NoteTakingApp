using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace NoteTakingApp
{
    public partial class MainWindow
    {
        public static void SerialiazingError(Exception evt)
        {
            SerialiazingErrorWindow sew = new SerialiazingErrorWindow();
            sew.ErrorText.Text = $"Error: '{evt.Message}' :(";
            sew.Show();
        }

        private void SaveFile()
        {
            if (!GetMsgTitle()) return;
            if (!File.Exists(path)) SaveAsFile();
            else if (File.Exists(path))
            {
                File.WriteAllText(path, msg);
                Note note = new Note(msg, title, path);

                int index = notes.FindByPath(path);

                if (index > -1) notes[index] = note;

                RefreshList();
            }
        }

        private void SaveAsFile()
        {
            if (!GetMsgTitle()) return;

            if (SaveAs())
            {
                note = new Note(msg, title, path);
                notes.Add(note);

                RefreshList();
            }
        }

        private bool SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
            saveFileDialog.FileName = title;
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, msg);
                path = Path.GetFullPath(saveFileDialog.FileName);
                return true;
            }
            return false;
        }

        private bool GetMsgTitle()
        {
            if (Title.Text.Length == 0)
            {
                Title.BorderThickness = new Thickness(3);
                Title.BorderBrush = Brushes.Red;
                return false;
            }
            title = Title.Text;
            msg = TextMsg.Text;
            return true;
        }

        private void RefreshList()
        {
            TextList.Items.Refresh();
        }
    }
}