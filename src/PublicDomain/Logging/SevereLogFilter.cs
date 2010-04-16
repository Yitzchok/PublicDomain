using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// Always logs severe events, otherwise defers to normal threshold
    /// conditions.
    /// </summary>
    [Serializable]
    public class SevereLogFilter : DefaultLogFilter
    {
        /// <summary>
        /// Always logs severe events, otherwise defers to normal threshold
        /// conditions. Initializes a new instance of the <see cref="SevereLogFilter"/> class.
        /// </summary>
        public SevereLogFilter()
            : base()
        {
        }

        /// <summary>
        /// Determines whether the specified severity is loggable.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <returns>
        /// 	<c>true</c> if the specified severity is loggable; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsLoggable(LoggerSeverity threshold, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters)
        {
            return severity == LoggerSeverity.Fatal50 ? true : base.IsLoggable(threshold, severity, timestamp, entry, formatParameters);
        }
    }
}
