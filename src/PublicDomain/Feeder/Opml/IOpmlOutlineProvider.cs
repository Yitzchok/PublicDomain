using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Opml
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOpmlOutlineProvider
    {
        /// <summary>
        /// Gets or sets the outlines.
        /// </summary>
        /// <value>The outlines.</value>
        IList<IOpmlOutline> Outlines { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class OpmlOutlineProvider : CachedPropertiesProvider, IOpmlOutlineProvider
    {
        #region IOpmlOutlineProvider Members

        private IList<IOpmlOutline> _Outlines = new List<IOpmlOutline>();

        /// <summary>
        /// Gets or sets the outlines.
        /// </summary>
        /// <value>The outlines.</value>
        public IList<IOpmlOutline> Outlines
        {
            get
            {
                return _Outlines;
            }
            set
            {
                _Outlines = value;
            }
        }

        #endregion
    }
}
