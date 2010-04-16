using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// Generic class that encapsulates three objects of any type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    [Serializable]
    public class Triple<T, U, V>
    {
        /// <summary>
        /// 
        /// </summary>
        public T First;

        /// <summary>
        /// 
        /// </summary>
        public U Second;

        /// <summary>
        /// 
        /// </summary>
        public V Third;

        /// <summary>
        /// Initializes a new instance of the <see cref="Triple&lt;T, U, V&gt;"/> class.
        /// </summary>
        public Triple()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Triple&lt;T, U, V&gt;"/> class.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="third">The third.</param>
        public Triple(T first, U second, V third)
        {
            First = first;
            Second = second;
            Third = third;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return First + "," + Second + "," + Third;
        }
    }
}
