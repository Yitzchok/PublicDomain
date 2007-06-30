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
        internal static readonly int CATEGORY_LENGTH = 10;

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
                    m_prefix = m_className.Substring(lastPeriod + 1);
                }
                else
                {
                    m_prefix = m_className;
                }

                // Pad the prefix to ten characters
                if (m_prefix.Length > CATEGORY_LENGTH)
                {
                    m_prefix = m_prefix.Substring(0, CATEGORY_LENGTH);
                }
                else if (m_prefix.Length < CATEGORY_LENGTH)
                {
                    m_prefix = string.Format("{0,-" + CATEGORY_LENGTH + "}", m_prefix);
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
                // TODO call a notification interface, which could do something like
                // send an email
            }
            base.Log(severity, entry, formatParameters);
        }
    }
}
