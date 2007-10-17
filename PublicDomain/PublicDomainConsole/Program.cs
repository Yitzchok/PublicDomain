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
            Console.WriteLine(Win32.GetSystemTime());
            Console.WriteLine(Win32.GetSystemTimeTz().ToString());
            Console.WriteLine(Win32.GetLocalTime());
            Console.WriteLine(Win32.GetLocalTimeTz().ToStringLocal());
            Console.ReadKey(true);
        }
    }
}
