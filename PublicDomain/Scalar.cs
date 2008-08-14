using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Scalar<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public T Value;

        /// <summary>
        /// 
        /// </summary>
        public Scalar()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public Scalar(T value)
        {
            Value = value;
        }
    }
}
