using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// By default, writes to:
    /// * The Console
    /// * The "Application" Event Log
    /// </summary>
    [Serializable]
    public class CriticalLogger : CompositeLogger
    {
        /// <summary>
        /// 
        /// </summary>
        public static CriticalLogger Current = new CriticalLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="CriticalLogger"/> class.
        /// </summary>
        public CriticalLogger()
        {
            AddLogger(EventLogLogger.Application);
            AddLogger(ConsoleLogger.Current);
        }

        /// <summary>
        /// Adds the logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public override void AddLogger(Logger logger)
        {
            logger.Enabled = true;
            base.AddLogger(logger);
        }

        /// <summary>
        /// Adds the log filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public override void AddLogFilter(ILogFilter filter)
        {
        }

        /// <summary>
        /// Removes the log filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public override void RemoveLogFilter(ILogFilter filter)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void Log(LoggerSeverity severity, object entry, params object[] formatParameters)
        {
            base.Log(LoggerSeverity.Infinity, entry, formatParameters);
        }
    }
}
