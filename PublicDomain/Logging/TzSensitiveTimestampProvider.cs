using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class TzSensitiveTimestampProvider : ILogTimestampProvider
    {
        private TzTimeZone m_timeZone;
        private TimeSpan m_offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="TzSensitiveTimestampProvider"/> class.
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        public TzSensitiveTimestampProvider(TzTimeZone timeZone)
        {
            m_timeZone = timeZone;
            m_offset = timeZone.GetUtcOffset(DateTime.Now);
        }

        /// <summary>
        /// Gets the now.
        /// </summary>
        /// <value>The now.</value>
        public virtual DateTime Now
        {
            get
            {
                return m_timeZone.Now.DateTimeLocal;
            }
        }

        /// <summary>
        /// Gets the offset of the DateTime provided by Now from UTC.
        /// If unknown, returns null.
        /// </summary>
        /// <value>The utc offset.</value>
        public virtual TimeSpan? UtcOffset
        {
            get
            {
                return m_offset;
            }
        }
    }
}
