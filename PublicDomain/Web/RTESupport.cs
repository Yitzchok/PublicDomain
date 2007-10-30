using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Web
{
    /// <summary>
    /// Rich text editor support level
    /// </summary>
    public enum RTESupport
    {
        /// <summary>
        /// Unknown whether or how this clients supports an RTE
        /// </summary>
        None = 0,

        /// <summary>
        /// Logically equivalent to Internet Explorer and those
        /// that supports IE's model
        /// </summary>
        ContentEditable = 1,

        /// <summary>
        /// Logically equivalent to Firefox and those that
        /// support its model
        /// </summary>
        IFrameExec = 2
    }
}
