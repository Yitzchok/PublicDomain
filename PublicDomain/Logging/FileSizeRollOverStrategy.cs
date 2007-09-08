using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PublicDomain.Logging
{
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
            : this(DefaultFileSizeStrategyBytes)
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
                    int foundNumber = StringUtilities.ExtractFirstNumber(Path.GetFileName(foundFile));
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
                FileInfo info = new FileInfo(checkFileName);
                if (info.Exists && info.Length >= MaxFileSize)
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
}
