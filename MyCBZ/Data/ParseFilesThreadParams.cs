using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    [SupportedOSPlatform("windows")]
    internal class ParseFilesThreadParams : ThreadParam
    {
         
        public List<string> FileNamesToAdd { get; set; }

        public bool HasMetaData { get; set; }

        public int MaxCountPages { get; set; } = 0;

        public List<Page> Pages { get; set; }

        public MetaData.PageIndexVersion PageIndexVerToWrite { get; set; } = MetaData.PageIndexVersion.VERSION_1;

        public bool HashFiles { get; set; } = false;

        public bool ContinuePipeline { get; set; }
    }
}
