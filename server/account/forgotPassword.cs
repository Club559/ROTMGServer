using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace server.account
{
    class forgotPassword : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            byte[] status = Encoding.UTF8.GetBytes("<Error>This function has been disabled, please contact a server admin.</Error>");
            context.Response.OutputStream.Write(status, 0, status.Length);
        }
    }
}
