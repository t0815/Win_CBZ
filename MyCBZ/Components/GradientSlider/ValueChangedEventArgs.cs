using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Components.GradientSlider
{
    public class ValueChangedEventArgs : EventArgs
    {
       
        public ValueChangedEventArgs()
        {
        }

        public ValueChangedEventArgs(float newValue)
        {
            this.NewValue = newValue;
        }

        public float NewValue { get; set; }
    }
    
}
