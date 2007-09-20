using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace PublicDomain.Data
{
    /// <summary>
    /// Wraps both a transaction scope and a connection scope
    /// </summary>
    public class DbTransactionScope : IDisposable
    {
        [ThreadStatic]
        private static bool s_isInUse;

        [ThreadStatic]
        private static Stack<DbTransactionScope> s_scopes;

        private bool m_isDisposed;
        private TransactionScope m_scope;
        private DbConnectionScope m_connScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionScope"/> class.
        /// </summary>
        public DbTransactionScope()
        {
            m_scope = new TransactionScope();
            Initialize();
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
            m_scope = new TransactionScope(transactionToUse);
            Initialize();
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
            m_scope = new TransactionScope(scopeOption);
            Initialize();
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
            m_scope = new TransactionScope(transactionToUse, scopeTimeout);
            Initialize();
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
            m_scope = new TransactionScope(scopeOption, scopeTimeout);
            Initialize();
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
            m_scope = new TransactionScope(scopeOption, transactionOptions);
            Initialize();
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
            m_scope = new TransactionScope(transactionToUse, scopeTimeout, interopOption);
            Initialize();
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
            m_scope = new TransactionScope(scopeOption, transactionOptions, interopOption);
            Initialize();
        }

        /// <summary>
        /// Indicates that all operations within the scope are completed successfully.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">This method has already been called once.</exception>
        public virtual void Complete()
        {
            m_scope.Complete();
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
                currentConnScope.AllowConnectionInheritance = false;
            }

            // Create a default DbTransactionScope for this transaction
            m_connScope = new DbConnectionScope();

            if (s_scopes == null)
            {
                s_scopes = new Stack<DbTransactionScope>();
            }
            s_scopes.Push(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (!IsDisposed)
            {
                if (!object.ReferenceEquals(s_scopes.Peek(), this))
                {
                    throw new BaseException("Transaction scopes mismanaged. Was there a missing Dispose?");
                }
                s_scopes.Pop();

                m_isDisposed = true;
                m_connScope.Dispose();
                m_scope.Dispose();
            }
            else
            {
                throw new ObjectDisposedException(typeof(DbTransactionScope).FullName);
            }
        }

        /// <summary>
        /// Ares the transactions nested.
        /// </summary>
        /// <returns></returns>
        public static bool AreTransactionsNested()
        {
            if (s_scopes != null && s_scopes.Count > 1)
            {
                int uniqueTransactionScopes = s_scopes.Count;
                //foreach (DbTransactionScope scope in s_scopes)
                //{
                //}
                return uniqueTransactionScopes > 1;
            }
            return false;
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
    }
}
