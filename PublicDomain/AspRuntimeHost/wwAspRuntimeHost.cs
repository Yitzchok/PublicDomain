using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

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
}
