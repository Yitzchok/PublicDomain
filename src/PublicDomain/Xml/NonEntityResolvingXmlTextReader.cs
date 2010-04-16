using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace PublicDomain.Xml
{
    /// <summary>
    /// 
    /// </summary>
    public class NonEntityResolvingXmlTextReader : XmlTextReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        public NonEntityResolvingXmlTextReader()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public NonEntityResolvingXmlTextReader(string url)
            : base(url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        public NonEntityResolvingXmlTextReader(Stream input)
            : base(input)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        public NonEntityResolvingXmlTextReader(TextReader input)
            : base(input)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        /// <param name="nt">The nt.</param>
        public NonEntityResolvingXmlTextReader(XmlNameTable nt)
            : base(nt)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="nt">The nt.</param>
        public NonEntityResolvingXmlTextReader(Stream input, XmlNameTable nt)
            : base(input, nt)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="input">The input.</param>
        public NonEntityResolvingXmlTextReader(string url, Stream input)
            : base(url, input)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="input">The input.</param>
        public NonEntityResolvingXmlTextReader(string url, TextReader input)
            : base(url, input)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        /// <param name="url">The URL for the file containing the XML data to read.</param>
        /// <param name="nt">The XmlNameTable to use.</param>
        /// <exception cref="T:System.IO.FileNotFoundException">The specified file cannot be found.</exception>
        /// <exception cref="T:System.Net.WebException">The remote filename cannot be resolved.-or-An error occurred while processing the request.</exception>
        /// <exception cref="T:System.NullReferenceException">The nt value is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">url is an empty string.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">Part of the filename or directory cannot be found.</exception>
        /// <exception cref="T:System.UriFormatException">url is not a valid URI.</exception>
        public NonEntityResolvingXmlTextReader(string url, XmlNameTable nt)
            : base(url, nt)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="nt">The nt.</param>
        public NonEntityResolvingXmlTextReader(TextReader input, XmlNameTable nt)
            : base(input, nt)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        /// <param name="xmlFragment">The XML fragment.</param>
        /// <param name="fragType">Type of the frag.</param>
        /// <param name="context">The context.</param>
        public NonEntityResolvingXmlTextReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context)
            : base(xmlFragment, fragType, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="input">The input.</param>
        /// <param name="nt">The nt.</param>
        public NonEntityResolvingXmlTextReader(string url, Stream input, XmlNameTable nt)
            : base(url, input, nt)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="input">The input.</param>
        /// <param name="nt">The nt.</param>
        public NonEntityResolvingXmlTextReader(string url, TextReader input, XmlNameTable nt)
            : base(url, input, nt)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonEntityResolvingXmlTextReader"/> class.
        /// </summary>
        /// <param name="xmlFragment">The XML fragment.</param>
        /// <param name="fragType">Type of the frag.</param>
        /// <param name="context">The context.</param>
        public NonEntityResolvingXmlTextReader(string xmlFragment, XmlNodeType fragType, XmlParserContext context)
            : base(xmlFragment, fragType, context)
        {
        }

        /// <summary>
        /// Gets a value indicating whether this reader can parse and resolve entities.
        /// </summary>
        /// <value></value>
        /// <returns>true if the reader can parse and resolve entities; otherwise, false. The XmlTextReader class always returns true.</returns>
        public override bool CanResolveEntity
        {
            get
            {
                return false;
            }
        }
    }
}
