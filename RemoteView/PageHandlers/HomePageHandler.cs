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
            // page /click/ is called for handling clicks
            staticPage += "<script>" +

                "var http = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject(\"Microsoft.XMLHTTP\");" + Environment.NewLine +

                "image.addEventListener('contextmenu', function(e){ handleInput('r',e); });" + Environment.NewLine +
                "image.addEventListener('mousedown', function(e){ handleInput('d',e); });" + Environment.NewLine +
                "image.addEventListener('mouseup', function(e){ handleInput('u',e); });" + Environment.NewLine +

                "setTimeout('doubleBufferLoader();', 1000);" + Environment.NewLine +

            // reload image and trigger another reload in 1sec after image has loaded

            "function doubleBufferLoader () {" + Environment.NewLine +
                
                " var newImageUrl = '/screen/' + new Date();" + Environment.NewLine +
                " var anImage = new Image();" + Environment.NewLine +
                " anImage.addEventListener( 'load', function(){ image.src=newImageUrl; setTimeout('doubleBufferLoader();', 1000); }, false );" + Environment.NewLine +
                " anImage.src = newImageUrl; " + Environment.NewLine +
            
            "}" + Environment.NewLine +

            "function handleInput(c, e) { " + Environment.NewLine +

                "e.stopPropagation();" + Environment.NewLine +
                "e.preventDefault();" + Environment.NewLine +
                "px = e.offsetX ? e.offsetX :e.pageX-document.getElementById(\"image\").offsetLeft;" + Environment.NewLine +
                "py = e.offsetY ? e.offsetY :e.pageY-document.getElementById(\"image\").offsetTop;" + Environment.NewLine +

                "var request = '/click/" + screen + "/' + c + '/' + py + '/' + px;" + Environment.NewLine +

                "http.open('GET', request, true);" + Environment.NewLine +
                "http.send();" + Environment.NewLine +

            "}" +
            "</script>";

            // closing body and html tags

            return buildHTML(staticPage);
        }
    }
}