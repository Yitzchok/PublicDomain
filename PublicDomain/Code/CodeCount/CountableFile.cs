using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PublicDomain.Code.CodeCount
{
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
}
