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
    }
}
