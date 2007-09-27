using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public enum DatabaseFeature
    {
        /// <summary>
        /// 
        /// </summary>
        MultipleActiveResultSets,

        /// <summary>
        /// 
        /// </summary>
        NestedTransactions,

        /// <summary>
        /// 
        /// </summary>
        MultipleOpenConnectionsWithinSingleTransaction
    }
}
