using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// Generic class that encapsulates a pair of objects of any types. This class is similar
    /// to System.Collections.Generic.KeyValuePair except that it is a class, not
    /// a struct and also exposes the values as public fields and more generically than a "key" and
    /// a "value."
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    [Serializable]
    public class Pair<T, U>
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
        /// Initializes a new instance of the <see cref="Pair&lt;T, U&gt;"/> class.
        /// </summary>
        public Pair()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pair&lt;T, U&gt;"/> class.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        public Pair(T first, U second)
        {
            First = first;
            Second = second;
        }

        /// <summary>
        /// Finds the pair by key.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="find">The find.</param>
        /// <returns></returns>
        public static Pair<T, U> FindPairByKey(Pair<T, U>[] search, T find)
        {
            if (search != null)
            {
                foreach (Pair<T, U> item in search)
                {
                    if (item.First.Equals(find))
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return First + "," + Second;
        }
    }
}
