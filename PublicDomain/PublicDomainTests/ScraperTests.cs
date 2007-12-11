using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PublicDomain.ScreenScraper;
using PublicDomain.Xml;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class ScraperTests
    {
        /// <summary>
        /// Plays this instance.
        /// </summary>
        [Test]
        public void play()
        {
            LenientHtmlDocument doc = new LenientHtmlDocument();

            foreach (string url in new string[] {
                "http://www.cnn.com/",
                "http://www.google.com/",
                "http://www.bankofamerica.com/",
                "http://www.citicard.com/"
            })
            {
                Scraper scraper = new Scraper();
                ScrapedPage page = scraper.Scrape(ScrapeType.GET, url);
                doc.LoadXml(page.RawStream);
                Console.WriteLine(doc.InnerXml);
            }
        }
    }
}
