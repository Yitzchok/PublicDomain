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
    public class DynamicDbProviderFactory : DbProviderFactory
    {
        private DbProviderFactory m_underlyingFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicDbProviderFactory"/> class.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        public DynamicDbProviderFactory(string assemblyName, string typeName)
            : this(Assembly.Load(assemblyName), typeName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicDbProviderFactory"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        public DynamicDbProviderFactory(Assembly assembly, string typeName)
            : this(assembly.GetType(typeName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicDbProviderFactory"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public DynamicDbProviderFactory(Type type)
        {
            FieldInfo field = type.GetField("Instance");
            DbProviderFactory result = null;
            if (field != null)
            {
                result = (DbProviderFactory)field.GetValue(null);
            }
            UnderlyingFactory = result;
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="T:System.Data.Common.DbCommand"></see> class.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="T:System.Data.Common.DbCommand"></see>.
        /// </returns>
        public override DbCommand CreateCommand()
        {
            return m_underlyingFactory.CreateCommand();
        }

        /// <summary>
        /// Specifies whether the specific <see cref="T:System.Data.Common.DbProviderFactory"></see> supports the <see cref="T:System.Data.Common.DbDataSourceEnumerator"></see> class.
        /// </summary>
        /// <value></value>
        /// <returns>true if the instance of the <see cref="T:System.Data.Common.DbProviderFactory"></see> supports the <see cref="T:System.Data.Common.DbDataSourceEnumerator"></see> class; otherwise false.</returns>
        public override bool CanCreateDataSourceEnumerator
        {
            get
            {
                return m_underlyingFactory.CanCreateDataSourceEnumerator;
            }
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="T:System.Data.Common.DbCommandBuilder"></see> class.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="T:System.Data.Common.DbCommandBuilder"></see>.
        /// </returns>
        public override DbCommandBuilder CreateCommandBuilder()
        {
            return m_underlyingFactory.CreateCommandBuilder();
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="T:System.Data.Common.DbConnection"></see> class.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="T:System.Data.Common.DbConnection"></see>.
        /// </returns>
        public override DbConnection CreateConnection()
        {
            return m_underlyingFactory.CreateConnection();
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="T:System.Data.Common.DbConnectionStringBuilder"></see> class.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="T:System.Data.Common.DbConnectionStringBuilder"></see>.
        /// </returns>
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return m_underlyingFactory.CreateConnectionStringBuilder();
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="T:System.Data.Common.DbDataAdapter"></see> class.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="T:System.Data.Common.DbDataAdapter"></see>.
        /// </returns>
        public override DbDataAdapter CreateDataAdapter()
        {
            return m_underlyingFactory.CreateDataAdapter();
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="T:System.Data.Common.DbDataSourceEnumerator"></see> class.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="T:System.Data.Common.DbDataSourceEnumerator"></see>.
        /// </returns>
        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return m_underlyingFactory.CreateDataSourceEnumerator();
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="T:System.Data.Common.DbParameter"></see> class.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="T:System.Data.Common.DbParameter"></see>.
        /// </returns>
        public override DbParameter CreateParameter()
        {
            return m_underlyingFactory.CreateParameter();
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the provider's version of the <see cref="T:System.Security.CodeAccessPermission"></see> class.
        /// </summary>
        /// <param name="state">One of the <see cref="T:System.Security.Permissions.PermissionState"></see> values.</param>
        /// <returns>
        /// A <see cref="T:System.Data.Common.CodeAccessPermission"></see> object for the specified <see cref="T:System.Security.Permissions.PermissionState"></see>.
        /// </returns>
        public override System.Security.CodeAccessPermission CreatePermission(System.Security.Permissions.PermissionState state)
        {
            return m_underlyingFactory.CreatePermission(state);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return m_underlyingFactory.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return m_underlyingFactory.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return m_underlyingFactory.ToString();
        }

        /// <summary>
        /// Gets or sets the underlying factory.
        /// </summary>
        /// <value>The underlying factory.</value>
        public virtual DbProviderFactory UnderlyingFactory
        {
            get
            {
                return m_underlyingFactory;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Could not instantiate " + typeof(DbProviderFactory).Name);
                }
                m_underlyingFactory = value;
            }
        }
    }
}
