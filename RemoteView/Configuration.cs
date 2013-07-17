
using System;
using System.Collections;
using System.Globalization;

namespace RemoteView
{
    class Configuration
    {
        /// <summary>
        /// Allow multiple instances of process
        /// </summary>
        public bool AllowMultiple { get; private set; }

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
            this.AllowMultiple = false;
            this.Banner = true;
            this.Help = false;
            this.Port = 6060;
        }

        public static Configuration create(string[] parameters)
        {
            Configuration conf = new Configuration();

            // use default configuration if no parameters exist

            if (parameters.Length == 0) return conf;

            // cycle throught command line using enumerator on parameters array

            IEnumerator enumerator = parameters.GetEnumerator();
            enumerator.MoveNext();

            string parameter = (string)enumerator.Current;

            // parse if first parameter is a valid integer and use it as a port number for listener

            int port;
            bool hasPortParameter = int.TryParse(parameter, out port);

            if (hasPortParameter)
            {
                conf.Port = port;

                // continue parsing parameters (if they exist)
                if (!enumerator.MoveNext()) return conf;
                parameter = (string)enumerator.Current;
            }

            do
            {
                if (parameter.Equals("-m"))
                {
                    conf.AllowMultiple = true;
                }
                else if (parameter.Equals("-b"))
                {
                    conf.Banner = false;
                }
                else if (parameter.Equals("-h"))
                {
                    conf.Help = true;
                }
                else
                {
                    throw new ArgumentException(string.Format("Error: {0} is an invalid command line parameter.", parameter));
                }

                if (!enumerator.MoveNext()) break;

                parameter = (string)enumerator.Current;
            }
            while (true);

            return conf;
        }
    }
}