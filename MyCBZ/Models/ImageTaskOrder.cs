using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Models
{

    public enum ImageTaskOrderValue
    {
        Auto,
        no_1,
        no_2,
        no_3,
        no_4,
    }

    internal class ImageTaskOrder
    {

        public ImageTaskOrderValue Convert { get; set; } = ImageTaskOrderValue.Auto;

        public ImageTaskOrderValue Resize { get; set; } = ImageTaskOrderValue.Auto;

        public ImageTaskOrderValue Rotate { get; set; } = ImageTaskOrderValue.Auto;

        public ImageTaskOrderValue Split { get; set; } = ImageTaskOrderValue.Auto;


    }
}
