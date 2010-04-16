using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Atom
{
    /// <summary>
    /// Identifies the software used to generate the feed, for debugging and other purposes. Both the uri and version attributes are optional.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomGenerator
    {
        /// <summary>
        /// Gets or sets the generator URI.
        /// </summary>
        /// <value>The generator URI.</value>
        Uri GeneratorUri { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        string Version { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class AtomGenerator : CachedPropertiesProvider, Feeder.Atom.IAtomGenerator
    {
        #region IAtomGenerator Members

        /// <summary>
        /// Gets or sets the generator URI.
        /// </summary>
        /// <value>The generator URI.</value>
        public Uri GeneratorUri
        {
            get
            {
                return Getter<Uri>("GeneratorUri", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("GeneratorUri", value);
            }
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version
        {
            get
            {
                return Getter("Version");
            }
            set
            {
                Setter("Version", value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("Description");
            }
            set
            {
                Setter("Description", value);
            }
        }

        #endregion
    }
}
