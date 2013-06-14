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

            return Encoding.UTF8.GetBytes(

                "var http = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject(\"Microsoft.XMLHTTP\");" + Environment.NewLine +

                // incremented everytime a new screen image is requested
                "var counter = 0;" + Environment.NewLine +

                // cache last mouse down event
                "var mousedownEvt = null;" + Environment.NewLine +

                // listeners for mouse actions
                "image.addEventListener('contextmenu', function(e){ rightclick(e); });" + Environment.NewLine +
                "image.addEventListener('mousedown', function(e){ mouseDown(e); });" + Environment.NewLine +
                "image.addEventListener('mouseup', function(e){ mouseUp(e); });" + Environment.NewLine +

                // interval to ask server for a new screen image
                "setInterval('imageLoader();', 1000);" + Environment.NewLine +

            // reload image
            "function imageLoader () {" + Environment.NewLine +

                " var newImageUrl = '/screen/' + (counter++);" + Environment.NewLine +
                " image.src = newImageUrl; " + Environment.NewLine +

            "}" + Environment.NewLine +

            "function cancelUiEvts (e) {" + Environment.NewLine +
                "e.stopPropagation();" + Environment.NewLine +
                "e.preventDefault();" + Environment.NewLine +
            "}" + Environment.NewLine +

            "function sendRequest (e,c) {" + Environment.NewLine +

                "px = e.offsetX ? e.offsetX :e.pageX-document.getElementById(\"image\").offsetLeft;" + Environment.NewLine +
                "py = e.offsetY ? e.offsetY :e.pageY-document.getElementById(\"image\").offsetTop;" + Environment.NewLine +
                "var request = '/click/" + screen + "/' + c + '/' + py + '/' + px;" + Environment.NewLine +
                "http.open('GET', request, true);" + Environment.NewLine +
                "http.send();" + Environment.NewLine +

            "}" + Environment.NewLine +

            "function mouseDown (e) {" + Environment.NewLine +
                "cancelUiEvts(e);" + Environment.NewLine +
                "mousedownEvt = e;" + Environment.NewLine +
            "}" + Environment.NewLine +

            "function mouseUp (e) {" + Environment.NewLine +
                "cancelUiEvts(e);" + Environment.NewLine +
                "sendRequest(mousedownEvt,'d');" + Environment.NewLine +
                "sendRequest(e,'u');" + Environment.NewLine +
            "}" + Environment.NewLine +

            "function rightclick(e) { " + Environment.NewLine +

                "cancelUiEvts(e);" + Environment.NewLine +
                "sendRequest(e,'r');" + Environment.NewLine +

            "}");
        }
    }
}
