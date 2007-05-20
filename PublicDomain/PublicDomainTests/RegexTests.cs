using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class RegexTests
    {
        /// <summary>
        /// Simples this instance.
        /// </summary>
        [Test]
        public void Simple()
        {
            Assert.AreEqual(RegexUtilities.Replace("abcdabcd", @"(bc)", 1, delegate(int captureIndex, string captureValue)
            {
                return captureValue + " ";
            }), "abc dabc d");
            Assert.AreEqual(RegexUtilities.Replace("abcdabcd", @"(bc)", 1, delegate(int captureIndex, string captureValue)
            {
                return captureValue;
            }), "abcdabcd");
        }
    }
}
