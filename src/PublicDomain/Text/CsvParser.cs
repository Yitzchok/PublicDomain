using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public static class CsvParser
    {
        /// <summary>
        /// 
        /// </summary>
        public const char DefaultSeparator = ',';

        /// <summary>
        /// 
        /// </summary>
        public const char DefaultQuoteEncapsulation = '\"';

        /// <summary>
        /// \"
        /// </summary>
        public const char DefaultQuoteReplacementPrefix = '\\';

        /// <summary>
        /// 
        /// </summary>
        public const char DefaultKeyValueDelimeter = '=';

        /// <summary>
        /// Deserializes the specified data. If data is null or an empty
        /// string, null is returned.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static Dictionary<string, string> Deserialize(string data)
        {
            return DeserializeWithOptions(data);
        }

        /// <summary>
        /// Deserializes the with options.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static Dictionary<string, string> DeserializeWithOptions(string data)
        {
            return DeserializeWithOptions(data, DefaultSeparator, DefaultQuoteEncapsulation, DefaultQuoteReplacementPrefix, DefaultKeyValueDelimeter);
        }

        /// <summary>
        /// Deserializes the with options.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static Dictionary<string, string> DeserializeWithOptions(string data, char separator)
        {
            return DeserializeWithOptions(data, separator, DefaultQuoteEncapsulation, DefaultQuoteReplacementPrefix, DefaultKeyValueDelimeter);
        }

        /// <summary>
        /// Deserializes the with options.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="quoteEncapsulation">The quote encapsulation.</param>
        /// <returns></returns>
        public static Dictionary<string, string> DeserializeWithOptions(string data, char separator, char quoteEncapsulation)
        {
            return DeserializeWithOptions(data, separator, quoteEncapsulation, DefaultQuoteReplacementPrefix, DefaultKeyValueDelimeter);
        }

        /// <summary>
        /// Deserializes the with options.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="quoteEncapsulation">The quote encapsulation.</param>
        /// <param name="quoteReplacementPrefix">The quote replacement.</param>
        /// <returns></returns>
        public static Dictionary<string, string> DeserializeWithOptions(string data, char separator, char quoteEncapsulation, char quoteReplacementPrefix)
        {
            return DeserializeWithOptions(data, separator, quoteEncapsulation, quoteReplacementPrefix, DefaultKeyValueDelimeter);
        }

        /// <summary>
        /// Deserializes the with options. If data is null or an empty
        /// string, null is returned.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="quoteEncapsulation">The quote encapsulation.</param>
        /// <param name="quoteReplacementPrefix">The quote replacement.</param>
        /// <param name="keyValueDelimeter">The key value delimeter.</param>
        /// <returns></returns>
        public static Dictionary<string, string> DeserializeWithOptions(string data, char separator, char quoteEncapsulation, char quoteReplacementPrefix, char keyValueDelimeter)
        {
            if (!string.IsNullOrEmpty(data))
            {
                Dictionary<string, string> result = new Dictionary<string, string>();

                int l = data.Length;
                string name = null, val = null;
                State state = State.Key;
                StringBuilder sb = new StringBuilder();
                char c;
                bool inQuotes = false;

                for (int i = 0; i < l; i++)
                {
                    c = data[i];

                    if (c == separator)
                    {
                        switch (state)
                        {
                            case State.Value:
                                val = sb.ToString();
                                sb.Length = 0;
                                state = State.Key;
                                result[name] = val;
                                name = val = null;

                                break;
                            default:
                                sb.Append(c);
                                break;
                        }
                    }
                    else if (c == quoteEncapsulation)
                    {
                        switch (state)
                        {
                            case State.Value:

                                if (inQuotes)
                                {
                                    bool done = false;

                                    // We're only done with the value, iff the previous
                                    // character was not the quoteReplacementPrefix
                                    if (sb.Length > 0)
                                    {
                                        if (sb[sb.Length - 1] != quoteReplacementPrefix)
                                        {
                                            done = true;
                                            val = sb.ToString();
                                            sb.Length = 0;
                                            state = State.Key;
                                            result[name] = val;
                                            name = val = null;
                                        }
                                    }

                                    if (!done)
                                    {
                                        sb.Append(c);
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        // basically, if the first character of the value
                                        // is the quoteEncapsulation, then we assume
                                        // that the whole value is encapsulated
                                        inQuotes = true;
                                    }
                                    else
                                    {
                                        sb.Append(c);
                                    }
                                }

                                break;
                            default:
                                sb.Append(c);
                                break;
                        }
                    }
                    else if (c == keyValueDelimeter)
                    {
                        switch (state)
                        {
                            case State.Key:
                                name = sb.ToString();
                                sb.Length = 0;
                                state = State.Value;
                                inQuotes = false;

                                break;
                            default:
                                sb.Append(c);
                                break;
                        }
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }

                if (name != null)
                {
                    if (sb.Length > 0)
                    {
                        switch (state)
                        {
                            case State.Key:
                                name = sb.ToString();
                                break;
                            case State.Value:
                                val = sb.ToString();
                                break;
                        }
                    }

                    result[name] = val;
                }

                return result;
            }
            else
            {
                return null;
            }
        }

        private enum State
        {
            Key,
            Value
        }

        /// <summary>
        /// Serializes the specified data. If there is no data,
        /// null is returned.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string Serialize(params object[] data)
        {
            return SerializeWithOptions(data, DefaultSeparator, DefaultQuoteEncapsulation, DefaultQuoteReplacementPrefix, DefaultKeyValueDelimeter);
        }

        /// <summary>
        /// Serializes the with options.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string SerializeWithOptions(object[] data)
        {
            return SerializeWithOptions(data, DefaultSeparator, DefaultQuoteEncapsulation, DefaultQuoteReplacementPrefix, DefaultKeyValueDelimeter);
        }

        /// <summary>
        /// Serializes the with options.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string SerializeWithOptions(object[] data, char separator)
        {
            return SerializeWithOptions(data, separator, DefaultQuoteEncapsulation, DefaultQuoteReplacementPrefix, DefaultKeyValueDelimeter);
        }

        /// <summary>
        /// Serializes the with options.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="quoteEncapsulation">The quote encapsulation.</param>
        /// <returns></returns>
        public static string SerializeWithOptions(object[] data, char separator, char quoteEncapsulation)
        {
            return SerializeWithOptions(data, separator, quoteEncapsulation, DefaultQuoteReplacementPrefix, DefaultKeyValueDelimeter);
        }

        /// <summary>
        /// Serializes the with options.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="quoteEncapsulation">The quote encapsulation.</param>
        /// <param name="quoteReplacementPrefix">The quote replacement.</param>
        /// <returns></returns>
        public static string SerializeWithOptions(object[] data, char separator, char quoteEncapsulation, char quoteReplacementPrefix)
        {
            return SerializeWithOptions(data, separator, quoteEncapsulation, quoteReplacementPrefix, DefaultKeyValueDelimeter);
        }

        /// <summary>
        /// Serializes the with options. If there is no data,
        /// null is returned.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="quoteEncapsulation">The quote encapsulation.</param>
        /// <param name="quoteReplacementPrefix">The quote replacement.</param>
        /// <param name="keyValueDelimeter">The key value delimeter.</param>
        /// <returns></returns>
        public static string SerializeWithOptions(object[] data, char separator, char quoteEncapsulation, char quoteReplacementPrefix, char keyValueDelimeter)
        {
            if (data != null && data.Length > 0)
            {
                StringBuilder sb = new StringBuilder();

                if (data != null)
                {
                    int l = data.Length;

                    if ((l % 2) != 0)
                    {
                        throw new ArgumentException("Data length must be a multiple of two");
                    }

                    string name, val;
                    object obj;
                    bool acted;
                    string quoteReplacement = quoteReplacementPrefix.ToString() + quoteEncapsulation;

                    for (int i = 0; i < l; i += 2)
                    {
                        if (i > 0)
                        {
                            sb.Append(separator);
                        }
                        obj = data[i];
                        if (obj == null)
                        {
                            throw new ArgumentNullException("One of the key names is null");
                        }
                        name = obj.ToString().Trim();

                        if (name.Length == 0)
                        {
                            throw new ArgumentException("One of the key names is empty");
                        }

                        // Make sure the key name doesn't have the separator character

                        if (name.IndexOf(separator) != -1)
                        {
                            throw new ArgumentException("One of the key names includes the separator character: " + separator);
                        }
                        else if (name.IndexOf(keyValueDelimeter) != -1)
                        {
                            throw new ArgumentException("One of the key names includes the key value delimiter character: " + keyValueDelimeter);
                        }

                        sb.Append(name);
                        sb.Append(keyValueDelimeter);

                        acted = false;
                        obj = data[i + 1];

                        if (obj == null)
                        {
                            val = string.Empty;
                        }
                        else
                        {
                            val = obj.ToString();

                            if (val.Length > 0)
                            {
                                // Properly encapsulate the value
                                if (val.IndexOf(separator) != -1 || val.IndexOf(quoteEncapsulation) != -1)
                                {
                                    acted = true;

                                    sb.Append(quoteEncapsulation);

                                    val = val.Replace(quoteEncapsulation.ToString(), quoteReplacement);

                                    sb.Append(val);

                                    sb.Append(quoteEncapsulation);
                                }
                            }
                        }

                        if (!acted)
                        {
                            sb.Append(val);
                        }
                    }
                }

                return sb.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
