using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class StringLogger : Logger
    {
        /// <summary>
        /// 
        /// </summary>
        public const int DefaultCapacity = 10000;

        private StringBuilder m_sb;
        private string m_cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringLogger"/> class.
        /// </summary>
        public StringLogger()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringLogger"/> class.
        /// </summary>
        /// <param name="sb">The sb.</param>
        public StringLogger(StringBuilder sb)
        {
            m_sb = sb;
            if (m_sb == null)
            {
                m_sb = new StringBuilder(DefaultCapacity);
            }
        }

        /// <summary>
        /// High level final log that is called with all of the detailed information
        /// and the final log line as the last parameter.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
            m_cache = null;
            m_sb.Append(logLine);
        }

        /// <summary>
        /// Writes the specified artifact.
        /// </summary>
        /// <param name="artifact">The artifact.</param>
        public override void Write(LogArtifact artifact)
        {
            // should never get here
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            if (m_cache == null)
            {
                m_cache = m_sb.ToString();
            }
            return m_cache;
        }
    }
}
