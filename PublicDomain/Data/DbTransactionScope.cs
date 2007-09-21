using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using PublicDomain.Logging;

namespace PublicDomain.Data
{
    /// <summary>
    /// Wraps both a transaction scope and a connection scope
    /// </summary>
    public class DbTransactionScope : IDisposable
    {
        internal static readonly Logger Log = LoggingConfig.Current.CreateLogger(typeof(DbTransactionScope), GlobalConstants.LogClassDatabase);

        [ThreadStatic]
        private static bool s_isInUse;

        [ThreadStatic]
        private static Stack<DbTransactionScope> s_scopes;

        private bool m_isDisposed;
        private TransactionScope m_scope;
        private DbConnectionScope m_connScope;
        private Transaction m_transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionScope"/> class.
        /// </summary>
        public DbTransactionScope()
        {
            if (Log.Enabled) Log.Entry("DbTransactionScope", Transaction.Current);

            m_scope = new TransactionScope();
            Initialize();

            if (Log.Enabled) Log.Exit("DbTransactionScope", Transaction.Current);
        }

        /// <summary>
        /// Transactions the scope.
        /// </summary>
        /// <param name="transactionToUse">
        ///     The transaction to be set as the ambient transaction, so that transactional
        ///     work done inside the scope uses this transaction.
        /// </param>
        public DbTransactionScope(Transaction transactionToUse)
        {
            if (Log.Enabled) Log.Entry("DbTransactionScope", Transaction.Current);

            m_scope = new TransactionScope(transactionToUse);
            Initialize();

            if (Log.Enabled) Log.Exit("DbTransactionScope", Transaction.Current);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionScope"/> class.
        /// </summary>
        /// <param name="scopeOption">
        ///     An instance of the System.Transactions.TransactionScopeOption enumeration
        ///     that describes the transaction requirements associated with this transaction
        ///     scope.
        /// </param>
        public DbTransactionScope(TransactionScopeOption scopeOption)
        {
            if (Log.Enabled) Log.Entry("DbTransactionScope", Transaction.Current);

            m_scope = new TransactionScope(scopeOption);
            Initialize();

            if (Log.Enabled) Log.Exit("DbTransactionScope", Transaction.Current);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionScope"/> class, and sets the specified transaction as the
        ///     ambient transaction, so that transactional work done inside the scope uses
        ///     this transaction.
        /// </summary>
        /// <param name="transactionToUse">
        /// The transaction to be set as the ambient transaction, so that transactional
        ///  work done inside the scope uses this transaction.
        /// </param>
        /// <param name="scopeTimeout">
        ///     The System.TimeSpan after which the transaction scope times out and aborts
        ///     the transaction.
        /// </param>
        public DbTransactionScope(Transaction transactionToUse, TimeSpan scopeTimeout)
        {
            if (Log.Enabled) Log.Entry("DbTransactionScope", Transaction.Current);

            m_scope = new TransactionScope(transactionToUse, scopeTimeout);
            Initialize();

            if (Log.Enabled) Log.Exit("DbTransactionScope", Transaction.Current);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionScope"/> class.
        /// </summary>
        /// <param name="scopeOption">
        ///     An instance of the System.Transactions.TransactionScopeOption enumeration
        ///     that describes the transaction requirements associated with this transaction
        ///     scope.
        /// </param>
        /// <param name="scopeTimeout">
        ///     The System.TimeSpan after which the transaction scope times out and aborts
        ///     the transaction.
        /// </param>
        public DbTransactionScope(TransactionScopeOption scopeOption, TimeSpan scopeTimeout)
        {
            if (Log.Enabled) Log.Entry("DbTransactionScope", Transaction.Current);

            m_scope = new TransactionScope(scopeOption, scopeTimeout);
            Initialize();

            if (Log.Enabled) Log.Exit("DbTransactionScope", Transaction.Current);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionScope"/> class.
        /// </summary>
        /// <param name="scopeOption">
        ///     An instance of the System.Transactions.TransactionScopeOption enumeration
        ///     that describes the transaction requirements associated with this transaction
        ///     scope.
        /// </param>
        /// <param name="transactionOptions">
        ///     A System.Transactions.TransactionOptions structure that describes the transaction
        ///     options to use if a new transaction is created. If an existing transaction
        ///     is used, the timeout value in this parameter applies to the transaction scope.
        ///     If that time expires before the scope is disposed, the transaction is aborted.
        /// </param>
        public DbTransactionScope(TransactionScopeOption scopeOption, TransactionOptions transactionOptions)
        {
            if (Log.Enabled) Log.Entry("DbTransactionScope", Transaction.Current);

            m_scope = new TransactionScope(scopeOption, transactionOptions);
            Initialize();

            if (Log.Enabled) Log.Exit("DbTransactionScope", Transaction.Current);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionScope"/> class with the specified timeout value and COM+ interoperability requirements,
        ///     and sets the specified transaction as the ambient transaction, so that transactional
        ///     work done inside the scope uses this transaction.
        /// </summary>
        /// <param name="transactionToUse">
        ///     The transaction to be set as the ambient transaction, so that transactional
        ///     work done inside the scope uses this transaction.
        /// </param>
        /// <param name="scopeTimeout">
        ///     The System.TimeSpan after which the transaction scope times out and aborts
        ///     the transaction.
        /// </param>
        /// <param name="interopOption">
        ///     An instance of the System.Transactions.EnterpriseServicesInteropOption enumeration
        ///     that describes how the associated transaction interacts with COM+ transactions.
        /// </param>
        public DbTransactionScope(Transaction transactionToUse, TimeSpan scopeTimeout, EnterpriseServicesInteropOption interopOption)
        {
            if (Log.Enabled) Log.Entry("DbTransactionScope", Transaction.Current);

            m_scope = new TransactionScope(transactionToUse, scopeTimeout, interopOption);
            Initialize();

            if (Log.Enabled) Log.Exit("DbTransactionScope", Transaction.Current);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionScope"/> class
        /// with the specified scope and COM+ interoperability requirements, and transaction
        ///  options.
        /// </summary>
        /// <param name="scopeOption">
        ///     An instance of the System.Transactions.TransactionScopeOption enumeration
        ///     that describes the transaction requirements associated with this transaction
        ///     scope.
        /// </param>
        /// <param name="transactionOptions">
        ///     A System.Transactions.TransactionOptions structure that describes the transaction
        ///     options to use if a new transaction is created. If an existing transaction
        ///     is used, the timeout value in this parameter applies to the transaction scope.
        ///     If that time expires before the scope is disposed, the transaction is aborted.
        /// </param>
        /// <param name="interopOption">
        ///     An instance of the System.Transactions.EnterpriseServicesInteropOption enumeration
        ///     that describes how the associated transaction interacts with COM+ transactions.
        /// </param>
        public DbTransactionScope(TransactionScopeOption scopeOption, TransactionOptions transactionOptions, EnterpriseServicesInteropOption interopOption)
        {
            if (Log.Enabled) Log.Entry("DbTransactionScope", Transaction.Current);

            m_scope = new TransactionScope(scopeOption, transactionOptions, interopOption);
            Initialize();

            if (Log.Enabled) Log.Exit("DbTransactionScope", Transaction.Current);
        }

        /// <summary>
        /// Indicates that all operations within the scope are completed successfully.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">This method has already been called once.</exception>
        public virtual void Complete()
        {
            if (Log.Enabled) Log.Entry("Complete", m_scope);

            m_scope.Complete();

            if (Log.Enabled) Log.Exit("Complete");
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
        /// Initializes this instance.
        /// </summary>
        protected virtual void Initialize()
        {
            if (Log.Enabled) Log.Entry("Initialize");

            s_isInUse = true;

            // Since a new TransactionScope is created, this is 
            // a boundary for connection scope inheritance.
            DbConnectionScope currentConnScope = DbConnectionScope.Current;

            // We have created a new transaction scope. If one already exists, then
            // we make that a boundary so that any new connections come from within the context
            // of a new transaction scope. We only do this if the database supports
            // nested transactions; otherwise, we have to keep re-using the same connection.
            if (currentConnScope != null && Database.Current.SupportsFeature(DatabaseFeature.NestedTransactions))
            {
                if (Log.Enabled) Log.LogDebug10("Nested transactions supported and the currentConnScope is not null");

                currentConnScope.AllowConnectionInheritance = false;
            }

            // Create a default DbTransactionScope for this transaction
            if (Log.Enabled) Log.LogDebug10("Creating DbConnectionScope...");

            m_connScope = new DbConnectionScope();

            if (s_scopes == null)
            {
                if (Log.Enabled) Log.LogDebug10("Scopes null, creating new stack");

                s_scopes = new Stack<DbTransactionScope>();
            }
            s_scopes.Push(this);

            m_transaction = Transaction.Current;

            if (Log.Enabled) Log.Exit("Initialize");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (Log.Enabled) Log.Entry("Dispose");

            if (!IsDisposed)
            {
                if (!object.ReferenceEquals(s_scopes.Peek(), this))
                {
                    throw new BaseException("Transaction scopes mismanaged. Was there a missing Dispose?");
                }

                if (Log.Enabled) Log.LogDebug10("Popping scopes");

                s_scopes.Pop();

                m_isDisposed = true;
                m_connScope.Dispose();
                m_scope.Dispose();
            }
            else
            {
                throw new ObjectDisposedException(typeof(DbTransactionScope).FullName);
            }

            if (Log.Enabled) Log.Exit("Dispose");
        }

        /// <summary>
        /// Ares the transactions nested.
        /// </summary>
        /// <returns></returns>
        public static bool AreTransactionsNested()
        {
            if (Log.Enabled) Log.Entry("AreTransactionsNested", s_scopes);

            bool result = false;

            if (s_scopes != null && s_scopes.Count > 1)
            {
                int uniqueTransactionScopes = 1;
                int l = s_scopes.Count;

                if (Log.Enabled) Log.LogDebug10("Transaction scope count = {0}", l);

                DbTransactionScope[] ar = new DbTransactionScope[l];
                s_scopes.CopyTo(ar, 0);
                DbTransactionScope x;
                int j;
                for (int i = 1; i < l; i++)
                {
                    x = ar[i];

                    for (j = 0; j < l; j++)
                    {
                        if (i != j && !x.Transaction.Equals(ar[j].Transaction))
                        {
                            break;
                        }
                    }

                    if (j < l)
                    {
                        uniqueTransactionScopes++;
                    }
                }

                if (Log.Enabled) Log.LogDebug10("Unique transaction scopes = {0}", uniqueTransactionScopes);
                
                result = uniqueTransactionScopes > 1;
            }

            if (Log.Enabled) Log.Exit("AreTransactionsNested", result);

            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is in use.
        /// </summary>
        /// <value><c>true</c> if this instance is in use; otherwise, <c>false</c>.</value>
        public static bool IsInUse
        {
            get
            {
                return s_isInUse;
            }
        }

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        public virtual Transaction Transaction
        {
            get
            {
                return m_transaction;
            }
        }
    }
}
