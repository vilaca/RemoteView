
using System;
using System.Net;
namespace RemoteView
{
    abstract class PageHandler
    {
        public abstract byte[] handleRequest(HttpListenerResponse response, String[] uri);

    }
}
