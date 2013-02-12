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
        public CLSID Empty
        {
            get {
                return new CLSID { DATA1=0, DATA2 = 0, DATA3 = 0};
            }
        }
    }

}
