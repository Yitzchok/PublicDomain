using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Atom
{
    /// <summary>
    /// Describes a person, corporation, or similar entity. It has one required element, name, and two optional elements: uri, email.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomPerson
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the homepage.
        /// </summary>
        /// <value>The homepage.</value>
        Uri Homepage { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        string Email { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class AtomPerson : CachedPropertiesProvider, Feeder.Atom.IAtomPerson
    {
        #region IAtomPerson Members

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
        /// Gets or sets the homepage.
        /// </summary>
        /// <value>The homepage.</value>
        public Uri Homepage
        {
            get
            {
                return Getter<Uri>("Homepage", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Homepage", value);
            }
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get
            {
                return Getter("Email");
            }
            set
            {
                Setter("Email", value);
            }
        }

        #endregion
    }
}
