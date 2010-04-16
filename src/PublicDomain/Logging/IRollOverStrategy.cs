using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
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
        /// <param name="artifactSet">The artifact set.</param>
        /// <returns></returns>
        string GetFileName(string fileName, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine, LogArtifact[] artifactSet);
    }
}
