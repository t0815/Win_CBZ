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
    internal class AddImagesThreadParams : ThreadParam
    {
        public List<LocalFile> LocalFiles { get; set; }

        public MetaData.PageIndexVersion PageIndexVerToWrite { get; set; } = MetaData.PageIndexVersion.VERSION_1;

        public string[] InvalidFileNames { get; set; } = new string[0];

        public string[] FilterExtensions { get; set; } = new string[0];

        public string[] FilterFileNames { get; set; } = new string[0];

        public int MaxCountPages { get; set; } = 0;

        public bool HashFiles { get; set; } = false;

        public string Interpolation { get; set; } = "Default";
    }
}
