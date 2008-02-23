using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// Writes to a file, rolling over to a new version of a file
    /// when the previous file has filled to capacity.
    /// </summary>
    [Serializable]
    public class RollingFileLogger : FileLogger
    {
        private IRollOverStrategy m_strategy;

        /// <summary>
        /// Writes to a file, rolling over to a new version of a file
        /// when the previous file has filled to capacity.
        /// Initializes a new instance of the <see cref="RollingFileLogger"/> class.
        /// </summary>
        /// <param name="fileNameFormatted">The file name formatted.</param>
        public RollingFileLogger(string fileNameFormatted)
            : this(fileNameFormatted, new FileSizeRollOverStrategy())
        {
        }

        /// <summary>
        /// Writes to a file, rolling over to a new version of a file
        /// when the previous file has filled to capacity.
        /// Initializes a new instance of the <see cref="RollingFileLogger"/> class.
        /// </summary>
        /// <param name="fileNameFormatted">The file name formatted.</param>
        /// <param name="strategy">The strategy.</param>
        public RollingFileLogger(string fileNameFormatted, IRollOverStrategy strategy)
            : base(fileNameFormatted)
        {
            Strategy = strategy;
        }

        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>The strategy.</value>
        public IRollOverStrategy Strategy
        {
            get
            {
                return m_strategy;
            }
            set
            {
                m_strategy = value;
            }
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="timestamp"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        /// <param name="logLine"></param>
        /// <param name="artifactSet"></param>
        /// <returns></returns>
        public override string GetFileName(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine, LogArtifact[] artifactSet)
        {
            string fileName = base.GetFileName(severity, timestamp, entry, formatParameters, logLine, artifactSet);

            if (Strategy != null)
            {
                fileName = Strategy.GetFileName(fileName, severity, timestamp, entry, formatParameters, logLine, artifactSet);
            }

            return fileName;
        }
    }
}
