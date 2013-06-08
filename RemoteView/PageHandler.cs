
using System;
using System.Net;
using System.Windows.Forms;

namespace RemoteView
{
    abstract class PageHandler
    {
        public abstract byte[] handleRequest(HttpListenerResponse response, String[] uri);

        /// <summary>
        /// Parse from tokenized URI the selected Screen Device.
        /// Default to 0 if parameter not present or error
        /// </summary>
        /// <param name="uri">tokenized URI</param>
        /// <param name="screens">system screens</param>
        /// <returns></returns>
        internal int getRequestedScreenDevice(String[] uri, Screen[] screens)
        {
            int screen = 0;
            if (uri.Length > 2)
            {
                try
                {
                    screen = Convert.ToInt16(uri[2]);
                }
                catch { }

                if (screen < 0 || screen >= screens.Length)
                {
                    screen = 0;
                }
            }
            return screen;
        }
    }
}
