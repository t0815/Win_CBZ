using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Events;

namespace Win_CBZ.Models
{
    [SupportedOSPlatform("windows")]
    internal class MetaDataFieldConfig
    {
        protected List<MetaDataFieldType> FieldTypes;

        protected static MetaDataFieldConfig Instance;

        private readonly DataValidation Validation;

        private MetaDataFieldConfig()
        {
            FieldTypes = new List<MetaDataFieldType>();
            Validation = new DataValidation();
        }


        public static MetaDataFieldConfig GetInstance()
        {
            
            Instance ??= new MetaDataFieldConfig();
            
            //MetaDataVersionFlavorHandler.Instance.HandlePageIndexVersion();

            return Instance;
        }


        public MetaDataFieldType GetFieldConfigFor(string name)
        {
            if (FieldTypes != null)
            {
                foreach (MetaDataFieldType fieldType in FieldTypes)
                {
                    if (fieldType.Name.ToLower() == name.ToLower())
                    {
                        return fieldType;
                    }
                }
            }

            return new MetaDataFieldType(name, MetaDataFieldType.METADATA_FIELD_TYPE_TEXT_BOX);
        }

        public List<MetaDataFieldType> GetAllTypes()
        {
            return FieldTypes.ToList();
        }

        public void UpdateItem(string name, MetaDataFieldType field)
        {
            MetaDataFieldType existing = GetFieldConfigFor(name);

            if (existing != null)
            {
                existing.AutoUpdate = field.AutoUpdate;
                existing.Options = field.Options;
                existing.EditorType = field.EditorType;
                existing.FieldType = field.FieldType;
                existing.Name = field.Name;
                existing.AutoCompleteImageKey = field.AutoCompleteImageKey;
                existing.MultiValued = field.MultiValued;
                existing.MultiValueSeparator = field.MultiValueSeparator;
            }
        }

        public void UpdateAutoCompleteOptions(string name, string option)
        {
            List<string> optionList = new List<string>();
            List<string> validationTest = new List<string>();
            MetaDataFieldType existing = GetFieldConfigFor(name);

            if (existing != null)
            {
                if (existing.AutoUpdate)
                {
                    if (existing.Options != null && option != null && option.Length > 0)
                    {
                        optionList.AddRange(existing.OptionsAsList());

                        if (existing.EditorConfig.AllowDuplicateValues)
                        {
                            optionList.Add(option);
                        }
                        else
                        {
                            validationTest.Add(option);
                            validationTest.AddRange(existing.OptionsAsList().Where((s, index) => s.Length > 0).ToArray());

                            var duplicates = Validation.ValidateDuplicateStrings(validationTest.ToArray());

                            if (duplicates.Length == 0)
                            {
                                optionList.Add(option);
                            }

                        }

                        existing.Options = string.Join(",", optionList);
                        existing.EditorConfig.AutoCompleteItems = optionList.ToArray();
                    }
                }
            }
        }

        public void UpdateAutoCompleteOptions(string name, string[] options)
        {
            List<string> optionList = new List<string>();
            List<string> validationTest = new List<string>();
            List<string> duplicatesList = new List<string>();
            MetaDataFieldType existing = GetFieldConfigFor(name);

            if (existing != null)
            {
                if (existing.AutoUpdate)
                {
                    if (existing.Options != null && options != null && options.Length > 0)
                    {
                        optionList.AddRange(existing.OptionsAsList().Where((s, index) => s.Length > 0).ToArray());
                        if (existing.EditorConfig.AllowDuplicateValues)
                        {
                            optionList.AddRange(options.Where((s, index) => s.Length > 0).ToArray());
                        }
                        else
                        {
                            validationTest.AddRange(options.Where((s, index) => s.Length > 0).ToArray());
                            validationTest.AddRange(existing.OptionsAsList());

                            duplicatesList.AddRange(Validation.ValidateDuplicateStrings(validationTest.ToArray()));

                            foreach (string item in options)
                            {
                                if (duplicatesList.Count > 0)
                                {
                                    if (!duplicatesList.Contains(item))
                                    {
                                        optionList.Add(item);
                                    }
                                }
                                else
                                {
                                    if (item.Length > 0)
                                    {
                                        optionList.Add(item);
                                    }
                                }
                            }
                        }

                        existing.Options = string.Join(",", optionList);
                        existing.EditorConfig.AutoCompleteItems = optionList.ToArray();
                    }
                }
            }
        }


        public string[] PrepareForConfig()
        {
            List<string> result = new List<string>();

            foreach (MetaDataFieldType fieldType in FieldTypes)
            {
                result.Add(fieldType.ToString());
            }

            return result.ToArray();
        }

        public void UpdateFrom(string[] settingsList)
        {
            FieldTypes.Clear();

            foreach (string line in settingsList)
            {
                string[] typeParts = line.Split('|');

                if (typeParts.Length >= 5)
                {
                    try
                    {
                        FieldTypes.Add(new MetaDataFieldType(
                            typeParts[0],
                            typeParts[1],
                            typeParts[2],
                            typeParts[3],
                            bool.Parse(typeParts[4].ToLower()),
                            typeParts.Length >= 6 ? typeParts[5] : "",
                            typeParts.Length >= 7 ? bool.Parse(typeParts[6]) : false,
                            typeParts.Length >= 8 ? typeParts[7] : ","
                        ));
                    }
                    catch (Exception e)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Field-Type configuration Error! Unable to load Fieldtype for Field [" + typeParts[0] + "] !  [" + e.Message + "]");
                    }
                }
            }
        }
    }
}
