using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    [SupportedOSPlatform("windows")]
    internal class SaveArchiveThreadParams
    {

        public String FileName { get; set; }

        public bool ContinueOnError { get; set; }

        public ZipArchiveMode Mode { get; set; }

        public CompressionLevel CompressionLevel { get; set; }

        public MetaData.PageIndexVersion PageIndexVerToWrite { get; set; } = MetaData.PageIndexVersion.VERSION_1;

        public CancellationToken CancelToken { get; set; }
    }
}
