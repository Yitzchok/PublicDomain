using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
#if !(NOVJSLIB)
    /// <summary>
    /// Callback that gets called after a zip file entry is read.
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="zipEntryStream"></param>
    /// <param name="rock">Arbitrary data</param>
    public delegate void CallbackZip(ZipEntryE entry, ZipEntryInputStream zipEntryStream, object rock);

    /// <summary>
    /// Provides methods for manipulating archive files, such as ZIPs.
    /// </summary>
    public static class Archiver
    {
        /// <summary>
        /// Reads the zip stream.
        /// </summary>
        /// <param name="jInputStream">The j input stream.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="rock">The rock.</param>
        public static void ReadZipStream(java.io.InputStream jInputStream, CallbackZip callback, object rock)
        {
            ZipInputStream zis = null;
            try
            {
                zis = new ZipInputStream(jInputStream);

                ZipEntry entry;
                ZipEntryE extendedEntry;
                while ((entry = zis.getNextEntry()) != null)
                {
                    extendedEntry = new ZipEntryE(entry);
                    callback(extendedEntry, new ZipEntryInputStream(zis), rock);

                    // Close the entry that we read
                    zis.closeEntry();
                }
            }
            finally
            {
                if (zis != null)
                {
                    zis.close();
                }
            }
        }

        /// <summary>
        /// Extracts the specified zip file path.
        /// </summary>
        /// <param name="zipFilePath">The zip file path.</param>
        /// <param name="destinationDirectory">The destination directory.</param>
        public static void Extract(string zipFilePath, string destinationDirectory)
        {
            Extract(zipFilePath, destinationDirectory, false);
        }

        /// <summary>
        /// Extracts the specified zip file path.
        /// </summary>
        /// <param name="zipFilePath">The zip file path.</param>
        /// <param name="destinationDirectory">The destination directory.</param>
        /// <param name="truncatePaths">if set to <c>true</c> [truncate paths].</param>
        public static void Extract(string zipFilePath, string destinationDirectory, bool truncatePaths)
        {
            using (FileStream stream = new FileStream(zipFilePath, FileMode.Open))
            {
                using (JStream jstream = new JStream(stream))
                {
                    Extract(jstream, destinationDirectory, truncatePaths);
                }
            }
        }

        /// <summary>
        /// Extracts the specified j input stream.
        /// </summary>
        /// <param name="jInputStream">The j input stream.</param>
        /// <param name="destinationDirectory">The destination directory.</param>
        /// <param name="truncatePaths">if set to <c>true</c> [truncate paths].</param>
        public static void Extract(java.io.InputStream jInputStream, string destinationDirectory, bool truncatePaths)
        {
            if (string.IsNullOrEmpty(destinationDirectory))
            {
                throw new ArgumentException("destinationDirectory");
            }
            destinationDirectory = FileSystemUtilities.EnsureDirectoryEnding(destinationDirectory);
            ReadZipStream(jInputStream, delegate(ZipEntryE entry, ZipEntryInputStream zipEntryStream, object rock)
            {
                zipEntryStream.WriteTo(destinationDirectory + entry.getName(truncatePaths));
            }, null);
        }
    }

    /// <summary>
    /// Wrapper class around ZipEntry to provide some convenience
    /// methods.
    /// </summary>
    public class ZipEntryE : ZipEntry
    {
        /// <summary>
        /// 
        /// </summary>
        protected ZipEntry m_entry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipEntryE"/> class.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public ZipEntryE(ZipEntry entry)
            : base(entry.getName())
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            m_entry = entry;
        }

        /// <summary>
        /// Gets the comment.
        /// </summary>
        /// <returns></returns>
        public override string getComment()
        {
            return m_entry.getComment();
        }

        /// <summary>
        /// Gets the size of the compressed.
        /// </summary>
        /// <returns></returns>
        public override long getCompressedSize()
        {
            return m_entry.getCompressedSize();
        }

        /// <summary>
        /// Gets the CRC.
        /// </summary>
        /// <returns></returns>
        public override long getCrc()
        {
            return m_entry.getCrc();
        }

        /// <summary>
        /// Gets the extra.
        /// </summary>
        /// <returns></returns>
        public override sbyte[] getExtra()
        {
            return m_entry.getExtra();
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <returns></returns>
        public override int getMethod()
        {
            return m_entry.getMethod();
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return m_entry.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return m_entry.GetHashCode();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns></returns>
        public override string getName()
        {
            return getName(false);
        }

        /// <summary>
        /// Extension method which provides the option
        /// of truncating any path information in the file
        /// </summary>
        /// <param name="truncatePath"></param>
        /// <returns></returns>
        public virtual string getName(bool truncatePath)
        {
            string result = m_entry.getName();
            if (result != null)
            {
                result = result.Replace('/', '\\');
                if (truncatePath)
                {
                    int lastSlashIndex = result.LastIndexOf('\\');
                    if (lastSlashIndex != -1)
                    {
                        result = result.Substring(lastSlashIndex + 1);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <returns></returns>
        public override long getSize()
        {
            return m_entry.getSize();
        }

        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <returns></returns>
        public override long getTime()
        {
            return m_entry.getTime();
        }

        /// <summary>
        /// Determines whether this instance is directory.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is directory; otherwise, <c>false</c>.
        /// </returns>
        public override bool isDirectory()
        {
            return m_entry.isDirectory();
        }

        /// <summary>
        /// Sets the comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        public override void setComment(string comment)
        {
            m_entry.setComment(comment);
        }

        /// <summary>
        /// Sets the CRC.
        /// </summary>
        /// <param name="crc">The CRC.</param>
        public override void setCrc(long crc)
        {
            m_entry.setCrc(crc);
        }

        /// <summary>
        /// Sets the extra.
        /// </summary>
        /// <param name="extra">The extra.</param>
        public override void setExtra(sbyte[] extra)
        {
            m_entry.setExtra(extra);
        }

        /// <summary>
        /// Sets the method.
        /// </summary>
        /// <param name="m">The m.</param>
        public override void setMethod(int m)
        {
            m_entry.setMethod(m);
        }

        /// <summary>
        /// Sets the size.
        /// </summary>
        /// <param name="sz">The sz.</param>
        public override void setSize(long sz)
        {
            m_entry.setSize(sz);
        }

        /// <summary>
        /// Sets the time.
        /// </summary>
        /// <param name="t">The t.</param>
        public override void setTime(long t)
        {
            m_entry.setTime(t);
        }

        /// <summary>
        /// Toes the string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_entry.ToString();
        }
    }

    /// <summary>
    /// Abstraction over a <see cref="java.util.zip.ZipInputStream"/>
    /// </summary>
    public class ZipEntryInputStream : JInputStream
    {
        /// <summary>
        /// 
        /// </summary>
        protected ZipInputStream m_zis;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipEntryInputStream"/> class.
        /// </summary>
        /// <param name="zis">The zis.</param>
        public ZipEntryInputStream(ZipInputStream zis)
            : base(zis)
        {
            if (zis == null)
            {
                throw new ArgumentNullException("zis");
            }
            m_zis = zis;
        }

        /// <summary>
        /// Disposes the specified disposing.
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> [disposing].</param>
        protected override void Dispose(bool disposing)
        {
            // We don't want to close the underlying ZipInputStream,
            // so we just do nothing here.
        }

        /// <summary>
        /// Convenience method which writes the contents of the ZipEntry
        /// to the specified file and returns the number of bytes written.
        /// <c>FileMode.Create</c> is used when opening the file for writing.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public int WriteTo(string file)
        {
            using (FileStream fileStream = new FileStream(file, FileMode.Create))
            {
                return WriteTo(fileStream);
            }
        }

        /// <summary>
        /// Convenience method which writes the contents of the ZipEntry
        /// to the specified stream and returns the number of bytes written.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public int WriteTo(Stream stream)
        {
            byte[] buffer = new byte[GlobalConstants.DefaultStreamBlockSize];
            int bytesRead, totalBytesRead = 0;
            while ((bytesRead = Read(buffer, 0, GlobalConstants.DefaultStreamBlockSize)) > -1)
            {
                stream.Write(buffer, 0, bytesRead);
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }

    /// <summary>
    /// This class allows one to transparently pass a .NET System.IO.Stream
    /// class where a java.io.InputStream class is expected.
    /// </summary>
    public class JStream : java.io.InputStream, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected Stream m_stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="JStream"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public JStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            m_stream = stream;
        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        public override int read()
        {
            return m_stream.ReadByte();
        }

        /// <summary>
        /// Availables this instance.
        /// </summary>
        /// <returns></returns>
        public override int available()
        {
            return unchecked((int)(m_stream.Length - m_stream.Position));
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public override void close()
        {
            m_stream.Close();
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return m_stream.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return m_stream.GetHashCode();
        }

        /// <summary>
        /// Marks the specified readlimit.
        /// </summary>
        /// <param name="readlimit">The readlimit.</param>
        public override void mark(int readlimit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Marks the supported.
        /// </summary>
        /// <returns></returns>
        public override bool markSupported()
        {
            return false;
        }

        /// <summary>
        /// Reads the specified b.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public override int read(sbyte[] b)
        {
            return read(b, 0, b.Length);
        }

        /// <summary>
        /// Reads the specified b.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="off">The off.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public override int read(sbyte[] b, int off, int count)
        {
            if (b == null)
            {
                throw new NullReferenceException();
            }

            // Unfortunately, bytes are different, so we need to read
            // data into memory first
            byte[] data = new byte[count];
            int bytesRead = m_stream.Read(data, 0, count);
            if (bytesRead > 0)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    b[off + i] = (sbyte)data[i];
                }
            }
            return bytesRead;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public override void reset()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Skips the specified count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public override long skip(long count)
        {
            return m_stream.Seek(count, SeekOrigin.Current);
        }

        /// <summary>
        /// Toes the string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_stream.ToString();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // We assume the caller is managing the stream passed in, so we
            // won't actually close it
        }
    }

    /// <summary>
    /// This class allows one to transparently pass a java.io.InputStream class where
    /// a .NET System.IO.Stream class is expected.
    /// </summary>
    public class JInputStream : Stream
    {
        /// <summary>
        /// 
        /// </summary>
        protected java.io.InputStream m_jis;

        /// <summary>
        /// Initializes a new instance of the <see cref="JInputStream"/> class.
        /// </summary>
        /// <param name="javaInputStream">The java input stream.</param>
        public JInputStream(java.io.InputStream javaInputStream)
        {
            if (javaInputStream == null)
            {
                throw new ArgumentNullException("javaInputStream");
            }
            m_jis = javaInputStream;
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports reading; otherwise, false.</returns>
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports seeking; otherwise, false.</returns>
        public override bool CanSeek
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports writing; otherwise, false.</returns>
        public override bool CanWrite
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void Flush()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <value></value>
        /// <returns>A long value representing the length of the stream in bytes.</returns>
        /// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Length
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <value></value>
        /// <returns>The current position within the stream.</returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Position
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The sum of offset and count is larger than the buffer length. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
        /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (count < 0)
            {
                throw new ArgumentException("count");
            }
            sbyte[] b = new sbyte[count];
            int bytesRead = m_jis.read(b, 0, count);
            if (bytesRead > 0)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    buffer[offset + i] = (byte)b[i];
                }
            }
            return bytesRead;
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"></see> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
        /// <exception cref="T:System.ArgumentException">The sum of offset and count is greater than the buffer length. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream.
        /// </summary>
        public override void Close()
        {
            m_jis.close();
            base.Close();
        }

        /// <summary>
        /// Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.
        /// </summary>
        /// <returns>
        /// The unsigned byte cast to an Int32, or -1 if at the end of the stream.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override int ReadByte()
        {
            return m_jis.read();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream"></see> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            m_jis.close();
        }
    }
#endif
}
