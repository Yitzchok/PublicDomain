using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Rss
{
    /// <summary>
    /// Its value is the name of the RSS channel that the item came from, derived from its title. It has one required attribute, url, which links to the XMLization of the source.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssSource
    {
        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class RssSource : CachedPropertiesProvider, Feeder.Rss.IRssSource
    {
        #region IRssSource Members

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get
            {
                return Getter<Uri>("Link", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Link", value);
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
