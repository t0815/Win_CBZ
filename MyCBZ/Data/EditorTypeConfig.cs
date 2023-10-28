using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    public class EditorTypeConfig
    {

        public EditorTypeConfig(string type, string resultType, string separator, string append) 
        {
            ResultType = resultType;
            Type = type;
            Separator = separator;
            Append = append;
        }

        
        public string Type { get; set; }

        public string Separator { get; set; }

        public String Value { get; set; }

        public string Append {  get; set; }

        public object Result { get; set; }

        public string ResultType { get; set; }

    }
}
