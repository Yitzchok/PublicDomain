using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.CodeCount
{
    /// <summary>
    /// Abstract implementation of <see cref="PublicDomain.CodeCount.ICountable"/>
    /// </summary>
    public abstract class Countable : ICountable
    {
        private List<ICountable> m_children = new List<ICountable>();

        /// <summary>
        /// 
        /// </summary>
        protected string m_name;

        /// <summary>
        /// 
        /// </summary>
        protected string m_type;

        /// <summary>
        /// 
        /// </summary>
        protected string m_location;

        #region ICountable Members

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        public virtual List<ICountable> Children
        {
            get
            {
                return m_children;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name
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
        public virtual string Type
        {
            get
            {
                return m_type;
            }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>The location.</value>
        public virtual string Location
        {
            get
            {
                return m_location;
            }
        }

        #endregion

        /// <summary>
        /// Counts the lines.
        /// </summary>
        /// <returns></returns>
        public abstract long CountLines();

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return Location;
        }
    }
}
