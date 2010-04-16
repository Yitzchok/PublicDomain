using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace PublicDomain.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class WrappedException : Exception
    {
        private Exception m_wrappedException;

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappedException"/> class.
        /// </summary>
        public WrappedException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappedException"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public WrappedException(Exception exception)
            : this(exception, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappedException"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public WrappedException(Exception exception, Exception innerException)
            : base(null, innerException)
        {
            m_wrappedException = exception;
            Data["WrappedException"] = exception;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WrappedException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappedException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected WrappedException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// Gets the wrapped exception.
        /// </summary>
        /// <value>The wrapped exception.</value>
        public Exception ExceptionWrapped
        {
            get
            {
                return m_wrappedException;
            }
        }
    }
}
