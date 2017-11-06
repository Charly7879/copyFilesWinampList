using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinampCopyFavoriteFiles
{
    class CopyFiles
    {
        Form1 form;
        Thread myThread;
        string fileName;
        string fileCopy;
        string targetFiles;
        int countFiles;
        List<string> listFiles;

        public Thread MyThread { get => myThread; set => myThread = value; }

        public CopyFiles(Form1 form)
        {
            this.form = form;
            this.MyThread = new Thread(copy);
            this.fileName = string.Empty;
            this.fileCopy = string.Empty;
            this.targetFiles = form.TargetFiles;
            this.countFiles = form.ListFiles.Count;
            this.listFiles = form.ListFiles;
        }

        public void copy()
        {
            try
            {
                //form.btnStart.Enabled = false;

                foreach (String file in listFiles)
                {
                    fileName = Path.GetFileName(@file);
                    fileCopy = Path.Combine(targetFiles, fileName);
                    File.Copy(file, fileCopy, true);

                    if(form.InvokeRequired)
                    {
                        try
                        {
                            form.Invoke(new Action(() => form.progressBar1.PerformStep()));
                            form.Invoke(
                                new Action(
                                    () => form.labelInfo.Text = (int)((form.progressBar1.Value - form.progressBar1.Minimum) / (double)(form.progressBar1.Maximum - form.progressBar1.Minimum) * 100) + "%"
                                    )
                                );
                        }
                        catch (Exception e)
                        {
                            Thread.CurrentThread.Interrupt();
                        }
                    }

                    countFiles--;
                }

                if (countFiles == 0)
                {
                    MessageBox.Show("Los archivos han sido copiados correctamente.", "Copia finalizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    if (form.InvokeRequired)
                    {
                        try
                        {
                            form.Invoke(new Action(() => form.btnStart.Enabled = true ));
                            form.Invoke(new Action(() => form.progressBar1.Value = 0));
                            form.Invoke(new Action(() => form.labelInfo.Text = "0%"));
                        }
                        catch (Exception e)
                        {
                            Thread.CurrentThread.Interrupt();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("El proceso ha fallado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //form.btnStart.Enabled = true;
            }
        }

        public void esperar()
        {
            try
            {
                Thread.Sleep(10);
            }
            catch (Exception e)
            {
                Thread.CurrentThread.Interrupt();
            }
        }

        public void comenzar()
        {
            MyThread.Start();
        }

        public void detener()
        {
            MyThread.Interrupt();
        }

        public void finalizar()
        {
            MyThread.Abort();
        }

    }
}
