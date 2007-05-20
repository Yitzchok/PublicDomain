using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public static class General
    {
        /// <summary>
        /// Determines whether [is flag on] [the specified x].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="flag">The flag.</param>
        /// <returns>
        /// 	<c>true</c> if [is flag on] [the specified x]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFlagOn(int x, int flag)
        {
            return (x & flag) == flag;
        }

        /// <summary>
        /// Determines whether [is flag off] [the specified x].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="flag">The flag.</param>
        /// <returns>
        /// 	<c>true</c> if [is flag off] [the specified x]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFlagOff(int x, int flag)
        {
            return (x & flag) == 0;
        }
    }
}
