using System;
using System.Net;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    // This class is responsable for generating all the HTML and javascript the the application homepage

    class HomePageHandler : AbstractPageHandler
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

            // script for handling clicks/dblclicks/contextclicks
            staticPage += "<script src=\"/script/" + screen + "\"></script>";

            // closing body and html tags

            return buildHTML(staticPage);
        }
    }
}