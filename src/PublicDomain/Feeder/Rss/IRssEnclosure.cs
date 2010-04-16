using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Rss
{
    /// <summary>
    /// Describes a media object that is attached to the item.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssEnclosure
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
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        int Length
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class RssEnclosure : CachedPropertiesProvider, Feeder.Rss.IRssEnclosure
    {
        #region IRssEnclosure Members

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
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public int Length
        {
            get
            {
                return Getter<int>("Length", CachedPropertiesProvider.ConvertToInt);
            }
            set
            {
                Setter("Length", value);
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return Getter("Type");
            }
            set
            {
                Setter("Type", value);
            }
        }

        #endregion
    }
}
