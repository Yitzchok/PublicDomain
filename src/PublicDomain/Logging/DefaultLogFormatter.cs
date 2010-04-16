using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DefaultLogFormatter : LogFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        protected int m_appDomainId;

        /// <summary>
        /// 
        /// </summary>
        protected string m_utcOffsetStr;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLogFormatter"/> class.
        /// </summary>
        public DefaultLogFormatter()
            : this("[{0}{6} {4,2} {5,2} {1,-8}{3}] {2}")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLogFormatter"/> class.
        /// </summary>
        /// <param name="str">The STR.</param>
        public DefaultLogFormatter(string str)
        {
            FormatString = str;
            m_appDomainId = AppDomain.CurrentDomain.Id;
        }

        /// <summary>
        /// Does the format entry.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="logEntry">The log entry.</param>
        /// <param name="category">The category.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected override string DoFormatEntry(LoggerSeverity severity, DateTime timestamp, string logEntry, string category, Dictionary<string, object> data)
        {
            return string.Format(
                FormatString,
                timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                severity,
                logEntry,
                category,
                Thread.CurrentThread.ManagedThreadId,
                m_appDomainId,
                m_utcOffsetStr
            );
        }

        /// <summary>
        /// Gets or sets the utc offset.
        /// </summary>
        /// <value>The utc offset.</value>
        public override TimeSpan? UtcOffset
        {
            get
            {
                return base.UtcOffset;
            }
            set
            {
                base.UtcOffset = value;
                if (value != null)
                {
                    m_utcOffsetStr = DateTimeUtlities.TrimTimeSpan(DateTimeUtlities.ToStringTimeSpan(UtcOffset.Value));
                }
            }
        }
    }
}
