using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// Base interface that represents a feed item or entry.
    /// As for IFeed, the best way to get to intelligible properties,
    /// if not distilling into a IDistilledFeedItem, is
    /// to cast to specific sub-interfaces of IFeedItem and conditionally
    /// use those or assume that it is a specific item type. Again,
    /// casting to a specific sub-interface is not completely predictable.
    /// </summary>
    public interface IFeedItem : ICachedPropertiesProvider
    {
        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        IDistilledFeedItem Distill();
    }
}
