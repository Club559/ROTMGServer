using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using db;
using System.Xml.Serialization;
using System.IO;
using MySql.Data.MySqlClient;
using System.Web;
using System.Collections.Specialized;

namespace server.@char
{
    class list : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            using (var db = new Database())
            {
                List<ServerItem> filteredServers = null;
                Account a = db.Verify(query["guid"], query["password"]);
                if (a != null)
                {
                    if (a.Banned)
                    {
                        filteredServers = YoureBanned();
                    }
                    else
                    {
                        filteredServers = GetServersForRank(a.Rank);
                    }
                }
                else
                {
                    filteredServers = GetServersForRank(0);
                }

                Chars chrs = new Chars()
                {
                    Characters = new List<Char>() { },
                    NextCharId = 2,
                    MaxNumChars = 1,
                    Account = db.Verify(query["guid"], query["password"]),
                    Servers = filteredServers
                };
                Account dvh = null;
                if (chrs.Account != null)
                {
                    db.GetCharData(chrs.Account, chrs);
                    db.LoadCharacters(chrs.Account, chrs);
                    chrs.News = db.GetNews(chrs.Account);
                    dvh = chrs.Account;
                }
                else
                {
                    chrs.Account = Database.CreateGuestAccount(query["guid"]);
                    chrs.News = db.GetNews(null);
                }

                MemoryStream ms = new MemoryStream();
                XmlSerializer serializer = new XmlSerializer(chrs.GetType(), new XmlRootAttribute(chrs.GetType().Name) { Namespace = "" });

                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                xws.Encoding = Encoding.UTF8;
                XmlWriter xtw = XmlWriter.Create(context.Response.OutputStream, xws);
                serializer.Serialize(xtw, chrs, chrs.Namespaces);
            }
        }

        public static List<ServerItem> GetServersForRank(int r)
        {
            List<ServerItem> slist = GetServers();
            List<ServerItem> removedServers = new List<ServerItem>();

            foreach (var i in slist)
                if (i.RankRequired > r)
                    removedServers.Add(i);

            foreach (var i in removedServers)
                slist.Remove(i);

            return slist;
        }

        public static List<ServerItem> GetServers()
        {
            List<ServerItem> Servers
                   = new List<ServerItem>()
                    {
                        new ServerItem()
                        {
                            Name = "Trav Hamachi",
                            Lat = 22.28,
                            Long = 114.16,
                            DNS = "25.174.8.27",
                            Usage = 0.2,
                            AdminOnly = false,
                            RankRequired = 0
                        },                   
                        new ServerItem()
                        {
                            Name = "Localhost",
                            Lat = 22.28,
                            Long = 114.16,
                            DNS = "127.0.0.1",
                            Usage = 0.2,
                            AdminOnly = false,
                            RankRequired = 1
                        }
                    };
            return Servers;
        }

        public static List<ServerItem> YoureBanned()
        {
            List<ServerItem> Servers
                   = new List<ServerItem>()
                    {
                        new ServerItem()
                        {
                            Name = "You're Banned!",
                            Lat = 22.28,
                            Long = 114.16,
                            DNS = "",
                            Usage = 0.2,
                            AdminOnly = false
                        }
                    };
            return Servers;
        }
    }
}
