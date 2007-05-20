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
        /// Formats the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static string FormatXml(string xml)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Auto;
            settings.Indent = true;

            return FormatXml(xml, settings);
        }

        /// <summary>
        /// Formats the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public static string FormatXml(string xml, XmlWriterSettings settings)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            StringBuilder sb = new StringBuilder(xml.Length);

            using (StringWriter stringWriter = new StringWriter(sb))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    doc.Save(xmlWriter);
                }
            }

            return sb.ToString();
        }
    }
}
