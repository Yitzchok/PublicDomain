using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;

namespace PublicDomain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionScopedDatabase : Database
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionScopedDatabase"/> class.
        /// </summary>
        /// <param name="databaseType">Type of the database.</param>
        /// <param name="connectionString">The connection string.</param>
        public ConnectionScopedDatabase(DatabaseType databaseType, string connectionString)
            : base(databaseType)
        {
            ConnectionProvider = new ConnectionScopedDatabaseConnectionProvider(connectionString);
            ConnectionProvider.DbProviderFactory = DbProviderFactory;
        }

        /// <summary>
        /// Doeses the table exist.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public override bool DoesTableExist(string tableName)
        {
            QueryBuilder qb = new QueryBuilder("select count(*) as cnt from information_schema.tables where ?table_name? = ???");
            qb.AddParameterStringInsensitiveStart();
            qb.AddParameterStringInsensitiveEnd();
            qb.AddParameterStringInsensitiveStart();
            qb.AddParameter(tableName);
            qb.AddParameterStringInsensitiveEnd();
            using (new DbConnectionScope())
            {
                using (IDataReader reader = ExecuteQueryReader(qb, DbConnectionScope.Current.Connection))
                {
                    if (reader.Read())
                    {
                        bool ret = GetInt32(reader, 0) > 0;

                        return ret;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Deletes the rows.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="whereColumn">The where column.</param>
        /// <param name="whereValue">The where value.</param>
        /// <returns></returns>
        public override int DeleteRows(IDbConnection conn, string tableName, string whereColumn, object whereValue)
        {
            string sql = "delete from ? where ? = ?";

            QueryBuilder qb = new QueryBuilder(sql);

            qb.AddParameterString(tableName);
            qb.AddParameterString(whereColumn);
            qb.AddParameter(whereValue);

            return ExecuteNonQuery(qb, conn);
        }

        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public override int GetRowCount(IDbConnection conn, string tableName)
        {
            string sql = "select count(*) from " + tableName;

            using (IDataReader reader = ExecuteQueryReader(sql, conn))
            {
                if (reader.Read())
                {
                    return GetInt32(reader, 0);
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the new max key.
        /// </summary>
        /// <param name="transactionScopeConnection">The transaction scope connection.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public override int GetNewMaxKey(System.Data.IDbConnection transactionScopeConnection, string tableName, string columnName)
        {
            // this will join in to any root transaction
            TransactionOptions serializableTransaction = new TransactionOptions();
            serializableTransaction.IsolationLevel = System.Transactions.IsolationLevel.Serializable;
            using (DbTransactionScope scope = new DbTransactionScope(TransactionScopeOption.Required, serializableTransaction))
            {
                if (transactionScopeConnection == null)
                {
                    using (new DbConnectionScope())
                    {
                        int newMaxKey = GetNewMaxKey(DbConnectionScope.Current.Connection, tableName, columnName);

                        scope.Complete();

                        return newMaxKey;
                    }
                }
                else
                {
                    using (IDataReader reader = ExecuteQueryReader(string.Format("select max({0})+1 from {1}", columnName, tableName), transactionScopeConnection))
                    {
                        int ret = 1;
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            ret = Database.Current.GetInt32(reader, 0);
                        }
                        scope.Complete();

                        return ret;
                    }
                }
            }
        }

        /// <summary>
        /// The base implementation of this method opens and disposes a connection internally.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override int ExecuteNonQuery(string query)
        {
            return ExecuteNonQuery(query, ConnectionProvider.GetConnection());
        }

        /// <summary>
        /// The base implementation of this method opens and disposes a connection internally.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public override DataSet ExecuteQueryDataSet(string query)
        {
            return ExecuteQueryDataSet(query, ConnectionProvider.GetConnection());
        }

        /// <summary>
        /// The base implementation of this method opens and disposes a connection internally.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IDataReader ExecuteQueryReader(string query)
        {
            return ExecuteQueryReader(query, ConnectionProvider.GetConnection());
        }
    }
}
