using System;
using System.Net;
using System.Text;

namespace RemoteView.PageHandlers
{
    class NotFoundPageHandler:PageHandler
    {
        public override byte[] handleRequest(HttpListenerResponse response, String[] uri)
        {
            return Encoding.UTF8.GetBytes("<html><body>Page not found!</body></html>");
        }
    }
}
