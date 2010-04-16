using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.Feeder.Rss;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class RssFeedItem : FeedItem, IRssFeedItem
    {
        #region IRssFeedItem Members

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return Getter("Title");
            }
            set
            {
                Setter("Title", value);
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
                return Getter("Description");
            }
            set
            {
                Setter("Description", value);
            }
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get
            {
                return Getter<Uri>("Link", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Link", value);
            }
        }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>The author.</value>
        public string Author
        {
            get
            {
                return Getter("Author");
            }
            set
            {
                Setter("Author", value);
            }
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public Feeder.Rss.IRssCategory Category
        {
            get
            {
                return Getter<IRssCategory>("Category", RssFeedParser.ConvertToIRssCategory);
            }
            set
            {
                Setter("Category", value);
            }
        }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public Uri Comments
        {
            get
            {
                return Getter<Uri>("Comments", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Comments", value);
            }
        }

        /// <summary>
        /// Gets or sets the enclosure.
        /// </summary>
        /// <value>The enclosure.</value>
        public Feeder.Rss.IRssEnclosure Enclosure
        {
            get
            {
                return Getter<IRssEnclosure>("Enclosure", RssFeedParser.ConvertToIRssEnclosure);
            }
            set
            {
                Setter("Enclosure", value);
            }
        }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public Feeder.Rss.IRssGuid Guid
        {
            get
            {
                return Getter<IRssGuid>("Guid", RssFeedParser.ConvertToIRssGuid);
            }
            set
            {
                Setter("Guid", value);
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
                return Getter<TzDateTime>("PublicationDate", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("PublicationDate", value);
            }
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public Feeder.Rss.IRssSource Source
        {
            get
            {
                return Getter<IRssSource>("Source", RssFeedParser.ConvertToIRssSource);
            }
            set
            {
                Setter("Source", value);
            }
        }

        #endregion

        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        public override IDistilledFeedItem Distill()
        {
            IDistilledFeedItem distilled = new DistilledFeedItem();
            distilled.Properties = Properties;
            distilled.Description = Description;
            distilled.Link = Link;
            distilled.PublicationDate = PublicationDate;
            distilled.Title = Title;
            return distilled;
        }
    }
}
