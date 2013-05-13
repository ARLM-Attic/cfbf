
using System.ComponentModel;
namespace System.IO.CFBF
{

    /// <summary>
    /// 
    /// </summary>
    public enum ObjectType : byte
    {
        UNKNOWN_OR_UNALLOCATED = 0x00,

        STORAGE_OBJECT = 0x01,

        STREAM_OBJECT = 0x02,

        LOCKBYTES = 0x03,
        
        PROPERTY = 0x04,

        ROOT_STORAGE_OBJECT = 0x05
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ColorFlag : byte
    {
        RED = 0x00,

        BLACK = 0x01,
    }

    /// <summary>
    /// Each sector, except for the header, is identified by a non-negative 32-bit sector number. 
    /// The following sector numbers above 0xFFFFFFFA are reserved, and MUST NOT be used to 
    /// identify the location of a sector in a compound file.
    /// </summary>
    public enum SectorName : uint
    {
        /// <summary>
        /// Regular sector number
        /// </summary>
        REGSECT,
        
        /// <summary>
        /// Maximum regular sector number
        /// </summary>
        MAXREGSECT = 0xFFFFFFFA, //-1

        /// <summary>
        /// End of linked chain of sectors
        /// –2 End Of Chain SecID Trailing SecID in a SecID chain
        /// </summary>
        ENDOFCHAIN = 0xFFFFFFFE, //-2

        /// <summary>
        /// Specifies unallocated sector in the FAT, Mini FAT, or DIFAT
        /// –1 Free SecID Free sector, may exist in the file, but is not part of any stream
        /// </summary>
        FREESECT = 0xFFFFFFFF, //-1

        /// <summary>
        /// Specifies a FAT sector in the FAT
        /// –3 SAT SecID Sector is used by the sector allocation table
        /// </summary>
        FATSECT = 0xFFFFFFFD, //-3

        /// <summary>
        /// Specifies a DIFAT sector in the FAT
        /// –4 MSAT SecID Sector is used by the master sector allocation table
        /// </summary>
        DIFSECT = 0xFFFFFFC, //-4
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ByteOrder : ushort
    { 
        LITTLE_ENDIAN = 0xFFFE,

        BIG_ENDIAN = 0xFEFF
    }
}
