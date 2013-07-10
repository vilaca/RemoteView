
using System;

namespace RemoteView
{
    class Configuration
    {
        /// <summary>
        /// Display banner
        /// </summary>
        public bool Banner { get; private set; }

        /// <summary>
        /// Display help
        /// </summary>
        public bool Help { get; private set; }

        /// <summary>
        /// Port where to listen
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Private c'tor with default values for object instances
        /// Use factory method instead of instatiating c'tor
        /// </summary>
        private Configuration()
        {
            this.Banner = true;
            this.Help = false;
            this.Port = 6060;
        }

        public static Configuration CreateConfiguration(string[] args)
        {
            Configuration conf = new Configuration();

            // parse command line params

            foreach (string arg in args)
            {
                if (arg.Equals("-b"))
                {
                    conf.Banner = false;
                }
                else if (arg.Equals("-h"))
                {
                    conf.Help = true;
                }
                else if (arg.StartsWith("-p"))
                {
                    try
                    {
                        conf.Port = int.Parse(arg);
                    }
                    catch
                    {
                        throw new ArgumentException(string.Format("Error: {0} is not a valid port number.", arg));
                    }
                }
                else
                {
                    throw new ArgumentException(string.Format("Error: {0} is an invalid command line parameter.", arg));
                }
            }

            return conf;
        }
    }
}