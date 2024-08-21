using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class ExtractArchiveThreadParams : ThreadParam
    {
        public String OutputPath { get; set; }

        public List<Page> Pages { get; set; }

        public bool ContinuePipeline { get; set; }
    }
}
