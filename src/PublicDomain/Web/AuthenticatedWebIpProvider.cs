using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.ScreenScraper;

namespace PublicDomain.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticatedWebIpProvider : WebIpProvider
    {
        private string m_userName;
        private string m_password;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatedWebIpProvider"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="url">The URL.</param>
        public AuthenticatedWebIpProvider(string userName, string password, string url)
            : this(userName, password, url, null, -1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatedWebIpProvider"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="url">The URL.</param>
        /// <param name="successRegex">The success regex.</param>
        /// <param name="captureIndex">Index of the capture.</param>
        public AuthenticatedWebIpProvider(string userName, string password, string url, string successRegex, int captureIndex)
            : this(userName, password, url, successRegex, captureIndex, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatedWebIpProvider"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="url">The URL.</param>
        /// <param name="successRegex">The success regex.</param>
        /// <param name="captureIndex">Index of the capture.</param>
        /// <param name="excessiveMatch">The excessive match.</param>
        public AuthenticatedWebIpProvider(string userName, string password, string url, string successRegex, int captureIndex, string excessiveMatch)
            : base(url, successRegex, captureIndex, excessiveMatch)
        {
            m_userName = userName;
            m_password = password;
        }

        /// <summary>
        /// Prepares the scraper.
        /// </summary>
        /// <param name="scraper">The scraper.</param>
        protected override void PrepareScraper(Scraper scraper)
        {
            if (!string.IsNullOrEmpty(m_userName))
            {
                scraper.UseCredentials = true;
                scraper.SetNetworkCredentials(m_userName, m_password);
            }
        }
    }
}
