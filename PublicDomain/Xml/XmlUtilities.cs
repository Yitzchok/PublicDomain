using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public static class XmlUtilities
    {
        /// <summary>
        /// &lt;![CDATA[
        /// </summary>
        public const string CDataStart = "<![CDATA[";

        /// <summary>
        /// ]]&gt;
        /// </summary>
        public const string CDataEnd = "]]>";

        /// <summary>
        /// &lt;!--
        /// </summary>
        public const string CommentStart = "<!--";

        /// <summary>
        /// --&gt;
        /// </summary>
        public const string CommentEnd = "-->";

        /// <summary>
        /// &lt;?xml version="1.0" encoding="utf-8"?&gt;
        /// </summary>
        public const string DefaultXmlProlog = @"<?xml version=""1.0"" encoding=""utf-8""?>";

        /// <summary>
        /// Formats the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static string FormatXml(string xml)
        {
            return FormatXml(xml, GetDefaultXmlWriterSettings());
        }

        /// <summary>
        /// Gets the default XML writer settings.
        /// </summary>
        /// <returns></returns>
        public static XmlWriterSettings GetDefaultXmlWriterSettings()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Auto;
            settings.Indent = true;
            return settings;
        }

        /// <summary>
        /// Formats the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public static string FormatXml(string xml, XmlWriterSettings settings)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException(xml);
            }

            XmlDocument doc = new XmlDocument();

            // can't figure out how to make entities non-resolvable
            //xml = System.Text.RegularExpressions.Regex.Replace(xml, @"&[^;]+;", "");
            doc.LoadXml(xml);

            return DoFormatXml(doc, settings, xml.Length);
        }

        private static string DoFormatXml(XmlDocument doc, XmlWriterSettings settings, int stringBuilderSizeHint)
        {
            StringBuilder sb = new StringBuilder(stringBuilderSizeHint > 0 ? stringBuilderSizeHint : 512);

            using (StringWriter stringWriter = new StringWriter(sb))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    doc.Save(xmlWriter);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Formats the XML.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <returns></returns>
        public static string FormatXml(XmlDocument doc)
        {
            return FormatXml(doc, GetDefaultXmlWriterSettings());
        }

        /// <summary>
        /// Formats the XML.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public static string FormatXml(XmlDocument doc, XmlWriterSettings settings)
        {
            return DoFormatXml(doc, settings, -1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static bool NodeHasSignificantChild(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.CDATA &&
                    child.NodeType != XmlNodeType.Comment &&
                    child.NodeType != XmlNodeType.SignificantWhitespace &&
                    child.NodeType != XmlNodeType.Whitespace)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
