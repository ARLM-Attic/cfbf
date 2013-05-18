using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Outlook.PST
{
    public class PSTReader : IDisposable
    {
        #region fields

        internal const string PST_PATTERN = "!BDN";

        private FileStream _pstStream = null;

        private string _pstFilePath = null;

        private IHeader _Header;

        private HeaderType _headerType = HeaderType.UNICODE_PST;

        private bool disposed = false;
        #endregion fields


        public PSTReader(string pstFilePath)
        {
            if (string.IsNullOrEmpty(pstFilePath))
                throw new Exception("Invalid pst file path");

            if (!File.Exists(pstFilePath))
                throw new FileNotFoundException();

            this._pstFilePath = pstFilePath;
        }


        public unsafe void Read()
        {
            _pstStream = new FileStream(_pstFilePath, FileMode.Open);
            
            //READ INFORMATION FROM PST TO DETERMINE IF IS A VALID PST, AND TYPE OF PST (UNICODE OR ANSI)
            //INIZIALIZE THE POSITION OF STREAM TO ZERO            
            _pstStream.Position = 0;
            var dwMagic = new byte[4];
            _pstStream.Read(dwMagic, 0, 4);
            var magic = System.Text.ASCIIEncoding.ASCII.GetString(dwMagic);
            if (magic != PST_PATTERN )
                throw new InvalidPSTException("The file is not a valid PST file");

            //verify if pst is in unicode or pst
            var pstVersion = new byte[2];
            _pstStream.Position = 10;
            _pstStream.Read(pstVersion, 0, 2);
            var version = BitConverter.ToInt16(pstVersion, 0);
            if ( version == 23 )
                _headerType = HeaderType.UNICODE_PST;
            else if (version == 14 || version == 15)
            {
                _headerType = HeaderType.ANSI_PST;
            }


            if (_headerType == HeaderType.UNICODE_PST)
            {
                //reset position of stream to initial position
                _pstStream.Position = 0;

                byte[] buffer = null;
                int size = Marshal.SizeOf(typeof (HeaderUnicode));
                buffer = new byte[size];
                _pstStream.Read(buffer, 0, buffer.Length);

                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.Copy(buffer, 0, ptr, size);
                _Header = (HeaderUnicode) Marshal.PtrToStructure(ptr, typeof (HeaderUnicode));
                Marshal.FreeHGlobal(ptr);


                //var currPosition = _pstStream.Position;
                //var amapOffset = ((HeaderUnicode) _Header).root.ibAMapLast;
                //size = Marshal.SizeOf(typeof (AMapPageUnicode));
                //var buffer1 = new byte[size];
                //_pstStream.Position = amapOffset;
                //_pstStream.Read(buffer1, 0, buffer1.Length);

                ////ptr = IntPtr.Zero;
                //IntPtr ptr1 = Marshal.AllocHGlobal(size);
                //Marshal.Copy(buffer1, 0, ptr1, size);
                //var lastPage = (AMapPageUnicode)Marshal.PtrToStructure(ptr1, typeof(AMapPageUnicode));
                //Marshal.FreeHGlobal(ptr1);

                ////restore stream position
                //_pstStream.Position = currPosition;
            }
        }


        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                disposed = true;
            }
        }
        #endregion

        #region Destructor
        ~PSTReader()
        {
            Dispose(false);
        }
        #endregion

    }
}
