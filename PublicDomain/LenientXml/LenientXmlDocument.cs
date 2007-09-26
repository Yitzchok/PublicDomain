using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using PublicDomain.Exceptions;

namespace PublicDomain.LenientXml
{
    /// <summary>
    /// 
    /// </summary>
    public class LenientXmlDocument : XmlDocument
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultRootElementName = "root";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultEmptyXml = "<" + DefaultRootElementName + " />";

        private string m_defaultRootElementName;
        private bool m_ignoreDtd;

        private State state = State.None;
        private StringBuilder sb;
        private StringBuilder entity;
        private XmlNode current;
        private bool isAllWhitespace;
        private XmlAttribute attribute;
        private char attributeValueMatch;
        private XmlComment comment;
        private XmlCDataSection cdata;
        private State entitySaveState;

        /// <summary>
        /// Initializes a new instance of the <see cref="LenientXmlDocument"/> class.
        /// </summary>
        public LenientXmlDocument()
            : this(DefaultRootElementName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LenientXmlDocument"/> class.
        /// </summary>
        /// <param name="defaultRootElementName">Name of the default root element.</param>
        public LenientXmlDocument(string defaultRootElementName)
            : base()
        {
            m_defaultRootElementName = defaultRootElementName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LenientXmlDocument"/> class.
        /// </summary>
        /// <param name="nt">The nt.</param>
        public LenientXmlDocument(XmlNameTable nt)
            : this(nt, DefaultRootElementName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LenientXmlDocument"/> class.
        /// </summary>
        /// <param name="nt">The nt.</param>
        /// <param name="defaultRootElementName">Name of the default root element.</param>
        public LenientXmlDocument(XmlNameTable nt, string defaultRootElementName)
            : base(nt)
        {
            m_defaultRootElementName = defaultRootElementName;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [ignore DTD].
        /// </summary>
        /// <value><c>true</c> if [ignore DTD]; otherwise, <c>false</c>.</value>
        public bool IgnoreDtd
        {
            get
            {
                return m_ignoreDtd;
            }
            set
            {
                m_ignoreDtd = value;
            }
        }

        /// <summary>
        /// Loads the XML document from the specified string.
        /// </summary>
        /// <param name="xml">String containing the XML document to load.</param>
        /// <exception cref="T:System.Xml.XmlException">There is a load or parse error in the XML. In this case, the document remains empty. </exception>
        public override void LoadXml(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }
            RemoveAll();
            int l = xml.Length;
            char c;
            sb = new StringBuilder(100);
            entity = new StringBuilder(20);
            current = this;
            state = State.None;
            isAllWhitespace = true;
            attribute = null;
            attributeValueMatch = '\0';

            for (int i = 0; i < l; i++)
            {
                c = xml[i];

                switch (state)
                {
                    case State.EndElementImmediate:

                        if (IsWhitespace(c) || c == '/')
                        {
                            // kill as much white space as we can
                            // while we're in the end of the element
                            continue;
                        }
                        else if (c == '>')
                        {
                            // We've already hit the end slash, and
                            // now we've hit the final >, so we just
                            // reset to a base state
                            state = State.None;
                        }
                        else if (c == '<')
                        {
                            ContextSwitch(State.Element);
                            continue;
                        }
                        else
                        {
                            state = State.None;
                            isAllWhitespace = false;
                            sb.Append(c);
                        }

                        continue;
                    case State.EndAttribute:
                        if (c == '/')
                        {
                            ContextSwitch(State.EndElementImmediate);
                            continue;
                        }
                        else if (c == '>')
                        {
                            ContextSwitch(State.EndElement);
                            continue;
                        }
                        else if (CharUtilities.IsQuoteCharacter(c))
                        {
                            attributeValueMatch = c;
                            ContextSwitch(State.StartAttributeValue);
                            continue;
                        }
                        else if (!IsWhitespace(c))
                        {
                            attributeValueMatch = '\0';
                            ContextSwitch(State.StartAttributeValue);
                            sb.Append(c);
                            continue;
                        }

                        continue;

                    case State.StartAttributeValue:
                        if (c == '/')
                        {
                            ContextSwitch(State.EndElementImmediate);
                            continue;
                        }
                        else if (c == '>')
                        {
                            ContextSwitch(State.EndElement);
                            continue;
                        }
                        else if (c == attributeValueMatch)
                        {
                            ContextSwitch(State.EndAttributeValue);
                            continue;
                        }
                        else if (attributeValueMatch == '\0' && IsWhitespace(c))
                        {
                            ContextSwitch(State.EndAttributeValue);
                            continue;
                        }

                        sb.Append(c);

                        continue;

                    case State.StartAttribute:
                        if (c == '/')
                        {
                            ContextSwitch(State.EndElementImmediate);
                            continue;
                        }
                        else if (c == '>')
                        {
                            ContextSwitch(State.EndElement);
                            continue;
                        }
                        else if (IsValidNameCharacter(c))
                        {
                            sb.Append(c);
                        }
                        else if (IsWhitespace(c) || c == '=')
                        {
                            ContextSwitch(State.EndAttribute);
                            continue;
                        }

                        continue;

                    case State.InElement:
                    case State.EndAttributeValue:
                        if (c == '/')
                        {
                            ContextSwitch(State.EndElementImmediate);
                            continue;
                        }
                        else if (c == '>')
                        {
                            ContextSwitch(State.EndElement);
                            continue;
                        }
                        else if (IsWhitespace(c))
                        {
                            continue;
                        }
                        else if (IsValidFirstNameCharacter(c))
                        {
                            ContextSwitch(State.StartAttribute);
                            sb.Append(c);
                            continue;
                        }

                        continue;

                    case State.CloseElement:
                        if (char.IsWhiteSpace(c))
                        {
                            continue;
                        }
                        else if (c == '>')
                        {
                            ContextSwitch(State.EndCloseElement);
                            state = State.None;
                            continue;
                        }

                        isAllWhitespace = false;
                        sb.Append(c);

                        continue;

                    case State.InComment:

                        if (c == '-' && i + 2 < l && xml[i + 1] == '-' && xml[i + 2] == '>')
                        {
                            ContextSwitch(State.EndComment);
                            i += 2;
                            continue;
                        }

                        if (sb.Length == 0 && c == '-')
                        {
                            continue;
                        }

                        sb.Append(c);

                        continue;

                    case State.InCDATA:

                        if (c == ']' && i + 2 < l && xml[i + 1] == ']' && xml[i + 2] == '>')
                        {
                            ContextSwitch(State.EndCDATA);
                            i += 2;
                            continue;
                        }

                        sb.Append(c);
                        continue;

                    case State.StartComment1:

                        if (c == '-')
                        {
                            ContextSwitch(State.InComment);
                            continue;
                        }
                        else if (IsWhitespace(c))
                        {
                            continue;
                        }

                        state = State.Element;
                        i--;

                        continue;
                    case State.Element:
                        if (c == '>')
                        {
                            ContextSwitch(State.EndElement);
                            continue;
                        }
                        else if (char.IsWhiteSpace(c))
                        {
                            if (sb.Length > 0)
                            {
                                ContextSwitch(State.InElement);
                            }
                            continue;
                        }
                        else if (c == '!')
                        {
                            if (i + 7 < l &&
                                xml[i + 1] == '[' &&
                                xml[i + 2] == 'C' &&
                                xml[i + 3] == 'D' &&
                                xml[i + 4] == 'A' &&
                                xml[i + 5] == 'T' &&
                                xml[i + 6] == 'A' &&
                                xml[i + 7] == '[')
                            {
                                ContextSwitch(State.InCDATA);
                                i += 7;
                                continue;
                            }
                            else
                            {
                                ContextSwitch(State.StartComment1);
                                continue;
                            }
                        }
                        //else if (c == '?')
                        //{
                        //    continue;
                        //}
                        else if (c == '/')
                        {
                            if (sb.Length == 0)
                            {
                                ContextSwitch(State.CloseElement);
                            }
                            else
                            {
                                ContextSwitch(State.EndElementImmediate);
                            }
                            continue;
                        }

                        isAllWhitespace = false;
                        sb.Append(c);
                        continue;

                    case State.StartEntity:

                        if (entity.Length == 0 && IsValidEntityFirstCharacter(c))
                        {
                            entity.Append(c);
                            continue;
                        }
                        else if (entity.Length > 0 && IsValidEntityCharacter(c))
                        {
                            entity.Append(c);
                            continue;
                        }
                        else if (c == ';' && entity.Length > 0)
                        {
                            state = entitySaveState;
                            string entityName = entity.ToString();
                            sb.Append(ProcessEntity(entityName));
                            isAllWhitespace = false;
                            continue;
                        }
                        else
                        {
                            state = entitySaveState;
                            sb.Append("&amp;");
                            isAllWhitespace = false;
                            i--;

                            if (entity.Length > 0)
                            {
                                sb.Append(entity.ToString());
                            }
                            continue;
                        }

                    default:

                        // Fall back in case we are not in a state
                        // in which we should special case

                        if (c == '<')
                        {
                            ContextSwitch(State.Element);
                            continue;
                        }
                        else if (c == '&')
                        {
                            entitySaveState = state;
                            entity.Length = 0;
                            state = State.StartEntity;
                            continue;
                        }

                        if (isAllWhitespace && !IsWhitespace(c))
                        {
                            isAllWhitespace = false;
                        }

                        sb.Append(c);
                        continue;
                }
            }

            ContextSwitch(State.Finished);
        }

        /// <summary>
        /// Processes the entity.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns></returns>
        protected virtual string ProcessEntity(string entityName)
        {
            return "&" + entityName + ";";
        }

        private bool IsWhitespace(char c)
        {
            return (c == 0x9 || c == 0x10 || c == 0x13 || c == 0x20);
        }

        private static bool IsValidEntityCharacter(char p)
        {
            return char.IsLetterOrDigit(p) || p == '.' || p == '-' || p == '_' || p == ':';
        }

        private static bool IsValidEntityFirstCharacter(char p)
        {
            return char.IsLetter(p) || p == '_' || p == ':' || p == '#';
        }

        /// <summary>
        /// Contexts the switch.
        /// </summary>
        /// <param name="newState">The new state.</param>
        protected virtual void ContextSwitch(State newState)
        {
            // Process the old state
            if (sb.Length > 0)
            {
                string token = sb.ToString();

                // this is the LAST state
                switch (state)
                {
                    case State.CloseElement:
                        token = PrepareElementName(token);
                        if (token != null && current.Name.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                        {
                            current = current.ParentNode;
                        }
                        break;

                    case State.Element:
                    case State.InElement:
                        token = PrepareElementName(token);
                        if (token != null)
                        {
                            XmlElement el;

                            int colIndex = token.IndexOf(':');
                            if (colIndex > 0)
                            {
                                string prefix = token.Substring(0, colIndex);
                                string ns = null;
                                if (DocumentElement != null)
                                {
                                    ns = DocumentElement.GetNamespaceOfPrefix(prefix);
                                }
                                if (string.IsNullOrEmpty(ns))
                                {
                                    ns = "urn:unknown";
                                }
                                el = CreateElement(token, ns);
                            }
                            else
                            {
                                el = CreateElement(token);
                            }

                            InternalAppendChild(el, !FinishNewElement(el));
                        }
                        break;

                    case State.StartAttribute:
                        attribute = CreateAttribute(token);
                        current.Attributes.Append(attribute);

                        break;

                    case State.StartAttributeValue:
                        attribute.Value = token;
                        break;

                    case State.None:
                    case State.EndElement:
                    case State.EndComment:
                    case State.EndCDATA:
                        if (isAllWhitespace)
                        {
                            InternalAppendChild(CreateWhitespace(token), false);
                        }
                        else
                        {
                            InternalAppendChild(CreateTextNode(token), false);
                        }
                        break;

                    case State.InComment:
                        comment.Data = token;
                        break;

                    case State.InCDATA:
                        cdata.Data = token;
                        break;

                    default:
                        break;
                }
            }

            // now check the NEW state
            switch (newState)
            {
                case State.Finished:
                    if (FirstChild == null)
                    {
                        current.AppendChild(GetDefaultDocumentNode());
                    }
                    break;
                case State.EndElementImmediate:
                    current = current.ParentNode;
                    break;
                case State.StartAttribute:
                    attributeValueMatch = '\0';
                    break;
                case State.StartComment1:

                    comment = CreateComment(null);
                    InternalAppendChild(comment, false);

                    break;

                case State.InCDATA:
                    cdata = CreateCDataSection(null);
                    InternalAppendChild(cdata, false);
                    break;
            }

            ResetAfterContextSwitch();

            state = newState;
        }

        /// <summary>
        /// Prepares the name of the element.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        protected virtual string PrepareElementName(string token)
        {
            //http://www.w3.org/TR/REC-xml/#NT-NameChar

            // First, ensure the first character
            int l = token.Length;
            char c;
            int i;
            StringBuilder tokenBuilder = null;

            for (i = 0; i < l; i++)
            {
                c = token[i];
                if (IsValidFirstNameCharacter(c))
                {
                    break;
                }
            }

            if (i == l)
            {
                return null;
            }
            else if (i > 0)
            {
                token = token.Substring(i);
            }

            for (i = 1; i < l; i++)
            {
                c = token[i];
                if (!IsValidNameCharacter(c))
                {
                    if (tokenBuilder == null)
                    {
                        tokenBuilder = new StringBuilder(token.Length);
                        tokenBuilder.Append(token.Substring(0, i));
                    }
                }
                else if (tokenBuilder != null)
                {
                    tokenBuilder.Append(c);
                }
            }

            if (tokenBuilder != null)
            {
                token = tokenBuilder.ToString();
            }

            return token;
        }

        /// <summary>
        /// Determines whether [is valid first name character] [the specified c].
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid first name character] [the specified c]; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsValidFirstNameCharacter(char c)
        {
            return char.IsLetter(c) || c == '_' || c == ':';
        }

        /// <summary>
        /// Determines whether [is valid name character] [the specified c].
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid name character] [the specified c]; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsValidNameCharacter(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_' || c == ':';
        }

        /// <summary>
        /// Finishes the new element.
        /// </summary>
        /// <param name="el">The el.</param>
        /// <returns></returns>
        protected virtual bool FinishNewElement(XmlElement el)
        {
            return false;
        }

        private void InternalAppendChild(XmlNode child, bool mayHaveChildren)
        {
            XmlNode root = FirstChild;

            // There's already a root but we may be trying to
            // add another element causing multiple roots, which
            // is not allowed
            if (root == null && child.NodeType != XmlNodeType.Element)
            {
                AppendChild(GetDefaultDocumentNode());
                current = FirstChild;
            }
            else if (current == this && root != null)
            {
                RemoveChild(root);
                current.AppendChild(GetDefaultDocumentNode());
                current = FirstChild;
                current.AppendChild(root);
            }

            current.AppendChild(child);
            if (CanNodeContainSubNodes(child) && mayHaveChildren)
            {
                current = child;
            }
        }

        private bool CanNodeContainSubNodes(XmlNode child)
        {
            return child.NodeType == XmlNodeType.Element;
        }

        private void ResetAfterContextSwitch()
        {
            sb.Length = 0;
            isAllWhitespace = true;
        }

        /// <summary>
        /// Gets the default document node.
        /// </summary>
        /// <returns></returns>
        protected virtual XmlNode GetDefaultDocumentNode()
        {
            return CreateElement(m_defaultRootElementName);
        }

        /// <summary>
        /// 
        /// </summary>
        protected enum State
        {
            /// <summary>
            /// 
            /// </summary>
            None,

            /// <summary>
            /// Element has opened
            /// </summary>
            Element,

            /// <summary>
            /// Within whitespace in an element
            /// </summary>
            InElement,

            /// <summary>
            /// Opening of an element has finished. This is *not*
            /// the case of a self-enclosed tag.
            /// </summary>
            EndElement,

            /// <summary>
            /// Within closing tag
            /// </summary>
            CloseElement,

            /// <summary>
            /// The end of a closing tag
            /// </summary>
            EndCloseElement,

            /// <summary>
            /// Self-enclosed tag is finished
            /// </summary>
            EndElementImmediate,

            /// <summary>
            /// Processing of the entire document is finished
            /// </summary>
            Finished,

            /// <summary>
            /// 
            /// </summary>
            StartAttribute,

            /// <summary>
            /// 
            /// </summary>
            EndAttribute,

            /// <summary>
            /// 
            /// </summary>
            StartAttributeValue,

            /// <summary>
            /// 
            /// </summary>
            EndAttributeValue,

            /// <summary>
            /// 
            /// </summary>
            StartComment1,

            /// <summary>
            /// 
            /// </summary>
            InComment,

            /// <summary>
            /// 
            /// </summary>
            EndComment,

            /// <summary>
            /// 
            /// </summary>
            InCDATA,

            /// <summary>
            /// 
            /// </summary>
            EndCDATA,

            /// <summary>
            /// 
            /// </summary>
            StartEntity
        }
    }
}
