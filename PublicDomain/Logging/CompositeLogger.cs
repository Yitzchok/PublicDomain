using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// By default does not have any filters, and supposes that the composed logs will filter.
    /// </summary>
    public class CompositeLogger : Logger
    {
        private List<Logger> m_loggers = new List<Logger>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeLogger"/> class.
        /// </summary>
        /// <param name="loggers">The loggers.</param>
        public CompositeLogger(params Logger[] loggers)
        {
            foreach (Logger logger in loggers)
            {
                if (logger != null)
                {
                    Loggers.Add(logger);
                }
            }
        }

        /// <summary>
        /// Gets the loggers.
        /// </summary>
        /// <value>The loggers.</value>
        public virtual List<Logger> Loggers
        {
            get
            {
                return m_loggers;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void Log(LoggerSeverity severity, object entry, params object[] formatParameters)
        {
            foreach (Logger logger in Loggers)
            {
                logger.Log(severity, entry, formatParameters);
            }
        }

        /// <summary>
        /// Does the log.
        /// </summary>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(string logLine)
        {
            // This is not called
        }

        /// <summary>
        /// The severity threshold at which point a log message
        /// is logged. For example, if the threshold is Debug,
        /// all messages with severity greater than or equal to Debug
        /// will be logged. All other messages will be discarded.
        /// The default threshold is Warn.
        /// </summary>
        /// <value></value>
        public override LoggerSeverity Threshold
        {
            get
            {
                return base.Threshold;
            }
            set
            {
                base.Threshold = value;
                foreach (Logger logger in Loggers)
                {
                    logger.Threshold = value;
                }
            }
        }

        /// <summary>
        /// Writes the specified artifact.
        /// </summary>
        /// <param name="artifact">The artifact.</param>
        public override void Write(LogArtifact artifact)
        {
            foreach (Logger logger in Loggers)
            {
                logger.Write(artifact);
            }
        }
    }
}
