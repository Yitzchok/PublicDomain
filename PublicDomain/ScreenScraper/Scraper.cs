using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Web;
using System.IO;

namespace PublicDomain.ScreenScraper
{
    /// <summary>
    /// Entry point to scrape an HTML page.
    /// This class is not thread safe.
    /// </summary>
    [Serializable]
    public class Scraper
    {
        /// <summary>
        /// 
        /// </summary>
        public static int DefaultExternalCallTimeout = 12000;

        /// <summary>
        /// 
        /// </summary>
        protected bool m_FollowEquivRefreshes = true;

        private int m_timeout = 100000;

        /// <summary>
        /// The number of milliseconds to wait before the request times out. The default
        /// is 100,000 milliseconds (100 seconds).
        /// </summary>
        /// <value>The timeout.</value>
        public int Timeout
        {
            get
            {
                return m_timeout;
            }
            set
            {
                m_timeout = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [follow equiv refreshes].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [follow equiv refreshes]; otherwise, <c>false</c>.
        /// </value>
        public bool FollowEquivRefreshes
        {
            get
            {
                return m_FollowEquivRefreshes;
            }
            set
            {
                m_FollowEquivRefreshes = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ScrapeSession m_Session;

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        public ScrapeSession Session
        {
            get
            {
                return m_Session;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string m_Referer;

        /// <summary>
        /// Gets or sets the referer.
        /// </summary>
        /// <value>The referer.</value>
        public string Referer
        {
            get
            {
                return m_Referer;
            }
            set
            {
                m_Referer = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Uri _LastProcessResponseUri;

        /// <summary>
        /// Gets or sets the last process response URI.
        /// </summary>
        /// <value>The last process response URI.</value>
        public Uri LastProcessResponseUri
        {
            get
            {
                return _LastProcessResponseUri;
            }
            set
            {
                _LastProcessResponseUri = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string LastMetaFollow;

        /// <summary>
        /// 
        /// </summary>
        private ScrapeType? _MetaRefreshScrapeType;

        /// <summary>
        /// If there is a meta refresh, then this specified
        /// the scrape type to use to follow the link. If this
        /// value is null, then the scrape type of the previous request
        /// is used.
        /// </summary>
        /// <value>The type of the meta refresh scrape.</value>
        public ScrapeType? MetaRefreshScrapeType
        {
            get
            {
                return _MetaRefreshScrapeType;
            }
            set
            {
                _MetaRefreshScrapeType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Domain;

        /// <summary>
        /// This is used for a requests referer attribute as well as setting any cookies.
        /// This should be in the form "www.domain.com," without the prepended scheme.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain
        {
            get
            {
                return _Domain;
            }
            set
            {
                if (value != null && value.Contains("/"))
                {
                    throw new ArgumentException("The domain must not include a scheme (e.g. http), or a trailing slash ('/').");
                }
                _Domain = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scraper"/> class.
        /// </summary>
        public Scraper()
            : this(null)
        {
        }

        /// <summary>
        /// This is used for a requests referer attribute as well as setting any cookies.
        /// This should be in the form "www.domain.com," without the prepended scheme.
        /// </summary>
        /// <param name="domain">The domain.</param>
        public Scraper(string domain)
        {
            this.Domain = domain;
            m_Session = new ScrapeSession(this);
            if (this.Domain != null)
            {
                Referer = "http://" + this.Domain;
            }
        }

        private ICredentials m_credentials;

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        /// <value>The credentials.</value>
        public ICredentials Credentials
        {
            get
            {
                return m_credentials;
            }
            set
            {
                m_credentials = value;
            }
        }

        private bool m_useCredentials = true;

        /// <summary>
        /// Gets or sets a value indicating whether [use credentials].
        /// </summary>
        /// <value><c>true</c> if [use credentials]; otherwise, <c>false</c>.</value>
        public bool UseCredentials
        {
            get
            {
                return m_useCredentials;
            }
            set
            {
                m_useCredentials = value;
            }
        }

        /// <summary>
        /// Sets the network credentials.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public void SetNetworkCredentials(string user, string password)
        {
            SetNetworkCredentials(user, password, null);
        }

        /// <summary>
        /// Sets the network credentials.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <param name="domain">The domain.</param>
        public void SetNetworkCredentials(string user, string password, string domain)
        {
            Credentials = new NetworkCredential(user, password, domain);
        }

        /// <summary>
        /// Scrapes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="keyAndValuePairs">The key and value pairs.</param>
        /// <returns></returns>
        public ScrapedPage Scrape(ScrapeType type, string uri, params string[] keyAndValuePairs)
        {
            NameValueCollection query = GetNameValueCollectionFromParams(keyAndValuePairs);
            return Scrape(type, uri, query);
        }

        private static NameValueCollection GetNameValueCollectionFromParams(string[] keyAndValuePairs)
        {
            NameValueCollection query = new NameValueCollection();
            if (keyAndValuePairs != null)
            {
                for (int i = 0; i < keyAndValuePairs.Length; i += 2)
                {
                    query[keyAndValuePairs[i]] = keyAndValuePairs[i + 1];
                }
            }
            return query;
        }

        /// <summary>
        /// Simples the scrape.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public static ScrapedPage SimpleScrape(ScrapeType type, string uri, NameValueCollection query)
        {
            return SimpleScrape(type, uri, null, null, query);
        }

        /// <summary>
        /// Scrapes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public static ScrapedPage SimpleScrape(ScrapeType type, string uri, string userName, string password, NameValueCollection query)
        {
            Scraper scraper = new Scraper();
            if (!string.IsNullOrEmpty(userName))
            {
                scraper.UseCredentials = true;
                scraper.SetNetworkCredentials(userName, password);
            }
            return scraper.Scrape(type, uri, query);
        }

        /// <summary>
        /// Scrapes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="keyAndValuePairs">The key and value pairs.</param>
        /// <returns></returns>
        public static ScrapedPage SimpleScrape(ScrapeType type, string uri, params string[] keyAndValuePairs)
        {
            NameValueCollection query = GetNameValueCollectionFromParams(keyAndValuePairs);
            return SimpleScrape(type, uri, query);
        }

        /// <summary>
        /// Scrapes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public ScrapedPage Scrape(ScrapeType type, string uri, NameValueCollection query)
        {
            ScrapedPage page = new ScrapedPage();
            string qs = BuildQueryString(query);
            page.QueryParameters = query;
            page.ScrapeType = type;
            switch (type)
            {
                case ScrapeType.GET:
                    uri = uri.Contains("?") ? (uri + "&" + qs) : (uri + "?" + qs);
                    page.RawStream = HttpGet(uri);
                    break;
                case ScrapeType.POST:
                    page.RawStream = HttpPost(uri, qs);
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (page.RawStream == null)
            {
                throw new Exception("No data for " + uri);
            }
            else
            {
                page.Url = new Uri(uri);
                Referer = uri;

                page = PostProcessData(page);
            }
            return page;
        }

        /// <summary>
        /// Requireses the credentials.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public bool RequiresCredentials(ScrapeType type, string uri, NameValueCollection query)
        {
            try
            {
                UseCredentials = false;
                Scrape(type, uri, query);
            }
            catch (System.Net.WebException ex)
            {
                HttpWebResponse response = ex.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return true;
                }
            }
            finally
            {
                UseCredentials = true;
            }

            return false;
        }

        /// <summary>
        /// Requireses the credentials.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="keyAndValuePairs">The key and value pairs.</param>
        /// <returns></returns>
        public bool RequiresCredentials(ScrapeType type, string uri, params string[] keyAndValuePairs)
        {
            try
            {
                UseCredentials = false;
                Scrape(type, uri, keyAndValuePairs);
            }
            catch (System.Net.WebException ex)
            {
                HttpWebResponse response = ex.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return true;
                }
            }
            finally
            {
                UseCredentials = true;
            }

            return false;
        }

        /// <summary>
        /// Posts the process data.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        private ScrapedPage PostProcessData(ScrapedPage page)
        {
            if (FollowEquivRefreshes)
            {
                bool followed = false;
                // See if we can find an http-equiv refresh
                IList<ScreenScraperTag> metaTags = ScrapedPage.ConvertToTagList(page.FindChildlessTags("meta", false), true);

                // Now, we have all META tags. Try to find one with HTTP-EQUIV="refresh"
                ScreenScraperTag refreshTag = null;
                foreach (ScreenScraperTag metaTag in metaTags)
                {
                    string httpEquivValue = metaTag.FindAttributeValue("http-equiv");
                    if (httpEquivValue != null && httpEquivValue.Equals("refresh"))
                    {
                        refreshTag = metaTag;
                        break;
                    }
                }
                if (refreshTag != null)
                {
                    // It's a refresh. Try to figure out the URL we have to go to.
                    string contentValue = refreshTag.FindAttributeValue("content");
                    if (contentValue != null)
                    {
                        // First, split it by semicolon
                        string[] refreshPieces = contentValue.Split(';');
                        string url = null;
                        int time = 0;
                        foreach (string refreshPiece in refreshPieces)
                        {
                            if (refreshPiece.ToLower().Trim().StartsWith("url"))
                            {
                                // found the URL. Just take everything after the =
                                int equalPos = refreshPiece.IndexOf('=');
                                if (equalPos != -1)
                                {
                                    url = refreshPiece.Substring(equalPos + 1).Trim();
                                    break;
                                }
                            }
                            else if (time == 0)
                            {
                                int.TryParse(refreshPiece.Trim(), out time);
                            }
                        }
                        if (time == 0 && url != null)
                        {
                            // We have a refresh url, so we need to update the page

                            // If it is a relative url, make it absolute.
                            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                            {
                                url = LastProcessResponseUri.GetLeftPart(UriPartial.Authority) + "/" + (url.StartsWith("/") ? url.Substring(1) : url);
                            }

                            if (!url.Equals(LastMetaFollow))
                            {
                                page = Scrape(MetaRefreshScrapeType == null ? page.ScrapeType : MetaRefreshScrapeType.Value, url, page.QueryParameters);
                                LastMetaFollow = url;
                                followed = true;
                            }
                            else
                            {
                                throw new Exception("Appears to be a recursive loop of http-equiv redirects to the same page ('" + url + "').");
                            }
                        }
                    }
                }

                if (!followed)
                {
                    LastMetaFollow = null;
                }
            }
            return page;
        }

        /// <summary>
        /// Builds the query string.
        /// </summary>
        /// <param name="keyAndValuePairs">The key and value pairs.</param>
        /// <returns></returns>
        public static string BuildQueryString(params string[] keyAndValuePairs)
        {
            NameValueCollection query = new NameValueCollection();
            if (keyAndValuePairs != null)
            {
                for (int i = 0; i < keyAndValuePairs.Length; i += 2)
                {
                    query[keyAndValuePairs[i]] = keyAndValuePairs[i + 1];
                }
            }
            return BuildQueryString(query);
        }

        /// <summary>
        /// Builds the query string.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public static string BuildQueryString(NameValueCollection query)
        {
            string ret = "";
            foreach (string key in query.AllKeys)
            {
                if (!ret.Equals(string.Empty))
                {
                    ret += "&";
                }
                ret += key + "=" + HttpUtility.UrlEncode(query[key]);
            }
            return ret;
        }

        /// <summary>
        /// HTTPs the get.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public string HttpGet(string uri)
        {
            System.Net.HttpWebRequest req = CreateWebRequest(uri);
            return ProcessResponseStream(req);
        }

        /// <summary>
        /// HTTPs the post.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public string HttpPost(string uri, string parameters)
        {
            System.Net.HttpWebRequest req = CreateWebRequest(uri);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(parameters);
            req.ContentLength = bytes.Length;
            using (System.IO.Stream os = req.GetRequestStream())
            {
                os.Write(bytes, 0, bytes.Length); //Push it out there
            }
            return ProcessResponseStream(req);
        }

        /// <summary>
        /// Processes the response stream.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        private string ProcessResponseStream(System.Net.HttpWebRequest req)
        {
            using (System.Net.WebResponse resp = req.GetResponse())
            {
                if (resp == null) return null;
                // Update the domain we're now on
                Domain = resp.ResponseUri.Authority;
                LastProcessResponseUri = resp.ResponseUri;
                using (Stream stream = resp.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                    {
                        return sr.ReadToEnd().Trim();
                    }
                }
            }
        }

        /// <summary>
        /// Creates the web request.
        /// </summary>
        /// <param name="URI">The URI.</param>
        /// <returns></returns>
        private HttpWebRequest CreateWebRequest(string URI)
        {
            HttpWebRequest req = (HttpWebRequest)System.Net.WebRequest.Create(URI);
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322)";
            if (Domain != null)
            {
                req.Referer = Referer;
            }
            req.CookieContainer = Session.Cookies;
            req.AllowAutoRedirect = true;
            req.Timeout = Timeout;
            if (UseCredentials)
            {
                req.Credentials = Credentials;
            }
            return req;
        }
    }
}
