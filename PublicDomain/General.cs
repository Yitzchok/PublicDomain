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

        /// <summary>
        /// Parses the enum.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        [PendingPublicDomain]
        public static T ParseEnum<T>(string str)
        {
            return (T)Enum.Parse(typeof(T), str, true);
        }

        /// <summary>
        /// Tries the parse enum.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        [PendingPublicDomain]
        public static T TryParseEnum<T>(string str, T defaultValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim();
                string[] names = Enum.GetNames(typeof(T));
                foreach (string name in names)
                {
                    if (name.Equals(str, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return ParseEnum<T>(name);
                    }
                }
            }
            return defaultValue;
        }
    }
}
