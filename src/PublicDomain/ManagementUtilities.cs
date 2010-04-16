using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public static class ManagementUtilities
    {
        /// <summary>
        /// Gets the total physical memory.
        /// </summary>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static ulong GetTotalPhysicalMemory()
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher(@"SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");

            ulong totalPhysicalMemory = 0;

            foreach (ManagementObject mo in mos.Get())
            {
                totalPhysicalMemory += (ulong)mo["TotalPhysicalMemory"];
            }

            if (totalPhysicalMemory <= 0)
            {
                throw new Exception("Could not query total physical memory");
            }

            return totalPhysicalMemory;
        }
    }
}
