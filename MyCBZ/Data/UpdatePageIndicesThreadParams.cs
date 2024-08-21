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
    internal class UpdatePageIndicesThreadParams : ThreadParam
    {
        public bool InitialIndexRebuild { get; set; }

        public bool ContinuePipeline { get; set; }

        public List<Page> Pages { get; set; }

        public MetaData.PageIndexVersion PageIndexVerToWrite { get; set; } = MetaData.PageIndexVersion.VERSION_1;
    }
}
