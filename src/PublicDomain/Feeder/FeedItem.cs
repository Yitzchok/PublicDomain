using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public abstract class FeedItem : CachedPropertiesProvider, IFeedItem
    {
        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        public abstract IDistilledFeedItem Distill();
    }
}
