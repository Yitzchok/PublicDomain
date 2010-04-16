using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class LogFormatter : ILogFormatter
    {
        private string m_formatString;
        private TimeSpan? m_utcOffset;

        /// <summary>
        /// </summary>
        /// <value></value>
        public virtual string FormatString
        {
            get
            {
                return m_formatString;
            }
            set
            {
                m_formatString = value;
            }
        }

        /// <summary>
        /// Gets or sets the utc offset.
        /// </summary>
        /// <value>The utc offset.</value>
        public virtual TimeSpan? UtcOffset
        {
            get
            {
                return m_utcOffset;
            }
            set
            {
                m_utcOffset = value;
            }
        }

        /// <summary>
        /// Formats the entry.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="category">The category.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public string FormatEntry(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string category, Dictionary<string, object> data)
        {
            string logEntry = PrepareEntry(entry, formatParameters);

            return DoFormatEntry(severity, timestamp, logEntry, category, data);
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
        protected abstract string DoFormatEntry(LoggerSeverity severity, DateTime timestamp, string logEntry, string category, Dictionary<string, object> data);

        /// <summary>
        /// Prepares the entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <returns></returns>
        protected virtual string PrepareEntry(object entry, object[] formatParameters)
        {
            if (entry == null) return null;

            if (formatParameters != null && formatParameters.Length > 0)
            {
                entry = string.Format(entry.ToString(), formatParameters);
            }

            return entry.ToString();
        }
    }
}
