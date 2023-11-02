using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class MinMaxDimensions
    {

        public Page Page { get; set; }

        public int MinWidth { get; set; } = 0;

        public int MinHeight { get; set; } = 0;

        public int MaxWidth { get; set; } = 0;

        public int MaxHeight { get; set; } = 0;
    }
}
