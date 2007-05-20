using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Serializer
    {
        /// <summary>
        /// Appends the new element.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <returns></returns>
        public static XmlElement AppendNewElement(XmlDocument doc, XmlNode parent, string elementName)
        {
            return AppendNewElement(doc, parent, elementName, null);
        }

        /// <summary>
        /// Appends the new element.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="elementValue">The element value.</param>
        /// <returns></returns>
        public static XmlElement AppendNewElement(XmlDocument doc, XmlNode parent, string elementName, string elementValue)
        {
            return AppendNewElement(doc, parent, elementName, elementValue, doc.NamespaceURI);
        }

        /// <summary>
        /// Appends the new element.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="elementValue">The element value.</param>
        /// <param name="elementNamespace">The element namespace.</param>
        /// <returns></returns>
        public static XmlElement AppendNewElement(XmlDocument doc, XmlNode parent, string elementName, string elementValue, string elementNamespace)
        {
            XmlElement ret = doc.CreateElement(elementName, elementNamespace);
            parent.AppendChild(ret);
            if (elementValue != null)
            {
                ret.InnerText = elementValue;
            }
            return ret;
        }

        /// <summary>
        /// Appends the new attribute.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <returns></returns>
        public static XmlAttribute AppendNewAttribute(XmlDocument doc, XmlNode parent, string attributeName, string attributeValue)
        {
            return AppendNewAttribute(doc, parent, attributeName, attributeValue, doc.NamespaceURI);
        }

        /// <summary>
        /// Appends the new attribute.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <param name="attributeNamespace">The attribute namespace.</param>
        /// <returns></returns>
        public static XmlAttribute AppendNewAttribute(XmlDocument doc, XmlNode parent, string attributeName, string attributeValue, string attributeNamespace)
        {
            XmlAttribute ret = doc.CreateAttribute(attributeName, attributeNamespace);
            if (attributeValue != null)
            {
                ret.Value = attributeValue;
            }
            parent.Attributes.Append(ret);
            return ret;
        }
    }
}
