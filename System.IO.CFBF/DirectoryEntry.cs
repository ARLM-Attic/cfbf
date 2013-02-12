using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace System.IO.CFBF
{
    /// <summary>
    /// The directory entry array is an array of directory entries grouped into a directory sector. 
    /// Each storage object or stream object within a compound file is represented by a single directory entry. 
    /// The space for the directory sectors holding the array is allocated from the FAT.
    /// The valid values for a stream ID—used in Child ID, Right Sibling ID, 
    /// and Left Sibling ID—are 0 through MAXREGSID (0xFFFFFFFA). 
    /// The special value NOSTREAM (0xFFFFFFFF) is used as a terminator.
    /// The directory entry size is fixed at 128 bytes. The name in the directory entry is limited to 
    /// 32 Unicode UTF-16 code points, including the required Unicode terminating null character.
    /// Directory entries are grouped into blocks to form directory sectors. 
    /// There are four directory entries in a 512-byte directory sector (version 3 compound file), 
    /// and there are 32 directory entries in a 4096-byte directory sector (version 4 compound file). 
    /// The number of directory entries can exceed the number of storage objects and stream objects 
    /// due to unallocated directory entries.
    /// The detailed Directory Entry structure is specified below.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 128, CharSet = CharSet.Unicode)]
    public struct DirectoryEntry
    {
        /// <summary>
        /// Directory Entry Name (64 bytes): This field MUST contain a Unicode string for the storage or 
        /// stream name encoded in UTF-16. The name MUST be terminated with a UTF-16 terminating null 
        /// character. Thus storage and stream names are limited to 32 UTF-16 code points, including 
        /// the terminating null character. When locating an object in the compound file except for 
        /// the root storage, the directory entry name is compared using a special case-insensitive 
        /// upper-case mapping, described in Red-Black Tree. The following characters are illegal and 
        /// MUST NOT be part of the name: '/', '\', ':', '!'.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        [FieldOffset(0)]
        public string DirectoryEntryName;

        /// <summary>
        /// Directory Entry Name Length (2 bytes): This field MUST match the length of the 
        /// Directory Entry Name Unicode string in bytes. The length MUST be a multiple of 2, 
        /// and include the terminating null character in the count. This length MUST NOT 
        /// exceed 64, the maximum size of the Directory Entry Name field.
        /// </summary>
        [FieldOffset(64)]
        public ushort DirectoryEntryNameLength;

        /// <summary>
        /// Object Type (1 byte): This field MUST be 0x00, 0x01, 0x02, or 0x05, 
        /// depending on the actual type of object. All other values are not valid.
        /// </summary>
        [FieldOffset(66)]
        public ObjectType ObjectType;

        /// <summary>
        /// Color Flag (1 byte): This field MUST be 0x00 (red) or 0x01 (black). All other values are not valid.
        /// </summary>
        [FieldOffset(67)]
        public ColorFlag ColorFlag;

        /// <summary>
        /// Left Sibling ID (4 bytes): This field contains the Stream ID of the left sibling. 
        /// If there is no left sibling, the field MUST be set to NOSTREAM (0xFFFFFFFF).
        /// </summary>
        [FieldOffset(68)]
        public uint LeftSiblingID;

        /// <summary>
        /// Right Sibling ID (4 bytes): This field contains the Stream ID of the right sibling. 
        /// If there is no right sibling, the field MUST be set to NOSTREAM (0xFFFFFFFF).
        /// </summary>
        [FieldOffset(72)]
        public uint RightSiblingID;

        /// <summary>
        /// Child ID (4 bytes): This field contains the Stream ID of a child object. 
        /// If there is no child object, then the field MUST be set to NOSTREAM (0xFFFFFFFF).
        /// </summary>
        [FieldOffset(76)]
        public uint ChildID;

        /// <summary>
        /// CLSID (16 bytes): This field contains an object class GUID, 
        /// if this entry is a storage or root storage. 
        /// If there is no object class GUID set on this object, then the field 
        /// MUST be set to all zeroes. In a stream object, this field MUST be set to 
        /// all zeroes. If not NULL, the object class GUID can be used as a 
        /// parameter to launch applications.
        /// </summary>
        [FieldOffset(80)]
        public CLSID ClassID;

        /// <summary>
        /// State Bits (4 bytes): This field contains the user-defined flags if this entry is a 
        /// storage object or root storage object. If there are no state bits set on the 
        /// object, then this field MUST be set to all zeroes.
        /// </summary>
        [FieldOffset(96)]
        public uint StateBits;

        /// <summary>
        /// Creation Time (8 bytes): This field contains the creation time for a storage object. 
        /// The Windows FILETIME structure is used to represent this field in UTC. 
        /// If there is no creation time set on the object, this field MUST be all zeroes. 
        /// For a root storage object, this field MUST be all zeroes, and the creation time is 
        /// retrieved or set on the compound file itself.
        /// </summary>
        [FieldOffset(100)]
        public FILETIME CreationTime;

        /// <summary>
        /// Modified Time (8 bytes): This field contains the modification time for a storage object. 
        /// The Windows FILETIME structure is used to represent this field in UTC. 
        /// If there is no modified time set on the object, this field MUST be all zeroes. 
        /// For a root storage object, this field MUST be all zeroes, and the modified time 
        /// is retrieved or set on the compound file itself.
        /// </summary>
        [FieldOffset(108)]
        public FILETIME ModifiedTime;

        /// <summary>
        /// Starting Sector Location (4 bytes): This field contains the first sector location if 
        /// this is a stream object. For a root storage object, this field MUST contain the 
        /// first sector of the mini stream, if the mini stream exists.
        /// </summary>
        [FieldOffset(116)]
        public uint StartingSectorLocation;

        /// <summary>
        /// Stream Size (8 bytes): This 64-bit integer field contains the size of the user-defined data,
        /// if this is a stream object. For a root storage object, this field contains the size of the mini stream.
        /// For a version 3 compound file 512-byte sector size, this value of this field MUST 
        /// be less than or equal to 0x80000000 (equivalently, this requirement could be stated: 
        /// the size of a stream or of the mini stream in a version 3 compound file MUST be less 
        /// than or equal to 2GB). Note that as a consequence of this requirement, the most 
        /// significant 32 bits of this field MUST be zero in a version 3 compound file. However, 
        /// implementers should be aware that some older implementations did not initialize the most 
        /// significant 32 bits of this field, and these bits might therefore be nonzero in files that 
        /// are otherwise valid version 3 compound files. Although this document does not normatively 
        /// specify parser behavior, it is recommended that parsers ignore the most significant 32 
        /// bits of this field in version 3 compound files, treating it as if its value were zero, 
        /// unless there is a specific reason to do otherwise (for example, a parser whose purpose 
        /// is to verify the correctness of a compound file).
        /// </summary>
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
             string s = string.Format("Direcotry Entry:{0} (ChildID:{1}, Left: {2}, Right: {3})", DirectoryEntryName, ChildID, LeftSiblingID, RightSiblingID);
 	         return s;
        }

        public DateTime MODIFIED_TIME
        {
            get { return ModifiedTime.ToDateTime(); }
        }

        public DateTime CREATION_TIME
        {
            get { return CreationTime.ToDateTime(); }
        }
    }
  }
