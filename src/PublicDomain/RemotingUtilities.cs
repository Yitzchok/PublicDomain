using System;
using System.Collections.Generic;
using System.Text;

#if MSCOREE
using mscoree;
#endif

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public static class RemotingUtilities
    {
#if MSCOREE
        /// <summary>
        /// Gets all AppDomains in the current process, including the currently
        /// executing AppDomain
        /// </summary>
        /// <returns></returns>
        public static List<AppDomain> GetProcessAppDomains()
        {
            return GetProcessAppDomains(false);
        }

        /// <summary>
        /// Gets all AppDomains in the current process. The current AppDomain
        /// is included if <paramref name="removeCurrentAppDomain"/> is true, otherwise
        /// it is not included.
        /// </summary>
        /// <param name="removeCurrentAppDomain">if set to <c>true</c> [remove current app domain].</param>
        /// <returns></returns>
        public static List<AppDomain> GetProcessAppDomains(bool removeCurrentAppDomain)
        {
            List<AppDomain> result = new List<AppDomain>();

            IntPtr enumHandle = IntPtr.Zero;
            CorRuntimeHostClass host = null;
            try
            {
                host = new CorRuntimeHostClass();
                host.EnumDomains(out enumHandle);
                object domain = null;

                host.NextDomain(enumHandle, out domain);
                while (domain != null)
                {
                    result.Add((AppDomain)domain);
                    host.NextDomain(enumHandle, out domain);
                }
            }
            finally
            {
                if (enumHandle != IntPtr.Zero)
                {
                    host.CloseEnum(enumHandle);
                }
                if (host != null)
                {
                    Marshal.ReleaseComObject(host);
                }
            }

            if (removeCurrentAppDomain)
            {
                result.Remove(AppDomain.CurrentDomain);
            }

            return result;
        }
#endif
    }
}
