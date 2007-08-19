using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class LoggerBackgroundThread : FinalizableBackgroundThread
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerBackgroundThread"/> class.
        /// </summary>
        /// <param name="intervalMs">The interval in milliseconds.</param>
        /// <param name="exec">The exec.</param>
        public LoggerBackgroundThread(int intervalMs, CallbackBackgroundThread exec)
            : base(intervalMs, exec)
        {
        }

        /// <summary>
        /// Handles the execution exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        protected override void HandleExecutionException(Exception ex)
        {
            base.HandleExecutionException(ex);
        }
    }
}
