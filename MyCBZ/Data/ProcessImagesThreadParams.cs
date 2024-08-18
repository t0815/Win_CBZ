using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class ProcessImagesThreadParams
    {

        public bool ApplyImageProcessing { get; set; }

        public bool ContinuePipeline { get; set; } = true;

        public string[] SkipPages { get; set; } = new string[0];


    }
}
