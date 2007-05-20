using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// Generic class that encapsulates four objects of any type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <typeparam name="W"></typeparam>
    [Serializable]
    public class Quadruple<T, U, V, W>
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
        /// 
        /// </summary>
        public W Fourth;

        /// <summary>
        /// Initializes a new instance of the <see cref="Quadruple&lt;T, U, V, W&gt;"/> class.
        /// </summary>
        public Quadruple()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quadruple&lt;T, U, V, W&gt;"/> class.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="third">The third.</param>
        /// <param name="fourth">The fourth.</param>
        public Quadruple(T first, U second, V third, W fourth)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return First + "," + Second + "," + Third + "," + Fourth;
        }
    }
}
