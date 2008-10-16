using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using PublicDomain.Exceptions;

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
            if (exceptions != null)
            {
                foreach (Exception ex in exceptions)
                {
                    writer.WriteLine(GetExceptionDetailsAsString(ex));
                }
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

                sb.AppendFormat(@"{0} ({1})", ex.Message, ex.GetType().Name);

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
                    @"Exception ({0}), Type={1}, Message={3}, Stack Trace={4}{2}",
                    i,
                    ex.GetType().Name,
                    ex.StackTrace,
                    ex.Message,
                    Environment.NewLine
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
        /// will be thrown is a <see cref="PublicDomain.Exceptions.WrappedException"/>
        /// with all exceptions as a chain of Inner exceptions in WrappedException objects
        /// </summary>
        /// <param name="exceptions">The exceptions.</param>
        /// <exception cref="PublicDomain.Exceptions.WrappedException">If <paramref name="exceptions"/> has more than one element.</exception>
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
    }
}
