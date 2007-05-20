using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace PublicDomain
{
    /// <summary>
    /// Methods to help in date and time manipulation.
    /// </summary>
    public static class DateTimeUtlities
    {
        /// <summary>
        /// Parses the month.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static Month ParseMonth(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            if (str.Length < 3)
            {
                throw new ArgumentException("Month should be at least 3 characters (" + str + ")");
            }
            str = str.ToLower().Trim().Substring(0, 3);
            switch (str)
            {
                case "jan":
                    return Month.January;
                case "feb":
                    return Month.February;
                case "mar":
                    return Month.March;
                case "apr":
                    return Month.April;
                case "may":
                    return Month.May;
                case "jun":
                    return Month.June;
                case "jul":
                    return Month.July;
                case "aug":
                    return Month.August;
                case "sep":
                    return Month.September;
                case "oct":
                    return Month.October;
                case "nov":
                    return Month.November;
                case "dec":
                    return Month.December;
                default:
                    throw new DateException("Unknown month " + str);
            }
        }

        /// <summary>
        /// Parses the day of week.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static DayOfWeek ParseDayOfWeek(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            if (str.Length < 3)
            {
                throw new ArgumentException("Day of week should be at least 3 characters (" + str + ")");
            }
            str = str.ToLower().Trim().Substring(0, 3);
            switch (str)
            {
                case "sun":
                    return DayOfWeek.Sunday;
                case "mon":
                    return DayOfWeek.Monday;
                case "tue":
                    return DayOfWeek.Tuesday;
                case "wed":
                    return DayOfWeek.Wednesday;
                case "thu":
                    return DayOfWeek.Thursday;
                case "fri":
                    return DayOfWeek.Friday;
                case "sat":
                    return DayOfWeek.Saturday;
                default:
                    throw new DateException("Unknown day of week " + str);
            }
        }

        /// <summary>
        /// Clones the date time as UTC.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static DateTime CloneDateTimeAsUTC(DateTime time)
        {
            return new DateTime(time.Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Thrown when there is an error relating to dates.
        /// </summary>
        [Serializable]
        public class DateException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DateException"/> class.
            /// </summary>
            public DateException() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="DateException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            public DateException(string message) : base(message) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="DateException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="inner">The inner.</param>
            public DateException(string message, Exception inner) : base(message, inner) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="DateException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
            protected DateException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }

        /// <summary>
        /// Gets the last day.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <returns></returns>
        public static DateTime GetLastDay(int year, int month, DayOfWeek dayOfWeek)
        {
            // Start at the last day of the month, until we get to the day of the week
            // we were looking for
            DateTime start = new DateTime(year, month, new GregorianCalendar().GetDaysInMonth(year, month));
            for (; ; start = start.AddDays(-1))
            {
                if (start.DayOfWeek == dayOfWeek)
                {
                    return start;
                }
            }
        }

        /// <summary>
        /// Determines whether [is time span negative] [the specified time span].
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns>
        /// 	<c>true</c> if [is time span negative] [the specified time span]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTimeSpanNegative(TimeSpan timeSpan)
        {
            return timeSpan.ToString().IndexOf('-') != -1;
        }

        /// <summary>
        /// Converts the time span to double.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns></returns>
        public static double ConvertTimeSpanToDouble(string timeSpan)
        {
            return ConvertTimeSpanToDouble(DateTimeUtlities.ParseTimeSpan(timeSpan));
        }

        /// <summary>
        /// Converts the time span to integer.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns></returns>
        public static double ConvertTimeSpanToDouble(TimeSpan timeSpan)
        {
            return timeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// Parses the time span. TimeSpan.Parse does not accept
        /// a plus (+) designator, only minus (-). This parse method
        /// accepts both. Does not throw any exceptions, but returns
        /// false on failure. Return true on success.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryParseTimeSpan(string timeSpan, out TimeSpan result)
        {
            result = TimeSpan.Zero;
            try
            {
                result = ParseTimeSpan(timeSpan);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// Parses the time span. TimeSpan.Parse does not accept
        /// a plus (+) designator, only minus (-). This parse method
        /// accepts both.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns></returns>
        public static TimeSpan ParseTimeSpan(string timeSpan)
        {
            if (timeSpan != null && timeSpan[0] == '+')
            {
                timeSpan = timeSpan.Substring(1);
            }
            return TimeSpan.Parse(timeSpan);
        }
    }
}
