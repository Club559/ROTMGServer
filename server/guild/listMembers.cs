using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using db;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml;

namespace server.guild
{
    class listMembers : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            using (var db = new Database())
            {
                var acc = db.Verify(query["guid"], query["password"]);
                byte[] status;
                if (acc == null)
                    status = Encoding.UTF8.GetBytes("<Error>Bad login</Error>");
                else
                {
                    try
                    {
                        status = Encoding.UTF8.GetBytes(db.HTTPGetGuildMembers(Convert.ToInt32(query["num"]), Convert.ToInt32(query["offset"]), acc));
                    }
                    catch
                    {
                        status = Encoding.UTF8.GetBytes("<Error>Guild member error</Error>");
                    }
                }
                context.Response.OutputStream.Write(status, 0, status.Length);
                context.Response.Close();
            }
        }
    }
}
