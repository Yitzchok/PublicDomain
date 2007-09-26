using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PublicDomain.LenientXml;
using System.Xml;

namespace PublicDomain
{
    [TestFixture]
    public class LenientXmlTests
    {
        [Test]
        public void SimpleTests()
        {
            Dictionary<string, string> cmp = new Dictionary<string, string>();
            string defaultRootElementName = "html";
            string full = "<" + defaultRootElementName + " />";
            string start = "<" + defaultRootElementName + ">";
            string end = "</" + defaultRootElementName + ">";

            cmp[@""] = full;
            cmp[@" "] = start + @" " + end;
            cmp[@"
"] = start + @"
" + end;
            cmp["<p>"] = "<p />";
            cmp["<p/>"] = "<p />";
            cmp["<p />"] = "<p />";
            cmp["<p / >"] = "<p />";
            cmp["<p /<p>"] = start + "<p /><p />" + end;
            cmp["<p / ><p>"] = start + "<p /><p />" + end;
            cmp["<      p        >"] = "<p />";
            cmp["<p /    >  < p >"] = start + "<p />  <p />" + end;
            cmp["<p><p><p><p><p>"] = "<p><p><p><p><p /></p></p></p></p>";
            cmp["<br><br><br>"] = start + "<br /><br /><br />" + end;
            cmp["</br>"] = full;
            cmp["<br></br>"] = "<br />";
            cmp["<br><br></br></br>"] = start + "<br /><br />" + end;
            cmp["<br> <br> </br> </br>"] = start + "<br /> <br />  " + end;
            cmp["<br> <br> a</br> </br>"] = start + "<br /> <br /> a " + end;
            cmp["<"] = full;
            cmp[">"] = start + "&gt;" + end;
            cmp["<br>>"] = start + "<br />&gt;" + end;
            cmp[@"
<option>C#</option><option>HTML</option><option>Java</option><option>JScript</option><option>J#</option><option>PHP</option><option>Visual Basic</option>
"] = start + @"
<option>C#</option><option>HTML</option><option>Java</option><option>JScript</option><option>J#</option><option>PHP</option><option>Visual Basic</option>
" + end;
            cmp[@"
<option>C#</option> <option>HTML</option><option>Java</option><option>JScript</option><option>J#</option><option>PHP</option><option>Visual Basic</option>
"] = start + @"
<option>C#</option> <option>HTML</option><option>Java</option><option>JScript</option><option>J#</option><option>PHP</option><option>Visual Basic</option>
" + end;
            cmp[@"
<option>C#</option><option> HTML </option><option>Java</option><option>JScript</option><option>J#</option><option>PHP</option><option>Visual Basic</option>
"] = start + @"
<option>C#</option><option> HTML </option><option>Java</option><option>JScript</option><option>J#</option><option>PHP</option><option>Visual Basic</option>
" + end;
            cmp[@"
<option  > C#<  /   option  ><option>HTML</option><option>Java</option><option>JScript</option><option>J#</option><option>PHP</option><option>Visual Basic</option>
"] = start + @"
<option> C#</option><option>HTML</option><option>Java</option><option>JScript</option><option>J#</option><option>PHP</option><option>Visual Basic</option>
" + end;
            cmp["<option>blah yakety yak</option>"] = "<option>blah yakety yak</option>";
            cmp["<option><></option>"] = "<option />";
            //cmp["<option><</option>"] = "<option>&lt;</option>";
            cmp["<test at1=\"a\" at2/>"] = "<test at1=\"a\" at2=\"\" />";
            cmp["<test at1=\"a   \" at2/>"] = "<test at1=\"a   \" at2=\"\" />";
            cmp["<input type=checkbox name=test enabled>"] = "<input type=\"checkbox\" name=\"test\" enabled=\"\" />";
            cmp[@"
This is a <b>simple</B> test.
"] = start + @"
This is a <b>simple</b> test.
" + end;
            cmp[@"<table border=0 cellspacing='1' cellpadding=""2"" style=""width:100%;"">
<tr rowspan=2><td valign='top'>   blah</td></tr></table>"] = @"<table border=""0"" cellspacing=""1"" cellpadding=""2"" style=""width:100%;"">
<tr rowspan=""2""><td valign=""top"">   blah</td></tr></table>";
            //cmp[@"<table border=0 cellspacing='1' cellpadding=""2"" style=""width:100%;""><tr rowspan=2><td valign='top'>&nbsp;</td></tr></table>"] = @"<table border=""0"" cellspacing=""1"" cellpadding=""2"" style=""width:100%;""><tr rowspan=""2""><td valign=""top"">&nbsp;</td></tr></table>";
            cmp[@"<script language=javascript>
<!--
function test()
{
    alert('hi');
}
//-->
</script>"] = @"<script language=""javascript"">
<!--
function test()
{
    alert('hi');
}
//-->
</script>";
            cmp[@"<script language=javascript>
<![CDATA[
function test()
{
    alert('hi');
}
]]>
</script>"] = @"<script language=""javascript"">
<![CDATA[
function test()
{
    alert('hi');
}
]]>
</script>";
            cmp[@"<script language=javascript>
<!--<![CDATA[
function test()
{
    alert('hi');
}
]]>//-->
</script>"] = @"<script language=""javascript"">
<!--<![CDATA[
function test()
{
    alert('hi');
}
]]>//-->
</script>";
            cmp["<![CDATA[]]>"] = start + "<![CDATA[]]>" + end;
            cmp["<![CDATA[<>]]>"] = start + "<![CDATA[<>]]>" + end;
            cmp["<![CDATA[<<!----->>]]>"] = start + "<![CDATA[<<!----->>]]>" + end;
            cmp["<![CDATA[<<!----->>]]><p>"] = start + "<![CDATA[<<!----->>]]><p />" + end;
            cmp["<html xmlns:dorp=\"urn:test\"><dorp:test></html>"] = "<html xmlns:dorp=\"urn:test\"><dorp:test /></html>";
            cmp["&gt;"] = start + "&gt;" + end;

            LenientHtmlDocument doc = new LenientHtmlDocument();
            foreach (string x in cmp.Keys)
            {
                Console.WriteLine(GlobalConstants.DividerEquals);
                Console.WriteLine("Loading:");
                Console.WriteLine(x);
                string y = cmp[x];
                doc.LoadXml(x);
                Console.WriteLine(GlobalConstants.DividerEquals);
                string output = doc.DocumentElement.OuterXml;
                Console.WriteLine(output);
                Assert.AreEqual(y, output);
            }
        }

        [Test]
        public void XmlTest()
        {
            XmlDocument doc = new XmlDocument();
            //doc.LoadXml(@"");
            //Console.WriteLine(doc.DocumentElement.OuterXml);
        }
    }
}
