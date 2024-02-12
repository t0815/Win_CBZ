using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Components.Rating
{
    public class RatingChangedEvent
    {

        public int Value { get; }

        public RatingChangedEvent(int value) 
        {
            Value = value;
        }
    }
}
