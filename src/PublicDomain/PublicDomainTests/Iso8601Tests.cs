using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class Iso8601Tests
    {
        /// <summary>
        /// Test1s this instance.
        /// </summary>
        [Test]
        public void Test1()
        {
            foreach (string test in new string[] {
                    "2007", "2007-02", "2007-02-15",
                    "2007-02-15T14:57Z",
                    "2007-02-15T14:57-05:00",
                    "2007-02-15T14:57+03:00",
                    "2007-02-15T14:57:30Z",
                    "2007-02-15T14:57:30-08:00",
                    "2007-02-15T14:57:30.983Z",
                })
            {
                TzDateTime dt = Iso8601.Parse(test, TzTimeZone.ZoneUsEastern);
                Console.WriteLine(dt + "," + dt.ToStringLocal());
            }
        }
    }
}
