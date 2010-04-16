using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public static class VersionUtilities
    {
        /// <summary>
        /// There cannot be a min version.
        /// </summary>
        public static readonly Version MaxVersion;

        /// <summary>
        /// 
        /// </summary>
        public static readonly Version ZeroVersion;

        static VersionUtilities()
        {
            MaxVersion = new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
            ZeroVersion = new Version(0, 0, 0, 0);
        }

        /// <summary>
        /// Adds the major.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="majorAmount">The major amount.</param>
        /// <returns></returns>
        public static Version AddMajor(Version version, int majorAmount)
        {
            return new Version(version.Major + majorAmount, version.Minor, version.Build, version.Revision);
        }

        /// <summary>
        /// Adds the minor.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="minorAmount">The minor amount.</param>
        /// <returns></returns>
        public static Version AddMinor(Version version, int minorAmount)
        {
            return new Version(version.Major, version.Minor + minorAmount, version.Build, version.Revision);
        }

        /// <summary>
        /// Creates a new instance of <see cref="System.Version"/>, adding
        /// <paramref name="buildAmount"/> to the <see cref="System.Version.Build"/>
        /// portion of the version, the third portion.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="buildAmount">The build amount.</param>
        /// <returns></returns>
        public static Version AddBuild(Version version, int buildAmount)
        {
            return new Version(version.Major, version.Minor, version.Build + buildAmount, version.Revision);
        }

        /// <summary>
        /// Creates a new instance of <see cref="System.Version"/>, adding
        /// <paramref name="revisionAmount"/> to the <see cref="System.Version.Revision"/>
        /// portion of the version, the fourth portion.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="revisionAmount">The revision amount.</param>
        /// <returns></returns>
        public static Version AddRevision(Version version, int revisionAmount)
        {
            return new Version(version.Major, version.Minor, version.Build, version.Revision + revisionAmount);
        }

        /// <summary>
        /// Determines whether [is non zero] [the specified version].
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>
        /// 	<c>true</c> if [is non zero] [the specified version]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNonZero(Version version)
        {
            return version.Major > 0 || version.Minor > 0 || version.Revision > 0 || version.Build > 0;
        }

        /// <summary>
        /// Tries to extract a version from the beginning of <paramref name="str"/>.
        /// Discards anything that follows. Returns null if <paramref name="str"/>
        /// does not begin with an integer.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static Version ParseFirstVersion(string str)
        {
            bool found = false;
            int major = 0, minor = 0, build = 0, revision = 0, step = 0;

            if (!string.IsNullOrEmpty(str) && char.IsDigit(str[0]))
            {
                found = true;
                foreach (char c in str)
                {
                    if (char.IsDigit(c))
                    {
                        switch (step)
                        {
                            case 0:
                                major = (major * 10) + int.Parse(c.ToString());
                                break;
                            case 1:
                                minor = (minor * 10) + int.Parse(c.ToString());
                                break;
                            case 2:
                                build = (build * 10) + int.Parse(c.ToString());
                                break;
                            case 3:
                                revision = (revision * 10) + int.Parse(c.ToString());
                                break;
                            default:
                                break;
                        }
                    }
                    else if (c == '.')
                    {
                        step++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return found ? new Version(major, minor, build, revision) : null;
        }

        /// <summary>
        /// Ares the equal.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static bool AreEqual(Version v1, Version v2)
        {
            return (v1.Major == v2.Major || (v1.Major == -1 && v2.Major == 0) || (v2.Major == -1 && v1.Major == 0)) &&
                (v1.Minor == v2.Minor || (v1.Minor == -1 && v2.Minor == 0) || (v2.Minor == -1 && v1.Minor == 0)) &&
            (v1.Build == v2.Build || (v1.Build == -1 && v2.Build == 0) || (v2.Build == -1 && v1.Build == 0)) &&
            (v1.Revision == v2.Revision || (v1.Revision == -1 && v2.Revision == 0) || (v2.Revision == -1 && v1.Revision == 0));
        }
    }
}
