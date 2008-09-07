using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.IO;

namespace PublicDomain.Code
{
    /// <summary>
    /// Methods for working with code and languages.
    /// </summary>
    public static class CodeUtilities
    {
        /// <summary>
        /// urn:language:
        /// See http://tools.ietf.org/html/rfc2141. All Uris should be in lower case
        /// </summary>
        public const string LanguageUriPrefix = "urn:language:";

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
        /// Strips the non file name characters.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="lang">The lang.</param>
        /// <returns></returns>
        public static string StripNonFileNameCharacters(string str, Language lang)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = StringUtilities.RemoveCharacters(str, Path.GetInvalidFileNameChars());
            }
            return str;
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
                default:
                    return RegexUtilities.RemoveNonWordCharacters(str);
            }
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
                case Language.JavaScript:
                    return "JavaScript";
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
                case Language.Sql:
                    return "SQL";
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
        /// Gets a <seealso cref="PublicDomain.Code.Language"/> given a string name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static Language GetLanguageByName(string name)
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
                case "cplusplus.net":
                case "c++.net":
                case "c++net":
                    return Language.CPlusPlus;
                case "c#":
                case "csharp":
                case "c#.net":
                case "c#net":
                case "csharp.net":
                    return Language.CSharp;
                case "java":
                case "java.net":
                case "javanet":
                case "vj#":
                case "j#":
                case "jsharp":
                    return Language.Java;
                case "js":
                case "javascript":
                case "jscript":
                    return Language.JavaScript;
                case "php":
                case "php.net":
                case "phpnet":
                    return Language.Php;
                case "visualbasic.net":
                case "visualbasicnet":
                case "vb":
                case "visualbasic":
                    return Language.VisualBasic;
                case "ruby":
                case "ruby.net":
                case "rubynet":
                    return Language.Ruby;
                case "perl":
                case "perl.net":
                case "perlnet":
                    return Language.Perl;
                case "python":
                case "python.net":
                case "pythonnet":
                    return Language.Python;
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

            if (uri.Length < LanguageUriPrefix.Length)
            {
                throw new ArgumentException("uri should begin with " + LanguageUriPrefix);
            }

            uri = uri.Substring(LanguageUriPrefix.Length);
            return General.ParseEnum<Language>(uri);
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
                default:
                    return LanguageUriPrefix + lang.ToString().ToLower();
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

        /// <summary>
        /// Gets the primary language file extension. This does not begin with a period.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static string GetPrimaryLanguageFileExtension(Language language)
        {
            // The default is just to take the language name and do ToLower
            // on it, so the switch is only necessary if this mapping doesn't do
            // the job.
            switch (language)
            {
                case Language.Unknown:
                    return "txt";
                case Language.CSharp:
                    return "cs";
                case Language.CPlusPlus:
                    return "cpp";
                case Language.JavaScript:
                    return "js";
                case Language.VisualBasic:
                    return "vb";
                case Language.Ruby:
                    return "rb";
                case Language.Python:
                    return "py";
                case Language.Perl:
                    return "pl";
                case Language.FSharp:
                    return "fs";
                case Language.PlainText:
                    return "txt";
                case Language.Haskell:
                    return "hs";
                case Language.Cobol:
                    return "cbl";
                case Language.Scheme:
                    return "scm";
                case Language.Fortran:
                    return "for";
                case Language.Pascal:
                    return "pas";
                case Language.Eiffel:
                    return "ei";
                case Language.Prolog:
                    return "pl";
                case Language.Smalltalk:
                    return "st";
                case Language.Forth:
                    return "4th";
                case Language.Modula:
                    return "mod";
                case Language.LOLCODE:
                    return "lol";
            }
            return language.ToString().ToLower();
        }
    }
}
