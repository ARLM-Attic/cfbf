using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outlook.PST
{
    public class Util
    {

        //RowsPerBlock = Floor((sizeof(block) – sizeof(BLOCKTRAILER)) / TCINFO.rgib[TCI_bm])BlockIndex = N / RowsPerBlockRowIndex = N % RowsPerBlock
        public uint RowsPerBlock()
        {
            return 0;
        }
    }
}
