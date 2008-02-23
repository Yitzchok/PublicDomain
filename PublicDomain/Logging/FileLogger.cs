using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class FileLogger : Logger
    {
        /// <summary>
        /// 
        /// </summary>
        public static bool UseCarriageReturn = true;

        /// <summary>
        /// 
        /// </summary>
        public static Encoding Encoding = Encoding.Default;

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
        /// Writes the internal.
        /// </summary>
        /// <param name="artifacts">The artifacts.</param>
        protected virtual void WriteInternal(params LogArtifact[] artifacts)
        {
            LogArtifact first = artifacts[0];
            string fileName = GetFileName(first.Severity, first.Timestamp, first.RawEntry, first.RawFormatParameters, first.FormattedMessage, artifacts);

            if (!string.IsNullOrEmpty(fileName))
            {
                using (FileStream stream = GetStream(fileName))
                {
                    foreach (LogArtifact artifact in artifacts)
                    {
                        byte[] data = Encoding.GetBytes(artifact.FormattedMessage);

                        stream.Write(data, 0, data.Length);
                        if (UseCarriageReturn)
                        {
                            stream.WriteByte((byte)'\r');
                        }
                        stream.WriteByte((byte)'\n');
                    }
                    stream.Flush();
                }
            }
        }

        /// <summary>
        /// Writes the specified artifact.
        /// </summary>
        /// <param name="artifact">The artifact.</param>
        public override void Write(LogArtifact artifact)
        {
            WriteInternal(artifact);
        }

        /// <summary>
        /// Writes the specified artifacts.
        /// </summary>
        /// <param name="artifacts">The artifacts.</param>
        public override void Write(LogArtifact[] artifacts)
        {
            WriteInternal(artifacts);
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
        /// Gets the name of the file.
        /// </summary>
        /// <returns></returns>
        public virtual string GetFileName(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine, LogArtifact[] artifactSet)
        {
            return FileName;
        }
    }
}
