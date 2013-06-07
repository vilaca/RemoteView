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

            staticPage += "</p>";

            string screen = uri.Length > 2 ? "/" + uri[2] : "/0";
            staticPage += "<img id=\"image\" name=\"image\" src=\"/screen" + screen + "\">";

            staticPage += "<script>" +
                "image.addEventListener('contextmenu', function(e){ handleInput('r',e); });" +
                "image.addEventListener('click', function(e){ handleInput('l',e); });" +
                "function handleInput(c, e) { e.stopPropagation();e.preventDefault(); }" +
                "</script>";

            staticPage += "</body></html>";

            return System.Text.Encoding.UTF8.GetBytes(staticPage);
        }
    }
}
