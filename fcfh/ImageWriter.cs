﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace fcfh
{
    public static class ImageWriter
    {
        public const string MAGIC = "BMPENC";

        public static class HeaderMode
        {
            public class Header
            {
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
                public const ulong PNG = 0x0a1a0a0d474e5089;

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
                        return hton(CalcChecksum(Data));
                    }
                }

                /// <summary>
                /// Reads A Header from the given Source
                /// </summary>
                /// <param name="Source"></param>
                public Header(Stream Source)
                {
                    using (var BR = new BinaryReader(Source, Encoding.UTF8, true))
                    {
                        //Format: <length:i><headername:s(4)><data:b(length)><crc:i>
                        int DataLength = ntoh(BR.ReadInt32());
                        HeaderName = Encoding.Default.GetString(BR.ReadBytes(4));
                        if (DataLength > 0)
                        {
                            Data = BR.ReadBytes(DataLength);
                        }
                        else
                        {
                            Data = new byte[0];
                        }
                        uint StoredChecksum = ntoh(BR.ReadUInt32());
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
                    }
                }

                /// <summary>
                /// Creates a new Header
                /// </summary>
                /// <param name="HeaderName">Header Name</param>
                /// <param name="Data">Header Data</param>
                public Header(string HeaderName, byte[] Data)
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
                        BW.Write(hton(Data.Length));
                        BW.Write(HeaderName);
                        BW.Write(Data);
                        BW.Write(hton(CalcChecksum()));
                    }
                }

                #region Utils

                /// <summary>
                /// Host to network (int)
                /// </summary>
                /// <param name="i">Number</param>
                /// <returns>Number</returns>
                public static int hton(int i)
                {
                    return System.Net.IPAddress.HostToNetworkOrder(i);
                }

                /// <summary>
                /// Host to network (uint)
                /// </summary>
                /// <param name="i">Number</param>
                /// <returns>Number</returns>
                public static uint hton(uint i)
                {
                    return (uint)System.Net.IPAddress.HostToNetworkOrder((int)i);
                }

                /// <summary>
                /// Network to Host (int)
                /// </summary>
                /// <param name="i">Number</param>
                /// <returns>Number</returns>
                public static int ntoh(int i)
                {
                    return System.Net.IPAddress.NetworkToHostOrder(i);
                }

                /// <summary>
                /// Network to Host (uint)
                /// </summary>
                /// <param name="i">Number</param>
                /// <returns>Number</returns>
                public static uint ntoh(uint i)
                {
                    return (uint)System.Net.IPAddress.NetworkToHostOrder((int)i);
                }
                #endregion

                #region CRC

                /// <summary>
                /// Table of CRCs of all 8-bit messages.
                /// </summary>
                private static uint[] crc_table = null;

                /// <summary>
                /// Static initializer
                /// </summary>
                static Header()
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

            /// <summary>
            /// Header name
            /// </summary>
            /// <remarks>Upper-and lowercase follow a format: http://www.libpng.org/pub/png/spec/1.2/PNG-Structure.html#Chunk-naming-conventions</remarks>
            public const string DEFAULT_HEADER = "fcFh";

            /// <summary>
            /// Stores data in a PNG header
            /// </summary>
            /// <param name="FullFileName">Source file</param>
            /// <param name="ImageFile">Existing Image file</param>
            /// <param name="HeaderName">Name of Header</param>
            /// <returns>PNG with custom header</returns>
            /// <remarks>This process is repeatable</remarks>
            public static byte[] CreateImageFromFile(string FullFileName, string ImageFile, string HeaderName = DEFAULT_HEADER)
            {
                using (var FS = File.OpenRead(FullFileName))
                {
                    using (var IMG = File.OpenRead(ImageFile))
                    {
                        return CreateImageFromFile(FS, (new FileInfo(FullFileName)).Name, IMG, HeaderName);
                    }
                }
            }

            /// <summary>
            /// Stores data in a PNG header
            /// </summary>
            /// <param name="InputFile">Source File Stream</param>
            /// <param name="FileName">Source File Name (no path)</param>
            /// <param name="InputImage">Source Image Stream</param>
            /// <param name="HeaderName">Name of Header</param>
            /// <returns>PNG with custom header</returns>
            /// <remarks>This process is repeatable</remarks>
            public static byte[] CreateImageFromFile(Stream InputFile, string FileName, Stream InputImage, string HeaderName = DEFAULT_HEADER)
            {

                return null;
            }
        }

        public static class PixelMode
        {
            /// <summary>
            /// Bytes for each Pixel. In 24bpp Mode this is 24/8=3
            /// </summary>
            private const int BYTES_PER_PIXEL = 24 / 8;

            /// <summary>
            /// Saves binary data as an image
            /// </summary>
            /// <param name="FullFileName">Source file name to process</param>
            /// <param name="PNG">Use PNG instead of BMP</param>
            /// <param name="AllowDirectDecode">if true, data is stored so it appears in order when viewed as BMP</param>
            /// <returns>Image data</returns>
            public static byte[] CreateImageFromFile(string FullFileName, bool PNG = false, bool AllowDirectDecode = false)
            {
                using (var FS = File.OpenRead(FullFileName))
                {
                    return CreateImageFromFile(FS, (new FileInfo(FullFileName)).Name, PNG, AllowDirectDecode);
                }
            }

            /// <summary>
            /// Saves binary data as an image
            /// </summary>
            /// <param name="Input">Source content</param>
            /// <param name="FileName">file name to store</param>
            /// <param name="PNG">Use PNG instead of BMP</param>
            /// <param name="AllowDirectDecode">if true, data is stored so it appears in order when viewed as BMP</param>
            /// <returns>Image data</returns>
            public static byte[] CreateImageFromFile(Stream Input, string FileName, bool PNG = false, bool AllowDirectDecode = false)
            {
                if (Input == null)
                {
                    throw new ArgumentNullException(nameof(Input));
                }
                byte[] AllData = Tools.ReadAll(Input);
                byte[] Data =
                    //Header
                    Encoding.UTF8.GetBytes(MAGIC)
                    //File name length
                    .Concat(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(FileName)))
                    //File name
                    .Concat(Encoding.UTF8.GetBytes(FileName))
                    //Data length
                    .Concat(BitConverter.GetBytes(AllData.Length))
                    //Data
                    .Concat(AllData)
                    //Make array
                    .ToArray();

                var W = (int)Math.Sqrt(Data.Length / BYTES_PER_PIXEL);
                //Width must be a multiple of 4
                W -= W % 4;
                var H = (int)Math.Ceiling(Data.Length / (double)BYTES_PER_PIXEL / W);

                //The way W and H are created the image should be a square

                using (var B = new Bitmap(W, H, PixelFormat.Format24bppRgb))
                {
                    var Locker = B.LockBits(new Rectangle(0, 0, W, H), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

                    if (!AllowDirectDecode)
                    {
                        //In case of PNG we just write data "as-is"
                        Marshal.Copy(Data, 0, Locker.Scan0, Data.Length);
                    }
                    else
                    {
                        var Pos = Locker.Scan0 + (Locker.Stride * (Locker.Height - 1));
                        //For BMP we need to write the rows in reverse
                        for (var i = 0; i < Data.Length; i += Locker.Stride)
                        {
                            Marshal.Copy(Data, i, Pos, Locker.Stride < Data.Length - i ? Locker.Stride : Data.Length - i);
                            Pos -= Locker.Stride;
                        }
                    }

                    B.UnlockBits(Locker);

                    using (var MS = new MemoryStream())
                    {
                        B.Save(MS, PNG ? ImageFormat.Png : ImageFormat.Bmp);
                        return MS.ToArray();
                    }
                }
            }

            /// <summary>
            /// Extracts a file From an image
            /// </summary>
            /// <param name="Input"></param>
            /// <returns></returns>
            public static byte[] CreateFileFromImage(Stream Input)
            {
                using (Bitmap B = (Bitmap)Image.FromStream(Input))
                {
                    var Locker = B.LockBits(new Rectangle(0, 0, B.Width, B.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    using (var MS = new MemoryStream())
                    {
                        byte[] Data = new byte[Locker.Stride * Locker.Height];
                        Marshal.Copy(Locker.Scan0, Data, 0, Data.Length);
                        if ((new string(Data.Take(6).Select(m => (char)m).ToArray())) != MAGIC)
                        {
                            //Data is not in Order because BMP.
                            int Pos = Locker.Stride * Locker.Height - Locker.Stride;
                            using (var TempMS = new MemoryStream())
                            {
                                while (Pos >= 0)
                                {
                                    TempMS.Write(Data, Pos, Locker.Stride);
                                    Pos -= Locker.Stride;
                                }
                                Data = TempMS.ToArray();
                            }
                        }
                        //Data is in order now, get actual payload length
                        var Offset = BitConverter.ToInt32(Data, 6) + 6 + 4;
                        var DataLen = BitConverter.ToInt32(Data, Offset);
                        MS.Write(Data, Offset + 4, DataLen);
                        B.UnlockBits(Locker);
                        return MS.ToArray();
                    }
                }
            }

        }
    }
}
