using System;
using System.Net;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    // This class is responsable for generaring all the HTML and javascript the the application homepage

    class HomePageHandler : PageHandler
    {
        // screen devices list

        private Screen[] screens = Screen.AllScreens;

        /// <summary>
        /// Generate the HTML and javascript needed for this the homepage
        /// </summary>
        /// <param name="response">not used</param>
        /// <param name="uri">raw URI tokenized by '/'</param>
        /// <returns>HTML page + javascript</returns>
        public override byte[] handleRequest(HttpListenerResponse response, String[] uri)
        {

            // list amount of screen devices

            string staticPage = "<p>";
            for (int i = 0; i < screens.Length; i++)
            {
                staticPage += "| <a href=\"/home/" + i + "\">Screen:" + i + "</a>";
            }
            staticPage += "</p>";

            // display current screen on image
            // if no parameter for screen, default to 0
            int screen = getRequestedScreenDevice(uri, screens);
            staticPage += "<img id=\"image\" name=\"image\" src=\"/screen/" + screen + "\">";

            // script for handling clicks/dblclicks
            // page /script/ is called for handling clicks
            staticPage += "<script>" +

                "image.addEventListener('contextmenu', function(e){ handleInput('r',e); });" +
                "image.addEventListener('click', function(e){ handleInput('l',e); });" +

                "function handleInput(c, e) { " +
                    "e.stopPropagation();" +
                    "e.preventDefault();" +
                    "px = e.offsetX ? e.offsetX :e.pageX-document.getElementById(\"image\").offsetLeft;" +
                    "py = e.offsetY ? e.offsetY :e.pageY-document.getElementById(\"image\").offsetTop;" +
                    "window.location='/click/" + screen + "/' + c + '/' + py + '/' + px;"
                + "}" +
                "</script>";

            // closing body and html tags

            return buildHTML(staticPage);
        }
    }
}
