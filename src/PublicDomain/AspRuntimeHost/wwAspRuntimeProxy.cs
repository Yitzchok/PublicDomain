using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Web;
using System.Reflection;
using System.Web.Hosting;
using System.Runtime.Remoting.Lifetime;

namespace PublicDomain.AspRuntimeHost
{
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
}
