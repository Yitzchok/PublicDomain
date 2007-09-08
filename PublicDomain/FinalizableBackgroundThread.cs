using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.ConstrainedExecution;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="isFinal"></param>
    public delegate void CallbackBackgroundThread(bool isFinal);

    /// <summary>
    /// 
    /// </summary>
    public class FinalizableBackgroundThread : CriticalFinalizerObject
    {
        private Thread m_thread;
        private int m_intervalMs;
        private CallbackBackgroundThread m_exec;
        private bool m_isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FinalizableBackgroundThread"/> class.
        /// </summary>
        /// <param name="intervalMs">The interval in milliseconds.</param>
        /// <param name="exec">The exec.</param>
        public FinalizableBackgroundThread(int intervalMs, CallbackBackgroundThread exec)
        {
            Initialize(intervalMs, exec);
        }

        /// <summary>
        /// Initializes the specified interval ms.
        /// </summary>
        /// <param name="intervalMs">The interval ms.</param>
        /// <param name="exec"></param>
        protected virtual void Initialize(int intervalMs, CallbackBackgroundThread exec)
        {
            m_intervalMs = intervalMs;
            m_exec = exec;

            if (m_exec == null)
            {
                throw new ArgumentNullException("exec");
            }
            if (intervalMs <= 0)
            {
                throw new ArgumentException("intervalMs");
            }

            m_thread = new Thread(Run);
            m_thread.IsBackground = true;
            m_thread.Name = typeof(FinalizableBackgroundThread).Name;
            m_thread.Priority = ThreadPriority.BelowNormal;
            m_thread.Start();
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        protected virtual void Run()
        {
            while (true)
            {
                Thread.Sleep(m_intervalMs);

                try
                {
                    Execute(false);
                }
                catch (Exception ex)
                {
                    HandleExecutionException(ex);
                }
            }
        }

        /// <summary>
        /// Handles the execution exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        protected virtual void HandleExecutionException(Exception ex)
        {
            Console.WriteLine(ExceptionUtilities.GetExceptionDetailsAsString(ex));
        }

        /// <summary>
        /// Executes the specified is final.
        /// </summary>
        /// <param name="isFinal">if set to <c>true</c> [is final].</param>
        public virtual void Execute(bool isFinal)
        {
            m_exec(isFinal);
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="FinalizableBackgroundThread"/> is reclaimed by garbage collection.
        /// </summary>
        ~FinalizableBackgroundThread()
        {
            DoFinalize();
        }

        /// <summary>
        /// Does the finalize.
        /// </summary>
        protected virtual void DoFinalize()
        {
            if (!m_isDisposed)
            {
                m_isDisposed = true;

                // do the final execute
                try
                {
                    Execute(true);
                }
                catch (Exception ex)
                {
                    HandleExecutionException(ex);
                }
            }
        }
    }
}
