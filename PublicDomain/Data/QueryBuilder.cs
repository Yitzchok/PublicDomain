using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PublicDomain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryBuilder
    {
        private const int MySQLDefaultVarcharLength = 1000;
        private string m_sql;
        private int m_paramCount;
        private string[] m_params;
        private string[] m_splitSql;
        private int m_addCount;
        private DatabaseType m_databaseType;
        private bool m_paramsFilled;

        /// <summary>
        /// Gets or sets the SQL text.
        /// </summary>
        /// <value>The SQL text.</value>
        public string SqlText
        {
            get
            {
                return m_sql;
            }
            set
            {
                m_splitSql = null;
                m_paramCount = 0;
                if (value != null)
                {
                    m_splitSql = value.Split('?');
                    m_paramCount = m_splitSql.Length - 1;
                }
                m_sql = value;
                ClearParameters();
            }
        }

        /// <summary>
        /// Gets the parameter count.
        /// </summary>
        /// <value>The parameter count.</value>
        public int ParameterCount
        {
            get
            {
                return m_paramCount;
            }
        }

        /// <summary>
        /// Gets or sets the type of the database.
        /// </summary>
        /// <value>The type of the database.</value>
        public DatabaseType DatabaseType
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
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        public QueryBuilder()
            : this(null, Database.Current.DatabaseType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        /// <param name="databaseType">Type of the database.</param>
        public QueryBuilder(DatabaseType databaseType)
            : this(null, databaseType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        public QueryBuilder(string sql)
            : this(sql, Database.Current.DatabaseType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="databaseType">Type of the database.</param>
        public QueryBuilder(string sql, DatabaseType databaseType)
        {
            DatabaseType = databaseType;
            SqlText = sql;
        }

        /// <summary>
        /// Adds the parameter string.
        /// </summary>
        /// <param name="str">The STR.</param>
        public void AddParameterString(string str)
        {
            m_params[m_addCount++] = str;
        }

        /// <summary>
        /// Adds the name of the parameter db object.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        public void AddParameterDbObjectName(string objectName)
        {
            m_params[m_addCount++] = objectName;
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="val">The val.</param>
        public void AddParameter(object val)
        {
            m_params[m_addCount++] = SQLHelper.ObjectToDbString(DatabaseType, val);
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="type">The type.</param>
        public void AddParameter(object val, DbType type)
        {
            m_params[m_addCount++] = SQLHelper.ObjectToDbString(DatabaseType, val, type);
        }

        /// <summary>
        /// Adds the parameter string insensitive start.
        /// </summary>
        public void AddParameterStringInsensitiveStart()
        {
            switch (DatabaseType)
            {
                case DatabaseType.PostgreSql:
                case DatabaseType.MySql:
                    m_params[m_addCount++] = "UPPER(";
                    break;
                default:
                    m_params[m_addCount++] = string.Empty;
                    break;
            }
        }

        /// <summary>
        /// Adds the parameter string insensitive end.
        /// </summary>
        public void AddParameterStringInsensitiveEnd()
        {
            switch (DatabaseType)
            {
                case DatabaseType.PostgreSql:
                case DatabaseType.MySql:
                    m_params[m_addCount++] = ")";
                    break;
                default:
                    m_params[m_addCount++] = string.Empty;
                    break;
            }
        }

        /// <summary>
        /// Adds the parameters.
        /// </summary>
        /// <param name="p">The p.</param>
        public void AddParameters(object[] p)
        {
            foreach (object param in p)
            {
                AddParameter(param);
            }
        }

        /// <summary>
        /// Adds the parameters.
        /// </summary>
        /// <param name="p">The p.</param>
        public void AddParameters(ICollection<object> p)
        {
            foreach (object param in p)
            {
                AddParameter(param);
            }
        }

        /// <summary>
        /// Adds the parameter concat.
        /// </summary>
        /// <param name="str1">The STR1.</param>
        /// <param name="str2">The STR2.</param>
        public void AddParameterConcat(string str1, string str2)
        {
            AddParameterConcat(str1, str2, false);
        }

        /// <summary>
        /// Adds the parameter concat.
        /// </summary>
        /// <param name="str1">The STR1.</param>
        /// <param name="str2">The STR2.</param>
        /// <param name="skipFirst">if set to <c>true</c> [skip first].</param>
        public void AddParameterConcat(string str1, string str2, bool skipFirst)
        {
            AddParameterString(SQLHelper.ConcatFunction(DatabaseType, skipFirst, str1, str2));
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <param name="val">The val.</param>
        public void SetParameter(int i, object val)
        {
            m_params[i] = SQLHelper.ObjectToDbString(DatabaseType, val);
            m_paramsFilled = true;
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <param name="val">The val.</param>
        /// <param name="type">The type.</param>
        public void SetParameter(int i, object val, DbType type)
        {
            m_params[i] = SQLHelper.ObjectToDbString(DatabaseType, val, type);
            m_paramsFilled = true;
        }

        /// <summary>
        /// Sets the parameter string.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <param name="str">The STR.</param>
        public void SetParameterString(int i, string str)
        {
            m_params[i] = str;
            m_paramsFilled = true;
        }

        /// <summary>
        /// Sets the parameter concat.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <param name="str1">The STR1.</param>
        /// <param name="str2">The STR2.</param>
        public void SetParameterConcat(int i, string str1, string str2)
        {
            SetParameterString(i, SQLHelper.ConcatFunction(DatabaseType, false, str1, str2));
        }

        /// <summary>
        /// Clears the parameters.
        /// </summary>
        public void ClearParameters()
        {
            m_params = new string[m_paramCount];
            m_addCount = 0;
            m_paramsFilled = false;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            if (m_addCount != ParameterCount && !m_paramsFilled)
            {
                throw new ArgumentException(string.Format("Not enough parameters were supplied to convert from the parameterized form. There are {0} parameters and only {1} values were supplied.", ParameterCount, m_addCount));
            }

            // build the final sql
            StringBuilder ret = new StringBuilder((m_splitSql.Length + m_params.Length) * 32);
            int x = 0;
            foreach (string s in m_splitSql)
            {
                ret.Append(s);
                if (x < ParameterCount)
                {
                    ret.Append(m_params[x++]);
                }
            }
            return ret.ToString();
        }

        /// <summary>
        /// Implicit operators the specified qb.
        /// </summary>
        /// <param name="qb">The qb.</param>
        /// <returns></returns>
        public static implicit operator string(QueryBuilder qb)
        {
            return qb.ToString();
        }

        /// <summary>
        /// Generates the create table statement.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static string GenerateCreateTableStatement(DataTable table)
        {
            StringBuilder sb = new StringBuilder(256);

            sb.AppendFormat("CREATE TABLE {0} (\n", table.TableName);

            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(",\n");
                }
                DataColumn col = table.Columns[i];
                int storedMaxLength = SQLHelper.GetMaxLength(col);
                int maxLength = Math.Max(col.MaxLength, storedMaxLength);
                int? scale = SQLHelper.GetScale(col);
                if (storedMaxLength == -1 && Database.Current.DatabaseType == DatabaseType.SqlServer)
                {
                    maxLength = storedMaxLength;
                }
                if (maxLength == -1 && Database.Current.DatabaseType == DatabaseType.MySql && col.DataType == typeof(string))
                {
                    maxLength = MySQLDefaultVarcharLength;
                }

                // Name of the column followed by a space
                sb.Append(col.ColumnName);
                sb.Append(' ');

                // If it is SQL Server and the column is set as unicode,
                // then we need an "N" in front, specifying unicode
                if (Database.Current.DatabaseType == DatabaseType.SqlServer && SQLHelper.IsUnicode(col))
                {
                    sb.Append('N');
                }

                // The actual database type
                sb.Append(SQLHelper.GetDatabaseType(Database.Current.DatabaseType, col.DataType));

                if (maxLength == -1 && Database.Current.DatabaseType == DatabaseType.SqlServer && col.DataType.Equals(typeof(string)))
                {
                    sb.Append("(max)");
                }
                else if (maxLength > 0)
                {
                    sb.Append("(" + maxLength);
                    if (scale != null)
                    {
                        sb.Append(", " + scale.Value);
                    }
                    sb.Append(')');
                }

                if (col.DefaultValue != null && col.DefaultValue != DBNull.Value)
                {
                    sb.Append(" DEFAULT " + SQLHelper.ObjectToDbString(Database.Current.DatabaseType, col.DefaultValue));
                }

                if (!col.AllowDBNull)
                {
                    sb.Append(" NOT");
                }

                sb.Append(" NULL");

            }
            sb.Append("\n");

            IList<UniqueConstraint> primaryKeys = GetPrimaryKeyConstraints(table);
            // Now append the primary key line, if any
            foreach (UniqueConstraint primaryKey in primaryKeys)
            {
                sb.AppendFormat(",CONSTRAINT {0} PRIMARY KEY (", primaryKey.ConstraintName);
                for (int i = 0; i < primaryKey.Columns.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(',');
                    }
                    sb.Append(primaryKey.Columns[i].ColumnName);
                }
                sb.Append(")\n");
            }

            IList<ForeignKeyConstraint> foreignKeys = GetForeignKeyConstraints(table);
            // Append foreign key constraints, if any
            foreach (ForeignKeyConstraint foreignKey in foreignKeys)
            {
                sb.AppendFormat(",CONSTRAINT {0} FOREIGN KEY (", foreignKey.ConstraintName);
                for (int i = 0; i < foreignKey.Columns.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(',');
                    }
                    sb.Append(foreignKey.Columns[i].ColumnName);
                }
                sb.AppendFormat(") REFERENCES {0} (", foreignKey.RelatedTable.TableName);
                for (int i = 0; i < foreignKey.RelatedColumns.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(',');
                    }
                    sb.Append(foreignKey.RelatedColumns[i].ColumnName);
                }
                sb.Append(")\n");
            }

            sb.Append(")");
            sb.Append(SQLHelper.GetStatementEndCharacter(Database.Current.DatabaseType));

            return sb.ToString();
        }

        /// <summary>
        /// Generates the drop table statement.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static string GenerateDropTableStatement(DataTable table)
        {
            return "DROP TABLE " + table.TableName + SQLHelper.GetStatementEndCharacter(Database.Current.DatabaseType);
        }

        /// <summary>
        /// Gets the primary key constraints.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static IList<UniqueConstraint> GetPrimaryKeyConstraints(DataTable table)
        {
            IList<UniqueConstraint> ret = new List<UniqueConstraint>();
            foreach (Constraint constraint in table.Constraints)
            {
                UniqueConstraint uniqueConstraint = constraint as UniqueConstraint;
                if (uniqueConstraint != null && uniqueConstraint.IsPrimaryKey)
                {
                    ret.Add(uniqueConstraint);
                }
            }
            return ret;
        }

        /// <summary>
        /// Gets the foreign key constraints.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static IList<ForeignKeyConstraint> GetForeignKeyConstraints(DataTable table)
        {
            IList<ForeignKeyConstraint> ret = new List<ForeignKeyConstraint>();
            foreach (Constraint constraint in table.Constraints)
            {
                ForeignKeyConstraint foreignConstraint = constraint as ForeignKeyConstraint;
                if (foreignConstraint != null)
                {
                    ret.Add(foreignConstraint);
                }
            }
            return ret;
        }
    }
}
