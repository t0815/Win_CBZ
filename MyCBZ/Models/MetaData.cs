using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Win_CBZ
{
    internal class MetaData
    {

        protected static readonly string[] ProtectedProperties = { "pages" };

        protected static readonly String[] DefaultProperties = { "AgeRating", "Title", 
            "Series", "SeriesGroup", "AlternateSeries", "Number", "Count", "Volume", "StoryArc", "StoryArcNumber", 
            "Manga", "Web", "Summary", "Publisher", "Imprint", "Genre", "Tags", "LanguageISO", "Format",
            "Artist", "Writer", "Penciller", "Inker", "Colorist", "Cover", "Translator", "Editor", "Letterer", 
            "Year", "Month", "Day", "Characters", "BlackAndWhite", "Review", "Rating", "CommunityRating", 
            "Locations", "Notes", "PageCount", "GTIN" };

        protected static readonly string[] Ratings =
        {
            "Unknown",
            "Adults Only 18+",
            "Early Childhood",
            "Everyone",
            "Everyone 10+",
            "G",
            "Kids to Adults",
            "M",
            "MA15+",
            "Mature 17+",
            "PG",
            "R18+",
            "Rating Pending",
            "Teen",
            "X18+"
        };

        protected static readonly string[] Manga = 
        { 
            "Unknown",
            "Yes", 
            "YesAndRightToLeft", 
            "No",            
        };

        protected static readonly string[] BlackAndWhite =
        {
            "Unknown",
            "Yes",
            "No",           
        };


        protected static readonly Dictionary<String, string[]> CustomEditorValueMappings = new Dictionary<String, string[]>()
        {
            { "Manga", Manga },
            { "AgeRating", Ratings },
            { "ParentalRating", Ratings },
            { "BlackAndWhite", BlackAndWhite }
        };


        protected static readonly Dictionary<String, string[]> ValidationRules = new Dictionary<String, string[]>()
        {
            { "Manga", Manga },
            { "AgeRating", Ratings },
            { "ParentalRating", Ratings },
            { "BlackAndWhite", BlackAndWhite }
        };



        public List<String> CustomDefaultProperties { get; set; }

        private List<String> RemovedKeys { get; set; }

        public List<String> ProtectedKeys { get; }

        public String MetaDataFileName { get; set; }

        public XmlNode Root { get; set; }

        private XmlDocument Document { get; set; }

        private XmlReader MetaDataReader { get; set; }

        private XmlWriter MetaDataWriter { get; set; }

        private List<MetaDataEntry> Defaults { get; set; }

        public BindingList<MetaDataEntry> Values { get; set; }

        public BindingList<MetaDataEntryPage> PageIndex { get; set; }


        private readonly Stream InputStream;


        public event EventHandler<MetaDataEntryChangedEvent> MetaDataEntryChanged;


        public MetaData(bool createDefault = false, String name = "ComicInfo.xml")
        {
            Defaults = new List<MetaDataEntry>();
            Values = new BindingList<MetaDataEntry>();
            PageIndex = new BindingList<MetaDataEntryPage>();
            ProtectedKeys = new List<string>(ProtectedProperties);
            RemovedKeys = new List<string>();

            Document = new XmlDocument();

            MakeDefaultKeys();

            MetaDataFileName = name;

            if (createDefault)
            {
                foreach (var entry in Defaults)
                {
                    Values.Add(HandleNewEntry(entry.Key, entry.Value));
                }
            }
        }

        public MetaData(Stream fileInputStream, String name)
        {
            InputStream = fileInputStream;

            Defaults = new List<MetaDataEntry>();
            Values = new BindingList<MetaDataEntry>();
            PageIndex = new BindingList<MetaDataEntryPage>();
            ProtectedKeys = new List<string>(ProtectedProperties);
            RemovedKeys = new List<string>();

            MakeDefaultKeys();

            Document = new XmlDocument();
            MetaDataReader = XmlReader.Create(InputStream);
            MetaDataReader.Read();
            Document.Load(MetaDataReader);

            foreach (XmlNode node in Document.ChildNodes)
            {
                if (node.Name.ToLower().Equals("comicinfo"))
                {
                    Root = node;
                    foreach (XmlNode subNode in node.ChildNodes)
                    {
                        switch (subNode.Name.ToLower())
                        {
                            case "pages":
                                HandlePageMetaData(subNode);
                                break;
                            default:
                                Values.Add(HandleNewEntry(subNode.Name, subNode.InnerText));
                                break;
                        }   
                    }
                }     
            }

            MetaDataFileName = name;

            FillMissingDefaultProps();
        }

        protected MetaDataEntry HandleNewEntry(String key, String value = null, bool readOnly = false)
        {
            if (ProtectedKeys.IndexOf(key.ToLower()) != -1)
            {
                throw new MetaDataValidationException(new MetaDataEntry(key, value, readOnly), "Metadata Value Error! Value with key ['" + key + "'] is not allowed!", true, true);
            }

            if (CustomEditorValueMappings.ContainsKey(key))
            {
                CustomEditorValueMappings.TryGetValue(key, out var mapping);

                int index = mapping != null ? Array.IndexOf(mapping, value) : -1;

                value = value != null && index > -1 ? value : (mapping[0] ?? "???");

                return new MetaDataEntry(key, value, mapping, readOnly);
            }

            return new MetaDataEntry(key, value, readOnly);
        }

        public void Save(String path)
        {
            try
            {
                MemoryStream ms = BuildComicInfoXMLStream();
                using (FileStream fs = File.Create(path, 4096, FileOptions.RandomAccess))
                {
                    ms.CopyTo(fs);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw new MetaDataException(this, "Error saving MataData [" + path + "] to disk! [" + ex.Message + "]", true);
            }
        }


        public MemoryStream BuildComicInfoXMLStream(bool withoutXMLHeaderTag = false)
        {
            MemoryStream ms = new MemoryStream();
            XmlWriterSettings writerSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = withoutXMLHeaderTag
            };
            XmlWriter xmlWriter = XmlWriter.Create(ms, writerSettings);
             
            xmlWriter.WriteStartDocument();
            
            xmlWriter.WriteStartElement("ComicInfo");
            foreach (MetaDataEntry entry in Values)
            {
                    xmlWriter.WriteElementString(entry.Key, entry.Value);
            }
            xmlWriter.WriteStartElement("Pages");
            foreach (MetaDataEntryPage page in PageIndex)
            {
                xmlWriter.WriteStartElement("Page");
                foreach (KeyValuePair<String, String> attibute in page.Attributes)
                {
                    xmlWriter.WriteAttributeString(attibute.Key, attibute.Value);
                }
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            //if (!withoutXMLHeaderTag)
            //{
                xmlWriter.WriteEndDocument();
            //}

            xmlWriter.Close();

            ms.Position = 0;

            return ms;
        }

        /**
         * 
         */
        public void RebuildPageMetaData(List<Page> pages)
        {
            List<MetaDataEntryPage> originalPageMetaData = PageIndex.ToList<MetaDataEntryPage>();
            
            PageIndex.Clear();
            foreach (Page page in pages)
            {
                try
                {
                    if (!page.Deleted)
                    {
                        MetaDataEntryPage newPageEntry = new MetaDataEntryPage();
                        newPageEntry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE, page.Name)
                            .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_TYPE, page.ImageType)
                            .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_SIZE, page.Size.ToString())
                            .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY, page.Key)
                            .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_DOUBLE_PAGE, page.DoublePage.ToString());

                        if (page.Format.W > 0 && page.Format.H > 0)
                        {
                            newPageEntry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_WIDTH, page.Format.W.ToString())
                                .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_HEIGHT, page.Format.H.ToString());
                        }

                        PageIndex.Add(newPageEntry);
                    }
                }
                catch (Exception ex)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error rebuilding <pages> metadata for pagee->" + page.Name + "! [" + ex.Message + "]");
                    //throw new MetaDataPageEntryException(newPageEntry, "Error rebuilding <pages> metadata for page->" + page.Name + "! [" + ex.Message + "]");
                }
            }
        }

        public MetaDataEntryPage UpdatePageIndexMetaDataEntry(Page page, String key)
        {
            foreach (MetaDataEntryPage entry in PageIndex)
            {
                if (!entry.Attributes.ContainsKey(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY))
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error getting page metadata entry for key [" + key + "]! [Attribute 'key' not found!]");

                    throw new MetaDataPageEntryException(entry, "Error getting page metadata entry for key [" + key + "]! [Attribute 'key' not found!]", true);
                }

                try
                {
                    if (entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY).Equals(key))
                    {
                        entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE, page.Name)
                             .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_TYPE, page.ImageType)
                             .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_SIZE, page.Size.ToString())
                             .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY, page.Key)
                             .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_DOUBLE_PAGE, page.DoublePage.ToString());

                        if (page.Format.W > 0 && page.Format.H > 0)
                        {
                            entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_WIDTH, page.Format.W.ToString())
                                 .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_HEIGHT, page.Format.H.ToString());
                        }

                        return entry;
                    }
                } catch (Exception ex)
                {
                    throw new MetaDataPageEntryException(entry, "Error updating <pages> metadata for page with key " + key + "! [" + ex.Message + "]");
                }
                
            }

            return null;
        }

        public void UpdatePageIndexMetaDataEntry(Page page, String oldName, String newName)
        {
            foreach (MetaDataEntryPage entry in PageIndex)
            {
                if (entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE).Equals(oldName))
                {
                    entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE, page.Name)
                         .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_TYPE, page.ImageType)
                         .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_SIZE, page.Size.ToString())
                         .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY, page.Key)
                         .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_DOUBLE_PAGE, page.DoublePage.ToString());

                    if (page.Format.W > 0 && page.Format.H > 0)
                    {
                        entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_WIDTH, page.Format.W.ToString())
                             .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_HEIGHT, page.Format.H.ToString());
                    }

                    break;
                }
            }
        }

        public MetaDataEntryPage FindIndexEntryForPage(Page page)
        {
            foreach (MetaDataEntryPage entry in PageIndex)
            {
                if (entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE).Equals(page.Name))
                {
                    return entry;
                }
            }

            return null;
        }

        public MetaDataEntryPage FindIndexEntryForPageByKey(Page page)
        {
            foreach (MetaDataEntryPage entry in PageIndex)
            {
                if (entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY).Equals(page.Key))
                {
                    return entry;
                }
            }

            return null;
        }

        public bool Exists() 
        { 
            return Values.Count > 0; 
        }

        public bool HasValues()
        {
            bool defaultValueChanged = true;
            bool keyChanged = false;
            foreach (MetaDataEntry entry in Values)
            {
                defaultValueChanged = false;
                keyChanged = true;
                if (entry.Value != null && entry.Value != "")
                {
                    foreach (var defaultEntry in Defaults)
                    {
                        if (defaultEntry.Key.ToLower().Equals(entry.Key.ToLower()))
                        {
                            keyChanged = false;
                            defaultValueChanged = !defaultEntry.Value.Equals(entry.Value);
                            break;
                        }
                    }

                    if (defaultValueChanged || keyChanged)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool HasRemovedValues()
        {
            return RemovedKeys.Count > 0;
        }

        public String ValueForKey(String key)
        {
            foreach (MetaDataEntry entry in Values)
            {
                if (entry.Key.ToLower().Equals(key.ToLower()))
                {
                    return entry.Value;
                }
            }

            return "";
        }

        public MetaDataEntry EntryByKey(String key)
        {
            foreach (MetaDataEntry entry in Values)
            {
                if (entry.Key.ToLower().Equals(key.ToLower()))
                {
                    return entry;
                }
            }

            return null;
        }

        public int Add(MetaDataEntry entry)
        {
            Values.Add(HandleNewEntry(entry.Key, entry.Value));

            if (RemovedKeys.IndexOf(entry.Key) > -1)
            {
                RemovedKeys.Remove(entry.Key);
            }

            return Values.Count - 1;
        }

        public int Add(String key, String value = null)
        {
            MetaDataEntry newEntry = HandleNewEntry(key, value);

            Values.Add(newEntry);

            if (RemovedKeys.IndexOf(key) > -1)
            {
                RemovedKeys.Remove(key);
            }

            OnMetaDataEntryChanged(new MetaDataEntryChangedEvent(MetaDataEntryChangedEvent.ENTRY_NEW, Values.Count - 1, newEntry));

            return Values.Count - 1;
        }

        public int Remove(String key)
        {
            MetaDataEntry entry = EntryByKey(key);
            
            if (entry != null)
            {
                int index = Values.IndexOf(entry);
                bool success = Values.Remove(entry);

                if (success)
                {
                    if (RemovedKeys.IndexOf(key) == -1)
                    {
                        RemovedKeys.Add(key);
                    }

                    OnMetaDataEntryChanged(new MetaDataEntryChangedEvent(MetaDataEntryChangedEvent.ENTRY_DELETED, index, entry));

                    return index;
                }
            }

            return -1;
        }

        public int Remove(int index)
        {
            MetaDataEntry entry = EntryByIndex(index);

            if (entry != null)
            {
                Values.RemoveAt(index);

                if (RemovedKeys.IndexOf(entry.Key) == -1)
                {
                    RemovedKeys.Add(entry.Key);
                }

                OnMetaDataEntryChanged(new MetaDataEntryChangedEvent(MetaDataEntryChangedEvent.ENTRY_DELETED, index, entry));

                return index;             
            }

            return -1;
        }

        public int Remove(MetaDataEntry entry)
        {
            int index = Values.IndexOf(entry);

            bool success = Values.Remove(entry);

            if (success)
            {
                if (RemovedKeys.IndexOf(entry.Key) == -1)
                {
                    RemovedKeys.Add(entry.Key);
                }

                OnMetaDataEntryChanged(new MetaDataEntryChangedEvent(MetaDataEntryChangedEvent.ENTRY_DELETED, index, entry));

                return index;
            }

            return -1;
        }

        public MetaDataEntry EntryByIndex(int index)
        {
            try
            {
                return Values[index];
            } catch
            {
                return null;
            }
        }

        public MetaDataEntry UpdateEntry(int index, MetaDataEntry entry)
        {
            MetaDataEntry existing = EntryByIndex(index);
            if (existing != null)
            {
                existing.Key = entry.Key;
                if (ProtectedKeys.IndexOf(entry.Key.ToLower()) != -1)
                {
                    throw new MetaDataValidationException(entry, "Metadata Value Error! Value with key ['" + entry.Key + "'] is not allowed!", true, true);
                }
                
                existing.Value = entry.Value;

                if (CustomEditorValueMappings.ContainsKey(entry.Key))
                {
                    CustomEditorValueMappings.TryGetValue(entry.Key, out var mapping);

                    existing.Options = mapping;
                    if (entry.Value == null || entry.Value == "")
                    {
                        existing.Value = mapping[0] ?? "???";
                    }
                } else
                {
                    if (existing.Options.Length > 0)
                    {
                        existing.Options = new string[] { };
                    }
                }

                return existing;
            }

            return entry;
        }

        protected void HandlePageMetaData(XmlNode pageNodes)
        {
            MetaDataEntryPage pageMeta;

            if (pageNodes != null)
            {
                foreach (XmlNode subNode in pageNodes.ChildNodes)
                {
                    if (subNode.Attributes.Count > 0)
                    {
                        pageMeta = new MetaDataEntryPage();

                        foreach (XmlAttribute attrib in subNode.Attributes)
                        {
                            try
                            {
                                pageMeta.SetAttribute(attrib.Name, attrib.Value);
                            } catch (Exception) {
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to read value from index for Pages->" + attrib.Name + " from ComicInfo.xml");
                            }
                        }

                        PageIndex.Add(pageMeta);
                    }
                }
            }
        }

        public void MakeDefaultKeys(List<string> custom = null)
        {
            Defaults.Clear();
            if (custom == null) {

                if (Win_CBZSettings.Default.CustomDefaultProperties != null) {
                    foreach (String prop in Win_CBZSettings.Default.CustomDefaultProperties)
                    {
                        try
                        {
                            MetaDataEntry defaultEntry = ParseDefaultProp(prop);

                            Defaults.Add(HandleNewEntry(defaultEntry.Key, defaultEntry.Value));
                        }
                        catch (MetaDataValidationException ve)
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to parse default metadata entry ['" + prop + "']!  [" + ve.Message + "]");

                            if (ve.ShowErrorDialog)
                            {
                                throw ve;
                            }
                        }
                        catch (Exception e)
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to parse default metadata entry ['" + prop + "']! [" + e.Message + "]");
                        }
                    }
                }
            } else
            {
                foreach (String prop in custom)
                {
                    try
                    {
                        MetaDataEntry defaultEntry = ParseDefaultProp(prop);

                        Defaults.Add(HandleNewEntry(defaultEntry.Key, defaultEntry.Value));
                    }
                    catch (MetaDataValidationException ve)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to parse default metadata entry ['" + prop + "']!  [" + ve.Message + "]");
                    
                        if (ve.ShowErrorDialog)
                        {
                            throw ve;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to parse default metadata entry ['" + prop + "']! With error  [" + e.Message + "]");
                    }
                }
            }

            if (Defaults.Count == 0)
            {
                foreach (String prop in DefaultProperties)
                {
                    Defaults.Add(new MetaDataEntry(prop, ""));
                }               
            }

            Defaults = Defaults.Distinct<MetaDataEntry>().ToList();
        }

        public String GetDefaultKeys()
        {
            var result = new StringBuilder();

            foreach (String prop in DefaultProperties) {
                result.AppendLine(prop);
            }

            return result.ToString();
        }

        public MetaDataEntry ParseDefaultProp(String prop)
        {
            MetaDataEntry entry = new MetaDataEntry(prop, "");

            if (prop.Split('=').Length == 2)
            {
                entry = new MetaDataEntry(prop.Split('=')[0].Trim(), prop.Split('=')[1].Trim());
            }

            return entry;
        }

        public int FillMissingDefaultProps()
        {
            int i = 0;
            bool valueExists;

            foreach (MetaDataEntry entry in Defaults)
            {
                valueExists = false;
                foreach (MetaDataEntry v in Values)
                {
                    if (v.Key.ToLower().Equals(entry.Key.ToLower()))
                    {
                        valueExists = true;
                        break;
                    }
                }

                if (!valueExists)
                {
                    Values.Add(HandleNewEntry(entry.Key, entry.Value));
                    i++;
                }

            }

            return i;
        }

        public void Validate(MetaDataEntry entry, String newKey)
        {
            int occurence = 0;

            if (ProtectedKeys.IndexOf(newKey.ToLower()) != -1)
            {
                throw new MetaDataValidationException(entry, "Metadata Value Error! Value with key ['" + newKey + "'] is not allowed!", true, true);
            }

            foreach (MetaDataEntry entryA in Values)
            {
                if (entryA.Key.ToLower().Equals(newKey.ToLower()))
                {
                    occurence++;
                }
            }

            if (occurence > 1)
            {
                throw new MetaDataValidationException(entry, "Duplicate keys ['" + newKey + "'] not allowed!");
            }


        }

        public void ValidateDefaults()
        {
            int occurence;
            foreach (MetaDataEntry entryA in Defaults)
            {
                occurence = 0;
                foreach (MetaDataEntry entryB in Defaults)
                {
                    if (entryA.Key.ToLower().Equals(entryB.Key.ToLower()))
                    {
                        occurence++;
                    }
                }

                if (occurence > 1)
                {
                    throw new MetaDataValidationException(entryA, "Duplicate keys ['" + entryA.Key + "'] not allowed!");
                }              
            }  
        }

        public void Free()
        {
            if (InputStream != null) { 
                if (InputStream.CanRead) {
                    InputStream.Close();
                    InputStream.Dispose();
                }
            }

            if (Document.HasChildNodes)
            {
                Document.RemoveAll();
            }

            MetaDataReader?.Dispose();

            RemovedKeys.Clear();
            Defaults.Clear();
            Values.Clear();
            PageIndex.Clear();
        }

        protected virtual void OnMetaDataEntryChanged(MetaDataEntryChangedEvent e)
        {
            MetaDataEntryChanged?.Invoke(this, e);
        }
    }
}
