using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Hashing;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;
using System.IO;
using System.Runtime.Versioning;
using System.Numerics;

namespace Win_CBZ.Hash
{
    internal class HashCrc32
    {
        [SupportedOSPlatform("windows")]
        public static void Calculate(ref Page page) 
        {
            if (page == null)
            {
                throw new ArgumentNullException("page");
            }

            if (page.LocalFile == null)
            {
                throw new ArgumentNullException("page.LocalFile");
            }

            if (!page.LocalFile.Exists())
            {
                throw new FileNotFoundException("File not found", page.LocalFile.FullPath);
            }

            LocalFile localFile = page.LocalFile;
            if (page.Compressed || page.Changed)
            {
                localFile = page.TemporaryFile;
            }

            using (var stream = localFile.LocalFileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var crc32 = new Crc32();
                crc32.Append(stream);

                var hash = crc32.GetHashAndReset();

                page.Hash = BitConverter.ToString(hash).Replace("-", "").ToUpper();

                crc32 = null;
            }
        }  
    }
}
