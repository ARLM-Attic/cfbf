namespace Outlook.PST
{
    public enum PType : byte
    {
        ptypeBBT = 0x80,
        ptypeNBT = 0x81,
        ptypeFMap = 0x82,
        ptypePMap = 0x83,
        ptypeAMap = 0x84,
        ptypeFPMap = 0x85,
        ptypeDL = 0x86,
    }
    
    
    public enum HeaderType
    {
        ANSI_PST,
        UNICODE_PST,
    }
    
    
    public enum CryptMethodType : byte
    {
        NDB_CRYPT_NONE = 0x00,
        NDB_CRYPT_PERMUTE = 0x01,
        NDB_CRYPT_CYCLIC = 0x02,
    }

    public enum MapValidType : byte
    {
        INVALID_AMAP = 0x00,
        VALID_AMAP1 = 0x01,
        VALID_AMAP2 = 0x02,
    }

    public enum NidType : byte
    {
        NID_TYPE_HID = 0x00,
        NID_TYPE_INTERNAL = 0x01,
        NID_TYPE_NORMAL_FOLDER = 0x02,
        NID_TYPE_SEARCH_FOLDER = 0x03,
        NID_TYPE_NORMAL_MESSAGE = 0x04,
        NID_TYPE_ATTACHMENT = 0x05,
        NID_TYPE_SEARCH_UPDATE_QUEUE = 0x06,
        NID_TYPE_SEARCH_CRITERIA_OBJECT = 0x07,
        NID_TYPE_ASSOC_MESSAGE = 0x08,
        NID_TYPE_CONTENTS_TABLE_INDEX = 0x0A,
        NID_TYPE_RECEIVE_FOLDER_TABLE = 0x0B,
        NID_TYPE_OUTGOING_QUEUE_TABLE = 0x0C,
        NID_TYPE_HIERARCHY_TABLE = 0x0D,
        NID_TYPE_CONTENTS_TABLE = 0x0E,
        NID_TYPE_ASSOC_CONTENTS_TABLE = 0x0F,
        NID_TYPE_SEARCH_CONTENTS_TABLE = 0x010,
        NID_TYPE_ATTACHMENT_TABLE = 0x11,
        NID_TYPE_RECIPIENT_TABLE = 0x12,
        NID_TYPE_SEARCH_TABLE_INDEX = 0x13,
        NID_TYPE_LTP = 0x1F,
    }

    public enum BlockType : byte
    {
        INTERNAL = 1,

        EXTERNAL = 0,
    }

    public enum RowMatrixLayout : byte
    {
        /// <summary>
        /// Ending offset of 8- and 4-byte data value group.
        /// </summary>
        TCI_4b = 0,

        /// <summary>
        /// Ending offset of 2-byte data value group.
        /// </summary>
        TCI_2b = 1,

        /// <summary>
        /// Ending offset of 1-byte data value group.
        /// </summary>
        TCI_1b = 2,

        /// <summary>
        /// Ending offset of the Cell Existence Block.
        /// </summary>
        TCI_bm = 3
    }

    public enum ClientSignature : byte
    {
        bTypeReserved1 = 0x6C,

        /// <summary>
        /// Table Context (TC/HN)
        /// </summary>
        bTypeTC = 0x7C,

        bTypeReserved2 = 0x8C,

        bTypeReserved3 = 0x9C,

        bTypeReserved4 = 0xA5,

        bTypeReserved5 = 0xAC,

        /// <summary>
        /// BTree-on-Heap (BTH)
        /// </summary>
        bTypeBTH = 0xB5,

        /// <summary>
        /// Property Context (PC/BTH)
        /// </summary>
        bTypePC = 0xBC,

        bTypeReserved6 = 0xCC,
    }

    
}
