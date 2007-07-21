using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace PublicDomain
{
    /// <summary>
    /// Parses the tz database files.
    /// 
    /// Notes:
    /// * The zone.tab file is a mapping between ISO 3166 2-character country codes
    /// and the main ZONE for that country.
    /// * See the 'Theory' file in tzcode
    /// </summary>
    [Serializable]
    public class TzDatabase
    {
        /// <summary>
        /// -
        /// </summary>
        public const string NotApplicableValue = "-";

        /// <summary>
        /// 
        /// </summary>
        public const string TzDatabaseDirectory = @"..\..\..\tzdata\";

        /// <summary>
        /// 
        /// </summary>
        public const string Iso3166TabFile = TzDatabaseDirectory + @"iso3166.tab";

        /// <summary>
        /// 
        /// </summary>
        public const string ZoneTabFile = TzDatabaseDirectory + @"zone.tab";

        /// <summary>
        /// 
        /// </summary>
        public const string FactoryZoneName = "Factory";

        /// <summary>
        /// Reads the tz database from the specific <paramref name="dir"/>.
        /// All files without extensions are checked for relevant data. The
        /// directory is not recursively searched. Parameters <paramref name="rules"/>,
        /// <paramref name="zones"/>, and <paramref name="links"/> should be non-null
        /// arrays into which the database will be added.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="rules">The rules.</param>
        /// <param name="zones">The zones.</param>
        /// <param name="links">The links.</param>
        public static void ReadDatabase(string dir, List<TzRule> rules, List<TzZone> zones, List<string[]> links)
        {
            if (dir == null)
            {
                throw new ArgumentNullException("dir");
            }
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            FileInfo[] files = dirInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                // If there is no file extension, we assume
                // that it is a data file.
                if (string.IsNullOrEmpty(file.Extension))
                {
                    ReadDatabaseFile(file, rules, zones, links);
                }
            }
        }

        /// <summary>
        /// Reads the database file.
        /// 
        /// See zic.txt in tzcode
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="rules">The rules.</param>
        /// <param name="zones">The zones.</param>
        /// <param name="links">The links.</param>
        /// <exception cref="PublicDomain.TzDatabase.TzException"/>
        private static void ReadDatabaseFile(FileInfo file, List<TzRule> rules, List<TzZone> zones, List<string[]> links)
        {
            string[] lines = System.IO.File.ReadAllLines(file.FullName);
            TzZone tempZone;
            int length = lines.Length;
            for (int i = 0; i < length; i++)
            {
                string line = lines[i];
                if (line == null)
                {
                    continue;
                }

                // This line may be a continuation Zone
                if (line.Length > 0)
                {
                    // Avoid comment lines
                    string leftTrimmed = line.TrimStart();

                    if (leftTrimmed.Length > 0 && leftTrimmed[0] != '#')
                    {
                        // Non-comment line
                        if (char.IsWhiteSpace(line[0]) && zones.Count > 0)
                        {
                            if (line.Trim() != string.Empty)
                            {
                                // This is a continuation of a previous Zone
                                TzZone previousZone = zones[zones.Count - 1];
                                zones.Add(TzDatabase.CloneDataZone(previousZone, line));
                            }
                        }
                        else
                        {
                            string[] pieces = StringUtilities.SplitQuoteSensitive(line, true, '\"', '#');
                            if (pieces.Length > 0)
                            {
                                switch (pieces[0].ToLower())
                                {
                                    case "rule":
                                        rules.Add(TzDatabase.ParseDataRule(line));
                                        break;
                                    case "zone":
                                        tempZone = TzDatabase.ParseDataZone(line);
                                        if (tempZone.ZoneName != FactoryZoneName)
                                        {
                                            zones.Add(tempZone);
                                        }
                                        break;
                                    case "link":
                                        links.Add(StringUtilities.RemoveEmptyPieces(pieces));
                                        break;
                                    case "leap":
                                        // Not yet handled
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static TzZone FindTzDataZone(List<TzZone> zones, string zoneName)
        {
            foreach (TzZone zone in zones)
            {
                if (zone.ZoneName.Equals(zoneName))
                {
                    return zone;
                }
            }
            throw new TzParseException("Could not find LINKed zone {0}", zoneName);
        }

        /// <summary>
        /// Parses the tz database iso3166.tab file and returns a map
        /// which maps the ISO 3166 two letter country code to the
        /// country name.
        /// </summary>
        /// <param name="iso3166TabFile">The iso3166 tab file.</param>
        /// <returns></returns>
        public static Dictionary<string, Iso3166> ParseIso3166Tab(string iso3166TabFile)
        {
            Dictionary<string, Iso3166> map = new Dictionary<string, Iso3166>();
            string[] lines = System.IO.File.ReadAllLines(iso3166TabFile);
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line) && line[0] != '#')
                {
                    // We expect two characters for the country code,
                    // followed by the country name
                    Iso3166 iso = new Iso3166(line.Substring(0, 2), line.Substring(2).Trim());
                    map[iso.TwoLetterCode] = iso;
                }
            }
            return map;
        }

        /// <summary>
        /// Parses the tz database zone.tab file into all the zone descriptions.
        /// 
        /// From 'Theory' file:
        /// "The file 'zone.tab' lists the geographical locations used to name
        /// time zone rule files.  It is intended to be an exhaustive list
        /// of canonical names for geographic regions."
        /// </summary>
        /// <param name="tabFile"></param>
        /// <returns></returns>
        public static List<PublicDomain.TzTimeZone.TzZoneDescription> ParseZoneTab(string tabFile)
        {
            List<PublicDomain.TzTimeZone.TzZoneDescription> result = new List<PublicDomain.TzTimeZone.TzZoneDescription>();
            string[] lines = System.IO.File.ReadAllLines(tabFile);
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line) && line[0] != '#')
                {
                    string[] pieces = line.Split('\t');
                    result.Add(new PublicDomain.TzTimeZone.TzZoneDescription(pieces[0], Iso6709.Parse(pieces[1]), pieces[2], pieces.Length > 3 ? pieces[3] : null));
                }
            }
            return result;
        }

        /// <summary>
        /// Logical representation of a RULE field in the tz database.
        /// </summary>
        [Serializable]
        public class TzRule : IComparable<TzRule>
        {
            /// <summary>
            /// 
            /// </summary>
            public const string ModifierDaylight = "D";

            /// <summary>
            /// 
            /// </summary>
            public const string ModifierStandard = "S";

            /// <summary>
            /// 
            /// </summary>
            public const string ModifierWar = "W";

            /// <summary>
            /// 
            /// </summary>
            public const string ModifierPeace = "P";

            /// <summary>
            /// Each Zone rule has a name which each zone refers to.
            /// A zone rule name is not unique in its own right, but
            /// only with all of its properties. Therefore, there may
            /// be multiple zone rules with the same name but different properties.
            /// </summary>
            public string RuleName;

            /// <summary>
            /// The effective year the zone rule is effective on.
            /// </summary>
            public int FromYear;

            /// <summary>
            /// The year the zone rule effectively ends.
            /// </summary>
            public int ToYear;

            /// <summary>
            /// The integer month the rule starts on. January = 1, February = 2, ..., December = 12
            /// </summary>
            public Month StartMonth;

            /// <summary>
            /// The day of the month the rule starts on.
            /// </summary>
            public int StartDay = -1;

            /// <summary>
            /// The day of the month is optionally modified by
            /// a day of week.
            /// </summary>
            public DayOfWeek? StartDay_DayOfWeek;

            /// <summary>
            /// The time of the day the Rule starts on.
            /// </summary>
            public TimeSpan StartTime;

            /// <summary>
            /// 
            /// </summary>
            public string StartTimeModifier;

            /// <summary>
            /// The amount of time saved by the Rule.
            /// </summary>
            public TimeSpan SaveTime;

            /// <summary>
            /// The character D means daylight savings time.
            /// The character S means standard time.
            /// Other characters may also be presents, such as
            /// W for War, P for Peace, etc.
            /// </summary>
            public string Modifier;

            /// <summary>
            /// 
            /// </summary>
            public string Comment;

            /// <summary>
            /// Initializes a new instance of the <see cref="PublicDomain.TzDatabase.TzRule"/> class.
            /// </summary>
            public TzRule()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="PublicDomain.TzDatabase.TzRule"/> class.
            /// </summary>
            /// <param name="ruleName">Name of the rule.</param>
            /// <param name="fromYear">From.</param>
            /// <param name="toYear">To.</param>
            /// <param name="startMonth">The start month.</param>
            /// <param name="startDay">The start day.</param>
            /// <param name="startDay_dayOfWeek">The start day_day of week.</param>
            /// <param name="startTime">The start time.</param>
            /// <param name="startTimeModifier">The start time modifier.</param>
            /// <param name="saveTime">The save time.</param>
            /// <param name="modifier">The modifier.</param>
            /// <param name="comment">The comment.</param>
            public TzRule(string ruleName, int fromYear, int toYear, Month startMonth, int startDay,
                DayOfWeek? startDay_dayOfWeek, TimeSpan startTime, string startTimeModifier,
                TimeSpan saveTime, string modifier, string comment)
            {
                RuleName = ruleName;
                FromYear = fromYear;
                ToYear = toYear;
                StartMonth = startMonth;
                StartDay = startDay;
                StartDay_DayOfWeek = startDay_dayOfWeek;
                StartTime = startTime;
                StartTimeModifier = startTimeModifier;
                SaveTime = saveTime;
                Modifier = modifier;
                Comment = comment;
            }

            /// <summary>
            /// Gets the on date time.
            /// </summary>
            /// <returns></returns>
            public DateTime GetOnDateTime()
            {
                return GetOnDateTime(1);
            }

            /// <summary>
            /// Gets the on date time.
            /// </summary>
            /// <param name="year">The year.</param>
            /// <returns></returns>
            public DateTime GetOnDateTime(int year)
            {
                return GetDateTime(year, StartMonth, StartDay, StartDay_DayOfWeek, StartTime);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool DoesStartLastDay()
            {
                return StartDay_DayOfWeek != null && StartDay == -1;
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                string result = "Rule";
                result += "\t" + RuleName;
                result += "\t" + FromYear;
                result += "\t" + ToYear;
                result += "\t" + NotApplicableValue;
                result += "\t" + StartMonth.ToString().Substring(0, 3);
                result += "\t";
                if (DoesStartLastDay())
                {
                    result += "last" + this.StartDay_DayOfWeek.Value.ToString().Substring(0, 3);
                }
                else
                {
                    if (this.StartDay_DayOfWeek != null)
                    {
                        result += this.StartDay_DayOfWeek.Value.ToString().Substring(0, 3);
                        result += ">=";
                    }
                    result += this.StartDay;
                }
                result += "\t" + this.StartTime + StartTimeModifier;
                result += " " + this.SaveTime;
                result += "\t" + this.Modifier;
                if (!string.IsNullOrEmpty(Comment))
                {
                    result += " # " + Comment;
                }
                return result;
            }

            /// <summary>
            /// Gets the object string.
            /// </summary>
            /// <returns></returns>
            public string GetObjectString()
            {
                string result = string.Format("new PublicDomain.TzDatabase.TzRule({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})",
                    '\"' + RuleName + '\"',
                    FromYear,
                    ToYear,
                    StartMonth == 0 ? "0" : "PublicDomain.Month." + StartMonth.ToString(),
                    StartDay,
                    StartDay_DayOfWeek == null ? "null" : "DayOfWeek." + StartDay_DayOfWeek.Value.ToString(),
                    "new TimeSpan(" + StartTime.Ticks + ")",
                    StartTimeModifier == null ? "null" : '\"' + StartTimeModifier + '\"',
                    "new TimeSpan(" + SaveTime.Ticks + ")",
                    Modifier == null ? "null" : '\"' + Modifier + '\"',
                    Comment == null ? "null" : '\"' + Comment + '\"'
                );
                return result;
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
                TzRule c = obj as TzRule;
                if (c != null)
                {
                    return c.RuleName == RuleName &&
                        c.FromYear == FromYear &&
                        c.ToYear == ToYear &&
                        c.StartMonth == StartMonth &&
                        c.StartDay == StartDay &&
                        c.StartDay_DayOfWeek == StartDay_DayOfWeek &&
                        c.StartTime == StartTime &&
                        c.StartTimeModifier == StartTimeModifier &&
                        c.SaveTime == SaveTime;
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
                return base.GetHashCode();
            }

            /// <summary>
            /// Compares the current object with another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.
            /// </returns>
            public int CompareTo(TzRule other)
            {
                if (Equals(other))
                {
                    return 0;
                }
                else
                {
                    if ((FromYear > other.FromYear) ||
                        (FromYear == other.FromYear && StartMonth > other.StartMonth) ||
                        (FromYear == other.FromYear && StartMonth == other.StartMonth && StartDay > other.StartDay))
                    {
                        return 1;
                    }
                    return -1;
                }
            }
        }

        /// <summary>
        /// Logical representation of a ZONE data field in the tz database.
        /// </summary>
        [Serializable]
        public class TzZone : ICloneable, IComparable<TzZone>
        {
            /// <summary>
            /// Unique name for each zone, for example America/New_York, though
            /// there can be multiple zones with the same name but different properties.
            /// </summary>
            public string ZoneName;

            /// <summary>
            /// Offset from UTC
            /// </summary>
            public TimeSpan UtcOffset;

            /// <summary>
            /// The rule that applies to this zone.
            /// </summary>
            public string RuleName;

            /// <summary>
            /// 
            /// </summary>
            public string Format;

            /// <summary>
            /// 
            /// </summary>
            public int UntilYear;

            /// <summary>
            /// 
            /// </summary>
            public Month UntilMonth = 0;

            /// <summary>
            /// 
            /// </summary>
            public int UntilDay = -1;

            /// <summary>
            /// 
            /// </summary>
            public DayOfWeek? UntilDay_DayOfWeek;

            /// <summary>
            /// 
            /// </summary>
            public TimeSpan UntilTime;

            /// <summary>
            /// 
            /// </summary>
            public string UntilTimeModifier;

            /// <summary>
            /// 
            /// </summary>
            public string Comment;

            /// <summary>
            /// Caches the until time value
            /// </summary>
            private DateTime? m_cachedUntilTime;

            /// <summary>
            /// Initializes a new instance of the <see cref="PublicDomain.TzDatabase.TzZone"/> class.
            /// </summary>
            public TzZone()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="PublicDomain.TzDatabase.TzZone"/> class.
            /// </summary>
            /// <param name="zoneName">Name of the zone.</param>
            /// <param name="utcOffset">The utc offset.</param>
            /// <param name="ruleName">Name of the rule.</param>
            /// <param name="format">The format.</param>
            /// <param name="untilYear">The until year.</param>
            /// <param name="untilMonth">The until month.</param>
            /// <param name="untilDay">The until day.</param>
            /// <param name="untilDay_dayOfWeek">The until day_day of week.</param>
            /// <param name="untilTime">The until time.</param>
            /// <param name="untilTimeModifier">The until time modifier.</param>
            /// <param name="comment">The comment.</param>
            public TzZone(string zoneName, TimeSpan utcOffset, string ruleName,
                string format, int untilYear, Month untilMonth, int untilDay, DayOfWeek? untilDay_dayOfWeek,
                TimeSpan untilTime, string untilTimeModifier, string comment)
            {
                ZoneName = zoneName;
                UtcOffset = utcOffset;
                RuleName = ruleName;
                Format = format;
                UntilYear = untilYear;
                UntilMonth = untilMonth;
                UntilDay = untilDay;
                UntilDay_DayOfWeek = untilDay_dayOfWeek;
                UntilTime = untilTime;
                UntilTimeModifier = untilTimeModifier;
                Comment = comment;
            }

            /// <summary>
            /// Gets the until date time.
            /// </summary>
            /// <returns></returns>
            public DateTime GetUntilDateTime()
            {
                if (m_cachedUntilTime == null)
                {
                    m_cachedUntilTime = GetDateTime(UntilYear, UntilMonth, UntilDay, UntilDay_DayOfWeek, UntilTime);
                }
                return m_cachedUntilTime.Value;
            }

            /// <summary>
            /// Determines whether [is greater than until] [the specified point].
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns>
            /// 	<c>true</c> if [is greater than until] [the specified point]; otherwise, <c>false</c>.
            /// </returns>
            public bool IsGreaterThanUntil(DateTime point)
            {
                if (point > GetUntilDateTime())
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Determines whether this instance has rules.
            /// </summary>
            /// <returns>
            /// 	<c>true</c> if this instance has rules; otherwise, <c>false</c>.
            /// </returns>
            public bool HasRules()
            {
                TimeSpan trash;
                return !string.IsNullOrEmpty(RuleName) && !RuleName.Trim().Equals(NotApplicableValue) && !DateTimeUtlities.TryParseTimeSpan(RuleName, out trash);
            }

            /// <summary>
            /// Creates a new object that is a copy of the current instance.
            /// </summary>
            /// <returns>
            /// A new object that is a copy of this instance.
            /// </returns>
            public object Clone()
            {
                return (TzZone)MemberwiseClone();
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
                TzZone z = obj as TzZone;
                if (z != null)
                {
                    return z.ZoneName == ZoneName &&
                        z.RuleName == RuleName &&
                        z.UntilDay == UntilDay &&
                        z.UntilDay_DayOfWeek == UntilDay_DayOfWeek &&
                        z.UntilMonth == UntilMonth &&
                        z.UntilTime == UntilTime &&
                        z.UntilTimeModifier == UntilTimeModifier &&
                        z.UntilYear == UntilYear &&
                        z.UtcOffset == UtcOffset;
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
                return base.GetHashCode();
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                return ZoneName;
            }

            /// <summary>
            /// Gets the object string.
            /// </summary>
            /// <returns></returns>
            public string GetObjectString()
            {
                string result = string.Format("new PublicDomain.TzDatabase.TzZone({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})",
                    '\"' + ZoneName + '\"',
                    "new TimeSpan(" + UtcOffset.Ticks + ")",
                    '\"' + RuleName + '\"',
                    Format == null ? "null" : '\"' + Format + '\"',
                    UntilYear,
                    UntilMonth == 0 ? "0" : "PublicDomain.Month." + UntilMonth.ToString(),
                    UntilDay,
                    UntilDay_DayOfWeek == null ? "null" : ("DayOfWeek." + UntilDay_DayOfWeek.Value.ToString()),
                    "new TimeSpan(" + UntilTime.Ticks + ")",
                    UntilTimeModifier == null ? "null" : '\"' + UntilTimeModifier + '\"',
                    Comment == null ? "null" : '\"' + Comment + '\"'
                );

                return result;
            }

            /// <summary>
            /// Compares the current object with another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.
            /// </returns>
            public int CompareTo(TzZone other)
            {
                if (Equals(other))
                {
                    return 0;
                }
                else
                {
                    if (UntilYear > other.UntilYear)
                    {
                        return 1;
                    }
                    return -1;
                }
            }
        }

        /// <summary>
        /// Gets the tz data day.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="startDay">The start day.</param>
        /// <param name="startDay_dayOfWeek">The start day_day of week.</param>
        public static void GetTzDataDay(string str, out int startDay, out DayOfWeek? startDay_dayOfWeek)
        {
            startDay = 0;
            startDay_dayOfWeek = null;
            if (ConversionUtilities.IsStringAnInteger(str))
            {
                startDay = int.Parse(str);
            }
            else
            {
                if (str.Contains(">="))
                {
                    startDay_dayOfWeek = DateTimeUtlities.ParseDayOfWeek(str.Trim().Substring(0, 3));
                    startDay = int.Parse(str.Substring(str.LastIndexOf('=') + 1));
                }
                else if (str.ToLower().StartsWith("last"))
                {
                    startDay_dayOfWeek = DateTimeUtlities.ParseDayOfWeek(str.Substring("last".Length));
                    startDay = -1;
                }
            }
        }

        /// <summary>
        /// Gets the tz data time.
        /// </summary>
        /// <param name="saveTime">The save time.</param>
        /// <param name="timeModifier">The time modifier.</param>
        /// <returns></returns>
        public static TimeSpan GetTzDataTime(string saveTime, out string timeModifier)
        {
            timeModifier = null;
            if (char.IsLetter(saveTime[saveTime.Length - 1]))
            {
                timeModifier = saveTime[saveTime.Length - 1].ToString();
                saveTime = saveTime.Substring(0, saveTime.Length - 1);
            }
            return DateTimeUtlities.ParseTimeSpan(saveTime);
        }

        /// <summary>
        /// Parses the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static TzRule ParseDataRule(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            TzRule t = new TzRule();
            string[] pieces = StringUtilities.SplitQuoteSensitive(str, true, '\"', '#');
            if (pieces.Length != 10 && pieces.Length != 11)
            {
                throw new TzParseException("Rule has an invalid number of pieces: {0}, expecting {1} ({2})", pieces.Length, 10, str);
            }
            t.RuleName = pieces[1];
            if (pieces[2] == "min")
            {
                t.FromYear = 0;
            }
            else
            {
                t.FromYear = int.Parse(pieces[2]);
            }
            switch (pieces[3])
            {
                case "only":
                    t.ToYear = t.FromYear;
                    break;
                case "max":
                    t.ToYear = int.MaxValue;
                    break;
                default:
                    t.ToYear = int.Parse(pieces[3]);
                    break;
            }
            t.StartMonth = DateTimeUtlities.ParseMonth(pieces[5]);

            TzDatabase.GetTzDataDay(pieces[6], out t.StartDay, out t.StartDay_DayOfWeek);

            t.StartTime = TzDatabase.GetTzDataTime(pieces[7], out t.StartTimeModifier);
            t.SaveTime = DateTimeUtlities.ParseTimeSpan(pieces[8]);
            t.Modifier = pieces[9];
            if (pieces.Length == 11)
            {
                t.Comment = pieces[10].Trim();
            }
            return t;
        }

        /// <summary>
        /// Parses the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static TzZone ParseDataZone(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            TzZone z = new TzZone();
            ParsePieces(str, z);
            return z;
        }

        /// <summary>
        /// Parses the pieces.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="z">The z.</param>
        private static void ParsePieces(string str, TzZone z)
        {
            string[] pieces = StringUtilities.SplitQuoteSensitive(str, true, '\"', '#');
            z.ZoneName = pieces[1];

            if (z.ZoneName != FactoryZoneName)
            {
                z.UtcOffset = DateTimeUtlities.ParseTimeSpan(pieces[2]);
                z.RuleName = pieces[3];
                z.Format = pieces[4];

                // The rest of the format is optional an erratic, so we combine
                // the rest of the array into a big string
                if (pieces.Length > 5)
                {
                    if (pieces[5][0] == '#')
                    {
                        z.Comment = pieces[5].Trim();
                        SetMaxZone(z);
                    }
                    else
                    {
                        z.UntilYear = int.Parse(pieces[5]);
                        if (pieces.Length > 6)
                        {
                            if (pieces[6][0] == '#')
                            {
                                z.Comment = pieces[6].Trim();
                            }
                            else
                            {
                                z.UntilMonth = DateTimeUtlities.ParseMonth(pieces[6]);
                                if (pieces.Length > 7)
                                {
                                    if (pieces[7][0] == '#')
                                    {
                                        z.Comment = pieces[7].Trim();
                                    }
                                    else
                                    {
                                        TzDatabase.GetTzDataDay(pieces[7], out z.UntilDay, out z.UntilDay_DayOfWeek);
                                        if (pieces.Length > 8)
                                        {
                                            if (pieces[8][0] == '#')
                                            {
                                                z.Comment = pieces[8].Trim();
                                            }
                                            else
                                            {
                                                z.UntilTime = TzDatabase.GetTzDataTime(pieces[8], out z.UntilTimeModifier);
                                                if (pieces.Length > 9)
                                                {
                                                    z.Comment = pieces[9].Trim();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    z.Comment = null;
                    SetMaxZone(z);
                }
            }
        }

        private static void SetMaxZone(TzZone z)
        {
            // Reset any potential cloned date
            z.UntilYear = int.MaxValue;
            z.UntilMonth = Month.December;
            z.UntilDay = 31;
            z.UntilDay_DayOfWeek = null;
            z.UntilTime = TimeSpan.Zero;
            z.UntilTimeModifier = null;
        }

        /// <summary>
        /// Clones the specified line.
        /// </summary>
        /// <param name="zone">The zone.</param>
        /// <param name="line">The line.</param>
        /// <returns></returns>
        public static TzZone CloneDataZone(TzZone zone, string line)
        {
            TzZone z = (TzZone)zone.Clone();
            line = "Zone\t" + z.ZoneName + "\t" + string.Join("\t", StringUtilities.RemoveEmptyPieces(line.Split('\t')));
            ParsePieces(line, z);
            return z;
        }

        /// <summary>
        /// Thrown when there is an error interpreting the tz database.
        /// </summary>
        [Serializable]
        public class TzException : BaseException
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TzException"/> class.
            /// </summary>
            public TzException()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="formatParameters">The format parameters.</param>
            public TzException(string message, params object[] formatParameters)
                : base(message, formatParameters)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzException"/> class.
            /// </summary>
            /// <param name="inner">The inner.</param>
            /// <param name="message">The message.</param>
            /// <param name="formatParameters">The format parameters.</param>
            public TzException(Exception inner, string message, params object[] formatParameters)
                : base(inner, message, formatParameters)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
            protected TzException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
            }
        }

        /// <summary>
        /// Thrown when there is a parse exception parsing the tz databse.
        /// </summary>
        [Serializable]
        public class TzParseException : TzException
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TzParseException"/> class.
            /// </summary>
            public TzParseException()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzParseException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="formatParameters">The format parameters.</param>
            public TzParseException(string message, params object[] formatParameters)
                : base(message, formatParameters)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzParseException"/> class.
            /// </summary>
            /// <param name="inner">The inner.</param>
            /// <param name="message">The message.</param>
            /// <param name="formatParameters">The format parameters.</param>
            public TzParseException(Exception inner, string message, params object[] formatParameters)
                : base(message, inner, formatParameters)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzParseException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
            protected TzParseException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
            }
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="pieceYear">The piece year.</param>
        /// <param name="pieceMonth">The piece month.</param>
        /// <param name="pieceDay">The piece day.</param>
        /// <param name="pieceDayOfWeek">The piece day of week.</param>
        /// <param name="pieceTime">The piece time.</param>
        /// <returns></returns>
        public static DateTime GetDateTime(int pieceYear, Month pieceMonth, int pieceDay, DayOfWeek? pieceDayOfWeek, TimeSpan pieceTime)
        {
            int year = pieceYear;
            int month = pieceMonth == 0 ? (int)Month.January : (int)pieceMonth;
            int day = pieceDay == -1 ? 1 : pieceDay;

            if (year == int.MaxValue)
            {
                // This means that there is no Until Year, and no Until time
                // at all, and runs until infinity
                return DateTime.MaxValue;
            }
            else
            {
                // Check if it is a last* day
                if (pieceDayOfWeek != null)
                {
                    if (pieceDay == -1)
                    {
                        // It's the last day of some weekday
                        DateTime untilDay = DateTimeUtlities.GetLastDay(year, month, pieceDayOfWeek.Value);
                        day = untilDay.Day;
                    }
                    else
                    {
                        // This means we're looking for the first day of the week, as
                        // specified, which is on or after the UntilDay
                        DateTime start = new DateTime(year, month, day);
                        DayOfWeek val = pieceDayOfWeek.Value;
                        for (; start.DayOfWeek != val; start = start.AddDays(1))
                        {
                        }
                        day = start.Day;
                    }
                }

                return new DateTime(year, month, day, pieceTime.Hours, pieceTime.Minutes, pieceTime.Seconds);
            }
        }
    }
}
