using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Outlook.PST
{
    /*
     Blocks are the fundamental units of data storage at the NDB layer. Blocks are always assigned in sizes that are multiples of 64 bytes and are always aligned on 64-byte boundaries. The maximum size of any block is 8 kilobytes (8192 bytes).
     Similar to pages, each block stores its metadata in a block trailer placed at the very end of the block so that the end of the trailer is aligned with the end of the block.
     Blocks generally fall into one of two categories: data blocks and subnode blocks. Data blocks are used to store raw data, where subnode blocks are used to represent nodes contained within a node.
     The storage capacity of each data block is the size of the data block (from 64 to 8192 bytes) minus the size of the trailer block. 
     */

    public interface IBlockTrailer { }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct BlockTrailerUnicode : IBlockTrailer
    {
        /// <summary>
        /// cb (2 bytes): The amount of data, in bytes, contained within the data section of the block. 
        /// This value does not include the block trailer or any unused bytes that can exist after 
        /// the end of the data and before the start of the block trailer.
        /// </summary>
        [FieldOffset(0)]
        short cb;

        /// <summary>
        /// wSig (2 bytes): Block signature. See section 5.5 for the algorithm to calculate the block signature.
        /// </summary>
        [FieldOffset(2)]
        short wSig;

        /// <summary>
        /// dwCRC (4 bytes): 32-bit CRC of the cb bytes of raw data, see section 5.3 for the algorithm to calculate the CRC.
        /// </summary>
        [FieldOffset(4)]
        int dwCRC;

        /// <summary>
        /// bid (Unicode: 8 bytes; ANSI 4 bytes): The BID (section 2.2.2.2) of the data block.
        /// </summary>
        [FieldOffset(8)]
        long bid;
    }


    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct BlockTrailerAnsi : IBlockTrailer
    {
        /// <summary>
        /// cb (2 bytes): The amount of data, in bytes, contained within the data section of the block. 
        /// This value does not include the block trailer or any unused bytes that can exist after 
        /// the end of the data and before the start of the block trailer.
        /// </summary>
        [FieldOffset(0)]
        short cb;

        /// <summary>
        /// wSig (2 bytes): Block signature. See section 5.5 for the algorithm to calculate the block signature.
        /// </summary>
        [FieldOffset(2)]
        short wSig;

        /// <summary>
        /// dwCRC (4 bytes): 32-bit CRC of the cb bytes of raw data, see section 5.3 for the algorithm to calculate the CRC.
        /// </summary>
        [FieldOffset(4)]
        int dwCRC;

        /// <summary>
        /// bid (Unicode: 8 bytes; ANSI 4 bytes): The BID (section 2.2.2.2) of the data block.
        /// </summary>
        [FieldOffset(8)]
        int bid;
    }


    /// <summary>
    /// A data block is a block that is "External" (that is, not marked "Internal") 
    /// and contains data streamed from higher layer structures. 
    /// The data contained in data blocks have no meaning to the structures defined at the NDB Layer.
    /// </summary>
    public interface IDataBlock
    { 
    }


    public class DataBlockUnicode : IDataBlock
    { 


    }

    public class DataBlockAnsi : IDataBlock
    {


    }

    /// <summary>
    /// XBLOCKs are used when the data associated with a node data that exceeds 8,176 bytes in size. 
    /// The XBLOCK expands the data that is associated with a node by using an array of BIDs 
    /// that reference data blocks that contain the data stream associated with the node. 
    /// A BLOCKTRAILER is present at the end of an XBLOCK, and the end of the BLOCKTRAILER MUST be aligned on a 64-byte boundary.
    /// </summary>
    public interface IXBlock
    { 
    }

    /// <summary>
    /// XBLOCKs are used when the data associated with a node data that exceeds 8,176 bytes in size. 
    /// The XBLOCK expands the data that is associated with a node by using an array of BIDs 
    /// that reference data blocks that contain the data stream associated with the node. 
    /// A BLOCKTRAILER is present at the end of an XBLOCK, and the end of the BLOCKTRAILER MUST be aligned on a 64-byte boundary.
    /// </summary>
    public class XBlockUnicode : IXBlock
    {
        /// <summary>
        /// btype (1 byte): Block type; MUST be set to 0x01 to indicate an XBLOCK or XXBLOCK.
        /// </summary>
        public byte btype;

        /// <summary>
        /// cLevel (1 byte): MUST be set to 0x01 to indicate an XBLOCK.
        /// </summary>
        public byte cLevel;

        /// <summary>
        /// cEnt (2 bytes): The count of BID entries in the XBLOCK.
        /// </summary>
        public short cEnt;

        /// <summary>
        /// lcbTotal (4 bytes): Total count of bytes of all the external data stored in the data blocks referenced by XBLOCK.
        /// </summary>
        public int lcbTotal;

        /// <summary>
        /// rgbid (variable): Array of BIDs that reference data blocks. 
        /// The size is equal to the number of entries indicated by cEnt multiplied 
        /// by the size of a BID (8 bytes for Unicode PST files, 4 bytes for ANSI PST files).
        /// </summary>
        IList<long> rgbid;

        /// <summary>
        /// rgbPadding (variable, optional): This field is present if the total size of all of the other 
        /// fields is not a multiple of 64. The size of this field is the smallest number of bytes required 
        /// to make the size of the XBLOCK a multiple of 64. Implementations MUST ignore this field
        /// </summary>
        IList<int> rgbPadding;

        /// <summary>
        /// blockTrailer (ANSI: 12 bytes; Unicode: 16 bytes): 
        /// A BLOCKTRAILER structure (section 2.2.2.8.1).
        /// </summary>
        public byte[] blockTrailer = new byte[16];
    }

    /// <summary>
    /// XBLOCKs are used when the data associated with a node data that exceeds 8,176 bytes in size. 
    /// The XBLOCK expands the data that is associated with a node by using an array of BIDs 
    /// that reference data blocks that contain the data stream associated with the node. 
    /// A BLOCKTRAILER is present at the end of an XBLOCK, and the end of the BLOCKTRAILER MUST be aligned on a 64-byte boundary.
    /// </summary>
    public class XBlockAnsi : IXBlock
    {
        /// <summary>
        /// btype (1 byte): Block type; MUST be set to 0x01 to indicate an XBLOCK or XXBLOCK.
        /// </summary>
        public byte btype;

        /// <summary>
        /// cLevel (1 byte): MUST be set to 0x01 to indicate an XBLOCK.
        /// </summary>
        public byte cLevel;

        /// <summary>
        /// cEnt (2 bytes): The count of BID entries in the XBLOCK.
        /// </summary>
        public short cEnt;

        /// <summary>
        /// lcbTotal (4 bytes): Total count of bytes of all the external data stored in the data blocks referenced by XBLOCK.
        /// </summary>
        public int lcbTotal;

        /// <summary>
        /// rgbid (variable): Array of BIDs that reference data blocks. 
        /// The size is equal to the number of entries indicated by cEnt multiplied 
        /// by the size of a BID (8 bytes for Unicode PST files, 4 bytes for ANSI PST files).
        /// </summary>
        IList<int> rgbid;

        /// <summary>
        /// rgbPadding (variable, optional): This field is present if the total size of all of the other 
        /// fields is not a multiple of 64. The size of this field is the smallest number of bytes required 
        /// to make the size of the XBLOCK a multiple of 64. Implementations MUST ignore this field
        /// </summary>
        IList<int> rgbPadding;

        /// <summary>
        /// blockTrailer (ANSI: 12 bytes; Unicode: 16 bytes): 
        /// A BLOCKTRAILER structure (section 2.2.2.8.1).
        /// </summary>
        public byte[] blockTrailer = new byte[12];
    }

    public interface IXXBlock { }

    /// <summary>
    /// The XXBLOCK further expands the data that is associated with a node by using an array of 
    /// BIDs that reference XBLOCKs. A BLOCKTRAILER is present at the end of an XXBLOCK, 
    /// and the end of the BLOCKTRAILER MUST be aligned on a 64-byte boundary.
    /// </summary>
    public class XXBlockUnicode : IXXBlock
    {
        /// <summary>
        ///btype (1 byte): Block type; MUST be set to 0x01 to indicate an XBLOCK or XXBLOCK.
        /// </summary>
        public byte btype;

        /// <summary>
        /// cLevel (1 byte): MUST be set to 0x02 to indicate and XXBLOCK.
        /// </summary>
        public byte cLevel;

        /// <summary>
        /// cEnt (2 bytes): The count of BID entries in the XBLOCK.
        /// </summary>
        public short cEnt;

        /// <summary>
        /// lcbTotal (4 bytes): Total count of bytes of all the external data stored in XBLOCKs under this XXBLOCK.
        /// </summary>
        public int lcbTotal;

        /// <summary>
        /// rgbid (variable): Array of BIDs that reference XBLOCKs. The size is equal to the number of entries indicated by cEnt multiplied by the size of a BID (8 bytes for Unicode PST files, 4 bytes for ANSI PST Files).
        /// </summary>
        IList<long> rgbid;

        /// <summary>
        /// rgbPadding (variable, optional): This field is present if the total size of all of the other 
        /// fields is not a multiple of 64. The size of this field is the smallest number of bytes 
        /// required to make the size of the XXBLOCK a multiple of 64. Implementations MUST ignore this field.
        /// </summary>
        IList<int> rgbPadding;

        /// <summary>
        /// blockTrailer (ANSI: 12 bytes; Unicode: 16 bytes): 
        /// A BLOCKTRAILER structure (section 2.2.2.8.1).
        /// </summary>
        public byte[] blockTrailer = new byte[16];
    }


    /// <summary>
    /// The XXBLOCK further expands the data that is associated with a node by using an array of 
    /// BIDs that reference XBLOCKs. A BLOCKTRAILER is present at the end of an XXBLOCK, 
    /// and the end of the BLOCKTRAILER MUST be aligned on a 64-byte boundary.
    /// </summary>
    public class XXBlockAnsi : IXXBlock
    {
        /// <summary>
        ///btype (1 byte): Block type; MUST be set to 0x01 to indicate an XBLOCK or XXBLOCK.
        /// </summary>
        public byte btype;

        /// <summary>
        /// cLevel (1 byte): MUST be set to 0x02 to indicate and XXBLOCK.
        /// </summary>
        public byte cLevel;

        /// <summary>
        /// cEnt (2 bytes): The count of BID entries in the XBLOCK.
        /// </summary>
        public short cEnt;

        /// <summary>
        /// lcbTotal (4 bytes): Total count of bytes of all the external data stored in XBLOCKs under this XXBLOCK.
        /// </summary>
        public int lcbTotal;

        /// <summary>
        /// rgbid (variable): Array of BIDs that reference XBLOCKs. 
        /// The size is equal to the number of entries indicated by cEnt multiplied by the size of a BID 
        /// (8 bytes for Unicode PST files, 4 bytes for ANSI PST Files).
        /// </summary>
        IList<int> rgbid;

        /// <summary>
        /// rgbPadding (variable, optional): This field is present if the total size of all of the other 
        /// fields is not a multiple of 64. The size of this field is the smallest number of bytes 
        /// required to make the size of the XXBLOCK a multiple of 64. Implementations MUST ignore this field.
        /// </summary>
        IList<int> rgbPadding;

        /// <summary>
        /// blockTrailer (ANSI: 12 bytes; Unicode: 16 bytes): 
        /// A BLOCKTRAILER structure (section 2.2.2.8.1).
        /// </summary>
        public byte[] blockTrailer = new byte[12];
    }


    public interface ISLEntry { }

    /// <summary>
    /// SLENTRY are records that refer to internal subnodes of a node.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size= 24)]
    public struct SLEntryUnicode : ISLEntry
    {      
        /// <summary>
        /// nid (Unicode: 8 bytes; ANSI: 4 bytes): Local NID of the subnode. This NID is guaranteed to be unique only within the parent node.
        /// </summary>
        [FieldOffset(0)]
        long nid;

        /// <summary>
        /// bidData (Unicode: 8 bytes; ANSI: 4 bytes): The BID of the data block associated with the subnode.
        /// </summary>
        [FieldOffset(8)]
        long bidData;

        /// <summary>
        /// bidSub (Unicode: 8 bytes; ANSI: 4 bytes): If nonzero, the BID of the subnode of this subnode.
        /// </summary>
        [FieldOffset(16)]
        long bidSub;
    }

    /// <summary>
    /// SLENTRY are records that refer to internal subnodes of a node.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct SLEntryAnsi : ISLEntry
    {
        /// <summary>
        /// nid (Unicode: 8 bytes; ANSI: 4 bytes): Local NID of the subnode. This NID is guaranteed to be unique only within the parent node.
        /// </summary>
        [FieldOffset(0)]
        int nid;

        /// <summary>
        /// bidData (Unicode: 8 bytes; ANSI: 4 bytes): The BID of the data block associated with the subnode.
        /// </summary>
        [FieldOffset(4)]
        int bidData;

        /// <summary>
        /// bidSub (Unicode: 8 bytes; ANSI: 4 bytes): If nonzero, the BID of the subnode of this subnode.
        /// </summary>
        [FieldOffset(8)]
        int bidSub;
    }




    public interface ISLBlock { }

    /// <summary>
    /// The XXBLOCK further expands the data that is associated with a node by using an array of 
    /// BIDs that reference XBLOCKs. A BLOCKTRAILER is present at the end of an XXBLOCK, 
    /// and the end of the BLOCKTRAILER MUST be aligned on a 64-byte boundary.
    /// </summary>
    public class SLBlockUnicode : ISLBlock
    {
        /// <summary>
        ///btype (1 byte): Block type; MUST be set to 0x02.
        /// </summary>
        public byte btype;

        /// <summary>
        ///cLevel (1 byte): MUST be set to 0x00.
        /// </summary>
        public byte cLevel;

        /// <summary>
        /// cEnt (2 bytes): The number of SLENTRYs in the SLBLOCK.
        /// </summary>
        public short cEnt;

        /// <summary>
        /// dwPadding (4 bytes): Padding; MUST be set to zero.
        /// </summary>
        public int dwPadding;

        /// <summary>
        /// rgentries (variable size): Array of SLENTRY structures. The size is equal to the number of entries indicated by cEnt multiplied by the size of an SLENTRY (24 bytes for Unicode PST files, 12 bytes for ANSI PST Files).
        /// </summary>
        IList<SLEntryUnicode> rgentries;

        /// <summary>
        /// rgbPadding (optional, variable): This field is present if the total size of all of the other fields 
        /// is not a multiple of 64. The size of this field is the smallest number of bytes
        /// required to make the size of the SLBLOCK a multiple of 64. Implementations MUST ignore this field.
        /// </summary>
        IList<int> rgbPadding;

        /// <summary>
        /// blockTrailer (ANSI: 12 bytes; Unicode: 16 bytes): 
        /// A BLOCKTRAILER structure (section 2.2.2.8.1).
        /// </summary>
        public byte[] blockTrailer = new byte[16];
    }

    /// <summary>
    /// The XXBLOCK further expands the data that is associated with a node by using an array of 
    /// BIDs that reference XBLOCKs. A BLOCKTRAILER is present at the end of an XXBLOCK, 
    /// and the end of the BLOCKTRAILER MUST be aligned on a 64-byte boundary.
    /// </summary>
    public class SLBlockAnsi : ISLBlock
    {
        /// <summary>
        ///btype (1 byte): Block type; MUST be set to 0x02.
        /// </summary>
        public byte btype;

        /// <summary>
        ///cLevel (1 byte): MUST be set to 0x00.
        /// </summary>
        public byte cLevel;

        /// <summary>
        /// cEnt (2 bytes): The number of SLENTRYs in the SLBLOCK.
        /// </summary>
        public short cEnt;

        /// <summary>
        /// dwPadding (4 bytes): Padding; MUST be set to zero.
        /// </summary>
        public int dwPadding;

        /// <summary>
        /// rgentries (variable size): Array of SLENTRY structures. The size is equal to the number of entries indicated by cEnt multiplied by the size of an SLENTRY (24 bytes for Unicode PST files, 12 bytes for ANSI PST Files).
        /// </summary>
        IList<SLEntryAnsi> rgentries;

        /// <summary>
        /// rgbPadding (optional, variable): This field is present if the total size of all of the other fields 
        /// is not a multiple of 64. The size of this field is the smallest number of bytes
        /// required to make the size of the SLBLOCK a multiple of 64. Implementations MUST ignore this field.
        /// </summary>
        IList<int> rgbPadding;

        /// <summary>
        /// blockTrailer (ANSI: 12 bytes; Unicode: 16 bytes): 
        /// A BLOCKTRAILER structure (section 2.2.2.8.1).
        /// </summary>
        public byte[] blockTrailer = new byte[12];
    }


    public interface ISIEentry { }

    public struct SIEntryUnicode : ISIEentry
    {
        long nid;

        long bid;
    }

    public struct SIEntryAnsi : ISIEentry
    {
        int nid;

        int bid;
    }


    public interface ISIBlock { }

    /// <summary>
    /// The XXBLOCK further expands the data that is associated with a node by using an array of 
    /// BIDs that reference XBLOCKs. A BLOCKTRAILER is present at the end of an XXBLOCK, 
    /// and the end of the BLOCKTRAILER MUST be aligned on a 64-byte boundary.
    /// </summary>
    public class SIBlockUnicode : ISIBlock
    {
        /// <summary>
        ///btype (1 byte): Block type; MUST be set to 0x02.
        /// </summary>
        public byte btype;

        /// <summary>
        ///cLevel (1 byte): MUST be set to 0x01.
        /// </summary>
        public byte cLevel;

        /// <summary>
        /// cEnt (2 bytes): The number of SIENTRYs in the SIBLOCK.
        /// </summary>
        public short cEnt;

        /// <summary>
        /// dwPadding (4 bytes): Padding; MUST be set to zero.
        /// </summary>
        public int dwPadding;

        /// <summary>
        /// rgentries (variable size): Array of SIENTRY structures. The size is equal to the number of entries indicated by cEnt multiplied by the size of an SIENTRY (16 bytes for Unicode PST files, 8 bytes for ANSI PST Files).
        /// </summary>
        IList<SIEntryUnicode> rgentries;

        /// <summary>
        /// rgbPadding (optional, variable): This field is present if the total size of all of the other fields is not a multiple of 64. The size of this field is the smallest number of bytes required to make the size of the SIBLOCK a multiple of 64. Implementations MUST ignore this field.
        /// </summary>
        IList<int> rgbPadding;

        /// <summary>
        /// blockTrailer (ANSI: 12 bytes; Unicode: 16 bytes): 
        /// A BLOCKTRAILER structure (section 2.2.2.8.1).
        /// </summary>
        public byte[] blockTrailer = new byte[16];
    }

    /// <summary>
    /// The XXBLOCK further expands the data that is associated with a node by using an array of 
    /// BIDs that reference XBLOCKs. A BLOCKTRAILER is present at the end of an XXBLOCK, 
    /// and the end of the BLOCKTRAILER MUST be aligned on a 64-byte boundary.
    /// </summary>
    public class SIBlockAnsi : ISIBlock
    {
        /// <summary>
        ///btype (1 byte): Block type; MUST be set to 0x02.
        /// </summary>
        public byte btype;

        /// <summary>
        ///cLevel (1 byte): MUST be set to 0x01.
        /// </summary>
        public byte cLevel;

        /// <summary>
        /// cEnt (2 bytes): The number of SIENTRYs in the SIBLOCK.
        /// </summary>
        public short cEnt;

        /// <summary>
        /// dwPadding (4 bytes): Padding; MUST be set to zero.
        /// </summary>
        public int dwPadding;

        /// <summary>
        /// rgentries (variable size): Array of SIENTRY structures. The size is equal to the number of entries indicated by cEnt multiplied by the size of an SIENTRY (16 bytes for Unicode PST files, 8 bytes for ANSI PST Files).
        /// </summary>
        IList<SIEntryAnsi> rgentries;

        /// <summary>
        /// rgbPadding (optional, variable): This field is present if the total size of all of the other fields is not a multiple of 64. The size of this field is the smallest number of bytes required to make the size of the SIBLOCK a multiple of 64. Implementations MUST ignore this field.
        /// </summary>
        IList<int> rgbPadding;

        /// <summary>
        /// blockTrailer (ANSI: 12 bytes; Unicode: 16 bytes): 
        /// A BLOCKTRAILER structure (section 2.2.2.8.1).
        /// </summary>
        public byte[] blockTrailer = new byte[12];
    }



}
