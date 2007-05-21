using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Atom
{
    /// <summary>
    /// Identifies a related Web page.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomLink
    {
        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>The href.</value>
        Uri Href { get; set; }

        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        /// <value>The relationship.</value>
        string Relationship { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the link language.
        /// </summary>
        /// <value>The link language.</value>
        string LinkLanguage { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        int? Length { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class AtomLink : CachedPropertiesProvider, Feeder.Atom.IAtomLink
    {
        #region IAtomLink Members

        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>The href.</value>
        public Uri Href
        {
            get
            {
                return Getter<Uri>("Href", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Href", value);
            }
        }

        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        /// <value>The relationship.</value>
        public string Relationship
        {
            get
            {
                return Getter("Relationship");
            }
            set
            {
                Setter("Relationship", value);
            }
        }

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
        /// Gets or sets the link language.
        /// </summary>
        /// <value>The link language.</value>
        public string LinkLanguage
        {
            get
            {
                return Getter("LinkLanguage");
            }
            set
            {
                Setter("LinkLanguage", value);
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
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public int? Length
        {
            get
            {
                return Getter<int?>("Length", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("Length", value);
            }
        }

        #endregion
    }
}
