using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Win_CBZ.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Win_CBZ.Helper
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
            //if (MetaDataFieldConfig.Instance == null)
            //{
                MetaDataFieldConfig.Instance ??= new MetaDataFieldConfig();
            //}

            //MetaDataVersionFlavorHandler.Instance.HandlePageIndexVersion();

            return MetaDataFieldConfig.Instance;
        }


        public MetaDataFieldType GetFieldConfigFor(String name)
        {
            if (FieldTypes != null)
            {
                foreach (MetaDataFieldType fieldType in FieldTypes)
                {
                    if (fieldType.Name == name)
                    {
                        return fieldType;
                    }
                }
            }
            
            return new MetaDataFieldType(name, MetaDataFieldType.METADATA_FIELD_TYPE_TEXT_BOX);
        }

        public List<MetaDataFieldType> GetAllTypes()
        {
            return FieldTypes.ToList<MetaDataFieldType>();
        }

        public void UpdateItem(String name, MetaDataFieldType field) 
        {
            MetaDataFieldType existing = GetFieldConfigFor(name);

            if (existing != null)
            {
                existing.AutoUpdate = field.AutoUpdate;
                existing.Options = field.Options;
                existing.EditorType = field.EditorType;
                existing.FieldType = field.FieldType;
                existing.Name = field.Name;
            }
        }

        public void UpdateAutoCompleteOptions(String name, String option)
        {
            List<String> optionList = new List<String>();
            List<String> validationTest = new List<String>();
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
                        } else
                        {
                            validationTest.Add(option);
                            validationTest.AddRange(existing.OptionsAsList().Where((s, index) => s.Length > 0).ToArray());

                            var duplicates = Validation.ValidateDuplicateStrings(validationTest.ToArray());

                            if (duplicates.Length == 0)
                            {
                                optionList.Add(option);
                            }

                        }

                        existing.Options = String.Join(",", optionList);
                        existing.EditorConfig.AutoCompleteItems = optionList.ToArray();
                    }
                }
            }
        }

        public void UpdateAutoCompleteOptions(String name, String[] options)
        {
            List<String> optionList = new List<String>();
            List<String> validationTest = new List<String>();
            List<String> duplicatesList = new List<String>();
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
                                } else
                                {
                                    if (item.Length > 0)
                                    {
                                        optionList.Add(item);
                                    }                                   
                                }                                
                            }
                        }

                        existing.Options = String.Join(",", optionList);
                        existing.EditorConfig.AutoCompleteItems = optionList.ToArray();
                    }
                }
            }
        }


        public string[] PrepareForConfig()
        {
            List<String> result = new List<String>(); 

            foreach (MetaDataFieldType fieldType in FieldTypes)
            {
                result.Add(fieldType.ToString());
            }

            return result.ToArray();
        }

        public void UpdateFrom(string[] settingsList)
        {
            FieldTypes.Clear();

            foreach (String line in settingsList)
            {
                String[] typeParts = line.Split('|');

                if (typeParts.Length == 5)
                {
                    try
                    {
                        FieldTypes.Add(new MetaDataFieldType(
                            typeParts[0],
                            typeParts[1],
                            typeParts[2],
                            typeParts[3],
                            bool.Parse(typeParts[4].ToLower())
                        ));
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }
    }
}
