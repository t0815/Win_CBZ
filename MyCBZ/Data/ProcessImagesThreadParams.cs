using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win_CBZ.Models;

namespace Win_CBZ.Data
{
    internal class ProcessImagesThreadParams : ThreadParam
    {

        public bool ApplyImageProcessing { get; set; }

        public string[] SkipPages { get; set; } = new string[0];

        public List<Page> Pages { get; set; }

        public ProcessImagesThreadParams() { }

    }
}
