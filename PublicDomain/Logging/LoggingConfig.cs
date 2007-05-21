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
        public const string AllLoggersDesignator = "all";

        /// <summary>
        /// 
        /// </summary>
        public static bool Enabled = true;

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
        /// <param name="createLogger">The create logger.</param>
        public LoggingConfig(string configString, CallbackCreateLogger createLogger)
        {
            Load(configString, createLogger);
        }

        /// <summary>
        /// Loads the specified config string.
        /// </summary>
        /// <param name="configString">The config string.</param>
        /// <param name="createLogger">The create logger.</param>
        public void Load(string configString, CallbackCreateLogger createLogger)
        {
            if (configString != null && Enabled)
            {
                string[] pieces = configString.Trim().Split(';');
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

                        if (key == "*")
                        {
                            key = AllLoggersDesignator;
                        }

                        string val = parts.Length == 1 ? "*" : parts[1];
                        LoggerSeverity threshold = GetLogValue(val);
                        if (threshold == LoggerSeverity.Infinity)
                        {
                            m_loggers[key] = NullLogger.Current;
                        }
                        else
                        {
                            m_loggers[key] = createLogger(parts[0].Trim(), threshold);
                        }
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Check format of log string ({0}).", piece));
                    }
                }
            }
        }

        private static LoggerSeverity GetDefaultLogThreshold()
        {
            return LoggerSeverity.None0;
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

            if (val == "" || val == "*" || val == "on" || val == "1" || val == AllLoggersDesignator)
            {
                return GetDefaultLogThreshold();
            }
            else if (val == "off" || val == "0")
            {
                return GetOffValue();
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
            throw new ArgumentException(string.Format("Unknown logging value {0}", logValue));
        }

        private LoggerSeverity GetOffValue()
        {
            return LoggerSeverity.Infinity;
        }

        /// <summary>
        /// Gets the <see cref="PublicDomain.Logging.Logger"/> with the specified log class.
        /// </summary>
        /// <param name="logClasses">The log classes.</param>
        /// <returns></returns>
        /// <value></value>
        public Logger CreateLogger(params string[] logClasses)
        {
            Logger result = NullLogger.Current;

            Logger test;
            string testClass;

            foreach (string logClass in logClasses)
            {
                testClass = logClass.ToLower().Trim();
                if (m_loggers.TryGetValue(testClass, out test))
                {
                    result = test;
                }
            }

            if (object.ReferenceEquals(result, NullLogger.Current) && m_loggers.TryGetValue(AllLoggersDesignator, out test))
            {
                result = test;
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
