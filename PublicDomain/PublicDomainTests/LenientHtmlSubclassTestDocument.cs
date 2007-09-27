using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.Xml;
using System.Xml;

namespace PublicDomain
{
    public class LenientHtmlSubclassTestDocument : LenientHtmlDocument
    {
        public LenientHtmlSubclassTestDocument()
            : base(new NameTable())
        {
            m_createDefaultDocumentElement = true;
        }

        protected override System.Xml.XmlNode GetDefaultDocumentNode()
        {
            XmlElement root = CreateElement("tst", "resource", "http://www.testuri.org/");
            root.SetAttribute("xmlns:tst", "http://www.testuri.org/");
            return root;
        }

        protected override bool TryChangeNamespace(string token, out string ns, out string prefix)
        {
            ns = prefix = null;
            if (token == "img")
            {
                ns = "http://www.testuri.org/";
                prefix = "tst";
                return true;
            }
            return base.TryChangeNamespace(token, out ns, out prefix);
        }
    }
}
