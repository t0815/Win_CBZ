using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Win_CBZ.Data;
using System.Runtime.Versioning;
using Win_CBZ.Exceptions;

namespace Win_CBZ.Helper
{
    [SupportedOSPlatform("windows")]
    internal class FactoryDefaults
    {
        public static readonly String[] DefaultMetaDataFieldTypes =
        {
            "AgeRating|ComboBox||Unknown,Unknown,Rating Pending,Early Childhood,Everyone,Everyone 10+,G,PG,Kids to Adults,Teen,M,MA15+,Mature 17+,Adults Only 18+,R18+,X18+|False",
            "Manga|ComboBox||Unknown,Yes,YesAndLeftToRight,No|False",
            "BlackAndWhite|ComboBox||Unknown,Yes,No|False",
            "LanguageISO|Text|LanguageEditor||False",
            "Tags|AutoComplete|TagEditor||True|tag",
            "Writer|AutoComplete|||True|user",
            "Characters|Text|MultiLineTextEditor||False|users"
        };

        public const String DefaultMetaDataFileName = "ComicInfo.xml";

        public const int DefaultMetaDataFileIndexVersion = 1;

        public static readonly String[] DefaultKeys =
        {
            "AgeRating", 
            "Title",
            "Series", 
            "SeriesGroup", 
            "AlternateSeries", 
            "Number", 
            "Count", 
            "Volume", 
            "StoryArc", 
            "StoryArcNumber",
            "Manga",
            "ReadingDirection",
            "Format",
            "Web", 
            "Summary", 
            "Publisher", 
            "Imprint", 
            "Genre", 
            "Tags", 
            "LanguageISO", 
            "Format",
            "Artist", 
            "Writer", 
            "Penciller", 
            "Inker", 
            "Colorist", 
            "Cover", 
            "Translator", 
            "Editor", 
            "Letterer",
            "Year", 
            "Month", 
            "Day", 
            "Characters",
            "MainCharacterOrTeam",
            "Teams",
            "BlackAndWhite", 
            "Review", 
            "Rating", 
            "CommunityRating",
            "Locations", 
            "Notes",
            "ScanInformation",
            "PageCount", 
            "GTIN",           
        };

        public static readonly Dictionary<int, String[]> ValuesToReset = new Dictionary<int, String[]>()
        {
            { 1, new string[] { "DefaultMetaDataFileIndexVersion", "DefaultMetaDataFieldTypes.4.$.0=Tags.1", "DefaultMetaDataFieldTypes.4.$.0=Tags.2", "DefaultMetaDataFieldTypes.5.+" } },
            { 2, new string[] { "DefaultMetaDataFieldTypes.0.$.*.4", "DefaultMetaDataFieldTypes.1.$.*.4", "DefaultMetaDataFieldTypes.2.$.*.4", "DefaultMetaDataFieldTypes.3.$.*.4", "DefaultMetaDataFieldTypes.4.$.*.4", "DefaultMetaDataFieldTypes.0.$.*.5" } },
            { 3, new string[] { "DefaultMetaDataFieldTypes.6.+" } },
            { 4, new string[] { "DefaultMetaDataFieldTypes.4.$.*.5", "DefaultMetaDataFieldTypes.5.$.*.5", "DefaultMetaDataFieldTypes.6.$.*.5", "DefaultKeys.6.+", "DefaultKeys.11.+", "DefaultKeys.12.+", "DefaultKeys.20.+", "DefaultKeys.34.+", "DefaultKeys.35.+", "DefaultKeys.42.+", "Messages.0" } }
        };

        public static readonly string[] Messages = new string[]
        {
            "Updated default Metadata-Keys. Added 'ReadingDirection, Format, MainCharacterOrTeam, Teams, ScanInformation'.",
        };


        public static int GetLastPatchVersion()
        {
            return ValuesToReset.Keys.Last();
        }


        /// <summary>
        /// Patch users config/settings with specific new values
        /// </summary>
        /// <param name="lastVersion">Users current config version</param>
        /// <returns>updated version number</returns>
        public static int PatchUserSettings(int lastVersion)
        {
            int updatedVersion = 0;
            String[] values = null;
            int index = -1;
            int subIndex = -1;
            string match = "";
            string key;
            bool update = false;

            foreach (int v in ValuesToReset.Keys)
            { 
                if (v > lastVersion)
                {
                    bool ok = ValuesToReset.TryGetValue(v, out values);

                    if (ok)
                    {
                        try
                        {
                            foreach (String value in values)
                            {
                                if (value.Contains("."))
                                {
                                    String[] values2 = value.Split('.');
                                    try
                                    {
                                        index = int.Parse(values2[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        index = -1;
                                    }

                                    key = values2[0];
                                    if (values2.Length > 2)
                                    {
                                        update = values2[2] == "$";
                                        if (values2.Length == 5)
                                        {
                                            subIndex = int.Parse(values2[4]);
                                            match = values2[3];
                                        }
                                    }
                                    else
                                    {
                                        update = false;
                                    }
                                }
                                else
                                {
                                    key = value;
                                    update = false;
                                }

                                switch (key)
                                {
                                    case "Messages":
                                        if (index > -1)
                                        {
                                            ApplicationMessage.Show(Messages[index], "Updated User Settings", ApplicationMessage.DialogType.MT_INFORMATION);
                                        }
                                        break;
                                    case "DefaultKeys":
                                        if (index > -1)
                                        {
                                            if (update)
                                            {
                                                if (Win_CBZSettings.Default.CustomDefaultProperties.Count > index)
                                                {
                                                    Win_CBZSettings.Default.CustomDefaultProperties[index] = FactoryDefaults.DefaultKeys[index];
                                                }
                                                else
                                                {
                                                    Win_CBZSettings.Default.CustomDefaultProperties.Add(FactoryDefaults.DefaultKeys[index]);
                                                }
                                            }
                                            else
                                            {
                                                bool found = false;
                                                foreach (String keyName in Win_CBZSettings.Default.CustomDefaultProperties)
                                                {
                                                    if (keyName.ToLower().Contains(FactoryDefaults.DefaultKeys[index].ToLower()))
                                                    {
                                                        found = true;
                                                        break;
                                                    }
                                                }

                                                if (!found)
                                                {
                                                    try
                                                    {
                                                        Win_CBZSettings.Default.CustomDefaultProperties.Insert(index, FactoryDefaults.DefaultKeys[index]);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Win_CBZSettings.Default.CustomDefaultProperties.Add(FactoryDefaults.DefaultKeys[index]);
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            Win_CBZSettings.Default.CustomDefaultProperties.Clear();
                                            Win_CBZSettings.Default.CustomDefaultProperties.AddRange(FactoryDefaults.DefaultKeys);
                                        }

                                        break;
                                    case "DefaultMetaDataFieldTypes":
                                        if (index > -1)
                                        {
                                            if (update)
                                            {
                                                if (Win_CBZSettings.Default.CustomMetadataFields.Count > index)
                                                {
                                                    if (subIndex > -1)
                                                    {
                                                        string[] matching = match.Split('=');
                                                        string[] factParts = FactoryDefaults.DefaultMetaDataFieldTypes[index].Split('|');
                                                        string[] parts = Win_CBZSettings.Default.CustomMetadataFields[index].Split('|');
                                                        string[] result = new string[Math.Max(parts.Length, factParts.Length)];
                                                        for (int i = 0; i < Math.Max(parts.Length, factParts.Length); i++)
                                                        {
                                                            if (i != subIndex)
                                                            {
                                                                result[i] = parts[i];
                                                            }
                                                            else
                                                            {
                                                                if (matching.Length == 2)
                                                                {
                                                                    if (parts[int.Parse(matching[0])] == factParts[int.Parse(matching[0])])
                                                                    {
                                                                        result[i] = factParts[i];
                                                                    }
                                                                }
                                                                else if (matching.Length == 1)
                                                                {
                                                                    if (matching[0] == "*")
                                                                    {
                                                                        if (i == subIndex)
                                                                        {
                                                                            result[i] = factParts[i];
                                                                        }
                                                                        else
                                                                        {
                                                                            if (subIndex < i)
                                                                            {
                                                                                result[i] = "";
                                                                            }
                                                                            else
                                                                            {
                                                                                result[i] = parts[i];
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    result[i] = factParts[i];
                                                                }
                                                            }
                                                        }
                                                        Win_CBZSettings.Default.CustomMetadataFields[index] = String.Join("|", result);
                                                    }
                                                    else
                                                    {
                                                        Win_CBZSettings.Default.CustomMetadataFields[index] = FactoryDefaults.DefaultMetaDataFieldTypes[index];
                                                    }

                                                }
                                                else
                                                {
                                                    Win_CBZSettings.Default.CustomMetadataFields.Add(FactoryDefaults.DefaultMetaDataFieldTypes[index]);
                                                }
                                            }
                                            else
                                            {
                                                Win_CBZSettings.Default.CustomMetadataFields.Add(FactoryDefaults.DefaultMetaDataFieldTypes[index]);
                                            }

                                        }
                                        else
                                        {
                                            Win_CBZSettings.Default.CustomMetadataFields.Clear();
                                            Win_CBZSettings.Default.CustomMetadataFields.AddRange(FactoryDefaults.DefaultMetaDataFieldTypes);
                                        }

                                        break;
                                    case "DefaultMetaDataFileIndexVersion":
                                        Win_CBZSettings.Default.MetaDataPageIndexVersionToWrite = FactoryDefaults.DefaultMetaDataFileIndexVersion;
                                        break;
                                }

                                update = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new SettingsPatchException(ex.Message, index, true, ex);
                        }
                    }
                    updatedVersion = v;
                }
                
            }

            return updatedVersion;
        }
    }
}
