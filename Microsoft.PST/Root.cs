using System.Runtime.InteropServices;

namespace Outlook.PST
{
    /// <summary>
    /// The ROOT structure contains current file state.
    /// </summary>
    public interface IRoot
    { 
    }
    
    [StructLayout(LayoutKind.Explicit, Size=72)]
    public struct RootUnicode : IRoot
    {
        /// <summary>
        /// dwReserved (4 bytes): Implementations SHOULD ignore this value and SHOULD NOT modify it. 
        /// Creators of a new PST file MUST initialize this value to zero.<
        /// </summary>
        [FieldOffset(0)]
        public int dwReserved;

        /// <summary>
        /// ibFileEof (Unicode: 8 bytes; ANSI 4 bytes): The size of the PST file, in bytes.
        /// </summary>
        [FieldOffset(4)]
        public long ibFileEof;

        /// <summary>
        /// ibAMapLast (Unicode: 8 bytes; ANSI 4 bytes): An IB structure (section 2.2.2.3) that contains 
        /// the absolute file offset to the last AMap page of the PST file.
        /// </summary>
        [FieldOffset(12)]
        public long ibAMapLast;

        /// <summary>
        /// (Unicode: 8 bytes; ANSI 4 bytes): The total free space in all AMaps, combined.
        /// </summary>
        [FieldOffset(20)]
        public long cbAMapFree;

        /// <summary>
        /// (Unicode: 8 bytes; ANSI 4 bytes): The total free space in all PMaps, combined. 
        /// Because the PMap is deprecated, this value SHOULD be zero. Creators of new PST 
        /// files MUST initialize this value to zero.
        /// </summary>
        [FieldOffset(28)]
        public long cbPMapFree;

        /// <summary>
        /// (Unicode: 16 bytes; ANSI: 8 bytes): 
        /// A BREF structure (section 2.2.2.4) that references the root page of the Node BTree (NBT).
        /// </summary>
        [FieldOffset(36)]
        public BrefUnicode BREFNBT;


        /// <summary>
        /// BREFBBT (Unicode: 16 bytes; ANSI: 8 bytes): A BREF structure that references the root page of the Block BTree (BBT).
        /// </summary>
        [FieldOffset(52)]
        public BrefUnicode BREFBBT;

        /// <summary>
        /// Indicates whether all of the AMaps in this PST file are valid. For more details, see section 2.6.1.3.7.
        /// </summary>
        [FieldOffset(68)]
        public MapValidType fAMapValid;
         
        /// <summary>
        /// bReserved (1 byte): Implementations SHOULD ignore this value and SHOULD NOT modify it. 
        /// Creators of a new PST file MUST initialize this value to zero
        /// </summary>
        [FieldOffset(69)]
        public byte bReserved;

        /// <summary>
        /// wReserved (2 bytes): Implementations SHOULD ignore this value and SHOULD NOT modify it. 
        /// Creators of a new PST file MUST initialize this value to zero.
        /// </summary>
        [FieldOffset(70)]
        public short wReserved;

    }
        
    public struct RootAnsi : IRoot
    {
        /// <summary>
        /// dwReserved (4 bytes): Implementations SHOULD ignore this value and SHOULD NOT modify it. 
        /// Creators of a new PST file MUST initialize this value to zero.
        /// </summary>
        public int dwReserved;

        /// <summary>
        /// ibFileEof (Unicode: 8 bytes; ANSI 4 bytes): The size of the PST file, in bytes.
        /// </summary>
        public int ibFileEof;

        /// <summary>
        /// ibAMapLast (Unicode: 8 bytes; ANSI 4 bytes): An IB structure (section 2.2.2.3) that contains 
        /// the absolute file offset to the last AMap page of the PST file.
        /// </summary>
        public int ibAMapLast;

        /// <summary>
        /// (Unicode: 8 bytes; ANSI 4 bytes): The total free space in all AMaps, combined.
        /// </summary>
        public int ibAMapFree;

        /// <summary>
        /// (Unicode: 8 bytes; ANSI 4 bytes): The total free space in all PMaps, combined. 
        /// Because the PMap is deprecated, this value SHOULD be zero. Creators of new PST 
        /// files MUST initialize this value to zero.
        /// </summary>
        public int cbPMapFree;

        /// <summary>
        /// (Unicode: 16 bytes; ANSI: 8 bytes): 
        /// A BREF structure (section 2.2.2.4) that references the root page of the Node BTree (NBT).
        /// </summary>
        public BrefAnsi BREFNBT;


        /// <summary>
        /// BREFBBT (Unicode: 16 bytes; ANSI: 8 bytes): A BREF structure that references the root page of the Block BTree (BBT).
        /// </summary>
        public BrefAnsi BREFBBT;

        /// <summary>
        /// Indicates whether all of the AMaps in this PST file are valid. For more details, see section 2.6.1.3.7.
        /// </summary>
        public MapValidType fAMapValid;

        /// <summary>
        /// bReserved (1 byte): Implementations SHOULD ignore this value and SHOULD NOT modify it. 
        /// Creators of a new PST file MUST initialize this value to zero
        /// </summary>
        public byte bReserved;

        /// <summary>
        /// wReserved (2 bytes): Implementations SHOULD ignore this value and SHOULD NOT modify it. 
        /// Creators of a new PST file MUST initialize this value to zero.
        /// </summary>
        public short wReserved;
    }
}
