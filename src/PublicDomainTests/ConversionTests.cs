using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace PublicDomain
{
    [TestFixture]
    public class ConversionTests
    {
        [Test]
        public void TestConversions()
        {
            Version version = ConversionUtilities.ParseVersion("(*&$#");
            Console.WriteLine(version);
        }
    }
}
