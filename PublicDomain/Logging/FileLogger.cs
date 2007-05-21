using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PublicDomain.Logging
{
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
}
