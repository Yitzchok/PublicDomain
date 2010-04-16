using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.EcmaScript
{
    /// <summary>
    /// 
    /// </summary>
    public class EcmaScriptFunction : EcmaScriptSourceElement
    {
        private bool m_isAnonymous;

        /// <summary>
        /// Gets a value indicating whether this instance is anonymous.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is anonymous; otherwise, <c>false</c>.
        /// </value>
        public bool IsAnonymous
        {
            get
            {
                return m_isAnonymous;
            }
            set
            {
                m_isAnonymous = value;
            }
        }
    }
}
