/// PublicDomain
///  PublicDomain, Version=0.0.1.0, Culture=neutral, PublicKeyToken=fd3f43b5776a962b
/// ======================================
///  Original Author: Kevin Grigorenko (kevgrig@gmail.com)
///  Contributing Authors:
///   * William M. Leszczuk (billl@eden.rutgers.edu)
/// 
///  - "Be free Jedi, be free!"
/// ======================================
/// The purpose of the PublicDomain package is to solve two problems or annoyances
/// of .NET development:
/// 
/// 1. .NET projects and utilities are scattered, difficult to
/// deploy and integrate, difficult to find, difficult to contribute to, and
/// 2. Licenses are confusing and/or restrictive
/// 
/// This package solves these two problems as follows (in reverse order):
/// 
/// 2. This code is in the Public Domain (http://www.copyright.gov/help/faq/faq-definitions.html),
/// meaning that the code has no legal authority, will ask nothing for its use, and
/// has absolutely no restrictions! That is true open source. It may be included
/// in commercial applications, redistributed, altered, or even eaten without any worries.
/// Its use need not be attributed in any way. This package is inherently provided
/// 'as-is', without any express or implied warranty. In no event will any authors
/// be held liable for any damages arising from the use of this package.
/// 
/// 1. This package explicitly breaks some fundamental paradigms of software engineering
/// to solve problem #1. One major goal is that I should be able to embed a single file
/// into my project and harness this package, without adding too much bloat to my application.
/// For this, precompiler directives are used to include or exclude code that is
/// unnecessary or necessitates DLL dependencies that I cannot take on. Second,
/// everything is packaged in a single file to make using this package dead simple,
/// especially in a C# context (non-C# projects will need a built version of this file
/// and reference the DLL). There are no obfuscated build or install procedures,
/// or the complexity of managing 10 referenced open source projects in my solution.
/// I simply place this file anywhere I need its useful code.
/// 
/// Any additions to this file must not introduce non-Public Domain code, or code
/// that must be externally attributed in any way (i.e. attributed by consumers of this package).
/// If you have taken code from someone else which has a similar license and
/// does not require external attribution, make sure with the author that this
/// is truly a proper place for the code, that external attribution is not necessary,
/// and finally make sure to internally attribute the code with a #region to the author(s).
///
/// Version History:
/// ======================================
/// V0.0.1.1
///  [kevgrig@gmail.com]
///   * Added bunch of methods to ConversionUtilities courtesy of
///     William M. Leszczuk (billl@eden.rutgers.edu)
///   * Parsing of tz files works
/// V0.0.1.0
///  [kevgrig@gmail.com]
///   * Project creation in CodePlex (http://www.codeplex.com/PublicDomain)
///   * Added various code from my projects
///   * tz database code unfinished
/// V0.0.0.1
///  [kevgrig@gmail.com]
///   * Added Win32 class and some ExitWindowsEx calls
/// V0.0.0.0
///  [kevgrig@gmail.com]
///   * Wrapper around vjslib for zip file reading
///   * java.io.InputStream <-> System.IO.Stream wrappers
///

#region Directives
// The following section provides
// directives for conditional compilation
// of various sections of the code.
// ======================================

// !!!EDIT DIRECTIVES HERE START!!!

#define NOVJSLIB
#define NOBROKEN

// Commonly non-referenced projects:
#define NOSYSTEMWEB
#define NONUNIT

// Other switches:
//#define NOSCREENSCRAPER
//#define NOCLSCOMPLIANTWARNINGSOFF
//#define NOTZ
//#define NOSTATES

// !!!EDIT DIRECTIVES HERE END!!!!!

// Dependency directives -- do not modify as they
// are very easy to break
#if NOANYTHING
#define NOIMPORTS
#endif

#if NOSYSTEMWEB
#define NOSCREENSCRAPER
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
        public const int StreamBlockSize = 1024;

        public const int ExecuteSmallProcessTimeout = 60000;

        public const double EarthRadiusStatuteMiles = 3963.1D;
        public const double EarthRadiusNauticalMiles = 3443.9D;
        public const double EarthRadiusKilometers = 6376D;

        public const double EarthDiameterStatuteMiles = EarthRadiusStatuteMiles * 2;
        public const double EarthDiameterNauticalMiles = EarthRadiusNauticalMiles * 2;
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
        public T First;
        public U Second;

        public Pair()
        {
        }

        public Pair(T first, U second)
        {
            First = first;
            Second = second;
        }

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
        public T First;
        public U Second;
        public V Third;

        public Triple()
        {
        }

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
        /// <param name="size"></param>
        /// <param name="lowerCase"></param>
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
        /// <param name="val"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string PadIntegerLeft(int val, int length)
        {
            return PadIntegerLeft(val, length, '0');
        }

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
        /// <param name="str"></param>
        /// <param name="find"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string ReplaceFirst(string str, string find, string replace)
        {
            return ReplaceFirst(str, find, replace, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Replace the first occurrence of <paramref name="find"/> with
        /// <paramref name="replace"/>.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="find"></param>
        /// <param name="replace"></param>
        /// <param name="findComparison"></param>
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
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveConsecutiveWhitespace(string str)
        {
            return ReplaceConsecutiveWhitespace(str, " ");
        }

        /// <summary>
        /// Ensures that within <paramref name="str"/> there are no two
        /// consecutive whitespace characters.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceConsecutiveWhitespace(string str, string replacement)
        {
            return Regex.Replace(str, @"\s+", replacement, RegexOptions.Compiled);
        }

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

        public static int IndexOfEmptyPiece(string[] array)
        {
            return IndexOfEmptyPiece(array, 0);
        }

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
        [TestFixture]
        public class Tests
        {
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
        public static readonly char[] AsciiCharacters;

        static CharUtilities()
        {
            AsciiCharacters = GetAsciiCharacters().ToArray();
        }

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
        public static bool IsStringAnInteger(string str)
        {
            return IsStringAnInteger64(str);
        }

        public static bool IsStringAnInteger16(string str)
        {
            Int16 trash;
            return Int16.TryParse(str, out trash);
        }

        public static bool IsStringAnInteger32(string str)
        {
            Int32 trash;
            return Int32.TryParse(str, out trash);
        }

        public static bool IsStringAnInteger64(string str)
        {
            Int64 trash;
            return Int64.TryParse(str, out trash);
        }

        public static bool IsStringAnUnsignedIntegerAny(string str)
        {
            return IsStringAnUnsignedInteger64(str);
        }

        public static bool IsStringAnUnsignedInteger16(string str)
        {
            UInt16 trash;
            return UInt16.TryParse(str, out trash);
        }

        public static bool IsStringAnUnsignedInteger32(string str)
        {
            UInt32 trash;
            return UInt32.TryParse(str, out trash);
        }

        public static bool IsStringAnUnsignedInteger64(string str)
        {
            UInt64 trash;
            return UInt64.TryParse(str, out trash);
        }

        public static bool IsStringADouble(string str)
        {
            double trash;
            return double.TryParse(str, out trash);
        }

        public static bool IsStringADecimal(string str)
        {
            decimal trash;
            return decimal.TryParse(str, out trash);
        }

        public static bool IsStringAFloat(string str)
        {
            float trash;
            return float.TryParse(str, out trash);
        }

        public static bool IsStringAChar(string str)
        {
            char trash;
            return char.TryParse(str, out trash);
        }

        public static bool IsStringABoolean(string str)
        {
            bool trash;
            return bool.TryParse(str, out trash);
        }

        public static bool IsStringAByte(string str)
        {
            byte trash;
            return byte.TryParse(str, out trash);
        }

        public static int ParseInt(string str)
        {
            int result;
            int.TryParse(str, out result);
            return result;
        }

        public static short ParseShort(string str)
        {
            short result;
            short.TryParse(str, out result);
            return result;
        }

        public static long ParseLong(string str)
        {
            long result;
            long.TryParse(str, out result);
            return result;
        }

        public static float ParseFloat(string str)
        {
            float result;
            float.TryParse(str, out result);
            return result;
        }

        public static double ParseDouble(string str)
        {
            double result;
            double.TryParse(str, out result);
            return result;
        }

        public static decimal ParseDecimal(string str)
        {
            decimal result;
            decimal.TryParse(str, out result);
            return result;
        }

        public static uint ParseUInt(string str)
        {
            uint result;
            uint.TryParse(str, out result);
            return result;
        }

        public static ushort ParseUShort(string str)
        {
            ushort result;
            ushort.TryParse(str, out result);
            return result;
        }

        public static ulong ParseULong(string str)
        {
            ulong result;
            ulong.TryParse(str, out result);
            return result;
        }

        public static byte ParseByte(string str)
        {
            byte result;
            byte.TryParse(str, out result);
            return result;
        }

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
        public static void WriteExceptions(Exception ex)
        {
            WriteExceptions(ex, Console.Error);
        }

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

        public static void EnsureDirectoryEnding(ref string directory)
        {
            if (directory != null && directory[directory.Length - 1] != '\\')
            {
                directory += '\\';
            }
        }

        public static string EnsureDirectoryEnding(string directory)
        {
            EnsureDirectoryEnding(ref directory);
            return directory;
        }

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

        public static string GetTemporaryDirectory()
        {
            string ret = PathCombine(Path.GetTempPath(), "t" + new Random((int)unchecked(DateTime.Now.Ticks)).Next(1, 10000).ToString() + @"\");
            Directory.CreateDirectory(ret);
            return ret;
        }

        public static string GetTempFileName(string extension)
        {
            return GetTempFileName(extension, null);
        }

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

        public static void DeleteDirectoryForcefully(string dir)
        {
            RemoveReadOnly(dir);

            Directory.Delete(dir, true);
        }

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
        public static MemoryStream SerializeObjectToBinaryStream(object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(ms, o);
            return ms;
        }

        public static byte[] SerializeObjectToBinary(object o)
        {
            return SerializeObjectToBinaryStream(o).GetBuffer();
        }

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
    /// <param name="rock">Arbitrary data</param>
    public delegate void CallbackZip(ZipEntryE entry, ZipEntryInputStream zipEntryStream, object rock);

    public static class Archiver
    {
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

        public static void Extract(string zipFilePath, string destinationDirectory)
        {
            Extract(zipFilePath, destinationDirectory, false);
        }

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
        protected ZipEntry m_entry;

        public ZipEntryE(ZipEntry entry)
            : base(entry.getName())
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            m_entry = entry;
        }

        public override string getComment()
        {
            return m_entry.getComment();
        }

        public override long getCompressedSize()
        {
            return m_entry.getCompressedSize();
        }

        public override long getCrc()
        {
            return m_entry.getCrc();
        }

        public override sbyte[] getExtra()
        {
            return m_entry.getExtra();
        }

        public override int getMethod()
        {
            return m_entry.getMethod();
        }

        public override bool Equals(object obj)
        {
            return m_entry.Equals(obj);
        }

        public override int GetHashCode()
        {
            return m_entry.GetHashCode();
        }

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

        public override long getSize()
        {
            return m_entry.getSize();
        }

        public override long getTime()
        {
            return m_entry.getTime();
        }

        public override bool isDirectory()
        {
            return m_entry.isDirectory();
        }

        public override void setComment(string comment)
        {
            m_entry.setComment(comment);
        }

        public override void setCrc(long crc)
        {
            m_entry.setCrc(crc);
        }

        public override void setExtra(sbyte[] extra)
        {
            m_entry.setExtra(extra);
        }

        public override void setMethod(int m)
        {
            m_entry.setMethod(m);
        }

        public override void setSize(long sz)
        {
            m_entry.setSize(sz);
        }

        public override void setTime(long t)
        {
            m_entry.setTime(t);
        }

        public override string ToString()
        {
            return m_entry.ToString();
        }
    }

    public class ZipEntryInputStream : JInputStream
    {
        protected ZipInputStream m_zis;

        public ZipEntryInputStream(ZipInputStream zis)
            : base(zis)
        {
            if (zis == null)
            {
                throw new ArgumentNullException("zis");
            }
            m_zis = zis;
        }

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
        /// <param name="file"></param>
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
        /// <param name="stream"></param>
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
        protected Stream m_stream;

        public JStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            m_stream = stream;
        }

        public override int read()
        {
            return m_stream.ReadByte();
        }

        public override int available()
        {
            return unchecked((int)(m_stream.Length - m_stream.Position));
        }

        public override void close()
        {
            m_stream.Close();
        }

        public override bool Equals(object obj)
        {
            return m_stream.Equals(obj);
        }

        public override int GetHashCode()
        {
            return m_stream.GetHashCode();
        }

        public override void mark(int readlimit)
        {
            throw new NotImplementedException();
        }

        public override bool markSupported()
        {
            return false;
        }

        public override int read(sbyte[] b)
        {
            return read(b, 0, b.Length);
        }

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

        public override void reset()
        {
            throw new NotImplementedException();
        }

        public override long skip(long count)
        {
            return m_stream.Seek(count, SeekOrigin.Current);
        }

        public override string ToString()
        {
            return m_stream.ToString();
        }

        public void Dispose()
        {
            // We assume the caller is managing the stream passed in, so we
            // won't actually close it
        }
    }

    public class JInputStream : Stream
    {
        protected InputStream m_jis;

        public JInputStream(java.io.InputStream javaInputStream)
        {
            if (javaInputStream == null)
            {
                throw new ArgumentNullException("javaInputStream");
            }
            m_jis = javaInputStream;
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override bool CanWrite
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override void Flush()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override long Length
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

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

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Close()
        {
            m_jis.close();
            base.Close();
        }

        public override int ReadByte()
        {
            return m_jis.read();
        }

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

        public enum WindowsControl : uint
        {
            Logoff = Win32Constants.EWX_LOGOFF,
            ShutdownAndPowerOff = Win32Constants.EWX_POWEROFF,
            ShutdownNoPowerOff = Win32Constants.EWX_SHUTDOWN,
            Restart = Win32Constants.EWX_REBOOT,
            RestartApps = Win32Constants.EWX_RESTARTAPPS,
        }

        private static void GetLastErrorThrow()
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public static void LogoffCurrentUser()
        {
            LogoffCurrentUser(false);
        }

        public static void LogoffCurrentUser(bool force)
        {
            ExitWindows(WindowsControl.Logoff, force);
        }

        public static void Shutdown()
        {
            Shutdown(false);
        }

        public static void Shutdown(bool force)
        {
            Shutdown(true);
        }

        public static void Shutdown(bool force, bool powerOff)
        {
            ExitWindows(powerOff ? WindowsControl.ShutdownAndPowerOff : WindowsControl.ShutdownNoPowerOff, force);
        }

        public static void RestartWindows()
        {
            RestartWindows(false);
        }

        public static void RestartWindows(bool force)
        {
            RestartWindows(false);
        }

        public static void RestartWindows(bool force, bool restartApps)
        {
            ExitWindows(restartApps ? WindowsControl.RestartApps : WindowsControl.Restart, force);
        }

        public static void ExitWindows(WindowsControl control)
        {
            ExitWindows(control, false);
        }

        public static void ExitWindows(WindowsControl control, bool force)
        {
            ExitWindows(control, force, true, Win32Constants.SHTDN_REASON_MAJOR_OTHER, Win32Constants.SHTDN_REASON_MINOR_OTHER);
        }

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

        public ProcessHelper()
            : this(true)
        {
        }

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

                Out = new StringWriter(m_outBuilder);
                Error = new StringWriter(m_errorBuilder);
            }
        }

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

        public void SetArguments(params string[] args)
        {
            if (args != null)
            {
                StringBuilder sb = GetMangledArguments(args);
                Arguments = sb.ToString();
            }
        }

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

        protected virtual void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                m_out.WriteLine(e.Data);
            }
        }

        protected virtual void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                m_error.WriteLine(e.Data);
            }
        }

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

        public ProcessStartInfo StartInfo
        {
            get
            {
                return m_process.StartInfo;
            }
        }

        public void Start()
        {
            Start(true);
        }

        /// <summary>
        /// Starts the process, begins asynchronous reads on
        /// both standard output and standard error.
        /// </summary>
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
        /// <returns>Return code of completed process</returns>
        public int StartAndWaitForExit(bool throwOnError)
        {
            return StartAndWaitForExit(GlobalConstants.ExecuteSmallProcessTimeout, throwOnError);
        }

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

        public int ExitCode
        {
            get
            {
                return m_process.ExitCode;
            }
        }

        public string StandardOutput
        {
            get
            {
                return m_outBuilder == null ? null : m_outBuilder.ToString();
            }
        }

        public string StandardError
        {
            get
            {
                return m_errorBuilder == null ? null : m_errorBuilder.ToString();
            }
        }

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

        public string CheckForError()
        {
            return CheckForError(true);
        }

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
        public const string CultureInvariant = "";
        public const int CultureInvariantIdentifier = 127;

        public const string CultureNeutralAfrikaans = "af";
        public const int CultureNeutralAfrikaansIdentifier = 54;

        public const string CultureSpecificAfrikaansSouthAfrica = "af-ZA";
        public const int CultureSpecificAfrikaansSouthAfricaIdentifier = 1078;

        public const string CultureNeutralAlbanian = "sq";
        public const int CultureNeutralAlbanianIdentifier = 28;

        public const string CultureSpecificAlbanianAlbania = "sq-AL";
        public const int CultureSpecificAlbanianAlbaniaIdentifier = 1052;

        public const string CultureNeutralArabic = "ar";
        public const int CultureNeutralArabicIdentifier = 1;

        public const string CultureNeutralArabicAlgeria = "ar-DZ";
        public const int CultureNeutralArabicAlgeriaIdentifier = 5121;

        public const string CultureSpecificArabicBahrain = "ar-BH";
        public const int CultureSpecificArabicBahrainIdentifier = 15361;

        public const string CultureSpecificArabicEgypt = "ar-EG";
        public const int CultureSpecificArabicEgyptIdentifier = 3073;

        public const string CultureSpecificArabicIraq = "ar-IQ";
        public const int CultureSpecificArabicIraqIdentifier = 2049;

        public const string CultureSpecificArabicJordan = "ar-JO";
        public const int CultureSpecificArabicJordanIdentifier = 11265;

        public const string CultureSpecificArabicKuwait = "ar-KW";
        public const int CultureSpecificArabicKuwaitIdentifier = 13313;

        public const string CultureSpecificArabicLebanon = "ar-LB";
        public const int CultureSpecificArabicLebanonIdentifier = 12289;

        public const string CultureSpecificArabicLibya = "ar-LY";
        public const int CultureSpecificArabicLibyaIdentifier = 4097;

        public const string CultureSpecificArabicMorocco = "ar-MA";
        public const int CultureSpecificArabicMoroccoIdentifier = 6145;

        public const string CultureSpecificArabicOman = "ar-OM";
        public const int CultureSpecificArabicOmanIdentifier = 8193;

        public const string CultureSpecificArabicQatar = "ar-QA";
        public const int CultureSpecificArabicQatarIdentifier = 16385;

        public const string CultureSpecificArabicSaudiArabia = "ar-SA";
        public const int CultureSpecificArabicSaudiArabiaIdentifier = 1025;

        public const string CultureSpecificArabicSyria = "ar-SY";
        public const int CultureSpecificArabicSyriaIdentifier = 10241;

        public const string CultureSpecificArabicTunisia = "ar-TN";
        public const int CultureSpecificArabicTunisiaIdentifier = 7169;

        public const string CultureSpecificArabicUnitedArabEmirates = "ar-AE";
        public const int CultureSpecificArabicUnitedArabEmiratesIdentifier = 14337;

        public const string CultureSpecificArabicYemen = "ar-YE";
        public const int CultureSpecificArabicYemenIdentifier = 9217;

        public const string CultureNeutralArmenian = "hy";
        public const int CultureNeutralArmenianIdentifier = 43;

        public const string CultureSpecificArmenianArmenia = "hy-AM";
        public const int CultureSpecificArmenianArmeniaIdentifier = 1067;

        public const string CultureNeutralAzeri = "az";
        public const int CultureNeutralAzeriIdentifier = 44;

        public const string CultureSpecificAzeriCyrillicAzerbaijan = "az-AZ-Cyrl";
        public const int CultureSpecificAzeriCyrillicAzerbaijanIdentifier = 2092;

        public const string CultureSpecificAzeriLatinAzerbaijan = "az-AZ-Latn";
        public const int CultureSpecificAzeriLatinAzerbaijanIdentifier = 1068;

        public const string CultureNeutralBasque = "eu";
        public const int CultureNeutralBasqueIdentifier = 45;

        public const string CultureSpecificBasqueBasque = "eu-ES";
        public const int CultureSpecificBasqueBasqueIdentifier = 1069;

        public const string CultureNeutralBelarusian = "be";
        public const int CultureNeutralBelarusianIdentifier = 35;

        public const string CultureSpecificBelarusianBelarus = "be-BY";
        public const int CultureSpecificBelarusianBelarusIdentifier = 1059;

        public const string CultureNeutralBulgarian = "bg";
        public const int CultureNeutralBulgarianIdentifier = 2;

        public const string CultureSpecificBulgarianBulgaria = "bg-BG";
        public const int CultureSpecificBulgarianBulgariaIdentifier = 1026;

        public const string CultureNeutralCatalan = "ca";
        public const int CultureNeutralCatalanIdentifier = 3;

        public const string CultureSpecificCatalanCatalan = "ca-ES";
        public const int CultureSpecificCatalanCatalanIdentifier = 1027;

        public const string CultureSpecificChineseHongKongSar = "zh-HK";
        public const int CultureSpecificChineseHongKongSarIdentifier = 3076;

        public const string CultureSpecificChineseMacaoSar = "zh-MO";
        public const int CultureSpecificChineseMacaoSarIdentifier = 5124;

        public const string CultureSpecificChineseChina = "zh-CN";
        public const int CultureSpecificChineseChinaIdentifier = 2052;

        public const string CultureSpecificChineseSimplified = "zh-CHS";
        public const int CultureSpecificChineseSimplifiedIdentifier = 4;

        public const string CultureSpecificChineseSingapore = "zh-SG";
        public const int CultureSpecificChineseSingaporeIdentifier = 4100;

        public const string CultureSpecificChineseTaiwan = "zh-TW";
        public const int CultureSpecificChineseTaiwanIdentifier = 1028;

        public const string CultureSpecificChineseTraditional = "zh-CHT";
        public const int CultureSpecificChineseTraditionalIdentifier = 31748;

        public const string CultureNeutralCroatian = "hr";
        public const int CultureNeutralCroatianIdentifier = 26;

        public const string CultureSpecificCroatianCroatia = "hr-HR";
        public const int CultureSpecificCroatianCroatiaIdentifier = 1050;

        public const string CultureNeutralCzech = "cs";
        public const int CultureNeutralCzechIdentifier = 5;

        public const string CultureSpecificCzechCzechRepublic = "cs-CZ";
        public const int CultureSpecificCzechCzechRepublicIdentifier = 1029;

        public const string CultureNeutralDanish = "da";
        public const int CultureNeutralDanishIdentifier = 6;

        public const string CultureSpecificDanishDenmark = "da-DK";
        public const int CultureSpecificDanishDenmarkIdentifier = 1030;

        public const string CultureNeutralDhivehi = "div";
        public const int CultureNeutralDhivehiIdentifier = 101;

        public const string CultureSpecificDhivehiMaldives = "div-MV";
        public const int CultureSpecificDhivehiMaldivesIdentifier = 1125;

        public const string CultureNeutralDutch = "nl";
        public const int CultureNeutralDutchIdentifier = 19;

        public const string CultureSpecificDutchBelgium = "nl-BE";
        public const int CultureSpecificDutchBelgiumIdentifier = 2067;

        public const string CultureSpecificDutchTheNetherlands = "nl-NL";
        public const int CultureSpecificDutchTheNetherlandsIdentifier = 1043;

        public const string CultureNeutralEnglish = "en";
        public const int CultureNeutralEnglishIdentifier = 9;

        public const string CultureSpecificEnglishAustralia = "en-AU";
        public const int CultureSpecificEnglishAustraliaIdentifier = 3081;

        public const string CultureSpecificEnglishBelize = "en-BZ";
        public const int CultureSpecificEnglishBelizeIdentifier = 10249;

        public const string CultureSpecificEnglishCanada = "en-CA";
        public const int CultureSpecificEnglishCanadaIdentifier = 4105;

        public const string CultureSpecificEnglishCaribbean = "en-CB";
        public const int CultureSpecificEnglishCaribbeanIdentifier = 9225;

        public const string CultureSpecificEnglishIreland = "en-IE";
        public const int CultureSpecificEnglishIrelandIdentifier = 6153;

        public const string CultureSpecificEnglishJamaica = "en-JM";
        public const int CultureSpecificEnglishJamaicaIdentifier = 8201;

        public const string CultureSpecificEnglishNewZealand = "en-NZ";
        public const int CultureSpecificEnglishNewZealandIdentifier = 5129;

        public const string CultureSpecificEnglishPhilippines = "en-PH";
        public const int CultureSpecificEnglishPhilippinesIdentifier = 13321;

        public const string CultureSpecificEnglishSouthAfrica = "en-ZA";
        public const int CultureSpecificEnglishSouthAfricaIdentifier = 7177;

        public const string CultureSpecificEnglishTrinidadAndTobago = "en-TT";
        public const int CultureSpecificEnglishTrinidadAndTobagoIdentifier = 11273;

        public const string CultureSpecificEnglishUnitedKingdom = "en-GB";
        public const int CultureSpecificEnglishUnitedKingdomIdentifier = 2057;

        public const string CultureSpecificEnglishUnitedStates = "en-US";
        public const int CultureSpecificEnglishUnitedStatesIdentifier = 1033;

        public const string CultureSpecificEnglishZimbabwe = "en-ZW";
        public const int CultureSpecificEnglishZimbabweIdentifier = 12297;

        public const string CultureNeutralEstonian = "et";
        public const int CultureNeutralEstonianIdentifier = 37;

        public const string CultureSpecificEstonianEstonia = "et-EE";
        public const int CultureSpecificEstonianEstoniaIdentifier = 1061;

        public const string CultureNeutralFaroese = "fo";
        public const int CultureNeutralFaroeseIdentifier = 56;

        public const string CultureSpecificFaroeseFaroeIslands = "fo-FO";
        public const int CultureSpecificFaroeseFaroeIslandsIdentifier = 1080;

        public const string CultureNeutralFarsi = "fa";
        public const int CultureNeutralFarsiIdentifier = 41;

        public const string CultureSpecificFarsiIran = "fa-IR";
        public const int CultureSpecificFarsiIranIdentifier = 1065;

        public const string CultureNeutralFinnish = "fi";
        public const int CultureNeutralFinnishIdentifier = 11;

        public const string CultureSpecificFinnishFinland = "fi-FI";
        public const int CultureSpecificFinnishFinlandIdentifier = 1035;

        public const string CultureNeutralFrench = "fr";
        public const int CultureNeutralFrenchIdentifier = 12;

        public const string CultureSpecificFrenchBelgium = "fr-BE";
        public const int CultureSpecificFrenchBelgiumIdentifier = 2060;

        public const string CultureSpecificFrenchCanada = "fr-CA";
        public const int CultureSpecificFrenchCanadaIdentifier = 3084;

        public const string CultureSpecificFrenchFrance = "fr-FR";
        public const int CultureSpecificFrenchFranceIdentifier = 1036;

        public const string CultureSpecificFrenchLuxembourg = "fr-LU";
        public const int CultureSpecificFrenchLuxembourgIdentifier = 5132;

        public const string CultureSpecificFrenchMonaco = "fr-MC";
        public const int CultureSpecificFrenchMonacoIdentifier = 6156;

        public const string CultureSpecificFrenchSwitzerland = "fr-CH";
        public const int CultureSpecificFrenchSwitzerlandIdentifier = 4108;

        public const string CultureNeutralGalician = "gl";
        public const int CultureNeutralGalicianIdentifier = 86;

        public const string CultureSpecificGalicianGalician = "gl-ES";
        public const int CultureSpecificGalicianGalicianIdentifier = 1110;

        public const string CultureNeutralGeorgian = "ka";
        public const int CultureNeutralGeorgianIdentifier = 55;

        public const string CultureSpecificGeorgianGeorgia = "ka-GE";
        public const int CultureSpecificGeorgianGeorgiaIdentifier = 1079;

        public const string CultureNeutralGerman = "de";
        public const int CultureNeutralGermanIdentifier = 7;

        public const string CultureSpecificGermanAustria = "de-AT";
        public const int CultureSpecificGermanAustriaIdentifier = 3079;

        public const string CultureSpecificGermanGermany = "de-DE";
        public const int CultureSpecificGermanGermanyIdentifier = 1031;

        public const string CultureSpecificGermanLiechtenstein = "de-LI";
        public const int CultureSpecificGermanLiechtensteinIdentifier = 5127;

        public const string CultureSpecificGermanLuxembourg = "de-LU";
        public const int CultureSpecificGermanLuxembourgIdentifier = 4103;

        public const string CultureSpecificGermanSwitzerland = "de-CH";
        public const int CultureSpecificGermanSwitzerlandIdentifier = 2055;

        public const string CultureNeutralGreek = "el";
        public const int CultureNeutralGreekIdentifier = 8;

        public const string CultureSpecificGreekGreece = "el-GR";
        public const int CultureSpecificGreekGreeceIdentifier = 1032;

        public const string CultureNeutralGujarati = "gu";
        public const int CultureNeutralGujaratiIdentifier = 71;

        public const string CultureSpecificGujaratiIndia = "gu-IN";
        public const int CultureSpecificGujaratiIndiaIdentifier = 1095;

        public const string CultureNeutralHebrew = "he";
        public const int CultureNeutralHebrewIdentifier = 13;

        public const string CultureSpecificHebrewIsrael = "he-IL";
        public const int CultureSpecificHebrewIsraelIdentifier = 1037;

        public const string CultureNeutralHindi = "hi";
        public const int CultureNeutralHindiIdentifier = 57;

        public const string CultureSpecificHindiIndia = "hi-IN";
        public const int CultureSpecificHindiIndiaIdentifier = 1081;

        public const string CultureNeutralHungarian = "hu";
        public const int CultureNeutralHungarianIdentifier = 14;

        public const string CultureSpecificHungarianHungary = "hu-HU";
        public const int CultureSpecificHungarianHungaryIdentifier = 1038;

        public const string CultureNeutralIcelandic = "is";
        public const int CultureNeutralIcelandicIdentifier = 15;

        public const string CultureSpecificIcelandicIceland = "is-IS";
        public const int CultureSpecificIcelandicIcelandIdentifier = 1039;

        public const string CultureNeutralIndonesian = "id";
        public const int CultureNeutralIndonesianIdentifier = 33;

        public const string CultureSpecificIndonesianIndonesia = "id-ID";
        public const int CultureSpecificIndonesianIndonesiaIdentifier = 1057;

        public const string CultureNeutralItalian = "it";
        public const int CultureNeutralItalianIdentifier = 16;

        public const string CultureSpecificItalianItaly = "it-IT";
        public const int CultureSpecificItalianItalyIdentifier = 1040;

        public const string CultureSpecificItalianSwitzerland = "it-CH";
        public const int CultureSpecificItalianSwitzerlandIdentifier = 2064;

        public const string CultureNeutralJapanese = "ja";
        public const int CultureNeutralJapaneseIdentifier = 17;

        public const string CultureSpecificJapaneseJapan = "ja-JP";
        public const int CultureSpecificJapaneseJapanIdentifier = 1041;

        public const string CultureNeutralKannada = "kn";
        public const int CultureNeutralKannadaIdentifier = 75;

        public const string CultureSpecificKannadaIndia = "kn-IN";
        public const int CultureSpecificKannadaIndiaIdentifier = 1099;

        public const string CultureNeutralKazakh = "kk";
        public const int CultureNeutralKazakhIdentifier = 63;

        public const string CultureSpecificKazakhKazakhstan = "kk-KZ";
        public const int CultureSpecificKazakhKazakhstanIdentifier = 1087;

        public const string CultureNeutralKonkani = "kok";
        public const int CultureNeutralKonkaniIdentifier = 87;

        public const string CultureSpecificKonkaniIndia = "kok-IN";
        public const int CultureSpecificKonkaniIndiaIdentifier = 1111;

        public const string CultureNeutralKorean = "ko";
        public const int CultureNeutralKoreanIdentifier = 18;

        public const string CultureSpecificKoreanKorea = "ko-KR";
        public const int CultureSpecificKoreanKoreaIdentifier = 1042;

        public const string CultureNeutralKyrgyz = "ky";
        public const int CultureNeutralKyrgyzIdentifier = 64;

        public const string CultureSpecificKyrgyzKyrgyzstan = "ky-KG";
        public const int CultureSpecificKyrgyzKyrgyzstanIdentifier = 1088;

        public const string CultureNeutralLatvian = "lv";
        public const int CultureNeutralLatvianIdentifier = 38;

        public const string CultureSpecificLatvianLatvia = "lv-LV";
        public const int CultureSpecificLatvianLatviaIdentifier = 1062;

        public const string CultureNeutralLithuanian = "lt";
        public const int CultureNeutralLithuanianIdentifier = 39;

        public const string CultureSpecificLithuanianLithuania = "lt-LT";
        public const int CultureSpecificLithuanianLithuaniaIdentifier = 1063;

        public const string CultureNeutralMacedonian = "mk";
        public const int CultureNeutralMacedonianIdentifier = 47;

        public const string CultureSpecificMacedonianFormerYugoslavRepublicOfMacedonia = "mk-MK";
        public const int CultureSpecificMacedonianFormerYugoslavRepublicOfMacedoniaIdentifier = 1071;

        public const string CultureNeutralMalay = "ms";
        public const int CultureNeutralMalayIdentifier = 62;

        public const string CultureSpecificMalayBrunei = "ms-BN";
        public const int CultureSpecificMalayBruneiIdentifier = 2110;

        public const string CultureSpecificMalayMalaysia = "ms-MY";
        public const int CultureSpecificMalayMalaysiaIdentifier = 1086;

        public const string CultureNeutralMarathi = "mr";
        public const int CultureNeutralMarathiIdentifier = 78;

        public const string CultureSpecificMarathiIndia = "mr-IN";
        public const int CultureSpecificMarathiIndiaIdentifier = 1102;

        public const string CultureNeutralMongolian = "mn";
        public const int CultureNeutralMongolianIdentifier = 80;

        public const string CultureSpecificMongolianMongolia = "mn-MN";
        public const int CultureSpecificMongolianMongoliaIdentifier = 1104;

        public const string CultureNeutralNorwegian = "no";
        public const int CultureNeutralNorwegianIdentifier = 20;

        public const string CultureSpecificNorwegianBokmlNorway = "nb-NO";
        public const int CultureSpecificNorwegianBokmlNorwayIdentifier = 1044;

        public const string CultureSpecificNorwegianNynorskNorway = "nn-NO";
        public const int CultureSpecificNorwegianNynorskNorwayIdentifier = 2068;

        public const string CultureNeutralPolish = "pl";
        public const int CultureNeutralPolishIdentifier = 21;

        public const string CultureSpecificPolishPoland = "pl-PL";
        public const int CultureSpecificPolishPolandIdentifier = 1045;

        public const string CultureNeutralPortuguese = "pt";
        public const int CultureNeutralPortugueseIdentifier = 22;

        public const string CultureSpecificPortugueseBrazil = "pt-BR";
        public const int CultureSpecificPortugueseBrazilIdentifier = 1046;

        public const string CultureSpecificPortuguesePortugal = "pt-PT";
        public const int CultureSpecificPortuguesePortugalIdentifier = 2070;

        public const string CultureNeutralPunjabi = "pa";
        public const int CultureNeutralPunjabiIdentifier = 70;

        public const string CultureSpecificPunjabiIndia = "pa-IN";
        public const int CultureSpecificPunjabiIndiaIdentifier = 1094;

        public const string CultureNeutralRomanian = "ro";
        public const int CultureNeutralRomanianIdentifier = 24;

        public const string CultureSpecificRomanianRomania = "ro-RO";
        public const int CultureSpecificRomanianRomaniaIdentifier = 1048;

        public const string CultureNeutralRussian = "ru";
        public const int CultureNeutralRussianIdentifier = 25;

        public const string CultureSpecificRussianRussia = "ru-RU";
        public const int CultureSpecificRussianRussiaIdentifier = 1049;

        public const string CultureNeutralSanskrit = "sa";
        public const int CultureNeutralSanskritIdentifier = 79;

        public const string CultureSpecificSanskritIndia = "sa-IN";
        public const int CultureSpecificSanskritIndiaIdentifier = 1103;

        public const string CultureSpecificSerbianCyrrilicSerbia = "sr-SP-Cyrl";
        public const int CultureSpecificSerbianCyrrilicSerbiaIdentifier = 3098;

        public const string CultureSpecificSerbianLatinSerbia = "sr-SP-Latn";
        public const int CultureSpecificSerbianLatinSerbiaIdentifier = 2074;

        public const string CultureNeutralSlovak = "sk";
        public const int CultureNeutralSLOVAKIdentifier = 27;

        public const string CultureSpecificSlovakSlovakia = "sk-SK";
        public const int CultureSpecificSlovakSlovakiaIdentifier = 1051;

        public const string CultureNeutralSlovenian = "sl";
        public const int CultureNeutralSlovenianIdentifier = 36;

        public const string CultureSpecificSlovenianSlovenia = "sl-SI";
        public const int CultureSpecificSlovenianSloveniaIdentifier = 1060;

        public const string CultureNeutralSpanish = "es";
        public const int CultureNeutralSPANISHIdentifier = 10;

        public const string CultureSpecificSpanishArgentina = "es-AR";
        public const int CultureSpecificSpanishArgentinaIdentifier = 11274;

        public const string CultureSpecificSpanishBolivia = "es-BO";
        public const int CultureSpecificSpanishBoliviaIdentifier = 16394;

        public const string CultureSpecificSpanishChile = "es-CL";
        public const int CultureSpecificSpanishChileIdentifier = 13322;

        public const string CultureSpecificSpanishColombia = "es-CO";
        public const int CultureSpecificSpanishColombiaIdentifier = 9226;

        public const string CultureSpecificSpanishCostaRica = "es-CR";
        public const int CultureSpecificSpanishCostaRicaIdentifier = 5130;

        public const string CultureSpecificSpanishDominicanRepublic = "es-DO";
        public const int CultureSpecificSpanishDominicanRepublicIdentifier = 7178;

        public const string CultureSpecificSpanishEcuador = "es-EC";
        public const int CultureSpecificSpanishEcuadorIdentifier = 12298;

        public const string CultureSpecificSpanishElSalvador = "es-SV";
        public const int CultureSpecificSpanishElSalvadorIdentifier = 17418;

        public const string CultureSpecificSpanishGuatemala = "es-GT";
        public const int CultureSpecificSpanishGuatemalaIdentifier = 4106;

        public const string CultureSpecificSpanishHonduras = "es-HN";
        public const int CultureSpecificSpanishHondurasIdentifier = 18442;

        public const string CultureSpecificSpanishMexico = "es-MX";
        public const int CultureSpecificSpanishMexicoIdentifier = 2058;

        public const string CultureSpecificSpanishNicaragua = "es-NI";
        public const int CultureSpecificSpanishNicaraguaIdentifier = 19466;

        public const string CultureSpecificSpanishPanama = "es-PA";
        public const int CultureSpecificSpanishPanamaIdentifier = 6154;

        public const string CultureSpecificSpanishParaguay = "es-PY";
        public const int CultureSpecificSpanishParaguayIdentifier = 15370;

        public const string CultureSpecificSpanishPeru = "es-PE";
        public const int CultureSpecificSpanishPeruIdentifier = 10250;

        public const string CultureSpecificSpanishPuertoRico = "es-PR";
        public const int CultureSpecificSpanishPuertoRicoIdentifier = 20490;

        public const string CultureSpecificSpanishSpain = "es-ES";
        public const int CultureSpecificSpanishSpainIdentifier = 3082;

        public const string CultureSpecificSpanishUruguay = "es-UY";
        public const int CultureSpecificSpanishUruguayIdentifier = 14346;

        public const string CultureSpecificSpanishVenezuela = "es-VE";
        public const int CultureSpecificSpanishVenezuelaIdentifier = 8202;

        public const string CultureNeutralSwahili = "sw";
        public const int CultureNeutralSwahiliIdentifier = 65;

        public const string CultureSpecificSwahiliKenya = "sw-KE";
        public const int CultureSpecificSwahiliKenyaIdentifier = 1089;

        public const string CultureNeutralSwedish = "sv";
        public const int CultureNeutralSwedishIdentifier = 29;

        public const string CultureSpecificSwedishFinland = "sv-FI";
        public const int CultureSpecificSwedishFinlandIdentifier = 2077;

        public const string CultureSpecificSwedishSweden = "sv-SE";
        public const int CultureSpecificSwedishSwedenIdentifier = 1053;

        public const string CultureNeutralSyriac = "syr";
        public const int CultureNeutralSyriacIdentifier = 90;

        public const string CultureSpecificSyriacSyria = "syr-SY";
        public const int CultureSpecificSyriacSyriaIdentifier = 1114;

        public const string CultureNeutralTamil = "ta";
        public const int CultureNeutralTamilIdentifier = 73;

        public const string CultureSpecificTamilIndia = "ta-IN";
        public const int CultureSpecificTamilIndiaIdentifier = 1097;

        public const string CultureNeutralTatar = "tt";
        public const int CultureNeutralTatarIdentifier = 68;

        public const string CultureSpecificTatarRussia = "tt-RU";
        public const int CultureSpecificTatarRussiaIdentifier = 1092;

        public const string CultureNeutralTelugu = "te";
        public const int CultureNeutralTeluguIdentifier = 74;

        public const string CultureSpecificTeluguIndia = "te-IN";
        public const int CultureSpecificTeluguIndiaIdentifier = 1098;

        public const string CultureNeutralThai = "th";
        public const int CultureNeutralThaiIdentifier = 30;

        public const string CultureSpecificThaiThailand = "th-TH";
        public const int CultureSpecificThaiThailandIdentifier = 1054;

        public const string CultureNeutralTurkish = "tr";
        public const int CultureNeutralTurkishIdentifier = 31;

        public const string CultureSpecificTurkishTurkey = "tr-TR";
        public const int CultureSpecificTurkishTurkeyIdentifier = 1055;

        public const string CultureNeutralUkrainian = "uk";
        public const int CultureNeutralUkrainianIdentifier = 34;

        public const string CultureSpecificUkrainianUkraine = "uk-UA";
        public const int CultureSpecificUkrainianUkraineIdentifier = 1058;

        public const string CultureNeutralUrdu = "ur";
        public const int CultureNeutralUrduIdentifier = 32;

        public const string CultureSpecificUrduPakistan = "ur-PK";
        public const int CultureSpecificUrduPakistanIdentifier = 1056;

        public const string CultureNeutralUzbek = "uz";
        public const int CultureNeutralUzbekIdentifier = 67;

        public const string CultureSpecificUzbekCyrillicUzbekistan = "uz-UZ-Cyrl";
        public const int CultureSpecificUzbekCyrillicUzbekistanIdentifier = 2115;

        public const string CultureSpecificUzbekLatinUzbekistan = "uz-UZ-Latn";
        public const int CultureSpecificUzbekLatinUzbekistanIdentifier = 1091;

        public const string CultureNeutralVietnamese = "vi";
        public const int CultureNeutralVietnameseIdentifier = 42;

        public const string CultureSpecificVietnameseVietnam = "vi-VN";
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
        ReaderWriterLock Sync { get; }
        void OnBeforeAcquire(ReaderWriterLockSynchronizeType desiredType);
    }

    /// <summary>
    /// Generic Reader/Writer lock that can be used in a using() statement.
    /// </summary>
    public class DisposableReaderWriter : IDisposable
    {
        public DisposableReaderWriter(IExposesReaderWriterLock root, ReaderWriterLockSynchronizeType type)
        {
            m_Root = root;
            m_SynchronizeType = type;

            AcquireLock();
        }

        public virtual void Dispose()
        {
            ReleaseLock();
        }

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

        protected virtual void AcquireReaderLock()
        {
            m_Root.Sync.AcquireReaderLock(DefaultLockTimeout);
        }

        protected virtual void AcquireWriterLock()
        {
            m_Root.Sync.AcquireWriterLock(DefaultLockTimeout);
        }

        protected virtual void ReleaseReaderLock()
        {
            m_Root.Sync.ReleaseReaderLock();
        }

        protected virtual void ReleaseWriterLock()
        {
            m_Root.Sync.ReleaseWriterLock();
        }

        public virtual void UpgradeToWriterLock()
        {
            m_SynchronizeType = ReaderWriterLockSynchronizeType.Write;
            m_UpgradeLockCookie = m_Root.Sync.UpgradeToWriterLock(DefaultLockTimeout);
        }

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

        public const int DefaultLockTimeout = 100;

        protected ReaderWriterLockSynchronizeType m_SynchronizeType;
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

        public InvariantCultureContext()
        {
            oldCulture = Thread.CurrentThread.CurrentCulture;
            oldUICulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        }

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
        public const string HtmlBreakExpression = @"<\s*br\s*/?\s*>";
        public const string HtmlParagraphExpression = @"<\s*p\s*/?\s*>";
        public const string HtmlBreakOrParagraphExpression = @"<\s*([bp]r?)\s*/?\s*>";

        public static Regex HtmlBreak = new Regex(HtmlBreakExpression, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex HtmlParagraph = new Regex(HtmlParagraphExpression, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex HtmlBreakOrParagraph = new Regex(HtmlBreakOrParagraphExpression, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public const string HtmlBreakOrParagraphTrimLeftExpression = @"^" + HtmlBreakOrParagraphExpression;
        public const string HtmlBreakOrParagraphTrimRightExpression = HtmlBreakOrParagraphExpression + @"$";

        public static Regex HtmlBreakOrParagraphTrim = new Regex(string.Format("({0})|({1})", HtmlBreakOrParagraphTrimLeftExpression, HtmlBreakOrParagraphTrimRightExpression), RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public const string UriChars = @"[^\s)<>\]}!([]+";

        public static readonly Regex Uri = new Regex(@"\w+://" + UriChars, RegexOptions.Compiled);
        public static readonly Regex UriLenient = new Regex(@"(\w+://)?" + UriChars, RegexOptions.Compiled);

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

        [Serializable]
        public class CompileException : Exception
        {
            public CompileException() { }
            public CompileException(string message) : base(message) { }
            public CompileException(string message, Exception inner) : base(message, inner) { }
            protected CompileException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }


        [Serializable]
        public class NativeCompileException : CompileException
        {
            public NativeCompileException() { }
            public NativeCompileException(string message) : base(message) { }
            public NativeCompileException(string message, Exception inner) : base(message, inner) { }
            protected NativeCompileException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }

#if !(NOSCREENSCRAPER)
    [Serializable]
    public class ScreenScraperTag
    {
        public string Name;
        public NameValueCollection Attributes = new NameValueCollection();

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

    [Serializable]
    public class ScrapedPage
    {
        protected string m_RawStream;
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

        private string _RawStreamLowercase;
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

        protected Uri m_Url;
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

        private NameValueCollection _QueryParameters;
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

        private ScrapeType _ScrapeType;
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

        private string _Title;
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

        public string FindSubstring(string pretext, string posttext, bool caseSensitive)
        {
            return FindSubstring(GetSubject(ref pretext, ref posttext, null, caseSensitive), pretext, posttext, caseSensitive);
        }

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
        /// <param name="Pretext"></param>
        /// <param name="Posttext"></param>
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

        public string FindSubstringByContext(string contextFind, string prettext, string posttext, bool caseSensitive)
        {
            return FindSubstring(GetSubject(ref prettext, ref posttext, contextFind, caseSensitive), prettext, posttext, caseSensitive);
        }

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

        private string CreateEndTag(string tagName)
        {
            int ltindex = tagName.IndexOf("<");
            if (ltindex != -1)
            {
                return tagName.Insert(ltindex + 1, "/");
            }
            return null;
        }

        public Pair<string, string> ConvertLinkToPair(string subject)
        {
            return ConvertLinkToPair(subject, true);
        }

        /// <summary>
        /// The first element in the pair is the HREF Link, and the second element
        /// is the text of the link.
        /// </summary>
        /// <param name="subject"></param>
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
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CanonicalizeString(string str)
        {
            return str == null ? null : str.Trim().Replace("\n", "").Replace("\r", "").Replace("\t", "");
        }

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

        public Match Match(string regex)
        {
            return Match(regex, true);
        }

        public Match Match(string regex, bool caseSensitive)
        {
            return new Regex(regex, caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase).Match(RawStream);
        }

        private static Regex tagRegex = new Regex(@"<([\w\-]+)(\s+([\w\-]+)\s*=\s*[""]([^""]*)[""])*\s*/?\s*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex nameValueRegex = new Regex(@"([\w\-]+)\s*=\s*[""]([^""]*)[""]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

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
        public static string FindCurrency(string subject)
        {
            Match m = CurrencyRegex.Match(subject);
            if (m.Success)
            {
                return m.Groups[1].ToString();
            }
            return null;
        }

        public static bool ConvertCurrencyStringToDecimal(string subject, out decimal ret)
        {
            ret = 0;
            return subject == null ? false : decimal.TryParse(subject, out ret);
        }

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

#if !(NOBROKEN)
        public static bool ConvertStringToDateTime(string subject, OlsonTimeZone timeZone, out OlsonDateTime ret)
        {
            return OlsonDateTime.TryParseLenient(subject, timeZone, System.Globalization.DateTimeStyles.AssumeUniversal, out ret);
        }
#endif
    }

    [Serializable]
    public class ScrapeSession
    {
        private Scraper _ContainingScraper;
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

        public ScrapeSession(Scraper scraper)
        {
            ContainingScraper = scraper;
        }

        protected CookieContainer m_Cookies = new CookieContainer();
        public CookieContainer Cookies
        {
            get
            {
                return m_Cookies;
            }
        }

        public void AddCookie(string name, string value)
        {
            Cookies.Add(new Cookie(name, value, "/", ContainingScraper.Domain));
        }
    }

    public enum ScrapeType
    {
        GET, POST
    }

    [Serializable]
    public class Scraper
    {
        public static int DefaultExternalCallTimeout = 12000;

        protected bool m_FollowEquivRefreshes = true;
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

        protected ScrapeSession m_Session;
        public ScrapeSession Session
        {
            get
            {
                return m_Session;
            }
        }

        protected string m_Referer;
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

        private Uri _LastProcessResponseUri;
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

        private string LastMetaFollow;

        private ScrapeType? _MetaRefreshScrapeType;
        /// <summary>
        /// If there is a meta refresh, then this specified
        /// the scrape type to use to follow the link. If this
        /// value is null, then the scrape type of the previous request
        /// is used.
        /// </summary>
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

        private string _Domain;

        /// <summary>
        /// This is used for a requests referer attribute as well as setting any cookies.
        /// This should be in the form "www.domain.com," without the prepended scheme.
        /// </summary>
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

        public Scraper()
            : this(null)
        {
        }

        /// <summary>
        /// This is used for a requests referer attribute as well as setting any cookies.
        /// This should be in the form "www.domain.com," without the prepended scheme.
        /// </summary>
        /// <param name="Domain"></param>
        public Scraper(string domain)
        {
            this.Domain = domain;
            m_Session = new ScrapeSession(this);
            if (this.Domain != null)
            {
                Referer = "http://" + this.Domain;
            }
        }

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

        public string HttpGet(string uri)
        {
            System.Net.HttpWebRequest req = CreateWebRequest(uri);
            return ProcessResponseStream(req);
        }

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
        public string TwoLetterCode;
        public string CountryName;

        public Iso3166(string twoLetterCode, string countryName)
        {
            TwoLetterCode = twoLetterCode;
            CountryName = countryName;
        }

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
        public int LatitudeDegrees;
        public int LatitudeMinutes;
        public int LatitudeSeconds;
        public bool IsLatitudeNorth;

        public int LongitudeDegrees;
        public int LongitudeMinutes;
        public int LongitudeSeconds;
        public bool IsLongitudeEast;

        public static Regex Iso6709Form1 = new Regex(@"(\+|-)(\d\d)(\d\d)(\+|-)(\d\d\d)(\d\d)", RegexOptions.Compiled);
        public static Regex Iso6709Form2 = new Regex(@"(\+|-)(\d\d)(\d\d)(\d\d)(\+|-)(\d\d\d)(\d\d)(\d\d)", RegexOptions.Compiled);

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
        public double m_latitude;
        public double m_longitude;

        public LatitudeLongitudePoint()
        {
        }

        public LatitudeLongitudePoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public LatitudeLongitudePoint(string latitude, string longitude)
        {
            Latitude = double.Parse(latitude);
            Longitude = double.Parse(longitude);
        }

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
        public int LatitudeDegrees
        {
            get
            {
                return (int)Latitude;
            }
        }

        public int LatitudeMinutes
        {
            get
            {
                return (int)((Latitude - (double)LatitudeDegrees) * 60D);
            }
        }

        public double LatitudeSeconds
        {
            get
            {
                return (((Latitude - (double)LatitudeDegrees) * 60D) - (double)LatitudeMinutes) * 60D;
            }
        }

        public int LongitudeDegrees
        {
            get
            {
                return (int)Longitude;
            }
        }

        public int LongitudeMinutes
        {
            get
            {
                return (int)((Longitude - (double)LongitudeDegrees) * 60D);
            }
        }

        public double LongitudeSeconds
        {
            get
            {
                return (((Longitude - (double)LongitudeDegrees) * 60D) - (double)LongitudeMinutes) * 60D;
            }
        }

        public override string ToString()
        {
            return "(" + Latitude + "," + Longitude + ")";
        }

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

        [Serializable]
        public class DateException : Exception
        {
            public DateException() { }
            public DateException(string message) : base(message) { }
            public DateException(string message, Exception inner) : base(message, inner) { }
            protected DateException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }

#if !(NOTZ)
    /// From http://www.twinsun.com/tz/tz-link.htm
    /// "The public-domain time zone database contains code
    /// and data that represent the history of local time
    /// for many representative locations around the globe."

#if !(NONUNIT)
    [TestFixture]
#endif
    public class TzParser
    {
        private const string Iso3166TabFile = @"C:\temp\tzdata\iso3166.tab";
        private const string ZoneTabFile = @"C:\temp\tzdata\zone.tab";

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
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <exception cref="PublicDomain.TzException" />
        private static void ReadDatabaseFile(FileInfo file, List<TzDataRule> rules, List<TzDataZone> zones, List<string[]> links)
        {
            string[] lines = File.ReadAllLines(file.FullName);
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

            public string Format;

            public int UntilYear;

            public Month UntilMonth = 0;

            public int UntilDay;

            public DayOfWeek? UntilDay_DayOfWeek;

            public TimeSpan UntilTime;

            public string Comment;

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

            public object Clone()
            {
                return (TzDataZone)MemberwiseClone();
            }

            public TzDataZone Clone(string line)
            {
                TzDataZone z = (TzDataZone)Clone();
                line = "Zone\t" + z.ZoneName + "\t" + string.Join("\t", StringUtilities.RemoveEmptyPieces(line.Split('\t')));
                ParsePieces(line, z);
                return z;
            }
        }

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

            public string Modifier;

            public string Comment;

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
        /// <param name="tabFile"></param>
        /// <returns></returns>
        public static Dictionary<string, Iso3166> ParseIso3166Tab(string iso3166TabFile)
        {
            Dictionary<string, Iso3166> map = new Dictionary<string, Iso3166>();
            string[] lines = File.ReadAllLines(iso3166TabFile);
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
            string[] lines = File.ReadAllLines(tabFile);
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

        [Serializable]
        public struct TzZoneDescription
        {
            public string TwoLetterCode;
            public Iso6709 Location;
            public string ZoneName;
            public string Comments;

            public TzZoneDescription(string twoLetterCode, Iso6709 location, string zoneName, string comments)
            {
                TwoLetterCode = twoLetterCode;
                Location = location;
                ZoneName = zoneName;
                Comments = comments;
            }

            public override string ToString()
            {
                return string.Format("{0}\t{1}\t{2}\t{3}", TwoLetterCode, Location, ZoneName, Comments);
            }
        }

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

        private static TimeSpan GetTzDataTime(string saveTime)
        {
            if (char.IsLetter(saveTime[saveTime.Length - 1]))
            {
                saveTime = saveTime.Substring(0, saveTime.Length - 1);
            }
            return TimeSpan.Parse(saveTime);
        }
    }

    [Serializable]
    public class TzException : Exception
    {
        public TzException() { }
        public TzException(string message) : base(message) { }
        public TzException(string message, Exception inner) : base(message, inner) { }
        protected TzException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }


    [Serializable]
    public class TzParseException : TzException
    {
        public TzParseException() { }
        public TzParseException(string message) : base(message) { }
        public TzParseException(string message, Exception inner) : base(message, inner) { }
        protected TzParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class StandardTime : DaylightTime
    {
        public StandardTime(DateTime start, DateTime end, TimeSpan delta)
            : base(start, end, delta)
        {
        }
    }

    /// <summary>
    /// Represents a Time Zone from the Olson tz database.
    /// </summary>
    [Serializable]
    public class TzTimeZone : TimeZone
    {
        public const string TimzoneUsEastern = "US/Eastern";
        public const string TimezoneUsCentral = "US/Central";
        public const string TimezoneUsMountain = "US/Mountain";
        public const string TimezoneUsPacific = "US/Pacific";

        private string m_standardName;
        private string m_daylightName;

        public TzTimeZone(string standardName)
        {
            m_standardName = standardName;
            m_daylightName = standardName;
        }

        public override string StandardName
        {
            get
            {
                return m_standardName;
            }
        }

        public override string DaylightName
        {
            get
            {
                return m_daylightName;
            }
        }

        public override DaylightTime GetDaylightChanges(int year)
        {
            return null;
        }

        public override TimeSpan GetUtcOffset(DateTime time)
        {
            return new TimeSpan();
        }

        public override bool IsDaylightSavingTime(DateTime time)
        {
            return base.IsDaylightSavingTime(time);
        }

        public override DateTime ToLocalTime(DateTime time)
        {
            return base.ToLocalTime(time);
        }

        public override DateTime ToUniversalTime(DateTime time)
        {
            return base.ToUniversalTime(time);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    /// <summary>
    /// Wraps DateTime to provide time zone information
    /// with an <see cref="PublicDomain.TzTimeZone"> from
    /// the Olson tz database.
    /// </summary>
    [Serializable]
    public class TzDateTime
    {
    }

#endif

    /// <summary>
    /// Methods and date related to the United States, such as a list
    /// of States.
    /// </summary>
#if !(NOSTATES)
    public static class UnitedStatesUtilities
    {
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
        /// <returns>The <see cref="PublicDomain.UnitedStatesUtilities.USState"/> that represents the
        /// <c>abbreviation</c>, or if it is not found, throws a <see cref="PublicDomain.UnitedStatesUtilities.StateNotFoundException"/></returns>
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
        /// <returns>The <see cref="PublicDomain.UnitedStatesUtilities.USState"/> that represents the
        /// <c>abbreviation</c>, or if it is not found, throws a <see cref="PublicDomain.UnitedStatesUtilities.StateNotFoundException"/></returns>
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

        [Serializable]
        public struct USState
        {
            public int UniqueId;
            public string Name;
            public string Abbreviation;

            public USState(int uniqueId, string name, string abbreviation)
            {
                this.UniqueId = uniqueId;
                this.Name = name;
                this.Abbreviation = abbreviation;
            }

            public string GetName(bool toUpperCase)
            {
                return toUpperCase ? Name.ToUpper() : Name.ToLower();
            }

            public string GetAbbreviation(bool toUpperCase)
            {
                return toUpperCase ? Abbreviation.ToUpper() : Abbreviation.ToLower();
            }
        }

        [Serializable]
        public class StateNotFoundException : Exception
        {
            public StateNotFoundException() { }
            public StateNotFoundException(string message) : base(message) { }
            public StateNotFoundException(string message, Exception inner) : base(message, inner) { }
            protected StateNotFoundException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }
#endif

    #region Enums

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public enum Language
    {
        CSharp,
        PHP,
        JSharp,
        CPlusPlus,
        JScript,
        VisualBasic,
        Java,
        Ruby,
    }

    public enum DistanceType
    {
        StatuteMiles,
        NauticalMiles,
        Kilometers
    }

    public enum ReaderWriterLockSynchronizeType
    {
        Read, Write
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
