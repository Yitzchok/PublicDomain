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
        /// Gets the new max key.
        /// </summary>
        /// <param name="transactionScopeConnection">The transaction scope connection.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public override Int32 GetNewMaxKeyInt32(string tableName, string columnName, IDbConnection transactionScopeConnection)
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
                        Int32 newMaxKey = GetNewMaxKeyInt32(tableName, columnName, DbConnectionScope.Current.Connection);

                        scope.Complete();

                        return newMaxKey;
                    }
                }
                else
                {
                    using (IDataReader reader = ExecuteQueryReader(string.Format("select max({0})+1 from {1}", columnName, tableName), transactionScopeConnection))
                    {
                        Int32 ret = 1;
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
        /// Gets the new max key.
        /// </summary>
        /// <param name="transactionScopeConnection">The transaction scope connection.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public override Int64 GetNewMaxKeyInt64(string tableName, string columnName, IDbConnection transactionScopeConnection)
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
                        Int64 newMaxKey = GetNewMaxKeyInt64(tableName, columnName, DbConnectionScope.Current.Connection);

                        scope.Complete();

                        return newMaxKey;
                    }
                }
                else
                {
                    using (IDataReader reader = ExecuteQueryReader(string.Format("select max({0})+1 from {1}", columnName, tableName), transactionScopeConnection))
                    {
                        Int64 ret = 1;
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            ret = Database.Current.GetInt64(reader, 0);
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
