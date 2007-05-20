using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// Attempts to distill any feed format (RSS, Atom, etc.) into
    /// a form that can be dealt with more logically. Information IS
    /// lost in this process, and very few fields can be assumed to
    /// have any data. Actually, almost none *must* have content.
    /// </summary>
    public interface IDistilledFeed : ICachedPropertiesProvider
    {
        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        Uri FeedUri { get; set; }

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
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        string Generator { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        string Category { get; set; }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        TzDateTime PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the last changed.
        /// </summary>
        /// <value>The last changed.</value>
        TzDateTime LastChanged { get; set; }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        /// <value>The time to live.</value>
        int? TimeToLive { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        IList<IDistilledFeedItem> Items { get; set; }
    }
}
