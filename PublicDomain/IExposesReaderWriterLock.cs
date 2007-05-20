using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PublicDomain
{
    /// <summary>
    /// Interface that represents an object that can be locked by a reader/writer lock.
    /// </summary>
    public interface IExposesReaderWriterLock
    {
        /// <summary>
        /// Gets the sync.
        /// </summary>
        /// <value>The sync.</value>
        ReaderWriterLock Sync { get; }

        /// <summary>
        /// Called when [before acquire].
        /// </summary>
        /// <param name="desiredType">Type of the desired.</param>
        void OnBeforeAcquire(ReaderWriterLockSynchronizeType desiredType);
    }

    /// <summary>
    /// The type of lock to acquire on a <see cref="PublicDomain.DisposableReaderWriter"/> lock.
    /// </summary>
    public enum ReaderWriterLockSynchronizeType
    {
        /// <summary>
        /// 
        /// </summary>
        Read,

        /// <summary>
        /// 
        /// </summary>
        Write
    }
}
