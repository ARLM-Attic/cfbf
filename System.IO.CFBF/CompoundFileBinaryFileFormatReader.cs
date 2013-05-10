using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using SSL.Util;

namespace System.IO.CFBF
{
    /// <summary>
    /// Coumpound File Reader
    /// </summary>
    public unsafe class CompoundFileBinaryFileFormatReader : IDisposable
    {
        public ushort SectorSize;

        private long NumberOfSIDs;

        public Header Header;

        private Stream cfbfStream = null;

        private Stream ShortStreamContainerStream = null;

        internal uint[] MasterSectorAllocationTable
        {
            get;
            private set;
        }

        /// <summary>
        /// The sector allocation table (SAT) is an array of SecIDs. It contains the SecID chain of all user streams (except
        /// short-streams) and of the remaining internal control streams (the short-stream container stream, the shortsector
        /// allocation table, and the directory). The size of the SAT (number of SecIDs) is equal to the number of
        /// /// existing sectors in the compound document file.
        /// 
        /// The SAT is built by reading and concatenating the contents of all sectors given in the MSAT. 
        /// The sectors have to be read according to the order of the SecIDs in the MSAT.
        /// </summary>
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

        public IList<DirectoryEntry> DirectoryEntries
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
            if (cfbfStream.Length == 0)
                throw new InvalidCFBFException("Invalid Coumpound File Binary File Format!");

            this.cfbfStream = cfbfStream;
            
        }

        public CompoundFileBinaryFileFormatReader(string FilePath)
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException();

            this.cfbfStream = new FileStream(FilePath, FileMode.Open);

            if (cfbfStream.Length == 0)
                throw new InvalidCFBFException("Invalid Coumpound File Binary File Format!");
        }
        #endregion

        public void Parse()
        {
            #region parsing file cfbf
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
            if (Header.MajorVersion == 0x0003 && Header.SectorShift == 0x0009)
                SectorSize = 512;

            if (Header.MajorVersion == 0x0004 && Header.SectorShift == 0x000C)
                SectorSize = 4096;
            #endregion

            NumberOfSIDs = (cfbfStream.Length / SectorSize) - 1;

            #region MASTER SECTOR ALLOCATION TABLE
            MasterSectorAllocationTable = ReadMasterSectorAllocationTable();
            #endregion

            #region SECTOR ALLOCATION TABLE
            SectorAllocationTable = ReadSectorAllocationTable();
            #endregion

            #region DIRECTORY STORAGE
            DirectoryEntries = ReadDirectoryStorage();
            #endregion

            #region SHORT ALLOCATION TABLE
            ShortSectorAllocationTable = ReadShortSectorAllocationTable();
            #endregion

            ShortStreamContainerStream = GetDirectoryEntryStream(GetRootEntry());
            #endregion
        }        

        public DirectoryEntry GetRootEntry()
        {
            return DirectoryEntries.Where(root => root.ObjectType == ObjectType.ROOT_STORAGE_OBJECT).FirstOrDefault();
        }

        public Stream GetDirectoryEntryStream(DirectoryEntry dir)
        {
            Stream stream = null;

            if (dir.IsStoredInShortStream)
                stream = ReadShortStreamContainerStream(dir);
            else
                stream = ReadStream(dir);

            return stream;
        }

        public IList<DirectoryEntry> GetDirectoriesEntryByName(string directoryName)
        {
           return DirectoryEntries.Where(a=> a.DirectoryEntryName.Contains(directoryName)).ToList<DirectoryEntry>();
        }
      
        private uint[] ReadSectorAllocationTable()
        {
            var results = new List<uint>();

            for (int i = 0; i < this.MasterSectorAllocationTable.Length; i++)
            {
                cfbfStream.Position = SectorPosition(MasterSectorAllocationTable[i]);
                results.AddRange(ReadSectorChain(cfbfStream));
            }
            return results.ToArray();
        }

        private uint[] ReadShortSectorAllocationTable()
        {
            var results = new List<uint>();
            uint valueId = this.Header.FirstMiniFATSectorLocation;

            while (valueId != (uint)SectorName.ENDOFCHAIN)
            {
                cfbfStream.Position = SectorPosition(valueId);
                results.AddRange(ReadSectorChain(cfbfStream));

                valueId = SectorAllocationTable[valueId];
            }
            return results.ToArray();
        }

        private uint[] ReadMasterSectorAllocationTable()
        {
            var MSAT = new List<uint>();

            //Read first the 109 FAT Sectors
            MSAT.AddRange(this.Header.DIFAT.ToUInt32());

            if (this.Header.NumberFATSectors > (this.Header.DIFAT.Length / 4))
            {
                //Get The Next Sector for DIFAT
                uint sectorid = this.Header.FirstDIFATSectorLocation;

                //loop all sectors while an end of chain not found
                while (sectorid != (uint)SectorName.ENDOFCHAIN)
                {
                    cfbfStream.Position = SectorPosition(sectorid);
                    var buffer = cfbfStream.ReadBytes(this.SectorSize);
                    MSAT.AddRange(buffer.ToUInt32());                    
                    sectorid = BitConverter.ToUInt32(buffer.ReadBytes(this.SectorSize - 4, 4), 0);
                }
            }

            return MSAT.ToArray();
        }

        public void PrintDirectoriesEntryNames(string filePath)
        {
            StreamWriter writer = File.CreateText(filePath);

            foreach (var entry in DirectoryEntries)
            {                
                writer.WriteLine(entry.ToString());
            }

            writer.Close();
            writer.Dispose();
        }

        private IList<DirectoryEntry> ReadDirectoryStorage()
        {
            IList<DirectoryEntry> Directories = new List<DirectoryEntry>();
            uint valueId = this.Header.FirstDirectorySectorLocation;

            while (valueId != (uint)SectorName.ENDOFCHAIN)
            {
                cfbfStream.Position = SectorPosition(valueId);
                for (int j = 0; j < (this.SectorSize / 128); j++)
                {
                    GCHandle pinnedPacket = GCHandle.Alloc(cfbfStream.ReadBytes(Marshal.SizeOf(typeof(DirectoryEntry))), GCHandleType.Pinned);
                    var dir = (DirectoryEntry)Marshal.PtrToStructure(pinnedPacket.AddrOfPinnedObject(), typeof(DirectoryEntry));
                    pinnedPacket.Free();
                    Directories.Add(dir);
                }

                valueId = SectorAllocationTable[valueId];
            }
            return Directories;
        }

        //private uint[] ReadShortStreamContainerStream()
        //{
        //    var results = new List<uint>();
        //    var root = GetRootEntry();
        //    var secID = root.StartingSectorLocation;

        //    while (secID != (uint)SectorName.ENDOFCHAIN)
        //    {
        //        cfbfStream.Position = SectorPosition(secID);
        //        results.AddRange(ReadSectorChain(cfbfStream));

        //        secID = SectorAllocationTable[secID];
        //    }

        //    return results.ToArray();
        //}

        private Stream ReadShortStreamContainerStream(DirectoryEntry dir)
        {
            var outStream = new MemoryStream();
            var sectorId = dir.StartingSectorLocation;
            var streamSize = dir.StreamByte;
            var bytesLeft = streamSize;
            var valueId = sectorId;

            while (valueId != (uint)SectorName.ENDOFCHAIN)
            {
                ulong amountToRead = bytesLeft < 64 ? bytesLeft : 64;
                ShortStreamContainerStream.Position = ShortSectorPosition(valueId);
                outStream.Write(ShortStreamContainerStream.ReadBytes((int)amountToRead), 0, (int)amountToRead);
                valueId = ShortSectorAllocationTable[valueId];
                bytesLeft = bytesLeft - amountToRead;
            }
            return outStream;
        }

        private Stream ReadStream(DirectoryEntry dir)
        {
            var outStream = new MemoryStream();
            var sectorId = dir.StartingSectorLocation;
            var streamSize = dir.StreamByte;
            var bytesLeft = streamSize;
            var valueId = sectorId;

            while (valueId != (uint)SectorName.ENDOFCHAIN)
            {
                ulong amountToRead = bytesLeft < this.SectorSize ? bytesLeft : this.SectorSize;
                cfbfStream.Position = SectorPosition(valueId);
                outStream.Write(cfbfStream.ReadBytes((int)amountToRead), 0, (int)amountToRead);
                valueId = SectorAllocationTable[valueId];
                bytesLeft = bytesLeft - amountToRead;
            }

            outStream.Flush();
            outStream.Position = 0;

            return outStream;
        }

        private IList<uint> ReadSectorChain(Stream inStream)
        {
            var sectorNumbers = new List<uint>();
            bool continueToAdd = true;

            for (int i = 0; i < (this.SectorSize / 4); i++)
            {
                var sectorId = BitConverter.ToUInt32(inStream.ReadBytes(4), 0);
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
                    if (this.cfbfStream != null)
                    {
                        this.cfbfStream.Close();
                        this.cfbfStream.Dispose();
                    }
                }

                // There are no unmanaged resources to release, but
                // if we add them, they need to be released here.
            }
            disposed = true;
        }

        private long SectorPosition(uint SecID)
        {
            return 512 + SecID * this.SectorSize;
        }

        private long ShortSectorPosition(uint SecID)
        {
            return SecID * 64;
        }

    }
}
