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
                staticPage += "| <a href=\"javascript:image.src='screen/" + i + "'\">Screen:" + i + "</a>";
            }

            staticPage += "</p><img id=\"image\" name=\"image\" src=\"screen\"></body></html>";

            return System.Text.Encoding.UTF8.GetBytes(staticPage);
        }
    }
}
