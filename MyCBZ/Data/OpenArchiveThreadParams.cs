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
    internal class OpenArchiveThreadParams : ThreadParam
    {
        public String FileName { get; set; }

        public bool ContinueOnError { get; set; }

        public MetaData.PageIndexVersion CurrentPageIndexVer { get; set; } = MetaData.PageIndexVersion.VERSION_1;

        public ZipArchiveMode Mode { get; set; }

        public bool SkipIndexCheck { get; set; }

        public bool ContinuePipeline { get; set; }

        public string Interpolation { get; set; } = "Default";
    }
}
