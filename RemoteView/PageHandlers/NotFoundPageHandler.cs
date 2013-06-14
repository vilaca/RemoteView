using System;
using System.Net;

namespace RemoteView.PageHandlers
{
    class NotFoundPageHandler : AbstractPageHandler
    {
        public override byte[] handleRequest(HttpListenerResponse response, String[] uri)
        {
            response.StatusCode = 404;
            return buildHTML("Page not found!");
        }
    }
}
