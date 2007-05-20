using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PublicDomain
{
    /// <summary>
    /// Methods to work with generic objects, such as serializing and deserializing
    /// them to byte arrays or memory streams.
    /// </summary>
    public static class ObjectUtilities
    {
        /// <summary>
        /// Serializes the object to binary stream.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static MemoryStream SerializeObjectToBinaryStream(object o)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(ms, o);
                return ms;
            }
        }

        /// <summary>
        /// Serializes the object to binary.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static byte[] SerializeObjectToBinary(object o)
        {
            return SerializeObjectToBinaryStream(o).GetBuffer();
        }

        /// <summary>
        /// Deserializes the object from binary.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static T DeserializeObjectFromBinary<T>(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryFormatter b = new BinaryFormatter();
                return (T)b.Deserialize(ms);
            }
        }

        /// <summary>
        /// Sorts the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        public static void Sort<T>(T x, T y, out T x2, out T y2) where T : IComparable<T>
        {
            x2 = x;
            y2 = y;
            if (x.CompareTo(y) > 0)
            {
                x2 = y;
                y2 = x;
            }
        }
    }
}
