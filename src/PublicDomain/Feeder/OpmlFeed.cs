using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.Feeder.Opml;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class OpmlFeed : CachedPropertiesProvider, IOpmlFeed
    {
        private string rawContents;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpmlFeed"/> class.
        /// </summary>
        public OpmlFeed()
        {
            Head = new OpmlHead();
            Body = new OpmlBody();
        }

        #region IOpmlFeed Members

        /// <summary>
        /// Gets or sets the head.
        /// </summary>
        /// <value>The head.</value>
        public IOpmlHead Head
        {
            get
            {
                return Getter<IOpmlHead>("Head", OpmlParser.ConvertToIOpmlHead);
            }
            set
            {
                Setter("Head", value);
            }
        }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public IOpmlBody Body
        {
            get
            {
                return Getter<IOpmlBody>("Body", OpmlParser.ConvertToIOpmlBody);
            }
            set
            {
                Setter("Body", value);
            }
        }

        /// <summary>
        /// Gets or sets the raw contents.
        /// </summary>
        /// <value>The raw contents.</value>
        public string RawContents
        {
            get
            {
                return rawContents;
            }
            set
            {
                rawContents = value;
            }
        }

        #endregion
    }
}
