using System.Net;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class InfoPageHandler : AbstractPageHandler
    {
        private Screen[] screens = Screen.AllScreens;

        public override byte[] handleRequest(HttpListenerResponse response, string[] uri)
        {
            string page = "<p><b>Desktop Viewer - Desktop sharing Application</b></p>";

            for (int i = 0; i < screens.Length; i++)
            {
                page += "<p><b>Screen " + i + ":</b> '" + screens[i].DeviceName + "', Width:" + screens[i].Bounds.Width + ", Height:" + screens[i].Bounds.Height + "</p>";
            }

            return buildHTML(page);
        }
    }
}
