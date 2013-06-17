using System;
using System.Net;
using System.Reflection;
using System.Threading;
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
                if (arg.Equals("-b"))
                {
                    banner = false;
                }
                else if (arg.Equals("-h"))
                {
                    help = true;
                }
                else if (arg.StartsWith("-p"))
                {
                    int n;
                    if (int.TryParse(arg.Substring(2), out n))
                    {
                        port = n;
                    }
                    else
                    {
                        error = true;
                    }
                }
                else
                {
                    error = true;
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
            Server server = new Server();

            new Thread(() =>
            {
                server.start(port);
            }).Start();

            Thread.Sleep(2000);

            if (!server.isRunning())
            {
                Console.WriteLine("Could not start server... Exiting.");
                return;
            }

            Console.WriteLine("Server running press [c] to stop");
            while (server.isRunning() && Console.ReadKey(true).Key != ConsoleKey.C) ;
            server.stop();
        }

        private static void showBanner()
        {
            Console.WriteLine(Application.ProductName + " - Desktop sharing server" + Environment.NewLine );
        }

        private static void showHelpMessage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("RemoteView [-pN] [-b] [-h]");
            Console.WriteLine("\t-p :\tListen on port N;");
            Console.WriteLine("\t-b :\tDon't show banner message;");
            Console.WriteLine("\t-h :\tThis screen;");
            //            Console.WriteLine("\t-i :\tInstall as Windows service");
            //            Console.WriteLine("\t-u :\tUninstall as Windows service");
        }
    }
}