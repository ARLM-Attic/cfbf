using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Outlook.PST
{

    /// <summary>
    /// The TCOLDESC structure describes a single column in the TC, which includes 
    /// metadata about the size of the data associated with this column, as well 
    /// as whether a column exists, and how to locate the column data from the Row Matrix.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct TCCOLDESC
    {
        /// <summary>
        /// tag (4 bytes): This field specifies that 32-bit tag that is associated with the column.
        /// </summary>
        [FieldOffset(0)]
        public int tag;

        /// <summary>
        /// ibData (2 bytes): Data Offset. This field indicates the offset from the beginning 
        /// of the row data (in the Row Matrix) where the data for this column can be retrieved. 
        /// Because each data row is laid out the same way in the Row Matrix, the Column data 
        /// for each row can be found at the same offset.
        /// </summary>
        [FieldOffset(4)]
        public short ibData;

        /// <summary>
        /// cbData (1 byte): Data size. This field specifies the size of the data associated 
        /// with this column (that is, "width" of the column), in bytes per row. However, 
        /// in the case of variable-sized data, this value is set to the size of an HNID 
        /// instead. This is explained further in section 2.3.4.4.
        /// </summary>
        [FieldOffset(6)]
        public byte cbData;

        /// <summary>
        /// iBit (1 byte): Cell Existence Bitmap Index. This value is the 0-based index into the 
        /// CEB bit that corresponds to this Column. A detailed explanation of the 
        /// mapping mechanism will be discussed in section 2.3.4.4.1.
        /// </summary>
        [FieldOffset(7)]
        public byte iBit;
    }

    /// <summary>
    /// TCINFO is the header structure for the TC. The TCINFO is accessed using the 
    /// hidUserRoot field in the HNHDR structure of the containing HN. 
    /// The header contains the column definitions and other relevant data.
    /// Unicode / ANSI:
    /// </summary>
    public class TCInfo
    {
        /// <summary>
        /// bType (1 byte): TC signature; MUST be set to bTypeTC.
        /// </summary>
        public byte bType;

        /// <summary>
        /// cCols (1 byte): Column count. This specifies the number of columns in the TC.
        /// </summary>
        public byte cCols;

        /// <summary>
        /// rgib (8 bytes): This is an array of 4 16-bit values that specify 
        /// the offsets of various groups of data in the actual row data. 
        /// The application of this array is specified in section 2.3.4.4, 
        /// which covers the data layout of the Row Matrix.
        /// </summary>
        public long rgib;

        /// <summary>
        /// hidRowIndex (4 bytes): HID to the Row ID BTH. The Row ID BTH contains (RowID, RowIndex) value pairs that correspond to each row of the TC. The RowID is a value that is associated with the row identified by the RowIndex, whose meaning depends on the higher levelstructure that implements this TC. The RowIndex is the zero-based index to a particular row in the Row Matrix.
        /// </summary>
        public uint hidRowIndex;

        /// <summary>
        /// hnidRows (4 bytes): HNID to the Row Matrix (that is, actual table data). 
        /// This value is set to zero if the TC contains no rows.
        /// </summary>
        public uint hnidRows;

        /// <summary>
        /// hidIndex (4 bytes): Deprecated. 
        /// Implementations SHOULD ignore this value, and creators of a new PST MUST set this value to zero.
        /// </summary>
        public uint hidIndex;

        /// <summary>
        /// rgTCOLDESC (variable): Array of Column Descriptors. 
        /// This array contains cCol entries of type TCOLDESC structures that define each TC column.
        /// </summary>
        public byte[] rgTCOLDESC;

    }

    public interface ITCROWID
    {
    }
     
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct TCROWIDUnicode : ITCROWID
    {
        /// <summary>
        /// dwRowID (4 bytes): This is the 32-bit primary key value that uniquely identifies a row in the Row Matrix.
        /// </summary>
        [FieldOffset(0)]
        public uint dwRowID;

        /// <summary>
        /// dwRowIndex (Unicode: 4 bytes; ANSI: 2 bytes): The 0-based index to the corresponding row in the Row Matrix. 
        /// Note that for ANSI PSTs, the maximum number of rows is 2^16.
        /// </summary>
        [FieldOffset(4)]
        public uint dwRowIndex;
    }
    
    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public struct TCROWIDAnsi : ITCROWID
    {
        /// <summary>
        /// dwRowID (4 bytes): This is the 32-bit primary key value that uniquely identifies a row in the Row Matrix.
        /// </summary>
        [FieldOffset(0)]
        public uint dwRowID;

        /// <summary>
        /// dwRowIndex (Unicode: 4 bytes; ANSI: 2 bytes): The 0-based index to the corresponding row in the Row Matrix. 
        /// Note that for ANSI PSTs, the maximum number of rows is 2^16.
        /// </summary>
        [FieldOffset(4)]
        public short dwRowIndex;
    }


}
