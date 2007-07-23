using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class LoggingConfig
    {
        /// <summary>
        /// Returned for off
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

        private bool m_fallbackToNullLogger;

        private CallbackCreateLogger m_createLogger;
        private CallbackUpdateLogger m_updateLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingConfig"/> class.
        /// </summary>
        public LoggingConfig()
        {
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
        }

        /// <summary>
        /// Gets or sets a value indicating whether [fallback to null logger].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [fallback to null logger]; otherwise, <c>false</c>.
        /// </value>
        public bool FallbackToNullLogger
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
        /// Loads the specified config string.
        /// </summary>
        /// <example>Namespace1.Class1=*;Class2=off;Namespace1.Namespace2.Class3=Debug</example>
        /// <param name="configString">The config string.</param>
        public void Load(string configString)
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
        public void Load(string configString, CallbackCreateLogger createLogger, CallbackUpdateLogger updateLogger)
        {
            if (createLogger == null)
            {
                createLogger = DefaultCallbackCreateLogger;
            }
            if (updateLogger == null)
            {
                updateLogger = DefaultCallbackUpdateLogger;
            }

            m_createLogger = createLogger;
            m_updateLogger = updateLogger;

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
        public Logger DefaultCallbackCreateLogger(string className, LoggerSeverity threshold)
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
        public void DefaultCallbackUpdateLogger(Logger logger, LoggerSeverity threshold)
        {
            logger.Threshold = threshold;
        }

        private void PostProcessNewLogger(string key, LoggerSeverity threshold, Logger logger)
        {
            if (logger == null)
            {
                throw new InvalidOperationException("Create logger delegate returned a null logger");
            }

            PrepareLogger(threshold, logger);
            m_loggers[key] = logger;
        }

        private void PrepareLogger(LoggerSeverity threshold, Logger logger)
        {
            if (threshold == OffValue)
            {
                logger.Enabled = false;
            }
        }

        /// <summary>
        /// Gets the log value.
        /// </summary>
        /// <param name="logValue">The log value.</param>
        /// <returns></returns>
        private LoggerSeverity GetLogValue(string logValue)
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
        public Logger CreateLogger(params string[] logClasses)
        {
            Logger result = null;

            Logger test;
            string testClass;

            // First try the specific classes
            foreach (string logClass in logClasses)
            {
                testClass = logClass.ToLower().Trim();
                if (m_loggers.TryGetValue(testClass, out test))
                {
                    result = test;
                    break;
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
                    if (logClasses.Length == 0)
                    {
                        throw new ArgumentNullException("Could not create fallback logger because no logClasses were specified");
                    }

                    // A fallback logger is always turned off
                    LoggerSeverity fallbackThreshold = OffValue;
                    string key = logClasses[0];

                    result = m_createLogger(key, fallbackThreshold);

                    PostProcessNewLogger(key, fallbackThreshold, result);
                }
            }

            return result;
        }

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="otherLogClasses">The other log classes.</param>
        /// <returns></returns>
        public Logger CreateLogger(Type type, params string[] otherLogClasses)
        {
            string[] classes = new string[otherLogClasses.Length + 1];
            Array.Copy(otherLogClasses, classes, otherLogClasses.Length);
            classes[classes.Length - 1] = type.ToString();
            return CreateLogger(classes);
        }
    }
}