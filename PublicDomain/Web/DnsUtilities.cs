using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.ScreenScraper;

namespace PublicDomain.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class DnsUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        public static List<IIpProvider> IpProviders = new List<IIpProvider>();

        static DnsUtilities()
        {
            IpProviders.Add(new WebIpProvider("http://www.whatismyip.org/"));
            IpProviders.Add(new WebIpProvider("http://www.ip-adress.com/", @"My IP address: ([^<]+)", 1));
            IpProviders.Add(new WebIpProvider("http://www.whatismyip.com/automation/n09230945.asp"));
            IpProviders.Add(new WebIpProvider("http://www.myipaddress.com/show-my-ip-address/", @"<p><b>([^<]+)", 1));

            //IpProviders.Add(new AuthenticatedWebIpProvider(
            //    "$ZONEEDITUSER",
            //    "$ZONEEDITPASSWORD",
            //    "http://dynamic.zoneedit.com/checkip.html",
            //    @"^Current IP Address: (.+)$",
            //    1,
            //    "excessive")
            //);
        }

        /// <summary>
        /// Gets the public ip address.
        /// </summary>
        /// <returns></returns>
        public static string GetPublicIpAddress()
        {
            string result = null;
            if (IpProviders != null)
            {
                foreach (IIpProvider ipProvider in IpProviders)
                {
                    result = ipProvider.GetIpAddress();
                    if (!string.IsNullOrEmpty(result))
                    {
                        break;
                    }
                }
            }
            return result;
        }
    }
}
