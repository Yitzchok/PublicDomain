using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PublicDomain.Code.CodeCount
{
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
}
