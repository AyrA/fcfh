using System;
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
        public static class PixelMode
        {
            private const int BYTES_PER_PIXEL = 3;

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
                    Encoding.UTF8.GetBytes("BMPENC")
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
                        if ((new string(Data.Take(6).Select(m => (char)m).ToArray())) != "BMPENC")
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
