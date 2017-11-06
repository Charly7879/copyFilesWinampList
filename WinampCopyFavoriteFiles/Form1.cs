using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinampCopyFavoriteFiles
{
    public partial class Form1 : Form
    {
        String sourceFiles = string.Empty;
        String targetFiles = string.Empty;
        String source = string.Empty;
        String file = string.Empty;
        List<string> listFiles = new List<string>();
        String[] lines;
        BackgroundWorker tarea = new BackgroundWorker();

        public List<string> ListFiles { get => ListFiles1; set => ListFiles1 = value; }
        public string TargetFiles { get => targetFiles; set => targetFiles = value; }
        public List<string> ListFiles1 { get => listFiles; set => listFiles = value; }

        public Form1()
        {
            
            InitializeComponent();
        }

        private void btnSourceFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog opfd = new OpenFileDialog();
            opfd.Filter = "Winamp list files|*.m3u8";
            opfd.Title = "Select a file";

            if(opfd.ShowDialog() == DialogResult.OK)
            {
                source = Path.GetDirectoryName(@opfd.FileName) + "\\";
                sourceFiles = opfd.FileName;
                lines = File.ReadAllLines(@sourceFiles);

                txtBoxSourceFile.Text = sourceFiles;

                foreach (string line in lines)
                {
                    if (!line.Contains("#EXTINF") && !line.Contains("#EXTM3U"))
                    {
                        if (!line.Contains("\\"))
                        {
                            ListFiles.Add(source + line);
                        }
                        else
                        {
                            ListFiles.Add(line);
                        }
                    }
                }                
            }
        }

        private void btnTargetFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                TargetFiles = fbd.SelectedPath;
                txtBoxTargetFile.Text = TargetFiles;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            progressBar1.Minimum = 0;
            progressBar1.Step = 1;
            
            CopyFiles copyFiles = new CopyFiles(this);
            copyFiles.comenzar();
        }

    }
}
