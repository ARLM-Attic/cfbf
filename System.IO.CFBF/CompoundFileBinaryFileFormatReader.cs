using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace System.IO.CFBF
{
    /// <summary>
    /// Coumpound File Reader
    /// </summary>
    public unsafe class CompoundFileBinaryFileFormatReader : IDisposable
    {
        private ushort SectorSize;

        public Header Header;

        private Stream cfbfStream = null;

        internal uint[] MasterSectorAllocationTable
        {
            get;
            private set;
        }

        internal uint[] SectorAllocationTable
        {
            get;
            private set;
        }

        internal uint[] ShortSectorAllocationTable
        {
            get;
            private set;
        }

        internal IList<DirectoryEntry> DirectoryEntries
        {
            get;
            private set;
        }

        private bool disposed = false; 

        #region Constructor
        private CompoundFileBinaryFileFormatReader()
        {
            //for private use only
        }

        public CompoundFileBinaryFileFormatReader(Stream cfbfStream)
        {
            this.cfbfStream = cfbfStream;
            ParseCFBF();
        }

        public CompoundFileBinaryFileFormatReader(string FilePath)
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException();

            this.cfbfStream = new FileStream(FilePath, FileMode.Open);

            ParseCFBF();
        }
        #endregion

        private void ParseCFBF()
        {
            #region parsing file cfbf
            using (var reader = new System.IO.BinaryReader(cfbfStream, ASCIIEncoding.ASCII))
            {
                //inizialize default offset to zero to read header
                int offset = 0;

                #region READ HEADER FROM STREAM
                //read buffer to cast it to the header
                byte[] buffer = cfbfStream.ReadBytes(offset, Marshal.SizeOf(typeof(Header)), false);
                GCHandle pinnedPacket = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                Header = (Header)Marshal.PtrToStructure(pinnedPacket.AddrOfPinnedObject(), typeof(Header));
                pinnedPacket.Free();
                #endregion

                #region SECTOR SIZE
                if (Header.SectorShift == 0x0009 || Header.SectorShift == 0x000c)
                {
                    if (Header.MajorVersion == 0x0003 && Header.SectorShift == 0x0009)
                        SectorSize = 512;

                    if (Header.MajorVersion == 0x0004 && Header.SectorShift == 0x000C)
                        SectorSize = 4096;
                }
                #endregion

                #region MASTER SECTOR ALLOCATION TABLE
                MasterSectorAllocationTable = readMasterSectorAllocationTable();
                #endregion

                #region SECTOR ALLOCATION TABLE
                SectorAllocationTable = readSectorAllocationTable();
                #endregion

                #region DIRECTORY STORAGE
                DirectoryEntries = readDirectoryStorage();
                #endregion

                #region SHORT ALLOCATION TABLE
                ShortSectorAllocationTable = readShortSectorAllocationTable();
                #endregion


            }
            #endregion
        }

        public DirectoryEntry GetRootEntry()
        {
            return DirectoryEntries.Where(root => root.ObjectType == ObjectType.ROOT_STORAGE_OBJECT).FirstOrDefault();
        }

        private uint[] readSectorAllocationTable()
        {
            var results = new List<uint>();
            for (int i = 0; i < this.Header.NumberFATSectors; i++)
            {
                if (i == 0)                    
                    if ( this.Header.FirstDIFATSectorLocation == (uint)SectorName.ENDOFCHAIN )
                        cfbfStream.Position = 512 + (0 * this.SectorSize);
                    else
                        cfbfStream.Position = 512 + (this.Header.FirstDIFATSectorLocation * this.SectorSize);
                else
                    cfbfStream.Position = 512 + (MasterSectorAllocationTable[i] * this.SectorSize);

                results.AddRange(readSectorChain(cfbfStream));
            }

            return results.ToArray();

        }

        private uint[] readShortSectorAllocationTable()
        {
            var results = new List<uint>();
            uint valueId = this.Header.FirstMiniFATSectorLocation;// documentHeader.SecIDOfFirstSectorOfTheShortSector;

            while (valueId != (uint)SectorName.ENDOFCHAIN)
            {
                cfbfStream.Position = 512 + (valueId * this.SectorSize);
                results.AddRange(readSectorChain(cfbfStream));
                valueId = SectorAllocationTable[valueId];
            }
            return results.ToArray();
        }

        private uint[] readMasterSectorAllocationTable()
        {
            var masterSectorAllocationTable = new List<uint>();
            int location = 0;

            Stream masterAllocationBytes = null;
            try
            {
                masterAllocationBytes = new MemoryStream(this.Header.DIFAT, false);
                masterAllocationBytes.Position = 0;
                
                for (int i = 0; i < masterAllocationBytes.Length / 4; i++)
                {
                    var sector = masterAllocationBytes.ReadBytes(4);
                    location = location + 4;
                    uint sectorId;

                    if (i < this.Header.NumberFATSectors)
                    {
                        sectorId = BitConverter.ToUInt32(sector, 0);

                        if (masterAllocationBytes.Length <= masterAllocationBytes.Position)
                        {
                            // read in next sector for master allocation table
                            masterAllocationBytes.Close();
                            masterAllocationBytes.Dispose();
                            cfbfStream.Position = 512 + this.SectorSize * sectorId;
                            masterAllocationBytes = new MemoryStream(cfbfStream.ReadBytes(this.SectorSize), true);
                            masterAllocationBytes.Position = 0;
                            location = 0;
                        }
                        else
                        {
                            masterSectorAllocationTable.Add(sectorId);
                        }
                    }
                }
            }
            finally
            {
                if (masterAllocationBytes != null)
                {
                    masterAllocationBytes.Close();
                    masterAllocationBytes.Dispose();
                }
            }
            return masterSectorAllocationTable.ToArray();
        }

        private IList<DirectoryEntry> readDirectoryStorage()
        {
            IList<DirectoryEntry> Directories = new List<DirectoryEntry>();
            
            uint valueId;
            valueId = this.Header.FirstDirectorySectorLocation;

            while (valueId != (uint)SectorName.ENDOFCHAIN)
            {
                cfbfStream.Position = 512 + (valueId * this.SectorSize);

                for (int j = 0; j < this.SectorSize / 128; j++)
                {
                    //var dir = new Directory(Directories.Count, inStream);
                    var dir = new DirectoryEntry();

                    byte[] buffer = cfbfStream.ReadBytes(Marshal.SizeOf(typeof(DirectoryEntry)));
                    GCHandle pinnedPacket = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    dir = (DirectoryEntry)Marshal.PtrToStructure(pinnedPacket.AddrOfPinnedObject(), typeof(DirectoryEntry));
                    pinnedPacket.Free();

                    if (dir.StreamByte > 0)
                        Directories.Add(dir);
                }
                valueId = SectorAllocationTable[(int)valueId];
            }
            return Directories;
        }

        //private static Stream readShortStreamContainerStream(Stream inStream, DocumentHeader documentHeader, int[] sectorAllocationTable, Directory logicalDir)
        //{
        //    var shortStreamContainerStream = new MemoryStream();
        //    int valueId = logicalDir.SectorIdOfFirstSector;
        //    int streamSize = logicalDir.TotalStreamSizeInBytes;
        //    int bytesLeft = streamSize;
        //    while (valueId != -2)
        //    {
        //        int amountToRead = bytesLeft < documentHeader.SizeOfSector ? bytesLeft : documentHeader.SizeOfSector;
        //        inStream.Position = 512 + (valueId * documentHeader.SizeOfSector);
        //        shortStreamContainerStream.Write(inStream.ReadStreamPart(amountToRead), 0, amountToRead);
        //        valueId = sectorAllocationTable[valueId];
        //        bytesLeft = bytesLeft - amountToRead;
        //    }
        //    return shortStreamContainerStream;
        //}

        //private static Stream readGenericStream(Stream inStream, Directory logicalDir, int[] sectorAllocationTable, int sizeOfSector, int offset)
        //{
        //    var outStream = new MemoryStream();

        //    var sectorId = logicalDir.SectorIdOfFirstSector;
        //    var streamSize = logicalDir.TotalStreamSizeInBytes;

        //    int bytesLeft = streamSize;
        //    int valueId = sectorId;
        //    while (valueId != -2)
        //    {
        //        int amountToRead = bytesLeft < sizeOfSector ? bytesLeft : sizeOfSector;
        //        inStream.Position = offset + (valueId * sizeOfSector);
        //        outStream.Write(inStream.ReadStreamPart(amountToRead), 0, amountToRead);
        //        valueId = sectorAllocationTable[valueId];
        //        bytesLeft = bytesLeft - amountToRead;
        //    }
        //    outStream.Flush();
        //    outStream.Position = 0;
        //    return outStream;
        //}

        private IList<uint> readSectorChain(Stream inStream)
        {
            var sectorNumbers = new List<uint>();
            bool continueToAdd = true;

            for (int i = 0; i < this.SectorSize / 4; i++)
            {
                var vector = new byte[4];
                inStream.Read(vector, 0, 4);

                var sectorId = BitConverter.ToUInt32(vector, 0);
                
                if (continueToAdd)
                {
                    sectorNumbers.Add(sectorId);
                    continueToAdd = sectorId != (uint)SectorName.FREESECT;
                }
            }
            return sectorNumbers;
        }

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
                    // Dispose managed resources.
                    if (this.cfbfStream != null )
                    {
                        this.cfbfStream.Close();
                        this.cfbfStream.Dispose();
                    }
                }

                // There are no unmanaged resources to release, but
                // if we add them, they need to be released here.
            }
            disposed = true;

            // If it is available, make the call to the
            // base class's Dispose(Boolean) method
            Dispose(disposing);
        }
    }
}
