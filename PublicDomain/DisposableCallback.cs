using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DisposableCallback : IDisposable
    {
        private CallbackNoArgs m_callback1;
        private CallbackWithRock m_callback2;
        private object m_rock;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableCallback"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public DisposableCallback(CallbackNoArgs callback)
        {
            m_callback1 = callback;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableCallback"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public DisposableCallback(CallbackWithRock callback, object rock)
        {
            m_callback2 = callback;
            m_rock = rock;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (m_callback1 != null)
            {
                m_callback1();
            }
            else if (m_callback2 != null)
            {
                m_callback2(m_rock);
            }
        }
    }
}
