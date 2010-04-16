using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class DistilledFeedItem : CachedPropertiesProvider, IDistilledFeedItem
    {
        #region IDistilledFeedItem Members

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
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get
            {
                return Getter<Uri>("DistilledLink", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("DistilledLink", value);
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

        #endregion
    }
}
