using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PublicDomain.Logging
{
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
        /// Writes the specified artifact.
        /// </summary>
        /// <param name="artifact">The artifact.</param>
        public override void Write(LogArtifact artifact)
        {
            m_writer.WriteLine(artifact.FormattedMessage);
        }
    }
}
