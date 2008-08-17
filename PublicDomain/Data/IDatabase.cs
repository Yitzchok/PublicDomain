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
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// <paramref name="columnIndex"/> is a 0-based
        /// column ordinal
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">0-based column ordinal</param>
        /// <returns></returns>
        Int16 GetInt16(IDataReader reader, int columnIndex);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// Gets the int16.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        Int16 GetInt16(IDataReader reader, string columnName);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// Gets the value as an Int32. <paramref name="columnIndex"/> is a 0-based
        /// column ordinal
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">0-based column ordinal</param>
        /// <returns></returns>
        Int32 GetInt32(IDataReader reader, int columnIndex);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// Gets the int32.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        Int32 GetInt32(IDataReader reader, string columnName);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// Gets the value as an Int64. <paramref name="columnIndex"/> is a 0-based
        /// column ordinal
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">0-based column ordinal</param>
        /// <returns></returns>
        Int64 GetInt64(IDataReader reader, int columnIndex);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// Gets the int64.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        Int64 GetInt64(IDataReader reader, string columnName);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// Gets the decimal. <paramref name="columnIndex"/> is a 0-based
        /// column ordinal
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns></returns>
        decimal GetDecimal(IDataReader reader, int columnIndex);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// Gets the decimal.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        decimal GetDecimal(IDataReader reader, string columnName);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// 0-based column index
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        bool GetBool(IDataReader reader, int columnIndex);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        bool GetBool(IDataReader reader, string columnName);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// 0-based column index
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        string GetString(IDataReader reader, int columnIndex);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        string GetString(IDataReader reader, string columnName);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// 0-based column index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        T GetEnum<T>(IDataReader reader, int columnIndex);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        T GetEnum<T>(IDataReader reader, string columnName);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// 0-based column index.
        /// If the caller knows that the date time stored in the database
        /// was stored in UTC format, then this will return the exact date time
        /// (without conversion from local to UTC), and set it's Kind property to
        /// UTC. The value taken from the database is not modified.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        TzDateTime GetUtcDateTime(IDataReader reader, int columnIndex);

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// If the caller knows that the date time stored in the database
        /// was stored in UTC format, then this will return the exact date time
        /// (without conversion from local to UTC), and set it's Kind property to
        /// UTC. The value taken from the database is not modified.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        TzDateTime GetUtcDateTime(IDataReader reader, string columnName);

        /// <summary>
        /// Doeses the table exist.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="openConnection"></param>
        /// <returns></returns>
        bool DoesTableExist(string tableName, IDbConnection openConnection);

        /// <summary>
        /// Gets the new max key.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="transactionScopeConnection">The transaction scope connection.</param>
        /// <returns></returns>
        Int32 GetNewMaxKeyInt32(string tableName, string columnName, IDbConnection transactionScopeConnection);

        /// <summary>
        /// Gets the new max key.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="transactionScopeConnection">The transaction scope connection.</param>
        /// <returns></returns>
        Int64 GetNewMaxKeyInt64(string tableName, string columnName, IDbConnection transactionScopeConnection);

        /// <summary>
        /// Deletes the rows.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="whereColumn">The where column.</param>
        /// <param name="whereValue">The where value.</param>
        /// <param name="openConnection"></param>
        /// <returns></returns>
        int DeleteRows(string tableName, string whereColumn, object whereValue, IDbConnection openConnection);

        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="openConnection"></param>
        /// <returns></returns>
        int GetRowCount(string tableName, IDbConnection openConnection);

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
        /// <param name="openConnection">The conn.</param>
        /// <returns></returns>
        DataSet ExecuteQueryDataSet(string query, IDbConnection openConnection);

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
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// <paramref name="columnIndex"/> is a 0-based column index
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">0-based column ordinal</param>
        /// <returns></returns>
        public virtual Int16 GetInt16(IDataReader reader, int columnIndex)
        {
            Int16 result = 0;
            object val = reader.GetValue(columnIndex);
            if (val != DBNull.Value)
            {
                result = (val is Int16) ? (Int16)val : Convert.ToInt16(val);
            }
            return result;
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual Int16 GetInt16(IDataReader reader, string columnName)
        {
            Int16 result = 0;
            object val = reader[columnName];
            if (val != DBNull.Value)
            {
                result = (val is Int16) ? (Int16)val : Convert.ToInt16(val);
            }
            return result;
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// Gets the value as an Int32. <paramref name="columnIndex"/> is a 0-based
        /// column ordinal
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">0-based column ordinal</param>
        /// <returns></returns>
        public virtual Int32 GetInt32(IDataReader reader, int columnIndex)
        {
            Int32 result = 0;
            object val = reader.GetValue(columnIndex);
            if (val != DBNull.Value)
            {
                result = (val is Int32) ? (Int32)val : Convert.ToInt32(val);
            }
            return result;
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// Gets the int32.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual Int32 GetInt32(IDataReader reader, string columnName)
        {
            Int32 result = 0;
            object val = reader[columnName];
            if (val != DBNull.Value)
            {
                result = (val is Int32) ? (Int32)val : Convert.ToInt32(val);
            }
            return result;
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// <paramref name="columnIndex"/> is a 0-based column index
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">0-based column ordinal</param>
        /// <returns></returns>
        public virtual Int64 GetInt64(IDataReader reader, int columnIndex)
        {
            Int64 result = 0;
            object val = reader.GetValue(columnIndex);
            if (val != DBNull.Value)
            {
                result = (val is Int64) ? (Int64)val : Convert.ToInt64(val);
            }
            return result;
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual Int64 GetInt64(IDataReader reader, string columnName)
        {
            Int64 result = 0;
            object val = reader[columnName];
            if (val != DBNull.Value)
            {
                result = (val is Int64) ? (Int64)val : Convert.ToInt64(val);
            }
            return result;
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// <paramref name="columnIndex"/> is a 0-based column index.
        /// If the value in the database is NULL, false is returned.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">0-based column ordinal</param>
        /// <returns></returns>
        public virtual bool GetBool(IDataReader reader, int columnIndex)
        {
            bool result = false;
            object val = reader.GetValue(columnIndex);
            if (val != DBNull.Value)
            {
                result = (val is bool) ? (bool)val : Convert.ToBoolean(val);
            }
            return result;
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual bool GetBool(IDataReader reader, string columnName)
        {
            bool result = false;
            object val = reader[columnName];
            if (val != DBNull.Value)
            {
                result = (val is bool) ? (bool)val : Convert.ToBoolean(val);
            }
            return result;
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// <paramref name="columnIndex"/> is a 0-based column index
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">0-based column ordinal</param>
        /// <returns></returns>
        public virtual T GetEnum<T>(IDataReader reader, int columnIndex)
        {
            int i = GetInt32(reader, columnIndex);
            return (T)Enum.ToObject(typeof(T), i);
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual T GetEnum<T>(IDataReader reader, string columnName)
        {
            int i = GetInt32(reader, columnName);
            return (T)Enum.ToObject(typeof(T), i);
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// <paramref name="columnIndex"/> is a 0-based column index
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">0-based column ordinal</param>
        /// <returns></returns>
        public virtual string GetString(IDataReader reader, int columnIndex)
        {
            string result = null;
            object val = reader.GetValue(columnIndex);
            if (val != DBNull.Value)
            {
                result = (val is string) ? (string)val : Convert.ToString(val);
            }
            return result;
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual string GetString(IDataReader reader, string columnName)
        {
            string result = null;
            object val = reader[columnName];
            if (val != DBNull.Value)
            {
                result = (val is string) ? (string)val : Convert.ToString(val);
            }
            return result;
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// <paramref name="columnIndex"/> is a 0-based column index
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">0-based column ordinal</param>
        /// <returns></returns>
        public virtual TzDateTime GetUtcDateTime(IDataReader reader, int columnIndex)
        {
            TzDateTime result = null;
            object val = reader.GetValue(columnIndex);
            if (val != DBNull.Value)
            {
                DateTime dt = val is DateTime ? (DateTime)val : Convert.ToDateTime(val);
                result = new TzDateTime(dt, true);
            }
            return result;
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual TzDateTime GetUtcDateTime(IDataReader reader, string columnName)
        {
            TzDateTime result = null;
            object o = reader[columnName];
            if (o != DBNull.Value)
            {
                DateTime dt = o is DateTime ? (DateTime)o : Convert.ToDateTime(o);
                result = new TzDateTime(dt, true);
            }
            return result;
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// Gets the decimal. <paramref name="columnIndex"/> is a 0-based
        /// column ordinal
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns></returns>
        public virtual decimal GetDecimal(IDataReader reader, int columnIndex)
        {
            decimal result = 0;
            object val = reader.GetValue(columnIndex);
            if (val != DBNull.Value)
            {
                result = (val is decimal) ? (decimal)val : Convert.ToDecimal(val);
            }
            return result;
        }

        /// <summary>
        /// Gets the specified value as the specified type. If the value
        /// in the database is NULL, no exception is thrown. Instead,
        /// the default value for this type (e.g. NULL for objects, false for Boolean,
        /// 0 for numerics, etc.) is returned. The caller should explicitly
        /// check for NULL on the IDataReader itself for this column to check
        /// NULLness.
        /// Gets the decimal.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual decimal GetDecimal(IDataReader reader, string columnName)
        {
            decimal result = 0;
            object val = reader[columnName];
            if (val != DBNull.Value)
            {
                result = (val is decimal) ? (decimal)val : Convert.ToDecimal(val);
            }
            return result;
        }

        /// <summary>
        /// Doeses the table exist.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="openConnection"></param>
        /// <returns></returns>
        public virtual bool DoesTableExist(string tableName, IDbConnection openConnection)
        {
            QueryBuilder qb = new QueryBuilder("select count(*) as cnt from information_schema.tables where ?table_name? = ???");
            qb.AddParameterStringInsensitiveStart();
            qb.AddParameterStringInsensitiveEnd();
            qb.AddParameterStringInsensitiveStart();
            qb.AddParameter(tableName);
            qb.AddParameterStringInsensitiveEnd();
            using (new DbConnectionScope())
            {
                using (IDataReader reader = ExecuteQueryReader(qb, openConnection))
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
        /// Gets the new max key.
        /// </summary>
        /// <param name="transactionScopeConnection">The transaction scope connection.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual Int32 GetNewMaxKeyInt32(string tableName, string columnName, IDbConnection transactionScopeConnection)
        {
            using (IDataReader reader = ExecuteQueryReader(string.Format("select max({0})+1 from {1}", columnName, tableName), transactionScopeConnection))
            {
                Int32 ret = 1;
                if (reader.Read() && !reader.IsDBNull(0))
                {
                    ret = GetInt32(reader, 0);
                }

                return ret;
            }
        }

        /// <summary>
        /// Gets the new max key.
        /// </summary>
        /// <param name="transactionScopeConnection">The transaction scope connection.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual Int64 GetNewMaxKeyInt64(string tableName, string columnName, IDbConnection transactionScopeConnection)
        {
            using (IDataReader reader = ExecuteQueryReader(string.Format("select max({0})+1 from {1}", columnName, tableName), transactionScopeConnection))
            {
                Int64 ret = 1;
                if (reader.Read() && !reader.IsDBNull(0))
                {
                    ret = GetInt64(reader, 0);
                }

                return ret;
            }
        }

        /// <summary>
        /// Deletes the rows.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="whereColumn">The where column.</param>
        /// <param name="whereValue">The where value.</param>
        /// <param name="openConnection">The conn.</param>
        /// <returns></returns>
        public virtual int DeleteRows(string tableName, string whereColumn, object whereValue, IDbConnection openConnection)
        {
            string sql = "delete from ? where ? = ?";

            QueryBuilder qb = new QueryBuilder(sql);

            qb.AddParameterString(tableName);
            qb.AddParameterString(whereColumn);
            qb.AddParameter(whereValue);

            return ExecuteNonQuery(qb, openConnection);
        }

        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="openConnection">The conn.</param>
        /// <returns></returns>
        public virtual int GetRowCount(string tableName, IDbConnection openConnection)
        {
            string sql = "select count(*) from " + tableName;

            using (IDataReader reader = ExecuteQueryReader(sql, openConnection))
            {
                if (reader.Read())
                {
                    return GetInt32(reader, 0);
                }
            }
            return -1;
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
