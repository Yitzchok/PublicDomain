using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PublicDomain.Web;

namespace PublicDomain
{
    [TestFixture]
    public class WebTests
    {
        [Test]
        public void TestCheckIp()
        {
            Console.WriteLine(DnsUtilities.GetPublicIpAddress());
        }
    }
}
