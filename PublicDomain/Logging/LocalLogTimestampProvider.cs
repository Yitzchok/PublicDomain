using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalLogTimestampProvider : ILogTimestampProvider
    {
        /// <summary>
        /// Gets the now.
        /// </summary>
        /// <value>The now.</value>
        public DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
