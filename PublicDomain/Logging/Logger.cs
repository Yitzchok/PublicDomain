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
        private LoggerSeverity m_threshold = LoggerSeverity.Warn30;

        private List<ILogFilter> m_filters = new List<ILogFilter>();

        private ILogFormatter m_formatter = new DefaultLogFormatter();

        private string m_category;

        private Dictionary<string, object> m_data = new Dictionary<string, object>();

        private static Dictionary<int, int> m_stack = new Dictionary<int, int>();

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
            // Get the current timestamp
            DateTime timestamp = DateTime.UtcNow;

            // Check all the filters
            if (Filters != null)
            {
                foreach (ILogFilter filter in Filters)
                {
                    if (!filter.IsLoggable(Threshold, severity, timestamp, entry, formatParameters))
                    {
                        return;
                    }
                }
            }

            string logLine = null;

            if (Formatter == null)
            {
                if (entry != null)
                {
                    logLine = entry.ToString();
                }
            }
            else
            {
                logLine = Formatter.FormatEntry(severity, timestamp, entry, formatParameters, Category, Data);
            }

            DoLog(severity, timestamp, entry, formatParameters, logLine);
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
            DoLog(logLine);
        }

        /// <summary>
        /// Called by the detailed version, forgetting about the details
        /// and simply having the final log line.
        /// </summary>
        /// <param name="logLine">The log line.</param>
        protected abstract void DoLog(string logLine);

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
            Log(severity, ex);
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        [Conditional("DEBUG")]
        public virtual void Start(params object[] args)
        {
            LogEntryExit(true, true, args);
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        [Conditional("DEBUG")]
        public virtual void End(params object[] args)
        {
            LogEntryExit(false, true, args);
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        [Conditional("DEBUG")]
        public virtual void WhereAmI(params object[] args)
        {
            LogEntryExit(false, false, args);
        }

        /// <summary>
        /// Logs the entry exit.
        /// </summary>
        /// <param name="isEntry">if set to <c>true</c> [is entry].</param>
        /// <param name="useMarker">if set to <c>true</c> [use marker].</param>
        /// <param name="args">The args.</param>
        protected virtual void LogEntryExit(bool isEntry, bool useMarker, object[] args)
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
    }
}
