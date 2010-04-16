using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using System.IO;
using PublicDomain.Util;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class GeneralTests
    {
        [Test]
        public void play()
        {
            Console.WriteLine("done");
        }

        /// <summary>
        /// Plays this instance.
        /// </summary>
        [Test]
        public void TestJobs()
        {
            Process p;
            using (PublicDomain.Win32.Job job = new PublicDomain.Win32.Job("dorp"))
            {
                job.SetLimitWorkingSetSize((uint)GlobalConstants.BytesInAMegabyte, (uint)GlobalConstants.BytesInAMegabyte * 10);

                PublicDomain.Win32.Win32Structures.JOBOBJECT_BASIC_LIMIT_INFORMATION limitInfo = job.QueryInformationJobObjectLimit();

                Console.WriteLine(limitInfo.MaximumWorkingSetSize);

                p = new Process();
                p.StartInfo = new ProcessStartInfo(@"c:\windows\system32\cmd.exe");
                p.Start();

                job.AssignProcess(p);
            }

            p.Kill();
        }

        /// <summary>
        /// Tests the dispoable callback.
        /// </summary>
        [Test]
        public void TestDispoableCallback()
        {
            bool hit = false;
            using (new DisposableCallback(delegate() { hit = true; }))
            {
            }
            Assert.IsTrue(hit);

            StringContainer obj = new StringContainer(bool.FalseString);
            using (new DisposableCallback(delegate(object rock) { ((StringContainer)rock).UnderlyingString = bool.TrueString; }, obj))
            {
            }
            Assert.IsTrue(obj.UnderlyingString == bool.TrueString);
        }
    }
}
