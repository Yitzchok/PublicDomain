using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Rss
{
    /// <summary>
    /// In RSS 2.0, a provision is made for linking a channel to its identifier in a cataloging system, using the channel-level category feature. For example, to link a channel to its Syndic8 identifier, include a category element as a sub-element of channel, with domain "Syndic8", and value the identifier for your channel in the Syndic8 database. The appropriate category element for Scripting News would be <category domain="Syndic8">1765</category>.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssCategory
    {
        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        string Domain
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>The name of the category.</value>
        string CategoryName
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssCategory : CachedPropertiesProvider, Feeder.Rss.IRssCategory
    {
        #region IRssCategory Members

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain
        {
            get
            {
                return Getter("Domain");
            }
            set
            {
                Setter("Domain", value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>The name of the category.</value>
        public string CategoryName
        {
            get
            {
                return Getter("CategoryName");
            }
            set
            {
                Setter("CategoryName", value);
            }
        }

        #endregion
    }
}
