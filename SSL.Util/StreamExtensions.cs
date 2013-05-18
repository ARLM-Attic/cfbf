using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSL.Util
{
    public static class StreamExtensions
    {
        /// <summary>
        /// this is a helper to read all bytes from a stream.
        /// This method dosen't close the stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(this Stream stream)
        {
            if (stream != null && stream.Length >0)
            {
                stream.Position = 0;
                var buff = new byte[stream.Length];
                stream.Read(buff, 0, (int)stream.Length-1);

                return buff;
            }
            else
                return null;
        }

        /// <summary>
        /// Read bytes from a stream
        /// </summary>
        /// <param name="stream">Stream object from where to read bytes</param>
        /// <param name="offset">offset from where to begin reading bytes</param>
        /// <param name="count"></param>
        /// <param name="resetStreamPosition">Restore stream position to value before reading</param>
        /// <returns>return a byte array read from stream</returns>
        public static byte[] ReadBytes(this Stream stream, int offset, int count, bool resetStreamPosition)
        {
            byte[] buffer = null;

            //save the stream position
            long position = stream.Position;

            if (stream.CanRead)
            {
                buffer = new byte[count];
                stream.Read(buffer, offset, count);

                if (resetStreamPosition)
                    stream.Position = position;
            }

            return buffer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] ReadBytes(this Stream stream, int size)
        {
            byte[] buffer = null;

            if (stream.CanRead)
            {
                buffer = new byte[size];
                stream.Read(buffer, 0, size);
            }

            return buffer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static byte[] ReadBytes(this byte[] buffer, int offset, int count)
        {
            var byteStream = new MemoryStream(buffer, false);
            byteStream.Position = offset;

            var bytes = byteStream.ReadBytes(count);
            byteStream.Close();
            byteStream.Dispose();

            return bytes;
        }

    }
}
