﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class UpdatePageIndicesThreadParams
    {
        public bool InitialIndexRebuild {  get; set; }

        public bool ContinuePipeline {  get; set; }

        public List<StackItem> Stack {  get; set; }   
    }
}
