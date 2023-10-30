using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    public class EditorTypeConfig
    {

        public EditorTypeConfig(string type, string resultType, string separator, string append, bool allowDuplicate) 
        {
            ResultType = resultType;
            Type = type;
            Separator = separator;
            Append = append;
            AllowDuplicateValues = allowDuplicate;
        }

        public EditorTypeConfig(string type, string resultType)
        {
            ResultType = resultType;
            Type = type;
            Separator = ",";
            Append = " ";
        }

        public EditorTypeConfig(string type)
        {
            Type = type;
            Separator = ",";
            Append = " ";
            ResultType = "String";
        }

        public string Type { get; set; }

        public string Separator { get; set; }

        public String Value { get; set; }

        public object DefaultValue { get; set; }

        public string Append {  get; set; }

        public object Result { get; set; }

        public string ResultType { get; set; }

        public bool AllowDuplicateValues { get; set; }

    }
}
