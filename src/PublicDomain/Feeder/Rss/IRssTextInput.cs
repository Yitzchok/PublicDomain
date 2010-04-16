using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Rss
{
    /// <summary>
    /// The purpose of the textInput element is something of a mystery. You can use it to specify a search engine box. Or to allow a reader to provide feedback. Most aggregators ignore it.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssTextInput
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class RssTextInput : CachedPropertiesProvider, Feeder.Rss.IRssTextInput
    {
        #region IRssTextInput Members

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
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return Getter("Name");
            }
            set
            {
                Setter("Name", value);
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

        #endregion
    }
}
