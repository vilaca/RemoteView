using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace RemoteView
{
    class Program
    {
        static Mutex mutex = new Mutex(true, "-RemoteView-Mutex-");

        private static string ApplicationName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            }
        }

        static void Main(string[] args)
        {

            // get configuration from command line parameters

            Configuration conf;
            try
            {
                conf = Configuration.create(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            if (conf.Banner)
            {
                ShowBanner();
            }

            if (conf.Help)
            {
                ShowHelpMessage();
                return;
            }

            // make sure only one instance is online

            if (!conf.AllowMultiple && !InstanceIsUnique())
            {
                Console.WriteLine("Only one instance of process allowed. User -m for muliple instances.");
                return;
            }

            // check if http listener is supported

            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            // run server
            RunServer(conf);
        }

        /// <summary>
        /// Find out if there are one or more instances of this program running
        /// </summary>
        /// <returns>n processes</returns>
        private static bool InstanceIsUnique()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                mutex.ReleaseMutex();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Run server
        /// </summary>
        /// <param name="conf">server configuration</param>
        private static void RunServer(Configuration conf)
        {
            using (Server server = new Server().Start(conf.Port))
            {
                if (!server.IsRunning())
                {
                    Console.WriteLine("Could not start server... Exiting.");
                    return;
                }

                Console.WriteLine("Server running press [c] to stop");
                while (server.IsRunning() && Console.ReadKey(true).Key != ConsoleKey.C) ;
                server.Stop();
            }
        }

        private static void ShowBanner()
        {
            Console.WriteLine(Application.ProductName + " - Desktop sharing server");
            Console.WriteLine("Copyright (c) Joao Vilaca, 2013, Email: jvilaca@gmail.com");
            Console.WriteLine();
        }

        private static void ShowHelpMessage()
        {
            Console.WriteLine("Syntax: " + ApplicationName + " [Port to listen] [Options]");
            Console.WriteLine("Example: " + ApplicationName + " 6060 -b");
            Console.WriteLine("Options: -b :\tDon't show banner message;");
            Console.WriteLine("         -m :\tAllow multiple instances;");
            Console.WriteLine("         -h :\tHelp (This screen);");
            //            Console.WriteLine("\t-i :\tInstall as Windows service");
            //            Console.WriteLine("\t-u :\tUninstall as Windows service");
        }
    }
}