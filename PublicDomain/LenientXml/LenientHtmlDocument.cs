using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PublicDomain.LenientXml
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
            return el.LocalName.ToLower() == "br";
        }

        /// <summary>
        /// Prepares the name of the entity.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        protected override string PrepareEntityName(string token)
        {
            // How to place a value instead of an entity:
            //if (token == "token")
            //{
            //    InternalAppendChild(CreateTextNode("text"), false);
            //    return null;
            //}
            return base.PrepareEntityName(token);
        }
    }
}
