using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.CFBF;

namespace msg2eml
{
    class Program
    {
        static void Main(string[] args)
        {

            var filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "linda.msg");
            var cfbf = new System.IO.CFBF.CompoundFileBinaryFileFormatReader(filePath);


            //var i = cfbf.DirectoryEntries;

            //var reader = new CompoundDocumentReader();
            //IList<ResultSet> lista = reader.Read( new FileStream(filePath, FileMode.Open));



            Console.WriteLine();
        }
    }
}
