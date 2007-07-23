using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.ComponentModel;

namespace PublicDomain
{
    /// <summary>
    /// Interfaces into Win32 calls.
    /// http://www.codeproject.com/csharp/essentialpinvoke.asp
    /// </summary>
    public static class Win32
    {
        /// <summary>
        /// Gets the free disk space of the main system volume.
        /// </summary>
        /// <returns></returns>
        public static long GetFreeDiskSpaceOfMainSystemVolume()
        {
            return GetFreeDiskSpace(Environment.SystemDirectory);
        }

        /// <summary>
        /// Gets the free disk space.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static long GetFreeDiskSpace(string path)
        {
            long result = -1;
            long available, total, free;
            if (ExternalMethods.GetDiskFreeSpaceEx(path, out available, out total, out free))
            {
                result = free;
            }
            else
            {
                int error = GetLastError();
                if (error != 0)
                {
                    result = error;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the total disk space of main system volume.
        /// </summary>
        /// <returns></returns>
        public static long GetTotalDiskSpaceOfMainSystemVolume()
        {
            return GetTotalDiskSpace(Environment.SystemDirectory);
        }

        /// <summary>
        /// Gets the total disk space.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static long GetTotalDiskSpace(string path)
        {
            long result = -1;
            long available, total, free;
            if (ExternalMethods.GetDiskFreeSpaceEx(path, out available, out total, out free))
            {
                result = total;
            }
            else
            {
                int error = GetLastError();
                if (error != 0)
                {
                    result = error;
                }
            }
            return result;
        }

        /// <summary>
        /// Wraps a COM interface pointer.
        /// </summary>
        /// <typeparam name="T">The COM interface</typeparam>
        public interface IComWrapper<T>
        {
            /// <summary>
            /// Gets the COM interface.
            /// </summary>
            /// <value>The COM interface.</value>
            T ComInterface { get; }
        }

        /// <summary>
        /// 
        /// </summary>
        public interface IInstalledProgram
        {
            /// <summary>
            /// Gets the registry key.
            /// </summary>
            /// <value>The registry key.</value>
            string RegistryKey { get; }

            /// <summary>
            /// Gets or sets the name of the display.
            /// </summary>
            /// <value>The name of the display.</value>
            string DisplayName { get; set; }

            /// <summary>
            /// Gets or sets the display icon.
            /// </summary>
            /// <value>The display icon.</value>
            string DisplayIcon { get; set; }

            /// <summary>
            /// Gets or sets the size of the estimated.
            /// </summary>
            /// <value>The size of the estimated.</value>
            int? EstimatedSize { get; set; }

            /// <summary>
            /// Gets or sets the comments.
            /// </summary>
            /// <value>The comments.</value>
            string Comments { get; set; }

            /// <summary>
            /// Gets or sets the contact.
            /// </summary>
            /// <value>The contact.</value>
            string Contact { get; set; }

            /// <summary>
            /// Gets or sets the display version.
            /// </summary>
            /// <value>The display version.</value>
            string DisplayVersion { get; set; }

            /// <summary>
            /// Gets or sets the help link.
            /// </summary>
            /// <value>The help link.</value>
            string HelpLink { get; set; }

            /// <summary>
            /// Gets or sets the help telephone.
            /// </summary>
            /// <value>The help telephone.</value>
            string HelpTelephone { get; set; }

            /// <summary>
            /// Gets or sets the install date.
            /// </summary>
            /// <value>The install date.</value>
            TzDateTime InstallDate { get; set; }

            /// <summary>
            /// Gets or sets the publisher.
            /// </summary>
            /// <value>The publisher.</value>
            string Publisher { get; set; }

            /// <summary>
            /// Gets the modify path.
            /// </summary>
            /// <value>The modify path.</value>
            string ModifyPath { get; }

            /// <summary>
            /// Gets or sets the readme.
            /// </summary>
            /// <value>The readme.</value>
            string Readme { get; set; }

            /// <summary>
            /// Gets or sets the size.
            /// </summary>
            /// <value>The size.</value>
            string Size { get; set; }

            /// <summary>
            /// Gets the uninstall string.
            /// </summary>
            /// <value>The uninstall string.</value>
            string UninstallString { get; }

            /// <summary>
            /// Gets or sets the URI info about.
            /// </summary>
            /// <value>The URI info about.</value>
            Uri UriInfoAbout { get; set; }

            /// <summary>
            /// Gets or sets the URI update info.
            /// </summary>
            /// <value>The URI update info.</value>
            Uri UriUpdateInfo { get; set; }

            /// <summary>
            /// Gets or sets the version.
            /// </summary>
            /// <value>The version.</value>
            Version Version { get; set; }

            /// <summary>
            /// Uninstalls this instance.
            /// </summary>
            void Uninstall();

            /// <summary>
            /// Uninstalls this instance.
            /// </summary>
            /// <param name="quiet">if set to <c>true</c> [quiet].</param>
            void Uninstall(bool quiet);

            /// <summary>
            /// Refreshes this instance.
            /// </summary>
            void Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        public class InstalledProgram : IInstalledProgram
        {
            private string m_DisplayName;
            private string m_DisplayIcon;
            private int? m_EstimatedSize;
            private string m_Comments;
            private string m_Contact;
            private string m_DisplayVersion;
            private string m_HelpLink;
            private string m_HelpTelephone;
            private TzDateTime m_InstallDate;
            private string m_Publisher;
            private string m_ModifyPath;
            private string m_Readme;
            private string m_Size;
            private string m_UninstallString;
            private Uri m_UriInfoAbout;
            private Uri m_UriUpdateInfo;
            private Version m_Version;
            private string m_keyName;
            private bool m_hasData = false;

            /// <summary>
            /// Initializes a new instance of the <see cref="InstalledProgram"/> class.
            /// </summary>
            /// <param name="keyName">Name of the key.</param>
            public InstalledProgram(string keyName)
            {
                m_keyName = keyName;
            }

            private void EnsureData()
            {
                if (!m_hasData)
                {
                    Refresh();
                    m_hasData = true;
                }
            }

            /// <summary>
            /// Gets the registry key.
            /// </summary>
            /// <value>The registry key.</value>
            public string RegistryKey
            {
                get
                {
                    return m_keyName;
                }
            }

            /// <summary>
            /// Gets or sets the name of the display.
            /// </summary>
            /// <value>The name of the display.</value>
            public string DisplayName
            {
                get
                {
                    EnsureData();
                    return m_DisplayName;
                }
                set
                {
                    m_DisplayName = value;
                }
            }

            /// <summary>
            /// Gets or sets the display icon.
            /// </summary>
            /// <value>The display icon.</value>
            public string DisplayIcon
            {
                get
                {
                    EnsureData();
                    return m_DisplayIcon;
                }
                set
                {
                    m_DisplayIcon = value;
                }
            }

            /// <summary>
            /// Gets or sets the size of the estimated.
            /// </summary>
            /// <value>The size of the estimated.</value>
            public int? EstimatedSize
            {
                get
                {
                    EnsureData();
                    return m_EstimatedSize;
                }
                set
                {
                    m_EstimatedSize = value;
                }
            }

            /// <summary>
            /// Gets or sets the comments.
            /// </summary>
            /// <value>The comments.</value>
            public string Comments
            {
                get
                {
                    EnsureData();
                    return m_Comments;
                }
                set
                {
                    m_Comments = value;
                }
            }

            /// <summary>
            /// Gets or sets the contact.
            /// </summary>
            /// <value>The contact.</value>
            public string Contact
            {
                get
                {
                    EnsureData();
                    return m_Contact;
                }
                set
                {
                    m_Contact = value;
                }
            }

            /// <summary>
            /// Gets or sets the display version.
            /// </summary>
            /// <value>The display version.</value>
            public string DisplayVersion
            {
                get
                {
                    EnsureData();
                    return m_DisplayVersion;
                }
                set
                {
                    m_DisplayVersion = value;
                }
            }

            /// <summary>
            /// Gets or sets the help link.
            /// </summary>
            /// <value>The help link.</value>
            public string HelpLink
            {
                get
                {
                    EnsureData();
                    return m_HelpLink;
                }
                set
                {
                    m_HelpLink = value;
                }
            }

            /// <summary>
            /// Gets or sets the help telephone.
            /// </summary>
            /// <value>The help telephone.</value>
            public string HelpTelephone
            {
                get
                {
                    EnsureData();
                    return m_HelpTelephone;
                }
                set
                {
                    m_HelpTelephone = value;
                }
            }

            /// <summary>
            /// Gets or sets the install date.
            /// </summary>
            /// <value>The install date.</value>
            public TzDateTime InstallDate
            {
                get
                {
                    EnsureData();
                    return m_InstallDate;
                }
                set
                {
                    m_InstallDate = value;
                }
            }

            /// <summary>
            /// Gets or sets the publisher.
            /// </summary>
            /// <value>The publisher.</value>
            public string Publisher
            {
                get
                {
                    EnsureData();
                    return m_Publisher;
                }
                set
                {
                    m_Publisher = value;
                }
            }

            /// <summary>
            /// Gets the modify path.
            /// </summary>
            /// <value>The modify path.</value>
            public string ModifyPath
            {
                get
                {
                    EnsureData();
                    return m_ModifyPath;
                }
            }

            /// <summary>
            /// Gets or sets the readme.
            /// </summary>
            /// <value>The readme.</value>
            public string Readme
            {
                get
                {
                    EnsureData();
                    return m_Readme;
                }
                set
                {
                    m_Readme = value;
                }
            }

            /// <summary>
            /// Gets or sets the size.
            /// </summary>
            /// <value>The size.</value>
            public string Size
            {
                get
                {
                    EnsureData();
                    return m_Size;
                }
                set
                {
                    m_Size = value;
                }
            }

            /// <summary>
            /// Gets the uninstall string.
            /// </summary>
            /// <value>The uninstall string.</value>
            public string UninstallString
            {
                get
                {
                    EnsureData();
                    return m_UninstallString;
                }
            }

            /// <summary>
            /// Gets or sets the URI info about.
            /// </summary>
            /// <value>The URI info about.</value>
            public Uri UriInfoAbout
            {
                get
                {
                    EnsureData();
                    return m_UriInfoAbout;
                }
                set
                {
                    m_UriInfoAbout = value;
                }
            }

            /// <summary>
            /// Gets or sets the URI update info.
            /// </summary>
            /// <value>The URI update info.</value>
            public Uri UriUpdateInfo
            {
                get
                {
                    EnsureData();
                    return m_UriUpdateInfo;
                }
                set
                {
                    m_UriUpdateInfo = value;
                }
            }

            /// <summary>
            /// Gets or sets the version.
            /// </summary>
            /// <value>The version.</value>
            public Version Version
            {
                get
                {
                    EnsureData();
                    return m_Version;
                }
                set
                {
                    m_Version = value;
                }
            }

            /// <summary>
            /// Uninstalls this instance.
            /// </summary>
            public void Uninstall()
            {
                Uninstall(true);
            }

            /// <summary>
            /// Uninstalls this instance.
            /// </summary>
            public void Uninstall(bool quiet)
            {
                EnsureData();
                if (string.IsNullOrEmpty(UninstallString))
                {
                    throw new ArgumentNullException("UninstallString");
                }
                ProcessHelper process = ProcessHelper.Parse(UninstallString);
                if (quiet && process.FileName.IndexOf("msiexec.exe", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    process.AddArguments("/quiet", "/qn");
                    process.Arguments = process.Arguments.Replace("/I", "/x");
                }
                process.StartAndWaitForExit(true);
            }

            /// <summary>
            /// Refreshes this instance.
            /// </summary>
            public void Refresh()
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(m_keyName))
                {
                    m_Comments = key.GetValue("Comments") as string;
                    m_Contact = key.GetValue("Contact") as string;
                    m_DisplayIcon = key.GetValue("DisplayIcon") as string;
                    m_DisplayName = key.GetValue("DisplayName") as string;
                    m_DisplayVersion = key.GetValue("DisplayVersion") as string;
                    object obj = key.GetValue("EstimatedSize");
                    if (obj != null)
                    {
                        m_EstimatedSize = (int)obj;
                    }
                    m_HelpLink = key.GetValue("HelpLink") as string;
                    m_HelpTelephone = key.GetValue("HelpTelephone") as string;
                    string str = key.GetValue("InstallDate") as string;
                    if (!string.IsNullOrEmpty(str))
                    {
                        TzDateTime dateTime;
                        if (TzDateTime.TryParseTz(str, out dateTime, TzTimeZone.CurrentTimeZone))
                        {
                            m_InstallDate = dateTime;
                        }
                    }
                    m_ModifyPath = key.GetValue("ModifyPath") as string;
                    m_Publisher = key.GetValue("Publisher") as string;
                    m_Readme = key.GetValue("Readme") as string;
                    m_Size = key.GetValue("Size") as string;
                    m_UninstallString = key.GetValue("UninstallString") as string;
                    str = key.GetValue("URLInfoAbout") as string;
                    if (!string.IsNullOrEmpty(str))
                    {
                        Uri url;
                        if (ConversionUtilities.TryParseUri(str, out url))
                        {
                            m_UriInfoAbout = url;
                        }
                    }
                    str = key.GetValue("URLUpdateInfo") as string;
                    if (!string.IsNullOrEmpty(str))
                    {
                        Uri url;
                        if (ConversionUtilities.TryParseUri(str, out url))
                        {
                            m_UriUpdateInfo = url;
                        }
                    }
                    str = key.GetValue("Version") as string;
                    if (!string.IsNullOrEmpty(str))
                    {
                        Version version;
                        if (ConversionUtilities.TryParseVersion(str, out version))
                        {
                            m_Version = version;
                        }
                    }
                }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                return DisplayName;
            }
        }

        /// <summary>
        /// Gets the add remove program list.
        /// </summary>
        /// <returns></returns>
        public static List<IInstalledProgram> GetAddRemoveProgramList()
        {
            return GetAddRemoveProgramList(null);
        }

        /// <summary>
        /// Gets the add remove program list.
        /// </summary>
        /// <returns></returns>
        public static List<IInstalledProgram> GetAddRemoveProgramList(string filter)
        {
            string uninstallSubKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            List<IInstalledProgram> result = new List<IInstalledProgram>();
            RegistryKey key = Registry.LocalMachine.OpenSubKey(uninstallSubKey);
            if (key != null)
            {
                using (key)
                {
                    foreach (string name in key.GetSubKeyNames())
                    {
                        IInstalledProgram prog = new InstalledProgram(uninstallSubKey + @"\" + name);
                        if (filter == null || (filter != null && prog.DisplayName != null && prog.DisplayName.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) != -1))
                        {
                            result.Add(prog);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Logoffs the current user.
        /// </summary>
        public static void LogoffCurrentUser()
        {
            LogoffCurrentUser(false);
        }

        /// <summary>
        /// Logoffs the current user.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public static void LogoffCurrentUser(bool force)
        {
            ExitWindows(WindowsControl.Logoff, force);
        }

        /// <summary>
        /// Shutdowns this instance.
        /// </summary>
        public static void Shutdown()
        {
            Shutdown(false);
        }

        /// <summary>
        /// Shutdowns the specified force.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public static void Shutdown(bool force)
        {
            Shutdown(true);
        }

        /// <summary>
        /// Shutdowns the specified force.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <param name="powerOff">if set to <c>true</c> [power off].</param>
        public static void Shutdown(bool force, bool powerOff)
        {
            ExitWindows(powerOff ? WindowsControl.ShutdownAndPowerOff : WindowsControl.ShutdownNoPowerOff, force);
        }

        /// <summary>
        /// Restarts the windows.
        /// </summary>
        public static void RestartWindows()
        {
            RestartWindows(false);
        }

        /// <summary>
        /// Restarts the windows.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public static void RestartWindows(bool force)
        {
            RestartWindows(false);
        }

        /// <summary>
        /// Restarts the windows.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <param name="restartApps">if set to <c>true</c> [restart apps].</param>
        public static void RestartWindows(bool force, bool restartApps)
        {
            ExitWindows(restartApps ? WindowsControl.RestartApps : WindowsControl.Restart, force);
        }

        /// <summary>
        /// Exits the windows.
        /// </summary>
        /// <param name="control">The control.</param>
        [CLSCompliant(false)]
        public static void ExitWindows(WindowsControl control)
        {
            ExitWindows(control, false);
        }

        /// <summary>
        /// Exits the windows.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        [CLSCompliant(false)]
        public static void ExitWindows(WindowsControl control, bool force)
        {
            ExitWindows(control, force, true, Win32Constants.SHTDN_REASON_MAJOR_OTHER, Win32Constants.SHTDN_REASON_MINOR_OTHER);
        }

        /// <summary>
        /// Exits the windows.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <param name="planned">if set to <c>true</c> [planned].</param>
        /// <param name="majorReason">The major reason.</param>
        /// <param name="minorReason">The minor reason.</param>
        [CLSCompliant(false)]
        public static void ExitWindows(WindowsControl control, bool force, bool planned, uint majorReason, uint minorReason)
        {
            uint flags = (uint)control;
            if (force)
            {
                flags |= Win32Constants.EWX_FORCEIFHUNG;
            }

            uint reason = majorReason | minorReason;
            if (planned)
            {
                reason |= Win32Constants.SHTDN_REASON_FLAG_PLANNED;
            }

            if (!ExternalMethods.ExitWindowsEx(flags, reason))
            {
                GetLastErrorThrow();
            }
        }

        /// <summary>
        /// Wrapper around <see cref="PublicDomain.Win32.Win32Interfaces.IAssemblyName"/>
        /// </summary>
        [CLSCompliant(false)]
        public class GacAssemblyName : IComWrapper<PublicDomain.Win32.Win32Interfaces.IAssemblyName>
        {
            private PublicDomain.Win32.Win32Interfaces.IAssemblyName m_assemblyName;

            /// <summary>
            /// Initializes a new instance of the <see cref="AssemblyName"/> class.
            /// </summary>
            /// <param name="assemblyName">Name of the assembly.</param>
            public GacAssemblyName(PublicDomain.Win32.Win32Interfaces.IAssemblyName assemblyName)
            {
                m_assemblyName = assemblyName;
            }

            /// <summary>
            /// Gets the COM interface.
            /// </summary>
            /// <value>The COM interface.</value>
            public Win32Interfaces.IAssemblyName ComInterface
            {
                get
                {
                    return m_assemblyName;
                }
            }

            /// <summary>
            /// Gets the display name of the assembly. This
            /// is the common format strong name, with the Name of the assembly,
            /// followed by the version, followed by the Culture, and finally
            /// followed by the PublicKeyToken. For example:
            /// 
            /// System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
            /// </summary>
            public string DisplayName
            {
                get
                {
                    return GetDisplayName(
                        Win32Enums.ASM_DISPLAY_FLAGS.ASM_DISPLAYF_PUBLIC_KEY_TOKEN |
                        Win32Enums.ASM_DISPLAY_FLAGS.ASM_DISPLAYF_VERSION |
                        Win32Enums.ASM_DISPLAY_FLAGS.ASM_DISPLAYF_CULTURE
                    );
                }
            }

            /// <summary>
            /// Gets the name of the assembly.
            /// </summary>
            /// <value>The name of the assembly.</value>
            public AssemblyName AssemblyName
            {
                get
                {
                    AssemblyName result = new AssemblyName();
                    result.Name = Name;
                    result.Version = Version;
                    result.CultureInfo = Culture;
                    result.SetPublicKeyToken(PublicKeyToken);
                    return result;
                }
            }

            /// <summary>
            /// Gets the name of the display.
            /// </summary>
            /// <param name="flags">The flags.</param>
            /// <returns></returns>
            public string GetDisplayName(PublicDomain.Win32.Win32Enums.ASM_DISPLAY_FLAGS flags)
            {
                uint bufferSize = 255;
                StringBuilder buffer = new StringBuilder((int)bufferSize);
                m_assemblyName.GetDisplayName(buffer, ref bufferSize, flags);
                string result = buffer.ToString();

                // For some reason, sometimes the name comes back with \'s
                result = result.Replace(@"\", "");

                return result;
            }

            /// <summary>
            /// Gets the simple display name.
            /// </summary>
            /// <value>The simple display name.</value>
            public string Name
            {
                get
                {
                    uint bufferSize = 255;
                    StringBuilder buffer = new StringBuilder((int)bufferSize);
                    m_assemblyName.GetName(ref bufferSize, buffer);
                    return buffer.ToString();
                }
            }

            /// <summary>
            /// Gets the version.
            /// </summary>
            /// <value>The version.</value>
            public Version Version
            {
                get
                {
                    string sn = DisplayName;
                    return new Version(RegexUtilities.Extract(sn, @"Version=(\d+\.\d+\.\d+\.\d+)", 1));
                }
            }

            /// <summary>
            /// Gets the public key token.
            /// </summary>
            /// <value>The public key token.</value>
            public byte[] PublicKeyToken
            {
                get
                {
                    byte[] result = new byte[8];
                    uint bufferSize = 8;
                    IntPtr buffer = Marshal.AllocHGlobal((int)bufferSize);
                    m_assemblyName.GetProperty(PublicDomain.Win32.Win32Enums.ASM_NAME.ASM_NAME_PUBLIC_KEY_TOKEN, buffer, ref bufferSize);
                    for (int i = 0; i < 8; i++)
                    {
                        result[i] = Marshal.ReadByte(buffer, i);
                    }
                    Marshal.FreeHGlobal(buffer);
                    return result;
                }
            }

            /// <summary>
            /// Gets the public key.
            /// </summary>
            /// <value>The public key.</value>
            public byte[] PublicKey
            {
                get
                {
                    uint bufferSize = 512;
                    IntPtr buffer = Marshal.AllocHGlobal((int)bufferSize);
                    m_assemblyName.GetProperty(PublicDomain.Win32.Win32Enums.ASM_NAME.ASM_NAME_PUBLIC_KEY, buffer, ref bufferSize);
                    byte[] result = new byte[bufferSize];
                    for (int i = 0; i < bufferSize; i++)
                    {
                        result[i] = Marshal.ReadByte(buffer, i);
                    }
                    Marshal.FreeHGlobal(buffer);
                    return result;
                }
            }

            /// <summary>
            /// Gets the culture.
            /// </summary>
            /// <value>The culture.</value>
            public CultureInfo Culture
            {
                get
                {
                    uint bufferSize = 255;
                    IntPtr buffer = Marshal.AllocHGlobal((int)bufferSize);
                    m_assemblyName.GetProperty(PublicDomain.Win32.Win32Enums.ASM_NAME.ASM_NAME_CULTURE, buffer, ref bufferSize);
                    string result = Marshal.PtrToStringAuto(buffer);
                    Marshal.FreeHGlobal(buffer);
                    return new CultureInfo(result);
                }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                return DisplayName;
            }
        }

        /// <summary>
        /// Wraps <see cref="PublicDomain.Win32.Win32Interfaces.IAssemblyEnum"/>
        /// </summary>
        [CLSCompliant(false)]
        public class GacAssemblyEnum : IComWrapper<Win32Interfaces.IAssemblyEnum>, IEnumerable<GacAssemblyName>
        {
            private PublicDomain.Win32.Win32Interfaces.IAssemblyEnum m_assemblyEnum;

            /// <summary>
            /// Initializes a new instance of the <see cref="GacAssemblyEnum"/> class.
            /// </summary>
            /// <param name="assemblyEnum">The assembly enum.</param>
            public GacAssemblyEnum(PublicDomain.Win32.Win32Interfaces.IAssemblyEnum assemblyEnum)
            {
                m_assemblyEnum = assemblyEnum;
            }

            /// <summary>
            /// Gets the COM interface.
            /// </summary>
            /// <value>The COM interface.</value>
            public Win32Interfaces.IAssemblyEnum ComInterface
            {
                get
                {
                    return m_assemblyEnum;
                }
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<GacAssemblyName> GetEnumerator()
            {
                return new AssemblyEnumerator(this);
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <summary>
            /// 
            /// </summary>
            private class AssemblyEnumerator : IEnumerator<GacAssemblyName>
            {
                private GacAssemblyEnum m_assemblyEnum;
                private GacAssemblyName m_current;

                /// <summary>
                /// Initializes a new instance of the <see cref="AssemblyEnumerator"/> class.
                /// </summary>
                /// <param name="assemblyEnum">The assembly enum.</param>
                public AssemblyEnumerator(GacAssemblyEnum assemblyEnum)
                {
                    m_assemblyEnum = assemblyEnum;
                }

                /// <summary>
                /// Gets the element in the collection at the current position of the enumerator.
                /// </summary>
                /// <value></value>
                /// <returns>The element in the collection at the current position of the enumerator.</returns>
                public GacAssemblyName Current
                {
                    get
                    {
                        return m_current;
                    }
                }

                /// <summary>
                /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
                /// </summary>
                public void Dispose()
                {
                }

                /// <summary>
                /// Gets the element in the collection at the current position of the enumerator.
                /// </summary>
                /// <value></value>
                /// <returns>The element in the collection at the current position of the enumerator.</returns>
                object IEnumerator.Current
                {
                    get
                    {
                        return Current;
                    }
                }

                /// <summary>
                /// Advances the enumerator to the next element of the collection.
                /// </summary>
                /// <returns>
                /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
                /// </returns>
                /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
                public bool MoveNext()
                {
                    PublicDomain.Win32.Win32Interfaces.IAssemblyName next;
                    int result = m_assemblyEnum.ComInterface.GetNextAssembly((IntPtr)0, out next, 0);
                    bool advanced = result == Win32Constants.S_OK ? true : false;
                    if (advanced)
                    {
                        m_current = new GacAssemblyName(next);
                    }
                    return advanced;
                }

                /// <summary>
                /// Sets the enumerator to its initial position, which is before the first element in the collection.
                /// </summary>
                /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
                public void Reset()
                {
                    m_assemblyEnum.ComInterface.Reset();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public class GlobalAssemblyCache
        {
            private static string s_path;
            private static object s_lock = new object();

            /// <summary>
            /// Gets the path of the GAC.
            /// </summary>
            /// <value>The path of the GAC.</value>
            public static string Path
            {
                get
                {
                    if (s_path == null)
                    {
                        lock (s_lock)
                        {
                            s_path = Win32GetCachePath(Win32Enums.ASM_CACHE_FLAGS.ASM_CACHE_GAC);
                        }
                    }
                    return s_path;
                }
            }

            /// <summary>
            /// Gets the zap path.
            /// </summary>
            /// <value>The zap path.</value>
            public static string ZapPath
            {
                get
                {
                    return Win32GetCachePath(Win32Enums.ASM_CACHE_FLAGS.ASM_CACHE_ZAP);
                }
            }

            /// <summary>
            /// Gets the download path.
            /// </summary>
            /// <value>The download path.</value>
            public static string DownloadPath
            {
                get
                {
                    return Win32GetCachePath(Win32Enums.ASM_CACHE_FLAGS.ASM_CACHE_DOWNLOAD);
                }
            }

            /// <summary>
            /// The assembly is referenced by an application that has been
            /// installed by using Windows Installer. The szIdentifier field
            /// is set to MSI, and szNonCannonicalData is set to Windows Installer.
            /// This scheme must only be used by Windows Installer itself.
            /// </summary>
            public static Guid FUSION_REFCOUNT_MSI_GUID = new Guid("25DF0FC1-7F97-4070-ADD7-4B13BBFD7CB8");

            /// <summary>
            /// The assembly is referenced by an application that appears
            /// in Add/Remove Programs. The szIdentifier field is the token
            /// that is used to register the application with Add/Remove programs.
            /// </summary>
            public static Guid FUSION_REFCOUNT_UNINSTALL_SUBKEY_GUID = new Guid("8CEDC215-AC4b-488B-93C0-A50A49CB2FB8");

            /// <summary>
            /// The assembly is referenced by an application that is represented
            /// by a file in the file system. The szIdentifier field is the path
            /// to this file.
            /// </summary>
            public static Guid FUSION_REFCOUNT_FILEPATH_GUID = new Guid("B02F9D65-FB77-4F7A-AFA5-B391309F11C9");

            /// <summary>
            /// The assembly is referenced by an application that is only
            /// represented by an opaque string. The szIdentifier is this
            /// opaque string. The GAC does not perform existence checking
            /// for opaque references when you remove this.
            /// </summary>
            public static Guid FUSION_REFCOUNT_OPAQUE_STRING_GUID = new Guid("2EC93463-B0C3-45E1-8364-327E96AEA856");

            /// <summary>
            /// Wrapper around <see cref="PublicDomain.Win32.ExternalMethods.CreateAssemblyCache"/>
            /// </summary>
            /// <returns></returns>
            public static PublicDomain.Win32.Win32Interfaces.IAssemblyCache Win32CreateAssemblyCache()
            {
                PublicDomain.Win32.Win32Interfaces.IAssemblyCache result;
                ExternalMethods.CreateAssemblyCache(out result, 0);
                return result;
            }

            /// <summary>
            /// Wrapper around <see cref="PublicDomain.Win32.ExternalMethods.CreateAssemblyNameObject"/>.
            /// Certain properties, such as processor architecture, are set to their default values.
            /// </summary>
            /// <param name="name">A string representation of the assembly name or of a full assembly reference that is determined by dwFlags. The string representation can be null.</param>
            /// <returns></returns>
            public static PublicDomain.Win32.Win32Interfaces.IAssemblyName Win32CreateAssemblyName(string name)
            {
                return Win32CreateAssemblyName(name, Win32Enums.CREATE_ASM_NAME_OBJ_FLAGS.CANOF_SET_DEFAULT_VALUES);
            }

            /// <summary>
            /// Wrapper around <see cref="PublicDomain.Win32.ExternalMethods.CreateAssemblyNameObject"/>
            /// </summary>
            /// <param name="name">A string representation of the assembly name or of a full assembly reference that is determined by dwFlags. The string representation can be null.</param>
            /// <param name="flags">Zero or more of the bits that are defined in the CREATE_ASM_NAME_OBJ_FLAGS enumeration.</param>
            /// <returns></returns>
            public static PublicDomain.Win32.Win32Interfaces.IAssemblyName Win32CreateAssemblyName(string name, PublicDomain.Win32.Win32Enums.CREATE_ASM_NAME_OBJ_FLAGS flags)
            {
                PublicDomain.Win32.Win32Interfaces.IAssemblyName result;
                ExternalMethods.CreateAssemblyNameObject(out result, name, flags, (IntPtr)0);
                return result;
            }

            /// <summary>
            /// Wrapper around <see cref="PublicDomain.Win32.ExternalMethods.CreateAssemblyEnum"/>.
            /// Enumerates only the GAC with no filter.
            /// </summary>
            /// <returns></returns>
            public static PublicDomain.Win32.Win32Interfaces.IAssemblyEnum Win32CreateAssemblyEnum()
            {
                return Win32CreateAssemblyEnum(null);
            }

            /// <summary>
            /// Wrapper around <see cref="PublicDomain.Win32.ExternalMethods.CreateAssemblyEnum"/>.
            /// Enumerates only the GAC.
            /// </summary>
            /// <param name="filterName">An assembly name that is used to filter the enumeration. Can be null to enumerate all assemblies in the GAC.</param>
            /// <returns></returns>
            public static PublicDomain.Win32.Win32Interfaces.IAssemblyEnum Win32CreateAssemblyEnum(PublicDomain.Win32.Win32Interfaces.IAssemblyName filterName)
            {
                return Win32CreateAssemblyEnum(filterName, Win32Enums.ASM_CACHE_FLAGS.ASM_CACHE_GAC);
            }

            /// <summary>
            /// Wrapper around <see cref="PublicDomain.Win32.ExternalMethods.CreateAssemblyEnum"/>
            /// </summary>
            /// <param name="filterName">An assembly name that is used to filter the enumeration. Can be null to enumerate all assemblies in the GAC.</param>
            /// <param name="flags">Exactly one bit from the ASM_CACHE_FLAGS enumeration.</param>
            /// <returns></returns>
            public static PublicDomain.Win32.Win32Interfaces.IAssemblyEnum Win32CreateAssemblyEnum(PublicDomain.Win32.Win32Interfaces.IAssemblyName filterName, PublicDomain.Win32.Win32Enums.ASM_CACHE_FLAGS flags)
            {
                PublicDomain.Win32.Win32Interfaces.IAssemblyEnum result;
                ExternalMethods.CreateAssemblyEnum(out result, (IntPtr)0, filterName, flags, (IntPtr)0);
                return result;
            }

            /// <summary>
            /// Wrapper around <see cref="PublicDomain.Win32.ExternalMethods.CreateInstallReferenceEnum"/>
            /// </summary>
            /// <param name="name">The assembly name for which the references are enumerated.</param>
            /// <returns></returns>
            public static PublicDomain.Win32.Win32Interfaces.IInstallReferenceEnum Win32CreateInstallReferenceEnum(PublicDomain.Win32.Win32Interfaces.IAssemblyName name)
            {
                PublicDomain.Win32.Win32Interfaces.IInstallReferenceEnum result;
                ExternalMethods.CreateInstallReferenceEnum(out result, name, 0, (IntPtr)0);
                return result;
            }

            /// <summary>
            /// Wrapper around <see cref="PublicDomain.Win32.ExternalMethods.GetCachePath"/>
            /// </summary>
            /// <param name="flags">The flags.</param>
            /// <returns></returns>
            public static string Win32GetCachePath(PublicDomain.Win32.Win32Enums.ASM_CACHE_FLAGS flags)
            {
                uint bufferSize = 255;
                StringBuilder buffer = new StringBuilder((int)bufferSize);
                ExternalMethods.GetCachePath(flags, buffer, ref bufferSize);
                return buffer.ToString();
            }

            /// <summary>
            /// Gets all the assemblies in the GAC.
            /// </summary>
            /// <returns></returns>
            public static GacAssemblyEnum GetAllAssemblies()
            {
                return new GacAssemblyEnum(Win32CreateAssemblyEnum());
            }

            /// <summary>
            /// Finds all the assemblies in the GAC, matching the <paramref name="filterName"/> filter
            /// and <paramref name="flags"/>.
            /// </summary>
            /// <param name="filterName">An assembly name that is used to filter the enumeration. Can be null to enumerate all assemblies in the GAC.</param>
            /// <returns></returns>
            public static GacAssemblyEnum FindAssemblies(GacAssemblyName filterName)
            {
                return new GacAssemblyEnum(Win32CreateAssemblyEnum(filterName == null ? null : filterName.ComInterface));
            }

            /// <summary>
            /// Finds the assemblies.
            /// </summary>
            /// <param name="filterName">Name of the filter.</param>
            /// <returns></returns>
            public static GacAssemblyEnum FindAssemblies(string filterName)
            {
                return FindAssemblies(CreateAssemblyName(filterName));
            }

            /// <summary>
            /// Finds all the assemblies in the GAC, matching the <paramref name="filterName"/> filter
            /// and <paramref name="flags"/>.
            /// </summary>
            /// <param name="filterName">An assembly name that is used to filter the enumeration. Can be null to enumerate all assemblies in the GAC.</param>
            /// <param name="flags">Exactly one bit from the ASM_CACHE_FLAGS enumeration.</param>
            /// <returns></returns>
            public static GacAssemblyEnum FindAssemblies(GacAssemblyName filterName, PublicDomain.Win32.Win32Enums.ASM_CACHE_FLAGS flags)
            {
                return new GacAssemblyEnum(Win32CreateAssemblyEnum(filterName == null ? null : filterName.ComInterface, flags));
            }

            /// <summary>
            /// Creates the name of the assembly.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns></returns>
            public static GacAssemblyName CreateAssemblyName(string name)
            {
                return new GacAssemblyName(Win32CreateAssemblyName(name));
            }

            /// <summary>
            /// Finds the assembly with the largest version.
            /// </summary>
            /// <param name="filterName">Name of the filter.</param>
            /// <returns></returns>
            public static GacAssemblyName FindAssemblyWithLargestVersion(string filterName)
            {
                List<GacAssemblyName> assemblies = ArrayUtilities.GetListFromEnumerable<GacAssemblyName>(FindAssemblies(filterName));
                if (assemblies.Count > 0)
                {
                    assemblies.Sort(delegate(GacAssemblyName x, GacAssemblyName y)
                    {
                        return x.Version.CompareTo(y.Version);
                    });
                    return assemblies[assemblies.Count - 1];
                }
                return null;
            }

            /// <summary>
            /// Installs the assembly.
            /// </summary>
            /// <param name="dll">The DLL.</param>
            /// <param name="references">The references.</param>
            [CLSCompliant(false)]
            public static void InstallAssembly(string dll, params PublicDomain.Win32.Win32Structures.FUSION_INSTALL_REFERENCE[] references)
            {
                InstallAssembly(dll, Win32Enums.IASSEMBLYCACHE_INSTALL_FLAG.IASSEMBLYCACHE_INSTALL_FLAG_REFRESH, references);
            }

            /// <summary>
            /// Installs the assembly.
            /// </summary>
            /// <param name="dll">The DLL.</param>
            /// <param name="flag">The flag.</param>
            /// <param name="references">The references.</param>
            [CLSCompliant(false)]
            public static void InstallAssembly(string dll, Win32Enums.IASSEMBLYCACHE_INSTALL_FLAG flag, params PublicDomain.Win32.Win32Structures.FUSION_INSTALL_REFERENCE[] references)
            {
                if (references.Length == 0)
                {
                    references = null;
                }
                PublicDomain.Win32.Win32Interfaces.IAssemblyCache cache = Win32CreateAssemblyCache();
                cache.InstallAssembly(flag, dll, references);
            }

            /// <summary>
            /// Installs the assembly.
            /// </summary>
            /// <param name="dll">Full path to the dll.</param>
            /// <param name="referenceType">Type of the reference.</param>
            /// <param name="referenceDetails">The reference details.</param>
            /// <param name="nonCanonicalData">The non canonical data.</param>
            public static void InstallAssembly(string dll, Win32Enums.INSTALL_GAC_REFERENCE referenceType, string referenceDetails, string nonCanonicalData)
            {
                Win32Structures.FUSION_INSTALL_REFERENCE reference = BuildInstallReference(referenceType, referenceDetails, nonCanonicalData);
                InstallAssembly(dll, reference);
            }

            private static Win32Structures.FUSION_INSTALL_REFERENCE BuildInstallReference(Win32Enums.INSTALL_GAC_REFERENCE referenceType, string referenceDetails, string nonCanonicalData)
            {
                Win32Structures.FUSION_INSTALL_REFERENCE reference = new Win32Structures.FUSION_INSTALL_REFERENCE();
                reference.cbSize = (uint)Marshal.SizeOf(reference);
                reference.dwFlags = 0;
                reference.szNonCannonicalData = nonCanonicalData;
                switch (referenceType)
                {
                    case Win32Enums.INSTALL_GAC_REFERENCE.ApplicationInFilesystem:
                        reference.guidScheme = FUSION_REFCOUNT_FILEPATH_GUID;
                        reference.szIdentifier = referenceDetails;
                        break;
                    case Win32Enums.INSTALL_GAC_REFERENCE.OpaqueProgram:
                        reference.guidScheme = FUSION_REFCOUNT_OPAQUE_STRING_GUID;
                        reference.szIdentifier = referenceDetails;
                        break;
                    case Win32Enums.INSTALL_GAC_REFERENCE.ProgramInAddRemoveProgramsList:
                        reference.guidScheme = FUSION_REFCOUNT_UNINSTALL_SUBKEY_GUID;
                        reference.szIdentifier = referenceDetails;
                        break;
                    case Win32Enums.INSTALL_GAC_REFERENCE.MSI:
                        reference.guidScheme = FUSION_REFCOUNT_MSI_GUID;
                        reference.szIdentifier = "MSI";
                        reference.szNonCannonicalData = "Windows Installer";
                        break;
                    default:
                        throw new NotImplementedException();
                }
                return reference;
            }

            /// <summary>
            /// Uninstalls the assembly from the GAC.
            /// </summary>
            /// <param name="assemblyStrongName">Name of the assembly strong.</param>
            public static Win32Enums.AssemblyCacheUninstallDisposition UninstallAssembly(string assemblyStrongName)
            {
                PublicDomain.Win32.Win32Interfaces.IAssemblyCache cache = Win32CreateAssemblyCache();
                Win32Enums.AssemblyCacheUninstallDisposition result = Win32Enums.AssemblyCacheUninstallDisposition.Uninstalled;
                uint disposition;
                if (cache.UninstallAssembly(0, assemblyStrongName, null, out disposition) == Win32Constants.S_FALSE)
                {
                    result = (Win32Enums.AssemblyCacheUninstallDisposition)disposition;
                }
                return result;
            }
        }

        /// <summary>
        /// Win32 constants
        /// </summary>
        [CLSCompliant(false)]
        public static class Win32Constants
        {
            /// <summary>
            /// Success
            /// </summary>
            public const int S_OK = 0x00000000;

            /// <summary>
            /// 
            /// </summary>
            public const int S_FALSE = 0x00000001;

            /// <summary>
            /// Shuts down all processes running in the logon session of the process that called the ExitWindowsEx function. Then it logs the user off.
            /// This flag can be used only by processes running in an interactive user's logon session.
            /// </summary>
            public const uint EWX_LOGOFF = 0;

            /// <summary>
            /// Shuts down the system and turns off the power. The system must support the power-off feature.
            /// The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
            /// </summary>
            public const uint EWX_POWEROFF = 0x00000008;

            /// <summary>
            /// Shuts down the system and then restarts the system.
            /// The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
            /// </summary>
            public const uint EWX_REBOOT = 0x00000002;

            /// <summary>
            /// Shuts down the system and then restarts it, as well as any applications that have been registered for restart using the RegisterApplicationRestart function. These application receive the WM_QUERYENDSESSION message with lParam set to the ENDSESSION_CLOSEAPP value. For more information, see Guidelines for Applications.
            /// </summary>
            public const uint EWX_RESTARTAPPS = 0x00000040;

            /// <summary>
            /// Shuts down the system to a point at which it is safe to turn off the power. All file buffers have been flushed to disk, and all running processes have stopped.
            /// The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
            /// Specifying this flag will not turn off the power even if the system supports the power-off feature. You must specify EWX_POWEROFF to do this.
            /// Windows XP SP1:  If the system supports the power-off feature, specifying this flag turns off the power.
            /// </summary>
            public const uint EWX_SHUTDOWN = 0x00000001;

            /// <summary>
            /// This flag has no effect if terminal services is enabled. Otherwise, the system does not send the WM_QUERYENDSESSION and WM_ENDSESSION messages. This can cause applications to lose data. Therefore, you should only use this flag in an emergency.
            /// </summary>
            public const uint EWX_FORCE = 0x00000004;

            /// <summary>
            /// Forces processes to terminate if they do not respond to the WM_QUERYENDSESSION or WM_ENDSESSION message within the timeout interval. For more information, see the Remarks.
            /// Windows NT and Windows Me/98/95:  This value is not supported.
            /// </summary>
            public const uint EWX_FORCEIFHUNG = 0x00000010;

            /// <summary>
            /// Application issue.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_APPLICATION = 0x00040000;

            /// <summary>
            /// Hardware issue.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_HARDWARE = 0x00010000;

            /// <summary>
            /// The InitiateSystemShutdown function was used instead of InitiateSystemShutdownEx.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_LEGACY_API = 0x00070000;

            /// <summary>
            /// Operating system issue.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_OPERATINGSYSTEM = 0x00020000;

            /// <summary>
            /// Other issue.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_OTHER = 0x00000000;

            /// <summary>
            /// Power failure.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_POWER = 0x00060000;

            /// <summary>
            /// Software issue.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_SOFTWARE = 0x00030000;

            /// <summary>
            /// System failure.
            /// </summary>
            public const uint SHTDN_REASON_MAJOR_SYSTEM = 0x00050000;

            /// <summary>
            /// Blue screen crash event.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_BLUESCREEN = 0x0000000F;

            /// <summary>
            /// Unplugged.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_CORDUNPLUGGED = 0x0000000b;

            /// <summary>
            /// Disk.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_DISK = 0x00000007;

            /// <summary>
            /// Environment.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_ENVIRONMENT = 0x0000000c;

            /// <summary>
            /// Driver.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_HARDWARE_DRIVER = 0x0000000d;

            /// <summary>
            /// Hot fix.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_HOTFIX = 0x00000011;

            /// <summary>
            /// Hot fix uninstallation.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_HOTFIX_UNINSTALL = 0x00000017;

            /// <summary>
            /// Unresponsive.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_HUNG = 0x00000005;

            /// <summary>
            /// Installation.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_INSTALLATION = 0x00000002;

            /// <summary>
            /// Maintenance.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_MAINTENANCE = 0x00000001;

            /// <summary>
            /// MMC issue.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_MMC = 0x00000019;

            /// <summary>
            /// Network connectivity.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_NETWORK_CONNECTIVITY = 0x00000014;

            /// <summary>
            /// Network card.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_NETWORKCARD = 0x00000009;

            /// <summary>
            /// Other issue.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_OTHER = 0x00000000;

            /// <summary>
            /// Other driver event.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_OTHERDRIVER = 0x0000000e;

            /// <summary>
            /// Power supply.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_POWER_SUPPLY = 0x0000000a;

            /// <summary>
            /// Processor.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_PROCESSOR = 0x00000008;

            /// <summary>
            /// Reconfigure.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_RECONFIG = 0x00000004;

            /// <summary>
            /// Security issue.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_SECURITY = 0x00000013;

            /// <summary>
            /// Security patch.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_SECURITYFIX = 0x00000012;

            /// <summary>
            /// Security patch uninstallation.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_SECURITYFIX_UNINSTALL = 0x00000018;

            /// <summary>
            /// Service pack.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_SERVICEPACK = 0x00000010;

            /// <summary>
            /// Service pack uninstallation.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_SERVICEPACK_UNINSTALL = 0x00000016;

            /// <summary>
            /// Terminal Services.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_TERMSRV = 0x00000020;

            /// <summary>
            /// Unstable.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_UNSTABLE = 0x00000006;

            /// <summary>
            /// Upgrade.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_UPGRADE = 0x00000003;

            /// <summary>
            /// WMI issue.
            /// </summary>
            public const uint SHTDN_REASON_MINOR_WMI = 0x00000015;

            /// <summary>
            /// The shutdown was planned. The system generates a System State Data (SSD) file. This file contains system state information such as the processes, threads, memory usage, and configuration.
            /// If this flag is not present, the shutdown was unplanned. Notification and reporting options are controlled by a set of policies. For example, after logging in, the system displays a dialog box reporting the unplanned shutdown if the policy has been enabled. An SSD file is created only if the SSD policy is enabled on the system. The administrator can use Windows Error Reporting to send the SSD data to a central location, or to Microsoft.
            /// </summary>
            public const uint SHTDN_REASON_FLAG_PLANNED = 0x80000000;
        }

        /// <summary>
        /// Class that contains PInvoke methods into Win32
        /// </summary>
        [CLSCompliant(false)]
        public static class ExternalMethods
        {
            /// <summary>
            /// Logs off the interactive user, shuts down the system, or shuts down and restarts the system. It sends the WM_QUERYENDSESSION message to all applications to determine if they can be terminated.
            /// http://msdn2.microsoft.com/en-us/library/aa376868.aspx
            /// </summary>
            /// <param name="uFlags"></param>
            /// <param name="dwReason">The reason for initiating the shutdown. This parameter must be one of the system shutdown reason codes.
            /// If this parameter is zero, the SHTDN_REASON_FLAG_PLANNED reason code will not be set and therefore the default action is an undefined shutdown that is logged as "No title for this reason could be found". By default, it is also an unplanned shutdown. Depending on how the system is configured, an unplanned shutdown triggers the creation of a file that contains the system state information, which can delay shutdown. Therefore, do not use zero for this parameter.</param>
            /// <returns>If the function succeeds, the return value is nonzero. Because the function executes asynchronously, a nonzero return value indicates that the shutdown has been initiated. It does not indicate whether the shutdown will succeed. It is possible that the system, the user, or another application will abort the shutdown.
            /// If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

            /// <summary>
            /// To obtain an instance of the CreateAssemblyCache API
            /// </summary>
            /// <param name="ppAsmCache">Pointer to return IAssemblyCache</param>
            /// <param name="dwReserved">Reserved, must be zero.</param>
            [DllImport("fusion.dll", SetLastError = true, PreserveSig = false)]
            public static extern void CreateAssemblyCache(out PublicDomain.Win32.Win32Interfaces.IAssemblyCache ppAsmCache, uint dwReserved);

            /// <summary>
            /// An instance of IAssemblyName is obtained by calling the CreateAssemblyNameObject API
            /// </summary>
            /// <param name="ppAssemblyNameObj">Pointer to a memory location that receives the IAssemblyName pointer that is created.</param>
            /// <param name="szAssemblyName">A string representation of the assembly name or of a full assembly reference that is determined by dwFlags. The string representation can be null.</param>
            /// <param name="dwFlags">Zero or more of the bits that are defined in the CREATE_ASM_NAME_OBJ_FLAGS enumeration.</param>
            /// <param name="pvReserved">Must be null.</param>
            [DllImport("fusion.dll", SetLastError = true, CharSet = CharSet.Auto, PreserveSig = false)]
            public static extern void CreateAssemblyNameObject(
                out PublicDomain.Win32.Win32Interfaces.IAssemblyName ppAssemblyNameObj,
                string szAssemblyName,
                PublicDomain.Win32.Win32Enums.CREATE_ASM_NAME_OBJ_FLAGS dwFlags,
                IntPtr pvReserved
            );

            /// <summary>
            /// To obtain an instance of the CreateAssemblyEnum API, call the CreateAssemblyNameObject API
            /// </summary>
            /// <param name="pEnum">Pointer to a memory location that contains the IAssemblyEnum pointer.</param>
            /// <param name="pUnkReserved">Must be null.</param>
            /// <param name="pName">An assembly name that is used to filter the enumeration. Can be null to enumerate all assemblies in the GAC.</param>
            /// <param name="dwFlags">Exactly one bit from the ASM_CACHE_FLAGS enumeration.</param>
            /// <param name="pvReserved">Must be NULL.</param>
            [DllImport("fusion.dll", SetLastError = true, PreserveSig = false)]
            public static extern void CreateAssemblyEnum(
                out PublicDomain.Win32.Win32Interfaces.IAssemblyEnum pEnum,
                IntPtr pUnkReserved,
                PublicDomain.Win32.Win32Interfaces.IAssemblyName pName,
                PublicDomain.Win32.Win32Enums.ASM_CACHE_FLAGS dwFlags,
                IntPtr pvReserved
            );

            /// <summary>
            /// To obtain an instance of the CreateInstallReferenceEnum API, call the CreateInstallReferenceEnum API
            /// </summary>
            /// <param name="ppRefEnum">A pointer to a memory location that receives the IInstallReferenceEnum pointer.</param>
            /// <param name="pName">The assembly name for which the references are enumerated.</param>
            /// <param name="dwFlags">Must be zero.</param>
            /// <param name="pvReserved">Must be null.</param>
            [DllImport("fusion.dll", SetLastError = true, PreserveSig = false)]
            public static extern void CreateInstallReferenceEnum(
                out PublicDomain.Win32.Win32Interfaces.IInstallReferenceEnum ppRefEnum,
                PublicDomain.Win32.Win32Interfaces.IAssemblyName pName,
                uint dwFlags,
                IntPtr pvReserved
            );

            /// <summary>
            /// The GetCachePath API returns the storage location of the GAC.
            /// </summary>
            /// <param name="dwCacheFlags">Exactly one of the bits defined in the ASM_CACHE_FLAGS enumeration.</param>
            /// <param name="pwzCachePath">Pointer to a buffer that is to receive the path of the GAC as a Unicode string.</param>
            /// <param name="pcchPath">Length of the pwszCachePath buffer, in Unicode characters.</param>
            [DllImport("fusion.dll", SetLastError = true, CharSet = CharSet.Auto, PreserveSig = false)]
            public static extern void GetCachePath(
                PublicDomain.Win32.Win32Enums.ASM_CACHE_FLAGS dwCacheFlags,
                [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwzCachePath,
                ref uint pcchPath
            );

            /// <summary>
            /// The GetDiskFreeSpaceEx function retrieves information about the amount of space that is available on a disk volume, which is the total amount of space, the total amount of free space, and the total amount of free space available to the user that is associated with the calling thread.
            /// </summary>
            /// <param name="lpDirectoryName">A pointer to a null-terminated string that specifies a directory on a disk.
            /// If this parameter is NULL, the function uses the root of the current disk.
            /// If this parameter is a UNC name, it must include a trailing backslash, for example, \\MyServer\MyShare\.
            /// This parameter does not have to specify the root directory on a disk. The function accepts any directory on a disk.
            /// The calling application must have FILE_LIST_DIRECTORY access rights for this directory.</param>
            /// <param name="lpFreeBytesAvailableToCaller">A pointer to a variable that receives the total number of free bytes on a disk that are available to the user who is associated with the calling thread.
            /// This parameter can be NULL.
            /// Windows Me/98/95:  This parameter cannot be NULL.
            /// If per-user quotas are being used, this value may be less than the total number of free bytes on a disk.</param>
            /// <param name="lpTotalNumberOfBytes">A pointer to a variable that receives the total number of bytes on a disk that are available to the user who is associated with the calling thread.
            /// This parameter can be NULL.
            /// Windows Me/98/95 and Windows NT 4.0:  This parameter cannot be NULL.
            /// If per-user quotas are being used, this value may be less than the total number of bytes on a disk.
            /// To determine the total number of bytes on a disk or volume, use IOCTL_DISK_GET_LENGTH_INFO.</param>
            /// <param name="lpTotalNumberOfFreeBytes">A pointer to a variable that receives the total number of free bytes on a disk.
            /// This parameter can be NULL.</param>
            /// <returns>If the function succeeds, the return value is nonzero.
            /// If the function fails, the return value is 0 (zero). To get extended error information, call GetLastError.</returns>
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern bool GetDiskFreeSpaceEx(
                string lpDirectoryName,
                out long lpFreeBytesAvailableToCaller,
                out long lpTotalNumberOfBytes,
                out long lpTotalNumberOfFreeBytes
            );

            /// <summary>
            /// Creates or opens a job object.
            /// </summary>
            /// <param name="lpJobAttributes">A pointer to a SECURITY_ATTRIBUTES structure that specifies the security descriptor for the job object and determines whether child processes can inherit the returned handle. If lpJobAttributes is NULL, the job object gets a default security descriptor and the handle cannot be inherited. The ACLs in the default security descriptor for a job object come from the primary or impersonation token of the creator.</param>
            /// <param name="lpName">The name of the job. The name is limited to MAX_PATH characters. Name comparison is case-sensitive.
            /// If lpName is NULL, the job is created without a name.
            /// If lpName matches the name of an existing event, semaphore, mutex, waitable timer, or file-mapping object, the function fails and the GetLastError function returns ERROR_INVALID_HANDLE. This occurs because these objects share the same name space.
            /// The object can be created in a private namespace. For more information, see Object Namespaces.
            /// Terminal Services:  The name can have a "Global\" or "Local\" prefix to explicitly create the object in the global or session name space. The remainder of the name can contain any character except the backslash character (\). For more information, see Kernel Object Namespaces.
            /// Windows 2000:  If Terminal Services is not running, the "Global\" and "Local\" prefixes are ignored. The remainder of the name can contain any character except the backslash character.</param>
            /// <returns>If the function succeeds, the return value is a handle to the job object. The handle has the JOB_OBJECT_ALL_ACCESS access right. If the object existed before the function call, the function returns a handle to the existing job object and GetLastError returns ERROR_ALREADY_EXISTS.
            /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.</returns>
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern IntPtr CreateJobObject(
                [In] ref PublicDomain.Win32.Win32Structures.SECURITY_ATTRIBUTES lpJobAttributes,
                string lpName
            );

            /// <summary>
            /// Closes an open object handle.
            /// </summary>
            /// <param name="hObject">A valid handle to an open object.</param>
            /// <returns>If the function succeeds, the return value is nonzero.
            /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
            /// If the application is running under a debugger, the function will throw an exception if it receives either a handle value that is not valid or a pseudo-handle value. This can happen if you close a handle twice, or if you call CloseHandle on a handle returned by the FindFirstFile function.</returns>
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool CloseHandle(IntPtr hObject);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="hJob"></param>
            /// <param name="JobObjectInfoClass"></param>
            /// <param name="lpJobObjectInfo"></param>
            /// <param name="cbJobObjectInfoLength"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "SetInformationJobObject")]
            internal static extern bool SetInformationJobObjectLimit(
                IntPtr hJob,
                PublicDomain.Win32.Win32Enums.JobObjectInfoClass JobObjectInfoClass,
                ref Win32Structures.JOBOBJECT_BASIC_LIMIT_INFORMATION lpJobObjectInfo,
                int cbJobObjectInfoLength
            );

            /// <summary>
            /// 
            /// </summary>
            /// <param name="hJob"></param>
            /// <param name="JobObjectInfoClass"></param>
            /// <param name="lpJobObjectInfo"></param>
            /// <param name="cbJobObjectInfoLength"></param>
            /// <param name="lpReturnLength"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "QueryInformationJobObject")]
            internal static extern bool QueryInformationJobObjectLimit(
                IntPtr hJob,
                PublicDomain.Win32.Win32Enums.JobObjectInfoClass JobObjectInfoClass,
                out Win32Structures.JOBOBJECT_BASIC_LIMIT_INFORMATION lpJobObjectInfo,
                int cbJobObjectInfoLength,
                out int lpReturnLength
            );

            /// <summary>
            /// 
            /// </summary>
            /// <param name="hJob"></param>
            /// <param name="hProcess"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool AssignProcessToJobObject(
                IntPtr hJob,
                IntPtr hProcess
            );
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public class Job : IDisposable
        {
            private string m_name;
            private IntPtr? m_handle;
            private bool m_inheritSecurityHandle;
            private IntPtr m_securityDescriptor;

            /// <summary>
            /// Initializes a new instance of the <see cref="Job"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            public Job(string name)
                : this(name, true, (IntPtr)null)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Job"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="inheritSecurityHandle">if set to <c>true</c> [inherit security handle].</param>
            /// <param name="securityDescriptor">The security descriptor.</param>
            public Job(string name, bool inheritSecurityHandle, IntPtr securityDescriptor)
            {
                m_name = name;
                m_inheritSecurityHandle = inheritSecurityHandle;
                m_securityDescriptor = securityDescriptor;
                Create();
            }

            /// <summary>
            /// Creates the job with memory limits.
            /// </summary>
            /// <param name="minWorkingSetSize">Size of the min working set.</param>
            /// <param name="maxWorkingSetSize">Size of the max working set.</param>
            /// <param name="processesToLimit">The processes to limit.</param>
            /// <returns></returns>
            public static Job CreateJobWithMemoryLimits(uint minWorkingSetSize, uint maxWorkingSetSize, params Process[] processesToLimit)
            {
                Job job = new Job(StringUtilities.RandomString(10, true));
                job.SetLimitWorkingSetSize(minWorkingSetSize, maxWorkingSetSize);
                foreach (Process p in processesToLimit)
                {
                    job.AssignProcess(p);
                }
                return job;
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name
            {
                get
                {
                    return m_name;
                }
            }

            /// <summary>
            /// Gets a value indicating whether [inherit security handle].
            /// </summary>
            /// <value>
            /// 	<c>true</c> if [inherit security handle]; otherwise, <c>false</c>.
            /// </value>
            public bool InheritSecurityHandle
            {
                get
                {
                    return m_inheritSecurityHandle;
                }
            }

            /// <summary>
            /// Gets the security descriptor.
            /// </summary>
            /// <value>The security descriptor.</value>
            public IntPtr SecurityDescriptor
            {
                get
                {
                    return m_securityDescriptor;
                }
            }

            /// <summary>
            /// Releases unmanaged resources and performs other cleanup operations before the
            /// <see cref="Job"/> is reclaimed by garbage collection.
            /// </summary>
            ~Job()
            {
                Dispose();
            }

            /// <summary>
            /// Creates this instance.
            /// </summary>
            public void Create()
            {
                if (m_handle != null && m_handle.Value != null)
                {
                    throw new Exception("Previous handle exists");
                }

                PublicDomain.Win32.Win32Structures.SECURITY_ATTRIBUTES securityAttributes = new Win32Structures.SECURITY_ATTRIBUTES();

                securityAttributes.bInheritHandle = InheritSecurityHandle ? 1 : 0;
                securityAttributes.lpSecurityDescriptor = SecurityDescriptor;
                securityAttributes.nLength = Marshal.SizeOf(securityAttributes);

                m_handle = ExternalMethods.CreateJobObject(ref securityAttributes, Name);
                if (m_handle == null || m_handle.Value == null)
                {
                    GetLastErrorThrow();
                }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (m_handle != null && m_handle.Value != null)
                {
                    bool result = ExternalMethods.CloseHandle(m_handle.Value);

                    m_handle = null;

                    if (!result)
                    {
                        Win32.GetLastErrorThrow();
                    }
                }
            }

            /// <summary>
            /// Sets the size of the limit maximum working set.
            /// </summary>
            /// <param name="minWorkingSetSize">Size of the min working set.</param>
            /// <param name="maxWorkingSetSize">Size of the max working set.</param>
            public void SetLimitWorkingSetSize(uint minWorkingSetSize, uint maxWorkingSetSize)
            {
                Win32Structures.JOBOBJECT_BASIC_LIMIT_INFORMATION limitInfo = new Win32Structures.JOBOBJECT_BASIC_LIMIT_INFORMATION();

                limitInfo.LimitFlags = Win32Enums.LimitFlags.JOB_OBJECT_LIMIT_WORKINGSET;
                limitInfo.MinimumWorkingSetSize = minWorkingSetSize;
                limitInfo.MaximumWorkingSetSize = maxWorkingSetSize;

                int size = Marshal.SizeOf(limitInfo);
                if (!ExternalMethods.SetInformationJobObjectLimit(
                    m_handle.Value,
                    Win32Enums.JobObjectInfoClass.JobObjectBasicLimitInformation,
                    ref limitInfo,
                    size
                ))
                {
                    GetLastErrorThrow();
                }
            }

            /// <summary>
            /// Queries the information job object limit.
            /// </summary>
            public Win32Structures.JOBOBJECT_BASIC_LIMIT_INFORMATION QueryInformationJobObjectLimit()
            {
                Win32Structures.JOBOBJECT_BASIC_LIMIT_INFORMATION limitInfo = new Win32Structures.JOBOBJECT_BASIC_LIMIT_INFORMATION();

                int size;
                if (!ExternalMethods.QueryInformationJobObjectLimit(
                    m_handle.Value,
                    Win32Enums.JobObjectInfoClass.JobObjectBasicLimitInformation,
                    out limitInfo,
                    Marshal.SizeOf(limitInfo),
                    out size
                ))
                {
                    GetLastErrorThrow();
                }

                return limitInfo;
            }

            /// <summary>
            /// Assigns the process.
            /// </summary>
            /// <param name="process">The process.</param>
            public void AssignProcess(Process process)
            {
                if (!ExternalMethods.AssignProcessToJobObject(m_handle.Value, process.Handle))
                {
                    GetLastErrorThrow();
                }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                return m_name + "(" + m_handle + ")";
            }
        }

        /// <summary>
        /// Enumeration that directs a windows control action.
        /// </summary>
        [CLSCompliant(false)]
        public enum WindowsControl : uint
        {
            /// <summary>
            /// 
            /// </summary>
            Logoff = Win32Constants.EWX_LOGOFF,

            /// <summary>
            /// 
            /// </summary>
            ShutdownAndPowerOff = Win32Constants.EWX_POWEROFF,

            /// <summary>
            /// 
            /// </summary>
            ShutdownNoPowerOff = Win32Constants.EWX_SHUTDOWN,

            /// <summary>
            /// 
            /// </summary>
            Restart = Win32Constants.EWX_REBOOT,

            /// <summary>
            /// 
            /// </summary>
            RestartApps = Win32Constants.EWX_RESTARTAPPS,
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Win32Enums
        {
            /// <summary>
            /// 
            /// </summary>
            public enum INSTALL_GAC_REFERENCE
            {
                /// <summary>
                /// The assembly is referenced by an application that is represented
                /// by a file in the file system.
                /// </summary>
                ApplicationInFilesystem,

                /// <summary>
                /// The assembly is referenced by an application that appears
                /// in Add/Remove Programs.
                /// </summary>
                ProgramInAddRemoveProgramsList,

                /// <summary>
                /// The assembly is referenced by an application that is only
                /// represented by an opaque string. The GAC does not perform existence checking
                /// for opaque references when you remove this.
                /// </summary>
                OpaqueProgram,

                /// <summary>
                /// 
                /// </summary>
                MSI
            }

            /// <summary>
            /// 
            /// </summary>
            public enum AssemblyCacheUninstallDisposition
            {
                /// <summary>
                /// The assembly files have been removed from the GAC.
                /// </summary>
                Uninstalled = 1,

                /// <summary>
                /// An application is using the assembly. This value is returned on Microsoft Windows 95 and Microsoft Windows 98.
                /// </summary>
                StillInUs = 2,

                /// <summary>
                /// The assembly does not exist in the GAC.
                /// </summary>
                AlreadyUninstalled = 3,

                /// <summary>
                /// Not used.
                /// </summary>
                DeletePending = 4,

                /// <summary>
                /// The assembly has not been removed from the GAC because another application reference exists.
                /// </summary>
                HasInstallReferences = 5,

                /// <summary>
                /// The reference that is specified in pRefData is not found in the GAC.
                /// </summary>
                ReferenceNotFound = 6
            }

            /// <summary>
            /// 
            /// </summary>
            [Flags]
            public enum CREATE_ASM_NAME_OBJ_FLAGS
            {
                /// <summary>
                /// If this flag is specified, the szAssemblyName parameter is a full assembly name and is parsed to the individual properties. If the flag is not specified, szAssemblyName is the "Name" portion of the assembly name.
                /// </summary>
                CANOF_PARSE_DISPLAY_NAME = 0x1,

                /// <summary>
                /// If this flag is specified, certain properties, such as processor architecture, are set to their default values.
                /// </summary>
                CANOF_SET_DEFAULT_VALUES = 0x2
            }

            /// <summary>
            /// The ASM_NAME enumeration property ID describes the valid names of the name-value pairs in an assembly name.
            /// </summary>
            public enum ASM_NAME
            {
                /// <summary>
                /// Property ID for the assembly's public key. The value is a byte array.
                /// </summary>
                ASM_NAME_PUBLIC_KEY = 0,

                /// <summary>
                /// Property ID for the assembly's public key token. The value is a byte array.
                /// </summary>
                ASM_NAME_PUBLIC_KEY_TOKEN,

                /// <summary>
                /// Property ID for a reserved name-value pair. The value is a byte array.
                /// </summary>
                ASM_NAME_HASH_VALUE,

                /// <summary>
                /// Property ID for the assembly's simple name. The value is a string value.
                /// </summary>
                ASM_NAME_NAME,

                /// <summary>
                /// Property ID for the assembly's major version. The value is a WORD value.
                /// </summary>
                ASM_NAME_MAJOR_VERSION,

                /// <summary>
                /// Property ID for the assembly's minor version. The value is a WORD value.
                /// </summary>
                ASM_NAME_MINOR_VERSION,

                /// <summary>
                /// Property ID for the assembly's build version. The value is a WORD value.
                /// </summary>
                ASM_NAME_BUILD_NUMBER,

                /// <summary>
                /// Property ID for the assembly's revision version. The value is a WORD value.
                /// </summary>
                ASM_NAME_REVISION_NUMBER,

                /// <summary>
                /// Property ID for the assembly's culture. The value is a string value.
                /// </summary>
                ASM_NAME_CULTURE,

                /// <summary>
                /// Property ID for a reserved name-value pair.
                /// </summary>
                ASM_NAME_PROCESSOR_ID_ARRAY,

                /// <summary>
                /// Property ID for a reserved name-value pair.
                /// </summary>
                ASM_NAME_OSINFO_ARRAY,

                /// <summary>
                /// Property ID for a reserved name-value pair. The value is a DWORD value.
                /// </summary>
                ASM_NAME_HASH_ALGID,

                /// <summary>
                /// Property ID for a reserved name-value pair.
                /// </summary>
                ASM_NAME_ALIAS,

                /// <summary>
                /// Property ID for a reserved name-value pair.
                /// </summary>
                ASM_NAME_CODEBASE_URL,

                /// <summary>
                /// Property ID for a reserved name-value pair. The value is a FILETIME structure.
                /// </summary>
                ASM_NAME_CODEBASE_LASTMOD,

                /// <summary>
                /// Property ID for the assembly as a simply named assembly that does not have a public key.
                /// </summary>
                ASM_NAME_NULL_PUBLIC_KEY,

                /// <summary>
                /// Property ID for the assembly as a simply named assembly that does not have a public key token.
                /// </summary>
                ASM_NAME_NULL_PUBLIC_KEY_TOKEN,

                /// <summary>
                /// Property ID for a reserved name-value pair. The value is a string value.
                /// </summary>
                ASM_NAME_CUSTOM,

                /// <summary>
                /// Property ID for a reserved name-value pair.
                /// </summary>
                ASM_NAME_NULL_CUSTOM,

                /// <summary>
                /// Property ID for a reserved name-value pair.
                /// </summary>
                ASM_NAME_MVID,

                /// <summary>
                /// Reserved.
                /// </summary>
                ASM_NAME_MAX_PARAMS
            }

            /// <summary>
            /// 
            /// </summary>
            [Flags]
            public enum ASM_DISPLAY_FLAGS
            {
                /// <summary>
                /// Includes the version number as part of the display name.
                /// </summary>
                ASM_DISPLAYF_VERSION = 0x1,

                /// <summary>
                /// Includes the culture.
                /// </summary>
                ASM_DISPLAYF_CULTURE = 0x2,

                /// <summary>
                /// Includes the public key token.
                /// </summary>
                ASM_DISPLAYF_PUBLIC_KEY_TOKEN = 0x4,

                /// <summary>
                /// Includes the public key.
                /// </summary>
                ASM_DISPLAYF_PUBLIC_KEY = 0x8,

                /// <summary>
                /// Includes the custom part of the assembly name.
                /// </summary>
                ASM_DISPLAYF_CUSTOM = 0x10,

                /// <summary>
                /// Includes the processor architecture.
                /// </summary>
                ASM_DISPLAYF_PROCESSORARCHITECTURE = 0x20,

                /// <summary>
                /// Includes the language ID.
                /// </summary>
                ASM_DISPLAYF_LANGUAGEID = 0x40
            }

            /// <summary>
            /// 
            /// </summary>
            [Flags]
            public enum ASM_CMP_FLAGS
            {
                /// <summary>
                /// Compare the name portion of the assembly names.
                /// </summary>
                ASM_CMPF_NAME = 0x1,

                /// <summary>
                /// Compare the major version portion of the assembly names.
                /// </summary>
                ASM_CMPF_MAJOR_VERSION = 0x2,

                /// <summary>
                /// Compare the minor version portion of the assembly names.
                /// </summary>
                ASM_CMPF_MINOR_VERSION = 0x4,

                /// <summary>
                /// Compare the build version portion of the assembly names.
                /// </summary>
                ASM_CMPF_BUILD_NUMBER = 0x8,

                /// <summary>
                /// Compare the revision version portion of the assembly names.
                /// </summary>
                ASM_CMPF_REVISION_NUMBER = 0x10,

                /// <summary>
                /// Compare the public key token portion of the assembly names.
                /// </summary>
                ASM_CMPF_PUBLIC_KEY_TOKEN = 0x20,

                /// <summary>
                /// Compare the culture portion of the assembly names.
                /// </summary>
                ASM_CMPF_CULTURE = 0x40,

                /// <summary>
                /// Compare the custom portion of the assembly names.
                /// </summary>
                ASM_CMPF_CUSTOM = 0x80,

                /// <summary>
                /// Compare all portions of the assembly names.
                /// </summary>
                ASM_CMPF_ALL = ASM_CMPF_NAME | ASM_CMPF_MAJOR_VERSION | ASM_CMPF_MINOR_VERSION |
                               ASM_CMPF_REVISION_NUMBER | ASM_CMPF_BUILD_NUMBER |
                               ASM_CMPF_PUBLIC_KEY_TOKEN | ASM_CMPF_CULTURE | ASM_CMPF_CUSTOM,

                /// <summary>
                /// Ignore the version number to compare assemblies with simple names.
                /// 
                /// For strongly named assemblies, ASM_CMPF_DEFAULT==ASM_CMPF_ALL.
                /// For simply named assemblies, this is also true. However, when
                /// performing IAssemblyName::IsEqual, the build number/revision 
                /// number will be removed from the comparison.
                /// </summary>
                ASM_CMPF_DEFAULT = 0x100
            }

            /// <summary>
            /// 
            /// </summary>
            [Flags]
            public enum ASM_CACHE_FLAGS
            {
                /// <summary>
                /// Enumerates the cache of precompiled assemblies by using Ngen.exe.
                /// </summary>
                ASM_CACHE_ZAP = 0x1,

                /// <summary>
                /// Enumerates the GAC.
                /// </summary>
                ASM_CACHE_GAC = 0x2,

                /// <summary>
                /// Enumerates the assemblies that have been downloaded on-demand or that have been shadow-copied.
                /// </summary>
                ASM_CACHE_DOWNLOAD = 0x4
            }

            /// <summary>
            /// 
            /// </summary>
            public enum IASSEMBLYCACHE_INSTALL_FLAG
            {
                /// <summary>
                /// If the assembly is already installed in the GAC and the file version numbers of the assembly being installed are the same or later, the files are replaced.
                /// </summary>
                IASSEMBLYCACHE_INSTALL_FLAG_REFRESH = 1,

                /// <summary>
                /// The files of an existing assembly are overwritten regardless of their version number.
                /// </summary>
                IASSEMBLYCACHE_INSTALL_FLAG_FORCE_REFRESH = 2
            }

            /// <summary>
            /// 
            /// </summary>
            public enum JobObjectInfoClass
            {
                /// <summary>
                /// 
                /// </summary>
                JobObjectBasicAccountingInformation = 1,

                /// <summary>
                /// 
                /// </summary>
                JobObjectBasicLimitInformation = 2,

                /// <summary>
                /// 
                /// </summary>
                JobObjectBasicProcessIdList = 3,

                /// <summary>
                /// 
                /// </summary>
                JobObjectBasicUIRestrictions = 4,

                /// <summary>
                /// 
                /// </summary>
                JobObjectSecurityLimitInformation = 5,

                /// <summary>
                /// 
                /// </summary>
                JobObjectEndOfJobTimeInformation = 6,

                /// <summary>
                /// 
                /// </summary>
                JobObjectAssociateCompletionPortInformation = 7,

                /// <summary>
                /// 
                /// </summary>
                JobObjectBasicAndIoAccountingInformation = 8,

                /// <summary>
                /// 
                /// </summary>
                JobObjectExtendedLimitInformation = 9
            }

            /// <summary>
            /// 
            /// </summary>
            [Flags]
            public enum LimitFlags
            {
                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_ACTIVE_PROCESS = 0x00000008,

                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_AFFINITY = 0x00000010,

                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_BREAKAWAY_OK = 0x00000800,

                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION = 0x00000400,

                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_JOB_MEMORY = 0x00000200,

                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_JOB_TIME = 0x00000004,

                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x00002000,

                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME = 0x00000040,

                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_PRIORITY_CLASS = 0x00000020,

                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_PROCESS_MEMORY = 0x00000100,

                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_PROCESS_TIME = 0x00000002,

                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_SCHEDULING_CLASS = 0x00000080,

                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK = 0x00001000,

                /// <summary>
                /// 
                /// </summary>
                JOB_OBJECT_LIMIT_WORKINGSET = 0x00000001
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Win32Structures
        {
            /// <summary>
            /// The FUSION_INSTALL_REFERENCE structure represents a reference
            /// that is made when an application has installed an assembly in the GAC.
            /// </summary>
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            [CLSCompliant(false)]
            public struct FUSION_INSTALL_REFERENCE
            {
                /// <summary>
                /// The size of the structure in bytes.
                /// </summary>
                public uint cbSize;

                /// <summary>
                /// Reserved, must be zero.
                /// </summary>
                public uint dwFlags;

                /// <summary>
                /// The entity that adds the reference.
                /// 
                /// Possible values for the guidScheme field can be one of the following:
                /// FUSION_REFCOUNT_MSI_GUID - The assembly is referenced by an application that has been installed by using Windows Installer. The szIdentifier field is set to MSI, and szNonCannonicalData is set to Windows Installer. This scheme must only be used by Windows Installer itself.
                /// FUSION_REFCOUNT_UNINSTALL_SUBKEY_GUID - The assembly is referenced by an application that appears in Add/Remove Programs. The szIdentifier field is the token that is used to register the application with Add/Remove programs.
                /// FUSION_REFCOUNT_FILEPATH_GUID - The assembly is referenced by an application that is represented by a file in the file system. The szIdentifier field is the path to this file.
                /// FUSION_REFCOUNT_OPAQUE_STRING_GUID - The assembly is referenced by an application that is only represented by an opaque string. The szIdentifier is this opaque string. The GAC does not perform existence checking for opaque references when you remove this.
                /// </summary>
                public Guid guidScheme;

                /// <summary>
                /// A unique string that identifies the application that installed the assembly.
                /// </summary>
                public string szIdentifier;

                /// <summary>
                /// A string that is only understood by the entity that adds the reference. The GAC only stores this string.
                /// </summary>
                public string szNonCannonicalData;
            }

            /// <summary>
            /// The ASSEMBLY_INFO structure represents information about an
            /// assembly in the assembly cache.
            /// </summary>
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            [CLSCompliant(false)]
            public struct ASSEMBLY_INFO
            {
                /// <summary>
                /// Size of the structure in bytes. Permits additions
                /// to the structure in future version of the .NET Framework.
                /// </summary>
                public uint cbAssemblyInfo;

                /// <summary>
                /// Indicates one or more of the ASSEMBLYINFO_FLAG_* bits.
                /// 
                /// dwAssemblyFlags can have one of the following values:
                /// ASSEMBLYINFO_FLAG__INSTALLED - Indicates that the assembly is actually installed. Always set in current version of the .NET Framework.
                /// ASSEMBLYINFO_FLAG__PAYLOADRESIDENT - Never set in the current version of the .NET Framework.
                /// </summary>
                public uint dwAssemblyFlags;

                /// <summary>
                /// The size of the files that make up the assembly in kilobytes (KB).
                /// </summary>
                public ulong uliAssemblySizeInKB;

                /// <summary>
                /// A pointer to a string buffer that holds the current path of the directory that contains the files that make up the assembly. The path must end with a zero.
                /// </summary>
                public string pszCurrentAssemblyPathBuf;

                /// <summary>
                /// Size of the buffer that the pszCurrentAssemblyPathBug field points to.
                /// </summary>
                public uint cchBuf;
            }

            /// <summary>
            /// The SECURITY_ATTRIBUTES structure contains the security descriptor for an object and specifies whether the handle retrieved by specifying this structure is inheritable. This structure provides security settings for objects created by various functions, such as CreateFile, CreatePipe, CreateProcess, RegCreateKeyEx, or RegSaveKeyEx.
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct SECURITY_ATTRIBUTES
            {
                /// <summary>
                /// The size, in bytes, of this structure. Set this value to the size of the SECURITY_ATTRIBUTES structure.
                /// </summary>
                public int nLength;

                /// <summary>
                /// A pointer to a security descriptor for the object that controls the sharing of it. If NULL is specified for this member, the object is assigned the default security descriptor of the calling process. This is not the same as granting access to everyone by assigning a NULL discretionary access control list (DACL). The default security descriptor is based on the default DACL of the access token belonging to the calling process. By default, the default DACL in the access token of a process allows access only to the user represented by the access token. If other users must access the object, you can either create a security descriptor with the appropriate access, or add ACEs to the DACL that grants access to a group of users.
                /// </summary>
                public IntPtr lpSecurityDescriptor;

                /// <summary>
                /// A Boolean value that specifies whether the returned handle is inherited when a new process is created. If this member is TRUE, the new process inherits the handle.
                /// </summary>
                public int bInheritHandle;
            }

            /// <summary>
            /// 
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            [CLSCompliant(false)]
            public struct JOBOBJECT_BASIC_LIMIT_INFORMATION
            {
                /// <summary>
                /// 
                /// </summary>
                public long PerProcessUserTimeLimit;

                /// <summary>
                /// 
                /// </summary>
                public long PerJobUserTimeLimit;

                /// <summary>
                /// 
                /// </summary>
                public PublicDomain.Win32.Win32Enums.LimitFlags LimitFlags;

                /// <summary>
                /// 
                /// </summary>
                public uint MinimumWorkingSetSize;

                /// <summary>
                /// 
                /// </summary>
                public uint MaximumWorkingSetSize;

                /// <summary>
                /// 
                /// </summary>
                public int ActiveProcessLimit;

                /// <summary>
                /// 
                /// </summary>
                public IntPtr Affinity;

                /// <summary>
                /// 
                /// </summary>
                public int PriorityClass;

                /// <summary>
                /// 
                /// </summary>
                public int SchedulingClass;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public static class Win32Interfaces
        {
            /// <summary>
            /// 
            /// </summary>
            [ComImport, Guid("E707DCDE-D1CD-11D2-BAB9-00C04F8ECEAE"),
                InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IAssemblyCache
            {
                /// <summary>
                /// The IAssemblyCache::UninstallAssembly method removes a reference to an assembly from the GAC. If other applications hold no other references to the assembly, the files that make up the assembly are removed from the GAC.
                /// </summary>
                /// <param name="dwFlags">No flags defined. Must be zero.</param>
                /// <param name="pszAssemblyName">The name of the assembly. A zero-ended Unicode string.</param>
                /// <param name="pRefData">A pointer to a FUSION_INSTALL_REFERENCE structure. Although this is not recommended, this parameter can be null. The assembly is installed without an application reference, or all existing application references are gone.</param>
                /// <param name="pulDisposition">Pointer to an integer that indicates the action that is performed by the function.
                /// 
                /// If pulDisposition is not null, pulDisposition contains one of the following values:
                /// IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNINSTALLED - The assembly files have been removed from the GAC.
                /// IASSEMBLYCACHE_UNINSTALL_DISPOSITION_STILL_IN_USE - An application is using the assembly. This value is returned on Microsoft Windows 95 and Microsoft Windows 98.
                /// IASSEMBLYCACHE_UNINSTALL_DISPOSITION_ALREADY_UNINSTALLED - The assembly does not exist in the GAC.
                /// IASSEMBLYCACHE_UNINSTALL_DISPOSITION_DELETE_PENDING - Not used.
                /// IASSEMBLYCACHE_UNINSTALL_DISPOSITION_HAS_INSTALL_REFERENCES - The assembly has not been removed from the GAC because another application reference exists.
                /// IASSEMBLYCACHE_UNINSTALL_DISPOSITION_REFERENCE_NOT_FOUND - The reference that is specified in pRefData is not found in the GAC.
                /// </param>
                /// <returns>S_OK - The assembly has been uninstalled.
                /// S_FALSE - The operation succeeded, but the assembly was not removed from the GAC. The reason is described in pulDisposition.</returns>
                [PreserveSig]
                int UninstallAssembly(
                    int dwFlags,
                    [MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName,
                    [MarshalAs(UnmanagedType.LPArray)] PublicDomain.Win32.Win32Structures.FUSION_INSTALL_REFERENCE[] pRefData,
                    out uint pulDisposition
                );

                /// <summary>
                /// The IAssemblyCache::QueryAssemblyInfo method retrieves information about an assembly from the GAC.
                /// </summary>
                /// <param name="dwFlags">One of QUERYASMINFO_FLAG_VALIDATE or QUERYASMINFO_FLAG_GETSIZE:
                /// *_VALIDATE - Performs validation of the files in the GAC against the assembly manifest, including hash verification and strong name signature verification.
                /// *_GETSIZE - Returns the size of all files in the assembly (disk footprint). If this is not specified, the ASSEMBLY_INFO::uliAssemblySizeInKB field is not modified.</param>
                /// <param name="pszAssemblyName">Name of the assembly that is queried.</param>
                /// <param name="pAsmInfo">Pointer to the returned ASSEMBLY_INFO structure.</param>
                /// <returns></returns>
                [PreserveSig]
                void QueryAssemblyInfo(
                    uint dwFlags,
                    [MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName,
                    ref PublicDomain.Win32.Win32Structures.ASSEMBLY_INFO pAsmInfo
                );

                /// <summary>
                /// The IAssemblyCache::InstallAssembly method adds a new assembly to the GAC. The assembly must be persisted in the file system and is copied to the GAC.
                /// </summary>
                /// <param name="dwFlags">At most, one of the bits of the IASSEMBLYCACHE_INSTALL_FLAG_* values can be specified:
                /// *_REFRESH - If the assembly is already installed in the GAC and the file version numbers of the assembly being installed are the same or later, the files are replaced.
                /// *_FORCE_REFRESH - The files of an existing assembly are overwritten regardless of their version number.</param>
                /// <param name="pszManifestFilePath">A string pointing to the dynamic-linked library (DLL) that contains the assembly manifest. Other assembly files must reside in the same directory as the DLL that contains the assembly manifest.</param>
                /// <param name="pRefData">A pointer to a FUSION_INSTALL_REFERENCE that indicates the application on whose behalf the assembly is being installed. Although this is not recommended, this parameter can be null, but this leaves the assembly without any application reference.</param>
                /// <returns></returns>
                [PreserveSig]
                void InstallAssembly(
                    PublicDomain.Win32.Win32Enums.IASSEMBLYCACHE_INSTALL_FLAG dwFlags,
                    [MarshalAs(UnmanagedType.LPWStr)] string pszManifestFilePath,
                    [MarshalAs(UnmanagedType.LPArray)] PublicDomain.Win32.Win32Structures.FUSION_INSTALL_REFERENCE[] pRefData
                );
            }

            /// <summary>
            /// The IAssemblyName interface represents an assembly name. An assembly name includes a predetermined set of name-value pairs. The assembly name is described in detail in the .NET Framework SDK.
            /// </summary>
            [ComImport, Guid("CD193BC0-B4BC-11D2-9833-00C04FC31D2E"),
                InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IAssemblyName
            {
                /// <summary>
                /// The IAssemblyName::SetProperty method adds a name-value pair to the assembly name, or, if a name-value pair with the same name already exists, modifies or deletes the value of a name-value pair.
                /// </summary>
                /// <param name="PropertyId">The ID that represents the name part of the name-value pair that is to be added or to be modified. Valid property IDs are defined in the ASM_NAME enumeration.</param>
                /// <param name="pvProperty">A pointer to a buffer that contains the value of the property.</param>
                /// <param name="cbProperty">The length of the pvProperty buffer in bytes. If cbProperty is zero, the name-value pair is removed from the assembly name.</param>
                /// <returns></returns>
                [PreserveSig]
                int SetProperty(PublicDomain.Win32.Win32Enums.ASM_NAME PropertyId, IntPtr pvProperty, uint cbProperty);

                /// <summary>
                /// The IAssemblyName::GetProperty method retrieves the value of a name-value pair in the assembly name that specifies the name.
                /// </summary>
                /// <param name="PropertyId">The ID that represents the name of the name-value pair whose value is to be retrieved. Specified property IDs are defined in the ASM_NAME enumeration.</param>
                /// <param name="pvProperty">A pointer to a buffer that is to contain the value of the property.</param>
                /// <param name="pcbProperty">The length of the pvProperty buffer, in bytes.</param>
                /// <returns></returns>
                [PreserveSig]
                int GetProperty(PublicDomain.Win32.Win32Enums.ASM_NAME PropertyId, IntPtr pvProperty, ref uint pcbProperty);

                /// <summary>
                /// The IAssemblyName::Finalize method freezes an assembly name. Additional calls to IAssemblyName::SetProperty are unsuccessful after this method has been called.
                /// </summary>
                /// <returns></returns>
                [PreserveSig]
                int Finalize();

                /// <summary>
                /// The IAssemblyName::GetDisplayName method returns a string representation of the assembly name.
                /// </summary>
                /// <param name="szDisplayName">A pointer to a buffer that is to contain the display name. The display name is returned in Unicode.</param>
                /// <param name="pccDisplayName">The size of the buffer in characters (on input). The length of the returned display name (on return).</param>
                /// <param name="dwDisplayFlags">One or more of the bits defined in the ASM_DISPLAY_FLAGS enumeration</param>
                /// <returns></returns>
                [PreserveSig]
                int GetDisplayName(
                    [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szDisplayName,
                    ref uint pccDisplayName,
                    PublicDomain.Win32.Win32Enums.ASM_DISPLAY_FLAGS dwDisplayFlags
                );

                /// <summary>
                /// The IAssemblyName::GetName method returns the name part of the assembly name.
                /// </summary>
                /// <param name="lpcwBuffer">Size of the pwszName buffer (on input). Length of the name (on return).</param>
                /// <param name="pwszName">Pointer to the buffer that is to contain the name part of the assembly name.</param>
                /// <returns></returns>
                [PreserveSig]
                int GetName(
                    ref uint lpcwBuffer,
                    [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszName
                );

                /// <summary>
                /// The IAssemblyName::GetVersion method returns the version part of the assembly name.
                /// </summary>
                /// <param name="pdwVersionHi">Pointer to a DWORD that contains the upper 32 bits of the version number.</param>
                /// <param name="pdwVersionLow">Pointer to a DWORD that contain the lower 32 bits of the version number.</param>
                /// <returns></returns>
                [PreserveSig]
                int GetVersion(out uint pdwVersionHi, out uint pdwVersionLow);

                /// <summary>
                /// The IAssemblyName::IsEqual method compares the assembly name to another assembly names.
                /// </summary>
                /// <param name="pName">The assembly name to compare to.</param>
                /// <param name="dwCmpFlags">Indicates which part of the assembly name to use in the comparison.</param>
                /// <returns>S_OK: - The names match according to the comparison criteria.
                /// S_FALSE: - The names do not match.</returns>
                [PreserveSig]
                int IsEqual(IAssemblyName pName, PublicDomain.Win32.Win32Enums.ASM_CMP_FLAGS dwCmpFlags);

                /// <summary>
                /// The IAssemblyName::Clone method creates a copy of an assembly name.
                /// </summary>
                /// <param name="pName">New instance</param>
                /// <returns></returns>
                [PreserveSig]
                int Clone(out IAssemblyName pName);
            }

            /// <summary>
            /// The IAssemblyEnum interface enumerates the assemblies in the GAC.
            /// </summary>
            [ComImport, Guid("21B8916C-F28E-11D2-A473-00C04F8EF448"),
                InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IAssemblyEnum
            {
                /// <summary>
                /// The IAssemblyEnum::GetNextAssembly method enumerates the assemblies in the GAC.
                /// </summary>
                /// <param name="pvReserved">Must be null.</param>
                /// <param name="ppName">Pointer to a memory location that is to receive the interface pointer to the assembly name of the next assembly that is enumerated.</param>
                /// <param name="dwFlags">Must be zero.</param>
                /// <returns></returns>
                [PreserveSig]
                int GetNextAssembly(IntPtr pvReserved, out IAssemblyName ppName, uint dwFlags);

                /// <summary>
                /// Resets this instance.
                /// </summary>
                /// <returns></returns>
                [PreserveSig]
                int Reset();
            }

            /// <summary>
            /// The IInstallReferenceItem interface represents a reference that has been set on an assembly in the GAC. Instances of IInstallReferenceIteam are returned by the IInstallReferenceEnum interface.
            /// </summary>
            [ComImport, Guid("582DAC66-E678-449F-ABA6-6FAAEC8A9394"),
                InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IInstallReferenceItem
            {
                /// <summary>
                /// The IInstallReferenceItem::GetReference method returns a FUSION_INSTALL_REFERENCE structure.
                /// </summary>
                /// <param name="ppRefData">A pointer to a FUSION_INSTALL_REFERENCE structure. The memory is allocated by the GetReference method and is freed when IInstallReferenceItem is released. Callers must not hold a reference to this buffer after the IInstallReferenceItem object is released.</param>
                /// <param name="dwFlags">Must be zero.</param>
                /// <param name="pvReserved">Must be null.</param>
                /// <returns></returns>
                [PreserveSig]
                int GetReference(
                    [MarshalAs(UnmanagedType.LPArray)] out PublicDomain.Win32.Win32Structures.FUSION_INSTALL_REFERENCE[] ppRefData,
                    uint dwFlags,
                    IntPtr pvReserved
                );
            }

            /// <summary>
            /// The IInstallReferenceEnum interface enumerates all references that are set on an assembly in the GAC.
            /// NOTE: References that belong to the assembly are locked for changes while those references are being enumerated.
            /// </summary>
            [ComImport, Guid("56B1A988-7C0C-4AA2-8639-C3EB5A90226F"),
                InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IInstallReferenceEnum
            {
                /// <summary>
                /// IInstallReferenceEnum::GetNextInstallReferenceItem returns the next reference information for an assembly.
                /// </summary>
                /// <param name="ppRefItem">Pointer to a memory location that receives the IInstallReferenceItem pointer.</param>
                /// <param name="dwFlags">Must be zero.</param>
                /// <param name="pvReserved">Must be null.</param>
                /// <returns>S_OK: - The next item is returned successfully.
                /// S_FALSE: - No more items.</returns>
                [PreserveSig()]
                int GetNextInstallReferenceItem(
                    out IInstallReferenceItem ppRefItem,
                    uint dwFlags,
                    IntPtr pvReserved
                );
            }
        }

        /// <summary>
        /// Gets the last error.
        /// </summary>
        /// <returns></returns>
        public static int GetLastError()
        {
            return Marshal.GetLastWin32Error();
        }

        /// <summary>
        /// Gets the last error throw.
        /// </summary>
        public static void GetLastErrorThrow()
        {
            throw new Win32Exception(GetLastError());
        }
    }
}
