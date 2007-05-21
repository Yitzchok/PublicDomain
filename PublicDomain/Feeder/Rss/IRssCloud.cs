using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Rss
{
    /// <summary>
    /// Specifies a web service that supports the rssCloud interface which can be implemented in HTTP-POST, XML-RPC or SOAP 1.1. 
    /// Its purpose is to allow processes to register with a cloud to be notified of updates to the channel, implementing a lightweight publish-subscribe protocol for RSS feeds.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssCloud
    {
        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        string Domain
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        int Port
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        string Path
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the register procedure.
        /// </summary>
        /// <value>The register procedure.</value>
        string RegisterProcedure
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        RssCloudProtocol Protocol
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum RssCloudProtocol
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown,

        /// <summary>
        /// 
        /// </summary>
        XmlRpc,

        /// <summary>
        /// 
        /// </summary>
        HttpPost,

        /// <summary>
        /// /
        /// </summary>
        Soap
    }

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class RssCloud : CachedPropertiesProvider, Feeder.Rss.IRssCloud
    {
        #region IRssCloud Members

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain
        {
            get
            {
                return Getter("Domain");
            }
            set
            {
                Setter("Domain", value);
            }
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port
        {
            get
            {
                return Getter<int>("Port", CachedPropertiesProvider.ConvertToInt);
            }
            set
            {
                Setter("Port", value);
            }
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get
            {
                return Getter("Path");
            }
            set
            {
                Setter("Path", value);
            }
        }

        /// <summary>
        /// Gets or sets the register procedure.
        /// </summary>
        /// <value>The register procedure.</value>
        public string RegisterProcedure
        {
            get
            {
                return Getter("RegisterProcedure");
            }
            set
            {
                Setter("RegisterProcedure", value);
            }
        }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        public Feeder.Rss.RssCloudProtocol Protocol
        {
            get
            {
                return Getter<RssCloudProtocol>("Protocol", RssFeedParser.ConvertToRssCloudProtocol);
            }
            set
            {
                Setter("Protocol", value);
            }
        }

        #endregion
    }
}
