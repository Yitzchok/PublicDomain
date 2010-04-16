using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    public class FeedSerializer : Serializer
    {
        /// <summary>
        /// Serializes the specified feed.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static XmlDocument Serialize(IFeed feed, SerializeType type)
        {
            XmlDocument doc = new XmlDocument();
            switch (type)
            {
                case SerializeType.Rss2:
                    SerializeRss2(doc, (IRssFeed)feed);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return doc;
        }

        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string SerializeToString(IFeed feed, SerializeType type)
        {
            return Serialize(feed, type).OuterXml;
        }

        /// <summary>
        /// Serializes the RSS2.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="feed">The feed.</param>
        public static void SerializeRss2(XmlDocument doc, IRssFeed feed)
        {
            XmlElement root = AppendNewElement(doc, doc, "rss");
            AppendNewAttribute(doc, root, "version", "2.0");
            XmlElement channel = AppendNewElement(doc, root, "channel");
            AppendNewElement(doc, channel, "title", feed.Title);
            if (feed.FeedUri != null)
            {
                AppendNewElement(doc, channel, "link", feed.FeedUri.ToString());
            }
            AppendNewElement(doc, channel, "description", feed.Description);
            AppendNewElement(doc, channel, "copyright", feed.Copyright);
            foreach (IRssFeedItem item in feed.Items)
            {
                XmlElement itemel = AppendNewElement(doc, channel, "item");
                AppendNewElement(doc, itemel, "title", item.Title);
                if (item.Link != null)
                {
                    AppendNewElement(doc, itemel, "link", item.Link.ToString());
                }
                AppendNewElement(doc, itemel, "description", item.Description);
                if (item.PublicationDate != null)
                {
                    AppendNewElement(doc, itemel, "pubDate", item.PublicationDate.DateTimeUtc.ToString("r"));
                }
            }
        }
    }
}
