using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// Unknown database type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// http://www.ibm.com/db2/
        /// </summary>
        Db2,

        /// <summary>
        /// http://www.mysql.com/
        /// </summary>
        MySql,

        /// <summary>
        /// 
        /// </summary>
        Odbc,

        /// <summary>
        /// 
        /// </summary>
        OleDb,

        /// <summary>
        /// http://www.oracle.com/database/
        /// </summary>
        Oracle,

        /// <summary>
        /// http://www.postgresql.org/
        /// </summary>
        PostgreSql,

        /// <summary>
        /// http://www.microsoft.com/sql/
        /// </summary>
        SqlServer,
    }
}
