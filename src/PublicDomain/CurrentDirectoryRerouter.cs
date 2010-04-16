using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public class CurrentDirectoryRerouter : IDisposable
    {
        private string previousCurrentDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentDirectoryRerouter"/> class.
        /// </summary>
        /// <param name="newCurrentDirectory">The new current directory.</param>
        public CurrentDirectoryRerouter(string newCurrentDirectory)
        {
            string curdir = Directory.GetCurrentDirectory();

            Directory.SetCurrentDirectory(newCurrentDirectory);

            previousCurrentDirectory = curdir;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (previousCurrentDirectory != null)
            {
                Directory.SetCurrentDirectory(previousCurrentDirectory);
            }
        }
    }
}
