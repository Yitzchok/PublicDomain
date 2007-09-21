using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using PublicDomain.Logging;

namespace PublicDomain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabaseConnectionProvider
    {
        /// <summary>
        /// Gets a database connection which is opened and there
        /// is no explicit cache bypassing.
        /// </summary>
        /// <returns></returns>
        IDbConnection GetConnection();

        /// <summary>
        /// Gets a database connection. There is no explicit cache bypassing.
        /// </summary>
        /// <param name="open">if set to <c>true</c> [open].</param>
        /// <returns></returns>
        IDbConnection GetConnection(bool open);

        /// <summary>
        /// Gets or sets the type of the database.
        /// </summary>
        /// <value>The type of the database.</value>
        DatabaseType DatabaseType { get; set; }

        /// <summary>
        /// Gets a database connection.
        /// </summary>
        /// <param name="open">if set to <c>true</c> [open].</param>
        /// <param name="bypassCache">if set to <c>true</c> [bypass cache].</param>
        /// <returns></returns>
        IDbConnection GetConnection(bool open, bool bypassCache);

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the db provider factory.
        /// </summary>
        /// <value>The db provider factory.</value>
        DbProviderFactory DbProviderFactory { get; set; }

        /// <summary>
        /// Removes the connection string parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        bool RemoveConnectionStringParameter(ConnectionParameterName parameterName);

        /// <summary>
        /// Removes the connection string parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        bool RemoveConnectionStringParameter(string parameterName);

        /// <summary>
        /// Determines whether [has connection string parameter] [the specified parameter name].
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// 	<c>true</c> if [has connection string parameter] [the specified parameter name]; otherwise, <c>false</c>.
        /// </returns>
        bool HasConnectionStringParameter(ConnectionParameterName parameterName);

        /// <summary>
        /// Determines whether [has connection string parameter] [the specified parameter name].
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// 	<c>true</c> if [has connection string parameter] [the specified parameter name]; otherwise, <c>false</c>.
        /// </returns>
        bool HasConnectionStringParameter(string parameterName);

        /// <summary>
        /// Adds the connection string parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        void AddConnectionStringParameter(ConnectionParameterName parameterName, string parameterValue);
        
        /// <summary>
        /// Adds the connection string parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        void AddConnectionStringParameter(string parameterName, string parameterValue);

        /// <summary>
        /// Sets the connection string parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        void SetConnectionStringParameter(ConnectionParameterName parameterName, string parameterValue);

        /// <summary>
        /// Sets the connection string parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        void SetConnectionStringParameter(string parameterName, string parameterValue);
    }

    /// <summary>
    /// 
    /// </summary>
    public class DatabaseConnectionProvider : IDatabaseConnectionProvider
    {
        internal static readonly Logger Log = LoggingConfig.Current.CreateLogger(typeof(DatabaseConnectionProvider), GlobalConstants.LogClassDatabase);

        private DbProviderFactory m_dbProviderFactory;
        private string m_connectionString;
        private DatabaseType m_databaseType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DatabaseConnectionProvider(string connectionString)
            : this(connectionString, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbProviderFactory">The db provider factory.</param>
        public DatabaseConnectionProvider(string connectionString, DbProviderFactory dbProviderFactory)
        {
            ConnectionString = connectionString;
            DbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// Gets a database connection which is unopened and there
        /// is no explicit cache bypassing.
        /// </summary>
        /// <returns></returns>
        public virtual IDbConnection GetConnection()
        {
            return GetConnection(true, false);
        }

        /// <summary>
        /// Gets a database connection. There is no explicit cache bypassing.
        /// </summary>
        /// <param name="open">if set to <c>true</c> [open].</param>
        /// <returns></returns>
        public virtual IDbConnection GetConnection(bool open)
        {
            return GetConnection(open, false);
        }

        /// <summary>
        /// Gets a database connection.
        /// </summary>
        /// <param name="open">if set to <c>true</c> [open].</param>
        /// <param name="bypassCache">if set to <c>true</c> [bypass cache].</param>
        /// <returns></returns>
        public virtual IDbConnection GetConnection(bool open, bool bypassCache)
        {
            if (Log.Enabled) Log.Entry("GetConnection", open, bypassCache, ConnectionString);

            IDbConnection result = m_dbProviderFactory.CreateConnection();
            result.ConnectionString = ConnectionString;
            if (open)
            {
                if (Log.Enabled) Log.LogDebug10("Opening database connection...");

                result.Open();
            }

            if (Log.Enabled) Log.Exit("GetConnection", result);

            return result;
        }

        /// <summary>
        /// Gets or sets the type of the database.
        /// </summary>
        /// <value>The type of the database.</value>
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
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public virtual string ConnectionString
        {
            get
            {
                return m_connectionString;
            }
            set
            {
                m_connectionString = value;
            }
        }

        /// <summary>
        /// Gets or sets the db provider factory.
        /// </summary>
        /// <value>The db provider factory.</value>
        public virtual DbProviderFactory DbProviderFactory
        {
            get
            {
                return m_dbProviderFactory;
            }
            set
            {
                m_dbProviderFactory = value;
            }
        }

        /// <summary>
        /// Removes the connection string parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        public virtual bool RemoveConnectionStringParameter(ConnectionParameterName parameterName)
        {
            return RemoveConnectionStringParameter(DatabaseNexus.GetConnectionStringParameter(DatabaseType, parameterName));
        }

        /// <summary>
        /// Removes the connection string parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        public virtual bool RemoveConnectionStringParameter(string parameterName)
        {
            string newVal;
            bool result = HasConnectionStringParameterExtract(parameterName, out newVal);
            if (result)
            {
                ConnectionString = newVal;
            }
            return result;
        }

        /// <summary>
        /// Determines whether [has connection string parameter] [the specified parameter name].
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// 	<c>true</c> if [has connection string parameter] [the specified parameter name]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool HasConnectionStringParameter(string parameterName)
        {
            string trash;
            return HasConnectionStringParameterExtract(parameterName, out trash);
        }

        private bool HasConnectionStringParameterExtract(string parameterName, out string newVal)
        {
            newVal = null;
            if (!string.IsNullOrEmpty(parameterName))
            {
                string divider = DatabaseNexus.GetConnectionStringParameterDivider(DatabaseType);
                if (divider != null && divider.Length == 1)
                {
                    string match = DatabaseNexus.GetConnectionStringPart(DatabaseType, parameterName, "[^" + divider + "]*");
                    if (match != null)
                    {
                        string str = ConnectionString;
                        if (!string.IsNullOrEmpty(str))
                        {
                            string newStr = Regex.Replace(str, match, "");
                            if (str.Length != newStr.Length)
                            {
                                newVal = newStr;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether [has connection string parameter] [the specified parameter name].
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// 	<c>true</c> if [has connection string parameter] [the specified parameter name]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool HasConnectionStringParameter(ConnectionParameterName parameterName)
        {
            return HasConnectionStringParameter(DatabaseNexus.GetConnectionStringParameter(DatabaseType, parameterName));
        }

        /// <summary>
        /// Adds the connection string parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        public virtual void AddConnectionStringParameter(ConnectionParameterName parameterName, string parameterValue)
        {
            AddConnectionStringParameter(DatabaseNexus.GetConnectionStringParameter(DatabaseType, parameterName), parameterValue);
        }

        /// <summary>
        /// Adds the connection string parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        public virtual void AddConnectionStringParameter(string parameterName, string parameterValue)
        {
            if (!string.IsNullOrEmpty(parameterName))
            {
                if (!HasConnectionStringParameter(parameterName))
                {
                    ConnectionString += DatabaseNexus.GetConnectionStringPart(DatabaseType, parameterName, parameterValue);
                }
            }
        }

        /// <summary>
        /// Sets the connection string parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        public virtual void SetConnectionStringParameter(ConnectionParameterName parameterName, string parameterValue)
        {
            SetConnectionStringParameter(DatabaseNexus.GetConnectionStringParameter(DatabaseType, parameterName), parameterValue);
        }

        /// <summary>
        /// Sets the connection string parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        public virtual void SetConnectionStringParameter(string parameterName, string parameterValue)
        {
            if (!string.IsNullOrEmpty(parameterName))
            {
                RemoveConnectionStringParameter(parameterName);
                ConnectionString += DatabaseNexus.GetConnectionStringPart(DatabaseType, parameterName, parameterValue);
            }
        }
    }
}
