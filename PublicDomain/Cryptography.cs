using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Cryptography
    {
        /// <summary>
        /// Creates the cryptographer. Give random bytes for the last 8 parameters. It's essentially part of password -- the initialization vector.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="algorithm">The algorithm.</param>
        /// <param name="hashAlgorithm">The hash algorithm.</param>
        /// <param name="vector1">The vector1.</param>
        /// <param name="vector2">The vector2.</param>
        /// <param name="vector3">The vector3.</param>
        /// <param name="vector4">The vector4.</param>
        /// <param name="vector5">The vector5.</param>
        /// <param name="vector6">The vector6.</param>
        /// <param name="vector7">The vector7.</param>
        /// <param name="vector8">The vector8.</param>
        /// <returns></returns>
        public static ICryptographer CreateCryptographer(string password, CryptographyAlgorithm algorithm, CryptographyHashAlgorithm hashAlgorithm, byte vector1, byte vector2, byte vector3, byte vector4, byte vector5, byte vector6, byte vector7, byte vector8)
        {
            PasswordDeriveBytes passwd = new PasswordDeriveBytes(password, null);
            byte[] iv = new byte[] { vector1, vector2, vector3, vector4, vector5, vector6, vector7, vector8 };
            byte[] deriveIV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] key = passwd.CryptDeriveKey(algorithm.ToString(), hashAlgorithm.ToString(), 128, deriveIV);
            return new DefaultCryptographer(key, iv);
        }
    }
}
