using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
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
}
