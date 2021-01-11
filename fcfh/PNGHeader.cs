using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fcfh
{
    /// <summary>
    /// Represents a PNG Header
    /// </summary>
    public class PNGHeader
    {
        /// <summary>
        /// Chunk Flags
        /// </summary>
        [Flags]
        public enum ChunkFlags : int
        {
            /// <summary>
            /// No Flags
            /// </summary>
            None = 0,
            /// <summary>
            /// This header can be safely stripped
            /// </summary>
            /// <remarks>Removing this header will not delete any information important for rendering.</remarks>
            Ancillary = 1,
            /// <summary>
            /// This header is vendor defined
            /// </summary>
            /// <remarks>Header is not part of PNG standard</remarks>
            Private = 2,
            /// <summary>
            /// Reserved Keyword
            /// </summary>
            /// <remarks>This should never be set</remarks>
            Reserved = 4,
            /// <summary>
            /// This header is safe to copy to other formats and after editing
            /// </summary>
            /// <remarks>If this header is set it indicates that the chunk can be left in the image even if it is edited</remarks>
            SafeToCopy = 8
        }

        /// <summary>
        /// The first 8 bytes of a PNG file
        /// </summary>
        public const ulong PNG_MAGIC = 0x0a1a0a0d474e5089;

        /// <summary>
        /// Flags according to the Chunk name
        /// </summary>
        public ChunkFlags Flags
        {
            get
            {
                return
                    (char.IsLower(HeaderName[0]) ? ChunkFlags.Ancillary : ChunkFlags.None) |
                    (char.IsLower(HeaderName[1]) ? ChunkFlags.Private : ChunkFlags.None) |
                    (char.IsLower(HeaderName[2]) ? ChunkFlags.Reserved : ChunkFlags.None) |
                    (char.IsLower(HeaderName[3]) ? ChunkFlags.SafeToCopy : ChunkFlags.None);
            }
        }
        /// <summary>
        /// 4-byte name of the header
        /// </summary>
        public string HeaderName
        {
            get; private set;
        }
        /// <summary>
        /// Raw binary data
        /// </summary>
        public byte[] Data
        { get; private set; }
        /// <summary>
        /// Checksum of <see cref="HeaderName">+<see cref="Data"/>
        /// </summary>
        public uint Checksum
        {
            get
            {
                return Tools.IntToNetwork(CalcChecksum(Data));
            }
        }
        /// <summary>
        /// Gets if this Header has encoded data
        /// </summary>
        public bool IsDataHeader
        {
            get
            {
                //Data must be at least 14 bytes (BMPENC+int+int)
                return Data.Length > 14 && Encoding.ASCII.GetString(Data, 0, 6) == ImageWriter.MAGIC;
            }
        }
        /// <summary>
        /// Gets the File Name from a Header
        /// </summary>
        public string FileName
        {
            get
            {
                if (IsDataHeader)
                {
                    return Encoding.UTF8.GetString(Data, 10, Tools.IntToHost(BitConverter.ToInt32(Data, 6)));
                }
                return null;
            }
        }
        /// <summary>
        /// Gets the File Data from a Data Header
        /// </summary>
        public byte[] FileData
        {
            get
            {
                if (IsDataHeader)
                {
                    var StartOfData = 6 + 4 + 4 + Tools.IntToHost(BitConverter.ToInt32(Data, 6));
                    var LengthOfData = Tools.IntToHost(BitConverter.ToInt32(Data, StartOfData - 4));
                    return Data
                        .Skip(StartOfData)
                        .Take(LengthOfData)
                        .ToArray();
                }
                return null;
            }
        }

        /// <summary>
        /// Reads A Header from the given Source
        /// </summary>
        /// <param name="Source"></param>
        public PNGHeader(Stream Source)
        {
            using (var BR = new BinaryReader(Source, Encoding.UTF8, true))
            {
                //Format: <length:i><headername:s(4)><data:b(length)><crc:i>
                int DataLength = Tools.IntToHost(BR.ReadInt32());
                HeaderName = Encoding.Default.GetString(BR.ReadBytes(4));
                if (DataLength > 0)
                {
                    Data = BR.ReadBytes(DataLength);
                }
                else
                {
                    Data = new byte[0];
                }
#if DEBUG
                uint StoredChecksum = Tools.IntToHost(BR.ReadUInt32());
                if (CalcChecksum() != StoredChecksum)
                {
                    Console.Error.WriteLine(@"
Invalid Checksum!
=================
HEADER    : {0}
CALCULATED: {1}
STORED    : {2}
",
HeaderName,
string.Join("-", BitConverter.GetBytes(CalcChecksum()).Select(m => m.ToString("X2")).ToArray()),
string.Join("-", BitConverter.GetBytes(StoredChecksum).Select(m => m.ToString("X2")).ToArray())
);
                }
                else
                {
                    Console.Error.WriteLine("Checksum OK for {0}", HeaderName);
                }
#else
                    //Discard Checksum for release build
                    Tools.IntToHost(BR.ReadUInt32());
#endif
            }
        }

        /// <summary>
        /// Creates a new Header
        /// </summary>
        /// <param name="HeaderName">Header Name</param>
        /// <param name="Data">Header Data</param>
        public PNGHeader(string HeaderName, byte[] Data)
        {
            if (string.IsNullOrEmpty(HeaderName) || Encoding.UTF8.GetByteCount(HeaderName) != 4)
            {
                throw new ArgumentException("HeaderName must be 4 bytes in length");
            }
            this.HeaderName = HeaderName;
            this.Data = Data == null ? new byte[0] : (byte[])Data.Clone();
        }

        /// <summary>
        /// Writes the Header to an output stream
        /// </summary>
        /// <param name="Output">Output Stream</param>
        public void WriteHeader(Stream Output)
        {
            using (var BW = new BinaryWriter(Output, Encoding.UTF8, true))
            {
                BW.Write(Tools.IntToNetwork(Data.Length));
                BW.Write(Encoding.Default.GetBytes(HeaderName));
                BW.Write(Data);
                BW.Write(Tools.IntToNetwork(CalcChecksum()));
            }
        }

        #region CRC

        /// <summary>
        /// Table of CRCs of all 8-bit messages.
        /// </summary>
        private static uint[] crc_table = null;

        /// <summary>
        /// Static initializer
        /// </summary>
        static PNGHeader()
        {
            make_crc_table();
        }

        /// <summary>
        /// Make the table for a fast CRC.
        /// </summary>
        private static void make_crc_table()
        {
            uint c;
            uint n, k;
            if (crc_table == null)
            {
                crc_table = new uint[256];
                for (n = 0; n < crc_table.Length; n++)
                {
                    c = n;
                    for (k = 0; k < 8; k++)
                    {
                        if ((c & 1) != 0)
                        {
                            c = 0xedb88320 ^ (c >> 1);
                        }
                        else
                        {
                            c = c >> 1;
                        }
                    }
                    crc_table[n] = c;
                }
            }
        }

        /// <summary>
        /// Update a running CRC with the bytes buf[0..len-1]--the CRC
        /// should be initialized to all 1's, and the transmitted value
        /// is the 1's complement of the final running CRC (see the
        /// crc() routine below)).
        /// </summary>
        /// <param name="crc">CRC Start value</param>
        /// <param name="buf">Bytes</param>
        /// <returns>Updated CRC</returns>
        private static uint update_crc(uint crc, byte[] buf)
        {
            uint c = crc;
            int n;
            for (n = 0; n < buf.Length; n++)
            {
                c = crc_table[(c ^ buf[n]) & 0xff] ^ (c >> 8);
            }
            return c;
        }

        /// <summary>
        /// Return the CRC of the bytes buf[0..len-1].
        /// </summary>
        /// <param name="buf">Byte array</param>
        /// <returns>Calculates PNG CRC of a byte array</returns>
        private static uint crc(byte[] buf)
        {
            return update_crc(0xffffffff, buf) ^ 0xffffffff;
        }

        /// <summary>
        /// Calculates CRC of a byte array
        /// </summary>
        /// <param name="Data">Bytes</param>
        /// <returns>CRC</returns>
        public static uint CalcChecksum(byte[] Data)
        {
            return crc(Data);
        }

        /// <summary>
        /// Calculates the Checksum of the current header
        /// </summary>
        /// <returns>Checksum</returns>
        public uint CalcChecksum()
        {
            return CalcChecksum(Encoding.Default.GetBytes(HeaderName).Concat(Data).ToArray());
        }

        #endregion
    }
}
