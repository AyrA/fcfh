using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace fcfh
{
    public static class ImageWriter
    {
        public const string MAGIC = "BMPENC";

        /// <summary>
        /// BMPENC file
        /// </summary>
        public struct ImageFile
        {
            /// <summary>
            /// Checks if this Instance is Empty or Valid
            /// </summary>
            public bool IsEmpty
            {
                get
                {
                    return FileName == null && Data == null;
                }
            }

            /// <summary>
            /// File Name
            /// </summary>
            public string FileName;
            /// <summary>
            /// File Data
            /// </summary>
            public byte[] Data;
        }

        /// <summary>
        /// Provides encoding and decoding from Image Headers
        /// </summary>
        public static class HeaderMode
        {

            /// <summary>
            /// Header name
            /// </summary>
            /// <remarks>Upper-and lowercase follow a format: http://www.libpng.org/pub/png/spec/1.2/PNG-Structure.html#Chunk-naming-conventions</remarks>
            public const string DEFAULT_HEADER = "fcFh";

            /// <summary>
            /// Reads a byte array as PNG
            /// </summary>
            /// <param name="Data">Image Bytes</param>
            /// <returns>Header list</returns>
            public static PNGHeader[] ReadPNG(byte[] Data)
            {
                using (var MS = new MemoryStream(Data, false))
                {
                    return ReadPNG(MS);
                }
            }

            /// <summary>
            /// Reads a stream as PNG
            /// </summary>
            /// <remarks>Stream must start with a PNG header at the current position, Stream is left open</remarks>
            /// <param name="S">Stream</param>
            /// <returns>Header list</returns>
            public static PNGHeader[] ReadPNG(Stream S)
            {
                var Headers = new List<PNGHeader>();

                byte[] HeaderData = new byte[8];
                S.Read(HeaderData, 0, 8);
                if (BitConverter.ToUInt64(HeaderData, 0) == PNGHeader.PNG_MAGIC)
                {
                    do
                    {
                        Headers.Add(new PNGHeader(S));
                    } while (Headers.Last().HeaderName != "IEND");
                }
                return Headers.ToArray();
            }

            /// <summary>
            /// Reads a file as PNG
            /// </summary>
            /// <param name="FileName">File name</param>
            /// <returns>Header list</returns>
            public static PNGHeader[] ReadPNG(string FileName)
            {
                using (var FS = File.OpenRead(FileName))
                {
                    return ReadPNG(FS);
                }
            }

            /// <summary>
            /// Writes a collection of PNG Headers to a byte array
            /// </summary>
            /// <remarks>This will check for IHDR and IEND positions</remarks>
            /// <param name="Headers">PNG Headers</param>
            /// <returns>PNG bytes</returns>
            public static byte[] WritePNG(IEnumerable<PNGHeader> Headers)
            {
                var Arr = Headers.ToArray();
                if (Arr.Length > 1 && Arr.First().HeaderName == "IHDR" && Arr.Last().HeaderName == "IEND")
                {
                    using (var MS = new MemoryStream())
                    {
                        MS.Write(BitConverter.GetBytes(PNGHeader.PNG_MAGIC), 0, 8);
                        foreach (var H in Headers)
                        {
                            H.WriteHeader(MS);
                        }
                        return MS.ToArray();
                    }
                }
                return null;
            }

            /// <summary>
            /// Checks if the given File is a PNG
            /// </summary>
            /// <remarks>Only checks the 8 byte header</remarks>
            /// <param name="FileName">File name</param>
            /// <returns>True if PNG, false otherwise or on I/O error</returns>
            public static bool IsPNG(string FileName)
            {
                try
                {
                    using (var FS = File.OpenRead(FileName))
                    {
                        using (var BR = new BinaryReader(FS))
                        {
                            return BR.ReadUInt64() == PNGHeader.PNG_MAGIC;
                        }
                    }
                }
                catch
                {
                }
                return false;
            }

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
                        return CreateImageFromFile(FS, Path.GetFileName(FullFileName), IMG, HeaderName);
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
                var Headers = ReadPNG(InputImage).ToList();
                if (Headers.Count > 0)
                {
                    var Data = Tools.ReadAll(InputFile);
                    Headers.Insert(1, new PNGHeader(HeaderName,
                        Encoding.Default.GetBytes(MAGIC)
                        .Concat(BitConverter.GetBytes(Tools.IntToNetwork(Encoding.Default.GetByteCount(FileName))))
                        .Concat(Encoding.Default.GetBytes(FileName))
                        .Concat(BitConverter.GetBytes(Tools.IntToNetwork(Data.Length)))
                        .Concat(Data)
                        .ToArray()));
                    return WritePNG(Headers);
                }
                return null;
            }
        }

        /// <summary>
        /// Provides encoding and decoding from Pixel Data
        /// </summary>
        public static class PixelMode
        {
            /// <summary>
            /// Bytes for each Pixel. In 24bpp Mode this is 24/8=3
            /// </summary>
            private const int BYTES_PER_PIXEL = 24 / 8;

            /// <summary>
            /// Saves binary Data as an Image
            /// </summary>
            /// <param name="FullFileName">Source File Name to process</param>
            /// <param name="PNG">Use PNG instead of BMP</param>
            /// <param name="AllowDirectDecode">if true, Data is stored so it appears in Order when viewed as BMP</param>
            /// <returns>Image Data</returns>
            public static byte[] CreateImageFromFile(string FullFileName, bool PNG = false, bool AllowDirectDecode = false)
            {
                using (var FS = File.OpenRead(FullFileName))
                {
                    return CreateImageFromFile(FS, Path.GetFileName(FullFileName), PNG, AllowDirectDecode);
                }
            }

            /// <summary>
            /// Saves binary Data as an Image
            /// </summary>
            /// <param name="Input">Source Content</param>
            /// <param name="FileName">File Name to store</param>
            /// <param name="PNG">Use PNG instead of BMP</param>
            /// <param name="AllowDirectDecode">if true, Data is stored so it appears in Order when viewed as BMP</param>
            /// <returns>Image Data</returns>
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
                    .Concat(BitConverter.GetBytes(Tools.IntToNetwork(Encoding.UTF8.GetByteCount(FileName))))
                    //File name
                    .Concat(Encoding.UTF8.GetBytes(FileName))
                    //Data length
                    .Concat(BitConverter.GetBytes(Tools.IntToNetwork(AllData.Length)))
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
            /// Extracts a File From an Image
            /// </summary>
            /// <param name="Input"></param>
            /// <returns></returns>
            public static ImageFile CreateFileFromImage(Stream Input)
            {
                using (Bitmap B = (Bitmap)Image.FromStream(Input))
                {
                    var Locker = B.LockBits(new Rectangle(0, 0, B.Width, B.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
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
                    if ((new string(Data.Take(6).Select(m => (char)m).ToArray())) == MAGIC)
                    {
                        //Data is in order now, get actual payload length
                        var FileName = Encoding.UTF8.GetString(Data, 10, Tools.IntToHost(BitConverter.ToInt32(Data, 6)));
                        var Offset = Tools.IntToHost(BitConverter.ToInt32(Data, 6)) + 6 + 4;
                        var DataLen = Tools.IntToHost(BitConverter.ToInt32(Data, Offset));
                        ImageFile IF = new ImageFile()
                        {
                            Data = Data.Skip(Offset + 4).Take(DataLen).ToArray(),
                            FileName = FileName
                        };
                        B.UnlockBits(Locker);
                        return IF;
                    }
                    //Even after reordering, there is no encoded pixel data
                    B.UnlockBits(Locker);
                    return default(ImageFile);
                }
            }

        }
    }
}
