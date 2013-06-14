
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class IconPageHandler : PageHandler
    {
        byte[] buffer;

        public IconPageHandler()
        {
            using (MemoryStream icon = new MemoryStream())
            {
                Icon bitmap = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                bitmap.Save(icon);
                buffer = icon.GetBuffer();
            }
        }

        public override byte[] handleRequest(HttpListenerResponse response, string[] uri)
        {
            return buffer;
        }
    }
}
