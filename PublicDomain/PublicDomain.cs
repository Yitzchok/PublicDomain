// PublicDomain
// ======================================
//  Original Author: Kevin Grigorenko (kevgrig@gmail.com)
//      Download README.txt for a list of contributing authors
// 
//  - "Be free Jedi, be free!"
// ======================================
// The purpose of the PublicDomain package is to solve two problems or annoyances
// of .NET development:
// 
// 1. .NET projects and utilities are scattered, difficult to
// deploy and integrate, difficult to find, difficult to contribute to, and
// 2. Licenses are confusing and/or restrictive
// 
// This package solves these two problems as follows (in reverse order):
// 
// 2. This code is in the Public Domain (http://www.copyright.gov/help/faq/faq-definitions.html),
// meaning that the code has no legal authority, will ask nothing for its use, and
// has absolutely no restrictions! That is true open source. It may be included
// in commercial applications, redistributed, altered, or even eaten without any worries.
// Its use need not be attributed in any way. This package is inherently provided
// 'as-is', without any express or implied warranty. In no event will any authors
// be held liable for any damages arising from the use of this package.
// 
// 1. This package explicitly breaks some fundamental paradigms of software engineering
// to solve problem #1. One major goal is that I should be able to embed a single file
// into my project and harness this package, without adding too much bloat to my application.
// For this, precompiler directives are used to include or exclude code that is
// unnecessary or necessitates DLL dependencies that I cannot take on. Second,
// everything is packaged in a single file to make using this package dead simple,
// especially in a C# context (non-C# projects will need a built version of this file
// and reference the DLL). There are no obfuscated build or install procedures,
// or the complexity of managing 10 referenced open source projects in my solution.
// I simply place this file anywhere I need its useful code.
// 
// Any additions to this file must not introduce non-Public Domain code, or code
// that must be externally attributed in any way (i.e. attributed by consumers of this package).
// If you have taken code from someone else which has a similar license and
// does not require external attribution, make sure with the author that this
// is truly a proper place for the code, that external attribution is not necessary,
// and finally make sure to internally attribute the code with a #region to the author(s).
//
// NOTE: Some code documentation may appear wrongly worded. This is due to auto-documentation
// using GhostDoc (http://www.roland-weigelt.de/ghostdoc/).
//
// Version History: Download README.txt for the version history.
//

#region Directives
// The following section provides
// directives for conditional compilation
// of various sections of the code.
// ======================================

// !!!EDIT DIRECTIVES START!!!

#if !(PD)

// Commonly non-referenced projects:
#define NOVJSLIB
#define NOSYSTEMWEB
#define NOASPELL

// Other switches:
//#define NOSCREENSCRAPER
//#define NOFEEDER
//#define NOCLSCOMPLIANTWARNINGSOFF
//#define NOTZ
//#define NOSTATES
//#define NOCODECOUNT
//#define NOLOGGING
//#define NODYNACODE
//#define NOASPNETRUNTIMEHOST

#endif

// !!!EDIT DIRECTIVES END!!!!!

// Dependency directives -- do not modify as they
// are very easy to break
#if NOANYTHING
#define NOIMPORTS
#endif

#if NOSYSTEMWEB
#define NOSCREENSCRAPER
#endif

#endregion // Directives

#region Meat
// All of the code

#if !(NOPINVOKE)
using System.Runtime.InteropServices;
#endif
#if !(NOVJSLIB)
using java.util.zip;
#endif
#if !(NOSYSTEMWEB)
using System.Web;
#endif

// Core includes
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Permissions;
//using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Serialization;
using PublicDomain;
using PublicDomain.Feeder.Opml;
using PublicDomain.Feeder.Rss;
using PublicDomain.Feeder.Atom;
using PublicDomain.Dynacode;
using Microsoft.Win32;

#if !(NOCLSCOMPLIANTWARNINGSOFF)
#pragma warning disable 3001
#pragma warning disable 3002
#pragma warning disable 3003
#pragma warning disable 3006
#pragma warning disable 3009
#endif

#if !(NOFEEDER)
namespace PublicDomain.Feeder
{
}

namespace PublicDomain.Feeder.Atom
{
    /// <summary>
    /// Specifies a category that the feed belongs to. A feed may have multiple category elements.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomCategory
    {
        /// <summary>
        /// Gets or sets the term.
        /// </summary>
        /// <value>The term.</value>
        string Term { get; set; }

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        /// <value>The scheme.</value>
        Uri Scheme { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        string Label { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomCategory : CachedPropertiesProvider, Feeder.Atom.IAtomCategory
    {
        #region IAtomCategory Members

        /// <summary>
        /// Gets or sets the term.
        /// </summary>
        /// <value>The term.</value>
        public string Term
        {
            get
            {
                return Getter("Term");
            }
            set
            {
                Setter("Term", value);
            }
        }

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        /// <value>The scheme.</value>
        public Uri Scheme
        {
            get
            {
                return Getter<Uri>("Scheme", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Scheme", value);
            }
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Label
        {
            get
            {
                return Getter("Label");
            }
            set
            {
                Setter("Label", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Contains or links to the complete content of the entry. Content must be provided if there is no alternate link, and should be provided if there is no summary.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomContent
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the SRC.
        /// </summary>
        /// <value>The SRC.</value>
        Uri Src { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        string Content { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomContent : CachedPropertiesProvider, IAtomContent
    {
        #region IAtomContent Members

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return Getter("Type");
            }
            set
            {
                Setter("Type", value);
            }
        }

        /// <summary>
        /// Gets or sets the SRC.
        /// </summary>
        /// <value>The SRC.</value>
        public Uri Src
        {
            get
            {
                return Getter<Uri>("Src", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Src", value);
            }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content
        {
            get
            {
                return Getter("Content");
            }
            set
            {
                Setter("Content", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Identifies the software used to generate the feed, for debugging and other purposes. Both the uri and version attributes are optional.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomGenerator
    {
        /// <summary>
        /// Gets or sets the generator URI.
        /// </summary>
        /// <value>The generator URI.</value>
        Uri GeneratorUri { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        string Version { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomGenerator : CachedPropertiesProvider, Feeder.Atom.IAtomGenerator
    {
        #region IAtomGenerator Members

        /// <summary>
        /// Gets or sets the generator URI.
        /// </summary>
        /// <value>The generator URI.</value>
        public Uri GeneratorUri
        {
            get
            {
                return Getter<Uri>("GeneratorUri", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("GeneratorUri", value);
            }
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version
        {
            get
            {
                return Getter("Version");
            }
            set
            {
                Setter("Version", value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("Description");
            }
            set
            {
                Setter("Description", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Identifies a related Web page.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomLink
    {
        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>The href.</value>
        Uri Href { get; set; }

        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        /// <value>The relationship.</value>
        string Relationship { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the link language.
        /// </summary>
        /// <value>The link language.</value>
        string LinkLanguage { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        int? Length { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomLink : CachedPropertiesProvider, Feeder.Atom.IAtomLink
    {
        #region IAtomLink Members

        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>The href.</value>
        public Uri Href
        {
            get
            {
                return Getter<Uri>("Href", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Href", value);
            }
        }

        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        /// <value>The relationship.</value>
        public string Relationship
        {
            get
            {
                return Getter("Relationship");
            }
            set
            {
                Setter("Relationship", value);
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return Getter("Type");
            }
            set
            {
                Setter("Type", value);
            }
        }

        /// <summary>
        /// Gets or sets the link language.
        /// </summary>
        /// <value>The link language.</value>
        public string LinkLanguage
        {
            get
            {
                return Getter("LinkLanguage");
            }
            set
            {
                Setter("LinkLanguage", value);
            }
        }

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
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public int? Length
        {
            get
            {
                return Getter<int?>("Length", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("Length", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Describes a person, corporation, or similar entity. It has one required element, name, and two optional elements: uri, email.
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomPerson
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the homepage.
        /// </summary>
        /// <value>The homepage.</value>
        Uri Homepage { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        string Email { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomPerson : CachedPropertiesProvider, Feeder.Atom.IAtomPerson
    {
        #region IAtomPerson Members

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return Getter("Name");
            }
            set
            {
                Setter("Name", value);
            }
        }

        /// <summary>
        /// Gets or sets the homepage.
        /// </summary>
        /// <value>The homepage.</value>
        public Uri Homepage
        {
            get
            {
                return Getter<Uri>("Homepage", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Homepage", value);
            }
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get
            {
                return Getter("Email");
            }
            set
            {
                Setter("Email", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Contains human-readable text, usually in small quantities. The type attribute determines how this information is encoded (default="text")
    /// Taken verbatim from http://www.atomenabled.org/developers/syndication/.
    /// </summary>
    public interface IAtomText
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomText : CachedPropertiesProvider, Feeder.Atom.IAtomText
    {
        #region IAtomText Members

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return Getter("Type");
            }
            set
            {
                Setter("Type", value);
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                return Getter("Text");
            }
            set
            {
                Setter("Text", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtomFeedParser : FeedParser
    {
        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override T Parse<T>(System.Xml.XmlReader reader)
        {
            IAtomFeed ret = (IAtomFeed)new AtomFeed();
            reader.Read();

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "id":
                            ret.FeedUri = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString(), reader.BaseURI);
                            break;
                        case "title":
                            ret.Title = reader.ReadElementContentAsString();
                            break;
                        case "updated":
                            ret.LastUpdated = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "generator":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Generator = ConvertToIAtomGenerator(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "author":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Authors.Add(ConvertToIAtomPerson(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "link":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Links.Add(ConvertToIAtomLink(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "category":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Categories.Add(ConvertToIAtomCategory(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "entry":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Items.Add(ParseItem(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "contributor":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Contributors.Add(ConvertToIAtomPerson(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "logo":
                            ret.Logo = reader.ReadElementContentAsString();
                            break;
                        case "icon":
                            ret.Icon = reader.ReadElementContentAsString();
                            break;
                        case "rights":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Rights = ConvertToIAtomText(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "subtitle":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Subtitle = ConvertToIAtomText(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        default:
                            UnhandledElement(ret, reader);
                            break;
                    }
                }
            }
            reader.Close();
            return (T)ret;
        }

        /// <summary>
        /// Parses the item.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public IFeedItem ParseItem(XmlReader reader)
        {
            IAtomFeedItem item = new AtomFeedItem();
            reader.ReadToDescendant("entry");

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "id":
                            item.Id = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString(), reader.BaseURI);
                            break;
                        case "title":
                            item.Title = reader.ReadElementContentAsString();
                            break;
                        case "updated":
                            item.LastUpdated = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "author":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Authors.Add(ConvertToIAtomPerson(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "link":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Links.Add(ConvertToIAtomLink(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "category":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Categories.Add(ConvertToIAtomCategory(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "contributor":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Contributors.Add(ConvertToIAtomPerson(subReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "rights":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Rights = ConvertToIAtomText(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "summary":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Summary = ConvertToIAtomText(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "content":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Content = ConvertToIAtomContent(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "published":
                            item.Published = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "source":
                            item.Source = ConvertToIAtomFeed(reader.ReadOuterXml());
                            break;
                        default:
                            UnhandledElement(item, reader);
                            break;
                    }
                }
            }
            reader.Close();
            return item;
        }

        /// <summary>
        /// Converts to I atom generator.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomGenerator ConvertToIAtomGenerator(XmlReader input)
        {
            AtomGenerator gen = new AtomGenerator();
            input.ReadToDescendant("generator");
            gen.GeneratorUri = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("uri"), input.BaseURI);
            gen.Version = input.GetAttribute("version");
            gen.Description = input.ReadElementContentAsString();
            input.Close();
            return gen;
        }

        /// <summary>
        /// Converts to I atom generator.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomGenerator ConvertToIAtomGenerator(string input)
        {
            return ConvertToIAtomGenerator(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I atom text.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomText ConvertToIAtomText(XmlReader input)
        {
            AtomText text = new AtomText();
            input.Read();
            text.Type = input.GetAttribute("type");
            text.Text = input.ReadInnerXml();
            input.Close();
            return text;
        }

        /// <summary>
        /// Converts to I atom text.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomText ConvertToIAtomText(string input)
        {
            return ConvertToIAtomText(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts the content of to I atom.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomContent ConvertToIAtomContent(XmlReader input)
        {
            AtomContent content = new AtomContent();
            input.ReadToDescendant("content");
            content.Type = input.GetAttribute("type");
            content.Src = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("src"), input.BaseURI);
            content.Content = input.ReadInnerXml();
            input.Close();
            return content;
        }

        /// <summary>
        /// Converts the content of to I atom.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomContent ConvertToIAtomContent(string input)
        {
            return ConvertToIAtomContent(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I atom feed.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomFeed ConvertToIAtomFeed(string input)
        {
            using (StringReader stringReader = new StringReader(input))
            {
                return new AtomFeedParser().CreateFeed<IAtomFeed>(input);
            }
        }

        /// <summary>
        /// Converts to I atom person.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomPerson ConvertToIAtomPerson(XmlReader input)
        {
            AtomPerson person = new AtomPerson();
            input.Read();
            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "name":
                            person.Name = input.ReadElementContentAsString();
                            break;
                        case "uri":
                            person.Homepage = CachedPropertiesProvider.ConvertToUri(input.ReadElementContentAsString(), input.BaseURI);
                            break;
                        case "email":
                            person.Email = input.ReadElementContentAsString();
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
            input.Close();
            return person;
        }

        /// <summary>
        /// Converts to I atom link.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomLink ConvertToIAtomLink(XmlReader input)
        {
            AtomLink link = new AtomLink();
            input.ReadToDescendant("link");
            link.Href = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("href"), input.BaseURI);
            link.Relationship = input.GetAttribute("rel");
            link.Type = input.GetAttribute("type");
            link.LinkLanguage = input.GetAttribute("hreflang");
            link.Title = input.GetAttribute("title");
            link.Length = CachedPropertiesProvider.ConvertToIntNullable(input.GetAttribute("length"));
            input.Close();
            return link;
        }

        /// <summary>
        /// Converts to I atom category.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IAtomCategory ConvertToIAtomCategory(XmlReader input)
        {
            AtomCategory cat = new AtomCategory();
            input.ReadToDescendant("link");
            cat.Label = input.GetAttribute("label");
            cat.Term = input.GetAttribute("term");
            cat.Scheme = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("scheme"), input.BaseURI);
            input.Close();
            return cat;
        }
    }
}

namespace PublicDomain.Feeder.Opml
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOpmlBody : IOpmlOutlineProvider
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class OpmlBody : OpmlOutlineProvider, IOpmlBody
    {
    }

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

    /// <summary>
    /// 
    /// </summary>
    public interface IOpmlOutline : IOpmlOutlineProvider
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the is comment.
        /// </summary>
        /// <value>The is comment.</value>
        bool? IsComment { get; set; }

        /// <summary>
        /// Gets or sets the is breakpoint.
        /// </summary>
        /// <value>The is breakpoint.</value>
        bool? IsBreakpoint { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OpmlOutline : OpmlOutlineProvider, IOpmlOutline
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpmlOutline"/> class.
        /// </summary>
        public OpmlOutline()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpmlOutline"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public OpmlOutline(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpmlOutline"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="type">The type.</param>
        public OpmlOutline(string text, string type)
        {
            this.Text = text;
            this.Type = type;
        }

        #region IOpmlOutline Members

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                return Getter("Text");
            }
            set
            {
                Setter("Text", value);
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return Getter("Type");
            }
            set
            {
                Setter("Type", value);
            }
        }

        /// <summary>
        /// Gets or sets the is comment.
        /// </summary>
        /// <value>The is comment.</value>
        public bool? IsComment
        {
            get
            {
                return Getter<bool?>("IsComment", CachedPropertiesProvider.ConvertToBoolNullable);
            }
            set
            {
                Setter("IsComment", value);
            }
        }

        /// <summary>
        /// Gets or sets the is breakpoint.
        /// </summary>
        /// <value>The is breakpoint.</value>
        public bool? IsBreakpoint
        {
            get
            {
                return Getter<bool?>("IsBreakpoint", CachedPropertiesProvider.ConvertToBoolNullable);
            }
            set
            {
                Setter("IsBreakpoint", value);
            }
        }

        #endregion
    }

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

    /// <summary>
    /// 
    /// </summary>
    public class OpmlParser : Parser
    {
        /// <summary>
        /// Creates the feed base.
        /// </summary>
        /// <param name="feedReader">The feed reader.</param>
        /// <returns></returns>
        protected override T CreateFeedBase<T>(XmlReader feedReader)
        {
            feedReader.MoveToContent();
            return new OpmlParser().Parse<T>(feedReader);
        }

        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override T Parse<T>(System.Xml.XmlReader reader)
        {
            IOpmlFeed ret = new OpmlFeed();

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "head":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Head = ConvertToIOpmlHead(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "body":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Body = ConvertToIOpmlBody(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        default:
                            UnhandledElement(ret, reader);
                            break;
                    }
                }
            }
            reader.Close();
            return (T)ret;
        }

        internal static IOpmlOutline ConvertToIOpmlOutline(string input)
        {
            return ConvertToIOpmlOutline(CreateXmlReaderFromString(input));
        }

        internal static IOpmlOutline ConvertToIOpmlOutline(XmlReader input)
        {
            OpmlOutline outline = new OpmlOutline();
            input.ReadToDescendant("outline");
            outline.Text = input.GetAttribute("text");
            outline.Type = input.GetAttribute("type");
            outline.IsBreakpoint = CachedPropertiesProvider.ConvertToBoolNullable(input.GetAttribute("isBreakpoint"));
            outline.IsComment = CachedPropertiesProvider.ConvertToBoolNullable(input.GetAttribute("isComment"));
            ReadOutlines(input, outline);
            input.Close();
            return outline;
        }

        internal static IOpmlBody ConvertToIOpmlBody(string input)
        {
            return ConvertToIOpmlBody(CreateXmlReaderFromString(input));
        }

        internal static IOpmlBody ConvertToIOpmlBody(XmlReader input)
        {
            OpmlBody body = new OpmlBody();
            input.ReadToDescendant("body");
            ReadOutlines(input, body);
            input.Close();
            return body;
        }

        private static void ReadOutlines(XmlReader input, IOpmlOutlineProvider outlineProvider)
        {
            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "outline":
                            using (XmlReader subReader = input.ReadSubtree())
                            {
                                outlineProvider.Outlines.Add(ConvertToIOpmlOutline(subReader));
                            }
                            if (input.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
        }

        internal static IOpmlHead ConvertToIOpmlHead(string input)
        {
            return ConvertToIOpmlHead(CreateXmlReaderFromString(input));
        }

        internal static IOpmlHead ConvertToIOpmlHead(XmlReader input)
        {
            OpmlHead head = new OpmlHead();
            input.ReadToDescendant("head");
            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "title":
                            head.Title = input.ReadElementContentAsString();
                            break;
                        case "dateCreated":
                            head.DateCreated = CachedPropertiesProvider.ConvertToTzDateTime(input.ReadElementContentAsString());
                            break;
                        case "dateModified":
                            head.DateModified = CachedPropertiesProvider.ConvertToTzDateTime(input.ReadElementContentAsString());
                            break;
                        case "ownerName":
                            head.Owner = input.ReadElementContentAsString();
                            break;
                        case "ownerEmail":
                            head.OwnerEmail = input.ReadElementContentAsString();
                            break;
                        case "expansionState":
                            head.ExpansionState = input.ReadElementContentAsString();
                            break;
                        case "vertScrollState":
                            head.VerticalScrollState = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        case "windowTop":
                            head.WindowTop = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        case "windowLeft":
                            head.WindowLeft = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        case "windowBottom":
                            head.WindowBottom = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        case "windowRight":
                            head.WindowRight = CachedPropertiesProvider.ConvertToIntNullable(input.ReadElementContentAsString());
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
            input.Close();
            return head;
        }
    }
}

namespace PublicDomain.Feeder.Rss
{
    /// <summary>
    /// In RSS 2.0, a provision is made for linking a channel to its identifier in a cataloging system, using the channel-level category feature. For example, to link a channel to its Syndic8 identifier, include a category element as a sub-element of channel, with domain "Syndic8", and value the identifier for your channel in the Syndic8 database. The appropriate category element for Scripting News would be <category domain="Syndic8">1765</category>.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssCategory
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
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>The name of the category.</value>
        string CategoryName
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssCategory : CachedPropertiesProvider, Feeder.Rss.IRssCategory
    {
        #region IRssCategory Members

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
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>The name of the category.</value>
        public string CategoryName
        {
            get
            {
                return Getter("CategoryName");
            }
            set
            {
                Setter("CategoryName", value);
            }
        }

        #endregion
    }

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

    /// <summary>
    /// Describes a media object that is attached to the item.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssEnclosure
    {
        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        int Length
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssEnclosure : CachedPropertiesProvider, Feeder.Rss.IRssEnclosure
    {
        #region IRssEnclosure Members

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get
            {
                return Getter<Uri>("Link", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Link", value);
            }
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public int Length
        {
            get
            {
                return Getter<int>("Length", CachedPropertiesProvider.ConvertToInt);
            }
            set
            {
                Setter("Length", value);
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return Getter("Type");
            }
            set
            {
                Setter("Type", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// guid stands for globally unique identifier. It's a string that uniquely identifies the item. When present, an aggregator may choose to use this string to determine if an item is new.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssGuid
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        string UniqueIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the is perma link.
        /// </summary>
        /// <value>The is perma link.</value>
        bool? IsPermaLink
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssGuid : CachedPropertiesProvider, Feeder.Rss.IRssGuid
    {
        #region IRssGuid Members

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public string UniqueIdentifier
        {
            get
            {
                return Getter("UniqueIdentifier");
            }
            set
            {
                Setter("UniqueIdentifier", value);
            }
        }

        /// <summary>
        /// Gets or sets the is perma link.
        /// </summary>
        /// <value>The is perma link.</value>
        public bool? IsPermaLink
        {
            get
            {
                return Getter<bool?>("IsPermaLink", CachedPropertiesProvider.ConvertToBoolNullable);
            }
            set
            {
                Setter("IsPermaLink", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Specifies a GIF, JPEG or PNG image that can be displayed with the channel.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssImage
    {
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        Uri Location
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        int? Width
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        int? Height
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssImage : CachedPropertiesProvider, Feeder.Rss.IRssImage
    {
        #region IRssImage Members

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Uri Location
        {
            get
            {
                return Getter<Uri>("Location", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Location", value);
            }
        }

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
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get
            {
                return Getter<Uri>("Link", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Link", value);
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int? Width
        {
            get
            {
                return Getter<int?>("Width", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("Width", value);
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int? Height
        {
            get
            {
                return Getter<int?>("Height", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("Height", value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("Description");
            }
            set
            {
                Setter("Description", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Its value is the name of the RSS channel that the item came from, derived from its title. It has one required attribute, url, which links to the XMLization of the source.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssSource
    {
        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssSource : CachedPropertiesProvider, Feeder.Rss.IRssSource
    {
        #region IRssSource Members

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get
            {
                return Getter<Uri>("Link", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Link", value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("Description");
            }
            set
            {
                Setter("Description", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// The purpose of the textInput element is something of a mystery. You can use it to specify a search engine box. Or to allow a reader to provide feedback. Most aggregators ignore it.
    /// Taken verbatim from http://blogs.law.harvard.edu/tech/rss.
    /// </summary>
    public interface IRssTextInput
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        Uri Link
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssTextInput : CachedPropertiesProvider, Feeder.Rss.IRssTextInput
    {
        #region IRssTextInput Members

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
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("Description");
            }
            set
            {
                Setter("Description", value);
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return Getter("Name");
            }
            set
            {
                Setter("Name", value);
            }
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get
            {
                return Getter<Uri>("Link", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Link", value);
            }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class RssFeedParser : FeedParser
    {
        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override T Parse<T>(System.Xml.XmlReader reader)
        {
            IRssFeed ret = (IRssFeed)new RssFeed();
            reader.Read();

            // RDF versions of RSS don't have version tags.
            //double version = double.Parse(reader.GetAttribute("version"));

            reader.ReadToDescendant("channel");

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "title":
                            ret.Title = reader.ReadElementContentAsString();
                            break;
                        case "link":
                            ret.FeedUri = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString());
                            break;
                        case "description":
                            ret.Description = reader.ReadElementContentAsString();
                            break;
                        case "language":
                            ret.Culture = CachedPropertiesProvider.ConvertToCultureInfo(reader.ReadElementContentAsString());
                            break;
                        case "copyright":
                            ret.Copyright = reader.ReadElementContentAsString();
                            break;
                        case "managingEditor":
                            ret.ManagingEditor = reader.ReadElementContentAsString();
                            break;
                        case "webMaster":
                            ret.WebMaster = reader.ReadElementContentAsString();
                            break;
                        case "pubDate":
                            ret.PublicationDate = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "lastBuildDate":
                            ret.LastChanged = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "category":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Category = ConvertToIRssCategory(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "generator":
                            ret.Generator = reader.ReadElementContentAsString();
                            break;
                        case "docs":
                            ret.Doc = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString());
                            break;
                        case "cloud":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Cloud = ConvertToIRssCloud(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "ttl":
                            ret.TimeToLive = CachedPropertiesProvider.ConvertToInt(reader.ReadElementContentAsString());
                            break;
                        case "image":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.Image = ConvertToIRssImage(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        /*case "rating":
                            break;*/
                        case "textInput":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.TextInput = ConvertToIRssTextInput(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "skipHours":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.SkipHours = ConvertToSkipHourList(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "skipDays":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                ret.SkipDays = ConvertToDayOfWeekList(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "item":
                            using (XmlReader itemReader = reader.ReadSubtree())
                            {
                                ret.Items.Add(ParseItem(itemReader));
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        default:
                            UnhandledElement(ret, reader);
                            break;
                    }
                }
            }
            reader.Close();
            return (T)ret;
        }

        /// <summary>
        /// Parses the item.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public IFeedItem ParseItem(XmlReader reader)
        {
            IRssFeedItem item = new RssFeedItem();
            reader.ReadToDescendant("item");

            bool readContent = false;
            while (readContent || reader.Read())
            {
                readContent = false;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (reader.Name)
                    {
                        case "author":
                            item.Author = reader.ReadElementContentAsString();
                            break;
                        case "category":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Category = ConvertToIRssCategory(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "comments":
                            item.Comments = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString());
                            break;
                        case "description":
                            item.Description = reader.ReadElementContentAsString();
                            break;
                        case "enclosure":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Enclosure = ConvertToIRssEnclosure(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "guid":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Guid = ConvertToIRssGuid(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "link":
                            item.Link = CachedPropertiesProvider.ConvertToUri(reader.ReadElementContentAsString());
                            break;
                        case "pubDate":
                            item.PublicationDate = CachedPropertiesProvider.ConvertToTzDateTime(reader.ReadElementContentAsString());
                            break;
                        case "source":
                            using (XmlReader subReader = reader.ReadSubtree())
                            {
                                item.Source = ConvertToIRssSource(subReader);
                            }
                            if (reader.IsEmptyElement)
                            {
                                readContent = false;
                            }
                            break;
                        case "title":
                            item.Title = reader.ReadElementContentAsString();
                            break;
                        default:
                            UnhandledElement(item, reader);
                            break;
                    }
                }
            }
            reader.Close();
            return item;
        }

        /// <summary>
        /// Converts to I RSS cloud.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssCloud ConvertToIRssCloud(XmlReader input)
        {
            RssCloud cloud = new RssCloud();
            input.ReadToDescendant("cloud");
            cloud.Domain = input.GetAttribute("domain");
            cloud.Port = CachedPropertiesProvider.ConvertToInt(input.GetAttribute("port"));
            cloud.Path = input.GetAttribute("path");
            cloud.Protocol = ConvertToRssCloudProtocol(input.GetAttribute("protocol"));
            cloud.RegisterProcedure = input.GetAttribute("registerProcedure");
            input.Close();
            return cloud;
        }

        /// <summary>
        /// Converts to I RSS cloud.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssCloud ConvertToIRssCloud(string input)
        {
            return ConvertToIRssCloud(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS category.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssCategory ConvertToIRssCategory(XmlReader input)
        {
            RssCategory cat = new RssCategory();
            input.ReadToDescendant("category");
            cat.Domain = input.GetAttribute("domain");
            cat.CategoryName = input.ReadElementContentAsString();
            input.Close();
            return cat;
        }

        /// <summary>
        /// Converts to I RSS category.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssCategory ConvertToIRssCategory(string input)
        {
            return ConvertToIRssCategory(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS text input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssTextInput ConvertToIRssTextInput(XmlReader input)
        {
            RssTextInput text = new RssTextInput();
            input.ReadToDescendant("textInput");

            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "description":
                            text.Description = input.ReadElementContentAsString();
                            break;
                        case "link":
                            text.Link = CachedPropertiesProvider.ConvertToUri(input.ReadElementContentAsString());
                            break;
                        case "name":
                            text.Name = input.ReadElementContentAsString();
                            break;
                        case "title":
                            text.Title = input.ReadElementContentAsString();
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
            input.Close();
            return text;
        }

        /// <summary>
        /// Converts to I RSS text input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssTextInput ConvertToIRssTextInput(string input)
        {
            return ConvertToIRssTextInput(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to day of week list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IList<DayOfWeek> ConvertToDayOfWeekList(XmlReader input)
        {
            IList<DayOfWeek> skipDays = new List<DayOfWeek>();
            input.ReadToDescendant("skipDays");

            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element && input.Name == "day")
                {
                    skipDays.Add(ConvertToDayOfWeek(input.ReadElementContentAsString()));
                    readContent = true;
                }
            }
            input.Close();
            return skipDays;
        }

        /// <summary>
        /// Converts to day of week list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IList<DayOfWeek> ConvertToDayOfWeekList(string input)
        {
            return ConvertToDayOfWeekList(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to skip hour list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IList<uint> ConvertToSkipHourList(XmlReader input)
        {
            IList<uint> skipHours = new List<uint>();
            input.ReadToDescendant("skipHours");

            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element && input.Name == "hour")
                {
                    skipHours.Add(CachedPropertiesProvider.ConvertToUInt(input.ReadElementContentAsString()));
                    readContent = true;
                }
            }
            input.Close();
            return skipHours;
        }

        /// <summary>
        /// Converts to skip hour list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IList<uint> ConvertToSkipHourList(string input)
        {
            return ConvertToSkipHourList(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS image.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssImage ConvertToIRssImage(XmlReader input)
        {
            RssImage image = new RssImage();
            input.ReadToDescendant("image");

            bool readContent = false;
            while (readContent || input.Read())
            {
                readContent = false;
                if (input.NodeType == XmlNodeType.Element)
                {
                    readContent = true;
                    switch (input.Name)
                    {
                        case "description":
                            image.Description = input.ReadElementContentAsString();
                            break;
                        case "link":
                            image.Link = CachedPropertiesProvider.ConvertToUri(input.ReadElementContentAsString());
                            break;
                        case "location":
                            image.Location = CachedPropertiesProvider.ConvertToUri(input.ReadElementContentAsString());
                            break;
                        case "title":
                            image.Title = input.ReadElementContentAsString();
                            break;
                        case "width":
                            image.Width = CachedPropertiesProvider.ConvertToInt(input.ReadElementContentAsString());
                            break;
                        case "height":
                            image.Height = CachedPropertiesProvider.ConvertToInt(input.ReadElementContentAsString());
                            break;
                        default:
                            readContent = false;
                            break;
                    }
                }
            }
            input.Close();
            return image;
        }

        /// <summary>
        /// Converts to I RSS image.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssImage ConvertToIRssImage(string input)
        {
            return ConvertToIRssImage(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS enclosure.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssEnclosure ConvertToIRssEnclosure(XmlReader input)
        {
            RssEnclosure enc = new RssEnclosure();
            input.ReadToDescendant("enclosure");
            enc.Length = CachedPropertiesProvider.ConvertToInt(input.GetAttribute("length"));
            enc.Link = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("url"));
            enc.Type = input.GetAttribute("type");
            input.Close();
            return enc;
        }

        /// <summary>
        /// Converts to I RSS enclosure.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssEnclosure ConvertToIRssEnclosure(string input)
        {
            return ConvertToIRssEnclosure(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS GUID.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssGuid ConvertToIRssGuid(XmlReader input)
        {
            RssGuid guid = new RssGuid();
            input.ReadToDescendant("guid");
            guid.IsPermaLink = CachedPropertiesProvider.ConvertToBoolNullable(input.GetAttribute("isPermaLink"));
            guid.UniqueIdentifier = input.ReadElementContentAsString();
            input.Close();
            return guid;
        }

        /// <summary>
        /// Converts to I RSS GUID.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssGuid ConvertToIRssGuid(string input)
        {
            return ConvertToIRssGuid(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to I RSS source.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssSource ConvertToIRssSource(XmlReader input)
        {
            RssSource source = new RssSource();
            input.ReadToDescendant("source");
            source.Link = CachedPropertiesProvider.ConvertToUri(input.GetAttribute("url"));
            source.Description = input.ReadElementContentAsString();
            input.Close();
            return source;
        }

        /// <summary>
        /// Converts to I RSS source.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IRssSource ConvertToIRssSource(string input)
        {
            return ConvertToIRssSource(CreateXmlReaderFromString(input));
        }

        /// <summary>
        /// Converts to RSS cloud protocol.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static RssCloudProtocol ConvertToRssCloudProtocol(string input)
        {
            input = input.Trim().ToLower();
            switch (input)
            {
                case "xml-rpc":
                    return RssCloudProtocol.XmlRpc;
                case "http-post":
                    return RssCloudProtocol.HttpPost;
                case "soap":
                    return RssCloudProtocol.Soap;
                default:
                    return RssCloudProtocol.Unknown;
            }
        }

        /// <summary>
        /// Converts to day of week.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static DayOfWeek ConvertToDayOfWeek(string input)
        {
            return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), input);
        }
    }
}
#endif

#if !(NOLOGGING)
namespace PublicDomain.Logging
{
    /// <summary>
    /// Severity of the log entry. The numeric value of the severity
    /// is in the name itself for immediate feedback.
    /// </summary>
    public enum LoggerSeverity
    {
        /// <summary>
        /// Lowest severity.
        /// </summary>
        None0 = 0,

        /// <summary>
        /// Detailed programmatic informational messages used
        /// as an aid in troubleshooting problems by programmers.
        /// </summary>
        Debug10 = 10,

        /// <summary>
        /// Brief informative messages to use as an aid in
        /// troubleshooting problems by production support and programmers.
        /// </summary>
        Info20 = 20,

        /// <summary>
        /// Messages intended to notify help desk, production support and programmers
        /// of possible issues with respect to the running application.
        /// </summary>
        Warn30 = 30,

        /// <summary>
        /// Messages that detail a programmatic error, these are typically messages
        /// intended for help desk, production support, programmers and occasionally users.
        /// </summary>
        Error40 = 40,

        /// <summary>
        /// Severe messages that are programmatic violations that will usually
        /// result in application failure. These messages are intended for help
        /// desk, production support, programmers and possibly users.
        /// </summary>
        Fatal50 = 50,

        /// <summary>
        /// 
        /// </summary>
        Infinity = int.MaxValue
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ILogFormatter
    {
        /// <summary>
        /// Formats the entry.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="category">The category.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        string FormatEntry(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string category, Dictionary<string, object> data);

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        /// <value>The format string.</value>
        string FormatString { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ILogFilter
    {
        /// <summary>
        /// Determines whether the specified severity is loggable.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <returns>
        /// 	<c>true</c> if the specified severity is loggable; otherwise, <c>false</c>.
        /// </returns>
        bool IsLoggable(LoggerSeverity threshold, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters);
    }

    /// <summary>
    /// There is no interface for this class to allow for certain methods
    /// to be overriden and removed in debug builds.
    /// </summary>
    public abstract class Logger
    {
        private LoggerSeverity m_threshold = LoggerSeverity.Warn30;

        private List<ILogFilter> m_filters = new List<ILogFilter>();

        private ILogFormatter m_formatter = new DefaultLogFormatter();

        private string m_category;

        private Dictionary<string, object> m_data = new Dictionary<string, object>();

        private static Dictionary<int, int> m_stack = new Dictionary<int, int>();

        internal static int LogStackCount
        {
            get
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                if (!m_stack.ContainsKey(threadId))
                {
                    m_stack[threadId] = 0;
                }
                return m_stack[threadId];
            }
            set
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                m_stack[threadId] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger()
            : this(new DefaultLogFilter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger(ILogFilter logFilter)
        {
            if (logFilter != null)
            {
                AddLogFilter(logFilter);
            }
        }

        /// <summary>
        /// The severity threshold at which point a log message
        /// is logged. For example, if the threshold is Debug,
        /// all messages with severity greater than or equal to Debug
        /// will be logged. All other messages will be discarded.
        /// The default threshold is Warn.
        /// </summary>
        /// <value></value>
        public virtual LoggerSeverity Threshold
        {
            get
            {
                return m_threshold;
            }
            set
            {
                m_threshold = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <value></value>
        public virtual ILogFormatter Formatter
        {
            get
            {
                return m_formatter;
            }
            set
            {
                m_formatter = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <value></value>
        public virtual List<ILogFilter> Filters
        {
            get
            {
                return m_filters;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        public virtual Dictionary<string, object> Data
        {
            get
            {
                return m_data;
            }
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public virtual string Category
        {
            get
            {
                return m_category;
            }
            set
            {
                m_category = value;
            }
        }

        /// <summary>
        /// Adds the log filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public virtual void AddLogFilter(ILogFilter filter)
        {
            Filters.Add(filter);
        }

        /// <summary>
        /// Removes the log filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public virtual void RemoveLogFilter(ILogFilter filter)
        {
            Filters.Remove(filter);
        }

        /// <summary>
        /// Clears the log filters.
        /// </summary>
        public virtual void ClearLogFilters()
        {
            Filters.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public virtual void Log(LoggerSeverity severity, object entry, params object[] formatParameters)
        {
            // Get the current timestamp
            DateTime timestamp = DateTime.UtcNow;

            // Check all the filters
            if (Filters != null)
            {
                foreach (ILogFilter filter in Filters)
                {
                    if (!filter.IsLoggable(Threshold, severity, timestamp, entry, formatParameters))
                    {
                        return;
                    }
                }
            }

            string logLine = null;

            if (Formatter == null)
            {
                if (entry != null)
                {
                    logLine = entry.ToString();
                }
            }
            else
            {
                logLine = Formatter.FormatEntry(severity, timestamp, entry, formatParameters, Category, Data);
            }

            DoLog(severity, timestamp, entry, formatParameters, logLine);
        }

        /// <summary>
        /// High level final log that is called with all of the detailed information
        /// and the final log line as the last parameter.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="logLine">The log line.</param>
        protected virtual void DoLog(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
            DoLog(logLine);
        }

        /// <summary>
        /// Called by the detailed version, forgetting about the details
        /// and simply having the final log line.
        /// </summary>
        /// <param name="logLine">The log line.</param>
        protected abstract void DoLog(string logLine);

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public virtual void LogDebug10(object entry, params object[] formatParameters)
        {
            Log(LoggerSeverity.Debug10, entry, formatParameters);
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public virtual void LogInfo20(object entry, params object[] formatParameters)
        {
            Log(LoggerSeverity.Info20, entry, formatParameters);
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public virtual void LogWarn30(object entry, params object[] formatParameters)
        {
            Log(LoggerSeverity.Warn30, entry, formatParameters);
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public virtual void LogError40(object entry, params object[] formatParameters)
        {
            Log(LoggerSeverity.Error40, entry, formatParameters);
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public virtual void LogFatal50(object entry, params object[] formatParameters)
        {
            Log(LoggerSeverity.Fatal50, entry, formatParameters);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public virtual void LogException(Exception ex)
        {
            LogException(ex, LoggerSeverity.Error40);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="severity">The severity.</param>
        public virtual void LogException(Exception ex, LoggerSeverity severity)
        {
            Log(severity, ex);
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        [Conditional("DEBUG")]
        public virtual void Start(params object[] args)
        {
            LogEntryExit(true, true, args);
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        [Conditional("DEBUG")]
        public virtual void End(params object[] args)
        {
            LogEntryExit(false, true, args);
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        [Conditional("DEBUG")]
        public virtual void WhereAmI(params object[] args)
        {
            LogEntryExit(false, false, args);
        }

        /// <summary>
        /// Logs the entry exit.
        /// </summary>
        /// <param name="isEntry">if set to <c>true</c> [is entry].</param>
        /// <param name="useMarker">if set to <c>true</c> [use marker].</param>
        /// <param name="args">The args.</param>
        protected virtual void LogEntryExit(bool isEntry, bool useMarker, object[] args)
        {
            StackTrace trace = new StackTrace(true);
            StackFrame caller = trace.GetFrame(2);
            MethodBase method = caller.GetMethod();
            int cnt = LogStackCount;

            if (useMarker)
            {
                cnt += (isEntry ? 1 : -1);

                if (cnt < 0)
                {
                    cnt = 0;
                }

                LogStackCount = cnt;
            }

            StringBuilder sb = new StringBuilder();
            if (useMarker)
            {
                if (isEntry)
                {
                    cnt--;
                }
            }
            else
            {
                cnt++;
            }

            if (cnt > 0)
            {
                sb.Append(' ', cnt);
            }

            if (useMarker)
            {
                if (isEntry)
                {
                    sb.Append("> ");
                }
                else
                {
                    sb.Append("< ");
                }
            }

            sb.AppendFormat(
                "{0}.{1} [{2}]",
                method.DeclaringType,
                method.Name,
                caller.GetFileLineNumber()
            );

            if (args != null)
            {
                sb.Append(" (");
                for (int i = 0; i < args.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    if (args[i] == null)
                    {
                        sb.Append("[null]");
                    }
                    else
                    {
                        sb.Append(args[i]);
                    }
                }
                sb.Append(")");
            }
            StackFrame caller2 = trace.GetFrame(3);
            if (caller2 != null)
            {
                sb.AppendFormat(
                    " {{{0}:{1}:{2}}}",
                    caller2.GetMethod().Name,
                    caller2.GetFileName(),
                    caller2.GetFileLineNumber()
                );
            }
            LogDebug10(sb.ToString());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileLogger : Logger
    {
        private string m_fileName;

        private static Dictionary<string, FileStream> m_streams = new Dictionary<string, FileStream>();

        /// <summary>
        /// 
        /// </summary>
        public static bool CacheFileStreams = false;

        private static object m_streamsLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        public FileLogger()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public FileLogger(string fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        ~FileLogger()
        {
            CloseAllStreams();
        }

        private static void CloseAllStreams()
        {
            lock (m_streamsLock)
            {
                foreach (FileStream fileStream in m_streams.Values)
                {
                    fileStream.Flush();
                    fileStream.Close();
                }
                m_streams.Clear();
            }
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public virtual string FileName
        {
            get
            {
                return m_fileName;
            }
            set
            {
                m_fileName = value;
                if (!string.IsNullOrEmpty(m_fileName))
                {
                    FileSystemUtilities.EnsureDirectoriesInPath(m_fileName);
                }
            }
        }

        /// <summary>
        /// High level final log that is called with all of the detailed information
        /// and the final log line as the last parameter.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
            string fileName = GetFileName(severity, timestamp, entry, formatParameters, logLine);

            if (!string.IsNullOrEmpty(fileName))
            {
                FileStream stream = GetStream(fileName);

                try
                {
                    byte[] data = Encoding.Default.GetBytes(logLine);

                    stream.Write(data, 0, data.Length);
                    stream.WriteByte((byte)'\n');
                    stream.Flush();
                }
                finally
                {
                    if (!CacheFileStreams)
                    {
                        stream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        protected virtual FileStream GetStream(string fileName)
        {
            FileStream result = null;

            lock (m_streamsLock)
            {
                if (!CacheFileStreams || !m_streams.TryGetValue(fileName, out result))
                {
                    result = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                    if (CacheFileStreams)
                    {
                        m_streams[fileName] = result;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Called by the detailed version, forgetting about the details
        /// and simply having the final log line.
        /// </summary>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(string logLine)
        {
            // Should never get here
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetFileName(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
            return FileName;
        }
    }

    /// <summary>
    /// Provides a common application logger, which writes to a rolling
    /// log file in the application's working directory. The logger
    /// always logs severe log events using the <see cref="PublicDomain.Logging.SevereLogFilter"/>,
    /// and by default, uses the default Logger <see cref="PublicDomain.Logging.Logger.Threshold"/> value of Warn.
    /// </summary>
    public class ApplicationLogger : CompositeLogger
    {
        /// <summary>
        /// Static logger provides a common application logger, which writes to a rolling
        /// log file in the application's working directory. The logger
        /// always logs severe log events using the <see cref="PublicDomain.Logging.SevereLogFilter"/>,
        /// and by default, uses the default Logger <see cref="PublicDomain.Logging.Logger.Threshold"/> value of Warn.
        /// </summary>
        public static ApplicationLogger Current = new ApplicationLogger();

        /// <summary>
        /// Provides a common application logger, which writes to a rolling
        /// log file in the application's working directory. The logger
        /// always logs severe log events using the <see cref="PublicDomain.Logging.SevereLogFilter"/>,
        /// and by default, uses the default Logger <see cref="PublicDomain.Logging.Logger.Threshold"/> value.
        /// Initializes a new instance of the <see cref="ApplicationLogger"/> class.
        /// </summary>
        public ApplicationLogger()
        {
            // Figure out where we'll be logging the files
            string fileNameFormatted = FileSystemUtilities.PathCombine(Environment.CurrentDirectory, @"\app{0}.log");
            Loggers.Add(new RollingFileLogger(fileNameFormatted));
            AddLogFilter(new SevereLogFilter());

            // So that we know where the log is going
            string msg = string.Format("Application logging to {0}", fileNameFormatted);
            Console.WriteLine(msg);

#if DEBUG
            // Sometimes the Console does not go anywhere logical (or nowhere at all),
            // so it becomes difficult to know where the current directory is. Therefore,
            // we write the same message to a global file
            try
            {
                Logger loggers = new FileLogger(GlobalConstants.PublicDomainDefaultInstallLocation + @"loggers.log");
                loggers.Threshold = LoggerSeverity.Info20;
                loggers.LogInfo20(msg);
            }
            catch (Exception)
            {
                // No permissions to write to the directory, and we don't bother trying anywhere else
            }
#endif
        }
    }

    /// <summary>
    /// Can be used for logging based on a class name, which is used
    /// as the category. Also, delineates a new static run on the first log, in debug mode.
    /// </summary>
    public class SimpleCompositeLogger : CompositeLogger
    {
		private string m_className;
        private string m_prefix;
        private const string InitialLine = "NEW STATIC INITIALIZATION";
        internal static readonly int CATEGORY_LENGTH = 10;

        private static int logcount = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCompositeLogger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="className">Name of the class.</param>
        public SimpleCompositeLogger(Logger log, string className)
            : base(log)
		{
            ClearLogFilters();
            AddLogFilter(new SevereLogFilter());

            m_className = className;
            if (!string.IsNullOrEmpty(className))
            {
                // First, we create a friendly name for the logger which we
                // will use as the "category"
                int lastPeriod = m_className.LastIndexOf('.');
                if (lastPeriod != -1)
                {
                    m_prefix = m_className.Substring(lastPeriod + 1);
                }
                else
                {
                    m_prefix = m_className;
                }

                // Pad the prefix to ten characters
                if (m_prefix.Length > CATEGORY_LENGTH)
                {
                    m_prefix = m_prefix.Substring(0, CATEGORY_LENGTH);
                }
                else if (m_prefix.Length < CATEGORY_LENGTH)
                {
                    m_prefix = string.Format("{0,-" + CATEGORY_LENGTH + "}", m_prefix);
                }
            }
		}

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void Log(LoggerSeverity severity, object entry, params object[] formatParameters)
        {
            if (logcount++ == 1)
            {
                base.Log(LoggerSeverity.Infinity, null);
                base.Log(LoggerSeverity.Infinity, InitialLine);
            }

            Category = m_prefix;
            if (severity == LoggerSeverity.Fatal50)
            {
                // TODO call a notification interface, which could do something like
                // send an email
            }
            base.Log(severity, entry, formatParameters);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SimpleLogFormatter : DefaultLogFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleLogFormatter"/> class.
        /// </summary>
        public SimpleLogFormatter()
        {
            FormatString = "[{0} {3,5} {1,-7}{4}] {2}";
        }

        /// <summary>
        /// Does the format entry.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="logEntry">The log entry.</param>
        /// <param name="category">The category.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected override string DoFormatEntry(LoggerSeverity severity, DateTime timestamp, string logEntry, string category, Dictionary<string, object> data)
        {
            if (!string.IsNullOrEmpty(category))
            {
                category = " " + category;
            }
            return string.Format(FormatString, timestamp.ToString("s"), severity, logEntry, Thread.CurrentThread.ManagedThreadId, category);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NullLogger : SimpleCompositeLogger
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly NullLogger Current = new NullLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="NullLogger"/> class.
        /// </summary>
        public NullLogger()
			: base(null, null)
		{
            Threshold = LoggerSeverity.None0;
		}

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void Log(LoggerSeverity severity, object entry, params object[] formatParameters)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void LogDebug10(object entry, params object[] formatParameters)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void LogError40(object entry, params object[] formatParameters)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void LogFatal50(object entry, params object[] formatParameters)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void LogInfo20(object entry, params object[] formatParameters)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void LogWarn30(object entry, params object[] formatParameters)
        {
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        public override void Start(params object[] args)
        {
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        public override void End(params object[] args)
        {
        }

        /// <summary>
        /// Logs the entry exit.
        /// </summary>
        /// <param name="isEntry">if set to <c>true</c> [is entry].</param>
        /// <param name="useMarker">if set to <c>true</c> [use marker].</param>
        /// <param name="args">The args.</param>
        protected override void LogEntryExit(bool isEntry, bool useMarker, object[] args)
        {
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public override void LogException(Exception ex)
        {
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="severity">The severity.</param>
        public override void LogException(Exception ex, LoggerSeverity severity)
        {
        }

        /// <summary>
        /// Prints method information and the arguments passed.
        /// This code is only compiled in with DEBUG set as
        /// the configuration mode.
        /// </summary>
        /// <param name="args"></param>
        public override void WhereAmI(params object[] args)
        {
        }

        /// <summary>
        /// High level final log that is called with all of the detailed information
        /// and the final log line as the last parameter.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
        }

        /// <summary>
        /// Does the log.
        /// </summary>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(string logLine)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LoggingConfig
    {
        private Dictionary<string, Logger> m_loggers = new Dictionary<string, Logger>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public delegate Logger CallbackCreateLogger(string className, LoggerSeverity threshold);

        /// <summary>
        /// 
        /// </summary>
        public const string AllLoggersDesignator = "all";

        /// <summary>
        /// 
        /// </summary>
        public static bool Enabled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingConfig"/> class.
        /// </summary>
        public LoggingConfig()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingConfig"/> class.
        /// </summary>
        /// <param name="configString">The config string.</param>
        /// <param name="createLogger">The create logger.</param>
        public LoggingConfig(string configString, CallbackCreateLogger createLogger)
        {
            Load(configString, createLogger);
        }

        /// <summary>
        /// Loads the specified config string.
        /// </summary>
        /// <param name="configString">The config string.</param>
        /// <param name="createLogger">The create logger.</param>
        public void Load(string configString, CallbackCreateLogger createLogger)
        {
            if (configString != null && Enabled)
            {
                string[] pieces = configString.Trim().Split(';');
                if (pieces.Length == 1 && pieces[0] == "")
                {
                    return;
                }
                foreach (string piece in pieces)
                {
                    string[] parts = piece.Trim().Split('=');
                    if (parts != null && parts.Length > 0 && parts.Length <= 2 && parts[0].Trim().Length > 0)
                    {
                        string key = parts[0].ToLower().Trim();

                        if (key == "*")
                        {
                            key = AllLoggersDesignator;
                        }

                        string val = parts.Length == 1 ? "*" : parts[1];
                        LoggerSeverity threshold = GetLogValue(val);
                        if (threshold == LoggerSeverity.Infinity)
                        {
                            m_loggers[key] = NullLogger.Current;
                        }
                        else
                        {
                            m_loggers[key] = createLogger(parts[0].Trim(), threshold);
                        }
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Check format of log string ({0}).", piece));
                    }
                }
            }
        }

        private static LoggerSeverity GetDefaultLogThreshold()
        {
            return LoggerSeverity.None0;
        }

        /// <summary>
        /// Gets the log value.
        /// </summary>
        /// <param name="logValue">The log value.</param>
        /// <returns></returns>
        private LoggerSeverity GetLogValue(string logValue)
        {
            if (logValue == null)
            {
                throw new ArgumentNullException("logValue");
            }
            string val = logValue.ToLower().Trim();

            if (val == "" || val == "*" || val == "on" || val == "1" || val == AllLoggersDesignator)
            {
                return GetDefaultLogThreshold();
            }
            else if (val == "off" || val == "0")
            {
                return GetOffValue();
            }
            string[] names = Enum.GetNames(typeof(LoggerSeverity));
            foreach (string name in names)
            {
                if (name.ToLower().StartsWith(val))
                {
                    return (LoggerSeverity)Enum.Parse(typeof(LoggerSeverity), name);
                }
            }

            // If we can't figure out the value, throw an exception
            throw new ArgumentException(string.Format("Unknown logging value {0}", logValue));
        }

        private LoggerSeverity GetOffValue()
        {
            return LoggerSeverity.Infinity;
        }

        /// <summary>
        /// Gets the <see cref="PublicDomain.Logging.Logger"/> with the specified log class.
        /// </summary>
        /// <param name="logClasses">The log classes.</param>
        /// <returns></returns>
        /// <value></value>
        public Logger CreateLogger(params string[] logClasses)
        {
            Logger result = NullLogger.Current;

            Logger test;
            string testClass;

            foreach (string logClass in logClasses)
            {
                testClass = logClass.ToLower().Trim();
                if (m_loggers.TryGetValue(testClass, out test))
                {
                    result = test;
                }
            }

            if (object.ReferenceEquals(result, NullLogger.Current) && m_loggers.TryGetValue(AllLoggersDesignator, out test))
            {
                result = test;
            }

            return result;
        }

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="otherLogClasses">The other log classes.</param>
        /// <returns></returns>
        public Logger CreateLogger(Type type, params string[] otherLogClasses)
        {
            string[] classes = new string[otherLogClasses.Length + 1];
            Array.Copy(otherLogClasses, classes, otherLogClasses.Length);
            classes[classes.Length - 1] = type.ToString();
            return CreateLogger(classes);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IRollOverStrategy
    {
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="logLine">The log line.</param>
        /// <returns></returns>
        string GetFileName(string fileName, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine);
    }

    /// <summary>
    /// Writes to a file, rolling over to a new version of a file
    /// when the previous file has filled to capacity.
    /// </summary>
    public class RollingFileLogger : FileLogger
    {
        private IRollOverStrategy m_strategy;

        /// <summary>
        /// Writes to a file, rolling over to a new version of a file
        /// when the previous file has filled to capacity.
        /// Initializes a new instance of the <see cref="RollingFileLogger"/> class.
        /// </summary>
        /// <param name="fileNameFormatted">The file name formatted.</param>
        public RollingFileLogger(string fileNameFormatted)
            : this(fileNameFormatted, new FileSizeRollOverStrategy())
        {
        }

        /// <summary>
        /// Writes to a file, rolling over to a new version of a file
        /// when the previous file has filled to capacity.
        /// Initializes a new instance of the <see cref="RollingFileLogger"/> class.
        /// </summary>
        /// <param name="fileNameFormatted">The file name formatted.</param>
        /// <param name="strategy">The strategy.</param>
        public RollingFileLogger(string fileNameFormatted, IRollOverStrategy strategy)
            : base(fileNameFormatted)
        {
            Strategy = strategy;
        }

        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>The strategy.</value>
        public IRollOverStrategy Strategy
        {
            get
            {
                return m_strategy;
            }
            set
            {
                m_strategy = value;
            }
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="timestamp"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        /// <param name="logLine"></param>
        /// <returns></returns>
        protected override string GetFileName(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
            string fileName = base.GetFileName(severity, timestamp, entry, formatParameters, logLine);

            if (Strategy != null)
            {
                fileName = Strategy.GetFileName(fileName, severity, timestamp, entry, formatParameters, logLine);
            }

            return fileName;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileSizeRollOverStrategy : IRollOverStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        static FileSizeRollOverStrategy()
        {
            DefaultFileSizeStrategyBytes = GlobalConstants.BytesInAMegabyte * 10;
        }

        /// <summary>
        /// 10 megs
        /// </summary>
        public static readonly int DefaultFileSizeStrategyBytes;

        private long m_maxFileSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSizeRollOverStrategy"/> class.
        /// </summary>
        public FileSizeRollOverStrategy()
            : this (DefaultFileSizeStrategyBytes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSizeRollOverStrategy"/> class.
        /// </summary>
        /// <param name="fileSizeBytes">The file size bytes.</param>
        public FileSizeRollOverStrategy(long fileSizeBytes)
        {
            m_maxFileSize = fileSizeBytes;
        }

        /// <summary>
        /// Gets or sets the max size of the log file after which
        /// a new log file is started.
        /// </summary>
        /// <value>The size of the max file.</value>
        public long MaxFileSize
        {
            get
            {
                return m_maxFileSize;
            }
            set
            {
                m_maxFileSize = value;
            }
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <param name="logLine">The log line.</param>
        /// <returns></returns>
        public string GetFileName(string fileName, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string logLine)
        {
            // First, find the largest numbered file
            string[] pieces = FileSystemUtilities.SplitFileIntoDirectoryAndName(fileName, true);
            string search = pieces[1].Replace("{0}", "*");

            string[] files = Directory.GetFiles(pieces[0], search);

            int maxNumber = 0;

            // Now, build the list of numbers from the file names
            if (files != null)
            {
                foreach (string foundFile in files)
                {
                    int foundNumber = StringUtilities.ExtractFirstNumber(foundFile);
                    if (foundNumber > maxNumber)
                    {
                        maxNumber = foundNumber;
                    }
                }
            }

            if (maxNumber == 0)
            {
                fileName = string.Format(fileName, maxNumber + 1);
            }
            else
            {
                string checkFileName = string.Format(fileName, maxNumber);

                // Check if this file is too big or not
                if (new FileInfo(checkFileName).Length >= MaxFileSize)
                {
                    // Increment the file number
                    fileName = string.Format(fileName, maxNumber + 1);
                }
                else
                {
                    fileName = checkFileName;
                }
            }

            return fileName;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TextWriterLogger : Logger
    {
        private TextWriter m_writer;

        /// <summary>
        /// Gets or sets the writer.
        /// </summary>
        /// <value>The writer.</value>
        public virtual TextWriter Writer
        {
            get
            {
                return m_writer;
            }
            set
            {
                m_writer = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWriterLogger"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public TextWriterLogger(TextWriter writer)
        {
            m_writer = writer;
        }

        /// <summary>
        /// Does the log.
        /// </summary>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(string logLine)
        {
            m_writer.WriteLine(logLine);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConsoleLogger : TextWriterLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
        /// </summary>
        public ConsoleLogger()
            : base(Console.Out)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class LogFormatter : ILogFormatter
    {
        private string m_formatString;

        /// <summary>
        /// </summary>
        /// <value></value>
        public virtual string FormatString
        {
            get
            {
                return m_formatString;
            }
            set
            {
                m_formatString = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="timestamp"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        /// <param name="category"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string FormatEntry(LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters, string category, Dictionary<string, object> data)
        {
            string logEntry = PrepareEntry(entry, formatParameters);

            return DoFormatEntry(severity, timestamp, logEntry, category, data);
        }

        /// <summary>
        /// Does the format entry.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="logEntry">The log entry.</param>
        /// <param name="category">The category.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected abstract string DoFormatEntry(LoggerSeverity severity, DateTime timestamp, string logEntry, string category, Dictionary<string, object> data);

        /// <summary>
        /// Prepares the entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <returns></returns>
        protected virtual string PrepareEntry(object entry, object[] formatParameters)
        {
            if (entry == null) return null;

            if (formatParameters != null && formatParameters.Length > 0)
            {
                entry = string.Format(entry.ToString(), formatParameters);
            }

            return entry.ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DefaultLogFormatter : LogFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLogFormatter"/> class.
        /// </summary>
        public DefaultLogFormatter()
        {
            FormatString = "[{0} {1,-7}{3}] {2}";
        }

        /// <summary>
        /// Does the format entry.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="logEntry">The log entry.</param>
        /// <param name="category">The category.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected override string DoFormatEntry(LoggerSeverity severity, DateTime timestamp, string logEntry, string category, Dictionary<string, object> data)
        {
            if (!string.IsNullOrEmpty(category))
            {
                category = " " + category;
            }
            return string.Format(FormatString, timestamp, severity, logEntry, category);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DefaultLogFilter : ILogFilter
    {
        /// <summary>
        /// Determines whether the specified severity is loggable.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <returns>
        /// 	<c>true</c> if the specified severity is loggable; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsLoggable(LoggerSeverity threshold, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters)
        {
            return (int)severity >= (int)threshold;
        }
    }

    /// <summary>
    /// Always logs severe events, otherwise defers to normal threshold
    /// conditions.
    /// </summary>
    public class SevereLogFilter : DefaultLogFilter
    {
        /// <summary>
        /// Always logs severe events, otherwise defers to normal threshold
        /// conditions. Initializes a new instance of the <see cref="SevereLogFilter"/> class.
        /// </summary>
        public SevereLogFilter()
            : base()
        {
        }

        /// <summary>
        /// Determines whether the specified severity is loggable.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="formatParameters">The format parameters.</param>
        /// <returns>
        /// 	<c>true</c> if the specified severity is loggable; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsLoggable(LoggerSeverity threshold, LoggerSeverity severity, DateTime timestamp, object entry, object[] formatParameters)
        {
            return severity == LoggerSeverity.Fatal50 ? true : base.IsLoggable(threshold, severity, timestamp, entry, formatParameters);
        }
    }

    /// <summary>
    /// By default does not have any filters, and supposes that the composed logs will filter.
    /// </summary>
    public class CompositeLogger : Logger
    {
        private List<Logger> m_loggers = new List<Logger>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeLogger"/> class.
        /// </summary>
        /// <param name="loggers">The loggers.</param>
        public CompositeLogger(params Logger[] loggers)
        {
            foreach (Logger logger in loggers)
            {
                if (logger != null)
                {
                    Loggers.Add(logger);
                }
            }
        }

        /// <summary>
        /// Gets the loggers.
        /// </summary>
        /// <value>The loggers.</value>
        public virtual List<Logger> Loggers
        {
            get
            {
                return m_loggers;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="entry"></param>
        /// <param name="formatParameters"></param>
        public override void Log(LoggerSeverity severity, object entry, params object[] formatParameters)
        {
            foreach (Logger logger in Loggers)
            {
                logger.Log(severity, entry, formatParameters);
            }
        }

        /// <summary>
        /// Does the log.
        /// </summary>
        /// <param name="logLine">The log line.</param>
        protected override void DoLog(string logLine)
        {
            // This is not called
        }

        /// <summary>
        /// The severity threshold at which point a log message
        /// is logged. For example, if the threshold is Debug,
        /// all messages with severity greater than or equal to Debug
        /// will be logged. All other messages will be discarded.
        /// The default threshold is Warn.
        /// </summary>
        /// <value></value>
        public override LoggerSeverity Threshold
        {
            get
            {
                return base.Threshold;
            }
            set
            {
                base.Threshold = value;
                foreach (Logger logger in Loggers)
                {
                    logger.Threshold = value;
                }
            }
        }
    }
}
#endif

namespace PublicDomain.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationValues
    {
        private Dictionary<string, string> m_values = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValues"/> class.
        /// </summary>
        public ConfigurationValues()
        {
        }

        /// <summary>
        /// Reads the parameters from assembly.
        /// </summary>
        /// <param name="assemblyStreamName">Name of the assembly stream.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public ConfigurationValues(string assemblyStreamName, bool intersectAlternateConfig)
            : this(assemblyStreamName, Assembly.GetExecutingAssembly(), intersectAlternateConfig)
        {
        }

        /// <summary>
        /// Reads the parameters from assembly.
        /// </summary>
        /// <param name="assemblyStreamName">Name of the assembly stream.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public ConfigurationValues(string assemblyStreamName, Assembly assembly, bool intersectAlternateConfig)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            Stream stream = assembly.GetManifestResourceStream(assemblyStreamName);
            if (stream == null)
            {
                throw new ArgumentNullException(string.Format("Could not find embedded resource named {0} in assembly {1}.", assemblyStreamName, assembly));
            }
            ReadParametersFromStream(stream, intersectAlternateConfig);
        }

        /// <summary>
        /// Reads the parameters from file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public void ReadParametersFromStream(string fileName, bool intersectAlternateConfig)
        {
            using (TextReader textReader = new StreamReader(fileName))
            {
                ReadParametersFromTextReader(textReader, intersectAlternateConfig);
            }
        }

        /// <summary>
        /// Reads the parameters from stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public void ReadParametersFromStream(Stream stream, bool intersectAlternateConfig)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            using (XmlReader reader = XmlReader.Create(stream))
            {
                ReadParameters(reader, intersectAlternateConfig);
            }
        }

        /// <summary>
        /// Reads the parameters from text reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="intersectAlternateConfig">if set to <c>true</c> [intersect alternate config].</param>
        /// <returns></returns>
        public void ReadParametersFromTextReader(TextReader reader, bool intersectAlternateConfig)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            using (XmlReader xmlReader = XmlReader.Create(reader))
            {
                ReadParameters(xmlReader, intersectAlternateConfig);
            }
        }

        private void ReadParameters(XmlReader reader, bool intersectAlternateConfig)
        {
            ReadParamsReader(reader);

            if (intersectAlternateConfig)
            {
                string alternateConfigFile;
                if (TryGetString("externalconfig", out alternateConfigFile))
                {
                    if (File.Exists(alternateConfigFile))
                    {
                        ConfigurationValues fileValues = new ConfigurationValues();
                        fileValues.ReadParametersFromStream(alternateConfigFile, false);
                        IntersectValues(fileValues);
                    }
                }
            }
        }

        private void ReadParamsReader(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.LocalName.ToLower() == "param")
                {
                    string name = reader.GetAttribute("name");
                    string val = reader.GetAttribute("value");
                    if (!string.IsNullOrEmpty(name))
                    {
                        m_values[name.ToLower()] = val;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the value with the specified key.
        /// </summary>
        /// <value></value>
        public string this[string key]
        {
            get
            {
                string result;
                if (!m_values.TryGetValue(key.ToLower(), out result))
                {
                    // Next, go to the machine config
                    Configuration machineConfig = ConfigurationManager.OpenMachineConfiguration();
                    if (machineConfig.AppSettings != null)
                    {
                        KeyValueConfigurationElement k = machineConfig.AppSettings.Settings[key];
                        if (k != null)
                        {
                            result = k.Value;
                            this[key] = result;
                        }
                    }
                }
                return result;
            }
            set
            {
                m_values[key.ToLower()] = value;
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<string> Keys
        {
            get
            {
                return m_values.Keys;
            }
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetString(string key)
        {
            return GetString(key, null);
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : val;
        }

        /// <summary>
        /// Gets the long.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public long GetLong(string key)
        {
            return GetLong(key, 0);
        }

        /// <summary>
        /// Gets the long.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public long GetLong(string key, long defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : long.Parse(val);
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public int GetInt(string key, int defaultValue)
        {
            string val = this[key];
            return val == null ? defaultValue : int.Parse(val);
        }

        /// <summary>
        /// Tries the get string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public bool TryGetString(string key, out string val)
        {
            val = null;
            string config = this[key];
            if (config != null)
            {
                val = config;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Intersects the values.
        /// </summary>
        /// <param name="intersectValues">The intersect values.</param>
        public void IntersectValues(ConfigurationValues intersectValues)
        {
            foreach (string key in intersectValues.Keys)
            {
                this[key] = intersectValues[key];
            }
        }

        private bool m_wasExternalConfigRead;

        /// <summary>
        /// Gets or sets a value indicating whether [was external config read].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [was external config read]; otherwise, <c>false</c>.
        /// </value>
        public bool WasExternalConfigRead
        {
            get
            {
                return m_wasExternalConfigRead;
            }
            set
            {
                m_wasExternalConfigRead = value;
            }
        }
    }
}

#if !(NODYNACODE)
namespace PublicDomain.Dynacode
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICodeRunner
    {
        /// <summary>
        /// Runs the specified arguments.
        /// </summary>
        /// <param name="compilerResults">The compiler results.</param>
        /// <param name="execMethod">The exec method.</param>
        /// <param name="output">The output.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        object Run(CompilerResults compilerResults, string execMethod, StringBuilder output, params string[] arguments);

        /// <summary>
        /// Runs to string.
        /// </summary>
        /// <param name="compilerResults">The compiler results.</param>
        /// <param name="execMethod">The exec method.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        string RunToString(CompilerResults compilerResults, string execMethod, params string[] arguments);

        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        Language Language { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DotNetCodeRunner : ICodeRunner
    {
        /// <summary>
        /// 
        /// </summary>
        protected Language m_language;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCodeRunner"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        public DotNetCodeRunner(Language language)
        {
            m_language = language;
        }

        /// <summary>
        /// Runs the specified arguments.
        /// </summary>
        /// <param name="compilerResults">The compiler results.</param>
        /// <param name="execMethod">The exec method.</param>
        /// <param name="output">The output.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public virtual object Run(CompilerResults compilerResults, string execMethod, StringBuilder output, params string[] arguments)
        {
            // Find the method in the assembly
            using (new ConsoleRerouter(output))
            {
                return ReflectionUtilities.InvokeMethod(compilerResults.CompiledAssembly, execMethod, new object[] { arguments });
            }
        }

        /// <summary>
        /// Runs to string.
        /// </summary>
        /// <param name="compilerResults">The compiler results.</param>
        /// <param name="execMethod">The exec method.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public virtual string RunToString(CompilerResults compilerResults, string execMethod, params string[] arguments)
        {
            StringBuilder sb = new StringBuilder();
            Run(compilerResults, execMethod, sb, arguments);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        public virtual Language Language
        {
            get
            {
                return m_language;
            }
        }
    }

    /// <summary>
    /// Methods for working with code and languages.
    /// </summary>
    public static class CodeUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultNamespace = "DefaultNamespace";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultClassName = "DefaultClassName";

        private static Language[] s_supportedLanguages;

        static CodeUtilities()
        {
            List<Language> supportedLanguages = new List<Language>();
            supportedLanguages.Add(Language.CSharp);
            supportedLanguages.Add(Language.VisualBasic);
            s_supportedLanguages = supportedLanguages.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StripNonIdentifierCharacters(Language lang, string str)
        {
            switch (lang)
            {
                case Language.CSharp:
                case Language.JSharp:
                    // http://www.ecma-international.org/publications/standards/Ecma-334.htm
                    // Page 70, Printed Page 92

                    // TODO The following is not complete
                    // TODO JSharp should its own version of this -- it is different

                    str = StringUtilities.RemoveCharactersInverse(str, '_', 'a', '-', 'z', 'A', '-', 'Z', '0', '-', '9');
                    break;
                case Language.VisualBasic:
                    // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/vbls7/html/vblrfVBSpec2_2.asp

                    str = StringUtilities.RemoveCharactersInverse(str, '_', 'a', '-', 'z', 'A', '-', 'Z', '0', '-', '9', '\\', '[', '\\', ']');
                    break;
                default:
                    throw new NotImplementedException();
            }
            return str;
        }

        /// <summary>
        /// Evals the snippet.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="simpleCode">The simple code.</param>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.CompileException" />
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.NativeCompileException" />
        /// <returns></returns>
        public static string EvalSnippet(Language language, string simpleCode)
        {
            return Eval(language, GetSnippetCode(language, simpleCode));
        }

        /// <summary>
        /// Runs a snippet of code.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="code">The code.</param>
        /// <param name="arguments">The arguments.</param>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.CompileException" />
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.NativeCompileException" />
        /// <returns></returns>
        public static string Eval(Language language, string code, params string[] arguments)
        {
            return Eval(language, code, true, arguments);
        }

        /// <summary>
        /// Evals the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="code">The code.</param>
        /// <param name="isSnippet">if set to <c>true</c> [is snippet].</param>
        /// <param name="arguments">The arguments.</param>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.CompileException" />
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.NativeCompileException" />
        /// <returns></returns>
        public static string Eval(Language language, string code, bool isSnippet, params string[] arguments)
        {
            CompilerResults compilerResults = Compile(language, code, isSnippet, true);

            // Now, run the code
            ICodeRunner codeRunner = GetCodeRunner(language);
            return codeRunner.RunToString(compilerResults, string.Format("{0}.{1}.{2}", DefaultNamespace, DefaultClassName, GetDefaultMainMethodName(language)), arguments);
        }

        /// <summary>
        /// Compiles the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="code">The code.</param>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.CompileException" />
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.NativeCompileException" />
        /// <returns></returns>
        public static CompilerResults Compile(Language language, string code)
        {
            return Compile(language, code, true, true);
        }

        /// <summary>
        /// Compiles the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="code">The code.</param>
        /// <param name="isSnippet">if set to <c>true</c> then <paramref name="code"/> will be placed into
        /// templated "application code", such as a static void main.</param>
        /// <param name="throwExceptionOnCompileError">if set to <c>true</c> [throw exception on compile error].</param>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.CompileException" />
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.NativeCompileException" />
        /// <returns></returns>
        public static CompilerResults Compile(Language language, string code, bool isSnippet, bool throwExceptionOnCompileError)
        {
            using (CodeDomProvider domProvider = CodeDomProvider.CreateProvider(language.ToString()))
            {
                CompilerInfo compilerInfo = CodeDomProvider.GetCompilerInfo(language.ToString());
                CompilerParameters compilerParameters = compilerInfo.CreateDefaultCompilerParameters();
                PrepareCompilerParameters(language, compilerParameters);
                if (isSnippet)
                {
                    code = GetApplicationCode(language, code, DefaultClassName, DefaultNamespace);
                }
                CompilerResults results = domProvider.CompileAssemblyFromSource(compilerParameters, code);
                if (throwExceptionOnCompileError)
                {
                    CheckCompilerResultsThrow(results);
                }
                return results;
            }
        }

        /// <summary>
        /// Prepares the compiler parameters.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="compilerParameters">The compiler parameters.</param>
        public static void PrepareCompilerParameters(Language language, CompilerParameters compilerParameters)
        {
            switch (language)
            {
                case Language.CSharp:
                    break;
                case Language.VisualBasic:
                    compilerParameters.ReferencedAssemblies.Add(@"c:\windows\Microsoft.NET\Framework\v2.0.50727\System.dll");
                    compilerParameters.ReferencedAssemblies.Add(@"c:\windows\Microsoft.NET\Framework\v2.0.50727\System.Xml.dll");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the supported languages.
        /// </summary>
        /// <returns></returns>
        public static Language[] GetSupportedLanguages()
        {
            return s_supportedLanguages;
        }

        /// <summary>
        /// Gets the code runner.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static ICodeRunner GetCodeRunner(Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                case Language.VisualBasic:
                case Language.JSharp:
                    return new DotNetCodeRunner(language);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the name of the default main method.
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <returns></returns>
        public static string GetDefaultMainMethodName(Language lang)
        {
            switch (lang)
            {
                case Language.CSharp:
                case Language.VisualBasic:
                    return "Main";
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the application template code.
        /// Parameters:
        /// 0: Code
        /// 1: Class Name
        /// 2: Namespace
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <returns></returns>
        public static string GetApplicationTemplateCode(Language lang)
        {
            switch (lang)
            {
                case Language.CSharp:
                    return @"using System;
using System.Collections.Generic;
using System.Text;

namespace {2}
{{
    public class {1}
    {{
        public static void Main(string[] args)
        {{
            {0}
        }}
    }}
}}
";
                case Language.VisualBasic:
                    return @"Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace {2}
    Module {1}
        Sub Main(ByVal Args() as String)
            {0}
        End Sub
    End Module
End Namespace
";
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the snippet code.
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static string GetSnippetCode(Language lang, string code)
        {
            return string.Format(GetSnippetTemplateCode(lang), code);
        }

        /// <summary>
        /// Gets the snippet template code.
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <returns></returns>
        public static string GetSnippetTemplateCode(Language lang)
        {
            switch (lang)
            {
                case Language.CSharp:
                    return @"Console.Write({0});";
                case Language.VisualBasic:
                    return @"Console.Write({0})";
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the application code.
        /// Parameters:
        /// 0: Code
        /// 1: Class Name
        /// 2: Namespace
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static string GetApplicationCode(Language lang, params string[] args)
        {
            return string.Format(GetApplicationTemplateCode(lang), args);
        }

        /// <summary>
        /// This method throws an Exception if it finds an error in the
        /// <c>results</c>, otherwise it returns without side effect.
        /// </summary>
        /// <param name="results"></param>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.CompileException"/>
        /// <exception cref="PublicDomain.Dynacode.CodeUtilities.NativeCompileException"/>
        public static void CheckCompilerResultsThrow(CompilerResults results)
        {
            if (results.Errors.HasErrors)
            {
                string msg = GetCompilerErrorsAsString(results.Errors);
                if (results.NativeCompilerReturnValue != 0)
                {
                    msg += Environment.NewLine + GetNativeCompilerErrorMessage(results);
                }
                throw new CompileException(msg);
            }
            else if (results.NativeCompilerReturnValue != 0)
            {
                throw new NativeCompileException(GetNativeCompilerErrorMessage(results));
            }
        }

        private static string GetNativeCompilerErrorMessage(CompilerResults results)
        {
            return "Compiler returned exit code " + results.NativeCompilerReturnValue;
        }

        /// <summary>
        /// Gets the compiler errors as string.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static string GetCompilerErrorsAsString(CompilerErrorCollection errors)
        {
            StringBuilder sb = new StringBuilder(errors.Count * 10);
            CompilerError error;
            for (int i = 0; i < errors.Count; i++)
            {
                error = errors[i];
                if (i > 0)
                {
                    sb.Append(Environment.NewLine);
                }
                sb.Append(error.ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the display name of the language.
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <returns></returns>
        public static string GetLanguageDisplayName(Language lang)
        {
            switch (lang)
            {
                case Language.CPlusPlus:
                    return "C++";
                case Language.CSharp:
                    return "C#";
                case Language.Java:
                    return "Java";
                case Language.JScript:
                    return "JScript";
                case Language.JSharp:
                    return "J#";
                case Language.PHP:
                    return "PHP";
                case Language.Ruby:
                    return "Ruby";
                case Language.VisualBasic:
                    return "Visual Basic";
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a <seealso cref="PublicDomain.Language"/> given a string name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static Language GetLanguageByName(string name)
        {
            name = name.ToLower().Trim();
            switch (name)
            {
                case "cplusplus":
                case "c++":
                    return Language.CPlusPlus;
                case "c#":
                case "csharp":
                    return Language.CSharp;
                case "java":
                    return Language.Java;
                case "js":
                case "jscript":
                    return Language.JScript;
                case "vj#":
                case "j#":
                case "jsharp":
                    return Language.JSharp;
                case "php":
                    return Language.PHP;
                case "vb":
                case "visual basic":
                case "visualbasic":
                    return Language.VisualBasic;
                case "ruby":
                    return Language.Ruby;
                default:
                    throw new ArgumentException("Could not find language by name " + name);
            }
        }

        /// <summary>
        /// Thrown when an error is encountered compiling.
        /// </summary>
        [Serializable]
        public class CompileException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CompileException"/> class.
            /// </summary>
            public CompileException() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="CompileException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>

            public CompileException(string message) : base(message) { }
            /// <summary>
            /// Initializes a new instance of the <see cref="CompileException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="inner">The inner.</param>

            public CompileException(string message, Exception inner) : base(message, inner) { }
            /// <summary>
            /// Initializes a new instance of the <see cref="CompileException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>

            protected CompileException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }

        /// <summary>
        /// Thrown when the compiler returns an unexpected value.
        /// </summary>
        [Serializable]
        public class NativeCompileException : CompileException
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="NativeCompileException"/> class.
            /// </summary>
            public NativeCompileException() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="NativeCompileException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            public NativeCompileException(string message) : base(message) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="NativeCompileException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="inner">The inner.</param>
            public NativeCompileException(string message, Exception inner) : base(message, inner) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="NativeCompileException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
            protected NativeCompileException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }
}
#endif

#if !(NOASPNETRUNTIMEHOST)

// This entire namespace is courtesy of Rick Strahl (http://www.west-wind.com/), who
// has marked it as Public Domain (http://www.west-wind.com/wwThreads/default.asp?msgid=20D16295H). Thank's Rick!
namespace PublicDomain.AspRuntimeHost
{
    /// <summary>
    /// 
    /// </summary>
    public class wwAspRuntimeHost : IDisposable
    {
        /// <summary>
        /// Location for the generated HTML output.
        /// </summary>
        public string OutputFile = "d:\\temp\\__preview.htm";

        /// <summary>
        /// Hashtable of parameters that can be added to the Host object
        /// </summary>
        public Hashtable Context = new Hashtable();

        /// <summary>
        /// An optional PostBuffer in binary format.
        /// </summary>
        protected byte[] PostData = null;

        /// <summary>
        /// An optional POST buffer Content Type
        /// </summary>
        protected string PostContentType = "application/x-www-form-urlencoded";

        /// <summary>
        /// Name of the directory that AspRunTimeHost class's parent assembly is located in. This is so the DLL/EXE
        /// can be found. Default is blank which uses the current application directory. 
        /// </summary>
        public string ApplicationBase = "";

        /// <summary>
        /// Location of the web.Config file. Defaults to the Application Base path.
        /// </summary>
        public string ConfigFile = "web.config";

        /// <summary>
        /// Name of the Physical Directory assigned with Start(). Required!
        /// </summary>
        public string PhysicalDirectory = "";

        /// <summary>
        /// Name of the virtual directory assigned to the application with Start.Not used internally, only exposed for
        /// external apps to retrieve. 
        /// </summary>
        public string VirtualPath = "/";

        /// <summary>
        /// A hashtable that contains all the HTPP Headers the server sent in header / value pair
        /// </summary>
        public Hashtable ResponseHeaders = null;

        /// <summary>
        /// Send any Request headers - optional. You can pick up response headers and post them right back.
        /// </summary>
        public Hashtable RequestHeaders = null;

        /// <summary>
        /// the Response status code the server sent. 200 on success, 500 on error, 404 for redirect etc.
        /// </summary>
        public int ResponseStatusCode = 200;

        /// <summary>
        /// A comma delimited list of assemblies that should be automatically
        /// copied to the Web applications' BIN directory to avoid having
        /// to manually copy them there.
        /// 
        /// Assign any assemblies that contain types you might be using 
        /// in your parent application and passing to the ASP.NET application
        /// </summary>
        public string ShadowCopyAssemblies = "";

        /// <summary>
        /// Collection of cookies set by the request.
        /// </summary>
        public Hashtable Cookies = new Hashtable();

        // <summary>
        // The code to be used when writing the Response output
        // </summary>
        //public Encoding ResponseEncoding = Encoding.UTF8;

        /// <summary>
        /// An error message if bError is set. Only works for the ProcessRequest method
        /// </summary>
        public string ErrorMessage = "";

        /// <summary>
        /// 
        /// </summary>
        public bool Error = false;


        /// <summary>
        /// Instance of the Proxy object. Exposed to allow access to the ResponseData object.
        /// </summary>
        public wwAspRuntimeProxy Proxy = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="wwAspRuntimeHost"/> class.
        /// </summary>
        public wwAspRuntimeHost()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="wwAspRuntimeHost"/> class.
        /// </summary>
        /// <param name="physicalDirectory">The physical directory.</param>
        /// <param name="virtualPath">The virtual path.</param>
        public wwAspRuntimeHost(string physicalDirectory, string virtualPath)
        {
            PhysicalDirectory = physicalDirectory;
            VirtualPath = virtualPath;

            Start();
        }

        /// <summary>
        /// Processes a page request against the ASP.Net runtime. 
        /// </summary>
        /// <param name="Page">A page filename relative to the Virtual directory. Use subdir\sub2\test.aspx style syntax for subdirs. (note forward slash!)</param>
        /// <param name="QueryString">Optional - query string in key value pair format. Pass null for non.</param>
        /// <returns>true or false. False returns only if a real failure occurs - most page errors will result in an HTTP error page.</returns>
        public virtual bool ProcessRequest(string Page, string QueryString)
        {
            if (!this.PreProcessing())
                return false;

            bool Result = false;
            try
            {
                Result = this.Proxy.ProcessRequest(Page, QueryString);
            }
            catch (Exception ex)
            {
                this.ResponseStatusCode = 500;
                this.ErrorMessage = ex.Message;
                this.Error = true;
                this.ClearRequestData();
                return false;
            }

            this.PostProcessing();

            return Result;
        }


        /// <summary>
        /// Processes a page request against the ASP.Net runtime and runs the result to a string
        /// </summary>
        /// <param name="Page">A page filename relative to the Virtual directory. Use subdir\sub2\test.aspx style syntax for subdirs. (note forward slash!)</param>
        /// <param name="queryStringKeysAndValues">The query string keys and values.</param>
        /// <returns>
        /// true or false. False returns only if a real failure occurs - most page errors will result in an HTTP error page.
        /// </returns>
        public virtual string ProcessRequestToString(string Page, params string[] queryStringKeysAndValues)
        {
            string queryString = "";

            if (queryStringKeysAndValues != null)
            {
                for (int i = 0; i < queryStringKeysAndValues.Length; i += 2)
                {
                    if (queryString.Length > 0)
                    {
                        queryString += "&";
                    }
                    if (queryStringKeysAndValues[i + 1] != null)
                    {
                        queryString += string.Format("{0}={1}", queryStringKeysAndValues[i], HttpUtility.UrlEncode(queryStringKeysAndValues[i + 1]));
                    }
                    else
                    {
                        queryString += string.Format("{0}", queryStringKeysAndValues[i]);
                    }
                }
            }

            if (!this.PreProcessing())
                return "";

            string Result = "";
            try
            {
                Result = this.Proxy.ProcessRequestToString(Page, queryString);
            }
            catch (Exception ex)
            {
                this.ClearRequestData();
                this.ResponseStatusCode = 500;
                this.ErrorMessage = ex.Message;
                this.Error = true;
                return "";
            }

            //this.PostProcessing();

            return Result;
        }

        /// <summary>
        /// Pre-Processing routine common to the Processing methods
        /// </summary>
        /// <returns></returns>
        private bool PreProcessing()
        {
            this.ErrorMessage = "";
            this.Error = false;

            // Use this to check if host has unloaded from proxy
            try
            {
                string Path = this.Proxy.OutputFile;
            }
            catch (Exception)
            {
                // *** Most likely the runtime unloaded on us
                if (!this.Start())
                    return false;
            }

            try
            {
                // *** Pass Parameter info
                this.Proxy.Context = this.Context;

                if (this.Cookies != null)
                    this.AddCookiesToRequest();

                this.Proxy.OutputFile = this.OutputFile;
                this.Proxy.PostData = this.PostData;
                this.Proxy.PostContentType = this.PostContentType;
                this.Proxy.RequestHeaders = this.RequestHeaders;
            }
            catch (Exception ex)
            {
                this.ClearRequestData();
                this.ResponseStatusCode = 500;
                this.ErrorMessage = ex.Message;
                this.Error = true;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Post-Processing code common to both of the processing routines
        /// </summary>
        private void PostProcessing()
        {
            this.ResponseHeaders = this.Proxy.ResponseHeaders;
            this.ResponseStatusCode = this.Proxy.ResponseStatusCode;

            // *** Pick up the server's Cookies and add to internal Cookie Collection
            if (this.Proxy.Cookies != null)
            {
                foreach (string Key in this.Proxy.Cookies.Keys)
                {
                    if (this.Cookies.ContainsKey(Key))
                        this.Cookies[Key] = this.Proxy.Cookies[Key];
                    else
                        this.Cookies.Add(Key, this.Proxy.Cookies[Key]);
                }
            }

            // Copy the Context
            this.Context = this.Proxy.Context;

            this.ClearRequestData();

        }


        /// <summary>
        /// Resets the host so on the next request we start with a clean slate
        /// </summary>
        private void ClearRequestData()
        {
            this.PostData = null;
            this.PostContentType = "application/x-www-form-urlencoded";
            this.RequestHeaders = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Header"></param>
        /// <param name="value"></param>
        public void AddRequestHeader(string Header, string value)
        {
            if (this.RequestHeaders == null)
                this.RequestHeaders = new Hashtable();

            if (!this.RequestHeaders.Contains(Header))
                this.RequestHeaders.Add(Header, value);
        }


        /// <summary>
        /// Adds all the cookies in the Cookie Collection
        /// </summary>
        protected void AddCookiesToRequest()
        {
            // *** Forward any cookies we've picked up previously
            if (this.Cookies != null)
            {
                string TCookies = "";
                foreach (DictionaryEntry Cookie in Cookies)
                    TCookies += (string)Cookie.Value + "; ";

                if (TCookies != "")
                    this.AddRequestHeader("cookie", TCookies);
            }
        }



        /// <summary>
        /// Starts the ASP.Net runtime hosting by creating a new appdomain and loading the runtime into it.
        /// </summary>
        /// <returns>true or false</returns>
        public bool Start()
        {
            if (this.Proxy == null)
            {
                // *** Make sure ASP.Net registry keys exist 
                // *** if IIS was never registered, required aspnet_isapi.dll 
                // *** cannot be found otherwise
                this.GetInstallPathAndConfigureAspNetIfNeeded();

                if (this.VirtualPath.Length == 0 || this.PhysicalDirectory.Length == 0)
                {
                    this.ErrorMessage = "Virtual or Physical Path not set.";
                    this.Error = true;
                    return false;
                }

                // *** Force any assemblies assemblies to be copied
                this.MakeShadowCopies(this.ShadowCopyAssemblies);

                try
                {
                    this.Proxy = wwAspRuntimeProxy.Start(this.PhysicalDirectory, this.VirtualPath,
                        this.ApplicationBase, this.ConfigFile);

                    this.Proxy.PhysicalDirectory = this.PhysicalDirectory;
                    this.Proxy.VirtualPath = this.VirtualPath;

                }
                catch (Exception ex)
                {
                    this.ErrorMessage = ex.Message;
                    this.Error = true;
                    this.Proxy = null;
                    return false;
                }
                this.Cookies.Clear();
            }


            return true;
        }

        /// <summary>
        /// Stops the ASP.Net runtime unloading the AppDomain
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            if (this.Proxy != null)
            {
                try
                {
                    wwAspRuntimeProxy.Stop(this.Proxy);
                    this.Proxy = null;
                }
                catch (Exception ex)
                {
                    this.ErrorMessage = ex.Message;
                    this.Error = true;
                    return false;
                }
                return true;
            }
            return false;
        }



        /// <summary>
        /// Adds a complete POST buffer to the current request.
        /// </summary>
        /// <param name="PostBuffer">raw POST buffer as byte[]</param>
        /// <param name="ContentType">the content type of the buffer.</param>
        public void AddPostBuffer(byte[] PostBuffer, string ContentType)
        {
            if (ContentType != null)
                this.PostContentType = ContentType;

            this.PostData = PostBuffer;
            return;
        }

        /// <summary>
        /// Adds a complete POST buffer to the current request.
        /// </summary>
        /// <param name="PostBuffer">raw POST buffer as a string</param>
        /// <param name="ContentType">the content type of the buffer.</param>
        public void AddPostBuffer(string PostBuffer, string ContentType)
        {
            this.AddPostBuffer(Encoding.GetEncoding(1252).GetBytes(PostBuffer), ContentType);
        }

        /// <summary>
        /// Adds a complete POST buffer to the current request.
        /// </summary>
        /// <param name="PostBuffer">raw POST buffer as byte[]</param>
        public void AddPostBuffer(string PostBuffer)
        {
            this.AddPostBuffer(PostBuffer, "application/x-www-form-urlencoded");
        }


        /// <summary>
        /// Copies any assemblies marked for ShadowCopying into the BIN directory
        /// of the Web physical director. Copies only 
        /// if the assemblies in the source dir is newer
        /// </summary>
        private void MakeShadowCopies(string ShadowCopyAssemblies)
        {
            if (ShadowCopyAssemblies == null ||
                ShadowCopyAssemblies == string.Empty)
                return;

            string[] Assemblies = ShadowCopyAssemblies.Split(';', ',');
            foreach (string Assembly in Assemblies)
            {
                try
                {
                    string TargetFile = PhysicalDirectory + "bin\\" + Path.GetFileName(Assembly);

                    if (File.Exists(TargetFile))
                    {
                        // *** Compare Timestamps
                        DateTime SourceTime = File.GetLastWriteTime(Assembly);
                        DateTime TargetTime = File.GetLastWriteTime(TargetFile);
                        if (SourceTime == TargetTime)
                            continue;
                    }

                    File.Copy(Assembly, TargetFile, true);
                }
                catch { ;  } // nothing we can do on failure 
            }
        }


        /// <summary>
        /// The ASP.NET Runtime requires certain keys configured in the registry.
        /// This code checks for those keys on startup and if not found sets them up
        /// even if ASP.NET is not installed.
        /// 
        /// Taken from the Cassini Source
        /// </summary>
        /// <returns></returns>
        private string GetInstallPathAndConfigureAspNetIfNeeded()
        {
            // If ASP.NET was never registered on this machine, the registry 
            // needs to be patched up for System.Web.dll to find aspnet_isapi.dll
            //
            // If HKLM\Microsoft\ASP.NET key is missing, this will be added
            //      (adjusted for the correct directory and version number
            // [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\ASP.NET]
            //      "RootVer"="1.0.3514.0"
            // [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\ASP.NET\1.0.3514.0]
            //      "Path"="E:\WINDOWS\Microsoft.NET\Framework\v1.0.3514"
            //      "DllFullPath"="E:\WINDOWS\Microsoft.NET\Framework\v1.0.3514\aspnet_isapi.dll"

            const String aspNetKeyName = @"Software\Microsoft\ASP.NET";

            RegistryKey aspNetKey = null;
            RegistryKey aspNetVersionKey = null;
            RegistryKey frameworkKey = null;

            String installPath = null;

            try
            {
                // get the version corresponding to System.Web.Dll currently loaded
                String aspNetVersion = FileVersionInfo.GetVersionInfo(typeof(HttpRuntime).Module.FullyQualifiedName).FileVersion;
                String aspNetVersionKeyName = aspNetKeyName + "\\" + aspNetVersion;

                // non 1.0 names should have 0 QFE in the registry
                if (!aspNetVersion.StartsWith("1.0."))
                    aspNetVersionKeyName = aspNetVersionKeyName.Substring(0, aspNetVersionKeyName.LastIndexOf('.') + 1) + "0";

                // check if the subkey with version number already exists
                aspNetVersionKey = Registry.LocalMachine.OpenSubKey(aspNetVersionKeyName);

                if (aspNetVersionKey != null)
                {
                    // already created -- just get the path
                    installPath = (String)aspNetVersionKey.GetValue("Path");
                }
                else
                {
                    // open/create the ASP.NET key
                    aspNetKey = Registry.LocalMachine.OpenSubKey(aspNetKeyName);
                    if (aspNetKey == null)
                    {
                        aspNetKey = Registry.LocalMachine.CreateSubKey(aspNetKeyName);
                        // add RootVer if creating
                        aspNetKey.SetValue("RootVer", aspNetVersion);
                    }

                    // version dir name is almost version: "1.0.3514.0" -> "v1.0.3514"
                    String versionDirName = "v" + aspNetVersion.Substring(0, aspNetVersion.LastIndexOf('.'));

                    // install directory from "InstallRoot" under ".NETFramework" key
                    frameworkKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NETFramework");
                    String rootDir = (String)frameworkKey.GetValue("InstallRoot");
                    if (rootDir.EndsWith("\\"))
                        rootDir = rootDir.Substring(0, rootDir.Length - 1);

                    // create the version subkey
                    aspNetVersionKey = Registry.LocalMachine.CreateSubKey(aspNetVersionKeyName);

                    // install path
                    installPath = rootDir + "\\" + versionDirName;

                    // set path and dllfullpath
                    aspNetVersionKey.SetValue("Path", installPath);
                    aspNetVersionKey.SetValue("DllFullPath", installPath + "\\aspnet_isapi.dll");
                }
            }
            catch
            {
            }
            finally
            {
                if (aspNetVersionKey != null)
                    aspNetVersionKey.Close();
                if (aspNetKey != null)
                    aspNetKey.Close();
                if (frameworkKey != null)
                    frameworkKey.Close();
            }

            return installPath;
        }


        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class wwAspRuntimeProxy : MarshalByRefObject
    {
        /// <summary>
        /// Location for the generated HTML output.
        /// </summary>
        public string OutputFile = "d:\\temp\\__preview.htm";

        /// <summary>
        /// Context parameters that can be read back in the page from the Context object.
        /// </summary>
        public Hashtable Context = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        public byte[] PostData = null;

        /// <summary>
        /// 
        /// </summary>
        public string PostContentType = "application/x-www-form-urlencoded";

        /// <summary>
        /// Reference to the AppDomain to allow unloading the hosted application.
        /// </summary>
        public AppDomain AppDomain = null;

        /// <summary>
        /// Name of the Physical Directory assigned with Start(). Not used internally, only exposed for
        /// external apps to retrieve.
        /// </summary>
        public string PhysicalDirectory = "";

        /// <summary>
        /// Name of the virtual directory assigned to the application with Start.Not used internally, only exposed for
        /// external apps to retrieve.
        /// </summary>
        public string VirtualPath = "";

        /// <summary>
        /// An error message if bError is set. Only works for the ProcessRequest method
        /// </summary>
        public string ErrorMessage = "";

        /// <summary>
        /// 
        /// </summary>
        public bool Error = false;

        /// <summary>
        /// The timeout for the ASP.Net runtime after which it is automatically unloaded when idle
        /// to release resources. Note this can't be externally set because the lease is set 
        /// during object construction. All you can do is change this property value here statically
        /// </summary>
        public static int IdleTimeoutMinutes = 15;

        /// <summary>
        /// A hashtable that contains all the HTPP Headers the server sent in header / value pair
        /// </summary>
        public Hashtable ResponseHeaders = null;

        /// <summary>
        /// 
        /// </summary>
        public Hashtable RequestHeaders = null;

        /// <summary>
        /// the Response status code the server sent. 200 on success, 500 on error, 404 for redirect etc.
        /// </summary>
        public int ResponseStatusCode = 200;

        /// <summary>
        /// Collection of cookies set by the request.
        /// </summary>
        public Hashtable Cookies = null;

        /// <summary>
        /// Processes script execution on the specified page.
        /// </summary>
        /// <param name="Page">A page filename relative to the Virtual directory. Use subdir\sub2\test.aspx style syntax for subdirs. (note forward slash!)</param>
        /// <param name="QueryString">Optional - query string in key value pair format. Pass null for non.</param>
        /// <returns>true or false. False returns only if a real failure occurs - most page errors will result in an HTTP error page.</returns>
        public virtual bool ProcessRequest(string Page, string QueryString)
        {
            TextWriter Output;

            try
            {
                // *** Note you have to write the right 'codepage'. If you use the default UTF-8
                // *** everything will be double encoded.
                Output = new StreamWriter(this.OutputFile, false, Encoding.GetEncoding(1252));

                // *** Write the UTF-8 prefix
                Output.Write("﻿");
            }
            catch (Exception ex)
            {
                this.Error = true;
                this.ErrorMessage = ex.Message;
                return false;
            }

            // *** Reset the Response settings
            this.ResponseHeaders = null;
            this.Cookies = null;
            this.ResponseStatusCode = 200;

            wwWorkerRequest Request = new wwWorkerRequest(Page, QueryString, Output);
            if (this.Context != null)
                Request.Context = this.Context;

            Request.PostData = this.PostData;
            Request.PostContentType = this.PostContentType;
            Request.RequestHeaders = this.RequestHeaders;
            Request.PhysicalPath = this.PhysicalDirectory;

            try
            {
                HttpRuntime.ProcessRequest(Request);
            }
            catch (Exception ex)
            {
                Output.Close();
                this.ResponseStatusCode = 500;
                this.ErrorMessage = ex.Message;
                this.Error = true;
                return false;
            }

            Output.Close();

            this.ResponseHeaders = Request.ResponseHeaders;
            this.ResponseStatusCode = Request.ResponseStatusCode;


            // *** Capture the Cookies that were set by the server
            this.Cookies = Request.Cookies;

            if (Request.Context != null)
                this.Context = Request.Context;

            return true;
        }

        /// <summary>
        /// Processes a script and returns the result as a string.
        /// </summary>
        /// <param name="Page">Name of the page to return</param>
        /// <param name="QueryString">Optional query string</param>
        /// <returns>script result or null on failure. Script errors are returned as errors in the script result string.</returns>
        public virtual string ProcessRequestToString(string Page, string QueryString)
        {
            StringWriter sw = new StringWriter();
            TextWriter Writer = new System.Web.UI.HtmlTextWriter(sw);

            // *** Reset the Response settings
            this.ResponseHeaders = null;
            this.Cookies = null;
            this.ResponseStatusCode = 200;

            wwWorkerRequest Request = new wwWorkerRequest(Page, QueryString, Writer);
            if (this.Context != null)
                Request.Context = this.Context;

            Request.PostData = this.PostData;
            Request.PostContentType = this.PostContentType;
            Request.RequestHeaders = this.RequestHeaders;
            Request.PhysicalPath = this.PhysicalDirectory;

            try
            {
                HttpRuntime.ProcessRequest(Request);
            }
            catch (Exception ex)
            {
                this.ResponseStatusCode = Request.ResponseStatusCode;
                this.ErrorMessage = ex.Message;
                this.Error = true;
                return null;
            }

            string Result = sw.ToString();
            Writer.Close();

            this.ResponseHeaders = Request.ResponseHeaders;
            this.ResponseStatusCode = Request.ResponseStatusCode;

            this.Cookies = Request.Cookies;
            this.Context = Request.Context;

            return Result;
        }

        /// <summary>
        /// Creates an instance of this class in the ASP.NET AppDomain
        /// </summary>
        /// <param name="hostType">Type of the application to be hosted. Essentially this class.</param>
        /// <param name="virtualDir">Name of the Virtual Directory that hosts this application. Not really used, other than on error messages and ASP Server Variable return values.</param>
        /// <param name="physicalDir">The physical location of the Virtual Directory for the application</param>
        /// <param name="PrivateBinPath">The private bin path.</param>
        /// <param name="ConfigurationFile">Location of the configuration file. Default to web.config in the bin directory.</param>
        /// <returns>
        /// object instance to the wwAspRuntimeProxy class you can call ProcessRequest on. Note this instance returned
        /// is a remoting proxy
        /// </returns>
        public static wwAspRuntimeProxy CreateApplicationHost(Type hostType, string virtualDir, string physicalDir,
                                                               string PrivateBinPath, string ConfigurationFile)
        {
            if (!(physicalDir.EndsWith("\\")))
                physicalDir = physicalDir + "\\";

            // *** Copy this hosting DLL into the /bin directory of the application
            string FileName = Assembly.GetExecutingAssembly().Location;
            try
            {
                if (!Directory.Exists(physicalDir + "bin\\"))
                    Directory.CreateDirectory(physicalDir + "bin\\");

                string JustFname = Path.GetFileName(FileName);
                File.Copy(FileName, physicalDir + "bin\\" + JustFname, true);
            }
            catch { ;}

            wwAspRuntimeProxy Proxy = ApplicationHost.CreateApplicationHost(
                                                                hostType,
                                                                virtualDir,
                                                                physicalDir)
                                                       as wwAspRuntimeProxy;

            if (Proxy != null)
                // *** Grab the AppDomain reference and add the ApplicationBase
                // *** Must call into the Proxy to do this
                Proxy.CaptureAppDomain();


            return Proxy;
        }


        /// <summary>
        /// Internal method that captures the Proxy's AppDomain so we can shut
        /// the ASP.NET runtime down externally.
        /// Also serves as an
        /// </summary>
        internal void CaptureAppDomain()
        {
            this.AppDomain = AppDomain.CurrentDomain;
        }


#if false
		/// <summary>
		/// Creates a minimal Application domain to allow the ASP.Net runtime to be hosted.
		/// </summary>
		/// <param name="hostType">Type of the application to be hosted. Essentially this class.</param>
		/// <param name="virtualDir">Name of the Virtual Directory that hosts this application. Not really used, other than on error messages and ASP Server Variable return values.</param>
		/// <param name="physicalDir">The physical location of the Virtual Directory for the application</param>
		/// <param name="ApplicationBase">Location of the 'bin' directory</param>
		/// <param name="ConfigurationFile">Location of the configuration file. Default to web.config in the bin directory.</param>
		/// <returns>object instance to the wwAspRuntimeProxy class you can call ProcessRequest on.</returns>
		public static wwAspRuntimeProxy CreateApplicationHostX(Type hostType, string virtualDir, string physicalDir, 
		                                                      string ApplicationBase, string ConfigurationFile) 
		{
			if (!(physicalDir.EndsWith("\\")))
				physicalDir = physicalDir + "\\";

			string aspDir = HttpRuntime.AspInstallDirectory;
			string domainId = "ASPHOST_" + DateTime.Now.ToString().GetHashCode().ToString("x");
			string appName = (virtualDir + physicalDir).GetHashCode().ToString("x");
			AppDomainSetup setup = new AppDomainSetup();

			//	setup.ApplicationBase =  physicalDir;
			//	setup.PrivateBinPath = Directory.GetCurrentDirectory();
			setup.ApplicationName = appName;

			setup.ConfigurationFile = ConfigurationFile;   //"web.config";  // not necessary execept for debugging

			/// Assign the application base where this class' assembly is hosted
			/// Otherwise the ApplicationBase is inherited from the current process
			if (ApplicationBase != null && ApplicationBase != "")
				setup.ApplicationBase = ApplicationBase;

            AppDomain Domain = AppDomain.CreateDomain(domainId, GetDefaultDomainIdentity(), setup);
			Domain.SetData(".appDomain", "*");
			Domain.SetData(".appPath", physicalDir);
			Domain.SetData(".appVPath", virtualDir);
			Domain.SetData(".domainId", domainId);
			Domain.SetData(".hostingVirtualPath", virtualDir);
			Domain.SetData(".hostingInstallDir", aspDir);

			ObjectHandle oh = Domain.CreateInstance(hostType.Module.Assembly.FullName, hostType.FullName);
			wwAspRuntimeProxy Host = (wwAspRuntimeProxy) oh.Unwrap();
			
			// *** Save virtual and physical path so we can tell where app runs later
			Host.VirtualPath = virtualDir;
			Host.PhysicalDirectory = physicalDir;

			// *** Save Domain so we can unload later
			Host.AppDomain = Domain;

			return Host;
		}

        private static Evidence GetDefaultDomainIdentity()
        {
            Evidence evidence1 = new Evidence();
            bool flag1 = false;
            IEnumerator enumerator1 = AppDomain.CurrentDomain.Evidence.GetHostEnumerator();
            while (enumerator1.MoveNext())
            {
                if (enumerator1.Current is Zone)
                {
                    flag1 = true;
                }
                evidence1.AddHost(enumerator1.Current);
            }
            enumerator1 = AppDomain.CurrentDomain.Evidence.GetAssemblyEnumerator();
            while (enumerator1.MoveNext())
            {
                evidence1.AddAssembly(enumerator1.Current);
            }
            if (!flag1)
            {
                evidence1.AddHost(new Zone(SecurityZone.MyComputer));
            }
            return evidence1;
        }
#endif


        /// <summary>
        /// Starts the Runtime host by creating an AppDomain and loading the runtime into it
        /// </summary>
        /// <param name="PhysicalPath">The physical disk path for the 'Web' directory where files are executed</param>
        /// <param name="VirtualPath">The name of the virtual path. Typically this will be "/" or the root path.</param>
        /// <param name="PrivateBinPath">The private bin path.</param>
        /// <param name="ConfigFile">The config file.</param>
        /// <returns></returns>
        public static wwAspRuntimeProxy Start(string PhysicalPath, string VirtualPath,
                                              string PrivateBinPath, string ConfigFile)
        {
            wwAspRuntimeProxy Host = wwAspRuntimeProxy.CreateApplicationHost(
            typeof(wwAspRuntimeProxy),
            VirtualPath, PhysicalPath, PrivateBinPath, ConfigFile);

            return Host;
        }

        /// <summary>
        /// Unloads the runtime host by unloading the AppDomain. Use this to free memory if you are compiling lots of pages or recycle the host.
        /// </summary>
        /// <param name="Host">The host.</param>
        public static void Stop(wwAspRuntimeProxy Host)
        {
            if (Host != null)
            {
                Host.Context.Clear();
                Host.Context = null;

                Host.UnloadRuntime();

                AppDomain.Unload(Host.AppDomain);
                Host = null;
            }
        }

        /// <summary>
        /// Method used to shut down the ASP.NET AppDomain from within
        /// the AppDomain. 
        /// </summary>
        internal void UnloadRuntime()
        {
            HttpRuntime.UnloadAppDomain();
        }

        /// <summary>
        /// Overrides the default Lease setting to allow the runtime to not
        /// expire after 5 minutes. 
        /// </summary>
        /// <returns></returns>
        public override Object InitializeLifetimeService()
        {
            // return null; // never expire
            ILease lease = (ILease)base.InitializeLifetimeService();

            // *** Set the initial lease which determines how long the remote ref sticks around
            // *** before .Net automatically releases it. Although our code has the logic to
            // *** to automatically restart it's better to keep it loaded
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromMinutes(wwAspRuntimeProxy.IdleTimeoutMinutes);
                lease.RenewOnCallTime = TimeSpan.FromMinutes(wwAspRuntimeProxy.IdleTimeoutMinutes);
                lease.SponsorshipTimeout = TimeSpan.FromMinutes(5);
            }

            return lease;
        }
    }

    /// <summary>
    /// A subclass of SimpleWorkerRequest that allows to push data to the ASP.Net request
    /// via the Context object.
    /// </summary>
    public class wwWorkerRequest : SimpleWorkerRequest
    {

        /// <summary>
        /// Optional parameter data sent to the ASP.Net page. This value is stored into the 
        /// Context object as Context["Content"]. Only a single parameter can be passed,
        /// but you can pass an object that contains additional properties.
        /// Objects passed must be serializable or inherit from MarshalByRefObject.
        /// </summary>
        public object ParameterData = null;

        /// <summary>
        /// Contains a set of parameters
        /// </summary>
        public Hashtable Context = new Hashtable();

        /// <summary>
        /// Returns optional Response data that is retrieved from the Context object
        /// via the Context["ResultContent"] key.
        /// </summary>
        public object ResponseData = null;

        /// <summary>
        /// Optional PostBuffer that allows sending Postable data to the ASPX page.
        /// </summary>
        public byte[] PostData = null;

        /// <summary>
        /// The content type for the POST operation. Defaults to application/x-www-form-urlencoded.
        /// </summary>
        public string PostContentType = "application/x-www-form-urlencoded";


        /// <summary>
        /// Hashtable that contains the server headers as header/value pairs
        /// </summary>
        public Hashtable ResponseHeaders = new Hashtable();

        /// <summary>
        /// Collection that captures all the cookies in the request
        /// </summary>
        public Hashtable Cookies = null;

        /// <summary>
        /// Pass in a set of request headers as Header / Value pairs
        /// </summary>
        public Hashtable RequestHeaders = null;

        /// <summary>
        /// Numeric Server Response Code
        /// </summary>
        public int ResponseStatusCode;

        /// <summary>
        /// The physical path for this application
        /// </summary>
        public string PhysicalPath = "";


        /// <summary>
        /// Internal property used to keep track of the HTTP Context object.
        /// Used to retrieve the Context.Item["ResultContent"] value
        /// </summary>
        private HttpContext CurrentContext = null;


        /// <summary>
        /// Callback to basic constructor
        /// </summary>
        /// <param name="Page">Name of the page to execute in the Web app. Must be in the VRoot defined for the app with the app host.</param>
        /// <param name="QueryString">Optional QueryString. Pass null if no query string data.</param>
        /// <param name="Output">TextWriter object that receives the output from the request.</param>
        public wwWorkerRequest(string Page, string QueryString, TextWriter Output)
            :
            base(Page, QueryString, Output) { }


        /// <summary>
        /// Returns the UNC-translated path to the currently executing server application.
        /// </summary>
        /// <returns>
        /// The physical path of the current application.
        /// </returns>
        public override string GetAppPathTranslated()
        {
            return this.PhysicalPath;
        }

        /// <summary>
        /// Method that is called just before the ASP.Net page gets executed. Allows
        /// setting of the Context object item collection with arbitrary data. Also saves
        /// the Context object so it can be used later to retrieve any result data.
        /// Inbound: Context.Items["Content"] (Parameter data)
        ///          OR: you can add Context items directly by name and pick them up by name
        /// Outbound: Context.Items["ResultContent"]
        /// </summary>
        /// <param name="callback">callback delegate</param>
        /// <param name="extraData">extraData for system purpose</param>
        public override void SetEndOfSendNotification(EndOfSendNotification callback, object extraData)
        {
            base.SetEndOfSendNotification(callback, extraData);

            this.CurrentContext = extraData as HttpContext;

            if (this.ParameterData != null)
            {
                // *** Use 'as' instead of cast to ensure additional calls don't throw exceptions

                if (this.CurrentContext != null)
                    // *** Add any extra data here to the 
                    this.CurrentContext.Items.Add("Content", this.ParameterData);
            }

            // *** Copy inbound context data
            if (this.Context != null)
            {
                foreach (object Item in this.Context.Keys)
                {
                    this.CurrentContext.Items.Add(Item, this.Context[Item]);
                }
            }

        }


        // *** the following three methods are overridden to provide the
        // *** ability to POST information to the Web Server

        /// <summary>
        /// We must send the Verb so the server knows that it's a POST request.
        /// </summary>
        /// <returns></returns>
        public override String GetHttpVerbName()
        {
            if (this.PostData == null)
                return base.GetHttpVerbName();

            return "POST";
        }

        /// <summary>
        /// We must override this method to send the ContentType to the client
        /// when POSTing so that the request is recognized as a POST.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override string GetKnownRequestHeader(int index)
        {
            if (index == HttpWorkerRequest.HeaderContentLength)
            {
                if (this.PostData != null)
                    return this.PostData.Length.ToString();
            }
            else if (index == HttpWorkerRequest.HeaderContentType)
            {
                if (this.PostData != null)
                    return this.PostContentType;
            }
            else
            {
                // *** if we need to pass headers write them out
                if (this.RequestHeaders != null)
                {
                    string header = HttpWorkerRequest.GetKnownRequestHeaderName(index);
                    if (header != null)
                    {
                        header = header.ToLower();
                        if (this.RequestHeaders.Contains(header))
                            return (string)RequestHeaders[header];
                    }
                }
            }

            return ""; //base.GetKnownRequestHeader(index);
        }

        /// <summary>
        /// Return any POST data if provided
        /// </summary>
        /// <returns></returns>
        public override byte[] GetPreloadedEntityBody()
        {
            if (this.PostData != null)
                return this.PostData;

            return base.GetPreloadedEntityBody();
        }

        /// <summary>
        /// Set the internal status code we can pick up
        /// Pick up ResultContent Content variable 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="statusDescription"></param>
        public override void SendStatus(int statusCode, string statusDescription)
        {
            if (this.CurrentContext != null)
            {
                this.ResponseData = this.CurrentContext.Items["ResultContent"];

            }
            // *** Copy outbound Context
            if (this.CurrentContext.Items.Count > 0)
            {
                this.Context.Clear();
                foreach (object Key in this.CurrentContext.Items.Keys)
                {
                    this.Context.Add(Key, this.CurrentContext.Items[Key]);
                }
            }

            this.ResponseStatusCode = statusCode;
            base.SendStatus(statusCode, statusDescription);
        }

        /// <summary>
        /// Retrieve Response Headers and store in ResponseHeaders() collection
        /// so we can simulate them from the browser.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public override void SendKnownResponseHeader(int index, string value)
        {
            string header = HttpWorkerRequest.GetKnownResponseHeaderName(index).ToLower();
            switch (index)
            {
                case HttpWorkerRequest.HeaderSetCookie:
                    {
                        if (this.Cookies == null)
                            this.Cookies = new Hashtable();

                        string CookieName = value.Substring(0, value.IndexOf("=")).ToLower();
                        if (!Cookies.Contains(CookieName))
                            Cookies.Add(CookieName, value);
                        else
                            Cookies[CookieName] = value;

                        break;
                    }
                default:
                    {
                        try
                        {
                            ResponseHeaders.Add(header, value);
                        }
                        catch
                        {
                            string name = header;
                        }
                        break;
                    }
            }

            base.SendKnownResponseHeader(index, value);
        }

        /// <summary>
        /// Store custom headers to ResponseHeaders Hashtable collection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public override void SendUnknownResponseHeader(string name, string value)
        {
            ResponseHeaders.Add(name, value);

            base.SendUnknownResponseHeader(name, value);
        }
    }
}
#endif

#if !(NOCLSCOMPLIANTWARNINGSOFF)
#pragma warning restore 3001
#pragma warning restore 3002
#pragma warning restore 3003
#pragma warning restore 3006
#pragma warning restore 3009
#endif

#endregion // Meat
