using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain;
using System.Threading;
using PublicDomain.Logging;

namespace PublicDomainConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new LoggerTests().TestCriticalLogger();
        }
    }
}
