using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using PublicDomain;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Set<T> : IEnumerable<T>
    {
        private Dictionary<T, object> m_set = new Dictionary<T, object>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            return m_set.Keys.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_set.Keys.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Put(T obj)
        {
            m_set[obj] = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Contains(T obj)
        {
            object trash;
            return m_set.TryGetValue(obj, out trash);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Remove(T obj)
        {
            return m_set.Remove(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_set.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Count
        {
            get
            {
                return m_set.Keys.Count;
            }
        }
    }
}
