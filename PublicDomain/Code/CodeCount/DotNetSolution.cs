using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Code.CodeCount
{
    /// <summary>
    /// Represents a .NET solution which is countable
    /// </summary>
    public class DotNetSolution : Countable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetSolution"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public DotNetSolution(string filename)
        {
        }

        /// <summary>
        /// Counts the lines.
        /// </summary>
        /// <returns></returns>
        public override long CountLines()
        {
            return 0;
        }
    }
}
