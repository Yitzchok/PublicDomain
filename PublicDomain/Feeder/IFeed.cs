using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// Common denominator for a feed. If you are looking for
    /// specific properties, yet you don't want to lose information
    /// through distilling, then you need to conditionally check
    /// the dynamic type of this instance and cast to that type
    /// (e.g. IRssFeed or IAtomFeed). If you are expecting a specific
    /// type of feed, then you can just cast to that type of feed; however,
    /// note that you can never guarantee the dynamic type of the
    /// instance, since the type that is instantiated is determined at
    /// run-time by sniffing out the feed.
    /// </summary>
    public interface IFeed : IXmlFeed
    {
        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        Uri FeedUri { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        IList<IFeedItem> Items { get; set; }

        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        IDistilledFeed Distill();
    }
}
