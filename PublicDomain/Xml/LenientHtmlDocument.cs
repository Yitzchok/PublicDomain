using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PublicDomain.Xml
{
    /// <summary>
    /// 
    /// </summary>
    public class LenientHtmlDocument : LenientXmlDocument
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultRootHtmlElementName = "html";

        /// <summary>
        /// Initializes a new instance of the <see cref="LenientHtmlDocument"/> class.
        /// </summary>
        public LenientHtmlDocument()
            : base(DefaultRootHtmlElementName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LenientHtmlDocument"/> class.
        /// </summary>
        /// <param name="nt">The nt.</param>
        public LenientHtmlDocument(XmlNameTable nt)
            : base(nt, DefaultRootHtmlElementName)
        {
        }

        /// <summary>
        /// Finishes the new element.
        /// </summary>
        /// <param name="el">The el.</param>
        /// <returns></returns>
        protected override bool FinishNewElement(XmlElement el)
        {
            string name = el.LocalName.ToLower();
            return name == "br" || name == "meta" || name == "link";
        }

        /// <summary>
        /// Prepares the name of the entity.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        protected override string PrepareEntityName(string token)
        {
            if (token == "#160")
            {
                return "nbsp";
            }
            // How to place a value instead of an entity:
            //if (token == "token")
            //{
            //    InternalAppendChild(CreateTextNode("text"), false);
            //    return null;
            //}
            return base.PrepareEntityName(token);
        }

        /// <summary>
        /// Adds the new element to parent.
        /// </summary>
        /// <param name="el">The el.</param>
        /// <returns></returns>
        protected override bool AddNewElementToParent(XmlElement el)
        {
            if (m_current != null && m_current.Name.ToLower() == "option" && el.Name.ToLower() == "option")
            {
                return true;
            }
            return base.AddNewElementToParent(el);
        }

        /// <summary>
        /// Internals the append child.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="mayHaveChildren">if set to <c>true</c> [may have children].</param>
        protected override void InternalAppendChild(XmlNode child, bool mayHaveChildren)
        {
            if (m_current != null)
            {
                // wrap <SCRIPT> innards with a comment if they're not already wrapped by a comment or CDATA section
                if (m_current.NodeType == XmlNodeType.Element && m_current.Name.ToLower() == "script" && child.NodeType != XmlNodeType.CDATA && child.NodeType != XmlNodeType.Comment && child.NodeType != XmlNodeType.Whitespace && child.NodeType != XmlNodeType.SignificantWhitespace)
                {
                    m_current = m_current.AppendChild(CreateComment(null));
                    XmlCharacterData charData = child as XmlCharacterData;
                    if (charData != null)
                    {
                        ((XmlComment)m_current).Value += charData.Value;
                        return;
                    }
                }
                else if (m_current.NodeType == XmlNodeType.Comment)
                {
                    XmlCharacterData charData = child as XmlCharacterData;
                    if (charData != null)
                    {
                        ((XmlComment)m_current).Value += charData.Value;
                        return;
                    }
                    else
                    {
                        m_current = m_current.ParentNode;
                    }
                }
            }
            base.InternalAppendChild(child, mayHaveChildren);
        }
    }
}
