using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PublicDomain.Logging;
using System.IO;

namespace PublicDomain
{
    [TestFixture]
    public class LoggerTests
    {
        [Test]
        public void play()
        {
        }

        [Test]
        public void TestConfig()
        {
            string configString = "test=*,test2=off;test3=debug";

            LoggingConfig config = new LoggingConfig(configString);

            TestConfigHelper(config);

            Logger logger = config.CreateLogger("test4");
            Assert.AreEqual((int)logger.Threshold, (int)LoggingConfig.OffValue);

            config.Load(configString + ";test4=info");

            TestConfigHelper(config);

            logger = config.CreateLogger("test4");
            Assert.AreEqual((int)logger.Threshold, (int)LoggerSeverity.Info20);

            // Finally, try updating an existing logger
            config.Load(configString.Replace("test=*", "test=off"));

            logger = config.CreateLogger("test");
            Assert.AreEqual((int)logger.Threshold, (int)LoggingConfig.OffValue);
        }

        private static void TestConfigHelper(LoggingConfig config)
        {
            Logger logger = config.CreateLogger("test");
            Assert.AreEqual((int)logger.Threshold, (int)LoggingConfig.DefaultLogThreshold);

            logger = config.CreateLogger("test2");
            Assert.AreEqual((int)logger.Threshold, (int)LoggingConfig.OffValue);

            logger = config.CreateLogger("test3");
            Assert.AreEqual((int)logger.Threshold, (int)LoggerSeverity.Debug10);
        }

        [Test]
        public void SimpleApplicationLogging()
        {
            string line = "test" + RandomGenerationUtilities.GetRandomInteger();
            ApplicationLogger.Current.Threshold = LoggerSeverity.Debug10;
            ApplicationLogger.Current.Log(LoggerSeverity.Debug10, line);

            // Find the file logger
            FileLogger fileLogger = null;
            foreach (Logger logger in ApplicationLogger.Current.Loggers)
            {
                fileLogger = logger as FileLogger;
            }

            if (fileLogger != null)
            {
                string fileName = fileLogger.GetFileName(LoggerSeverity.Infinity, DateTime.UtcNow, null, null, null, null);
                Console.WriteLine(fileName);
            }
        }

        [Test]
        public void TestCategory()
        {
            string file = FileSystemUtilities.PathCombine(Environment.CurrentDirectory, "testcategory.log");
            Console.WriteLine(file);
            Logger logger = new SimpleCompositeLogger(new FileLogger(file), "testcat");
            logger.Threshold = LoggerSeverity.None0;
            logger.LogDebug10("testmsg");
        }

        [Test]
        public void TestCriticalLogger()
        {
            try
            {
                throw new ArgumentNullException("test");
            }
            catch (Exception ex)
            {
                CriticalLogger.Current.LogException(ex);
            }
        }

        [Test]
        public void TestConsoleLogger()
        {
            ConsoleLogger.Current.Log(LoggerSeverity.Infinity, "test");
        }
    }
}
