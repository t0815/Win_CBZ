﻿using System;
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

        public bool ContinueOnError { get; set; } = false;

        public MetaData.PageIndexVersion CurrentPageIndexVer { get; set; } = MetaData.PageIndexVersion.VERSION_1;

        public ZipArchiveMode Mode { get; set; }

        public bool SkipIndexCheck { get; set; } = false;

        public bool WriteIndex { get; set; } = true;

        public bool SkipFilesInSubDirectories { get; set; } = true;

        public string Interpolation { get; set; } = "Default";

        public bool ApplyKeyUserFilter { get; set; } = false;

        public string[] FilterKeys { get; set; } = null;

        public int FilterBaseCondition { get; set; } = 0;
    }
}
