using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SpellCheckerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpellCheckerException"/> class.
        /// </summary>
        public SpellCheckerException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellCheckerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SpellCheckerException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellCheckerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public SpellCheckerException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellCheckerException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected SpellCheckerException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SpellChecker
    {
        /// <summary>
        /// Always ends in a trailing slash.
        /// </summary>
        public const string AspellDirectory = @"c:\aspell\";

        /// <summary>
        /// 
        /// </summary>
        public const string AspellDllLocation = AspellDirectory + @"aspelldll.dll";

        /// <summary>
        /// Always ends in a trailing slash.
        /// </summary>
        public const string DefaultDataDirectory = AspellDirectory + @"data\";

        /// <summary>
        /// Always ends in a trailing slash.
        /// </summary>
        public const string DefaultDictionaryDirectory = AspellDirectory + @"dict\";

        private const int CheckBufferSize = 512;
        private const int SuggestBufferSize = 1024;

        /// <summary>
        /// Checks the specified datadir.
        /// </summary>
        /// <param name="datadir">The datadir.</param>
        /// <param name="dictdir">The dictdir.</param>
        /// <param name="language">The language.</param>
        /// <param name="word">The word.</param>
        /// <param name="retval">The retval.</param>
        /// <param name="errorMsg">The error MSG.</param>
        /// <returns>0 if successful</returns>
        [DllImport(AspellDllLocation)]
        public static extern int check(string datadir, string dictdir, string language, string word, out int retval, ref WordStruct errorMsg);

        /// <summary>
        /// Suggests the specified datadir.
        /// </summary>
        /// <param name="datadir">The datadir.</param>
        /// <param name="dictdir">The dictdir.</param>
        /// <param name="language">The language.</param>
        /// <param name="word">The word.</param>
        /// <param name="errorMsg">The error MSG.</param>
        /// <param name="suggest">The suggest.</param>
        /// <returns>0 if successful</returns>
        [DllImport(AspellDllLocation)]
        public static extern int suggest(string datadir, string dictdir, string language, string word, ref WordStruct errorMsg, ref WordStruct suggest);

        private static string s_dataDirectory = DefaultDataDirectory;

        /// <summary>
        /// Gets or sets the data directory.
        /// </summary>
        /// <value>The data directory.</value>
        public static string DataDirectory
        {
            get
            {
                return s_dataDirectory;
            }
            set
            {
                s_dataDirectory = value;
            }
        }

        private static string s_dictionaryDirectory = DefaultDictionaryDirectory;

        /// <summary>
        /// Gets or sets the dictionary directory.
        /// </summary>
        /// <value>The dictionary directory.</value>
        public static string DictionaryDirectory
        {
            get
            {
                return s_dictionaryDirectory;
            }
            set
            {
                s_dictionaryDirectory = value;
            }
        }

        /// <summary>
        /// Checks the word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public static bool IsWordSpelledCorrectly(string word)
        {
            string language = GetLanguage();

            // initialize buffer and append something to the end so whole
            // buffer is passed to unmanaged side
            StringBuilder buffer = new StringBuilder(CheckBufferSize);
            buffer.Append((char)0);
            buffer.Append('*', buffer.Capacity - 8);

            WordStruct buf;
            buf.Buffer = buffer.ToString();
            buf.Size = buf.Buffer.Length;

            int retval;
            if (check(DataDirectory, DictionaryDirectory, language, word, out retval, ref buf) != 0)
            {
                throw new SpellCheckerException(buf.Buffer);
            }
            return retval == 1 ? true : false;
        }

        private static string GetLanguage()
        {
            return "en_US";
        }

        /// <summary>
        /// Suggests the specified word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public static List<string> SuggestWords(string word)
        {
            string language = GetLanguage();
            List<string> ret = new List<string>();

            StringBuilder buffer = new StringBuilder(SuggestBufferSize);
            buffer.Append((char)0);
            buffer.Append('*', buffer.Capacity - 8);

            WordStruct buf;
            buf.Buffer = buffer.ToString();
            buf.Size = buf.Buffer.Length;

            WordStruct suggestStruct;
            suggestStruct.Buffer = buffer.ToString();
            suggestStruct.Size = suggestStruct.Buffer.Length;

            if (!IsWordSpelledCorrectly(word))
            {
                if (suggest(DataDirectory, DictionaryDirectory, language, word, ref buf, ref suggestStruct) != 0)
                {
                    throw new SpellCheckerException(buf.Buffer);
                }

                string[] suggestions = suggestStruct.Buffer.ToString().Split(',');
                foreach (string str in suggestions)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        ret.Add(str);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WordStruct
        {
            /// <summary>
            /// 
            /// </summary>
            public string Buffer;

            /// <summary>
            /// 
            /// </summary>
            public int Size;
        }
    }
}
