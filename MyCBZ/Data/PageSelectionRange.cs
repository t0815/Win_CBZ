using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    public class PageSelectionRange
    {
        public int Start { get; set; }
        public int End { get; set; }

        public PageSelectionRange(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(int index)
        {
            return index >= Start && index <= End;
        }

        public bool Overlaps(PageSelectionRange other)
        {
            return !(other.End < Start || other.Start > End);
        }

        public override string ToString()
        {
            return $"{Start} - {End}";
        }
    }
}
