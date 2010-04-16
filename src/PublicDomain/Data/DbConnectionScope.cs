using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Transactions;
using PublicDomain.Logging;
using System.Threading;

namespace PublicDomain.Data
{
    /// <summary>
    /// This object is used to create a scope in which one database connection
    /// is re-used. A stack of scopes exists, so that once the current instance
    /// is disposed, we back out to the previous contextual scope.
    /// 
    /// You should not re-use connections across TransactionScope boundaries.
    /// </summary>
    public class DbConnectionScope : IDisposable
    {
        internal static readonly Logger Log = LoggingConfig.Current.CreateLogger(typeof(DbConnectionScope), GlobalConstants.LogClassDatabase);

        [ThreadStatic]
        private static Stack<DbConnectionScope> scopes;

        private bool m_isDisposed;
        private IDbConnection m_conn;
        private bool m_inheritConnections;
        private bool m_allowConnectionInheritance = true;
        private bool m_isInheritedConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionScope"/> class.
        /// By default, this will inherit connections from containing scopes. To
        /// ensure a new connection, use the other constructor and pass true.
        /// </summary>
        public DbConnectionScope()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionScope"/> class.
        /// If <paramref name="inheritConnections"/> is false, a new Database connection
        /// will be used in this scope.
        /// </summary>
        /// <param name="inheritConnections">if set to <c>true</c> [inherit connections].</param>
        public DbConnectionScope(bool inheritConnections)
        {
            if (Log.Enabled) Log.Entry("DbConnectionScope", inheritConnections);

            // If the database doesn't support MARS, and there is no transaction or a single transaction,
            // then we will open a new connection each time
            if (!Database.Current.SupportsFeature(DatabaseFeature.MultipleActiveResultSets))
            {
                if (Log.Enabled) Log.LogDebug10("Database feature {0} not supported", DatabaseFeature.MultipleActiveResultSets);

                if (Transaction.Current == null)
                {
                    if (Log.Enabled) Log.LogDebug10("Transaction.Current is null");

                    inheritConnections = false;
                }
                else if (Database.Current.SupportsFeature(DatabaseFeature.MultipleOpenConnectionsWithinSingleTransaction) && DbTransactionScope.IsInUse)
                {
                    if (Log.Enabled) Log.LogDebug10("DbTransactionScope is inuse");

                    // If this thread is using DbTransactionScope, then we check to see
                    // if there is only a single transaction or if they are nested
                    if (DbTransactionScope.AreTransactionsNested())
                    {
                        inheritConnections = false;
                    }
                }
            }

            m_inheritConnections = inheritConnections;

            if (Log.Enabled) Log.LogDebug10("InheritConnections = {0}", m_inheritConnections);

            if (scopes == null)
            {
                if (Log.Enabled) Log.LogDebug10("Scopes is null, thread {0}", Thread.CurrentThread);

                scopes = new Stack<DbConnectionScope>();
            }
            scopes.Push(this);

            if (Log.Enabled) Log.Exit("DbConnectionScope");
        }

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        public static DbConnectionScope Current
        {
            get
            {
                if (Log.Enabled) Log.Entry("Current");

                DbConnectionScope result = null;

                if (scopes != null && scopes.Count != 0)
                {
                    result = scopes.Peek();
                }

                if (Log.Enabled) Log.Exit("Current", result);

                return result;
            }
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public virtual IDbConnection Connection
        {
            get
            {
                if (Log.Enabled) Log.Entry("Connection", m_conn);

                if (m_conn == null)
                {
                    // See if we can inherit a connection
                    if (InheritConnections)
                    {
                        if (Log.Enabled) Log.LogDebug10("Attempting to inherit connection");

                        // First we find the "previous" scope
                        DbConnectionScope prevScope = null;
                        bool prepped = false;
                        foreach (DbConnectionScope scope in scopes)
                        {
                            if (prepped)
                            {
                                prevScope = scope;
                                break;
                            }
                            if (object.ReferenceEquals(this, scope))
                            {
                                prepped = true;
                            }
                        }

                        if (Log.Enabled) Log.LogDebug10("prevScope = {0}", prevScope);

                        if (prevScope != null)
                        {
                            if (Log.Enabled) Log.LogDebug10("AllowConnectionInheritance = {0}", prevScope.AllowConnectionInheritance);

                            if (prevScope.AllowConnectionInheritance)
                            {
                                m_conn = prevScope.Connection;

                                if (Log.Enabled) Log.LogDebug10("prevScope connection = {0}", m_conn);

                                if (m_conn != null)
                                {
                                    m_isInheritedConnection = true;
                                }
                            }
                            else
                            {
                            }
                        }
                    }

                    if (m_conn == null)
                    {
                        if (Log.Enabled) Log.LogDebug10("connection not found, requesting new connection");

                        m_conn = Database.Current.ConnectionProvider.GetConnection(true, true);
                    }
                    else
                    {
                        if (Log.Enabled) Log.LogDebug10("connection found, re-using");
                    }
                }
                else
                {
                    if (Log.Enabled) Log.LogDebug10("connection found, re-using");
                }

                // If it is closed, re-open it
                if (m_conn.State == ConnectionState.Closed)
                {
                    if (Log.Enabled) Log.LogDebug10("Connection closed, re-opening...");

                    m_conn.Open();
                }

                if (Log.Enabled) Log.Exit("Connection", m_conn);

                return m_conn;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsDisposed
        {
            get
            {
                return m_isDisposed;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [inherit connections].
        /// </summary>
        /// <value><c>true</c> if [inherit connections]; otherwise, <c>false</c>.</value>
        public virtual bool InheritConnections
        {
            get
            {
                return m_inheritConnections;
            }
            set
            {
                m_inheritConnections = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [allow connection inheritance].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [allow connection inheritance]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool AllowConnectionInheritance
        {
            get
            {
                return m_allowConnectionInheritance;
            }
            set
            {
                m_allowConnectionInheritance = value;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (Log.Enabled) Log.Entry("Dispose");

            if (!IsDisposed)
            {
                m_isDisposed = true;

                try
                {
                    // We should be the top of the stack
                    if (!object.ReferenceEquals(scopes.Peek(), this))
                    {
                        throw new BaseException("Connection scopes mismanaged. Was there a missing Dispose?");
                    }

                    if (Log.Enabled) Log.LogDebug10("Popping scope");

                    scopes.Pop();
                }
                finally
                {
                    // close any open connections
                    CloseConnections();
                }
            }
            else
            {
                throw new ObjectDisposedException(typeof(DbConnectionScope).FullName);
            }

            if (Log.Enabled) Log.Exit("Dispose");
        }

        /// <summary>
        /// Closes the connections.
        /// </summary>
        protected virtual void CloseConnections()
        {
            if (Log.Enabled) Log.Entry("CloseConnections", m_conn);

            if (m_conn != null)
            {
                if (!m_isInheritedConnection)
                {
                    if (Log.Enabled) Log.LogDebug10("Not inherited connection, closing {0}...", m_conn);

                    m_conn.Close();
                }
                m_conn = null;
            }

            if (Log.Enabled) Log.Exit("CloseConnections");
        }
    }
}
