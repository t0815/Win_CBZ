﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class DeleteTemporaryFilesThreadParams : ThreadParam
    {
        public String Path { get; set; }

        public bool ContinuePipeline { get; set; }
    }
}