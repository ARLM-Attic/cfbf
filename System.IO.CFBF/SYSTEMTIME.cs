using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO.CFBF
{

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct SYSTEMTIME
    {
        [FieldOffset(0)]
        public short wYear;

        [FieldOffset(2)]
        public short wMonth;

        [FieldOffset(4)]
        public short wDayOfWeek;

        [FieldOffset(6)]
        public short wDay;

        [FieldOffset(8)]
        public short wHour;

        [FieldOffset(10)]
        public short wMinute;

        [FieldOffset(12)]
        public short wSecond;

        [FieldOffset(14)]
        public short wMilliseconds;
    }

}
