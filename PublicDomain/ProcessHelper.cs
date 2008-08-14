using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace PublicDomain
{
    /// <summary>
    /// Wrapper around the Process class to add some convenience methods but
    /// most importantly deal with the complex nature of getting both
    /// StandardOutput and StandardError streams concurrently (this must be done with
    /// callbacks). See http://msdn2.microsoft.com/en-us/library/system.diagnostics.process.standarderror.aspx
    /// </summary>
    [Obsolete("Use IronProcess Instead")]
    public class ProcessHelper
    {
        private Process m_process = new Process();
        private TextWriter m_error = Console.Error;
        private TextWriter m_out = Console.Out;
        private StringBuilder m_outBuilder;
        private StringBuilder m_errorBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessHelper"/> class.
        /// </summary>
        public ProcessHelper()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessHelper"/> class.
        /// </summary>
        /// <param name="sendStreamsToStrings">if set to <c>true</c> [send streams to strings].</param>
        public ProcessHelper(bool sendStreamsToStrings)
        {
            m_process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            m_process.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
            m_process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);

            if (sendStreamsToStrings)
            {
                // Caller wants the standard output and error streams
                // to be written in memory to strings that can later
                // be extracted
                m_outBuilder = new StringBuilder(512);
                m_errorBuilder = new StringBuilder();

                Out = new System.IO.StringWriter(m_outBuilder);
                Error = new System.IO.StringWriter(m_errorBuilder);
            }
        }

        /// <summary>
        /// Parses the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static ProcessHelper Parse(string str)
        {
            // Assumes there is an .exe somwhere
            int exeIndex = str.IndexOf(".exe", StringComparison.CurrentCultureIgnoreCase);
            if (exeIndex != -1)
            {
                if (str.Length > exeIndex + 4 && str[exeIndex + 4] == '\"')
                {
                    exeIndex++;
                }
                string[] pieces = StringUtilities.SplitOn(str, exeIndex + 3, true);
                pieces[0] = pieces[0].Trim();
                pieces[1] = pieces[1].Trim();
                if (pieces[0].Length > 0 && pieces[0][0] == '\"')
                {
                    pieces[0] = pieces[0].Substring(1);
                }
                if (pieces[0].Length > 0 && pieces[0][pieces[0].Length - 1] == '\"')
                {
                    pieces[0] = pieces[0].Substring(0, pieces[0].Length - 1);
                }
                ProcessHelper result = new ProcessHelper();
                result.FileName = pieces[0];
                result.Arguments = pieces[1];
                return result;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get
            {
                return m_process.StartInfo.FileName;
            }
            set
            {
                m_process.StartInfo.FileName = value;
            }
        }

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string Arguments
        {
            get
            {
                return m_process.StartInfo.Arguments;
            }
            set
            {
                m_process.StartInfo.Arguments = value;
            }
        }

        /// <summary>
        /// Sets the arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        public void SetArguments(params string[] args)
        {
            if (args != null)
            {
                StringBuilder sb = GetMangledArguments(args);
                Arguments = sb.ToString();
            }
        }

        /// <summary>
        /// Gets the mangled arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        private static StringBuilder GetMangledArguments(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            int c = 0;
            bool containsSpace;
            foreach (string val in args)
            {
                if (c > 0)
                {
                    sb.Append(' ');
                }
                containsSpace = (val.IndexOf(' ') != -1);
                if (containsSpace)
                {
                    sb.Append('\"');
                }
                sb.Append(val);
                if (containsSpace)
                {
                    sb.Append('\"');
                }
                c++;
            }
            return sb;
        }

        /// <summary>
        /// Adds the arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        public void AddArguments(params string[] args)
        {
            if (args != null)
            {
                StringBuilder sb = GetMangledArguments(args);
                if (!string.IsNullOrEmpty(Arguments))
                {
                    sb.Insert(0, ' ');
                    sb.Insert(0, Arguments);
                }
                Arguments = sb.ToString();
            }
        }

        /// <summary>
        /// Handles the OutputDataReceived event of the process control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Diagnostics.DataReceivedEventArgs"/> instance containing the event data.</param>
        protected virtual void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                m_out.WriteLine(e.Data);
            }
        }

        /// <summary>
        /// Handles the ErrorDataReceived event of the process control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Diagnostics.DataReceivedEventArgs"/> instance containing the event data.</param>
        protected virtual void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                m_error.WriteLine(e.Data);
            }
        }

        /// <summary>
        /// Gets or sets the process.
        /// </summary>
        /// <value>The process.</value>
        public Process Process
        {
            get
            {
                return m_process;
            }
            set
            {
                m_process = value;
            }
        }

        /// <summary>
        /// Gets the start info.
        /// </summary>
        /// <value>The start info.</value>
        public ProcessStartInfo StartInfo
        {
            get
            {
                return m_process.StartInfo;
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            Start(true);
        }

        /// <summary>
        /// Starts the process, begins asynchronous reads on
        /// both standard output and standard error.
        /// </summary>
        /// <param name="useRedirect">if set to <c>true</c> [use redirect].</param>
        public void Start(bool useRedirect)
        {
            // Initialize the asynchronous stuff
            m_process.StartInfo.UseShellExecute = false;
            if (useRedirect)
            {
                m_process.StartInfo.RedirectStandardError = true;
                m_process.StartInfo.RedirectStandardOutput = true;
            }
            m_process.StartInfo.CreateNoWindow = true;

            m_process.Start();

            if (useRedirect)
            {
                m_process.BeginErrorReadLine();
                m_process.BeginOutputReadLine();
            }
        }

        /// <summary>
        /// Starts with a timeout of <see cref="GlobalConstants.DefaultExecuteSmallProcessTimeout"/>
        /// milliseconds and does not throw an exception when it sees an error, but returns
        /// the standard error and output.
        /// </summary>
        /// <returns></returns>
        public int StartAndWaitForExit()
        {
            return StartAndWaitForExit(false);
        }

        /// <summary>
        /// Starts the and wait for exit.
        /// </summary>
        /// <param name="timeoutMs">The timeout ms.</param>
        /// <returns></returns>
        public int StartAndWaitForExit(int timeoutMs)
        {
            return StartAndWaitForExit(timeoutMs, false);
        }

        /// <summary>
        /// Starts the process, begins asynchronous reads on
        /// both standard output and standard error, and
        /// waits for the process to exit. The return code
        /// of the process is returned.
        /// </summary>
        /// <param name="timeoutMs">The timeout ms.</param>
        /// <param name="throwOnError">if set to <c>true</c> [throw on error].</param>
        /// <returns>Return code of completed process</returns>
        public int StartAndWaitForExit(int timeoutMs, bool throwOnError)
        {
            Start();

            m_process.WaitForExit(timeoutMs);

            int exit = m_process.ExitCode;

            if (throwOnError)
            {
                CheckForError(true);
            }

            return exit;
        }

        /// <summary>
        /// Starts the process, begins asynchronous reads on
        /// both standard output and standard error, and
        /// waits for the process to exit. The return code
        /// of the process is returned.
        /// </summary>
        /// <param name="throwOnError">if set to <c>true</c> [throw on error].</param>
        /// <returns>Return code of completed process</returns>
        public int StartAndWaitForExit(bool throwOnError)
        {
            return StartAndWaitForExit(GlobalConstants.DefaultExecuteSmallProcessTimeout, throwOnError);
        }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public TextWriter Error
        {
            get
            {
                return m_error;
            }
            set
            {
                m_error = value;
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
                return m_out;
            }
            set
            {
                m_out = value;
            }
        }

        /// <summary>
        /// Gets the exit code.
        /// </summary>
        /// <value>The exit code.</value>
        public int ExitCode
        {
            get
            {
                return m_process.ExitCode;
            }
        }

        /// <summary>
        /// Gets the standard output.
        /// </summary>
        /// <value>The standard output.</value>
        public string StandardOutput
        {
            get
            {
                return m_outBuilder == null ? null : m_outBuilder.ToString();
            }
        }

        /// <summary>
        /// Gets the standard error.
        /// </summary>
        /// <value>The standard error.</value>
        public string StandardError
        {
            get
            {
                return m_errorBuilder == null ? null : m_errorBuilder.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                string result = FileName;
                if (result.IndexOf(' ') != -1)
                {
                    result = "\"" + result + "\"";
                }

                if (!string.IsNullOrEmpty(Arguments))
                {
                    result += " " + Arguments;
                }
                return result;
            }
            return base.ToString();
        }

        /// <summary>
        /// Checks for error.
        /// </summary>
        /// <returns></returns>
        public string CheckForError()
        {
            return CheckForError(true);
        }

        /// <summary>
        /// Checks for error.
        /// </summary>
        /// <param name="throwOnError">if set to <c>true</c> [throw on error].</param>
        /// <returns></returns>
        public string CheckForError(bool throwOnError)
        {
            string error = null;
            if (ExitCode != 0)
            {
                error = "" + StandardError + "\n" + StandardOutput;
            }
            else if (!string.IsNullOrEmpty(StandardError))
            {
                error = StandardError + "\n" + StandardOutput;
            }

            if (throwOnError && error != null)
            {
                throw new Exception(error);
            }

            return error;
        }
    }
}
