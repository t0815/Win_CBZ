using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Models
{
    public class EditorTypeConfig
    {
        public const string RESULT_TYPE_STRING = "string";
        public const string RESULT_TYPE_STRINGS = "string[]";

        public const string EDITOR_TYPE_NONE = "";
        public const string EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR = "MultiLineTextEditor";
        public const string EDITOR_TYPE_LANGUAGE_EDITOR = "LanguageEditor";
        public const string EDITOR_TYPE_TAG_EDITOR = "TagEditor";
        public const string EDITOR_TYPE_DATE_EDITOR = "DateEditor";
        public const string EDITOR_TYPE_GLOBAL_LOOKUP_EDITOR = "GlobalLookupEditor";
        public const string EDITOR_TYPE_ROMAJI_EDITOR = "RomajiEditor";
        public const string EDITOR_TYPE_VARIABLE_EDITOR = "VariableEditor";

        public static string[] Editors =
        {
            EDITOR_TYPE_NONE,
            EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR,
            EDITOR_TYPE_LANGUAGE_EDITOR,
            EDITOR_TYPE_TAG_EDITOR,
            // EditorTypeConfig.EDITOR_TYPE_VARIABLE_EDITOR,  // todo: implement v1.1
        };

        public string[] AutoCompleteItems { get; set; }

        public string Type { get; set; }

        public string Separator { get; set; }

        public string Value { get; set; }

        public object DefaultValue { get; set; }

        public string Append { get; set; }

        public object Result { get; set; }

        public string ResultType { get; set; }

        public bool AllowDuplicateValues { get; set; }

        public string AutoCompleteImageKey { get; set; }

        public string[] ComposeValuFrom { get; set; }


        public EditorTypeConfig(string type, string resultType, string separator, string append, bool allowDuplicate, string[] autoCompleteItems)
        {
            ResultType = resultType;
            Type = type;
            Separator = separator;
            Append = append;
            AllowDuplicateValues = allowDuplicate;
            AutoCompleteItems = autoCompleteItems;
        }

        public EditorTypeConfig(string type, string resultType, string separator, string append, bool allowDuplicate)
        {
            ResultType = resultType;
            Type = type;
            Separator = separator;
            Append = append;
            AllowDuplicateValues = allowDuplicate;
            AutoCompleteItems = new string[0];
        }

        public EditorTypeConfig(string type, string resultType)
        {
            ResultType = resultType;
            Type = type;
            Separator = ",";
            Append = " ";
            AutoCompleteItems = new string[0];
        }

        public EditorTypeConfig(string type)
        {
            Type = type;
            Separator = ",";
            Append = " ";
            ResultType = RESULT_TYPE_STRING;
            AutoCompleteItems = new string[0];
        }

        public override string ToString()
        {
            string name = Type;

            switch (Type)
            {
                case EDITOR_TYPE_NONE:
                    name = "";
                    break;
                case EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR:
                    name = "Multi Line Text Editor";
                    break;
                case EDITOR_TYPE_LANGUAGE_EDITOR:
                    name = "Language Editor";
                    break;
                case EDITOR_TYPE_TAG_EDITOR:
                    name = "Tag Editor";
                    break;
                case EDITOR_TYPE_DATE_EDITOR:
                    name = "Date Editor";
                    break;
                case EDITOR_TYPE_GLOBAL_LOOKUP_EDITOR:
                    name = "Global Lookup Editor";
                    break;
                case EDITOR_TYPE_ROMAJI_EDITOR:
                    name = "Romaji Editor";
                    break;
                case EDITOR_TYPE_VARIABLE_EDITOR:
                    name = "Insert...";
                    break;
            }

            return name;
        }
    }
}
