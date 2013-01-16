using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.CFBF
{
    /// <summary>
    /// 
    /// </summary>
    public class InvalidCFBFException : Exception
    {
        public InvalidCFBFException()
            : base()
        {           
        }

        public InvalidCFBFException(string message)
            : base(message)
        {
        }

        public InvalidCFBFException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InvalidCFBFException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
