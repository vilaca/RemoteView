using System;
using System.Net;

namespace RemoteView.PageHandlers
{
    class NotFoundPageHandler : AbstractPageHandler
    {
        /// <summary>
        /// 404 error page, used when client requests and unexisting resource
        /// </summary>
        /// <param name="response"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public override byte[] HandleRequest(HttpListenerResponse response, String[] uri)
        {
            response.StatusCode = 404;
            return BuildHTML("Page not found!");
        }
    }
}
