using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Xml.Serialization;

namespace PublicDomain
{
    /// <summary>
    /// Wraps DateTime to provide time zone information
    /// with an <see cref="PublicDomain.TzTimeZone" /> from
    /// the Olson tz database.
    /// </summary>
    [Serializable]
    [XmlType(Namespace = GlobalConstants.PublicDomainNamespace)]
    [SoapType(Namespace = GlobalConstants.PublicDomainNamespace)]
    public class TzDateTime
    {
        /// <summary>
        /// +00:00
        /// </summary>
        public const string UtcOffsetModifier = "+00:00";

        private DateTime m_dateTimeUtc;
        private TzTimeZone m_timeZone;

        /// <summary>
        /// Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        public TzDateTime()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        public TzDateTime(DateTime time)
            : this(time, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="forceTimeAsUtc">if set to <c>true</c> [force time as utc].</param>
        public TzDateTime(DateTime time, bool forceTimeAsUtc)
            : this(forceTimeAsUtc ? DateTimeUtlities.CloneDateTimeAsUTC(time) : time)
        {
        }

        /// <summary>
        /// Assumes a local date/time. Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        public TzDateTime(int year, int month, int day)
            : this(year, month, day, null)
        {
        }

        /// <summary>
        /// Assumes a local date/time. Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="timeZone">The time zone.</param>
        public TzDateTime(int year, int month, int day, TzTimeZone timeZone)
            : this(year, month, day, 0, 0, 0, 0, timeZone)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="kind">The kind.</param>
        /// <param name="timeZone">The time zone.</param>
        public TzDateTime(int year, int month, int day, DateTimeKind kind, TzTimeZone timeZone)
            : this(year, month, day, 0, 0, 0, 0, kind, timeZone)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="kind">The kind.</param>
        public TzDateTime(int year, int month, int day, DateTimeKind kind)
            : this(year, month, day, 0, 0, 0, 0, kind, null)
        {
        }

        /// <summary>
        /// Assumes a local date/time. Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        public TzDateTime(int year, int month, int day, int hour, int minutes, int seconds)
            : this(year, month, day, hour, minutes, seconds, 0, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="kind">The kind.</param>
        public TzDateTime(int year, int month, int day, int hour, int minutes, int seconds, DateTimeKind kind)
            : this(year, month, day, hour, minutes, seconds, 0, kind, null)
        {
        }

        /// <summary>
        /// Assumes a local date/time. Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="timeZone">The time zone.</param>
        public TzDateTime(int year, int month, int day, int hour, int minutes, int seconds, TzTimeZone timeZone)
            : this(year, month, day, hour, minutes, seconds, 0, DateTimeKind.Local, timeZone)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="kind">The kind.</param>
        /// <param name="timeZone">The time zone.</param>
        public TzDateTime(int year, int month, int day, int hour, int minutes, int seconds, DateTimeKind kind, TzTimeZone timeZone)
            : this(year, month, day, hour, minutes, seconds, 0, kind, timeZone)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="millisecond">The millisecond.</param>
        /// <param name="kind">The kind.</param>
        public TzDateTime(int year, int month, int day, int hour, int minutes, int seconds, int millisecond, DateTimeKind kind)
            : this(year, month, day, hour, minutes, seconds, millisecond, kind, null)
        {
        }

        /// <summary>
        /// Assumes a local date/time. Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="millisecond">The millisecond.</param>
        /// <param name="timeZone">The time zone.</param>
        public TzDateTime(int year, int month, int day, int hour, int minutes, int seconds, int millisecond, TzTimeZone timeZone)
            : this(year, month, day, hour, minutes, seconds, millisecond, DateTimeKind.Local, timeZone)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="millisecond">The millisecond.</param>
        /// <param name="kind">The kind.</param>
        /// <param name="timeZone">The time zone.</param>
        public TzDateTime(int year, int month, int day, int hour, int minutes, int seconds, int millisecond, DateTimeKind kind, TzTimeZone timeZone)
            : this(new DateTime(year, month, day, hour, minutes, seconds, millisecond, kind), timeZone)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="timeZone">The time zone.</param>
        public TzDateTime(DateTime time, TzTimeZone timeZone)
        {
            if (timeZone == null && (time.Kind == DateTimeKind.Local || TzTimeZone.TreatUnspecifiedKindAsLocal && time.Kind == DateTimeKind.Unspecified))
            {
                throw new ArgumentException("A date/time with DateTimeKind Local or Unspecified must be initialized with a time zone. Otherwise, the date/time is ambiguous. You must explicitly provide a time zone for predictable results, for example, you may provide the local time zone of the computer running this program.", "timeZone");
            }
            m_timeZone = timeZone;
            SetDateTime(time);
        }

        private void SetDateTime(DateTime time)
        {
            switch (time.Kind)
            {
                case DateTimeKind.Local:
                    ThrowIfNullTimeZone();
                    m_dateTimeUtc = m_timeZone.ToUniversalTime(time);
                    break;
                case DateTimeKind.Unspecified:
                    if (TzTimeZone.TreatUnspecifiedKindAsLocal)
                    {
                        ThrowIfNullTimeZone();
                        m_dateTimeUtc = m_timeZone.ToUniversalTime(time);
                        break;
                    }
                    else
                    {
                        throw new ArgumentException("unspecified kind");
                    }
                case DateTimeKind.Utc:
                    m_dateTimeUtc = time;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the date time utc.
        /// </summary>
        /// <value>The date time utc.</value>
        public DateTime DateTimeUtc
        {
            get
            {
                return m_dateTimeUtc;
            }
            set
            {
                SetDateTime(value);
            }
        }

        /// <summary>
        /// Gets the date time local.
        /// </summary>
        /// <value>The date time local.</value>
        [XmlIgnore]
        [SoapIgnore]
        public DateTime DateTimeLocal
        {
            get
            {
                ThrowIfNullTimeZone();
                return m_timeZone.ToLocalTime(m_dateTimeUtc);
            }
            set
            {
                SetDateTime(value);
            }
        }

        private void ThrowIfNullTimeZone()
        {
            if (m_timeZone == null)
            {
                throw new Exception("Time zone not specified to retrieve local date.");
            }
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public TzTimeZone TimeZone
        {
            get
            {
                return m_timeZone;
            }
            set
            {
                m_timeZone = value;
            }
        }

        /// <summary>
        /// Gets the utc offset.
        /// </summary>
        /// <value>The utc offset.</value>
        public TimeSpan UtcOffset
        {
            get
            {
                ThrowIfNullTimeZone();
                return TimeZone.GetUtcOffset(m_dateTimeUtc);
            }
        }

        /// <summary>
        /// Parses the specified input. By default, this does
        /// not assume any time zone -- not even UTC. If the time
        /// zone cannot be parsed from the input, a null time zone
        /// is set in the date.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static TzDateTime Parse(string input)
        {
            return Parse(input, null, DateTimeStyles.None);
        }

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="styles">The styles.</param>
        /// <returns></returns>
        public static TzDateTime Parse(string input, DateTimeStyles styles)
        {
            return Parse(input, null, styles);
        }

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns></returns>
        public static TzDateTime Parse(string input, TzTimeZone timeZone)
        {
            return Parse(input, timeZone, DateTimeStyles.None);
        }

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="styles">The styles.</param>
        /// <returns></returns>
        public static TzDateTime Parse(string input, TzTimeZone timeZone, DateTimeStyles styles)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            input = input.Trim();

            // First, see if this is an ISO 8601 date
            TzDateTime result;
            if (!Iso8601.TryParse(input, timeZone, out result))
            {
                // If it's not ISO 8601, we use the normal DateTime.Parse
                // method; however, we also check if there is a time zone
                // designator on the end
                if (input.Length >= UtcOffsetModifier.Length)
                {
                    // There may be a time zone designator
                    if (input[input.Length - UtcOffsetModifier.Length] == '+' ||
                        input[input.Length - UtcOffsetModifier.Length] == '-')
                    {
                        // Looks like there is a time zone designator
                        char modifier = input[input.Length - UtcOffsetModifier.Length];
                        string[] pieces = StringUtilities.SplitAroundLastIndexOfAny(input, '+', '-');
                        input = pieces[0];
                        timeZone = TzTimeZone.GetTimeZoneByOffset(modifier + pieces[1]);
                    }
                }

                result = new TzDateTime(DateTime.Parse(input), timeZone);
            }
            return result;
        }

        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="styles">The styles.</param>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static bool TryParse(string input, DateTimeStyles styles, out TzDateTime val)
        {
            return TryParse(input, null, styles, out val);
        }

        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static bool TryParse(string input, out TzDateTime val)
        {
            return TryParse(input, null, DateTimeStyles.None, out val);
        }

        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static bool TryParse(string input, TzTimeZone timeZone, out TzDateTime val)
        {
            return TryParse(input, timeZone, DateTimeStyles.None, out val);
        }

        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="styles">The styles.</param>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static bool TryParse(string input, TzTimeZone timeZone, DateTimeStyles styles, out TzDateTime val)
        {
            val = null;
            try
            {
                val = Parse(input, timeZone, styles);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// Returns the UTC form of this date time. Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return DateTimeUtc.ToString() + UtcOffsetModifier;
        }

        /// <summary>
        /// Returns the UTC form of this date time. Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public string ToString(IFormatProvider provider)
        {
            return DateTimeUtc.ToString(provider) + UtcOffsetModifier;
        }

        /// <summary>
        /// Returns the UTC form of this date time. Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public string ToString(string format)
        {
            return DateTimeUtc.ToString(format) + UtcOffsetModifier;
        }

        /// <summary>
        /// Returns the UTC form of this date time. Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return DateTimeUtc.ToString(format, provider) + UtcOffsetModifier;
        }

        /// <summary>
        /// Returns the local form of this date time. Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public string ToStringLocal()
        {
            return DateTimeLocal.ToString();
        }

        /// <summary>
        /// Returns the local form of this date time. Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public string ToStringLocal(IFormatProvider provider)
        {
            return DateTimeLocal.ToString(provider);
        }

        /// <summary>
        /// Returns the local form of this date time. Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public string ToStringLocal(string format)
        {
            return DateTimeLocal.ToString(format);
        }

        /// <summary>
        /// Returns the local form of this date time. Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public string ToStringLocal(string format, IFormatProvider provider)
        {
            return DateTimeLocal.ToString(format, provider);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            TzDateTime y = obj as TzDateTime;
            if (y != null)
            {
                return DateTimeUtc.Equals(y.DateTimeUtc);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return DateTimeUtc.GetHashCode();
        }

        /// <summary>
        /// Gets the utc now.
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        /// <returns></returns>
        public static TzDateTime GetUtcNow(TzTimeZone timeZone)
        {
            return new TzDateTime(DateTime.UtcNow, timeZone);
        }

        /// <summary>
        /// Adds the days.
        /// </summary>
        /// <param name="days">The days.</param>
        /// <returns></returns>
        public TzDateTime AddDays(double days)
        {
            return new TzDateTime(DateTimeUtc.AddDays(days), TimeZone);
        }

        /// <summary>
        /// Adds the hours.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <returns></returns>
        public TzDateTime AddHours(double hours)
        {
            return new TzDateTime(DateTimeUtc.AddHours(hours), TimeZone);
        }

        /// <summary>
        /// Adds the months.
        /// </summary>
        /// <param name="months">The months.</param>
        /// <returns></returns>
        public TzDateTime AddMonths(int months)
        {
            return new TzDateTime(DateTimeUtc.AddMonths(months), TimeZone);
        }

        /// <summary>
        /// Adds the seconds.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public TzDateTime AddSeconds(double seconds)
        {
            return new TzDateTime(DateTimeUtc.AddSeconds(seconds), TimeZone);
        }

        /// <summary>
        /// Adds the minutes.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        public TzDateTime AddMinutes(double minutes)
        {
            return new TzDateTime(DateTimeUtc.AddMinutes(minutes), TimeZone);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public TzDateTime Clone()
        {
            TzDateTime ret = (TzDateTime)MemberwiseClone();
            return ret;
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public int CompareTo(TzDateTime y)
        {
            return DateTimeUtc.CompareTo(y.DateTimeUtc);
        }

        /// <summary>
        /// Returns a new instance with the saved time zone, but
        /// with the hours, minutes, and seconds set to 0.
        /// </summary>
        /// <returns></returns>
        public TzDateTime GetDate()
        {
            return new TzDateTime(DateTimeUtc.Date, TimeZone);
        }

        /// <summary>
        /// Returns a new instance with the saved time zone, but
        /// with the hours, minutes, and seconds set to 0.
        /// </summary>
        /// <returns></returns>
        public TzDateTime GetDateLocal()
        {
            return new TzDateTime(DateTimeLocal.Date, TimeZone);
        }

        /// <summary>
        /// Gets the date local.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="tz">The tz.</param>
        /// <returns></returns>
        public static DateTime GetDateLocal(TzDateTime dt, TzTimeZone tz)
        {
            return tz.ToLocalTime(dt.DateTimeUtc);
        }

        /// <summary>
        /// Subtracts the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static TimeSpan Subtract(TzDateTime x, TzDateTime y)
        {
            return x.DateTimeUtc - y.DateTimeUtc;
        }

        /// <summary>
        /// Adds the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static TzDateTime Add(TzDateTime x, TimeSpan y)
        {
            return new TzDateTime(x.DateTimeUtc + y, x.TimeZone);
        }

        /// <summary>
        /// Greaters the than equal to.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static bool GreaterThanEqualTo(TzDateTime x, TzDateTime y)
        {
            return x.DateTimeUtc >= y.DateTimeUtc;
        }

        /// <summary>
        /// Greaters the than.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static bool GreaterThan(TzDateTime x, TzDateTime y)
        {
            return x.DateTimeUtc > y.DateTimeUtc;
        }

        /// <summary>
        /// Lesses the than.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static bool LessThan(TzDateTime x, TzDateTime y)
        {
            return x.DateTimeUtc < y.DateTimeUtc;
        }

        /// <summary>
        /// Lesses the than equal to.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static bool LessThanEqualTo(TzDateTime x, TzDateTime y)
        {
            return x.DateTimeUtc <= y.DateTimeUtc;
        }
    }
}
