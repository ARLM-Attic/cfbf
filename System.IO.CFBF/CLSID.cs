using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO.CFBF
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct CLSID
    {
        [FieldOffset(0)]
        public ulong DATA1;

        [FieldOffset(8)]
        public uint DATA2;

        [FieldOffset(12)]
        public uint DATA3;

        public override string ToString()
        {
            return string.Format("{0:X}", DATA1).PadLeft(16, '0') + "-"
                + string.Format("{0:X}", DATA2).PadLeft(8, '0') + "-"
                + string.Format("{0:X}", DATA3).PadLeft(8, '0');
        }

        /// <summary>
        /// 
        /// </summary>
        public static CLSID Empty()
        {
                return new CLSID { DATA1=0, DATA2 = 0, DATA3 = 0};
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(CLSID c1, CLSID c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(CLSID c1, CLSID c2)
        {
            return !c1.Equals(c2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else
            {
                if (obj is CLSID)
                    return (this.DATA1 == ((CLSID)obj).DATA1 && this.DATA2 == ((CLSID)obj).DATA2 && this.DATA3 == ((CLSID)obj).DATA3);
                else
                    return false;
            }
        }
    }

}
