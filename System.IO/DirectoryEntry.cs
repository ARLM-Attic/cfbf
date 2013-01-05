using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace System.IO.CFBF
{
    [StructLayout(LayoutKind.Explicit, Size = 128, CharSet = CharSet.Unicode)]
    public struct DirectoryEntry
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        [FieldOffset(0)]
        public string DirectoryEntryName;

        [FieldOffset(64)]
        public ushort DirectoryEntryNameLength;

        [FieldOffset(66)]
        public ObjectType ObjectType;

        [FieldOffset(67)]
        public ColorFlag ColorFlag;

        [FieldOffset(68)]
        public uint LeftSiblingID;

        [FieldOffset(72)]
        public uint RightSiblingID;

        [FieldOffset(76)]
        public uint ChildID;

        [FieldOffset(80)]
        public CLASSID ClassID;

        [FieldOffset(96)]
        public uint StateBits;

        [FieldOffset(100)]
        public FILETIME CreationTime;

        [FieldOffset(108)]
        public FILETIME ModifiedTime;

        [FieldOffset(116)]
        public uint StartingSectorLocation;

        [FieldOffset(120)]
        public ulong StreamByte;

        public bool IsRoot
        {
            get { return ObjectType==CFBF.ObjectType.ROOT_STORAGE_OBJECT;}
        }

        public bool IsStoredInShortStream
        {
            get
            {
                if (IsRoot)
                    return false;

                return StreamByte < 4096;
            }
        }

        public override string ToString()
        {
 	         return base.ToString();
        }
    }

     /// <summary>
    /// 
    /// </summary>
    public enum ObjectType : byte
    {
        UNKNOWN_OR_UNALLOCATED = 0x00,

        STORAGE_OBJECT = 0x01,

        STREAM_OBJECT = 0x02,

        ROOT_STORAGE_OBJECT = 0x05
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ColorFlag : byte
    {
        RED = 0x00,

        BLACK = 0x01,
    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct CLASSID
    {
        [FieldOffset(0)]
        public ulong DATA1;

        [FieldOffset(8)]
        public uint DATA2;

        [FieldOffset(12)]
        public uint DATA3;

        public override string ToString()
        {
            return string.Format("{0:X}", DATA1).PadRight(16,'0') + "-" 
                + string.Format("{0:X}", DATA2).PadRight(8,'0') + "-"
                + string.Format("{0:X}", DATA3).PadRight(8,'0');
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct FILETIME
    {
        [FieldOffset(0)]
        public uint dwLowDateTime;

        [FieldOffset(4)]
        public uint dwHighDateTime;
    }

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

    public static class FILETIMEExtension
    {
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern bool FileTimeToSystemTime([In] ref FILETIME lpFileTime, out SYSTEMTIME lpSystemTime);

        public static DateTime ToDateTime(this FILETIME ft)
        { 
            var sysTime = new SYSTEMTIME();
            FileTimeToSystemTime( ref ft, out sysTime);
            return new DateTime(sysTime.wYear, sysTime.wMonth, sysTime.wDay, sysTime.wHour, sysTime.wMinute, sysTime.wSecond);
        }
    }
}
