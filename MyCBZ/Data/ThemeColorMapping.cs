using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class ThemeColorMapping
    {

        public string Label { get; set; }

        public string ColorName { get; set; }

        public string ColorValue { get; set; }

        public string Description { get; set; }

        public string DefaultValue { get; set; }

        public string Category { get; set; }

        public List<string> ExampleValues { get; set; } = new List<string>();


        public void AddExampleValue(string value)
        {
            if (!ExampleValues.Contains(value))
            {
                ExampleValues.Add(value);
            }
        }
    }
}
