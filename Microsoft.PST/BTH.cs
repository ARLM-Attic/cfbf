using System.Runtime.InteropServices;

namespace Outlook.PST
{
    /// <summary>
    /// The BTHHEADER contains the BTH metadata, which instructs the reader how to access the other objects of the BTH structure.
    /// Unicode / ANSI:
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct BTHHEADER
    {
        /// <summary>
        /// bType (1 byte): MUST be bTypeBTH.
        /// </summary>
        [FieldOffset(0)]
        byte bType;

        /// <summary>
        /// cbKey (1 byte): Size of the BTree Key value, in bytes. This value MUST be set to 2, 4, 8, or 16.
        /// </summary>
        [FieldOffset(1)]
        byte cbKey;

        /// <summary>
        /// cbEnt (1 byte): Size of the data value, in bytes. This MUST be greater than zero and less than or equal to 32.
        /// </summary>
        [FieldOffset(2)]
        byte cbEnt;


        /// <summary>
        /// bIdxLevels (1 byte): Index depth. This number indicates how many levels of intermediate 
        /// indices exist in the BTH. Note that this number is zero-based, meaning that a 
        /// value of zero actually means that the BTH has one level of indices. 
        /// If this value is greater than zero, then its value indicates how many intermediate index levels are present.
        /// </summary>
        [FieldOffset(3)]
        byte bIdxLevels;


        /// <summary>
        /// hidRoot (4 bytes): This is the HID that points to the BTH entries for this BTHHEADER.
        /// The data consists of an array of BTH records. This value is set to zero if the BTH is empty.
        /// </summary>
        [FieldOffset(4)]
        int hidRoot;
    }


    /// <summary>
    /// Index records do not contain actual data, but point to other index records or leaf records. 
    /// The format of the intermediate index record is as follows. 
    /// The number of index records can be determined based on the size of the heap allocation.
    /// Unicode / ANSI:
    /// </summary>
    public struct IntermediateBTHIndexRecords
    {
        /// <summary>
        /// key (variable): This is the key of the first record in the next level index record array. 
        /// The size of the key is specified in the cbKey field in the corresponding BTHHEADER structure (section 2.3.2.1). 
        /// The size and contents of the key are specific to the higher level structure that implements this BTH.
        /// </summary>
        byte[] key;

        /// <summary>
        /// hidNextLevel (4 bytes): HID of the next level index record array. 
        /// This contains the HID of the heap item that contains the next level index record array.
        /// </summary>
        int hidNextLevel;
    }


    /// <summary>
    /// Leaf BTH records contain the actual data associated with each key entry. 
    /// The BTH records are tightly packed (that is, byte-aligned), and each record is exactly cbKey + cbEnt bytes in size. 
    /// The number of data records can be determined based on the size of the heap allocation.
    /// Unicode / ANSI:
    /// </summary>
    public struct LeafBTHDataRecords
    {
        /// <summary>
        /// key (variable): This is the key of the record. 
        /// The size of the key is specified in the cbKey field in the corresponding BTHHEADER structure(section 2.3.2.1). 
        /// The size and contents of the key are specific to the higher level structure that implements this BTH.
        /// </summary>
        byte[] key;

        /// <summary>
        /// data (variable): This contains the actual data associated with the key. 
        /// The size of the data is specified in the cbEnt field in the corresponding BTHHEADER structure. 
        /// The size and contents of the data are specific to the higher level structure that implements this BTH.
        /// </summary>
        byte[] data;
    }


}
