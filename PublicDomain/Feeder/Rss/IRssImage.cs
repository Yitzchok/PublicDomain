using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Rss
{
    /// <summary>
    /// Specifies a GIF, JPEG or PNG image that can be displayed with the channel.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssImage
    {
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        Uri Location
        {
            get;
            set;
        }

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
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        int? Width
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        int? Height
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
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssImage : CachedPropertiesProvider, Feeder.Rss.IRssImage
    {
        #region IRssImage Members

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Uri Location
        {
            get
            {
                return Getter<Uri>("Location", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Location", value);
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
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int? Width
        {
            get
            {
                return Getter<int?>("Width", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("Width", value);
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int? Height
        {
            get
            {
                return Getter<int?>("Height", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("Height", value);
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

        #endregion
    }
}
