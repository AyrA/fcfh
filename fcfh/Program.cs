using System;
using System.IO;

namespace fcfh
{
    class Program
    {
        static void Main(string[] args)
        {
            //OGG --> IMG
            var Data = File.ReadAllBytes(@"C:\Users\Administrator\Desktop\x.ogg");
            using (var MS = new MemoryStream(Data, false))
            {
                File.WriteAllBytes(@"C:\Users\Administrator\Desktop\x.png", ImageWriter.CreateImageFromFile(MS, "x.ogg", true, true));
            }
            //IMG --> OGG
            Data = File.ReadAllBytes(@"C:\Users\Administrator\Desktop\x.png");
            using (var MS = new MemoryStream(Data, false))
            {
                File.WriteAllBytes(@"C:\Users\Administrator\Desktop\y.ogg", ImageWriter.CreateFileFromImage(MS));
            }


            Console.Error.WriteLine("#END");
            Console.ReadKey(true);
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
            C.GeneratePassword(Password, 10000);
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
