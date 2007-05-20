using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.CodeCount
{
    /// <summary>
    /// Represents something that can be counted.
    /// </summary>
    public interface ICountable
    {
        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        List<ICountable> Children
        {
            get;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type
        {
            get;
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>The location.</value>
        string Location
        {
            get;
        }

        /// <summary>
        /// Counts the lines.
        /// </summary>
        /// <returns></returns>
        long CountLines();
    }
}
