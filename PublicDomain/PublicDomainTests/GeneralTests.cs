using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class GeneralTests
    {
        [Test]
        public void play()
        {
            Console.WriteLine("done");
        }

        /// <summary>
        /// Plays this instance.
        /// </summary>
        [Test]
        public void TestJobs()
        {
            using (PublicDomain.Win32.Job job = new PublicDomain.Win32.Job("dorp"))
            {
                job.SetLimitWorkingSetSize((uint)GlobalConstants.BytesInAMegabyte, (uint)GlobalConstants.BytesInAMegabyte * 10);

                PublicDomain.Win32.Win32Structures.JOBOBJECT_BASIC_LIMIT_INFORMATION limitInfo = job.QueryInformationJobObjectLimit();

                Console.WriteLine(limitInfo.MaximumWorkingSetSize);

                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(@"c:\windows\system32\cmd.exe");
                p.Start();

                job.AssignProcess(p);
            }
        }

        /// <summary>
        /// Adds the revision.
        /// </summary>
        [Test]
        public void AddRevision()
        {
            string csFile = FileSystemUtilities.PathCombine(Environment.CurrentDirectory, @"..\..\PublicDomain.cs");
            string text = File.ReadAllText(csFile);
            Version newVersion = null;
            string newText = RegexUtilities.Replace(text, @"public const string PublicDomainVersion = ""(\d+\.\d+\.\d+\.\d+)"";", 1, delegate(int captureIndex, string captureValue)
            {
                newVersion = VersionUtilities.AddBuild(new Version(captureValue), 1);
                return newVersion.ToString();
            });

            if (text != newText)
            {
                File.WriteAllText(csFile, newText);
                Console.WriteLine("PublicDomain version updated");
                UpdateProductVersion(newVersion);
            }
            else
            {
                Console.WriteLine("PublicDomain version NOT updated");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void UpdateProductVersion()
        {
            Version currentVersion = new Version(GlobalConstants.PublicDomainVersion);
            UpdateProductVersion(currentVersion);
        }

        private static void UpdateProductVersion(Version currentVersion)
        {
            string newProductVersion = currentVersion.ToString(3);

            string projFile = FileSystemUtilities.CombineTrackbacksInPath(FileSystemUtilities.PathCombine(Environment.CurrentDirectory, @"..\..\pdsetup\pdsetup.vdproj"));
            string text = File.ReadAllText(projFile);
            string newText = RegexUtilities.Replace(text, @"""ProductVersion"" = ""\d+:(\d+\.\d+\.\d+)""", 1, delegate(int captureIndex, string captureValue)
            {
                return newProductVersion;
            });

            if (text != newText)
            {
                // We also need a new ProductCode and PackageCode
                newText = RegexUtilities.Replace(newText, @"""ProductCode"" = ""8:{(....................................)}""", 1, delegate(int captureIndex, string captureValue)
                {
                    return Guid.NewGuid().ToString().ToUpper();
                });

                newText = RegexUtilities.Replace(newText, @"""PackageCode"" = ""8:{(....................................)}""", 1, delegate(int captureIndex, string captureValue)
                {
                    return Guid.NewGuid().ToString().ToUpper();
                });

                File.WriteAllText(projFile, newText);
                Console.WriteLine("Version updated");
            }
            else
            {
                Console.WriteLine("Version NOT updated");
            }
        }
    }
}
