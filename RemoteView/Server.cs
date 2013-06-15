
using RemoteView.PageHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace RemoteView
{
    class Server
    {
        Dictionary<String, AbstractPageHandler> decoder = new Dictionary<string, AbstractPageHandler>();
        HttpListener listener;

        private volatile bool running;

        /// <summary>
        /// Constructor
        /// </summary>
        public Server()
        {
            // Homepage 
            decoder.Add("", new HomePageHandler());
            decoder.Add("home", new HomePageHandler());

            // javascript page handles client side clicks and screen image updates
            decoder.Add("script", new JavascriptPageHandler());

            // information about the system
            decoder.Add("info", new InfoPageHandler());

            // this pages process clicks into server side windows events
            decoder.Add("leftclick", new LeftClickPageHandler());
            decoder.Add("rightclick", new RightClickPageHandler());

            // image of choosen device as a png
            decoder.Add("screen", new ScreenPageHandler());

            // image of choosen device as a png
            decoder.Add("favicon.ico", new IconPageHandler());

            // 404 error page
            decoder.Add("404", new NotFoundPageHandler());
        }

        /// <summary>
        /// Start server
        /// </summary>
        /// <param name="port">port to listen to</param>
        public void start(int port)
        {
            try
            {
                listener = new HttpListener();
                listener.Prefixes.Add("http://*:" + port + "/");
                listener.Start();
            }
            catch
            {
                Console.WriteLine("Could not listen on port: {0}.", port);
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

#if DEBUG
                    Console.WriteLine(context.Request.RawUrl);
#endif

                    AbstractPageHandler page;
                    bool found = decoder.TryGetValue(uri[1], out page);
                    if (!found)
                    {
                        // if page not found display 404 page
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
                catch (Exception e)
                {
                    if (this.running) Console.WriteLine(e.Message);
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

        /// <summary>
        /// Stop server
        /// </summary>
        public void stop()
        {
            this.running = false;
            this.listener.Stop();
        }

        /// <summary>
        /// is server running?
        /// </summary>
        /// <returns></returns>
        public bool isRunning()
        {
            return this.running == true;
        }
    }
}