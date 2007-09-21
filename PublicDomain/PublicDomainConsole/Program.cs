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
            Console.WriteLine(PublicDomain.TzTimeZone.GetTimeZone("America/Phoenix").GetAbbreviation());
            Console.ReadKey(true);
        }
    }
}
