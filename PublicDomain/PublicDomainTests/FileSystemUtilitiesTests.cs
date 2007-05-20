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
    }
}
