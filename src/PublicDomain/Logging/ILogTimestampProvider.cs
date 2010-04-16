using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogTimestampProvider
    {
        /// <summary>
        /// Gets the now.
        /// </summary>
        /// <value>The now.</value>
        DateTime Now { get; }

        /// <summary>
        /// Gets the offset of the DateTime provided by Now from UTC.
        /// If unknown, returns null.
        /// </summary>
        /// <value>The utc offset.</value>
        TimeSpan? UtcOffset { get; }
    }
}
