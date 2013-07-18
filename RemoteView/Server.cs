using RemoteView.PageHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace RemoteView {
    sealed class Server : IDisposable {
        /// <summary>
        /// Keep list of all resources to be invoked according to received HTTP requests
        /// </summary>
        private Dictionary<String, AbstractPageHandler> decoder = new Dictionary<string, AbstractPageHandler> ();
        /// <summary>
        /// HTTP listener for server
        /// </summary>
        private HttpListener listener = new HttpListener ();

        /// <summary>
        /// Constructor
        /// </summary>
        public Server () {

            // Homepage 
            decoder.Add ("", new HomePageHandler ());
            decoder.Add ("home", new HomePageHandler ());

            // javascript page handles client side clicks and screen image updates
            decoder.Add ("script", new JavascriptPageHandler ());

            // information about the system
            decoder.Add ("info", new InfoPageHandler ());

            // this pages process clicks into server side windows events
            decoder.Add ("leftclick", new LeftClickPageHandler ());
            decoder.Add ("rightclick", new RightClickPageHandler ());
            decoder.Add ("mousemove", new MouseMovePageHandler ());

            // image of choosen device as a png
            decoder.Add ("screen", new ScreenPageHandler ());

            // image of choosen device as a png
            decoder.Add ("favicon.ico", new IconPageHandler ());

            // 404 error page
            decoder.Add ("404", new NotFoundPageHandler ());
        }

        /// <summary>
        /// Start server
        /// </summary>
        /// <param name="port">port to listen to</param>
        public Server Start (string address, int port) {
            try {
                Console.WriteLine ("Starting server at {0}:{1}", address, port);
                listener.Prefixes.Add ("http://" + address + ":" + port + "/");
                listener.IgnoreWriteExceptions = true;
                listener.Start ();

                new Thread (Start).Start ();
            } catch {
                Console.WriteLine ("Could not listen on port: {0}.", port);
            }

            return this;
        }

        /// <summary>
        /// This where the server runs
        /// </summary>
        private void Start () {
            do {
                HttpListenerResponse response = null;
                Stream output = null;
                try {
                    // Note: The GetContext method blocks while waiting for a request. 
                    HttpListenerContext context = listener.GetContext ();

                    String[] uri = context.Request.RawUrl.Split ('/');

#if DEBUG
                    Console.WriteLine (context.Request.RawUrl);
#endif

                    AbstractPageHandler page;
                    bool found = decoder.TryGetValue (uri [1], out page);
                    if (!found) {
                        // if page not found display 404 page
                        page = decoder ["404"];
                    }

                    response = context.Response;

                    byte[] buffer = page.HandleRequest (response, uri);

                    // Get a response stream and write the response to it.
                    response.ContentLength64 = buffer.Length;
                    output = response.OutputStream;
                    output.Write (buffer, 0, buffer.Length);

                } catch (Exception e) {
                    if (listener.IsListening)
                        Console.WriteLine (e.Message);
                } finally {
                    if (response != null) {
                        try {
                            if (output != null)
                                output.Close ();
                        } catch {
                        }
                        response.Close ();
                    }
                }
            } while (listener.IsListening);
        }

        /// <summary>
        /// Stop server
        /// </summary>
        public void Stop () {
            this.listener.Stop ();
        }

        /// <summary>
        /// is server running?
        /// </summary>
        /// <returns></returns>
        public bool IsRunning () {
            return listener.IsListening;
        }

        public void Dispose () {
            if (listener != null)
                listener.Close ();

            // some page handlers might need to be disposed

            foreach (AbstractPageHandler page in decoder.Values) {
                if (page is IDisposable) {
                    ((IDisposable)page).Dispose ();
                }
            }
        }
    }
}