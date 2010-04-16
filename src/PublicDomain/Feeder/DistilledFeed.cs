using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class DistilledFeed : CachedPropertiesProvider, IDistilledFeed
    {
        private IList<IDistilledFeedItem> m_items = new List<IDistilledFeedItem>();

        #region IDistilledFeed Members

        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        public Uri FeedUri
        {
            get
            {
                return Getter<Uri>("DistilledFeedUri", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("DistilledFeedUri", value);
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return Getter("DistilledTitle");
            }
            set
            {
                Setter("DistilledTitle", value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("DistilledDescription");
            }
            set
            {
                Setter("DistilledDescription", value);
            }
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public System.Globalization.CultureInfo Culture
        {
            get
            {
                return Getter<System.Globalization.CultureInfo>("DistilledCulture", CachedPropertiesProvider.ConvertToCultureInfo);
            }
            set
            {
                Setter("DistilledCulture", value);
            }
        }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        public string Copyright
        {
            get
            {
                return Getter("DistilledCopyright");
            }
            set
            {
                Setter("DistilledCopyright", value);
            }
        }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        public string Generator
        {
            get
            {
                return Getter("DistilledGenerator");
            }
            set
            {
                Setter("DistilledGenerator", value);
            }
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category
        {
            get
            {
                return Getter("DistilledCategory");
            }
            set
            {
                Setter("DistilledCategory", value);
            }
        }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        public TzDateTime PublicationDate
        {
            get
            {
                return Getter<TzDateTime>("DistilledPublicationDate", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("DistilledPublicationDate", value);
            }
        }

        /// <summary>
        /// Gets or sets the last changed.
        /// </summary>
        /// <value>The last changed.</value>
        public TzDateTime LastChanged
        {
            get
            {
                return Getter<TzDateTime>("DistilledLastChanged", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("DistilledLastChanged", value);
            }
        }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        /// <value>The time to live.</value>
        public int? TimeToLive
        {
            get
            {
                return Getter<int?>("DistilledTimeToLive", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("DistilledTimeToLive", value);
            }
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public IList<IDistilledFeedItem> Items
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

        #endregion
    }
}
