using System.Runtime.InteropServices;

namespace Outlook.PST
{

    public interface IHeader
    {
    }

    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 576)]
    public unsafe struct HeaderUnicode : IHeader
    {
        /// <summary>
        /// dwMagic (4 bytes): MUST be "{ 0x21, 0x42, 0x44, 0x4E } ("!BDN")".
        /// </summary>
        [FieldOffset(0)]
        public int dwMagic;

        /// <summary>
        /// dwCRCPartial (4 bytes): The 32-bit cyclic redundancy check (CRC) value of the 471 bytes of data starting from wMagicClient (0ffset 0x0008)
        /// </summary>
        [FieldOffset(4)]
        public uint dwCRCPartial;

        /// <summary>
        /// wMagicClient (2 bytes): MUST be "{ 0x53, 0x4D }".
        /// </summary>
        [FieldOffset(8)]
        public short wMagicClient;

        /// <summary>
        /// wVer (2 bytes): File format version. 
        /// This value MUST be 14 or 15 if the file is an ANSI PST file, and MUST be 23 if the file is a Unicode PST file.
        /// </summary>
        [FieldOffset(10)]
        public short wVer;

        /// <summary>
        /// wVerClient (2 bytes): Client file format version. 
        /// The version that corresponds to the format described in this document is 19. 
        /// Creators of a new PST file based on this document SHOULD initialize this value to 19.
        /// </summary>
        [FieldOffset(12)]
        public short wVerClient;

        /// <summary>
        /// bPlatformCreate (1 byte): This value MUST be set to 0x01.
        /// </summary>
        [FieldOffset(14)]
        public byte bPlatformCreate;

        /// <summary>
        /// bPlatformAccess (1 byte): This value MUST be set to 0x01.
        /// </summary>
        [FieldOffset(15)]
        public byte bPlatformAccess;

        /// <summary>
        /// dwReserved1 (4 bytes): Implementations SHOULD ignore this value and SHOULD NOT modify it. 
        /// Creators of a new PST file MUST initialize this value to zero
        /// </summary>
        [FieldOffset(16)]
        public int dwReserved1;

        /// <summary>
        /// dwReserved2 (4 bytes): Implementations SHOULD ignore this value and SHOULD NOT modify it. 
        /// Creators of a new PST file MUST initialize this value to zero
        /// </summary>
        [FieldOffset(20)]
        public int dwReserved2;

        /// <summary>
        /// bidUnused (8 bytes Unicode only): 
        /// Unused padding added when the Unicode PST file format was created
        /// </summary>
        [FieldOffset(24)]
        public long bidUnused;

        /// <summary>
        /// bidNextP (Unicode: 8 bytes; ANSI: 4 bytes): Next page BID. 
        /// Pages have a special counter for allocating bidIndex values. The value of bidIndex for BIDs for pages is allocated from this counter.
        /// </summary>
        [FieldOffset(32)]
        public long bidNextP;

        ///// <summary>
        ///// bidNextP (Unicode: 8 bytes; ANSI: 4 bytes): Next page BID. 
        ///// Pages have a special counter for allocating bidIndex values. The value of bidIndex for BIDs for pages is allocated from this counter.
        ///// </summary>
        //[FieldOffset(40)]
        //public long bidNextB;

        /// <summary>
        /// dwUnique (4 bytes): 
        /// This is a monotonically-increasing value that is modified every time the PST file's HEADER structure is modified. 
        /// The function of this value is to provide a unique value, and to ensure that the HEADER CRCs are different after each header modification
        /// </summary>
        [FieldOffset(40)]
        public int dwUnique;

        /// <summary>
        /// rgnid[] (128 bytes): A fixed array of 32 NIDs, each corresponding to one of the 32 possible NID_TYPEs (section 2.2.2.1). 
        /// Different NID_TYPEs can have different starting nidIndex values. When a blank PST file is created, 
        /// these values are initialized by NID_TYPE according to the following table. 
        /// Each of these NIDs indicates the last nidIndex value that had been allocated for the corresponding NID_TYPE. 
        /// When an NID of a particular type is assigned, the corresponding slot in rgnid is also incremented by 1.
        /// </summary>
        [FieldOffset(44)]
        public fixed int rgnid[32];

        /// <summary>
        /// qwUnused (8 bytes): 
        /// Unused space; MUST be set to zero. Unicode PST file format only.
        /// </summary>
        [FieldOffset(172)]
        public long qwUnused;

        /// <summary>
        /// root (Unicode: 72 bytes; ANSI: 40 bytes): 
        /// A ROOT structure
        /// </summary>
        [FieldOffset(180)]
        public RootUnicode root;

        /// <summary>
        /// dwAlign (4 bytes): Unused alignment bytes; MUST be set to zero. Unicode PST file format only.
        /// </summary>
        [FieldOffset(252)]
        public int dwAlign;

        /// <summary>
        /// rgbFM (128 bytes): Deprecated FMap. This is no longer used and MUST be filled with 0xFF. Readers SHOULD ignore the value of these bytes.
        /// </summary>
        [FieldOffset(256)]
        public fixed byte rgbFM[128];

        /// <summary>
        /// rgbFP (128 bytes): Deprecated FPMap. This is no longer used and MUST be filled with 0xFF. Readers SHOULD ignore the value of these bytes
        /// </summary>
        [FieldOffset(384)]
        public fixed byte rgbFP[128];

        /// <summary>
        /// bSentinel (1 byte): MUST be set to 0x80.
        /// </summary>
        [FieldOffset(512)]
        public byte bSentinel;

        /// <summary>
        /// bCryptMethod (1 byte): Indicates how the data within the PST file is encoded. 
        /// MUST be set to one of the pre-defined values described in the following table.
        /// </summary>
        [FieldOffset(513)]
        public CryptMethodType bCryptMethod;

        /// <summary>
        /// rgbReserved (2 bytes): 
        /// Reserved; MUST be set to zero.
        /// </summary>
        [FieldOffset(514)]
        public short rgbReserved;

        /// <summary>
        /// bidNextB (Unicode: 8 bytes; ANSI: 4 bytes): Next BID. 
        ///This value is the monotonic counter that indicates the BID to be assigned for the next allocated block. BID values advance in increments of 4.
        /// </summary>
        [FieldOffset(516)]
        public long bidNextB;

        /// <summary>
        /// dwCRCFull (4 bytes): The 32-bit CRC value of the 516 bytes of data starting from wMagicClient to bidNextB, inclusive. Unicode PST file format only.
        /// </summary>
        [FieldOffset(524)]
        public uint dwCRCFull;

        /// <summary>
        /// ullReserved (8 bytes): Reserved; MUST be set to zero. ANSI PST file format only.
        /// </summary>
        [FieldOffset(528)]
        public long ullReserved;

        /// <summary>
        /// dwReserved (4 bytes): Reserved; MUST be set to zero. ANSI PST file format only.
        /// </summary>
        [FieldOffset(536)]
        public int dwReserved;

        /// <summary>
        /// rgbReserved2 (3 bytes): Implementations SHOULD ignore this value and SHOULD NOT modify it. Creators of a new PST MUST initialize this value to zero.
        /// </summary>
        [FieldOffset(540)]
        public fixed byte rgbReserved2[3];

        /// <summary>
        /// bReserved (1 byte): Implementations SHOULD ignore this value and SHOULD NOT modify it. Creators of a new PST file MUST initialize this value to zero.
        /// </summary>
        [FieldOffset(543)]
        public byte bReserved;

        /// <summary>
        /// rgbReserved3 (32 bytes): Implementations SHOULD ignore this value and SHOULD NOT modify it. Creators of a new PST MUST initialize this value to zero.
        /// </summary>
        [FieldOffset(544)]
        public fixed byte rgbReserved3[32];
    }

    /// <summary>
    /// 
    /// </summary>
    public struct HeaderANSI : IHeader
    {
        public int bidNextP;
        public int bidNextB;
    }
}
