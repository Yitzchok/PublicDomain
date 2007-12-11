using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public static class MathUtilities
    {
        /// <summary>
        /// Gets the average.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static double GetAverage(double[] data)
        {
            int len = data.Length;

            if (len == 0)
            {
                return 0;
            }

            double sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum / len;
        }

        /// <summary>
        /// Gets the variance.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static double GetVariance(double[] data)
        {
            return GetVariance(data, 0);
        }

        /// <summary>
        /// Gets the variance.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="avg">The avg.</param>
        /// <returns></returns>
        public static double GetVariance(double[] data, double avg)
        {
            int len = data.Length;

            if (len == 0)
            {
                return 0;
            }

            // Get average
            if (avg == 0)
            {
                avg = GetAverage(data);
            }

            double sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += Math.Pow((data[i] - avg), 2);
            }
            return sum / len;
        }

        /// <summary>
        /// Gets the standard deviation.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static double GetStandardDeviation(double[] data)
        {
            return GetStandardDeviation(data, 0);
        }

        /// <summary>
        /// Gets the standard deviation.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="avg">The avg.</param>
        /// <returns></returns>
        public static double GetStandardDeviation(double[] data, double avg)
        {
            return Math.Sqrt(GetVariance(data, avg));
        }
    }
}
