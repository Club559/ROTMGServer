using System;
using System.Net;
using System.Threading;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Collections.Generic;

namespace dataserver
{
    public static class Program
    {
        public static HttpListener listener;
        public static Thread listen;
        static readonly Thread[] workers = new Thread[5];
        static readonly Queue<HttpListenerContext> contextQueue = new Queue<HttpListenerContext>();
        static readonly object queueLock = new object();
        static readonly ManualResetEvent queueReady = new ManualResetEvent(false);

        public static int port = 8889;

        public static void Main(string[] args)
        {
            try
            {
                listener = new HttpListener();
                listener.Prefixes.Add("http://*:"+port+"/launcher/");
                listener.Start();
                listen = new Thread(ResponseLoop);
                listen.Start();
                for (var i = 0; i < workers.Length; i++)
                {
                    workers[i] = new Thread(Worker);
                    workers[i].Start();
                }
                Console.CancelKeyPress += (sender, e) =>
                {
                    Console.WriteLine("Terminating...");
                    listener.Stop();
                    while (contextQueue.Count > 0)
                        Thread.Sleep(100);
                    Environment.Exit(0);
                };
                Console.WriteLine("Listening at port " + port + "...");
                Thread.CurrentThread.Join();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                //string[] z = new string[]
                //{
                //    ""
                //};
                //listener.Stop();
                //Main(z);
            }
        }
        public static void ResponseLoop()
        {
            try
            {
                do
                {
                    HttpListenerContext context = listener.GetContext();
                    lock (queueLock)
                    {
                        contextQueue.Enqueue(context);
                        queueReady.Set();
                    }
                } while (true);
            }
            catch { }
        }
        public static void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                IRequestHandler handler;

                if (!Handlers.Handlerg.TryGetValue(context.Request.Url.LocalPath, out handler))
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad request";
                    using (StreamWriter wtr = new StreamWriter(context.Response.OutputStream))
                        wtr.Write("<h1>Bad request</h1>");
                }
                else
                    handler.HandleRequest(context);
            }
            catch (Exception e)
            {
                using (StreamWriter wtr = new StreamWriter(context.Response.OutputStream))
                    wtr.Write("<Error>Internal Server Error</Error>");
                Console.Error.WriteLine(e);
            }

            context.Response.Close();
        }
        
        static void Worker()
        {
            while (queueReady.WaitOne())
            {
                HttpListenerContext context;
                lock (queueLock)
                {
                    if (contextQueue.Count > 0)
                        context = contextQueue.Dequeue();
                    else
                    {
                        queueReady.Reset();
                        continue;
                    }
                }

                try
                {
                    ProcessRequest(context);
                }
                catch { }
            }
        }
    }
}
