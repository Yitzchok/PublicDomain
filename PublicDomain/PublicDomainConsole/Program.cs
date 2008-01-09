using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain;
using System.Threading;
using PublicDomain.Logging;
using PublicDomain.Data;

namespace PublicDomainConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new LoggingConfig("", new LoggingConfig.CallbackCreateLogger(delegate(string category, LoggerSeverity threshold)
            {
                Logger result = new SimpleCompositeLogger(new ConsoleLogger(), typeof(Program).FullName);
                result.Threshold = threshold;
                result.Category = category;
                return result;
            }), null);

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"));

            Logger log = LoggingConfig.Current.CreateLogger(typeof(Program), "blah");

            log.LogDebug10("this should not show");

            LoggingConfig.Current.Load(typeof(Program).ToString());

            log.LogDebug10("this should show");

            Console.Write("Done: ");
            Console.ReadKey(true);
        }
    }
}
