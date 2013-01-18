using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.CFBF
{
    public static class ByteArrayExtension
    {

        public static uint[] ToUInt32(this byte[] buffer)
        {
            if (buffer.Length % 4 != 0)
                throw new Exception();

            List<uint> retArray = new List<uint>();
            MemoryStream byteStream = null;

            try
            {
                byteStream = new MemoryStream(buffer, false);

                for (int i = 0; i < byteStream.Length / 4; i++)
                {
                    var bytes = byteStream.ReadBytes(4);
                    uint value = BitConverter.ToUInt32(bytes, 0);
                    retArray.Add(value);
                }

            }
            finally
            {
                if (byteStream != null)
                {
                    byteStream.Close();
                    byteStream.Dispose();
                }
            }

            return retArray.ToArray();
        }

        public static ulong[] ToUInt64(this byte[] buffer)
        {
            if (buffer.Length % 8 != 0)
                throw new Exception();

            List<ulong> retArray = new List<ulong>();
            MemoryStream byteStream = null;

            try
            {
                byteStream = new MemoryStream(buffer, false);

                for (int i = 0; i < byteStream.Length / 8; i++)
                {
                    var bytes = byteStream.ReadBytes(8);
                    ulong value = BitConverter.ToUInt64(bytes, 0);
                    retArray.Add(value);
                }

            }
            finally
            {
                if (byteStream != null)
                {
                    byteStream.Close();
                    byteStream.Dispose();
                }
            }

            return retArray.ToArray();
        }

        public static uint[] ToUInt32(this byte[] buffer, params uint[] ignoreValues)
        {
            if (buffer.Length % 4 != 0)
                throw new Exception();

            List<uint> retArray = new List<uint>();
            MemoryStream byteStream = null;

            try
            {
                byteStream = new MemoryStream(buffer, false);
                for (int i = 0; i < byteStream.Length / 4; i++) {
                    var bytes = byteStream.ReadBytes(4);
                    uint value = BitConverter.ToUInt32(bytes, 0);

                    if (!ignoreValues.Contains(value))
                        retArray.Add(value);
                }
            }
            finally
            {
                if (byteStream != null)
                {
                    byteStream.Close();
                    byteStream.Dispose();
                }
            }
            return retArray.ToArray();
        }

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
