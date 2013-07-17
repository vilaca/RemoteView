using System;
using System.Globalization;
using System.Net;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class LeftClickPageHandler : AbstractPageHandler
    {
        // screen devices list
        private Screen[] screens = Screen.AllScreens;

        /// <summary>
        /// handle left clicks from client
        /// </summary>
        /// <param name="response"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public override byte[] HandleRequest(HttpListenerResponse response, string[] uri)
        {
            // must have 5 tokens
            if (uri.Length != 7)
            {
                response.StatusCode = 400;
                return BuildHTML("Error...");
            }

            int screen = GetRequestedScreenDevice(uri, screens);

            bool error = handleMouseDown(uri, screen);
            if (error)
            {
                // parameter error
                response.StatusCode = 400;
                return BuildHTML("Error...");
            }

            error = handleMouseUp(uri, screen);
            if (error)
            {
                // parameter error
                response.StatusCode = 400;
                return BuildHTML("Error...");
            }

            return BuildHTML("Updating...");
        }

        private bool handleMouseDown(string[] uri, int screen)
        {
            int x, y;
            try
            {
                y = Convert.ToInt16(uri[3], CultureInfo.InvariantCulture);
                x = Convert.ToInt16(uri[4], CultureInfo.InvariantCulture);
            }
            catch
            {
                return true;
            }

            // check bounds
            Screen device = screens[screen];
            if (x < 0 || x >= device.Bounds.Width || y < 0 || y >= device.Bounds.Height)
            {
                return true;
            }


            // adapt to real screen bounds

            x = device.Bounds.X + x;
            y = device.Bounds.X + y;

            NativeMethods.LeftMouseButton(NativeMethods.MouseEventFlags.MOUSEEVENTF_LEFTDOWN, x, y);

            return false;
        }

        private bool handleMouseUp(string[] uri, int screen)
        {
            int x, y;
            try
            {
                y = Convert.ToInt16(uri[5], CultureInfo.InvariantCulture);
                x = Convert.ToInt16(uri[6], CultureInfo.InvariantCulture);
            }
            catch
            {
                return true;
            }

            // check bounds
            Screen device = screens[screen];
            if (x < 0 || x >= device.Bounds.Width || y < 0 || y >= device.Bounds.Height)
            {
                return true;
            }

            // adapt to real screen bounds

            x = device.Bounds.X + x;
            y = device.Bounds.X + y;

            NativeMethods.LeftMouseButton(NativeMethods.MouseEventFlags.MOUSEEVENTF_LEFTUP, x, y);

            return false;
        }
    }
}