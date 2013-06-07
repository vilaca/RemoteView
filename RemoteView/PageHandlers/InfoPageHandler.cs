using System;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class InfoPageHandler : PageHandler
    {
        private Screen[] screens = Screen.AllScreens;

        public override byte[] handleRequest(HttpListenerResponse response, string[] uri)
        {
            String page = "";
            page += "<html>";
            page += "<head><title>Desktop Viewer</title></head>";
            page += "<body>";

            page += "<p><b>Desktop Viewer - Desktop sharing Application</b></p>";

            for (int i = 0; i < screens.Length; i++)
            {
                page += "<p><b>Screen " + i + ":</b> '" + screens[i].DeviceName + "', Width:" + screens[i].Bounds.Width + ", Height:" + screens[i].Bounds.Height + "</p>";
            }

            page += "</body>";
            page += "</html>";
            return Encoding.UTF8.GetBytes(page);
        }
    }
}
