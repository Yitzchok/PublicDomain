using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Atom
{
    /// <summary>
    /// Contains human-readable text, usually in small quantities. The type attribute determines how this information is encoded (default="text")
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomText
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomText : CachedPropertiesProvider, Feeder.Atom.IAtomText
    {
        #region IAtomText Members

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
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                return Getter("Text");
            }
            set
            {
                Setter("Text", value);
            }
        }

        #endregion
    }
}
