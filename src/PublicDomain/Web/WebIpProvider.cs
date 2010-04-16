using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.ScreenScraper;

namespace PublicDomain.Web
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class WebIpProvider : IpProvider
    {
        private string m_url;
        private string m_successRegex;
        private int m_captureIndex;
        private string m_excessiveMatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebIpProvider"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public WebIpProvider(string url)
            : this(url, null, -1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebIpProvider"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="successRegex">The success regex.</param>
        /// <param name="captureIndex">Index of the capture.</param>
        public WebIpProvider(string url, string successRegex, int captureIndex)
            : this(url, successRegex, captureIndex, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebIpProvider"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="successRegex">The success regex.</param>
        /// <param name="captureIndex">Index of the capture.</param>
        /// <param name="excessiveMatch">The excessive match.</param>
        public WebIpProvider(string url, string successRegex, int captureIndex, string excessiveMatch)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            m_url = url;
            m_successRegex = successRegex;
            m_captureIndex = captureIndex;
            m_excessiveMatch = excessiveMatch;
        }

        /// <summary>
        /// Gets the ip address.
        /// </summary>
        /// <returns></returns>
        public override string GetIpAddress()
        {
            try
            {
                Scraper scraper = new Scraper();
                PrepareScraper(scraper);
                ScrapedPage checkip = scraper.Scrape(ScrapeType.GET, m_url);
                if (checkip != null && checkip.RawStream != null)
                {
                    if (string.IsNullOrEmpty(m_successRegex))
                    {
                        string result = checkip.RawStream.Trim();
                        if (StringUtilities.ExtractFirstNumber(result) > 0)
                        {
                            return result;
                        }
                    }
                    else
                    {
                        return RegexUtilities.Extract(checkip.RawStream, m_successRegex, m_captureIndex);
                    }
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Prepares the scraper.
        /// </summary>
        /// <param name="scraper">The scraper.</param>
        protected virtual void PrepareScraper(Scraper scraper)
        {
        }
    }
}
