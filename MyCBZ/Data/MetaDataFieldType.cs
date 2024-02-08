using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class MetaDataFieldType
    {

        public const String METADATA_FIELD_TYPE_TEXT_BOX = "TextBox";
        public const String METADATA_FIELD_TYPE_COMBO_BOX = "ComboBox";
        public const String METADATA_FIELD_TYPE_AUTO_COMPLETE = "AutoComplete";

        public static string[] FieldTypes =
        {
            MetaDataFieldType.METADATA_FIELD_TYPE_TEXT_BOX,
            MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX,
            MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE,
        };


        public String Name { get; set; }

        public String FieldType { get; set; }

        public String EditorType { get; set; }

        public String Options { get; set; }

        public bool AutoUpdate { get; set; }

        public EditorTypeConfig EditorConfig { get; set; } = new EditorTypeConfig(EditorTypeConfig.EDITOR_TYPE_NONE);
        

        public MetaDataFieldType() 
        {
            FieldType = MetaDataFieldType.METADATA_FIELD_TYPE_TEXT_BOX;
            EditorType = EditorTypeConfig.EDITOR_TYPE_NONE;
            AutoUpdate = false;
        }


        public MetaDataFieldType(String name)
        {
            Name = name;
            FieldType = MetaDataFieldType.METADATA_FIELD_TYPE_TEXT_BOX;
            EditorType = EditorTypeConfig.EDITOR_TYPE_NONE;
            Options = "";
            AutoUpdate = false;
        }

        public MetaDataFieldType(String name, String type)
        {
            Name = name;
            FieldType = type;
            EditorType = EditorTypeConfig.EDITOR_TYPE_NONE;
            Options = "";
            AutoUpdate = false;
        }

        public MetaDataFieldType(String name, String fieldType, String editorType, String options, bool autoUpdate = false) 
        {
            Name = name;
            FieldType = fieldType;
            EditorType = editorType;
            Options = options;
            AutoUpdate = autoUpdate;
            MakeEditorConfig(EditorTypeConfig.RESULT_TYPE_STRING, ",", "", false, options.Split(','));
        }

        public MetaDataFieldType MakeEditorConfig(string resultType, string separator, string append, bool allowDuplicate = false, string[] autoCompleteItems = null)
        {
            EditorConfig = new EditorTypeConfig(EditorType);
            EditorConfig.ResultType = resultType;
            EditorConfig.Separator = separator;
            EditorConfig.Append = append;
            EditorConfig.AllowDuplicateValues = allowDuplicate;
            EditorConfig.AutoCompleteItems = autoCompleteItems != null ? autoCompleteItems : OptionsAsList().ToArray();

            return this;
        }


        public static MetaDataFieldType MakeComboBoxField(String name, String options, String editorType = EditorTypeConfig.EDITOR_TYPE_NONE)
        {
            MetaDataFieldType result = new MetaDataFieldType(name);
            result.EditorType = editorType;
            result.Options = options;
            result.FieldType = MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX;
            result.MakeEditorConfig(EditorTypeConfig.RESULT_TYPE_STRING, ",", "");

            return result;
        }


        public String[] OptionsAsList(char separator = ',')
        {
            return Options.Split(separator);
        }

        public MetaDataFieldType MakeOptionsFromStrings(string[] options, String separator = ",")
        {
            Options = String.Join(separator, options);

            return this;
        }

        public override String ToString()
        {
            return Name + "|" + FieldType + "|" + EditorType + "|" + Options + "|" + AutoUpdate.ToString();
        }
    }
}
