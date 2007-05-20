using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace PublicDomain.ScreenScraper
{
    /// <summary>
    /// Represents an HTTP session during a scraping of a page.
    /// </summary>
    [Serializable]
    public class ScrapeSession
    {
        private Scraper _ContainingScraper;

        /// <summary>
        /// Gets or sets the containing scraper.
        /// </summary>
        /// <value>The containing scraper.</value>
        protected Scraper ContainingScraper
        {
            get
            {
                return _ContainingScraper;
            }
            set
            {
                _ContainingScraper = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrapeSession"/> class.
        /// </summary>
        /// <param name="scraper">The scraper.</param>
        public ScrapeSession(Scraper scraper)
        {
            ContainingScraper = scraper;
        }

        /// <summary>
        /// 
        /// </summary>
        protected CookieContainer m_Cookies = new CookieContainer();

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        /// <value>The cookies.</value>
        public CookieContainer Cookies
        {
            get
            {
                return m_Cookies;
            }
        }

        /// <summary>
        /// Adds the cookie.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void AddCookie(string name, string value)
        {
            Cookies.Add(new Cookie(name, value, "/", ContainingScraper.Domain));
        }
    }
}
