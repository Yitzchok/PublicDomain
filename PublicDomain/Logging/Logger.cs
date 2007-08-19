using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

namespace PublicDomain.Logging
{
    /// <summary>
    /// There is no interface for this class to allow for certain methods
    /// to be overriden and removed in debug builds.
    /// </summary>
    public abstract class Logger
    {
        /// <summary>
        /// In milliseconds
        /// </summary>
        private static int BackgroundThreadInterval = 1000 * 5;
        private static Dictionary<int, int> m_stack = new Dictionary<int, int>();
        private static FinalizableBackgroundThread m_loggerThread = new LoggerBackgroundThread(BackgroundThreadInterval, LoggerThread);
        private static List<LogArtifact> s_artifacts = new List<LogArtifact>();
        private static ILogTimestampProvider s_defaultTimestampProvider = new LocalLogTimestampProvider();

        private LoggerSeverity m_threshold = LoggerSeverity.Warn30;
        private List<ILogFilter> m_filters = new List<ILogFilter>();
        private ILogFormatter m_formatter = new DefaultLogFormatter();
        private ILogTimestampProvider m_timestampProvider = s_defaultTimestampProvider;
        private string m_category;
        private Dictionary<string, object> m_data = new Dictionary<string, object>();

        /// <summary>
        /// Actually does the processing of the background thread to call the writes
        /// </summary>
        /// <param name="isFinal">if set to <c>true</c> [is final].</param>
        private static void LoggerThread(bool isFinal)
        {
            int length = s_artifacts.Count;

            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    LogArtifact artifact = s_artifacts[i];
                    try
                    {
                        artifact.Logger.Write(artifact);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            CriticalLogger.Current.LogException(ex);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                s_artifacts.RemoveRange(0, length);
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
        /// Pushes the artifact.
        /// </summary>
        /// <param name="artifact">The artifact.</param>
        public static void PushArtifact(LogArtifact artifact)
        {
            s_artifacts.Add(artifact);
        }

        /// <summary>
        /// Can be used as a log guard
        /// </summary>
        public bool Enabled = true;

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
        /// Gets or sets the timestamp provider.
        /// </summary>
        /// <value>The timestamp provider.</value>
        public virtual ILogTimestampProvider TimestampProvider
        {
            get
            {
                return m_timestampProvider;
            }
            set
            {
                m_timestampProvider = value;
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
            try
            {
                // Get the current timestamp
                DateTime timestamp = m_timestampProvider.Now;
                TimeSpan? utcOffset = m_timestampProvider.UtcOffset;

                // Check all the filters
                if (m_filters != null)
                {
                    foreach (ILogFilter filter in m_filters)
                    {
                        if (!filter.IsLoggable(m_threshold, severity, timestamp, entry, formatParameters))
                        {
                            return;
                        }
                    }
                }

                string logLine = null;

                if (m_formatter == null)
                {
                    if (entry != null)
                    {
                        logLine = entry.ToString();
                    }
                }
                else
                {
                    logLine = Formatter.FormatEntry(severity, timestamp, utcOffset, entry, formatParameters, m_category, m_data);
                }

                DoLog(severity, timestamp, entry, formatParameters, logLine);
            }
#if DEBUG
            catch (Exception ex)
            {
                Console.WriteLine(ExceptionUtilities.GetHumanReadableExceptionDetailsAsString(ex));
            }
#else
            catch (Exception)
            {
            }
#endif
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
            LogArtifact artifact = new LogArtifact(this, severity, timestamp, entry, formatParameters, logLine);
            Logger.PushArtifact(artifact);
        }

        /// <summary>
        /// Writes the specified artifact.
        /// </summary>
        /// <param name="artifact">The artifact.</param>
        public abstract void Write(LogArtifact artifact);

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
            Log(severity, ExceptionUtilities.GetExceptionDetailsAsString(ex));
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        [Conditional("DEBUG")]
        public virtual void DebugDumpEntry(params object[] args)
        {
            DebugLogEntryExit(true, true, args);
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        [Conditional("DEBUG")]
        public virtual void DebugDumpExit(params object[] args)
        {
            DebugLogEntryExit(false, true, args);
        }

        /// <summary>
        /// Dumps the entry.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="args">The args.</param>
        public virtual void Entry(string methodName, params object[] args)
        {
            LogEntryExit(methodName, true, args);
        }

        /// <summary>
        /// Logs the entry exit.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="isEntry">if set to <c>true</c> [is entry].</param>
        /// <param name="args">The args.</param>
        protected virtual void LogEntryExit(string methodName, bool isEntry, params object[] args)
        {
            StringBuilder sb = new StringBuilder(32);
            if (isEntry)
            {
                sb.Append("> ");
            }
            else
            {
                sb.Append("< ");
            }
            sb.Append(methodName);
            if (args.Length > 0)
            {
                sb.Append(": ");
                BuildArgList(sb, args);
            }
            LogDebug10(sb.ToString());
        }

        /// <summary>
        /// Exits
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="args">The args.</param>
        public virtual void Exit(string methodName, params object[] args)
        {
            LogEntryExit(methodName, false, args);
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        public virtual void DumpStack(params object[] args)
        {
            DebugLogEntryExit(false, false, args);
        }

        /// <summary>
        /// Logs the entry exit.
        /// </summary>
        /// <param name="isEntry">if set to <c>true</c> [is entry].</param>
        /// <param name="useMarker">if set to <c>true</c> [use marker].</param>
        /// <param name="args">The args.</param>
        protected virtual void DebugLogEntryExit(bool isEntry, bool useMarker, object[] args)
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

            BuildArgList(sb, args);

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

        private static void BuildArgList(StringBuilder sb, object[] args)
        {
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
        }
    }
}
