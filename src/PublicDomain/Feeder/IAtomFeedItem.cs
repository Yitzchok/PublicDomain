using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.Feeder.Atom;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAtomFeedItem : IFeedItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        Uri Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>The last updated.</value>
        TzDateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        /// <value>The authors.</value>
        IList<IAtomPerson> Authors { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        IAtomContent Content { get; set; }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>The links.</value>
        IList<IAtomLink> Links { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        IAtomText Summary { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The categories.</value>
        IList<IAtomCategory> Categories { get; set; }

        /// <summary>
        /// Gets or sets the contributors.
        /// </summary>
        /// <value>The contributors.</value>
        IList<IAtomPerson> Contributors { get; set; }

        /// <summary>
        /// Gets or sets the published.
        /// </summary>
        /// <value>The published.</value>
        TzDateTime Published { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        IAtomFeed Source { get; set; }

        /// <summary>
        /// Gets or sets the rights.
        /// </summary>
        /// <value>The rights.</value>
        IAtomText Rights { get; set; }
    }
}
