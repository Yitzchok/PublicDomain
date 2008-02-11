using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Xml;
using PublicDomain.Xml;

namespace PublicDomain
{
    [TestFixture]
    public class LenientXmlTests
    {
        [Test]
        public void SimpleTests()
        {
            Dictionary<string, string> cmp = new Dictionary<string, string>();
            string defaultRootElementName = LenientHtmlDocument.DefaultRootHtmlElementName;
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
            cmp[@"<table border=0 cellspacing='1' cellpadding=""2"" style=""width:100%;""><tr rowspan=2><td valign='top'>&nbsp;</td></tr></table>"] = @"<table border=""0"" cellspacing=""1"" cellpadding=""2"" style=""width:100%;""><tr rowspan=""2""><td valign=""top"">&nbsp;</td></tr></table>";
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
            cmp["&gt;&nbsp; &nbsp;"] = start + "&gt;&nbsp; &nbsp;" + end;
            cmp["<p>&gt;&nbsp; &nbsp;</p>"] = "<p>&gt;&nbsp; &nbsp;</p>";
            cmp["<dorp:map location=\"new york\"><dorp:point location=\"coffee shop\"/><dorp:point location=\"central park\"/></dorp:map>"] = "<dorp:map location=\"new york\" xmlns:dorp=\"urn:dorp\"><dorp:point location=\"coffee shop\" /><dorp:point location=\"central park\" /></dorp:map>";
            cmp["&blah;"] = start + "&blah;" + end;
            cmp["& blah;"] = start + "&amp; blah;" + end;
            cmp["<div onclick='if(this.style && this.style.visibility==\"hidden\"'>"] = "<div onclick=\"if(this.style &amp;&amp; this.style.visibility==&quot;hidden&quot;\" />";
            cmp["<select id=blah><option>1</option><option value=\"2a\">2</option>"] = "<select id=\"blah\"><option>1</option><option value=\"2a\">2</option></select>";
            cmp["<select id=blah><option>1<option value=\"2a\">2"] = "<select id=\"blah\"><option>1</option><option value=\"2a\">2</option></select>";

            cmp[@"<meta http-equiv=""Content-Style-Type"" content=""text/css"">"] = @"<meta http-equiv=""Content-Style-Type"" content=""text/css"" />";
            cmp[@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"">"] = full;
            cmp["&#160;"] = start + "&nbsp;" + end;
            cmp[@"<a><b><c>y</c><br />z</b></a>"] = @"<a><b><c>y</c><br />z</b></a>";
            cmp[@"<td width=""200""><span class=""class""><span style=""font-size: larger"">Welcome to $BLANK!</span><br />Population: {4}</span></td>"] = @"<td width=""200""><span class=""class""><span style=""font-size: larger"">Welcome to $BLANK!</span><br />Population: {4}</span></td>";
            cmp[@"<test at=""&lt;"" />"] = "<test at=\"&amp;lt;\" />";
            cmp[@"<script>
    var query = 'query=execute&namingContainer=' + dorp.escape(tab.contentid) + '&resourceUri=' + dorp.escape(resourceUri) + '&action=' + dorp.escape(action);
</script>"] = @"<script><!--
    var query = 'query=execute&namingContainer=' + dorp.escape(tab.contentid) + '&resourceUri=' + dorp.escape(resourceUri) + '&action=' + dorp.escape(action);
--></script>";
            cmp[@"<select><option>a<option>b<option>c</select>"] = @"<select><option>a</option><option>b</option><option>c</option></select>";
            cmp[@"<select><option><br/><option><br/><option><br/></select>"] = @"<select><option><br /></option><option><br /></option><option><br /></option></select>";
            cmp[@"<select><option><title a=""test""/><option><title/><option><title/></select>"] = @"<select><option><title a=""test"" /></option><option><title /></option><option><title /></option></select>";
            cmp[@"<div width=""&test;"">"] = @"<div width=""blah"" />";
            cmp[@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd""><html xmlns=""http://www.w3.org/1999/xhtml"">
<head>"] = @"<html xmlns=""http://www.w3.org/1999/xhtml"">
<head /></html>";
            cmp[@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd""><html xmlns=""http://www.w3.org/1999/xhtml"">
<head><title>blah</title></head><body><br><a name=""top"" id=""top""></a></body>"] = @"<html xmlns=""http://www.w3.org/1999/xhtml"">
<head><title>blah</title></head><body><br /><a name=""top"" id=""top"" /></body></html>";

            LenientHtmlDocument doc = new TestLenientHtmlDocument();
            DoCompare(cmp, doc);
        }

        private static void DoCompare(Dictionary<string, string> cmp, LenientHtmlDocument doc)
        {
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
                output = doc.OuterHtml;
                Console.WriteLine(output);
            }
        }

        [Test]
        public void XmlTest()
        {
            XmlDocument doc = new XmlDocument();
            //doc.LoadXml(@"");
            //Console.WriteLine(doc.DocumentElement.OuterXml);
        }

        [Test]
        public void TestHtmlToText()
        {
            Dictionary<string, string> cmp = new Dictionary<string, string>();
            string defaultRootElementName = LenientHtmlToTextDocument.DefaultTextRootElementName;
            string full = "<" + defaultRootElementName + " />";
            string start = "<" + defaultRootElementName + ">";
            string end = "</" + defaultRootElementName + ">";

            cmp["<test>test</test>"] = start + "test" + end;
            cmp[@""] = full;
            cmp[@" "] = start + @" " + end;
            cmp[@"
"] = start + @"
" + end;
            cmp["<p>"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + end;
            cmp["<p/>"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + end;
            cmp["<p />"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + end;
            cmp["<p / >"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + end;
            cmp["<p /<p>"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + end;
            cmp["<p / ><p>"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + end;
            cmp["<      p        >"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + end;
            cmp["<p /    >  < p >"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + "  " + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + end;
            cmp["<p><p><p><p><p>"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + end;
            cmp["<br><br><br>"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + end;
            cmp["</br>"] = full;
            cmp["<br></br>"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + end;
            cmp["<br><br></br></br>"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + end;
            cmp["<br> <br> </br> </br>"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + " " + LenientHtmlToTextDocument.DefaultHtmlNewline + "  " + end;
            cmp["<br> <br> a</br> </br>"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + " " + LenientHtmlToTextDocument.DefaultHtmlNewline + " a " + end;
            cmp["<"] = full;
            cmp[">"] = start + "&gt;" + end;
            cmp["<br>>"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + "&gt;" + end;
            cmp[@"
<option>C#</option><option>HTML</option><option>Java</option><option>JScript</option><option>J#</option><option>PHP</option><option>Visual Basic</option>
"] = start + @"
C#HTMLJavaJScriptJ#PHPVisual Basic
" + end;
            cmp["<option>blah yakety yak</option>"] = start + "blah yakety yak" + end;
            cmp["<option><></option>"] = full;
            cmp["<test at1=\"a\" at2/>"] = full;
            cmp["<test at1=\"a   \" at2/>"] = full;
            cmp["<input type=checkbox name=test enabled>"] = full;
            cmp[@"
This is a <b>simple</B> test.
"] = start + @"
This is a simple test.
" + end;
            cmp[@"<table border=0 cellspacing='1' cellpadding=""2"" style=""width:100%;"">
<tr rowspan=2><td valign='top'>   blah</td></tr></table>"] = start + @"
   blah" + end;
            cmp[@"<table border=0 cellspacing='1' cellpadding=""2"" style=""width:100%;""><tr rowspan=2><td valign='top'>&nbsp;</td></tr></table>"] = start + @" " + end;
            cmp[@"<script language=javascript>
<!--
function test()
{
    alert('hi');
}
//-->
</script>"] = start + @"

" + end;
            cmp[@"<script language=javascript>
<![CDATA[
function test()
{
    alert('hi');
}
]]>
</script>"] = start + @"

function test()
{
    alert('hi');
}

" + end;
            cmp["<![CDATA[]]>"] = full;
            cmp["<![CDATA[<>]]>"] = start + "&lt;&gt;" + end;
            cmp["<![CDATA[<<!----->>]]>"] = start + "&lt;&lt;!-----&gt;&gt;" + end;
            cmp["<![CDATA[<<!----->>]]><p>"] = start + "&lt;&lt;!-----&gt;&gt;" + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + end;
            cmp["<html xmlns:dorp=\"urn:test\"><dorp:test></html>"] = full;
            cmp["&gt;"] = start + "&gt;" + end;
            cmp["&gt;&nbsp; &nbsp;"] = start + "&gt;   " + end;
            cmp["<p>&gt;&nbsp; &nbsp;</p>"] = start + LenientHtmlToTextDocument.DefaultHtmlNewline + LenientHtmlToTextDocument.DefaultHtmlNewline + "&gt;   " + end;
            cmp["<dorp:map location=\"new york\"><dorp:point location=\"coffee shop\"/><dorp:point location=\"central park\"/></dorp:map>"] = full;
            cmp["&blah;"] = start + "&amp;blah;" + end;
            cmp["& blah;"] = start + "&amp; blah;" + end;
            cmp["<div onclick='if(this.style && this.style.visibility==\"hidden\"'>"] = full;
            cmp["<select id=blah><option>1</option><option value=\"2a\">2</option>"] = start + "12" + end;
            cmp["<select id=blah><option>1<option value=\"2a\">2"] = start + "12" + end;

            DoCompare(cmp, new LenientHtmlToTextDocument());
        }

        [Test]
        public void TestSubclass()
        {
            LenientHtmlSubclassTestDocument doc = new LenientHtmlSubclassTestDocument();
            doc.LoadXml(@"
<dorp:response nomenu=""true"" nofocusonclick=""true"" />
<img>
<![CDATA[
function collapse(obj)
{
}
]]>
</img>
<div style=""width: 100%; height: 67px;"" id=""spuibar"">
");
            Console.WriteLine(doc.DocumentElement.OuterXml);
        }
    }
}
