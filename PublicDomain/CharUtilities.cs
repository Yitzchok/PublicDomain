using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// Methods to work with characters, such as an indexable ASCII table.
    /// </summary>
    public class CharUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly char[] AsciiCharacters;

        /// <summary>
        /// Initializes the <see cref="CharUtilities"/> class.
        /// </summary>
        static CharUtilities()
        {
            AsciiCharacters = GetAsciiCharacters().ToArray();
        }

        /// <summary>
        /// Gets the ASCII characters.
        /// </summary>
        /// <returns></returns>
        public static List<char> GetAsciiCharacters()
        {
            List<char> result = new List<char>(256);
            for (int i = 0; i < 256; i++)
            {
                result.Add((char)i);
            }
            return result;
        }
    }
}
