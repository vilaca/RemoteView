using System;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace RemoteView
{
    class Program
    {
        static void Main(string[] args)
        {
            bool banner = true, help = false, error = false;
            int port = 6060;

            // check if http listener is supported

            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            // parse command line params

            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "-b":
                        banner = false;
                        break;
                    case "-h":
                    case "-help":
                        help = true;
                        break;
                    default:
                        if (arg.StartsWith("-p"))
                        {
                            try
                            {
                                port = Convert.ToInt16(arg.Substring(2));
                            }
                            catch
                            {
                                error = true;
                            }
                        }
                        else
                        {
                            error = true;
                        }
                        break;
                }
            }

            if (banner)
            {
                showBanner();
            }

            if (help)
            {
                showHelpMessage();
                return;
            }

            if (error)
            {
                Console.WriteLine("Error in Parameter.");
                return;
            }

            // run server
            new Server().start(port);
        }

        private static void showBanner()
        {
            Console.WriteLine(Application.ProductName + " - Desktop sharing server");
        }

        private static void showHelpMessage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("\t-b :\tDon't show banner message;");
            Console.WriteLine("\t-h :\tThis screen;");
            Console.WriteLine("\t-pXXXX :\tListen in port XXXX.");
            //            Console.WriteLine("\t-i :\tInstall as Windows service");
            //            Console.WriteLine("\t-u :\tUninstall as Windows service");
            Console.WriteLine("Usage:");
        }
    }
}