using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class ScreenPageHandler : AbstractPageHandler
    {
        private volatile byte[][] caches = new byte[Screen.AllScreens.Length][];
        private volatile int lastScreenRequested = 0;
        private Screen[] screens = Screen.AllScreens;
        private System.Timers.Timer timer;

        /// <summary>
        /// C'tor init cache for all screens
        /// </summary>
        public ScreenPageHandler()
        {
            for (int i = 0; i < caches.Length; i++)
            {
                caches[i] = SerializeScreenImage(i);
            }

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += reloadCache;
            timer.Enabled = true;
        }

        /// <summary>
        /// reload image for last requested screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reloadCache(object sender, System.Timers.ElapsedEventArgs e)
        {
            int toReload = lastScreenRequested;
#if DEBUG
            Stopwatch perfCounter = new Stopwatch();
            perfCounter.Start();
#endif
            caches[toReload] = SerializeScreenImage(toReload);
#if DEBUG
            perfCounter.Stop();
            Console.WriteLine("Time elapsed: {0}", perfCounter.Elapsed);
#endif
        }

        /// <summary>
        /// Get a copy of the requested device screen image from cache
        /// </summary>
        /// <param name="response"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public override byte[] HandleRequest(HttpListenerResponse response, String[] uri)
        {
            int requested = GetRequestedScreenDevice(uri, screens);
            lastScreenRequested = requested;
            response.Headers.Set("Content-Type", "image/png");
            return caches[requested];
        }

        /// <summary>
        /// Serizalize a screen
        /// </summary>
        /// <param name="requested">screen to serialize</param>
        /// <returns></returns>
        private byte[] SerializeScreenImage(int requested)
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