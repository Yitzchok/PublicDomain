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
    public class TzDateTimeTests
    {
        /// <summary>
        /// Test1s this instance.
        /// </summary>
        [Test]
        public void Test1()
        {
            foreach (string test in new string[] {
                    "1984-06-20 3:00:00 PM-05:00",
                    "2007-01-01+00:00",
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

        [Test]
        public void TestDaylightChanges()
        {
            TzTimeZone zone = TzTimeZone.ZoneUsCentral;

            TestDaylightChangesYear(zone, 1999, 4, 4, 1999, 10, 31, 1999);
            TestDaylightChangesYear(zone, 2000, 4, 2, 2000, 10, 29, 2000);
            TestDaylightChangesYear(zone, 2001, 4, 1, 2001, 10, 28, 2001);
            TestDaylightChangesYear(zone, 2002, 4, 7, 2002, 10, 27, 2002);
            TestDaylightChangesYear(zone, 2003, 4, 6, 2003, 10, 26, 2003);
            TestDaylightChangesYear(zone, 2004, 4, 4, 2004, 10, 31, 2004);
            TestDaylightChangesYear(zone, 2005, 4, 3, 2005, 10, 30, 2005);
            TestDaylightChangesYear(zone, 2006, 4, 2, 2006, 10, 29, 2006);
            TestDaylightChangesYear(zone, 2007, 3, 11, 2007, 11, 4, 2007);
            TestDaylightChangesYear(zone, 2008, 3, 9, 2008, 11, 2, 2008);
            TestDaylightChangesYear(zone, 2009, 3, 8, 2009, 11, 1, 2009);
            TestDaylightChangesYear(zone, 2010, 3, 14, 2010, 11, 7, 2010);
        }

        private static void TestDaylightChangesYear(TzTimeZone zone, int year, int expectedStartMonth, int expectedStartDay, int expectedStartYear, int expectedEndMonth, int expectedEndDay, int expectedEndYear)
        {
            DaylightTime daylight = zone.GetDaylightChanges(year);
            Console.WriteLine(daylight.Start);
            Console.WriteLine(daylight.End);

            AssertEqualDaylightPosition(daylight.Start, expectedStartMonth, expectedStartDay, expectedStartYear);
            AssertEqualDaylightPosition(daylight.End, expectedEndMonth, expectedEndDay, expectedEndYear);
        }

        private static void AssertEqualDaylightPosition(DateTime dt, int month, int day, int year)
        {
            Assert.AreEqual(month, dt.Month);
            Assert.AreEqual(day, dt.Day);
            Assert.AreEqual(year, dt.Year);
        }

        [Test]
        public void TestIsDaylightSavingsTime()
        {
            TzTimeZone zone = TzTimeZone.ZoneUsEastern;

            AssertIsDaylightSavingsTime(zone, "2007-12-15", false);
            AssertIsDaylightSavingsTime(zone, "2007-6-1", true);
            AssertIsDaylightSavingsTime(zone, "2007-1-1", false);
            AssertIsDaylightSavingsTime(zone, "2007-3-12", true);
            AssertIsDaylightSavingsTime(zone, "2007-11-5", false);

            AssertIsDaylightSavingsTime(zone, "2006-1-1", false);
            AssertIsDaylightSavingsTime(zone, "2006-4-3", true);
            AssertIsDaylightSavingsTime(zone, "2006-10-30", false);
            AssertIsDaylightSavingsTime(zone, "2006-6-1", true);
            AssertIsDaylightSavingsTime(zone, "2006-12-15", false);
        }

        private static void AssertIsDaylightSavingsTime(TzTimeZone zone, string dt, bool expectIsDaylightSavingsTime)
        {
            Assert.AreEqual(expectIsDaylightSavingsTime, zone.IsDaylightSavingTime(DateTime.Parse(dt)));
        }

        [Test]
        public void TestGetUtcOffset()
        {
            TzTimeZone zone = TzTimeZone.ZoneUsEastern;
            TzDateTime dtDate = new TzDateTime(zone.ToLocalTime(DateTime.UtcNow), zone);

            dtDate.DateTimeLocal = DateTime.Parse("2007-01-01 03:00");
            Assert.AreEqual(-5, zone.GetUtcOffset(dtDate.DateTimeLocal).TotalHours);
            dtDate.DateTimeLocal = DateTime.Parse("2007-09-12 03:00");
            Assert.AreEqual(-4, zone.GetUtcOffset(dtDate.DateTimeLocal).TotalHours);
            dtDate.DateTimeLocal = DateTime.Parse("2007-12-12 03:00");
            Assert.AreEqual(-5, zone.GetUtcOffset(dtDate.DateTimeLocal).TotalHours);
        }

        [Test]
        public void WorkItem12159()
        {
            Console.WriteLine("12159");

            TzTimeZone zone = TzTimeZone.ZoneUsCentral;

            Console.WriteLine(zone.Now.DateTimeLocal);

            // Bug #1:
            Console.WriteLine("zone.StandardName: '" + zone.StandardName + "'");
            Console.WriteLine("zone.GetUtcOffset(Now).TotalHours: '" + zone.GetUtcOffset(DateTime.Now).TotalHours + "'");
            Console.WriteLine("zone.ToUniversalTime(now): '" + zone.ToUniversalTime(DateTime.Now) + "'");
            Console.WriteLine("zone.DaylightName(): '" + zone.DaylightName + "'");
            Console.WriteLine("zone.GetDaylightChanges(2004).Start: '" + zone.GetDaylightChanges(2004).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2004).End+#58; '" + zone.GetDaylightChanges(2004).End + "'");
            Console.WriteLine("zone.GetDaylightChanges(2005).Start: '" + zone.GetDaylightChanges(2005).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2005).End+#58; '" + zone.GetDaylightChanges(2005).End + "'");
            Console.WriteLine("zone.GetDaylightChanges(2006).Start: '" + zone.GetDaylightChanges(2006).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2006).End+#58; '" + zone.GetDaylightChanges(2006).End + "'");
            Console.WriteLine("zone.GetDaylightChanges(2007).Start: '" + zone.GetDaylightChanges(2007).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2007).End+#58; '" + zone.GetDaylightChanges(2007).End + "'");
            Console.WriteLine("zone.GetDaylightChanges(2008).Start: '" + zone.GetDaylightChanges(2008).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2008).End+#58; '" + zone.GetDaylightChanges(2008).End + "'");
            Console.WriteLine("zone.GetDaylightChanges(2009).Start: '" + zone.GetDaylightChanges(2009).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2009).End+#58; '" + zone.GetDaylightChanges(2009).End + "'");
            Console.WriteLine("zone.GetDaylightChanges(2010).Start: '" + zone.GetDaylightChanges(2010).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2010).End+#58; '" + zone.GetDaylightChanges(2010).End + "'");

            // Bug #2:
            Console.WriteLine("zone.IsDaylightSavingTime(Now): '" + zone.IsDaylightSavingTime(DateTime.Now) + "'");
            Console.WriteLine("zone.IsDaylightSavingTime(#12/15/2007#): '" + zone.IsDaylightSavingTime(DateTime.Parse("2007-12-15")) + "'");
            Console.WriteLine("zone.IsDaylightSavingTime('2007-12-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("2007-12-15")) + "'");

            // Bug #3:
            TzDateTime dtDate = new PublicDomain.TzDateTime(zone.ToLocalTime(DateTime.UtcNow), zone);
            dtDate.DateTimeLocal = DateTime.Parse("2007-09-12 03:00");
            Console.WriteLine("dtDate.DateTimeLocal = '2007-09-12 03:00'");
            Console.WriteLine("zone.GetUtcOffset(dtDate).TotalHours: '" + zone.GetUtcOffset(dtDate.DateTimeLocal).TotalHours + "'");
            dtDate.DateTimeLocal = DateTime.Parse("2007-12-12 03:00");
            Console.WriteLine("dtDate.DateTimeLocal = '2007-12-12 03:00'");
            Console.WriteLine("zone.GetUtcOffset(dtDate).TotalHours: '" + zone.GetUtcOffset(dtDate.DateTimeLocal).TotalHours + "'");

            // Bug #4:
            Console.WriteLine("zone.ToUniversalTime('2007-09-12 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-09-12 03:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-11-04 01:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-11-04 01:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-11-04 02:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-11-04 02:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-11-04 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-11-04 03:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-12-04 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-12-04 03:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-03-09 01:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-09 01:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-03-09 02:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-09 02:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-03-09 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-09 03:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-05-10 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-05-10 03:00")) + "'");

            // Bug #5:
            zone = PublicDomain.TzTimeZone.GetTimeZone("Europe/Paris");
            Console.WriteLine("zone.GetDaylightChanges(2004).Start: '" + zone.GetDaylightChanges(2004).Start + "'");
        }

        [Test]
        public void WorkItem12159_2()
        {
            // New set of bugs
            // ===============
            //
            Console.WriteLine("============ Zone America/Chicago (GMT-06:00) =============");
            TzTimeZone zone = TzTimeZone.CurrentTimeZone;
            Assert.AreEqual(DateTime.UtcNow.Hour, zone.ToUniversalTime(DateTime.Now).Hour);

            zone = TzTimeZone.GetTimeZone(TzConstants.TimezoneAmericaChicago);
            Console.WriteLine("zone.StandardName: '" + zone.StandardName + "'");
            Console.WriteLine("zone.GetUtcOffset(Now).TotalHours: '" + zone.GetUtcOffset(DateTime.Now).TotalHours + "'");
            Console.WriteLine("DateTime.UtcNow: " + DateTime.UtcNow);
            Console.WriteLine("zone.ToUniversalTime(now): '" + zone.ToUniversalTime(DateTime.Now) + "'");
            TzDateTime dtDate = new TzDateTime(zone.ToLocalTime(DateTime.UtcNow), zone);
            dtDate.DateTimeLocal = DateTime.Parse("2007-09-12 03:00");
            Console.WriteLine("dtDate.DateTimeLocal = '2007-09-12 03:00'");
            Console.WriteLine("zone.GetUtcOffset(dtDate).TotalHours: '" + zone.GetUtcOffset(dtDate.DateTimeLocal).TotalHours + "'");
            dtDate.DateTimeLocal = DateTime.Parse("2007-12-12 03:00");
            Console.WriteLine("dtDate.DateTimeLocal = '2007-12-12 03:00'");
            Console.WriteLine("zone.GetUtcOffset(dtDate).TotalHours: '" + zone.GetUtcOffset(dtDate.DateTimeLocal).TotalHours + "'");
            Console.WriteLine("zone.IsDaylightSavingTime(Now): '" + zone.IsDaylightSavingTime(DateTime.Now) + "'");
            Console.WriteLine("zone.IsDaylightSavingTime('2007-09-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("2007-09-15")) + "'");
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2007-09-15")));
            Console.WriteLine("zone.IsDaylightSavingTime('2007-12-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("2007-12-15")) + "'");
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2007-12-15")));
            Console.WriteLine("zone.IsDaylightSavingTime('2008-09-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("2008-09-15")) + "'");
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2008-09-15")));
            Console.WriteLine("zone.IsDaylightSavingTime('2008-12-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("2008-12-15")) + "'");
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2008-12-15")));
            //Console.WriteLine("zone.IsDaylightSavingTime('2016-09-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("2016-09-15")) + "'");
            //Console.WriteLine("zone.IsDaylightSavingTime('2016-12-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("2016-12-15")) + "'");
            Console.WriteLine("zone.IsDaylightSavingTime('1974-09-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("1974-09-15")) + "'");
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("1974-09-15")));
            Console.WriteLine("zone.IsDaylightSavingTime('1974-12-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("1974-12-15")) + "'");
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("1974-12-15")));
            Console.WriteLine("--------- Converting local time to UTC -------------");

            // April 2nd, 2006 2am time changes
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2006-04-01")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2006-04-02")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2006-04-03")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2006-04-02 01:00")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2006-04-02 01:59")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2006-04-02 02:00")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2006-04-02 02:01")));
            Console.WriteLine("DST on rule <= 2006 for America/Chicago: Apr Sun>=1 2:00 (2am (8:00 UTC) becomes 3am)");
            Console.WriteLine("zone.ToUniversalTime('2006-04-02 01:00'): '" + zone.ToUniversalTime(DateTime.Parse("2006-04-02 01:00")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2006-04-02 01:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-04-02 01:30'): '" + zone.ToUniversalTime(DateTime.Parse("2006-04-02 01:30")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2006-04-02 01:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-04-02 02:00'): '" + zone.ToUniversalTime(DateTime.Parse("2006-04-02 02:00")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2006-04-02 02:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-04-02 02:30'): '" + zone.ToUniversalTime(DateTime.Parse("2006-04-02 02:30")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2006-04-02 02:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-04-02 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2006-04-02 03:00")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse("2006-04-02 03:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-04-02 03:30'): '" + zone.ToUniversalTime(DateTime.Parse("2006-04-02 03:30")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse("2006-04-02 03:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-04-02 04:00'): '" + zone.ToUniversalTime(DateTime.Parse("2006-04-02 04:00")) + "'");
            Assert.AreEqual(9, zone.ToUniversalTime(DateTime.Parse("2006-04-02 04:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-05-10 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2006-05-10 03:00")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse("2006-05-10 03:00")).Hour);

            // 2006 October last sun 29 at 2 am
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2006-10-28")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2006-10-29")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2006-10-30")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2006-10-29 01:00")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2006-10-29 01:59")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2006-10-29 02:00")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2006-10-29 02:01")));
            Console.WriteLine("DST off rule <= 2006 for America/Chicago: Nov Sun>=1 2:00 (2am (7:00 UTC) becomes 1am)");
            Console.WriteLine("zone.ToUniversalTime('2006-10-29 00:00'): '" + zone.ToUniversalTime(DateTime.Parse("2006-10-29 00:00")) + "'");
            Assert.AreEqual(5, zone.ToUniversalTime(DateTime.Parse("2006-10-29 00:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-10-29 00:30'): '" + zone.ToUniversalTime(DateTime.Parse("2006-10-29 00:30")) + "'");
            Assert.AreEqual(5, zone.ToUniversalTime(DateTime.Parse("2006-10-29 00:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-10-29 01:00'): '" + zone.ToUniversalTime(DateTime.Parse("2006-11-04 01:00")) + "'");
            Assert.AreEqual(6, zone.ToUniversalTime(DateTime.Parse("2006-10-29 01:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-10-29 01:30'): '" + zone.ToUniversalTime(DateTime.Parse("2006-11-04 01:30")) + "'");
            Assert.AreEqual(6, zone.ToUniversalTime(DateTime.Parse("2006-10-29 01:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-10-29 02:00'): '" + zone.ToUniversalTime(DateTime.Parse("2006-11-04 02:00")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse("2006-10-29 02:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-10-29 02:30'): '" + zone.ToUniversalTime(DateTime.Parse("2006-11-04 02:30")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse("2006-10-29 02:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-10-29 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2006-11-04 03:00")) + "'");
            Assert.AreEqual(9, zone.ToUniversalTime(DateTime.Parse("2006-10-29 03:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-10-29 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2006-12-12 03:00")) + "'");
            Assert.AreEqual(9, zone.ToUniversalTime(DateTime.Parse("2006-10-29 03:00")).Hour);

            CheckToUniversalTime(zone, "2007");

            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2008-03-8")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2008-03-9")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2008-03-10")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2008-03-9 01:00")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2008-03-9 01:59")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2008-03-9 02:00")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2008-03-9 02:01")));
            Console.WriteLine("DST on rule >= 2007 for America/Chicago: Mar Sun>=8 2:00 (2am (8:00 UTC) becomes 3am)");
            Console.WriteLine("zone.ToUniversalTime('2008-03-09 01:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-09 01:00")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2008-03-09 01:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-03-09 01:30'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-09 01:30")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2008-03-09 01:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-03-09 02:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-09 02:00")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2008-03-09 02:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-03-09 02:30'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-09 02:30")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2008-03-09 02:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-03-09 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-09 03:00")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse("2008-03-09 03:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-03-09 03:30'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-09 03:30")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse("2008-03-09 03:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-03-09 04:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-09 04:00")) + "'");
            Assert.AreEqual(9, zone.ToUniversalTime(DateTime.Parse("2008-03-09 04:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-05-10 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-05-10 03:00")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse("2008-05-09 03:00")).Hour);

            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2008-11-1")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2008-11-2")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2008-11-3")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2008-11-2 01:00")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse("2008-11-2 01:59")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2008-11-2 02:00")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse("2008-11-2 02:01")));
            Console.WriteLine("DST off rule >= 2007 for America/Chicago: Nov Sun>=1 2:00 (2am (7:00 UTC) becomes 1am)");
            Console.WriteLine("zone.ToUniversalTime('2008-11-02 00:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-11-02 00:00")) + "'");
            Assert.AreEqual(5, zone.ToUniversalTime(DateTime.Parse("2008-11-02 00:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-11-02 01:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-11-02 01:00")) + "'");
            Assert.AreEqual(6, zone.ToUniversalTime(DateTime.Parse("2008-11-02 01:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-11-02 01:30'): '" + zone.ToUniversalTime(DateTime.Parse("2008-11-02 01:30")) + "'");
            Assert.AreEqual(6, zone.ToUniversalTime(DateTime.Parse("2008-11-02 01:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-11-02 02:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-11-02 02:00")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse("2008-11-02 02:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-11-02 02:30'): '" + zone.ToUniversalTime(DateTime.Parse("2008-11-02 02:30")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse("2008-11-02 02:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-11-02 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-11-02 03:00")) + "'");
            Assert.AreEqual(9, zone.ToUniversalTime(DateTime.Parse("2008-11-02 03:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-11-02 03:30'): '" + zone.ToUniversalTime(DateTime.Parse("2008-11-02 03:30")) + "'");
            Assert.AreEqual(9, zone.ToUniversalTime(DateTime.Parse("2008-11-02 03:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2008-12-12 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-12-12 03:00")) + "'");
            Assert.AreEqual(9, zone.ToUniversalTime(DateTime.Parse("2008-12-02 03:00")).Hour);

            zone = PublicDomain.TzTimeZone.GetTimeZone(TzConstants.TimezoneEuropeParis);

            Console.WriteLine("zone = PublicDomain.TzTimeZone.GetTimeZone('Europe/Paris')");
            Console.WriteLine("zone.GetUtcOffset(Now).TotalHours: '" + zone.GetUtcOffset(DateTime.Parse("2007-01-01")).TotalHours + "'");
            Console.WriteLine("zone.GetUtcOffset(Now).TotalHours: '" + zone.GetUtcOffset(DateTime.Parse("2007-06-01")).TotalHours + "'");
            Console.WriteLine("zone.ToUniversalTime(now): '" + zone.ToUniversalTime(DateTime.Now) + "'");
            Console.WriteLine("zone.DaylightName(): '" + zone.DaylightName + "'");
            Console.WriteLine("zone.GetDaylightChanges(2004).Start: '" + zone.GetDaylightChanges(2004).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2004).End: '" + zone.GetDaylightChanges(2004).End + "'");
            Console.WriteLine("zone.GetDaylightChanges(2005).Start: '" + zone.GetDaylightChanges(2005).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2005).End: '" + zone.GetDaylightChanges(2005).End + "'");
            Console.WriteLine("zone.GetDaylightChanges(2006).Start: '" + zone.GetDaylightChanges(2006).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2006).End: '" + zone.GetDaylightChanges(2006).End + "'");
            Console.WriteLine("zone.GetDaylightChanges(2007).Start: '" + zone.GetDaylightChanges(2007).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2007).End: '" + zone.GetDaylightChanges(2007).End + "'");
            Console.WriteLine("zone.GetDaylightChanges(2008).Start: '" + zone.GetDaylightChanges(2008).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2008).End: '" + zone.GetDaylightChanges(2008).End + "'");
            Console.WriteLine("zone.GetDaylightChanges(2009).Start: '" + zone.GetDaylightChanges(2009).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2009).End: '" + zone.GetDaylightChanges(2009).End + "'");
            Console.WriteLine("zone.GetDaylightChanges(2010).Start: '" + zone.GetDaylightChanges(2010).Start + "'");
            Console.WriteLine("zone.GetDaylightChanges(2010).End: '" + zone.GetDaylightChanges(2010).End + "'");
            Console.WriteLine("---------------");

            dtDate = new PublicDomain.TzDateTime(zone.ToLocalTime(DateTime.UtcNow), zone);
            //DST
            dtDate.DateTimeLocal = DateTime.Parse("2007-09-12 03:00");
            Console.WriteLine("dtDate.DateTimeLocal = '2007-09-12 03:00'");
            Console.WriteLine("zone.GetUtcOffset(dtDate).TotalHours: '" + zone.GetUtcOffset(dtDate.DateTimeLocal).TotalHours + "'");
            //Non-DST
            dtDate.DateTimeLocal = DateTime.Parse("2007-12-12 03:00");
            Console.WriteLine("dtDate.DateTimeLocal = '2007-12-12 03:00'");
            Console.WriteLine("zone.GetUtcOffset(dtDate).TotalHours: '" + zone.GetUtcOffset(dtDate.DateTimeLocal).TotalHours + "'");
            //IsDaylightSavingTime
            Console.WriteLine("zone.IsDaylightSavingTime(Now): '" + zone.IsDaylightSavingTime(DateTime.Now) + "'");
            Console.WriteLine("zone.IsDaylightSavingTime('2007-09-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("2007-09-15")) + "'");
            Console.WriteLine("zone.IsDaylightSavingTime('2007-12-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("2007-12-15")) + "'");
            Console.WriteLine("zone.IsDaylightSavingTime('2008-09-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("2008-09-15")) + "'");
            Console.WriteLine("zone.IsDaylightSavingTime('2008-12-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("2008-12-15")) + "'");
            Console.WriteLine("zone.IsDaylightSavingTime('2016-09-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("2016-09-15")) + "'");
            Console.WriteLine("zone.IsDaylightSavingTime('2016-12-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("2016-12-15")) + "'");
            Console.WriteLine("zone.IsDaylightSavingTime('1974-09-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("1974-09-15")) + "'");
            Console.WriteLine("zone.IsDaylightSavingTime('1974-12-15'): '" + zone.IsDaylightSavingTime(DateTime.Parse("1974-12-15")) + "'");
            Console.WriteLine("--------- Converting local time to UTC -------------");
            //ToUniversalTime around DST breaks
            Console.WriteLine("zone.ToUniversalTime('2007-01-25 00:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-01-25 00:00")) + "'");
            Console.WriteLine("DST on rule for Europe/Paris: Mar lastSun 1:00u (2am (1:00 UTC) becomes 3am)");
            Console.WriteLine("zone.ToUniversalTime('2007-03-25 00:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-03-25 00:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-03-25 01:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-03-25 01:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-03-25 02:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-03-25 02:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-03-25 02:30'): '" + zone.ToUniversalTime(DateTime.Parse("2007-03-25 02:30")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-03-25 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-03-25 03:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-03-25 03:30'): '" + zone.ToUniversalTime(DateTime.Parse("2007-03-25 03:30")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-03-25 04:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-03-25 04:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-03-25 04:30'): '" + zone.ToUniversalTime(DateTime.Parse("2007-03-25 04:30")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-09-12 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-09-12 03:00")) + "'");
            Console.WriteLine("DST off rule for Europe/Paris: Oct lastSun 1:00u (3am (1:00 UTC) becomes 2am)");
            Console.WriteLine("zone.ToUniversalTime('2007-10-28 00:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-10-28 00:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-10-28 01:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-10-28 01:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-10-28 01:30'): '" + zone.ToUniversalTime(DateTime.Parse("2007-10-28 01:30")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-10-28 02:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-10-28 02:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-10-28 02:30'): '" + zone.ToUniversalTime(DateTime.Parse("2007-10-28 02:30")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-10-28 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-10-28 03:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-10-28 03:30'): '" + zone.ToUniversalTime(DateTime.Parse("2007-10-28 03:30")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-10-28 04:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-10-28 04:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2007-12-12 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2007-12-12 03:00")) + "'");
            Console.WriteLine("DST on rule for Europe/Paris: Mar lastSun 1:00u (2am (1:00 UTC) becomes 3am)");
            Console.WriteLine("zone.ToUniversalTime('2008-03-30 00:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-30 00:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-03-30 01:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-30 01:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-03-30 02:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-30 02:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-03-30 02:30'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-30 02:30")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-03-30 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-30 03:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-03-30 03:30'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-30 03:30")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-03-30 04:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-30 04:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-03-30 04:30'): '" + zone.ToUniversalTime(DateTime.Parse("2008-03-30 04:30")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-05-10 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-05-10 03:00")) + "'");
            Console.WriteLine("DST off rule for Europe/Paris: Oct lastSun 1:00u (3am (1:00 UTC) becomes 2am)");
            Console.WriteLine("zone.ToUniversalTime('2008-10-26 00:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-10-26 00:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-10-26 01:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-10-26 01:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-10-26 01:30'): '" + zone.ToUniversalTime(DateTime.Parse("2008-10-26 01:30")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-10-26 02:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-10-26 02:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-10-26 02:30'): '" + zone.ToUniversalTime(DateTime.Parse("2008-10-26 02:30")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-10-26 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-10-26 03:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-10-26 03:30'): '" + zone.ToUniversalTime(DateTime.Parse("2008-10-26 03:30")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-10-26 04:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-10-26 04:00")) + "'");
            Console.WriteLine("zone.ToUniversalTime('2008-12-12 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2008-12-12 03:00")) + "'");
            Console.WriteLine("--------- Converting UTC to local time -------------");
            Console.WriteLine("zone.ToLocalTime('2007-01-25 00:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-01-25 00:00")) + "'");
            Console.WriteLine("DST on rule for Europe/Paris: Mar lastSun 1:00u (2am (1:00 UTC) becomes 3am)");
            Console.WriteLine("zone.ToLocalTime('2007-03-25 00:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-25 00:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-03-25 00:30'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-25 00:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-03-25 01:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-25 01:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-03-25 01:30'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-25 01:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-03-25 02:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-25 02:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-03-25 02:30'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-25 02:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-03-25 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-25 03:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-03-25 04:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-25 04:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-09-12 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-09-12 03:00")) + "'");
            Console.WriteLine("DST off rule for Europe/Paris: Oct lastSun 1:00u (3am (1:00 UTC) becomes 2am)");
            Console.WriteLine("zone.ToLocalTime('2007-10-28 00:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-10-28 00:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-10-28 00:30'): '" + zone.ToLocalTime(DateTime.Parse("2007-10-28 00:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-10-28 01:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-10-28 01:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-10-28 01:30'): '" + zone.ToLocalTime(DateTime.Parse("2007-10-28 01:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-10-28 02:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-10-28 02:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-10-28 02:30'): '" + zone.ToLocalTime(DateTime.Parse("2007-10-28 02:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-10-28 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-10-28 03:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-10-28 04:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-10-28 04:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-12-12 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-12-12 03:00")) + "'");
            Console.WriteLine("DST on rule for Europe/Paris: Mar lastSun 1:00u (2am (1:00 UTC) becomes 3am)");
            Console.WriteLine("zone.ToLocalTime('2008-03-30 00:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-30 00:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-03-30 00:30'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-30 00:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-03-30 01:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-30 01:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-03-30 01:30'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-30 01:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-03-30 02:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-30 02:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-03-30 02:30'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-30 02:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-03-30 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-30 03:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-03-30 04:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-30 04:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-05-10 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-05-10 03:00")) + "'");
            Console.WriteLine("DST off rule for Europe/Paris: Oct lastSun 1:00u (3am (1:00 UTC) becomes 2am)");
            Console.WriteLine("zone.ToLocalTime('2008-10-26 00:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-10-26 00:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-10-26 00:30'): '" + zone.ToLocalTime(DateTime.Parse("2008-10-26 00:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-10-26 01:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-10-26 01:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-10-26 01:30'): '" + zone.ToLocalTime(DateTime.Parse("2008-10-26 01:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-10-26 02:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-10-26 02:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-10-26 02:30'): '" + zone.ToLocalTime(DateTime.Parse("2008-10-26 02:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-10-26 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-10-26 03:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-10-26 04:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-10-26 04:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-12-12 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-12-12 03:00")) + "'");
        }

        private static void TestToLocalTime(TzTimeZone zone)
        {
            Console.WriteLine("--------- Converting UTC to local time -------------");
            Console.WriteLine("DST on rule <= 2006 for America/Chicago: Apr Sun>=1 2:00 (2am (8:00 UTC) becomes 3am)");
            Console.WriteLine("zone.ToLocalTime('2006-04-02 07:00'): '" + zone.ToLocalTime(DateTime.Parse("2006-04-02 07:00")) + "'");
            Assert.AreEqual(1, zone.ToLocalTime(new DateTime(DateTime.Parse("2006-04-02 06:00").Ticks, DateTimeKind.Utc)).Hour);
            Assert.AreEqual(2, zone.ToLocalTime(new DateTime(DateTime.Parse("2006-04-02 07:00").Ticks, DateTimeKind.Utc)).Hour);
            Console.WriteLine("zone.ToLocalTime('2006-04-02 07:30'): '" + zone.ToLocalTime(DateTime.Parse("2006-04-02 07:30")) + "'");
            Assert.AreEqual(2, zone.ToLocalTime(new DateTime(DateTime.Parse("2006-04-02 07:30").Ticks, DateTimeKind.Utc)).Hour);
            Console.WriteLine("zone.ToLocalTime('2006-04-02 08:00'): '" + zone.ToLocalTime(DateTime.Parse("2006-04-02 08:00")) + "'");
            Assert.AreEqual(3, zone.ToLocalTime(new DateTime(DateTime.Parse("2006-04-02 08:00").Ticks, DateTimeKind.Utc)).Hour);
            Console.WriteLine("zone.ToLocalTime('2006-04-02 08:30'): '" + zone.ToLocalTime(DateTime.Parse("2006-04-02 08:30")) + "'");
            Assert.AreEqual(3, zone.ToLocalTime(new DateTime(DateTime.Parse("2006-04-02 08:30").Ticks, DateTimeKind.Utc)).Hour);
            Console.WriteLine("zone.ToLocalTime('2006-04-02 09:00'): '" + zone.ToLocalTime(DateTime.Parse("2006-04-02 09:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2006-04-02 09:30'): '" + zone.ToLocalTime(DateTime.Parse("2006-04-02 09:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2006-04-02 10:00'): '" + zone.ToLocalTime(DateTime.Parse("2006-04-02 10:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2006-05-10 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2006-05-10 03:00")) + "'");
            Console.WriteLine("DST off rule <= 2006 for America/Chicago: Nov Sun>=1 2:00 (2am (7:00 UTC) becomes 1am)");
            Console.WriteLine("zone.ToLocalTime('2006-10-29 06:00'): '" + zone.ToLocalTime(DateTime.Parse("2006-10-29 06:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2006-10-29 06:30'): '" + zone.ToLocalTime(DateTime.Parse("2006-10-29 06:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2006-11-04 07:00'): '" + zone.ToLocalTime(DateTime.Parse("2006-11-04 07:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2006-11-04 07:30'): '" + zone.ToLocalTime(DateTime.Parse("2006-11-04 07:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2006-11-04 08:00'): '" + zone.ToLocalTime(DateTime.Parse("2006-11-04 08:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2006-11-04 08:30'): '" + zone.ToLocalTime(DateTime.Parse("2006-11-04 08:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2006-11-04 09:00'): '" + zone.ToLocalTime(DateTime.Parse("2006-11-04 09:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2006-12-12 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2006-12-12 03:00")) + "'");
            Console.WriteLine("DST on rule >= 2007 for America/Chicago: Mar Sun>=8 2:00 (2am (8:00 UTC) becomes 3am)");
            Console.WriteLine("zone.ToLocalTime('2007-03-11 07:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-11 07:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-03-11 07:30'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-11 07:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-03-11 08:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-11 08:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-03-11 08:30'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-11 08:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-03-11 09:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-11 09:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-03-11 09:30'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-11 09:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-03-11 10:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-03-11 10:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-05-10 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-05-10 03:00")) + "'");
            Console.WriteLine("DST off rule >= 2007 for America/Chicago: Nov Sun>=1 2:00 (2am (7:00 UTC) becomes 1am)");
            Console.WriteLine("zone.ToLocalTime('2007-11-04 06:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-11-04 06:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-11-04 06:30'): '" + zone.ToLocalTime(DateTime.Parse("2007-11-04 06:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-11-04 07:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-11-04 07:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-11-04 07:30'): '" + zone.ToLocalTime(DateTime.Parse("2007-11-04 07:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-11-04 08:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-11-04 08:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-11-04 08:30'): '" + zone.ToLocalTime(DateTime.Parse("2007-11-04 08:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-11-04 09:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-11-04 09:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2007-12-12 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2007-12-12 03:00")) + "'");
            Console.WriteLine("DST on rule >= 2007 for America/Chicago: Mar Sun>=8 2:00 (2am (8:00 UTC) becomes 3am)");
            Console.WriteLine("zone.ToLocalTime('2008-03-09 07:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-09 07:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-03-09 07:30'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-09 07:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-03-09 08:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-09 08:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-03-09 08:30'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-09 08:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-03-09 09:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-09 09:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-03-09 09:30'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-09 09:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-03-09 10:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-03-09 10:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-05-10 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-05-10 03:00")) + "'");
            Console.WriteLine("DST off rule >= 2007 for America/Chicago: Nov Sun>=1 2:00 (2am (7:00 UTC) becomes 1am)");
            Console.WriteLine("zone.ToLocalTime('2008-11-02 06:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-11-02 06:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-11-02 06:30'): '" + zone.ToLocalTime(DateTime.Parse("2008-11-02 06:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-11-02 07:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-11-02 07:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-11-02 07:30'): '" + zone.ToLocalTime(DateTime.Parse("2008-11-02 07:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-11-02 08:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-11-02 08:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-11-02 08:30'): '" + zone.ToLocalTime(DateTime.Parse("2008-11-02 08:30")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-11-02 09:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-11-02 09:00")) + "'");
            Console.WriteLine("zone.ToLocalTime('2008-12-12 03:00'): '" + zone.ToLocalTime(DateTime.Parse("2008-12-12 03:00")) + "'");
        }

        private static void CheckToUniversalTime(TzTimeZone zone, string year)
        {
            // 2007-3-11 @ 2am
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse(year + "-03-10")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse(year + "-03-11")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse(year + "-03-12")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse(year + "-03-11 01:00")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse(year + "-03-11 01:59")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse(year + "-03-11 02:00")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse(year + "-03-11 02:01")));
            Console.WriteLine("DST on rule >= " + year + " for America/Chicago: Mar Sun>=8 2:00 (2am (8:00 UTC) becomes 3am)");
            Console.WriteLine("zone.ToUniversalTime('" + year + "-03-11 01:00'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-03-11 01:00")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse(year + "-03-11 01:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-03-11 01:30'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-03-11 01:30")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse(year + "-03-11 01:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-03-11 02:00'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-03-11 02:00")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse(year + "-03-11 02:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-03-11 02:30'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-03-11 02:30")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse(year + "-03-11 02:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-03-11 03:00'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-03-11 03:00")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse(year + "-03-11 03:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-03-11 03:30'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-03-11 03:30")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse(year + "-03-11 03:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-03-11 04:00'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-03-11 04:00")) + "'");
            Assert.AreEqual(9, zone.ToUniversalTime(DateTime.Parse(year + "-03-11 04:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-05-10 03:00'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-05-10 03:00")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse(year + "-05-11 03:00")).Hour);

            // 2007-11-4 @ 2 am
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse(year + "-11-4")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse(year + "-11-4")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse(year + "-11-5")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse(year + "-11-4 01:00")));
            Assert.IsTrue(zone.IsDaylightSavingTime(DateTime.Parse(year + "-11-4 01:59")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse(year + "-11-4 02:00")));
            Assert.IsFalse(zone.IsDaylightSavingTime(DateTime.Parse(year + "-11-4 02:01")));
            Console.WriteLine("DST off rule >= " + year + " for America/Chicago: Nov Sun>=1 2:00 (2am (7:00 UTC) becomes 1am)");
            Console.WriteLine("zone.ToUniversalTime('" + year + "-11-04 00:00'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-11-04 00:00")) + "'");
            Assert.AreEqual(5, zone.ToUniversalTime(DateTime.Parse(year + "-11-04 00:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-11-04 01:00'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-11-04 01:00")) + "'");
            Assert.AreEqual(6, zone.ToUniversalTime(DateTime.Parse(year + "-11-04 01:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-11-04 01:30'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-11-04 01:30")) + "'");
            Assert.AreEqual(6, zone.ToUniversalTime(DateTime.Parse(year + "-11-04 01:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-11-04 02:00'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-11-04 02:00")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse(year + "-11-04 02:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-11-04 02:30'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-11-04 02:30")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse(year + "-11-04 02:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-11-04 03:00'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-11-04 03:00")) + "'");
            Assert.AreEqual(9, zone.ToUniversalTime(DateTime.Parse(year + "-11-04 03:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-11-04 03:30'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-11-04 03:30")) + "'");
            Assert.AreEqual(9, zone.ToUniversalTime(DateTime.Parse(year + "-11-04 03:30")).Hour);
            Console.WriteLine("zone.ToUniversalTime('" + year + "-12-12 03:00'): '" + zone.ToUniversalTime(DateTime.Parse(year + "-12-12 03:00")) + "'");
            Assert.AreEqual(9, zone.ToUniversalTime(DateTime.Parse(year + "-12-04 03:00")).Hour);
        }

        [Test]
        public void WorkItem12159_3()
        {
            TzTimeZone zone = TzTimeZone.ZoneUsCentral;
            Console.WriteLine("zone.ToUniversalTime('2006-04-02 01:00'): '" + zone.ToUniversalTime(DateTime.Parse("2006-04-02 01:00")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2006-04-02 01:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-04-02 02:00'): '" + zone.ToUniversalTime(DateTime.Parse("2006-04-02 02:00")) + "'");
            Assert.AreEqual(7, zone.ToUniversalTime(DateTime.Parse("2006-04-02 02:00")).Hour);
            Console.WriteLine("zone.ToUniversalTime('2006-04-02 03:00'): '" + zone.ToUniversalTime(DateTime.Parse("2006-04-02 03:00")) + "'");
            Assert.AreEqual(8, zone.ToUniversalTime(DateTime.Parse("2006-04-02 03:00")).Hour);
            Console.WriteLine(zone.ToLocalTime(new DateTime(DateTime.Parse("2006-04-02 07:00").Ticks, DateTimeKind.Utc)));
        }
    }
}
