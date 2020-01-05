using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TagLib;
using System.IO;
using System.Diagnostics;

namespace MP3Helper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            directories = new List<string>();
        }

        private static List<string> directories;

        private void OpenMP3(string path)
        {
            var audioFile = TagLib.File.Create(path);
            performersRichTextBox.Text = audioFile.Tag.JoinedPerformers;
            albumTextBox.Text = audioFile.Tag.Album;
            yearTextBox.Text = audioFile.Tag.Year.ToString();
            genresRichTextBox.Text = audioFile.Tag.JoinedGenres;
            bpmTextBox.Text = audioFile.Tag.BeatsPerMinute.ToString();
            titleTextBox.Text = audioFile.Tag.Title;
            commentRichTextBox.Text = audioFile.Tag.Comment;
            filenameTextBox.Text = Path.GetFileName(path);
        }

        private void WriteToMP3(string path)
        {
            var audioFile = TagLib.File.Create(path);
            audioFile.Tag.Performers = performersRichTextBox.Text.Split(';');
            audioFile.Tag.Album = albumTextBox.Text;
            audioFile.Tag.Year = Convert.ToUInt32(yearTextBox.Text);
            audioFile.Tag.Genres = genresRichTextBox.Text.Split(';');
            audioFile.Tag.BeatsPerMinute = Convert.ToUInt32(bpmTextBox.Text);
            audioFile.Tag.Title = titleTextBox.Text;
            audioFile.Tag.Comment = commentRichTextBox.Text;

            System.IO.File.Move(path, Path.GetDirectoryName(path) + "\\" + filenameTextBox.Text);
        }

        private void ChooseDirectory()
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                directories = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.mp3*", SearchOption.AllDirectories).ToList();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OpenMP3(directories[listBox1.SelectedIndex]);
        }

        private void saveChangesButton_Click(object sender, EventArgs e)
        {
            WriteToMP3(directories[listBox1.SelectedIndex]);
            MessageBox.Show("Changes saved!");

            directories = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.mp3*", SearchOption.AllDirectories).ToList();
            listBox1.DataSource = null;
            listBox1.SelectedIndex = -1;
            listBox1.DataSource = directories;
            listBox1.Update();
        }

        private void selectFolderButton_Click(object sender, EventArgs e)
        {
            ChooseDirectory();
            listBox1.DataSource = directories;
        }
    }
}
