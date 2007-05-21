using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Atom
{
    /// <summary>
    /// Specifies a category that the feed belongs to. A feed may have multiple category elements.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomCategory
    {
        /// <summary>
        /// Gets or sets the term.
        /// </summary>
        /// <value>The term.</value>
        string Term { get; set; }

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        /// <value>The scheme.</value>
        Uri Scheme { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        string Label { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomCategory : CachedPropertiesProvider, Feeder.Atom.IAtomCategory
    {
        #region IAtomCategory Members

        /// <summary>
        /// Gets or sets the term.
        /// </summary>
        /// <value>The term.</value>
        public string Term
        {
            get
            {
                return Getter("Term");
            }
            set
            {
                Setter("Term", value);
            }
        }

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        /// <value>The scheme.</value>
        public Uri Scheme
        {
            get
            {
                return Getter<Uri>("Scheme", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Scheme", value);
            }
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Label
        {
            get
            {
                return Getter("Label");
            }
            set
            {
                Setter("Label", value);
            }
        }

        #endregion
    }
}
