
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class IconPageHandler : AbstractPageHandler
    {
        /// <summary>
        /// cached copy of favicon
        /// </summary>
        byte[] buffer;

        /// <summary>
        /// c'tor, creates chached copy of favicon
        /// </summary>
        public IconPageHandler()
        {
            using (MemoryStream icon = new MemoryStream())
            {
                Icon bitmap = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                bitmap.Save(icon);
                buffer = icon.GetBuffer();
            }
        }

        /// <summary>
        /// not much to do here, just return the cached favicon
        /// </summary>
        /// <param name="response"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public override byte[] handleRequest(HttpListenerResponse response, string[] uri)
        {
            return buffer;
        }
    }
}
