using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class ScreenPageHandler : AbstractPageHandler
    {
        private Screen[] screens = Screen.AllScreens;

        public override byte[] handleRequest(HttpListenerResponse response, String[] uri)
        {
            response.Headers.Set("Content-Type", "image/png");

            Screen screen;
            if (uri.Length > 2)
            {
                try
                {
                    int n = Convert.ToInt16(uri[2]);
                    screen = screens[n];
                }
                catch
                {
                    screen = screens[0];
                }
            }
            else
            {
                screen = screens[0];
            }

            using (MemoryStream ms = new MemoryStream())
            {                
                Bitmap bmp = new Bitmap(screen.Bounds.Width, screen.Bounds.Height);
                Graphics.FromImage(bmp).CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, screen.Bounds.Size);
                bmp.Save(ms, ImageFormat.Png);
                return ms.GetBuffer();
            }
        }
    }
}
