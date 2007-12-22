using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace PublicDomain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class SQLHelper
    {
        /// <summary>
        /// Normalizes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string Normalize(DatabaseType type, string str)
        {
            switch (type)
            {
                case DatabaseType.SqlServer:
                    str = str.Replace("\'", "\'\'");
                    str = str.Replace("´", "\'´");
                    str = str.Replace("’", "\'’");
                    break;
                default:
                    str = str.Replace("\\", "\\\\");
                    str = str.Replace("\'", "\\\'");
                    str = str.Replace("\"", "\\\"");
                    str = str.Replace("`", "\\`");
                    str = str.Replace("´", "\\´");
                    str = str.Replace("’", "\\’");
                    str = str.Replace("‘", "\\‘");
                    str = str.Replace("\x1a", "\\\x1a");
                    break;
            }
            return str;
        }

        /// <summary>
        /// Objects to db string.
        /// </summary>
        /// <param name="databaseType">Type of the database.</param>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static string ObjectToDbString(DatabaseType databaseType, object val)
        {
            if (val is TzDateTime)
            {
                if (((TzDateTime)val).DateTimeUtc.TimeOfDay == new TimeSpan(0))
                {
                    return ObjectToDbString(databaseType, val, DbType.Date);
                }
                else
                {
                    return ObjectToDbString(databaseType, val, DbType.DateTime);
                }
            }
            else if (val is DateTime)
            {
                if (((DateTime)val).TimeOfDay == new TimeSpan(0))
                {
                    return ObjectToDbString(databaseType, new TzDateTime((DateTime)val), DbType.Date);
                }
                else
                {
                    return ObjectToDbString(databaseType, new TzDateTime((DateTime)val), DbType.DateTime);
                }
            }
            else if (val is bool)
            {
                return ObjectToDbString(databaseType, val, DbType.Boolean);
            }
            else if (val is string)
            {
                return ObjectToDbString(databaseType, val, DbType.String);
            }
            else if (val is char)
            {
                return ObjectToDbString(databaseType, val, DbType.String);
            }
            else if (val is byte[])
            {
                return ObjectToDbString(databaseType, val, DbType.Binary);
            }
            else if (val is IEnumerable<int>)
            {
                string ret = null;
                IEnumerable<int> ival = (IEnumerable<int>)val;
                foreach (int i in ival)
                {
                    if (ret != null)
                    {
                        ret += ",";
                    }
                    ret += i;
                }
                return ret;
            }
            else if (val is int[])
            {
                string ret = null;
                foreach (int i in (int[])val)
                {
                    if (ret != null)
                    {
                        ret += ",";
                    }
                    ret += i;
                }
                return ret;
            }
            else
            {
                return ObjectToDbString(databaseType, val, DbType.Object);
            }
        }

        /// <summary>
        /// Objects to db string.
        /// </summary>
        /// <param name="databaseType">Type of the database.</param>
        /// <param name="val">The val.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string ObjectToDbString(DatabaseType databaseType, object val, DbType type)
        {
            string valStr = "NULL";
            if (val != null && val != DBNull.Value)
            {
                valStr = "";
                switch (type)
                {
                    case DbType.String:
                    case DbType.StringFixedLength:
                    case DbType.AnsiString:
                    case DbType.AnsiStringFixedLength:
                        if (databaseType == DatabaseType.SqlServer)
                        {
                            // TODO only if unicode?
                            valStr = "N";
                        }
                        valStr += "'" + Normalize(databaseType, (string)val) + "'";
                        break;
                    case DbType.Date:
                        valStr += ConvertDateTimeToDbDateString(databaseType, (TzDateTime)val);
                        break;
                    case DbType.Time:
                        valStr += ConvertDateTimeToDbTimeString(databaseType, (TzDateTime)val);
                        break;
                    case DbType.DateTime:
                        valStr += "'";
                        valStr += ConvertDateTimeToDbDateString(databaseType, (TzDateTime)val, false);
                        valStr += " ";
                        valStr += ConvertDateTimeToDbTimeString(databaseType, (TzDateTime)val, false);
                        valStr += "'";
                        break;
                    case DbType.Boolean:
                        bool boolVal = (bool)val;
                        if (boolVal)
                        {
                            valStr += "1";
                        }
                        else
                        {
                            valStr += "0";
                        }
                        break;
                    case DbType.Binary:
                        byte[] data = (byte[])val;
                        int length = data.Length;
                        StringBuilder sb = new StringBuilder(data.Length * 2);
                        sb.Append("0x");
                        foreach (byte datum in data)
                        {
                            if (datum <= 0xF)
                            {
                                sb.Append('0');
                            }
                            sb.Append(datum.ToString("X"));
                        }
                        valStr += sb.ToString();
                        break;
                    default:
                        using (new InvariantCultureContext())
                        {
                            valStr = val.ToString();
                        }
                        break;
                }
            }
            return valStr;
        }

        /// <summary>
        /// Concats the function.
        /// </summary>
        /// <param name="targetDB">The target DB.</param>
        /// <param name="skipFirst">if set to <c>true</c> [skip first].</param>
        /// <param name="toConcat">To concat.</param>
        /// <returns></returns>
        public static string ConcatFunction(DatabaseType targetDB, bool skipFirst, params object[] toConcat)
        {
            StringBuilder builder = new StringBuilder(16);
            switch (targetDB)
            {
                case DatabaseType.MySql:
                    builder.Append("CONCAT(");
                    break;
                case DatabaseType.SqlServer:
                case DatabaseType.PostgreSql:
                    builder.Append("(");
                    break;
                default:
                    throw new NotImplementedException();
            }
            bool first = true;
            if (toConcat != null)
            {
                foreach (string s in toConcat)
                {
                    if (first) first = false;
                    else
                    {
                        switch (targetDB)
                        {
                            case DatabaseType.MySql:
                                builder.Append(", ");
                                break;
                            case DatabaseType.SqlServer:
                                builder.Append(" + ");
                                break;
                            case DatabaseType.PostgreSql:
                                builder.Append(" || ");
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    switch (targetDB)
                    {
                        case DatabaseType.MySql:
                        case DatabaseType.PostgreSql:
                            builder.Append(s);
                            break;
                        case DatabaseType.SqlServer:
                            if (!s.StartsWith("\'") && !skipFirst)
                            {
                                builder.Append("CAST(");
                                builder.Append(s);
                                builder.Append(" as varchar(255))");
                            }
                            else
                            {
                                builder.Append(s);
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            builder.Append(")");
            return builder.ToString();
        }

        private static string ConvertDateTimeToDbDateString(DatabaseType databaseType, TzDateTime dt)
        {
            return ConvertDateTimeToDbDateString(databaseType, dt, true);
        }

        private static string ConvertDateTimeToDbDateString(DatabaseType databaseType, TzDateTime dt, bool doStringEncapsulation)
        {
            string valStr = "";
            switch (databaseType)
            {
                case DatabaseType.MySql:
                case DatabaseType.SqlServer:
                case DatabaseType.PostgreSql:
                    if (doStringEncapsulation)
                    {
                        valStr += "'";
                    }
                    valStr += dt.DateTimeUtc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (doStringEncapsulation)
                    {
                        valStr += "'";
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return valStr;
        }

        private static string ConvertDateTimeToDbTimeString(DatabaseType databaseType, TzDateTime dt)
        {
            return ConvertDateTimeToDbTimeString(databaseType, dt, true);
        }

        private static string ConvertDateTimeToDbTimeString(DatabaseType databaseType, TzDateTime dt, bool doStringEncapsulation)
        {
            string valStr = "";
            switch (databaseType)
            {
                case DatabaseType.MySql:
                case DatabaseType.SqlServer:
                case DatabaseType.PostgreSql:
                    if (doStringEncapsulation)
                    {
                        valStr += "'";
                    }
                    valStr += dt.DateTimeUtc.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                    if (doStringEncapsulation)
                    {
                        valStr += "'";
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return valStr;
        }

        /// <summary>
        /// If the caller knows that the date time stored in the database
        /// was stored in UTC format, then this will return the exact date time
        /// (without conversion from local to UTC), and set it's Kind property to
        /// UTC. The value taken from the database is not modified.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static TzDateTime ReadDateTimeToUtc(IDataReader reader, int index)
        {
            return new TzDateTime(reader.GetDateTime(index), true);
        }

        /// <summary>
        /// Gets the type of the database.
        /// </summary>
        /// <param name="databaseType">Type of the database.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetDatabaseType(DatabaseType databaseType, Type type)
        {
            switch (type.Name.ToLower())
            {
                case "string":
                    return "VARCHAR";
                case "blob":
                    switch (databaseType)
                    {
                        case DatabaseType.MySql:
                            return "LONGBLOB";
                        case DatabaseType.SqlServer:
                            return "IMAGE";
                        case DatabaseType.PostgreSql:
                            return "OID";
                        default:
                            throw new NotImplementedException();
                    }
                case "int16":
                    return "SMALLINT";
                case "int32":
                    return "INT";
                case "stringbuilder":
                    switch (databaseType)
                    {
                        case DatabaseType.MySql:
                            return "LONGTEXT";
                        case DatabaseType.SqlServer:
                        case DatabaseType.PostgreSql:
                            return "TEXT";
                        default:
                            throw new NotImplementedException();
                    }
                case "single":
                case "float":
                case "double":
                    switch (databaseType)
                    {
                        case DatabaseType.MySql:
                            return "DOUBLE";
                        case DatabaseType.SqlServer:
                            return "FLOAT";
                        case DatabaseType.PostgreSql:
                            return "NUMERIC";
                        default:
                            throw new NotImplementedException();
                    }
                case "decimal":
                    // Considered an "exact" numeric type
                    switch (databaseType)
                    {
                        case DatabaseType.MySql:
                            return "DOUBLE";
                        case DatabaseType.SqlServer:
                            // http://msdn2.microsoft.com/en-us/library/ms187746.aspx
                            return "DECIMAL";
                        case DatabaseType.PostgreSql:
                            return "NUMERIC";
                        default:
                            throw new NotImplementedException();
                    }
                case "datetime":
                    switch (databaseType)
                    {
                        case DatabaseType.MySql:
                        case DatabaseType.SqlServer:
                            return "DATETIME";
                        case DatabaseType.PostgreSql:
                            return "TIMESTAMP";
                        default:
                            throw new NotImplementedException();
                    }
                default:
                    return type.Name.ToUpper();
            }
        }

        /// <summary>
        /// Generates the create tables query.
        /// </summary>
        /// <param name="tables">The tables.</param>
        /// <returns></returns>
        public static string GenerateCreateTablesQuery(DataTable[] tables)
        {
            string[] tableCreateStatements = new string[tables.Length];
            for (int i = 0; i < tables.Length; i++)
            {
                tableCreateStatements[i] = QueryBuilder.GenerateCreateTableStatement(tables[i]);
            }

            // Now, for most database types, we'll just create one massive statement which we will execute
            StringBuilder sb = new StringBuilder();
            foreach (string tableCreateStatement in tableCreateStatements)
            {
                sb.AppendLine(tableCreateStatement);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Adds the foreign key constraint.
        /// </summary>
        /// <param name="localTable">The local table.</param>
        /// <param name="localColumnName">Name of the local column.</param>
        /// <param name="foreignTable">The foreign table.</param>
        /// <param name="foreignColumnName">Name of the foreign column.</param>
        public static void AddForeignKeyConstraint(DataTable localTable, string localColumnName, DataTable foreignTable, string foreignColumnName)
        {
            localTable.Constraints.Add(GetIdentifier("fk_" + localTable.TableName + "_" + localColumnName + "_" + foreignTable.TableName + "_" + foreignColumnName), foreignTable.Columns[foreignColumnName], localTable.Columns[localColumnName]);
        }

        private static string GetIdentifier(string str)
        {
            return str.Length > 63 ? str.Substring(0, 63) : str;
        }

        /// <summary>
        /// Adds the primary key constraint.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnNames">The column names.</param>
        public static void AddPrimaryKeyConstraint(DataTable table, params string[] columnNames)
        {
            if (columnNames != null)
            {
                DataColumn[] cols = new DataColumn[columnNames.Length];
                for (int i = 0; i < columnNames.Length; i++)
                {
                    cols[i] = table.Columns[columnNames[i]];
                }
                table.Constraints.Add(GetIdentifier("pk_" + table.TableName), cols, true);
            }
        }

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="columnType">Type of the column.</param>
        /// <returns></returns>
        public static DataColumn AddColumn(DataTable table, string columnName, Type columnType)
        {
            return AddColumn(table, columnName, columnType, false, null);
        }

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="columnType">Type of the column.</param>
        /// <param name="allowDBNull">if set to <c>true</c> [allow DB null].</param>
        /// <returns></returns>
        public static DataColumn AddColumn(DataTable table, string columnName, Type columnType, bool allowDBNull)
        {
            return AddColumn(table, columnName, columnType, allowDBNull, null);
        }

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="columnType">Type of the column.</param>
        /// <param name="allowDBNull">if set to <c>true</c> [allow DB null].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static DataColumn AddColumn(DataTable table, string columnName, Type columnType, bool allowDBNull, object defaultValue)
        {
            DataColumn ret = table.Columns.Add(columnName, columnType);
            ret.AllowDBNull = allowDBNull;
            if (defaultValue != null)
            {
                ret.DefaultValue = defaultValue;
            }
            if (columnType.Equals(typeof(string)))
            {
                if (Database.Current.DatabaseType == DatabaseType.MySql)
                {
                    ret.MaxLength = 255;
                }
                else
                {
                    ret.MaxLength = 255;
                }
            }
            else if (columnType.Equals(typeof(decimal)))
            {
                switch (Database.Current.DatabaseType)
                {
                    case DatabaseType.SqlServer:
                    case DatabaseType.PostgreSql:
                        SQLHelper.SetMaxLength(ret, 24);
                        SQLHelper.SetScale(ret, 9);
                        break;
                    case DatabaseType.MySql:
                        SQLHelper.SetMaxLength(ret, 24);
                        SQLHelper.SetScale(ret, 9);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return ret;
        }

        private static DataColumn SetScale(DataColumn col, int scale)
        {
            col.ExtendedProperties[EXTENDED_PROPERTY_SCALE] = scale;
            return col;
        }

        private const string EXTENDED_PROPERTY_MAX_LENGTH = "SQLHelper.MaxLength";
        private const string EXTENDED_PROPERTY_IS_UNICODE = "SQLHelper.IsUnicode";
        private const string EXTENDED_PROPERTY_SCALE = "SQLHelper.Scale";

        /// <summary>
        /// Sets the length of the max.
        /// </summary>
        /// <param name="col">The col.</param>
        /// <param name="maxLength">Length of the max.</param>
        /// <returns></returns>
        public static DataColumn SetMaxLength(DataColumn col, int maxLength)
        {
            if (col.DataType.Equals(typeof(string)))
            {
                col.MaxLength = maxLength;
            }
            col.ExtendedProperties[EXTENDED_PROPERTY_MAX_LENGTH] = maxLength;
            return col;
        }

        /// <summary>
        /// Gets the length of the max.
        /// </summary>
        /// <param name="col">The col.</param>
        /// <returns></returns>
        public static int GetMaxLength(DataColumn col)
        {
            if (col.ExtendedProperties.ContainsKey(EXTENDED_PROPERTY_MAX_LENGTH))
            {
                return (int)col.ExtendedProperties[EXTENDED_PROPERTY_MAX_LENGTH];
            }
            return 0;
        }

        /// <summary>
        /// Sets the is unicode.
        /// </summary>
        /// <param name="col">The col.</param>
        /// <param name="isUnicode">if set to <c>true</c> [is unicode].</param>
        /// <returns></returns>
        public static DataColumn SetIsUnicode(DataColumn col, bool isUnicode)
        {
            col.ExtendedProperties[EXTENDED_PROPERTY_IS_UNICODE] = isUnicode;
            return col;
        }

        /// <summary>
        /// Determines whether the specified col is unicode.
        /// </summary>
        /// <param name="col">The col.</param>
        /// <returns>
        /// 	<c>true</c> if the specified col is unicode; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUnicode(DataColumn col)
        {
            if (col.ExtendedProperties.ContainsKey(EXTENDED_PROPERTY_IS_UNICODE))
            {
                return (bool)col.ExtendedProperties[EXTENDED_PROPERTY_IS_UNICODE];
            }
            return false;
        }

        /// <summary>
        /// Gets the scale.
        /// </summary>
        /// <param name="col">The col.</param>
        /// <returns></returns>
        public static int? GetScale(DataColumn col)
        {
            if (col.ExtendedProperties.ContainsKey(EXTENDED_PROPERTY_SCALE))
            {
                return (int)col.ExtendedProperties[EXTENDED_PROPERTY_SCALE];
            }
            return null;
        }

        /// <summary>
        /// Gets the statement end character.
        /// </summary>
        /// <param name="supportedDBType">Type of the supported DB.</param>
        /// <returns></returns>
        public static char GetStatementEndCharacter(DatabaseType supportedDBType)
        {
            switch (supportedDBType)
            {
                case DatabaseType.MySql:
                case DatabaseType.SqlServer:
                case DatabaseType.PostgreSql:
                    return ';';
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Determines whether [is exception foreing key constraint] [the specified se].
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns>
        /// 	<c>true</c> if [is exception foreing key constraint] [the specified se]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsExceptionForeignKeyConstraintError(Exception e)
        {
            if (e.Message != null)
            {
                if (e.Message.Contains("statement conflicted with the FOREIGN KEY constraint"))
                {
                    // this is for sql server
                    return true;
                }
                else if (e.Message.StartsWith("ERROR: 23503"))
                {
                    // this is for postgre
                    return true;
                }
                else if (e.Message.StartsWith("Cannot add or update a child row: a foreign key constraint fails"))
                {
                    // this is for mysql
                    return true;
                }
            }

            return false;
        }
    }
}
