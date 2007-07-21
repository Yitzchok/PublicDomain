using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// And IDisposable objects add to the Current property of this
    /// class within this scope of a LogicalDisposeScope will be disposed
    /// at the end of this or a containing scope (depending on combination logic).
    /// This ensures that Dispose is either manually called or, in the case of
    /// an exception, outside some logical context
    /// </summary>
    public class LogicalDisposeScope : LogicalTransactionScope<LogicalDisposeScope>
    {
        private List<IDisposable> m_disposables = new List<IDisposable>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalDisposeScope"/> class.
        /// </summary>
        public LogicalDisposeScope()
            : this(TransactionCombinationLogic.Combine)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalDisposeScope"/> class.
        /// </summary>
        /// <param name="combinationLogic">The combination logic.</param>
        public LogicalDisposeScope(TransactionCombinationLogic combinationLogic)
        {
            LogicalTransactionScope<LogicalDisposeScope>.Push(this, combinationLogic);
        }

        /// <summary>
        /// Adds the disposable.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        public void AddDisposable(IDisposable disposable)
        {
            Target.m_disposables.Add(disposable);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                base.Dispose();

                List<Exception> exceptions = null;
                foreach (IDisposable disposable in Target.m_disposables)
                {
                    try
                    {
                        disposable.Dispose();
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }

                ExceptionUtilities.ThrowExceptionList(exceptions);
            }
        }
    }
}
