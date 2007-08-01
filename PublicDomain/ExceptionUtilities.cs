using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace PublicDomain
{
    /// <summary>
    /// Methods to work with Exceptions.
    /// </summary>
    public static class ExceptionUtilities
    {
        /// <summary>
        /// Writes the exceptions.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void WriteExceptions(Exception ex)
        {
            WriteExceptions(Console.Error, ex);
        }

        /// <summary>
        /// Writes the exception.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="exceptions">The exceptions.</param>
        public static void WriteExceptions(TextWriter writer, params Exception[] exceptions)
        {
            foreach (Exception ex in exceptions)
            {
                writer.WriteLine(GetExceptionDetailsAsString(ex));
            }
        }

        /// <summary>
        /// Gets the human readable exception details as a string. This
        /// simply accumulates the Message value of this exception and
        /// all inner exceptions. It does not return programmer related details
        /// such as the stack trace. For that, use the GetExceptionDetailsAsString
        /// method.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static string GetHumanReadableExceptionDetailsAsString(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            while (ex != null)
            {
                if (i > 0)
                {
                    sb.AppendFormat(@"\nRelated to ");
                }

                sb.AppendFormat(@"{0}", ex.Message);

                ex = ex.InnerException;
                i++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the exception details as a string. This will also
        /// gather together all inner exceptions in the result. The results
        /// are not in a human-readable form. For that, see the GetHumanReadable
        /// method.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns></returns>
        public static string GetExceptionDetailsAsString(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            while (ex != null)
            {
                if (i > 0)
                {
                    sb.Append("\nInner ");
                }

                sb.AppendFormat(
                    @"Exception ({0}), Type={1}, Message={3}, Stack Trace={2}",
                    i,
                    ex.GetType().Name,
                    ex.StackTrace,
                    ex.Message
                );

                ex = ex.InnerException;

                i++;
            }
            if (i > 0)
            {
                sb.Append('\n');
            }
            return sb.ToString();
        }

        /// <summary>
        /// Throws the exception list, if there are any. Null and zero-length
        /// lists are permissable and will not throw an Exception. If there
        /// are more than one exceptions, then the ultimate Exception that
        /// will be thrown is a <see cref="PublicDomain.ExceptionUtilities.WrappedException"/>
        /// with all exceptions as a chain of Inner exceptions in WrappedException objects
        /// </summary>
        /// <param name="exceptions">The exceptions.</param>
        /// <exception cref="PublicDomain.ExceptionUtilities.WrappedException">If <paramref name="exceptions"/> has more than one element.</exception>
        public static void ThrowExceptionList(ICollection<Exception> exceptions)
        {
            if (exceptions != null && exceptions.Count > 0)
            {
                List<Exception> exceptionList = new List<Exception>(exceptions);
                if (exceptionList.Count == 1)
                {
                    throw exceptionList[0];
                }
                else
                {
                    // Build a list of exceptions, with the first of the list at the top
                    Exception exception = null;
                    for (int i = exceptionList.Count - 1; i >= 0; i--)
                    {
                        if (exception == null)
                        {
                            exception = exceptionList[i];
                        }
                        else
                        {
                            exception = new WrappedException(exceptionList[i], exception);
                        }
                    }
                    throw exception;
                }
            }
        }

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
}
