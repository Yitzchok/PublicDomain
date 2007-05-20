using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PublicDomain
{
    /// <summary>
    /// Generic Reader/Writer lock that can be used in a using() statement.
    /// </summary>
    public class DisposableReaderWriter : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableReaderWriter"/> class.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="type">The type.</param>
        public DisposableReaderWriter(IExposesReaderWriterLock root, ReaderWriterLockSynchronizeType type)
        {
            m_Root = root;
            m_SynchronizeType = type;

            AcquireLock();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            ReleaseLock();
        }

        /// <summary>
        /// Acquires the lock.
        /// </summary>
        protected virtual void AcquireLock()
        {
            m_Root.OnBeforeAcquire(m_SynchronizeType);
            switch (m_SynchronizeType)
            {
                case ReaderWriterLockSynchronizeType.Read:
                    AcquireReaderLock();
                    break;
                case ReaderWriterLockSynchronizeType.Write:
                    AcquireWriterLock();
                    break;
            }
        }

        /// <summary>
        /// Releases the lock.
        /// </summary>
        protected virtual void ReleaseLock()
        {
            DowngradeFromWriterLock();
            switch (m_SynchronizeType)
            {
                case ReaderWriterLockSynchronizeType.Read:
                    ReleaseReaderLock();
                    break;
                case ReaderWriterLockSynchronizeType.Write:
                    ReleaseWriterLock();
                    break;
            }
        }

        /// <summary>
        /// Acquires the reader lock.
        /// </summary>
        protected virtual void AcquireReaderLock()
        {
            m_Root.Sync.AcquireReaderLock(DefaultLockTimeout);
        }

        /// <summary>
        /// Acquires the writer lock.
        /// </summary>
        protected virtual void AcquireWriterLock()
        {
            m_Root.Sync.AcquireWriterLock(DefaultLockTimeout);
        }

        /// <summary>
        /// Releases the reader lock.
        /// </summary>
        protected virtual void ReleaseReaderLock()
        {
            m_Root.Sync.ReleaseReaderLock();
        }

        /// <summary>
        /// Releases the writer lock.
        /// </summary>
        protected virtual void ReleaseWriterLock()
        {
            m_Root.Sync.ReleaseWriterLock();
        }

        /// <summary>
        /// Upgrades to writer lock.
        /// </summary>
        public virtual void UpgradeToWriterLock()
        {
            m_SynchronizeType = ReaderWriterLockSynchronizeType.Write;
            m_UpgradeLockCookie = m_Root.Sync.UpgradeToWriterLock(DefaultLockTimeout);
        }

        /// <summary>
        /// Downgrades from writer lock.
        /// </summary>
        public virtual void DowngradeFromWriterLock()
        {
            if (m_UpgradeLockCookie != null)
            {
                m_SynchronizeType = ReaderWriterLockSynchronizeType.Read;
                LockCookie tempCookie = m_UpgradeLockCookie.Value;
                m_Root.Sync.DowngradeFromWriterLock(ref tempCookie);
                m_UpgradeLockCookie = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public const int DefaultLockTimeout = 100;

        /// <summary>
        /// 
        /// </summary>
        protected ReaderWriterLockSynchronizeType m_SynchronizeType;

        /// <summary>
        /// 
        /// </summary>
        protected IExposesReaderWriterLock m_Root;

        private LockCookie? m_UpgradeLockCookie;
    }
}
