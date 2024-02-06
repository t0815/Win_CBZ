using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Win_CBZ.Data;

namespace Win_CBZ.Helper
{
    internal class FactoryDefaults
    {
        public static readonly String[] DefaultMetaDataFieldTypes =
        {
            "AgeRating|ComboBox||Unknown,Adults Only 18+,Early Childhood,Everyone,Everyone 10+,G,Kids to Adults,M,MA15+,Mature 17+,PG,R18+,Rating Pending,Teen,X18+",
            "Manga|ComboBox||Unknown,Yes,YesAndLeftToRight,No",
            "BlackAndWhite|ComboBox||Unknown,Yes,No",
            "LanguageISO|Text|LanguageEditor|",
            "Tags|AutoComplete|TagEditor|",
            "Writer|AutoComplete||"
        };

        public const String DefaultMetaDataFileName = "ComicInfo.xml";

        public const int DefaultMetaDataFileIndexVersion = 1;

        public static readonly Dictionary<int, String[]> ValuesToReset = new Dictionary<int, String[]>()
        {
            { 1, new string[] { "DefaultMetaDataFieldTypes.4.$.1", "DefaultMetaDataFieldTypes.4.$.2", "DefaultMetaDataFieldTypes.5.+" } }
        };

        public static int HandleSettingsValueUpdates(int lastVersion)
        {
            int updatedVersion = 0;
            String[] values = null;
            int index = -1;
            int subIndex = -1;
            string key;
            bool update = false;

            foreach (int v in ValuesToReset.Keys)
            { 
                if (v > lastVersion)
                {
                    bool ok = ValuesToReset.TryGetValue(v, out values);

                    if (ok)
                    {
                        foreach (String value in values)
                        {
                            if (value.Contains("."))
                            {
                                String[] values2 = value.Split('.');
                                index = int.Parse(values2[1]);
                                key = values2[0];
                                update = values2[2] == "$";
                                if (values2.Length == 4)
                                {
                                    subIndex = int.Parse(values2[3]);
                                }
                            } else
                            {
                                key = value;
                                update = false;
                            }

                            switch (key)
                            {
                                case "DefaultMetaDataFieldTypes":
                                    if (index > -1)
                                    {
                                        if (update)
                                        {
                                            if (Win_CBZSettings.Default.CustomMetadataFields.Count > index)
                                            {
                                                if (subIndex > -1)
                                                {
                                                    string[] factParts = FactoryDefaults.DefaultMetaDataFieldTypes[index].Split('|');
                                                    string[] parts = Win_CBZSettings.Default.CustomMetadataFields[index].Split('|');
                                                    string[] result = new string[parts.Length];
                                                    for (int i = 0; i < parts.Length; i++)
                                                    {
                                                        if (i != subIndex)
                                                        {
                                                            result[i] = parts[i];
                                                        } else
                                                        {
                                                            result[i] = factParts[i];
                                                        }
                                                    }
                                                    Win_CBZSettings.Default.CustomMetadataFields[index] = String.Join("|", result);
                                                } else
                                                {
                                                    Win_CBZSettings.Default.CustomMetadataFields[index] = FactoryDefaults.DefaultMetaDataFieldTypes[index];
                                                }
                                                
                                            }
                                            else
                                            {
                                                Win_CBZSettings.Default.CustomMetadataFields.Add(FactoryDefaults.DefaultMetaDataFieldTypes[index]);
                                            }
                                        } else
                                        {
                                            Win_CBZSettings.Default.CustomMetadataFields.Add(FactoryDefaults.DefaultMetaDataFieldTypes[index]);
                                        }
                                        
                                    } else
                                    {
                                        Win_CBZSettings.Default.CustomMetadataFields.Clear();
                                        Win_CBZSettings.Default.CustomMetadataFields.AddRange(FactoryDefaults.DefaultMetaDataFieldTypes);
                                    }
                                    
                                    break;
                            }
                            update = false;
                        }
                    }
                    updatedVersion = v;
                }
                
            }

            return updatedVersion;
        }
    }
}
