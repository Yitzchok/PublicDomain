using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;

namespace PublicDomain.Code
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICodeRunner
    {
        /// <summary>
        /// Runs the specified arguments.
        /// </summary>
        /// <param name="compilerResults">The compiler results.</param>
        /// <param name="execMethod">The exec method.</param>
        /// <param name="output">The output.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        object Run(CompilerResults compilerResults, string execMethod, StringBuilder output, params string[] arguments);

        /// <summary>
        /// Runs to string.
        /// </summary>
        /// <param name="compilerResults">The compiler results.</param>
        /// <param name="execMethod">The exec method.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        string RunToString(CompilerResults compilerResults, string execMethod, params string[] arguments);

        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        Language Language { get; }
    }
}
