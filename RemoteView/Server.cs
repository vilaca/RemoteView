
using RemoteView.PageHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace RemoteView
{
    class Server
    {
        Dictionary<String, PageHandler> decoder = new Dictionary<string, PageHandler>();

        private volatile bool running;
        private int port = 6060;

        public Server()
        {
            // Homepage 
            decoder.Add("", new HomePageHandler());
            decoder.Add("home", new HomePageHandler());

            // information about the system
            decoder.Add("info", new InfoPageHandler());

            // this page processes clicks into windows events
            decoder.Add("click", new ClickPageHandler());

            // image of choosen device as a png
            decoder.Add("screen", new ScreenPageHandler());

            // image of choosen device as a png
            decoder.Add("favicon.ico", new IconPageHandler());

            // 404 error page
            decoder.Add("404", new NotFoundPageHandler());
        }

        public void start()
        {
            HttpListener listener;
            try
            {
                listener = new HttpListener();
                listener.Prefixes.Add(@"http://*:" + port + "/");
                listener.Start();
            }
            catch
            {
                // oops
                return;
            }

            this.running = true;

            do
            {
                Stream output = null;
                try
                {
                    // Note: The GetContext method blocks while waiting for a request. 
                    HttpListenerContext context = listener.GetContext();

                    String[] uri = context.Request.RawUrl.Split('/');

                    PageHandler page;
                    bool found = decoder.TryGetValue(uri[1], out page);

                    if (!found)
                    {
                        page = decoder["404"];
                    }

                    HttpListenerResponse response = context.Response;

                    byte[] buffer = page.handleRequest(response, uri);

                    // Get a response stream and write the response to it.
                    response.ContentLength64 = buffer.Length;
                    output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);

                    // must close the output stream.
                    output.Close();
                }
                catch
                {
                    try
                    {
                        if (output != null) output.Close();
                    }
                    catch { }
                }
            }
            while (this.running);

            listener.Stop();
        }

        public void stop()
        {
            this.running = false;
        }

        public bool isRunning()
        {
            return this.running == true;
        }
    }
}