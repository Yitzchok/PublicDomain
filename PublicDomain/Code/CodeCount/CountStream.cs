using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Code.CodeCount
{
    /// <summary>
    /// Abstract stream of countable items
    /// </summary>
    public abstract class CountStream : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected ICountable m_first;

        /// <summary>
        /// 
        /// </summary>
        protected long? m_length;

        /// <summary>
        /// 
        /// </summary>
        protected CountStreamType m_type;

        /// <summary>
        /// 
        /// </summary>
        protected string m_location;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountStream"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="location">The location.</param>
        public CountStream(CountStreamType type, string location)
        {
            m_type = type;
            m_location = location;
            switch (type)
            {
                case CountStreamType.Directory:
                    m_first = new CountableDirectory(location);
                    break;
                case CountStreamType.VSSolution2005:
                    m_first = new DotNetSolution(location);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Opens the specified type.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static CountStream Open(string location, CountStreamType type)
        {
            return new DepthFirstCountStream(type, location);
        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        public abstract ICountable Read();

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public abstract void Cancel();

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <returns></returns>
        public long GetLength()
        {
            if (m_length == null)
            {
                m_length = 0;

                // Create a temporary stream with the same type
                // and location, and we will count up how many countables
                // we have
                using (CountStream privateStream = CountStream.Open(m_location, m_type))
                {
                    while (privateStream.Read() != null)
                    {
                        m_length++;
                    }
                }
            }
            return m_length.Value;
        }
    }
}
