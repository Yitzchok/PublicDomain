using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// Provides a common application logger, which writes to a rolling
    /// log file in the application's working directory. The logger
    /// always logs severe log events using the <see cref="PublicDomain.Logging.SevereLogFilter"/>,
    /// and by default, uses the default Logger <see cref="PublicDomain.Logging.Logger.Threshold"/> value of Warn.
    /// </summary>
    public class ApplicationLogger : CompositeLogger
    {
        /// <summary>
        /// Static logger provides a common application logger, which writes to a rolling
        /// log file in the application's working directory. The logger
        /// always logs severe log events using the <see cref="PublicDomain.Logging.SevereLogFilter"/>,
        /// and by default, uses the default Logger <see cref="PublicDomain.Logging.Logger.Threshold"/> value of Warn.
        /// </summary>
        public static ApplicationLogger Current = new ApplicationLogger();

        /// <summary>
        /// Provides a common application logger, which writes to a rolling
        /// log file in the application's working directory. The logger
        /// always logs severe log events using the <see cref="PublicDomain.Logging.SevereLogFilter"/>,
        /// and by default, uses the default Logger <see cref="PublicDomain.Logging.Logger.Threshold"/> value.
        /// Initializes a new instance of the <see cref="ApplicationLogger"/> class.
        /// </summary>
        public ApplicationLogger()
        {
            // Figure out where we'll be logging the files
            string fileNameFormatted = FileSystemUtilities.PathCombine(Environment.CurrentDirectory, @"\app{0}.log");
            Loggers.Add(new RollingFileLogger(fileNameFormatted));
            AddLogFilter(new SevereLogFilter());

            // So that we know where the log is going
            string msg = string.Format("Application logging to {0}", fileNameFormatted);
            Console.WriteLine(msg);

#if DEBUG
            // Sometimes the Console does not go anywhere logical (or nowhere at all),
            // so it becomes difficult to know where the current directory is. Therefore,
            // we write the same message to a global file
            try
            {
                FileSystemUtilities.EnsureDirectoriesInPath(GlobalConstants.PublicDomainDefaultInstallLocation);
                Logger loggers = new FileLogger(GlobalConstants.PublicDomainDefaultInstallLocation + @"loggers.log");
                loggers.Threshold = LoggerSeverity.Info20;
                loggers.LogInfo20(msg);
            }
            catch (Exception)
            {
                // No permissions to write to the directory, and we don't bother trying anywhere else
            }
#endif
        }
    }
}
