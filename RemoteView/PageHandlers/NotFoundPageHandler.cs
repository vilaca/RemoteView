using System.Text;

namespace RemoteView.PageHandlers
{
    class NotFoundPageHandler:PageHandler
    {
        public override byte[] getRequest()
        {
            return Encoding.UTF8.GetBytes("<html><body>Page not found!</body></html>");
        }
    }
}
