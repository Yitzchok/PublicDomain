using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Creates the feed.
        /// </summary>
        /// <param name="feedUri">The feed URI.</param>
        /// <param name="saveStream">if set to <c>true</c> [save stream].</param>
        /// <returns></returns>
        T CreateFeed<T>(Uri feedUri, bool saveStream) where T : IXmlFeed;

        /// <summary>
        /// Creates the feed.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        T CreateFeed<T>(System.IO.Stream input) where T : IXmlFeed;

        /// <summary>
        /// Creates the feed.
        /// </summary>
        /// <param name="feedUri">The feed URI.</param>
        /// <returns></returns>
        T CreateFeed<T>(string feedUri) where T : IXmlFeed;

        /// <summary>
        /// Creates the feed from stream.
        /// </summary>
        /// <param name="rawContent">Content of the raw.</param>
        /// <returns></returns>
        T CreateFeedFromStream<T>(string rawContent) where T : IXmlFeed;

        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        T Parse<T>(System.Xml.XmlReader reader);
    }
}
