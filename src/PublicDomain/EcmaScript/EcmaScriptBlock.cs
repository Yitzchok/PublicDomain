using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.EcmaScript
{
    /// <summary>
    /// Represents code within {}
    /// </summary>
    public class EcmaScriptBlock : EcmaScriptStatement
    {
        private bool m_forcedNoBraces;

        /// <summary>
        /// Gets a value indicating whether [forced no braces].
        /// </summary>
        /// <value><c>true</c> if [forced no braces]; otherwise, <c>false</c>.</value>
        public bool ForcedNoBraces
        {
            get
            {
                return m_forcedNoBraces;
            }
            set
            {
                m_forcedNoBraces = value;
            }
        }
    }
}
