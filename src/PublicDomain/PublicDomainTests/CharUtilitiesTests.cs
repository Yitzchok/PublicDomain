using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace PublicDomain
{
    [TestFixture]
    public class CharUtilitiesTests
    {
        /// <summary>
        /// Prints the ASCII table.
        /// </summary>
        [Test]
        public void PrintAsciiTable()
        {
            for (int i = 0; i < CharUtilities.AsciiCharacters.Length; i++)
            {
                Console.WriteLine("{0}: {1}", i, CharUtilities.AsciiCharacters[i]);
            }
        }
    }
}
