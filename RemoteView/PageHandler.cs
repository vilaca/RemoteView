
using System;
using System.Net;
namespace RemoteView
{
    abstract class PageHandler
    {
        public abstract byte[] getRequest(HttpListenerResponse response, String[] uri);

    }
}
