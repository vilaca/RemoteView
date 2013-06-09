
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace RemoteView.PageHandlers
{
    class IconPageHandler : PageHandler
    {
        public override byte[] handleRequest(HttpListenerResponse response, string[] uri)
        {
            using (MemoryStream icon = new MemoryStream())
            {
                Icon bitmap = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                bitmap.Save(icon);
                return icon.GetBuffer();
            }
        }
    }
}
