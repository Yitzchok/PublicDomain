using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Feed : CachedPropertiesProvider, IFeed
    {
        private IList<IFeedItem> m_items = new List<IFeedItem>();
        private string rawContents;

        #region IFeed Members

        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        public Uri FeedUri
        {
            get
            {
                return Getter<Uri>("FeedUri", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("FeedUri", value);
            }
        }

        /// <summary>
        /// Gets or sets the raw contents.
        /// </summary>
        /// <value>The raw contents.</value>
        public string RawContents
        {
            get
            {
                return rawContents;
            }
            set
            {
                rawContents = value;
            }
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public IList<IFeedItem> Items
        {
            get
            {
                return m_items;
            }
            set
            {
                m_items = value;
            }
        }

        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        public abstract IDistilledFeed Distill();

        #endregion
    }
}
