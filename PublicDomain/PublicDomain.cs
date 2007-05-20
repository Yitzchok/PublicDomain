// PublicDomain
// ======================================
//  Original Author: Kevin Grigorenko (kevgrig@gmail.com)
//      Download README.txt for a list of contributing authors
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
// NOTE: Some code documentation may appear wrongly worded. This is due to auto-documentation
// using GhostDoc (http://www.roland-weigelt.de/ghostdoc/).
//
// Version History: Download README.txt for the version history.
//

#region Directives
// The following section provides
// directives for conditional compilation
// of various sections of the code.
// ======================================

// !!!EDIT DIRECTIVES START!!!

#if !(PD)

// Commonly non-referenced projects:
#define NOVJSLIB
#define NOSYSTEMWEB
#define NOASPELL

// Other switches:
//#define NOSCREENSCRAPER
//#define NOFEEDER
//#define NOCLSCOMPLIANTWARNINGSOFF
//#define NOTZ
//#define NOSTATES
//#define NOCODECOUNT
//#define NOLOGGING
//#define NODYNACODE
//#define NOASPNETRUNTIMEHOST

#endif

// !!!EDIT DIRECTIVES END!!!!!

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
using java.util.zip;
#endif
#if !(NOSYSTEMWEB)
using System.Web;
#endif

// Core includes
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Permissions;
//using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Serialization;
using PublicDomain;
using PublicDomain.Feeder.Opml;
using PublicDomain.Feeder.Rss;
using PublicDomain.Feeder.Atom;
using PublicDomain.Dynacode;
using Microsoft.Win32;

#if !(NOCLSCOMPLIANTWARNINGSOFF)
#pragma warning disable 3001
#pragma warning disable 3002
#pragma warning disable 3003
#pragma warning disable 3006
#pragma warning disable 3009
#endif

#if !(NOCODECOUNT)
namespace PublicDomain.CodeCount
{
    /// <summary>
    /// Represents something that can be counted.
    /// </summary>
    public interface ICountable
    {
        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        List<ICountable> Children
        {
            get;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type
        {
            get;
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>The location.</value>
        string Location
        {
            get;
        }

        /// <summary>
        /// Counts the lines.
        /// </summary>
        /// <returns></returns>
        long CountLines();
    }

    /// <summary>
    /// Abstract implementation of <see cref="PublicDomain.CodeCount.ICountable"/>
    /// </summary>
    public abstract class Countable : ICountable
    {
        private List<ICountable> m_children = new List<ICountable>();

        /// <summary>
        /// 
        /// </summary>
        protected string m_name;

        /// <summary>
        /// 
        /// </summary>
        protected string m_type;

        /// <summary>
        /// 
        /// </summary>
        protected string m_location;

        #region ICountable Members

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        public virtual List<ICountable> Children
        {
            get
            {
                return m_children;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name
        {
            get
            {
                return m_name;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public virtual string Type
        {
            get
            {
                return m_type;
            }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>The location.</value>
        public virtual string Location
        {
            get
            {
                return m_location;
            }
        }

        #endregion

        /// <summary>
        /// Counts the lines.
        /// </summary>
        /// <returns></returns>
        public abstract long CountLines();

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return Location;
        }
    }

    /// <summary>
    /// Abstract stream of countable items
    /// </summary>
    public abstract class CountStream : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected ICountable m_first;

        /// <summary>
        /// 
        /// </summary>
        protected long? m_length;

        /// <summary>
        /// 
        /// </summary>
        protected CountStreamType m_type;

        /// <summary>
        /// 
        /// </summary>
        protected string m_location;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountStream"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="location">The location.</param>
        public CountStream(CountStreamType type, string location)
        {
            m_type = type;
            m_location = location;
            switch (type)
            {
                case CountStreamType.Directory:
                    m_first = new CountableDirectory(location);
                    break;
                case CountStreamType.VSSolution2005:
                    m_first = new DotNetSolution(location);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Opens the specified type.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static CountStream Open(string location, CountStreamType type)
        {
            return new DepthFirstCountStream(type, location);
        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        public abstract ICountable Read();

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public abstract void Cancel();

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <returns></returns>
        public long GetLength()
        {
            if (m_length == null)
            {
                m_length = 0;

                // Create a temporary stream with the same type
                // and location, and we will count up how many countables
                // we have
                using (CountStream privateStream = CountStream.Open(m_location, m_type))
                {
                    while (privateStream.Read() != null)
                    {
                        m_length++;
                    }
                }
            }
            return m_length.Value;
        }
    }

    /// <summary>
    /// Enumeration of known countable stream types
    /// </summary>
    public enum CountStreamType
    {
        /// <summary>
        /// 
        /// </summary>
        Directory,
        
        /// <summary>
        /// 
        /// </summary>
        VSSolution2005
    }

    /// <summary>
    /// Represents a file that is countable
    /// </summary>
    public class CountableFile : Countable
    {
        private FileInfo m_file;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountableFile"/> class.
        /// </summary>
        /// <param name="fileLocation">The file location.</param>
        public CountableFile(string fileLocation)
            : this(new FileInfo(fileLocation))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountableFile"/> class.
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        public CountableFile(FileInfo fileInfo)
        {
            m_file = fileInfo;
            if (!m_file.Exists)
            {
                throw new ArgumentException("File " + Location + " does not exist.");
            }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>The location.</value>
        public override string Location
        {
            get
            {
                return m_file.FullName;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get
            {
                return m_file.Name;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public override string Type
        {
            get
            {
                return m_file.Extension;
            }
        }

        /// <summary>
        /// Counts the lines.
        /// </summary>
        /// <returns></returns>
        public override long CountLines()
        {
            long result = 0;

            string line;
            using (StreamReader stream = m_file.OpenText())
            {
                while ((line = stream.ReadLine()) != null)
                {
                    if (IsCountable(ref line))
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified line is countable.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>
        /// 	<c>true</c> if the specified line is countable; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsCountable(ref string line)
        {
            line = line.Trim();
            if (line.Length == 0)
            {
                // Emtpy lines do not count
                return false;
            }
            if (line.StartsWith("//"))
            {
                // Single line comment
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Represents a dountable directory
    /// </summary>
    public class CountableDirectory : Countable
    {
        private DirectoryInfo m_dir;
        private static List<string> m_validExtensions = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CountableDirectory"/> class.
        /// </summary>
        /// <param name="directoryInfo">The directory info.</param>
        public CountableDirectory(DirectoryInfo directoryInfo)
        {
            m_dir = directoryInfo;

            if (!m_dir.Exists)
            {
                throw new ArgumentException("Directory " + m_dir.FullName + " does not exist.");
            }

            Load();
        }

        /// <summary>
        /// Initializes the <see cref="CountableDirectory"/> class.
        /// </summary>
        static CountableDirectory()
        {
            m_validExtensions.Add(".cs");
            m_validExtensions.Add(".aspx");
            m_validExtensions.Add(".ascx");
            m_validExtensions.Add(".xml");
            m_validExtensions.Add(".asax");
            m_validExtensions.Add(".config");
            m_validExtensions.Add(".js");
            m_validExtensions.Add(".java");
            m_validExtensions.Add(".php");
            m_validExtensions.Add(".master");
            m_validExtensions.Add(".htm");
            m_validExtensions.Add(".html");
            m_validExtensions.Add(".asmx");
            m_validExtensions.Add(".css");
            m_validExtensions.Add(".cpp");
            m_validExtensions.Add(".h");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountableDirectory"/> class.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        public CountableDirectory(string directoryName)
            : this(new DirectoryInfo(directoryName))
        {
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        protected virtual void Load()
        {
            Children.Clear();

            DirectoryInfo[] directories = m_dir.GetDirectories();
            foreach (DirectoryInfo directory in directories)
            {
                if (IsCountableDirectory(directory))
                {
                    Children.Add(new CountableDirectory(directory));
                }
            }

            // Load up all the files in the directory
            FileInfo[] files = m_dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (IsCountableFile(file))
                {
                    Children.Add(new CountableFile(file.FullName));
                }
            }
        }

        /// <summary>
        /// Determines whether [is countable file] [the specified file].
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        /// 	<c>true</c> if [is countable file] [the specified file]; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsCountableFile(FileInfo file)
        {
            return m_validExtensions.Contains(file.Extension.ToLower());
        }

        /// <summary>
        /// Determines whether [is countable directory] [the specified directory].
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>
        /// 	<c>true</c> if [is countable directory] [the specified directory]; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsCountableDirectory(DirectoryInfo directory)
        {
            return directory.Name != ".svn";
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>The location.</value>
        public override string Location
        {
            get
            {
                return m_dir.FullName;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get
            {
                return m_dir.Name;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public override string Type
        {
            get
            {
                return base.Type;
            }
        }

        /// <summary>
        /// Counts the lines.
        /// </summary>
        /// <returns></returns>
        public override long CountLines()
        {
            return 0;
        }
    }

    /// <summary>
    /// Represents a .NET solution which is countable
    /// </summary>
    public class DotNetSolution : Countable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetSolution"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public DotNetSolution(string filename)
        {
        }

        /// <summary>
        /// Counts the lines.
        /// </summary>
        /// <returns></returns>
        public override long CountLines()
        {
            return 0;
        }
    }

    /// <summary>
    /// Represents a .NET project which is countable
    /// </summary>
    public class DotNetProject : Countable
    {
        /// <summary>
        /// Counts the lines.
        /// </summary>
        /// <returns></returns>
        public override long CountLines()
        {
            return 0;
        }
    }

    /// <summary>
    /// Counts a stream depth first
    /// </summary>
    public class DepthFirstCountStream : CountStream
    {
        private Stack<Pair<ICountable, int>> m_stack = new Stack<Pair<ICountable, int>>();

        /// <summary>
        /// 
        /// </summary>
        protected ICountable m_current;

        /// <summary>
        /// 
        /// </summary>
        protected int m_currentChildIndex;

        /// <summary>
        /// 
        /// </summary>
        protected bool m_finished;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstCountStream"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="location">The location.</param>
        public DepthFirstCountStream(CountStreamType type, string location)
            : base(type, location)
        {
        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        public override ICountable Read()
        {
            if (m_finished)
            {
                return null;
            }

            // If current is NULL, then this is the first ICountable to read
            if (m_current == null)
            {
                m_current = m_first;
                m_currentChildIndex = -1;
                m_stack.Push(new Pair<ICountable, int>(m_current, 0));
            }

            // If the current index is -1, we return the current item and
            // increment the current child index
            if (m_currentChildIndex == -1)
            {
                m_currentChildIndex++;
                return m_current;
            }

            // Check if we have read all children, recursively
            while (m_current.Children.Count == 0 || m_current.Children.Count == m_currentChildIndex)
            {
                Pair<ICountable, int> last = m_stack.Pop();
                m_current = last.First;
                m_currentChildIndex = last.Second;

                if (m_current == m_first && m_current.Children.Count == m_currentChildIndex)
                {
                    m_finished = true;
                    return null;
                }
            }

            ICountable result = m_current.Children[m_currentChildIndex];

            m_stack.Push(new Pair<ICountable, int>(m_current, m_currentChildIndex + 1));
            m_currentChildIndex = 0;
            m_current = result;

            return result;
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public override void Cancel()
        {
            m_finished = true;
        }
    }
}
#endif

#if !(NOSCREENSCRAPER)
namespace PublicDomain.ScreenScraper
{
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
            return TzDateTime.TryParse(subject, timeZone, DateTimeStyles.AssumeUniversal, out ret);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return RawStream;
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
    /// This class is not thread safe.
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

        private int m_timeout = 100000;

        /// <summary>
        /// The number of milliseconds to wait before the request times out. The default
        /// is 100,000 milliseconds (100 seconds).
        /// </summary>
        /// <value>The timeout.</value>
        public int Timeout
        {
            get
            {
                return m_timeout;
            }
            set
            {
                m_timeout = value;
            }
        }

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

        private ICredentials m_credentials;

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        /// <value>The credentials.</value>
        public ICredentials Credentials
        {
            get
            {
                return m_credentials;
            }
            set
            {
                m_credentials = value;
            }
        }

        private bool m_useCredentials = true;

        /// <summary>
        /// Gets or sets a value indicating whether [use credentials].
        /// </summary>
        /// <value><c>true</c> if [use credentials]; otherwise, <c>false</c>.</value>
        public bool UseCredentials
        {
            get
            {
                return m_useCredentials;
            }
            set
            {
                m_useCredentials = value;
            }
        }

        /// <summary>
        /// Sets the network credentials.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public void SetNetworkCredentials(string user, string password)
        {
            SetNetworkCredentials(user, password, null);
        }

        /// <summary>
        /// Sets the network credentials.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <param name="domain">The domain.</param>
        public void SetNetworkCredentials(string user, string password, string domain)
        {
            Credentials = new NetworkCredential(user, password, domain);
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
        /// Requireses the credentials.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public bool RequiresCredentials(ScrapeType type, string uri, NameValueCollection query)
        {
            try
            {
                UseCredentials = false;
                Scrape(type, uri, query);
            }
            catch (System.Net.WebException ex)
            {
                HttpWebResponse response = ex.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return true;
                }
            }
            finally
            {
                UseCredentials = true;
            }

            return false;
        }

        /// <summary>
        /// Requireses the credentials.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="keyAndValuePairs">The key and value pairs.</param>
        /// <returns></returns>
        public bool RequiresCredentials(ScrapeType type, string uri, params string[] keyAndValuePairs)
        {
            try
            {
                UseCredentials = false;
                Scrape(type, uri, keyAndValuePairs);
            }
            catch (System.Net.WebException ex)
            {
                HttpWebResponse response = ex.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return true;
                }
            }
            finally
            {
                UseCredentials = true;
            }

            return false;
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
            req.Timeout = Timeout;
            if (UseCredentials)
            {
                req.Credentials = Credentials;
            }
            return req;
        }
    }
}
#endif

#if !(NOFEEDER)
namespace PublicDomain.Feeder
{
    /// <summary>
    /// Attempts to distill any feed format (RSS, Atom, etc.) into
    /// a form that can be dealt with more logically. Information IS
    /// lost in this process, and very few fields can be assumed to
    /// have any data. Actually, almost none *must* have content.
    /// </summary>
    public interface IDistilledFeed : ICachedPropertiesProvider
    {
        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        Uri FeedUri { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        string Generator { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        string Category { get; set; }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        TzDateTime PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the last changed.
        /// </summary>
        /// <value>The last changed.</value>
        TzDateTime LastChanged { get; set; }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        /// <value>The time to live.</value>
        int? TimeToLive { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        IList<IDistilledFeedItem> Items { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DistilledFeed : CachedPropertiesProvider, IDistilledFeed
    {
        private IList<IDistilledFeedItem> m_items = new List<IDistilledFeedItem>();

        #region IDistilledFeed Members

        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        public Uri FeedUri
        {
            get
            {
                return Getter<Uri>("DistilledFeedUri", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("DistilledFeedUri", value);
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return Getter("DistilledTitle");
            }
            set
            {
                Setter("DistilledTitle", value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("DistilledDescription");
            }
            set
            {
                Setter("DistilledDescription", value);
            }
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public System.Globalization.CultureInfo Culture
        {
            get
            {
                return Getter<System.Globalization.CultureInfo>("DistilledCulture", CachedPropertiesProvider.ConvertToCultureInfo);
            }
            set
            {
                Setter("DistilledCulture", value);
            }
        }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        public string Copyright
        {
            get
            {
                return Getter("DistilledCopyright");
            }
            set
            {
                Setter("DistilledCopyright", value);
            }
        }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        public string Generator
        {
            get
            {
                return Getter("DistilledGenerator");
            }
            set
            {
                Setter("DistilledGenerator", value);
            }
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category
        {
            get
            {
                return Getter("DistilledCategory");
            }
            set
            {
                Setter("DistilledCategory", value);
            }
        }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        public TzDateTime PublicationDate
        {
            get
            {
                return Getter<TzDateTime>("DistilledPublicationDate", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("DistilledPublicationDate", value);
            }
        }

        /// <summary>
        /// Gets or sets the last changed.
        /// </summary>
        /// <value>The last changed.</value>
        public TzDateTime LastChanged
        {
            get
            {
                return Getter<TzDateTime>("DistilledLastChanged", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("DistilledLastChanged", value);
            }
        }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        /// <value>The time to live.</value>
        public int? TimeToLive
        {
            get
            {
                return Getter<int?>("DistilledTimeToLive", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("DistilledTimeToLive", value);
            }
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public IList<IDistilledFeedItem> Items
        {
            get
            {
                return m_items;
            }
            set
            {
                m_items = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// Similar to IDistilledFeed, this
    /// attempts to find the common denominator for a 
    /// feed entry or item. Few of these fields may be
    /// assumed to contain data.
    /// </summary>
    public interface IDistilledFeedItem : ICachedPropertiesProvider
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link { get; set; }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        TzDateTime PublicationDate { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DistilledFeedItem : CachedPropertiesProvider, IDistilledFeedItem
    {
        #region IDistilledFeedItem Members

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return Getter("DistilledTitle");
            }
            set
            {
                Setter("DistilledTitle", value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("DistilledDescription");
            }
            set
            {
                Setter("DistilledDescription", value);
            }
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get
            {
                return Getter<Uri>("DistilledLink", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("DistilledLink", value);
            }
        }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        public TzDateTime PublicationDate
        {
            get
            {
                return Getter<TzDateTime>("DistilledPublicationDate", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("DistilledPublicationDate", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IXmlFeed : ICachedPropertiesProvider
    {
        /// <summary>
        /// Gets or sets the raw contents.
        /// </summary>
        /// <value>The raw contents.</value>
        string RawContents { get; set; }
    }

    /// <summary>
    /// Common denominator for a feed. If you are looking for
    /// specific properties, yet you don't want to lose information
    /// through distilling, then you need to conditionally check
    /// the dynamic type of this instance and cast to that type
    /// (e.g. IRssFeed or IAtomFeed). If you are expecting a specific
    /// type of feed, then you can just cast to that type of feed; however,
    /// note that you can never guarantee the dynamic type of the
    /// instance, since the type that is instantiated is determined at
    /// run-time by sniffing out the feed.
    /// </summary>
    public interface IFeed : IXmlFeed
    {
        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        Uri FeedUri { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        IList<IFeedItem> Items { get; set; }

        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        IDistilledFeed Distill();
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class Feed : CachedPropertiesProvider, IFeed
    {
        private IList<IFeedItem> m_items = new List<IFeedItem>();
        private string rawContents;

        #region IFeed Members

        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        public Uri FeedUri
        {
            get
            {
                return Getter<Uri>("FeedUri", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("FeedUri", value);
            }
        }

        /// <summary>
        /// Gets or sets the raw contents.
        /// </summary>
        /// <value>The raw contents.</value>
        public string RawContents
        {
            get
            {
                return rawContents;
            }
            set
            {
                rawContents = value;
            }
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public IList<IFeedItem> Items
        {
            get
            {
                return m_items;
            }
            set
            {
                m_items = value;
            }
        }

        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        public abstract IDistilledFeed Distill();

        #endregion
    }

    /// <summary>
    /// Base interface that represents a feed item or entry.
    /// As for IFeed, the best way to get to intelligible properties,
    /// if not distilling into a IDistilledFeedItem, is
    /// to cast to specific sub-interfaces of IFeedItem and conditionally
    /// use those or assume that it is a specific item type. Again,
    /// casting to a specific sub-interface is not completely predictable.
    /// </summary>
    public interface IFeedItem : ICachedPropertiesProvider
    {
        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        IDistilledFeedItem Distill();
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class FeedItem : CachedPropertiesProvider, IFeedItem
    {
        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        public abstract IDistilledFeedItem Distill();
    }

    /// <summary>
    /// The FeedParser is, from the client's perspective, the
    /// entry point into the framework. The static methods of
    /// this class should be used to instantiate IFeeds
    /// which can then be manipulated.
    /// </summary>
    public class FeedParser : Parser
    {
        /// <summary>
        /// Creates the feed base.
        /// </summary>
        /// <param name="feedReader">The feed reader.</param>
        /// <returns></returns>
        protected override T CreateFeedBase<T>(XmlReader feedReader)
        {
            feedReader.MoveToContent();
            return new FeedParser().Parse<T>(feedReader);
        }

        #region IFeedParser Members

        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override T Parse<T>(System.Xml.XmlReader reader)
        {
            // Need a specific parser
            IParser subparser = null;
            IFeed ret = null;

            string localRootName = reader.LocalName.ToLower().Trim();
            if (localRootName == "rss" || localRootName == "rdf")
            {
                subparser = new RssFeedParser();
            }
            else if (localRootName == "feed")
            {
                subparser = new AtomFeedParser();
            }
            if (subparser != null)
            {
                using (XmlReader subreader = reader.ReadSubtree())
                {
                    ret = (IFeed)subparser.Parse<T>(subreader);
                }
            }
            else
            {
                throw new Exception(string.Format("Unknown feed type '{0}'.", reader.Name));
            }
            reader.Close();
            return (T)ret;
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Creates the feed.
        /// </summary>
        /// <param name="feedUri">The feed URI.</param>
        /// <param name="saveStream">if set to <c>true</c> [save stream].</param>
        /// <returns></returns>
        T CreateFeed<T>(Uri feedUri, bool saveStream) where T : IXmlFeed;

        /// <summary>
        /// Creates the feed.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        T CreateFeed<T>(System.IO.Stream input) where T : IXmlFeed;

        /// <summary>
        /// Creates the feed.
        /// </summary>
        /// <param name="feedUri">The feed URI.</param>
        /// <returns></returns>
        T CreateFeed<T>(string feedUri) where T : IXmlFeed;

        /// <summary>
        /// Creates the feed from stream.
        /// </summary>
        /// <param name="rawContent">Content of the raw.</param>
        /// <returns></returns>
        T CreateFeedFromStream<T>(string rawContent) where T : IXmlFeed;

        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        T Parse<T>(System.Xml.XmlReader reader);
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class Parser : Feeder.IParser
    {
        /// <summary>
        /// Gets the default XML reader settings.
        /// </summary>
        /// <returns></returns>
        protected static XmlReaderSettings GetDefaultXmlReaderSettings()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            // MSDN doesn't prefer this, but it seems to avoid a lot
            // of issues.
            settings.ProhibitDtd = false;
            return settings;
        }

        internal static void UnhandledElement(ICachedPropertiesProvider propsProvider, XmlReader reader)
        {
            string name = reader.Name;
            string val = reader.ReadOuterXml();
            if (propsProvider.Properties.ContainsKey(name))
            {
                val = propsProvider.Properties[name] + val;
            }
            propsProvider.Properties[name] = val;
        }

        /// <summary>
        /// Creates the XML reader from string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        protected static XmlReader CreateXmlReaderFromString(string input)
        {
            StringReader stringReader = new StringReader(input);
            return XmlReader.Create(stringReader);
        }

        /// <summary>
        /// Reads the URI stream.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string ReadUriStream(string uri)
        {
            return ReadUriStream(new Uri(uri));
        }

        /// <summary>
        /// Reads the URI stream.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static string ReadUriStream(string uri, int timeout)
        {
            return ReadUriStream(new Uri(uri));
        }

        /// <summary>
        /// Reads the URI stream.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string ReadUriStream(Uri uri)
        {
            return ReadUriStream(uri, 2000);
        }

        /// <summary>
        /// Reads the URI stream.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static string ReadUriStream(Uri uri, int timeout)
        {
            string wholeResponse = null;
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(uri);
            //hwr.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; Maxthon; .NET CLR 1.1.4322)";
            hwr.Timeout = timeout;
            HttpWebResponse wr = null;
            try
            {
                wr = (HttpWebResponse)hwr.GetResponse();
                Stream responseStream = wr.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                wholeResponse = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                wr.Close();
                wr = null;
            }
            finally
            {
                if (wr != null) wr.Close();
            }
            return wholeResponse;
        }

        /// <summary>
        /// Creates the feed from stream.
        /// </summary>
        /// <param name="rawContent">Content of the raw.</param>
        /// <returns></returns>
        public T CreateFeedFromStream<T>(string rawContent) where T : IXmlFeed
        {
            byte[] b = new byte[rawContent.Length];
            Encoding.ASCII.GetBytes(rawContent.ToCharArray(),
                        0,
                        rawContent.Length,
                        b,
                        0);
            using (MemoryStream ms = new MemoryStream(b))
            {
                T ret = (T)CreateFeed<T>(ms);
                ret.RawContents = rawContent;
                return ret;
            }
        }

        /// <summary>
        /// Parses the specified URI into an IFeed.
        /// </summary>
        /// <param name="feedUri">Valid URI to an accessible resource stream.</param>
        /// <returns>IFeed representing the feed.</returns>
        public T CreateFeed<T>(string feedUri) where T : IXmlFeed
        {
            return CreateFeed<T>(new Uri(feedUri), true);
        }

        /// <summary>
        /// Parses the specified URI into an IFeed.
        /// </summary>
        /// <param name="feedUri">Valid URI to an accessible resource stream.</param>
        /// <param name="saveStream">if set to <c>true</c> [save stream].</param>
        /// <returns>IFeed representing the feed.</returns>
        public T CreateFeed<T>(Uri feedUri, bool saveStream) where T : IXmlFeed
        {
            if (saveStream)
            {
                string stream = ReadUriStream(feedUri);
                using (StringReader reader = new StringReader(stream))
                {
                    T feed = CreateFeedBase<T>(XmlReader.Create(reader, GetDefaultXmlReaderSettings()));
                    feed.RawContents = stream;
                    return feed;
                }
            }
            else
            {
                return CreateFeedBase<T>(XmlReader.Create(feedUri.ToString(), GetDefaultXmlReaderSettings()));
            }
        }

        /// <summary>
        /// Parses the specified stream into an IFeed.
        /// The parser does not close the stream.
        /// </summary>
        /// <param name="input">An open stream to a feed stream.</param>
        /// <returns>IFeed representing the feed.</returns>
        public T CreateFeed<T>(Stream input) where T : IXmlFeed
        {
            return CreateFeedBase<T>(XmlReader.Create(input, GetDefaultXmlReaderSettings()));
        }

        /// <summary>
        /// Creates the feed base.
        /// </summary>
        /// <param name="feedReader">The feed reader.</param>
        /// <returns></returns>
        protected abstract T CreateFeedBase<T>(XmlReader feedReader) where T : IXmlFeed;

        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public abstract T Parse<T>(System.Xml.XmlReader reader);
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class Serializer
    {
        /// <summary>
        /// Appends the new element.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <returns></returns>
        public static XmlElement AppendNewElement(XmlDocument doc, XmlNode parent, string elementName)
        {
            return AppendNewElement(doc, parent, elementName, null);
        }

        /// <summary>
        /// Appends the new element.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="elementValue">The element value.</param>
        /// <returns></returns>
        public static XmlElement AppendNewElement(XmlDocument doc, XmlNode parent, string elementName, string elementValue)
        {
            return AppendNewElement(doc, parent, elementName, elementValue, doc.NamespaceURI);
        }

        /// <summary>
        /// Appends the new element.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="elementValue">The element value.</param>
        /// <param name="elementNamespace">The element namespace.</param>
        /// <returns></returns>
        public static XmlElement AppendNewElement(XmlDocument doc, XmlNode parent, string elementName, string elementValue, string elementNamespace)
        {
            XmlElement ret = doc.CreateElement(elementName, elementNamespace);
            parent.AppendChild(ret);
            if (elementValue != null)
            {
                ret.InnerText = elementValue;
            }
            return ret;
        }

        /// <summary>
        /// Appends the new attribute.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <returns></returns>
        public static XmlAttribute AppendNewAttribute(XmlDocument doc, XmlNode parent, string attributeName, string attributeValue)
        {
            return AppendNewAttribute(doc, parent, attributeName, attributeValue, doc.NamespaceURI);
        }

        /// <summary>
        /// Appends the new attribute.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <param name="attributeNamespace">The attribute namespace.</param>
        /// <returns></returns>
        public static XmlAttribute AppendNewAttribute(XmlDocument doc, XmlNode parent, string attributeName, string attributeValue, string attributeNamespace)
        {
            XmlAttribute ret = doc.CreateAttribute(attributeName, attributeNamespace);
            if (attributeValue != null)
            {
                ret.Value = attributeValue;
            }
            parent.Attributes.Append(ret);
            return ret;
        }
    }

    /// <summary>
    /// Represents an OPML feed. http://www.opml.org/spec
    /// </summary>
    public interface IOpmlFeed : IXmlFeed
    {
        /// <summary>
        /// Gets or sets the head.
        /// </summary>
        /// <value>The head.</value>
        IOpmlHead Head { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        IOpmlBody Body { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OpmlFeed : CachedPropertiesProvider, IOpmlFeed
    {
        private string rawContents;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpmlFeed"/> class.
        /// </summary>
        public OpmlFeed()
        {
            Head = new OpmlHead();
            Body = new OpmlBody();
        }

        #region IOpmlFeed Members

        /// <summary>
        /// Gets or sets the head.
        /// </summary>
        /// <value>The head.</value>
        public IOpmlHead Head
        {
            get
            {
                return Getter<IOpmlHead>("Head", OpmlParser.ConvertToIOpmlHead);
            }
            set
            {
                Setter("Head", value);
            }
        }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public IOpmlBody Body
        {
            get
            {
                return Getter<IOpmlBody>("Body", OpmlParser.ConvertToIOpmlBody);
            }
            set
            {
                Setter("Body", value);
            }
        }

        /// <summary>
        /// Gets or sets the raw contents.
        /// </summary>
        /// <value>The raw contents.</value>
        public string RawContents
        {
            get
            {
                return rawContents;
            }
            set
            {
                rawContents = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IRssFeed : IFeed
    {
        /// <summary>
        /// Required.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Required.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the managing editor.
        /// </summary>
        /// <value>The managing editor.</value>
        string ManagingEditor { get; set; }

        /// <summary>
        /// Gets or sets the web master.
        /// </summary>
        /// <value>The web master.</value>
        string WebMaster { get; set; }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        string Generator { get; set; }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        TzDateTime PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the last changed.
        /// </summary>
        /// <value>The last changed.</value>
        TzDateTime LastChanged { get; set; }

        /// <summary>
        /// Gets or sets the doc.
        /// </summary>
        /// <value>The doc.</value>
        Uri Doc { get; set; }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        /// <value>The time to live.</value>
        int? TimeToLive { get; set; }

        // TODO PICS rating
        /// <summary>
        /// Gets or sets the cloud.
        /// </summary>
        /// <value>The cloud.</value>
        IRssCloud Cloud { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        IRssCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the text input.
        /// </summary>
        /// <value>The text input.</value>
        IRssTextInput TextInput { get; set; }

        /// <summary>
        /// Gets or sets the skip hours.
        /// </summary>
        /// <value>The skip hours.</value>
        IList<uint> SkipHours { get; set; }

        /// <summary>
        /// Gets or sets the skip days.
        /// </summary>
        /// <value>The skip days.</value>
        IList<DayOfWeek> SkipDays { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        IRssImage Image { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssFeed : Feed, IRssFeed
    {
        #region IRssFeed Members

        /// <summary>
        /// Required.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return Getter("Title");
            }
            set
            {
                Setter("Title", value);
            }
        }

        /// <summary>
        /// Required.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("Description");
            }
            set
            {
                Setter("Description", value);
            }
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture
        {
            get
            {
                return Getter<CultureInfo>("Culture", CachedPropertiesProvider.ConvertToCultureInfo);
            }
            set
            {
                Setter("Culture", value);
            }
        }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        public string Copyright
        {
            get
            {
                return Getter("Copyright");
            }
            set
            {
                Setter("Copyright", value);
            }
        }

        /// <summary>
        /// Gets or sets the managing editor.
        /// </summary>
        /// <value>The managing editor.</value>
        public string ManagingEditor
        {
            get
            {
                return Getter("ManagingEditor");
            }
            set
            {
                Setter("ManagingEditor", value);
            }
        }

        /// <summary>
        /// Gets or sets the web master.
        /// </summary>
        /// <value>The web master.</value>
        public string WebMaster
        {
            get
            {
                return Getter("WebMaster");
            }
            set
            {
                Setter("WebMaster", value);
            }
        }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        public string Generator
        {
            get
            {
                return Getter("Generator");
            }
            set
            {
                Setter("Generator", value);
            }
        }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        public TzDateTime PublicationDate
        {
            get
            {
                return Getter<TzDateTime>("PublicationDate", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("PublicationDate", value);
            }
        }

        /// <summary>
        /// Gets or sets the last changed.
        /// </summary>
        /// <value>The last changed.</value>
        public TzDateTime LastChanged
        {
            get
            {
                return Getter<TzDateTime>("LastChanged", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("LastChanged", value);
            }
        }

        /// <summary>
        /// Gets or sets the doc.
        /// </summary>
        /// <value>The doc.</value>
        public Uri Doc
        {
            get
            {
                return Getter<Uri>("Doc", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Doc", value);
            }
        }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        /// <value>The time to live.</value>
        public int? TimeToLive
        {
            get
            {
                return Getter<int?>("TimeToLive", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("TimeToLive", value);
            }
        }

        /// <summary>
        /// Gets or sets the cloud.
        /// </summary>
        /// <value>The cloud.</value>
        public Feeder.Rss.IRssCloud Cloud
        {
            get
            {
                return Getter<IRssCloud>("Cloud", RssFeedParser.ConvertToIRssCloud);
            }
            set
            {
                Setter("Cloud", value);
            }
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public Feeder.Rss.IRssCategory Category
        {
            get
            {
                return Getter<IRssCategory>("Category", RssFeedParser.ConvertToIRssCategory);
            }
            set
            {
                Setter("Category", value);
            }
        }

        /// <summary>
        /// Gets or sets the text input.
        /// </summary>
        /// <value>The text input.</value>
        public Feeder.Rss.IRssTextInput TextInput
        {
            get
            {
                return Getter<IRssTextInput>("TextInput", RssFeedParser.ConvertToIRssTextInput);
            }
            set
            {
                Setter("TextInput", value);
            }
        }

        /// <summary>
        /// Gets or sets the skip hours.
        /// </summary>
        /// <value>The skip hours.</value>
        public IList<uint> SkipHours
        {
            get
            {
                return Getter<IList<uint>>("SkipHours", RssFeedParser.ConvertToSkipHourList);
            }
            set
            {
                Setter("SkipHours", value);
            }
        }

        /// <summary>
        /// Gets or sets the skip days.
        /// </summary>
        /// <value>The skip days.</value>
        public IList<DayOfWeek> SkipDays
        {
            get
            {
                return Getter<IList<DayOfWeek>>("SkipDays", RssFeedParser.ConvertToDayOfWeekList);
            }
            set
            {
                Setter("SkipDays", value);
            }
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Feeder.Rss.IRssImage Image
        {
            get
            {
                return Getter<IRssImage>("Image", RssFeedParser.ConvertToIRssImage);
            }
            set
            {
                Setter("Image", value);
            }
        }

        #endregion

        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        public override IDistilledFeed Distill()
        {
            IDistilledFeed distilled = new DistilledFeed();
            distilled.Properties = Properties;
            if (Category != null)
            {
                distilled.Category = Category.CategoryName;
            }
            distilled.Copyright = Copyright;
            distilled.Culture = Culture;
            distilled.Description = Description;
            distilled.FeedUri = FeedUri;
            distilled.Generator = Generator;
            distilled.LastChanged = LastChanged;
            distilled.PublicationDate = PublicationDate;
            distilled.TimeToLive = TimeToLive;
            distilled.Title = Title;
            distilled.Items.Clear();

            foreach (IFeedItem feedItem in Items)
            {
                distilled.Items.Add(feedItem.Distill());
            }
            return distilled;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IRssFeedItem : IFeedItem
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>The author.</value>
        string Author { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        IRssCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        Uri Comments { get; set; }

        /// <summary>
        /// Gets or sets the enclosure.
        /// </summary>
        /// <value>The enclosure.</value>
        IRssEnclosure Enclosure { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        IRssGuid Guid { get; set; }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        TzDateTime PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        IRssSource Source { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssFeedItem : FeedItem, IRssFeedItem
    {
        #region IRssFeedItem Members

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return Getter("Title");
            }
            set
            {
                Setter("Title", value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("Description");
            }
            set
            {
                Setter("Description", value);
            }
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get
            {
                return Getter<Uri>("Link", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Link", value);
            }
        }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>The author.</value>
        public string Author
        {
            get
            {
                return Getter("Author");
            }
            set
            {
                Setter("Author", value);
            }
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public Feeder.Rss.IRssCategory Category
        {
            get
            {
                return Getter<IRssCategory>("Category", RssFeedParser.ConvertToIRssCategory);
            }
            set
            {
                Setter("Category", value);
            }
        }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public Uri Comments
        {
            get
            {
                return Getter<Uri>("Comments", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Comments", value);
            }
        }

        /// <summary>
        /// Gets or sets the enclosure.
        /// </summary>
        /// <value>The enclosure.</value>
        public Feeder.Rss.IRssEnclosure Enclosure
        {
            get
            {
                return Getter<IRssEnclosure>("Enclosure", RssFeedParser.ConvertToIRssEnclosure);
            }
            set
            {
                Setter("Enclosure", value);
            }
        }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public Feeder.Rss.IRssGuid Guid
        {
            get
            {
                return Getter<IRssGuid>("Guid", RssFeedParser.ConvertToIRssGuid);
            }
            set
            {
                Setter("Guid", value);
            }
        }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        public TzDateTime PublicationDate
        {
            get
            {
                return Getter<TzDateTime>("PublicationDate", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("PublicationDate", value);
            }
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public Feeder.Rss.IRssSource Source
        {
            get
            {
                return Getter<IRssSource>("Source", RssFeedParser.ConvertToIRssSource);
            }
            set
            {
                Setter("Source", value);
            }
        }

        #endregion

        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        public override IDistilledFeedItem Distill()
        {
            IDistilledFeedItem distilled = new DistilledFeedItem();
            distilled.Properties = Properties;
            distilled.Description = Description;
            distilled.Link = Link;
            distilled.PublicationDate = PublicationDate;
            distilled.Title = Title;
            return distilled;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IAtomFeed : IFeed
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>The last updated.</value>
        TzDateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        /// <value>The authors.</value>
        IList<IAtomPerson> Authors { get; set; }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>The links.</value>
        IList<IAtomLink> Links { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The categories.</value>
        IList<IAtomCategory> Categories { get; set; }

        /// <summary>
        /// Gets or sets the contributors.
        /// </summary>
        /// <value>The contributors.</value>
        IList<IAtomPerson> Contributors { get; set; }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        IAtomGenerator Generator { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        string Icon { get; set; }

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>The logo.</value>
        string Logo { get; set; }

        /// <summary>
        /// Gets or sets the rights.
        /// </summary>
        /// <value>The rights.</value>
        IAtomText Rights { get; set; }

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        /// <value>The subtitle.</value>
        IAtomText Subtitle { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomFeed : Feed, IAtomFeed
    {
        private IList<IAtomPerson> m_authors = new List<IAtomPerson>();
        private IList<IAtomLink> m_links = new List<IAtomLink>();
        private IList<IAtomCategory> m_categories = new List<IAtomCategory>();
        private IList<IAtomPerson> m_contributors = new List<IAtomPerson>();

        #region IAtomFeed Members

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return Getter("Title");
            }
            set
            {
                Setter("Title", value);
            }
        }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>The last updated.</value>
        public TzDateTime LastUpdated
        {
            get
            {
                return Getter<TzDateTime>("LastUpdated", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("LastUpdated", value);
            }
        }

        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        /// <value>The authors.</value>
        public IList<IAtomPerson> Authors
        {
            get
            {
                return m_authors;
            }
            set
            {
                m_authors = value;
            }
        }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>The links.</value>
        public IList<IAtomLink> Links
        {
            get
            {
                return m_links;
            }
            set
            {
                m_links = value;
            }
        }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The categories.</value>
        public IList<IAtomCategory> Categories
        {
            get
            {
                return m_categories;
            }
            set
            {
                m_categories = value;
            }
        }

        /// <summary>
        /// Gets or sets the contributors.
        /// </summary>
        /// <value>The contributors.</value>
        public IList<IAtomPerson> Contributors
        {
            get
            {
                return m_contributors;
            }
            set
            {
                m_contributors = value;
            }
        }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        public IAtomGenerator Generator
        {
            get
            {
                return Getter<IAtomGenerator>("Generator", AtomFeedParser.ConvertToIAtomGenerator);
            }
            set
            {
                Setter("Generator", value);
            }
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public string Icon
        {
            get
            {
                return Getter("Icon");
            }
            set
            {
                Setter("Icon", value);
            }
        }

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>The logo.</value>
        public string Logo
        {
            get
            {
                return Getter("Logo");
            }
            set
            {
                Setter("Logo", value);
            }
        }

        /// <summary>
        /// Gets or sets the rights.
        /// </summary>
        /// <value>The rights.</value>
        public IAtomText Rights
        {
            get
            {
                return Getter<IAtomText>("Rights", AtomFeedParser.ConvertToIAtomText);
            }
            set
            {
                Setter("Rights", value);
            }
        }

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        /// <value>The subtitle.</value>
        public IAtomText Subtitle
        {
            get
            {
                return Getter<IAtomText>("Subtitle", AtomFeedParser.ConvertToIAtomText);
            }
            set
            {
                Setter("Subtitle", value);
            }
        }

        #endregion

        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        public override IDistilledFeed Distill()
        {
            IDistilledFeed distilled = new Feeder.DistilledFeed();
            if (Categories != null && Categories.Count == 1)
            {
                distilled.Category = Categories[0].Term;
            }
            if (Rights != null)
            {
                distilled.Copyright = Rights.Text;
            }
            if (Subtitle != null)
            {
                distilled.Description = Subtitle.Text;
            }
            distilled.FeedUri = FeedUri;
            if (Generator != null)
            {
                distilled.Generator = Generator.Description;
            }
            distilled.LastChanged = LastUpdated;
            distilled.Properties = Properties;
            distilled.PublicationDate = LastUpdated;
            distilled.Title = Title;
            distilled.Items.Clear();
            foreach (IAtomFeedItem item in Items)
            {
                distilled.Items.Add(item.Distill());
            }
            return distilled;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IAtomFeedItem : IFeedItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        Uri Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>The last updated.</value>
        TzDateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        /// <value>The authors.</value>
        IList<IAtomPerson> Authors { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        IAtomContent Content { get; set; }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>The links.</value>
        IList<IAtomLink> Links { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        IAtomText Summary { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The categories.</value>
        IList<IAtomCategory> Categories { get; set; }

        /// <summary>
        /// Gets or sets the contributors.
        /// </summary>
        /// <value>The contributors.</value>
        IList<IAtomPerson> Contributors { get; set; }

        /// <summary>
        /// Gets or sets the published.
        /// </summary>
        /// <value>The published.</value>
        TzDateTime Published { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        IAtomFeed Source { get; set; }

        /// <summary>
        /// Gets or sets the rights.
        /// </summary>
        /// <value>The rights.</value>
        IAtomText Rights { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomFeedItem : FeedItem, IAtomFeedItem
    {
        private IList<IAtomPerson> m_authors = new List<IAtomPerson>();
        private IList<IAtomLink> m_links = new List<IAtomLink>();
        private IList<IAtomCategory> m_categories = new List<IAtomCategory>();
        private IList<IAtomPerson> m_contributors = new List<IAtomPerson>();

        #region IAtomFeedItem Members

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public Uri Id
        {
            get
            {
                return Getter<Uri>("Id", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Id", value);
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return Getter("Title");
            }
            set
            {
                Setter("Title", value);
            }
        }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>The last updated.</value>
        public TzDateTime LastUpdated
        {
            get
            {
                return Getter<TzDateTime>("LastUpdated", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("LastUpdated", value);
            }
        }

        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        /// <value>The authors.</value>
        public IList<IAtomPerson> Authors
        {
            get
            {
                return m_authors;
            }
            set
            {
                m_authors = value;
            }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public IAtomContent Content
        {
            get
            {
                return Getter<IAtomContent>("Content", AtomFeedParser.ConvertToIAtomContent);
            }
            set
            {
                Setter("Content", value);
            }
        }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>The links.</value>
        public IList<IAtomLink> Links
        {
            get
            {
                return m_links;
            }
            set
            {
                m_links = value;
            }
        }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public IAtomText Summary
        {
            get
            {
                return Getter<IAtomText>("Summary", AtomFeedParser.ConvertToIAtomText);
            }
            set
            {
                Setter("Summary", value);
            }
        }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The categories.</value>
        public IList<IAtomCategory> Categories
        {
            get
            {
                return m_categories;
            }
            set
            {
                m_categories = value;
            }
        }

        /// <summary>
        /// Gets or sets the contributors.
        /// </summary>
        /// <value>The contributors.</value>
        public IList<IAtomPerson> Contributors
        {
            get
            {
                return m_contributors;
            }
            set
            {
                m_contributors = value;
            }
        }

        /// <summary>
        /// Gets or sets the published.
        /// </summary>
        /// <value>The published.</value>
        public TzDateTime Published
        {
            get
            {
                return Getter<TzDateTime>("Published", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("Published", value);
            }
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public IAtomFeed Source
        {
            get
            {
                return Getter<IAtomFeed>("Source", AtomFeedParser.ConvertToIAtomFeed);
            }
            set
            {
                Setter("Source", value);
            }
        }

        /// <summary>
        /// Gets or sets the rights.
        /// </summary>
        /// <value>The rights.</value>
        public IAtomText Rights
        {
            get
            {
                return Getter<IAtomText>("Rights", AtomFeedParser.ConvertToIAtomText);
            }
            set
            {
                Setter("Rights", value);
            }
        }

        #endregion

        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        public override IDistilledFeedItem Distill()
        {
            IDistilledFeedItem distilled = new DistilledFeedItem();
            distilled.Link = Id;
            distilled.Properties = Properties;
            distilled.PublicationDate = LastUpdated;
            distilled.Title = Title;

            // TODO the "description" needs some logic to
            // it as per the spec
            distilled.Description = Title;
            return distilled;
        }
    }

    /// <summary>
    /// http://www.kbcafe.com/rss/?guid=20051003145153
    /// </summary>
    public class OpmlSerializer : Serializer
    {
        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <returns></returns>
        public static string SerializeToString(IOpmlFeed feed)
        {
            return Serialize(feed).OuterXml;
        }

        /// <summary>
        /// Serializes the specified feed.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <returns></returns>
        public static XmlDocument Serialize(IOpmlFeed feed)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = AppendNewElement(doc, doc, "opml");
            AppendNewAttribute(doc, root, "version", "1.0");
            XmlElement head = AppendNewElement(doc, root, "head");
            if (feed.Head != null)
            {
                if (feed.Head.Title != null)
                {
                    AppendNewElement(doc, head, "title", feed.Head.Title);
                }
                if (feed.Head.DateCreated != null)
                {
                    AppendNewElement(doc, head, "dateCreated", feed.Head.DateCreated.DateTimeUtc.ToString("r"));
                }
                if (feed.Head.DateModified != null)
                {
                    AppendNewElement(doc, head, "dateModified", feed.Head.DateModified.DateTimeUtc.ToString("r"));
                }
                if (feed.Head.Owner != null)
                {
                    AppendNewElement(doc, head, "ownerName", feed.Head.Owner);
                }
                if (feed.Head.OwnerEmail != null)
                {
                    AppendNewElement(doc, head, "ownerEmail", feed.Head.OwnerEmail);
                }
                if (feed.Head.ExpansionState != null)
                {
                    AppendNewElement(doc, head, "expansionState", feed.Head.ExpansionState);
                }
                if (feed.Head.VerticalScrollState != null)
                {
                    AppendNewElement(doc, head, "vertScrollState", feed.Head.VerticalScrollState.Value.ToString());
                }
                if (feed.Head.WindowBottom != null)
                {
                    AppendNewElement(doc, head, "windowBottom", feed.Head.WindowBottom.Value.ToString());
                }
                if (feed.Head.WindowTop != null)
                {
                    AppendNewElement(doc, head, "windowTop", feed.Head.WindowTop.Value.ToString());
                }
                if (feed.Head.WindowLeft != null)
                {
                    AppendNewElement(doc, head, "windowLeft", feed.Head.WindowLeft.Value.ToString());
                }
                if (feed.Head.WindowRight != null)
                {
                    AppendNewElement(doc, head, "windowRight", feed.Head.WindowRight.Value.ToString());
                }
            }

            XmlElement body = AppendNewElement(doc, root, "body");
            SerializeOutlines(doc, body, feed.Body);
            return doc;
        }

        private static void SerializeOutlines(XmlDocument doc, XmlElement parent, IOpmlOutlineProvider outlineProvider)
        {
            if (outlineProvider != null)
            {
                foreach (IOpmlOutline outline in outlineProvider.Outlines)
                {
                    XmlElement outlineElement = AppendNewElement(doc, parent, "outline");
                    if (outline.Type != null)
                    {
                        AppendNewAttribute(doc, outlineElement, "type", outline.Type);
                    }
                    if (outline.Text != null)
                    {
                        AppendNewAttribute(doc, outlineElement, "text", outline.Text);
                    }
                    SerializeOutlines(doc, outlineElement, outline);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SerializeType
    {
        /// <summary>
        /// 
        /// </summary>
        Rss2
    }

    /// <summary>
    /// 
    /// </summary>
    public class FeedSerializer : Serializer
    {
        /// <summary>
        /// Serializes the specified feed.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static XmlDocument Serialize(IFeed feed, SerializeType type)
        {
            XmlDocument doc = new XmlDocument();
            switch (type)
            {
                case SerializeType.Rss2:
                    SerializeRss2(doc, (IRssFeed)feed);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return doc;
        }

        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string SerializeToString(IFeed feed, SerializeType type)
        {
            return Serialize(feed, type).OuterXml;
        }

        /// <summary>
        /// Serializes the RSS2.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="feed">The feed.</param>
        public static void SerializeRss2(XmlDocument doc, IRssFeed feed)
        {
            XmlElement root = AppendNewElement(doc, doc, "rss");
            AppendNewAttribute(doc, root, "version", "2.0");
            XmlElement channel = AppendNewElement(doc, root, "channel");
            AppendNewElement(doc, channel, "title", feed.Title);
            if (feed.FeedUri != null)
            {
                AppendNewElement(doc, channel, "link", feed.FeedUri.ToString());
            }
            AppendNewElement(doc, channel, "description", feed.Description);
            AppendNewElement(doc, channel, "copyright", feed.Copyright);
            foreach (IRssFeedItem item in feed.Items)
            {
                XmlElement itemel = AppendNewElement(doc, channel, "item");
                AppendNewElement(doc, itemel, "title", item.Title);
                if (item.Link != null)
                {
                    AppendNewElement(doc, itemel, "link", item.Link.ToString());
                }
                AppendNewElement(doc, itemel, "description", item.Description);
                if (item.PublicationDate != null)
                {
                    AppendNewElement(doc, itemel, "pubDate", item.PublicationDate.DateTimeUtc.ToString("r"));
                }
            }
        }
    }
}

namespace PublicDomain.Feeder.Atom
{
    /// <summary>
    /// Specifies a category that the feed belongs to. A feed may have multiple category elements.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomCategory
    {
        /// <summary>
        /// Gets or sets the term.
        /// </summary>
        /// <value>The term.</value>
        string Term { get; set; }

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        /// <value>The scheme.</value>
        Uri Scheme { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        string Label { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomCategory : CachedPropertiesProvider, Feeder.Atom.IAtomCategory
    {
        #region IAtomCategory Members

        /// <summary>
        /// Gets or sets the term.
        /// </summary>
        /// <value>The term.</value>
        public string Term
        {
            get
            {
                return Getter("Term");
            }
            set
            {
                Setter("Term", value);
            }
        }

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        /// <value>The scheme.</value>
        public Uri Scheme
        {
            get
            {
                return Getter<Uri>("Scheme", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Scheme", value);
            }
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Label
        {
            get
            {
                return Getter("Label");
            }
            set
            {
                Setter("Label", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Contains or links to the complete content of the entry. Content must be provided if there is no alternate link, and should be provided if there is no summary.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomContent
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the SRC.
        /// </summary>
        /// <value>The SRC.</value>
        Uri Src { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        string Content { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomContent : CachedPropertiesProvider, IAtomContent
    {
        #region IAtomContent Members

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return Getter("Type");
            }
            set
            {
                Setter("Type", value);
            }
        }

        /// <summary>
        /// Gets or sets the SRC.
        /// </summary>
        /// <value>The SRC.</value>
        public Uri Src
        {
            get
            {
                return Getter<Uri>("Src", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Src", value);
            }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content
        {
            get
            {
                return Getter("Content");
            }
            set
            {
                Setter("Content", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Identifies the software used to generate the feed, for debugging and other purposes. Both the uri and version attributes are optional.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomGenerator
    {
        /// <summary>
        /// Gets or sets the generator URI.
        /// </summary>
        /// <value>The generator URI.</value>
        Uri GeneratorUri { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        string Version { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomGenerator : CachedPropertiesProvider, Feeder.Atom.IAtomGenerator
    {
        #region IAtomGenerator Members

        /// <summary>
        /// Gets or sets the generator URI.
        /// </summary>
        /// <value>The generator URI.</value>
        public Uri GeneratorUri
        {
            get
            {
                return Getter<Uri>("GeneratorUri", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("GeneratorUri", value);
            }
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version
        {
            get
            {
                return Getter("Version");
            }
            set
            {
                Setter("Version", value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("Description");
            }
            set
            {
                Setter("Description", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Identifies a related Web page.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomLink
    {
        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>The href.</value>
        Uri Href { get; set; }

        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        /// <value>The relationship.</value>
        string Relationship { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the link language.
        /// </summary>
        /// <value>The link language.</value>
        string LinkLanguage { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        int? Length { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomLink : CachedPropertiesProvider, Feeder.Atom.IAtomLink
    {
        #region IAtomLink Members

        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>The href.</value>
        public Uri Href
        {
            get
            {
                return Getter<Uri>("Href", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Href", value);
            }
        }

        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        /// <value>The relationship.</value>
        public string Relationship
        {
            get
            {
                return Getter("Relationship");
            }
            set
            {
                Setter("Relationship", value);
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return Getter("Type");
            }
            set
            {
                Setter("Type", value);
            }
        }

        /// <summary>
        /// Gets or sets the link language.
        /// </summary>
        /// <value>The link language.</value>
        public string LinkLanguage
        {
            get
            {
                return Getter("LinkLanguage");
            }
            set
            {
                Setter("LinkLanguage", value);
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return Getter("Title");
            }
            set
            {
                Setter("Title", value);
            }
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public int? Length
        {
            get
            {
                return Getter<int?>("Length", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("Length", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Describes a person, corporation, or similar entity. It has one required element, name, and two optional elements: uri, email.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomPerson
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the homepage.
        /// </summary>
        /// <value>The homepage.</value>
        Uri Homepage { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        string Email { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomPerson : CachedPropertiesProvider, Feeder.Atom.IAtomPerson
    {
        #region IAtomPerson Members

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return Getter("Name");
            }
            set
            {
                Setter("Name", value);
            }
        }

        /// <summary>
        /// Gets or sets the homepage.
        /// </summary>
        /// <value>The homepage.</value>
        public Uri Homepage
        {
            get
            {
                return Getter<Uri>("Homepage", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Homepage", value);
            }
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get
            {
                return Getter("Email");
            }
            set
            {
                Setter("Email", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Contains human-readable text, usually in small quantities. The type attribute determines how this information is encoded (default="text")
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomText
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomText : CachedPropertiesProvider, Feeder.Atom.IAtomText
    {
        #region IAtomText Members

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return Getter("Type");
            }
            set
            {
                Setter("Type", value);
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                return Getter("Text");
            }
            set
            {
                Setter("Text", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomFeedParser : FeedParser
    {
        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override T Parse<T>(System.Xml.XmlReader reader)
        {
            IAtomFeed ret = (IAtomFeed)new AtomFeed();
            reader.Read();

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "id":
                            ret.FeedUri = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString(), reader.BaseURI);
                            break;
                        case "title":
                            ret.Title = reader.ReadElementContentAsString();
                            break;
                        case "updated":
                            ret.LastUpdated = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "generator":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Generator = ConvertToIAtomGenerator(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "author":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Authors.Add(ConvertToIAtomPerson(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "link":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Links.Add(ConvertToIAtomLink(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "category":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Categories.Add(ConvertToIAtomCategory(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "entry":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Items.Add(ParseItem(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "contributor":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Contributors.Add(ConvertToIAtomPerson(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "logo":
                            ret.Logo = reader.ReadElementContentAsString();
                            break;
                        case "icon":
                            ret.Icon = reader.ReadElementContentAsString();
                            break;
                        case "rights":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Rights = ConvertToIAtomText(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "subtitle":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Subtitle = ConvertToIAtomText(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        default:
                            UnhandledElement(ret, reader);
                            break;
                    }
                }
            }
            reader.Close();
            return (T)ret;
        }

        /// <summary>
        /// Parses the item.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public IFeedItem ParseItem(XmlReader reader)
        {
            IAtomFeedItem item = new AtomFeedItem();
            reader.ReadToDescendant("entry");

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "id":
                            item.Id = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString(), reader.BaseURI);
                            break;
                        case "title":
                            item.Title = reader.ReadElementContentAsString();
                            break;
                        case "updated":
                            item.LastUpdated = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "author":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Authors.Add(ConvertToIAtomPerson(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "link":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Links.Add(ConvertToIAtomLink(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "category":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Categories.Add(ConvertToIAtomCategory(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "contributor":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Contributors.Add(ConvertToIAtomPerson(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "rights":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Rights = ConvertToIAtomText(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "summary":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Summary = ConvertToIAtomText(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "content":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Content = ConvertToIAtomContent(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "published":
                            item.Published = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "source":
                            item.Source = ConvertToIAtomFeed(reader.ReadOuterXml());
                            break;
                        default:
                            UnhandledElement(item, reader);
                            break;
                    }
                }
            }
            reader.Close();
            return item;
        }

        /// <summary>
        /// Converts to I atom generator.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomGenerator ConvertToIAtomGenerator(XmlReader input)
        {
            AtomGenerator gen = new AtomGenerator();
            input.ReadToDescendant("generator");
            gen.GeneratorUri = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("uri"), input.BaseURI);
            gen.Version = input.GetAttribute("version");
            gen.Description = input.ReadElementContentAsString();
            input.Close();
            return gen;
        }

        /// <summary>
        /// Converts to I atom generator.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomGenerator ConvertToIAtomGenerator(string input)
        {
            return ConvertToIAtomGenerator(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I atom text.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomText ConvertToIAtomText(XmlReader input)
        {
            AtomText text = new AtomText();
            input.Read();
            text.Type = input.GetAttribute("type");
            text.Text = input.ReadInnerXml();
            input.Close();
            return text;
        }

        /// <summary>
        /// Converts to I atom text.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomText ConvertToIAtomText(string input)
        {
            return ConvertToIAtomText(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts the content of to I atom.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomContent ConvertToIAtomContent(XmlReader input)
        {
            AtomContent content = new AtomContent();
            input.ReadToDescendant("content");
            content.Type = input.GetAttribute("type");
            content.Src = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("src"), input.BaseURI);
            content.Content = input.ReadInnerXml();
            input.Close();
            return content;
        }

        /// <summary>
        /// Converts the content of to I atom.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomContent ConvertToIAtomContent(string input)
        {
            return ConvertToIAtomContent(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I atom feed.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomFeed ConvertToIAtomFeed(string input)
        {
            using (StringReader stringReader = new StringReader(input))
            {
                return new AtomFeedParser().CreateFeed<IAtomFeed>(input);
            }
        }

        /// <summary>
        /// Converts to I atom person.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomPerson ConvertToIAtomPerson(XmlReader input)
        {
            AtomPerson person = new AtomPerson();
            input.Read();
            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "name":
                            person.Name = input.ReadElementContentAsString();
                            break;
                        case "uri":
                            person.Homepage = CachedPropertiesProvider.ConvertToUri(input.ReadElementContentAsString(), input.BaseURI);
                            break;
                        case "email":
                            person.Email = input.ReadElementContentAsString();
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
            input.Close();
            return person;
        }

        /// <summary>
        /// Converts to I atom link.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomLink ConvertToIAtomLink(XmlReader input)
        {
            AtomLink link = new AtomLink();
            input.ReadToDescendant("link");
            link.Href = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("href"), input.BaseURI);
            link.Relationship = input.GetAttribute("rel");
            link.Type = input.GetAttribute("type");
            link.LinkLanguage = input.GetAttribute("hreflang");
            link.Title = input.GetAttribute("title");
            link.Length = CachedPropertiesProvider.ConvertToIntNullable(input.GetAttribute("length"));
            input.Close();
            return link;
        }

        /// <summary>
        /// Converts to I atom category.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomCategory ConvertToIAtomCategory(XmlReader input)
        {
            AtomCategory cat = new AtomCategory();
            input.ReadToDescendant("link");
            cat.Label = input.GetAttribute("label");
            cat.Term = input.GetAttribute("term");
            cat.Scheme = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("scheme"), input.BaseURI);
            input.Close();
            return cat;
        }
    }
}

namespace PublicDomain.Feeder.Opml
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOpmlBody : IOpmlOutlineProvider
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class OpmlBody : OpmlOutlineProvider, IOpmlBody
    {
    }

    /// <summary>
    /// A head contains zero or more optional element.
    /// 
    /// All the sub-elements of head may be ignored by the processor.
    /// If an outline is opened within another outline, the processor
    /// must ignore the windowXxx elements, those elements only control
    /// the size and position of outlines that are opened in their own windows.
    /// 
    /// If you load an OPML document into your client, you may choose to
    /// respect expansionState, or not. We're not in any way trying to
    /// dictate user experience. The expansionState info is there because
    /// it's needed in certain contexts. It's easy to imagine contexts where
    /// it would make sense to completely ignore it.
    /// 
    /// Taken verbatim from http://www.opml.org/spec
    /// </summary>
    public interface IOpmlHead : ICachedPropertiesProvider
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        TzDateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the date modified.
        /// </summary>
        /// <value>The date modified.</value>
        TzDateTime DateModified { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        string Owner { get; set; }

        /// <summary>
        /// Gets or sets the owner email.
        /// </summary>
        /// <value>The owner email.</value>
        string OwnerEmail { get; set; }

        /// <summary>
        /// Gets or sets the state of the expansion.
        /// </summary>
        /// <value>The state of the expansion.</value>
        string ExpansionState { get; set; }

        /// <summary>
        /// Gets or sets the state of the vertical scroll.
        /// </summary>
        /// <value>The state of the vertical scroll.</value>
        int? VerticalScrollState { get; set; }

        /// <summary>
        /// Gets or sets the window top.
        /// </summary>
        /// <value>The window top.</value>
        int? WindowTop { get; set; }

        /// <summary>
        /// Gets or sets the window left.
        /// </summary>
        /// <value>The window left.</value>
        int? WindowLeft { get; set; }

        /// <summary>
        /// Gets or sets the window bottom.
        /// </summary>
        /// <value>The window bottom.</value>
        int? WindowBottom { get; set; }

        /// <summary>
        /// Gets or sets the window right.
        /// </summary>
        /// <value>The window right.</value>
        int? WindowRight { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OpmlHead : CachedPropertiesProvider, IOpmlHead
    {
        #region IOpmlHead Members

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return Getter("Title");
            }
            set
            {
                Setter("Title", value);
            }
        }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        public TzDateTime DateCreated
        {
            get
            {
                return Getter<TzDateTime>("DateCreated", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("DateCreated", value);
            }
        }

        /// <summary>
        /// Gets or sets the date modified.
        /// </summary>
        /// <value>The date modified.</value>
        public TzDateTime DateModified
        {
            get
            {
                return Getter<TzDateTime>("DateModified", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("DateModified", value);
            }
        }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public string Owner
        {
            get
            {
                return Getter("Owner");
            }
            set
            {
                Setter("Owner", value);
            }
        }

        /// <summary>
        /// Gets or sets the owner email.
        /// </summary>
        /// <value>The owner email.</value>
        public string OwnerEmail
        {
            get
            {
                return Getter("OwnerEmail");
            }
            set
            {
                Setter("OwnerEmail", value);
            }
        }

        /// <summary>
        /// Gets or sets the state of the expansion.
        /// </summary>
        /// <value>The state of the expansion.</value>
        public string ExpansionState
        {
            get
            {
                return Getter("ExpansionState");
            }
            set
            {
                Setter("ExpansionState", value);
            }
        }

        /// <summary>
        /// Gets or sets the state of the vertical scroll.
        /// </summary>
        /// <value>The state of the vertical scroll.</value>
        public int? VerticalScrollState
        {
            get
            {
                return Getter<int?>("VerticalScrollState", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("VerticalScrollState", value);
            }
        }

        /// <summary>
        /// Gets or sets the window top.
        /// </summary>
        /// <value>The window top.</value>
        public int? WindowTop
        {
            get
            {
                return Getter<int?>("WindowTop", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("WindowTop", value);
            }
        }

        /// <summary>
        /// Gets or sets the window left.
        /// </summary>
        /// <value>The window left.</value>
        public int? WindowLeft
        {
            get
            {
                return Getter<int?>("WindowLeft", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("WindowLeft", value);
            }
        }

        /// <summary>
        /// Gets or sets the window bottom.
        /// </summary>
        /// <value>The window bottom.</value>
        public int? WindowBottom
        {
            get
            {
                return Getter<int?>("WindowBottom", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("WindowBottom", value);
            }
        }

        /// <summary>
        /// Gets or sets the window right.
        /// </summary>
        /// <value>The window right.</value>
        public int? WindowRight
        {
            get
            {
                return Getter<int?>("WindowRight", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("WindowRight", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IOpmlOutline : IOpmlOutlineProvider
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the is comment.
        /// </summary>
        /// <value>The is comment.</value>
        bool? IsComment { get; set; }

        /// <summary>
        /// Gets or sets the is breakpoint.
        /// </summary>
        /// <value>The is breakpoint.</value>
        bool? IsBreakpoint { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OpmlOutline : OpmlOutlineProvider, IOpmlOutline
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpmlOutline"/> class.
        /// </summary>
        public OpmlOutline()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpmlOutline"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public OpmlOutline(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpmlOutline"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="type">The type.</param>
        public OpmlOutline(string text, string type)
        {
            this.Text = text;
            this.Type = type;
        }

        #region IOpmlOutline Members

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                return Getter("Text");
            }
            set
            {
                Setter("Text", value);
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return Getter("Type");
            }
            set
            {
                Setter("Type", value);
            }
        }

        /// <summary>
        /// Gets or sets the is comment.
        /// </summary>
        /// <value>The is comment.</value>
        public bool? IsComment
        {
            get
            {
                return Getter<bool?>("IsComment", CachedPropertiesProvider.ConvertToBoolNullable);
            }
            set
            {
                Setter("IsComment", value);
            }
        }

        /// <summary>
        /// Gets or sets the is breakpoint.
        /// </summary>
        /// <value>The is breakpoint.</value>
        public bool? IsBreakpoint
        {
            get
            {
                return Getter<bool?>("IsBreakpoint", CachedPropertiesProvider.ConvertToBoolNullable);
            }
            set
            {
                Setter("IsBreakpoint", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IOpmlOutlineProvider
    {
        /// <summary>
        /// Gets or sets the outlines.
        /// </summary>
        /// <value>The outlines.</value>
        IList<IOpmlOutline> Outlines { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OpmlOutlineProvider : CachedPropertiesProvider, IOpmlOutlineProvider
    {
        #region IOpmlOutlineProvider Members

        private IList<IOpmlOutline> _Outlines = new List<IOpmlOutline>();

        /// <summary>
        /// Gets or sets the outlines.
        /// </summary>
        /// <value>The outlines.</value>
        public IList<IOpmlOutline> Outlines
        {
            get
            {
                return _Outlines;
            }
            set
            {
                _Outlines = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class OpmlParser : Parser
    {
        /// <summary>
        /// Creates the feed base.
        /// </summary>
        /// <param name="feedReader">The feed reader.</param>
        /// <returns></returns>
        protected override T CreateFeedBase<T>(XmlReader feedReader)
        {
            feedReader.MoveToContent();
            return new OpmlParser().Parse<T>(feedReader);
        }

        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override T Parse<T>(System.Xml.XmlReader reader)
        {
            IOpmlFeed ret = new OpmlFeed();

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "head":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Head = ConvertToIOpmlHead(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "body":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Body = ConvertToIOpmlBody(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        default:
                            UnhandledElement(ret, reader);
                            break;
                    }
                }
            }
            reader.Close();
            return (T)ret;
        }

        internal static IOpmlOutline ConvertToIOpmlOutline(string input)
        {
            return ConvertToIOpmlOutline(CreateXmlReaderFromString(input));
        }

        internal static IOpmlOutline ConvertToIOpmlOutline(XmlReader input)
        {
            OpmlOutline outline = new OpmlOutline();
            input.ReadToDescendant("outline");
            outline.Text = input.GetAttribute("text");
            outline.Type = input.GetAttribute("type");
            outline.IsBreakpoint = CachedPropertiesProvider.ConvertToBoolNullable(input.GetAttribute("isBreakpoint"));
            outline.IsComment = CachedPropertiesProvider.ConvertToBoolNullable(input.GetAttribute("isComment"));
            ReadOutlines(input, outline);
            input.Close();
            return outline;
        }

        internal static IOpmlBody ConvertToIOpmlBody(string input)
        {
            return ConvertToIOpmlBody(CreateXmlReaderFromString(input));
        }

        internal static IOpmlBody ConvertToIOpmlBody(XmlReader input)
        {
            OpmlBody body = new OpmlBody();
            input.ReadToDescendant("body");
            ReadOutlines(input, body);
            input.Close();
            return body;
        }

        private static void ReadOutlines(XmlReader input, IOpmlOutlineProvider outlineProvider)
        {
            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "outline":
                            using (XmlReader subReader = input.ReadSubtree())
                            {
                                outlineProvider.Outlines.Add(ConvertToIOpmlOutline(subReader));
                            }
                            if (input.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
        }

        internal static IOpmlHead ConvertToIOpmlHead(string input)
        {
            return ConvertToIOpmlHead(CreateXmlReaderFromString(input));
        }

        internal static IOpmlHead ConvertToIOpmlHead(XmlReader input)
        {
            OpmlHead head = new OpmlHead();
            input.ReadToDescendant("head");
            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "title":
                            head.Title = input.ReadElementContentAsString();
                            break;
                        case "dateCreated":
                            head.DateCreated = CachedPropertiesProvider.ConvertToTzDateTime(input.ReadElementContentAsString());
                            break;
                        case "dateModified":
                            head.DateModified = CachedPropertiesProvider.ConvertToTzDateTime(input.ReadElementContentAsString());
                            break;
                        case "ownerName":
                            head.Owner = input.ReadElementContentAsString();
                            break;
                        case "ownerEmail":
                            head.OwnerEmail = input.ReadElementContentAsString();
                            break;
                        case "expansionState":
                            head.ExpansionState = input.ReadElementContentAsString();
                            break;
                        case "vertScrollState":
                            head.VerticalScrollState = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        case "windowTop":
                            head.WindowTop = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        case "windowLeft":
                            head.WindowLeft = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        case "windowBottom":
                            head.WindowBottom = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        case "windowRight":
                            head.WindowRight = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
            input.Close();
            return head;
        }
    }
}

namespace PublicDomain.Feeder.Rss
{
    /// <summary>
    /// In RSS 2.0, a provision is made for linking a channel to its identifier in a cataloging system, using the channel-level category feature. For example, to link a channel to its Syndic8 identifier, include a category element as a sub-element of channel, with domain "Syndic8", and value the identifier for your channel in the Syndic8 database. The appropriate category element for Scripting News would be <category domain="Syndic8">1765</category>.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssCategory
    {
        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        string Domain
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>The name of the category.</value>
        string CategoryName
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssCategory : CachedPropertiesProvider, Feeder.Rss.IRssCategory
    {
        #region IRssCategory Members

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain
        {
            get
            {
                return Getter("Domain");
            }
            set
            {
                Setter("Domain", value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>The name of the category.</value>
        public string CategoryName
        {
            get
            {
                return Getter("CategoryName");
            }
            set
            {
                Setter("CategoryName", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Specifies a web service that supports the rssCloud interface which can be implemented in HTTP-POST, XML-RPC or SOAP 1.1. 
    /// Its purpose is to allow processes to register with a cloud to be notified of updates to the channel, implementing a lightweight publish-subscribe protocol for RSS feeds.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssCloud
    {
        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        string Domain
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        int Port
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        string Path
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the register procedure.
        /// </summary>
        /// <value>The register procedure.</value>
        string RegisterProcedure
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        RssCloudProtocol Protocol
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum RssCloudProtocol
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown,

        /// <summary>
        /// 
        /// </summary>
        XmlRpc,

        /// <summary>
        /// 
        /// </summary>
        HttpPost,

        /// <summary>
        /// /
        /// </summary>
        Soap
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssCloud : CachedPropertiesProvider, Feeder.Rss.IRssCloud
    {
        #region IRssCloud Members

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain
        {
            get
            {
                return Getter("Domain");
            }
            set
            {
                Setter("Domain", value);
            }
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port
        {
            get
            {
                return Getter<int>("Port", CachedPropertiesProvider.ConvertToInt);
            }
            set
            {
                Setter("Port", value);
            }
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get
            {
                return Getter("Path");
            }
            set
            {
                Setter("Path", value);
            }
        }

        /// <summary>
        /// Gets or sets the register procedure.
        /// </summary>
        /// <value>The register procedure.</value>
        public string RegisterProcedure
        {
            get
            {
                return Getter("RegisterProcedure");
            }
            set
            {
                Setter("RegisterProcedure", value);
            }
        }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        public Feeder.Rss.RssCloudProtocol Protocol
        {
            get
            {
                return Getter<RssCloudProtocol>("Protocol", RssFeedParser.ConvertToRssCloudProtocol);
            }
            set
            {
                Setter("Protocol", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Describes a media object that is attached to the item.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssEnclosure
    {
        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        int Length
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssEnclosure : CachedPropertiesProvider, Feeder.Rss.IRssEnclosure
    {
        #region IRssEnclosure Members

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get
            {
                return Getter<Uri>("Link", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Link", value);
            }
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public int Length
        {
            get
            {
                return Getter<int>("Length", CachedPropertiesProvider.ConvertToInt);
            }
            set
            {
                Setter("Length", value);
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return Getter("Type");
            }
            set
            {
                Setter("Type", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// guid stands for globally unique identifier. It's a string that uniquely identifies the item. When present, an aggregator may choose to use this string to determine if an item is new.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssGuid
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        string UniqueIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the is perma link.
        /// </summary>
        /// <value>The is perma link.</value>
        bool? IsPermaLink
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssGuid : CachedPropertiesProvider, Feeder.Rss.IRssGuid
    {
        #region IRssGuid Members

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public string UniqueIdentifier
        {
            get
            {
                return Getter("UniqueIdentifier");
            }
            set
            {
                Setter("UniqueIdentifier", value);
            }
        }

        /// <summary>
        /// Gets or sets the is perma link.
        /// </summary>
        /// <value>The is perma link.</value>
        public bool? IsPermaLink
        {
            get
            {
                return Getter<bool?>("IsPermaLink", CachedPropertiesProvider.ConvertToBoolNullable);
            }
            set
            {
                Setter("IsPermaLink", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Specifies a GIF, JPEG or PNG image that can be displayed with the channel.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssImage
    {
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        Uri Location
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        int? Width
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        int? Height
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssImage : CachedPropertiesProvider, Feeder.Rss.IRssImage
    {
        #region IRssImage Members

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Uri Location
        {
            get
            {
                return Getter<Uri>("Location", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Location", value);
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return Getter("Title");
            }
            set
            {
                Setter("Title", value);
            }
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get
            {
                return Getter<Uri>("Link", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Link", value);
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int? Width
        {
            get
            {
                return Getter<int?>("Width", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("Width", value);
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int? Height
        {
            get
            {
                return Getter<int?>("Height", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("Height", value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("Description");
            }
            set
            {
                Setter("Description", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Its value is the name of the RSS channel that the item came from, derived from its title. It has one required attribute, url, which links to the XMLization of the source.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssSource
    {
        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssSource : CachedPropertiesProvider, Feeder.Rss.IRssSource
    {
        #region IRssSource Members

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get
            {
                return Getter<Uri>("Link", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Link", value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("Description");
            }
            set
            {
                Setter("Description", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// The purpose of the textInput element is something of a mystery. You can use it to specify a search engine box. Or to allow a reader to provide feedback. Most aggregators ignore it.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssTextInput
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssTextInput : CachedPropertiesProvider, Feeder.Rss.IRssTextInput
    {
        #region IRssTextInput Members

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return Getter("Title");
            }
            set
            {
                Setter("Title", value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("Description");
            }
            set
            {
                Setter("Description", value);
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return Getter("Name");
            }
            set
            {
                Setter("Name", value);
            }
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get
            {
                return Getter<Uri>("Link", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Link", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssFeedParser : FeedParser
    {
        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override T Parse<T>(System.Xml.XmlReader reader)
        {
            IRssFeed ret = (IRssFeed)new RssFeed();
            reader.Read();

            // RDF versions of RSS don't have version tags.
            //double version = double.Parse(reader.GetAttribute("version"));

            reader.ReadToDescendant("channel");

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "title":
                            ret.Title = reader.ReadElementContentAsString();
                            break;
                        case "link":
                            ret.FeedUri = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString());
                            break;
                        case "description":
                            ret.Description = reader.ReadElementContentAsString();
                            break;
                        case "language":
                            ret.Culture = CachedPropertiesProvider.ConvertToCultureInfo(reader.ReadElementContentAsString());
                            break;
                        case "copyright":
                            ret.Copyright = reader.ReadElementContentAsString();
                            break;
                        case "managingEditor":
                            ret.ManagingEditor = reader.ReadElementContentAsString();
                            break;
                        case "webMaster":
                            ret.WebMaster = reader.ReadElementContentAsString();
                            break;
                        case "pubDate":
                            ret.PublicationDate = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "lastBuildDate":
                            ret.LastChanged = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "category":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Category = ConvertToIRssCategory(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "generator":
                            ret.Generator = reader.ReadElementContentAsString();
                            break;
                        case "docs":
                            ret.Doc = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString());
                            break;
                        case "cloud":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Cloud = ConvertToIRssCloud(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "ttl":
                            ret.TimeToLive = CachedPropertiesProvider.ConvertToInt(reader.ReadElementContentAsString());
                            break;
                        case "image":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Image = ConvertToIRssImage(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        /*case "rating":
                            break;*/
                        case "textInput":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.TextInput = ConvertToIRssTextInput(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "skipHours":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.SkipHours = ConvertToSkipHourList(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "skipDays":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.SkipDays = ConvertToDayOfWeekList(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "item":
                            using (XmlReader itemReader = reader.ReadSubtree())
                            {
                                ret.Items.Add(ParseItem(itemReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        default:
                            UnhandledElement(ret, reader);
                            break;
                    }
                }
            }
            reader.Close();
            return (T)ret;
        }

        /// <summary>
        /// Parses the item.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public IFeedItem ParseItem(XmlReader reader)
        {
            IRssFeedItem item = new RssFeedItem();
            reader.ReadToDescendant("item");

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "author":
                            item.Author = reader.ReadElementContentAsString();
                            break;
                        case "category":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Category = ConvertToIRssCategory(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "comments":
                            item.Comments = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString());
                            break;
                        case "description":
                            item.Description = reader.ReadElementContentAsString();
                            break;
                        case "enclosure":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Enclosure = ConvertToIRssEnclosure(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "guid":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Guid = ConvertToIRssGuid(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "link":
                            item.Link = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString());
                            break;
                        case "pubDate":
                            item.PublicationDate = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "source":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Source = ConvertToIRssSource(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "title":
                            item.Title = reader.ReadElementContentAsString();
                            break;
                        default:
                            UnhandledElement(item, reader);
                            break;
                    }
                }
            }
            reader.Close();
            return item;
        }

        /// <summary>
        /// Converts to I RSS cloud.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssCloud ConvertToIRssCloud(XmlReader input)
        {
            RssCloud cloud = new RssCloud();
            input.ReadToDescendant("cloud");
            cloud.Domain = input.GetAttribute("domain");
            cloud.Port = CachedPropertiesProvider.ConvertToInt(input.GetAttribute("port"));
            cloud.Path = input.GetAttribute("path");
            cloud.Protocol = ConvertToRssCloudProtocol(input.GetAttribute("protocol"));
            cloud.RegisterProcedure = input.GetAttribute("registerProcedure");
            input.Close();
            return cloud;
        }

        /// <summary>
        /// Converts to I RSS cloud.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssCloud ConvertToIRssCloud(string input)
        {
            return ConvertToIRssCloud(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS category.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssCategory ConvertToIRssCategory(XmlReader input)
        {
            RssCategory cat = new RssCategory();
            input.ReadToDescendant("category");
            cat.Domain = input.GetAttribute("domain");
            cat.CategoryName = input.ReadElementContentAsString();
            input.Close();
            return cat;
        }

        /// <summary>
        /// Converts to I RSS category.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssCategory ConvertToIRssCategory(string input)
        {
            return ConvertToIRssCategory(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS text input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssTextInput ConvertToIRssTextInput(XmlReader input)
        {
            RssTextInput text = new RssTextInput();
            input.ReadToDescendant("textInput");

            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "description":
                            text.Description = input.ReadElementContentAsString();
                            break;
                        case "link":
                            text.Link = CachedPropertiesProvider.ConvertToUri(input.ReadElementContentAsString());
                            break;
                        case "name":
                            text.Name = input.ReadElementContentAsString();
                            break;
                        case "title":
                            text.Title = input.ReadElementContentAsString();
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
            input.Close();
            return text;
        }

        /// <summary>
        /// Converts to I RSS text input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssTextInput ConvertToIRssTextInput(string input)
        {
            return ConvertToIRssTextInput(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to day of week list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IList<DayOfWeek> ConvertToDayOfWeekList(XmlReader input)
        {
            IList<DayOfWeek> skipDays = new List<DayOfWeek>();
            input.ReadToDescendant("skipDays");

            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element && input.Name == "day")
                {
                    skipDays.Add(ConvertToDayOfWeek(input.ReadElementContentAsString()));
                    readContent = true;
                }
            }
            input.Close();
            return skipDays;
        }

        /// <summary>
        /// Converts to day of week list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IList<DayOfWeek> ConvertToDayOfWeekList(string input)
        {
            return ConvertToDayOfWeekList(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to skip hour list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IList<uint> ConvertToSkipHourList(XmlReader input)
        {
            IList<uint> skipHours = new List<uint>();
            input.ReadToDescendant("skipHours");

            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element && input.Name == "hour")
                {
                    skipHours.Add(CachedPropertiesProvider.ConvertToUInt(input.ReadElementContentAsString()));
                    readContent = true;
                }
            }
            input.Close();
            return skipHours;
        }

        /// <summary>
        /// Converts to skip hour list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IList<uint> ConvertToSkipHourList(string input)
        {
            return ConvertToSkipHourList(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS image.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssImage ConvertToIRssImage(XmlReader input)
        {
            RssImage image = new RssImage();
            input.ReadToDescendant("image");

            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "description":
                            image.Description = input.ReadElementContentAsString();
                            break;
                        case "link":
                            image.Link = CachedPropertiesProvider.ConvertToUri(input.ReadElementContentAsString());
                            break;
                        case "location":
                            image.Location = CachedPropertiesProvider.ConvertToUri(input.ReadElementContentAsString());
                            break;
                        case "title":
                            image.Title = input.ReadElementContentAsString();
                            break;
                        case "width":
                            image.Width = CachedPropertiesProvider.ConvertToInt(input.ReadElementContentAsString());
                            break;
                        case "height":
                            image.Height = CachedPropertiesProvider.ConvertToInt(input.ReadElementContentAsString());
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
            input.Close();
            return image;
        }

        /// <summary>
        /// Converts to I RSS image.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssImage ConvertToIRssImage(string input)
        {
            return ConvertToIRssImage(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS enclosure.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssEnclosure ConvertToIRssEnclosure(XmlReader input)
        {
            RssEnclosure enc = new RssEnclosure();
            input.ReadToDescendant("enclosure");
            enc.Length = CachedPropertiesProvider.ConvertToInt(input.GetAttribute("length"));
            enc.Link = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("url"));
            enc.Type = input.GetAttribute("type");
            input.Close();
            return enc;
        }

        /// <summary>
        /// Converts to I RSS enclosure.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssEnclosure ConvertToIRssEnclosure(string input)
        {
            return ConvertToIRssEnclosure(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS GUID.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssGuid ConvertToIRssGuid(XmlReader input)
        {
            RssGuid guid = new RssGuid();
            input.ReadToDescendant("guid");
            guid.IsPermaLink = CachedPropertiesProvider.ConvertToBoolNullable(input.GetAttribute("isPermaLink"));
            guid.UniqueIdentifier = input.ReadElementContentAsString();
            input.Close();
            return guid;
        }

        /// <summary>
        /// Converts to I RSS GUID.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssGuid ConvertToIRssGuid(string input)
        {
            return ConvertToIRssGuid(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS source.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssSource ConvertToIRssSource(XmlReader input)
        {
            RssSource source = new RssSource();
            input.ReadToDescendant("source");
            source.Link = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("url"));
            source.Description = input.ReadElementContentAsString();
            input.Close();
            return source;
        }

        /// <summary>
        /// Converts to I RSS source.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssSource ConvertToIRssSource(string input)
        {
            return ConvertToIRssSource(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to RSS cloud protocol.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static RssCloudProtocol ConvertToRssCloudProtocol(string input)
        {
            input = input.Trim().ToLower();
            switch (input)
            {
                case "xml-rpc":
                    return RssCloudProtocol.XmlRpc;
                case "http-post":
                    return RssCloudProtocol.HttpPost;
                case "soap":
                    return RssCloudProtocol.Soap;
                default:
                    return RssCloudProtocol.Unknown;
            }
        }

        /// <summary>
        /// Converts to day of week.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static DayOfWeek ConvertToDayOfWeek(string input)
        {
            return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), input);
        }
    }
}
#endif

#if !(NOLOGGING)
namespace PublicDomain.Logging
{
    /// <summary>
    /// Severity of the log entry. The numeric value of the severity
    /// is in the name itself for immediate feedback.
    /// </summary>
    public enum LoggerSeverity
    {
        /// <summary>
        /// Lowest severity.
        /// </summary>
        None0 = 0,

        /// <summary>
        /// Detailed programmatic informational messages used
        /// as an aid in troubleshooting problems by programmers.
        /// </summary>
        Debug10 = 10,

        /// <summary>
        /// Brief informative messages to use as an aid in
        /// troubleshooting problems by production support and programmers.
        /// </summary>
        Info20 = 20,

        /// <summary>
        /// Messages intended to notify help desk, production support and programmers
        /// of possible issues with respect to the running application.
        /// </summary>
        Warn30 = 30,

        /// <summary>
        /// Messages that detail a programmatic error, these are typically messages
        /// intended for help desk, production support, programmers and occasionally users.
        /// </summary>
        Error40 = 40,

        /// <summary>
        /// Severe messages that are programmatic violations that will usually
        /// result in application failure. These messages are intended for help
        /// desk, production support, programmers and possibly users.
        /// </summary>
        Fatal50 = 50,

        /// <summary>
        /// 
        /// </summary>
        Infinity = int.MaxValue
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ILogFormatter
    {
        /// <summary>
        /// Formats the entry.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="category">The category.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        string FormatEntry(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string category, Dictionary<string, object> data);

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        /// <value>The format string.</value>
        string FormatString { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ILogFilter
    {
        /// <summary>
        /// Determines whether the specified severity is loggable.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <returns>
        /// 	<c>true</c> if the specified severity is loggable; otherwise, <c>false</c>.
        /// </returns>
        bool IsLoggable(LoggerSeverity threshold, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters);
    }

    /// <summary>
    /// There is no interface for this class to allow for certain methods
    /// to be overriden and removed in debug builds.
    /// </summary>
    public abstract class Logger
    {
        private LoggerSeverity m_threshold = LoggerSeverity.Warn30;

        private List<ILogFilter> m_filters = new List<ILogFilter>();

        private ILogFormatter m_formatter = new DefaultLogFormatter();

        private string m_category;

        private Dictionary<string, object> m_data = new Dictionary<string, object>();

        private static Dictionary<int, int> m_stack = new Dictionary<int, int>();

        internal static int LogStackCount
        {
            get
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                if (!m_stack.ContainsKey(threadId))
                {
                    m_stack[threadId] = 0;
                }
                return m_stack[threadId];
            }
            set
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                m_stack[threadId] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger()
            : this(new DefaultLogFilter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger(ILogFilter logFilter)
        {
            if (logFilter != null)
            {
                AddLogFilter(logFilter);
            }
        }

        /// <summary>
        /// The severity threshold at which point a log message
        /// is logged. For example, if the threshold is Debug,
        /// all messages with severity greater than or equal to Debug
        /// will be logged. All other messages will be discarded.
        /// The default threshold is Warn.
        /// </summary>
        /// <value></value>
        public virtual LoggerSeverity Threshold
        {
            get
            {
                return m_threshold;
            }
            set
            {
                m_threshold = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <value></value>
        public virtual ILogFormatter Formatter
        {
            get
            {
                return m_formatter;
            }
            set
            {
                m_formatter = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <value></value>
        public virtual List<ILogFilter> Filters
        {
            get
            {
                return m_filters;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        public virtual Dictionary<string, object> Data
        {
            get
            {
                return m_data;
            }
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public virtual string Category
        {
            get
            {
                return m_category;
            }
            set
            {
                m_category = value;
            }
        }

        /// <summary>
        /// Adds the log filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public virtual void AddLogFilter(ILogFilter filter)
        {
            Filters.Add(filter);
        }

        /// <summary>
        /// Removes the log filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public virtual void RemoveLogFilter(ILogFilter filter)
        {
            Filters.Remove(filter);
        }

        /// <summary>
        /// Clears the log filters.
        /// </summary>
        public virtual void ClearLogFilters()
        {
            Filters.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public virtual void Log(LoggerSeverity severity, object entry, params object[] formatParameters)
        {
            // Get the current timestamp
            DateTime timestamp = DateTime.UtcNow;

            // Check all the filters
            if (Filters != null)
            {
                foreach (ILogFilter filter in Filters)
                {
                    if (!filter.IsLoggable(Threshold, severity, timestamp, entry, formatParameters))
                    {
                        return;
                    }
                }
            }

            string logLine = null;

            if (Formatter == null)
            {
                if (entry != null)
                {
                    logLine = entry.ToString();
                }
            }
            else
            {
                logLine = Formatter.FormatEntry(severity, timestamp, entry, formatParameters, Category, Data);
            }

            DoLog(severity, timestamp, entry, formatParameters, logLine);
        }

        /// <summary>
        /// High level final log that is called with all of the detailed information
        /// and the final log line as the last parameter.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="logLine">The log line.</param>
        protected virtual void DoLog(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
            DoLog(logLine);
        }

        /// <summary>
        /// Called by the detailed version, forgetting about the details
        /// and simply having the final log line.
        /// </summary>
        /// <param name="logLine">The log line.</param>
        protected abstract void DoLog(string logLine);

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public virtual void LogDebug10(object entry, params object[] formatParameters)
        {
            Log(LoggerSeverity.Debug10, entry, formatParameters);
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public virtual void LogInfo20(object entry, params object[] formatParameters)
        {
            Log(LoggerSeverity.Info20, entry, formatParameters);
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public virtual void LogWarn30(object entry, params object[] formatParameters)
        {
            Log(LoggerSeverity.Warn30, entry, formatParameters);
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public virtual void LogError40(object entry, params object[] formatParameters)
        {
            Log(LoggerSeverity.Error40, entry, formatParameters);
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public virtual void LogFatal50(object entry, params object[] formatParameters)
        {
            Log(LoggerSeverity.Fatal50, entry, formatParameters);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public virtual void LogException(Exception ex)
        {
            LogException(ex, LoggerSeverity.Error40);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="severity">The severity.</param>
        public virtual void LogException(Exception ex, LoggerSeverity severity)
        {
            Log(severity, ex);
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        [Conditional("DEBUG")]
        public virtual void Start(params object[] args)
        {
            LogEntryExit(true, true, args);
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        [Conditional("DEBUG")]
        public virtual void End(params object[] args)
        {
            LogEntryExit(false, true, args);
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        [Conditional("DEBUG")]
        public virtual void WhereAmI(params object[] args)
        {
            LogEntryExit(false, false, args);
        }

        /// <summary>
        /// Logs the entry exit.
        /// </summary>
        /// <param name="isEntry">if set to <c>true</c> [is entry].</param>
        /// <param name="useMarker">if set to <c>true</c> [use marker].</param>
        /// <param name="args">The args.</param>
        protected virtual void LogEntryExit(bool isEntry, bool useMarker, object[] args)
        {
            StackTrace trace = new StackTrace(true);
            StackFrame caller = trace.GetFrame(2);
            MethodBase method = caller.GetMethod();
            int cnt = LogStackCount;

            if (useMarker)
            {
                cnt += (isEntry ? 1 : -1);

                if (cnt < 0)
                {
                    cnt = 0;
                }

                LogStackCount = cnt;
            }

            StringBuilder sb = new StringBuilder();
            if (useMarker)
            {
                if (isEntry)
                {
                    cnt--;
                }
            }
            else
            {
                cnt++;
            }

            if (cnt > 0)
            {
                sb.Append(' ', cnt);
            }

            if (useMarker)
            {
                if (isEntry)
                {
                    sb.Append("> ");
                }
                else
                {
                    sb.Append("< ");
                }
            }

            sb.AppendFormat(
                "{0}.{1} [{2}]",
                method.DeclaringType,
                method.Name,
                caller.GetFileLineNumber()
            );

            if (args != null)
            {
                sb.Append(" (");
                for (int i = 0; i < args.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    if (args[i] == null)
                    {
                        sb.Append("[null]");
                    }
                    else
                    {
                        sb.Append(args[i]);
                    }
                }
                sb.Append(")");
            }
            StackFrame caller2 = trace.GetFrame(3);
            if (caller2 != null)
            {
                sb.AppendFormat(
                    " {{{0}:{1}:{2}}}",
                    caller2.GetMethod().Name,
                    caller2.GetFileName(),
                    caller2.GetFileLineNumber()
                );
            }
            LogDebug10(sb.ToString());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileLogger : Logger
    {
        private string m_fileName;

        private static Dictionary<string, FileStream> m_streams = new Dictionary<string, FileStream>();

        /// <summary>
        /// 
        /// </summary>
        public static bool CacheFileStreams = false;

        private static object m_streamsLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        public FileLogger()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public FileLogger(string fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        ~FileLogger()
        {
            CloseAllStreams();
        }

        private static void CloseAllStreams()
        {
            lock (m_streamsLock)
            {
                foreach (FileStream fileStream in m_streams.Values)
                {
                    fileStream.Flush();
                    fileStream.Close();
                }
                m_streams.Clear();
            }
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public virtual string FileName
        {
            get
            {
                return m_fileName;
            }
            set
            {
                m_fileName = value;
                if (!string.IsNullOrEmpty(m_fileName))
                {
                    FileSystemUtilities.EnsureDirectoriesInPath(m_fileName);
                }
            }
        }

        /// <summary>
        /// High level final log that is called with all of the detailed information
        /// and the final log line as the last parameter.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
            string fileName = GetFileName(severity, timestamp, entry, formatParameters, logLine);

            if (!string.IsNullOrEmpty(fileName))
            {
                FileStream stream = GetStream(fileName);

                try
                {
                    byte[] data = Encoding.Default.GetBytes(logLine);

                    stream.Write(data, 0, data.Length);
                    stream.WriteByte((byte)'\n');
                    stream.Flush();
                }
                finally
                {
                    if (!CacheFileStreams)
                    {
                        stream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        protected virtual FileStream GetStream(string fileName)
        {
            FileStream result = null;

            lock (m_streamsLock)
            {
                if (!CacheFileStreams || !m_streams.TryGetValue(fileName, out result))
                {
                    result = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                    if (CacheFileStreams)
                    {
                        m_streams[fileName] = result;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Called by the detailed version, forgetting about the details
        /// and simply having the final log line.
        /// </summary>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(string logLine)
        {
            // Should never get here
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetFileName(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
            return FileName;
        }
    }

    /// <summary>
    /// Provides a common application logger, which writes to a rolling
    /// log file in the application's working directory. The logger
    /// always logs severe log events using the <see cref="PublicDomain.Logging.SevereLogFilter"/>,
    /// and by default, uses the default Logger <see cref="PublicDomain.Logging.Logger.Threshold"/> value of Warn.
    /// </summary>
    public class ApplicationLogger : CompositeLogger
    {
        /// <summary>
        /// Static logger provides a common application logger, which writes to a rolling
        /// log file in the application's working directory. The logger
        /// always logs severe log events using the <see cref="PublicDomain.Logging.SevereLogFilter"/>,
        /// and by default, uses the default Logger <see cref="PublicDomain.Logging.Logger.Threshold"/> value of Warn.
        /// </summary>
        public static ApplicationLogger Current = new ApplicationLogger();

        /// <summary>
        /// Provides a common application logger, which writes to a rolling
        /// log file in the application's working directory. The logger
        /// always logs severe log events using the <see cref="PublicDomain.Logging.SevereLogFilter"/>,
        /// and by default, uses the default Logger <see cref="PublicDomain.Logging.Logger.Threshold"/> value.
        /// Initializes a new instance of the <see cref="ApplicationLogger"/> class.
        /// </summary>
        public ApplicationLogger()
        {
            // Figure out where we'll be logging the files
            string fileNameFormatted = FileSystemUtilities.PathCombine(Environment.CurrentDirectory, @"\app{0}.log");
            Loggers.Add(new RollingFileLogger(fileNameFormatted));
            AddLogFilter(new SevereLogFilter());

            // So that we know where the log is going
            string msg = string.Format("Application logging to {0}", fileNameFormatted);
            Console.WriteLine(msg);

#if DEBUG
            // Sometimes the Console does not go anywhere logical (or nowhere at all),
            // so it becomes difficult to know where the current directory is. Therefore,
            // we write the same message to a global file
            try
            {
                Logger loggers = new FileLogger(GlobalConstants.PublicDomainDefaultInstallLocation + @"loggers.log");
                loggers.Threshold = LoggerSeverity.Info20;
                loggers.LogInfo20(msg);
            }
            catch (Exception)
            {
                // No permissions to write to the directory, and we don't bother trying anywhere else
            }
#endif
        }
    }

    /// <summary>
    /// Can be used for logging based on a class name, which is used
    /// as the category. Also, delineates a new static run on the first log, in debug mode.
    /// </summary>
    public class SimpleCompositeLogger : CompositeLogger
    {
		private string m_className;
        private string m_prefix;
        private const string InitialLine = "NEW STATIC INITIALIZATION";
        internal static readonly int CATEGORY_LENGTH = 10;

        private static int logcount = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCompositeLogger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="className">Name of the class.</param>
        public SimpleCompositeLogger(Logger log, string className)
            : base(log)
		{
            ClearLogFilters();
            AddLogFilter(new SevereLogFilter());

            m_className = className;
            if (!string.IsNullOrEmpty(className))
            {
                // First, we create a friendly name for the logger which we
                // will use as the "category"
                int lastPeriod = m_className.LastIndexOf('.');
                if (lastPeriod != -1)
                {
                    m_prefix = m_className.Substring(lastPeriod + 1);
                }
                else
                {
                    m_prefix = m_className;
                }

                // Pad the prefix to ten characters
                if (m_prefix.Length > CATEGORY_LENGTH)
                {
                    m_prefix = m_prefix.Substring(0, CATEGORY_LENGTH);
                }
                else if (m_prefix.Length < CATEGORY_LENGTH)
                {
                    m_prefix = string.Format("{0,-" + CATEGORY_LENGTH + "}", m_prefix);
                }
            }
		}

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void Log(LoggerSeverity severity, object entry, params object[] formatParameters)
        {
            if (logcount++ == 1)
            {
                base.Log(LoggerSeverity.Infinity, null);
                base.Log(LoggerSeverity.Infinity, InitialLine);
            }

            Category = m_prefix;
            if (severity == LoggerSeverity.Fatal50)
            {
                // TODO call a notification interface, which could do something like
                // send an email
            }
            base.Log(severity, entry, formatParameters);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SimpleLogFormatter : DefaultLogFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleLogFormatter"/> class.
        /// </summary>
        public SimpleLogFormatter()
        {
            FormatString = "[{0} {3,5} {1,-7}{4}] {2}";
        }

        /// <summary>
        /// Does the format entry.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="logEntry">The log entry.</param>
        /// <param name="category">The category.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected override string DoFormatEntry(LoggerSeverity severity, DateTime timestamp, string logEntry, string category, Dictionary<string, object> data)
        {
            if (!string.IsNullOrEmpty(category))
            {
                category = " " + category;
            }
            return string.Format(FormatString, timestamp.ToString("s"), severity, logEntry, Thread.CurrentThread.ManagedThreadId, category);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NullLogger : SimpleCompositeLogger
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly NullLogger Current = new NullLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="NullLogger"/> class.
        /// </summary>
        public NullLogger()
			: base(null, null)
		{
            Threshold = LoggerSeverity.None0;
		}

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void Log(LoggerSeverity severity, object entry, params object[] formatParameters)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void LogDebug10(object entry, params object[] formatParameters)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void LogError40(object entry, params object[] formatParameters)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void LogFatal50(object entry, params object[] formatParameters)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void LogInfo20(object entry, params object[] formatParameters)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void LogWarn30(object entry, params object[] formatParameters)
        {
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        public override void Start(params object[] args)
        {
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        public override void End(params object[] args)
        {
        }

        /// <summary>
        /// Logs the entry exit.
        /// </summary>
        /// <param name="isEntry">if set to <c>true</c> [is entry].</param>
        /// <param name="useMarker">if set to <c>true</c> [use marker].</param>
        /// <param name="args">The args.</param>
        protected override void LogEntryExit(bool isEntry, bool useMarker, object[] args)
        {
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public override void LogException(Exception ex)
        {
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="severity">The severity.</param>
        public override void LogException(Exception ex, LoggerSeverity severity)
        {
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        public override void WhereAmI(params object[] args)
        {
        }

        /// <summary>
        /// High level final log that is called with all of the detailed information
        /// and the final log line as the last parameter.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
        }

        /// <summary>
        /// Does the log.
        /// </summary>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(string logLine)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LoggingConfig
    {
        private Dictionary<string, Logger> m_loggers = new Dictionary<string, Logger>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public delegate Logger CallbackCreateLogger(string className, LoggerSeverity threshold);

        /// <summary>
        /// 
        /// </summary>
        public const string AllLoggersDesignator = "all";

        /// <summary>
        /// 
        /// </summary>
        public static bool Enabled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingConfig"/> class.
        /// </summary>
        public LoggingConfig()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingConfig"/> class.
        /// </summary>
        /// <param name="configString">The config string.</param>
        /// <param name="createLogger">The create logger.</param>
        public LoggingConfig(string configString, CallbackCreateLogger createLogger)
        {
            Load(configString, createLogger);
        }

        /// <summary>
        /// Loads the specified config string.
        /// </summary>
        /// <param name="configString">The config string.</param>
        /// <param name="createLogger">The create logger.</param>
        public void Load(string configString, CallbackCreateLogger createLogger)
        {
            if (configString != null && Enabled)
            {
                string[] pieces = configString.Trim().Split(';');
                if (pieces.Length == 1 && pieces[0] == "")
                {
                    return;
                }
                foreach (string piece in pieces)
                {
                    string[] parts = piece.Trim().Split('=');
                    if (parts != null && parts.Length > 0 && parts.Length <= 2 && parts[0].Trim().Length > 0)
                    {
                        string key = parts[0].ToLower().Trim();

                        if (key == "*")
                        {
                            key = AllLoggersDesignator;
                        }

                        string val = parts.Length == 1 ? "*" : parts[1];
                        LoggerSeverity threshold = GetLogValue(val);
                        if (threshold == LoggerSeverity.Infinity)
                        {
                            m_loggers[key] = NullLogger.Current;
                        }
                        else
                        {
                            m_loggers[key] = createLogger(parts[0].Trim(), threshold);
                        }
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Check format of log string ({0}).", piece));
                    }
                }
            }
        }

        private static LoggerSeverity GetDefaultLogThreshold()
        {
            return LoggerSeverity.None0;
        }

        /// <summary>
        /// Gets the log value.
        /// </summary>
        /// <param name="logValue">The log value.</param>
        /// <returns></returns>
        private LoggerSeverity GetLogValue(string logValue)
        {
            if (logValue == null)
            {
                throw new ArgumentNullException("logValue");
            }
            string val = logValue.ToLower().Trim();

            if (val == "" || val == "*" || val == "on" || val == "1" || val == AllLoggersDesignator)
            {
                return GetDefaultLogThreshold();
            }
            else if (val == "off" || val == "0")
            {
                return GetOffValue();
            }
            string[] names = Enum.GetNames(typeof(LoggerSeverity));
            foreach (string name in names)
            {
                if (name.ToLower().StartsWith(val))
                {
                    return (LoggerSeverity)Enum.Parse(typeof(LoggerSeverity), name);
                }
            }

            // If we can't figure out the value, throw an exception
            throw new ArgumentException(string.Format("Unknown logging value {0}", logValue));
        }

        private LoggerSeverity GetOffValue()
        {
            return LoggerSeverity.Infinity;
        }

        /// <summary>
        /// Gets the <see cref="PublicDomain.Logging.Logger"/> with the specified log class.
        /// </summary>
        /// <param name="logClasses">The log classes.</param>
        /// <returns></returns>
        /// <value></value>
        public Logger CreateLogger(params string[] logClasses)
        {
            Logger result = NullLogger.Current;

            Logger test;
            string testClass;

            foreach (string logClass in logClasses)
            {
                testClass = logClass.ToLower().Trim();
                if (m_loggers.TryGetValue(testClass, out test))
                {
                    result = test;
                }
            }

            if (object.ReferenceEquals(result, NullLogger.Current) && m_loggers.TryGetValue(AllLoggersDesignator, out test))
            {
                result = test;
            }

            return result;
        }

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="otherLogClasses">The other log classes.</param>
        /// <returns></returns>
        public Logger CreateLogger(Type type, params string[] otherLogClasses)
        {
            string[] classes = new string[otherLogClasses.Length + 1];
            Array.Copy(otherLogClasses, classes, otherLogClasses.Length);
            classes[classes.Length - 1] = type.ToString();
            return CreateLogger(classes);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IRollOverStrategy
    {
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="logLine">The log line.</param>
        /// <returns></returns>
        string GetFileName(string fileName, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine);
    }

    /// <summary>
    /// Writes to a file, rolling over to a new version of a file
    /// when the previous file has filled to capacity.
    /// </summary>
    public class RollingFileLogger : FileLogger
    {
        private IRollOverStrategy m_strategy;

        /// <summary>
        /// Writes to a file, rolling over to a new version of a file
        /// when the previous file has filled to capacity.
        /// Initializes a new instance of the <see cref="RollingFileLogger"/> class.
        /// </summary>
        /// <param name="fileNameFormatted">The file name formatted.</param>
        public RollingFileLogger(string fileNameFormatted)
            : this(fileNameFormatted, new FileSizeRollOverStrategy())
        {
        }

        /// <summary>
        /// Writes to a file, rolling over to a new version of a file
        /// when the previous file has filled to capacity.
        /// Initializes a new instance of the <see cref="RollingFileLogger"/> class.
        /// </summary>
        /// <param name="fileNameFormatted">The file name formatted.</param>
        /// <param name="strategy">The strategy.</param>
        public RollingFileLogger(string fileNameFormatted, IRollOverStrategy strategy)
            : base(fileNameFormatted)
        {
            Strategy = strategy;
        }

        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>The strategy.</value>
        public IRollOverStrategy Strategy
        {
            get
            {
                return m_strategy;
            }
            set
            {
                m_strategy = value;
            }
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="timestamp"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        /// <param name="logLine"></param>
        /// <returns></returns>
        protected override string GetFileName(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
            string fileName = base.GetFileName(severity, timestamp, entry, formatParameters, logLine);

            if (Strategy != null)
            {
                fileName = Strategy.GetFileName(fileName, severity, timestamp, entry, formatParameters, logLine);
            }

            return fileName;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileSizeRollOverStrategy : IRollOverStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        static FileSizeRollOverStrategy()
        {
            DefaultFileSizeStrategyBytes = GlobalConstants.BytesInAMegabyte * 10;
        }

        /// <summary>
        /// 10 megs
        /// </summary>
        public static readonly int DefaultFileSizeStrategyBytes;

        private long m_maxFileSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSizeRollOverStrategy"/> class.
        /// </summary>
        public FileSizeRollOverStrategy()
            : this (DefaultFileSizeStrategyBytes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSizeRollOverStrategy"/> class.
        /// </summary>
        /// <param name="fileSizeBytes">The file size bytes.</param>
        public FileSizeRollOverStrategy(long fileSizeBytes)
        {
            m_maxFileSize = fileSizeBytes;
        }

        /// <summary>
        /// Gets or sets the max size of the log file after which
        /// a new log file is started.
        /// </summary>
        /// <value>The size of the max file.</value>
        public long MaxFileSize
        {
            get
            {
                return m_maxFileSize;
            }
            set
            {
                m_maxFileSize = value;
            }
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="logLine">The log line.</param>
        /// <returns></returns>
        public string GetFileName(string fileName, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
            // First, find the largest numbered file
            string[] pieces = FileSystemUtilities.SplitFileIntoDirectoryAndName(fileName, true);
            string search = pieces[1].Replace("{0}", "*");

            string[] files = Directory.GetFiles(pieces[0], search);

            int maxNumber = 0;

            // Now, build the list of numbers from the file names
            if (files != null)
            {
                foreach (string foundFile in files)
                {
                    int foundNumber = StringUtilities.ExtractFirstNumber(foundFile);
                    if (foundNumber > maxNumber)
                    {
                        maxNumber = foundNumber;
                    }
                }
            }

            if (maxNumber == 0)
            {
                fileName = string.Format(fileName, maxNumber + 1);
            }
            else
            {
                string checkFileName = string.Format(fileName, maxNumber);

                // Check if this file is too big or not
                if (new FileInfo(checkFileName).Length >= MaxFileSize)
                {
                    // Increment the file number
                    fileName = string.Format(fileName, maxNumber + 1);
                }
                else
                {
                    fileName = checkFileName;
                }
            }

            return fileName;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TextWriterLogger : Logger
    {
        private TextWriter m_writer;

        /// <summary>
        /// Gets or sets the writer.
        /// </summary>
        /// <value>The writer.</value>
        public virtual TextWriter Writer
        {
            get
            {
                return m_writer;
            }
            set
            {
                m_writer = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWriterLogger"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public TextWriterLogger(TextWriter writer)
        {
            m_writer = writer;
        }

        /// <summary>
        /// Does the log.
        /// </summary>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(string logLine)
        {
            m_writer.WriteLine(logLine);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConsoleLogger : TextWriterLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
        /// </summary>
        public ConsoleLogger()
            : base(Console.Out)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class LogFormatter : ILogFormatter
    {
        private string m_formatString;

        /// <summary>
        /// </summary>
        /// <value></value>
        public virtual string FormatString
        {
            get
            {
                return m_formatString;
            }
            set
            {
                m_formatString = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="timestamp"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        /// <param name="category"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string FormatEntry(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string category, Dictionary<string, object> data)
        {
            string logEntry = PrepareEntry(entry, formatParameters);

            return DoFormatEntry(severity, timestamp, logEntry, category, data);
        }

        /// <summary>
        /// Does the format entry.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="logEntry">The log entry.</param>
        /// <param name="category">The category.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected abstract string DoFormatEntry(LoggerSeverity severity, DateTime timestamp, string logEntry, string category, Dictionary<string, object> data);

        /// <summary>
        /// Prepares the entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <returns></returns>
        protected virtual string PrepareEntry(object entry, object[] formatParameters)
        {
            if (entry == null) return null;

            if (formatParameters != null && formatParameters.Length > 0)
            {
                entry = string.Format(entry.ToString(), formatParameters);
            }

            return entry.ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DefaultLogFormatter : LogFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLogFormatter"/> class.
        /// </summary>
        public DefaultLogFormatter()
        {
            FormatString = "[{0} {1,-7}{3}] {2}";
        }

        /// <summary>
        /// Does the format entry.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="logEntry">The log entry.</param>
        /// <param name="category">The category.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected override string DoFormatEntry(LoggerSeverity severity, DateTime timestamp, string logEntry, string category, Dictionary<string, object> data)
        {
            if (!string.IsNullOrEmpty(category))
            {
                category = " " + category;
            }
            return string.Format(FormatString, timestamp, severity, logEntry, category);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DefaultLogFilter : ILogFilter
    {
        /// <summary>
        /// Determines whether the specified severity is loggable.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <returns>
        /// 	<c>true</c> if the specified severity is loggable; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsLoggable(LoggerSeverity threshold, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters)
        {
            return (int)severity >= (int)threshold;
        }
    }

    /// <summary>
    /// Always logs severe events, otherwise defers to normal threshold
    /// conditions.
    /// </summary>
    public class SevereLogFilter : DefaultLogFilter
    {
        /// <summary>
        /// Always logs severe events, otherwise defers to normal threshold
        /// conditions. Initializes a new instance of the <see cref="SevereLogFilter"/> class.
        /// </summary>
        public SevereLogFilter()
            : base()
        {
        }

        /// <summary>
        /// Determines whether the specified severity is loggable.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <returns>
        /// 	<c>true</c> if the specified severity is loggable; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsLoggable(LoggerSeverity threshold, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters)
        {
            return severity == LoggerSeverity.Fatal50 ? true : base.IsLoggable(threshold, severity, timestamp, entry, formatParameters);
        }
    }

    /// <summary>
    /// By default does not have any filters, and supposes that the composed logs will filter.
    /// </summary>
    public class CompositeLogger : Logger
    {
        private List<Logger> m_loggers = new List<Logger>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeLogger"/> class.
        /// </summary>
        /// <param name="loggers">The loggers.</param>
        public CompositeLogger(params Logger[] loggers)
        {
            foreach (Logger logger in loggers)
            {
                if (logger != null)
                {
                    Loggers.Add(logger);
                }
            }
        }

        /// <summary>
        /// Gets the loggers.
        /// </summary>
        /// <value>The loggers.</value>
        public virtual List<Logger> Loggers
        {
            get
            {
                return m_loggers;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void Log(LoggerSeverity severity, object entry, params object[] formatParameters)
        {
            foreach (Logger logger in Loggers)
            {
                logger.Log(severity, entry, formatParameters);
            }
        }

        /// <summary>
        /// Does the log.
        /// </summary>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(string logLine)
        {
            // This is not called
        }

        /// <summary>
        /// The severity threshold at which point a log message
        /// is logged. For example, if the threshold is Debug,
        /// all messages with severity greater than or equal to Debug
        /// will be logged. All other messages will be discarded.
        /// The default threshold is Warn.
        /// </summary>
        /// <value></value>
        public override LoggerSeverity Threshold
        {
            get
            {
                return base.Threshold;
            }
            set
            {
                base.Threshold = value;
                foreach (Logger logger in Loggers)
                {
                    logger.Threshold = value;
                }
            }
        }
    }
}
#endif

namespace PublicDomain.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationValues
    {
        private Dictionary<string, string> m_values = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValues"/> class.
        /// </summary>
        public ConfigurationValues()
        {
        }

        /// <summary>
        /// Reads the parameters from assembly.
        /// </summary>
        /// <param name="assemblyStreamName">Name of the assembly stream.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public ConfigurationValues(string assemblyStreamName, bool intersectAlternateConfig)
            : this(assemblyStreamName, Assembly.GetExecutingAssembly(), intersectAlternateConfig)
        {
        }

        /// <summary>
        /// Reads the parameters from assembly.
        /// </summary>
        /// <param name="assemblyStreamName">Name of the assembly stream.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public ConfigurationValues(string assemblyStreamName, Assembly assembly, bool intersectAlternateConfig)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            Stream stream = assembly.GetManifestResourceStream(assemblyStreamName);
            if (stream == null)
            {
                throw new ArgumentNullException(string.Format("Could not find embedded resource named {0} in assembly {1}.", assemblyStreamName, assembly));
            }
            ReadParametersFromStream(stream, intersectAlternateConfig);
        }

        /// <summary>
        /// Reads the parameters from file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public void ReadParametersFromStream(string fileName, bool intersectAlternateConfig)
        {
            using (TextReader textReader = new StreamReader(fileName))
            {
                ReadParametersFromTextReader(textReader, intersectAlternateConfig);
            }
        }

        /// <summary>
        /// Reads the parameters from stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public void ReadParametersFromStream(Stream stream, bool intersectAlternateConfig)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            using (XmlReader reader = XmlReader.Create(stream))
            {
                ReadParameters(reader, intersectAlternateConfig);
            }
        }

        /// <summary>
        /// Reads the parameters from text reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public void ReadParametersFromTextReader(TextReader reader, bool intersectAlternateConfig)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            using (XmlReader xmlReader = XmlReader.Create(reader))
            {
                ReadParameters(xmlReader, intersectAlternateConfig);
            }
        }

        private void ReadParameters(XmlReader reader, bool intersectAlternateConfig)
        {
            ReadParamsReader(reader);

            if (intersectAlternateConfig)
            {
                string alternateConfigFile;
                if (TryGetString("externalconfig", out alternateConfigFile))
                {
                    if (File.Exists(alternateConfigFile))
                    {
                        ConfigurationValues fileValues = new ConfigurationValues();
                        fileValues.ReadParametersFromStream(alternateConfigFile, false);
                        IntersectValues(fileValues);
                    }
                }
            }
        }

        private void ReadParamsReader(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.LocalName.ToLower() == "param")
                {
                    string name = reader.GetAttribute("name");
                    string val = reader.GetAttribute("value");
                    if (!string.IsNullOrEmpty(name))
                    {
                        m_values[name.ToLower()] = val;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the value with the specified key.
        /// </summary>
        /// <value></value>
        public string this[string key]
        {
            get
            {
                string result;
                if (!m_values.TryGetValue(key.ToLower(), out result))
                {
                    // Next, go to the machine config
                    Configuration machineConfig = ConfigurationManager.OpenMachineConfiguration();
                    if (machineConfig.AppSettings != null)
                    {
                        KeyValueConfigurationElement k = machineConfig.AppSettings.Settings[key];
                        if (k != null)
                        {
                            result = k.Value;
                            this[key] = result;
                        }
                    }
                }
                return result;
            }
            set
            {
                m_values[key.ToLower()] = value;
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<string> Keys
        {
            get
            {
                return m_values.Keys;
            }
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetString(string key)
        {
            return GetString(key, null);
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : val;
        }

        /// <summary>
        /// Gets the long.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public long GetLong(string key)
        {
            return GetLong(key, 0);
        }

        /// <summary>
        /// Gets the long.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public long GetLong(string key, long defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : long.Parse(val);
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public int GetInt(string key, int defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : int.Parse(val);
        }

        /// <summary>
        /// Tries the get string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public bool TryGetString(string key, out string val)
        {
            val = null;
            string config = this[key];
            if (config != null)
            {
                val = config;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Intersects the values.
        /// </summary>
        /// <param name="intersectValues">The intersect values.</param>
        public void IntersectValues(ConfigurationValues intersectValues)
        {
            foreach (string key in intersectValues.Keys)
            {
                this[key] = intersectValues[key];
            }
        }

        private bool m_wasExternalConfigRead;

        /// <summary>
        /// Gets or sets a value indicating whether [was external config read].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [was external config read]; otherwise, <c>false</c>.
        /// </value>
        public bool WasExternalConfigRead
        {
            get
            {
                return m_wasExternalConfigRead;
            }
            set
            {
                m_wasExternalConfigRead = value;
            }
        }
    }
}

#if !(NODYNACODE)
namespace PublicDomain.Dynacode
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICodeRunner
    {
        /// <summary>
        /// Runs the specified arguments.
        /// </summary>
        /// <param name="compilerResults">The compiler results.</param>
        /// <param name="execMethod">The exec method.</param>
        /// <param name="output">The output.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        object Run(CompilerResults compilerResults, string execMethod, StringBuilder output, params string[] arguments);

        /// <summary>
        /// Runs to string.
        /// </summary>
        /// <param name="compilerResults">The compiler results.</param>
        /// <param name="execMethod">The exec method.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        string RunToString(CompilerResults compilerResults, string execMethod, params string[] arguments);

        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        Language Language { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DotNetCodeRunner : ICodeRunner
    {
        /// <summary>
        /// 
        /// </summary>
        protected Language m_language;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCodeRunner"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        public DotNetCodeRunner(Language language)
        {
            m_language = language;
        }

        /// <summary>
        /// Runs the specified arguments.
        /// </summary>
        /// <param name="compilerResults">The compiler results.</param>
        /// <param name="execMethod">The exec method.</param>
        /// <param name="output">The output.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public virtual object Run(CompilerResults compilerResults, string execMethod, StringBuilder output, params string[] arguments)
        {
            // Find the method in the assembly
            using (new ConsoleRerouter(output))
            {
                return ReflectionUtilities.InvokeMethod(compilerResults.CompiledAssembly, execMethod, new object[] { arguments });
            }
        }

        /// <summary>
        /// Runs to string.
        /// </summary>
        /// <param name="compilerResults">The compiler results.</param>
        /// <param name="execMethod">The exec method.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public virtual string RunToString(CompilerResults compilerResults, string execMethod, params string[] arguments)
        {
            StringBuilder sb = new StringBuilder();
            Run(compilerResults, execMethod, sb, arguments);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        public virtual Language Language
        {
            get
            {
                return m_language;
            }
        }
    }

    /// <summary>
    /// Methods for working with code and languages.
    /// </summary>
    public static class CodeUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultNamespace = "DefaultNamespace";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultClassName = "DefaultClassName";

        private static Language[] s_supportedLanguages;

        static CodeUtilities()
        {
            List<Language> supportedLanguages = new List<Language>();
            supportedLanguages.Add(Language.CSharp);
            supportedLanguages.Add(Language.VisualBasic);
            s_supportedLanguages = supportedLanguages.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StripNonIdentifierCharacters(Language lang, string str)
        {
            switch (lang)
            {
                case Language.CSharp:
                case Language.JSharp:
                    // http://www.ecma-international.org/publications/standards/Ecma-334.htm
                    // Page 70, Printed Page 92

                    // TODO The following is not complete
                    // TODO JSharp should its own version of this -- it is different

                    str = StringUtilities.RemoveCharactersInverse(str, '_', 'a', '-', 'z', 'A', '-', 'Z', '0', '-', '9');
                    break;
                case Language.VisualBasic:
                    // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/vbls7/html/vblrfVBSpec2_2.asp

                    str = StringUtilities.RemoveCharactersInverse(str, '_', 'a', '-', 'z', 'A', '-', 'Z', '0', '-', '9', '\\', '[', '\\', ']');
                    break;
                default:
                    throw new NotImplementedException();
            }
            return str;
        }

        /// <summary>
        /// Evals the snippet.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="simpleCode">The simple code.</param>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.CompileException" />
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.NativeCompileException" />
        /// <returns></returns>
        public static string EvalSnippet(Language language, string simpleCode)
        {
            return Eval(language, GetSnippetCode(language, simpleCode));
        }

        /// <summary>
        /// Runs a snippet of code.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="code">The code.</param>
        /// <param name="arguments">The arguments.</param>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.CompileException" />
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.NativeCompileException" />
        /// <returns></returns>
        public static string Eval(Language language, string code, params string[] arguments)
        {
            return Eval(language, code, true, arguments);
        }

        /// <summary>
        /// Evals the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="code">The code.</param>
        /// <param name="isSnippet">if set to <c>true</c> [is snippet].</param>
        /// <param name="arguments">The arguments.</param>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.CompileException" />
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.NativeCompileException" />
        /// <returns></returns>
        public static string Eval(Language language, string code, bool isSnippet, params string[] arguments)
        {
            CompilerResults compilerResults = Compile(language, code, isSnippet, true);

            // Now, run the code
            ICodeRunner codeRunner = GetCodeRunner(language);
            return codeRunner.RunToString(compilerResults, string.Format("{0}.{1}.{2}", DefaultNamespace, DefaultClassName, GetDefaultMainMethodName(language)), arguments);
        }

        /// <summary>
        /// Compiles the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="code">The code.</param>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.CompileException" />
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.NativeCompileException" />
        /// <returns></returns>
        public static CompilerResults Compile(Language language, string code)
        {
            return Compile(language, code, true, true);
        }

        /// <summary>
        /// Compiles the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="code">The code.</param>
        /// <param name="isSnippet">if set to <c>true</c> then <paramref name="code"/> will be placed into
        /// templated "application code", such as a static void main.</param>
        /// <param name="throwExceptionOnCompileError">if set to <c>true</c> [throw exception on compile error].</param>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.CompileException" />
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.NativeCompileException" />
        /// <returns></returns>
        public static CompilerResults Compile(Language language, string code, bool isSnippet, bool throwExceptionOnCompileError)
        {
            using (CodeDomProvider domProvider = CodeDomProvider.CreateProvider(language.ToString()))
            {
                CompilerInfo compilerInfo = CodeDomProvider.GetCompilerInfo(language.ToString());
                CompilerParameters compilerParameters = compilerInfo.CreateDefaultCompilerParameters();
                PrepareCompilerParameters(language, compilerParameters);
                if (isSnippet)
                {
                    code = GetApplicationCode(language, code, DefaultClassName, DefaultNamespace);
                }
                CompilerResults results = domProvider.CompileAssemblyFromSource(compilerParameters, code);
                if (throwExceptionOnCompileError)
                {
                    CheckCompilerResultsThrow(results);
                }
                return results;
            }
        }

        /// <summary>
        /// Prepares the compiler parameters.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="compilerParameters">The compiler parameters.</param>
        public static void PrepareCompilerParameters(Language language, CompilerParameters compilerParameters)
        {
            switch (language)
            {
                case Language.CSharp:
                    break;
                case Language.VisualBasic:
                    compilerParameters.ReferencedAssemblies.Add(@"c:\windows\Microsoft.NET\Framework\v2.0.50727\System.dll");
                    compilerParameters.ReferencedAssemblies.Add(@"c:\windows\Microsoft.NET\Framework\v2.0.50727\System.Xml.dll");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the supported languages.
        /// </summary>
        /// <returns></returns>
        public static Language[] GetSupportedLanguages()
        {
            return s_supportedLanguages;
        }

        /// <summary>
        /// Gets the code runner.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static ICodeRunner GetCodeRunner(Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                case Language.VisualBasic:
                case Language.JSharp:
                    return new DotNetCodeRunner(language);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the name of the default main method.
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <returns></returns>
        public static string GetDefaultMainMethodName(Language lang)
        {
            switch (lang)
            {
                case Language.CSharp:
                case Language.VisualBasic:
                    return "Main";
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the application template code.
        /// Parameters:
        /// 0: Code
        /// 1: Class Name
        /// 2: Namespace
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <returns></returns>
        public static string GetApplicationTemplateCode(Language lang)
        {
            switch (lang)
            {
                case Language.CSharp:
                    return @"using System;
using System.Collections.Generic;
using System.Text;

namespace {2}
{{
    public class {1}
    {{
        public static void Main(string[] args)
        {{
            {0}
        }}
    }}
}}
";
                case Language.VisualBasic:
                    return @"Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace {2}
    Module {1}
        Sub Main(ByVal Args() as String)
            {0}
        End Sub
    End Module
End Namespace
";
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the snippet code.
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static string GetSnippetCode(Language lang, string code)
        {
            return string.Format(GetSnippetTemplateCode(lang), code);
        }

        /// <summary>
        /// Gets the snippet template code.
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <returns></returns>
        public static string GetSnippetTemplateCode(Language lang)
        {
            switch (lang)
            {
                case Language.CSharp:
                    return @"Console.Write({0});";
                case Language.VisualBasic:
                    return @"Console.Write({0})";
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the application code.
        /// Parameters:
        /// 0: Code
        /// 1: Class Name
        /// 2: Namespace
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static string GetApplicationCode(Language lang, params string[] args)
        {
            return string.Format(GetApplicationTemplateCode(lang), args);
        }

        /// <summary>
        /// This method throws an Exception if it finds an error in the
        /// <c>results</c>, otherwise it returns without side effect.
        /// </summary>
        /// <param name="results"></param>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.CompileException"/>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.NativeCompileException"/>
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
        /// Gets the display name of the language.
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <returns></returns>
        public static string GetLanguageDisplayName(Language lang)
        {
            switch (lang)
            {
                case Language.CPlusPlus:
                    return "C++";
                case Language.CSharp:
                    return "C#";
                case Language.Java:
                    return "Java";
                case Language.JScript:
                    return "JScript";
                case Language.JSharp:
                    return "J#";
                case Language.PHP:
                    return "PHP";
                case Language.Ruby:
                    return "Ruby";
                case Language.VisualBasic:
                    return "Visual Basic";
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a <seealso cref="PublicDomain.Language"/> given a string name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static Language GetLanguageByName(string name)
        {
            name = name.ToLower().Trim();
            switch (name)
            {
                case "cplusplus":
                case "c++":
                    return Language.CPlusPlus;
                case "c#":
                case "csharp":
                    return Language.CSharp;
                case "java":
                    return Language.Java;
                case "js":
                case "jscript":
                    return Language.JScript;
                case "vj#":
                case "j#":
                case "jsharp":
                    return Language.JSharp;
                case "php":
                    return Language.PHP;
                case "vb":
                case "visual basic":
                case "visualbasic":
                    return Language.VisualBasic;
                case "ruby":
                    return Language.Ruby;
                default:
                    throw new ArgumentException("Could not find language by name " + name);
            }
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
}
#endif

#if !(NOASPNETRUNTIMEHOST)

// This entire namespace is courtesy of Rick Strahl (http://www.west-wind.com/), who
// has marked it as Public Domain (http://www.west-wind.com/wwThreads/default.asp?msgid=20D16295H). Thank's Rick!
namespace PublicDomain.AspRuntimeHost
{
    /// <summary>
    /// 
    /// </summary>
    public class wwAspRuntimeHost : IDisposable
    {
        /// <summary>
        /// Location for the generated HTML output.
        /// </summary>
        public string OutputFile = "d:\\temp\\__preview.htm";

        /// <summary>
        /// Hashtable of parameters that can be added to the Host object
        /// </summary>
        public Hashtable Context = new Hashtable();

        /// <summary>
        /// An optional PostBuffer in binary format.
        /// </summary>
        protected byte[] PostData = null;

        /// <summary>
        /// An optional POST buffer Content Type
        /// </summary>
        protected string PostContentType = "application/x-www-form-urlencoded";

        /// <summary>
        /// Name of the directory that AspRunTimeHost class's parent assembly is located in. This is so the DLL/EXE
        /// can be found. Default is blank which uses the current application directory. 
        /// </summary>
        public string ApplicationBase = "";

        /// <summary>
        /// Location of the web.Config file. Defaults to the Application Base path.
        /// </summary>
        public string ConfigFile = "web.config";

        /// <summary>
        /// Name of the Physical Directory assigned with Start(). Required!
        /// </summary>
        public string PhysicalDirectory = "";

        /// <summary>
        /// Name of the virtual directory assigned to the application with Start.Not used internally, only exposed for
        /// external apps to retrieve. 
        /// </summary>
        public string VirtualPath = "/";

        /// <summary>
        /// A hashtable that contains all the HTPP Headers the server sent in header / value pair
        /// </summary>
        public Hashtable ResponseHeaders = null;

        /// <summary>
        /// Send any Request headers - optional. You can pick up response headers and post them right back.
        /// </summary>
        public Hashtable RequestHeaders = null;

        /// <summary>
        /// the Response status code the server sent. 200 on success, 500 on error, 404 for redirect etc.
        /// </summary>
        public int ResponseStatusCode = 200;

        /// <summary>
        /// A comma delimited list of assemblies that should be automatically
        /// copied to the Web applications' BIN directory to avoid having
        /// to manually copy them there.
        /// 
        /// Assign any assemblies that contain types you might be using 
        /// in your parent application and passing to the ASP.NET application
        /// </summary>
        public string ShadowCopyAssemblies = "";

        /// <summary>
        /// Collection of cookies set by the request.
        /// </summary>
        public Hashtable Cookies = new Hashtable();

        // <summary>
        // The code to be used when writing the Response output
        // </summary>
        //public Encoding ResponseEncoding = Encoding.UTF8;

        /// <summary>
        /// An error message if bError is set. Only works for the ProcessRequest method
        /// </summary>
        public string ErrorMessage = "";

        /// <summary>
        /// 
        /// </summary>
        public bool Error = false;


        /// <summary>
        /// Instance of the Proxy object. Exposed to allow access to the ResponseData object.
        /// </summary>
        public wwAspRuntimeProxy Proxy = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="wwAspRuntimeHost"/> class.
        /// </summary>
        public wwAspRuntimeHost()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="wwAspRuntimeHost"/> class.
        /// </summary>
        /// <param name="physicalDirectory">The physical directory.</param>
        /// <param name="virtualPath">The virtual path.</param>
        public wwAspRuntimeHost(string physicalDirectory, string virtualPath)
        {
            PhysicalDirectory = physicalDirectory;
            VirtualPath = virtualPath;

            Start();
        }

        /// <summary>
        /// Processes a page request against the ASP.Net runtime. 
        /// </summary>
        /// <param name="Page">A page filename relative to the Virtual directory. Use subdir\sub2\test.aspx style syntax for subdirs. (note forward slash!)</param>
        /// <param name="QueryString">Optional - query string in key value pair format. Pass null for non.</param>
        /// <returns>true or false. False returns only if a real failure occurs - most page errors will result in an HTTP error page.</returns>
        public virtual bool ProcessRequest(string Page, string QueryString)
        {
            if (!this.PreProcessing())
                return false;

            bool Result = false;
            try
            {
                Result = this.Proxy.ProcessRequest(Page, QueryString);
            }
            catch (Exception ex)
            {
                this.ResponseStatusCode = 500;
                this.ErrorMessage = ex.Message;
                this.Error = true;
                this.ClearRequestData();
                return false;
            }

            this.PostProcessing();

            return Result;
        }


        /// <summary>
        /// Processes a page request against the ASP.Net runtime and runs the result to a string
        /// </summary>
        /// <param name="Page">A page filename relative to the Virtual directory. Use subdir\sub2\test.aspx style syntax for subdirs. (note forward slash!)</param>
        /// <param name="queryStringKeysAndValues">The query string keys and values.</param>
        /// <returns>
        /// true or false. False returns only if a real failure occurs - most page errors will result in an HTTP error page.
        /// </returns>
        public virtual string ProcessRequestToString(string Page, params string[] queryStringKeysAndValues)
        {
            string queryString = "";

            if (queryStringKeysAndValues != null)
            {
                for (int i = 0; i < queryStringKeysAndValues.Length; i += 2)
                {
                    if (queryString.Length > 0)
                    {
                        queryString += "&";
                    }
                    if (queryStringKeysAndValues[i + 1] != null)
                    {
                        queryString += string.Format("{0}={1}", queryStringKeysAndValues[i], HttpUtility.UrlEncode(queryStringKeysAndValues[i + 1]));
                    }
                    else
                    {
                        queryString += string.Format("{0}", queryStringKeysAndValues[i]);
                    }
                }
            }

            if (!this.PreProcessing())
                return "";

            string Result = "";
            try
            {
                Result = this.Proxy.ProcessRequestToString(Page, queryString);
            }
            catch (Exception ex)
            {
                this.ClearRequestData();
                this.ResponseStatusCode = 500;
                this.ErrorMessage = ex.Message;
                this.Error = true;
                return "";
            }

            //this.PostProcessing();

            return Result;
        }

        /// <summary>
        /// Pre-Processing routine common to the Processing methods
        /// </summary>
        /// <returns></returns>
        private bool PreProcessing()
        {
            this.ErrorMessage = "";
            this.Error = false;

            // Use this to check if host has unloaded from proxy
            try
            {
                string Path = this.Proxy.OutputFile;
            }
            catch (Exception)
            {
                // *** Most likely the runtime unloaded on us
                if (!this.Start())
                    return false;
            }

            try
            {
                // *** Pass Parameter info
                this.Proxy.Context = this.Context;

                if (this.Cookies != null)
                    this.AddCookiesToRequest();

                this.Proxy.OutputFile = this.OutputFile;
                this.Proxy.PostData = this.PostData;
                this.Proxy.PostContentType = this.PostContentType;
                this.Proxy.RequestHeaders = this.RequestHeaders;
            }
            catch (Exception ex)
            {
                this.ClearRequestData();
                this.ResponseStatusCode = 500;
                this.ErrorMessage = ex.Message;
                this.Error = true;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Post-Processing code common to both of the processing routines
        /// </summary>
        private void PostProcessing()
        {
            this.ResponseHeaders = this.Proxy.ResponseHeaders;
            this.ResponseStatusCode = this.Proxy.ResponseStatusCode;

            // *** Pick up the server's Cookies and add to internal Cookie Collection
            if (this.Proxy.Cookies != null)
            {
                foreach (string Key in this.Proxy.Cookies.Keys)
                {
                    if (this.Cookies.ContainsKey(Key))
                        this.Cookies[Key] = this.Proxy.Cookies[Key];
                    else
                        this.Cookies.Add(Key, this.Proxy.Cookies[Key]);
                }
            }

            // Copy the Context
            this.Context = this.Proxy.Context;

            this.ClearRequestData();

        }


        /// <summary>
        /// Resets the host so on the next request we start with a clean slate
        /// </summary>
        private void ClearRequestData()
        {
            this.PostData = null;
            this.PostContentType = "application/x-www-form-urlencoded";
            this.RequestHeaders = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Header"></param>
        /// <param name="value"></param>
        public void AddRequestHeader(string Header, string value)
        {
            if (this.RequestHeaders == null)
                this.RequestHeaders = new Hashtable();

            if (!this.RequestHeaders.Contains(Header))
                this.RequestHeaders.Add(Header, value);
        }


        /// <summary>
        /// Adds all the cookies in the Cookie Collection
        /// </summary>
        protected void AddCookiesToRequest()
        {
            // *** Forward any cookies we've picked up previously
            if (this.Cookies != null)
            {
                string TCookies = "";
                foreach (DictionaryEntry Cookie in Cookies)
                    TCookies += (string)Cookie.Value + "; ";

                if (TCookies != "")
                    this.AddRequestHeader("cookie", TCookies);
            }
        }



        /// <summary>
        /// Starts the ASP.Net runtime hosting by creating a new appdomain and loading the runtime into it.
        /// </summary>
        /// <returns>true or false</returns>
        public bool Start()
        {
            if (this.Proxy == null)
            {
                // *** Make sure ASP.Net registry keys exist 
                // *** if IIS was never registered, required aspnet_isapi.dll 
                // *** cannot be found otherwise
                this.GetInstallPathAndConfigureAspNetIfNeeded();

                if (this.VirtualPath.Length == 0 || this.PhysicalDirectory.Length == 0)
                {
                    this.ErrorMessage = "Virtual or Physical Path not set.";
                    this.Error = true;
                    return false;
                }

                // *** Force any assemblies assemblies to be copied
                this.MakeShadowCopies(this.ShadowCopyAssemblies);

                try
                {
                    this.Proxy = wwAspRuntimeProxy.Start(this.PhysicalDirectory, this.VirtualPath,
                        this.ApplicationBase, this.ConfigFile);

                    this.Proxy.PhysicalDirectory = this.PhysicalDirectory;
                    this.Proxy.VirtualPath = this.VirtualPath;

                }
                catch (Exception ex)
                {
                    this.ErrorMessage = ex.Message;
                    this.Error = true;
                    this.Proxy = null;
                    return false;
                }
                this.Cookies.Clear();
            }


            return true;
        }

        /// <summary>
        /// Stops the ASP.Net runtime unloading the AppDomain
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            if (this.Proxy != null)
            {
                try
                {
                    wwAspRuntimeProxy.Stop(this.Proxy);
                    this.Proxy = null;
                }
                catch (Exception ex)
                {
                    this.ErrorMessage = ex.Message;
                    this.Error = true;
                    return false;
                }
                return true;
            }
            return false;
        }



        /// <summary>
        /// Adds a complete POST buffer to the current request.
        /// </summary>
        /// <param name="PostBuffer">raw POST buffer as byte[]</param>
        /// <param name="ContentType">the content type of the buffer.</param>
        public void AddPostBuffer(byte[] PostBuffer, string ContentType)
        {
            if (ContentType != null)
                this.PostContentType = ContentType;

            this.PostData = PostBuffer;
            return;
        }

        /// <summary>
        /// Adds a complete POST buffer to the current request.
        /// </summary>
        /// <param name="PostBuffer">raw POST buffer as a string</param>
        /// <param name="ContentType">the content type of the buffer.</param>
        public void AddPostBuffer(string PostBuffer, string ContentType)
        {
            this.AddPostBuffer(Encoding.GetEncoding(1252).GetBytes(PostBuffer), ContentType);
        }

        /// <summary>
        /// Adds a complete POST buffer to the current request.
        /// </summary>
        /// <param name="PostBuffer">raw POST buffer as byte[]</param>
        public void AddPostBuffer(string PostBuffer)
        {
            this.AddPostBuffer(PostBuffer, "application/x-www-form-urlencoded");
        }


        /// <summary>
        /// Copies any assemblies marked for ShadowCopying into the BIN directory
        /// of the Web physical director. Copies only 
        /// if the assemblies in the source dir is newer
        /// </summary>
        private void MakeShadowCopies(string ShadowCopyAssemblies)
        {
            if (ShadowCopyAssemblies == null ||
                ShadowCopyAssemblies == string.Empty)
                return;

            string[] Assemblies = ShadowCopyAssemblies.Split(';', ',');
            foreach (string Assembly in Assemblies)
            {
                try
                {
                    string TargetFile = PhysicalDirectory + "bin\\" + Path.GetFileName(Assembly);

                    if (File.Exists(TargetFile))
                    {
                        // *** Compare Timestamps
                        DateTime SourceTime = File.GetLastWriteTime(Assembly);
                        DateTime TargetTime = File.GetLastWriteTime(TargetFile);
                        if (SourceTime == TargetTime)
                            continue;
                    }

                    File.Copy(Assembly, TargetFile, true);
                }
                catch { ;  } // nothing we can do on failure 
            }
        }


        /// <summary>
        /// The ASP.NET Runtime requires certain keys configured in the registry.
        /// This code checks for those keys on startup and if not found sets them up
        /// even if ASP.NET is not installed.
        /// 
        /// Taken from the Cassini Source
        /// </summary>
        /// <returns></returns>
        private string GetInstallPathAndConfigureAspNetIfNeeded()
        {
            // If ASP.NET was never registered on this machine, the registry 
            // needs to be patched up for System.Web.dll to find aspnet_isapi.dll
            //
            // If HKLM\Microsoft\ASP.NET key is missing, this will be added
            //      (adjusted for the correct directory and version number
            // [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\ASP.NET]
            //      "RootVer"="1.0.3514.0"
            // [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\ASP.NET\1.0.3514.0]
            //      "Path"="E:\WINDOWS\Microsoft.NET\Framework\v1.0.3514"
            //      "DllFullPath"="E:\WINDOWS\Microsoft.NET\Framework\v1.0.3514\aspnet_isapi.dll"

            const String aspNetKeyName = @"Software\Microsoft\ASP.NET";

            RegistryKey aspNetKey = null;
            RegistryKey aspNetVersionKey = null;
            RegistryKey frameworkKey = null;

            String installPath = null;

            try
            {
                // get the version corresponding to System.Web.Dll currently loaded
                String aspNetVersion = FileVersionInfo.GetVersionInfo(typeof(HttpRuntime).Module.FullyQualifiedName).FileVersion;
                String aspNetVersionKeyName = aspNetKeyName + "\\" + aspNetVersion;

                // non 1.0 names should have 0 QFE in the registry
                if (!aspNetVersion.StartsWith("1.0."))
                    aspNetVersionKeyName = aspNetVersionKeyName.Substring(0, aspNetVersionKeyName.LastIndexOf('.') + 1) + "0";

                // check if the subkey with version number already exists
                aspNetVersionKey = Registry.LocalMachine.OpenSubKey(aspNetVersionKeyName);

                if (aspNetVersionKey != null)
                {
                    // already created -- just get the path
                    installPath = (String)aspNetVersionKey.GetValue("Path");
                }
                else
                {
                    // open/create the ASP.NET key
                    aspNetKey = Registry.LocalMachine.OpenSubKey(aspNetKeyName);
                    if (aspNetKey == null)
                    {
                        aspNetKey = Registry.LocalMachine.CreateSubKey(aspNetKeyName);
                        // add RootVer if creating
                        aspNetKey.SetValue("RootVer", aspNetVersion);
                    }

                    // version dir name is almost version: "1.0.3514.0" -> "v1.0.3514"
                    String versionDirName = "v" + aspNetVersion.Substring(0, aspNetVersion.LastIndexOf('.'));

                    // install directory from "InstallRoot" under ".NETFramework" key
                    frameworkKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NETFramework");
                    String rootDir = (String)frameworkKey.GetValue("InstallRoot");
                    if (rootDir.EndsWith("\\"))
                        rootDir = rootDir.Substring(0, rootDir.Length - 1);

                    // create the version subkey
                    aspNetVersionKey = Registry.LocalMachine.CreateSubKey(aspNetVersionKeyName);

                    // install path
                    installPath = rootDir + "\\" + versionDirName;

                    // set path and dllfullpath
                    aspNetVersionKey.SetValue("Path", installPath);
                    aspNetVersionKey.SetValue("DllFullPath", installPath + "\\aspnet_isapi.dll");
                }
            }
            catch
            {
            }
            finally
            {
                if (aspNetVersionKey != null)
                    aspNetVersionKey.Close();
                if (aspNetKey != null)
                    aspNetKey.Close();
                if (frameworkKey != null)
                    frameworkKey.Close();
            }

            return installPath;
        }


        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class wwAspRuntimeProxy : MarshalByRefObject
    {
        /// <summary>
        /// Location for the generated HTML output.
        /// </summary>
        public string OutputFile = "d:\\temp\\__preview.htm";

        /// <summary>
        /// Context parameters that can be read back in the page from the Context object.
        /// </summary>
        public Hashtable Context = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        public byte[] PostData = null;

        /// <summary>
        /// 
        /// </summary>
        public string PostContentType = "application/x-www-form-urlencoded";

        /// <summary>
        /// Reference to the AppDomain to allow unloading the hosted application.
        /// </summary>
        public AppDomain AppDomain = null;

        /// <summary>
        /// Name of the Physical Directory assigned with Start(). Not used internally, only exposed for
        /// external apps to retrieve.
        /// </summary>
        public string PhysicalDirectory = "";

        /// <summary>
        /// Name of the virtual directory assigned to the application with Start.Not used internally, only exposed for
        /// external apps to retrieve.
        /// </summary>
        public string VirtualPath = "";

        /// <summary>
        /// An error message if bError is set. Only works for the ProcessRequest method
        /// </summary>
        public string ErrorMessage = "";

        /// <summary>
        /// 
        /// </summary>
        public bool Error = false;

        /// <summary>
        /// The timeout for the ASP.Net runtime after which it is automatically unloaded when idle
        /// to release resources. Note this can't be externally set because the lease is set 
        /// during object construction. All you can do is change this property value here statically
        /// </summary>
        public static int IdleTimeoutMinutes = 15;

        /// <summary>
        /// A hashtable that contains all the HTPP Headers the server sent in header / value pair
        /// </summary>
        public Hashtable ResponseHeaders = null;

        /// <summary>
        /// 
        /// </summary>
        public Hashtable RequestHeaders = null;

        /// <summary>
        /// the Response status code the server sent. 200 on success, 500 on error, 404 for redirect etc.
        /// </summary>
        public int ResponseStatusCode = 200;

        /// <summary>
        /// Collection of cookies set by the request.
        /// </summary>
        public Hashtable Cookies = null;

        /// <summary>
        /// Processes script execution on the specified page.
        /// </summary>
        /// <param name="Page">A page filename relative to the Virtual directory. Use subdir\sub2\test.aspx style syntax for subdirs. (note forward slash!)</param>
        /// <param name="QueryString">Optional - query string in key value pair format. Pass null for non.</param>
        /// <returns>true or false. False returns only if a real failure occurs - most page errors will result in an HTTP error page.</returns>
        public virtual bool ProcessRequest(string Page, string QueryString)
        {
            TextWriter Output;

            try
            {
                // *** Note you have to write the right 'codepage'. If you use the default UTF-8
                // *** everything will be double encoded.
                Output = new StreamWriter(this.OutputFile, false, Encoding.GetEncoding(1252));

                // *** Write the UTF-8 prefix
                Output.Write("﻿");
            }
            catch (Exception ex)
            {
                this.Error = true;
                this.ErrorMessage = ex.Message;
                return false;
            }

            // *** Reset the Response settings
            this.ResponseHeaders = null;
            this.Cookies = null;
            this.ResponseStatusCode = 200;

            wwWorkerRequest Request = new wwWorkerRequest(Page, QueryString, Output);
            if (this.Context != null)
                Request.Context = this.Context;

            Request.PostData = this.PostData;
            Request.PostContentType = this.PostContentType;
            Request.RequestHeaders = this.RequestHeaders;
            Request.PhysicalPath = this.PhysicalDirectory;

            try
            {
                HttpRuntime.ProcessRequest(Request);
            }
            catch (Exception ex)
            {
                Output.Close();
                this.ResponseStatusCode = 500;
                this.ErrorMessage = ex.Message;
                this.Error = true;
                return false;
            }

            Output.Close();

            this.ResponseHeaders = Request.ResponseHeaders;
            this.ResponseStatusCode = Request.ResponseStatusCode;


            // *** Capture the Cookies that were set by the server
            this.Cookies = Request.Cookies;

            if (Request.Context != null)
                this.Context = Request.Context;

            return true;
        }

        /// <summary>
        /// Processes a script and returns the result as a string.
        /// </summary>
        /// <param name="Page">Name of the page to return</param>
        /// <param name="QueryString">Optional query string</param>
        /// <returns>script result or null on failure. Script errors are returned as errors in the script result string.</returns>
        public virtual string ProcessRequestToString(string Page, string QueryString)
        {
            StringWriter sw = new StringWriter();
            TextWriter Writer = new System.Web.UI.HtmlTextWriter(sw);

            // *** Reset the Response settings
            this.ResponseHeaders = null;
            this.Cookies = null;
            this.ResponseStatusCode = 200;

            wwWorkerRequest Request = new wwWorkerRequest(Page, QueryString, Writer);
            if (this.Context != null)
                Request.Context = this.Context;

            Request.PostData = this.PostData;
            Request.PostContentType = this.PostContentType;
            Request.RequestHeaders = this.RequestHeaders;
            Request.PhysicalPath = this.PhysicalDirectory;

            try
            {
                HttpRuntime.ProcessRequest(Request);
            }
            catch (Exception ex)
            {
                this.ResponseStatusCode = Request.ResponseStatusCode;
                this.ErrorMessage = ex.Message;
                this.Error = true;
                return null;
            }

            string Result = sw.ToString();
            Writer.Close();

            this.ResponseHeaders = Request.ResponseHeaders;
            this.ResponseStatusCode = Request.ResponseStatusCode;

            this.Cookies = Request.Cookies;
            this.Context = Request.Context;

            return Result;
        }

        /// <summary>
        /// Creates an instance of this class in the ASP.NET AppDomain
        /// </summary>
        /// <param name="hostType">Type of the application to be hosted. Essentially this class.</param>
        /// <param name="virtualDir">Name of the Virtual Directory that hosts this application. Not really used, other than on error messages and ASP Server Variable return values.</param>
        /// <param name="physicalDir">The physical location of the Virtual Directory for the application</param>
        /// <param name="PrivateBinPath">The private bin path.</param>
        /// <param name="ConfigurationFile">Location of the configuration file. Default to web.config in the bin directory.</param>
        /// <returns>
        /// object instance to the wwAspRuntimeProxy class you can call ProcessRequest on. Note this instance returned
        /// is a remoting proxy
        /// </returns>
        public static wwAspRuntimeProxy CreateApplicationHost(Type hostType, string virtualDir, string physicalDir,
                                                               string PrivateBinPath, string ConfigurationFile)
        {
            if (!(physicalDir.EndsWith("\\")))
                physicalDir = physicalDir + "\\";

            // *** Copy this hosting DLL into the /bin directory of the application
            string FileName = Assembly.GetExecutingAssembly().Location;
            try
            {
                if (!Directory.Exists(physicalDir + "bin\\"))
                    Directory.CreateDirectory(physicalDir + "bin\\");

                string JustFname = Path.GetFileName(FileName);
                File.Copy(FileName, physicalDir + "bin\\" + JustFname, true);
            }
            catch { ;}

            wwAspRuntimeProxy Proxy = ApplicationHost.CreateApplicationHost(
                                                                hostType,
                                                                virtualDir,
                                                                physicalDir)
                                                       as wwAspRuntimeProxy;

            if (Proxy != null)
                // *** Grab the AppDomain reference and add the ApplicationBase
                // *** Must call into the Proxy to do this
                Proxy.CaptureAppDomain();


            return Proxy;
        }


        /// <summary>
        /// Internal method that captures the Proxy's AppDomain so we can shut
        /// the ASP.NET runtime down externally.
        /// Also serves as an
        /// </summary>
        internal void CaptureAppDomain()
        {
            this.AppDomain = AppDomain.CurrentDomain;
        }


#if false
		/// <summary>
		/// Creates a minimal Application domain to allow the ASP.Net runtime to be hosted.
		/// </summary>
		/// <param name="hostType">Type of the application to be hosted. Essentially this class.</param>
		/// <param name="virtualDir">Name of the Virtual Directory that hosts this application. Not really used, other than on error messages and ASP Server Variable return values.</param>
		/// <param name="physicalDir">The physical location of the Virtual Directory for the application</param>
		/// <param name="ApplicationBase">Location of the 'bin' directory</param>
		/// <param name="ConfigurationFile">Location of the configuration file. Default to web.config in the bin directory.</param>
		/// <returns>object instance to the wwAspRuntimeProxy class you can call ProcessRequest on.</returns>
		public static wwAspRuntimeProxy CreateApplicationHostX(Type hostType, string virtualDir, string physicalDir, 
		                                                      string ApplicationBase, string ConfigurationFile) 
		{
			if (!(physicalDir.EndsWith("\\")))
				physicalDir = physicalDir + "\\";

			string aspDir = HttpRuntime.AspInstallDirectory;
			string domainId = "ASPHOST_" + DateTime.Now.ToString().GetHashCode().ToString("x");
			string appName = (virtualDir + physicalDir).GetHashCode().ToString("x");
			AppDomainSetup setup = new AppDomainSetup();

			//	setup.ApplicationBase =  physicalDir;
			//	setup.PrivateBinPath = Directory.GetCurrentDirectory();
			setup.ApplicationName = appName;

			setup.ConfigurationFile = ConfigurationFile;   //"web.config";  // not necessary execept for debugging

			/// Assign the application base where this class' assembly is hosted
			/// Otherwise the ApplicationBase is inherited from the current process
			if (ApplicationBase != null && ApplicationBase != "")
				setup.ApplicationBase = ApplicationBase;

            AppDomain Domain = AppDomain.CreateDomain(domainId, GetDefaultDomainIdentity(), setup);
			Domain.SetData(".appDomain", "*");
			Domain.SetData(".appPath", physicalDir);
			Domain.SetData(".appVPath", virtualDir);
			Domain.SetData(".domainId", domainId);
			Domain.SetData(".hostingVirtualPath", virtualDir);
			Domain.SetData(".hostingInstallDir", aspDir);

			ObjectHandle oh = Domain.CreateInstance(hostType.Module.Assembly.FullName, hostType.FullName);
			wwAspRuntimeProxy Host = (wwAspRuntimeProxy) oh.Unwrap();
			
			// *** Save virtual and physical path so we can tell where app runs later
			Host.VirtualPath = virtualDir;
			Host.PhysicalDirectory = physicalDir;

			// *** Save Domain so we can unload later
			Host.AppDomain = Domain;

			return Host;
		}

        private static Evidence GetDefaultDomainIdentity()
        {
            Evidence evidence1 = new Evidence();
            bool flag1 = false;
            IEnumerator enumerator1 = AppDomain.CurrentDomain.Evidence.GetHostEnumerator();
            while (enumerator1.MoveNext())
            {
                if (enumerator1.Current is Zone)
                {
                    flag1 = true;
                }
                evidence1.AddHost(enumerator1.Current);
            }
            enumerator1 = AppDomain.CurrentDomain.Evidence.GetAssemblyEnumerator();
            while (enumerator1.MoveNext())
            {
                evidence1.AddAssembly(enumerator1.Current);
            }
            if (!flag1)
            {
                evidence1.AddHost(new Zone(SecurityZone.MyComputer));
            }
            return evidence1;
        }
#endif


        /// <summary>
        /// Starts the Runtime host by creating an AppDomain and loading the runtime into it
        /// </summary>
        /// <param name="PhysicalPath">The physical disk path for the 'Web' directory where files are executed</param>
        /// <param name="VirtualPath">The name of the virtual path. Typically this will be "/" or the root path.</param>
        /// <param name="PrivateBinPath">The private bin path.</param>
        /// <param name="ConfigFile">The config file.</param>
        /// <returns></returns>
        public static wwAspRuntimeProxy Start(string PhysicalPath, string VirtualPath,
                                              string PrivateBinPath, string ConfigFile)
        {
            wwAspRuntimeProxy Host = wwAspRuntimeProxy.CreateApplicationHost(
            typeof(wwAspRuntimeProxy),
            VirtualPath, PhysicalPath, PrivateBinPath, ConfigFile);

            return Host;
        }

        /// <summary>
        /// Unloads the runtime host by unloading the AppDomain. Use this to free memory if you are compiling lots of pages or recycle the host.
        /// </summary>
        /// <param name="Host">The host.</param>
        public static void Stop(wwAspRuntimeProxy Host)
        {
            if (Host != null)
            {
                Host.Context.Clear();
                Host.Context = null;

                Host.UnloadRuntime();

                AppDomain.Unload(Host.AppDomain);
                Host = null;
            }
        }

        /// <summary>
        /// Method used to shut down the ASP.NET AppDomain from within
        /// the AppDomain. 
        /// </summary>
        internal void UnloadRuntime()
        {
            HttpRuntime.UnloadAppDomain();
        }

        /// <summary>
        /// Overrides the default Lease setting to allow the runtime to not
        /// expire after 5 minutes. 
        /// </summary>
        /// <returns></returns>
        public override Object InitializeLifetimeService()
        {
            // return null; // never expire
            ILease lease = (ILease)base.InitializeLifetimeService();

            // *** Set the initial lease which determines how long the remote ref sticks around
            // *** before .Net automatically releases it. Although our code has the logic to
            // *** to automatically restart it's better to keep it loaded
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromMinutes(wwAspRuntimeProxy.IdleTimeoutMinutes);
                lease.RenewOnCallTime = TimeSpan.FromMinutes(wwAspRuntimeProxy.IdleTimeoutMinutes);
                lease.SponsorshipTimeout = TimeSpan.FromMinutes(5);
            }

            return lease;
        }
    }

    /// <summary>
    /// A subclass of SimpleWorkerRequest that allows to push data to the ASP.Net request
    /// via the Context object.
    /// </summary>
    public class wwWorkerRequest : SimpleWorkerRequest
    {

        /// <summary>
        /// Optional parameter data sent to the ASP.Net page. This value is stored into the 
        /// Context object as Context["Content"]. Only a single parameter can be passed,
        /// but you can pass an object that contains additional properties.
        /// Objects passed must be serializable or inherit from MarshalByRefObject.
        /// </summary>
        public object ParameterData = null;

        /// <summary>
        /// Contains a set of parameters
        /// </summary>
        public Hashtable Context = new Hashtable();

        /// <summary>
        /// Returns optional Response data that is retrieved from the Context object
        /// via the Context["ResultContent"] key.
        /// </summary>
        public object ResponseData = null;

        /// <summary>
        /// Optional PostBuffer that allows sending Postable data to the ASPX page.
        /// </summary>
        public byte[] PostData = null;

        /// <summary>
        /// The content type for the POST operation. Defaults to application/x-www-form-urlencoded.
        /// </summary>
        public string PostContentType = "application/x-www-form-urlencoded";


        /// <summary>
        /// Hashtable that contains the server headers as header/value pairs
        /// </summary>
        public Hashtable ResponseHeaders = new Hashtable();

        /// <summary>
        /// Collection that captures all the cookies in the request
        /// </summary>
        public Hashtable Cookies = null;

        /// <summary>
        /// Pass in a set of request headers as Header / Value pairs
        /// </summary>
        public Hashtable RequestHeaders = null;

        /// <summary>
        /// Numeric Server Response Code
        /// </summary>
        public int ResponseStatusCode;

        /// <summary>
        /// The physical path for this application
        /// </summary>
        public string PhysicalPath = "";


        /// <summary>
        /// Internal property used to keep track of the HTTP Context object.
        /// Used to retrieve the Context.Item["ResultContent"] value
        /// </summary>
        private HttpContext CurrentContext = null;


        /// <summary>
        /// Callback to basic constructor
        /// </summary>
        /// <param name="Page">Name of the page to execute in the Web app. Must be in the VRoot defined for the app with the app host.</param>
        /// <param name="QueryString">Optional QueryString. Pass null if no query string data.</param>
        /// <param name="Output">TextWriter object that receives the output from the request.</param>
        public wwWorkerRequest(string Page, string QueryString, TextWriter Output)
            :
            base(Page, QueryString, Output) { }


        /// <summary>
        /// Returns the UNC-translated path to the currently executing server application.
        /// </summary>
        /// <returns>
        /// The physical path of the current application.
        /// </returns>
        public override string GetAppPathTranslated()
        {
            return this.PhysicalPath;
        }

        /// <summary>
        /// Method that is called just before the ASP.Net page gets executed. Allows
        /// setting of the Context object item collection with arbitrary data. Also saves
        /// the Context object so it can be used later to retrieve any result data.
        /// Inbound: Context.Items["Content"] (Parameter data)
        ///          OR: you can add Context items directly by name and pick them up by name
        /// Outbound: Context.Items["ResultContent"]
        /// </summary>
        /// <param name="callback">callback delegate</param>
        /// <param name="extraData">extraData for system purpose</param>
        public override void SetEndOfSendNotification(EndOfSendNotification callback, object extraData)
        {
            base.SetEndOfSendNotification(callback, extraData);

            this.CurrentContext = extraData as HttpContext;

            if (this.ParameterData != null)
            {
                // *** Use 'as' instead of cast to ensure additional calls don't throw exceptions

                if (this.CurrentContext != null)
                    // *** Add any extra data here to the 
                    this.CurrentContext.Items.Add("Content", this.ParameterData);
            }

            // *** Copy inbound context data
            if (this.Context != null)
            {
                foreach (object Item in this.Context.Keys)
                {
                    this.CurrentContext.Items.Add(Item, this.Context[Item]);
                }
            }

        }


        // *** the following three methods are overridden to provide the
        // *** ability to POST information to the Web Server

        /// <summary>
        /// We must send the Verb so the server knows that it's a POST request.
        /// </summary>
        /// <returns></returns>
        public override String GetHttpVerbName()
        {
            if (this.PostData == null)
                return base.GetHttpVerbName();

            return "POST";
        }

        /// <summary>
        /// We must override this method to send the ContentType to the client
        /// when POSTing so that the request is recognized as a POST.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override string GetKnownRequestHeader(int index)
        {
            if (index == HttpWorkerRequest.HeaderContentLength)
            {
                if (this.PostData != null)
                    return this.PostData.Length.ToString();
            }
            else if (index == HttpWorkerRequest.HeaderContentType)
            {
                if (this.PostData != null)
                    return this.PostContentType;
            }
            else
            {
                // *** if we need to pass headers write them out
                if (this.RequestHeaders != null)
                {
                    string header = HttpWorkerRequest.GetKnownRequestHeaderName(index);
                    if (header != null)
                    {
                        header = header.ToLower();
                        if (this.RequestHeaders.Contains(header))
                            return (string)RequestHeaders[header];
                    }
                }
            }

            return ""; //base.GetKnownRequestHeader(index);
        }

        /// <summary>
        /// Return any POST data if provided
        /// </summary>
        /// <returns></returns>
        public override byte[] GetPreloadedEntityBody()
        {
            if (this.PostData != null)
                return this.PostData;

            return base.GetPreloadedEntityBody();
        }

        /// <summary>
        /// Set the internal status code we can pick up
        /// Pick up ResultContent Content variable 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="statusDescription"></param>
        public override void SendStatus(int statusCode, string statusDescription)
        {
            if (this.CurrentContext != null)
            {
                this.ResponseData = this.CurrentContext.Items["ResultContent"];

            }
            // *** Copy outbound Context
            if (this.CurrentContext.Items.Count > 0)
            {
                this.Context.Clear();
                foreach (object Key in this.CurrentContext.Items.Keys)
                {
                    this.Context.Add(Key, this.CurrentContext.Items[Key]);
                }
            }

            this.ResponseStatusCode = statusCode;
            base.SendStatus(statusCode, statusDescription);
        }

        /// <summary>
        /// Retrieve Response Headers and store in ResponseHeaders() collection
        /// so we can simulate them from the browser.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public override void SendKnownResponseHeader(int index, string value)
        {
            string header = HttpWorkerRequest.GetKnownResponseHeaderName(index).ToLower();
            switch (index)
            {
                case HttpWorkerRequest.HeaderSetCookie:
                    {
                        if (this.Cookies == null)
                            this.Cookies = new Hashtable();

                        string CookieName = value.Substring(0, value.IndexOf("=")).ToLower();
                        if (!Cookies.Contains(CookieName))
                            Cookies.Add(CookieName, value);
                        else
                            Cookies[CookieName] = value;

                        break;
                    }
                default:
                    {
                        try
                        {
                            ResponseHeaders.Add(header, value);
                        }
                        catch
                        {
                            string name = header;
                        }
                        break;
                    }
            }

            base.SendKnownResponseHeader(index, value);
        }

        /// <summary>
        /// Store custom headers to ResponseHeaders Hashtable collection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public override void SendUnknownResponseHeader(string name, string value)
        {
            ResponseHeaders.Add(name, value);

            base.SendUnknownResponseHeader(name, value);
        }
    }
}
#endif

#if !(NOCLSCOMPLIANTWARNINGSOFF)
#pragma warning restore 3001
#pragma warning restore 3002
#pragma warning restore 3003
#pragma warning restore 3006
#pragma warning restore 3009
#endif

#endregion // Meat
