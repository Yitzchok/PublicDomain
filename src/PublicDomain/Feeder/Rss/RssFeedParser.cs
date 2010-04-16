using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PublicDomain.Feeder.Rss
{
    /// <summary>
    /// 
    /// </summary>
    public class RssFeedParser : FeedParser
    {
        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override T Parse<T>(System.Xml.XmlReader reader)
        {
            IRssFeed ret = (IRssFeed)new RssFeed();
            reader.Read();

            // RDF versions of RSS don't have version tags.
            //double version = double.Parse(reader.GetAttribute("version"));

            reader.ReadToDescendant("channel");

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "title":
                            ret.Title = reader.ReadElementContentAsString();
                            break;
                        case "link":
                            ret.FeedUri = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString());
                            break;
                        case "description":
                            ret.Description = reader.ReadElementContentAsString();
                            break;
                        case "language":
                            ret.Culture = CachedPropertiesProvider.ConvertToCultureInfo(reader.ReadElementContentAsString());
                            break;
                        case "copyright":
                            ret.Copyright = reader.ReadElementContentAsString();
                            break;
                        case "managingEditor":
                            ret.ManagingEditor = reader.ReadElementContentAsString();
                            break;
                        case "webMaster":
                            ret.WebMaster = reader.ReadElementContentAsString();
                            break;
                        case "pubDate":
                            ret.PublicationDate = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "lastBuildDate":
                            ret.LastChanged = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "category":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Category = ConvertToIRssCategory(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "generator":
                            ret.Generator = reader.ReadElementContentAsString();
                            break;
                        case "docs":
                            ret.Doc = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString());
                            break;
                        case "cloud":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Cloud = ConvertToIRssCloud(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "ttl":
                            ret.TimeToLive = CachedPropertiesProvider.ConvertToInt(reader.ReadElementContentAsString());
                            break;
                        case "image":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Image = ConvertToIRssImage(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        /*case "rating":
                            break;*/
                        case "textInput":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.TextInput = ConvertToIRssTextInput(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "skipHours":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.SkipHours = ConvertToSkipHourList(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "skipDays":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.SkipDays = ConvertToDayOfWeekList(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "item":
                            using (XmlReader itemReader = reader.ReadSubtree())
                            {
                                ret.Items.Add(ParseItem(itemReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        default:
                            UnhandledElement(ret, reader);
                            break;
                    }
                }
            }
            reader.Close();
            return (T)ret;
        }

        /// <summary>
        /// Parses the item.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public IFeedItem ParseItem(XmlReader reader)
        {
            IRssFeedItem item = new RssFeedItem();
            reader.ReadToDescendant("item");

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "author":
                            item.Author = reader.ReadElementContentAsString();
                            break;
                        case "category":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Category = ConvertToIRssCategory(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "comments":
                            item.Comments = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString());
                            break;
                        case "description":
                            item.Description = reader.ReadElementContentAsString();
                            break;
                        case "enclosure":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Enclosure = ConvertToIRssEnclosure(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "guid":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Guid = ConvertToIRssGuid(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "link":
                            item.Link = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString());
                            break;
                        case "pubDate":
                            item.PublicationDate = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "source":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Source = ConvertToIRssSource(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "title":
                            item.Title = reader.ReadElementContentAsString();
                            break;
                        default:
                            UnhandledElement(item, reader);
                            break;
                    }
                }
            }
            reader.Close();
            return item;
        }

        /// <summary>
        /// Converts to I RSS cloud.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssCloud ConvertToIRssCloud(XmlReader input)
        {
            RssCloud cloud = new RssCloud();
            input.ReadToDescendant("cloud");
            cloud.Domain = input.GetAttribute("domain");
            cloud.Port = CachedPropertiesProvider.ConvertToInt(input.GetAttribute("port"));
            cloud.Path = input.GetAttribute("path");
            cloud.Protocol = ConvertToRssCloudProtocol(input.GetAttribute("protocol"));
            cloud.RegisterProcedure = input.GetAttribute("registerProcedure");
            input.Close();
            return cloud;
        }

        /// <summary>
        /// Converts to I RSS cloud.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssCloud ConvertToIRssCloud(string input)
        {
            return ConvertToIRssCloud(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS category.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssCategory ConvertToIRssCategory(XmlReader input)
        {
            RssCategory cat = new RssCategory();
            input.ReadToDescendant("category");
            cat.Domain = input.GetAttribute("domain");
            cat.CategoryName = input.ReadElementContentAsString();
            input.Close();
            return cat;
        }

        /// <summary>
        /// Converts to I RSS category.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssCategory ConvertToIRssCategory(string input)
        {
            return ConvertToIRssCategory(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS text input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssTextInput ConvertToIRssTextInput(XmlReader input)
        {
            RssTextInput text = new RssTextInput();
            input.ReadToDescendant("textInput");

            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "description":
                            text.Description = input.ReadElementContentAsString();
                            break;
                        case "link":
                            text.Link = CachedPropertiesProvider.ConvertToUri(input.ReadElementContentAsString());
                            break;
                        case "name":
                            text.Name = input.ReadElementContentAsString();
                            break;
                        case "title":
                            text.Title = input.ReadElementContentAsString();
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
            input.Close();
            return text;
        }

        /// <summary>
        /// Converts to I RSS text input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssTextInput ConvertToIRssTextInput(string input)
        {
            return ConvertToIRssTextInput(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to day of week list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IList<DayOfWeek> ConvertToDayOfWeekList(XmlReader input)
        {
            IList<DayOfWeek> skipDays = new List<DayOfWeek>();
            input.ReadToDescendant("skipDays");

            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element && input.Name == "day")
                {
                    skipDays.Add(ConvertToDayOfWeek(input.ReadElementContentAsString()));
                    readContent = true;
                }
            }
            input.Close();
            return skipDays;
        }

        /// <summary>
        /// Converts to day of week list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IList<DayOfWeek> ConvertToDayOfWeekList(string input)
        {
            return ConvertToDayOfWeekList(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to skip hour list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IList<uint> ConvertToSkipHourList(XmlReader input)
        {
            IList<uint> skipHours = new List<uint>();
            input.ReadToDescendant("skipHours");

            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element && input.Name == "hour")
                {
                    skipHours.Add(CachedPropertiesProvider.ConvertToUInt(input.ReadElementContentAsString()));
                    readContent = true;
                }
            }
            input.Close();
            return skipHours;
        }

        /// <summary>
        /// Converts to skip hour list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IList<uint> ConvertToSkipHourList(string input)
        {
            return ConvertToSkipHourList(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS image.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssImage ConvertToIRssImage(XmlReader input)
        {
            RssImage image = new RssImage();
            input.ReadToDescendant("image");

            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "description":
                            image.Description = input.ReadElementContentAsString();
                            break;
                        case "link":
                            image.Link = CachedPropertiesProvider.ConvertToUri(input.ReadElementContentAsString());
                            break;
                        case "location":
                            image.Location = CachedPropertiesProvider.ConvertToUri(input.ReadElementContentAsString());
                            break;
                        case "title":
                            image.Title = input.ReadElementContentAsString();
                            break;
                        case "width":
                            image.Width = CachedPropertiesProvider.ConvertToInt(input.ReadElementContentAsString());
                            break;
                        case "height":
                            image.Height = CachedPropertiesProvider.ConvertToInt(input.ReadElementContentAsString());
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
            input.Close();
            return image;
        }

        /// <summary>
        /// Converts to I RSS image.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssImage ConvertToIRssImage(string input)
        {
            return ConvertToIRssImage(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS enclosure.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssEnclosure ConvertToIRssEnclosure(XmlReader input)
        {
            RssEnclosure enc = new RssEnclosure();
            input.ReadToDescendant("enclosure");
            enc.Length = CachedPropertiesProvider.ConvertToInt(input.GetAttribute("length"));
            enc.Link = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("url"));
            enc.Type = input.GetAttribute("type");
            input.Close();
            return enc;
        }

        /// <summary>
        /// Converts to I RSS enclosure.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssEnclosure ConvertToIRssEnclosure(string input)
        {
            return ConvertToIRssEnclosure(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS GUID.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssGuid ConvertToIRssGuid(XmlReader input)
        {
            RssGuid guid = new RssGuid();
            input.ReadToDescendant("guid");
            guid.IsPermaLink = CachedPropertiesProvider.ConvertToBoolNullable(input.GetAttribute("isPermaLink"));
            guid.UniqueIdentifier = input.ReadElementContentAsString();
            input.Close();
            return guid;
        }

        /// <summary>
        /// Converts to I RSS GUID.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssGuid ConvertToIRssGuid(string input)
        {
            return ConvertToIRssGuid(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS source.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssSource ConvertToIRssSource(XmlReader input)
        {
            RssSource source = new RssSource();
            input.ReadToDescendant("source");
            source.Link = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("url"));
            source.Description = input.ReadElementContentAsString();
            input.Close();
            return source;
        }

        /// <summary>
        /// Converts to I RSS source.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssSource ConvertToIRssSource(string input)
        {
            return ConvertToIRssSource(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to RSS cloud protocol.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static RssCloudProtocol ConvertToRssCloudProtocol(string input)
        {
            input = input.Trim().ToLower();
            switch (input)
            {
                case "xml-rpc":
                    return RssCloudProtocol.XmlRpc;
                case "http-post":
                    return RssCloudProtocol.HttpPost;
                case "soap":
                    return RssCloudProtocol.Soap;
                default:
                    return RssCloudProtocol.Unknown;
            }
        }

        /// <summary>
        /// Converts to day of week.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static DayOfWeek ConvertToDayOfWeek(string input)
        {
            return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), input);
        }
    }
}
