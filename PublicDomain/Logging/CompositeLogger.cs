using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace PublicDomain.Logging
{
    /// <summary>
    /// By default does not have any filters, and supposes that the composed logs will filter.
    /// </summary>
    public class CompositeLogger : Logger
    {
        /// <summary>
        /// 
        /// </summary>
        protected List<Logger> m_loggers = new List<Logger>();

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
                    AddLogger(logger);
                }
            }
        }

        /// <summary>
        /// Adds the logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public virtual void AddLogger(Logger logger)
        {
            m_loggers.Add(logger);
        }

        /// <summary>
        /// Gets the loggers.
        /// </summary>
        /// <value>The loggers.</value>
        public virtual ReadOnlyCollection<Logger> Loggers
        {
            get
            {
                return m_loggers.AsReadOnly();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void Log(LoggerSeverity severity, object entry, params object[] formatParameters)
        {
            foreach (Logger logger in m_loggers)
            {
                logger.Log(severity, entry, formatParameters);
            }
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
                foreach (Logger logger in m_loggers)
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
            // should never get here
            throw new NotImplementedException();
        }
    }
}
