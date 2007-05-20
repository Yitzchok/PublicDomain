using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// Similar to IDistilledFeed, this
    /// attempts to find the common denominator for a 
    /// feed entry or item. Few of these fields may be
    /// assumed to contain data.
    /// </summary>
    public interface IDistilledFeedItem : ICachedPropertiesProvider
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link { get; set; }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        TzDateTime PublicationDate { get; set; }
    }
}
