using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Models
{
    internal class MetaDataFieldType
    {

        public const string METADATA_FIELD_TYPE_TEXT_BOX = "TextBox";
        public const string METADATA_FIELD_TYPE_COMBO_BOX = "ComboBox";
        public const string METADATA_FIELD_TYPE_AUTO_COMPLETE = "AutoComplete";
        public const string METADATA_FIELD_TYPE_RATING = "Rating";
        public const string METADATA_FIELD_TYPE_CHECK_BOX = "CheckBox";


        public static string[] FieldTypes =
        {
            METADATA_FIELD_TYPE_TEXT_BOX,
            METADATA_FIELD_TYPE_COMBO_BOX,
            METADATA_FIELD_TYPE_AUTO_COMPLETE,
            /* MetaDataFieldType.METADATA_FIELD_TYPE_RATING, */
        };


        public string Name { get; set; }

        public string FieldType { get; set; }

        public string EditorType { get; set; }

        public string Options { get; set; }

        public bool AutoUpdate { get; set; }

        public string AutoCompleteImageKey { get; set; }

        public string MultiValueSeparator { get; set; } = ",";

        public bool MultiValued { get; set; }

        public EditorTypeConfig EditorConfig { get; set; } = new EditorTypeConfig(EditorTypeConfig.EDITOR_TYPE_NONE);


        public MetaDataFieldType()
        {
            FieldType = METADATA_FIELD_TYPE_TEXT_BOX;
            EditorType = EditorTypeConfig.EDITOR_TYPE_NONE;
            AutoUpdate = false;
            MultiValued = false;
        }


        public MetaDataFieldType(string name)
        {
            Name = name;
            FieldType = METADATA_FIELD_TYPE_TEXT_BOX;
            EditorType = EditorTypeConfig.EDITOR_TYPE_NONE;
            Options = "";
            AutoUpdate = false;
            MultiValued = false;
        }

        public MetaDataFieldType(string name, string type)
        {
            Name = name;
            FieldType = type;
            EditorType = EditorTypeConfig.EDITOR_TYPE_NONE;
            Options = "";
            AutoUpdate = false;
            MultiValued = false;
        }

        public MetaDataFieldType(string name, 
            string fieldType, 
            string editorType, 
            string options, 
            bool autoUpdate = false, 
            string autoCompleteImageKey = null,
            bool multiValued = false,
            string multiValueSeparator = ",")
        {
            Name = name;
            FieldType = fieldType;
            EditorType = editorType;
            Options = options;
            AutoUpdate = autoUpdate;
            AutoCompleteImageKey = autoCompleteImageKey;
            MultiValueSeparator = multiValueSeparator;
            MultiValued = multiValued;
            MakeEditorConfig(EditorTypeConfig.RESULT_TYPE_STRING, MultiValueSeparator, "", false, options.Split(MultiValueSeparator), autoCompleteImageKey);
        }

        public MetaDataFieldType MakeEditorConfig(string resultType, string separator, string append, bool allowDuplicate = false, string[] autoCompleteItems = null, string autoCompleteImageIndex = null)
        {
            EditorConfig = new EditorTypeConfig(EditorType);
            EditorConfig.ResultType = resultType;
            EditorConfig.Separator = separator;
            EditorConfig.Append = append;
            EditorConfig.AllowDuplicateValues = allowDuplicate;
            EditorConfig.AutoCompleteItems = autoCompleteItems != null ? autoCompleteItems : OptionsAsList().ToArray();
            EditorConfig.AutoCompleteImageKey = autoCompleteImageIndex;

            return this;
        }


        public static MetaDataFieldType MakeComboBoxField(string name, string options, string editorType = EditorTypeConfig.EDITOR_TYPE_NONE)
        {
            MetaDataFieldType result = new MetaDataFieldType(name);
            result.EditorType = editorType;
            result.Options = options;
            result.FieldType = METADATA_FIELD_TYPE_COMBO_BOX;
            result.MultiValued = false;
            result.MakeEditorConfig(EditorTypeConfig.RESULT_TYPE_STRING, ",", "");

            return result;
        }


        public string[] OptionsAsList(char separator = ',')
        {
            return Options.Split(separator).Select((s) => s.TrimEnd(' ').TrimStart(' ')).ToArray();
        }

        public MetaDataFieldType MakeOptionsFromStrings(string[] options, string separator = ",")
        {
            Options = string.Join(separator, options);

            return this;
        }

        public override string ToString()
        {
            return Name + "|" + FieldType + "|" + EditorType + "|" + Options + "|" + AutoUpdate.ToString() + "|" + AutoCompleteImageKey.ToString() + "|" + MultiValued.ToString() + "|" + MultiValueSeparator;
        }
    }
}
