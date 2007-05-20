using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace PublicDomain
{
    /// <summary>
    /// Tests for <see cref="PublicDomain.StringUtilities"/>
    /// </summary>
    [TestFixture]
    public class Tests
    {
        /// <summary>
        /// Tests the replace first.
        /// </summary>
        [Test]
        public void TestReplaceFirst()
        {
            // Test single character finds
            Assert.AreEqual(StringUtilities.ReplaceFirst("aa", "a", "b"), "ba");
            Assert.AreEqual(StringUtilities.ReplaceFirst("bba", "a", "b"), "bbb");
            Assert.AreEqual(StringUtilities.ReplaceFirst("baa", "a", "b"), "bba");

            // Test multi-character finds
            Assert.AreEqual(StringUtilities.ReplaceFirst("aa", "aa", "bb"), "bb");
            Assert.AreEqual(StringUtilities.ReplaceFirst("baa", "aa", "bb"), "bbb");
            Assert.AreEqual(StringUtilities.ReplaceFirst("baab", "aa", "bb"), "bbbb");

            // Test empty replaces
            Assert.AreEqual(StringUtilities.ReplaceFirst("aa", "aa", ""), "");
            Assert.AreEqual(StringUtilities.ReplaceFirst("baa", "aa", ""), "b");
            Assert.AreEqual(StringUtilities.ReplaceFirst("baab", "aa", ""), "bb");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestFormat()
        {
            Console.WriteLine(StringUtilities.FormatNumberWithBytes(500, true));
            Console.WriteLine(StringUtilities.FormatNumberWithBytes(5000, true));
            Console.WriteLine(StringUtilities.FormatNumberWithBytes(50000, true));
            Console.WriteLine(StringUtilities.FormatNumberWithBytes(500000, true));
            Console.WriteLine(StringUtilities.FormatNumberWithBytes(5000000, true));
            Console.WriteLine(StringUtilities.FormatNumberWithBytes(50000000, true));
        }
    }
}
