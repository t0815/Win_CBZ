using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class SaveArchiveThreadParams
    {

        public String FileName {  get; set; }

        public bool ContinueOnError { get; set; }

        public ZipArchiveMode Mode { get; set; }

        public CompressionLevel CompressionLevel { get; set; }
    }
}
