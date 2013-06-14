using System;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class JavascriptPageHandler : PageHandler
    {
        // screen devices list
        private Screen[] screens = Screen.AllScreens;


        public override byte[] handleRequest(HttpListenerResponse response, string[] uri)
        {
            int screen = getRequestedScreenDevice(uri, screens);

            return Encoding.UTF8.GetBytes("var http = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject(\"Microsoft.XMLHTTP\");" + Environment.NewLine +

                "image.addEventListener('contextmenu', function(e){ handleInput('r',e); });" + Environment.NewLine +
                "image.addEventListener('mousedown', function(e){ handleInput('d',e); });" + Environment.NewLine +
                "image.addEventListener('mouseup', function(e){ handleInput('u',e); });" + Environment.NewLine +

                "setTimeout('doubleBufferLoader();', 1000);" + Environment.NewLine +

            // reload image and trigger another reload in 1sec after image has loaded

            "function doubleBufferLoader () {" + Environment.NewLine +

                " var newImageUrl = '/screen/' + new Date().getMilliseconds();" + Environment.NewLine +
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

            "}");
        }
    }
}
