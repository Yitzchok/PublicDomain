using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace PublicDomain
{
    /// <summary>
    /// Make sure to put this in a using() clause to dispose the process
    /// </summary>
    public class IronProcess : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public const int DefaultExecutionTimeout = 120000;

        /// <summary>
        /// 
        /// </summary>
        public const int DefaultThreadJoinTimeout = 10000;

        private Process m_process;
        private ProcessStartInfo m_startInfo;
        private Thread m_stderrThread;
        private Thread m_stdoutThread;
        private string m_stdout;
        private string m_stderr;
        private int m_exitCode;

        /// <summary>
        /// Create a new IronProcess which wraps a Process. The underlying Process is not automatically
        /// started. This will create a Process which has common optimizations such as
        /// redirecting all streams to strings, using no shell window, etc. This is the preferred constructor.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="args"></param>
        public IronProcess(string fileName, params string[] args)
        {
            Reset(fileName, args);
        }

        /// <summary>
        /// Create a new IronProcess which wraps a Process. The underlying Process is not automatically
        /// started. This constructor does not modify the <paramref name="startInfo"/>,
        /// and should only be used when requiring fine control over this IronProcess. Otherwise,
        /// the other overloads should be used which prepares this class to do common
        /// optimizations such as reading streams into strings, etc.
        /// </summary>
        /// <param name="startInfo"></param>
        public IronProcess(ProcessStartInfo startInfo)
        {
            Reset(startInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="args"></param>
        public void Reset(string fileName, string[] args)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                if (File.Exists(fileName))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();

                    // It is required that if one of the redirections is true,
                    // then they must all be true
                    startInfo.RedirectStandardError = true;
                    startInfo.RedirectStandardInput = true;
                    startInfo.RedirectStandardOutput = true;

                    startInfo.CreateNoWindow = true;
                    startInfo.ErrorDialog = false;
                    startInfo.UseShellExecute = false;
                    startInfo.WorkingDirectory = Path.GetDirectoryName(fileName);
                    startInfo.LoadUserProfile = false;

                    //fileName = Environment.ExpandEnvironmentVariables(fileName);
                    startInfo.FileName = fileName;
                    startInfo.Arguments = GetMangledArguments(args).ToString();

                    Reset(startInfo);
                }
                else
                {
                    throw new ApplicationException("Filename " + fileName + " specified to start process does not exist");
                }
            }
            else
            {
                throw new ArgumentNullException("Filename of process to start not specified");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startInfo"></param>
        public void Reset(ProcessStartInfo startInfo)
        {
            m_startInfo = startInfo;
            m_process = Process.Start(startInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int StartAndWaitForExit()
        {
            return StartAndWaitForExit(DefaultExecutionTimeout);
        }

        /// <summary>
        /// 
        /// </summary>
        public int StartAndWaitForExit(int executionTimeout)
        {
            if ((m_stdoutThread != null && StartInfo.RedirectStandardOutput) ||
                (m_stderrThread != null && StartInfo.RedirectStandardError))
            {
                // threads still running?
                throw new NotImplementedException();
            }

            if (StartInfo.RedirectStandardOutput)
            {
                // TODO thread pool?
                m_stdoutThread = new Thread(ThreadReadStdout);
                m_stdoutThread.IsBackground = true;
            }

            if (StartInfo.RedirectStandardError)
            {
                m_stderrThread = new Thread(ThreadReadStderr);
                m_stderrThread.IsBackground = true;
            }

            m_process.Start();

            if (StartInfo.RedirectStandardOutput)
            {
                m_stdoutThread.Start();
            }
            if (StartInfo.RedirectStandardError)
            {
                m_stderrThread.Start();
            }

            try
            {
                m_process.WaitForExit(executionTimeout);
            }
            catch (Exception)
            {
                // An exception here could be a timeout exception, security, etc.
                // so we go ahead and kill the stream threads
                if (m_stdoutThread.IsAlive)
                {
                    try
                    {
                        m_stdoutThread.Abort();
                    }
                    catch (Exception)
                    {
                    }
                }

                if (m_stderrThread.IsAlive)
                {
                    try
                    {
                        m_stderrThread.Abort();
                    }
                    catch (Exception)
                    {
                    }
                }
                throw;
            }

            if (StartInfo.RedirectStandardOutput)
            {
                // We don't expect to wait long to join since
                // the process has finished, so it's just
                // a matter of serializing the stream across
                m_stdoutThread.Join(DefaultThreadJoinTimeout);
            }
            if (StartInfo.RedirectStandardError)
            {
                m_stderrThread.Join(DefaultThreadJoinTimeout);
            }

            m_exitCode = m_process.ExitCode;

            return m_exitCode;
        }

        /// <summary>
        /// Start the process asynchronously. Do not read standard input or standard
        /// error streams
        /// </summary>
        public void Spawn()
        {
            m_process.Start();
        }

        /// <summary>
        /// Returns the ProcessStartInfo which was used to spawn the process.
        /// </summary>
        public ProcessStartInfo StartInfo
        {
            get
            {
                return m_startInfo;
            }
        }

        private void ThreadReadStdout()
        {
            m_stdout = m_process.StandardOutput.ReadToEnd();
        }

        private void ThreadReadStderr()
        {
            m_stderr = m_process.StandardError.ReadToEnd();
        }

        /// <summary>
        /// 
        /// </summary>
        public int ExitCode
        {
            get
            {
                return m_exitCode;
            }
        }

        /// <summary>
        /// Returns non-null string representing the
        /// data read from the Standard Output of the
        /// last executed process. If there is no data,
        /// or the standardout output is not ever read due
        /// to other exceptions, the result will still
        /// be an empty string and never null.
        /// </summary>
        public string StandardOutput
        {
            get
            {
                if (m_stdout == null)
                {
                    m_stdout = string.Empty;
                }
                return m_stdout;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string StandardError
        {
            get
            {
                return m_stderr;
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
            if (args != null)
            {
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
            }
            return sb;
        }

        /// <summary>
        /// Adds the arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        public void AddArguments(params string[] args)
        {
            StringBuilder sb = GetMangledArguments(args);
            if (!string.IsNullOrEmpty(StartInfo.Arguments))
            {
                sb.Insert(0, ' ');
                sb.Insert(0, StartInfo.Arguments);
            }
            StartInfo.Arguments = sb.ToString();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(StartInfo.FileName))
            {
                string result = StartInfo.FileName;
                if (result.IndexOf(' ') != -1)
                {
                    result = "\"" + result + "\"";
                }

                if (!string.IsNullOrEmpty(StartInfo.Arguments))
                {
                    result += " " + StartInfo.Arguments;
                }
                return result;
            }
            return base.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ThrowDetailsAsException()
        {
            ThrowDetailsAsException(
                "Process {0} failed. Exit code={1}. Standard Error={2}, Standard Output={3}"
            );
        }

        /// <summary>
        /// string.Format parameters:
        /// {0} = Process Filename
        /// {1} = Exit code
        /// {2} = Process Standard Error
        /// {3} = Process Standard Output
        /// </summary>
        /// <param name="format"></param>
        public void ThrowDetailsAsException(string format)
        {
            string msg = string.Format(format, StartInfo.FileName, ExitCode, StandardError, StandardOutput);
            throw new ApplicationException(msg);
        }

        /// <summary>
        /// This starts the process, waits for it to exit, then checks
        /// the return code and the standard error. If either the
        /// return code is not 0 or there is any non-whitespace data
        /// in standard error, then an exception is thrown.
        /// This entire operation is done synchronously.
        /// </summary>
        public void Run()
        {
            int result = StartAndWaitForExit();
            if (result == 0)
            {
                if (StandardError != null)
                {
                    if (StandardError.Trim().Length > 0)
                    {
                        ThrowDetailsAsException();
                    }
                }
            }
            else
            {
                ThrowDetailsAsException();
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
                StartInfo.Arguments = sb.ToString();
            }
        }

        /// <summary>
        /// Sets the arguments raw.
        /// </summary>
        /// <param name="argLine">The arg line.</param>
        public void SetArgumentsRaw(string argLine)
        {
            StartInfo.Arguments = argLine;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (m_process != null)
            {
                m_process.Dispose();
                m_process = null;
            }
        }

        /// <summary>
        /// Parses the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static IronProcess Parse(string str)
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
                IronProcess result = new IronProcess(pieces[0]);
                result.SetArgumentsRaw(pieces[1]);
                return result;
            }
            throw new NotImplementedException();
        }
    }
}
