using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.Logging;
using System.Data;

namespace PublicDomain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionScopedDatabaseConnectionProvider : DatabaseConnectionProvider
    {
        internal static new readonly Logger Log = LoggingConfig.Current.CreateLogger(typeof(ConnectionScopedDatabaseConnectionProvider), GlobalConstants.LogClassDatabase);

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionScopedDatabaseConnectionProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ConnectionScopedDatabaseConnectionProvider(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Gets a database connection.
        /// </summary>
        /// <param name="open">if set to <c>true</c> [open].</param>
        /// <param name="bypassCache">if set to <c>true</c> [bypass cache].</param>
        /// <returns></returns>
        public override IDbConnection GetConnection(bool open, bool bypassCache)
        {
            IDbConnection result = null;

            if (Log.Enabled) Log.Entry("GetConnection", open, bypassCache);
            
            if (bypassCache)
            {
                if (Log.Enabled) Log.LogDebug10("cache bypased, requesting connection");

                result = base.GetConnection(open, bypassCache);
            }
            else
            {
                if (Log.Enabled) Log.LogDebug10("checking cache for inheritable connection");

                result = DbConnectionScope.Current.Connection;
            }

            if (Log.Enabled) Log.Exit("GetConnection", result);

            return result;
        }
    }
}
