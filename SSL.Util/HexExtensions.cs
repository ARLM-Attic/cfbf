using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSL.Util
{
    public static class HexExtensions
    {
        public static string ToHex(this byte value)
        {
            return string.Format("{0:x}", value).ToUpper().PadLeft(2, '0');
        }

        public static string ToHex(this int value)
        {
            return string.Format("{0:x}", value).ToUpper().PadLeft(8, '0');
        }

        public static string ToHex(this long value)
        {
            return string.Format("{0:x}", value).ToUpper().PadLeft(16, '0');
        }

        public static string ToHex(this short value)
        {
            return string.Format("{0:x}", value).ToUpper().PadLeft(4, '0');
        }

        public static string ToHex(this uint value)
        {
            return string.Format("{0:x}", value).ToUpper().PadLeft(8, '0');
        }

        public static string ToHex(this ulong value)
        {
            return string.Format("{0:x}", value).ToUpper().PadLeft(16, '0');
        }

        public static string ToHex(this UInt16 value)
        {
            return string.Format("{0:x}", value).ToUpper().PadLeft(8, '0');
        }
    }
}
