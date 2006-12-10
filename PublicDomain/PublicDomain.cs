// PublicDomain
//  PublicDomain, Version=0.0.1.0, Culture=neutral, PublicKeyToken=fd3f43b5776a962b
// ======================================
//  Original Author: Kevin Grigorenko (kevgrig@gmail.com)
//  Contributing Authors:
//   * William M. Leszczuk (billl@eden.rutgers.edu)
// 
//  - "Be free Jedi, be free!"
// ======================================
// The purpose of the PublicDomain package is to solve two problems or annoyances
// of .NET development:
// 
// 1. .NET projects and utilities are scattered, difficult to
// deploy and integrate, difficult to find, difficult to contribute to, and
// 2. Licenses are confusing and/or restrictive
// 
// This package solves these two problems as follows (in reverse order):
// 
// 2. This code is in the Public Domain (http://www.copyright.gov/help/faq/faq-definitions.html),
// meaning that the code has no legal authority, will ask nothing for its use, and
// has absolutely no restrictions! That is true open source. It may be included
// in commercial applications, redistributed, altered, or even eaten without any worries.
// Its use need not be attributed in any way. This package is inherently provided
// 'as-is', without any express or implied warranty. In no event will any authors
// be held liable for any damages arising from the use of this package.
// 
// 1. This package explicitly breaks some fundamental paradigms of software engineering
// to solve problem #1. One major goal is that I should be able to embed a single file
// into my project and harness this package, without adding too much bloat to my application.
// For this, precompiler directives are used to include or exclude code that is
// unnecessary or necessitates DLL dependencies that I cannot take on. Second,
// everything is packaged in a single file to make using this package dead simple,
// especially in a C# context (non-C# projects will need a built version of this file
// and reference the DLL). There are no obfuscated build or install procedures,
// or the complexity of managing 10 referenced open source projects in my solution.
// I simply place this file anywhere I need its useful code.
// 
// Any additions to this file must not introduce non-Public Domain code, or code
// that must be externally attributed in any way (i.e. attributed by consumers of this package).
// If you have taken code from someone else which has a similar license and
// does not require external attribution, make sure with the author that this
// is truly a proper place for the code, that external attribution is not necessary,
// and finally make sure to internally attribute the code with a #region to the author(s).
//
// Version History:
// ======================================
// V0.0.1.2
//  [kevgrig@gmail.com]
//   * Added pdsetup project
// V0.0.1.1
//  [kevgrig@gmail.com]
//   * Added bunch of methods to ConversionUtilities courtesy of
//     William M. Leszczuk (billl@eden.rutgers.edu)
//   * Parsing of tz files works
// V0.0.1.0
//  [kevgrig@gmail.com]
//   * Project creation in CodePlex (http://www.codeplex.com/PublicDomain)
//   * Added various code from my projects
//   * tz database code unfinished
// V0.0.0.1
//  [kevgrig@gmail.com]
//   * Added Win32 class and some ExitWindowsEx calls
// V0.0.0.0
//  [kevgrig@gmail.com]
//   * Wrapper around vjslib for zip file reading
//   * java.io.InputStream <-> System.IO.Stream wrappers
//

#region Directives
// The following section provides
// directives for conditional compilation
// of various sections of the code.
// ======================================

// !!!EDIT DIRECTIVES HERE START!!!

#if !(PD)
#define NOVJSLIB

// Commonly non-referenced projects:
#define NOSYSTEMWEB
#define NONUNIT
#define NOTZPARSER

// Other switches:
//#define NOSCREENSCRAPER
//#define NOCLSCOMPLIANTWARNINGSOFF
//#define NOTZ
//#define NOSTATES
#endif

// !!!EDIT DIRECTIVES HERE END!!!!!

// Dependency directives -- do not modify as they
// are very easy to break
#if NOANYTHING
#define NOIMPORTS
#endif

#if NOSYSTEMWEB
#define NOSCREENSCRAPER
#endif

#if NOTZ
#define NOTZPARSER
#endif

#endregion // Directives

#region Meat
// All of the code

#if !(NOPINVOKE)
using System.Runtime.InteropServices;
#endif
#if !(NOVJSLIB)
using java.io;
using java.util.zip;
#endif
#if !(NOSYSTEMWEB)
using System.Web;
#endif
#if !(NONUNIT)
using NUnit.Framework;
#endif

// Core includes (at the bottom for Visual Studio's sake [place optional includes above])
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading;

#if !(NOCLSCOMPLIANTWARNINGSOFF)
#pragma warning disable 3001
#pragma warning disable 3002
#pragma warning disable 3003
#pragma warning disable 3006
#pragma warning disable 3009
#endif

namespace PublicDomain
{
    /// <summary>
    /// Various useful global constants.
    /// </summary>
    public static class GlobalConstants
    {
        /// <summary>
        /// 
        /// </summary>
        public const int StreamBlockSize = 1024;

        /// <summary>
        /// 
        /// </summary>
        public const int ExecuteSmallProcessTimeout = 60000;

        /// <summary>
        /// 
        /// </summary>
        public const double EarthRadiusStatuteMiles = 3963.1D;

        /// <summary>
        /// 
        /// </summary>
        public const double EarthRadiusNauticalMiles = 3443.9D;

        /// <summary>
        /// 
        /// </summary>
        public const double EarthRadiusKilometers = 6376D;

        /// <summary>
        /// 
        /// </summary>
        public const double EarthDiameterStatuteMiles = EarthRadiusStatuteMiles * 2;

        /// <summary>
        /// 
        /// </summary>
        public const double EarthDiameterNauticalMiles = EarthRadiusNauticalMiles * 2;

        /// <summary>
        /// 
        /// </summary>
        public const double EarthDiameterKilometers = EarthRadiusKilometers * 2;
    }

    /// <summary>
    /// Generic class that encapsulates a pair of objects of any types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    [Serializable]
    public class Pair<T, U>
    {
        /// <summary>
        /// 
        /// </summary>
        public T First;

        /// <summary>
        /// 
        /// </summary>
        public U Second;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pair&lt;T, U&gt;"/> class.
        /// </summary>
        public Pair()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pair&lt;T, U&gt;"/> class.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        public Pair(T first, U second)
        {
            First = first;
            Second = second;
        }

        /// <summary>
        /// Finds the pair by key.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="find">The find.</param>
        /// <returns></returns>
        public static Pair<T, U> FindPairByKey(Pair<T, U>[] search, T find)
        {
            if (search != null)
            {
                foreach (Pair<T, U> item in search)
                {
                    if (item.First.Equals(find))
                    {
                        return item;
                    }
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Generic class that encapsulates three objects of any type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    [Serializable]
    public class Triple<T, U, V>
    {
        /// <summary>
        /// 
        /// </summary>
        public T First;

        /// <summary>
        /// 
        /// </summary>
        public U Second;

        /// <summary>
        /// 
        /// </summary>
        public V Third;

        /// <summary>
        /// Initializes a new instance of the <see cref="Triple&lt;T, U, V&gt;"/> class.
        /// </summary>
        public Triple()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Triple&lt;T, U, V&gt;"/> class.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="third">The third.</param>
        public Triple(T first, U second, V third)
        {
            First = first;
            Second = second;
            Third = third;
        }
    }

    /// <summary>
    /// String manipulation and generation methods, as well as string array manipulation.
    /// </summary>
    public static class StringUtilities
    {
        /// <summary>
        /// Returns a string of length <paramref name="size"/> filled
        /// with random ASCII characters in the range A-Z, a-z. If <paramref name="lowerCase"/>
        /// is <c>true</c>, then the range is only a-z.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="lowerCase">if set to <c>true</c> [lower case].</param>
        /// <returns></returns>
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random(unchecked((int)DateTime.UtcNow.Ticks));
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
            {
                return builder.ToString().ToLower();
            }
            return builder.ToString();
        }

        /// <summary>
        /// Returns a string of length <paramref name="length"/> with
        /// 0's padded to the left, if necessary.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string PadIntegerLeft(int val, int length)
        {
            return PadIntegerLeft(val, length, '0');
        }

        /// <summary>
        /// Pads the integer left.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="length">The length.</param>
        /// <param name="pad">The pad.</param>
        /// <returns></returns>
        public static string PadIntegerLeft(int val, int length, char pad)
        {
            string result = val.ToString();
            while (result.Length < length)
            {
                result = pad + result;
            }
            return result;
        }

        /// <summary>
        /// Replace the first occurrence of <paramref name="find"/> (case sensitive) with
        /// <paramref name="replace"/>.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="find">The find.</param>
        /// <param name="replace">The replace.</param>
        /// <returns></returns>
        public static string ReplaceFirst(string str, string find, string replace)
        {
            return ReplaceFirst(str, find, replace, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Replace the first occurrence of <paramref name="find"/> with
        /// <paramref name="replace"/>.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="find">The find.</param>
        /// <param name="replace">The replace.</param>
        /// <param name="findComparison">The find comparison.</param>
        /// <returns></returns>
        public static string ReplaceFirst(string str, string find, string replace, StringComparison findComparison)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            else if (string.IsNullOrEmpty(find))
            {
                throw new ArgumentNullException("find");
            }
            int firstIndex = str.IndexOf(find, findComparison);
            if (firstIndex != -1)
            {
                if (firstIndex == 0)
                {
                    str = replace + str.Substring(find.Length);
                }
                else if (firstIndex == (str.Length - find.Length))
                {
                    str = str.Substring(0, firstIndex) + replace;
                }
                else
                {
                    str = str.Substring(0, firstIndex) + replace + str.Substring(firstIndex + find.Length);
                }
            }
            return str;
        }

        /// <summary>
        /// Splits the specified pieces.
        /// </summary>
        /// <param name="pieces">The pieces.</param>
        /// <param name="splitChar">The split char.</param>
        /// <param name="indices">The indices.</param>
        /// <returns></returns>
        public static string[] Split(string[] pieces, char splitChar, params int[] indices)
        {
            if (pieces == null)
            {
                throw new ArgumentNullException("pieces");
            }
            else if (indices == null)
            {
                throw new ArgumentNullException("indices");
            }

            // First, we need to sort the indices
            Array.Sort(indices);

            int offset = 0;
            foreach (int index in indices)
            {
                if (index + offset < pieces.Length)
                {
                    string[] subPieces = pieces[index + offset].Split(splitChar);
                    if (subPieces.Length > 1)
                    {
                        pieces = ArrayUtilities.InsertReplace<string>(pieces, index + offset, subPieces);
                        offset += subPieces.Length - 1;
                    }
                }
            }

            return pieces;
        }

        /// <summary>
        /// Ensures that within <paramref name="str"/> there are no two
        /// consecutive whitespace characters.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string RemoveConsecutiveWhitespace(string str)
        {
            return ReplaceConsecutiveWhitespace(str, " ");
        }

        /// <summary>
        /// Ensures that within <paramref name="str"/> there are no two
        /// consecutive whitespace characters.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static string ReplaceConsecutiveWhitespace(string str, string replacement)
        {
            return Regex.Replace(str, @"\s+", replacement, RegexOptions.Compiled);
        }

        /// <summary>
        /// Removes the empty pieces.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static string[] RemoveEmptyPieces(string[] array)
        {
            int index = IndexOfEmptyPiece(array);
            while (index != -1)
            {
                ArrayUtilities.Remove<string>(ref array, index);
                index = IndexOfEmptyPiece(array, index);
            }
            return array;
        }

        /// <summary>
        /// Indexes the of empty piece.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static int IndexOfEmptyPiece(string[] array)
        {
            return IndexOfEmptyPiece(array, 0);
        }

        /// <summary>
        /// Indexes the of empty piece.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static int IndexOfEmptyPiece(string[] array, int startIndex)
        {
            for (int i = startIndex; i < array.Length; i++)
            {
                if (string.IsNullOrEmpty(array[i]))
                {
                    return i;
                }
            }
            return -1;
        }

#if !(NONUNIT)
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
                Assert.AreEqual(ReplaceFirst("aa", "a", "b"), "ba");
                Assert.AreEqual(ReplaceFirst("bba", "a", "b"), "bbb");
                Assert.AreEqual(ReplaceFirst("baa", "a", "b"), "bba");

                // Test multi-character finds
                Assert.AreEqual(ReplaceFirst("aa", "aa", "bb"), "bb");
                Assert.AreEqual(ReplaceFirst("baa", "aa", "bb"), "bbb");
                Assert.AreEqual(ReplaceFirst("baab", "aa", "bb"), "bbbb");

                // Test empty replaces
                Assert.AreEqual(ReplaceFirst("aa", "aa", ""), "");
                Assert.AreEqual(ReplaceFirst("baa", "aa", ""), "b");
                Assert.AreEqual(ReplaceFirst("baab", "aa", ""), "bb");
            }
        }
#endif
    }

    /// <summary>
    /// Methods to work with characters, such as an indexable ASCII table.
    /// </summary>
#if !(NONUNIT)
    [TestFixture]
#endif
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
        /// Prints the ASCII table.
        /// </summary>
#if !(NONUNIT)
        [Test]
#endif
        public void PrintAsciiTable()
        {
            for (int i = 0; i < AsciiCharacters.Length; i++)
            {
                Console.WriteLine("{0}: {1}", i, AsciiCharacters[i]);
            }
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

    /// <summary>
    /// Common conversion tasks such as parsing string values into various types.
    /// </summary>
    public static class ConversionUtilities
    {
        /// <summary>
        /// Determines whether [is string an integer] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an integer] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnInteger(string str)
        {
            return IsStringAnInteger64(str);
        }

        /// <summary>
        /// Determines whether [is string an integer16] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an integer16] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnInteger16(string str)
        {
            Int16 trash;
            return Int16.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string an integer32] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an integer32] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnInteger32(string str)
        {
            Int32 trash;
            return Int32.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string an integer64] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an integer64] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnInteger64(string str)
        {
            Int64 trash;
            return Int64.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string an unsigned integer any] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an unsigned integer any] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnUnsignedIntegerAny(string str)
        {
            return IsStringAnUnsignedInteger64(str);
        }

        /// <summary>
        /// Determines whether [is string an unsigned integer16] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an unsigned integer16] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnUnsignedInteger16(string str)
        {
            UInt16 trash;
            return UInt16.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string an unsigned integer32] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an unsigned integer32] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnUnsignedInteger32(string str)
        {
            UInt32 trash;
            return UInt32.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string an unsigned integer64] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an unsigned integer64] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnUnsignedInteger64(string str)
        {
            UInt64 trash;
            return UInt64.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A double] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A double] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringADouble(string str)
        {
            double trash;
            return double.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A decimal] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A decimal] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringADecimal(string str)
        {
            decimal trash;
            return decimal.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A float] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A float] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAFloat(string str)
        {
            float trash;
            return float.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A char] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A char] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAChar(string str)
        {
            char trash;
            return char.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A boolean] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A boolean] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringABoolean(string str)
        {
            bool trash;
            return bool.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A byte] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A byte] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAByte(string str)
        {
            byte trash;
            return byte.TryParse(str, out trash);
        }

        /// <summary>
        /// Parses the int.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static int ParseInt(string str)
        {
            int result;
            int.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// Parses the short.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static short ParseShort(string str)
        {
            short result;
            short.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// Parses the long.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static long ParseLong(string str)
        {
            long result;
            long.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// Parses the float.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static float ParseFloat(string str)
        {
            float result;
            float.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// Parses the double.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static double ParseDouble(string str)
        {
            double result;
            double.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// Parses the decimal.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static decimal ParseDecimal(string str)
        {
            decimal result;
            decimal.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// Parses the U int.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static uint ParseUInt(string str)
        {
            uint result;
            uint.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// Parses the U short.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static ushort ParseUShort(string str)
        {
            ushort result;
            ushort.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// Parses the U long.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static ulong ParseULong(string str)
        {
            ulong result;
            ulong.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// Parses the byte.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static byte ParseByte(string str)
        {
            byte result;
            byte.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// Parses the char.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static char ParseChar(string str)
        {
            char result;
            char.TryParse(str, out result);
            return result;
        }
    }

    /// <summary>
    /// Methods to manipulate arrays.
    /// </summary>
    public static class ArrayUtilities
    {
        /// <summary>
        /// Inserts the replace.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <param name="insert">The insert.</param>
        /// <returns></returns>
        public static T[] InsertReplace<T>(T[] array, int index, T[] insert)
        {
            int newLength = array.Length + insert.Length - 1;
            Array.Resize<T>(ref array, newLength);
            // Shift all the elements from the index up to the end
            int offset = (insert.Length - 1);
            for (int i = array.Length - 1; i >= index; i--)
            {
                if (i - offset < 0)
                {
                    break;
                }
                array[i] = array[i - offset];
            }

            // Now put all the insert elements starting at index
            for (int j = 0; j < insert.Length; j++)
            {
                array[index++] = insert[j];
            }
            return array;
        }

        /// <summary>
        /// Removes the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static T[] Remove<T>(ref T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (index < 0 || index >= array.Length)
            {
                throw new IndexOutOfRangeException("Cannot remove element at index " + index);
            }

            // Crush the elemnts from the end down to the index
            for (int i = index; i < array.Length - 1; i++)
            {
                array[i] = array[i + 1];
            }

            Array.Resize<T>(ref array, array.Length - 1);
            return array;
        }
    }

    /// <summary>
    /// Methods to work with Exceptions.
    /// </summary>
    public static class ExceptionUtilities
    {
        /// <summary>
        /// Writes the exceptions.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void WriteExceptions(Exception ex)
        {
            WriteExceptions(ex, Console.Error);
        }

        /// <summary>
        /// Writes the exceptions.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="writer">The writer.</param>
        public static void WriteExceptions(Exception ex, TextWriter writer)
        {
            while (ex != null)
            {
                writer.WriteLine(ex.Message);
                writer.WriteLine(ex.StackTrace);

                ex = ex.InnerException;

                if (ex != null)
                {
                    writer.WriteLine();
                }
            }
        }

        /// <summary>
        /// Gets the exception details as string.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static string GetExceptionDetailsAsString(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            while (ex != null)
            {
                sb.AppendFormat(@"Exception ({0}), Type={1}, Message={3}, Stack Trace={2}
", i, ex.GetType().Name, ex.StackTrace, ex.Message);
                ex = ex.InnerException;
                i++;
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Methods to help in file system related manipulations.
    /// </summary>
    public static class FileSystemUtilities
    {
        private static char[] trackbackChars = new char[] { '\\', '/' };

        /// <summary>
        /// Ensures the directory ending.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public static void EnsureDirectoryEnding(ref string directory)
        {
            if (directory != null && directory[directory.Length - 1] != '\\')
            {
                directory += '\\';
            }
        }

        /// <summary>
        /// Ensures the directory ending.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns></returns>
        public static string EnsureDirectoryEnding(string directory)
        {
            EnsureDirectoryEnding(ref directory);
            return directory;
        }

        /// <summary>
        /// Pathes the combine.
        /// </summary>
        /// <param name="path1">The path1.</param>
        /// <param name="path2">The path2.</param>
        /// <returns></returns>
        public static string PathCombine(string path1, string path2)
        {
            if (string.IsNullOrEmpty(path2))
            {
                return path1;
            }

            if (path2[0] == '\\' || path2[0] == '/')
            {
                path2 = path2.Substring(1);
            }

            return Path.Combine(path1, path2);
        }

        /// <summary>
        /// Gets the temporary directory.
        /// </summary>
        /// <returns></returns>
        public static string GetTemporaryDirectory()
        {
            string ret = PathCombine(Path.GetTempPath(), "t" + new Random((int)unchecked(DateTime.Now.Ticks)).Next(1, 10000).ToString() + @"\");
            Directory.CreateDirectory(ret);
            return ret;
        }

        /// <summary>
        /// Gets the name of the temp file.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        public static string GetTempFileName(string extension)
        {
            return GetTempFileName(extension, null);
        }

        /// <summary>
        /// Gets the name of the temp file.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string GetTempFileName(string extension, string fileName)
        {
            string tempFile = Path.GetTempFileName();

            // Find the last slash
            int lastSlash = tempFile.LastIndexOf('\\');

            string filePart = tempFile.Substring(lastSlash + 1);

            tempFile = tempFile.Substring(0, lastSlash + 1);

            // Split the file name into extension and name
            int periodIndex = filePart.LastIndexOf('.');

            string preName = null;

            if (periodIndex != -1)
            {
                preName = filePart.Substring(0, periodIndex);
            }

            if (fileName == null)
            {
                if (preName == null)
                {
                    tempFile += "t" + new Random((int)unchecked(DateTime.Now.Ticks)).Next(1, 10000).ToString();
                }
                else
                {
                    tempFile += preName;
                }
            }
            else
            {
                tempFile += fileName;
            }

            tempFile += "." + extension;

            return tempFile;
        }

        /// <summary>
        /// Deletes the directory forcefully.
        /// </summary>
        /// <param name="dir">The dir.</param>
        public static void DeleteDirectoryForcefully(string dir)
        {
            RemoveReadOnly(dir);

            Directory.Delete(dir, true);
        }

        /// <summary>
        /// Removes the read only.
        /// </summary>
        /// <param name="path">The path.</param>
        private static void RemoveReadOnly(string path)
        {
            DirectoryInfo current = new DirectoryInfo(path);
            current.Attributes = FileAttributes.Normal;

            foreach (FileSystemInfo file in current.GetFileSystemInfos())
            {
                file.Attributes = FileAttributes.Normal;
            }

            foreach (DirectoryInfo folder in current.GetDirectories())
            {
                RemoveReadOnly(folder.FullName);
            }
        }

        /// <summary>
        /// Saves the input stream to file.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="newFile">The new file.</param>
        public static void SaveInputStreamToFile(Stream stream, string newFile)
        {
            // Now, write out the file
            int length = 256;
            byte[] buffer = new byte[length];
            using (FileStream fs = new FileStream(newFile, FileMode.Create))
            {
                int bytesRead = stream.Read(buffer, 0, length);
                // write the required bytes
                while (bytesRead > 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                    bytesRead = stream.Read(buffer, 0, length);
                }
            }
        }

        /// <summary>
        /// Combines the trackbacks in path.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string CombineTrackbacksInPath(string uri)
        {
            int trackbackIndex = GetTrackbackIndex(uri);
            int slashIndex;
            while (trackbackIndex != -1)
            {
                // There is a trackback starting at the index, so from
                // there we find the nearest slash, and remove that piece
                slashIndex = uri.LastIndexOfAny(trackbackChars, trackbackIndex - 2);
                if (slashIndex == -1)
                {
                    break;
                }

                uri = uri.Remove(slashIndex, trackbackIndex - slashIndex + 2);

                trackbackIndex = GetTrackbackIndex(uri);
            }
            return uri;
        }

        /// <summary>
        /// Gets the index of the trackback.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        private static int GetTrackbackIndex(string uri)
        {
            int index = uri.IndexOf("../");

            if (index == -1)
            {
                index = uri.IndexOf(@"..\");
            }

            return index;
        }
    }

    /// <summary>
    /// Methods to work with generic objects, such as serializing and deserializing
    /// them to byte arrays or memory streams.
    /// </summary>
    public static class ObjectUtilities
    {
        /// <summary>
        /// Serializes the object to binary stream.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static MemoryStream SerializeObjectToBinaryStream(object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(ms, o);
            return ms;
        }

        /// <summary>
        /// Serializes the object to binary.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static byte[] SerializeObjectToBinary(object o)
        {
            return SerializeObjectToBinaryStream(o).GetBuffer();
        }

        /// <summary>
        /// Deserializes the object from binary.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static T DeserializeObjectFromBinary<T>(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryFormatter b = new BinaryFormatter();
            return (T)b.Deserialize(ms);
        }
    }

    /// <summary>
    /// Methods to help in common Reflection tasks.
    /// </summary>
    public static class ReflectionUtilities
    {
        /// <summary>
        /// Gets the name of the strong.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static StrongName GetStrongName(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            AssemblyName assemblyName = assembly.GetName();

            // get the public key blob
            byte[] publicKey = assemblyName.GetPublicKey();
            if (publicKey == null || publicKey.Length == 0)
                throw new InvalidOperationException(
                    String.Format("{0} is not strongly named",
                    assembly));

            StrongNamePublicKeyBlob keyBlob =
                new StrongNamePublicKeyBlob(publicKey);

            // create the StrongName
            return new StrongName(
                keyBlob, assemblyName.Name, assemblyName.Version);
        }

        /// <summary>
        /// Finds the type by interface.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static Type FindTypeByInterface<T>(Assembly assembly) where T : class
        {
            Type[] types = assembly.GetTypes();
            Type interfaceType;
            foreach (Type type in types)
            {
                interfaceType = type.GetInterface(typeof(T).ToString(), false);
                if (interfaceType != null)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the instance by interface.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static T FindInstanceByInterface<T>(Assembly assembly) where T : class
        {
            Type type = FindTypeByInterface<T>(assembly);
            return assembly.CreateInstance(type.FullName) as T;
        }
    }

#if !(NOVJSLIB)
    /// <summary>
    /// Callback that gets called after a zip file entry is read.
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="zipEntryStream"></param>
    /// <param name="rock">Arbitrary data</param>
    public delegate void CallbackZip(ZipEntryE entry, ZipEntryInputStream zipEntryStream, object rock);

    /// <summary>
    /// Provides methods for manipulating archive files, such as ZIPs.
    /// </summary>
    public static class Archiver
    {
        /// <summary>
        /// Reads the zip stream.
        /// </summary>
        /// <param name="jInputStream">The j input stream.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="rock">The rock.</param>
        public static void ReadZipStream(java.io.InputStream jInputStream, CallbackZip callback, object rock)
        {
            ZipInputStream zis = null;
            try
            {
                zis = new ZipInputStream(jInputStream);

                ZipEntry entry;
                ZipEntryE extendedEntry;
                while ((entry = zis.getNextEntry()) != null)
                {
                    extendedEntry = new ZipEntryE(entry);
                    callback(extendedEntry, new ZipEntryInputStream(zis), rock);

                    // Close the entry that we read
                    zis.closeEntry();
                }
            }
            finally
            {
                if (zis != null)
                {
                    zis.close();
                }
            }
        }

        /// <summary>
        /// Extracts the specified zip file path.
        /// </summary>
        /// <param name="zipFilePath">The zip file path.</param>
        /// <param name="destinationDirectory">The destination directory.</param>
        public static void Extract(string zipFilePath, string destinationDirectory)
        {
            Extract(zipFilePath, destinationDirectory, false);
        }

        /// <summary>
        /// Extracts the specified zip file path.
        /// </summary>
        /// <param name="zipFilePath">The zip file path.</param>
        /// <param name="destinationDirectory">The destination directory.</param>
        /// <param name="truncatePaths">if set to <c>true</c> [truncate paths].</param>
        public static void Extract(string zipFilePath, string destinationDirectory, bool truncatePaths)
        {
            using (FileStream stream = new FileStream(zipFilePath, FileMode.Open))
            {
                using (JStream jstream = new JStream(stream))
                {
                    Extract(jstream, destinationDirectory, truncatePaths);
                }
            }
        }

        /// <summary>
        /// Extracts the specified j input stream.
        /// </summary>
        /// <param name="jInputStream">The j input stream.</param>
        /// <param name="destinationDirectory">The destination directory.</param>
        /// <param name="truncatePaths">if set to <c>true</c> [truncate paths].</param>
        public static void Extract(java.io.InputStream jInputStream, string destinationDirectory, bool truncatePaths)
        {
            if (string.IsNullOrEmpty(destinationDirectory))
            {
                throw new ArgumentException("destinationDirectory");
            }
            destinationDirectory = FileSystemUtilities.EnsureDirectoryEnding(destinationDirectory);
            ReadZipStream(jInputStream, delegate(ZipEntryE entry, ZipEntryInputStream zipEntryStream, object rock)
            {
                zipEntryStream.WriteTo(destinationDirectory + entry.getName(truncatePaths));
            }, null);
        }
    }

    /// <summary>
    /// Wrapper class around ZipEntry to provide some convenience
    /// methods.
    /// </summary>
    public class ZipEntryE : ZipEntry
    {
        /// <summary>
        /// 
        /// </summary>
        protected ZipEntry m_entry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipEntryE"/> class.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public ZipEntryE(ZipEntry entry)
            : base(entry.getName())
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            m_entry = entry;
        }

        /// <summary>
        /// Gets the comment.
        /// </summary>
        /// <returns></returns>
        public override string getComment()
        {
            return m_entry.getComment();
        }

        /// <summary>
        /// Gets the size of the compressed.
        /// </summary>
        /// <returns></returns>
        public override long getCompressedSize()
        {
            return m_entry.getCompressedSize();
        }

        /// <summary>
        /// Gets the CRC.
        /// </summary>
        /// <returns></returns>
        public override long getCrc()
        {
            return m_entry.getCrc();
        }

        /// <summary>
        /// Gets the extra.
        /// </summary>
        /// <returns></returns>
        public override sbyte[] getExtra()
        {
            return m_entry.getExtra();
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <returns></returns>
        public override int getMethod()
        {
            return m_entry.getMethod();
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return m_entry.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return m_entry.GetHashCode();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns></returns>
        public override string getName()
        {
            return getName(false);
        }

        /// <summary>
        /// Extension method which provides the option
        /// of truncating any path information in the file
        /// </summary>
        /// <param name="truncatePath"></param>
        /// <returns></returns>
        public virtual string getName(bool truncatePath)
        {
            string result = m_entry.getName();
            if (result != null)
            {
                result = result.Replace('/', '\\');
                if (truncatePath)
                {
                    int lastSlashIndex = result.LastIndexOf('\\');
                    if (lastSlashIndex != -1)
                    {
                        result = result.Substring(lastSlashIndex + 1);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <returns></returns>
        public override long getSize()
        {
            return m_entry.getSize();
        }

        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <returns></returns>
        public override long getTime()
        {
            return m_entry.getTime();
        }

        /// <summary>
        /// Determines whether this instance is directory.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is directory; otherwise, <c>false</c>.
        /// </returns>
        public override bool isDirectory()
        {
            return m_entry.isDirectory();
        }

        /// <summary>
        /// Sets the comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        public override void setComment(string comment)
        {
            m_entry.setComment(comment);
        }

        /// <summary>
        /// Sets the CRC.
        /// </summary>
        /// <param name="crc">The CRC.</param>
        public override void setCrc(long crc)
        {
            m_entry.setCrc(crc);
        }

        /// <summary>
        /// Sets the extra.
        /// </summary>
        /// <param name="extra">The extra.</param>
        public override void setExtra(sbyte[] extra)
        {
            m_entry.setExtra(extra);
        }

        /// <summary>
        /// Sets the method.
        /// </summary>
        /// <param name="m">The m.</param>
        public override void setMethod(int m)
        {
            m_entry.setMethod(m);
        }

        /// <summary>
        /// Sets the size.
        /// </summary>
        /// <param name="sz">The sz.</param>
        public override void setSize(long sz)
        {
            m_entry.setSize(sz);
        }

        /// <summary>
        /// Sets the time.
        /// </summary>
        /// <param name="t">The t.</param>
        public override void setTime(long t)
        {
            m_entry.setTime(t);
        }

        /// <summary>
        /// Toes the string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_entry.ToString();
        }
    }

    /// <summary>
    /// Abstraction over a <see cref="java.util.zip.ZipInputStream"/>
    /// </summary>
    public class ZipEntryInputStream : JInputStream
    {
        /// <summary>
        /// 
        /// </summary>
        protected ZipInputStream m_zis;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipEntryInputStream"/> class.
        /// </summary>
        /// <param name="zis">The zis.</param>
        public ZipEntryInputStream(ZipInputStream zis)
            : base(zis)
        {
            if (zis == null)
            {
                throw new ArgumentNullException("zis");
            }
            m_zis = zis;
        }

        /// <summary>
        /// Disposes the specified disposing.
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> [disposing].</param>
        protected override void Dispose(bool disposing)
        {
            // We don't want to close the underlying ZipInputStream,
            // so we just do nothing here.
        }

        /// <summary>
        /// Convenience method which writes the contents of the ZipEntry
        /// to the specified file and returns the number of bytes written.
        /// <c>FileMode.Create</c> is used when opening the file for writing.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public int WriteTo(string file)
        {
            using (FileStream fileStream = new FileStream(file, FileMode.Create))
            {
                return WriteTo(fileStream);
            }
        }

        /// <summary>
        /// Convenience method which writes the contents of the ZipEntry
        /// to the specified stream and returns the number of bytes written.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public int WriteTo(Stream stream)
        {
            byte[] buffer = new byte[GlobalConstants.StreamBlockSize];
            int bytesRead, totalBytesRead = 0;
            while ((bytesRead = Read(buffer, 0, GlobalConstants.StreamBlockSize)) > -1)
            {
                stream.Write(buffer, 0, bytesRead);
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }

    /// <summary>
    /// This class allows one to transparently pass a .NET System.IO.Stream
    /// class where a java.io.InputStream class is expected.
    /// </summary>
    public class JStream : InputStream, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected Stream m_stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="JStream"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public JStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            m_stream = stream;
        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        public override int read()
        {
            return m_stream.ReadByte();
        }

        /// <summary>
        /// Availables this instance.
        /// </summary>
        /// <returns></returns>
        public override int available()
        {
            return unchecked((int)(m_stream.Length - m_stream.Position));
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public override void close()
        {
            m_stream.Close();
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return m_stream.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return m_stream.GetHashCode();
        }

        /// <summary>
        /// Marks the specified readlimit.
        /// </summary>
        /// <param name="readlimit">The readlimit.</param>
        public override void mark(int readlimit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Marks the supported.
        /// </summary>
        /// <returns></returns>
        public override bool markSupported()
        {
            return false;
        }

        /// <summary>
        /// Reads the specified b.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public override int read(sbyte[] b)
        {
            return read(b, 0, b.Length);
        }

        /// <summary>
        /// Reads the specified b.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="off">The off.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public override int read(sbyte[] b, int off, int count)
        {
            if (b == null)
            {
                throw new NullReferenceException();
            }

            // Unfortunately, bytes are different, so we need to read
            // data into memory first
            byte[] data = new byte[count];
            int bytesRead = m_stream.Read(data, 0, count);
            if (bytesRead > 0)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    b[off + i] = (sbyte)data[i];
                }
            }
            return bytesRead;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public override void reset()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Skips the specified count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public override long skip(long count)
        {
            return m_stream.Seek(count, SeekOrigin.Current);
        }

        /// <summary>
        /// Toes the string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_stream.ToString();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // We assume the caller is managing the stream passed in, so we
            // won't actually close it
        }
    }

    /// <summary>
    /// This class allows one to transparently pass a java.io.InputStream class where
    /// a .NET System.IO.Stream class is expected.
    /// </summary>
    public class JInputStream : Stream
    {
        /// <summary>
        /// 
        /// </summary>
        protected InputStream m_jis;

        /// <summary>
        /// Initializes a new instance of the <see cref="JInputStream"/> class.
        /// </summary>
        /// <param name="javaInputStream">The java input stream.</param>
        public JInputStream(java.io.InputStream javaInputStream)
        {
            if (javaInputStream == null)
            {
                throw new ArgumentNullException("javaInputStream");
            }
            m_jis = javaInputStream;
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports reading; otherwise, false.</returns>
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports seeking; otherwise, false.</returns>
        public override bool CanSeek
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports writing; otherwise, false.</returns>
        public override bool CanWrite
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void Flush()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <value></value>
        /// <returns>A long value representing the length of the stream in bytes.</returns>
        /// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Length
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <value></value>
        /// <returns>The current position within the stream.</returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Position
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The sum of offset and count is larger than the buffer length. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
        /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (count < 0)
            {
                throw new ArgumentException("count");
            }
            sbyte[] b = new sbyte[count];
            int bytesRead = m_jis.read(b, 0, count);
            if (bytesRead > 0)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    buffer[offset + i] = (byte)b[i];
                }
            }
            return bytesRead;
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"></see> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
        /// <exception cref="T:System.ArgumentException">The sum of offset and count is greater than the buffer length. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream.
        /// </summary>
        public override void Close()
        {
            m_jis.close();
            base.Close();
        }

        /// <summary>
        /// Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.
        /// </summary>
        /// <returns>
        /// The unsigned byte cast to an Int32, or -1 if at the end of the stream.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override int ReadByte()
        {
            return m_jis.read();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream"></see> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            m_jis.close();
        }
    }
#endif

#if !(NOPINVOKE)
    /// <summary>
    /// Interfaces into Win32 calls.
    /// http://www.codeproject.com/csharp/essentialpinvoke.asp
    /// </summary>
    public static class Win32
    {
        /// <summary>
        /// Win32 constants
        /// </summary>
        public static class Win32Constants
        {
            /// <summary>
            /// Shuts down all processes running in the logon session of the process that called the ExitWindowsEx function. Then it logs the user off.
            /// This flag can be used only by processes running in an interactive user's logon session.
            /// </summary>
            public const uint EWX_LOGOFF = 0;

            /// <summary>
            /// Shuts down the system and turns off the power. The system must support the power-off feature.
            /// The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
            /// </summary>
            public const uint EWX_POWEROFF = 0x00000008;

            /// <summary>
            /// Shuts down the system and then restarts the system.
            /// The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
            /// </summary>
            public const uint EWX_REBOOT = 0x00000002;

            /// <summary>
            /// Shuts down the system and then restarts it, as well as any applications that have been registered for restart using the RegisterApplicationRestart function. These application receive the WM_QUERYENDSESSION message with lParam set to the ENDSESSION_CLOSEAPP value. For more information, see Guidelines for Applications.
            /// </summary>
            public const uint EWX_RESTARTAPPS = 0x00000040;

            /// <summary>
            /// Shuts down the system to a point at which it is safe to turn off the power. All file buffers have been flushed to disk, and all running processes have stopped.
            /// The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
            /// Specifying this flag will not turn off the power even if the system supports the power-off feature. You must specify EWX_POWEROFF to do this.
            /// Windows XP SP1:  If the system supports the power-off feature, specifying this flag turns off the power.
            /// </summary>
            public const uint EWX_SHUTDOWN = 0x00000001;

            /// <summary>
            /// This flag has no effect if terminal services is enabled. Otherwise, the system does not send the WM_QUERYENDSESSION and WM_ENDSESSION messages. This can cause applications to lose data. Therefore, you should only use this flag in an emergency.
            /// </summary>
            public const uint EWX_FORCE = 0x00000004;

            /// <summary>
            /// Forces processes to terminate if they do not respond to the WM_QUERYENDSESSION or WM_ENDSESSION message within the timeout interval. For more information, see the Remarks.
            /// Windows NT and Windows Me/98/95:  This value is not supported.
            /// </summary>
            public const uint EWX_FORCEIFHUNG = 0x00000010;

            /// <summary>
            /// Application issue.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_APPLICATION = 0x00040000;

            /// <summary>
            /// Hardware issue.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_HARDWARE = 0x00010000;

            /// <summary>
            /// The InitiateSystemShutdown function was used instead of InitiateSystemShutdownEx.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_LEGACY_API = 0x00070000;

            /// <summary>
            /// Operating system issue.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_OPERATINGSYSTEM = 0x00020000;

            /// <summary>
            /// Other issue.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_OTHER = 0x00000000;

            /// <summary>
            /// Power failure.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_POWER = 0x00060000;

            /// <summary>
            /// Software issue.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_SOFTWARE = 0x00030000;

            /// <summary>
            /// System failure.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_SYSTEM = 0x00050000;

            /// <summary>
            /// Blue screen crash event.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_BLUESCREEN = 0x0000000F;

            /// <summary>
            /// Unplugged.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_CORDUNPLUGGED = 0x0000000b;

            /// <summary>
            /// Disk.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_DISK = 0x00000007;

            /// <summary>
            /// Environment.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_ENVIRONMENT = 0x0000000c;

            /// <summary>
            /// Driver.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_HARDWARE_DRIVER = 0x0000000d;

            /// <summary>
            /// Hot fix.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_HOTFIX = 0x00000011;

            /// <summary>
            /// Hot fix uninstallation.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_HOTFIX_UNINSTALL = 0x00000017;

            /// <summary>
            /// Unresponsive.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_HUNG = 0x00000005;

            /// <summary>
            /// Installation.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_INSTALLATION = 0x00000002;

            /// <summary>
            /// Maintenance.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_MAINTENANCE = 0x00000001;

            /// <summary>
            /// MMC issue.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_MMC = 0x00000019;

            /// <summary>
            /// Network connectivity.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_NETWORK_CONNECTIVITY = 0x00000014;

            /// <summary>
            /// Network card.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_NETWORKCARD = 0x00000009;

            /// <summary>
            /// Other issue.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_OTHER = 0x00000000;

            /// <summary>
            /// Other driver event.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_OTHERDRIVER = 0x0000000e;

            /// <summary>
            /// Power supply.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_POWER_SUPPLY = 0x0000000a;

            /// <summary>
            /// Processor.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_PROCESSOR = 0x00000008;

            /// <summary>
            /// Reconfigure.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_RECONFIG = 0x00000004;

            /// <summary>
            /// Security issue.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_SECURITY = 0x00000013;

            /// <summary>
            /// Security patch.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_SECURITYFIX = 0x00000012;

            /// <summary>
            /// Security patch uninstallation.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_SECURITYFIX_UNINSTALL = 0x00000018;

            /// <summary>
            /// Service pack.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_SERVICEPACK = 0x00000010;

            /// <summary>
            /// Service pack uninstallation.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_SERVICEPACK_UNINSTALL = 0x00000016;

            /// <summary>
            /// Terminal Services.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_TERMSRV = 0x00000020;

            /// <summary>
            /// Unstable.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_UNSTABLE = 0x00000006;

            /// <summary>
            /// Upgrade.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_UPGRADE = 0x00000003;

            /// <summary>
            /// WMI issue.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_WMI = 0x00000015;

            /// <summary>
            /// The shutdown was planned. The system generates a System State Data (SSD) file. This file contains system state information such as the processes, threads, memory usage, and configuration.
            /// If this flag is not present, the shutdown was unplanned. Notification and reporting options are controlled by a set of policies. For example, after logging in, the system displays a dialog box reporting the unplanned shutdown if the policy has been enabled. An SSD file is created only if the SSD policy is enabled on the system. The administrator can use Windows Error Reporting to send the SSD data to a central location, or to Microsoft.
            /// </summary>
            public const uint SHTDN_REASON_FLAG_PLANNED = 0x80000000;
        }

        /// <summary>
        /// Class that contains PInvoke methods into Win32
        /// </summary>
        public static class ExternalMethods
        {
            /// <summary>
            /// Logs off the interactive user, shuts down the system, or shuts down and restarts the system. It sends the WM_QUERYENDSESSION message to all applications to determine if they can be terminated.
            /// http://msdn2.microsoft.com/en-us/library/aa376868.aspx
            /// </summary>
            /// <param name="uFlags"></param>
            /// <param name="dwReason">The reason for initiating the shutdown. This parameter must be one of the system shutdown reason codes.
            /// If this parameter is zero, the SHTDN_REASON_FLAG_PLANNED reason code will not be set and therefore the default action is an undefined shutdown that is logged as "No title for this reason could be found". By default, it is also an unplanned shutdown. Depending on how the system is configured, an unplanned shutdown triggers the creation of a file that contains the system state information, which can delay shutdown. Therefore, do not use zero for this parameter.</param>
            /// <returns>If the function succeeds, the return value is nonzero. Because the function executes asynchronously, a nonzero return value indicates that the shutdown has been initiated. It does not indicate whether the shutdown will succeed. It is possible that the system, the user, or another application will abort the shutdown.
            /// If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
        }

        /// <summary>
        /// Enumeration that directs a windows control action.
        /// </summary>
        public enum WindowsControl : uint
        {
            /// <summary>
            /// 
            /// </summary>
            Logoff = Win32Constants.EWX_LOGOFF,

            /// <summary>
            /// 
            /// </summary>
            ShutdownAndPowerOff = Win32Constants.EWX_POWEROFF,

            /// <summary>
            /// 
            /// </summary>
            ShutdownNoPowerOff = Win32Constants.EWX_SHUTDOWN,

            /// <summary>
            /// 
            /// </summary>
            Restart = Win32Constants.EWX_REBOOT,

            /// <summary>
            /// 
            /// </summary>
            RestartApps = Win32Constants.EWX_RESTARTAPPS,
        }

        private static void GetLastErrorThrow()
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// Logoffs the current user.
        /// </summary>
        public static void LogoffCurrentUser()
        {
            LogoffCurrentUser(false);
        }

        /// <summary>
        /// Logoffs the current user.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public static void LogoffCurrentUser(bool force)
        {
            ExitWindows(WindowsControl.Logoff, force);
        }

        /// <summary>
        /// Shutdowns this instance.
        /// </summary>
        public static void Shutdown()
        {
            Shutdown(false);
        }

        /// <summary>
        /// Shutdowns the specified force.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public static void Shutdown(bool force)
        {
            Shutdown(true);
        }

        /// <summary>
        /// Shutdowns the specified force.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <param name="powerOff">if set to <c>true</c> [power off].</param>
        public static void Shutdown(bool force, bool powerOff)
        {
            ExitWindows(powerOff ? WindowsControl.ShutdownAndPowerOff : WindowsControl.ShutdownNoPowerOff, force);
        }

        /// <summary>
        /// Restarts the windows.
        /// </summary>
        public static void RestartWindows()
        {
            RestartWindows(false);
        }

        /// <summary>
        /// Restarts the windows.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public static void RestartWindows(bool force)
        {
            RestartWindows(false);
        }

        /// <summary>
        /// Restarts the windows.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <param name="restartApps">if set to <c>true</c> [restart apps].</param>
        public static void RestartWindows(bool force, bool restartApps)
        {
            ExitWindows(restartApps ? WindowsControl.RestartApps : WindowsControl.Restart, force);
        }

        /// <summary>
        /// Exits the windows.
        /// </summary>
        /// <param name="control">The control.</param>
        public static void ExitWindows(WindowsControl control)
        {
            ExitWindows(control, false);
        }

        /// <summary>
        /// Exits the windows.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public static void ExitWindows(WindowsControl control, bool force)
        {
            ExitWindows(control, force, true, Win32Constants.SHTDN_REASON_MAJOR_OTHER, Win32Constants.SHTDN_REASON_MINOR_OTHER);
        }

        /// <summary>
        /// Exits the windows.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <param name="planned">if set to <c>true</c> [planned].</param>
        /// <param name="majorReason">The major reason.</param>
        /// <param name="minorReason">The minor reason.</param>
        public static void ExitWindows(WindowsControl control, bool force, bool planned, uint majorReason, uint minorReason)
        {
            uint flags = (uint)control;
            if (force)
            {
                flags |= Win32Constants.EWX_FORCEIFHUNG;
            }

            uint reason = majorReason | minorReason;
            if (planned)
            {
                reason |= Win32Constants.SHTDN_REASON_FLAG_PLANNED;
            }

            if (!ExternalMethods.ExitWindowsEx(flags, reason))
            {
                GetLastErrorThrow();
            }
        }
    }
#endif

    /// <summary>
    /// Wrapper around the Process class to add some convenience methods but
    /// most importantly deal with the complex nature of getting both
    /// StandardOutput and StandardError streams concurrently (this must be done with
    /// callbacks). See http://msdn2.microsoft.com/en-us/library/system.diagnostics.process.standarderror.aspx
    /// </summary>
    public class ProcessHelper
    {
        private Process m_process = new Process();
        private TextWriter m_error = Console.Error;
        private TextWriter m_out = Console.Out;
        private StringBuilder m_outBuilder;
        private StringBuilder m_errorBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessHelper"/> class.
        /// </summary>
        public ProcessHelper()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessHelper"/> class.
        /// </summary>
        /// <param name="sendStreamsToStrings">if set to <c>true</c> [send streams to strings].</param>
        public ProcessHelper(bool sendStreamsToStrings)
        {
            m_process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            m_process.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
            m_process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);

            if (sendStreamsToStrings)
            {
                // Caller wants the standard output and error streams
                // to be written in memory to strings that can later
                // be extracted
                m_outBuilder = new StringBuilder(512);
                m_errorBuilder = new StringBuilder();

                Out = new System.IO.StringWriter(m_outBuilder);
                Error = new System.IO.StringWriter(m_errorBuilder);
            }
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get
            {
                return m_process.StartInfo.FileName;
            }
            set
            {
                m_process.StartInfo.FileName = value;
            }
        }

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string Arguments
        {
            get
            {
                return m_process.StartInfo.Arguments;
            }
            set
            {
                m_process.StartInfo.Arguments = value;
            }
        }

        /// <summary>
        /// Sets the arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        public void SetArguments(params string[] args)
        {
            if (args != null)
            {
                StringBuilder sb = GetMangledArguments(args);
                Arguments = sb.ToString();
            }
        }

        /// <summary>
        /// Gets the mangled arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        private static StringBuilder GetMangledArguments(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            int c = 0;
            bool containsSpace;
            foreach (string val in args)
            {
                if (c > 0)
                {
                    sb.Append(' ');
                }
                containsSpace = (val.IndexOf(' ') != -1);
                if (containsSpace)
                {
                    sb.Append('\"');
                }
                sb.Append(val);
                if (containsSpace)
                {
                    sb.Append('\"');
                }
                c++;
            }
            return sb;
        }

        /// <summary>
        /// Adds the arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        public void AddArguments(params string[] args)
        {
            if (args != null)
            {
                StringBuilder sb = GetMangledArguments(args);
                if (!string.IsNullOrEmpty(Arguments))
                {
                    sb.Insert(0, ' ');
                    sb.Insert(0, Arguments);
                }
                Arguments = sb.ToString();
            }
        }

        /*public void AddUnmangledArguments(params string[] args)
        {
            if (args != null)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < args.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(' ');
                    }
                    sb.Append(args[i]);
                }
                if (!string.IsNullOrEmpty(Arguments))
                {
                    sb.Insert(0, ' ');
                    sb.Insert(0, Arguments);
                }
                Arguments = sb.ToString();
            }
        }*/

        /// <summary>
        /// Handles the OutputDataReceived event of the process control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Diagnostics.DataReceivedEventArgs"/> instance containing the event data.</param>
        protected virtual void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                m_out.WriteLine(e.Data);
            }
        }

        /// <summary>
        /// Handles the ErrorDataReceived event of the process control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Diagnostics.DataReceivedEventArgs"/> instance containing the event data.</param>
        protected virtual void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                m_error.WriteLine(e.Data);
            }
        }

        /// <summary>
        /// Gets or sets the process.
        /// </summary>
        /// <value>The process.</value>
        public Process Process
        {
            get
            {
                return m_process;
            }
            set
            {
                m_process = value;
            }
        }

        /// <summary>
        /// Gets the start info.
        /// </summary>
        /// <value>The start info.</value>
        public ProcessStartInfo StartInfo
        {
            get
            {
                return m_process.StartInfo;
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            Start(true);
        }

        /// <summary>
        /// Starts the process, begins asynchronous reads on
        /// both standard output and standard error.
        /// </summary>
        /// <param name="useRedirect">if set to <c>true</c> [use redirect].</param>
        public void Start(bool useRedirect)
        {
            // Initialize the asynchronous stuff
            m_process.StartInfo.UseShellExecute = false;
            if (useRedirect)
            {
                m_process.StartInfo.RedirectStandardError = true;
                m_process.StartInfo.RedirectStandardOutput = true;
            }
            m_process.StartInfo.CreateNoWindow = true;

            m_process.Start();

            if (useRedirect)
            {
                m_process.BeginErrorReadLine();
                m_process.BeginOutputReadLine();
            }
        }

        /// <summary>
        /// Starts with a timeout of <see cref="GlobalConstants.ExecuteSmallProcessTimeout"/>
        /// milliseconds and does not throw an exception when it sees an error, but returns
        /// the standard error and output.
        /// </summary>
        /// <returns></returns>
        public int StartAndWaitForExit()
        {
            return StartAndWaitForExit(false);
        }

        /// <summary>
        /// Starts the and wait for exit.
        /// </summary>
        /// <param name="timeoutMs">The timeout ms.</param>
        /// <returns></returns>
        public int StartAndWaitForExit(int timeoutMs)
        {
            return StartAndWaitForExit(timeoutMs, false);
        }

        /// <summary>
        /// Starts the process, begins asynchronous reads on
        /// both standard output and standard error, and
        /// waits for the process to exit. The return code
        /// of the process is returned.
        /// </summary>
        /// <param name="timeoutMs">The timeout ms.</param>
        /// <param name="throwOnError">if set to <c>true</c> [throw on error].</param>
        /// <returns>Return code of completed process</returns>
        public int StartAndWaitForExit(int timeoutMs, bool throwOnError)
        {
            Start();

            m_process.WaitForExit(timeoutMs);

            int exit = m_process.ExitCode;

            if (throwOnError)
            {
                CheckForError(true);
            }

            return exit;
        }

        /// <summary>
        /// Starts the process, begins asynchronous reads on
        /// both standard output and standard error, and
        /// waits for the process to exit. The return code
        /// of the process is returned.
        /// </summary>
        /// <param name="throwOnError">if set to <c>true</c> [throw on error].</param>
        /// <returns>Return code of completed process</returns>
        public int StartAndWaitForExit(bool throwOnError)
        {
            return StartAndWaitForExit(GlobalConstants.ExecuteSmallProcessTimeout, throwOnError);
        }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public TextWriter Error
        {
            get
            {
                return m_error;
            }
            set
            {
                m_error = value;
            }
        }

        /// <summary>
        /// Gets or sets the out.
        /// </summary>
        /// <value>The out.</value>
        public TextWriter Out
        {
            get
            {
                return m_out;
            }
            set
            {
                m_out = value;
            }
        }

        /// <summary>
        /// Gets the exit code.
        /// </summary>
        /// <value>The exit code.</value>
        public int ExitCode
        {
            get
            {
                return m_process.ExitCode;
            }
        }

        /// <summary>
        /// Gets the standard output.
        /// </summary>
        /// <value>The standard output.</value>
        public string StandardOutput
        {
            get
            {
                return m_outBuilder == null ? null : m_outBuilder.ToString();
            }
        }

        /// <summary>
        /// Gets the standard error.
        /// </summary>
        /// <value>The standard error.</value>
        public string StandardError
        {
            get
            {
                return m_errorBuilder == null ? null : m_errorBuilder.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                string result = FileName;
                if (result.IndexOf(' ') != -1)
                {
                    result = "\"" + result + "\"";
                }

                if (!string.IsNullOrEmpty(Arguments))
                {
                    result += " " + Arguments;
                }
                return result;
            }
            return base.ToString();
        }

        /// <summary>
        /// Checks for error.
        /// </summary>
        /// <returns></returns>
        public string CheckForError()
        {
            return CheckForError(true);
        }

        /// <summary>
        /// Checks for error.
        /// </summary>
        /// <param name="throwOnError">if set to <c>true</c> [throw on error].</param>
        /// <returns></returns>
        public string CheckForError(bool throwOnError)
        {
            string error = null;
            if (ExitCode != 0)
            {
                error = "" + StandardError + "\n" + StandardOutput;
            }
            else if (!string.IsNullOrEmpty(StandardError))
            {
                error = StandardError + "\n" + StandardOutput;
            }

            if (throwOnError && error != null)
            {
                throw new Exception(error);
            }

            return error;
        }
    }

    /// <summary>
    /// Windows culture constants such as LCIDs and culture names.
    /// See: http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemglobalizationcultureinfoclasstopic.asp
    /// </summary>
    public class CultureConstants
    {
        /// <summary>
        /// 
        /// </summary>
        public const string CultureInvariant = "";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureInvariantIdentifier = 127;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralAfrikaans = "af";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralAfrikaansIdentifier = 54;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificAfrikaansSouthAfrica = "af-ZA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificAfrikaansSouthAfricaIdentifier = 1078;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralAlbanian = "sq";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralAlbanianIdentifier = 28;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificAlbanianAlbania = "sq-AL";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificAlbanianAlbaniaIdentifier = 1052;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralArabic = "ar";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralArabicIdentifier = 1;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralArabicAlgeria = "ar-DZ";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralArabicAlgeriaIdentifier = 5121;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicBahrain = "ar-BH";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicBahrainIdentifier = 15361;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicEgypt = "ar-EG";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicEgyptIdentifier = 3073;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicIraq = "ar-IQ";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicIraqIdentifier = 2049;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicJordan = "ar-JO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicJordanIdentifier = 11265;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicKuwait = "ar-KW";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicKuwaitIdentifier = 13313;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicLebanon = "ar-LB";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicLebanonIdentifier = 12289;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicLibya = "ar-LY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicLibyaIdentifier = 4097;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicMorocco = "ar-MA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicMoroccoIdentifier = 6145;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicOman = "ar-OM";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicOmanIdentifier = 8193;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicQatar = "ar-QA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicQatarIdentifier = 16385;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicSaudiArabia = "ar-SA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicSaudiArabiaIdentifier = 1025;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicSyria = "ar-SY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicSyriaIdentifier = 10241;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicTunisia = "ar-TN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicTunisiaIdentifier = 7169;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicUnitedArabEmirates = "ar-AE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicUnitedArabEmiratesIdentifier = 14337;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicYemen = "ar-YE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicYemenIdentifier = 9217;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralArmenian = "hy";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralArmenianIdentifier = 43;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArmenianArmenia = "hy-AM";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArmenianArmeniaIdentifier = 1067;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralAzeri = "az";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralAzeriIdentifier = 44;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificAzeriCyrillicAzerbaijan = "az-AZ-Cyrl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificAzeriCyrillicAzerbaijanIdentifier = 2092;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificAzeriLatinAzerbaijan = "az-AZ-Latn";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificAzeriLatinAzerbaijanIdentifier = 1068;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralBasque = "eu";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralBasqueIdentifier = 45;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificBasqueBasque = "eu-ES";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificBasqueBasqueIdentifier = 1069;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralBelarusian = "be";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralBelarusianIdentifier = 35;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificBelarusianBelarus = "be-BY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificBelarusianBelarusIdentifier = 1059;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralBulgarian = "bg";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralBulgarianIdentifier = 2;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificBulgarianBulgaria = "bg-BG";
        
        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificBulgarianBulgariaIdentifier = 1026;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralCatalan = "ca";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralCatalanIdentifier = 3;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificCatalanCatalan = "ca-ES";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificCatalanCatalanIdentifier = 1027;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseHongKongSar = "zh-HK";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseHongKongSarIdentifier = 3076;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseMacaoSar = "zh-MO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseMacaoSarIdentifier = 5124;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseChina = "zh-CN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseChinaIdentifier = 2052;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseSimplified = "zh-CHS";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseSimplifiedIdentifier = 4;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseSingapore = "zh-SG";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseSingaporeIdentifier = 4100;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseTaiwan = "zh-TW";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseTaiwanIdentifier = 1028;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseTraditional = "zh-CHT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseTraditionalIdentifier = 31748;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralCroatian = "hr";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralCroatianIdentifier = 26;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificCroatianCroatia = "hr-HR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificCroatianCroatiaIdentifier = 1050;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralCzech = "cs";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralCzechIdentifier = 5;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificCzechCzechRepublic = "cs-CZ";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificCzechCzechRepublicIdentifier = 1029;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralDanish = "da";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralDanishIdentifier = 6;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificDanishDenmark = "da-DK";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificDanishDenmarkIdentifier = 1030;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralDhivehi = "div";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralDhivehiIdentifier = 101;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificDhivehiMaldives = "div-MV";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificDhivehiMaldivesIdentifier = 1125;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralDutch = "nl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralDutchIdentifier = 19;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificDutchBelgium = "nl-BE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificDutchBelgiumIdentifier = 2067;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificDutchTheNetherlands = "nl-NL";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificDutchTheNetherlandsIdentifier = 1043;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralEnglish = "en";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralEnglishIdentifier = 9;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishAustralia = "en-AU";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishAustraliaIdentifier = 3081;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishBelize = "en-BZ";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishBelizeIdentifier = 10249;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishCanada = "en-CA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishCanadaIdentifier = 4105;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishCaribbean = "en-CB";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishCaribbeanIdentifier = 9225;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishIreland = "en-IE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishIrelandIdentifier = 6153;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishJamaica = "en-JM";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishJamaicaIdentifier = 8201;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishNewZealand = "en-NZ";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishNewZealandIdentifier = 5129;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishPhilippines = "en-PH";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishPhilippinesIdentifier = 13321;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishSouthAfrica = "en-ZA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishSouthAfricaIdentifier = 7177;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishTrinidadAndTobago = "en-TT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishTrinidadAndTobagoIdentifier = 11273;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishUnitedKingdom = "en-GB";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishUnitedKingdomIdentifier = 2057;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishUnitedStates = "en-US";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishUnitedStatesIdentifier = 1033;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishZimbabwe = "en-ZW";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishZimbabweIdentifier = 12297;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralEstonian = "et";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralEstonianIdentifier = 37;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEstonianEstonia = "et-EE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEstonianEstoniaIdentifier = 1061;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralFaroese = "fo";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralFaroeseIdentifier = 56;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFaroeseFaroeIslands = "fo-FO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFaroeseFaroeIslandsIdentifier = 1080;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralFarsi = "fa";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralFarsiIdentifier = 41;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFarsiIran = "fa-IR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFarsiIranIdentifier = 1065;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralFinnish = "fi";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralFinnishIdentifier = 11;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFinnishFinland = "fi-FI";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFinnishFinlandIdentifier = 1035;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralFrench = "fr";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralFrenchIdentifier = 12;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFrenchBelgium = "fr-BE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFrenchBelgiumIdentifier = 2060;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFrenchCanada = "fr-CA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFrenchCanadaIdentifier = 3084;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFrenchFrance = "fr-FR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFrenchFranceIdentifier = 1036;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFrenchLuxembourg = "fr-LU";
        
        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFrenchLuxembourgIdentifier = 5132;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFrenchMonaco = "fr-MC";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFrenchMonacoIdentifier = 6156;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFrenchSwitzerland = "fr-CH";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFrenchSwitzerlandIdentifier = 4108;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralGalician = "gl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralGalicianIdentifier = 86;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGalicianGalician = "gl-ES";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGalicianGalicianIdentifier = 1110;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralGeorgian = "ka";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralGeorgianIdentifier = 55;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGeorgianGeorgia = "ka-GE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGeorgianGeorgiaIdentifier = 1079;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralGerman = "de";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralGermanIdentifier = 7;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGermanAustria = "de-AT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGermanAustriaIdentifier = 3079;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGermanGermany = "de-DE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGermanGermanyIdentifier = 1031;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGermanLiechtenstein = "de-LI";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGermanLiechtensteinIdentifier = 5127;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGermanLuxembourg = "de-LU";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGermanLuxembourgIdentifier = 4103;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGermanSwitzerland = "de-CH";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGermanSwitzerlandIdentifier = 2055;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralGreek = "el";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralGreekIdentifier = 8;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGreekGreece = "el-GR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGreekGreeceIdentifier = 1032;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralGujarati = "gu";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralGujaratiIdentifier = 71;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGujaratiIndia = "gu-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGujaratiIndiaIdentifier = 1095;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralHebrew = "he";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralHebrewIdentifier = 13;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificHebrewIsrael = "he-IL";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificHebrewIsraelIdentifier = 1037;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralHindi = "hi";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralHindiIdentifier = 57;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificHindiIndia = "hi-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificHindiIndiaIdentifier = 1081;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralHungarian = "hu";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralHungarianIdentifier = 14;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificHungarianHungary = "hu-HU";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificHungarianHungaryIdentifier = 1038;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralIcelandic = "is";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralIcelandicIdentifier = 15;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificIcelandicIceland = "is-IS";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificIcelandicIcelandIdentifier = 1039;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralIndonesian = "id";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralIndonesianIdentifier = 33;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificIndonesianIndonesia = "id-ID";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificIndonesianIndonesiaIdentifier = 1057;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralItalian = "it";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralItalianIdentifier = 16;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificItalianItaly = "it-IT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificItalianItalyIdentifier = 1040;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificItalianSwitzerland = "it-CH";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificItalianSwitzerlandIdentifier = 2064;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralJapanese = "ja";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralJapaneseIdentifier = 17;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificJapaneseJapan = "ja-JP";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificJapaneseJapanIdentifier = 1041;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralKannada = "kn";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralKannadaIdentifier = 75;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificKannadaIndia = "kn-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificKannadaIndiaIdentifier = 1099;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralKazakh = "kk";
        
        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralKazakhIdentifier = 63;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificKazakhKazakhstan = "kk-KZ";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificKazakhKazakhstanIdentifier = 1087;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralKonkani = "kok";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralKonkaniIdentifier = 87;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificKonkaniIndia = "kok-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificKonkaniIndiaIdentifier = 1111;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralKorean = "ko";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralKoreanIdentifier = 18;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificKoreanKorea = "ko-KR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificKoreanKoreaIdentifier = 1042;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralKyrgyz = "ky";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralKyrgyzIdentifier = 64;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificKyrgyzKyrgyzstan = "ky-KG";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificKyrgyzKyrgyzstanIdentifier = 1088;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralLatvian = "lv";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralLatvianIdentifier = 38;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificLatvianLatvia = "lv-LV";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificLatvianLatviaIdentifier = 1062;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralLithuanian = "lt";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralLithuanianIdentifier = 39;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificLithuanianLithuania = "lt-LT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificLithuanianLithuaniaIdentifier = 1063;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralMacedonian = "mk";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralMacedonianIdentifier = 47;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificMacedonianFormerYugoslavRepublicOfMacedonia = "mk-MK";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificMacedonianFormerYugoslavRepublicOfMacedoniaIdentifier = 1071;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralMalay = "ms";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralMalayIdentifier = 62;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificMalayBrunei = "ms-BN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificMalayBruneiIdentifier = 2110;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificMalayMalaysia = "ms-MY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificMalayMalaysiaIdentifier = 1086;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralMarathi = "mr";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralMarathiIdentifier = 78;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificMarathiIndia = "mr-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificMarathiIndiaIdentifier = 1102;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralMongolian = "mn";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralMongolianIdentifier = 80;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificMongolianMongolia = "mn-MN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificMongolianMongoliaIdentifier = 1104;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralNorwegian = "no";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralNorwegianIdentifier = 20;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificNorwegianBokmlNorway = "nb-NO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificNorwegianBokmlNorwayIdentifier = 1044;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificNorwegianNynorskNorway = "nn-NO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificNorwegianNynorskNorwayIdentifier = 2068;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralPolish = "pl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralPolishIdentifier = 21;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificPolishPoland = "pl-PL";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificPolishPolandIdentifier = 1045;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralPortuguese = "pt";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralPortugueseIdentifier = 22;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificPortugueseBrazil = "pt-BR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificPortugueseBrazilIdentifier = 1046;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificPortuguesePortugal = "pt-PT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificPortuguesePortugalIdentifier = 2070;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralPunjabi = "pa";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralPunjabiIdentifier = 70;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificPunjabiIndia = "pa-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificPunjabiIndiaIdentifier = 1094;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralRomanian = "ro";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralRomanianIdentifier = 24;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificRomanianRomania = "ro-RO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificRomanianRomaniaIdentifier = 1048;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralRussian = "ru";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralRussianIdentifier = 25;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificRussianRussia = "ru-RU";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificRussianRussiaIdentifier = 1049;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSanskrit = "sa";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSanskritIdentifier = 79;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSanskritIndia = "sa-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSanskritIndiaIdentifier = 1103;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSerbianCyrrilicSerbia = "sr-SP-Cyrl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSerbianCyrrilicSerbiaIdentifier = 3098;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSerbianLatinSerbia = "sr-SP-Latn";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSerbianLatinSerbiaIdentifier = 2074;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSlovak = "sk";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSLOVAKIdentifier = 27;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSlovakSlovakia = "sk-SK";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSlovakSlovakiaIdentifier = 1051;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSlovenian = "sl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSlovenianIdentifier = 36;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSlovenianSlovenia = "sl-SI";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSlovenianSloveniaIdentifier = 1060;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSpanish = "es";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSPANISHIdentifier = 10;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishArgentina = "es-AR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishArgentinaIdentifier = 11274;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishBolivia = "es-BO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishBoliviaIdentifier = 16394;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishChile = "es-CL";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishChileIdentifier = 13322;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishColombia = "es-CO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishColombiaIdentifier = 9226;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishCostaRica = "es-CR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishCostaRicaIdentifier = 5130;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishDominicanRepublic = "es-DO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishDominicanRepublicIdentifier = 7178;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishEcuador = "es-EC";
        
        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishEcuadorIdentifier = 12298;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishElSalvador = "es-SV";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishElSalvadorIdentifier = 17418;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishGuatemala = "es-GT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishGuatemalaIdentifier = 4106;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishHonduras = "es-HN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishHondurasIdentifier = 18442;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishMexico = "es-MX";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishMexicoIdentifier = 2058;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishNicaragua = "es-NI";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishNicaraguaIdentifier = 19466;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishPanama = "es-PA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishPanamaIdentifier = 6154;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishParaguay = "es-PY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishParaguayIdentifier = 15370;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishPeru = "es-PE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishPeruIdentifier = 10250;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishPuertoRico = "es-PR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishPuertoRicoIdentifier = 20490;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishSpain = "es-ES";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishSpainIdentifier = 3082;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishUruguay = "es-UY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishUruguayIdentifier = 14346;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishVenezuela = "es-VE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishVenezuelaIdentifier = 8202;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSwahili = "sw";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSwahiliIdentifier = 65;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSwahiliKenya = "sw-KE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSwahiliKenyaIdentifier = 1089;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSwedish = "sv";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSwedishIdentifier = 29;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSwedishFinland = "sv-FI";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSwedishFinlandIdentifier = 2077;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSwedishSweden = "sv-SE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSwedishSwedenIdentifier = 1053;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSyriac = "syr";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSyriacIdentifier = 90;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSyriacSyria = "syr-SY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSyriacSyriaIdentifier = 1114;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralTamil = "ta";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralTamilIdentifier = 73;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificTamilIndia = "ta-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificTamilIndiaIdentifier = 1097;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralTatar = "tt";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralTatarIdentifier = 68;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificTatarRussia = "tt-RU";
        
        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificTatarRussiaIdentifier = 1092;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralTelugu = "te";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralTeluguIdentifier = 74;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificTeluguIndia = "te-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificTeluguIndiaIdentifier = 1098;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralThai = "th";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralThaiIdentifier = 30;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificThaiThailand = "th-TH";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificThaiThailandIdentifier = 1054;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralTurkish = "tr";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralTurkishIdentifier = 31;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificTurkishTurkey = "tr-TR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificTurkishTurkeyIdentifier = 1055;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralUkrainian = "uk";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralUkrainianIdentifier = 34;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificUkrainianUkraine = "uk-UA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificUkrainianUkraineIdentifier = 1058;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralUrdu = "ur";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralUrduIdentifier = 32;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificUrduPakistan = "ur-PK";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificUrduPakistanIdentifier = 1056;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralUzbek = "uz";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralUzbekIdentifier = 67;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificUzbekCyrillicUzbekistan = "uz-UZ-Cyrl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificUzbekCyrillicUzbekistanIdentifier = 2115;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificUzbekLatinUzbekistan = "uz-UZ-Latn";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificUzbekLatinUzbekistanIdentifier = 1091;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralVietnamese = "vi";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralVietnameseIdentifier = 42;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificVietnameseVietnam = "vi-VN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificVietnameseVietnamIdentifier = 1066;

        /// <summary>
        /// Used for parsing MSDN output into C# code.
        /// </summary>
        public static void CreateClass()
        {
            using (StreamReader reader = new StreamReader(@"c:\temp\langs.txt"))
            {
                using (StreamWriter writer = new StreamWriter(@"c:\temp\langsout.txt"))
                {
                    string line;
                    string cult, lcidstr, name;
                    cult = name = lcidstr = null;
                    int stage = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line != string.Empty)
                        {
                            switch (stage)
                            {
                                case 0:
                                    cult = line;
                                    break;
                                case 1:
                                    lcidstr = line;
                                    break;
                                case 2:
                                    {
                                        name = line;
                                        stage = -1;
                                        int lcid = int.Parse(lcidstr.Substring(2), System.Globalization.NumberStyles.HexNumber);
                                        name = cleanName(name);
                                        writer.WriteLine(string.Format("\t\tpublic const string Culture{3}{2} = \"{0}\";\n\t\tpublic const int Culture{3}{2}Identifier = {1};\n", cult, lcid, name, cult.Contains("-") ? "Specific" : "Neutral"));
                                        break;
                                    }
                            }
                            stage++;
                        }
                    }
                }
            }
        }

        private static string cleanName(string name)
        {
            name = name.Replace("- ", "").Replace(" ", "_").Replace("(", "").Replace(")", "");
            return name;
        }
    }

    /// <summary>
    /// Interface that represents an object that can be locked by a reader/writer lock.
    /// </summary>
    public interface IExposesReaderWriterLock
    {
        /// <summary>
        /// Gets the sync.
        /// </summary>
        /// <value>The sync.</value>
        ReaderWriterLock Sync { get; }

        /// <summary>
        /// Called when [before acquire].
        /// </summary>
        /// <param name="desiredType">Type of the desired.</param>
        void OnBeforeAcquire(ReaderWriterLockSynchronizeType desiredType);
    }

    /// <summary>
    /// Generic Reader/Writer lock that can be used in a using() statement.
    /// </summary>
    public class DisposableReaderWriter : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableReaderWriter"/> class.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="type">The type.</param>
        public DisposableReaderWriter(IExposesReaderWriterLock root, ReaderWriterLockSynchronizeType type)
        {
            m_Root = root;
            m_SynchronizeType = type;

            AcquireLock();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            ReleaseLock();
        }

        /// <summary>
        /// Acquires the lock.
        /// </summary>
        protected virtual void AcquireLock()
        {
            m_Root.OnBeforeAcquire(m_SynchronizeType);
            switch (m_SynchronizeType)
            {
                case ReaderWriterLockSynchronizeType.Read:
                    AcquireReaderLock();
                    break;
                case ReaderWriterLockSynchronizeType.Write:
                    AcquireWriterLock();
                    break;
            }
        }

        /// <summary>
        /// Releases the lock.
        /// </summary>
        protected virtual void ReleaseLock()
        {
            DowngradeFromWriterLock();
            switch (m_SynchronizeType)
            {
                case ReaderWriterLockSynchronizeType.Read:
                    ReleaseReaderLock();
                    break;
                case ReaderWriterLockSynchronizeType.Write:
                    ReleaseWriterLock();
                    break;
            }
        }

        /// <summary>
        /// Acquires the reader lock.
        /// </summary>
        protected virtual void AcquireReaderLock()
        {
            m_Root.Sync.AcquireReaderLock(DefaultLockTimeout);
        }

        /// <summary>
        /// Acquires the writer lock.
        /// </summary>
        protected virtual void AcquireWriterLock()
        {
            m_Root.Sync.AcquireWriterLock(DefaultLockTimeout);
        }

        /// <summary>
        /// Releases the reader lock.
        /// </summary>
        protected virtual void ReleaseReaderLock()
        {
            m_Root.Sync.ReleaseReaderLock();
        }

        /// <summary>
        /// Releases the writer lock.
        /// </summary>
        protected virtual void ReleaseWriterLock()
        {
            m_Root.Sync.ReleaseWriterLock();
        }

        /// <summary>
        /// Upgrades to writer lock.
        /// </summary>
        public virtual void UpgradeToWriterLock()
        {
            m_SynchronizeType = ReaderWriterLockSynchronizeType.Write;
            m_UpgradeLockCookie = m_Root.Sync.UpgradeToWriterLock(DefaultLockTimeout);
        }

        /// <summary>
        /// Downgrades from writer lock.
        /// </summary>
        public virtual void DowngradeFromWriterLock()
        {
            if (m_UpgradeLockCookie != null)
            {
                m_SynchronizeType = ReaderWriterLockSynchronizeType.Read;
                LockCookie tempCookie = m_UpgradeLockCookie.Value;
                m_Root.Sync.DowngradeFromWriterLock(ref tempCookie);
                m_UpgradeLockCookie = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public const int DefaultLockTimeout = 100;

        /// <summary>
        /// 
        /// </summary>
        protected ReaderWriterLockSynchronizeType m_SynchronizeType;

        /// <summary>
        /// 
        /// </summary>
        protected IExposesReaderWriterLock m_Root;

        private LockCookie? m_UpgradeLockCookie;
    }

    /// <summary>
    /// The class can be used in a using() {} block or a try, finally block with
    /// a dispose call and allows for setting the current Thread's culture
    /// to the invariant culture during the length of the scope. This is useful
    /// when it is critical to have invariant culture rules, for example, if
    /// you are dependent that a real number is of the form X.XXXX, then you
    /// will be thrown off if there is a European culture.
    /// </summary>
    public class InvariantCultureContext : IDisposable
    {
        private CultureInfo oldCulture;
        private CultureInfo oldUICulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvariantCultureContext"/> class.
        /// </summary>
        public InvariantCultureContext()
        {
            oldCulture = Thread.CurrentThread.CurrentCulture;
            oldUICulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = oldCulture;
            Thread.CurrentThread.CurrentUICulture = oldUICulture;
        }
    }

    /// <summary>
    /// Common regular expressions.
    /// http://www.codeproject.com/dotnet/RegexTutorial.asp
    /// </summary>
    public static class RegexUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        public const string HtmlBreakExpression = @"<\s*br\s*/?\s*>";

        /// <summary>
        /// 
        /// </summary>
        public const string HtmlParagraphExpression = @"<\s*p\s*/?\s*>";

        /// <summary>
        /// 
        /// </summary>
        public const string HtmlBreakOrParagraphExpression = @"<\s*([bp]r?)\s*/?\s*>";

        /// <summary>
        /// 
        /// </summary>
        public static Regex HtmlBreak = new Regex(HtmlBreakExpression, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        public static Regex HtmlParagraph = new Regex(HtmlParagraphExpression, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        public static Regex HtmlBreakOrParagraph = new Regex(HtmlBreakOrParagraphExpression, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        public const string HtmlBreakOrParagraphTrimLeftExpression = @"^" + HtmlBreakOrParagraphExpression;

        /// <summary>
        /// 
        /// </summary>
        public const string HtmlBreakOrParagraphTrimRightExpression = HtmlBreakOrParagraphExpression + @"$";

        /// <summary>
        /// 
        /// </summary>
        public static Regex HtmlBreakOrParagraphTrim = new Regex(string.Format("({0})|({1})", HtmlBreakOrParagraphTrimLeftExpression, HtmlBreakOrParagraphTrimRightExpression), RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        public const string UriChars = @"[^\s)<>\]}!([]+";

        /// <summary>
        /// 
        /// </summary>
        public static readonly Regex Uri = new Regex(@"\w+://" + UriChars, RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Regex UriLenient = new Regex(@"(\w+://)?" + UriChars, RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Regex Email = new Regex(@"^[\w-\.]{1,}\@([\da-zA-Z-]{1,}\.){1,}[\da-zA-Z-]{2,3}$", RegexOptions.Compiled);
    }

    /// <summary>
    /// Methods for working with code and languages.
    /// </summary>
    public static class CodeUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="PublicDomain.CodeUtilities.CompileException"/>
        /// <exception cref="PublicDomain.CodeUtilities.NativeCompileException"/>
        public static string Eval(Language language, string code)
        {
            using (CodeDomProvider domProvider = CodeDomProvider.CreateProvider(language.ToString()))
            {
                CompilerInfo compilerInfo = CodeDomProvider.GetCompilerInfo(language.ToString());
                CompilerParameters compilerParameters = compilerInfo.CreateDefaultCompilerParameters();
                CompilerResults results = domProvider.CompileAssemblyFromSource(compilerParameters, code);
                CheckCompilerResultsThrow(results);
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// This method throws an Exception if it finds an error in the
        /// <c>results</c>, otherwise it returns without side effect.
        /// </summary>
        /// <param name="results"></param>
        /// <exception cref="PublicDomain.CodeUtilities.CompileException"/>
        /// <exception cref="PublicDomain.CodeUtilities.NativeCompileException"/>
        public static void CheckCompilerResultsThrow(CompilerResults results)
        {
            if (results.Errors.HasErrors)
            {
                string msg = GetCompilerErrorsAsString(results.Errors);
                if (results.NativeCompilerReturnValue != 0)
                {
                    msg += Environment.NewLine + GetNativeCompilerErrorMessage(results);
                }
                throw new CompileException(msg);
            }
            else if (results.NativeCompilerReturnValue != 0)
            {
                throw new NativeCompileException(GetNativeCompilerErrorMessage(results));
            }
        }

        private static string GetNativeCompilerErrorMessage(CompilerResults results)
        {
            return "Compiler returned exit code " + results.NativeCompilerReturnValue;
        }

        /// <summary>
        /// Gets the compiler errors as string.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static string GetCompilerErrorsAsString(CompilerErrorCollection errors)
        {
            StringBuilder sb = new StringBuilder(errors.Count * 10);
            CompilerError error;
            for (int i = 0; i < errors.Count; i++)
            {
                error = errors[i];
                if (i > 0)
                {
                    sb.Append(Environment.NewLine);
                }
                sb.Append(error.ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Thrown when an error is encountered compiling.
        /// </summary>
        [Serializable]
        public class CompileException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CompileException"/> class.
            /// </summary>
            public CompileException() { }
            
            /// <summary>
            /// Initializes a new instance of the <see cref="CompileException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            
            public CompileException(string message) : base(message) { }
            /// <summary>
            /// Initializes a new instance of the <see cref="CompileException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="inner">The inner.</param>
            
            public CompileException(string message, Exception inner) : base(message, inner) { }
            /// <summary>
            /// Initializes a new instance of the <see cref="CompileException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
            
            protected CompileException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }

        /// <summary>
        /// Thrown when the compiler returns an unexpected value.
        /// </summary>
        [Serializable]
        public class NativeCompileException : CompileException
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="NativeCompileException"/> class.
            /// </summary>
            public NativeCompileException() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="NativeCompileException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            public NativeCompileException(string message) : base(message) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="NativeCompileException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="inner">The inner.</param>
            public NativeCompileException(string message, Exception inner) : base(message, inner) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="NativeCompileException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
            protected NativeCompileException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }

#if !(NOSCREENSCRAPER)
    /// <summary>
    /// Represents a scraped HTML tag.
    /// </summary>
    [Serializable]
    public class ScreenScraperTag
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name;

        /// <summary>
        /// 
        /// </summary>
        public NameValueCollection Attributes = new NameValueCollection();

        /// <summary>
        /// Finds the attribute value.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns></returns>
        public string FindAttributeValue(string attributeName)
        {
            foreach (string key in Attributes.AllKeys)
            {
                if (key.Equals(attributeName))
                {
                    return Attributes[key];
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Represents a scraped HTML page.
    /// </summary>
    [Serializable]
    public class ScrapedPage
    {
        /// <summary>
        /// 
        /// </summary>
        protected string m_RawStream;

        /// <summary>
        /// Gets or sets the raw stream.
        /// </summary>
        /// <value>The raw stream.</value>
        public string RawStream
        {
            get
            {
                return m_RawStream;
            }
            set
            {
                m_RawStream = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _RawStreamLowercase;

        /// <summary>
        /// Gets the raw stream lowercase.
        /// </summary>
        /// <value>The raw stream lowercase.</value>
        public string RawStreamLowercase
        {
            get
            {
                if (_RawStreamLowercase == null && RawStream != null)
                {
                    _RawStreamLowercase = RawStream.ToLower();
                }
                return _RawStreamLowercase;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Uri m_Url;
        
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public Uri Url
        {
            get
            {
                return m_Url;
            }
            set
            {
                m_Url = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private NameValueCollection _QueryParameters;

        /// <summary>
        /// Gets or sets the query parameters.
        /// </summary>
        /// <value>The query parameters.</value>
        public NameValueCollection QueryParameters
        {
            get
            {
                return _QueryParameters;
            }
            set
            {
                _QueryParameters = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private ScrapeType _ScrapeType;

        /// <summary>
        /// Gets or sets the type of the scrape.
        /// </summary>
        /// <value>The type of the scrape.</value>
        public ScrapeType ScrapeType
        {
            get
            {
                return _ScrapeType;
            }
            set
            {
                _ScrapeType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Title;

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                if (_Title == null && RawStreamLowercase != null)
                {
                    int titleIndex = RawStreamLowercase.IndexOf("<title");
                    if (titleIndex != -1)
                    {
                        int titleEnd = RawStreamLowercase.IndexOf(">", titleIndex);
                        if (titleEnd != -1)
                        {
                            titleEnd++;
                            int titleEndTag = RawStreamLowercase.IndexOf("<", titleEnd);
                            if (titleEndTag != -1)
                            {
                                _Title = RawStream.Substring(titleEnd, titleEndTag - titleEnd);
                            }
                        }
                    }
                }
                return _Title == null ? "" : _Title;
            }
        }

        /// <summary>
        /// Finds the substring.
        /// </summary>
        /// <param name="pretext">The pretext.</param>
        /// <param name="posttext">The posttext.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        public string FindSubstring(string pretext, string posttext, bool caseSensitive)
        {
            return FindSubstring(GetSubject(ref pretext, ref posttext, null, caseSensitive), pretext, posttext, caseSensitive);
        }

        /// <summary>
        /// Gets the subject.
        /// </summary>
        /// <param name="pretext">The pretext.</param>
        /// <param name="posttext">The posttext.</param>
        /// <param name="contextFind">The context find.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        private string GetSubject(ref string pretext, ref string posttext, string contextFind, bool caseSensitive)
        {
            string subject = RawStream;
            if (!caseSensitive)
            {
                pretext = pretext.ToLower();
                posttext = posttext.ToLower();
                if (contextFind != null)
                {
                    contextFind = contextFind.ToLower();
                }
            }
            if (subject != null && contextFind != null)
            {
                string subjectSearch = subject;
                if (!caseSensitive)
                {
                    subjectSearch = subjectSearch.ToLower();
                }
                int contextFindIndex = subjectSearch.IndexOf(contextFind);
                if (contextFindIndex != -1)
                {
                    return subject.Substring(contextFindIndex + contextFind.Length);
                }
                return null;
            }
            else
            {
                return subject;
            }
        }

        /// <summary>
        /// This searches the content stream for any piece of text that is surrounded
        /// by the prettext and posttext arguments
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="pretext">The pretext.</param>
        /// <param name="posttext">The posttext.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        public string FindSubstring(string subject, string pretext, string posttext, bool caseSensitive)
        {
            if (subject != null)
            {
                string subjectSearch = subject;
                if (!caseSensitive)
                {
                    subjectSearch = subject.ToLower();
                    pretext = pretext.ToLower();
                    posttext = posttext.ToLower();
                }
                // First, try to find the prettext
                int pretextstart = subjectSearch.IndexOf(pretext);
                if (pretextstart != -1)
                {
                    // Now try to find the posttext, that is after the prettext
                    pretextstart += pretext.Length;
                    int posttextstart = subjectSearch.IndexOf(posttext, pretextstart);
                    if (posttextstart != -1)
                    {
                        // We always return the substring from the rawstream
                        return subject.Substring(pretextstart, posttextstart - pretextstart);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the substring by context.
        /// </summary>
        /// <param name="contextFind">The context find.</param>
        /// <param name="prettext">The prettext.</param>
        /// <param name="posttext">The posttext.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        public string FindSubstringByContext(string contextFind, string prettext, string posttext, bool caseSensitive)
        {
            return FindSubstring(GetSubject(ref prettext, ref posttext, contextFind, caseSensitive), prettext, posttext, caseSensitive);
        }

        /// <summary>
        /// Splits the by encapsulating tags.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        public IList<string> SplitByEncapsulatingTags(string subject, string tagName, bool caseSensitive)
        {
            string subjectSearch = subject;
            if (!tagName.Contains("<"))
            {
                tagName = "<" + tagName;
            }
            if (tagName.EndsWith(">"))
            {
                tagName = tagName.Substring(0, tagName.Length - 1);
            }
            if (!caseSensitive)
            {
                subjectSearch = subjectSearch.ToLower();
                tagName = tagName.ToLower();
            }
            string endTag = CreateEndTag(tagName);
            IList<string> ret = new List<string>();

            int searchStart = 0;
            while (searchStart >= 0)
            {
                int startIndex = subjectSearch.IndexOf(tagName, searchStart);
                if (startIndex == -1)
                {
                    break;
                }
                startIndex += tagName.Length;

                // now, try to find the end of the start tag
                startIndex = subjectSearch.IndexOf(">", startIndex);
                if (startIndex == -1)
                {
                    break;
                }
                startIndex++;

                int endIndex = subjectSearch.IndexOf(endTag, startIndex);
                if (endIndex == -1)
                {
                    break;
                }

                ret.Add(subject.Substring(startIndex, endIndex - startIndex));

                searchStart = endIndex + endTag.Length;
            }
            return ret;
        }

        /// <summary>
        /// Creates the end tag.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns></returns>
        private string CreateEndTag(string tagName)
        {
            int ltindex = tagName.IndexOf("<");
            if (ltindex != -1)
            {
                return tagName.Insert(ltindex + 1, "/");
            }
            return null;
        }

        /// <summary>
        /// Converts the link to pair.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        public Pair<string, string> ConvertLinkToPair(string subject)
        {
            return ConvertLinkToPair(subject, true);
        }

        /// <summary>
        /// The first element in the pair is the HREF Link, and the second element
        /// is the text of the link.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="decodeLink">if set to <c>true</c> [decode link].</param>
        /// <returns></returns>
        public Pair<string, string> ConvertLinkToPair(string subject, bool decodeLink)
        {
            Regex re = new Regex(@"<a\s+href\s*=\s*[""]([^""]+)[""](\s+[\w]+\s*=\s*[""]?[^""]+[""]?)*\s*>([^<]+)<\s*/\s*a\s*>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = re.Match(subject);
            if (m.Success)
            {
                string link = m.Groups[1].ToString().Trim();
                if (link.StartsWith("/"))
                {
                    link = Url.GetLeftPart(UriPartial.Authority) + link;
                }
                if (decodeLink)
                {
                    link = HttpUtility.UrlDecode(link);
                }
                return new Pair<string, string>(link, m.Groups[3].ToString());
            }
            return null;
        }

        /// <summary>
        /// Splits the string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="split">The split.</param>
        /// <returns></returns>
        public static IList<string> SplitString(string str, string split)
        {
            IList<string> pieces = new List<string>();
            do
            {
                int splitIndex = str.IndexOf(split);
                if (splitIndex == -1)
                {
                    pieces.Add(str);
                    break;
                }
                else
                {
                    pieces.Add(str.Substring(0, splitIndex));
                    str = str.Substring(splitIndex + split.Length);
                }
            } while (true);
            return pieces;
        }

        /// <summary>
        /// Basically removes extraneous characters like padding, newlines, tabs, etc.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string CanonicalizeString(string str)
        {
            return str == null ? null : str.Trim().Replace("\n", "").Replace("\r", "").Replace("\t", "");
        }

        /// <summary>
        /// Finds the childless tags.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        public IList<string> FindChildlessTags(string tagName, bool caseSensitive)
        {
            IList<string> ret = new List<string>();
            string searchSubject = caseSensitive ? RawStream : RawStreamLowercase;
            if (!caseSensitive)
            {
                tagName = tagName.ToLower();
            }
            if (!tagName.StartsWith("<"))
            {
                tagName = "<" + tagName;
            }
            if (searchSubject != null)
            {
                int startpos = 0;
                while (true)
                {
                    int startIndex = searchSubject.IndexOf(tagName, startpos);
                    if (startIndex == -1)
                    {
                        break;
                    }
                    int endIndex = searchSubject.IndexOf(">", startIndex);
                    if (endIndex == -1)
                    {
                        break;
                    }

                    ret.Add(RawStream.Substring(startIndex, endIndex - startIndex + 1));

                    startpos = endIndex + 1;
                }
            }
            return ret;
        }

        /// <summary>
        /// Matches the specified regex.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <returns></returns>
        public Match Match(string regex)
        {
            return Match(regex, true);
        }

        /// <summary>
        /// Matches the specified regex.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        public Match Match(string regex, bool caseSensitive)
        {
            return new Regex(regex, caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase).Match(RawStream);
        }

        private static Regex tagRegex = new Regex(@"<([\w\-]+)(\s+([\w\-]+)\s*=\s*[""]([^""]*)[""])*\s*/?\s*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex nameValueRegex = new Regex(@"([\w\-]+)\s*=\s*[""]([^""]*)[""]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Converts to tag.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="lowerNames">if set to <c>true</c> [lower names].</param>
        /// <returns></returns>
        public static ScreenScraperTag ConvertToTag(string html, bool lowerNames)
        {
            Match m = tagRegex.Match(html);
            if (m.Success)
            {
                ScreenScraperTag tag = new ScreenScraperTag();
                tag.Name = m.Groups[1].ToString();
                foreach (Capture capture in m.Groups[2].Captures)
                {
                    Match m2 = nameValueRegex.Match(capture.ToString());
                    if (m2.Success)
                    {
                        string id = m2.Groups[1].ToString();
                        if (lowerNames)
                        {
                            id = id.ToLower();
                        }
                        try
                        {
                            tag.Attributes[id] = m2.Groups[2].ToString();
                        }
                        catch (Exception)
                        {
                            // Catch if duplicate attributes are added
                        }
                    }
                }
                return tag;
            }
            return null;
        }

        /// <summary>
        /// Converts to tag list.
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <param name="lowerNames">if set to <c>true</c> [lower names].</param>
        /// <returns></returns>
        public static IList<ScreenScraperTag> ConvertToTagList(IList<string> tags, bool lowerNames)
        {
            IList<ScreenScraperTag> ret = new List<ScreenScraperTag>();
            foreach (string tag in tags)
            {
                ScreenScraperTag t = ConvertToTag(tag, lowerNames);
                if (t != null)
                {
                    ret.Add(t);
                }
            }
            return ret;
        }

        private static Regex CurrencyRegex = new Regex(@"\$([\d\.,]+)", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Finds the currency.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        public static string FindCurrency(string subject)
        {
            Match m = CurrencyRegex.Match(subject);
            if (m.Success)
            {
                return m.Groups[1].ToString();
            }
            return null;
        }

        /// <summary>
        /// Converts the currency string to decimal.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="ret">The ret.</param>
        /// <returns></returns>
        public static bool ConvertCurrencyStringToDecimal(string subject, out decimal ret)
        {
            ret = 0;
            return subject == null ? false : decimal.TryParse(subject, out ret);
        }

        /// <summary>
        /// Converts the currency string to double.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="ret">The ret.</param>
        /// <returns></returns>
        public static bool ConvertCurrencyStringToDouble(string subject, out double ret)
        {
            decimal dec;
            ret = 0;
            bool success = ConvertCurrencyStringToDecimal(subject, out dec);
            if (success)
            {
                ret = (double)dec;
            }
            return success;
        }

        /// <summary>
        /// Converts the string to date time.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="ret">The ret.</param>
        /// <returns></returns>
        public static bool ConvertStringToDateTime(string subject, TzTimeZone timeZone, out TzDateTime ret)
        {
            return TzDateTime.TryParseLenient(subject, timeZone, System.Globalization.DateTimeStyles.AssumeUniversal, out ret);
        }
    }

    /// <summary>
    /// Represents an HTTP session during a scraping of a page.
    /// </summary>
    [Serializable]
    public class ScrapeSession
    {
        private Scraper _ContainingScraper;
        
        /// <summary>
        /// Gets or sets the containing scraper.
        /// </summary>
        /// <value>The containing scraper.</value>
        protected Scraper ContainingScraper
        {
            get
            {
                return _ContainingScraper;
            }
            set
            {
                _ContainingScraper = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrapeSession"/> class.
        /// </summary>
        /// <param name="scraper">The scraper.</param>
        public ScrapeSession(Scraper scraper)
        {
            ContainingScraper = scraper;
        }

        /// <summary>
        /// 
        /// </summary>
        protected CookieContainer m_Cookies = new CookieContainer();

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        /// <value>The cookies.</value>
        public CookieContainer Cookies
        {
            get
            {
                return m_Cookies;
            }
        }

        /// <summary>
        /// Adds the cookie.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void AddCookie(string name, string value)
        {
            Cookies.Add(new Cookie(name, value, "/", ContainingScraper.Domain));
        }
    }

    /// <summary>
    /// The method of the scraping
    /// </summary>
    public enum ScrapeType
    {
        /// <summary>
        /// 
        /// </summary>
        GET,

        /// <summary>
        /// 
        /// </summary>
        POST
    }

    /// <summary>
    /// Entry point to scrape an HTML page.
    /// </summary>
    [Serializable]
    public class Scraper
    {
        /// <summary>
        /// 
        /// </summary>
        public static int DefaultExternalCallTimeout = 12000;

        /// <summary>
        /// 
        /// </summary>
        protected bool m_FollowEquivRefreshes = true;

        /// <summary>
        /// Gets or sets a value indicating whether [follow equiv refreshes].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [follow equiv refreshes]; otherwise, <c>false</c>.
        /// </value>
        public bool FollowEquivRefreshes
        {
            get
            {
                return m_FollowEquivRefreshes;
            }
            set
            {
                m_FollowEquivRefreshes = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ScrapeSession m_Session;

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        public ScrapeSession Session
        {
            get
            {
                return m_Session;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string m_Referer;

        /// <summary>
        /// Gets or sets the referer.
        /// </summary>
        /// <value>The referer.</value>
        public string Referer
        {
            get
            {
                return m_Referer;
            }
            set
            {
                m_Referer = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Uri _LastProcessResponseUri;

        /// <summary>
        /// Gets or sets the last process response URI.
        /// </summary>
        /// <value>The last process response URI.</value>
        public Uri LastProcessResponseUri
        {
            get
            {
                return _LastProcessResponseUri;
            }
            set
            {
                _LastProcessResponseUri = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string LastMetaFollow;

        /// <summary>
        /// 
        /// </summary>
        private ScrapeType? _MetaRefreshScrapeType;

        /// <summary>
        /// If there is a meta refresh, then this specified
        /// the scrape type to use to follow the link. If this
        /// value is null, then the scrape type of the previous request
        /// is used.
        /// </summary>
        /// <value>The type of the meta refresh scrape.</value>
        public ScrapeType? MetaRefreshScrapeType
        {
            get
            {
                return _MetaRefreshScrapeType;
            }
            set
            {
                _MetaRefreshScrapeType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Domain;

        /// <summary>
        /// This is used for a requests referer attribute as well as setting any cookies.
        /// This should be in the form "www.domain.com," without the prepended scheme.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain
        {
            get
            {
                return _Domain;
            }
            set
            {
                if (value != null && value.Contains("/"))
                {
                    throw new ArgumentException("The domain must not include a scheme (e.g. http), or a trailing slash ('/').");
                }
                _Domain = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scraper"/> class.
        /// </summary>
        public Scraper()
            : this(null)
        {
        }

        /// <summary>
        /// This is used for a requests referer attribute as well as setting any cookies.
        /// This should be in the form "www.domain.com," without the prepended scheme.
        /// </summary>
        /// <param name="domain">The domain.</param>
        public Scraper(string domain)
        {
            this.Domain = domain;
            m_Session = new ScrapeSession(this);
            if (this.Domain != null)
            {
                Referer = "http://" + this.Domain;
            }
        }

        /// <summary>
        /// Scrapes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="keyAndValuePairs">The key and value pairs.</param>
        /// <returns></returns>
        public ScrapedPage Scrape(ScrapeType type, string uri, params string[] keyAndValuePairs)
        {
            NameValueCollection query = new NameValueCollection();
            if (keyAndValuePairs != null)
            {
                for (int i = 0; i < keyAndValuePairs.Length; i += 2)
                {
                    query[keyAndValuePairs[i]] = keyAndValuePairs[i + 1];
                }
            }
            return Scrape(type, uri, query);
        }

        /// <summary>
        /// Scrapes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public ScrapedPage Scrape(ScrapeType type, string uri, NameValueCollection query)
        {
            ScrapedPage page = new ScrapedPage();
            string qs = BuildQueryString(query);
            page.QueryParameters = query;
            page.ScrapeType = type;
            switch (type)
            {
                case ScrapeType.GET:
                    uri = uri.Contains("?") ? (uri + "&" + qs) : (uri + "?" + qs);
                    page.RawStream = HttpGet(uri);
                    break;
                case ScrapeType.POST:
                    page.RawStream = HttpPost(uri, qs);
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (page.RawStream == null)
            {
                throw new Exception("No data for " + uri);
            }
            else
            {
                page.Url = new Uri(uri);
                Referer = uri;

                page = PostProcessData(page);
            }
            return page;
        }

        /// <summary>
        /// Posts the process data.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        private ScrapedPage PostProcessData(ScrapedPage page)
        {
            if (FollowEquivRefreshes)
            {
                bool followed = false;
                // See if we can find an http-equiv refresh
                IList<ScreenScraperTag> metaTags = ScrapedPage.ConvertToTagList(page.FindChildlessTags("meta", false), true);

                // Now, we have all META tags. Try to find one with HTTP-EQUIV="refresh"
                ScreenScraperTag refreshTag = null;
                foreach (ScreenScraperTag metaTag in metaTags)
                {
                    string httpEquivValue = metaTag.FindAttributeValue("http-equiv");
                    if (httpEquivValue != null && httpEquivValue.Equals("refresh"))
                    {
                        refreshTag = metaTag;
                        break;
                    }
                }
                if (refreshTag != null)
                {
                    // It's a refresh. Try to figure out the URL we have to go to.
                    string contentValue = refreshTag.FindAttributeValue("content");
                    if (contentValue != null)
                    {
                        // First, split it by semicolon
                        string[] refreshPieces = contentValue.Split(';');
                        string url = null;
                        int time = 0;
                        foreach (string refreshPiece in refreshPieces)
                        {
                            if (refreshPiece.ToLower().Trim().StartsWith("url"))
                            {
                                // found the URL. Just take everything after the =
                                int equalPos = refreshPiece.IndexOf('=');
                                if (equalPos != -1)
                                {
                                    url = refreshPiece.Substring(equalPos + 1).Trim();
                                    break;
                                }
                            }
                            else if (time == 0)
                            {
                                int.TryParse(refreshPiece.Trim(), out time);
                            }
                        }
                        if (time == 0 && url != null)
                        {
                            // We have a refresh url, so we need to update the page

                            // If it is a relative url, make it absolute.
                            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                            {
                                url = LastProcessResponseUri.GetLeftPart(UriPartial.Authority) + "/" + (url.StartsWith("/") ? url.Substring(1) : url);
                            }

                            if (!url.Equals(LastMetaFollow))
                            {
                                page = Scrape(MetaRefreshScrapeType == null ? page.ScrapeType : MetaRefreshScrapeType.Value, url, page.QueryParameters);
                                LastMetaFollow = url;
                                followed = true;
                            }
                            else
                            {
                                throw new Exception("Appears to be a recursive loop of http-equiv redirects to the same page ('" + url + "').");
                            }
                        }
                    }
                }

                if (!followed)
                {
                    LastMetaFollow = null;
                }
            }
            return page;
        }

        /// <summary>
        /// Builds the query string.
        /// </summary>
        /// <param name="keyAndValuePairs">The key and value pairs.</param>
        /// <returns></returns>
        public static string BuildQueryString(params string[] keyAndValuePairs)
        {
            NameValueCollection query = new NameValueCollection();
            if (keyAndValuePairs != null)
            {
                for (int i = 0; i < keyAndValuePairs.Length; i += 2)
                {
                    query[keyAndValuePairs[i]] = keyAndValuePairs[i + 1];
                }
            }
            return BuildQueryString(query);
        }

        /// <summary>
        /// Builds the query string.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public static string BuildQueryString(NameValueCollection query)
        {
            string ret = "";
            foreach (string key in query.AllKeys)
            {
                if (!ret.Equals(string.Empty))
                {
                    ret += "&";
                }
                ret += key + "=" + HttpUtility.UrlEncode(query[key]);
            }
            return ret;
        }

        /// <summary>
        /// HTTPs the get.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public string HttpGet(string uri)
        {
            System.Net.HttpWebRequest req = CreateWebRequest(uri);
            return ProcessResponseStream(req);
        }

        /// <summary>
        /// HTTPs the post.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public string HttpPost(string uri, string parameters)
        {
            System.Net.HttpWebRequest req = CreateWebRequest(uri);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(parameters);
            req.ContentLength = bytes.Length;
            using (System.IO.Stream os = req.GetRequestStream())
            {
                os.Write(bytes, 0, bytes.Length); //Push it out there
            }
            return ProcessResponseStream(req);
        }

        /// <summary>
        /// Processes the response stream.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        private string ProcessResponseStream(System.Net.HttpWebRequest req)
        {
            using (System.Net.WebResponse resp = req.GetResponse())
            {
                if (resp == null) return null;
                // Update the domain we're now on
                Domain = resp.ResponseUri.Authority;
                LastProcessResponseUri = resp.ResponseUri;
                using (Stream stream = resp.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                    {
                        return sr.ReadToEnd().Trim();
                    }
                }
            }
        }

        /// <summary>
        /// Creates the web request.
        /// </summary>
        /// <param name="URI">The URI.</param>
        /// <returns></returns>
        private HttpWebRequest CreateWebRequest(string URI)
        {
            HttpWebRequest req = (HttpWebRequest)System.Net.WebRequest.Create(URI);
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322)";
            if (Domain != null)
            {
                req.Referer = Referer;
            }
            req.CookieContainer = Session.Cookies;
            req.AllowAutoRedirect = true;
            return req;
        }
    }
#endif

    /// <summary>
    /// Codes for the representation of names of countries and their subdivisions.
    /// http://en.wikipedia.org/wiki/ISO_3166
    /// </summary>
    [Serializable]
    public struct Iso3166
    {
        /// <summary>
        /// 
        /// </summary>
        public string TwoLetterCode;

        /// <summary>
        /// 
        /// </summary>
        public string CountryName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Iso3166"/> class.
        /// </summary>
        /// <param name="twoLetterCode">The two letter code.</param>
        /// <param name="countryName">Name of the country.</param>
        public Iso3166(string twoLetterCode, string countryName)
        {
            TwoLetterCode = twoLetterCode;
            CountryName = countryName;
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return TwoLetterCode;
        }
    }

    /// <summary>
    /// Represents a latitude and longitude.
    /// </summary>
    [Serializable]
    public struct Iso6709
    {
        /// <summary>
        /// 
        /// </summary>
        public int LatitudeDegrees;

        /// <summary>
        /// 
        /// </summary>
        public int LatitudeMinutes;

        /// <summary>
        /// 
        /// </summary>
        public int LatitudeSeconds;

        /// <summary>
        /// 
        /// </summary>
        public bool IsLatitudeNorth;

        /// <summary>
        /// 
        /// </summary>
        public int LongitudeDegrees;

        /// <summary>
        /// 
        /// </summary>
        public int LongitudeMinutes;

        /// <summary>
        /// 
        /// </summary>
        public int LongitudeSeconds;

        /// <summary>
        /// 
        /// </summary>
        public bool IsLongitudeEast;

        /// <summary>
        /// 
        /// </summary>
        public static Regex Iso6709Form1 = new Regex(@"(\+|-)(\d\d)(\d\d)(\+|-)(\d\d\d)(\d\d)", RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public static Regex Iso6709Form2 = new Regex(@"(\+|-)(\d\d)(\d\d)(\d\d)(\+|-)(\d\d\d)(\d\d)(\d\d)", RegexOptions.Compiled);

        /// <summary>
        /// Parses the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static Iso6709 Parse(string str)
        {
            Iso6709 result = new Iso6709();
            Match m = Iso6709Form1.Match(str);
            if (m.Success)
            {
                result.IsLatitudeNorth = m.Groups[1].ToString()[0] == '+';
                result.LatitudeDegrees = int.Parse(m.Groups[2].ToString());
                result.LatitudeMinutes = int.Parse(m.Groups[3].ToString());
                result.IsLongitudeEast = m.Groups[4].ToString()[0] == '+';
                result.LongitudeDegrees = int.Parse(m.Groups[5].ToString());
                result.LongitudeMinutes = int.Parse(m.Groups[6].ToString());
            }
            else
            {
                m = Iso6709Form2.Match(str);
                if (m.Success)
                {
                    result.IsLatitudeNorth = m.Groups[1].ToString()[0] == '+';
                    result.LatitudeDegrees = int.Parse(m.Groups[2].ToString());
                    result.LatitudeMinutes = int.Parse(m.Groups[3].ToString());
                    result.LatitudeSeconds = int.Parse(m.Groups[4].ToString());
                    result.IsLongitudeEast = m.Groups[5].ToString()[0] == '+';
                    result.LongitudeDegrees = int.Parse(m.Groups[6].ToString());
                    result.LongitudeMinutes = int.Parse(m.Groups[7].ToString());
                    result.LongitudeSeconds = int.Parse(m.Groups[8].ToString());
                }
                else
                {
                    // Couldn't match a known format
                    throw new FormatException("ISO 6709 format expected, found " + str);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            if (LatitudeSeconds != 0 && LongitudeSeconds != 0)
            {
                return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                    IsLatitudeNorth ? '+' : '-',
                    StringUtilities.PadIntegerLeft(LatitudeDegrees, 2),
                    StringUtilities.PadIntegerLeft(LatitudeMinutes, 2),
                    StringUtilities.PadIntegerLeft(LatitudeSeconds, 2),
                    IsLongitudeEast ? '+' : '-',
                    StringUtilities.PadIntegerLeft(LongitudeDegrees, 3),
                    StringUtilities.PadIntegerLeft(LongitudeMinutes, 2),
                    StringUtilities.PadIntegerLeft(LongitudeSeconds, 2)
                );
            }
            else
            {
                return string.Format("{0}{1}{2}{3}{4}{5}",
                    IsLatitudeNorth ? '+' : '-',
                    StringUtilities.PadIntegerLeft(LatitudeDegrees, 2),
                    StringUtilities.PadIntegerLeft(LatitudeMinutes, 2),
                    IsLongitudeEast ? '+' : '-',
                    StringUtilities.PadIntegerLeft(LongitudeDegrees, 3),
                    StringUtilities.PadIntegerLeft(LongitudeMinutes, 2)
                );
            }
        }
    }

    /// <summary>
    /// Generic representation of a latitude and longitude point.
    /// </summary>
    [Serializable]
    public class LatitudeLongitudePoint
    {
        /// <summary>
        /// 
        /// </summary>
        public double m_latitude;

        /// <summary>
        /// 
        /// </summary>
        public double m_longitude;

        /// <summary>
        /// Initializes a new instance of the <see cref="LatitudeLongitudePoint"/> class.
        /// </summary>
        public LatitudeLongitudePoint()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LatitudeLongitudePoint"/> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public LatitudeLongitudePoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LatitudeLongitudePoint"/> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public LatitudeLongitudePoint(string latitude, string longitude)
        {
            Latitude = double.Parse(latitude);
            Longitude = double.Parse(longitude);
        }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude
        {
            get
            {
                return m_latitude;
            }
            set
            {
                m_latitude = value;
            }
        }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude
        {
            get
            {
                return m_longitude;
            }
            set
            {
                m_longitude = value;
            }
        }

        /// <summary>
        /// Gets or sets the latitude decimal.
        /// </summary>
        /// <value>The latitude decimal.</value>
        public Decimal LatitudeDecimal
        {
            get
            {
                return Convert.ToDecimal(Latitude);
            }
            set
            {
                Latitude = Decimal.ToDouble(value);
            }
        }

        /// <summary>
        /// Gets or sets the longitude decimal.
        /// </summary>
        /// <value>The longitude decimal.</value>
        public Decimal LongitudeDecimal
        {
            get
            {
                return Convert.ToDecimal(Longitude);
            }
            set
            {
                Longitude = Decimal.ToDouble(value);
            }
        }

        /// <summary>
        /// Procedure for converting degrees, minutes, seconds into decimal degrees:
        /// Degrees, minutes, seconds value: 37 degrees, 25 minutes, 40.5 seconds
        /// 1. Decimal degrees = degrees + (minutes/60) + (seconds/3600)
        /// 2. 37 degrees, 25 minutes, 40.5 seconds = 37. + (25/60) + (40.5/3600)
        /// 3. 37. + .416666 + .01125
        /// 4. So 37 degrees, 25 minutes, 40.5 seconds = 37.427916 in decimal degrees.
        /// </summary>
        /// <value>The latitude degrees.</value>
        public int LatitudeDegrees
        {
            get
            {
                return (int)Latitude;
            }
        }

        /// <summary>
        /// Gets the latitude minutes.
        /// </summary>
        /// <value>The latitude minutes.</value>
        public int LatitudeMinutes
        {
            get
            {
                return (int)((Latitude - (double)LatitudeDegrees) * 60D);
            }
        }

        /// <summary>
        /// Gets the latitude seconds.
        /// </summary>
        /// <value>The latitude seconds.</value>
        public double LatitudeSeconds
        {
            get
            {
                return (((Latitude - (double)LatitudeDegrees) * 60D) - (double)LatitudeMinutes) * 60D;
            }
        }

        /// <summary>
        /// Gets the longitude degrees.
        /// </summary>
        /// <value>The longitude degrees.</value>
        public int LongitudeDegrees
        {
            get
            {
                return (int)Longitude;
            }
        }

        /// <summary>
        /// Gets the longitude minutes.
        /// </summary>
        /// <value>The longitude minutes.</value>
        public int LongitudeMinutes
        {
            get
            {
                return (int)((Longitude - (double)LongitudeDegrees) * 60D);
            }
        }

        /// <summary>
        /// Gets the longitude seconds.
        /// </summary>
        /// <value>The longitude seconds.</value>
        public double LongitudeSeconds
        {
            get
            {
                return (((Longitude - (double)LongitudeDegrees) * 60D) - (double)LongitudeMinutes) * 60D;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return "(" + Latitude + "," + Longitude + ")";
        }

        /// <summary>
        /// Parses the specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns></returns>
        public static LatitudeLongitudePoint Parse(string latitude, string longitude)
        {
            if (!(latitude == null || longitude == null || latitude.ToLower() == "na" || longitude.ToLower() == "na"))
            {
                LatitudeLongitudePoint ret = new LatitudeLongitudePoint();
                Regex r = new Regex(@"(\d+)\.(\d+)(\.(\d+))?(\w)");
                double degrees, minutes, seconds = 0;
                Match m = r.Match(latitude);
                if (m.Success)
                {
                    degrees = double.Parse(m.Groups[1].Value);
                    minutes = double.Parse(m.Groups[2].Value);
                    seconds = m.Groups[3].Success ? double.Parse(m.Groups[3].Value) : 0;
                    ret.Latitude = degrees + (minutes / 60D) + (seconds / 3600D);
                    if (m.Groups[5].Value.ToLower() == "s")
                        ret.Latitude *= -1;
                }
                else
                    throw new Exception("Unsupported parse type!");
                m = r.Match(longitude);
                if (m.Success)
                {
                    degrees = double.Parse(m.Groups[1].Value);
                    minutes = double.Parse(m.Groups[2].Value);
                    seconds = m.Groups[3].Success ? double.Parse(m.Groups[3].Value) : 0;
                    ret.Longitude = degrees + (minutes / 60D) + (seconds / 3600D);
                    if (m.Groups[5].Value.ToLower() == "w")
                        ret.Longitude *= -1;
                }
                else
                    throw new Exception("Unsupported parse type!");
                return ret;
            }
            return null;
        }

        /// <summary>
        /// Returns the distance between two latitude/longitude points, in miles.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double DistanceBetween(LatitudeLongitudePoint point1, LatitudeLongitudePoint point2)
        {
            return DistanceBetween(point1, point2, DistanceType.StatuteMiles);
        }

        /// <summary>
        /// Distances the between.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <returns></returns>
        public static double DistanceBetween(LatitudeLongitudePoint point1, LatitudeLongitudePoint point2, DistanceType returnType)
        {
            // see http://www.mathforum.com/library/drmath/view/51711.html
            // TODO is this really correct, or do I have to first convert decimal to degrees and then to radians?
            if (point1.Latitude == point2.Latitude) return 0;
            double a = point1.Latitude / 57.29577951D;
            double b = point1.Longitude / 57.29577951D;
            double c = point2.Latitude / 57.29577951D;
            double d = point2.Longitude / 57.29577951D;

            double earthRadius = (returnType == DistanceType.StatuteMiles ? GlobalConstants.EarthRadiusStatuteMiles : (returnType == DistanceType.NauticalMiles ? GlobalConstants.EarthRadiusNauticalMiles : GlobalConstants.EarthRadiusKilometers));

            double sina = Math.Sin(a);
            double sinc = Math.Sin(c);
            double cosa = Math.Cos(a);
            double cosc = Math.Cos(c);
            double cosbd = Math.Cos(b - d);

            double ans1 = ((sina * sinc) + (cosa * cosc * cosbd));
            if (ans1 > 1D)
            {
                return earthRadius * Math.Acos(1D);
            }
            else
            {
                return earthRadius * Math.Acos(ans1);
            }
        }
    }

    /// <summary>
    /// Methods to help in date and time manipulation.
    /// </summary>
    public static class DateTimeUtlities
    {
        /// <summary>
        /// Parses the month.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static Month ParseMonth(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            if (str.Length < 3)
            {
                throw new ArgumentException("Month should be at least 3 characters (" + str + ")");
            }
            str = str.ToLower().Trim().Substring(0, 3);
            switch (str)
            {
                case "jan":
                    return Month.January;
                case "feb":
                    return Month.February;
                case "mar":
                    return Month.March;
                case "apr":
                    return Month.April;
                case "may":
                    return Month.May;
                case "jun":
                    return Month.June;
                case "jul":
                    return Month.July;
                case "aug":
                    return Month.August;
                case "sep":
                    return Month.September;
                case "oct":
                    return Month.October;
                case "nov":
                    return Month.November;
                case "dec":
                    return Month.December;
                default:
                    throw new DateException("Unknown month " + str);
            }
        }

        /// <summary>
        /// Parses the day of week.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static DayOfWeek ParseDayOfWeek(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            if (str.Length < 3)
            {
                throw new ArgumentException("Day of week should be at least 3 characters (" + str + ")");
            }
            str = str.ToLower().Trim().Substring(0, 3);
            switch (str)
            {
                case "sun":
                    return DayOfWeek.Sunday;
                case "mon":
                    return DayOfWeek.Monday;
                case "tue":
                    return DayOfWeek.Tuesday;
                case "wed":
                    return DayOfWeek.Wednesday;
                case "thu":
                    return DayOfWeek.Thursday;
                case "fri":
                    return DayOfWeek.Friday;
                case "sat":
                    return DayOfWeek.Saturday;
                default:
                    throw new DateException("Unknown day of week " + str);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class DateException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DateException"/> class.
            /// </summary>
            public DateException() { }
            
            /// <summary>
            /// Initializes a new instance of the <see cref="DateException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            public DateException(string message) : base(message) { }
            
            /// <summary>
            /// Initializes a new instance of the <see cref="DateException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="inner">The inner.</param>
            public DateException(string message, Exception inner) : base(message, inner) { }
            
            /// <summary>
            /// Initializes a new instance of the <see cref="DateException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
            protected DateException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }

#if !(NOTZ)
    // From http://www.twinsun.com/tz/tz-link.htm
    // "The public-domain time zone database contains code
    // and data that represent the history of local time
    // for many representative locations around the globe."


    /// <summary>
    /// Represents a Time Zone from the Olson tz database.
    /// </summary>
    [Serializable]
    public class TzTimeZone : TimeZone
    {
        /// <summary>
        /// 
        /// </summary>
        public const string TimzoneUsEastern = "US/Eastern";

        /// <summary>
        /// 
        /// </summary>
        public const string TimezoneUsCentral = "US/Central";

        /// <summary>
        /// 
        /// </summary>
        public const string TimezoneUsMountain = "US/Mountain";

        /// <summary>
        /// 
        /// </summary>
        public const string TimezoneUsPacific = "US/Pacific";

        private string m_standardName;
        private string m_daylightName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TzTimeZone"/> class.
        /// </summary>
        /// <param name="standardName">Name of the standard.</param>
        public TzTimeZone(string standardName)
        {
            m_standardName = standardName;
            m_daylightName = standardName;
        }

        /// <summary>
        /// Gets the standard time zone name.
        /// </summary>
        /// <value></value>
        /// <returns>The standard time zone name.</returns>
        /// <exception cref="T:System.ArgumentNullException">Attempted to set this property to null. </exception>
        public override string StandardName
        {
            get
            {
                return m_standardName;
            }
        }

        /// <summary>
        /// Gets the daylight saving time zone name.
        /// </summary>
        /// <value></value>
        /// <returns>The daylight saving time zone name.</returns>
        public override string DaylightName
        {
            get
            {
                return m_daylightName;
            }
        }

        /// <summary>
        /// Returns the daylight saving time period for a particular year.
        /// </summary>
        /// <param name="year">The year to which the daylight saving time period applies.</param>
        /// <returns>
        /// A <see cref="T:System.Globalization.DaylightTime"></see> instance containing the start and end date for daylight saving time in year.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">year is less than 1 or greater than 9999. </exception>
        public override DaylightTime GetDaylightChanges(int year)
        {
            return null;
        }

        /// <summary>
        /// Returns the coordinated universal time (UTC) offset for the specified local time.
        /// </summary>
        /// <param name="time">The local date and time.</param>
        /// <returns>
        /// The UTC offset from time, measured in ticks.
        /// </returns>
        public override TimeSpan GetUtcOffset(DateTime time)
        {
            return new TimeSpan();
        }

        /// <summary>
        /// Returns a value indicating whether the specified date and time is within a daylight saving time period.
        /// </summary>
        /// <param name="time">A date and time.</param>
        /// <returns>
        /// true if time is in a daylight saving time period; false otherwise, or if time is null.
        /// </returns>
        public override bool IsDaylightSavingTime(DateTime time)
        {
            return base.IsDaylightSavingTime(time);
        }

        /// <summary>
        /// Returns the local time that corresponds to a specified coordinated universal time (UTC).
        /// </summary>
        /// <param name="time">A UTC time.</param>
        /// <returns>
        /// A <see cref="T:System.DateTime"></see> instance whose value is the local time that corresponds to time.
        /// </returns>
        public override DateTime ToLocalTime(DateTime time)
        {
            return base.ToLocalTime(time);
        }

        /// <summary>
        /// Returns the coordinated universal time (UTC) that corresponds to a specified local time.
        /// </summary>
        /// <param name="time">The local date and time.</param>
        /// <returns>
        /// A <see cref="T:System.DateTime"></see> instance whose value is the UTC time that corresponds to time.
        /// </returns>
        public override DateTime ToUniversalTime(DateTime time)
        {
            return base.ToUniversalTime(time);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }

    /// <summary>
    /// Wraps DateTime to provide time zone information
    /// with an <see cref="PublicDomain.TzTimeZone" /> from
    /// the Olson tz database.
    /// </summary>
    [Serializable]
    public class TzDateTime
    {
        internal static bool TryParseLenient(string subject, TzTimeZone timeZone, DateTimeStyles dateTimeStyles, out TzDateTime ret)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

#if !(NOTZPARSER)

    /// <summary>
    /// Parses the tz database files
    /// </summary>
#if !(NONUNIT)
    [TestFixture]
#endif
    public class TzParser
    {
        private const string Iso3166TabFile = @"C:\temp\tzdata\iso3166.tab";
        private const string ZoneTabFile = @"C:\temp\tzdata\zone.tab";

        /// <summary>
        /// Parse3166s the tab.
        /// </summary>
#if !(NONUNIT)
        [Test]
#endif
        public void Parse3166Tab()
        {
            Dictionary<string, Iso3166> map = ParseIso3166Tab(Iso3166TabFile);
            foreach (string key in map.Keys)
            {
                Iso3166 data = map[key];
                Console.WriteLine(data);
            }
        }

        /// <summary>
        /// Parses the zone tab.
        /// </summary>
#if !(NONUNIT)
        [Test]
#endif
        public void ParseZoneTab()
        {
            List<TzZoneDescription> items = ParseZoneTab(ZoneTabFile);
            foreach (TzZoneDescription item in items)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// Reads the database.
        /// </summary>
#if !(NONUNIT)
        [Test]
#endif
        public void ReadDatabase()
        {
            // First, get all of the data from the tz db
            List<TzDataRule> rules = new List<TzDataRule>();
            List<TzDataZone> zones = new List<TzDataZone>();
            ReadDatabase(@"c:\temp\tzdata\", rules, zones);

            // Now, get all of the tab files
            Dictionary<string, Iso3166> map = ParseIso3166Tab(Iso3166TabFile);
            List<TzZoneDescription> items = ParseZoneTab(ZoneTabFile);

            // Now, we can start combining all of this information into TzZone objects

        }

        /// <summary>
        /// Reads the database.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="rules">The rules.</param>
        /// <param name="zones">The zones.</param>
        public static void ReadDatabase(string dir, List<TzDataRule> rules, List<TzDataZone> zones)
        {
            if (dir == null)
            {
                throw new ArgumentNullException("dir");
            }
            List<string[]> links = new List<string[]>();
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            FileInfo[] files = dirInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                if (string.IsNullOrEmpty(file.Extension))
                {
                    ReadDatabaseFile(file, rules, zones, links);
                }
            }

            foreach (string[] pieces in links)
            {
                // Find the time zone this is a link to
                TzDataZone linkedTo = FindTzDataZone(zones, pieces[1]);

                // Clone it and add it to the list
                TzDataZone link = (TzDataZone)linkedTo.Clone();
                link.ZoneName = pieces[2];

                zones.Add(link);
            }
        }

        /// <summary>
        /// Reads the database file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="rules">The rules.</param>
        /// <param name="zones">The zones.</param>
        /// <param name="links">The links.</param>
        /// <exception cref="PublicDomain.TzException"/>
        private static void ReadDatabaseFile(FileInfo file, List<TzDataRule> rules, List<TzDataZone> zones, List<string[]> links)
        {
            string[] lines = System.IO.File.ReadAllLines(file.FullName);
            foreach (string line in lines)
            {
                // This line may be a continuation Zone
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.TrimStart()[0] != '#')
                    {
                        if (char.IsWhiteSpace(line[0]) && zones.Count > 0)
                        {
                            if (line.Trim() != string.Empty)
                            {
                                // This is a continuation of a previous Zone
                                zones.Add(zones[zones.Count - 1].Clone(line));
                            }
                        }
                        else
                        {
                            string[] pieces = line.Split('\t');
                            pieces = StringUtilities.Split(pieces, ' ', 0);
                            if (pieces.Length > 0)
                            {
                                switch (pieces[0].ToLower())
                                {
                                    case "rule":
                                        rules.Add(TzDataRule.Parse(line));
                                        break;
                                    case "zone":
                                        zones.Add(TzDataZone.Parse(line));
                                        break;
                                    case "link":
                                        links.Add(StringUtilities.RemoveEmptyPieces(pieces));
                                        break;
                                    case "leap":
                                        // Not yet handled
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static TzDataZone FindTzDataZone(List<TzDataZone> zones, string zoneName)
        {
            foreach (TzDataZone zone in zones)
            {
                if (zone.ZoneName.Equals(zoneName))
                {
                    return zone;
                }
            }
            throw new TzParseException("Could not find LINKed zone " + zoneName);
        }

        /// <summary>
        /// Logical representation of a ZONE data field in the tz database.
        /// </summary>
        [Serializable]
        public class TzDataZone : ICloneable
        {
            /// <summary>
            /// Unique name for each zone, for example America/New_York, though
            /// there can be multiple zones with the same name but different properties.
            /// </summary>
            public string ZoneName;

            /// <summary>
            /// Offset from UTC
            /// </summary>
            public TimeSpan UtcOffset;

            /// <summary>
            /// The rule that applies to this zone.
            /// </summary>
            public string RuleName;

            /// <summary>
            /// 
            /// </summary>
            public string Format;

            /// <summary>
            /// 
            /// </summary>
            public int UntilYear;

            /// <summary>
            /// 
            /// </summary>
            public Month UntilMonth = 0;

            /// <summary>
            /// 
            /// </summary>
            public int UntilDay;

            /// <summary>
            /// 
            /// </summary>
            public DayOfWeek? UntilDay_DayOfWeek;

            /// <summary>
            /// 
            /// </summary>
            public TimeSpan UntilTime;

            /// <summary>
            /// 
            /// </summary>
            public string Comment;

            /// <summary>
            /// Parses the specified STR.
            /// </summary>
            /// <param name="str">The STR.</param>
            /// <returns></returns>
            public static TzDataZone Parse(string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    throw new ArgumentNullException("str");
                }
                TzDataZone z = new TzDataZone();
                ParsePieces(str, z);
                return z;
            }

            /// <summary>
            /// Parses the pieces.
            /// </summary>
            /// <param name="str">The STR.</param>
            /// <param name="z">The z.</param>
            private static void ParsePieces(string str, TzDataZone z)
            {
                string[] pieces = str.Split('\t');
                if (pieces[0].Contains(" ") || pieces[1].Contains(" ") || pieces[2].Contains(" "))
                {
                    pieces = StringUtilities.Split(pieces, ' ', 0, 1, 2);
                }
                pieces = StringUtilities.RemoveEmptyPieces(pieces);
                z.ZoneName = pieces[1];

                if (z.ZoneName != "Factory")
                {
                    z.UtcOffset = TimeSpan.Parse(pieces[2]);
                    z.RuleName = pieces[3];
                    z.Format = pieces[4];

                    // The rest of the format is optional an erratic, so we combine
                    // the rest of the array into a big string
                    if (pieces.Length > 5)
                    {
                        string ending = String.Join(" ", pieces, 5, pieces.Length - 5);
                        int poundIndex;
                        if ((poundIndex = ending.IndexOf('#')) != -1)
                        {
                            z.Comment = ending.Substring(poundIndex + 1).Trim();
                            ending = ending.Substring(0, poundIndex).Trim();
                            if (ending != "")
                            {
                                string[] untilPieces = StringUtilities.RemoveEmptyPieces(ending.Split(' '));
                                z.UntilYear = int.Parse(untilPieces[0]);
                                if (untilPieces.Length > 1)
                                {
                                    z.UntilMonth = DateTimeUtlities.ParseMonth(untilPieces[1]);
                                    if (untilPieces.Length > 2)
                                    {
                                        int untilDay;
                                        DayOfWeek? untilDay_dayOfWeek;
                                        GetTzDataDay(untilPieces[2], out untilDay, out untilDay_dayOfWeek);
                                        z.UntilDay = untilDay;
                                        z.UntilDay_DayOfWeek = untilDay_dayOfWeek;

                                        if (untilPieces.Length > 3)
                                        {
                                            z.UntilTime = GetTzDataTime(untilPieces[3]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Creates a new object that is a copy of the current instance.
            /// </summary>
            /// <returns>
            /// A new object that is a copy of this instance.
            /// </returns>
            public object Clone()
            {
                return (TzDataZone)MemberwiseClone();
            }

            /// <summary>
            /// Clones the specified line.
            /// </summary>
            /// <param name="line">The line.</param>
            /// <returns></returns>
            public TzDataZone Clone(string line)
            {
                TzDataZone z = (TzDataZone)Clone();
                line = "Zone\t" + z.ZoneName + "\t" + string.Join("\t", StringUtilities.RemoveEmptyPieces(line.Split('\t')));
                ParsePieces(line, z);
                return z;
            }
        }

        /// <summary>
        /// Logical representation of a RULE field in the tz database.
        /// </summary>
        [Serializable]
        public class TzDataRule
        {
            /// <summary>
            /// Each Zone rule has a name which each zone refers to.
            /// A zone rule name is not unique in its own right, but
            /// only with all of its properties. Therefore, there may
            /// be multiple zone rules with the same name but different properties.
            /// </summary>
            public string RuleName;

            /// <summary>
            /// The effective year the zone rule is effective on.
            /// </summary>
            public int From;

            /// <summary>
            /// The year the zone rule effectively ends.
            /// </summary>
            public int To;

            /// <summary>
            /// The integer month the rule starts on. January = 1, February = 2, ..., December = 12
            /// </summary>
            public Month StartMonth;

            /// <summary>
            /// The day of the month the rule starts on.
            /// </summary>
            public int StartDay;

            /// <summary>
            /// The day of the month is optionally modified by
            /// a day of week.
            /// </summary>
            public DayOfWeek? StartDay_DayOfWeek;

            /// <summary>
            /// The time of the day the Rule starts on.
            /// </summary>
            public TimeSpan StartTime;

            /// <summary>
            /// The amount of time saved by the Rule.
            /// </summary>
            public TimeSpan SaveTime;

            /// <summary>
            /// 
            /// </summary>
            public string Modifier;

            /// <summary>
            /// 
            /// </summary>
            public string Comment;

            /// <summary>
            /// Parses the specified STR.
            /// </summary>
            /// <param name="str">The STR.</param>
            /// <returns></returns>
            public static TzDataRule Parse(string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    throw new ArgumentNullException("str");
                }
                TzDataRule t = new TzDataRule();
                string[] pieces = str.Split('\t');
                pieces = StringUtilities.Split(pieces, ' ', 0);
                if (pieces.Length == 11 && pieces[10][0] == '#')
                {
                    // the comment was tabbed off the end
                    string comment = pieces[10];
                    Array.Resize<string>(ref pieces, 10);
                    pieces[9] += " " + comment;
                }
                if (pieces.Length != 10)
                {
                    throw new TzParseException("Rule has an invalid number of pieces: " + pieces.Length + " (" + str + ")");
                }
                t.RuleName = pieces[1];
                if (pieces[2] == "min")
                {
                    t.From = 0;
                }
                else
                {
                    t.From = int.Parse(pieces[2]);
                }
                switch (pieces[3])
                {
                    case "only":
                        t.To = t.From;
                        break;
                    case "max":
                        t.To = int.MaxValue;
                        break;
                    default:
                        t.To = int.Parse(pieces[3]);
                        break;
                }
                t.StartMonth = DateTimeUtlities.ParseMonth(pieces[5]);

                int startDay;
                DayOfWeek? startDay_dayOfWeek;
                GetTzDataDay(pieces[6], out startDay, out startDay_dayOfWeek);
                t.StartDay = startDay;
                t.StartDay_DayOfWeek = startDay_dayOfWeek;

                t.StartTime = GetTzDataTime(pieces[7]);
                t.SaveTime = TimeSpan.Parse(pieces[8]);
                t.Modifier = pieces[9];
                return t;
            }
        }

        /// <summary>
        /// Parses the tz database iso3166.tab file and returns a map
        /// which maps the ISO 3166 two letter country code to the
        /// country name.
        /// </summary>
        /// <param name="iso3166TabFile">The iso3166 tab file.</param>
        /// <returns></returns>
        public static Dictionary<string, Iso3166> ParseIso3166Tab(string iso3166TabFile)
        {
            Dictionary<string, Iso3166> map = new Dictionary<string, Iso3166>();
            string[] lines = System.IO.File.ReadAllLines(iso3166TabFile);
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line) && line[0] != '#')
                {
                    // We expect two characters for the country code,
                    // followed by the country name
                    Iso3166 iso = new Iso3166(line.Substring(0, 2), line.Substring(2).Trim());
                    map[iso.TwoLetterCode] = iso;
                }
            }
            return map;
        }

        /// <summary>
        /// Parses the tz database zone.tab file into all the zone descriptions.
        /// </summary>
        /// <param name="tabFile"></param>
        /// <returns></returns>
        public static List<TzZoneDescription> ParseZoneTab(string tabFile)
        {
            List<TzZoneDescription> result = new List<TzZoneDescription>();
            string[] lines = System.IO.File.ReadAllLines(tabFile);
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line) && line[0] != '#')
                {
                    string[] pieces = line.Split('\t');
                    result.Add(new TzZoneDescription(pieces[0], Iso6709.Parse(pieces[1]), pieces[2], pieces.Length > 3 ? pieces[3] : null));
                }
            }
            return result;
        }

        /// <summary>
        /// Logical zone description taken from the zone tab file in the tz database.
        /// </summary>
        [Serializable]
        public struct TzZoneDescription
        {
            /// <summary>
            /// 
            /// </summary>
            public string TwoLetterCode;

            /// <summary>
            /// 
            /// </summary>
            public Iso6709 Location;

            /// <summary>
            /// 
            /// </summary>
            public string ZoneName;

            /// <summary>
            /// 
            /// </summary>
            public string Comments;

            /// <summary>
            /// Initializes a new instance of the <see cref="TzZoneDescription"/> class.
            /// </summary>
            /// <param name="twoLetterCode">The two letter code.</param>
            /// <param name="location">The location.</param>
            /// <param name="zoneName">Name of the zone.</param>
            /// <param name="comments">The comments.</param>
            public TzZoneDescription(string twoLetterCode, Iso6709 location, string zoneName, string comments)
            {
                TwoLetterCode = twoLetterCode;
                Location = location;
                ZoneName = zoneName;
                Comments = comments;
            }

            /// <summary>
            /// Returns the fully qualified type name of this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> containing a fully qualified type name.
            /// </returns>
            public override string ToString()
            {
                return string.Format("{0}\t{1}\t{2}\t{3}", TwoLetterCode, Location, ZoneName, Comments);
            }
        }

        /// <summary>
        /// Gets the tz data day.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="startDay">The start day.</param>
        /// <param name="startDay_dayOfWeek">The start day_day of week.</param>
        private static void GetTzDataDay(string str, out int startDay, out DayOfWeek? startDay_dayOfWeek)
        {
            startDay = 0;
            startDay_dayOfWeek = null;
            if (ConversionUtilities.IsStringAnInteger(str))
            {
                startDay = int.Parse(str);
            }
            else
            {
                if (str.Contains(">="))
                {
                    startDay_dayOfWeek = DateTimeUtlities.ParseDayOfWeek(str.Trim().Substring(0, 3));
                    startDay = int.Parse(str.Substring(str.LastIndexOf('=') + 1));
                }
                else if (str.ToLower().Equals("lastsun"))
                {
                    startDay_dayOfWeek = DayOfWeek.Sunday;
                }
            }
        }

        /// <summary>
        /// Gets the tz data time.
        /// </summary>
        /// <param name="saveTime">The save time.</param>
        /// <returns></returns>
        private static TimeSpan GetTzDataTime(string saveTime)
        {
            if (char.IsLetter(saveTime[saveTime.Length - 1]))
            {
                saveTime = saveTime.Substring(0, saveTime.Length - 1);
            }
            return TimeSpan.Parse(saveTime);
        }
    }

    /// <summary>
    /// Thrown when there is an error interpreting the tz database.
    /// </summary>
    [Serializable]
    public class TzException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TzException"/> class.
        /// </summary>
        public TzException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public TzException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public TzException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected TzException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Thrown when there is a parse exception parsing the tz databse.
    /// </summary>
    [Serializable]
    public class TzParseException : TzException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TzParseException"/> class.
        /// </summary>
        public TzParseException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzParseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public TzParseException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzParseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public TzParseException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzParseException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected TzParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

#endif

#endif

    /// <summary>
    /// Methods and date related to the United States, such as a list
    /// of States.
    /// </summary>
#if !(NOSTATES)
    public static class UnitedStatesUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly USState[] States;

        static UnitedStatesUtilities()
        {
            int stateId = 1;
            States = new USState[] {
				new USState(stateId++, "Alabama", "AL"),
				new USState(stateId++, "Alaska", "AK"),
				new USState(stateId++, "Arizona", "AZ"),
				new USState(stateId++, "Arkansas", "AR"),
				new USState(stateId++, "California", "CA"),
				new USState(stateId++, "Colorado", "CO"),
				new USState(stateId++, "Connecticut", "CT"),
				new USState(stateId++, "Delaware", "DE"),
				new USState(stateId++, "District of Columbia", "DC"),
				new USState(stateId++, "Florida", "FL"),
				new USState(stateId++, "Georgia", "GA"),
				new USState(stateId++, "Hawaii", "HI"),
				new USState(stateId++, "Idaho", "ID"),
				new USState(stateId++, "Illinois", "IL"),
				new USState(stateId++, "Indiana", "IN"),
				new USState(stateId++, "Iowa", "IA"),
				new USState(stateId++, "Kansas", "KS"),
				new USState(stateId++, "Kentucky", "KY"),
				new USState(stateId++, "Louisiana", "LA"),
				new USState(stateId++, "Maine", "ME"),
				new USState(stateId++, "Maryland", "MD"),
				new USState(stateId++, "Massachusetts", "MA"),
				new USState(stateId++, "Michigan", "MI"),
				new USState(stateId++, "Minnesota", "MN"),
				new USState(stateId++, "Mississippi", "MS"),
				new USState(stateId++, "Missouri", "MO"),
				new USState(stateId++, "Montana", "MT"),
				new USState(stateId++, "Nebraska", "NE"),
				new USState(stateId++, "Nevada", "NV"),
				new USState(stateId++, "New Hampshire", "NH"),
				new USState(stateId++, "New Jersey", "NJ"),
				new USState(stateId++, "New Mexico", "NM"),
				new USState(stateId++, "New York", "NY"),
				new USState(stateId++, "North Carolina", "NC"),
				new USState(stateId++, "North Dakota", "ND"),
				new USState(stateId++, "Ohio", "OH"),
				new USState(stateId++, "Oklahoma", "OK"),
				new USState(stateId++, "Oregon", "OR"),
				new USState(stateId++, "Pennsylvania", "PA"),
				new USState(stateId++, "Rhode Island", "RI"),
				new USState(stateId++, "South Carolina", "SC"),
				new USState(stateId++, "South Dakota", "SD"),
				new USState(stateId++, "Tennessee", "TN"),
				new USState(stateId++, "Texas", "TX"),
				new USState(stateId++, "Utah", "UT"),
				new USState(stateId++, "Vermont", "VT"),
				new USState(stateId++, "Viginia", "VA"),
				new USState(stateId++, "Washington", "WA"),
				new USState(stateId++, "West Virginia", "WV"),
				new USState(stateId++, "Wisconsin", "WI"),
				new USState(stateId++, "Wyoming", "WY")
			};
        }

        /// <summary>
        /// Attempts to find a <see cref="PublicDomain.UnitedStatesUtilities.USState"/>
        /// by its abbreviate.
        /// </summary>
        /// <param name="abbreviation">The abbreviation of the state to search for. Not case sensitive.</param>
        /// <returns>
        /// The <see cref="PublicDomain.UnitedStatesUtilities.USState"/> that represents the
        /// <c>abbreviation</c>, or if it is not found, throws a <see cref="PublicDomain.UnitedStatesUtilities.StateNotFoundException"/>
        /// </returns>
        /// <exception cref="PublicDomain.UnitedStatesUtilities.StateNotFoundException"></exception>
        public static USState GetStateByAbbrivation(string abbreviation)
        {
            if (abbreviation == null)
            {
                throw new ArgumentNullException("abbreviation");
            }
            abbreviation = abbreviation.ToLower().Trim();
            foreach (USState state in States)
            {
                if (state.Abbreviation.ToLower() == abbreviation)
                {
                    return state;
                }
            }
            throw new StateNotFoundException(abbreviation);
        }

        /// <summary>
        /// Attempts to find a <see cref="PublicDomain.UnitedStatesUtilities.USState"/>
        /// by its name.
        /// </summary>
        /// <param name="stateName">The name of the state to search for. Not case sensitive.</param>
        /// <returns>
        /// The <see cref="PublicDomain.UnitedStatesUtilities.USState"/> that represents the
        /// <c>abbreviation</c>, or if it is not found, throws a <see cref="PublicDomain.UnitedStatesUtilities.StateNotFoundException"/>
        /// </returns>
        /// <exception cref="PublicDomain.UnitedStatesUtilities.StateNotFoundException"></exception>
        public static USState GetStateByName(string stateName)
        {
            if (stateName == null)
            {
                throw new ArgumentNullException("stateName");
            }
            stateName = stateName.ToLower().Trim();
            foreach (USState state in States)
            {
                if (state.Name.ToLower().Equals(stateName))
                {
                    return state;
                }
            }
            throw new StateNotFoundException(stateName);
        }

        /// <summary>
        /// Represents information about a state from the United States
        /// of America.
        /// </summary>
        [Serializable]
        public struct USState
        {
            /// <summary>
            /// 
            /// </summary>
            public int UniqueId;

            /// <summary>
            /// 
            /// </summary>
            public string Name;

            /// <summary>
            /// 
            /// </summary>
            public string Abbreviation;

            /// <summary>
            /// Initializes a new instance of the <see cref="USState"/> class.
            /// </summary>
            /// <param name="uniqueId">The unique id.</param>
            /// <param name="name">The name.</param>
            /// <param name="abbreviation">The abbreviation.</param>
            public USState(int uniqueId, string name, string abbreviation)
            {
                this.UniqueId = uniqueId;
                this.Name = name;
                this.Abbreviation = abbreviation;
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <param name="toUpperCase">if set to <c>true</c> [to upper case].</param>
            /// <returns></returns>
            public string GetName(bool toUpperCase)
            {
                return toUpperCase ? Name.ToUpper() : Name.ToLower();
            }

            /// <summary>
            /// Gets the abbreviation.
            /// </summary>
            /// <param name="toUpperCase">if set to <c>true</c> [to upper case].</param>
            /// <returns></returns>
            public string GetAbbreviation(bool toUpperCase)
            {
                return toUpperCase ? Abbreviation.ToUpper() : Abbreviation.ToLower();
            }
        }

        /// <summary>
        /// Thrown when the state being searched for does not exist.
        /// </summary>
        [Serializable]
        public class StateNotFoundException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="StateNotFoundException"/> class.
            /// </summary>
            public StateNotFoundException() { }
            
            /// <summary>
            /// Initializes a new instance of the <see cref="StateNotFoundException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            public StateNotFoundException(string message) : base(message) { }
            
            /// <summary>
            /// Initializes a new instance of the <see cref="StateNotFoundException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="inner">The inner.</param>
            public StateNotFoundException(string message, Exception inner) : base(message, inner) { }
            
            /// <summary>
            /// Initializes a new instance of the <see cref="StateNotFoundException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
            protected StateNotFoundException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }
#endif

    #region Enums

    /// <summary>
    /// Enumeration of the 12 months of a Gregorian calendar.
    /// Names are in English and values are from 1 to 12.
    /// </summary>
    public enum Month
    {
        /// <summary>
        /// 
        /// </summary>
        January = 1,

        /// <summary>
        /// 
        /// </summary>
        February = 2,

        /// <summary>
        /// 
        /// </summary>
        March = 3,

        /// <summary>
        /// 
        /// </summary>
        April = 4,

        /// <summary>
        /// 
        /// </summary>
        May = 5,

        /// <summary>
        /// 
        /// </summary>
        June = 6,

        /// <summary>
        /// 
        /// </summary>
        July = 7,

        /// <summary>
        /// 
        /// </summary>
        August = 8,
        
        /// <summary>
        /// 
        /// </summary>
        September = 9,

        /// <summary>
        /// 
        /// </summary>
        October = 10,
        
        /// <summary>
        /// 
        /// </summary>
        November = 11,

        /// <summary>
        /// 
        /// </summary>
        December = 12
    }

    /// <summary>
    /// Programming language enumeration (non-exchaustive)
    /// </summary>
    public enum Language
    {
        /// <summary>
        /// 
        /// </summary>
        CSharp,

        /// <summary>
        /// 
        /// </summary>
        PHP,

        /// <summary>
        /// 
        /// </summary>
        JSharp,

        /// <summary>
        /// 
        /// </summary>
        CPlusPlus,

        /// <summary>
        /// 
        /// </summary>
        JScript,

        /// <summary>
        /// 
        /// </summary>
        VisualBasic,

        /// <summary>
        /// 
        /// </summary>
        Java,

        /// <summary>
        /// 
        /// </summary>
        Ruby,
    }

    /// <summary>
    /// Measurement of distance
    /// </summary>
    public enum DistanceType
    {
        /// <summary>
        /// 
        /// </summary>
        StatuteMiles,

        /// <summary>
        /// 
        /// </summary>
        NauticalMiles,

        /// <summary>
        /// 
        /// </summary>
        Kilometers
    }

    /// <summary>
    /// The type of lock to acquire on a <see cref="PublicDomain.DisposableReaderWriter"/> lock.
    /// </summary>
    public enum ReaderWriterLockSynchronizeType
    {
        /// <summary>
        /// 
        /// </summary>
        Read,
        
        /// <summary>
        /// 
        /// </summary>
        Write
    }

    #endregion
}

#if !(NOCLSCOMPLIANTWARNINGSOFF)
#pragma warning restore 3001
#pragma warning restore 3002
#pragma warning restore 3003
#pragma warning restore 3006
#pragma warning restore 3009
#endif

#endregion // Meat
