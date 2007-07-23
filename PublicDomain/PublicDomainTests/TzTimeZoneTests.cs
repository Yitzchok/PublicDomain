using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Globalization;

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
            // Try getting the US eastern time zone
            TzTimeZone easternTimeZone = TzTimeZone.GetTimeZone(TzConstants.TimezoneUsEastern);

            // Make sure some things are there
            Assert.IsNotNull(easternTimeZone);
            Assert.Greater(TzTimeZone.ZoneList.Count, 0);
            Assert.Greater(TzTimeZone.Zones.Count, 0);
            Assert.AreEqual(TzTimeZone.Zones.Count, TzTimeZone.ZoneList.Count);
            Assert.AreEqual(easternTimeZone.DaylightName, TzConstants.TimezoneUsEastern);
            Assert.AreEqual(easternTimeZone.StandardName, TzConstants.TimezoneUsEastern);

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
                TzDatabase.TzZone infinityZone = null;
                foreach (TzDatabase.TzZone zoneInfo in zone.Zones)
                {
                    if (zoneInfo.UntilYear == int.MaxValue)
                    {
                        if (infinityZone != null)
                        {
                            throw new TzDatabase.TzException("Multiple ZONE information lines with infinity end dates ({0}, {1})", infinityZone, zoneInfo);
                        }
                        infinityZone = zoneInfo;
                    }
                }
                if (infinityZone == null)
                {
                    throw new TzDatabase.TzException("No zone goes to infinity for {0}", zone.ZoneName);
                }
            }
        }

        [Test]
        public void TestRules()
        {
            TzTimeZone eastern = TzTimeZone.ZoneUsEastern;
            Console.WriteLine(eastern.StandardName);
            Console.WriteLine(eastern.HistoricalData.ZoneName);

            Console.WriteLine(GlobalConstants.DividerEquals);
            foreach (TzDatabase.TzRule rule in eastern.HistoricalData.Rules)
            {
                Console.WriteLine(rule.ToString());
                Console.WriteLine(rule.GetFromDateTime());
                Console.WriteLine();
            }

            Console.WriteLine(GlobalConstants.DividerEquals);
            foreach (TzDatabase.TzZone zone in eastern.HistoricalData.Zones)
            {
                Console.WriteLine(zone.GetObjectString());
                Console.WriteLine(zone.GetUntilDateTime());
                Console.WriteLine();
            }

            Console.WriteLine(GlobalConstants.DividerEquals);
        }

        [Test]
        public void UseCase1()
        {
            // Emulate discovering the offset of a UTC value
            DateTime dt = DateTime.UtcNow;
            TzTimeZone timeZone = TzTimeZone.ZoneUsEastern;
            PublicDomain.TzDatabase.TzZone zone = timeZone.FindZone(dt);
            dt = zone.GetLocalTime(dt);
            PublicDomain.TzDatabase.TzRule rule = timeZone.FindRule(zone, dt);
            if (rule != null)
            {
                dt += rule.SaveTime;
            }
            Console.WriteLine(dt);
        }

        [Test]
        public void TestCurrentTimeZone()
        {
            Console.WriteLine(TzTimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now));
            Console.WriteLine(TzTimeZone.CurrentTimeZone.ToLocalTime(DateTime.Now));
            Console.WriteLine(TzTimeZone.CurrentTimeZone.ToLocalTime(DateTime.UtcNow));
            Console.WriteLine(TzTimeZone.CurrentTimeZone.ToUniversalTime(DateTime.Now));
            Console.WriteLine(TzTimeZone.CurrentTimeZone.ToUniversalTime(DateTime.UtcNow));
            Console.WriteLine(TzTimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now));
            Console.WriteLine(TzTimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.UtcNow));
            Console.WriteLine(TzTimeZone.CurrentTimeZone.GetAbbreviation(DateTime.Now));
            Console.WriteLine(TzTimeZone.CurrentTimeZone.GetAbbreviation(DateTime.UtcNow));
        }

        [Test]
        public void TestAbbreviations()
        {
            foreach (string zName in TzTimeZone.AllZoneNames)
            {
                TzTimeZone zone = TzTimeZone.GetTimeZone(zName);
                Console.WriteLine(zone.GetAbbreviation());
            }
        }

        [Test]
        public void Simple()
        {
            Console.WriteLine(TzTimeZone.CurrentTimeZone.Now.DateTimeLocal);
        }

        [Test]
        public void DaylightChanges()
        {
            foreach (string zName in TzTimeZone.AllZoneNames)
            {
                TzTimeZone zone = TzTimeZone.GetTimeZone(zName);
                DaylightTime daylightTime = zone.GetDaylightChanges(DateTime.Now.Year);
                if (daylightTime != null)
                {
                    Console.WriteLine(daylightTime.Start);
                    Console.WriteLine(daylightTime.End);
                }
            }
        }
    }
}
