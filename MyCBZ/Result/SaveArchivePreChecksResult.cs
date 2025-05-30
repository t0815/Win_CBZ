using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Result
{
    internal class SaveArchivePreChecksResult
    {
        public bool UpdatePageIndexMetadata { get; set; }

        public bool ApplyImageProcessing { get; set; }

        public bool WriteMetadataOnly { get; set; }
    }
}
