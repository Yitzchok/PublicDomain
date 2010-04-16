using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Logging
{
    /// <summary>
    /// Severity of the log entry. The numeric value of the severity
    /// is in the name itself for immediate feedback.
    /// </summary>
    public enum LoggerSeverity
    {
        /// <summary>
        /// Lowest severity.
        /// </summary>
        None0 = 0,

        /// <summary>
        /// Detailed programmatic informational messages used
        /// as an aid in troubleshooting problems by programmers.
        /// </summary>
        Debug10 = 10,

        /// <summary>
        /// Brief informative messages to use as an aid in
        /// troubleshooting problems by production support and programmers.
        /// </summary>
        Info20 = 20,

        /// <summary>
        /// Messages intended to notify help desk, production support and programmers
        /// of possible issues with respect to the running application.
        /// </summary>
        Warn30 = 30,

        /// <summary>
        /// Messages that detail a programmatic error, these are typically messages
        /// intended for help desk, production support, programmers and occasionally users.
        /// </summary>
        Error40 = 40,

        /// <summary>
        /// Severe messages that are programmatic violations that will usually
        /// result in application failure. These messages are intended for help
        /// desk, production support, programmers and possibly users.
        /// </summary>
        Fatal50 = 50,

        /// <summary>
        /// 
        /// </summary>
        Infinity = int.MaxValue
    }
}
