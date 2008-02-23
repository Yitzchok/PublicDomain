using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class EventLogLogger : Logger
    {
        private EventLog m_log;

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultSource = "Application";

        /// <summary>
        /// 
        /// </summary>
        public static EventLogLogger Application = new EventLogLogger(EventLogSource.Application);

        /// <summary>
        /// 
        /// </summary>
        public static EventLogLogger System = new EventLogLogger(EventLogSource.System);

        /// <summary>
        /// 
        /// </summary>
        public static EventLogLogger Security = new EventLogLogger(EventLogSource.Security);

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogLogger"/> class.
        /// </summary>
        public EventLogLogger()
            : this(DefaultSource)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogLogger"/> class.
        /// </summary>
        public EventLogLogger(string source)
            : this(EventLogSource.Application, source)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogLogger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public EventLogLogger(EventLogSource log)
            : this(log, DefaultSource)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogLogger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="source">The source.</param>
        public EventLogLogger(EventLogSource log, string source)
            : this(log.ToString(), source)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogLogger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="source">The source.</param>
        public EventLogLogger(string log, string source)
        {
            if (string.IsNullOrEmpty(log))
            {
                throw new ArgumentNullException("log");
            }
            else if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            m_log = new EventLog();
            
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, source);
            }

            m_log.Source = source;
            m_log.Log = log;
        }

        /// <summary>
        /// Writes the specified artifact.
        /// </summary>
        /// <param name="artifact">The artifact.</param>
        public override void Write(LogArtifact artifact)
        {
            try
            {
                m_log.WriteEntry(artifact.FormattedMessage, GetEventLogEntryType(artifact.Severity));
            }
            catch (ObjectDisposedException)
            {
                using (EventLog log = new EventLog())
                {
                    log.Source = m_log.Source;
                    log.Log = m_log.Log;

                    log.WriteEntry(artifact.FormattedMessage, GetEventLogEntryType(artifact.Severity));
                }
            }
        }

        /// <summary>
        /// Gets the type of the event log entry.
        /// </summary>
        /// <param name="loggerSeverity">The logger severity.</param>
        /// <returns></returns>
        public static EventLogEntryType GetEventLogEntryType(LoggerSeverity loggerSeverity)
        {
            switch (loggerSeverity)
            {
                case LoggerSeverity.Error40:
                case LoggerSeverity.Fatal50:
                case LoggerSeverity.Infinity:
                    return EventLogEntryType.Error;
                case LoggerSeverity.Warn30:
                    return EventLogEntryType.Warning;
                default:
                    return EventLogEntryType.Information;
            }
        }
    }
}
