using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Rss
{
    /// <summary>
    /// guid stands for globally unique identifier. It's a string that uniquely identifies the item. When present, an aggregator may choose to use this string to determine if an item is new.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssGuid
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        string UniqueIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the is perma link.
        /// </summary>
        /// <value>The is perma link.</value>
        bool? IsPermaLink
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssGuid : CachedPropertiesProvider, Feeder.Rss.IRssGuid
    {
        #region IRssGuid Members

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public string UniqueIdentifier
        {
            get
            {
                return Getter("UniqueIdentifier");
            }
            set
            {
                Setter("UniqueIdentifier", value);
            }
        }

        /// <summary>
        /// Gets or sets the is perma link.
        /// </summary>
        /// <value>The is perma link.</value>
        public bool? IsPermaLink
        {
            get
            {
                return Getter<bool?>("IsPermaLink", CachedPropertiesProvider.ConvertToBoolNullable);
            }
            set
            {
                Setter("IsPermaLink", value);
            }
        }

        #endregion
    }
}
