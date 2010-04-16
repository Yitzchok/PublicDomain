using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Opml
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOpmlOutline : IOpmlOutlineProvider
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the is comment.
        /// </summary>
        /// <value>The is comment.</value>
        bool? IsComment { get; set; }

        /// <summary>
        /// Gets or sets the is breakpoint.
        /// </summary>
        /// <value>The is breakpoint.</value>
        bool? IsBreakpoint { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class OpmlOutline : OpmlOutlineProvider, IOpmlOutline
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpmlOutline"/> class.
        /// </summary>
        public OpmlOutline()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpmlOutline"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public OpmlOutline(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpmlOutline"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="type">The type.</param>
        public OpmlOutline(string text, string type)
        {
            this.Text = text;
            this.Type = type;
        }

        #region IOpmlOutline Members

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
        /// Gets or sets the is comment.
        /// </summary>
        /// <value>The is comment.</value>
        public bool? IsComment
        {
            get
            {
                return Getter<bool?>("IsComment", CachedPropertiesProvider.ConvertToBoolNullable);
            }
            set
            {
                Setter("IsComment", value);
            }
        }

        /// <summary>
        /// Gets or sets the is breakpoint.
        /// </summary>
        /// <value>The is breakpoint.</value>
        public bool? IsBreakpoint
        {
            get
            {
                return Getter<bool?>("IsBreakpoint", CachedPropertiesProvider.ConvertToBoolNullable);
            }
            set
            {
                Setter("IsBreakpoint", value);
            }
        }

        #endregion
    }
}
