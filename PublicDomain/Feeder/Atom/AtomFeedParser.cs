using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace PublicDomain.Feeder.Atom
{
    /// <summary>
    /// 
    /// </summary>
    public class AtomFeedParser : FeedParser
    {
        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override T Parse<T>(System.Xml.XmlReader reader)
        {
            IAtomFeed ret = (IAtomFeed)new AtomFeed();
            reader.Read();

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "id":
                            ret.FeedUri = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString(), reader.BaseURI);
                            break;
                        case "title":
                            ret.Title = reader.ReadElementContentAsString();
                            break;
                        case "updated":
                            ret.LastUpdated = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "generator":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Generator = ConvertToIAtomGenerator(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "author":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Authors.Add(ConvertToIAtomPerson(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "link":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Links.Add(ConvertToIAtomLink(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "category":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Categories.Add(ConvertToIAtomCategory(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "entry":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Items.Add(ParseItem(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "contributor":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Contributors.Add(ConvertToIAtomPerson(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "logo":
                            ret.Logo = reader.ReadElementContentAsString();
                            break;
                        case "icon":
                            ret.Icon = reader.ReadElementContentAsString();
                            break;
                        case "rights":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Rights = ConvertToIAtomText(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "subtitle":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Subtitle = ConvertToIAtomText(subReader);
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
            IAtomFeedItem item = new AtomFeedItem();
            reader.ReadToDescendant("entry");

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "id":
                            item.Id = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString(), reader.BaseURI);
                            break;
                        case "title":
                            item.Title = reader.ReadElementContentAsString();
                            break;
                        case "updated":
                            item.LastUpdated = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "author":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Authors.Add(ConvertToIAtomPerson(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "link":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Links.Add(ConvertToIAtomLink(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "category":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Categories.Add(ConvertToIAtomCategory(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "contributor":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Contributors.Add(ConvertToIAtomPerson(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "rights":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Rights = ConvertToIAtomText(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "summary":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Summary = ConvertToIAtomText(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "content":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Content = ConvertToIAtomContent(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "published":
                            item.Published = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "source":
                            item.Source = ConvertToIAtomFeed(reader.ReadOuterXml());
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
        /// Converts to I atom generator.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomGenerator ConvertToIAtomGenerator(XmlReader input)
        {
            AtomGenerator gen = new AtomGenerator();
            input.ReadToDescendant("generator");
            gen.GeneratorUri = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("uri"), input.BaseURI);
            gen.Version = input.GetAttribute("version");
            gen.Description = input.ReadElementContentAsString();
            input.Close();
            return gen;
        }

        /// <summary>
        /// Converts to I atom generator.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomGenerator ConvertToIAtomGenerator(string input)
        {
            return ConvertToIAtomGenerator(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I atom text.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomText ConvertToIAtomText(XmlReader input)
        {
            AtomText text = new AtomText();
            input.Read();
            text.Type = input.GetAttribute("type");
            text.Text = input.ReadInnerXml();
            input.Close();
            return text;
        }

        /// <summary>
        /// Converts to I atom text.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomText ConvertToIAtomText(string input)
        {
            return ConvertToIAtomText(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts the content of to I atom.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomContent ConvertToIAtomContent(XmlReader input)
        {
            AtomContent content = new AtomContent();
            input.ReadToDescendant("content");
            content.Type = input.GetAttribute("type");
            content.Src = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("src"), input.BaseURI);
            content.Content = input.ReadInnerXml();
            input.Close();
            return content;
        }

        /// <summary>
        /// Converts the content of to I atom.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomContent ConvertToIAtomContent(string input)
        {
            return ConvertToIAtomContent(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I atom feed.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomFeed ConvertToIAtomFeed(string input)
        {
            using (StringReader stringReader = new StringReader(input))
            {
                return new AtomFeedParser().CreateFeed<IAtomFeed>(input);
            }
        }

        /// <summary>
        /// Converts to I atom person.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomPerson ConvertToIAtomPerson(XmlReader input)
        {
            AtomPerson person = new AtomPerson();
            input.Read();
            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "name":
                            person.Name = input.ReadElementContentAsString();
                            break;
                        case "uri":
                            person.Homepage = CachedPropertiesProvider.ConvertToUri(input.ReadElementContentAsString(), input.BaseURI);
                            break;
                        case "email":
                            person.Email = input.ReadElementContentAsString();
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
            input.Close();
            return person;
        }

        /// <summary>
        /// Converts to I atom link.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomLink ConvertToIAtomLink(XmlReader input)
        {
            AtomLink link = new AtomLink();
            input.ReadToDescendant("link");
            link.Href = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("href"), input.BaseURI);
            link.Relationship = input.GetAttribute("rel");
            link.Type = input.GetAttribute("type");
            link.LinkLanguage = input.GetAttribute("hreflang");
            link.Title = input.GetAttribute("title");
            link.Length = CachedPropertiesProvider.ConvertToIntNullable(input.GetAttribute("length"));
            input.Close();
            return link;
        }

        /// <summary>
        /// Converts to I atom category.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomCategory ConvertToIAtomCategory(XmlReader input)
        {
            AtomCategory cat = new AtomCategory();
            input.ReadToDescendant("link");
            cat.Label = input.GetAttribute("label");
            cat.Term = input.GetAttribute("term");
            cat.Scheme = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("scheme"), input.BaseURI);
            input.Close();
            return cat;
        }
    }
}
