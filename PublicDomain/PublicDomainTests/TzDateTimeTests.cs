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
    public class TzDateTimeTests
    {
        /// <summary>
        /// Test1s this instance.
        /// </summary>
        [Test]
        public void Test1()
        {
            foreach (string test in new string[] {
                    "06/20/1984 3:00:00 PM-05:00",
                    "1/1/2007+00:00",
                })
            {
                TzDateTime dt = TzDateTime.Parse(test);
                Console.WriteLine(dt + "," + dt.ToStringLocal());
            }
        }
    }
}
