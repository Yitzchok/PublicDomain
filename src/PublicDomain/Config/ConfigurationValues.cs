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
        /// <param name="intersectedConfigs">The intersected configs.</param>
        /// <returns></returns>
        public ConfigurationValues(string assemblyStreamName, List<string> intersectedConfigs)
            : this(assemblyStreamName, Assembly.GetExecutingAssembly(), intersectedConfigs)
        {
        }

        /// <summary>
        /// Reads the parameters from assembly.
        /// </summary>
        /// <param name="assemblyStreamName">Name of the assembly stream.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="intersectedConfigs">The intersected configs.</param>
        /// <returns></returns>
        public ConfigurationValues(string assemblyStreamName, Assembly assembly, List<string> intersectedConfigs)
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
            if (intersectedConfigs == null)
            {
                intersectedConfigs = new List<string>();
            }
            ReadParametersFromStream(stream, intersectedConfigs);
        }

        /// <summary>
        /// Reads the parameters from file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="intersectedConfigs">The intersected configs.</param>
        public void ReadParametersFromStream(string fileName, List<string> intersectedConfigs)
        {
            using (TextReader textReader = new StreamReader(fileName))
            {
                ReadParametersFromTextReader(textReader, intersectedConfigs);
            }
        }

        /// <summary>
        /// Reads the parameters from stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="intersectedConfigs">The intersected configs.</param>
        public void ReadParametersFromStream(Stream stream, List<string> intersectedConfigs)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            using (XmlReader reader = XmlReader.Create(stream))
            {
                ReadParameters(reader, intersectedConfigs);
            }
        }

        /// <summary>
        /// Reads the parameters from text reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="intersectedConfigs">The intersected configs.</param>
        public void ReadParametersFromTextReader(TextReader reader, List<string> intersectedConfigs)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            using (XmlReader xmlReader = XmlReader.Create(reader))
            {
                ReadParameters(xmlReader, intersectedConfigs);
            }
        }

        private void ReadParameters(XmlReader reader, List<string> intersectedConfigs)
        {
            List<string> alternateConfigs = ReadParamsReader(reader);

            foreach (string alternateConfigFile in alternateConfigs)
            {
                if (File.Exists(alternateConfigFile) && (intersectedConfigs == null || (intersectedConfigs != null && !intersectedConfigs.Contains(alternateConfigFile))))
                {
                    if (intersectedConfigs != null)
                    {
                        intersectedConfigs.Add(alternateConfigFile);
                    }
                    ConfigurationValues fileValues = new ConfigurationValues();
                    fileValues.ReadParametersFromStream(alternateConfigFile, intersectedConfigs);
                    IntersectValues(fileValues);
                }
            }
        }

        private List<string> ReadParamsReader(XmlReader reader)
        {
            List<string> alternateConfigs = new List<string>();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.LocalName.ToLower() == "param")
                {
                    string name = reader.GetAttribute("name");
                    string val = reader.GetAttribute("value");
                    if (!string.IsNullOrEmpty(name))
                    {
                        name = name.ToLower();
                        if (name.Equals("externalconfig"))
                        {
                            alternateConfigs.Add(val);
                        }
                        m_values[name] = val;
                    }
                }
            }
            return alternateConfigs;
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
        /// 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public Int64 GetInt64(string key)
        {
            return GetInt64(key, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public Int64 GetInt64(string key, Int64 defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : Int64.Parse(val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public Int32 GetInt32(string key)
        {
            return GetInt32(key, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public Int32 GetInt32(string key, Int32 defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : Int32.Parse(val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public Int16 GetInt16(string key)
        {
            return GetInt16(key, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public Int16 GetInt16(string key, Int16 defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : Int16.Parse(val);
        }

        /// <summary>
        /// Gets the bool.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool GetBool(string key)
        {
            return GetBool(key, false);
        }

        /// <summary>
        /// Gets the bool.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public bool GetBool(string key, bool defaultValue)
        {
            return ConversionUtilities.ParseBool(this[key], defaultValue);
        }

        /// <summary>
        /// Gets the enum.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public T GetEnum<T>(string key, T defaultValue)
        {
            return General.TryParseEnum<T>(this[key], defaultValue);
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
        /// 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public double GetDouble(string key)
        {
            return GetDouble(key, 0);
        }

        /// <summary>
        /// Gets the 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public double GetDouble(string key, double defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : double.Parse(val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public float GetFloat(string key)
        {
            return GetFloat(key, 0);
        }

        /// <summary>
        /// Gets the 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public float GetFloat(string key, float defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : float.Parse(val);
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
