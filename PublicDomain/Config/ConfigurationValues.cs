using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Configuration;

namespace PublicDomain.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationValues
    {
        private Dictionary<string, string> m_values = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValues"/> class.
        /// </summary>
        public ConfigurationValues()
        {
        }

        /// <summary>
        /// Reads the parameters from assembly.
        /// </summary>
        /// <param name="assemblyStreamName">Name of the assembly stream.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public ConfigurationValues(string assemblyStreamName, bool intersectAlternateConfig)
            : this(assemblyStreamName, Assembly.GetExecutingAssembly(), intersectAlternateConfig)
        {
        }

        /// <summary>
        /// Reads the parameters from assembly.
        /// </summary>
        /// <param name="assemblyStreamName">Name of the assembly stream.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public ConfigurationValues(string assemblyStreamName, Assembly assembly, bool intersectAlternateConfig)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            Stream stream = assembly.GetManifestResourceStream(assemblyStreamName);
            if (stream == null)
            {
                throw new ArgumentNullException(string.Format("Could not find embedded resource named {0} in assembly {1}.", assemblyStreamName, assembly));
            }
            ReadParametersFromStream(stream, intersectAlternateConfig);
        }

        /// <summary>
        /// Reads the parameters from file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public void ReadParametersFromStream(string fileName, bool intersectAlternateConfig)
        {
            using (TextReader textReader = new StreamReader(fileName))
            {
                ReadParametersFromTextReader(textReader, intersectAlternateConfig);
            }
        }

        /// <summary>
        /// Reads the parameters from stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public void ReadParametersFromStream(Stream stream, bool intersectAlternateConfig)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            using (XmlReader reader = XmlReader.Create(stream))
            {
                ReadParameters(reader, intersectAlternateConfig);
            }
        }

        /// <summary>
        /// Reads the parameters from text reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public void ReadParametersFromTextReader(TextReader reader, bool intersectAlternateConfig)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            using (XmlReader xmlReader = XmlReader.Create(reader))
            {
                ReadParameters(xmlReader, intersectAlternateConfig);
            }
        }

        private void ReadParameters(XmlReader reader, bool intersectAlternateConfig)
        {
            ReadParamsReader(reader);

            if (intersectAlternateConfig)
            {
                string alternateConfigFile;
                if (TryGetString("externalconfig", out alternateConfigFile))
                {
                    if (File.Exists(alternateConfigFile))
                    {
                        ConfigurationValues fileValues = new ConfigurationValues();
                        fileValues.ReadParametersFromStream(alternateConfigFile, false);
                        IntersectValues(fileValues);
                    }
                }
            }
        }

        private void ReadParamsReader(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.LocalName.ToLower() == "param")
                {
                    string name = reader.GetAttribute("name");
                    string val = reader.GetAttribute("value");
                    if (!string.IsNullOrEmpty(name))
                    {
                        m_values[name.ToLower()] = val;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the value with the specified key.
        /// </summary>
        /// <value></value>
        public string this[string key]
        {
            get
            {
                string result;
                if (!m_values.TryGetValue(key.ToLower(), out result))
                {
                    // Next, go to the machine config
                    Configuration machineConfig = ConfigurationManager.OpenMachineConfiguration();
                    if (machineConfig.AppSettings != null)
                    {
                        KeyValueConfigurationElement k = machineConfig.AppSettings.Settings[key];
                        if (k != null)
                        {
                            result = k.Value;
                            this[key] = result;
                        }
                    }
                }
                return result;
            }
            set
            {
                m_values[key.ToLower()] = value;
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<string> Keys
        {
            get
            {
                return m_values.Keys;
            }
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetString(string key)
        {
            return GetString(key, null);
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : val;
        }

        /// <summary>
        /// Gets the long.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public long GetLong(string key)
        {
            return GetLong(key, 0);
        }

        /// <summary>
        /// Gets the long.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public long GetLong(string key, long defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : long.Parse(val);
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public int GetInt(string key, int defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : int.Parse(val);
        }

        /// <summary>
        /// Tries the get string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public bool TryGetString(string key, out string val)
        {
            val = null;
            string config = this[key];
            if (config != null)
            {
                val = config;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Intersects the values.
        /// </summary>
        /// <param name="intersectValues">The intersect values.</param>
        public void IntersectValues(ConfigurationValues intersectValues)
        {
            foreach (string key in intersectValues.Keys)
            {
                this[key] = intersectValues[key];
            }
        }

        private bool m_wasExternalConfigRead;

        /// <summary>
        /// Gets or sets a value indicating whether [was external config read].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [was external config read]; otherwise, <c>false</c>.
        /// </value>
        public bool WasExternalConfigRead
        {
            get
            {
                return m_wasExternalConfigRead;
            }
            set
            {
                m_wasExternalConfigRead = value;
            }
        }
    }
}
