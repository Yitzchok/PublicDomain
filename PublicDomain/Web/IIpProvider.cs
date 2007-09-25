using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.ScreenScraper;

namespace PublicDomain.Web
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIpProvider
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the last check.
        /// </summary>
        /// <value>The last check.</value>
        DateTime LastCheck { get; set; }

        /// <summary>
        /// Gets the ip address.
        /// </summary>
        /// <returns></returns>
        string GetIpAddress();
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class IpProvider : IIpProvider
    {
        private bool m_isEnabled = true;
        private DateTime m_lastCheck = DateTime.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="IpProvider"/> class.
        /// </summary>
        public IpProvider()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsEnabled
        {
            get
            {
                return m_isEnabled;
            }
            set
            {
                m_isEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the last check.
        /// </summary>
        /// <value>The last check.</value>
        public virtual DateTime LastCheck
        {
            get
            {
                return m_lastCheck;
            }
            set
            {
                m_lastCheck = value;
            }
        }

        /// <summary>
        /// Gets the ip address.
        /// </summary>
        /// <returns></returns>
        public abstract string GetIpAddress();
    }
}
