using System;
using System.Collections.Generic;
using System.Text;
using System.Security;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public class RevertAsserts : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public static RevertAsserts Instance = new RevertAsserts();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            CodeAccessPermission.RevertAssert();
        }
    }
}
