using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PublicDomain.Logging;

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
    }
}
