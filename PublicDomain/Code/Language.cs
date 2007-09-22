using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Code
{
    /// <summary>
    /// Programming language enumeration (non-exhaustive)
    /// </summary>
    public enum Language
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// A language created by Microsoft for the .NET Framework
        /// http://msdn2.microsoft.com/vcsharp/
        /// </summary>
        CSharp = 1,

        /// <summary>
        /// PHP is a recursive acronym for "PHP: Hypertext Preprocessor."
        /// PHP is a dynamic programming language most often used for server-side webpages.
        /// http://www.php.net/
        /// </summary>
        Php = 2,

        /// <summary>
        /// Microsoft's implementation of Java for the .NET Framework,
        /// it is an evolution of J++.
        /// http://msdn2.microsoft.com/vjsharp/
        /// Microsoft is retiring support of J# and ceasing development, therefore
        /// it has become obsolete:
        /// http://msdn2.microsoft.com/en-us/vjsharp/default.aspx
        /// IKVM (JavaDotNet) is one alternative to J#.
        /// </summary>
        JSharp = 3,

        /// <summary>
        /// Microsoft's implementation of C++ for the .NET Framework
        /// http://msdn2.microsoft.com/visualc/
        /// </summary>
        CPlusPlusDotNet = 4,

        /// <summary>
        /// Microsoft's implementation of compiled JavaScript for the .NET Framework.
        /// http://msdn2.microsoft.com/en-us/library/72bd815a(vs.71).aspx
        /// </summary>
        JScriptDotNet = 5,

        /// <summary>
        /// Microsoft's implementation of Visual Basic for the .NET Framework
        /// http://msdn2.microsoft.com/vbasic/
        /// </summary>
        VisualBasicDotNet = 6,

        /// <summary>
        /// Java for the .NET Framework. Most popular implementation is IKVM
        /// http://www.ikvm.net/
        /// </summary>
        JavaDotNet = 7,

        /// <summary>
        /// Dynamic programming language most often used for server-side webpages.
        /// http://www.ruby-lang.org/
        /// </summary>
        Ruby = 8,

        /// <summary>
        /// Php for the .NET Framework. Most popular implementation is Phalanger
        /// http://www.codeplex.com/Phalanger
        /// </summary>
        PhpDotNet = 9,

        /// <summary>
        /// Class programming language
        /// </summary>
        C = 10,

        /// <summary>
        /// Object oriented programming language
        /// http://java.sun.com/
        /// </summary>
        Java = 11,

        /// <summary>
        /// Classic object oriented programming language
        /// </summary>
        CPlusPlus = 12,

        /// <summary>
        /// Ruby for the .NET Framework. Most popular implementation is IronRuby
        /// http://www.rubyforge.org/
        /// </summary>
        RubyDotNet = 13,

        /// <summary>
        /// Class object oriented programming language. This
        /// has been superceded by Visual Basic.NET
        /// </summary>
        VisualBasic = 14,

        /// <summary>
        /// Dynamic programming language most often used for server-side webpages.
        /// http://www.python.org/
        /// </summary>
        Python = 15,

        /// <summary>
        /// Python for the .NET Framework. Most popular implementation is IronPython
        /// http://www.codeplex.com/IronPython
        /// </summary>
        PythonDotNet = 16,

        /// <summary>
        /// Dynamic programming language most often used for server-side webpages and formatting tasks.
        /// http://www.perl.org/
        /// </summary>
        Perl = 17,

        /// <summary>
        /// Perl for the .NET Framework. Most popular implementation is ActiveState Perl Dev Kit
        /// http://www.activestate.com/Products/Perl_Dev_Kit/
        /// </summary>
        PerlDotNet = 18,

        /// <summary>
        /// Structured Query Language most often used to query an RDBMS
        /// </summary>
        Sql = 19,

        /// <summary>
        /// Object oriented programming language
        /// http://www.digitalmars.com/d/
        /// </summary>
        D = 20,

        /// <summary>
        /// Functional and object orient programming language for the .NET Framework
        /// http://research.microsoft.com/fsharp/fsharp.aspx
        /// </summary>
        FSharp = 21,

        /// <summary>
        /// Purely functional programming language
        /// http://www.haskell.org/
        /// </summary>
        Haskell = 22,

        /// <summary>
        /// Classic programming language
        /// http://www.adahome.com/
        /// </summary>
        Ada = 23,

        /// <summary>
        /// Classic programming language.
        /// COBOL stands for COmmon Business-Oriented Language.
        /// http://www.cobolstandards.com/
        /// </summary>
        Cobol = 24,

        /// <summary>
        /// Classic programming language.
        /// http://www.schemers.org/
        /// </summary>
        Scheme = 25,

        /// <summary>
        /// Classic programming language
        /// http://www.lisp.org/
        /// </summary>
        Lisp = 26,

        /// <summary>
        /// Classic programming language
        /// http://www.dmoz.org/Computers/Programming/Languages/Fortran/
        /// </summary>
        Fortran = 27,

        /// <summary>
        /// Classic programming language
        /// http://pascal-central.com/ppl/index.html
        /// </summary>
        Pascal = 28,

        /// <summary>
        /// Markup language for webpages
        /// http://www.w3.org/html/
        /// </summary>
        Html = 29,

        /// <summary>
        /// Xml conformant HTML
        /// http://www.w3.org/
        /// </summary>
        Xhtml = 30,

        /// <summary>
        /// General purpose markup language
        /// http://www.w3.org/XML/
        /// </summary>
        Xml = 31,

        /// <summary>
        /// Not a programming language but simply text characters
        /// </summary>
        PlainText = 32
    }

    /// <summary>
    /// Class holding various language constants (non-exhaustive), including
    /// a unique URI for a language
    /// </summary>
    public static class LanguageConstants
    {
        /// <summary>
        /// See http://tools.ietf.org/html/rfc2141. All Uris should be in lower case
        /// </summary>
        public const string LanguageUriPrefix = "urn:language:";

        /// <summary>
        /// 
        /// </summary>
        public const string UriAda = LanguageUriPrefix + "ada";

        /// <summary>
        /// 
        /// </summary>
        public const string UriC = LanguageUriPrefix + "c";

        /// <summary>
        /// 
        /// </summary>
        public const string UriCobol = LanguageUriPrefix + "cobol";

        /// <summary>
        /// 
        /// </summary>
        public const string UriCPlusPlus = LanguageUriPrefix + "c++";

        /// <summary>
        /// 
        /// </summary>
        public const string UriCPlusPlusDotNet = LanguageUriPrefix + "c++.net";

        /// <summary>
        /// 
        /// </summary>
        public const string UriCSharp = LanguageUriPrefix + "csharp";

        /// <summary>
        /// 
        /// </summary>
        public const string UriD = LanguageUriPrefix + "d";

        /// <summary>
        /// 
        /// </summary>
        public const string UriFortran = LanguageUriPrefix + "fortran";

        /// <summary>
        /// 
        /// </summary>
        public const string UriFSharp = LanguageUriPrefix + "fsharp";

        /// <summary>
        /// 
        /// </summary>
        public const string UriHaskell = LanguageUriPrefix + "haskell";

        /// <summary>
        /// 
        /// </summary>
        public const string UriHtml = LanguageUriPrefix + "html";

        /// <summary>
        /// 
        /// </summary>
        public const string UriJava = LanguageUriPrefix + "java";

        /// <summary>
        /// 
        /// </summary>
        public const string UriJavaDotNet = LanguageUriPrefix + "java.net";

        /// <summary>
        /// 
        /// </summary>
        public const string UriJScriptDotNet = LanguageUriPrefix + "jscript.net";

        /// <summary>
        /// 
        /// </summary>
        public const string UriJSharp = LanguageUriPrefix + "jsharp";

        /// <summary>
        /// 
        /// </summary>
        public const string UriLisp = LanguageUriPrefix + "lisp";

        /// <summary>
        /// 
        /// </summary>
        public const string UriPascal = LanguageUriPrefix + "pascal";

        /// <summary>
        /// 
        /// </summary>
        public const string UriPerl = LanguageUriPrefix + "perl";

        /// <summary>
        /// 
        /// </summary>
        public const string UriPerlDotNet = LanguageUriPrefix + "perl.net";

        /// <summary>
        /// 
        /// </summary>
        public const string UriPhp = LanguageUriPrefix + "php";

        /// <summary>
        /// 
        /// </summary>
        public const string UriPhpDotNet = LanguageUriPrefix + "php.net";

        /// <summary>
        /// 
        /// </summary>
        public const string UriPlainText = LanguageUriPrefix + "plaintext";

        /// <summary>
        /// 
        /// </summary>
        public const string UriPython = LanguageUriPrefix + "python";

        /// <summary>
        /// 
        /// </summary>
        public const string UriPythonDotNet = LanguageUriPrefix + "python.net";

        /// <summary>
        /// 
        /// </summary>
        public const string UriRuby = LanguageUriPrefix + "ruby";

        /// <summary>
        /// 
        /// </summary>
        public const string UriRubyDotNet = LanguageUriPrefix + "ruby.net";

        /// <summary>
        /// 
        /// </summary>
        public const string UriScheme = LanguageUriPrefix + "scheme";

        /// <summary>
        /// 
        /// </summary>
        public const string UriSql = LanguageUriPrefix + "sql";

        /// <summary>
        /// 
        /// </summary>
        public const string UriUnknown = LanguageUriPrefix + "unknown";

        /// <summary>
        /// 
        /// </summary>
        public const string UriVisualBasic = LanguageUriPrefix + "visualbasic";

        /// <summary>
        /// 
        /// </summary>
        public const string UriVisualBasicDotNet = LanguageUriPrefix + "visualbasic.net";

        /// <summary>
        /// 
        /// </summary>
        public const string UriXhtml = LanguageUriPrefix + "xhtml";

        /// <summary>
        /// 
        /// </summary>
        public const string UriXml = LanguageUriPrefix + "xml";
    }
}
