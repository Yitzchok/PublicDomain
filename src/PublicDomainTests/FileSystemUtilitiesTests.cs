using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class FSTests
    {
        /// <summary>
        /// Plays this instance.
        /// </summary>
        [Test]
        public void play()
        {
            Console.WriteLine(FileSystemUtilities.GetPathFromUri(FileSystemUtilities.GetFileUri(@"C:\test.html")));
        }

        [Test]
        public void Test()
        {
            foreach (string[] tests in new string[][] { 
                new string[] { "install/../templates/subSilver/images/logo_phpBB.gif", "/templates/subSilver/images/logo_phpBB.gif" },
                new string[] { "/install/../templates/subSilver/images/logo_phpBB.gif", "/templates/subSilver/images/logo_phpBB.gif" },
                new string[] { "http://www.tempuri.org/install/../templates/subSilver/images/logo_phpBB.gif", "http://www.tempuri.org/templates/subSilver/images/logo_phpBB.gif" },
                new string[] { "install/../templates/subSilver/images/logo_phpBB.gif", "/templates/subSilver/images/logo_phpBB.gif" }
            })
            {
                string result = FileSystemUtilities.CombineTrackbacksInPath(tests[0]);
                Console.WriteLine(result);
                Assert.AreEqual(tests[1], result);
            }
        }
    }
}
