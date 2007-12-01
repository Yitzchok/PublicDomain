using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PublicDomain
{
    /// <summary>
    /// Methods to help in file system related manipulations.
    /// TODO Directory.Copy()
    /// </summary>
    public static class FileSystemUtilities
    {
        /// <summary>
        /// / and \
        /// </summary>
        public static readonly char[] TrackbackChars = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        /// <summary>
        /// file:///
        /// </summary>
        public static readonly string FileUriPrefix;

        static FileSystemUtilities()
        {
            FileUriPrefix = "file" + Uri.SchemeDelimiter + "/";
        }

        /// <summary>
        /// Ensures the directory ending ref.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public static void EnsureDirectoryEndingRef(ref string directory)
        {
            EnsureDirectoryEndingRef(ref directory, Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Ensures the directory ending.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="appendChar">The append char.</param>
        public static void EnsureDirectoryEndingRef(ref string directory, char appendChar)
        {
            if (directory != null)
            {
                char lastChar = directory[directory.Length - 1];
                if (lastChar != Path.DirectorySeparatorChar && lastChar != Path.AltDirectorySeparatorChar)
                {
                    directory += appendChar;
                }
            }
        }

        /// <summary>
        /// Ensures the directory ending.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns></returns>
        public static string EnsureDirectoryEnding(string directory)
        {
            EnsureDirectoryEndingRef(ref directory);
            return directory;
        }

        /// <summary>
        /// Ensures the directory ending.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="appendChar">The append char.</param>
        /// <returns></returns>
        public static string EnsureDirectoryEnding(string directory, char appendChar)
        {
            EnsureDirectoryEndingRef(ref directory, appendChar);
            return directory;
        }

        /// <summary>
        /// Combines the two paths, making sure no two slashes are combined.
        /// </summary>
        /// <param name="path1">The path1.</param>
        /// <param name="path2">The path2.</param>
        /// <returns></returns>
        public static string PathCombine(string path1, string path2)
        {
            if (string.IsNullOrEmpty(path2))
            {
                return path1;
            }

            if (path2[0] == Path.DirectorySeparatorChar || path2[0] == Path.AltDirectorySeparatorChar)
            {
                path2 = path2.Substring(1);
            }

            return Path.Combine(path1, path2);
        }

        /// <summary>
        /// Checks whether the specified <paramref name="path"/>
        /// points to an existing file system object such as a
        /// directory or a file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static bool DoesFileSystemObjectExist(string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }

        /// <summary>
        /// Moves the file system object from <paramref name="fromPath"/>
        /// to <paramref name="toPath"/> whether the object is a file
        /// or a directory.
        /// </summary>
        /// <param name="fromPath">From path.</param>
        /// <param name="toPath">To path.</param>
        /// <returns></returns>
        public static bool MoveFileSystemObject(string fromPath, string toPath)
        {
            if (File.Exists(fromPath))
            {
                File.Move(fromPath, toPath);
                return true;
            }
            if (Directory.Exists(fromPath))
            {
                Directory.Move(fromPath, toPath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes the file system object, whether it is a file or
        /// a directory. If it is a directory, the directory is
        /// recursively deleted.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static bool DeleteFileSystemObject(string path)
        {
            return DeleteFileSystemObject(path, true);
        }

        /// <summary>
        /// Deletes the file system object, whether it is a file or
        /// a directory. If it is a directory, <paramref name="recurseIfDirectory"/>
        /// dictates whether the delete is recursive.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="recurseIfDirectory">if set to <c>true</c> [recurse if directory].</param>
        /// <returns></returns>
        public static bool DeleteFileSystemObject(string path, bool recurseIfDirectory)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            if (Directory.Exists(path))
            {
                if (recurseIfDirectory)
                {
                    DeleteDirectoryForcefully(path);
                }
                else
                {
                    Directory.Delete(path, false);
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the temporary directory.
        /// </summary>
        /// <returns></returns>
        public static string GetTemporaryDirectory()
        {
            string ret = PathCombine(Path.GetTempPath(), "t" + new Random((int)unchecked(DateTime.Now.Ticks)).Next(1, 10000).ToString() + @"\");
            Directory.CreateDirectory(ret);
            return ret;
        }

        /// <summary>
        /// Gets the location of a new temporary file name with the given
        /// extension. Extension should not begin with a period (e.g. just html, not .html).
        /// The file is created on disk with a file size of 0. It is guaranteed
        /// that the file is a new file that did not exist before.
        /// </summary>
        /// <param name="extension">The preferred file extension. Extension should not begin with a period (e.g. just html, not .html).</param>
        /// <returns>Location of the 0-byte file in a temporary location with the specified extension.</returns>
        public static string GetTempFileName(string extension)
        {
            return GetTempFileName(extension, null);
        }

        /// <summary>
        /// Gets the location of a new temporary file name with the given file name and
        /// extension. Extension should not begin with a period (e.g. just html, not .html).
        /// File name should not end with a period and should not contain the extension
        /// (as that is in the extension parameter).
        /// The file is created on disk with a file size of 0. It is guaranteed
        /// that the file is a new file that did not exist before.
        /// </summary>
        /// <param name="extension">The preferred file extension. Extension should not begin with a period (e.g. just html, not .html).</param>
        /// <param name="fileName">The preferred name of the file, without a trailing period, and without an extension (as that is specified by the extension parameter).</param>
        /// <returns>Location of the 0-byte file in a temporary location with the specified extension and name.</returns>
        public static string GetTempFileName(string extension, string fileName)
        {
            string tempFile = Path.GetTempFileName();

            // Find the last slash
            int lastSlash = tempFile.LastIndexOf(Path.DirectorySeparatorChar);

            string filePart = tempFile.Substring(lastSlash + 1);

            tempFile = tempFile.Substring(0, lastSlash + 1);

            // Split the file name into extension and name
            int periodIndex = filePart.LastIndexOf('.');

            string preName = null;

            if (periodIndex != -1)
            {
                preName = filePart.Substring(0, periodIndex);
            }

            if (fileName == null)
            {
                if (preName == null)
                {
                    tempFile += "t" + new Random((int)unchecked(DateTime.Now.Ticks)).Next(1, 10000).ToString();
                }
                else
                {
                    tempFile += preName;
                }
            }
            else
            {
                tempFile += fileName;
            }

            tempFile += "." + extension;

            return tempFile;
        }

        /// <summary>
        /// Deletes the directory forcefully.
        /// </summary>
        /// <param name="dir">The dir.</param>
        public static void DeleteDirectoryForcefully(string dir)
        {
            RemoveReadOnly(dir);

            Directory.Delete(dir, true);
        }

        /// <summary>
        /// Removes the read only.
        /// </summary>
        /// <param name="path">The path.</param>
        private static void RemoveReadOnly(string path)
        {
            DirectoryInfo current = new DirectoryInfo(path);
            current.Attributes = FileAttributes.Normal;

            foreach (FileSystemInfo file in current.GetFileSystemInfos())
            {
                file.Attributes = FileAttributes.Normal;
            }

            foreach (DirectoryInfo folder in current.GetDirectories())
            {
                RemoveReadOnly(folder.FullName);
            }
        }

        /// <summary>
        /// Saves the input stream to file.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="newFile">The new file.</param>
        public static void SaveInputStreamToFile(Stream stream, string newFile)
        {
            // Now, write out the file
            int length = 256;
            byte[] buffer = new byte[length];
            using (FileStream fs = new FileStream(newFile, FileMode.Create))
            {
                int bytesRead = stream.Read(buffer, 0, length);
                // write the required bytes
                while (bytesRead > 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                    bytesRead = stream.Read(buffer, 0, length);
                }
            }
        }

        /// <summary>
        /// Saves the text reader to file.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="newFile">The new file.</param>
        public static void SaveTextReaderToFile(TextReader stream, string newFile)
        {
            // Now, write out the file
            using (StreamWriter fs = new StreamWriter(newFile))
            {
                fs.Write(stream.ReadToEnd());
            }
        }

        /// <summary>
        /// Combines the trackbacks in path.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string CombineTrackbacksInPath(string uri)
        {
            int trackbackIndex = GetTrackbackIndex(uri);
            int slashIndex;
            while (trackbackIndex != -1)
            {
                // There is a trackback starting at the index, so from
                // there we find the nearest slash, and remove that piece
                slashIndex = uri.LastIndexOfAny(TrackbackChars, trackbackIndex - 2);
                if (slashIndex == -1)
                {
                    uri = uri.Remove(0, trackbackIndex + 2);
                }
                else
                {
                    uri = uri.Remove(slashIndex, trackbackIndex - slashIndex + 2);
                }

                trackbackIndex = GetTrackbackIndex(uri);
            }
            return uri;
        }

        /// <summary>
        /// Gets the index of the trackback.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        private static int GetTrackbackIndex(string uri)
        {
            int index = uri.IndexOf("../");

            if (index == -1)
            {
                index = uri.IndexOf(@"..\");
            }

            return index;
        }

        /// <summary>
        /// Replaces all occurrences of <paramref name="search"/> with
        /// <paramref name="replace"/> in the file located at <paramref name="path"/>.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="replace">The replace.</param>
        /// <param name="path">The path.</param>
        public static void ReplaceInFile(string search, string replace, string path)
        {
            string text = File.ReadAllText(path);
            text = text.Replace(search, replace);
            File.WriteAllText(path, text);
        }

        /// <summary>
        /// Replaces all occurrences of <paramref name="search"/> with
        /// <paramref name="replace"/> in the file located at <paramref name="path"/>.
        /// Returns true if the file has been changed, false otherwise.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="replace">The replace.</param>
        /// <param name="path">The path.</param>
        /// <returns>true if the file has been changed, false otherwise.</returns>
        public static bool ReplaceInFileDiff(string search, string replace, string path)
        {
            string oldText = File.ReadAllText(path);
            string newText = oldText.Replace(search, replace);
            File.WriteAllText(path, newText);

            // First, simply check the file lengths
            if (oldText.Length != newText.Length)
            {
                return true;
            }
            else
            {
                // Now, we need to generate unique hashes for both, this can take some time
                if (!StringUtilities.ComputeNonCollidingHash(oldText).Equals(StringUtilities.ComputeNonCollidingHash(newText)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Splits <paramref name="path"/> based on the last directory
        /// separator, return a string array of length 2. The first element
        /// is the left portion, the directory, and the second element
        /// is the right portion, the file name. The directory separator is
        /// stripped from both, so the first element does not end with a trailing
        /// separator, nor does the second element begin with a directory
        /// separator. The return result is never null, and the elements
        /// are never null, so one of the elements may be an empty string.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string[] SplitFileIntoDirectoryAndName(string path)
        {
            return SplitFileIntoDirectoryAndName(path, false);
        }

        /// <summary>
        /// Splits <paramref name="path"/> based on the last directory
        /// separator, return a string array of length 2. The first element
        /// is the left portion, the directory, and the second element
        /// is the right portion, the file name. The directory separator is
        /// stripped from the second element, so the second element never begins with a directory
        /// separator. <paramref name="ensureDirectoryElementEndingSlash"/> controls
        /// whether or not the first element retains a trailing directory separator.
        /// The return result is never null, and the elements
        /// are never null, so one of the elements may be an empty string.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="ensureDirectoryElementEndingSlash">if set to <c>true</c> [ensure directory element ending slash].</param>
        /// <returns></returns>
        public static string[] SplitFileIntoDirectoryAndName(string path, bool ensureDirectoryElementEndingSlash)
        {
            string[] result = StringUtilities.SplitAround(path, path.LastIndexOfAny(TrackbackChars));

            if (ensureDirectoryElementEndingSlash)
            {
                result[0] += Path.DirectorySeparatorChar;
            }

            return result;
        }

        /// <summary>
        /// Ensures the directory in <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The full directory path that was extracted
        /// from <paramref name="path"/> which is ensured to be
        /// created if it did not already exist.</returns>
        public static string EnsureDirectoriesInPath(string path)
        {
            string[] pieces = SplitFileIntoDirectoryAndName(path, true);
            if (!Directory.Exists(pieces[0]))
            {
                Directory.CreateDirectory(pieces[0]);
            }
            return pieces[0];
        }

        /// <summary>
        /// Gets the file URI as string.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetFileUriAsString(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return FileUriPrefix + path.Trim().Replace(Path.DirectorySeparatorChar, '/');
        }

        /// <summary>
        /// Gets the file URI.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static Uri GetFileUri(string path)
        {
            return new Uri(GetFileUriAsString(path));
        }

        /// <summary>
        /// Gets the path from URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string GetPathFromUri(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            string str = uri.ToString();

            if (!str.StartsWith(FileUriPrefix))
            {
                throw new ArgumentException("The specified uri is not a file system url (" + uri + ")");
            }

            str = str.Substring(FileUriPrefix.Length).Replace('/', Path.DirectorySeparatorChar);

            return str;
        }

        /// <summary>
        /// Gets the extension in lower case, without a period in the beginning
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string GetExtension(string uri)
        {
            return GetExtension(uri, true);
        }

        /// <summary>
        /// Gets the extension in lower case, without a period in the beginning
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="lowerCase">if set to <c>true</c> [lower case].</param>
        /// <returns></returns>
        public static string GetExtension(string uri, bool lowerCase)
        {
            return GetExtension(uri, lowerCase, true);
        }

        /// <summary>
        /// Gets the extension, without a period in the beginning
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="lowerCase">if set to <c>true</c> [lower case].</param>
        /// <param name="allowCompoundExtensions">if set to <c>true</c> [allow compound extensions].</param>
        /// <returns></returns>
        public static string GetExtension(string uri, bool lowerCase, bool allowCompoundExtensions)
        {
            string result = null;

            if (!string.IsNullOrEmpty(uri))
            {
                uri = uri.Trim();

                string[] pieces = uri.Split(FileSystemUtilities.TrackbackChars);
                if (pieces.Length > 0)
                {
                    result = pieces[pieces.Length - 1];
                    if (!string.IsNullOrEmpty(result))
                    {
                        int periodIndex = allowCompoundExtensions ? result.IndexOf('.') : result.LastIndexOf('.');
                        if (periodIndex != -1)
                        {
                            result = result.Substring(periodIndex + 1);
                            if (!string.IsNullOrEmpty(result))
                            {
                                if (lowerCase)
                                {
                                    result = result.ToLower();
                                }
                            }
                        }
                    }
                }
            }

            return string.IsNullOrEmpty(result) ? null : result;
        }

        /// <summary>
        /// Replaces the extension. <paramref name="newExtension"/> does not
        /// begin with a period.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="newExtension">The new extension.</param>
        /// <returns></returns>
        public static string ReplaceExtension(string path, string newExtension)
        {
            string existingExtension = GetExtension(path, false, true);
            path = path.Substring(0, path.Length - existingExtension.Length) + newExtension;
            return path;
        }

        /// <summary>
        /// Gets the relative location.
        /// </summary>
        /// <param name="fromPath">From path.</param>
        /// <param name="toPath">To path.</param>
        /// <returns></returns>
        public static string GetRelativeLocation(string fromPath, string toPath)
        {
            if (System.IO.File.Exists(fromPath))
            {
                fromPath = new FileInfo(fromPath).DirectoryName;
            }
            if (Directory.Exists(fromPath))
            {
                fromPath = FileSystemUtilities.EnsureDirectoryEnding(fromPath);
            }
            if (Directory.Exists(toPath))
            {
                toPath = FileSystemUtilities.EnsureDirectoryEnding(toPath);
            }

            if (toPath.StartsWith(fromPath))
            {
                return toPath.Substring(fromPath.Length);
            }

            throw new NotImplementedException();
        }
    }
}
