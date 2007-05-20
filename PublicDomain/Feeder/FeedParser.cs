using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using PublicDomain.Feeder.Rss;
using PublicDomain.Feeder.Atom;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// The FeedParser is, from the client's perspective, the
    /// entry point into the framework. The static methods of
    /// this class should be used to instantiate IFeeds
    /// which can then be manipulated.
    /// </summary>
    public class FeedParser : Parser
    {
        /// <summary>
        /// Creates the feed base.
        /// </summary>
        /// <param name="feedReader">The feed reader.</param>
        /// <returns></returns>
        protected override T CreateFeedBase<T>(XmlReader feedReader)
        {
            feedReader.MoveToContent();
            return new FeedParser().Parse<T>(feedReader);
        }

        #region IFeedParser Members

        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override T Parse<T>(System.Xml.XmlReader reader)
        {
            // Need a specific parser
            IParser subparser = null;
            IFeed ret = null;

            string localRootName = reader.LocalName.ToLower().Trim();
            if (localRootName == "rss" || localRootName == "rdf")
            {
                subparser = new RssFeedParser();
            }
            else if (localRootName == "feed")
            {
                subparser = new AtomFeedParser();
            }
            if (subparser != null)
            {
                using (XmlReader subreader = reader.ReadSubtree())
                {
                    ret = (IFeed)subparser.Parse<T>(subreader);
                }
            }
            else
            {
                throw new Exception(string.Format("Unknown feed type '{0}'.", reader.Name));
            }
            reader.Close();
            return (T)ret;
        }

        #endregion
    }
}
