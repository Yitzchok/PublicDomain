using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.Feeder.Rss;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRssFeedItem : IFeedItem
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
        /// Gets or sets the author.
        /// </summary>
        /// <value>The author.</value>
        string Author { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        IRssCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        Uri Comments { get; set; }

        /// <summary>
        /// Gets or sets the enclosure.
        /// </summary>
        /// <value>The enclosure.</value>
        IRssEnclosure Enclosure { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        IRssGuid Guid { get; set; }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        TzDateTime PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        IRssSource Source { get; set; }
    }
}
