namespace Outlook.PST.Layers
{
    /// <summary>
    /// The NDB layer consists of a database of nodes, which represents the lower-level storage facilities of the PST file format. 
    /// From an implementation standpoint, the NDB layer consists of the header, file allocation information, blocks, nodes, 
    /// and two BTrees: the Node BTree (NBT) and the Block BTree (BBT).
    /// The NBT contains references to all of the accessible nodes in the PST file. Its BTree implementation allows for efficient
    ///  searches to locate any specific node. Each node reference is represented using a set of four properties that includes its 
    /// NID, parent NID, data BID, and subnode BID. The data BID points to the block that contains the data associated with the 
    /// node, and the subnode BID points to the block that contains references to subnodes of this node. Top-level NIDs are 
    /// unique across the PST and are searchable from the NBT. Subnode NIDs are only unique within a node and are not searchable 
    /// (or found) from the NBT. The parent NID is an optimization for the higher layers and has no meaning for the NDB Layer.
    /// The BBT contains references to all of the data blocks of the PST file. Its BTree implementation allows for efficient 
    /// searches to locate any specific block. A block reference is represented using a set of four properties, which includes 
    /// its BID, IB, CB, and CREF. The IB is the offset within the file where the block is located. The CB is the count of bytes 
    /// stored within the block. The CREF is the count of references to the data stored within the block.
    /// The roots of the NBT and BBT can be accessed from the header of the PST file.
    /// The following diagram illustrates the high-level relationship between nodes and blocks.
    /// </summary>
    public class NDBLayer :ILayer
    {
    }
}
