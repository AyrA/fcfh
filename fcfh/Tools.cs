using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace fcfh
{
    public static class Tools
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

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

        public static bool HasConsole
        {
            get
            {
                return GetConsoleWindow() != IntPtr.Zero;
            }
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
