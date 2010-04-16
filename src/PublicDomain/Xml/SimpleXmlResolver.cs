using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PublicDomain.Xml
{
    /// <summary>
    /// 
    /// </summary>
    public class SimpleXmlResolver : XmlResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleXmlResolver"/> class.
        /// </summary>
        public SimpleXmlResolver()
            : base()
        {
        }

        /// <summary>
        /// Resolves the absolute URI from the base and relative URIs.
        /// </summary>
        /// <param name="baseUri">The base URI used to resolve the relative URI.</param>
        /// <param name="relativeUri">The URI to resolve. The URI can be absolute or relative. If absolute, this value effectively replaces the baseUri value. If relative, it combines with the baseUri to make an absolute URI.</param>
        /// <returns>
        /// A <see cref="T:System.Uri"></see> representing the absolute URI or null if the relative URI cannot be resolved.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">relativeUri is null</exception>
        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            return null;
        }

        /// <summary>
        /// Maps a URI to an object containing the actual resource.
        /// </summary>
        /// <param name="absoluteUri">The URI returned from <see cref="M:System.Xml.XmlResolver.ResolveUri(System.Uri,System.String)"></see></param>
        /// <param name="role">The current implementation does not use this parameter when resolving URIs. This is provided for future extensibility purposes. For example, this can be mapped to the xlink:role and used as an implementation specific argument in other scenarios.</param>
        /// <param name="ofObjectToReturn">The type of object to return. The current implementation only returns System.IO.Stream objects.</param>
        /// <returns>
        /// A System.IO.Stream object or null if a type other than stream is specified.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">absoluteUri is null. </exception>
        /// <exception cref="T:System.UriFormatException">The specified URI is not an absolute URI. </exception>
        /// <exception cref="T:System.Exception">There is a runtime error (for example, an interrupted server connection). </exception>
        /// <exception cref="T:System.Xml.XmlException">ofObjectToReturn is neither null nor a Stream type. </exception>
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            return null;
        }

        /// <summary>
        /// When overridden in a derived class, sets the credentials used to authenticate Web requests.
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref="T:System.Net.ICredentials"></see> object. If this property is not set, the value defaults to null; that is, the XmlResolver has no user credentials.</returns>
        public override System.Net.ICredentials Credentials
        {
            set
            {
            }
        }
    }
}
