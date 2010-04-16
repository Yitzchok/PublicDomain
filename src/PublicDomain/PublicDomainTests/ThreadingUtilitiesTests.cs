using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class ThreadingTests
    {
        /// <summary>
        /// Plays this instance.
        /// </summary>
        [Test]
        public void play()
        {
            Thread timer = ThreadingUtilities.SetTimer(5000, new CallbackNoArgs(delegate()
            {
                Console.WriteLine("Hello World");
            }));

            timer.Join();

            timer = ThreadingUtilities.SetTimerSimple(5000, delegate()
            {
                Console.WriteLine("Bonjour");
            });

            timer.Join();
        }

        /// <summary>
        /// Tests the interval.
        /// </summary>
        [Test]
        public void TestInterval()
        {
            Thread t = ThreadingUtilities.SetIntervalSimple(2000, delegate()
            {
                Console.WriteLine("Hello World");
            });

            // With this we don't even have to explicitly abort the thread
            t.IsBackground = true;

            Thread.Sleep(10000);
        }
    }
}
