using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PublicDomain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataTable
    {
        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>The columns.</value>
        DataColumnCollection Columns { get; }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>The table.</value>
        DataTable Table { get; }

        /// <summary>
        /// Gets the create SQL.
        /// </summary>
        /// <value>The create SQL.</value>
        string CreateSql { get; }

        /// <summary>
        /// Gets the drop SQL.
        /// </summary>
        /// <value>The drop SQL.</value>
        string DropSql { get; }

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="columnType">Type of the column.</param>
        /// <param name="allowDBNull">if set to <c>true</c> [allow DB null].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        DataColumn AddColumn(string columnName, Type columnType, bool allowDBNull, object defaultValue);

        /// <summary>
        /// Adds the primary key constraint.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        void AddPrimaryKeyConstraint(params string[] columnNames);

        /// <summary>
        /// Adds the foreign key constraint.
        /// </summary>
        /// <param name="localColumnName">Name of the local column.</param>
        /// <param name="foreignTable">The foreign table.</param>
        /// <param name="foreignColumnName">Name of the foreign column.</param>
        void AddForeignKeyConstraint(string localColumnName, IDataTable foreignTable, string foreignColumnName);

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="conn">The conn.</param>
        void CreateTable(IDbConnection conn);

        /// <summary>
        /// Drops the table.
        /// </summary>
        /// <param name="conn">The conn.</param>
        void DropTable(IDbConnection conn);

        /// <summary>
        /// Gets a value indicating whether this instance is existent.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is existent; otherwise, <c>false</c>.
        /// </value>
        bool IsExistent { get; }

        /// <summary>
        /// Inserts the dynamic.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="args">The args.</param>
        void InsertDynamic(IDbConnection conn, params object[] args);

        /// <summary>
        /// Gets the total rows.
        /// </summary>
        /// <value>The total rows.</value>
        int TotalRows { get; }

        /// <summary>
        /// Clears the table.
        /// </summary>
        void ClearTable();
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class DataTableAbstraction : IDataTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableAbstraction"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public DataTableAbstraction(string tableName)
        {
            m_table.TableName = tableName;
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public DataColumnCollection Columns
        {
            get
            {
                return Table.Columns;
            }
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>The table.</value>
        public DataTable Table
        {
            get
            {
                return m_table;
            }
        }

        /// <summary>
        /// Gets the create SQL.
        /// </summary>
        /// <value>The create SQL.</value>
        public string CreateSql
        {
            get
            {
                return QueryBuilder.GenerateCreateTableStatement(Table);
            }
        }

        /// <summary>
        /// Gets the drop SQL.
        /// </summary>
        /// <value>The drop SQL.</value>
        public string DropSql
        {
            get
            {
                return QueryBuilder.GenerateDropTableStatement(Table);
            }
        }

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="columnType">Type of the column.</param>
        /// <param name="AllowDBNull">if set to <c>true</c> [allow DB null].</param>
        /// <param name="DefaultValue">The default value.</param>
        /// <returns></returns>
        public DataColumn AddColumn(string columnName, Type columnType, bool AllowDBNull, object DefaultValue)
        {
            return SQLHelper.AddColumn(Table, columnName, columnType, AllowDBNull, DefaultValue);
        }

        /// <summary>
        /// Adds the primary key constraint.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        public void AddPrimaryKeyConstraint(params string[] columnNames)
        {
            SQLHelper.AddPrimaryKeyConstraint(Table, columnNames);
        }

        /// <summary>
        /// Adds the foreign key constraint.
        /// </summary>
        /// <param name="localColumnName">Name of the local column.</param>
        /// <param name="foreignTable">The foreign table.</param>
        /// <param name="foreignColumnName">Name of the foreign column.</param>
        public void AddForeignKeyConstraint(string localColumnName, IDataTable foreignTable, string foreignColumnName)
        {
            SQLHelper.AddForeignKeyConstraint(Table, localColumnName, foreignTable.Table, foreignColumnName);
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="conn">The conn.</param>
        public void CreateTable(IDbConnection conn)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Drops the table.
        /// </summary>
        /// <param name="conn">The conn.</param>
        public void DropTable(IDbConnection conn)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets a value indicating whether this instance is existent.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is existent; otherwise, <c>false</c>.
        /// </value>
        public bool IsExistent
        {
            get
            {
                // http://www.aspfaq.com/show.asp?id=2458
                // http://forums.mysql.com/read.php?101,33936,33936
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        /// <summary>
        /// Inserts the dynamic.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="args">The args.</param>
        public void InsertDynamic(IDbConnection conn, params object[] args)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets the total rows.
        /// </summary>
        /// <value>The total rows.</value>
        public int TotalRows
        {
            get
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        /// <summary>
        /// Clears the table.
        /// </summary>
        public void ClearTable()
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        protected DataTable m_table = new DataTable();
    }
}
