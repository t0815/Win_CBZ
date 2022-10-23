using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class CBZLocalFile
    {

        public String FileName { get; set; }

        public String FilePath { get; set; }

        public String FullPath { get; set; }

        public long FileSize { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public FileInfo FileInfo { get; set; }


        public CBZLocalFile(String fileName)
        {
            FullPath = fileName;
        }

    }
}
