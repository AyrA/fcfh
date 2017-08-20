using System;
using System.IO;
using System.Linq;

namespace fcfh
{
    class Program
    {
        #region Structures
        /// <summary>
        /// Mode of operation of the tool
        /// </summary>
        [Flags]
        private enum OperationMode : int
        {
            /// <summary>
            /// Do nothing.
            /// This is unobtainable
            /// </summary>
            None = 0,
            /// <summary>
            /// Encode content into an Image
            /// </summary>
            Encode = 1,
            /// <summary>
            /// Decode Content from an Image
            /// </summary>
            Decode = Encode << 1,
            /// <summary>
            /// Use Pixel Mode
            /// </summary>
            UsePixel = Decode << 1,
            /// <summary>
            /// Use Header Mode
            /// </summary>
            UseHeader = UsePixel << 1,
            /// <summary>
            /// Order Pixel in binary readable form
            /// </summary>
            Readable = UseHeader << 1,
            /// <summary>
            /// Encrypt/Decrypt content
            /// </summary>
            Crypt = Readable << 1
        }

        /// <summary>
        /// Command line argument structure
        /// </summary>
        private struct CMD
        {
            /// <summary>
            /// Operation mode
            /// </summary>
            public OperationMode Mode;
            /// <summary>
            /// Source File
            /// </summary>
            public string Input;
            /// <summary>
            /// Destination File
            /// </summary>
            public string Output;
            /// <summary>
            /// Header File (Header Mode only)
            /// </summary>
            public string HeaderFile;
            /// <summary>
            /// Password (if passed as argument)
            /// </summary>
            public string Password;
            /// <summary>
            /// Overall command line argument validation
            /// </summary>
            public bool Valid;
        }
        #endregion

        static void Main(string[] args)
        {
#if DEBUG
            args = new string[] {
                "/e",
                //"/p",
                "/header",
                @"C:\Users\Administrator\Desktop\mp3.png",
                @"D:\Programme\VIDEO\ffmpeg\MP3\Bad Apple!!.mp3",
                @"C:\Users\Administrator\Desktop\Bad Apple.png"
            };
#endif
            var C = ParseArgs(args);
            if (C.Valid)
            {
                if (C.Mode.HasFlag(OperationMode.Crypt) && C.Password == null)
                {
                    C.Password = AskPass();
                    Console.Error.WriteLine("Please repeat");
                    if (C.Password != AskPass())
                    {
                        Console.Error.WriteLine("Passwords do not match");
                        return;
                    }
                }
                if (C.Mode.HasFlag(OperationMode.Encode))
                {
                    if (C.Output == null)
                    {
                        if (C.Mode.HasFlag(OperationMode.UseHeader) || !C.Mode.HasFlag(OperationMode.Readable))
                        {
                            C.Output = Path.ChangeExtension(C.Input, "png");
                        }
                        else
                        {
                            C.Output = Path.ChangeExtension(C.Input, "bmp");
                        }
                        if (File.Exists(C.Output))
                        {
                            Console.Error.WriteLine("Auto-generated output name already exists. Aborting");
                            return;
                        }
                    }
                    byte[] Data = File.ReadAllBytes(C.Input);
                    if (C.Password != null)
                    {
                        Data = EncryptData(C.Password, Data);
                    }
                    if (C.Mode.HasFlag(OperationMode.UseHeader))
                    {
                        //Header mode
                        using (var InputFile = new MemoryStream(Data, false))
                        {
                            using (var ImageFile = File.OpenRead(C.HeaderFile))
                            {
                                File.WriteAllBytes(C.Output,
                                ImageWriter.HeaderMode.CreateImageFromFile(InputFile, Tools.NameOnly(C.Input), ImageFile));
                            }
                        }
                        //Trying to find data again for debug
                        using (var FS = File.OpenRead(C.Output))
                        {
                            var Headers = ImageWriter.HeaderMode.ReadPNG(FS).Where(m => m.IsDataHeader);
                            foreach (var H in Headers)
                            {
                                Console.Error.WriteLine($"File: Name={Tools.NameOnly(H.FileName)}; Length={H.FileData.Length}");
                            }
                        }
                    }
                    else
                    {
                        //Pixel mode
                        using (var InputFile = new MemoryStream(Data, false))
                        {
                            File.WriteAllBytes(C.Output, ImageWriter.PixelMode.CreateImageFromFile(
                                InputFile,
                                C.Input,
                                C.Output.ToLower().EndsWith(".png"),
                                C.Mode.HasFlag(OperationMode.Readable)));
                        }
                    }
                }
                else if (C.Mode.HasFlag(OperationMode.Decode))
                {
                    var Source = File.ReadAllBytes(C.Input);
                    if (ImageWriter.HeaderMode.IsPNG(C.Input))
                    {
                        //PNG mode can be header or pixel data. Prefer header
                        var Headers = ImageWriter.HeaderMode.ReadPNG(Source);
                        if (Headers != null)
                        {
                            foreach (var H in Headers.Where(m => m.IsDataHeader))
                            {
                                if (C.Output == null)
                                {
                                    //Decode all files if name is missing
                                    File.WriteAllBytes(Tools.NameOnly(H.FileName), H.FileData);
                                }
                                else
                                {
                                    //Decode only one file
                                    File.WriteAllBytes(C.Output, H.FileData);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            using (var MS = new MemoryStream(Source, false))
                            {
                                var IF = ImageWriter.PixelMode.CreateFileFromImage(MS);
                                if (IF.IsEmpty)
                                {
                                    Console.Error.WriteLine("Can't find encoded file in Pixels or Header");
                                    return;
                                }
                                if (!string.IsNullOrEmpty(C.Password))
                                {
                                    IF.Data = DecryptData(C.Password, IF.Data);
                                }
                                File.WriteAllBytes(string.IsNullOrEmpty(C.Output) ? IF.FileName : C.Output, IF.Data);
                            }
                        }
                    }
                    else
                    {
                        //Bitmap decode is pixel mode only
                        using (var MS = new MemoryStream(Source, false))
                        {
                            var IF = ImageWriter.PixelMode.CreateFileFromImage(MS);
                            if (IF.IsEmpty)
                            {
                                Console.Error.WriteLine("Can't find encoded file in Pixels");
                                return;
                            }
                            if (!string.IsNullOrEmpty(C.Password))
                            {
                                IF.Data = DecryptData(C.Password, IF.Data);
                            }
                            File.WriteAllBytes(string.IsNullOrEmpty(C.Output) ? IF.FileName : C.Output, IF.Data);
                        }
                    }
                }
            }
            Console.Error.WriteLine("#END");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Ask a user for a password without showing the input he types
        /// </summary>
        /// <returns>Password</returns>
        private static string AskPass()
        {
            const char BLANK = '░';
            const char PASS = '█';
            const int MAXLEN = 40;
            string pass = "";
            if (Console.CursorLeft != 0)
            {
                Console.Error.WriteLine();
            }
            Console.Error.Write("Password: ");
            Console.Error.Write(string.Empty.PadRight(MAXLEN, BLANK));
            Console.CursorLeft -= 40;
            while (true)
            {
                var key = Console.ReadKey(true);
                if (char.IsControl(key.KeyChar))
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, pass.Length - 1);
                        Console.Error.Write($"\b{BLANK}\b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        if (pass.Length > 0)
                        {
                            Console.CursorLeft = 0;
                            Console.Error.Write(string.Empty.PadRight(Console.BufferWidth));
                            Console.CursorTop--;
                            return pass;
                        }
                        else
                        {
                            Console.Beep();
                        }
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        while (pass.Length > 0)
                        {
                            pass = pass.Substring(0, pass.Length - 1);
                            Console.Error.Write($"\b{BLANK}\b");
                        }
                    }
                }
                else
                {
                    if (pass.Length < 40)
                    {
                        pass += key.KeyChar;
                        Console.Error.Write(PASS);
                    }
                    else
                    {
                        Console.Beep();
                    }
                }
            }
        }

        /// <summary>
        /// Processes command line arguments
        /// </summary>
        /// <param name="Args">Arguments</param>
        /// <returns>Argument structure</returns>
        private static CMD ParseArgs(string[] Args)
        {
            int i = 0;
            CMD C = new CMD();
            C.Valid = true;
            C.Mode = OperationMode.UsePixel;

            while (C.Valid && i < Args.Length)
            {
                switch (Args[i].ToLower())
                {
                    case "/e":
                        if (!C.Mode.HasFlag(OperationMode.Decode))
                        {
                            if (!C.Mode.HasFlag(OperationMode.Encode))
                            {
                                C.Mode |= OperationMode.Encode;
                            }
                            else
                            {
                                Console.Error.WriteLine("/d specified multiple times");
                                C.Valid = false;
                            }
                        }
                        else
                        {
                            Console.Error.WriteLine("/e is mutually exclusive with /d");
                            C.Valid = false;
                        }
                        break;
                    case "/d":
                        if (!C.Mode.HasFlag(OperationMode.Encode))
                        {
                            if (!C.Mode.HasFlag(OperationMode.Decode))
                            {
                                C.Mode |= OperationMode.Decode;
                            }
                            else
                            {
                                Console.Error.WriteLine("/d specified multiple times");
                                C.Valid = false;
                            }
                        }
                        else
                        {
                            Console.Error.WriteLine("/e is mutually exclusive with /d");
                            C.Valid = false;
                        }
                        break;
                    case "/p":
                        if (!C.Mode.HasFlag(OperationMode.Crypt))
                        {
                            C.Mode |= OperationMode.Crypt;
                        }
                        else
                        {
                            Console.Error.WriteLine("/p or /pass specified already");
                            C.Valid = false;
                        }
                        break;
                    case "/pass":
                        if (!C.Mode.HasFlag(OperationMode.Crypt))
                        {
                            C.Mode |= OperationMode.Crypt;
                            if (i < Args.Length - 1)
                            {
                                C.Password = Args[++i];
                            }
                            else
                            {
                                Console.Error.WriteLine("Missing password for /pass. Did you mean to use /p?");
                                C.Valid = false;
                            }
                        }
                        else
                        {
                            Console.Error.WriteLine("/p or /pass specified already");
                            C.Valid = false;
                        }
                        break;
                    case "/readable":
                        if (!C.Mode.HasFlag(OperationMode.Readable))
                        {
                            C.Mode |= OperationMode.Readable;
                        }
                        else
                        {
                            Console.Error.WriteLine("/readable specified multiple times");
                            C.Valid = false;
                        }
                        break;
                    case "/header":
                        if (i < Args.Length - 1)
                        {
                            C.HeaderFile = Args[++i];
                            if (File.Exists(C.HeaderFile))
                            {
                                if (!C.Mode.HasFlag(OperationMode.UseHeader))
                                {
                                    if (C.Mode.HasFlag(OperationMode.UsePixel))
                                    {
                                        C.Mode ^= OperationMode.UsePixel;
                                    }
                                    C.Mode |= OperationMode.UseHeader;
                                }
                                else
                                {
                                    Console.Error.WriteLine("/header specified multiple times");
                                    C.Valid = false;
                                }
                            }
                            else
                            {
                                Console.Error.WriteLine("Header file not found");
                            }
                        }
                        else
                        {
                            Console.Error.WriteLine("/header argument requires a PNG file argument after it");
                            C.Valid = false;
                        }
                        break;
                    default:
                        if (C.Input == null)
                        {
                            C.Input = Args[i];
                            if (!File.Exists(C.Input))
                            {
                                Console.Error.WriteLine("Input file not found");
                                C.Valid = false;
                            }
                        }
                        else if (C.Output == null)
                        {
                            C.Output = Args[i];
                        }
                        else
                        {
                            Console.Error.WriteLine("Unknown command line argument: {0}", Args[i]);
                            C.Valid = false;
                        }
                        break;
                }
                ++i;
            }
            if (C.Input == null)
            {
                Console.Error.WriteLine("No input file specified");
                C.Valid = false;
            }
            return C;
        }

        /// <summary>
        /// Shows Help
        /// </summary>
        private static void ShowHelp()
        {
            Console.Error.WriteLine(@"{0} /{{e|d}} <infile> [outfile] [/readable] [/header source] [/p|/pass password]

Encodes and decodes information to/from images.
This tool is not using steganography and literally just stores the data in an
image container, but ensures that said container is valid.

/e          - Encode a file into an image
/d          - Decode a file from an image
infile      - Source file
outfile     - Destination file
              If not specified it assumes png for header encoded data,
              Assumes PNG for pixel data without '/readable'
              Assumes BMP for pixel data with '/readable'.
/readable   - Encodes content in binary readable form.
              Only has an effect if encoding to 24 bit bitmap (.bmp),
              because bitmap files are stored bottom up.
/header     - Store content in a header instead of pixel data.
              Be aware that some applications strip unknown headers.
              The source argument is the file that is used as template.
              This only works on PNG files.
              Note: Some editors will strip unknown headers if you edit
              the image file.
/pass       - Encrypt/Decrypt using the given password.
              This uses proper AES, recovery of content is impossible
              if the password is lost.
/p            Same as /pass but prompts for a password at runtime.

Note: When decoding, the arguments /readable and /header are auto-detected", Tools.ProcessName);
        }

        /// <summary>
        /// Encrypts Data with a password
        /// </summary>
        /// <param name="Password">Password</param>
        /// <param name="Data">Data</param>
        /// <returns>Encrypted Data</returns>
        private static byte[] EncryptData(string Password, byte[] Data)
        {
            var C = new crypt.Crypt();
            C.GenerateSalt();
            C.GeneratePassword(Password);
            using (var IN = new MemoryStream(Data, false))
            {
                using (var OUT = new MemoryStream())
                {
                    C.Encrypt(IN, OUT);
                    return OUT.ToArray();
                }
            }
        }

        /// <summary>
        /// Decrypts data encrypted with <see cref="EncryptData"/>
        /// </summary>
        /// <param name="Password">Password</param>
        /// <param name="Data">Encrypted data</param>
        /// <returns>Decrypted data</returns>
        private static byte[] DecryptData(string Password, byte[] Data)
        {
            var C = new crypt.Crypt();
            //C.GenerateSalt();
            //C.GeneratePassword(Password, 10000);
            using (var IN = new MemoryStream(Data, false))
            {
                using (var OUT = new MemoryStream())
                {
                    C.Decrypt(IN, OUT, Password);
                    return OUT.ToArray();
                }
            }
        }
    }
}
