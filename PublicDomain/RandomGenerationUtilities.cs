using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public static class RandomGenerationUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        public static Random RNG;

        static RandomGenerationUtilities()
        {
            RNG = new Random(unchecked((int)DateTime.UtcNow.Ticks));
        }

        /// <summary>
        /// Gets a random integer.
        /// </summary>
        /// <returns></returns>
        public static int GetRandomInteger()
        {
            return RNG.Next();
        }

        /// <summary>
        /// Gets a random integer.
        /// </summary>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static int GetRandomInteger(int max)
        {
            return RNG.Next(max);
        }

        /// <summary>
        /// Gets a random integer.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static int GetRandomInteger(int min, int max)
        {
            return RNG.Next(min, max);
        }
    }
}
