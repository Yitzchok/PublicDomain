using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using System.Globalization;

namespace PublicDomain.ScreenScraper
{
    /// <summary>
    /// Represents a scraped HTML page.
    /// </summary>
    [Serializable]
    public class ScrapedPage
    {
        /// <summary>
        /// 
        /// </summary>
        protected string m_RawStream;

        /// <summary>
        /// Gets or sets the raw stream.
        /// </summary>
        /// <value>The raw stream.</value>
        public string RawStream
        {
            get
            {
                return m_RawStream;
            }
            set
            {
                m_RawStream = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _RawStreamLowercase;

        /// <summary>
        /// Gets the raw stream lowercase.
        /// </summary>
        /// <value>The raw stream lowercase.</value>
        public string RawStreamLowercase
        {
            get
            {
                if (_RawStreamLowercase == null && RawStream != null)
                {
                    _RawStreamLowercase = RawStream.ToLower();
                }
                return _RawStreamLowercase;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Uri m_Url;

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public Uri Url
        {
            get
            {
                return m_Url;
            }
            set
            {
                m_Url = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private NameValueCollection _QueryParameters;

        /// <summary>
        /// Gets or sets the query parameters.
        /// </summary>
        /// <value>The query parameters.</value>
        public NameValueCollection QueryParameters
        {
            get
            {
                return _QueryParameters;
            }
            set
            {
                _QueryParameters = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private ScrapeType _ScrapeType;

        /// <summary>
        /// Gets or sets the type of the scrape.
        /// </summary>
        /// <value>The type of the scrape.</value>
        public ScrapeType ScrapeType
        {
            get
            {
                return _ScrapeType;
            }
            set
            {
                _ScrapeType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Title;

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                if (_Title == null && RawStreamLowercase != null)
                {
                    int titleIndex = RawStreamLowercase.IndexOf("<title");
                    if (titleIndex != -1)
                    {
                        int titleEnd = RawStreamLowercase.IndexOf(">", titleIndex);
                        if (titleEnd != -1)
                        {
                            titleEnd++;
                            int titleEndTag = RawStreamLowercase.IndexOf("<", titleEnd);
                            if (titleEndTag != -1)
                            {
                                _Title = RawStream.Substring(titleEnd, titleEndTag - titleEnd);
                            }
                        }
                    }
                }
                return _Title == null ? "" : _Title;
            }
        }

        /// <summary>
        /// Finds the substring.
        /// </summary>
        /// <param name="pretext">The pretext.</param>
        /// <param name="posttext">The posttext.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        public string FindSubstring(string pretext, string posttext, bool caseSensitive)
        {
            return FindSubstring(GetSubject(ref pretext, ref posttext, null, caseSensitive), pretext, posttext, caseSensitive);
        }

        /// <summary>
        /// Gets the subject.
        /// </summary>
        /// <param name="pretext">The pretext.</param>
        /// <param name="posttext">The posttext.</param>
        /// <param name="contextFind">The context find.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        private string GetSubject(ref string pretext, ref string posttext, string contextFind, bool caseSensitive)
        {
            string subject = RawStream;
            if (!caseSensitive)
            {
                pretext = pretext.ToLower();
                posttext = posttext.ToLower();
                if (contextFind != null)
                {
                    contextFind = contextFind.ToLower();
                }
            }
            if (subject != null && contextFind != null)
            {
                string subjectSearch = subject;
                if (!caseSensitive)
                {
                    subjectSearch = subjectSearch.ToLower();
                }
                int contextFindIndex = subjectSearch.IndexOf(contextFind);
                if (contextFindIndex != -1)
                {
                    return subject.Substring(contextFindIndex + contextFind.Length);
                }
                return null;
            }
            else
            {
                return subject;
            }
        }

        /// <summary>
        /// This searches the content stream for any piece of text that is surrounded
        /// by the prettext and posttext arguments
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="pretext">The pretext.</param>
        /// <param name="posttext">The posttext.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        public string FindSubstring(string subject, string pretext, string posttext, bool caseSensitive)
        {
            if (subject != null)
            {
                string subjectSearch = subject;
                if (!caseSensitive)
                {
                    subjectSearch = subject.ToLower();
                    pretext = pretext.ToLower();
                    posttext = posttext.ToLower();
                }
                // First, try to find the prettext
                int pretextstart = subjectSearch.IndexOf(pretext);
                if (pretextstart != -1)
                {
                    // Now try to find the posttext, that is after the prettext
                    pretextstart += pretext.Length;
                    int posttextstart = subjectSearch.IndexOf(posttext, pretextstart);
                    if (posttextstart != -1)
                    {
                        // We always return the substring from the rawstream
                        return subject.Substring(pretextstart, posttextstart - pretextstart);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the substring by context.
        /// </summary>
        /// <param name="contextFind">The context find.</param>
        /// <param name="prettext">The prettext.</param>
        /// <param name="posttext">The posttext.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        public string FindSubstringByContext(string contextFind, string prettext, string posttext, bool caseSensitive)
        {
            return FindSubstring(GetSubject(ref prettext, ref posttext, contextFind, caseSensitive), prettext, posttext, caseSensitive);
        }

        /// <summary>
        /// Splits the by encapsulating tags.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        public IList<string> SplitByEncapsulatingTags(string subject, string tagName, bool caseSensitive)
        {
            string subjectSearch = subject;
            if (!tagName.Contains("<"))
            {
                tagName = "<" + tagName;
            }
            if (tagName.EndsWith(">"))
            {
                tagName = tagName.Substring(0, tagName.Length - 1);
            }
            if (!caseSensitive)
            {
                subjectSearch = subjectSearch.ToLower();
                tagName = tagName.ToLower();
            }
            string endTag = CreateEndTag(tagName);
            IList<string> ret = new List<string>();

            int searchStart = 0;
            while (searchStart >= 0)
            {
                int startIndex = subjectSearch.IndexOf(tagName, searchStart);
                if (startIndex == -1)
                {
                    break;
                }
                startIndex += tagName.Length;

                // now, try to find the end of the start tag
                startIndex = subjectSearch.IndexOf(">", startIndex);
                if (startIndex == -1)
                {
                    break;
                }
                startIndex++;

                int endIndex = subjectSearch.IndexOf(endTag, startIndex);
                if (endIndex == -1)
                {
                    break;
                }

                ret.Add(subject.Substring(startIndex, endIndex - startIndex));

                searchStart = endIndex + endTag.Length;
            }
            return ret;
        }

        /// <summary>
        /// Creates the end tag.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns></returns>
        private string CreateEndTag(string tagName)
        {
            int ltindex = tagName.IndexOf("<");
            if (ltindex != -1)
            {
                return tagName.Insert(ltindex + 1, "/");
            }
            return null;
        }

        /// <summary>
        /// Converts the link to pair.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        public Pair<string, string> ConvertLinkToPair(string subject)
        {
            return ConvertLinkToPair(subject, true);
        }

        /// <summary>
        /// The first element in the pair is the HREF Link, and the second element
        /// is the text of the link.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="decodeLink">if set to <c>true</c> [decode link].</param>
        /// <returns></returns>
        public Pair<string, string> ConvertLinkToPair(string subject, bool decodeLink)
        {
            Regex re = new Regex(@"<a\s+href\s*=\s*[""]([^""]+)[""](\s+[\w]+\s*=\s*[""]?[^""]+[""]?)*\s*>([^<]+)<\s*/\s*a\s*>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = re.Match(subject);
            if (m.Success)
            {
                string link = m.Groups[1].ToString().Trim();
                if (link.StartsWith("/"))
                {
                    link = Url.GetLeftPart(UriPartial.Authority) + link;
                }
                if (decodeLink)
                {
                    link = HttpUtility.UrlDecode(link);
                }
                return new Pair<string, string>(link, m.Groups[3].ToString());
            }
            return null;
        }

        /// <summary>
        /// Splits the string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="split">The split.</param>
        /// <returns></returns>
        public static IList<string> SplitString(string str, string split)
        {
            IList<string> pieces = new List<string>();
            do
            {
                int splitIndex = str.IndexOf(split);
                if (splitIndex == -1)
                {
                    pieces.Add(str);
                    break;
                }
                else
                {
                    pieces.Add(str.Substring(0, splitIndex));
                    str = str.Substring(splitIndex + split.Length);
                }
            } while (true);
            return pieces;
        }

        /// <summary>
        /// Basically removes extraneous characters like padding, newlines, tabs, etc.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string CanonicalizeString(string str)
        {
            return str == null ? null : str.Trim().Replace("\n", "").Replace("\r", "").Replace("\t", "");
        }

        /// <summary>
        /// Finds the childless tags.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        public IList<string> FindChildlessTags(string tagName, bool caseSensitive)
        {
            IList<string> ret = new List<string>();
            string searchSubject = caseSensitive ? RawStream : RawStreamLowercase;
            if (!caseSensitive)
            {
                tagName = tagName.ToLower();
            }
            if (!tagName.StartsWith("<"))
            {
                tagName = "<" + tagName;
            }
            if (searchSubject != null)
            {
                int startpos = 0;
                while (true)
                {
                    int startIndex = searchSubject.IndexOf(tagName, startpos);
                    if (startIndex == -1)
                    {
                        break;
                    }
                    int endIndex = searchSubject.IndexOf(">", startIndex);
                    if (endIndex == -1)
                    {
                        break;
                    }

                    ret.Add(RawStream.Substring(startIndex, endIndex - startIndex + 1));

                    startpos = endIndex + 1;
                }
            }
            return ret;
        }

        /// <summary>
        /// Matches the specified regex.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <returns></returns>
        public Match Match(string regex)
        {
            return Match(regex, true);
        }

        /// <summary>
        /// Matches the specified regex.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns></returns>
        public Match Match(string regex, bool caseSensitive)
        {
            return new Regex(regex, caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase).Match(RawStream);
        }

        private static Regex tagRegex = new Regex(@"<([\w\-]+)(\s+([\w\-]+)\s*=\s*[""]([^""]*)[""])*\s*/?\s*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex nameValueRegex = new Regex(@"([\w\-]+)\s*=\s*[""]([^""]*)[""]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Converts to tag.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="lowerNames">if set to <c>true</c> [lower names].</param>
        /// <returns></returns>
        public static ScreenScraperTag ConvertToTag(string html, bool lowerNames)
        {
            Match m = tagRegex.Match(html);
            if (m.Success)
            {
                ScreenScraperTag tag = new ScreenScraperTag();
                tag.Name = m.Groups[1].ToString();
                foreach (Capture capture in m.Groups[2].Captures)
                {
                    Match m2 = nameValueRegex.Match(capture.ToString());
                    if (m2.Success)
                    {
                        string id = m2.Groups[1].ToString();
                        if (lowerNames)
                        {
                            id = id.ToLower();
                        }
                        try
                        {
                            tag.Attributes[id] = m2.Groups[2].ToString();
                        }
                        catch (Exception)
                        {
                            // Catch if duplicate attributes are added
                        }
                    }
                }
                return tag;
            }
            return null;
        }

        /// <summary>
        /// Converts to tag list.
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <param name="lowerNames">if set to <c>true</c> [lower names].</param>
        /// <returns></returns>
        public static IList<ScreenScraperTag> ConvertToTagList(IList<string> tags, bool lowerNames)
        {
            IList<ScreenScraperTag> ret = new List<ScreenScraperTag>();
            foreach (string tag in tags)
            {
                ScreenScraperTag t = ConvertToTag(tag, lowerNames);
                if (t != null)
                {
                    ret.Add(t);
                }
            }
            return ret;
        }

        private static Regex CurrencyRegex = new Regex(@"\$([\d\.,]+)", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Finds the currency.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        public static string FindCurrency(string subject)
        {
            Match m = CurrencyRegex.Match(subject);
            if (m.Success)
            {
                return m.Groups[1].ToString();
            }
            return null;
        }

        /// <summary>
        /// Converts the currency string to decimal.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="ret">The ret.</param>
        /// <returns></returns>
        public static bool ConvertCurrencyStringToDecimal(string subject, out decimal ret)
        {
            ret = 0;
            return subject == null ? false : decimal.TryParse(subject, out ret);
        }

        /// <summary>
        /// Converts the currency string to double.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="ret">The ret.</param>
        /// <returns></returns>
        public static bool ConvertCurrencyStringToDouble(string subject, out double ret)
        {
            decimal dec;
            ret = 0;
            bool success = ConvertCurrencyStringToDecimal(subject, out dec);
            if (success)
            {
                ret = (double)dec;
            }
            return success;
        }

        /// <summary>
        /// Converts the string to date time.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="ret">The ret.</param>
        /// <returns></returns>
        public static bool ConvertStringToDateTime(string subject, TzTimeZone timeZone, out TzDateTime ret)
        {
            return TzDateTime.TryParseTz(subject, DateTimeStyles.AssumeUniversal, out ret, timeZone);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return RawStream;
        }
    }
}
