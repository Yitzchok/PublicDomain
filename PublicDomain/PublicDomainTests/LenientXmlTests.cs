using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PublicDomain.LenientXml;
using System.Xml;

namespace PublicDomain
{
    [TestFixture]
    public class LenientXmlTests
    {
        [Test]
        public void SimpleTests()
        {
            Dictionary<string, string> cmp = new Dictionary<string, string>();
            cmp[@""] = LenientXmlDocument.DefaultEmptyXml;
            cmp[@" "] = LenientXmlDocument.DefaultEmptyXml;
            cmp[@"
"] = LenientXmlDocument.DefaultEmptyXml;
            cmp[@"<"] = LenientXmlDocument.DefaultEmptyXml;
            //cmp[@">"] = LenientXmlDocument.DefaultEmptyXml;

            LenientXmlDocument doc = new LenientXmlDocument();
            foreach (string x in cmp.Keys)
            {
                Console.WriteLine(GlobalConstants.DividerEquals);
                Console.WriteLine("Loading:");
                Console.WriteLine(x);
                string y = cmp[x];
                doc.LoadXml(x);
                Console.WriteLine(GlobalConstants.DividerEquals);
                string output = doc.DocumentElement.OuterXml;
                Console.WriteLine(output);
                Assert.AreEqual(y, output);
            }
        }

        [Test]
        public void XmlTest()
        {
            XmlDocument doc = new XmlDocument();
            //doc.LoadXml(@"");
            //Console.WriteLine(doc.DocumentElement.OuterXml);
        }
    }
}
