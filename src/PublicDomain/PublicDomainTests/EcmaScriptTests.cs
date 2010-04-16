using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PublicDomain.EcmaScript;

namespace PublicDomain
{
    [TestFixture]
    public class EcmaScriptTests
    {
        [Test]
        public void TestOutput()
        {
            LenientEcmaScriptDocument doc = new LenientEcmaScriptDocument();
            List<Pair<string, string>> tests = new List<Pair<string, string>>();
            tests.Add(new Pair<string, string>(@"", @""));

            foreach (Pair<string, string> t in tests)
            {
                doc.Load(t.First);
            }
        }
    }
}
