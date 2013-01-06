using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.CFBF
{
    public struct Consts
    {       
        public const uint MAXREGSET = 0xFFFFFFFA;

        public const uint ENDOFCHAIN = 0xFFFFFFFE;

        public const uint FREESECT = 0xFFFFFFFF;

        public const uint FATSECT = 0xFFFFFFFD;

        public const uint DIFSEC = 0xFFFFFFC;

        public const ushort HEADER_OFFSET = 0x200;
    }
}
