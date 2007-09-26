using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PublicDomain.LenientXml
{
    /// <summary>
    /// 
    /// </summary>
    public class LenientHtmlToTextDocument : LenientHtmlDocument
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultTextRootElementName = "text";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultHtmlNewline = "\n";

        private string m_defaultHtmlNewLine = DefaultHtmlNewline;

        /// <summary>
        /// Initializes a new instance of the <see cref="LenientHtmlToTextDocument"/> class.
        /// </summary>
        public LenientHtmlToTextDocument()
            : base()
        {
            m_defaultRootElementName = DefaultTextRootElementName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LenientHtmlToTextDocument"/> class.
        /// </summary>
        /// <param name="nt">The nt.</param>
        public LenientHtmlToTextDocument(XmlNameTable nt)
            : base(nt)
        {
            m_defaultRootElementName = DefaultTextRootElementName;
        }

        /// <summary>
        /// Contexts the switch.
        /// </summary>
        /// <param name="newState">The new state.</param>
        protected override void ContextSwitch(LenientXmlDocument.State newState)
        {
            if (FirstChild == null)
            {
                m_current = AppendChild(GetDefaultDocumentNode());
            }

            if (m_sb.Length > 0)
            {
                string token = m_sb.ToString();

                switch (m_state)
                {
                    case State.Element:
                    case State.InElement:
                        token = token.ToLower();
                        if (token == "p")
                        {
                            m_current.AppendChild(CreateWhitespace(m_defaultHtmlNewLine + m_defaultHtmlNewLine));
                        }
                        else if (token == "br")
                        {
                            m_current.AppendChild(CreateWhitespace(m_defaultHtmlNewLine));
                        }
                        break;

                    case State.None:
                    case State.EndElement:
                    case State.EndComment:
                    case State.EndCDATA:
                    case State.InCDATA:
                        if (m_isAllWhitespace && m_state != State.InCDATA)
                        {
                            m_current.AppendChild(CreateWhitespace(token));
                        }
                        else
                        {
                            m_current.AppendChild(CreateTextNode(token));
                        }
                        break;
                    case State.StartEntity:

                        token = PrepareEntityName(token);

                        if (token != null)
                        {
                            string entityValue = ConvertEntityToValue(token);

                            if (entityValue != null)
                            {
                                m_current.AppendChild(CreateTextNode(entityValue));
                            }
                            else
                            {
                                m_current.AppendChild(CreateTextNode("&" + token + ";"));
                            }
                        }

                        break;
                }
            }

            ResetAfterContextSwitch();
            m_state = newState;
        }

        /// <summary>
        /// Gets or sets the HTML newline.
        /// </summary>
        /// <value>The HTML newline.</value>
        public virtual string HtmlNewline
        {
            get
            {
                return m_defaultHtmlNewLine;
            }
            set
            {
                m_defaultHtmlNewLine = value;
            }
        }

        /// <summary>
        /// Converts the entity to value.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        protected virtual string ConvertEntityToValue(string token)
        {
            token = token.ToLower();
            switch (token)
            {
                case "nbsp":
                    return " ";
                case "gt":
                    return ">";
                case "lt":
                    return "<";
                case "amp":
                    return "&";
                case "quot":
                    return "\"";
            }
            return null;
        }
    }
}
