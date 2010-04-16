using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    public interface IXmlFeed : ICachedPropertiesProvider
    {
        /// <summary>
        /// Gets or sets the raw contents.
        /// </summary>
        /// <value>The raw contents.</value>
        string RawContents { get; set; }
    }
}
