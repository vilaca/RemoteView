
using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    /// <summary>
    /// base class for all the request handlers
    /// </summary>
    abstract class AbstractPageHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response">response to be sent to client</param>
        /// <param name="uri">tokenized request URI</param>
        /// <returns>response body</returns>
        public abstract byte[] HandleRequest(HttpListenerResponse response, String[] uri);

        /// <summary>
        /// Parse from tokenized URI the selected Screen Device.
        /// Default to 0 if parameter not present or error
        /// </summary>
        /// <param name="uri">tokenized URI</param>
        /// <param name="screens">system screens</param>
        /// <returns>selected screen or default(0)</returns>
        internal static int GetRequestedScreenDevice(String[] uri, Screen[] screens)
        {
            int screen = 0;
            if (uri.Length > 2)
            {
                try
                {
                    screen = Convert.ToInt16(uri[2], CultureInfo.InvariantCulture);
                }
                catch { }

                if (screen < 0 || screen >= screens.Length)
                {
                    screen = 0;
                }
            }
            return screen;
        }

        /// <summary>
        /// boilerplate HTML wraping for all the response streams
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal static byte[] BuildHTML(string content)
        {
            return Encoding.UTF8.GetBytes("<!doctype html>" + Environment.NewLine +
                "<head><title>Remote View</title></head>" + Environment.NewLine +
                "<body>" + Environment.NewLine +
                content +
                "</body>" + Environment.NewLine +
                "</html>" + Environment.NewLine);
        }
    }
}
