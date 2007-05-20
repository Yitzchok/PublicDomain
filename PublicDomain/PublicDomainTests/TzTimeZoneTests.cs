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
    public class TzTimeZoneTests
    {
        /// <summary>
        /// Prints the time zones.
        /// </summary>
        [Test]
        public void PrintTimeZones()
        {
            foreach (string timeZone in TzTimeZone.Zones.Keys)
            {
                Console.WriteLine(timeZone);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestTimeZones()
        {
            // Try getting the US easter time zone
            TzTimeZone easternTimeZone = TzTimeZone.GetTimeZone(PublicDomain.TzTimeZone.TzConstants.TimezoneUsEastern);

            // Make sure some things are there
            Assert.IsNotNull(easternTimeZone);
            Assert.Greater(TzTimeZone.ZoneList.Count, 0);
            Assert.Greater(TzTimeZone.Zones.Count, 0);
            Assert.AreEqual(TzTimeZone.Zones.Count, TzTimeZone.ZoneList.Count);
            Assert.AreEqual(easternTimeZone.DaylightName, PublicDomain.TzTimeZone.TzConstants.TimezoneUsEastern);
            Assert.AreEqual(easternTimeZone.StandardName, PublicDomain.TzTimeZone.TzConstants.TimezoneUsEastern);

            // Create a test time
            DateTime test = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local);

            // Get the UTC offset
            TimeSpan testUtcOffset = easternTimeZone.GetUtcOffset(test);

            TimeSpan minusFive = new TimeSpan(-5, 0, 0);

            // We expect the offset to be -5
            Assert.AreEqual(testUtcOffset, minusFive);

            Console.WriteLine(testUtcOffset);

            Console.WriteLine("Now: " + DateTime.Now);
            Console.WriteLine("Now (Local): " + easternTimeZone.ToLocalTime(DateTime.Now));
            Console.WriteLine("Now (UTC): " + easternTimeZone.ToUniversalTime(DateTime.Now));

            // Now, check the actual offset
            //DaylightTime daylightTime = easternTimeZone.GetDaylightChanges(DateTime.Now.Year);

            // Create a local DateTime
            //DateTime test = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local);
            //easternTimeZone.ToUniversalTime(test);
        }

        /// <summary>
        /// Make sure that all <see cref="PublicDomain.TzDatabase.TzZone"/> objects
        /// in the zone lists have a unique time, and that only one goes to infinity.
        /// </summary>
        [Test]
        public void TestUniqueTimeZoneInfo()
        {
            foreach (PublicDomain.TzTimeZone.TzZoneInfo zone in TzTimeZone.ZoneList)
            {
                bool containsInfinity = false;
                foreach (TzDatabase.TzZone zoneInfo in zone.Zones)
                {
                    if (zoneInfo.UntilYear == int.MaxValue)
                    {
                        if (containsInfinity)
                        {
                            throw new TzDatabase.TzException("Multiple ZONE information lines with infinity end dates");
                        }
                        containsInfinity = true;
                    }
                }
                if (!containsInfinity)
                {
                    throw new TzDatabase.TzException("No zone goes to infinity for " + zone.ZoneName);
                }
            }
        }
    }
}
