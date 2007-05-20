using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using PublicDomain.Feeder.Opml;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// http://www.kbcafe.com/rss/?guid=20051003145153
    /// </summary>
    public class OpmlSerializer : Serializer
    {
        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <returns></returns>
        public static string SerializeToString(IOpmlFeed feed)
        {
            return Serialize(feed).OuterXml;
        }

        /// <summary>
        /// Serializes the specified feed.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <returns></returns>
        public static XmlDocument Serialize(IOpmlFeed feed)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = AppendNewElement(doc, doc, "opml");
            AppendNewAttribute(doc, root, "version", "1.0");
            XmlElement head = AppendNewElement(doc, root, "head");
            if (feed.Head != null)
            {
                if (feed.Head.Title != null)
                {
                    AppendNewElement(doc, head, "title", feed.Head.Title);
                }
                if (feed.Head.DateCreated != null)
                {
                    AppendNewElement(doc, head, "dateCreated", feed.Head.DateCreated.DateTimeUtc.ToString("r"));
                }
                if (feed.Head.DateModified != null)
                {
                    AppendNewElement(doc, head, "dateModified", feed.Head.DateModified.DateTimeUtc.ToString("r"));
                }
                if (feed.Head.Owner != null)
                {
                    AppendNewElement(doc, head, "ownerName", feed.Head.Owner);
                }
                if (feed.Head.OwnerEmail != null)
                {
                    AppendNewElement(doc, head, "ownerEmail", feed.Head.OwnerEmail);
                }
                if (feed.Head.ExpansionState != null)
                {
                    AppendNewElement(doc, head, "expansionState", feed.Head.ExpansionState);
                }
                if (feed.Head.VerticalScrollState != null)
                {
                    AppendNewElement(doc, head, "vertScrollState", feed.Head.VerticalScrollState.Value.ToString());
                }
                if (feed.Head.WindowBottom != null)
                {
                    AppendNewElement(doc, head, "windowBottom", feed.Head.WindowBottom.Value.ToString());
                }
                if (feed.Head.WindowTop != null)
                {
                    AppendNewElement(doc, head, "windowTop", feed.Head.WindowTop.Value.ToString());
                }
                if (feed.Head.WindowLeft != null)
                {
                    AppendNewElement(doc, head, "windowLeft", feed.Head.WindowLeft.Value.ToString());
                }
                if (feed.Head.WindowRight != null)
                {
                    AppendNewElement(doc, head, "windowRight", feed.Head.WindowRight.Value.ToString());
                }
            }

            XmlElement body = AppendNewElement(doc, root, "body");
            SerializeOutlines(doc, body, feed.Body);
            return doc;
        }

        private static void SerializeOutlines(XmlDocument doc, XmlElement parent, IOpmlOutlineProvider outlineProvider)
        {
            if (outlineProvider != null)
            {
                foreach (IOpmlOutline outline in outlineProvider.Outlines)
                {
                    XmlElement outlineElement = AppendNewElement(doc, parent, "outline");
                    if (outline.Type != null)
                    {
                        AppendNewAttribute(doc, outlineElement, "type", outline.Type);
                    }
                    if (outline.Text != null)
                    {
                        AppendNewAttribute(doc, outlineElement, "text", outline.Text);
                    }
                    SerializeOutlines(doc, outlineElement, outline);
                }
            }
        }
    }
}
