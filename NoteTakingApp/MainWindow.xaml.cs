using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NoteTakingApp
{
    public partial class MainWindow : Window
    {
        private string msg;
        private string title;
        private string path;
        private Note note;
        private NoteContainer notes;

        public MainWindow()
        {
            InitializeComponent();
            notes = new NoteContainer();
            path = "";
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            Stream stream = null;
            BinaryFormatter bf;
            string tmp = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string directory = Path.Combine(tmp, "NoteTakingApp");
            string pth = Path.Combine(directory, "notes.bin");
            try
            {
                if (Directory.Exists(directory) && File.Exists(pth))
                {
                    stream = File.Open(pth, FileMode.Open);
                    bf = new BinaryFormatter();
                    notes = (NoteContainer)bf.Deserialize(stream);
                    if (notes.Counts > 0)
                    {
                        msg = TextMsg.Text = notes[notes.Counts - 1].Message;
                        title = Title.Text = notes[notes.Counts - 1].Title;
                        path = notes[notes.Counts - 1].Path;
                    }
                }
            }
            catch (Exception evt)
            {
                SerialiazingError(evt);
            }
            finally
            {
                if (stream != null) stream.Close();
                TextList.DataContext = notes;
                RefreshList();
            }
        }

        private void SaveText_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void SaveAsText_Click(object sender, RoutedEventArgs e)
        {
            SaveAsFile();
        }

        private void NewText_Click(object sender, RoutedEventArgs e)
        {
            msg = title = TextMsg.Text = Title.Text = path = "";
            Title.BorderBrush = Brushes.LightGray;
            path = "";
        }

        private void RefreshTextList_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }

        private void DeleteText_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.DataContext is Note)
            {
                notes.Remove((Note)btn.DataContext);
            }
            RefreshList();
        }

        private void Title_KeyboardInput(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Title.Text.Length != 0) Title.BorderBrush = Brushes.LightGray;
        }

        private void ReadText_Click(object sender, RoutedEventArgs e)
        {
            int index = TextList.SelectedIndex;
            if (index < 0) return;
            Note textMsg = notes[index];

            msg = TextMsg.Text = textMsg.Message;
            title = Title.Text = textMsg.Title;
            path = textMsg.Path;
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt|C# file (*.cs)|*.cs";
            if (ofd.ShowDialog() == true)
            {
                path = Path.GetFullPath(ofd.FileName);
                title = Path.GetFileNameWithoutExtension(path);
                Title.Text = title == null ? "Undefined" : title;
                msg = TextMsg.Text = File.ReadAllText(path);

                Note textMessage = new Note(msg, title, path);

                notes.Add(textMessage);
                RefreshList();
            }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            string tmp = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string directory = Path.Combine(tmp, "NoteTakingApp");
            string pth = Path.Combine(directory, "notes.bin");
            Stream stream = null;
            BinaryFormatter bf;
            try
            {
                bf = new BinaryFormatter();
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    stream = File.Create(pth);
                }
                stream = File.Open(pth, FileMode.Create);

                bf.Serialize(stream, notes);
            }
            catch (Exception evt)
            {
                SerialiazingError(evt);
            }
            finally
            {
                if (stream != null) stream.Close();
            }
        }
    }
}