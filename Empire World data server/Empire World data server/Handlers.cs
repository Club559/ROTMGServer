using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using dataserver;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.IO;

namespace dataserver
{
    interface IRequestHandler
    {
        void HandleRequest(HttpListenerContext context);
    }
    static class Handlers
    {
        public static readonly Dictionary<string, IRequestHandler> Handlerg = new Dictionary<string, IRequestHandler>()
        {
            { "/launcher/getVersion", new getVersion() },
            { "/launcher/getNews", new getNews() },
            { "/launcher/getLatestClient", new getLatestClient() },
            { "/launcher/getNewsPage", new getNewsPage() }
        };
    }
    class getNewsPage : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
            {
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());
            }
            int offset = int.Parse(query["offset"]);
            using (Database dbx = new Database())
            {
                var cmd = dbx.CreateQuery();
                cmd.CommandText = "SELECT * FROM info ORDER BY date DESC LIMIT 1 OFFSET @off;";
                cmd.Parameters.AddWithValue("@off", offset);
                using (var rdr = cmd.ExecuteReader())
                {
                    rdr.Read();
                    var page = new PageItem()
                    {
                    };
                    page.Name = rdr.GetString("name");
                    List<string> contents = new List<string>();
                    if (rdr.GetInt32("newsType") == 0)
                    {
                        foreach (var i in rdr.GetString("contents").Split('&'))
                        {
                            contents.Add(i);
                        }
                        page.ContentType = 0;
                        page.ContentLines = contents.ToArray();
                    }
                    else
                    {
                        foreach (var i in rdr.GetString("contents").Split('&'))
                            contents.Add(i);
                        page.ContentType = rdr.GetInt32("newsType");
                        page.ContentLines = contents.ToArray();
                    }
                    DateTime time = rdr.GetDateTime("date");
                    page.Date = time.ToString("g");
                    if (!rdr.IsDBNull(rdr.GetOrdinal("link")) && rdr.GetString("link") != "")
                    {
                        page.Link = rdr.GetString("link");
                    }
                    byte[] fff = Encoding.ASCII.GetBytes(page.ToString());
                    context.Response.OutputStream.Write(fff, 0, fff.Length);
                    context.Response.Close();
                }
            }
        }
    }
    class getVersion : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            //reads database for latest version, might use executescalar depending on the read order, i'll see
            string version = "";
            using (Database db = new Database())
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "SELECT cliversion FROM clinews";
                object scalar = cmd.ExecuteScalar();
                version = scalar.ToString();
            }
            byte[] responsebytes = Encoding.ASCII.GetBytes(version);
            context.Response.OutputStream.Write(responsebytes, 0, responsebytes.Length);
            context.Response.Close();
        }
    }
    class getLatestClient : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            //elaborates a download link basing on the latest version number and the usual file location (luci's dropbox for example)
            using (Database db = new Database())
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "SELECT download FROM clinews";
                var scalar = Encoding.ASCII.GetBytes(cmd.ExecuteScalar().ToString());
                context.Response.OutputStream.Write(scalar, 0, scalar.Length);
                context.Response.Close();
            }
        }
    }
    class getNews : IRequestHandler //serializes as XML the news from the patches table ("launcher") of the database
    {
        //xml structure:
        //<news>
        //  <newsItem date="News item date" cliVersion="Client version">
        //      <name>News item name</name>
        //      <contentItem newsType="News type">News item content (line)</contentItem>
        //      <link>Whatever</link>
        //  </newsItem>
        //</news>
        public void HandleRequest(HttpListenerContext context)
        {
            try
            {
                var dof = new XDocument();
                dof.Add(new XElement("news"));
                var doc = dof.Root;
                bool nullnews = true;
                using (Database db = new Database())
                {
                    var cmd = db.CreateQuery();
                    cmd.CommandText = "SELECT * FROM clinews ORDER BY date DESC;";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (!rdr.HasRows) nullnews = true;
                        else
                        {
                            nullnews = false;
                            while (rdr.Read())
                            {
                                XElement newsItem = new XElement("newsItem");
                                newsItem.Add(new XElement("name", rdr.GetString("name")));
                                if (rdr.GetInt32("newsType") == 0)
                                {
                                    foreach (var i in rdr.GetString("contents").Split('&'))
                                        newsItem.Add(new XElement("contentItem", i, new XAttribute("newsType", rdr.GetInt32("newsType"))));
                                }
                                else
                                {
                                    foreach (var i in rdr.GetString("contents").Split('&'))
                                        newsItem.Add(new XElement("contentItem", i, new XAttribute("newsType", rdr.GetInt32("newsType"))));
                                }
                                DateTime time = rdr.GetDateTime("date");
                                newsItem.Add(new XAttribute("date", time.ToString("g")));
                                //ver.Text = "v" + rdr.GetString("cliVersion");
                                newsItem.Add(new XAttribute("cliVersion", rdr.GetString("cliVersion")));
                                if (!rdr.IsDBNull(rdr.GetOrdinal("link")))
                                {
                                    if (rdr.GetString("link") != "" || rdr.GetString("link") != null)
                                    {
                                        newsItem.Add(new XElement("link", rdr.GetString("link")));
                                    }
                                }
                                doc.Add(newsItem);
                            }
                        }
                    }
                }
                var bytes = System.Text.Encoding.ASCII.GetBytes(doc.ToString());
                if (nullnews == true)
                {
                    bytes = System.Text.Encoding.ASCII.GetBytes("nullnews");
                }
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                context.Response.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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
        public string ToString()
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