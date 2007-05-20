using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.Feeder.Atom;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    public class AtomFeedItem : FeedItem, IAtomFeedItem
    {
        private IList<IAtomPerson> m_authors = new List<IAtomPerson>();
        private IList<IAtomLink> m_links = new List<IAtomLink>();
        private IList<IAtomCategory> m_categories = new List<IAtomCategory>();
        private IList<IAtomPerson> m_contributors = new List<IAtomPerson>();

        #region IAtomFeedItem Members

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public Uri Id
        {
            get
            {
                return Getter<Uri>("Id", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Id", value);
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
                return Getter("Title");
            }
            set
            {
                Setter("Title", value);
            }
        }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>The last updated.</value>
        public TzDateTime LastUpdated
        {
            get
            {
                return Getter<TzDateTime>("LastUpdated", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("LastUpdated", value);
            }
        }

        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        /// <value>The authors.</value>
        public IList<IAtomPerson> Authors
        {
            get
            {
                return m_authors;
            }
            set
            {
                m_authors = value;
            }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public IAtomContent Content
        {
            get
            {
                return Getter<IAtomContent>("Content", AtomFeedParser.ConvertToIAtomContent);
            }
            set
            {
                Setter("Content", value);
            }
        }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>The links.</value>
        public IList<IAtomLink> Links
        {
            get
            {
                return m_links;
            }
            set
            {
                m_links = value;
            }
        }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public IAtomText Summary
        {
            get
            {
                return Getter<IAtomText>("Summary", AtomFeedParser.ConvertToIAtomText);
            }
            set
            {
                Setter("Summary", value);
            }
        }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The categories.</value>
        public IList<IAtomCategory> Categories
        {
            get
            {
                return m_categories;
            }
            set
            {
                m_categories = value;
            }
        }

        /// <summary>
        /// Gets or sets the contributors.
        /// </summary>
        /// <value>The contributors.</value>
        public IList<IAtomPerson> Contributors
        {
            get
            {
                return m_contributors;
            }
            set
            {
                m_contributors = value;
            }
        }

        /// <summary>
        /// Gets or sets the published.
        /// </summary>
        /// <value>The published.</value>
        public TzDateTime Published
        {
            get
            {
                return Getter<TzDateTime>("Published", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("Published", value);
            }
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public IAtomFeed Source
        {
            get
            {
                return Getter<IAtomFeed>("Source", AtomFeedParser.ConvertToIAtomFeed);
            }
            set
            {
                Setter("Source", value);
            }
        }

        /// <summary>
        /// Gets or sets the rights.
        /// </summary>
        /// <value>The rights.</value>
        public IAtomText Rights
        {
            get
            {
                return Getter<IAtomText>("Rights", AtomFeedParser.ConvertToIAtomText);
            }
            set
            {
                Setter("Rights", value);
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
            distilled.Link = Id;
            distilled.Properties = Properties;
            distilled.PublicationDate = LastUpdated;
            distilled.Title = Title;

            // TODO the "description" needs some logic to
            // it as per the spec
            distilled.Description = Title;
            return distilled;
        }
    }
}
