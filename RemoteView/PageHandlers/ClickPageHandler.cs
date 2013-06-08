
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class ClickPageHandler : PageHandler
    {
        private Screen[] screens = Screen.AllScreens;

        public override byte[] handleRequest(HttpListenerResponse response, string[] uri)
        {
            int screen = getRequestedScreenDevice(uri, screens);

            response.Redirect("/home/"+screen);

            return Encoding.UTF8.GetBytes("<html><body>Redirecting...</body></html>");
        }
    }
}
