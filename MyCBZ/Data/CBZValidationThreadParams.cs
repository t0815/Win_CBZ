using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class CBZValidationThreadParams : ThreadParam
    {
        public bool ShowDialog { get; set; }

        public List<Page> Pages { get; set; }   

        public MetaData.PageIndexVersion PageIndexVersion { get; set; }

    }
}
