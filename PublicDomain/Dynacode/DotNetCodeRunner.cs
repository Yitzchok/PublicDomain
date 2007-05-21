using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;

namespace PublicDomain.Dynacode
{
    /// <summary>
    /// 
    /// </summary>
    public class DotNetCodeRunner : ICodeRunner
    {
        /// <summary>
        /// 
        /// </summary>
        protected Language m_language;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCodeRunner"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        public DotNetCodeRunner(Language language)
        {
            m_language = language;
        }

        /// <summary>
        /// Runs the specified arguments.
        /// </summary>
        /// <param name="compilerResults">The compiler results.</param>
        /// <param name="execMethod">The exec method.</param>
        /// <param name="output">The output.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public virtual object Run(CompilerResults compilerResults, string execMethod, StringBuilder output, params string[] arguments)
        {
            // Find the method in the assembly
            using (new ConsoleRerouter(output))
            {
                return ReflectionUtilities.InvokeMethod(compilerResults.CompiledAssembly, execMethod, new object[] { arguments });
            }
        }

        /// <summary>
        /// Runs to string.
        /// </summary>
        /// <param name="compilerResults">The compiler results.</param>
        /// <param name="execMethod">The exec method.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public virtual string RunToString(CompilerResults compilerResults, string execMethod, params string[] arguments)
        {
            StringBuilder sb = new StringBuilder();
            Run(compilerResults, execMethod, sb, arguments);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        public virtual Language Language
        {
            get
            {
                return m_language;
            }
        }
    }
}
