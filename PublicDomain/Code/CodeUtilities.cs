using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;

namespace PublicDomain.Code
{
    /// <summary>
    /// Methods for working with code and languages.
    /// </summary>
    public static class CodeUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultNamespace = "DefaultNamespace";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultClassName = "DefaultClassName";

        private static Language[] s_supportedLanguages;

        static CodeUtilities()
        {
            List<Language> supportedLanguages = new List<Language>();
            supportedLanguages.Add(Language.CSharp);
            supportedLanguages.Add(Language.VisualBasic);
            s_supportedLanguages = supportedLanguages.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StripNonIdentifierCharacters(Language lang, string str)
        {
            switch (lang)
            {
                case Language.CSharp:
                case Language.JSharp:
                    // http://www.ecma-international.org/publications/standards/Ecma-334.htm
                    // Page 70, Printed Page 92

                    // TODO The following is not complete
                    // TODO JSharp should its own version of this -- it is different

                    str = StringUtilities.RemoveCharactersInverse(str, '_', 'a', '-', 'z', 'A', '-', 'Z', '0', '-', '9');
                    break;
                case Language.VisualBasic:
                    // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/vbls7/html/vblrfVBSpec2_2.asp

                    str = StringUtilities.RemoveCharactersInverse(str, '_', 'a', '-', 'z', 'A', '-', 'Z', '0', '-', '9', '\\', '[', '\\', ']');
                    break;
                default:
                    throw new NotImplementedException();
            }
            return str;
        }

        /// <summary>
        /// Evals the snippet.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="simpleCode">The simple code.</param>
        /// <returns></returns>
        /// <exception cref="PublicDomain.Code.CodeUtilities.CompileException"/>
        /// <exception cref="PublicDomain.Code.CodeUtilities.NativeCompileException"/>
        public static string EvalSnippet(Language language, string simpleCode)
        {
            return Eval(language, GetSnippetCode(language, simpleCode));
        }

        /// <summary>
        /// Runs a snippet of code.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="code">The code.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="PublicDomain.Code.CodeUtilities.CompileException"/>
        /// <exception cref="PublicDomain.Code.CodeUtilities.NativeCompileException"/>
        public static string Eval(Language language, string code, params string[] arguments)
        {
            return Eval(language, code, true, arguments);
        }

        /// <summary>
        /// Evals the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="code">The code.</param>
        /// <param name="isSnippet">if set to <c>true</c> [is snippet].</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="PublicDomain.Code.CodeUtilities.CompileException"/>
        /// <exception cref="PublicDomain.Code.CodeUtilities.NativeCompileException"/>
        public static string Eval(Language language, string code, bool isSnippet, params string[] arguments)
        {
            CompilerResults compilerResults = Compile(language, code, isSnippet, true);

            // Now, run the code
            ICodeRunner codeRunner = GetCodeRunner(language);
            return codeRunner.RunToString(compilerResults, string.Format("{0}.{1}.{2}", DefaultNamespace, DefaultClassName, GetDefaultMainMethodName(language)), arguments);
        }

        /// <summary>
        /// Compiles the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        /// <exception cref="PublicDomain.Code.CodeUtilities.CompileException"/>
        /// <exception cref="PublicDomain.Code.CodeUtilities.NativeCompileException"/>
        public static CompilerResults Compile(Language language, string code)
        {
            return Compile(language, code, true, true);
        }

        /// <summary>
        /// Compiles the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="code">The code.</param>
        /// <param name="isSnippet">if set to <c>true</c> then <paramref name="code"/> will be placed into
        /// templated "application code", such as a static void main.</param>
        /// <param name="throwExceptionOnCompileError">if set to <c>true</c> [throw exception on compile error].</param>
        /// <returns></returns>
        /// <exception cref="PublicDomain.Code.CodeUtilities.CompileException"/>
        /// <exception cref="PublicDomain.Code.CodeUtilities.NativeCompileException"/>
        public static CompilerResults Compile(Language language, string code, bool isSnippet, bool throwExceptionOnCompileError)
        {
            using (CodeDomProvider domProvider = CodeDomProvider.CreateProvider(language.ToString()))
            {
                CompilerInfo compilerInfo = CodeDomProvider.GetCompilerInfo(language.ToString());
                CompilerParameters compilerParameters = compilerInfo.CreateDefaultCompilerParameters();
                PrepareCompilerParameters(language, compilerParameters);
                if (isSnippet)
                {
                    code = GetApplicationCode(language, code, DefaultClassName, DefaultNamespace);
                }
                CompilerResults results = domProvider.CompileAssemblyFromSource(compilerParameters, code);
                if (throwExceptionOnCompileError)
                {
                    CheckCompilerResultsThrow(results);
                }
                return results;
            }
        }

        /// <summary>
        /// Prepares the compiler parameters.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="compilerParameters">The compiler parameters.</param>
        public static void PrepareCompilerParameters(Language language, CompilerParameters compilerParameters)
        {
            switch (language)
            {
                case Language.CSharp:
                    break;
                case Language.VisualBasic:
                    compilerParameters.ReferencedAssemblies.Add(@"c:\windows\Microsoft.NET\Framework\v2.0.50727\System.dll");
                    compilerParameters.ReferencedAssemblies.Add(@"c:\windows\Microsoft.NET\Framework\v2.0.50727\System.Xml.dll");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the supported languages.
        /// </summary>
        /// <returns></returns>
        public static Language[] GetSupportedLanguages()
        {
            return s_supportedLanguages;
        }

        /// <summary>
        /// Gets the code runner.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static ICodeRunner GetCodeRunner(Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                case Language.VisualBasic:
                case Language.JSharp:
                    return new DotNetCodeRunner(language);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the name of the default main method.
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <returns></returns>
        public static string GetDefaultMainMethodName(Language lang)
        {
            switch (lang)
            {
                case Language.CSharp:
                case Language.VisualBasic:
                    return "Main";
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the application template code.
        /// Parameters:
        /// 0: Code
        /// 1: Class Name
        /// 2: Namespace
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <returns></returns>
        public static string GetApplicationTemplateCode(Language lang)
        {
            switch (lang)
            {
                case Language.CSharp:
                    return @"using System;
using System.Collections.Generic;
using System.Text;

namespace {2}
{{
    public class {1}
    {{
        public static void Main(string[] args)
        {{
            {0}
        }}
    }}
}}
";
                case Language.VisualBasic:
                    return @"Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace {2}
    Module {1}
        Sub Main(ByVal Args() as String)
            {0}
        End Sub
    End Module
End Namespace
";
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the snippet code.
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static string GetSnippetCode(Language lang, string code)
        {
            return string.Format(GetSnippetTemplateCode(lang), code);
        }

        /// <summary>
        /// Gets the snippet template code.
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <returns></returns>
        public static string GetSnippetTemplateCode(Language lang)
        {
            switch (lang)
            {
                case Language.CSharp:
                    return @"Console.Write({0});";
                case Language.VisualBasic:
                    return @"Console.Write({0})";
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the application code.
        /// Parameters:
        /// 0: Code
        /// 1: Class Name
        /// 2: Namespace
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static string GetApplicationCode(Language lang, params string[] args)
        {
            return string.Format(GetApplicationTemplateCode(lang), args);
        }

        /// <summary>
        /// This method throws an Exception if it finds an error in the
        /// <c>results</c>, otherwise it returns without side effect.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <exception cref="PublicDomain.Code.CodeUtilities.CompileException"/>
        /// <exception cref="PublicDomain.Code.CodeUtilities.NativeCompileException"/>
        public static void CheckCompilerResultsThrow(CompilerResults results)
        {
            if (results.Errors.HasErrors)
            {
                string msg = GetCompilerErrorsAsString(results.Errors);
                if (results.NativeCompilerReturnValue != 0)
                {
                    msg += Environment.NewLine + GetNativeCompilerErrorMessage(results);
                }
                throw new CompileException(msg);
            }
            else if (results.NativeCompilerReturnValue != 0)
            {
                throw new NativeCompileException(GetNativeCompilerErrorMessage(results));
            }
        }

        private static string GetNativeCompilerErrorMessage(CompilerResults results)
        {
            return "Compiler returned exit code " + results.NativeCompilerReturnValue;
        }

        /// <summary>
        /// Gets the compiler errors as string.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static string GetCompilerErrorsAsString(CompilerErrorCollection errors)
        {
            StringBuilder sb = new StringBuilder(errors.Count * 10);
            CompilerError error;
            for (int i = 0; i < errors.Count; i++)
            {
                error = errors[i];
                if (i > 0)
                {
                    sb.Append(Environment.NewLine);
                }
                sb.Append(error.ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the display name of the language.
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <returns></returns>
        public static string GetLanguageDisplayName(Language lang)
        {
            switch (lang)
            {
                case Language.CPlusPlus:
                    return "C++";
                case Language.CSharp:
                    return "C#";
                case Language.Java:
                    return "Java";
                case Language.JScriptDotNet:
                    return "JScript.NET";
                case Language.JSharp:
                    return "J#";
                case Language.Php:
                    return "PHP";
                case Language.Ruby:
                    return "Ruby";
                case Language.VisualBasic:
                    return "Visual Basic";
                case Language.FSharp:
                    return "F#";
                case Language.Cobol:
                    return "COBOL";
                case Language.CPlusPlusDotNet:
                    return "C++.NET";
                case Language.JavaDotNet:
                    return "Java.NET";
                case Language.PerlDotNet:
                    return "Perl.NET";
                case Language.PhpDotNet:
                    return "PHP.NET";
                case Language.PythonDotNet:
                    return "Python.NET";
                case Language.RubyDotNet:
                    return "Ruby.NET";
                case Language.Sql:
                    return "SQL";
                case Language.VisualBasicDotNet:
                    return "Visual Basic.NET";
                case Language.Html:
                    return "HTML";
                case Language.Xhtml:
                    return "XHTML";
                case Language.Xml:
                    return "XML";
                case Language.PlainText:
                    return "Plain Text";
            }
            return lang.ToString();
        }

        /// <summary>
        /// Gets the name of the language by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static Language GetLanguageByName(string name)
        {
            return GetLanguageByName(name, true);
        }

        /// <summary>
        /// Gets a <seealso cref="PublicDomain.Code.Language"/> given a string name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="assumeDotNet">if set to <c>true</c> [assume dot net].</param>
        /// <returns></returns>
        public static Language GetLanguageByName(string name, bool assumeDotNet)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            name = name.ToLower().Trim().Replace(" ", "");
            switch (name)
            {
                case "cplusplus":
                case "c++":
                    return assumeDotNet ? Language.CPlusPlusDotNet : Language.CPlusPlus;
                case "cplusplus.net":
                case "c++.net":
                case "c++net":
                    return Language.CPlusPlusDotNet;
                case "c#":
                case "csharp":
                case "c#.net":
                case "c#net":
                case "csharp.net":
                    return Language.CSharp;
                case "java":
                    return assumeDotNet ? Language.JavaDotNet : Language.Java;
                case "java.net":
                case "javanet":
                    return Language.JavaDotNet;
                case "js":
                case "jscript":
                    return Language.JScriptDotNet;
                case "vj#":
                case "j#":
                case "jsharp":
                    return Language.JSharp;
                case "php":
                    return assumeDotNet ? Language.PhpDotNet : Language.Php;
                case "php.net":
                case "phpnet":
                    return Language.PhpDotNet;
                case "visualbasic.net":
                case "visualbasicnet":
                    return Language.VisualBasicDotNet;
                case "vb":
                case "visualbasic":
                    return assumeDotNet ? Language.VisualBasicDotNet : Language.VisualBasic;
                case "ruby":
                    return assumeDotNet ? Language.RubyDotNet : Language.Ruby;
                case "ruby.net":
                case "rubynet":
                    return Language.RubyDotNet;
                case "perl":
                    return assumeDotNet ? Language.PerlDotNet : Language.Perl;
                case "perl.net":
                case "perlnet":
                    return Language.PerlDotNet;
                case "python":
                    return assumeDotNet ? Language.PythonDotNet : Language.Python;
                case "python.net":
                case "pythonnet":
                    return Language.PythonDotNet;
                case "f#":
                case "fsharp":
                case "f#.net":
                case "f#net":
                case "fsharp.net":
                case "fsharpnet":
                    return Language.FSharp;
                case "plain":
                    return Language.PlainText;
                default:
                    return (Language)Enum.Parse(typeof(Language), name, true);
            }
        }

        /// <summary>
        /// Gets the language by URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static Language GetLanguageByUri(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException("uri");
            }
            uri = uri.Trim().ToLower();
            switch (uri)
            {
                case LanguageConstants.UriAda:
                    return Language.Ada;
                case LanguageConstants.UriC:
                    return Language.C;
                case LanguageConstants.UriCobol:
                    return Language.Cobol;
                case LanguageConstants.UriCPlusPlus:
                    return Language.CPlusPlus;
                case LanguageConstants.UriCPlusPlusDotNet:
                    return Language.CPlusPlusDotNet;
                case LanguageConstants.UriCSharp:
                    return Language.CSharp;
                case LanguageConstants.UriD:
                    return Language.D;
                case LanguageConstants.UriFortran:
                    return Language.Fortran;
                case LanguageConstants.UriFSharp:
                    return Language.FSharp;
                case LanguageConstants.UriHaskell:
                    return Language.Haskell;
                case LanguageConstants.UriHtml:
                    return Language.Html;
                case LanguageConstants.UriJava:
                    return Language.Java;
                case LanguageConstants.UriJavaDotNet:
                    return Language.JavaDotNet;
                case LanguageConstants.UriJScriptDotNet:
                    return Language.JScriptDotNet;
                case LanguageConstants.UriJSharp:
                    return Language.JSharp;
                case LanguageConstants.UriLisp:
                    return Language.Lisp;
                case LanguageConstants.UriPascal:
                    return Language.Pascal;
                case LanguageConstants.UriPerl:
                    return Language.Perl;
                case LanguageConstants.UriPerlDotNet:
                    return Language.PerlDotNet;
                case LanguageConstants.UriPhp:
                    return Language.Php;
                case LanguageConstants.UriPhpDotNet:
                    return Language.PhpDotNet;
                case LanguageConstants.UriPlainText:
                    return Language.PlainText;
                case LanguageConstants.UriPython:
                    return Language.Python;
                case LanguageConstants.UriPythonDotNet:
                    return Language.PythonDotNet;
                case LanguageConstants.UriRuby:
                    return Language.Ruby;
                case LanguageConstants.UriRubyDotNet:
                    return Language.RubyDotNet;
                case LanguageConstants.UriScheme:
                    return Language.Scheme;
                case LanguageConstants.UriSql:
                    return Language.Sql;
                case LanguageConstants.UriUnknown:
                    return Language.Unknown;
                case LanguageConstants.UriVisualBasic:
                    return Language.VisualBasic;
                case LanguageConstants.UriVisualBasicDotNet:
                    return Language.VisualBasicDotNet;
                case LanguageConstants.UriXhtml:
                    return Language.Xhtml;
                case LanguageConstants.UriXml:
                    return Language.Xml;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the language URI.
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <returns></returns>
        public static string GetLanguageUri(Language lang)
        {
            switch (lang)
            {
                case Language.Ada:
                    return LanguageConstants.UriAda;
                case Language.C:
                    return LanguageConstants.UriC;
                case Language.Cobol:
                    return LanguageConstants.UriCobol;
                case Language.CPlusPlus:
                    return LanguageConstants.UriCPlusPlus;
                case Language.CPlusPlusDotNet:
                    return LanguageConstants.UriCPlusPlusDotNet;
                case Language.CSharp:
                    return LanguageConstants.UriCSharp;
                case Language.D:
                    return LanguageConstants.UriD;
                case Language.Fortran:
                    return LanguageConstants.UriFortran;
                case Language.FSharp:
                    return LanguageConstants.UriFSharp;
                case Language.Haskell:
                    return LanguageConstants.UriHaskell;
                case Language.Html:
                    return LanguageConstants.UriHtml;
                case Language.Java:
                    return LanguageConstants.UriJava;
                case Language.JavaDotNet:
                    return LanguageConstants.UriJavaDotNet;
                case Language.JScriptDotNet:
                    return LanguageConstants.UriJScriptDotNet;
                case Language.JSharp:
                    return LanguageConstants.UriJSharp;
                case Language.Lisp:
                    return LanguageConstants.UriLisp;
                case Language.Pascal:
                    return LanguageConstants.UriPascal;
                case Language.Perl:
                    return LanguageConstants.UriPerl;
                case Language.PerlDotNet:
                    return LanguageConstants.UriPerlDotNet;
                case Language.Php:
                    return LanguageConstants.UriPhp;
                case Language.PhpDotNet:
                    return LanguageConstants.UriPhpDotNet;
                case Language.PlainText:
                    return LanguageConstants.UriPlainText;
                case Language.Python:
                    return LanguageConstants.UriPython;
                case Language.PythonDotNet:
                    return LanguageConstants.UriPythonDotNet;
                case Language.Ruby:
                    return LanguageConstants.UriRuby;
                case Language.RubyDotNet:
                    return LanguageConstants.UriRubyDotNet;
                case Language.Scheme:
                    return LanguageConstants.UriScheme;
                case Language.Sql:
                    return LanguageConstants.UriSql;
                case Language.Unknown:
                    return LanguageConstants.UriUnknown;
                case Language.VisualBasic:
                    return LanguageConstants.UriVisualBasic;
                case Language.VisualBasicDotNet:
                    return LanguageConstants.UriVisualBasicDotNet;
                case Language.Xhtml:
                    return LanguageConstants.UriXhtml;
                case Language.Xml:
                    return LanguageConstants.UriXml;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Thrown when an error is encountered compiling.
        /// </summary>
        [Serializable]
        public class CompileException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CompileException"/> class.
            /// </summary>
            public CompileException() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="CompileException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>

            public CompileException(string message) : base(message) { }
            /// <summary>
            /// Initializes a new instance of the <see cref="CompileException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="inner">The inner.</param>

            public CompileException(string message, Exception inner) : base(message, inner) { }
            /// <summary>
            /// Initializes a new instance of the <see cref="CompileException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>

            protected CompileException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }

        /// <summary>
        /// Thrown when the compiler returns an unexpected value.
        /// </summary>
        [Serializable]
        public class NativeCompileException : CompileException
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="NativeCompileException"/> class.
            /// </summary>
            public NativeCompileException() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="NativeCompileException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            public NativeCompileException(string message) : base(message) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="NativeCompileException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="inner">The inner.</param>
            public NativeCompileException(string message, Exception inner) : base(message, inner) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="NativeCompileException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
            protected NativeCompileException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }
}
