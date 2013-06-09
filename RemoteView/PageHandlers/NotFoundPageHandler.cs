using System;
using System.Net;

namespace RemoteView.PageHandlers
{
    class NotFoundPageHandler : PageHandler
    {
        public override byte[] handleRequest(HttpListenerResponse response, String[] uri)
        {
            return buildHTML("Page not found!");
        }
    }
}
