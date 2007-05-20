using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICryptographer
    {
        /// <summary>
        /// Encrypts the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        string Encrypt(string str);

        /// <summary>
        /// Decrypts the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        string Decrypt(string str);
    }

    /// <summary>
    /// 
    /// </summary>
    public enum CryptographyAlgorithm
    {
        /// <summary>
        /// 
        /// </summary>
        RC2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum CryptographyHashAlgorithm
    {
        /// <summary>
        /// 
        /// </summary>
        SHA1
    }
}
