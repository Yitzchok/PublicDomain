using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PublicDomain.Xml;

namespace PublicDomain
{
    [TestFixture]
    public class LenientXmlTests2
    {
        [Test]
        public void Test1()
        {
            string convert = @"
<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"">
<html>
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=iso-8859-1"">
<meta http-equiv=""Content-Style-Type"" content=""text/css"">
<title>Welcome to phpBB 2 Installation</title>
<link rel=""stylesheet"" href=""../templates/subSilver/subSilver.css"" type=""text/css"">
<style type=""text/css"">
<!--
th			{ background-image: url('../templates/subSilver/images/cellpic3.gif') }
td.cat		{ background-image: url('../templates/subSilver/images/cellpic1.gif') }
td.rowpic	{ background-image: url('../templates/subSilver/images/cellpic2.jpg'); background-repeat: repeat-y }
td.catHead,td.catSides,td.catLeft,td.catRight,td.catBottom { background-image: url('../templates/subSilver/images/cellpic1.gif') }

/* Import the fancy styles for IE only (NS4.x doesn't use the @import function) */
@import url(""../templates/subSilver/formIE.css""); 
//-->
</style>
</head>
<body bgcolor=""#E5E5E5"" text=""#000000"" link=""#006699"" vlink=""#5584AA"">

<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""10"" align=""center""> 
	<tr>
		<td class=""bodyline"" width=""100%""><table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
			<tr>
				<td><table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
					<tr>
						<td><img src=""../templates/subSilver/images/logo_phpBB.gif"" border=""0"" alt=""Forum Home"" vspace=""1"" /></td>
						<td align=""center"" width=""100%"" valign=""middle""><span class=""maintitle"">Welcome to phpBB 2 Installation</span></td>
					</tr>
				</table></td>
			</tr>
			<tr>
				<td><br /><br /></td>
			</tr>
			<tr>
				<td colspan=""2""><table width=""90%"" border=""0"" align=""center"" cellspacing=""0"" cellpadding=""0"">
					<tr>
						<td><span class=""gen"">Thank you for choosing phpBB 2. In order to complete this install please fill out the details requested below. Please note that the database you install into should already exist. If you are installing to a database that uses ODBC, e.g. MS Access you should first create a DSN for it before proceeding.</span></td>
					</tr>
				</table></td>
			</tr>
			<tr>
				<td><br /><br /></td>
			</tr>
			<tr>
				<td width=""100%""><table width=""100%"" cellpadding=""2"" cellspacing=""1"" border=""0"" class=""forumline""><form action=""install.php"" name=""install"" method=""post"">
					<tr>
						<th colspan=""2"">Basic Configuration</th>
					</tr>
					<tr>
						<td class=""row1"" align=""right"" width=""30%""><span class=""gen"">Default board language: </span></td>
						<td class=""row2""><select name=""lang"" onchange=""this.form.submit()""></select></td>
					</tr>
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Database Type: </span></td>
						<td class=""row2""><select name=""dbms"" onchange=""if(this.form.upgrade.options[this.form.upgrade.selectedIndex].value == 1){ this.selectedIndex = 0;}""><option value=""mysql"">MySQL 3.x</option><option value=""mysql4"">MySQL 4.x/5.x</option><option value=""postgres"">PostgreSQL 7.x</option><option value=""mssql"">MS SQL Server 7/2000</option><option value=""msaccess"">MS Access [ ODBC ]</option><option value=""mssql-odbc"">MS SQL Server [ ODBC ]</option></select></td>
					</tr>
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Choose your installation method:</span></td>
						<td class=""row2""><select name=""upgrade""onchange=""if (this.options[this.selectedIndex].value == 1) { this.form.dbms.selectedIndex = 0; }""><option value=""0"">Install</option><option value=""1"">Upgrade</option></select></td>
					</tr>
					<tr>
						<th colspan=""2"">Database Configuration</th>
					</tr>
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Database Server Hostname / DSN: </span></td>
						<td class=""row2""><input type=""text"" name=""dbhost"" value=""localhost"" /></td>
					</tr>
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Your Database Name: </span></td>
						<td class=""row2""><input type=""text"" name=""dbname"" value="""" /></td>
					</tr>
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Database Username: </span></td>
						<td class=""row2""><input type=""text"" name=""dbuser"" value="""" /></td>
					</tr>
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Database Password: </span></td>
						<td class=""row2""><input type=""password"" name=""dbpasswd"" value="""" /></td>
					</tr>
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Prefix for tables in database: </span></td>
						<td class=""row2""><input type=""text"" name=""prefix"" value=""phpbb_"" /></td>
					</tr>
					<tr>
						<th colspan=""2"">Admin Configuration</th>
					</tr>
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Admin Email Address: </span></td>
						<td class=""row2""><input type=""text"" name=""board_email"" value="""" /></td>
					</tr> 
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Domain Name: </span></td>
						<td class=""row2""><input type=""text"" name=""server_name"" value="""" /></td>
					</tr> 
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Server Port: </span></td>
						<td class=""row2""><input type=""text"" name=""server_port"" value=""80"" /></td>
					</tr>
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Script path: </span></td>
						<td class=""row2""><input type=""text"" name=""script_path"" value="""" /></td>
					</tr>
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Administrator Username: </span></td>
						<td class=""row2""><input type=""text"" name=""admin_name"" value="""" /></td>
					</tr>
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Administrator Password: </span></td>
						<td class=""row2""><input type=""password"" name=""admin_pass1"" value="""" /></td>
					</tr>
					<tr>
						<td class=""row1"" align=""right""><span class=""gen"">Administrator Password [ Confirm ]: </span></td>
						<td class=""row2""><input type=""password"" name=""admin_pass2"" value="""" /></td>
					</tr>
					<tr> 
					  <td class=""catBottom"" align=""center"" colspan=""2""><input type=""hidden"" name=""install_step"" value=""1"" /><input type=""hidden"" name=""cur_lang"" value=""english"" /><input class=""mainoption"" type=""submit"" value=""Start Install"" /></td>
					</tr>
				</table></form></td>
			</tr>
		</table></td>
	</tr>
</table>

</body>
</html>
";
            Test(convert, null);
        }

        public void Test2()
        {
            Test(@"<dorp:resource xmlns:mydorp=""http://www.mydorp.com/"" xmlns:dorp=""http://www.mydorp.com/"" xmlns:asp=""http://www.asp.net/"">
<dorp:RoundedCorners
    BottomLeftCorner=""~/images/spuirev/dorpletcornerbl.gif""
    BottomRightCorner=""~/images/spuirev/dorpletcornerbr.gif""
    TopLeftCorner=""~/images/spuirev/dorpletcornertl.gif""
    TopRightCorner=""~/images/spuirev/dorpletcornertr.gif""
    HorizontalBorderImageRepeatTop=""~/images/spuirev/dorplethorizontaltop.gif""
    HorizontalBorderImageRepeatBottom=""~/images/spuirev/dorplethorizontalbottom.gif""
    VerticalBorderImageRepeatLeft=""~/images/spuirev/dorpletverticalleft.gif""
    VerticalBorderImageRepeatRight=""~/images/spuirev/dorpletverticalright.gif""
    CornerImageWidth=""12""
    CornerImageHeight=""12""
    BorderColor=""#CCCCCC""
    FullWidth=""true""
    MainCellClass=""dorpletSpuiRevengeCell""
    IsDraghandle=""true""
    IsResizable=""true""
    IsAbsolutelyPositioned=""True""
>
    <table border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100%;"">
    <tr>
        <td valign=""top"">
            <table border=""0"" cellspacing=""0"" cellpadding=""0"">
            <tr>
                <td valign=""top"">
                    <dorp:dragHandle UseHandle=""None"" Options=""DetachContents,NoAutoExpand"">
                        <dorp:serviceProperty name=""icon"" href=""&resource1278;"" />
                    </dorp:dragHandle>
                </td><td>&#160;</td>
                <td valign=""top"">
                    <div class=""dorpletSpuiRevengeTitle"">
                    <dorp:dragHandle UseHandle=""None"" Options=""DetachContents,NoAutoExpand"">
                        <dorp:property href=""&resource1292;"" name=""title"" />
                    </dorp:dragHandle>
                    </div>
                </td>
            </tr>
            </table>
            <dorp:HR />
        </td>
    </tr>
    <tr>
        <td valign=""top"">

<html>
<h1> test
</html>
        </td>
    </tr>
    </table>
</dorp:RoundedCorners>
</dorp:resource>", null);
        }

        private void Test(string convert)
        {
            Test(convert, null);
        }

        private void Test(string convert, string compare)
        {
            LenientHtmlDocument doc = new LenientHtmlDocument();
            doc.CreateDefaultDocumentElement = true;
            doc.LoadXml(convert);
            Console.WriteLine(doc.DocumentElement.InnerXml);
        }
    }
}
