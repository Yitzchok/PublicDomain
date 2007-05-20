using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace PublicDomain.ScreenScraper
{
    /// <summary>
    /// Represents a scraped HTML tag.
    /// </summary>
    [Serializable]
    public class ScreenScraperTag
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name;

        /// <summary>
        /// 
        /// </summary>
        public NameValueCollection Attributes = new NameValueCollection();

        /// <summary>
        /// Finds the attribute value.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns></returns>
        public string FindAttributeValue(string attributeName)
        {
            foreach (string key in Attributes.AllKeys)
            {
                if (key.Equals(attributeName))
                {
                    return Attributes[key];
                }
            }
            return null;
        }
    }
}
