using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Code.CodeCount
{
    /// <summary>
    /// Counts a stream depth first
    /// </summary>
    public class DepthFirstCountStream : CountStream
    {
        private Stack<Pair<ICountable, int>> m_stack = new Stack<Pair<ICountable, int>>();

        /// <summary>
        /// 
        /// </summary>
        protected ICountable m_current;

        /// <summary>
        /// 
        /// </summary>
        protected int m_currentChildIndex;

        /// <summary>
        /// 
        /// </summary>
        protected bool m_finished;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstCountStream"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="location">The location.</param>
        public DepthFirstCountStream(CountStreamType type, string location)
            : base(type, location)
        {
        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        public override ICountable Read()
        {
            if (m_finished)
            {
                return null;
            }

            // If current is NULL, then this is the first ICountable to read
            if (m_current == null)
            {
                m_current = m_first;
                m_currentChildIndex = -1;
                m_stack.Push(new Pair<ICountable, int>(m_current, 0));
            }

            // If the current index is -1, we return the current item and
            // increment the current child index
            if (m_currentChildIndex == -1)
            {
                m_currentChildIndex++;
                return m_current;
            }

            // Check if we have read all children, recursively
            while (m_current.Children.Count == 0 || m_current.Children.Count == m_currentChildIndex)
            {
                Pair<ICountable, int> last = m_stack.Pop();
                m_current = last.First;
                m_currentChildIndex = last.Second;

                if (m_current == m_first && m_current.Children.Count == m_currentChildIndex)
                {
                    m_finished = true;
                    return null;
                }
            }

            ICountable result = m_current.Children[m_currentChildIndex];

            m_stack.Push(new Pair<ICountable, int>(m_current, m_currentChildIndex + 1));
            m_currentChildIndex = 0;
            m_current = result;

            return result;
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public override void Cancel()
        {
            m_finished = true;
        }
    }
}
