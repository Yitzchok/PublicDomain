using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class SpellCheckerTests
    {
        /// <summary>
        /// Tests the valid check.
        /// </summary>
        [Test]
        public void TestValidCheck()
        {
            SetupValidDirectories();

            Assert.IsFalse(SpellChecker.IsWordSpelledCorrectly("perfuntory"));
            Assert.IsTrue(SpellChecker.IsWordSpelledCorrectly("perfunctory"));
        }

        /// <summary>
        /// Tests the valid suggest.
        /// </summary>
        [Test]
        public void TestValidSuggest()
        {
            SetupValidDirectories();

            List<string> suggestions = SpellChecker.SuggestWords("perfuntory");

            Assert.AreNotSame(suggestions.IndexOf("perfunctory"), -1);

            foreach (string suggestion in suggestions)
            {
                Console.WriteLine(suggestion);
            }
        }

        /// <summary>
        /// Tests the invalid check.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SpellCheckerException))]
        public void TestInvalidCheck()
        {
            SetupInvalidDirectories();

            SpellChecker.IsWordSpelledCorrectly("test");
        }

        /// <summary>
        /// Tests the invalid suggest.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SpellCheckerException))]
        public void TestInvalidSuggest()
        {
            SetupInvalidDirectories();

            SpellChecker.SuggestWords("test");
        }

        private static void SetupInvalidDirectories()
        {
            SpellChecker.DataDirectory = Path.GetTempPath();
            SpellChecker.DictionaryDirectory = Path.GetTempPath();
        }

        private static void SetupValidDirectories()
        {
            SpellChecker.DataDirectory = SpellChecker.DefaultDataDirectory;
            SpellChecker.DictionaryDirectory = SpellChecker.DefaultDictionaryDirectory;
        }
    }
}
