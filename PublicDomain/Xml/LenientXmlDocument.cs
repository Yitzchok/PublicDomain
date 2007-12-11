using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using PublicDomain.Exceptions;

namespace PublicDomain.Xml
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
        protected XmlElement m_lastElement;

        /// <summary>
        /// 
        /// </summary>
        protected XmlNode m_attributeTarget;

        /// <summary>
        /// 
        /// </summary>
        protected StringBuilder m_sb;

        /// <summary>
        /// 
        /// </summary>
        protected StringBuilder m_exclamationInstruction;

        /// <summary>
        /// 
        /// </summary>
        protected bool m_createDefaultDocumentElement;

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
            m_exclamationInstruction = new StringBuilder(50);
            m_current = this;
            m_state = State.None;
            m_isAllWhitespace = true;
            m_attribute = null;
            m_attributeValueMatch = '\0';
            m_lastElement = null;

            if (CreateDefaultDocumentElement)
            {
                m_current = AppendChild(GetDefaultDocumentNode());
            }

            for (int i = 0; i < l; i++)
            {
                c = xml[i];

                switch (m_state)
                {
                    case State.EndElementImmediate:

                        if (IsXmlWhitespace(c) || c == '/')
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
                        else if (!IsXmlWhitespace(c))
                        {
                            m_attributeValueMatch = '\0';
                            ContextSwitch(State.StartAttributeValue);
                            m_sb.Append(c);
                            continue;
                        }

                        continue;

                    case State.StartAttributeValue:
                        if (m_attributeValueMatch == '\0' && c == '/')
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
                        else if (m_attributeValueMatch == '\0' && IsXmlWhitespace(c))
                        {
                            ContextSwitch(State.EndAttributeValue);
                            continue;
                        }
                        else if (c == '&')
                        {
                            m_preEntityState = m_state;
                            ContextSwitch(State.StartEntity);
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
                        else if (IsXmlWhitespace(c) || c == '=')
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
                        else if (IsXmlWhitespace(c))
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

                    case State.StartDoctype:

                        if (c == '>')
                        {
                            ContextSwitch(State.None);
                            continue;
                        }
                        else if (c == '<')
                        {
                            ContextSwitch(State.None);
                            i--;
                            continue;
                        }

                        m_exclamationInstruction.Append(c);

                        continue;

                    case State.StartExclamationPoint:

                        if (c == '-')
                        {
                            ContextSwitch(State.InComment);
                            continue;
                        }
                        else if (IsXmlWhitespace(c))
                        {
                            continue;
                        }

                        m_state = State.Element;
                        i--;

                        continue;

                    case State.StartProcessingInstruction:
                        if (c == '>')
                        {
                            ContextSwitch(State.None);
                            continue;
                        }
                        else if (c == '<')
                        {
                            ContextSwitch(State.None);
                            i--;
                            continue;
                        }

                        //.Append(c);
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
                                (xml[i + 2] == 'C' || xml[i + 2] == 'c') &&
                                (xml[i + 3] == 'D' || xml[i + 3] == 'd') &&
                                (xml[i + 4] == 'A' || xml[i + 4] == 'a') &&
                                (xml[i + 5] == 'T' || xml[i + 5] == 't') &&
                                (xml[i + 6] == 'A' || xml[i + 6] == 'a') &&
                                xml[i + 7] == '[')
                            {
                                ContextSwitch(State.InCDATA);
                                i += 7;
                                continue;
                            }
                            else if (i + 7 < l &&
                                (xml[i + 1] == 'D' || xml[i + 1] == 'd') &&
                                (xml[i + 2] == 'O' || xml[i + 2] == 'o') &&
                                (xml[i + 3] == 'C' || xml[i + 3] == 'c') &&
                                (xml[i + 4] == 'T' || xml[i + 4] == 't') &&
                                (xml[i + 5] == 'Y' || xml[i + 5] == 'y') &&
                                (xml[i + 6] == 'P' || xml[i + 6] == 'p') &&
                                (xml[i + 7] == 'E' || xml[i + 7] == 'e'))
                            {
                                m_exclamationInstruction.Length = 0;
                                ContextSwitch(State.StartDoctype);
                                i += 7;
                                continue;
                            }
                            else
                            {
                                ContextSwitch(State.StartExclamationPoint);
                                continue;
                            }
                        }
                        else if (c == '?')
                        {
                            ContextSwitch(State.StartProcessingInstruction);
                            continue;
                        }
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
                            m_preEntityState = State.None;
                            continue;
                        }
                        else
                        {
                            m_sb.Insert(0, "&");
                            m_state = State.None;
                            m_isAllWhitespace = false;
                            ContextSwitch(m_preEntityState);
                            m_preEntityState = State.None;

                            // redo this character
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

                        if (m_isAllWhitespace && !IsXmlWhitespace(c))
                        {
                            m_isAllWhitespace = false;
                        }

                        m_sb.Append(c);
                        continue;
                }
            }

            ContextSwitch(State.Finished);
        }

        /// <summary>
        /// Determines whether [is XML whitespace] [the specified c].
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>
        /// 	<c>true</c> if [is XML whitespace] [the specified c]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsXmlWhitespace(char c)
        {
            return (c == 9 || c == 10 || c == 13 || c == 32);
        }

        /// <summary>
        /// Determines whether [is valid entity character] [the specified p].
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid entity character] [the specified p]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEntityCharacter(char p)
        {
            return char.IsLetterOrDigit(p) || p == '.' || p == '-' || p == '_' || p == ':';
        }

        /// <summary>
        /// Determines whether [is valid entity first character] [the specified p].
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid entity first character] [the specified p]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEntityFirstCharacter(char p)
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
                        if (token != null)
                        {
                            string ns = TryApplyPrefixAndNamespace(ref token);

                            if (m_current.Name.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                            {
                                m_current = m_current.ParentNode;
                            }
                        }
                        break;

                    case State.StartEntity:

                        token = PrepareEntityName(token);

                        if (token != null)
                        {
                            string entityValue = ConvertEntityToValue(token);
                            if (entityValue != null)
                            {
                                if (m_attribute != null)
                                {
                                    m_attribute.Value += entityValue;
                                }
                                else
                                {
                                    InternalAppendChild(CreateTextNode(entityValue), false);
                                }
                            }
                            else
                            {
                                if (m_attribute != null)
                                {
                                    m_attribute.Value += "&" + token + ";";
                                }
                                else
                                {
                                    InternalAppendChild(CreateEntityReference(token), false);
                                }
                            }
                        }

                        break;

                    case State.Element:
                    case State.InElement:
                        token = PrepareElementName(token);
                        if (token != null)
                        {
                            XmlElement el;

                            string ns = TryApplyPrefixAndNamespace(ref token);

                            if (m_lastElement != null)
                            {
                                PostProcessElement(m_lastElement);
                            }

                            if (ns == null)
                            {
                                el = CreateElement(token);
                            }
                            else
                            {
                                el = CreateElement(token, ns);
                            }

                            m_lastElement = el;

                            if (AddNewElementToParent(el))
                            {
                                m_current = m_current.ParentNode;
                            }

                            InternalAppendChild(el, !FinishNewElement(el));
                        }
                        break;

                    case State.StartAttribute:
                        m_attribute = CreateAttribute(token);

                        if (m_attributeTarget != null)
                        {
                            m_attributeTarget.Attributes.SetNamedItem(m_attribute);
                        }
                        else
                        {
                            m_current.Attributes.SetNamedItem(m_attribute);
                        }

                        break;

                    case State.StartAttributeValue:
                        m_attribute.Value += token;
                        PostProcessSetAttributeValue(m_attribute);
                        break;

                    case State.None:
                    case State.EndElement:
                    case State.EndComment:
                    case State.EndCDATA:
                        if (newState == State.StartAttributeValue)
                        {
                            m_attribute.Value += token;
                        }
                        else
                        {
                            if (m_isAllWhitespace)
                            {
                                InternalAppendChild(CreateWhitespace(token), false);
                            }
                            else
                            {
                                InternalAppendChild(CreateTextNode(token), false);
                            }
                        }
                        break;

                    case State.InComment:
                        SetCommentData(m_comment, token);
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

            if (newState == State.EndElement || newState == State.EndElementImmediate)
            {
                m_attribute = null;
            }

            // now check the NEW state
            switch (newState)
            {
                case State.Finished:
                    if (m_lastElement != null)
                    {
                        PostProcessElement(m_lastElement);
                    }
                    if (DocumentElement == null)
                    {
                        m_current.AppendChild(GetDefaultDocumentNode());
                    }
                    break;
                case State.EndElementImmediate:
                    if (m_lastElement == null || !FinishNewElement(m_lastElement))
                    {
                        m_current = m_current.ParentNode;
                    }
                    break;
                case State.StartAttribute:
                    m_attributeValueMatch = '\0';
                    break;
                case State.StartExclamationPoint:

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
        /// Sets the comment data.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="token">The token.</param>
        protected virtual void SetCommentData(XmlComment comment, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                token = token.Replace("--", "");

                int l = token.Length;
                int i;

                for (i = l - 1; i >= 0; i--)
                {
                    if (token[i] != '-')
                    {
                        break;
                    }
                }

                if (i != l - 1)
                {
                    token = token.Substring(0, i + 1);
                }
            }
            comment.Data = token;
        }

        /// <summary>
        /// Adds the comment data.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="moreData">The more data.</param>
        protected virtual void AddCommentData(XmlComment comment, string moreData)
        {
            SetCommentData(comment, comment.Value + moreData);
        }

        private string TryApplyPrefixAndNamespace(ref string token)
        {
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
            return ns;
        }

        /// <summary>
        /// Posts the process element.
        /// </summary>
        /// <param name="lastElement">The last element.</param>
        protected virtual void PostProcessElement(XmlElement lastElement)
        {
        }

        /// <summary>
        /// Converts the entity to value.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        protected virtual string ConvertEntityToValue(string token)
        {
            string result = null;

            if (token[0] == '#')
            {
                result = GetNumericalCharacterReferenceValue(token);
            }

            return result;
        }

        /// <summary>
        /// Gets the numerical character reference value.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        protected string GetNumericalCharacterReferenceValue(string str)
        {
            if (str != null && str.Length > 1)
            {
                if (str[0] == '#')
                {
                    int val;
                    if (str[1] == 'x' || str[1] == 'X')
                    {
                        val = ConversionUtilities.ParseHex(str.Substring(2));
                    }
                    else
                    {
                        val = ConversionUtilities.ParseInt(str.Substring(1));
                    }

                    if (val > 0)
                    {
                        return ((char)val).ToString();
                    }
                }
            }
            return null;
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
                l = token.Length;
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
            return char.IsLetterOrDigit(c) || c == '_' || c == ':' || c == '-';
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
            XmlNode root = DocumentElement;

            // There's already a root but we may be trying to
            // add another element causing multiple roots, which
            // is not allowed
            if (root == null && child.NodeType != XmlNodeType.Element)
            {
                AppendChild(GetDefaultDocumentNode());
                m_current = DocumentElement;
            }
            else if (m_current == this && root != null)
            {
                RemoveChild(root);
                m_current.AppendChild(GetDefaultDocumentNode());
                m_current = DocumentElement;
                m_current.AppendChild(root);
            }

            if (m_current.NodeType == XmlNodeType.Comment)
            {
                // Comments can only have text underneath them
                m_current = m_current.ParentNode;
            }
            m_current.AppendChild(child);
            if (mayHaveChildren)
            {
                if (CanNodeContainSubNodes(child))
                {
                    m_current = child;
                }

                m_attributeTarget = null;
            }
            else
            {
                // save off this ended element in case we find attributes, we 
                // wouldn't want to add them to the wrong element
                m_attributeTarget = child;
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
        /// Creates the default document element.
        /// </summary>
        public virtual bool CreateDefaultDocumentElement
        {
            get
            {
                return m_createDefaultDocumentElement;
            }
            set
            {
                m_createDefaultDocumentElement = value;
            }
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
            StartExclamationPoint,

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
            StartEntity,

            /// <summary>
            /// 
            /// </summary>
            StartDoctype,

            /// <summary>
            /// 
            /// </summary>
            StartProcessingInstruction
        }
    }
}
