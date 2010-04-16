using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Code
{
    /// <summary>
    /// Programming language enumeration (non-exhaustive). See also
    /// http://en.wikipedia.org/wiki/List_of_programming_languages
    /// http://en.wikipedia.org/wiki/CLI_Languages
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
        /// PHP is a dynamic programming language most often used for server-side webpages:
        /// http://www.php.net/
        /// Php for the .NET Framework. Most popular implementation is Phalanger:
        /// http://www.codeplex.com/Phalanger
        /// </summary>
        Php = 2,

        /// <summary>
        /// </summary>
        Reserved3 = 3,

        /// <summary>
        /// </summary>
        Reserved4 = 4,

        /// <summary>
        /// </summary>
        Reserved5 = 5,

        /// <summary>
        /// </summary>
        Reserved6 = 6,

        /// <summary>
        /// </summary>
        Reserved7 = 7,

        /// <summary>
        /// Dynamic programming language most often used for server-side webpages:
        /// http://www.ruby-lang.org/
        /// Ruby for the .NET Framework. Most popular implementation is IronRuby:
        /// http://www.rubyforge.org/
        /// </summary>
        Ruby = 8,

        /// <summary>
        /// </summary>
        Reserved9 = 9,

        /// <summary>
        /// Class programming language
        /// </summary>
        C = 10,

        /// <summary>
        /// Object oriented programming language:
        /// http://java.sun.com/
        /// Java for the .NET Framework. Most popular implementation is IKVM:
        /// http://www.ikvm.net/
        /// </summary>
        Java = 11,

        /// <summary>
        /// Microsoft's implementation of C++ for the .NET Framework:
        /// http://msdn2.microsoft.com/visualc/
        /// </summary>
        CPlusPlus = 12,

        /// <summary>
        /// </summary>
        Reserved13 = 13,

        /// <summary>
        /// Class object oriented programming language.
        /// Microsoft's implementation of Visual Basic for the .NET Framework:
        /// http://msdn2.microsoft.com/vbasic/
        /// </summary>
        VisualBasic = 14,

        /// <summary>
        /// Dynamic programming language most often used for server-side webpages:
        /// http://www.python.org/
        /// Python for the .NET Framework. Most popular implementation is IronPython:
        /// http://www.codeplex.com/IronPython
        /// </summary>
        Python = 15,

        /// <summary>
        /// </summary>
        Reserved16 = 16,

        /// <summary>
        /// Dynamic programming language most often used for server-side webpages and formatting tasks:
        /// http://www.perl.org/
        /// Perl for the .NET Framework. Most popular implementation is ActiveState Perl Dev Kit:
        /// http://www.activestate.com/Products/Perl_Dev_Kit/
        /// </summary>
        Perl = 17,

        /// <summary>
        /// </summary>
        Reserved18 = 18,

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
        /// http://www.adahome.com/
        /// </summary>
        Ada = 23,

        /// <summary>
        /// COBOL stands for COmmon Business-Oriented Language.
        /// http://www.cobolstandards.com/
        /// </summary>
        Cobol = 24,

        /// <summary>
        /// http://www.schemers.org/
        /// </summary>
        Scheme = 25,

        /// <summary>
        /// http://www.lisp.org/
        /// </summary>
        Lisp = 26,

        /// <summary>
        /// http://www.dmoz.org/Computers/Programming/Languages/Fortran/
        /// </summary>
        Fortran = 27,

        /// <summary>
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
        PlainText = 32,

        /// <summary>
        /// Microsoft's implementation of compiled JavaScript for the .NET Framework.
        /// http://msdn2.microsoft.com/en-us/library/72bd815a(vs.71).aspx
        /// </summary>
        JavaScript = 33,

        /// <summary>
        /// Boo is an object oriented, statically typed programming language developed starting in 2003
        /// </summary>
        Boo = 34,

        /// <summary>
        /// 
        /// </summary>
        Eiffel = 35,

        /// <summary>
        /// 
        /// </summary>
        Cobra = 36,

        /// <summary>
        /// 
        /// </summary>
        Lexico = 37,

        /// <summary>
        /// 
        /// </summary>
        Mondrian = 38,

        /// <summary>
        /// 
        /// </summary>
        Nemerle = 39,

        /// <summary>
        /// 
        /// </summary>
        Prolog = 40,

        /// <summary>
        /// 
        /// </summary>
        Phrogram = 41,

        /// <summary>
        /// 
        /// </summary>
        Smalltalk = 42,

        /// <summary>
        /// 
        /// </summary>
        Rpg = 43,

        /// <summary>
        /// 
        /// </summary>
        Oberon = 44,

        /// <summary>
        /// 
        /// </summary>
        Apl = 45,

        /// <summary>
        /// 
        /// </summary>
        Forth = 46,

        /// <summary>
        /// 
        /// </summary>
        Modula = 47,

        /// <summary>
        /// 
        /// </summary>
        LOLCODE = 48,

        /// <summary>
        /// 
        /// </summary>
        Mercury = 49,

        /// <summary>
        /// 
        /// </summary>
        Io = 50,

        /// <summary>
        /// 
        /// </summary>
        PLI = 51,

        /// <summary>
        /// 
        /// </summary>
        Sml = 52,

        /// <summary>
        /// 
        /// </summary>
        Asm = 53,

        /// <summary>
        /// 
        /// </summary>
        Lua = 54,
    }
}
