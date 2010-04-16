using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class StringContainer
    {
        private string m_str;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringContainer"/> class.
        /// </summary>
        /// <param name="str">The STR.</param>
        public StringContainer(string str)
        {
            m_str = str;
        }

        /// <summary>
        /// Gets or sets the underlying string.
        /// </summary>
        /// <value>The underlying string.</value>
        public virtual string UnderlyingString
        {
            get
            {
                return m_str;
            }
            set
            {
                m_str = value;
            }
        }
    }
}
