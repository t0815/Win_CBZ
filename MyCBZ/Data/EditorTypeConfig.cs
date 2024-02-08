using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    public class EditorTypeConfig
    {
        public const String RESULT_TYPE_STRING = "string";
        public const String RESULT_TYPE_STRINGS = "string[]";

        public const String EDITOR_TYPE_NONE = "";
        public const String EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR = "MultiLineTextEditor";
        public const String EDITOR_TYPE_LANGUAGE_EDITOR = "LanguageEditor";
        public const String EDITOR_TYPE_TAG_EDITOR = "TagEditor";

        public static string[] Editors = 
        {
            EditorTypeConfig.EDITOR_TYPE_NONE,
            EditorTypeConfig.EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR,
            EditorTypeConfig.EDITOR_TYPE_LANGUAGE_EDITOR,
            EditorTypeConfig.EDITOR_TYPE_TAG_EDITOR,
        };

        public string[] AutoCompleteItems { get; set; }

        public string Type { get; set; }

        public string Separator { get; set; }

        public String Value { get; set; }

        public object DefaultValue { get; set; }

        public string Append { get; set; }

        public object Result { get; set; }

        public string ResultType { get; set; }

        public bool AllowDuplicateValues { get; set; }


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
            AutoCompleteItems = new String[0];
        }

        public EditorTypeConfig(string type, string resultType)
        {
            ResultType = resultType;
            Type = type;
            Separator = ",";
            Append = " ";
            AutoCompleteItems = new String[0];
        }

        public EditorTypeConfig(string type)
        {
            Type = type;
            Separator = ",";
            Append = " ";
            ResultType = EditorTypeConfig.RESULT_TYPE_STRING;
            AutoCompleteItems = new String[0];
        }
    }
}
