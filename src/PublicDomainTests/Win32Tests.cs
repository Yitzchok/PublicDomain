using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class Win32Tests
    {
        /// <summary>
        /// Tests the GAC.
        /// </summary>
        [Test]
        public void TestGAC()
        {
            Console.WriteLine(Win32.GlobalAssemblyCache.Path);
            Console.WriteLine(Win32.GlobalAssemblyCache.ZapPath);
            Console.WriteLine(Win32.GlobalAssemblyCache.DownloadPath);
        }

        /// <summary>
        /// Gets the assembly.
        /// </summary>
        [Test]
        public void GetAssembly()
        {
            foreach (Win32.GacAssemblyName it in Win32.GlobalAssemblyCache.FindAssemblies("System"))
            {
                Console.WriteLine(it);
            }
        }

        /// <summary>
        /// Finds the largest assembly.
        /// </summary>
        [Test]
        public void FindLargestAssembly()
        {
            Console.WriteLine(Win32.GlobalAssemblyCache.FindAssemblyWithLargestVersion("System"));
        }

        /// <summary>
        /// Enumerates the GAC.
        /// </summary>
        [Test]
        public void EnumerateGAC()
        {
            try
            {
                foreach (Win32.GacAssemblyName name in Win32.GlobalAssemblyCache.GetAllAssemblies())
                {
                    Console.WriteLine(name.AssemblyName);
                }
            }
            catch (SEHException)
            {
            }
            catch (AccessViolationException)
            {
            }
        }

        /// <summary>
        /// Test1s this instance.
        /// </summary>
        [Test]
        public void Test1()
        {
            foreach (Win32.GacAssemblyName assembly in Win32.GlobalAssemblyCache.FindAssemblies("PublicDomain"))
            {
                Console.WriteLine(assembly);
            }
        }

        /// <summary>
        /// Test2s this instance.
        /// </summary>
        [Test]
        public void Test2()
        {
            List<Win32.GacAssemblyName> pds = ArrayUtilities.GetListFromEnumerable<Win32.GacAssemblyName>(Win32.GlobalAssemblyCache.FindAssemblies("PublicDomain"));
            foreach (Win32.GacAssemblyName pd in pds)
            {
                PublicDomain.Win32.Win32Enums.AssemblyCacheUninstallDisposition result =
                    Win32.GlobalAssemblyCache.UninstallAssembly(pd.DisplayName);
                Console.WriteLine(result);
            }
        }

        /// <summary>
        /// Test3s this instance.
        /// </summary>
        [Test]
        public void Test3()
        {
        }

        /// <summary>
        /// Tests the add remove list.
        /// </summary>
        [Test]
        public void TestAddRemoveList()
        {
            List<Win32.IInstalledProgram> programs = Win32.GetAddRemoveProgramList();
            foreach (Win32.IInstalledProgram program in programs)
            {
                Console.WriteLine(program.DisplayName);
            }
        }

        /// <summary>
        /// Tests the add remove list.
        /// </summary>
        [Test]
        public void TestUninstall()
        {
            List<Win32.IInstalledProgram> programs = Win32.GetAddRemoveProgramList("Public Domain");
            foreach (Win32.IInstalledProgram program in programs)
            {
                Console.WriteLine("Uninstalling " + program.DisplayName);
                program.Uninstall();
            }
        }

        [Test]
        public void TestGetTimes()
        {
            Console.WriteLine(Win32.GetLocalTime());
            Console.WriteLine(Win32.GetSystemTime());

            List<PublicDomain.Win32.Win32Structures.TIME_ZONE_INFORMATION> zones = Win32.GetSystemTimeZones();
            foreach (PublicDomain.Win32.Win32Structures.TIME_ZONE_INFORMATION zone in zones)
            {
                Console.WriteLine(zone.standardName);
            }
        }
    }
}
