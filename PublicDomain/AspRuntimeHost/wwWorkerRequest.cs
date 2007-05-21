using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Hosting;
using System.Collections;
using System.Web;
using System.IO;

namespace PublicDomain.AspRuntimeHost
{
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
