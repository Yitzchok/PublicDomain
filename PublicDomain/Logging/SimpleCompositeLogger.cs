using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// Can be used for logging based on a class name, which is used
    /// as the category. Also, delineates a new static run on the first log, in debug mode.
    /// </summary>
    public class SimpleCompositeLogger : CompositeLogger
    {
        private string m_className;
        private string m_prefix;
        internal static readonly int DefaultCategoryLength = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCompositeLogger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="className">Name of the class.</param>
        public SimpleCompositeLogger(Logger log, string className)
            : base(log)
        {
            ClearLogFilters();
            AddLogFilter(new SevereLogFilter());

            m_className = className;
            if (!string.IsNullOrEmpty(className))
            {
                // First, we create a friendly name for the logger which we
                // will use as the "category"
                int lastPeriod = m_className.LastIndexOf('.');
                if (lastPeriod != -1)
                {
                    m_prefix = m_className.Substring(lastPeriod + 1) + "(" + m_className.Substring(0, lastPeriod) + ")";
                }
                else
                {
                    m_prefix = m_className;
                }

                // Pad the prefix to ten characters
                if (m_prefix.Length > DefaultCategoryLength)
                {
                    m_prefix = m_prefix.Substring(0, DefaultCategoryLength);
                }
                else if (m_prefix.Length < DefaultCategoryLength)
                {
                    m_prefix = string.Format("{0,-" + DefaultCategoryLength + "}", m_prefix);
                }
            }
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public override string Category
        {
            get
            {
                return base.Category;
            }
            set
            {
                base.Category = value;
                foreach (Logger logger in m_loggers)
                {
                    logger.Category = value;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void Log(LoggerSeverity severity, object entry, params object[] formatParameters)
        {
            Category = m_prefix;
            if (severity == LoggerSeverity.Fatal50)
            {
                HandleFatalLog(severity, entry, formatParameters);
            }
            base.Log(severity, entry, formatParameters);
        }

        /// <summary>
        /// Handles the fatal log.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        protected virtual void HandleFatalLog(LoggerSeverity severity, object entry, object[] formatParameters)
        {
        }

        /// <summary>
        /// Adds the logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public override void AddLogger(Logger logger)
        {
            base.AddLogger(logger);
            logger.Category = Category;
        }
    }
}
