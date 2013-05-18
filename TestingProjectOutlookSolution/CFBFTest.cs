using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.CFBF;
using SSL.Util;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Net.Mail;
using Outlook.MSG;

namespace TestingProjectOutlookSolution
{
    [TestClass]
    public class CFBFTest
    {
        [TestMethod]
        public void ReadSubjectPropertyFromMSG()
        {

            using (var cfbf = new CompoundFileBinaryFileFormatReader("untitled.msg"))
            {
                cfbf.Parse();

                //Get Direcotory entry of the subject property
                DirectoryEntry dirEntry = cfbf.GetDirectoriesEntryByName("0037001F").FirstOrDefault();

                cfbf.PrintDirectoriesEntryNames("c:\\note_directory_entry.txt");

                var l = cfbf.DirectoryEntries.Where(a => a.StreamByte > 0);

                foreach (var e in l)
                {
                    //Get the Stream for the subject property
                    using (var stream = cfbf.GetDirectoryEntryStream(e))
                    {
                        //Extrac the text from the stream
                        var buff = stream.ReadAllBytes();

                        //Convert byte array to string
                        if (buff != null && buff.Length > 0)
                        {
                            var subject = System.Text.ASCIIEncoding.Unicode.GetString(buff);
                        }
                    }
                }
            }

        }

        [TestMethod]
        public void SaveNoteMSGStreams()
        {

            using (var cfbf = new CompoundFileBinaryFileFormatReader("01.msg"))
            {
                cfbf.Parse();

                //cfbf.PrintDirectoriesEntryNames("c:\\note_directory_entry.txt");

                var l = cfbf.DirectoryEntries.Where(a => a.StreamByte > 0);

                foreach (var e in l)
                {
                    //Get the Stream for the subject property
                    using (var stream = cfbf.GetDirectoryEntryStream(e))
                    {
                        //Extrac the text from the stream
                        var buff = stream.ReadAllBytes();

                        File.WriteAllBytes("C:\\Temp\\" + e.DirectoryEntryName + "", buff);
                    }
                }
            }

        }

        [TestMethod]
        public void TestMsg()
        {
            using (var msg = new Outlook.MSG.OutlookMsg("01.msg"))
            {
                msg.PrintProperties("c:\\untiled.msg.properties.txt");
            }
        }

        [TestMethod]
        public void ContactItemMsg()
        {
            using (var msg = new Outlook.MSG.OutlookMsg("c1.msg"))
            {
                msg.PrintProperties("c:\\c1.msg.properties.txt");
            }
        }

        [TestMethod]
        public void SaveContactItemMSGStreams()
        {

            using (var cfbf = new CompoundFileBinaryFileFormatReader("c1.msg"))
            {
                cfbf.Parse();

                //cfbf.PrintDirectoriesEntryNames("c:\\note_directory_entry.txt");

                var l = cfbf.DirectoryEntries.Where(a => a.StreamByte > 0);

                foreach (var e in l)
                {
                    //Get the Stream for the subject property
                    using (var stream = cfbf.GetDirectoryEntryStream(e))
                    {
                        //Extrac the text from the stream
                        var buff = stream.ReadAllBytes();

                        File.WriteAllBytes("C:\\Temp\\" + e.DirectoryEntryName + "", buff);
                    }
                }
            }

        }

        [TestMethod]
        public void TestEml()
        {

            MailMessage Message = new MailMessage("simon.saliba.eid@tesoro.it", "simon.saliba.eid@tesoro.it", "subject of email", "body of email");
            string FileName = "c:\\p2.eml";

            Assembly assembly = typeof(SmtpClient).Assembly;
            Type _mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");
            
            using (FileStream _fileStream = new FileStream(FileName, FileMode.Create))
            {
                // Get reflection info for MailWriter contructor
                ConstructorInfo _mailWriterContructor = _mailWriterType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null,  new Type[] { typeof(Stream) }, null);

                // Construct MailWriter object with our FileStream
                object _mailWriter = _mailWriterContructor.Invoke(new object[] { _fileStream });

                // Get reflection info for Send() method on MailMessage
                MethodInfo _sendMethod = typeof(MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);

                // Call method passing in MailWriter
                _sendMethod.Invoke(
                    Message,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new object[] { _mailWriter, true, true },
                    null);

                // Finally get reflection info for Close() method on our MailWriter
                MethodInfo _closeMethod = _mailWriter.GetType().GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic);

                // Call close method
                _closeMethod.Invoke( _mailWriter,  BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { },  null);
            }

        }

        [TestMethod]
        public void TestMsgToEml()
        {
            using (var msg = new OutlookMsg("2.msg"))
            {
                if (msg.MessageType == MessageObjectType.EMAIL)
                {
                    ((EmailItem)msg.MessageObjectItem).SaveAsEml("c:\\1.eml");
                }
            }
            
        }
    
    }
}
