using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO.CFBF
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct FILETIME
    {
        [FieldOffset(0)]
        public uint dwLowDateTime;

        [FieldOffset(4)]
        public uint dwHighDateTime;
    }



    public static class FILETIMEExtension
    {
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern bool FileTimeToSystemTime([In] ref FILETIME lpFileTime, out SYSTEMTIME lpSystemTime);

        public static DateTime ToDateTime(this FILETIME ft)
        {
            var sysTime = new SYSTEMTIME();
            FileTimeToSystemTime(ref ft, out sysTime);
            return new DateTime(sysTime.wYear, sysTime.wMonth, sysTime.wDay, sysTime.wHour, sysTime.wMinute, sysTime.wSecond);
        }
    }

}
