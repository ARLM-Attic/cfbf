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
  }
