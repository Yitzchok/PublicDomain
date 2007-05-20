using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.Feeder.Atom;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    public class AtomFeed : Feed, IAtomFeed
    {
        private IList<IAtomPerson> m_authors = new List<IAtomPerson>();
        private IList<IAtomLink> m_links = new List<IAtomLink>();
        private IList<IAtomCategory> m_categories = new List<IAtomCategory>();
        private IList<IAtomPerson> m_contributors = new List<IAtomPerson>();

        #region IAtomFeed Members

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
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        public IAtomGenerator Generator
        {
            get
            {
                return Getter<IAtomGenerator>("Generator", AtomFeedParser.ConvertToIAtomGenerator);
            }
            set
            {
                Setter("Generator", value);
            }
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public string Icon
        {
            get
            {
                return Getter("Icon");
            }
            set
            {
                Setter("Icon", value);
            }
        }

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>The logo.</value>
        public string Logo
        {
            get
            {
                return Getter("Logo");
            }
            set
            {
                Setter("Logo", value);
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

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        /// <value>The subtitle.</value>
        public IAtomText Subtitle
        {
            get
            {
                return Getter<IAtomText>("Subtitle", AtomFeedParser.ConvertToIAtomText);
            }
            set
            {
                Setter("Subtitle", value);
            }
        }

        #endregion

        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        public override IDistilledFeed Distill()
        {
            IDistilledFeed distilled = new Feeder.DistilledFeed();
            if (Categories != null && Categories.Count == 1)
            {
                distilled.Category = Categories[0].Term;
            }
            if (Rights != null)
            {
                distilled.Copyright = Rights.Text;
            }
            if (Subtitle != null)
            {
                distilled.Description = Subtitle.Text;
            }
            distilled.FeedUri = FeedUri;
            if (Generator != null)
            {
                distilled.Generator = Generator.Description;
            }
            distilled.LastChanged = LastUpdated;
            distilled.Properties = Properties;
            distilled.PublicationDate = LastUpdated;
            distilled.Title = Title;
            distilled.Items.Clear();
            foreach (IAtomFeedItem item in Items)
            {
                distilled.Items.Add(item.Distill());
            }
            return distilled;
        }
    }
}
