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

        [Test]
        public void TestSplitQuoteSensitive()
        {
            TestSplitQuoteSensitive(null, "", 0);
            TestSplitQuoteSensitive("", "", 0);
            TestSplitQuoteSensitive(" test     ", "test", 1);
            TestSplitQuoteSensitive(" 1     test2   test3 test4", "1_test2_test3_test4", 4);
            TestSplitQuoteSensitive(" 1     \"test2\"   test3 test4", "1_test2_test3_test4", 4);
            TestSplitQuoteSensitive(" 1     \"te  st2\"   test3 test4", "1_te  st2_test3_test4", 4);
            TestSplitQuoteSensitive(" 1     \"te  st2\"   t\"est3\" test4", "1_te  st2_t\"est3\"_test4", 4);
            TestSplitQuoteSensitive(" 1     \"te  st2\"   t\"est3\" \"test4  5", "1_te  st2_t\"est3\"_test4  5", 4);
            TestSplitQuoteSensitive(" 1     \"te  st2\"   t\"est3\" \"test 4	\"", "1_te  st2_t\"est3\"_test 4	", 4);
            TestSplitQuoteSensitive(" 1     \"te  st2\"   t\"est3\" \"test 4	\" # abc", "1_te  st2_t\"est3\"_test 4	_ abc", 5, '\"', '#');
            TestSplitQuoteSensitive(" 1     \"te  #st2\"   t\"est3\" \"test 4	\" \"# abc", "1_te  #st2_t\"est3\"_test 4	_# abc", 5, '\"', '#');

            TestSplitQuoteSensitive("Link	America/Los_Angeles	US/Pacific-New	##", "Link_America/Los_Angeles_US/Pacific-New_", 4, '\"', '#');
            TestSplitQuoteSensitive("Rule	US	1945	only	-	Aug	14	23:00u	1:00	P # Peace", "Rule_US_1945_only_-_Aug_14_23:00u_1:00_P_ Peace", 11, '\"', '#');
            TestSplitQuoteSensitive("Rule	sol87	1987	only	-	Dec	30	12:02:15s -0:02:15 -", "Rule_sol87_1987_only_-_Dec_30_12:02:15s_-0:02:15_-", 10, '\"', '#');
        }

        private static void TestSplitQuoteSensitive(string line, string expectedOutput, int expectedLength, params char[] dividerChars)
        {
            string[] pieces = StringUtilities.SplitQuoteSensitive(line, dividerChars);
            string result = string.Join("_", pieces);
            Console.WriteLine(result);
            if (expectedLength != -1)
            {
                Assert.AreEqual(expectedLength, pieces.Length);
            }
            if (expectedOutput != null)
            {
                Assert.AreEqual(expectedOutput, result);
            }
        }
    }
}
