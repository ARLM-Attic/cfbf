using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace System.IO.CFBF
{
    /// <summary>
    /// The Compound File Header structure MUST be at the beginning of the file (offset 0).
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 512)]
    public struct Header
    {
        /// <summary>
        /// Header Signature (8 bytes): Identification signature for the compound file structure, and MUST be set to the value 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1.
        /// </summary>
        [FieldOffset(0)]
        public ulong HeaderSignature;

        /// <summary>
        /// Header CLSID (16 bytes): Reserved and unused class ID that MUST be set to all zeroes (CLSID_NULL).
        /// </summary>
        [FieldOffset(8)]
        public CLASSID HeaderCLSID;

        /// <summary>
        /// Minor Version (2 bytes): Version number for non-breaking changes. This field SHOULD be set
        /// to 0x003E if the major version field is either 0x0003 or 0x0004.
        /// </summary>
        [FieldOffset(24)]
        public ushort MinorVersion;

        /// <summary>
        /// Major Version (2 bytes): Version number for breaking changes. 
        /// This field MUST be set to either 0x0003 (version 3) or 0x0004 (version 4).
        /// Name Value
        /// version 3 0x0003
        /// version 4 0x0004
        /// </summary>
        [FieldOffset(26)]
        public ushort MajorVersion;

        /// <summary>
        /// Byte Order (2 bytes): This field MUST be set to 0xFFFE. 
        /// This field is a byte order mark for all integer fields, specifying little-endian byte order.
        /// </summary>
        [FieldOffset(28)]
        public ushort ByteOrder;

        /// <summary>
        /// Sector Shift (2 bytes): This field MUST be set to 0x0009, or 0x000c, depending on the Major Version field. This field specifies the sector size of the compound file as a power of 2.
        /// . If Major Version is 3, then the Sector Shift MUST be 0x0009, specifying a sector size of 512 bytes.
        /// . If Major Version is 4, then the Sector Shift MUST be 0x000C, specifying a sector size of 4096 bytes.
        /// </summary>
        [FieldOffset(30)]
        public ushort SectorShift;

        /// <summary>
        /// Mini Sector Shift (2 bytes): This field MUST be set to 0x0006. This field specifies the sector size of the Mini Stream as a power of 2. The sector size of the Mini Stream MUST be 64 bytes.
        /// </summary>
        [FieldOffset(32)]
        public ushort MinorSectorShift;

        /// <summary>
        /// Reserved (6 bytes): This field MUST be set to all zeroes.
        /// </summary>
        [FieldOffset(34)]
        public byte Reserved;

        /// <summary>
        /// Number of Directory Sectors (4 bytes): This integer field contains the count of the number of directory sectors in the compound file.
        /// . If Major Version is 3, then the Number of Directory Sectors MUST be zero. This field is not supported for version 3 compound files.
        /// </summary>
        [FieldOffset(40)]
        public uint NumberDirectorySector;

        /// <summary>
        /// Number of FAT Sectors (4 bytes): This integer field contains the count of the number of FAT sectors in the compound file.
        /// </summary>
        [FieldOffset(44)]
        public uint NumberFATSectors;

        /// <summary>
        /// First Directory Sector Location (4 bytes): This integer field contains the starting sector number for the directory stream.
        /// </summary>
        [FieldOffset(48)]
        public uint FirstDirectorySectorLocation;

        /// <summary>
        /// Transaction Signature Number (4 bytes): This integer field MAY contain a sequence number that is incremented 
        /// every time the compound file is saved by an implementation that supports file transactions. 
        /// This is field that MUST be set to all zeroes if file transactions are not implemented.
        /// </summary>
        [FieldOffset(52)]
        public uint TransactionSignatureNumber;

        /// <summary>
        /// Mini Stream Cutoff Size (4 bytes): This integer field MUST be set to 0x00001000. 
        /// This field specifies the maximum size of a user-defined data stream allocated from the mini FAT and mini stream, 
        /// and that cutoff is 4096 bytes. 
        /// Any user-defined data stream larger than or equal to this cutoff size must be allocated as normal sectors from the FAT.
        /// </summary>
        [FieldOffset(56)]
        public uint MiniStreamCutoffSize;

        /// <summary>
        /// First Mini FAT Sector Location (4 bytes): This integer field contains the starting sector number for the mini FAT.
        /// </summary>
        [FieldOffset(60)]
        public uint FirstMiniFATSectorLocation;

        /// <summary>
        /// Number of Mini FAT Sectors (4 bytes): This integer field contains the count of the number of mini FAT sectors in the compound file.
        /// </summary>
        [FieldOffset(64)]
        public uint NumberMiniFATSectors;

        /// <summary>
        /// First DIFAT Sector Location (4 bytes): This integer field contains the starting sector number for the DIFAT.
        /// </summary>
        [FieldOffset(68)]
        public uint FirstDIFATSectorLocation;

        /// <summary>
        /// Number of DIFAT Sectors (4 bytes): This integer field contains the count of the number of DIFAT sectors in the compound file.
        /// </summary>
        [FieldOffset(72)]
        public uint NumberDIFATSectors;

        /// <summary>
        /// DIFAT (436 bytes): This array of 32-bit integer fields contains the first 109 FAT sector locations of the compound file.
        /// For version 4 compound files, the header size (512 bytes) is less than the sector size (4096 bytes), so the remaining part of the header (3584 bytes) MUST be filled with all zeroes.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 436)]
        [FieldOffset(76)]
        public byte[] DIFAT;

        public string HEADER_SIGNATURE
        {
            get {
                return string.Format("0x{0:X}", this.HeaderSignature);
            }
        }
    }
}
