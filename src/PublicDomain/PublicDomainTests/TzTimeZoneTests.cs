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
        private static DateTime startYear = new DateTime(2007, 1, 1);
        private static DateTime middleYear = new DateTime(2007, 6, 1);

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
                Console.WriteLine(rule.GetFromDateTime(TimeSpan.Zero));
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

        [Test]
        public void Bug12541()
        {
            TzTimeZone timeZone = TzTimeZone.GetTimeZone("America/Denver");
            Assert.AreEqual("MST", timeZone.GetAbbreviation(DateTime.Parse("2008-1-1 12:00:00")));
            Assert.AreEqual("MDT", timeZone.GetAbbreviation(DateTime.Parse("2008-5-1 12:00:00")));
            Assert.AreEqual("MST", timeZone.GetAbbreviation(DateTime.Parse("2008-12-1 12:00:00")));
        }

        [Test]
        public void Bug12480()
        {
            TzTimeZone zone = TzTimeZone.GetTimeZone("America/Chicago");

            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2006-04-02 01:00")));
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2006-04-02 01:00")).Hour);
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2006-04-02 01:30")));
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2006-04-02 01:30")).Hour);
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2006-04-02 02:00")));
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2006-04-02 02:00")).Hour);
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2006-04-02 02:30")));
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2006-04-02 02:30")).Hour);

            Assert.AreEqual(19, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 01:30"), DateTimeKind.Utc)).Hour);
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 01:30"), DateTimeKind.Utc)));
            Assert.AreEqual(20, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 02:00"), DateTimeKind.Utc)).Hour);
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 02:00"), DateTimeKind.Utc)));
            Assert.AreEqual(20, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 02:30"), DateTimeKind.Utc)).Hour);
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 02:30"), DateTimeKind.Utc)));
            Assert.AreEqual(21, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 03:00"), DateTimeKind.Utc)).Hour);
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 03:00"), DateTimeKind.Utc)));
            Assert.AreEqual(1, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 07:00"), DateTimeKind.Utc)).Hour);
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 07:00"), DateTimeKind.Utc)));
            Assert.AreEqual(1, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 07:30"), DateTimeKind.Utc)).Hour);
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 07:30"), DateTimeKind.Utc)));
            Assert.AreEqual(3, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 08:00"), DateTimeKind.Utc)).Hour);
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 08:00"), DateTimeKind.Utc)));
            Assert.AreEqual(3, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 08:30"), DateTimeKind.Utc)).Hour);
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2006-04-02 08:30"), DateTimeKind.Utc)));

            zone = TzTimeZone.GetTimeZone(TzConstants.TimezoneEuropeParis);
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-03-25 00:00")));
            Assert.AreEqual(23, zone.ToUniversalTime(DateTime.Parse("2007-03-25 00:00")).Hour);
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-03-25 00:30")));
            Assert.AreEqual(23, zone.ToUniversalTime(DateTime.Parse("2007-03-25 00:30")).Hour);
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-03-25 01:00")));
            Assert.AreEqual(0, zone.ToUniversalTime(DateTime.Parse("2007-03-25 01:00")).Hour);
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-03-25 01:30")));
            Assert.AreEqual(0, zone.ToUniversalTime(DateTime.Parse("2007-03-25 01:30")).Hour);
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-03-25 02:00")));
            Assert.AreEqual(0, zone.ToUniversalTime(DateTime.Parse("2007-03-25 02:00")).Hour);

            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-03-25 00:00"), DateTimeKind.Utc)));
            Assert.AreEqual(1, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-03-25 00:00"), DateTimeKind.Utc)).Hour);
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-03-25 00:30"), DateTimeKind.Utc)));
            Assert.AreEqual(1, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-03-25 00:30"), DateTimeKind.Utc)).Hour);
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-03-25 01:00"), DateTimeKind.Utc)));
            Assert.AreEqual(3, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-03-25 01:00"), DateTimeKind.Utc)).Hour);
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-03-25 01:30"), DateTimeKind.Utc)));
            Assert.AreEqual(3, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-03-25 01:30"), DateTimeKind.Utc)).Hour);

            zone = TzTimeZone.GetTimeZone(TzConstants.TimezoneEuropeMoscow);
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-03-25 01:00")));
            Assert.AreEqual(22, zone.ToUniversalTime(DateTime.Parse("2007-03-25 01:00")).Hour);
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-03-25 01:30")));
            Assert.AreEqual(22, zone.ToUniversalTime(DateTime.Parse("2007-03-25 01:30")).Hour);
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-03-25 02:00")));
            Assert.AreEqual(22, zone.ToUniversalTime(DateTime.Parse("2007-03-25 02:00")).Hour);
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-03-25 02:30")));
            Assert.AreEqual(22, zone.ToUniversalTime(DateTime.Parse("2007-03-25 02:30")).Hour);

            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-10-28 00:00")));
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-10-28 01:00")));
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-10-28 01:30")));
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-10-28 02:00")));
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-10-28 02:30")));
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-10-28 03:00")));
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-10-28 03:30")));
            Console.WriteLine(zone.ToUniversalTime(DateTime.Parse("2007-10-28 04:00")));

        //    Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-03-24 23:00"), DateTimeKind.Utc)));
        //    Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-03-25 01:00"), DateTimeKind.Utc)));
        //    Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-03-25 01:30"), DateTimeKind.Utc)));
        //    Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-03-25 02:00"), DateTimeKind.Utc)));
        //    Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-03-25 02:30"), DateTimeKind.Utc)));
        }

        [Test]
        public void WorkItem12914()
        {
            TzTimeZone zone = TzTimeZone.GetTimeZone("Asia/Novosibirsk");
            Assert.AreEqual(2, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-27 19:00"), DateTimeKind.Utc)).Hour);
            Assert.AreEqual(2, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-27 19:30"), DateTimeKind.Utc)).Hour);
            Assert.AreEqual(3, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-27 20:00"), DateTimeKind.Utc)).Hour);
            Assert.AreEqual(3, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-27 20:30"), DateTimeKind.Utc)).Hour);
            Assert.AreEqual(3, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-28 21:00"), DateTimeKind.Utc)).Hour);
            Assert.AreEqual(3, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-28 21:30"), DateTimeKind.Utc)).Hour);
            Assert.AreEqual(4, zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-27 22:00"), DateTimeKind.Utc)).Hour);

            Console.WriteLine(GlobalConstants.DividerEquals);

            zone = TzTimeZone.GetTimeZone("Europe/Moscow");
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-27 22:00"), DateTimeKind.Utc)));
            //: "10/28/2007 2:00:00 AM"
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-27 22:30"), DateTimeKind.Utc)));
            //: "10/28/2007 2:30:00 AM"
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-27 23:00"), DateTimeKind.Utc)));
            //: "10/28/2007 3:00:00 AM" <- I think this should be 2:00 am...
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-27 23:30"), DateTimeKind.Utc)));
            //: "10/28/2007 3:30:00 AM" <- ...and this should be 2:30 am...
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-28 00:00"), DateTimeKind.Utc)));
            //: "10/28/2007 3:00:00 AM" <- ...instead of 4 am becoming 3 am
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-28 00:30"), DateTimeKind.Utc)));
            //: "10/28/2007 3:30:00 AM"
            Console.WriteLine(zone.ToLocalTime(DateTime.SpecifyKind(DateTime.Parse("2007-10-28 01:00"), DateTimeKind.Utc)));
            //: "10/28/2007 4:00:00 AM"
        }

        [Test]
        public void TestInitializationTime()
        {
            long start = DateTime.Now.Ticks;

            Console.WriteLine(TzTimeZone.ZoneUsEastern.GetAbbreviation());
            Console.WriteLine(TzTimeZone.GetTimeZone(TzConstants.TimezoneAmericaNewYork).GetAbbreviation());

            long end = DateTime.Now.Ticks;
            Console.WriteLine(end - start);
        }

        [Test]
        public void TestBug13252()
        {
            foreach (string key in new string[] { "Pacific/Auckland", "Australia/Adelaide", "America/Sao_Paulo" })
            {
                TzTimeZone zone = TzTimeZone.GetTimeZone(key);
                DaylightTime daylightTime = zone.GetDaylightChanges(DateTime.Now.Year);
                Console.WriteLine(zone.StandardName);
                Console.WriteLine("Start: {0}", daylightTime.Start);
                Console.WriteLine("End: {0}", daylightTime.End);
                Console.WriteLine();
            }
        }

        [Test]
        public void Bug13252a()
        {
            //wrong daylight savings time for adelaide
            TzTimeZone adelaide = TzTimeZone.GetTimeZone("Australia/Adelaide");
            DaylightTime dtAdelaide = adelaide.GetDaylightChanges(2007);
            Assert.AreEqual(10, dtAdelaide.Start.Hour);
            Assert.AreEqual(28, dtAdelaide.Start.Hour);
            Assert.AreEqual(3, dtAdelaide.End.Hour);
            Assert.AreEqual(25, dtAdelaide.End.Hour);

            TzTimeZone perth = TzTimeZone.GetTimeZone("Australia/Perth");
            DaylightTime dtPerth = adelaide.GetDaylightChanges(2007);
            Assert.AreEqual(10, dtPerth.Start.Hour);
            Assert.AreEqual(28, dtPerth.Start.Hour);
            Assert.AreEqual(3, dtPerth.End.Hour);
            Assert.AreEqual(25, dtPerth.End.Hour);

            TzTimeZone eucla = TzTimeZone.GetTimeZone("Australia/Eucla");
            DaylightTime dtEucla = adelaide.GetDaylightChanges(2007);
            Assert.AreEqual(false, eucla.IsDaylightSavingTime(middleYear));
            Assert.AreEqual(true, eucla.IsDaylightSavingTime(startYear));

            TzTimeZone west = TzTimeZone.GetTimeZone("Australia/West");
            DaylightTime dtWest = adelaide.GetDaylightChanges(2007);
            Assert.AreEqual(false, west.IsDaylightSavingTime(middleYear));
            Assert.AreEqual(true, west.IsDaylightSavingTime(startYear));
        }

        [Test]
        public void Bug13252b()
        {
            //wrong daylight savings time for auckland
            TzTimeZone auckland = TzTimeZone.GetTimeZone("Pacific/Auckland");
            DaylightTime dt = auckland.GetDaylightChanges(2007);
            Assert.AreEqual(3, dt.End.Month);
            Assert.AreEqual(18, dt.End.Day);
            Assert.AreEqual(9, dt.Start.Month);
            Assert.AreEqual(30, dt.Start.Day);

            //auckland should be in DST at end of the year
            Assert.AreEqual(false, auckland.IsDaylightSavingTime(middleYear));
            Assert.AreEqual(true, auckland.IsDaylightSavingTime(startYear));
        }

        [Test]
        public void Bug14464()
        {
            TzDateTime cmp = TzTimeZone.ZoneUTC.Now;
            DateTime cmpLocal = cmp.DateTimeLocal;

            Console.WriteLine(cmpLocal);

            foreach (string zuluName in new string[] {
                "Etc/GMT",
                "Etc/UTC",
                "Etc/UCT",
                "GMT",
                "Etc/Universal",
                "Etc/Zulu",
                "Etc/Greenwich",
                "Etc/GMT-0",
                "Etc/GMT+0",
                "Etc/GMT0"
            })
            {
                TzTimeZone zone = TzTimeZone.GetTimeZone(zuluName);
                Console.WriteLine(zone.Now.DateTimeUtc.ToString());
                DateTime local = zone.Now.DateTimeLocal;
                Assert.AreEqual(cmpLocal.Hour, local.Hour);
                Assert.AreEqual(cmpLocal.Date, local.Date);
            }

            cmpLocal = cmpLocal.AddHours(+14);
            for (int i = -14; i < 13; i++)
            {
                if (i != 0)
                {
                    string name = "Etc/GMT" + (i < 0 ? "" : "+") + i;
                    TzTimeZone zone = TzTimeZone.GetTimeZone(name);
                    Console.WriteLine("{1}={0}", zone.Now.DateTimeLocal, name);
                    Assert.AreEqual(cmpLocal.Hour, zone.Now.DateTimeLocal.Hour);
                }
                cmpLocal = cmpLocal.AddHours(-1);
            }
        }
    }
}
