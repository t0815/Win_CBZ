using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Base
{
    internal class Invalidatable
    {
        public bool Invalidated { get; set; } = false;
        
        public void Invalidate(bool invalid = true)
        {
            Invalidated = invalid;
        }
        
    }
}
