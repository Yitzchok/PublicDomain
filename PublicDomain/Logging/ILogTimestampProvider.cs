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
    }
}
