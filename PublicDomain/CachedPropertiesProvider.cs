using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public class CachedPropertiesProvider : ICachedPropertiesProvider
    {
        private IDictionary<string, string> m_properties = new Dictionary<string, string>();
        private IDictionary<string, object> m_cache = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public IDictionary<string, string> Properties
        {
            get
            {
                return m_properties;
            }
            set
            {
                m_properties = value;
            }
        }

        /// <summary>
        /// Setters the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public virtual void Setter(string propertyName, object value)
        {
            m_cache[propertyName] = value;
            Properties[propertyName] = value == null ? null : value.ToString();
        }

        /// <summary>
        /// Getters the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public virtual string Getter(string propertyName)
        {
            return Getter<string>(propertyName, ConvertStringToString);
        }

        /// <summary>
        /// Getters the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="convertDelegate">The convert delegate.</param>
        /// <returns></returns>
        public virtual T Getter<T>(string propertyName, Converter<string, T> convertDelegate)
        {
            T returnValue = default(T);
            object dicValue;

            // try the cache
            if (!m_cache.TryGetValue(propertyName, out dicValue))
            {
                if (convertDelegate != null)
                {
                    // We want this to throw an exception if it has a problem
                    // (e.g. Uri can't be parsed).
                    string strVal;
                    if (Properties.TryGetValue(propertyName, out strVal))
                    {
                        returnValue = convertDelegate.Invoke(strVal);
                        m_cache[propertyName] = returnValue;
                    }
                }
            }
            else
            {
                returnValue = (T)dicValue;
            }
            return returnValue;
        }

        /// <summary>
        /// Converts the string to string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ConvertStringToString(string input)
        {
            return input;
        }

        /// <summary>
        /// Assumes input is in UTC.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static TzDateTime ConvertToTzDateTime(string input)
        {
            TzDateTime result;
            TzDateTime.TryParse(input, out result);
            return result;
        }

        /// <summary>
        /// Converts to URI.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static Uri ConvertToUri(string input)
        {
            return string.IsNullOrEmpty(input) ? null : new Uri(input);
        }

        /// <summary>
        /// Converts to culture info.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static CultureInfo ConvertToCultureInfo(string input)
        {
            return string.IsNullOrEmpty(input) ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture(input);
        }

        /// <summary>
        /// Converts to U int nullable.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static uint? ConvertToUIntNullable(string input)
        {
            return string.IsNullOrEmpty(input) ? (uint?)null : uint.Parse(input);
        }

        /// <summary>
        /// Converts to int nullable.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static int? ConvertToIntNullable(string input)
        {
            return string.IsNullOrEmpty(input) ? (int?)null : int.Parse(input);
        }

        /// <summary>
        /// Converts to U int.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static uint ConvertToUInt(string input)
        {
            return uint.Parse(input);
        }

        /// <summary>
        /// Converts to int.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static int ConvertToInt(string input)
        {
            return int.Parse(input);
        }

        /// <summary>
        /// Converts to bool nullable.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static bool? ConvertToBoolNullable(string input)
        {
            return string.IsNullOrEmpty(input) ? (bool?)null : bool.Parse(input);
        }

        /// <summary>
        /// Converts to URI.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <returns></returns>
        public static Uri ConvertToUri(string input, string baseUri)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (string.IsNullOrEmpty(baseUri))
                {
                    return new Uri(input);
                }
                else
                {
                    return new Uri(new Uri(baseUri), input);
                }
            }
            return null;
        }
    }
}
