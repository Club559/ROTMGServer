using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using EmpireUpdate;
using System.Xml.Linq;

namespace Empire_World_Launcher
{
    public partial class Form1 : Form
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbfont, uint cbfont,
           IntPtr pdv, [In] ref uint pcFonts);

        string Version;
        string LocalVersion;
        string pVersion;
        int offset = 1;
        int maxnews = 0;

        public Form1()
        {
            InitializeComponent();
        }

        public void UpdateVersionNumber(string curVer)
        {
            if (File.Exists("client/version"))
                LocalVersion = File.ReadAllText("client/version");
            else
                LocalVersion = "0.0";
            ver.Text = "v" + LocalVersion;

            ver.Text += (curVer != LocalVersion) ? " - Update available" : "";
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
        public PageItem[] News;
        public void GetNews()
        {
            try
            {
                List<PageItem> pg = new List<PageItem>();
                WebRequest request;
                request = WebRequest.Create("http://80.241.222.17:8889/launcher/getNews");//80.241.222.17
                Stream stream = request.GetResponse().GetResponseStream();
                XDocument dox = XDocument.Load(stream);
                var qq = dox.Root.Elements();
                foreach (var i in qq)
                {
                    var x = ParseNewsItem(i);
                    pg.Add(x);
                    maxnews++;
                }
                News = pg.ToArray();
            }
            catch (Exception ef)
            {
                MessageBox.Show("The launcher encountered an error when trying to connect to the updates server. Please copy the error text and send it to Trapped or Travoos. - " + ef.ToString(), "Error", MessageBoxButtons.OK);
                Close();
            }
        }
        PageItem ParseNewsItem(XElement item)
        {
            //xml structure:
            //<news>
            //  <newsItem date="News item date" cliVersion="Client version">
            //      <name>News item name</name>
            //      <contentItem newsType="News type">News item content (line)</contentItem>
            //      <link>HTTP/FTP/whatever link</link>
            //  </newsItem>
            //</news>
            string name = null;
            string date = null;
            string version = null;
            List<string> content = new List<string>();
            int contentType = 0;
            string link = null;

            foreach (var i in item.Attributes())
            {
                switch (i.Name.ToString())
                {
                    case "date":
                        date = i.Value;
                        break;
                    case "cliVersion":
                        version = i.Value;
                        break;                        
                }
            }
            foreach (var i in item.Elements())
            {
                switch (i.Name.ToString())
                {
                    case "name":
                        name = i.Value;
                        break;
                    case "contentItem":
                        contentType = int.Parse(i.Attribute("newsType").Value);
                        content.Add(i.Value);
                        break;
                    case "link":
                        link = i.Value;
                        break;
                }
            }
            PageItem x = new PageItem()
            {
                Name = name,
                ContentLines = content.ToArray(),
                ContentType = contentType,
                Date = date,
                Version = version
            };
            if (link != null)
            {
                x.Link = link;
            }
            return x;
        }
        private void RetrieveNews()
        {
            GetNews();
            if (News.Length == 0) label1.Text = "No news found!";
                else
                {
                    label1.Text = News[0].Name;
                    label1.Text += "\r\n";
                    if (News[0].ContentType == 0)
                    {
                        foreach (var i in News[0].ContentLines)
                            label1.Text += "    - " + i + "\r\n";
                    }
                    else
                    {
                        label1.Text += "\r\n";
                        foreach (var i in News[0].ContentLines)
                            label1.Text += i + "\r\n";
                    }
                    label2.Text = News[0].Date;
                    //ver.Text = "v" + rdr.GetString("cliVersion");
                    Version = News[0].Version;
                    UpdateVersionNumber(Version);
                    if (News[0].Link != null && News[0].Link != "")
                    {
                        MoreLink.Links.Clear();
                        LinkLabel.Link ml = new LinkLabel.Link();
                        ml.LinkData = News[0].Link;
                        MoreLink.Visible = true;
                        MoreLink.Links.Add(ml);
                    }
                    else
                        MoreLink.Visible = false;

                    button1.Enabled = true;
                    
                    if (maxnews > 1)
                        olderButton.Visible = true;
            }
        }

        private void RetrieveNewsPage()
        {
            var item = News[offset - 1];
            if (News.Length == 0) label1.Text = "No news found!";
            else
                {
                    label1.Text = item.Name;
                    label1.Text += "\r\n";
                    if (item.ContentType == 0)
                    {
                        foreach (var i in item.ContentLines)
                            label1.Text += "    - " + i + "\r\n";
                    }
                    else
                    {
                        label1.Text += "\r\n";
                        foreach (var i in item.ContentLines)
                            label1.Text += i + "\r\n";
                    }
                    label2.Text = item.Date;
                    if (item.Link != null && item.Link != "")
                    {
                        MoreLink.Links.Clear();
                        LinkLabel.Link ml = new LinkLabel.Link();
                        ml.LinkData = item.Link;
                        MoreLink.Visible = true;
                        MoreLink.Links.Add(ml);
                    }
                    else
                        MoreLink.Visible = false;
                }
            

            if (offset > 1)
                newerButton.Visible = true;
            else
                newerButton.Visible = false;

            if (offset == maxnews)
                olderButton.Visible = false;
            else
                olderButton.Visible = true;
        }

        private void LoadPlayer()
        {
            if (!Directory.Exists("client"))
                Directory.CreateDirectory("client");

            if (File.Exists("client/EmpireWorld.swf") && File.Exists("client/version"))
            {
                if (File.ReadAllText("client/version") == Version)
                {
                    try
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/client/EmpireWorld.swf";
                        p.StartInfo.UseShellExecute = true;
                        p.Start();
                    }
                    catch
                    {
                        MessageBox.Show("Please manually choose a default program to open the client with.", "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
            }
            new Updater(Version, this).Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadFont();
            foreach (Control i in this.Controls)
            {
                if (i is Label)
                    (i as Label).Font = new Font(ff, (i as Label).Font.Size);
                if (i is LinkLabel)
                    (i as LinkLabel).Font = new Font(ff, (i as LinkLabel).Font.Size);
                if (i is Button)
                    (i as Button).Font = new Font(ff, (i as Button).Font.Size);
            }
            RetrieveNews();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadPlayer();
        }

        private void olderButton_Click(object sender, EventArgs e)
        {
            offset++;
            RetrieveNewsPage();
        }

        private void newerButton_Click(object sender, EventArgs e)
        {
            offset--;
            RetrieveNewsPage();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void MoreLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(MoreLink.Links[0].LinkData.ToString());
        }

        public string AppName
        {
            get { return "Empire World Launcher"; }
        }

        public string AppID
        {
            get { return "EmpireWorld"; }
        }

        public Assembly AppAssembly
        {
            get { return Assembly.GetExecutingAssembly(); }
        }

        public Icon AppIcon
        {
            get { return this.Icon; }
        }
        //old luci things i guess
        //public Uri UpdateXmlLoc
        //{
        //    get { return new Uri("http://empirewow.no-ip.org/launcher/update.xml"); }
        //}

        public Form Context
        {
            get { return this; }
        }
    }
    public class PageItem
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Version { get; set; }
        public string[] ContentLines { get; set; }
        public int ContentType { get; set; }
        public string Link { get; set; }
        public override string ToString()
        {
            var elem = new XElement("newsItem");
            if (Name != null && Name != "")
                elem.Add(new XElement("name", Name));
            if (Date != null && Date != "")
                elem.Add(new XAttribute("date", Date));
            if (Version != null && Version != "")
                elem.Add(new XAttribute("cliVersion", Version));
            if (ContentLines != null)
            {
                foreach (var i in ContentLines)
                    elem.Add(new XElement("contentItem", i, new XAttribute("newsType", ContentType)));
            }
            if (Link != null && Link != "")
                elem.Add(new XElement("link", Link));
            return elem.ToString();
        }
    }
}
