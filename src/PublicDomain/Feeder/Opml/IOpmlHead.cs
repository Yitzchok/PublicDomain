using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Opml
{
    /// <summary>
    /// A head contains zero or more optional element.
    /// 
    /// All the sub-elements of head may be ignored by the processor.
    /// If an outline is opened within another outline, the processor
    /// must ignore the windowXxx elements, those elements only control
    /// the size and position of outlines that are opened in their own windows.
    /// 
    /// If you load an OPML document into your client, you may choose to
    /// respect expansionState, or not. We're not in any way trying to
    /// dictate user experience. The expansionState info is there because
    /// it's needed in certain contexts. It's easy to imagine contexts where
    /// it would make sense to completely ignore it.
    /// 
    /// Taken verbatim from http://www.opml.org/spec
    /// </summary>
    public interface IOpmlHead : ICachedPropertiesProvider
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        TzDateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the date modified.
        /// </summary>
        /// <value>The date modified.</value>
        TzDateTime DateModified { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        string Owner { get; set; }

        /// <summary>
        /// Gets or sets the owner email.
        /// </summary>
        /// <value>The owner email.</value>
        string OwnerEmail { get; set; }

        /// <summary>
        /// Gets or sets the state of the expansion.
        /// </summary>
        /// <value>The state of the expansion.</value>
        string ExpansionState { get; set; }

        /// <summary>
        /// Gets or sets the state of the vertical scroll.
        /// </summary>
        /// <value>The state of the vertical scroll.</value>
        int? VerticalScrollState { get; set; }

        /// <summary>
        /// Gets or sets the window top.
        /// </summary>
        /// <value>The window top.</value>
        int? WindowTop { get; set; }

        /// <summary>
        /// Gets or sets the window left.
        /// </summary>
        /// <value>The window left.</value>
        int? WindowLeft { get; set; }

        /// <summary>
        /// Gets or sets the window bottom.
        /// </summary>
        /// <value>The window bottom.</value>
        int? WindowBottom { get; set; }

        /// <summary>
        /// Gets or sets the window right.
        /// </summary>
        /// <value>The window right.</value>
        int? WindowRight { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class OpmlHead : CachedPropertiesProvider, IOpmlHead
    {
        #region IOpmlHead Members

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
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        public TzDateTime DateCreated
        {
            get
            {
                return Getter<TzDateTime>("DateCreated", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("DateCreated", value);
            }
        }

        /// <summary>
        /// Gets or sets the date modified.
        /// </summary>
        /// <value>The date modified.</value>
        public TzDateTime DateModified
        {
            get
            {
                return Getter<TzDateTime>("DateModified", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("DateModified", value);
            }
        }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public string Owner
        {
            get
            {
                return Getter("Owner");
            }
            set
            {
                Setter("Owner", value);
            }
        }

        /// <summary>
        /// Gets or sets the owner email.
        /// </summary>
        /// <value>The owner email.</value>
        public string OwnerEmail
        {
            get
            {
                return Getter("OwnerEmail");
            }
            set
            {
                Setter("OwnerEmail", value);
            }
        }

        /// <summary>
        /// Gets or sets the state of the expansion.
        /// </summary>
        /// <value>The state of the expansion.</value>
        public string ExpansionState
        {
            get
            {
                return Getter("ExpansionState");
            }
            set
            {
                Setter("ExpansionState", value);
            }
        }

        /// <summary>
        /// Gets or sets the state of the vertical scroll.
        /// </summary>
        /// <value>The state of the vertical scroll.</value>
        public int? VerticalScrollState
        {
            get
            {
                return Getter<int?>("VerticalScrollState", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("VerticalScrollState", value);
            }
        }

        /// <summary>
        /// Gets or sets the window top.
        /// </summary>
        /// <value>The window top.</value>
        public int? WindowTop
        {
            get
            {
                return Getter<int?>("WindowTop", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("WindowTop", value);
            }
        }

        /// <summary>
        /// Gets or sets the window left.
        /// </summary>
        /// <value>The window left.</value>
        public int? WindowLeft
        {
            get
            {
                return Getter<int?>("WindowLeft", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("WindowLeft", value);
            }
        }

        /// <summary>
        /// Gets or sets the window bottom.
        /// </summary>
        /// <value>The window bottom.</value>
        public int? WindowBottom
        {
            get
            {
                return Getter<int?>("WindowBottom", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("WindowBottom", value);
            }
        }

        /// <summary>
        /// Gets or sets the window right.
        /// </summary>
        /// <value>The window right.</value>
        public int? WindowRight
        {
            get
            {
                return Getter<int?>("WindowRight", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("WindowRight", value);
            }
        }

        #endregion
    }
}
