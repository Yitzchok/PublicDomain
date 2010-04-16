using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PublicDomain.Xml;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class XmlTests
    {
        /// <summary>
        /// Plays this instance.
        /// </summary>
        [Test]
        public void play()
        {
            Console.WriteLine(XmlUtilities.FormatXml(@"<html><head><title>Hello World</title></head><body><h1>Hello World</h1></body></html>"));
            Console.WriteLine(XmlUtilities.CDataStart + XmlUtilities.CDataStart);
        }
    }
}
