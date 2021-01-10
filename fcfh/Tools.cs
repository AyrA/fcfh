using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace fcfh
{
    public static class Tools
    {
        public const string BROWSE_FILTER_ALL = "All files|*.*";

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        private static extern int GetConsoleProcessList(IntPtr mem, int count);

        private const int ATTACH_PARENT_PROCESS = -1;

        private static string _ProcessName;

        /// <summary>
        /// Gets the current processes file name
        /// </summary>
        public static string ProcessName
        {
            get
            {
                if (string.IsNullOrEmpty(_ProcessName))
                {
                    using (var Proc = Process.GetCurrentProcess())
                    {
                        _ProcessName = (new FileInfo(Proc.MainModule.FileName)).Name;
                    }
                }
                return _ProcessName;
            }
        }

        public static bool HasConsole()
        {
            IntPtr Ptr = Marshal.AllocHGlobal(sizeof(int));
            int count = 0;
            try
            {
                count = GetConsoleProcessList(Ptr, 1);
                if (count <= 0)
                {
                    throw new Win32Exception();
                }
            }
            finally
            {
                Marshal.FreeHGlobal(Ptr);
            }
            return count > 1;
        }

        public static string BrowseFile(string Title, string Filter = BROWSE_FILTER_ALL, string Preselected = null, bool IsSave = false)
        {
            FileDialog FD;
            if (IsSave)
            {
                FD = new SaveFileDialog();
                ((SaveFileDialog)FD).OverwritePrompt = true;
            }
            else
            {
                FD = new OpenFileDialog();
            }
            using (FD)
            {
                if (string.IsNullOrEmpty(Title))
                {
                    FD.Title = IsSave ? "Save As..." : "Open...";
                }
                else
                {
                    FD.Title = Title;
                }
                if (string.IsNullOrEmpty(Filter))
                {
                    FD.Filter = BROWSE_FILTER_ALL;
                }
                else
                {
                    FD.Filter = Filter;
                }
                if (!string.IsNullOrEmpty(Preselected))
                {
                    if (Directory.Exists(Preselected))
                    {
                        FD.InitialDirectory = Preselected;
                    }
                    else
                    {
                        FD.InitialDirectory = Path.GetDirectoryName(Preselected);
                        FD.FileName = Path.GetFileName(Preselected);
                    }
                }
                if (FD.ShowDialog() == DialogResult.OK)
                {
                    return FD.FileName;
                }
            }
            return null;
        }

        /// <summary>
        /// Reads all (remaining) content of a a stream
        /// </summary>
        /// <param name="In">Source stream</param>
        /// <returns>Bytes read</returns>
        /// <remarks>Will not rewind stream</remarks>
        public static byte[] ReadAll(Stream In)
        {
            using (MemoryStream MS = new MemoryStream())
            {
                In.CopyTo(MS);
                return MS.ToArray();
            }
        }


        /// <summary>
        /// Encrypts Data with a password
        /// </summary>
        /// <param name="Password">Password</param>
        /// <param name="Data">Data</param>
        /// <returns>Encrypted Data</returns>
        public static byte[] EncryptData(string Password, byte[] Data)
        {
            var C = new crypt.Crypt();
            C.GenerateSalt();
            C.GeneratePassword(Password);
            using (var IN = new MemoryStream(Data, false))
            {
                using (var OUT = new MemoryStream())
                {
                    if (C.Encrypt(IN, OUT) == crypt.Crypt.CryptResult.Success)
                    {
                        return OUT.ToArray();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Decrypts data encrypted with <see cref="EncryptData"/>
        /// </summary>
        /// <param name="Password">Password</param>
        /// <param name="Data">Encrypted data</param>
        /// <returns>Decrypted data</returns>
        public static byte[] DecryptData(string Password, byte[] Data)
        {
            var C = new crypt.Crypt();
            using (var IN = new MemoryStream(Data, false))
            {
                using (var OUT = new MemoryStream())
                {
                    if (C.Decrypt(IN, OUT, Password) == crypt.Crypt.CryptResult.Success)
                    {
                        return OUT.ToArray();
                    }
                }
            }
            return null;
        }

        //No real reason to have those, they are just a shortcut
        #region Integer Utils

        /// <summary>
        /// Host to network (int)
        /// </summary>
        /// <param name="i">Number</param>
        /// <returns>Number</returns>
        public static int IntToNetwork(int i)
        {
            return System.Net.IPAddress.HostToNetworkOrder(i);
        }

        /// <summary>
        /// Host to network (uint)
        /// </summary>
        /// <param name="i">Number</param>
        /// <returns>Number</returns>
        public static uint IntToNetwork(uint i)
        {
            return (uint)System.Net.IPAddress.HostToNetworkOrder((int)i);
        }

        /// <summary>
        /// Network to Host (int)
        /// </summary>
        /// <param name="i">Number</param>
        /// <returns>Number</returns>
        public static int IntToHost(int i)
        {
            return System.Net.IPAddress.NetworkToHostOrder(i);
        }

        /// <summary>
        /// Network to Host (uint)
        /// </summary>
        /// <param name="i">Number</param>
        /// <returns>Number</returns>
        public static uint IntToHost(uint i)
        {
            return (uint)System.Net.IPAddress.NetworkToHostOrder((int)i);
        }
        #endregion

    }
}
