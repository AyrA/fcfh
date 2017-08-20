using System.Diagnostics;
using System.IO;
using System.Linq;

namespace fcfh
{
    public static class Tools
    {
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
        /// Returns the last component of a Path string
        /// </summary>
        /// <remarks>File/Directory does not needs to exist</remarks>
        /// <param name="input">File or directory path</param>
        /// <returns>Name component</returns>
        public static string NameOnly(string input)
        {
            return input
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .Split(Path.DirectorySeparatorChar)
                .Last()
                .Split(Path.AltDirectorySeparatorChar)
                .Last();
        }

        //No real reason to have those, they are just a shortcut
        #region Integer Utils

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

    }
}
