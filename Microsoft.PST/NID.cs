using System;
using System.Runtime.InteropServices;

namespace Outlook.PST
{
    /// <summary>
    /// NID (Node ID)
    /// Nodes provide the primary abstraction used to reference data stored in the PST file that is not 
    /// interpreted by the NDB layer. Each node is identified using its NID. Each NID is unique within 
    /// the namespace in which it is used. Each node referenced by the NBT MUST have a unique NID. 
    /// However, two subnodes of two different nodes can have identical NIDs, but two subnodes of the same node MUST have different NIDs.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size= 4)]
    public struct NID
    {
        /// <summary>
        /// nidType (5 bits): Identifies the type of the node represented by the NID. 
        /// The following table specifies a list of values for nidType. 
        /// However, it is worth noting that nidType has no meaning to the structures defined in the NDB Layer.
        /// 
        /// nidIndex (27 bits): The identification portion of the NID.
        /// </summary>
        [FieldOffset(0)]
        public int nid;


        public NidType nidType()
        {
            byte[] _nidArray = BitConverter.GetBytes(nid);            
            return (NidType)(_nidArray[0] >> 3);
        }

        public int nidIndex()
        {            
            return (nid<<5)>>5;
        }

    }

    /// <summary>
    /// This section focuses on a special NID_TYPE: NID_TYPE_INTERNAL (0x01). As specified in section 2.2.2.1, 
    /// the nidType of an NID is ignored by the NDB Layer, and is left for the interpretation by higher level implementations.
    /// In the Messaging layer, nodes with various nidType values are also used to build related structures that collectively 
    /// represent complex structures (for example, a Folder object is a composite object that consists of a PC and three 
    /// TCs of various nidType values). In addition, the Messaging layer also uses NID_TYPE_INTERNAL to define special
    /// NIDs that have special functions.
    /// Because top-level NIDs are globally-unique within a PST, it follows that each instance of a 
    /// special NID can only appear once in a PST. The following table lists all predefined internal NIDs.
    /// </summary>
    public enum SpecialInternalNID
    {
        
        NID_MESSAGE_STORE = 0x21,
        
        NID_NAME_TO_ID_MAP = 0x61,

        NID_NORMAL_FOLDER_TEMPLATE = 0xA1,

        NID_SEARCH_FOLDER_TEMPLATE = 0xC1,

        NID_ROOT_FOLDER = 0x122,

        NID_SEARCH_MANAGEMENT_QUEUE = 0x1E1,

        NID_SEARCH_ACTIVITY_LIST = 0x201,

        NID_RESERVED1 = 0x241,

        NID_SEARCH_DOMAIN_OBJECT = 0x261,

        NID_SEARCH_GATHERER_QUEUE = 0x281,

        NID_SEARCH_GATHERER_DESCRIPTOR = 0x2A1,

        NID_RESERVED2 = 0x2E1,

        NID_RESERVED3 = 0x301,

        NID_SEARCH_GATHERER_FOLDER_QUEUE = 0x321,
    }
}
