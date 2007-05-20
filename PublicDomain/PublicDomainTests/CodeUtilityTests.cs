using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PublicDomain.Dynacode;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class CodeUtilityTests
    {
        /// <summary>
        /// Test1s this instance.
        /// </summary>
        [Test]
        public void Test1()
        {
            Console.WriteLine(CodeUtilities.EvalSnippet(Language.CSharp, "TimeSpan.Parse(\"-05:00\").Hours"));
            Console.WriteLine(CodeUtilities.EvalSnippet(Language.VisualBasic, "TimeSpan.Parse(\"-05:00\").Hours"));
        }
    }
}
