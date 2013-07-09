using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace RemoteView
{
    class Program
    {
        static void Main(string[] args)
        {

            // make sure only one instance is online

            Process[] RunningProcesses = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Application.ExecutablePath));
            if (RunningProcesses.Length != 1)
            {
                return;
            }

            // get admin level

            if (!IsRunningAsAdministrator())
            {
                ProcessStartInfo processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";
                processInfo.Arguments = String.Join(" ", args);

                try
                {
                    Process.Start(processInfo);
                }
                catch (Exception)
                {
                    Console.WriteLine("Needs administrator rights.");
                }
                return;
            }

            // check if http listener is supported

            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            Configuration conf;
            try
            {
                conf = Configuration.CreateConfiguration(args);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            if (conf.Banner)
            {
                showBanner();
            }

            if (conf.Help)
            {
                showHelpMessage();
                return;
            }

            // run server
            RunServer(conf);
        }

        /// <summary>
        /// Run server
        /// </summary>
        /// <param name="conf">server configuration</param>
        private static void RunServer(Configuration conf)
        {
            Server server = new Server();

            new Thread(() =>
            {
                server.start(conf.Port);
            }).Start();

            // works!
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
            Console.WriteLine(Application.ProductName + " - Desktop sharing server" + Environment.NewLine);
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

        private static bool IsRunningAsAdministrator()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}