using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// This class
    /// also provides a cache of property names to objects, similar in concept
    /// to the Properties collection, but representing concrete properties.
    /// </summary>
    public interface ICachedPropertiesProvider
    {
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>The properties.</value>
        IDictionary<string, string> Properties { get; set; }

        /// <summary>
        /// Setters the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        void Setter(string propertyName, object value);

        /// <summary>
        /// Getters the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        string Getter(string propertyName);

        /// <summary>
        /// Getters the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="convertDelegate">The convert delegate.</param>
        /// <returns></returns>
        T Getter<T>(string propertyName, Converter<string, T> convertDelegate);
    }
}
