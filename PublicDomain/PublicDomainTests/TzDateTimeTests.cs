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

        [Test]
        public void BasicUsage()
        {
            // Get the current time zone
            TzTimeZone currentTimeZone = TzTimeZone.CurrentTimeZone;
            Console.WriteLine(currentTimeZone.StandardName);
            Console.WriteLine(currentTimeZone.GetUtcOffset(DateTime.Now));
            Console.WriteLine(currentTimeZone.Now.DateTimeLocal);

            TzTimeZone easternTz = TzTimeZone.GetTimeZone(TzConstants.TimezoneUsEastern);
            TzDateTime now = easternTz.Now;
            DateTime localDateTime = now.DateTimeLocal;
            DateTime utcDateTime = now.DateTimeUtc;
            Console.WriteLine(localDateTime);
            Console.WriteLine(utcDateTime);
        }

        [Test]
        public void SimpleTest()
        {
            TzTimeZone timeZone = TzTimeZone.ZoneUsEastern;
            Console.WriteLine(timeZone.StandardName);
            TzDateTime dt = TzDateTime.UtcNow(timeZone);
            DateTime local = dt.DateTimeLocal;
            Console.WriteLine(local);
        }

        [Test]
        public void DateTimeInteroperability()
        {
            // Create a UTC DateTime
            DateTime dtUtc = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            // Create a UTC TzDateTime with the current time zone
            TzDateTime zdtUtc = new TzDateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc, TzTimeZone.CurrentTimeZone);

            Console.WriteLine(dtUtc);
            Console.WriteLine(zdtUtc);

            AssertUtcSame(dtUtc, zdtUtc);

            // Do some arithmetic operations
            DateTime test = dtUtc.Add(TimeSpan.FromHours(24));
            //TzDateTime ztest = zdtUtc.Add(TimeSpan.FromHours(24));
        }

        private static void AssertUtcSame(DateTime dtUtc, TzDateTime zdtUtc)
        {
            Assert.AreEqual(dtUtc, zdtUtc.DateTimeUtc);
            Assert.AreNotEqual(dtUtc, zdtUtc.DateTimeLocal);
        }

        [Test]
        public void TestEquals()
        {
            TzDateTime d1 = TzDateTime.Now(null), d2 = TzDateTime.Now(null);
            Console.WriteLine(d1 == d2);

            d1 = d2 = null;
            Console.WriteLine(d1 == d2);
        }
    }
}
