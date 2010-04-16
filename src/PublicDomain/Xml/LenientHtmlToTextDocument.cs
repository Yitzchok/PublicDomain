using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PublicDomain.Xml
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
            if (DocumentElement == null)
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
                        if (m_preEntityState != State.StartAttributeValue)
                        {
                            if (m_isAllWhitespace && m_state != State.InCDATA)
                            {
                                m_current.AppendChild(CreateWhitespace(token));
                            }
                            else
                            {
                                m_current.AppendChild(CreateTextNode(token));
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
        protected override string ConvertEntityToValue(string token)
        {
            string result = null;

            // http://en.wikipedia.org/wiki/List_of_XML_and_HTML_character_entity_references
            token = token.ToLower();

            switch (token)
            {
                case "nbsp":
                    return " ";
                case "amp":
                    return "&";
                case "lt":
                    return "<";
                case "gt":
                    return ">";
                case "quot":
                    return "\"";
                case "iexcl":
                    return "¡";
                case "cent":
                    return "¢";
                case "pound":
                    return "£";
                case "curren":
                    return "¤";
                case "yen":
                    return "¥";
                case "brvbar":
                    return "¦";
                case "sect":
                    return "§";
                case "uml":
                    return "¨";
                case "copy":
                    return "©";
                case "ordf":
                    return "ª";
                case "laquo":
                    return "«";
                case "not":
                    return "¬";
                case "shy":
                    return "-";
                case "&reg":
                    return "®";
                case "macr":
                    return "¯";
                case "deg":
                    return "°";
                case "plusmn":
                    return "±";
                case "sup2":
                    return "²";
                case "sup3":
                    return "³";
                case "acute":
                    return "´";
                case "micro":
                    return "µ";
                case "para":
                    return "¶";
                case "middot":
                    return "·";
                case "cedil":
                    return "¸";
                case "sup1":
                    return "¹";
                case "ordm":
                    return "º";
                case "raquo":
                    return "»";
                case "frac14":
                    return "¼";
                case "frac12":
                    return "½";
                case "frac34":
                    return "¾";
                case "iquest":
                    return "¿";
                case "times":
                    return "×";
                case "divide":
                    return "÷";
                case "eth":
                    return "ð";
                case "thorn":
                    return "Þ";
                case "aelig":
                    return "æ";
                case "oelig":
                    return "œ";
                case "aring":
                    return "Å";
                case "oslash":
                    return "Ø";
                case "ccedil":
                    return "ç";
                case "szlig":
                    return "ß";
                case "ntilde":
                    return "ñ";
            }

            return result;
        }
    }
}
