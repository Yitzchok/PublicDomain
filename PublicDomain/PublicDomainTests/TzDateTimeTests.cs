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

            AssertIsDaylightSavingsTime(zone, "12/15/2007", false);
            AssertIsDaylightSavingsTime(zone, "6/1/2007", true);
            AssertIsDaylightSavingsTime(zone, "1/1/2007", false);
            AssertIsDaylightSavingsTime(zone, "3/12/2007", true);
            AssertIsDaylightSavingsTime(zone, "11/5/2007", false);

            AssertIsDaylightSavingsTime(zone, "1/1/2006", false);
            AssertIsDaylightSavingsTime(zone, "4/3/2006", true);
            AssertIsDaylightSavingsTime(zone, "10/30/2006", false);
            AssertIsDaylightSavingsTime(zone, "6/1/2006", true);
            AssertIsDaylightSavingsTime(zone, "12/15/2006", false);
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
            Console.WriteLine("zone.IsDaylightSavingTime(#12/15/2007#): '" + zone.IsDaylightSavingTime(DateTime.Parse("12/15/2007")) + "'");
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
    }
}
