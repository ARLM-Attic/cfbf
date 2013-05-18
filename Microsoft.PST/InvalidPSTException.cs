using System;
using System.Runtime.Serialization;

namespace Outlook.PST
{
    public class InvalidPSTException : Exception
    {
        public InvalidPSTException() : base()
        {
        }

        public InvalidPSTException(string message)
            : base(message
            )
        {
        }

        public InvalidPSTException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        
        public InvalidPSTException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
