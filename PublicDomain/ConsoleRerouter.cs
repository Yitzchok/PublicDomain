using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsoleRerouter : IDisposable
    {
        private TextWriter m_oldOut, m_oldError;
        private TextReader m_oldIn;
        private StringWriter m_stringWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleRerouter"/> class.
        /// </summary>
        /// <param name="sb">The sb.</param>
        public ConsoleRerouter(StringBuilder sb)
        {
            if (sb != null)
            {
                m_stringWriter = new StringWriter(sb);
                SetStreams(m_stringWriter, m_stringWriter, null);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleRerouter"/> class.
        /// </summary>
        /// <param name="consoleOut">The console out.</param>
        public ConsoleRerouter(TextWriter consoleOut)
            : this(consoleOut, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleRerouter"/> class.
        /// </summary>
        /// <param name="consoleOut">The console out.</param>
        /// <param name="useOutForError">if set to <c>true</c> [use out for error].</param>
        public ConsoleRerouter(TextWriter consoleOut, bool useOutForError)
            : this(consoleOut, useOutForError ? consoleOut : null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleRerouter"/> class.
        /// </summary>
        /// <param name="consoleOut">The console out.</param>
        /// <param name="consoleError">The console error.</param>
        /// <param name="consoleIn">The console in.</param>
        public ConsoleRerouter(TextWriter consoleOut, TextWriter consoleError, TextReader consoleIn)
        {
            SetStreams(consoleOut, consoleError, consoleIn);
        }

        private void SetStreams(TextWriter consoleOut, TextWriter consoleError, TextReader consoleIn)
        {
            if (consoleOut != null)
            {
                m_oldOut = Console.Out;
                Console.SetOut(consoleOut);
            }

            if (consoleError != null)
            {
                m_oldError = Console.Error;
                Console.SetError(consoleError);
            }

            if (consoleIn != null)
            {
                m_oldIn = Console.In;
                Console.SetIn(consoleIn);
            }
        }

        /// <summary>
        /// Gets or sets the out.
        /// </summary>
        /// <value>The out.</value>
        public TextWriter Out
        {
            get
            {
                return Console.Out;
            }
            set
            {
                Console.SetOut(value);
            }
        }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public TextWriter Error
        {
            get
            {
                return Console.Error;
            }
            set
            {
                Console.SetError(value);
            }
        }

        /// <summary>
        /// Gets or sets the in.
        /// </summary>
        /// <value>The in.</value>
        public TextReader In
        {
            get
            {
                return Console.In;
            }
            set
            {
                Console.SetIn(value);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            List<Exception> exceptions = new List<Exception>();

            if (m_oldError != null)
            {
                try
                {
                    Console.SetError(m_oldError);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (m_oldOut != null)
            {
                try
                {
                    Console.SetOut(m_oldOut);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (m_oldIn != null)
            {
                try
                {
                    Console.SetIn(m_oldIn);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (m_stringWriter != null)
            {
                m_stringWriter.Dispose();
            }

            ExceptionUtilities.ThrowExceptionList(exceptions);
        }
    }
}
