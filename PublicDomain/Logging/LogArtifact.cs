using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class LogArtifact
    {
        /// <summary>
        /// 
        /// </summary>
        public Logger Logger;

        /// <summary>
        /// 
        /// </summary>
        public LoggerSeverity Severity;

        /// <summary>
        /// 
        /// </summary>
        public DateTime Timestamp;

        /// <summary>
        /// 
        /// </summary>
        public object Entry;

        /// <summary>
        /// 
        /// </summary>
        public object[] FormatParameters;

        /// <summary>
        /// 
        /// </summary>
        public string LogLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogArtifact"/> class.
        /// </summary>
        public LogArtifact()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogArtifact"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="logLine">The log line.</param>
        public LogArtifact(Logger logger, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
            Logger = logger;
            Severity = severity;
            Timestamp = timestamp;
            Entry = entry;
            FormatParameters = formatParameters;
            LogLine = logLine;
        }
    }
}
