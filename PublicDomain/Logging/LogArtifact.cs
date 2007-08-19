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
        public object RawEntry;

        /// <summary>
        /// 
        /// </summary>
        public object[] RawFormatParameters;

        /// <summary>
        /// This is the actual log message that should be written.
        /// </summary>
        public string FormattedMessage;

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
        /// <param name="rawEntry">The raw entry.</param>
        /// <param name="rawFormatParameters">The raw format parameters.</param>
        /// <param name="formattedMessage">The formatted message.</param>
        public LogArtifact(Logger logger, LoggerSeverity severity, DateTime timestamp, object rawEntry, object[] rawFormatParameters, string formattedMessage)
        {
            Logger = logger;
            Severity = severity;
            Timestamp = timestamp;
            RawEntry = rawEntry;
            RawFormatParameters = rawFormatParameters;
            FormattedMessage = formattedMessage;
        }
    }
}
