using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class EditorFieldMapping
    {
        public const String MetaDataFieldTypeText = "TextBox";
        public const String MetaDataFieldTypeComboBox = "ComboBox";
        public const String MetaDataFieldTypeAutoComplete = "AutoComplete";


        public String FieldType;

        public String EditorType;

        public String[] EditorOptions;
    }
}
