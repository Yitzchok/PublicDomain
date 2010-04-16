using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ConsoleLogger : TextWriterLogger
    {
        private static ConsoleLogger s_current;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
        /// </summary>
        public ConsoleLogger()
            : base(Console.Out)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public static ConsoleLogger Current
        {
            get
            {
                if (s_current == null)
                {
                    s_current = new ConsoleLogger();
                }
                return s_current;
            }
        }
    }
}
