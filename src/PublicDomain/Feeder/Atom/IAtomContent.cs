using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Atom
{
    /// <summary>
    /// Contains or links to the complete content of the entry. Content must be provided if there is no alternate link, and should be provided if there is no summary.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomContent
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the SRC.
        /// </summary>
        /// <value>The SRC.</value>
        Uri Src { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        string Content { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class AtomContent : CachedPropertiesProvider, IAtomContent
    {
        #region IAtomContent Members

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return Getter("Type");
            }
            set
            {
                Setter("Type", value);
            }
        }

        /// <summary>
        /// Gets or sets the SRC.
        /// </summary>
        /// <value>The SRC.</value>
        public Uri Src
        {
            get
            {
                return Getter<Uri>("Src", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Src", value);
            }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content
        {
            get
            {
                return Getter("Content");
            }
            set
            {
                Setter("Content", value);
            }
        }

        #endregion
    }
}
