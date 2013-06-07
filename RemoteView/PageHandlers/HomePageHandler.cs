using System;
using System.Net;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class HomePageHandler : PageHandler
    {
        private Screen[] screens = Screen.AllScreens;

        public override byte[] handleRequest(HttpListenerResponse response, String[] uri)
        {
            String staticPage = "<html><title>Desktop view</title><body><p>";

            for (int i = 0; i < screens.Length; i++)
            {
                staticPage += "| <a href=\"/home/" + i + "\">Screen:" + i + "</a>";
            }

            if (uri.Length == 2)
            {
                staticPage += "</p><img id=\"image\" name=\"image\" src=\"/screen\"></body></html>";
            }
            else
            {
                staticPage += "</p><img id=\"image\" name=\"image\" src=\"/screen/" + uri[2] + "\"></body></html>";
            }
            return System.Text.Encoding.UTF8.GetBytes(staticPage);
        }
    }
}
