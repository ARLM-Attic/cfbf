using System.Runtime.InteropServices;

namespace Outlook.PST
{
    /*
     * The Heap-on-Node defines a standard heap over a node's data stream. 
     * Taking advantage of the flexible structure of the node, the organization of the heap 
     * data can take on several forms, depending on how much data is stored in the heap.
     * For heaps whose size exceed the amount of data that can fit in one data block, 
     * the first data block in the HN contains a full header record and a trailer record. 
     * With the exception of blocks that require an HNBITMAPHDR structure, subsequent data 
     * blocks only have an abridged header and a trailer. This is explained in more detail 
     * in the following sections. Because the heap is a structure that is defined at a higher 
     * layer than the NDB, the heap structures are written to the external data sections of 
     * data blocks and do not use any information from the data block's NDB structure.
     */

    /// <summary>
    /// HID is a 4-byte value that identifies an item allocated from the heap. The value is unique only within the heap itself. The following is the structure of an HID.
    /// Unicode / ANSI:
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size=4)]
    public struct HID
    {
        [FieldOffset(0)]
        public uint Hid;

        /// <summary>
        /// hidType (5 bits): HID Type; MUST be set to 0 (NID_TYPE_HID) to indicate a valid HID.
        /// </summary>
        public uint hidType 
        {
            get {
                return (Hid >> 27);
            }
        }

        /// <summary>
        /// hidIndex (11 bits): HID index. This is the 1-based index value 
        /// that identifies an item allocated from the heap node. This value MUST NOT be zero.
        /// </summary>
        public uint hidIndex
        {
            get
            {
                return ((Hid<<5)>>21);
            }
        }

        /// <summary>
        /// hidBlockIndex (16 bits): This is the zero-based data block index. 
        /// This number indicates the zero-based index of the data block in which this heap item resides.
        /// </summary>
        public uint hidBlockIndex
        {
            get
            {
                return (Hid << 16) >> 16;
            }
        }
    }

    /// <summary>
    /// The HNHDR record resides at the beginning of the first data block in the HN (an HN can span several blocks), 
    /// which contains root information about the HN.
    /// 
    /// Unicode / ANSI
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size=12)]
    public struct HNHDR
    {
        /// <summary>
        /// ibHnpm (2 bytes): The byte offset to the HN page Map record (section 2.3.1.5), with respect to the beginning of the HNHDR structure.
        /// </summary>
        [FieldOffset(0)]
        ushort ibHnpm;

        /// <summary>
        /// bSig (1 byte): Block signature; MUST be set to 0xEC to indicate an HN.
        /// </summary>
        [FieldOffset(2)]
        byte bSig;

        /// <summary>
        /// bClientSig (1 byte): Client signature. This value describes the higher-level 
        /// structure that is implemented on top of the HN. This value is intended as a 
        /// hint for a higher-level structure and has no meaning for structures defined at the HN level. 
        /// The following values are pre-defined for bClientSig. 
        /// All other values not described in the following table are reserved and MUST NOT be assigned or used.
        /// </summary>
        [FieldOffset(3)]
        ClientSignature bClientSig;

        /// <summary>
        /// hidUserRoot (4 bytes): HID that points to the User Root record. The User Root record contains data that is specific to the higher level.
        /// </summary>
        [FieldOffset(4)]
        uint hidUserRoot;

        /// <summary>
        /// rgbFillLevel (4 bytes): Per-block Fill Level Map. This array consists of eight 4-bit values that indicate the fill level for each of the first 8 data blocks (including this header block). If the HN
        /// has fewer than 8 data blocks, then the values corresponding to the non-existent data blocks MUST be set to zero. The following table explains the values indicated by each 4-bit value.
        /// </summary>
        [FieldOffset(8)]
        uint rgbFillLevel;
    }

    /// <summary>
    /// Beginning with the eighth data block, a new Fill Level Map is required. 
    /// An HNBITMAPHDR fulfills this requirement. 
    /// The Fill Level Map in the HNBITMAPHDR can map 128 blocks.
    /// This means that an HNBITMAPHDR appears at data block 8 (the first data block is data block 0) 
    /// and thereafter every 128 blocks. (that is, data block 8, data block 136, data block 264, and so on).
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 66)]
    public unsafe struct HNBITMAPHDR
    {
        /// <summary>
        /// ibHnpm (2 bytes): The byte offset to the HNPAGEMAP record (section 2.3.1.5) relative to the beginning of the HNPAGEHDR structure.
        /// </summary>
        [FieldOffset(0)]
        ushort ibHnpm;

        /// <summary>
        /// rgbFillLevel (64 bytes): Per-block Fill Level Map. This array consists of one hundred and twenty-eight (128) 
        /// 4-bit values that indicate the fill level for the next 128 data blocks (including this data block). 
        /// If the HN has fewer than 128 data blocks after this data block, then the values corresponding to 
        /// the non-existent data blocks MUST be set to zero. See rgbFillLevel in section 2.3.1.2 for possible values.
        /// </summary>
        [FieldOffset(2)]
        fixed byte rgbFillLevel[64];
    }

    /// <summary>
    /// The HNPAGEMAP record is located at the end of each HN block immediately before the block trailer. 
    /// It contains the information about the allocations in the page. 
    /// The HNPAGEMAP is located using the ibHnpm field in the HNHDR, HNPAGEHDR and HNBITMAPHDR records.
    /// </summary>
    public struct HNPAGEMAP
    {
        /// <summary>
        /// cAlloc (2 bytes): Allocation count. This represents the number of items (allocations) in the HN.
        /// </summary>
        ushort cAlloc;

        /// <summary>
        /// cFree (2 bytes): Free count. This represents the number of freed items in the HN.
        /// </summary>
        ushort cFree;

        /// <summary>
        /// rgibAlloc (variable): Allocation table. This contains cAlloc + 1 entries. Each entry is a 
        /// WORD value that is the byte offset to the beginning of the allocation. 
        /// An extra entry exists at the cAlloc + 1st position to mark the offset of the next available slot. 
        /// Therefore, the nth allocation starts at offset rgibAlloc[n] (from the beginning of the HN header), 
        /// and its size is calculated as rgibAlloc[n + 1] – rgibAlloc[n] bytes.
        /// </summary>
        int[] rgibAlloc;
    }
}
