using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Code.CodeCount
{
    /// <summary>
    /// Represents a .NET project which is countable
    /// </summary>
    public class DotNetProject : Countable
    {
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
