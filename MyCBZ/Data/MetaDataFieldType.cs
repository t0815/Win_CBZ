using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class MetaDataFieldType
    {
        public String Name { get; set; }

        public String FieldType { get; set; }

        public String EditorType { get; set; }

        public String Options { get; set; }
        
        public MetaDataFieldType(String name, String fieldType, String editorType, String options) 
        {
            Name = name;
            FieldType = fieldType;
            EditorType = editorType;
            Options = options;
        }

        public override String ToString()
        {
            return Name + "|" + FieldType + "|" + EditorType + "|" + Options;
        }
    }
}
