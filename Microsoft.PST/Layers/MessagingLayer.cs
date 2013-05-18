namespace Outlook.PST.Layers
{
    /// <summary>
    /// The Messaging layer consists of the higher-level rules and business logic that allow 
    /// the structures of the LTP and NDB layers to be combined and interpreted as Folder objects,
    /// Message objects, Attachment objects, and properties. The Messaging layer also defines 
    /// the rules and requirements that need to be followed when modifying the contents of a 
    /// PST file so that the modified PST file can still be successfully read by implementations of this protocol
    /// </summary>
    public class MessagingLayer : ILayer
    {
    }
}
