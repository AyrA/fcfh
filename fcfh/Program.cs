﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace fcfh
{
    class Program
    {
        [Flags]
        private enum OperationMode : int
        {
            Encode = 1,
            Decode = Encode << 1,
            UsePixel = Decode << 1,
            UseHeader = UsePixel << 1,
            Crypt = UseHeader << 1
        }

        private struct CMD
        {
            public OperationMode Mode;
            public string Input;
            public string Output;
            public string Password;
        }

        static void Main(string[] args)
        {
            Console.Error.WriteLine("#END");
            Console.ReadKey(true);
        }

        private static void ShowHelp()
        {
            Console.Error.WriteLine(@"{0} /{{e|d}} <infile> [outfile] [/readable] [/header source] [/pass [password]]

Encodes and decodes information into images.
This tool is not using steganography and literally just stores the data in an
image container, but ensures that said container is valid.

/e          - Encode a file into an image
/d          - Decode a file from an image
infile      - Source file
outfile     - Destination file
/readable   - Encodes content in binary readable form.
              Only has an effect if encoding to 24 bit bitmap (.bmp),
              because bitmap files are stored bottom up.
/header     - Store content in a header instead of pixel data.
              Be aware that some applications strip unknown headers.
              The source argument is the file that is used as template.
              Note: Some editors will strip unknown headers if you edit
              the image file.
/pass       - Encrypt/Decrypt using the given password.
              This uses proper AES, recovery of content is impossible
              if the password is lost. If no password is given, you
              are prompted to enter one at runtime.", Tools.ProcessName);
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
