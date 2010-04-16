using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.EcmaScript
{
    /// <summary>
    /// http://www.ecma-international.org/publications/files/ECMA-ST/Ecma-262.pdf
    /// </summary>
    public class LenientEcmaScriptDocument
    {
        /// <summary>
        /// 
        /// </summary>
        protected StringBuilder m_sb;

        /// <summary>
        /// 
        /// </summary>
        protected State m_state;

        /// <summary>
        /// 
        /// </summary>
        protected bool m_isAllWhitespace;

        /// <summary>
        /// Loads the specified script.
        /// </summary>
        /// <param name="script">The script.</param>
        public virtual void Load(string script)
        {
            if (script != null)
            {
                int l = script.Length;
                char c;
                m_sb = new StringBuilder(100);
                m_state = State.None;
                m_isAllWhitespace = true;

                for (int i = 0; i < l; i++)
                {
                    c = script[i];

                    switch (m_state)
                    {
                        default:

                            if (c == '{')
                            {
                            }

                            if (m_isAllWhitespace && !IsEcmaScriptWhitespace(c))
                            {
                                m_isAllWhitespace = false;
                            }

                            m_sb.Append(c);
                            continue;
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether [is ECMA script whitespace] [the specified c].
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>
        /// 	<c>true</c> if [is ECMA script whitespace] [the specified c]; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsEcmaScriptWhitespace(char c)
        {
            return Char.IsWhiteSpace(c);
        }

        /// <summary>
        /// Contexts the switch.
        /// </summary>
        /// <param name="newState">The new state.</param>
        protected virtual void ContextSwitch(State newState)
        {
            // Process the old state
            if (m_sb.Length > 0)
            {
                string token = m_sb.ToString();

                // this is the LAST state
                switch (m_state)
                {
                    default:
                        break;
                }
            }

            // now check the NEW state
            switch (newState)
            {
                default:
                    break;
            }

            ResetAfterContextSwitch();

            m_state = newState;
        }

        /// <summary>
        /// Resets the after context switch.
        /// </summary>
        protected void ResetAfterContextSwitch()
        {
            m_sb.Length = 0;
            m_isAllWhitespace = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected enum State
        {
            /// <summary>
            /// 
            /// </summary>
            None
        }
    }
}
