using System.Runtime.InteropServices;

namespace Outlook.PST
{
    /// <summary>
    /// bid (Unicode: 64 bits; ANSI: 32 bits): A BID structure, as specified in section 2.2.2.2.
    /// ib  (Unicode: 64 bits; ANSI: 32 bits): An IB structure, as specified in section 2.2.2.3.
    /// </summary>
    public interface IBref
    {
    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct BrefUnicode : IBref
    {
        /// <summary>
        /// (Unicode: 64 bits; ANSI: 32 bits): A BID structure, as specified in section
        /// </summary>
        [FieldOffset(0)]
        public BidUnicode bid;

        /// <summary>
        /// (Unicode: 64 bits; ANSI: 32 bits): An IB structure, as specified in section
        /// </summary>
        [FieldOffset(8)]
        public long ib;
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct BrefAnsi : IBref
    {
        /// <summary>
        /// (Unicode: 64 bits; ANSI: 32 bits): A BID structure, as specified in section
        /// </summary>
        [FieldOffset(0)]
        public int bid;

        /// <summary>
        /// (Unicode: 64 bits; ANSI: 32 bits): An IB structure, as specified in section
        /// </summary>
        [FieldOffset(4)]
        public int ib;
    }
}
