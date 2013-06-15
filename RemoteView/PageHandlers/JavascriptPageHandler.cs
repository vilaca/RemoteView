using System;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class JavascriptPageHandler : AbstractPageHandler
    {
        // screen devices list
        private Screen[] screens = Screen.AllScreens;

        public override byte[] handleRequest(HttpListenerResponse response, string[] uri)
        {
            int screen = getRequestedScreenDevice(uri, screens);

            response.ContentType = "application/javascript";

            return Encoding.UTF8.GetBytes(

                "var http = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject(\"Microsoft.XMLHTTP\");" + Environment.NewLine +

                // cache last mouse down event
                "var mousedownEvt = null;" + Environment.NewLine +

                // cache mouse move event
                "var mouseMoveEvt = null;" + Environment.NewLine +

                // listeners for mouse actions
                "image.addEventListener('contextmenu', function(e){ rightclick(e); });" + Environment.NewLine +
                "image.addEventListener('mousedown', function(e){ mouseDown(e); });" + Environment.NewLine +
                "image.addEventListener('mouseup', function(e){ mouseUp(e); });" + Environment.NewLine +
                "image.addEventListener('load', setReloadTimeout );" + Environment.NewLine +
                "image.addEventListener('mousemove', function(e){ mouseMove(e); } );" + Environment.NewLine +

                // interval to ask server for a new screen image
                "setInterval('imageLoader();', 5555);" + Environment.NewLine +

                // interval for mouse move update
                "setInterval('mouseMoveUpdate();', 333);" + Environment.NewLine +

            // reload image
            "function mouseMoveUpdate () {" + Environment.NewLine +

                "if (mouseMoveEvt === null) return;" + Environment.NewLine +

                // update mouse position
                "e = mouseMoveEvt" + Environment.NewLine +
                "px = e.offsetX ? e.offsetX :e.pageX-document.getElementById(\"image\").offsetLeft;" + Environment.NewLine +
                "py = e.offsetY ? e.offsetY :e.pageY-document.getElementById(\"image\").offsetTop;" + Environment.NewLine +
                "var request = '/mousemove/" + screen + "/' + py + '/' + px;" + Environment.NewLine +
                "http.open('GET', request, true);" + Environment.NewLine +
                "http.send();" + Environment.NewLine +

            "}" + Environment.NewLine +
                
            "function setReloadTimeout () {" + Environment.NewLine +
                // interval to ask server for a new screen image
                "setTimeout('imageLoader();', 1000);" + Environment.NewLine +
            "}" + Environment.NewLine +

            // reload image
            "function imageLoader () {" + Environment.NewLine +
                " var newImageUrl = '/screen/' + new Date() / 1;" + Environment.NewLine +
                " document.getElementById(\"image\").src = newImageUrl; " + Environment.NewLine +
            "}" + Environment.NewLine +

            "function cancelUiEvts (e) {" + Environment.NewLine +
                "e.stopPropagation();" + Environment.NewLine +
                "e.preventDefault();" + Environment.NewLine +
            "}" + Environment.NewLine +

            "function mouseDown (e) {" + Environment.NewLine +
                "cancelUiEvts(e);" + Environment.NewLine +
                "mousedownEvt = e;" + Environment.NewLine +
            "}" + Environment.NewLine +

            "function mouseUp (e) {" + Environment.NewLine +
                "cancelUiEvts(e);" + Environment.NewLine +

                "dx = mousedownEvt.offsetX ? mousedownEvt.offsetX :mousedownEvt.pageX-document.getElementById(\"image\").offsetLeft;" + Environment.NewLine +
                "dy = mousedownEvt.offsetY ? mousedownEvt.offsetY :mousedownEvt.pageY-document.getElementById(\"image\").offsetTop;" + Environment.NewLine +

                "ux = e.offsetX ? e.offsetX :e.pageX-document.getElementById(\"image\").offsetLeft;" + Environment.NewLine +
                "uy = e.offsetY ? e.offsetY :e.pageY-document.getElementById(\"image\").offsetTop;" + Environment.NewLine +

                "var request = '/leftclick/" + screen + "/' + dy + '/' + dx + '/' + uy + '/' + ux;" + Environment.NewLine +
                "http.open('GET', request, true);" + Environment.NewLine +
                "http.send();" + Environment.NewLine +

                "mousedownEvt = null;" + Environment.NewLine +

            "}" + Environment.NewLine +

            "function rightclick(e) { " + Environment.NewLine +

                "cancelUiEvts(e);" + Environment.NewLine +

                "px = e.offsetX ? e.offsetX :e.pageX-document.getElementById(\"image\").offsetLeft;" + Environment.NewLine +
                "py = e.offsetY ? e.offsetY :e.pageY-document.getElementById(\"image\").offsetTop;" + Environment.NewLine +
                "var request = '/rightclick/" + screen + "/' + py + '/' + px;" + Environment.NewLine +
                "http.open('GET', request, true);" + Environment.NewLine +
                "http.send();" + Environment.NewLine +

            "}" + Environment.NewLine +

            "function mouseMove(e) { " + Environment.NewLine +

                "mouseMoveEvt = e;" + Environment.NewLine +

            "}");
        }
    }
}
