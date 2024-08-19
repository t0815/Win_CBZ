using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class IndexToDataMappings
    {

        protected Dictionary<int, string> ImageFormatIndexMapping = new Dictionary<int, string>()
        {
            { 0, null },
            { 1, "bmp" },
            { 2, "jpg" },
            { 3, "png" },
            { 4, "tif" }
        };

        static IndexToDataMappings instance;

        public static IndexToDataMappings GetInstance()
        {
            instance ??= new IndexToDataMappings();

            return instance;
        }

        public string GetImageFormatNameFromIndex(int index)
        {
            string outValue;

            ImageFormatIndexMapping.TryGetValue(index, out outValue);

            return outValue;
        }
    }
}
