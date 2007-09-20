using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Reflection;

namespace PublicDomain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class DatabaseNexus
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultDbProviderFactoryAssemblyDb2 = "IBM.Data.DB2";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultDbProviderFactoryAssemblyMySql = "MySql.Data, Version=5.1.3.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d, processorArchitecture=MSIL";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultDbProviderFactoryAssemblyOracle = "System.Data.OracleClient";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultDbProviderFactoryAssemblyPostgreSql = "Npgsql, Version=1.97.1.0, Culture=neutral, PublicKeyToken=5d8b90d52f46fda7, processorArchitecture=MSIL";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultDbProviderFactoryTypeDb2 = "IBM.Data.DB2.DB2Factory";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultDbProviderFactoryTypeMySql = "MySql.Data.MySqlClient.MySqlClientFactory";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultDbProviderFactoryTypeOracle = "System.Data.OracleClient.OracleClientFactory";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultDbProviderFactoryTypePostgreSql = "Npgsql.NpgsqlFactory";

        /// <summary>
        /// Gets the db provider factory.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static DbProviderFactory GetDbProviderFactory(DatabaseType type)
        {
            return GetDbProviderFactory(type, null);
        }

        /// <summary>
        /// Gets the db provider factory.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static DbProviderFactory GetDbProviderFactory(DatabaseType type, Version version)
        {
            switch (type)
            {
                case DatabaseType.Odbc:
                    return System.Data.Odbc.OdbcFactory.Instance;
                case DatabaseType.OleDb:
                    return System.Data.OleDb.OleDbFactory.Instance;
                case DatabaseType.SqlServer:
                    return System.Data.SqlClient.SqlClientFactory.Instance;
                default:
                    return GetDbProviderFactory(GetDefaultDbProviderFactoryAssemblyName(type, version), GetDefaultDbProviderFactoryTypeName(type));
            }
        }

        /// <summary>
        /// Gets the name of the default db provider factory assembly.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetDefaultDbProviderFactoryAssemblyName(DatabaseType type)
        {
            return GetDefaultDbProviderFactoryAssemblyName(type, null);
        }

        /// <summary>
        /// Gets the name of the default db provider factory assembly.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static string GetDefaultDbProviderFactoryAssemblyName(DatabaseType type, Version version)
        {
            string result = null;
            switch (type)
            {
                case DatabaseType.Db2:
                    if (version == null)
                    {
                        result = DefaultDbProviderFactoryAssemblyDb2;
                    }
                    else
                    {
                        ThrowUnknownVersionException(type, version);
                    }
                    break;
                case DatabaseType.MySql:
                    if (version == null)
                    {
                        result = DefaultDbProviderFactoryAssemblyMySql;
                    }
                    else
                    {
                        ThrowUnknownVersionException(type, version);
                    }
                    break;
                case DatabaseType.Oracle:
                    if (version == null)
                    {
                        result = DefaultDbProviderFactoryAssemblyOracle;
                    }
                    else
                    {
                        ThrowUnknownVersionException(type, version);
                    }
                    break;
                case DatabaseType.PostgreSql:
                    if (version == null)
                    {
                        result = DefaultDbProviderFactoryAssemblyPostgreSql;
                    }
                    else
                    {
                        ThrowUnknownVersionException(type, version);
                    }
                    break;
                case DatabaseType.Odbc:
                    result = typeof(System.Data.Odbc.OdbcFactory).Assembly.FullName;
                    break;
                case DatabaseType.OleDb:
                    result = typeof(System.Data.OleDb.OleDbFactory).Assembly.FullName;
                    break;
                case DatabaseType.SqlServer:
                    result = typeof(System.Data.SqlClient.SqlClientFactory).Assembly.FullName;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return result;
        }

        private static void ThrowUnknownVersionException(DatabaseType type, Version version)
        {
            throw new NotSupportedException(string.Format("Unknown version {0} for {1}. Please use the GetDbProviderFactory method with the explicit assembly name.", version, type));
        }

        /// <summary>
        /// Gets the name of the default DbProviderFactory type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetDefaultDbProviderFactoryTypeName(DatabaseType type)
        {
            switch (type)
            {
                case DatabaseType.Db2:
                    return DefaultDbProviderFactoryTypeDb2;
                case DatabaseType.MySql:
                    return DefaultDbProviderFactoryTypeMySql;
                case DatabaseType.Oracle:
                    return DefaultDbProviderFactoryTypeOracle;
                case DatabaseType.PostgreSql:
                    return DefaultDbProviderFactoryTypePostgreSql;
                case DatabaseType.Odbc:
                    return typeof(System.Data.Odbc.OdbcFactory).FullName;
                case DatabaseType.OleDb:
                    return typeof(System.Data.OleDb.OleDbFactory).FullName;
                case DatabaseType.SqlServer:
                    return typeof(System.Data.SqlClient.SqlClientFactory).FullName;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the db provider factory.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static DbProviderFactory GetDbProviderFactory(string assemblyName, string typeName)
        {
            return new DynamicDbProviderFactory(assemblyName, typeName);
        }

        /// <summary>
        /// Gets the connection string parameter.
        /// </summary>
        /// <param name="databaseType">Type of the database.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        public static string GetConnectionStringParameter(DatabaseType databaseType, ConnectionParameterName parameterName)
        {
            return GetConnectionStringParameter(databaseType, parameterName.ToString());
        }

        /// <summary>
        /// Gets the connection string parameter.
        /// </summary>
        /// <param name="databaseType">Type of the database.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        public static string GetConnectionStringParameter(DatabaseType databaseType, string parameterName)
        {
            switch (parameterName)
            {
                case "Database":
                    switch (databaseType)
                    {
                        case DatabaseType.PostgreSql:
                            // no database parameter should be specified
                            return null;
                    }
                    break;
                case "User":
                    switch (databaseType)
                    {
                        case DatabaseType.SqlServer:
                            return "User ID";
                        case DatabaseType.MySql:
                            return "UID";
                        case DatabaseType.PostgreSql:
                            return "Userid";
                    }
                    break;
            }
            return parameterName;
        }

        /// <summary>
        /// Gets the connection string parameter value separator. Default: '='
        /// </summary>
        /// <param name="databaseType">Type of the database.</param>
        /// <returns></returns>
        public static string GetConnectionStringParameterValueSeparator(DatabaseType databaseType)
        {
            return "=";
        }

        /// <summary>
        /// Gets the connection string parameter divider. Default: ';'
        /// </summary>
        /// <param name="databaseType">Type of the database.</param>
        /// <returns></returns>
        public static string GetConnectionStringParameterDivider(DatabaseType databaseType)
        {
            return ";";
        }

        /// <summary>
        /// Parses the type of the database.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static DatabaseType ParseDatabaseType(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim();
                string[] names = Enum.GetNames(typeof(DatabaseType));
                foreach (string name in names)
                {
                    if (name.Equals(str, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return (DatabaseType)Enum.Parse(typeof(DatabaseType), name);
                    }
                }
            }
            return DatabaseType.Unknown;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="server">The server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="otherParameters">The other parameters.</param>
        /// <returns></returns>
        public static string GetConnectionString(DatabaseType type, string server, string databaseName, string userName, string password, params string[] otherParameters)
        {
            return GetConnectionString(type, server, databaseName, userName, password, -1, otherParameters);
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="server">The server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="port">The port.</param>
        /// <param name="otherParameters">The other parameters.</param>
        /// <returns></returns>
        public static string GetConnectionString(DatabaseType type, string server, string databaseName, string userName, string password, int port, params string[] otherParameters)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(server))
            {
                server = "localhost";
            }

            string separator = GetConnectionStringParameterValueSeparator(type);
            string divider = GetConnectionStringParameterDivider(type);

            if (type == DatabaseType.SqlServer && server.ToLower() == "localhost")
            {
                server = ".";
            }

            GetConnectionStringPart(type, sb, separator, divider, ConnectionParameterName.Server, server);
            GetConnectionStringPart(type, sb, separator, divider, ConnectionParameterName.Database, databaseName);

            if (type == DatabaseType.SqlServer && string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password))
            {
                AddConnectionStringPart(type, sb, separator, divider, "Integrated Security", "sspi");
            }
            else
            {
                GetConnectionStringPart(type, sb, separator, divider, ConnectionParameterName.User, userName);
                GetConnectionStringPart(type, sb, separator, divider, ConnectionParameterName.Password, password);
            }

            if (port != -1)
            {
                GetConnectionStringPart(type, sb, separator, divider, ConnectionParameterName.Port, port.ToString());
            }

            if (otherParameters.Length > 0)
            {
                if ((otherParameters.Length % 2) != 0)
                {
                    throw new ArgumentException("The otherParameters array must be of even size containing pairs of keys and values");
                }

                for (int i = 0; i < otherParameters.Length; i += 2)
                {
                    AddConnectionStringPart(type, sb, separator, divider, otherParameters[i], otherParameters[i + 1]);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the connection string part.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <returns></returns>
        public static string GetConnectionStringPart(DatabaseType type, string parameterName, string parameterValue)
        {
            string result = null;
            if (!string.IsNullOrEmpty(parameterName))
            {
                result = parameterName + GetConnectionStringParameterValueSeparator(type) + parameterValue + GetConnectionStringParameterDivider(type);
            }
            return result;
        }

        /// <summary>
        /// Gets the connection string part.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <returns></returns>
        public static string GetConnectionStringPart(DatabaseType type, ConnectionParameterName parameterName, string parameterValue)
        {
            string name = GetConnectionStringParameter(type, parameterName);
            return GetConnectionStringPart(type, name, parameterValue);
        }

        private static void GetConnectionStringPart(DatabaseType type, StringBuilder sb, string separator, string divider, ConnectionParameterName parameterName, string parameterValue)
        {
            AddConnectionStringPart(type, sb, separator, divider, GetConnectionStringParameter(type, parameterName), parameterValue);
        }

        private static void AddConnectionStringPart(DatabaseType type, StringBuilder sb, string separator, string divider, string parameterName, string parameterValue)
        {
            if (!string.IsNullOrEmpty(parameterName))
            {
                sb.Append(parameterName);
                sb.Append(separator);
                sb.Append(parameterValue);
                sb.Append(divider);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ConnectionParameterName
    {
        /// <summary>
        /// 
        /// </summary>
        Server,

        /// <summary>
        /// 
        /// </summary>
        Database,

        /// <summary>
        /// 
        /// </summary>
        User,

        /// <summary>
        /// 
        /// </summary>
        Password,

        /// <summary>
        /// 
        /// </summary>
        Port
    }
}
