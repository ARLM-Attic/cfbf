using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSL.Util
{
    public static class IntegerExtensions
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

        public static uint[] ToUInt32(this byte[] buffer, params uint[] ignoreValues)
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

        /// <summary>
        /// Retrun a array 2 dimension from a ulong value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint[] ToUInt32Array(this ulong value)
        {
            uint[] retValue = new uint[2];
            byte[] buffer = BitConverter.GetBytes(value);
            var byteStream = new MemoryStream(buffer, false);
            retValue[0] = BitConverter.ToUInt32(byteStream.ReadBytes(4), 0);
            retValue[1] = BitConverter.ToUInt32(byteStream.ReadBytes(4), 0);
            byteStream.Close();
            byteStream.Dispose();
            return retValue;
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

        public static short Low(this uint value)
        {
            return (short)((value << 16) >> 16);
        }

        public static short High(this uint value)
        {
            return (short)(value >> 16);
        }

    }
}
