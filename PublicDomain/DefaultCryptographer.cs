using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.IO;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class DefaultCryptographer : ICryptographer
    {
        private byte[] m_key;
        private byte[] m_iv;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCryptographer"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        internal DefaultCryptographer(byte[] key, byte[] iv)
        {
            m_key = key;
            m_iv = iv;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            RC2CryptoServiceProvider provider = new RC2CryptoServiceProvider();
            provider.Key = m_key;
            provider.IV = m_iv;
            byte[] strbytes = Encoding.ASCII.GetBytes(str);
            byte[] encrypted;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(strbytes, 0, strbytes.Length);
                    encrypted = ms.ToArray();
                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            byte[] encrypted = Convert.FromBase64String(str);
            RC2CryptoServiceProvider provider = new RC2CryptoServiceProvider();
            provider.Key = m_key;
            provider.IV = m_iv;
            byte[] buffer = new byte[encrypted.Length];
            using (MemoryStream ms = new MemoryStream(encrypted))
            {
                using (CryptoStream cs = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    int bytesRead = cs.Read(buffer, 0, buffer.Length);

                    return new ASCIIEncoding().GetString(buffer, 0, bytesRead);
                }
            }
        }
    }
}
