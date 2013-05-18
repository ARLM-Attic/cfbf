using System.Runtime.InteropServices;

namespace Outlook.PST
{

    /// <summary>
    /// Each block is uniquely identified in the PST file using its BID value. 
    /// The indexes of BIDs are assigned in a monotonically increasing fashion so 
    /// that it is possible to establish the order in which blocks were created by examining the BIDs.
    /// 
    /// A - r (1 bit): Reserved bit. Readers MUST ignore this bit and treat it as zero before looking up the BID from the BBT. Writers MUST set this bit to zero.<4>
    /// B - i (1 bit): MUST set to 1 when the block is "Internal", or zero when the block is not "Internal". An internal block is an intermediate block that, instead of containing actual data, contains metadata about how to locate other data blocks that contain the desired information. For more details about technical details regarding blocks, see section 2.2.2.8.
    /// bidIndex (Unicode: 62 bits; ANSI: 30 bits): A monotonically increasing value that uniquely identifies the BID within the PST file. bidIndex values are assigned based on the bidNextB value in the HEADER structure (see section 2.1.2.3). The bidIndex increments by one each time a new BID is assigned.
    /// </summary>
    public interface IBid
    {

    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct BidUnicode : IBid
    {
        [FieldOffset(0)]
        public long bidIndex;


        public byte A {
            get { return (byte)(bidIndex>>61); }
        }

        /// <summary>
        /// B - i (1 bit): MUST set to 1 when the block is "Internal", or zero when the block is not "Internal". 
        /// An internal block is an intermediate block that, instead of containing actual data, 
        /// contains metadata about how to locate other data blocks that contain the desired information. 
        /// For more details about technical details regarding blocks, see section
        /// </summary>
        public BlockType B
        {
            get { return (BlockType)((bidIndex << 1) >> 60); }
        }

        public long BidIndex
        {
            get { return (bidIndex<<2)>>2; }
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct BidAnsi : IBid
    {
        [FieldOffset(0)]
        public int bidIndex;

    }
}
