using System;
using System.Net;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class MouseMovePageHandler: AbstractClickPageHandler
    {
        // screen devices list
        private Screen[] screens = Screen.AllScreens;

        public override byte[] handleRequest(HttpListenerResponse response, string[] uri)
        {
            // must have 5 tokens
            if (uri.Length != 5)
            {
                response.StatusCode = 400;
                return buildHTML("Error...");
            }

            int x, y;
            try
            {
                y = Convert.ToInt16(uri[3]);
                x = Convert.ToInt16(uri[4]);
            }
            catch
            {
                // parameter error
                response.StatusCode = 400;
                return buildHTML("Error...");
            }

            int screen = getRequestedScreenDevice(uri, screens);

            // check bounds
            Screen device = screens[screen];
            if (x < 0 || x >= device.Bounds.Width || y < 0 || y >= device.Bounds.Height)
            {
                response.StatusCode = 400;
                return buildHTML("Error...");
            }

            MoveMouse(x, y);

            return buildHTML("Updating...");
        }
    }
}
