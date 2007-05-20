using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public static class ThreadingUtilities
    {
        /// <summary>
        /// Sets the timer.
        /// </summary>
        /// <param name="afterMilliseconds">The after milliseconds.</param>
        /// <param name="d">The d.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static Thread SetTimer(int afterMilliseconds, Delegate d, params object[] args)
        {
            Thread result = new Thread(new ParameterizedThreadStart(ExecuteTimer));
            result.Start(new Triple<int, Delegate, object[]>(afterMilliseconds, d, args));
            return result;
        }

        /// <summary>
        /// Sets the timer simple.
        /// </summary>
        /// <param name="afterMilliseconds">The after milliseconds.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public static Thread SetTimerSimple(int afterMilliseconds, CallbackNoArgs callback)
        {
            return SetTimer(afterMilliseconds, callback);
        }

        /// <summary>
        /// Sets the interval.
        /// </summary>
        /// <param name="periodMilliseconds">The period milliseconds.</param>
        /// <param name="d">The d.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static Thread SetInterval(int periodMilliseconds, Delegate d, params object[] args)
        {
            Thread result = new Thread(new ParameterizedThreadStart(ExecuteInterval));
            result.Start(new Triple<int, Delegate, object[]>(periodMilliseconds, d, args));
            return result;
        }

        /// <summary>
        /// Sets the interval simple.
        /// </summary>
        /// <param name="periodMilliseconds">The period milliseconds.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public static Thread SetIntervalSimple(int periodMilliseconds, CallbackNoArgs callback)
        {
            return SetInterval(periodMilliseconds, callback);
        }

        private static void ExecuteTimer(object rock)
        {
            Triple<int, Delegate, object[]> state = (Triple<int, Delegate, object[]>)rock;
            Thread.Sleep(state.First);
            state.Second.DynamicInvoke(state.Third);
        }

        private static void ExecuteInterval(object rock)
        {
            while (true)
            {
                ExecuteTimer(rock);
            }
        }
    }
}
