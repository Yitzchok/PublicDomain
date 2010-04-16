using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.Feeder.Opml;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// Represents an OPML feed. http://www.opml.org/spec
    /// </summary>
    public interface IOpmlFeed : IXmlFeed
    {
        /// <summary>
        /// Gets or sets the head.
        /// </summary>
        /// <value>The head.</value>
        IOpmlHead Head { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        IOpmlBody Body { get; set; }
    }
}
