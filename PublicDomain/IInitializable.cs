using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// Initializes the specified stage.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        int Initialize(InitializeState state);
    }

    /// <summary>
    /// 
    /// </summary>
    public enum InitializeState : int
    {
        /// <summary>
        /// 
        /// </summary>
        Nothing = 0,

        /// <summary>
        /// Only initialize what is absolutely required
        /// </summary>
        Minimum = 1,

        /// <summary>
        /// Initialize the minimum as well as other basics
        /// </summary>
        Basic = 2,

        /// <summary>
        /// 
        /// </summary>
        Stage1 = 3,

        /// <summary>
        /// 
        /// </summary>
        Stage2 = 4,

        /// <summary>
        /// 
        /// </summary>
        Stage3 = 5,

        /// <summary>
        /// 
        /// </summary>
        Everything = unchecked((int)-1),
    }
}
