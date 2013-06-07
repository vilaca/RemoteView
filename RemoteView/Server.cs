
using RemoteView.PageHandlers;

using System;
using System.Collections.Generic;
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
            // application pages
            decoder.Add("", new HomePageHandler());
            decoder.Add("screen", new ScreenPageHandler());

            // error pages
            decoder.Add("404", new NotFoundPageHandler());
        }

        public void start()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(@"http://*:" + port + "/");
            listener.Start();
            
            this.running = true;

            do
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
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                // must close the output stream.
                output.Close();
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
