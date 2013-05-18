using System.Runtime.InteropServices;

namespace Outlook.PST
{
    /// <summary>
    /// A page is a fixed-size structure of 512 bytes that is used in the NDB Layer to represent allocation metadata 
    /// and BTree data structures. A page trailer is placed at the very end of every page 
    /// such that the end of the page trailer is aligned with the end of the page
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 512)]
    public struct Page
    {
    }

    #region PageTrailer
    /// <summary>
    /// A PAGETRAILER structure contains information about the page in which it is contained. 
    /// PAGETRAILER structure is present at the very end of each page in a PST file.
    /// </summary>
    public interface IPageTrailer
    {
    }

    /// <summary>
    /// A PAGETRAILER structure contains information about the page in which it is contained. 
    /// PAGETRAILER structure is present at the very end of each page in a PST file.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct PageTrailerUnicode : IPageTrailer
    {
        /// <summary>
        /// This value indicates the type of data contained within the page. This field MUST contain one of the following values
        /// </summary>
        [FieldOffset(0)]
        PType ptype;

        /// <summary>
        /// MUST be set to the same value as ptype.
        /// </summary>
        [FieldOffset(1)]
        byte ptypeRepeat;

        /// <summary>
        /// Page signature. This value depends on the value of the ptype field. This value is zero (0x0000) 
        /// for AMap, PMap, FMap, and FPMap pages. For BBT, NBT, and DList pages, a page / block signature is computed
        /// </summary>
        [FieldOffset(2)]
        short wSig;

        /// <summary>
        /// 32-bit CRC of the page data, excluding the page trailer. See section 5.3 for the CRC algorithm.
        /// </summary>
        [FieldOffset(4)]
        int dwCRC;

        /// <summary>
        /// The BID of the page's block. AMap, PMap, FMap, and FPMap pages have a special convention where their
        /// BID is assigned the same value as their IB (that is, the absolute file offset of the page). 
        /// The bidIndex for other page types are allocated from the special bidNextP counter in the HEADER structure
        /// </summary>
        [FieldOffset(8)]
        long bid;
    }

    /// <summary>
    /// A PAGETRAILER structure contains information about the page in which it is contained. 
    /// PAGETRAILER structure is present at the very end of each page in a PST file.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct PageTrailerAnsi : IPageTrailer
    {
        /// <summary>
        /// This value indicates the type of data contained within the page. This field MUST contain one of the following values
        /// </summary>
        [FieldOffset(0)]
        PType ptype;

        /// <summary>
        /// MUST be set to the same value as ptype.
        /// </summary>
        [FieldOffset(1)]
        byte ptypeRepeat;

        /// <summary>
        /// Page signature. This value depends on the value of the ptype field. This value is zero (0x0000) 
        /// for AMap, PMap, FMap, and FPMap pages. For BBT, NBT, and DList pages, a page / block signature is computed
        /// </summary>
        [FieldOffset(2)]
        short wSig;

        /// <summary>
        /// 32-bit CRC of the page data, excluding the page trailer. See section 5.3 for the CRC algorithm.
        /// </summary>
        [FieldOffset(4)]
        int dwCRC;

        /// <summary>
        /// The BID of the page's block. AMap, PMap, FMap, and FPMap pages have a special convention where their
        /// BID is assigned the same value as their IB (that is, the absolute file offset of the page). 
        /// The bidIndex for other page types are allocated from the special bidNextP counter in the HEADER structure
        /// </summary>
        [FieldOffset(8)]
        int bid;
    }
    #endregion

    #region AMapPage
    public interface IAMapPage
    {
    }

    /// <summary>
    /// An AMap page contains an array of 496 bytes that is used to track the space allocation within the data section 
    /// that immediately follows the AMap page. Each bit in the array maps to a block of 64 bytes in the data section. 
    /// Specifically, the first bit maps to the first 64 bytes of the data section, the second bit maps to the next 
    /// 64 bytes of data, and so on. AMap pages map a data section that consists of 253,952 bytes (496 * 8 * 64).
    ///An AMap is allocated out of the data section and, therefore, it actually "maps itself". 
    ///What this means is that the AMap actually occupies the first page of the data section and the first byte (that is, 8 bits) 
    ///of the AMap is always 0xFF, which indicates that the first 512 bytes are allocated for the AMap.
    ///The first AMap of a PST file is always located at absolute file offset 0x4400, and subsequent AMaps appear at 
    ///intervals of 253,952 bytes thereafter. The following is the structural representation of an AMap page.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 512)]
    public unsafe struct AMapPageUnicode : IAMapPage
    {
        /// <summary>
        /// AMap data. This is represented as a sequence of bits that marks whether blocks of 64 bytes 
        /// of data have been allocated. If the nth bit is set to 1, then the nth block of 64 bytes 
        /// has been allocated. Alternatively, if the nth bit is set to 0, the nth block of 64 bytes is not allocated (free).
        /// </summary>
        [FieldOffset(0)]
        public byte[] rgbAMapBits;

        /// <summary>
        /// pageTrailer (Unicode: 16 bytes; ANSI: 12 bytes): A PAGETRAILER structure (section 2.2.2.7.1). 
        /// The ptype subfield of pageTrailer MUST be set to ptypeAMap. The other subfields of pageTrailer MUST be set as specified in section 2.2.2.7.1.
        /// </summary>
        [FieldOffset(496)]
        public byte[] pageTrailer;
    }

    /// <summary>
    /// An AMap page contains an array of 496 bytes that is used to track the space allocation within the data section 
    /// that immediately follows the AMap page. Each bit in the array maps to a block of 64 bytes in the data section. 
    /// Specifically, the first bit maps to the first 64 bytes of the data section, the second bit maps to the next 
    /// 64 bytes of data, and so on. AMap pages map a data section that consists of 253,952 bytes (496 * 8 * 64).
    ///An AMap is allocated out of the data section and, therefore, it actually "maps itself". 
    ///What this means is that the AMap actually occupies the first page of the data section and the first byte (that is, 8 bits) 
    ///of the AMap is always 0xFF, which indicates that the first 512 bytes are allocated for the AMap.
    ///The first AMap of a PST file is always located at absolute file offset 0x4400, and subsequent AMaps appear at 
    ///intervals of 253,952 bytes thereafter. The following is the structural representation of an AMap page.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 512)]
    public struct AMapPageAnsi : IAMapPage
    {
        /// <summary>
        /// dwPadding (ANSI file format only, 4 bytes): Unused padding; MUST be set to zero.
        /// </summary>
        [FieldOffset(0)]
        int dwPadding;

        /// <summary>
        /// AMap data. This is represented as a sequence of bits that marks whether blocks of 64 bytes 
        /// of data have been allocated. If the nth bit is set to 1, then the nth block of 64 bytes 
        /// has been allocated. Alternatively, if the nth bit is set to 0, the nth block of 64 bytes is not allocated (free).
        /// </summary>
        [FieldOffset(4)]
        byte[] rgbAMapBits;

        /// <summary>
        /// pageTrailer (Unicode: 16 bytes; ANSI: 12 bytes): A PAGETRAILER structure (section 2.2.2.7.1). 
        /// The ptype subfield of pageTrailer MUST be set to ptypeAMap. The other subfields of pageTrailer MUST be set as specified in section 2.2.2.7.1.
        /// </summary>
        [FieldOffset(500)]
        byte[] pageTrailer;
    }
    #endregion

    #region PMapPage
    public interface IPMapPage
    {
    }

   /// <summary>
   /// 
   /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 512)]
    public struct PMapPageUnicode : IPMapPage
    {
        /// <summary>
        /// AMap data. This is represented as a sequence of bits that marks whether blocks of 64 bytes 
        /// of data have been allocated. If the nth bit is set to 1, then the nth block of 64 bytes 
        /// has been allocated. Alternatively, if the nth bit is set to 0, the nth block of 64 bytes is not allocated (free).
        /// </summary>
        [FieldOffset(0)]
        byte[] rgbPMapBits;

        /// <summary>
        /// pageTrailer (Unicode: 16 bytes; ANSI: 12 bytes): A PAGETRAILER structure (section 2.2.2.7.1). 
        /// The ptype subfield of pageTrailer MUST be set to ptypeAMap. The other subfields of pageTrailer MUST be set as specified in section 2.2.2.7.1.
        /// </summary>
        [FieldOffset(496)]
        byte[] pageTrailer;
    }

    /// <summary>
    /// An AMap page contains an array of 496 bytes that is used to track the space allocation within the data section 
    /// that immediately follows the AMap page. Each bit in the array maps to a block of 64 bytes in the data section. 
    /// Specifically, the first bit maps to the first 64 bytes of the data section, the second bit maps to the next 
    /// 64 bytes of data, and so on. AMap pages map a data section that consists of 253,952 bytes (496 * 8 * 64).
    ///An AMap is allocated out of the data section and, therefore, it actually "maps itself". 
    ///What this means is that the AMap actually occupies the first page of the data section and the first byte (that is, 8 bits) 
    ///of the AMap is always 0xFF, which indicates that the first 512 bytes are allocated for the AMap.
    ///The first AMap of a PST file is always located at absolute file offset 0x4400, and subsequent AMaps appear at 
    ///intervals of 253,952 bytes thereafter. The following is the structural representation of an AMap page.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 512)]
    public struct PMapPageAnsi : IPMapPage
    {
        /// <summary>
        /// dwPadding (ANSI file format only, 4 bytes): Unused padding; MUST be set to zero.
        /// </summary>
        [FieldOffset(0)]
        int dwPadding;

        /// <summary>
        /// AMap data. This is represented as a sequence of bits that marks whether blocks of 64 bytes 
        /// of data have been allocated. If the nth bit is set to 1, then the nth block of 64 bytes 
        /// has been allocated. Alternatively, if the nth bit is set to 0, the nth block of 64 bytes is not allocated (free).
        /// </summary>
        [FieldOffset(4)]
        byte[] rgbPMapBits;

        /// <summary>
        /// pageTrailer (Unicode: 16 bytes; ANSI: 12 bytes): A PAGETRAILER structure (section 2.2.2.7.1). 
        /// The ptype subfield of pageTrailer MUST be set to ptypeAMap. The other subfields of pageTrailer MUST be set as specified in section 2.2.2.7.1.
        /// </summary>
        [FieldOffset(500)]
        byte[] pageTrailer;
    }
    #endregion

    /// <summary>
    /// Each DLISTPAGEENT record in the DList represents a reference to an AMap PAGE in the PST file.
    /// </summary>
    public class DListPageEnt
    {
        private int _value = 0;

        public DListPageEnt(int value)
        {
            _value = value;
        }

        /// <summary>
        /// dwPageNum (20 bits): AMap page number. 
        /// This is the zero-based index to the AMap page that corresponds to this entry. 
        /// A dwPageNum of "n" corresponds to the nth AMap from the beginning of PST file.
        /// </summary>
        public int dwPageNum {
            get { return (_value>>12); }
        }

        /// <summary>
        /// dwFreeSlots (12 bits): Total number of free slots in the AMap. 
        /// This value is the aggregate sum of all free 64-byte slots in the AMap. 
        /// Note that the free slots can be of any random configuration, and are not guaranteed to be contiguous
        /// </summary>
        public short dwFreeSlots {
            get { return (short)((_value<<20)>>20); }
        }
    }

    #region DListPage
    public interface IDListPage
    {
    }

    /// <summary>
    /// 
    /// </summary>
    //[StructLayout(LayoutKind.Explicit, Size = 512)]
    public struct DListPageUnicode : IDListPage
    {
        /// <summary>
        /// Flags; MUST be set to zero or a combination of the defined values described in the following table.
        /// </summary>
        byte bFlags;

        /// <summary>
        /// Number of entries in the rgDListPageEnt array.
        /// </summary>
        byte cEntDList;

        /// <summary>
        /// Padding bytes; MUST be set to zero.
        /// </summary>
        short wPadding;

        /// <summary>
        /// The meaning of this field depends on the value of bFlags. 
        /// If DFL_BACKFILL _COMPLETE is set in bFlags, then this value indicates the AMap page index that 
        /// is used in the next allocation. If DFL_BACKFILL_COMPLETE is not set in bFlags, then this
        /// value indicates the AMap page index that is attempted for backfilling in the next allocation. 
        /// See section 2.6.1.3.4 for more information regarding Backfilling.
        /// </summary>
        int ulCurrentPage;

        /// <summary>
        /// (Unicode: 476 bytes; ANSI: 480 bytes): DList page entries. 
        /// This is an array of DLISTPAGEENT records with cEntDList entries that constitute the DList. 
        /// Each record contains an AMap page index and the aggregate amount of free slots available in that AMap. 
        /// Note that, while the size of the field is fixed, the size of valid data within the field is not. 
        /// Implementations MUST only read the number of DLISTPAGEENT entries from the array indicated by cEntDList
        /// </summary>
        byte[] rgDListPageEnt;

        /// <summary>
        /// (Unicode: 16 bytes; ANSI: 12 bytes): A PAGETRAILER structure (section 2.2.2.7.1). 
        /// The ptype subfield of pageTrailer MUST be set to ptypeDL. The other subfields of pageTrailer MUST be set as specified in section 2.2.2.7.1.
        /// </summary>
        byte[] pageTrailer;
    }

   // [StructLayout(LayoutKind.Explicit, Size = 512)]
    public struct DListPageAnsi : IDListPage
    {
        /// <summary>
        /// Flags; MUST be set to zero or a combination of the defined values described in the following table.
        /// </summary>
        byte bFlags;

        /// <summary>
        /// Number of entries in the rgDListPageEnt array.
        /// </summary>
        byte cEntDList;

        /// <summary>
        /// Padding bytes; MUST be set to zero.
        /// </summary>
        short wPadding;

        /// <summary>
        /// The meaning of this field depends on the value of bFlags. 
        /// If DFL_BACKFILL _COMPLETE is set in bFlags, then this value indicates the AMap page index that 
        /// is used in the next allocation. If DFL_BACKFILL_COMPLETE is not set in bFlags, then this
        /// value indicates the AMap page index that is attempted for backfilling in the next allocation. 
        /// See section 2.6.1.3.4 for more information regarding Backfilling.
        /// </summary>
        int ulCurrentPage;

        /// <summary>
        /// (Unicode: 476 bytes; ANSI: 480 bytes): DList page entries. 
        /// This is an array of DLISTPAGEENT records with cEntDList entries that constitute the DList. 
        /// Each record contains an AMap page index and the aggregate amount of free slots available in that AMap. 
        /// Note that, while the size of the field is fixed, the size of valid data within the field is not. 
        /// Implementations MUST only read the number of DLISTPAGEENT entries from the array indicated by cEntDList
        /// </summary>
        byte[] rgDListPageEnt;

        /// <summary>
        /// (Unicode: 16 bytes; ANSI: 12 bytes): A PAGETRAILER structure (section 2.2.2.7.1). 
        /// The ptype subfield of pageTrailer MUST be set to ptypeDL. The other subfields of pageTrailer MUST be set as specified in section 2.2.2.7.1.
        /// </summary>
        byte[] pageTrailer;
    }
    #endregion

    #region FMapPage
    public interface IFMapPage
    {
    }

    /// <summary>
    /// The general layout of an FMap is identical to that of an AMap, except that each byte in the FMap corresponds to one AMap page. The value of each byte indicates the longest number of free bits found in the corresponding AMap page. Generally, because each AMap covers about 250 kilobytes of data, each FMap page (496 bytes) covers around 125 megabytes of data.
    ///Implementations SHOULD NOT use FMaps. The Density List SHOULD be used for location free space.<16> However, the presence of FMap pages at the correct intervals MUST be preserved, and all corresponding checksums MUST be maintained for a PST file to remain valid.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 512)]
    public struct FMapPageUnicode : IFMapPage
    {
        /// <summary>
        /// AMap data. This is represented as a sequence of bits that marks whether blocks of 64 bytes 
        /// of data have been allocated. If the nth bit is set to 1, then the nth block of 64 bytes 
        /// has been allocated. Alternatively, if the nth bit is set to 0, the nth block of 64 bytes is not allocated (free).
        /// </summary>
        [FieldOffset(0)]
        byte[] rgbFMapBits;

        /// <summary>
        /// pageTrailer (Unicode: 16 bytes; ANSI: 12 bytes): A PAGETRAILER structure (section 2.2.2.7.1). 
        /// The ptype subfield of pageTrailer MUST be set to ptypeAMap. The other subfields of pageTrailer MUST be set as specified in section 2.2.2.7.1.
        /// </summary>
        [FieldOffset(496)]
        byte[] pageTrailer;
    }

    /// <summary>
    /// The general layout of an FMap is identical to that of an AMap, except that each byte in the FMap corresponds to one AMap page. The value of each byte indicates the longest number of free bits found in the corresponding AMap page. Generally, because each AMap covers about 250 kilobytes of data, each FMap page (496 bytes) covers around 125 megabytes of data.
    ///Implementations SHOULD NOT use FMaps. The Density List SHOULD be used for location free space.<16> However, the presence of FMap pages at the correct intervals MUST be preserved, and all corresponding checksums MUST be maintained for a PST file to remain valid.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 512)]
    public struct FMapPageAnsi : IFMapPage
    {
        /// <summary>
        /// dwPadding (ANSI file format only, 4 bytes): Unused padding; MUST be set to zero.
        /// </summary>
        [FieldOffset(0)]
        int dwPadding;

        /// <summary>
        /// FMap data. Each byte represents the maximum number of contiguous "0" 
        /// bits in the corresponding AMap (up to 16 kilobytes).
        /// </summary>
        [FieldOffset(4)]
        byte[] rgbFMapBits;

        /// <summary>
        /// pageTrailer (Unicode: 16 bytes; ANSI: 12 bytes): A PAGETRAILER structure (section 2.2.2.7.1). 
        /// The ptype subfield of pageTrailer MUST be set to ptypeAMap. The other subfields of pageTrailer MUST be set as specified in section 2.2.2.7.1.
        /// </summary>
        [FieldOffset(500)]
        byte[] pageTrailer;
    }
    #endregion

    #region FPMapPage
    public interface IFPMapPage
    {
    }

    [StructLayout(LayoutKind.Explicit, Size = 512)]
    public struct FPMapPageUnicode : IFPMapPage
    {
        /// <summary>
        /// FPMap data. Each bit corresponds to a PMap page. If the nth bit is set to 0, 
        /// then the nth PMap page from the beginning of the PST File has free pages. 
        /// If the nth bit is set to 1, then the nth PMap page has no free pages.
        /// </summary>
        [FieldOffset(0)]
        byte[] rgbFMapBits;

        /// <summary>
        /// A PAGETRAILER structure (section 2.2.2.7.1). The ptype subfield of pageTrailer MUST be set to ptypeFPMap. 
        /// The other subfields of pageTrailer MUST be set as specified in section 2.2.2.7.1.
        /// </summary>
        [FieldOffset(496)]
        byte[] pageTrailer;
    }
    #endregion

    /*
     2.2.2.7.4.1 DLISTPAGEENT
     Each DLISTPAGEENT record in the DList represents a reference to an AMap PAGE in the PST file. 0 1 2 3 4 5 6 7 8 9 1 0 1 2 3 4 5 6 7 8 9 2 0 1 2 3 4 5 6 7 8 9 3 0 1
     dwPageNum
     dwFreeSlots
     dwPageNum (20 bits): AMap page number. This is the zero-based index to the AMap page that corresponds to this entry. A dwPageNum of "n" corresponds to the nth AMap from the beginning of PST file.
     dwFreeSlots (12 bits): Total number of free slots in the AMap. This value is the aggregate sum of all free 64-byte slots in the AMap. Note that the free slots can be of any random configuration, and are not guaranteed to be contiguous.      
     */

    #region BTPage
    public interface IBTPage
    { 
    }

    /// <summary>
    /// A BTPAGE structure implements a generic BTree using 512-byte pages.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size=512)]
    public struct BTPageUnicode : IBTPage
    {
        /// <summary>
        /// (Unicode: 488 bytes; ANSI: 492 bytes): Entries of the BTree array. The entries in the array depend on the value of the cLevel field. If cLevel is greater than 0, then each entry
        /// in the array is of type BTENTRY. If cLevel is 0, then each entry is either of type BBTENTRY or NBTENTRY, depending on the ptype of the page.
        /// </summary>
        [FieldOffset(0)]
        byte[] rgentries;

        /// <summary>
        /// The number of BTree entries stored in the page data.
        /// </summary>
        [FieldOffset(488)]
        byte cEnt;

        /// <summary>
        /// The maximum number of entries that can fit inside the page data.
        /// </summary>
        [FieldOffset(489)]
        byte cEntMax;

        /// <summary>
        /// The size of each BTree entry, in bytes. Note that in some cases, cbEnt can be greater than the corresponding 
        /// size of the corresponding rgentries structure because of alignment or other considerations.
        /// Implementations MUST use the size specified in cbEnt to advance to the next entry.
        /// 
        /// BTree Type cLevel rgentries structure cbEnt (bytes)
        /// NBT     0               NBTENTRY    ANSI: 16, Unicode: 32
        ///         Greater than 0  BTENTRY     ANSI: 12, Unicode: 24
        /// BBT     0               BBTENTRY    ANSI: 12, Unicode: 24
        ///         Less than 0     BTENTRY     ANSI: 12, Unicode: 24
        /// </summary>
        [FieldOffset(490)]
        byte cbEnt;

        /// <summary>
        /// The depth level of this page. Leaf pages have a level of zero, whereas intermediate pages have a 
        /// level greater than 0. This value determines the type of the entries in rgentries, and is interpreted as unsigned.
        /// </summary>
        [FieldOffset(491)]
        byte cLevel;

        /// <summary>
        /// Padding; MUST be set to zero. Note the location of the padding differs between the Unicode and ANSI version of this structure.
        /// </summary>
        [FieldOffset(492)]
        int dwPadding;

        /// <summary>
        /// (Unicode: 16 bytes; ANSI: 12 bytes): A PAGETRAILER structure (section 2.2.2.7.1).
        /// The ptype subfield of pageTrailer MUST be set to ptypeBBT for a Block BTree page, or ptypeNBT for a Node BTree page. 
        /// The other subfields of pageTrailer MUST be set as specified in section 2.2.2.7.1.
        /// </summary>
        [FieldOffset(496)]
        byte[] pageTrailer;

    }

    /// <summary>
    /// A BTPAGE structure implements a generic BTree using 512-byte pages.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 512)]
    public struct BTPageAnsi : IBTPage
    {
        /// <summary>
        /// (Unicode: 488 bytes; ANSI: 492 bytes): Entries of the BTree array. The entries in the array depend on the value of the cLevel field. If cLevel is greater than 0, then each entry
        /// in the array is of type BTENTRY. If cLevel is 0, then each entry is either of type BBTENTRY or NBTENTRY, depending on the ptype of the page.
        /// </summary>
        [FieldOffset(0)]
        byte[] rgentries;

        /// <summary>
        /// The number of BTree entries stored in the page data.
        /// </summary>
        [FieldOffset(492)]
        byte cEnt;

        /// <summary>
        /// The maximum number of entries that can fit inside the page data.
        /// </summary>
        [FieldOffset(493)]
        byte cEntMax;

        /// <summary>
        /// The size of each BTree entry, in bytes. Note that in some cases, cbEnt can be greater than the corresponding 
        /// size of the corresponding rgentries structure because of alignment or other considerations.
        /// Implementations MUST use the size specified in cbEnt to advance to the next entry.
        /// BTree Type cLevel rgentries structure cbEnt (bytes)
        /// NBT     0               NBTENTRY    ANSI: 16, Unicode: 32
        ///         Greater than 0  BTENTRY     ANSI: 12, Unicode: 24
        /// BBT     0               BBTENTRY    ANSI: 12, Unicode: 24
        ///         Less than 0     BTENTRY     ANSI: 12, Unicode: 24
        /// </summary>
        [FieldOffset(494)]
        byte cbEnt;

        /// <summary>
        /// The depth level of this page. Leaf pages have a level of zero, whereas intermediate pages have a 
        /// level greater than 0. This value determines the type of the entries in rgentries, and is interpreted as unsigned.
        /// </summary>
        [FieldOffset(495)]
        byte cLevel;

        /// <summary>
        /// Padding; MUST be set to zero. Note the location of the padding differs between the Unicode and ANSI version of this structure.
        /// </summary>
        [FieldOffset(496)]
        int dwPadding;

        /// <summary>
        /// (Unicode: 16 bytes; ANSI: 12 bytes): A PAGETRAILER structure (section 2.2.2.7.1).
        /// The ptype subfield of pageTrailer MUST be set to ptypeBBT for a Block BTree page, or ptypeNBT for a Node BTree page. 
        /// The other subfields of pageTrailer MUST be set as specified in section 2.2.2.7.1.
        /// </summary>
        [FieldOffset(500)]
        byte[] pageTrailer;

    }
    #endregion

    #region BTEntry
    public interface IBTEntry
    { 
    }

    /// <summary>
    /// (Intermediate Entries)
    /// BTENTRY records contain a key value (NID or BID) and a reference to a child BTPAGE page in the BTree.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct BTEntryUnicode : IBTEntry
    {
        /// <summary>
        /// btkey (Unicode: 8 bytes; ANSI: 4 bytes): The key value associated with this BTENTRY. 
        /// All the entries in the child BTPAGE referenced by BREF have key values greater than or 
        /// equal to this key value. The btkey is either an NID (zero extended to 8 bytes for Unicode PSTs)
        /// or a BID, depending on the ptype of the page.
        /// </summary>
        [FieldOffset(0)]
        long btkey;

        /// <summary>
        /// (Unicode: 16 bytes; ANSI: 8 bytes): BREF structure (section 2.2.2.4) that points to the child BTPAGE.
        /// </summary>
        [FieldOffset(8)]
        BrefUnicode BREF;
    }

    /// <summary>
    /// (Intermediate Entries)
    /// BTENTRY records contain a key value (NID or BID) and a reference to a child BTPAGE page in the BTree.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct BTEntryAnsi : IBTEntry
    {
        /// <summary>
        /// btkey (Unicode: 8 bytes; ANSI: 4 bytes): The key value associated with this BTENTRY. 
        /// All the entries in the child BTPAGE referenced by BREF have key values greater than or 
        /// equal to this key value. The btkey is either an NID (zero extended to 8 bytes for Unicode PSTs)
        /// or a BID, depending on the ptype of the page.
        /// </summary>
        [FieldOffset(0)]
        int btkey;

        /// <summary>
        /// (Unicode: 16 bytes; ANSI: 8 bytes): BREF structure (section 2.2.2.4) that points to the child BTPAGE.
        /// </summary>
        [FieldOffset(4)]
        BrefAnsi BREF;
    }
    #endregion

    #region BBTEntry
    public interface IBBTEntry
    {
    }

    /// <summary>
    /// BBTENTRY records contain information about blocks and are found in BTPAGES with cLevel equal to 0, 
    /// with the ptype of "ptypeBBT". These are the leaf entries of the BBT. As noted in section 2.2.2.7.7.1, 
    /// these structures MAY NOT be tightly packed and the cbEnt field of the BTPAGE SHOULD be used to 
    /// iterate over the entries.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct BBTEntryUnicode : IBBTEntry
    {
        /// <summary>
        /// BREF (Unicode: 16 bytes; ANSI: 8 bytes): BREF structure (section 2.2.2.4) that contains the BID and IB of the block that the BBTENTRY references.
        /// </summary>
        [FieldOffset(0)]
        byte[] BREF;

        /// <summary>
        /// cb (2 bytes): The count of bytes of the raw data contained in the block referenced by BREF excluding the block trailer and alignment padding, if any.
        /// </summary>
        [FieldOffset(16)]
        short cb;

        /// <summary>
        /// cRef (2 bytes): Reference count indicating the count of references to this block. See section 2.2.2.7.7.3.1 regarding how reference counts work.
        /// </summary>
        [FieldOffset(18)]
        short cRef;

        /// <summary>
        /// dwPadding (Unicode file format only, 4 bytes): Padding; MUST be set to zero.
        /// </summary>
        [FieldOffset(20)]
        int dwPadding;

    }

    /// <summary>
    /// BBTENTRY records contain information about blocks and are found in BTPAGES with cLevel equal to 0, 
    /// with the ptype of "ptypeBBT". These are the leaf entries of the BBT. As noted in section 2.2.2.7.7.1, 
    /// these structures MAY NOT be tightly packed and the cbEnt field of the BTPAGE SHOULD be used to 
    /// iterate over the entries.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct BBTEntryAnsi : IBBTEntry
    {
        /// <summary>
        /// BREF (Unicode: 16 bytes; ANSI: 8 bytes): BREF structure (section 2.2.2.4) that contains the BID and IB of the block that the BBTENTRY references.
        /// </summary>
        [FieldOffset(0)]
        long BREF;

        /// <summary>
        /// cb (2 bytes): The count of bytes of the raw data contained in the block referenced by BREF excluding the block trailer and alignment padding, if any.
        /// </summary>
        [FieldOffset(8)]
        short cb;

        /// <summary>
        /// cRef (2 bytes): Reference count indicating the count of references to this block. See section 2.2.2.7.7.3.1 regarding how reference counts work.
        /// </summary>
        [FieldOffset(10)]
        short cRef;

    }
    #endregion

    #region NBTEntry
    /// <summary>
    /// Leaf NBT Entry
    /// </summary>
    public interface INBTEntry
    { }

    /// <summary>
    /// NBTENTRY records contain information about nodes and are found in BTPAGES with cLevel equal to 0, 
    /// with the ptype of ptypeNBT. These are the leaf entries of the NBT.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public struct NBTEntryUnicode : INBTEntry
    {
        /// <summary>
        /// nid (Unicode: 8 bytes; ANSI: 4 bytes): The NID (section 2.2.2.1) of the entry. Note that the NID 
        /// is a 4-byte value for both Unicode and ANSI formats. However, to stay consistent with the 
        /// size of the btkey member in BTENTRY, the 4-byte NID is extended to its 8-byte equivalent for Unicode PST files.
        /// </summary>
        [FieldOffset(0)]
        long nid;

        /// <summary>
        /// bidData (Unicode: 8 bytes; ANSI: 4 bytes): The BID of the data block for this node.
        /// </summary>
        [FieldOffset(8)]
        long bidData;

        /// <summary>
        /// bidSub (Unicode: 8 bytes; ANSI: 4 bytes): The BID of the subnode block for this node. 
        /// If this value is zero, a subnode block does not exist for this node. 
        /// </summary>
        [FieldOffset(16)]
        long bidSub;

        /// <summary>
        /// nidParent (4 bytes): If this node represents a child of a Folder object defined in the Messaging Layer, 
        /// then this value is nonzero and contains the NID of the parent Folder object's node. Otherwise,
        /// this value is zero. See section 2.2.2.7.7.4.1 for more information. 
        /// This field is not interpreted by any structure defined at the NDB Layer.
        /// </summary>
        [FieldOffset(24)]
        int nidParent;


        /// <summary>
        /// dwPadding (Unicode file format only, 4 bytes): Padding; MUST be set to zero.
        /// </summary>
        [FieldOffset(28)]
        int dwPadding;
    }

      /// <summary>
    /// NBTENTRY records contain information about nodes and are found in BTPAGES with cLevel equal to 0, 
    /// with the ptype of ptypeNBT. These are the leaf entries of the NBT.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct NBTEntryANSI : INBTEntry
    {
        /// <summary>
        /// nid (Unicode: 8 bytes; ANSI: 4 bytes): The NID (section 2.2.2.1) of the entry. Note that the NID 
        /// is a 4-byte value for both Unicode and ANSI formats. However, to stay consistent with the 
        /// size of the btkey member in BTENTRY, the 4-byte NID is extended to its 8-byte equivalent for Unicode PST files.
        /// </summary>
        [FieldOffset(0)]
        int nid;

        /// <summary>
        /// bidData (Unicode: 8 bytes; ANSI: 4 bytes): The BID of the data block for this node.
        /// </summary>
        [FieldOffset(4)]
        int bidData;

        /// <summary>
        /// bidSub (Unicode: 8 bytes; ANSI: 4 bytes): The BID of the subnode block for this node. 
        /// If this value is zero, a subnode block does not exist for this node. 
        /// </summary>
        [FieldOffset(8)]
        int bidSub;

        /// <summary>
        /// nidParent (4 bytes): If this node represents a child of a Folder object defined in the Messaging Layer, 
        /// then this value is nonzero and contains the NID of the parent Folder object's node. Otherwise,
        /// this value is zero. See section 2.2.2.7.7.4.1 for more information. 
        /// This field is not interpreted by any structure defined at the NDB Layer.
        /// </summary>
        [FieldOffset(12)]
        int nidParent;
    }
    #endregion

}
