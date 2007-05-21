using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PublicDomain.Feeder.Opml
{
    /// <summary>
    /// 
    /// </summary>
    public class OpmlParser : Parser
    {
        /// <summary>
        /// Creates the feed base.
        /// </summary>
        /// <param name="feedReader">The feed reader.</param>
        /// <returns></returns>
        protected override T CreateFeedBase<T>(XmlReader feedReader)
        {
            feedReader.MoveToContent();
            return new OpmlParser().Parse<T>(feedReader);
        }

        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override T Parse<T>(System.Xml.XmlReader reader)
        {
            IOpmlFeed ret = new OpmlFeed();

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "head":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Head = ConvertToIOpmlHead(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "body":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Body = ConvertToIOpmlBody(subReader);
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

        internal static IOpmlOutline ConvertToIOpmlOutline(string input)
        {
            return ConvertToIOpmlOutline(CreateXmlReaderFromString(input));
        }

        internal static IOpmlOutline ConvertToIOpmlOutline(XmlReader input)
        {
            OpmlOutline outline = new OpmlOutline();
            input.ReadToDescendant("outline");
            outline.Text = input.GetAttribute("text");
            outline.Type = input.GetAttribute("type");
            outline.IsBreakpoint = CachedPropertiesProvider.ConvertToBoolNullable(input.GetAttribute("isBreakpoint"));
            outline.IsComment = CachedPropertiesProvider.ConvertToBoolNullable(input.GetAttribute("isComment"));
            ReadOutlines(input, outline);
            input.Close();
            return outline;
        }

        internal static IOpmlBody ConvertToIOpmlBody(string input)
        {
            return ConvertToIOpmlBody(CreateXmlReaderFromString(input));
        }

        internal static IOpmlBody ConvertToIOpmlBody(XmlReader input)
        {
            OpmlBody body = new OpmlBody();
            input.ReadToDescendant("body");
            ReadOutlines(input, body);
            input.Close();
            return body;
        }

        private static void ReadOutlines(XmlReader input, IOpmlOutlineProvider outlineProvider)
        {
            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "outline":
                            using (XmlReader subReader = input.ReadSubtree())
                            {
                                outlineProvider.Outlines.Add(ConvertToIOpmlOutline(subReader));
                            }
                            if (input.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
        }

        internal static IOpmlHead ConvertToIOpmlHead(string input)
        {
            return ConvertToIOpmlHead(CreateXmlReaderFromString(input));
        }

        internal static IOpmlHead ConvertToIOpmlHead(XmlReader input)
        {
            OpmlHead head = new OpmlHead();
            input.ReadToDescendant("head");
            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "title":
                            head.Title = input.ReadElementContentAsString();
                            break;
                        case "dateCreated":
                            head.DateCreated = CachedPropertiesProvider.ConvertToTzDateTime(input.ReadElementContentAsString());
                            break;
                        case "dateModified":
                            head.DateModified = CachedPropertiesProvider.ConvertToTzDateTime(input.ReadElementContentAsString());
                            break;
                        case "ownerName":
                            head.Owner = input.ReadElementContentAsString();
                            break;
                        case "ownerEmail":
                            head.OwnerEmail = input.ReadElementContentAsString();
                            break;
                        case "expansionState":
                            head.ExpansionState = input.ReadElementContentAsString();
                            break;
                        case "vertScrollState":
                            head.VerticalScrollState = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        case "windowTop":
                            head.WindowTop = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        case "windowLeft":
                            head.WindowLeft = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        case "windowBottom":
                            head.WindowBottom = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        case "windowRight":
                            head.WindowRight = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
            input.Close();
            return head;
        }
    }
}
