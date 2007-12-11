using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.EcmaScript
{
    /// <summary>
    /// 
    /// </summary>
    public class EcmaScriptNode
    {
        /// <summary>
        /// Gets the inner script.
        /// </summary>
        /// <value>The inner script.</value>
        public virtual string InnerScript
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the outer script.
        /// </summary>
        /// <value>The outer script.</value>
        public virtual string OuterScript
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
