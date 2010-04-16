using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Parser : Feeder.IParser
    {
        /// <summary>
        /// Gets the default XML reader settings.
        /// </summary>
        /// <returns></returns>
        protected static XmlReaderSettings GetDefaultXmlReaderSettings()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            // MSDN doesn't prefer this, but it seems to avoid a lot
            // of issues.
            settings.ProhibitDtd = false;
            return settings;
        }

        internal static void UnhandledElement(ICachedPropertiesProvider propsProvider, XmlReader reader)
        {
            string name = reader.Name;
            string val = reader.ReadOuterXml();
            if (propsProvider.Properties.ContainsKey(name))
            {
                val = propsProvider.Properties[name] + val;
            }
            propsProvider.Properties[name] = val;
        }

        /// <summary>
        /// Creates the XML reader from string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        protected static XmlReader CreateXmlReaderFromString(string input)
        {
            StringReader stringReader = new StringReader(input);
            return XmlReader.Create(stringReader);
        }

        /// <summary>
        /// Reads the URI stream.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string ReadUriStream(string uri)
        {
            return ReadUriStream(new Uri(uri));
        }

        /// <summary>
        /// Reads the URI stream.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static string ReadUriStream(string uri, int timeout)
        {
            return ReadUriStream(new Uri(uri));
        }

        /// <summary>
        /// Reads the URI stream.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string ReadUriStream(Uri uri)
        {
            return ReadUriStream(uri, 2000);
        }

        /// <summary>
        /// Reads the URI stream.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static string ReadUriStream(Uri uri, int timeout)
        {
            string wholeResponse = null;
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(uri);
            //hwr.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; Maxthon; .NET CLR 1.1.4322)";
            hwr.Timeout = timeout;
            HttpWebResponse wr = null;
            try
            {
                wr = (HttpWebResponse)hwr.GetResponse();
                Stream responseStream = wr.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                wholeResponse = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                wr.Close();
                wr = null;
            }
            finally
            {
                if (wr != null) wr.Close();
            }
            return wholeResponse;
        }

        /// <summary>
        /// Creates the feed from stream.
        /// </summary>
        /// <param name="rawContent">Content of the raw.</param>
        /// <returns></returns>
        public T CreateFeedFromStream<T>(string rawContent) where T : IXmlFeed
        {
            byte[] b = new byte[rawContent.Length];
            Encoding.ASCII.GetBytes(rawContent.ToCharArray(),
                        0,
                        rawContent.Length,
                        b,
                        0);
            using (MemoryStream ms = new MemoryStream(b))
            {
                T ret = (T)CreateFeed<T>(ms);
                ret.RawContents = rawContent;
                return ret;
            }
        }

        /// <summary>
        /// Parses the specified URI into an IFeed.
        /// </summary>
        /// <param name="feedUri">Valid URI to an accessible resource stream.</param>
        /// <returns>IFeed representing the feed.</returns>
        public T CreateFeed<T>(string feedUri) where T : IXmlFeed
        {
            return CreateFeed<T>(new Uri(feedUri), true);
        }

        /// <summary>
        /// Parses the specified URI into an IFeed.
        /// </summary>
        /// <param name="feedUri">Valid URI to an accessible resource stream.</param>
        /// <param name="saveStream">if set to <c>true</c> [save stream].</param>
        /// <returns>IFeed representing the feed.</returns>
        public T CreateFeed<T>(Uri feedUri, bool saveStream) where T : IXmlFeed
        {
            if (saveStream)
            {
                string stream = ReadUriStream(feedUri);
                using (StringReader reader = new StringReader(stream))
                {
                    T feed = CreateFeedBase<T>(XmlReader.Create(reader, GetDefaultXmlReaderSettings()));
                    feed.RawContents = stream;
                    return feed;
                }
            }
            else
            {
                return CreateFeedBase<T>(XmlReader.Create(feedUri.ToString(), GetDefaultXmlReaderSettings()));
            }
        }

        /// <summary>
        /// Parses the specified stream into an IFeed.
        /// The parser does not close the stream.
        /// </summary>
        /// <param name="input">An open stream to a feed stream.</param>
        /// <returns>IFeed representing the feed.</returns>
        public T CreateFeed<T>(Stream input) where T : IXmlFeed
        {
            return CreateFeedBase<T>(XmlReader.Create(input, GetDefaultXmlReaderSettings()));
        }

        /// <summary>
        /// Creates the feed base.
        /// </summary>
        /// <param name="feedReader">The feed reader.</param>
        /// <returns></returns>
        protected abstract T CreateFeedBase<T>(XmlReader feedReader) where T : IXmlFeed;

        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public abstract T Parse<T>(System.Xml.XmlReader reader);
    }
}
