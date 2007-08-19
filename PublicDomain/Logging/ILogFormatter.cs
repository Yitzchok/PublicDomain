using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogFormatter
    {
        /// <summary>
        /// Formats the entry.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="utcOffset">The utc offset.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="category">The category.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        string FormatEntry(LoggerSeverity severity, DateTime timestamp, TimeSpan? utcOffset, object entry, object[] formatParameters, string category, Dictionary<string, object> data);

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        /// <value>The format string.</value>
        string FormatString { get; set; }
    }
}
