using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class LoggingConfig
    {
        /// <summary>
        /// Returned for off (value is Infinity)
        /// </summary>
        public const LoggerSeverity OffValue = LoggerSeverity.Infinity;

        /// <summary>
        /// Returned for *
        /// </summary>
        public const LoggerSeverity DefaultLogThreshold = LoggerSeverity.None0;

        private Dictionary<string, Logger> m_loggers = new Dictionary<string, Logger>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public delegate Logger CallbackCreateLogger(string className, LoggerSeverity threshold);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="threshold"></param>
        public delegate void CallbackUpdateLogger(Logger logger, LoggerSeverity threshold);

        /// <summary>
        /// 
        /// </summary>
        public const string AllLoggersDesignator = "all";

        /// <summary>
        /// 
        /// </summary>
        public const string AllLoggersDesignatorSplat = "*";

        /// <summary>
        /// 
        /// </summary>
        public static bool Enabled = true;

        private static LoggingConfig s_current = new LoggingConfig();

        private bool m_fallbackToNullLogger;
        private CallbackCreateLogger m_createLogger;
        private CallbackUpdateLogger m_updateLogger;
        private string m_value;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingConfig"/> class.
        /// </summary>
        public LoggingConfig()
        {
            InitializeCurrent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingConfig"/> class.
        /// </summary>
        /// <param name="configString">The config string.</param>
        public LoggingConfig(string configString)
        {
            Load(configString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingConfig"/> class.
        /// </summary>
        /// <param name="configString">The config string.</param>
        /// <param name="createLogger">The create logger.</param>
        /// <param name="updateLogger">The update logger.</param>
        public LoggingConfig(string configString, CallbackCreateLogger createLogger, CallbackUpdateLogger updateLogger)
        {
            Load(configString, createLogger, updateLogger);
            InitializeCurrent();
        }

        /// <summary>
        /// Gets the global LoggingConfig instance. The constructor
        /// of LoggingConfig sets the new instance to the global Current
        /// when called, so unless otherwise set, the Current value
        /// is the last instantiated instance of LoggingConfig or a default
        /// LogginConfig.
        /// </summary>
        /// <value>The current.</value>
        public static LoggingConfig Current
        {
            get
            {
                return s_current;
            }
            set
            {
                s_current = value;
            }
        }

        /// <summary>
        /// Initializes the current.
        /// </summary>
        protected virtual void InitializeCurrent()
        {
            s_current = this;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [fallback to null logger].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [fallback to null logger]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool FallbackToNullLogger
        {
            get
            {
                return m_fallbackToNullLogger;
            }
            set
            {
                m_fallbackToNullLogger = value;
            }
        }

        /// <summary>
        /// Gets the configuration string value
        /// previously loaded.
        /// </summary>
        /// <value>The value.</value>
        public virtual string Value
        {
            get
            {
                return m_value;
            }
        }

        /// <summary>
        /// Loads the specified config string.
        /// </summary>
        /// <example>Namespace1.Class1=*;Class2=off;Namespace1.Namespace2.Class3=Debug</example>
        /// <param name="configString">The config string.</param>
        public virtual void Load(string configString)
        {
            Load(configString, null, null);
        }

        /// <summary>
        /// Loads the specified config string.
        /// </summary>
        /// <example>Namespace1.Class1=*;Class2=off;Namespace1.Namespace2.Class3=Debug</example>
        /// <param name="configString">The config string.</param>
        /// <param name="createLogger">The create logger.</param>
        /// <param name="updateLogger">The update logger.</param>
        public virtual void Load(string configString, CallbackCreateLogger createLogger, CallbackUpdateLogger updateLogger)
        {
            m_value = configString;

            if (createLogger == null && m_createLogger == null)
            {
                createLogger = DefaultCallbackCreateLogger;
            }
            if (updateLogger == null && m_updateLogger == null)
            {
                updateLogger = DefaultCallbackUpdateLogger;
            }

            if (m_createLogger == null)
            {
                m_createLogger = createLogger;
            }

            if (m_updateLogger == null)
            {
                m_updateLogger = updateLogger;
            }

            DisableAllLoggers();

            if (configString != null && Enabled)
            {
                string[] pieces = configString.Trim().Split(';', ',');
                if (pieces.Length == 1 && pieces[0] == "")
                {
                    return;
                }
                foreach (string piece in pieces)
                {
                    string[] parts = piece.Trim().Split('=');
                    if (parts != null && parts.Length > 0 && parts.Length <= 2 && parts[0].Trim().Length > 0)
                    {
                        string key = parts[0].ToLower().Trim();

                        if (key == AllLoggersDesignatorSplat)
                        {
                            key = AllLoggersDesignator;
                        }

                        string val = parts.Length == 1 ? AllLoggersDesignatorSplat : parts[1];
                        LoggerSeverity threshold = GetLogValue(val);
                        Logger logger;

                        if (key == AllLoggersDesignator)
                        {
                            // Updating all loggers
                            foreach (Logger logval in m_loggers.Values)
                            {
                                m_updateLogger(logval, threshold);
                                PrepareLogger(threshold, logval);
                            }
                        }

                        // See if the logger already exists
                        if (m_loggers.TryGetValue(key, out logger))
                        {
                            // Already exists
                            m_updateLogger(logger, threshold);

                            PrepareLogger(threshold, logger);
                        }
                        else
                        {
                            logger = m_createLogger(key, threshold);

                            PostProcessNewLogger(key, threshold, logger);
                        }
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Check format of log string ({0}).", piece));
                    }
                }
            }
        }

        /// <summary>
        /// Defaults the callback create logger.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="threshold">The threshold.</param>
        /// <returns></returns>
        public virtual Logger DefaultCallbackCreateLogger(string className, LoggerSeverity threshold)
        {
            Logger result = new CompositeLogger(ApplicationLogger.Current);
            result.Threshold = threshold;
            result.Category = className;
            return result;
        }

        /// <summary>
        /// Defaults the callback update logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="threshold">The threshold.</param>
        public virtual void DefaultCallbackUpdateLogger(Logger logger, LoggerSeverity threshold)
        {
            logger.Threshold = threshold;
        }

        /// <summary>
        /// Posts the process new logger.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="threshold">The threshold.</param>
        /// <param name="logger">The logger.</param>
        protected virtual void PostProcessNewLogger(string key, LoggerSeverity threshold, Logger logger)
        {
            if (logger == null)
            {
                throw new InvalidOperationException("Create logger delegate returned a null logger");
            }

            PrepareLogger(threshold, logger);
            m_loggers[key] = logger;
        }

        /// <summary>
        /// Prepares the logger.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <param name="logger">The logger.</param>
        protected virtual void PrepareLogger(LoggerSeverity threshold, Logger logger)
        {
            logger.Enabled = threshold != OffValue;
        }

        /// <summary>
        /// Gets the log value.
        /// </summary>
        /// <param name="logValue">The log value.</param>
        /// <returns></returns>
        protected virtual LoggerSeverity GetLogValue(string logValue)
        {
            if (logValue == null)
            {
                throw new ArgumentNullException("logValue");
            }
            string val = logValue.ToLower().Trim();

            if (val == "" || val == AllLoggersDesignatorSplat || val == "on" || val == "1" || val == AllLoggersDesignator)
            {
                return DefaultLogThreshold;
            }
            else if (val == "off" || val == "0")
            {
                return OffValue;
            }
            string[] names = Enum.GetNames(typeof(LoggerSeverity));
            foreach (string name in names)
            {
                if (name.ToLower().StartsWith(val))
                {
                    return (LoggerSeverity)Enum.Parse(typeof(LoggerSeverity), name);
                }
            }

            // If we can't figure out the value, throw an exception
            throw new ArgumentException(string.Format("Unknown logging value {0}. Must be one of: off, *, or a threshold value such as {1}", logValue, LoggerSeverity.Debug10.ToString()));
        }

        /// <summary>
        /// Gets the <see cref="PublicDomain.Logging.Logger"/> with the specified log class.
        /// </summary>
        /// <param name="logClasses">The log classes.</param>
        /// <returns></returns>
        /// <value></value>
        public virtual Logger CreateLogger(params string[] logClasses)
        {
            Logger result = null;
            Logger test;

            // First try the specific classes
            if (logClasses != null)
            {
                string logClass;
                for (int i = 0; i < logClasses.Length; i++)
                {
                    logClass = logClasses[i].ToLower().Trim();
                    if (m_loggers.TryGetValue(logClass, out test))
                    {
                        result = test;
                        break;
                    }
                }
            }

            // Lastly try the all designator
            if (result == null && m_loggers.TryGetValue(AllLoggersDesignator, out test))
            {
                result = test;
            }

            if (result == null)
            {
                if (FallbackToNullLogger)
                {
                    result = NullLogger.Current;
                }
                else
                {
                    if (logClasses == null || logClasses.Length == 0)
                    {
                        throw new ArgumentNullException("Could not create fallback logger because no logClasses were specified");
                    }

                    // A fallback logger is always turned off
                    LoggerSeverity fallbackThreshold = OffValue;
                    string key = logClasses[0];

                    if (m_createLogger == null)
                    {
                        m_createLogger = DefaultCallbackCreateLogger;
                    }
                    result = m_createLogger(key, fallbackThreshold);

                    key = key.ToLower();

                    PostProcessNewLogger(key, fallbackThreshold, result);

                    UpdatePeerLoggers(logClasses, result, key);
                }
            }

            return result;
        }

        private void UpdatePeerLoggers(string[] logClasses, Logger result, string key)
        {
            for (int i = 1; i < logClasses.Length; i++)
            {
                string smallKey = logClasses[i].ToLower();

                if (smallKey != key)
                {
                    m_loggers[smallKey] = result;
                }
            }
        }

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="threshold">The threshold.</param>
        /// <returns></returns>
        public virtual Logger CreateLogger(Type type, LoggerSeverity threshold)
        {
            return CreateLogger(type.FullName, threshold);
        }

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="threshold">The threshold.</param>
        /// <returns></returns>
        public virtual Logger CreateLogger(string className, LoggerSeverity threshold)
        {
            Logger logger = CreateLogger(className);
            if (logger != null)
            {
                logger.Threshold = threshold;
            }
            return logger;
        }

        /// <summary>
        /// Gets the working logger.
        /// </summary>
        /// <returns></returns>
        public virtual Logger GetWorkingLogger()
        {
            return GetWorkingLogger(LoggerSeverity.None0);
        }

        /// <summary>
        /// Gets the working logger.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <returns></returns>
        public virtual Logger GetWorkingLogger(LoggerSeverity threshold)
        {
            return CreateLogger(typeof(Logger), threshold);
        }

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="otherLogClasses">The other log classes.</param>
        /// <returns></returns>
        public virtual Logger CreateLogger(Type type, params string[] otherLogClasses)
        {
            int ol = otherLogClasses == null ? 0 : otherLogClasses.Length;
            string[] classes = new string[ol + 1];
            classes[0] = type.ToString();
            Array.Copy(otherLogClasses, 0, classes, 1, ol);
            return CreateLogger(classes);
        }

        /// <summary>
        /// Enables all loggers.
        /// </summary>
        public virtual void EnableAllLoggers()
        {
            UpdateAllLoggers(true);
        }

        /// <summary>
        /// Disables all loggers.
        /// </summary>
        public virtual void DisableAllLoggers()
        {
            UpdateAllLoggers(false);
        }

        /// <summary>
        /// Updates all loggers.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        protected virtual void UpdateAllLoggers(bool enabled)
        {
            foreach (Logger logger in m_loggers.Values)
            {
                logger.Enabled = enabled;
            }
        }
    }
}
