using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;

namespace Win_CBZ.Helper
{
    internal class MetaDataFieldConfig
    {
        protected List<MetaDataFieldType> FieldTypes;

        protected static MetaDataFieldConfig Instance;

        private MetaDataFieldConfig() 
        {
            FieldTypes = new List<MetaDataFieldType>();
        }


        public static MetaDataFieldConfig GetInstance()
        {
            if (MetaDataFieldConfig.Instance == null)
            {
                MetaDataFieldConfig.Instance = new MetaDataFieldConfig();
            }

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
            return FieldTypes;
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

        public void AddUpdateItemOption(String name, String option)
        {
            MetaDataFieldType existing = GetFieldConfigFor(name);

            if (existing != null)
            {
                
                //existing.

            }
        }


        public List<String> PrepareForConfig()
        {
            List<String> result = new List<String>(); 

            foreach (MetaDataFieldType fieldType in FieldTypes)
            {
                result.Add(fieldType.ToString());
            }

            return result;
        }

        public void ParseFromConfig(String[] customFieldTypesCollection)
        {
            foreach (String line in customFieldTypesCollection)
            {
                String[] typeParts = line.Split('|');

                if (typeParts.Length == 5)
                {
                    FieldTypes.Add(new MetaDataFieldType(typeParts[0], typeParts[1], typeParts[2], typeParts[3], Boolean.Parse(typeParts[4])));
                }
            }
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
                        FieldTypes.Add(new MetaDataFieldType()
                        {
                            Name = typeParts[0],
                            FieldType = typeParts[1],
                            EditorType = typeParts[2],
                            Options = typeParts[3],
                            AutoUpdate = bool.Parse(typeParts[3]),
                        });
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }
    }
}
