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
            LogArtifact artifact = new LogArtifact(this, severity, timestamp, entry, formatParameters, logLine);
            Logger.PushArtifact(artifact);
        }

        /// <summary>
        /// Writes the specified artifact.
        /// </summary>
        /// <param name="artifact">The artifact.</param>
        public override void Write(LogArtifact artifact)
        {
            string fileName = GetFileName(artifact.Severity, artifact.Timestamp, artifact.Entry, artifact.FormatParameters, artifact.LogLine);

            if (!string.IsNullOrEmpty(fileName))
            {
                using (FileStream stream = GetStream(fileName))
                {
                    byte[] data = Encoding.Default.GetBytes(artifact.LogLine);

                    stream.Write(data, 0, data.Length);
                    stream.WriteByte((byte)'\n');
                    stream.Flush();
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
            return new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
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
        public virtual string GetFileName(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
            return FileName;
        }
    }
}
