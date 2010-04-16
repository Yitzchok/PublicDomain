using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public class DisposableLockGrabber : IDisposable
    {
        [ThreadStatic]
        private static DisposableLockGrabber s_lock;

        private ReaderWriterLock m_rwlock;
        private ReaderWriterLockSynchronizeType m_type;

        private DisposableLockGrabber()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableLockGrabber"/> class.
        /// </summary>
        /// <param name="rwlock">The rwlock.</param>
        /// <param name="type">The type.</param>
        public DisposableLockGrabber(ReaderWriterLock rwlock, ReaderWriterLockSynchronizeType type)
            : this(rwlock, type, -1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableLockGrabber"/> class.
        /// </summary>
        /// <param name="rwlock">The rwlock.</param>
        /// <param name="type">The type.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        public DisposableLockGrabber(ReaderWriterLock rwlock, ReaderWriterLockSynchronizeType type, int timeoutMilliseconds)
        {
            m_rwlock = rwlock;
            m_type = type;

            DoLock(timeoutMilliseconds);
        }

        private void DoLock(int timeoutMilliseconds)
        {
            if (m_type == ReaderWriterLockSynchronizeType.Read)
            {
                m_rwlock.AcquireReaderLock(timeoutMilliseconds);
            }
            else
            {
                m_rwlock.AcquireWriterLock(timeoutMilliseconds);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (m_type == ReaderWriterLockSynchronizeType.Read)
            {
                m_rwlock.ReleaseReaderLock();
            }
            else
            {
                m_rwlock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// This can only be used if there will be no other static calls to this method within the
        /// context of this call on this thread.
        /// </summary>
        /// <param name="rwlock">The rwlock.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static DisposableLockGrabber GetLock(ReaderWriterLock rwlock, ReaderWriterLockSynchronizeType type)
        {
            return GetLock(rwlock, type, -1);
        }

        /// <summary>
        /// This can only be used if there will be no other static calls to this method within the
        /// context of this call on this thread.
        /// </summary>
        /// <param name="rwlock">The rwlock.</param>
        /// <param name="type">The type.</param>
        /// <param name="timeoutMilliseconds">The timeout milliseconds.</param>
        /// <returns></returns>
        public static DisposableLockGrabber GetLock(ReaderWriterLock rwlock, ReaderWriterLockSynchronizeType type, int timeoutMilliseconds)
        {
            // This is because there is only one instance per thread
            if (s_lock == null)
            {
                s_lock = new DisposableLockGrabber();
            }

            s_lock.m_rwlock = rwlock;
            s_lock.m_type = type;

            s_lock.DoLock(timeoutMilliseconds);

            return s_lock;
        }
    }
}
