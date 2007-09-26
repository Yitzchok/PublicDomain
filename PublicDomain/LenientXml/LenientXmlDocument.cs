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

        /// <summary>
        /// 
        /// </summary>
        protected XmlNode m_current;

        /// <summary>
        /// 
        /// </summary>
        protected StringBuilder m_sb;

        /// <summary>
        /// 
        /// </summary>
        protected bool m_isAllWhitespace;

        /// <summary>
        /// 
        /// </summary>
        protected State m_state = State.None;

        /// <summary>
        /// 
        /// </summary>
        protected string m_defaultRootElementName;

        /// <summary>
        /// 
        /// </summary>
        protected bool m_ignoreDtd;

        /// <summary>
        /// 
        /// </summary>
        protected XmlAttribute m_attribute;

        /// <summary>
        /// 
        /// </summary>
        protected char m_attributeValueMatch;

        /// <summary>
        /// 
        /// </summary>
        protected XmlComment m_comment;

        /// <summary>
        /// 
        /// </summary>
        protected XmlCDataSection m_cdata;

        /// <summary>
        /// 
        /// </summary>
        protected State m_preEntityState;

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
            m_sb = new StringBuilder(100);
            m_current = this;
            m_state = State.None;
            m_isAllWhitespace = true;
            m_attribute = null;
            m_attributeValueMatch = '\0';

            for (int i = 0; i < l; i++)
            {
                c = xml[i];

                switch (m_state)
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
                            m_state = State.None;
                        }
                        else if (c == '<')
                        {
                            ContextSwitch(State.Element);
                            continue;
                        }
                        else
                        {
                            m_state = State.None;
                            m_isAllWhitespace = false;
                            m_sb.Append(c);
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
                            m_attributeValueMatch = c;
                            ContextSwitch(State.StartAttributeValue);
                            continue;
                        }
                        else if (!IsWhitespace(c))
                        {
                            m_attributeValueMatch = '\0';
                            ContextSwitch(State.StartAttributeValue);
                            m_sb.Append(c);
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
                        else if (c == m_attributeValueMatch)
                        {
                            ContextSwitch(State.EndAttributeValue);
                            continue;
                        }
                        else if (m_attributeValueMatch == '\0' && IsWhitespace(c))
                        {
                            ContextSwitch(State.EndAttributeValue);
                            continue;
                        }

                        m_sb.Append(c);

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
                            m_sb.Append(c);
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
                            m_sb.Append(c);
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
                            m_state = State.None;
                            continue;
                        }

                        m_isAllWhitespace = false;
                        m_sb.Append(c);

                        continue;

                    case State.InComment:

                        if (c == '-' && i + 2 < l && xml[i + 1] == '-' && xml[i + 2] == '>')
                        {
                            ContextSwitch(State.EndComment);
                            i += 2;
                            continue;
                        }

                        if (m_sb.Length == 0 && c == '-')
                        {
                            continue;
                        }

                        m_sb.Append(c);

                        continue;

                    case State.InCDATA:

                        if (c == ']' && i + 2 < l && xml[i + 1] == ']' && xml[i + 2] == '>')
                        {
                            ContextSwitch(State.EndCDATA);
                            i += 2;
                            continue;
                        }

                        m_sb.Append(c);
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

                        m_state = State.Element;
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
                            if (m_sb.Length > 0)
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
                            if (m_sb.Length == 0)
                            {
                                ContextSwitch(State.CloseElement);
                            }
                            else
                            {
                                ContextSwitch(State.EndElementImmediate);
                            }
                            continue;
                        }

                        m_isAllWhitespace = false;
                        m_sb.Append(c);
                        continue;

                    case State.StartEntity:

                        if (m_sb.Length == 0 && IsValidEntityFirstCharacter(c))
                        {
                            m_sb.Append(c);
                            continue;
                        }
                        else if (m_sb.Length > 0 && IsValidEntityCharacter(c))
                        {
                            m_sb.Append(c);
                            continue;
                        }
                        else if (c == ';' && m_sb.Length > 0)
                        {
                            ContextSwitch(m_preEntityState);
                            continue;
                        }
                        else
                        {
                            m_sb.Append("amp");
                            ContextSwitch(m_preEntityState);
                            i--;
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
                            m_preEntityState = m_state;
                            ContextSwitch(State.StartEntity);
                            continue;
                        }

                        if (m_isAllWhitespace && !IsWhitespace(c))
                        {
                            m_isAllWhitespace = false;
                        }

                        m_sb.Append(c);
                        continue;
                }
            }

            ContextSwitch(State.Finished);
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
            if (m_sb.Length > 0)
            {
                string token = m_sb.ToString();

                // this is the LAST state
                switch (m_state)
                {
                    case State.CloseElement:
                        token = PrepareElementName(token);
                        if (token != null && m_current.Name.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                        {
                            m_current = m_current.ParentNode;
                        }
                        break;

                    case State.StartEntity:

                        token = PrepareEntityName(token);

                        if (token != null)
                        {
                            XmlEntityReference entity = CreateEntityReference(token);
                            InternalAppendChild(entity, false);
                        }

                        break;

                    case State.Element:
                    case State.InElement:
                        token = PrepareElementName(token);
                        if (token != null)
                        {
                            XmlElement el;

                            int colIndex = token.IndexOf(':');
                            string ns = null;
                            if (colIndex > 0)
                            {
                                string prefix = token.Substring(0, colIndex);
                                ns = FindNamespaceByPrefix(prefix);
                            }

                            if (ns == null)
                            {
                                // There was no namespace specified, but see
                                // if the implemenation wants to set a namespace
                                string prefix;
                                if (TryChangeNamespace(token, out ns, out prefix) && !string.IsNullOrEmpty(prefix))
                                {
                                    token = prefix + ":" + token;
                                    if (ns == null)
                                    {
                                        ns = FindNamespaceByPrefix(prefix);
                                    }
                                }
                            }

                            if (ns == null)
                            {
                                el = CreateElement(token);
                            }
                            else
                            {
                                el = CreateElement(token, ns);
                            }

                            if (AddNewElementToParent(el))
                            {
                                m_current = m_current.ParentNode;
                            }

                            InternalAppendChild(el, !FinishNewElement(el));
                        }
                        break;

                    case State.StartAttribute:
                        m_attribute = CreateAttribute(token);
                        m_current.Attributes.Append(m_attribute);

                        break;

                    case State.StartAttributeValue:
                        m_attribute.Value = token;
                        PostProcessSetAttributeValue(m_attribute);
                        break;

                    case State.None:
                    case State.EndElement:
                    case State.EndComment:
                    case State.EndCDATA:
                        if (m_isAllWhitespace)
                        {
                            InternalAppendChild(CreateWhitespace(token), false);
                        }
                        else
                        {
                            InternalAppendChild(CreateTextNode(token), false);
                        }
                        break;

                    case State.InComment:
                        m_comment.Data = token;
                        PostProcessSetCommentData(m_comment);
                        break;

                    case State.InCDATA:
                        m_cdata.Data = token;
                        PostProcessSetCData(m_cdata);
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
                        m_current.AppendChild(GetDefaultDocumentNode());
                    }
                    break;
                case State.EndElementImmediate:
                    m_current = m_current.ParentNode;
                    break;
                case State.StartAttribute:
                    m_attributeValueMatch = '\0';
                    break;
                case State.StartComment1:

                    m_comment = CreateComment(null);
                    InternalAppendChild(m_comment, false);

                    break;

                case State.InCDATA:
                    m_cdata = CreateCDataSection(null);
                    InternalAppendChild(m_cdata, false);
                    break;
            }

            ResetAfterContextSwitch();

            m_state = newState;
        }

        /// <summary>
        /// Posts the process set C data.
        /// </summary>
        /// <param name="m_cdata">The m_cdata.</param>
        protected virtual void PostProcessSetCData(XmlCDataSection m_cdata)
        {
        }

        /// <summary>
        /// Posts the process set comment data.
        /// </summary>
        /// <param name="m_comment">The m_comment.</param>
        protected virtual void PostProcessSetCommentData(XmlComment m_comment)
        {
        }

        /// <summary>
        /// Posts the process set attribute value.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        protected virtual void PostProcessSetAttributeValue(XmlAttribute attribute)
        {
        }

        /// <summary>
        /// Finds the namespace by prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        protected virtual string FindNamespaceByPrefix(string prefix)
        {
            string ns = null;
            if (DocumentElement != null)
            {
                ns = DocumentElement.GetNamespaceOfPrefix(prefix);
            }
            if (string.IsNullOrEmpty(ns))
            {
                ns = GetDefaultNamespaceUriForPrefix(prefix);
            }
            return ns;
        }

        /// <summary>
        /// Tries the change namespace.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="ns">The ns.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        protected virtual bool TryChangeNamespace(string token, out string ns, out string prefix)
        {
            ns = prefix = null;
            return false;
        }

        /// <summary>
        /// Gets the default namespace URI for prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        protected virtual string GetDefaultNamespaceUriForPrefix(string prefix)
        {
            return "urn:" + prefix;
        }

        /// <summary>
        /// Adds the new element to parent.
        /// </summary>
        /// <param name="el">The el.</param>
        /// <returns></returns>
        protected virtual bool AddNewElementToParent(XmlElement el)
        {
            return false;
        }

        /// <summary>
        /// Prepares the name of the entity.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        protected virtual string PrepareEntityName(string token)
        {
            return token.ToLower();
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

        /// <summary>
        /// Internals the append child.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="mayHaveChildren">if set to <c>true</c> [may have children].</param>
        protected virtual void InternalAppendChild(XmlNode child, bool mayHaveChildren)
        {
            XmlNode root = FirstChild;

            // There's already a root but we may be trying to
            // add another element causing multiple roots, which
            // is not allowed
            if (root == null && child.NodeType != XmlNodeType.Element)
            {
                AppendChild(GetDefaultDocumentNode());
                m_current = FirstChild;
            }
            else if (m_current == this && root != null)
            {
                RemoveChild(root);
                m_current.AppendChild(GetDefaultDocumentNode());
                m_current = FirstChild;
                m_current.AppendChild(root);
            }

            m_current.AppendChild(child);
            if (CanNodeContainSubNodes(child) && mayHaveChildren)
            {
                m_current = child;
            }
        }

        private bool CanNodeContainSubNodes(XmlNode child)
        {
            return child.NodeType == XmlNodeType.Element;
        }

        /// <summary>
        /// Resets the after context switch.
        /// </summary>
        protected void ResetAfterContextSwitch()
        {
            m_sb.Length = 0;
            m_isAllWhitespace = true;
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
