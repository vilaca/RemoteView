using System;
using System.Net;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    /// <summary>
    /// This class is responsable for generaring all the HTML and javascript the the application homepage
    /// </summary>
    class HomePageHandler : PageHandler
    {
        // screen devices list

        private Screen[] screens = Screen.AllScreens;

        public override byte[] handleRequest(HttpListenerResponse response, String[] uri)
        {

            // basic html, including head tags
            
            String staticPage = "<html><head><title>Desktop view</title></head><body>";
            
            // list amount of screen devices

            staticPage +="<p>";
            for (int i = 0; i < screens.Length; i++)
            {
                staticPage += "| <a href=\"/home/" + i + "\">Screen:" + i + "</a>";
            }
            staticPage += "</p>";

            // image for current screen device

            string screen = uri.Length > 2 ? "/" + uri[2] : "/0";
            staticPage += "<img id=\"image\" name=\"image\" src=\"/screen" + screen + "\">";

            // script for handling clicks/dblclicks

            staticPage += "<script>" +
                "image.addEventListener('contextmenu', function(e){ handleInput('r',e); });" +
                "image.addEventListener('click', function(e){ handleInput('l',e); });" +
                "function handleInput(c, e) { e.stopPropagation();e.preventDefault(); }" +
                "</script>";

            // closing body and html tags

            staticPage += "</body></html>";

            return System.Text.Encoding.UTF8.GetBytes(staticPage);
        }
    }
}
