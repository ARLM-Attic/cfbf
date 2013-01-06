using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.CFBF
{
    public static class StreamHelper
    {

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
    }
}
