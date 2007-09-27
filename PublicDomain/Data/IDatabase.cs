using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using PublicDomain.Logging;

namespace PublicDomain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Gets the connection provider.
        /// </summary>
        /// <value>The connection provider.</value>
        IDatabaseConnectionProvider ConnectionProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DbProviderFactory DbProviderFactory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DatabaseType DatabaseType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="openConnection"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string query, IDbConnection openConnection);

        /// <summary>
        /// Executes the query reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IDataReader ExecuteQueryReader(string query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="openConnection"></param>
        /// <returns></returns>
        IDataReader ExecuteQueryReader(string query, IDbConnection openConnection);

        /// <summary>
        /// Sets the select command.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="openConnection">The open connection.</param>
        /// <returns></returns>
        DbCommand SetSelectCommand(DbDataAdapter adapter, string sql, IDbConnection openConnection);

        /// <summary>
        /// Gets the value as an Int32. <paramref name="columnIndex"/> is a zero-based
        /// column ordinal
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">zero-based column ordinal</param>
        /// <returns></returns>
        int GetInt32(IDataReader reader, int columnIndex);

        /// <summary>
        /// Gets the int32.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        int GetInt32(IDataReader reader, string columnName);

        /// <summary>
        /// Gets the decimal. <paramref name="columnIndex"/> is a zero-based
        /// column ordinal
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns></returns>
        decimal GetDecimal(IDataReader reader, int columnIndex);

        /// <summary>
        /// Gets the decimal.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        decimal GetDecimal(IDataReader reader, string columnName);

        /// <summary>
        /// Doeses the table exist.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        bool DoesTableExist(string tableName);

        /// <summary>
        /// Gets the new max key.
        /// </summary>
        /// <param name="transactionScopeConnection">The transaction scope connection.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        int GetNewMaxKey(IDbConnection transactionScopeConnection, string tableName, string columnName);

        /// <summary>
        /// Deletes the rows.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="whereColumn">The where column.</param>
        /// <param name="whereValue">The where value.</param>
        /// <returns></returns>
        int DeleteRows(IDbConnection conn, string tableName, string whereColumn, object whereValue);

        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        int GetRowCount(IDbConnection conn, string tableName);

        /// <summary>
        /// Executes the query data set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        DataSet ExecuteQueryDataSet(string query);

        /// <summary>
        /// Executes the query data set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="conn">The conn.</param>
        /// <returns></returns>
        DataSet ExecuteQueryDataSet(string query, IDbConnection conn);

        /// <summary>
        /// Supports the feature.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns></returns>
        bool SupportsFeature(DatabaseFeature feature);

        /// <summary>
        /// Tests the connection.
        /// </summary>
        void TestConnection();
    }

    /// <summary>
    /// 
    /// </summary>
    public class Database : IDatabase
    {
        internal static readonly Logger Log = LoggingConfig.Current.CreateLogger(typeof(Database), GlobalConstants.LogClassDatabase);

        private static IDatabase s_current = new Database();

        private DbProviderFactory m_providerFactory;
        private DatabaseType m_databaseType;
        private IDatabaseConnectionProvider m_connectionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class.
        /// </summary>
        public Database()
        {
            InitializeCurrent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class.
        /// </summary>
        /// <param name="databaseType">Type of the database.</param>
        public Database(string databaseType)
            : this((DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType, true))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class.
        /// </summary>
        /// <param name="databaseType">Type of the database.</param>
        public Database(DatabaseType databaseType)
            : this(databaseType, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class.
        /// </summary>
        /// <param name="databaseType">Type of the database.</param>
        /// <param name="connectionString">The connection string.</param>
        public Database(DatabaseType databaseType, string connectionString)
        {
            if (Log.Enabled) Log.Entry("Database", databaseType, connectionString);

            InitializeCurrent();

            DatabaseType = databaseType;
            DbProviderFactory = DatabaseNexus.GetDbProviderFactory(databaseType);
            if (!string.IsNullOrEmpty(connectionString))
            {
                ConnectionProvider = new DatabaseConnectionProvider(connectionString);
                ConnectionProvider.DbProviderFactory = DbProviderFactory;
            }

            if (Log.Enabled) Log.Exit("Database", DbProviderFactory, ConnectionProvider);
        }

        /// <summary>
        /// Initializes the current.
        /// </summary>
        protected virtual void InitializeCurrent()
        {
            if (Log.Enabled) Log.Entry("InitializeCurrent", this);

            s_current = this;

            if (Log.Enabled) Log.Exit("InitializeCurrent");
        }

        /// <summary>
        /// Gets the last instantiated database.
        /// </summary>
        /// <value>The current.</value>
        public static IDatabase Current
        {
            get
            {
                return s_current;
            }
            set
            {
                s_current = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <value></value>
        public virtual DbProviderFactory DbProviderFactory
        {
            get
            {
                return m_providerFactory;
            }
            set
            {
                m_providerFactory = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <value></value>
        public virtual DatabaseType DatabaseType
        {
            get
            {
                return m_databaseType;
            }
            set
            {
                m_databaseType = value;
            }
        }

        /// <summary>
        /// The base implementation of this method opens and disposes a connection internally.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(string query)
        {
            if (Log.Enabled) Log.Entry("ExecuteNonQuery", query);

            int result;

            using (IDbConnection conn = ConnectionProvider.GetConnection())
            {
                result = ExecuteNonQuery(query, conn);
            }

            if (Log.Enabled) Log.Exit("ExecuteNonQuery", result);

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="openConnection"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(string query, IDbConnection openConnection)
        {
            if (Log.Enabled) Log.Entry("ExecuteNonQuery", query, openConnection);

            IDbCommand cmd = openConnection.CreateCommand();
            cmd.CommandText = query;
            int result = cmd.ExecuteNonQuery();

            if (Log.Enabled) Log.Exit("ExecuteNonQuery", result);

            return result;
        }

        /// <summary>
        /// The base implementation of this method opens and disposes a connection internally.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteQueryReader(string query)
        {
            if (Log.Enabled) Log.Entry("ExecuteQueryReader", query);

            IDataReader result;

            using (IDbConnection conn = ConnectionProvider.GetConnection())
            {
                result = ExecuteQueryReader(query, conn);
            }

            if (Log.Enabled) Log.Exit("ExecuteQueryReader", result);

            return result;
        }

        /// <summary>
        /// If the database supports MultipleActiveResultSets (MARS), then opening nested readers
        /// is allowed, otherwise, see http://msdn2.microsoft.com/en-us/library/haa3afyz(VS.80).aspx
        /// "Note that while a DataReader is open, the Connection is in use exclusively
        /// by that DataReader. You cannot execute any commands for the Connection,
        /// including creating another DataReader, until the original DataReader
        /// is closed."
        /// </summary>
        /// <param name="query"></param>
        /// <param name="openConnection"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteQueryReader(string query, IDbConnection openConnection)
        {
            if (Log.Enabled) Log.Entry("ExecuteQueryReader", query, openConnection);

            IDbCommand cmd = openConnection.CreateCommand();
            cmd.CommandText = query;
            IDataReader result = cmd.ExecuteReader();

            if (Log.Enabled) Log.Exit("ExecuteQueryReader", result);

            return result;
        }

        /// <summary>
        /// Sets the select command.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="openConnection">The open connection.</param>
        /// <returns></returns>
        public virtual DbCommand SetSelectCommand(DbDataAdapter adapter, string sql, IDbConnection openConnection)
        {
            if (Log.Enabled) Log.Entry("SetSelectCommand", adapter, sql, openConnection);

            DbCommand command = DbProviderFactory.CreateCommand();
            command.Connection = (DbConnection)openConnection;
            command.CommandText = sql;
            ((DbDataAdapter)adapter).SelectCommand = command;

            if (Log.Enabled) Log.Exit("SetSelectCommand", command);

            return command;
        }

        /// <summary>
        /// Gets the value as an Int32. <paramref name="columnIndex"/> is a zero-based
        /// column ordinal
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">zero-based column ordinal</param>
        /// <returns></returns>
        public virtual int GetInt32(IDataReader reader, int columnIndex)
        {
            object val = reader.GetValue(columnIndex);
            return (val is int) ? (int)val : Convert.ToInt32(val);
        }

        /// <summary>
        /// Gets the int32.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual int GetInt32(IDataReader reader, string columnName)
        {
            object val = reader[columnName];
            return (val is int) ? (int)val : Convert.ToInt32(val);
        }

        /// <summary>
        /// Gets the decimal. <paramref name="columnIndex"/> is a zero-based
        /// column ordinal
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns></returns>
        public virtual decimal GetDecimal(IDataReader reader, int columnIndex)
        {
            object val = reader.GetValue(columnIndex);
            return (val is decimal) ? (decimal)val : Convert.ToDecimal(val);
        }

        /// <summary>
        /// Gets the decimal.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual decimal GetDecimal(IDataReader reader, string columnName)
        {
            object val = reader[columnName];
            return (val is decimal) ? (decimal)val : Convert.ToDecimal(val);
        }

        /// <summary>
        /// Doeses the table exist.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public virtual bool DoesTableExist(string tableName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the new max key.
        /// </summary>
        /// <param name="transactionScopeConnection">The transaction scope connection.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual int GetNewMaxKey(IDbConnection transactionScopeConnection, string tableName, string columnName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the rows.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="whereColumn">The where column.</param>
        /// <param name="whereValue">The where value.</param>
        /// <returns></returns>
        public virtual int DeleteRows(IDbConnection conn, string tableName, string whereColumn, object whereValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public virtual int GetRowCount(IDbConnection conn, string tableName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The base implementation of this method opens and disposes a connection internally.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public virtual DataSet ExecuteQueryDataSet(string query)
        {
            if (Log.Enabled) Log.Entry("ExecuteQueryDataSet", query);

            DataSet result;

            using (IDbConnection conn = ConnectionProvider.GetConnection())
            {
                result = ExecuteQueryDataSet(query, conn);
            }

            if (Log.Enabled) Log.Exit("ExecuteQueryDataSet", result);

            return result;
        }

        /// <summary>
        /// Executes the query data set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="conn">The conn.</param>
        /// <returns></returns>
        public virtual DataSet ExecuteQueryDataSet(string query, IDbConnection conn)
        {
            if (Log.Enabled) Log.Entry("ExecuteQueryDataSet", query, conn);

            DataSet result = new DataSet();

            using (DbDataAdapter adapter = DbProviderFactory.CreateDataAdapter())
            {
                using (SetSelectCommand(adapter, query, conn))
                {
                    adapter.Fill(result);
                }
            }

            if (Log.Enabled) Log.Exit("ExecuteQueryDataSet", result);

            return result;
        }

        /// <summary>
        /// Gets the connection provider.
        /// </summary>
        /// <value>The connection provider.</value>
        public virtual IDatabaseConnectionProvider ConnectionProvider
        {
            get
            {
                return m_connectionProvider;
            }
            set
            {
                m_connectionProvider = value;
                if (m_connectionProvider != null)
                {
                    m_connectionProvider.DatabaseType = DatabaseType;
                }
            }
        }

        /// <summary>
        /// Supports the feature.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns></returns>
        public virtual bool SupportsFeature(DatabaseFeature feature)
        {
            switch (DatabaseType)
            {
                case DatabaseType.MySql:
                    switch (feature)
                    {
                        case DatabaseFeature.NestedTransactions:
                        case DatabaseFeature.MultipleActiveResultSets:
                        case DatabaseFeature.MultipleOpenConnectionsWithinSingleTransaction:
                            return false;
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        public virtual void TestConnection()
        {
            using (ConnectionProvider.GetConnection())
            {
            }
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="server">The server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="otherParameters">The other parameters.</param>
        /// <returns></returns>
        public static IDatabase GetDatabase(DatabaseType type, string server, string databaseName, string userName, string password, params string[] otherParameters)
        {
            return GetDatabase(type, server, databaseName, userName, password, -1, otherParameters);
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="server">The server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="port">The port.</param>
        /// <param name="otherParameters">The other parameters.</param>
        /// <returns></returns>
        public static IDatabase GetDatabase(DatabaseType type, string server, string databaseName, string userName, string password, int port, params string[] otherParameters)
        {
            return GetDatabase(type, DatabaseNexus.GetConnectionString(type, server, databaseName, userName, password, port, otherParameters));
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static IDatabase GetDatabase(DatabaseType type, string connectionString)
        {
            return new Database(type, connectionString);
        }
    }
}
