using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Geography
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Region
    {
        private string m_name;
        private RegionType m_type;

        /// <summary>
        /// Initializes a new instance of the <see cref="Region"/> class.
        /// </summary>
        public Region()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Region"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Region(string name)
            : this(name, RegionType.Region)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Region"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        public Region(string name, RegionType type)
        {
            m_name = name;
            m_type = type;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return m_name;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public RegionType Type
        {
            get
            {
                return m_type;
            }
        }
    }
}
