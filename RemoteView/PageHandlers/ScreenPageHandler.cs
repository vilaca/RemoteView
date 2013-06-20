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
        private volatile byte[][] caches = new byte[Screen.AllScreens.Length][];
        volatile int lastScreenRequested = 0;
        private System.Timers.Timer timer;

        public ScreenPageHandler()
        {
            for (int i = 0; i < caches.Length; i++)
            {
                caches[i] = serializeScreenImage(i);
            }

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int toReload = lastScreenRequested;
            caches[toReload] = serializeScreenImage(toReload);
        }

        /// <summary>
        /// Get a copy of the requested device screen image from cache
        /// </summary>
        /// <param name="response"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public override byte[] handleRequest(HttpListenerResponse response, String[] uri)
        {
            int requested = getRequestedScreenDevice(uri, screens);
            lastScreenRequested = requested;
            response.Headers.Set("Content-Type", "image/png");
            return caches[requested];
        }

        /// <summary>
        /// Serizalize a screen
        /// </summary>
        /// <param name="requested">screen to serialize</param>
        /// <returns></returns>
        private byte[] serializeScreenImage(int requested)
        {
            Screen screen = screens[requested];
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