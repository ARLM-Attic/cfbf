
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

}
