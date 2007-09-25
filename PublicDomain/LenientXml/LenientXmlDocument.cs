using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

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

        private bool m_ignoreDtd;

        private StringBuilder sbElementName = new StringBuilder(10);
        private StringBuilder sbAttributeName = new StringBuilder(10);
        private StringBuilder sbValue = new StringBuilder(10);
        private StringBuilder sbText = new StringBuilder(10);
        private LenientXmlState state = LenientXmlState.Start;
        private XmlNode cur;
        private bool elementNameComplete;
        private bool isFirstAttributeValueChar;
        private char? firstAttributeChar;
        private bool isEndTag;

        /// <summary>
        /// Initializes a new instance of the <see cref="LenientXmlDocument"/> class.
        /// </summary>
        public LenientXmlDocument()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LenientXmlDocument"/> class.
        /// </summary>
        /// <param name="nt">The nt.</param>
        public LenientXmlDocument(XmlNameTable nt)
            : base(nt)
        {
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
        /// This method is not thread-safe, i.e. LoadXml should not be called
        /// concurrently on the same LenientXmlDocument instance.
        /// </summary>
        /// <param name="xml">String containing the XML document to load.</param>
        /// <exception cref="T:System.Xml.XmlException">There is a load or parse error in the XML. In this case, the document remains empty. </exception>
        public override void LoadXml(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            int l = xml.Length;
            char c;

            Complete((LenientXmlCompletionType)(-1));
            cur = this;

            for (int i = 0; i < l; i++)
            {
                c = xml[i];
                switch (c)
                {
                    case '<':
                        switch (state)
                        {
                            case LenientXmlState.Start:
                                Transition(LenientXmlState.BeginElement);
                                continue;
                        }
                        goto default;
                    case '>':
                        switch (state)
                        {
                            case LenientXmlState.WithinElement:
                            case LenientXmlState.WithinAttributeName:
                            case LenientXmlState.WithinAttributeValue:
                                Transition(LenientXmlState.Start);
                                Complete(LenientXmlCompletionType.ElementName);
                                continue;
                            case LenientXmlState.EndElement:
                                // This means the element was already
                                // completed due to a /
                                Transition(LenientXmlState.Start);
                                continue;
                        }
                        goto default;
                    case '!':
                        switch (state)
                        {
                            case LenientXmlState.BeginElement:
                            case LenientXmlState.Start:
                                Complete(LenientXmlCompletionType.ElementName);
                                Transition(LenientXmlState.BeginExclamation);
                                continue;
                        }
                        goto default;
                    case '/':
                        switch (state)
                        {
                            case LenientXmlState.WithinElement:

                                // We consider a slash to end an element,
                                // even before we see the greater than
                                Transition(LenientXmlState.EndElement);
                                Complete(LenientXmlCompletionType.ElementName);
                                continue;
                            case LenientXmlState.BeginElement:
                                isEndTag = true;
                                continue;
                        }
                        goto default;
                    case '=':
                        switch (state)
                        {
                            case LenientXmlState.WithinAttributeName:
                                Complete(LenientXmlCompletionType.Value);
                                SetStateWithinAttributeValue();
                                continue;
                        }
                        goto default;
                    case '[':
                        switch (state)
                        {
                            case LenientXmlState.BeginElement:
                                continue;
                        }
                        goto default;
                    case '\"':
                    case '\'':
                        switch (state)
                        {
                            case LenientXmlState.WithinAttributeValue:
                                if (isFirstAttributeValueChar)
                                {
                                    firstAttributeChar = c;
                                    isFirstAttributeValueChar = false;
                                }
                                else if (firstAttributeChar != null && firstAttributeChar.Value == c)
                                {
                                    // end of the attribute value
                                    Complete(LenientXmlCompletionType.AttributeName);
                                    Transition(LenientXmlState.WithinElement);
                                }
                                continue;
                        }
                        goto default;
                    case '-':
                        switch (state)
                        {
                            case LenientXmlState.BeginExclamation:
                                Transition(LenientXmlState.StartCommentDash1);
                                continue;
                        }
                        goto default;
                    default:
                        if (char.IsWhiteSpace(c))
                        {
                            switch (state)
                            {
                                case LenientXmlState.BeginElement:
                                case LenientXmlState.BeginExclamation:
                                case LenientXmlState.Start:
                                case LenientXmlState.EndElement:
                                case LenientXmlState.WithinAttributeName:
                                    // do nothing
                                    continue;
                                case LenientXmlState.WithinElement:
                                    // Potentially starting an attribute
                                    if (!elementNameComplete)
                                    {
                                        elementNameComplete = true;
                                    }
                                    continue;
                                case LenientXmlState.WithinAttributeValue:
                                    sbValue.Append(c);
                                    continue;
                                case LenientXmlState.ElementText:
                                    sbText.Append(c);
                                    continue;
                            }
                        }
                        else
                        {
                            // "other" character
                            switch (state)
                            {
                                case LenientXmlState.BeginElement:
                                    Transition(LenientXmlState.WithinElement);
                                    sbElementName.Append(c);
                                    continue;
                                case LenientXmlState.WithinElement:
                                    if (elementNameComplete)
                                    {
                                        Transition(LenientXmlState.WithinAttributeName);
                                        sbAttributeName.Append(c);
                                    }
                                    else
                                    {
                                        sbElementName.Append(c);
                                    }
                                    continue;
                                case LenientXmlState.WithinAttributeName:
                                    sbAttributeName.Append(c);
                                    continue;
                                case LenientXmlState.WithinAttributeValue:
                                    isFirstAttributeValueChar = false;
                                    sbValue.Append(c);
                                    continue;
                                case LenientXmlState.ElementText:
                                    sbValue.Append(c);
                                    continue;
                                case LenientXmlState.Start:
                                    sbText.Append(c);
                                    continue;
                            }
                        }
                        continue;
                }
            }

            Complete((LenientXmlCompletionType)(int)-1);

            if (DocumentElement == null)
            {
                AppendChild(CreateElement(DefaultRootElementName));
            }
        }

        private void Transition(LenientXmlState lenientXmlState)
        {
            Transition(lenientXmlState, true);
        }

        private void Transition(LenientXmlState lenientXmlState, bool act)
        {
            state = lenientXmlState;
            if (act)
            {
                switch (lenientXmlState)
                {
                    case LenientXmlState.BeginElement:
                        Complete(LenientXmlCompletionType.ElementName | LenientXmlCompletionType.Text);
                        break;
                }
            }
        }

        private void SetStateWithinAttributeValue()
        {
            Transition(LenientXmlState.WithinAttributeValue);
            isFirstAttributeValueChar = true;
        }

        private void Complete(LenientXmlCompletionType resetType)
        {
            if (General.IsFlagOn((int)resetType, (int)LenientXmlCompletionType.ElementName))
            {
                if (isEndTag)
                {
                    isEndTag = false;

                    if (sbElementName.Length > 0)
                    {
                        string name = sbElementName.ToString();
                        XmlNode check = cur;
                        while (check != this)
                        {
                            if (check.LocalName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                            {
                                cur = check;
                                break;
                            }

                            check = check.ParentNode;
                        }
                    }
                    ClearStringBuilder(sbElementName);
                    ClearStringBuilder(sbAttributeName);
                    ClearStringBuilder(sbValue);
                }
                else
                {
                    CompleteElement();
                    elementNameComplete = false;

                    CompleteAttribute();
                }
            }
            if (General.IsFlagOn((int)resetType, (int)LenientXmlCompletionType.AttributeName))
            {
                CompleteElement();
                CompleteAttribute();
            }
            if (General.IsFlagOn((int)resetType, (int)LenientXmlCompletionType.Value))
            {
                ClearStringBuilder(sbValue);
            }
            if (General.IsFlagOn((int)resetType, (int)LenientXmlCompletionType.State))
            {
                Transition(LenientXmlState.Start);
            }
            if (General.IsFlagOn((int)resetType, (int)LenientXmlCompletionType.Text))
            {
                if (sbText.Length > 0)
                {
                    XmlText text = CreateTextNode(sbText.ToString());
                    if (cur != null)
                    {
                        cur.AppendChild(text);
                    }
                }
                ClearStringBuilder(sbText);
            }
        }

        private void CompleteElement()
        {
            if (sbElementName.Length > 0)
            {
                string name = sbElementName.ToString();
                XmlElement el = CreateElement(name);
                if (name.ToLower().Equals("option") && cur.LocalName.ToLower() == "option")
                {
                    cur = cur.ParentNode;
                }
                cur.AppendChild(el);
                cur = el;
                ClearStringBuilder(sbElementName);
            }
        }

        private void CompleteAttribute()
        {
            if (sbAttributeName.Length > 0)
            {
                XmlAttribute at = CreateAttribute(sbAttributeName.ToString());
                at.Value = sbValue.ToString();
                if (cur != null && cur != this)
                {
                    cur.Attributes.Append(at);
                }
                ClearStringBuilder(sbAttributeName);
                ClearStringBuilder(sbValue);
            }
            isFirstAttributeValueChar = false;
            firstAttributeChar = null;
        }

        private void ClearStringBuilder(StringBuilder sb)
        {
            sb.Length = 0;
        }

        private enum LenientXmlState
        {
            Start,
            BeginElement,
            BeginExclamation,
            WithinElement,
            WithinAttributeName,
            WithinAttributeValue,
            EndElement,
            ElementText,
            StartCommentDash1,
            StartCommentDash2
        }

        [Flags]
        private enum LenientXmlCompletionType
        {
            ElementName = 1,
            AttributeName = 2,
            Value = 4,
            State = 8,
            Text = 16
        }
    }
}
