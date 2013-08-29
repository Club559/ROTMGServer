using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Drawing.Text;

namespace Empire_World_Launcher
{
    public partial class Updater : Form
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbfont, uint cbfont,
           IntPtr pdv, [In] ref uint pcFonts);

        string Version;
        Form1 PForm;
        public Updater(string v, Form1 parent)
        {
            InitializeComponent();
            Version = v;
            PForm = parent;
        }

        FontFamily ff;
        public void loadFont()
        {
            byte[] fontArray = Empire_World_Launcher.Properties.Resources.Diavlo2;
            int dataLength = Empire_World_Launcher.Properties.Resources.Diavlo2.Length;
            IntPtr ptrData = Marshal.AllocCoTaskMem(dataLength);
            Marshal.Copy(fontArray, 0, ptrData, dataLength);
            uint cFonts = 0;
            AddFontMemResourceEx(ptrData, (uint)fontArray.Length, IntPtr.Zero, ref cFonts);
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddMemoryFont(ptrData, dataLength);
            Marshal.FreeCoTaskMem(ptrData);
            ff = pfc.Families[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            deleteOld();
            preDownload();
            download();
        }

        private void preDownload()
        {
            label2.Visible = true;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
        }

        private void download()
        {
            label2.Text = "Downloading...";
            label2.Update();
            progressBar1.Visible = true;
            WebClient wc = new WebClient();
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            wc.DownloadFileAsync(new Uri("https://dl.dropboxusercontent.com/u/6436787/pserv.swf"), @"client/EmpireWorld.swf");
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            lblProgress.Visible = true;
            this.lblProgress.Text = String.Format("Downloaded {0} of {1}", FormatBytes(e.BytesReceived, 1, true), FormatBytes(e.TotalBytesToReceive, 1, true));
            progressBar1.Value = e.ProgressPercentage;
        }

        private string FormatBytes(long bytes, int decimalPlaces, bool showByteType)
        {
            double newBytes = bytes;
            string formatString = "{0";
            string byteType = "B";

            if (newBytes > 1024 && newBytes < 1048576)
            {
                newBytes /= 1024;
                byteType = "KB";
            }
            else if (newBytes > 1048576 && newBytes < 1073741824)
            {
                newBytes /= 1048576;
                byteType = "MB";
            }
            else
            {
                newBytes /= 1073741824;
                byteType = "GB";
            }

            if (decimalPlaces > 0)
                formatString += ":0.";

            for (int i = 0; i < decimalPlaces; i++)
                formatString += "0";

            formatString += "}";

            if (showByteType)
                formatString += byteType;

            return string.Format(formatString, newBytes);
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            label2.Text = "Completed!";
            File.WriteAllText("client/version", Version);
            button2.Visible = true;
            PForm.UpdateVersionNumber(Version);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Updater_Load(object sender, EventArgs e)
        {
            loadFont();
            label2.Visible = false;
            foreach (Control i in this.Controls)
            {
                if (i is Label)
                    (i as Label).Font = new Font(ff, (i as Label).Font.Size);
                if (i is LinkLabel)
                    (i as LinkLabel).Font = new Font(ff, (i as LinkLabel).Font.Size);
                if (i is Button)
                    (i as Button).Font = new Font(ff, (i as Button).Font.Size);
            }
        }

        private void deleteOld()
        {
            // Delete a file by using File class static method... 
            if (System.IO.File.Exists(@"client\EmpireWorld.swf"))
            {
                // Use a try block to catch IOExceptions, to 
                // handle the case of the file already being 
                // opened by another process. 
                try
                {
                    System.IO.File.Delete(@"client\EmpireWorld.swf");
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }
    }
}
