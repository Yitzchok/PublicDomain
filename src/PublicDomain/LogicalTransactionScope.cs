using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LogicalTransactionScope<T> : IDisposable where T : LogicalTransactionScope<T>
    {
        [ThreadStatic]
        private static Stack<T> m_stack;

        private bool m_isDisposed;

        /// <summary>
        /// 
        /// </summary>
        protected T m_target;

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        public static T Current
        {
            get
            {
                return m_stack == null || m_stack.Count == 0 ? null : m_stack.Peek();
            }
        }

        /// <summary>
        /// Pushes the specified scope.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="combinationLogic">The combination logic.</param>
        public static void Push(T scope, TransactionCombinationLogic combinationLogic)
        {
            if (m_stack == null)
            {
                m_stack = new Stack<T>();
            }

            switch (combinationLogic)
            {
                case TransactionCombinationLogic.Combine:
                    if (Current == null)
                    {
                        m_stack.Push(scope);
                    }
                    scope.m_target = Current;
                    break;
                case TransactionCombinationLogic.RequiresNew:
                    scope.m_target = scope;
                    m_stack.Push(scope);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Pops this instance.
        /// </summary>
        /// <returns></returns>
        public static T Pop()
        {
            if (m_stack == null)
            {
                throw new InvalidOperationException();
            }
            return m_stack.Pop();
        }

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        protected T Target
        {
            get
            {
                return m_target;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        protected bool IsDisposed
        {
            get
            {
                return m_isDisposed;
            }
            set
            {
                m_isDisposed = value;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                // First remove ourselves from the stack
                if (Current == this)
                {
                    LogicalTransactionScope<T>.Pop();
                }
            }
        }
    }
}
