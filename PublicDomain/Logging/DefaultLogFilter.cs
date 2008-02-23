using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DefaultLogFilter : ILogFilter
    {
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
        public virtual bool IsLoggable(LoggerSeverity threshold, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters)
        {
            return (int)severity >= (int)threshold;
        }
    }
}
