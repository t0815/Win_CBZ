using Microsoft.IdentityModel.Protocols.WsTrust;
using SharpCompress;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using Win_CBZ.Data;
using Win_CBZ.Events;
using Win_CBZ.Extensions;
using Win_CBZ.Handler;
using Win_CBZ.Helper;
using Win_CBZ.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Win_CBZ.Handler.AppEventHandler;

namespace Win_CBZ
{
    [SupportedOSPlatform("windows")]
    internal class MetaData
    {
        public enum PageIndexVersion : ushort
        {
            VERSION_1 = 1,
            VERSION_2 = 2,
        };

        protected static readonly string[] ProtectedProperties = { "pages" };

        protected static readonly String[] DefaultProperties = { "AgeRating", "Title", 
            "Series", "SeriesGroup", "AlternateSeries", "Number", "Count", "Volume", "StoryArc", "StoryArcNumber", 
            "Manga", "Web", "Summary", "Publisher", "Imprint", "Genre", "Tags", "LanguageISO", "Format",
            "Artist", "Writer", "Penciller", "Inker", "Colorist", "Cover", "Translator", "Editor", "Letterer", 
            "Year", "Month", "Day", "Characters", "MainCharacterOrTeam", "Teams", "BlackAndWhite", "Review", 
            "Rating", "CommunityRating", "Locations", "Notes", "ScanInformation", "PageCount", "GTIN" };

        /*
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
        */

        //protected static readonly Dictionary<String, MetaDataFieldType> MetaDataFieldTypeConfiguration = new Dictionary<String, MetaDataFieldType>();
        /*
         * Load from config
        {
            { "Manga", new EditorFieldMapping { EditorType = "ComboBox", EditorOptions = Manga } },
            { "AgeRating", new EditorFieldMapping { EditorType = "ComboBox", EditorOptions = Ratings } },
            { "BlackAndWhite", new EditorFieldMapping { EditorType = "ComboBox", EditorOptions = BlackAndWhite } },           
            { "ParentalRating", new EditorFieldMapping { EditorType = "ComboBox", EditorOptions = Ratings } },
        };
        

        /*
        protected static readonly Dictionary<String, string[]> ValidationRules = new Dictionary<String, string[]>()
        {
            { "Manga", Manga },
            { "AgeRating", Ratings },
            { "ParentalRating", Ratings },
            { "BlackAndWhite", BlackAndWhite }
        };
        */


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

        public List<String> DefaultSortOrderKeys { get; set; }

        public BindingList<MetaDataEntryPage> PageIndex { get; set; }

        public PageIndexVersion IndexVersionSpecification { get; set; } = PageIndexVersion.VERSION_1;


        private readonly Stream InputStream;

        public MetaDataEntryChangedDelegate MetaDataEntryChanged { get; set; }


        public MetaData(bool createDefault = false, String name = "ComicInfo.xml")
        {
            Defaults = new List<MetaDataEntry>();
            Values = new BindingList<MetaDataEntry>();
            PageIndex = new BindingList<MetaDataEntryPage>();
            ProtectedKeys = new List<string>(ProtectedProperties);
            RemovedKeys = new List<string>();
            DefaultSortOrderKeys = new List<string>();

            Document = new XmlDocument();

            MakeDefaultKeys();
            //UpdateCustomEditorMappings();

            MetaDataFileName = name;

            if (createDefault)
            {
                foreach (var entry in Defaults)
                {
                    DefaultSortOrderKeys.Add(entry.Key);
                    Values.Add(HandleNewEntry(entry.Key, entry.Value, entry.ReadOnly, entry.Uid));
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
            DefaultSortOrderKeys = new List<string>();

            MakeDefaultKeys();
            //UpdateCustomEditorMappings();

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
                                if (!DefaultSortOrderKeys.Contains(subNode.Name))
                                { 
                                    DefaultSortOrderKeys.Add(subNode.Name); 
                                }
                                Values.Add(HandleNewEntry(subNode.Name, subNode.InnerText));
                                break;
                        }   
                    }
                }     
            }

            MetaDataFileName = name;

            FillMissingDefaultProps();

            ReorderProps();
        }

        protected MetaDataEntry HandleNewEntry(String key, String value = null, bool readOnly = false, string uid = null)
        {
            if (ProtectedKeys.IndexOf(key.ToLower()) != -1)
            {
                throw new MetaDataValidationException(new MetaDataEntry(key, value, null, readOnly), null, "Metadata Value Error! Value with key ['" + key + "'] is not allowed!", true, true);
            }

            if (MetaDataFieldConfig.GetInstance().GetFieldConfigFor(key).Name.ToLower() == key.ToLower())
            {
                // todo: make case insensitive
                var mapping = MetaDataFieldConfig.GetInstance().GetFieldConfigFor(key);

                if (mapping.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX)
                {
                    int index = mapping != null ? Array.IndexOf(mapping.OptionsAsList().Select(s => s.ToLower()).ToArray(), value.ToLower()) : -1;

                    value = value != null && index > -1 ? value : (mapping.OptionsAsList()[0] ?? "???");

                    return new MetaDataEntry(key, value, mapping, readOnly, uid);
                } else
                {
                    return new MetaDataEntry(key, value, mapping, readOnly, uid);
                }
            }

            return new MetaDataEntry(key, value, new MetaDataFieldType(), readOnly, uid);
        }

        protected MetaDataEntry HandleNewEntry(MetaDataEntry copyFrom)
        {
            MetaDataEntry newEntry = HandleNewEntry(copyFrom.Key, copyFrom.Value, copyFrom.ReadOnly, copyFrom.Uid);

            newEntry.Visible = copyFrom.Visible;
            newEntry.UserFiltered = copyFrom.UserFiltered;
            newEntry.FilterMatchSourceCol = copyFrom.FilterMatchSourceCol;
            newEntry.Type = copyFrom.Type;

            return newEntry;
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

        public MemoryStream BuildComicInfoXMLStream(bool withoutXMLHeaderTag = false, bool writePageIndex = true)
        {
            MemoryStream ms = new MemoryStream();
            XmlWriterSettings writerSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = withoutXMLHeaderTag,
                Encoding = Encoding.UTF8,
            };
            XmlWriter xmlWriter = XmlWriter.Create(ms, writerSettings);
             
            xmlWriter.WriteStartDocument();
            
            xmlWriter.WriteStartElement("ComicInfo");
            foreach (MetaDataEntry entry in Values)
            {
                if (Win_CBZSettings.Default.OmitEmptyXMLTags)
                {
                    if (entry.Value != null && entry.Value.Length > 0)
                    {
                        xmlWriter.WriteElementString(entry.Key, entry.Value);
                    }
                }
                else
                {
                    xmlWriter.WriteElementString(entry.Key, entry.Value);
                }
            }

            if (writePageIndex)
            {
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
            } else
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "No Page-Index written, because it has been disabled in settings. [WriteXMLPageIndex = False]");
            }

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
        public void RebuildPageMetaData(List<Page> pages, PageIndexVersion indexVersion = PageIndexVersion.VERSION_1)
        {
            List<MetaDataEntryPage> originalPageMetaData = PageIndex.ToList<MetaDataEntryPage>();
            
            if (pages == null) 
            {
                AppEventHandler.OnMessageLogged(this, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Unable to rebuild Index! [MetaData::RebuildPageMetaData(), parameter PAGES was NULL] "));

                return; 
            }

            PageIndex.Clear();

            foreach (Page page in pages)
            {
                try
                {
                    if (!page.Deleted)
                    {
                        MetaDataEntryPage newPageEntry = MetaDataVersionFlavorHandler.GetInstance().CreateIndexEntry(page);

                        newPageEntry
                            .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_TYPE, page.ImageType)
                            .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_SIZE, page.Size.ToString())
                            .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_DOUBLE_PAGE, page.DoublePage.ToString());

                        if (page.Format.W > 0 && page.Format.H > 0)
                        {
                            newPageEntry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_WIDTH, page.Format.W.ToString())
                                .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_HEIGHT, page.Format.H.ToString());
                        }

                        if (page.Bookmark.Length > 0)
                        {
                            newPageEntry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_BOOKMARK, page.Bookmark);

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

            IndexVersionSpecification = indexVersion;
        }

        public MetaDataEntryPage UpdatePageIndexMetaDataEntry(Page page, String key)
        {
            foreach (MetaDataEntryPage entry in PageIndex)
            {
                if (!entry.Attributes.ContainsKey(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY))
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, new StringBuilder("Error getting page metadata entry for key [").Append(key).Append("]! [Attribute 'key' not found!]").ToString());

                    throw new MetaDataPageEntryException(entry, new StringBuilder("Error getting page metadata entry for key [").Append(key).Append("]! [Attribute 'key' not found!]").ToString());
                }

                try
                {
                    bool condition = false;
                    if (IndexVersionSpecification.HasFlag(PageIndexVersion.VERSION_1))
                    {
                        condition = entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY).Equals(key);                       
                    } else if (IndexVersionSpecification.HasFlag(PageIndexVersion.VERSION_2))
                    {
                        condition = entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE).Equals(key);
                    }

                    if (condition)
                    {
                        if (IndexVersionSpecification.HasFlag(PageIndexVersion.VERSION_2))
                        {
                            entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE, page.Name)
                                .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY, page.Key);
                        }
                        else if (IndexVersionSpecification.HasFlag(PageIndexVersion.VERSION_1))
                        {
                            entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE, page.Number.ToString())
                                .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY, page.Name);
                        }

                        entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_TYPE, page.ImageType)
                             .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_SIZE, page.Size.ToString())
                             .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_DOUBLE_PAGE, page.DoublePage.ToString());

                        if (page.Format.W > 0 && page.Format.H > 0)
                        {
                            entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_WIDTH, page.Format.W.ToString())
                                 .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_HEIGHT, page.Format.H.ToString());
                        }

                        if (page.Bookmark != null && page.Bookmark.Length > 0)
                        {
                            entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_BOOKMARK, page.Bookmark);
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

        public MetaData UpdatePageIndexMetaDataEntry(Page page, String oldName, String newName)
        {
            foreach (MetaDataEntryPage entry in PageIndex)
            {
                bool condition = false;
                if (IndexVersionSpecification.HasFlag(PageIndexVersion.VERSION_1))
                {
                    condition = entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE).Equals(oldName);
                }
                else if (IndexVersionSpecification.HasFlag(PageIndexVersion.VERSION_2))
                {
                    condition = entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY).Equals(oldName);
                }

                if (condition)
                {
                    if (IndexVersionSpecification.HasFlag(PageIndexVersion.VERSION_2))
                    {
                        entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE, page.Name)
                            .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY, page.Key);
                    }
                    else if (IndexVersionSpecification.HasFlag(PageIndexVersion.VERSION_1))
                    {
                        entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE, page.Number.ToString())
                            .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY, page.Name);
                    }

                    entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_TYPE, page.ImageType)
                         .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_SIZE, page.Size.ToString())
                         .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY, page.Key)
                         .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_DOUBLE_PAGE, page.DoublePage.ToString());

                    if (page.Format.W > 0 && page.Format.H > 0)
                    {
                        entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_WIDTH, page.Format.W.ToString())
                             .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_HEIGHT, page.Format.H.ToString());
                    }

                    if (page.Bookmark.Length > 0)
                    {
                        entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_BOOKMARK, page.Bookmark);

                    }

                    break;
                }
            }

            return this;
        }

        public void SortByKey(SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                Values = new BindingList<MetaDataEntry>(Values.OrderBy((entry) => entry.Key).ToList());
            }
            else
            {
                Values = new BindingList<MetaDataEntry>(Values.OrderByDescending((entry) => entry.Key).ToList());
            }
        }

        public void SortByValue(SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                Values = new BindingList<MetaDataEntry>(Values.OrderBy((entry) => entry.Value).ToList());
            }
            else
            {
                Values = new BindingList<MetaDataEntry>(Values.OrderByDescending((entry) => entry.Value).ToList());
            }
        }

        public void RemoveSort()
        {
            Values = new BindingList<MetaDataEntry>(Values.Select((entry) => { entry.Index = DefaultSortOrderKeys.IndexOf(entry.Key); return entry; })
                .OrderBy((entry) => entry.Index)
                .ToList());
        }

        public MetaData FilterMetaData(String search)
        {
            foreach (MetaDataEntry entry in Values)
            {
                if (search.Length > 0)
                {
                    bool keyMatch = false;
                    bool valueMatch = false;

                    if (entry.Value != null)
                    {
                        valueMatch = entry.Value.ToLower().Contains(search.ToLower());
                        entry.FilterMatchSourceCol[1] = valueMatch;
                    } else
                    {
                        entry.FilterMatchSourceCol[1] = false;
                    }
                        
                    if (entry.Key != null)
                    {
                        keyMatch = entry.Key.ToLower().Contains(search.ToLower());
                        entry.FilterMatchSourceCol[0] = keyMatch || entry.UserFiltered;                        
                    } else
                    {
                        entry.FilterMatchSourceCol[0] = entry.UserFiltered;
                    }

                    entry.Visible = keyMatch || valueMatch;
                } else
                {
                    entry.FilterMatchSourceCol[0] = entry.UserFiltered;
                    entry.FilterMatchSourceCol[1] = false;

                    entry.Visible = true;
                }               
            }

            return this;
        }

        public MetaData UserFilterMetaData(String[] keys = null, int condition = 0)
        {
            if (keys != null && keys.Length > 0)
            {
                if (condition == 0)
                {
                    Values.Each(entry =>
                      {
                          entry.UserFiltered = !keys.Contains(entry.Key);
                          entry.FilterMatchSourceCol[0] = entry.FilterMatchSourceCol[0] || keys.Contains(entry.Key);

                      });
                }
                else if (condition == 1)
                {
                    Values.Each(entry => {
                        entry.UserFiltered = keys.Contains(entry.Key);
                        entry.FilterMatchSourceCol[0] = entry.FilterMatchSourceCol[0] || !keys.Contains(entry.Key);
                    });
                }
            } else
            {
                Values.Each(entry => {
                    entry.UserFiltered = false;
                    entry.FilterMatchSourceCol[0] = !entry.Visible;
                });
            }

            return this;
        }

        public bool IsColumnFiltered(int col)
        {
            if (col < 2)
            {

                int countFilteredForCol = Values.Where(entry => entry.FilterMatchSourceCol[col]).Count();

                return countFilteredForCol > 0 || CountVisible() == 0;
            }

            return false;
        }

        public MetaData ClearUserFilterMetaData()
        {
            Values.Each(entry => entry.UserFiltered = false);

            return this;
        }

        public MetaDataEntry[] GetVisibleEntries()
        {
            return Values.Where(entry => entry.Visible && !entry.UserFiltered).ToArray();
        }

        public int CountVisible()
        {
            return Values.Where(entry => !entry.UserFiltered && entry.Visible).Count();
        }

        public int CountUserFiltered()
        {
            return Values.Where(entry => entry.UserFiltered).Count();
        }

        public bool HasUserFiltered()
        {
            return CountVisible() != Values.Count;
        }

        public MetaDataEntryPage FindIndexEntryForPage(Page page, PageIndexVersion indexFormatVersion = PageIndexVersion.VERSION_1)
        {
            foreach (MetaDataEntryPage entry in PageIndex)
            {
                bool condition = false;

                try
                {
                    if (indexFormatVersion.HasFlag(PageIndexVersion.VERSION_2))
                    {
                        if (entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE) != null)
                        {
                            condition = entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE).Equals(page.Name);
                        }
                    }
                    else if (indexFormatVersion.HasFlag(PageIndexVersion.VERSION_1))
                    {
                        if (entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY) != null)
                        {
                            condition = entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY).Equals(page.Name);
                        }
                    }
                } catch (Exception e)
                {
                    return null;
                }

                if (condition)
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

        public MetaDataEntry EntryById(String id)
        {
            MetaDataEntry result = Values.Where((e) => e.Uid == id).FirstOrDefault();

            return result;
        }

        public int Add(MetaDataEntry entry)
        {
            Values.Add(HandleNewEntry(entry));
            if (!DefaultSortOrderKeys.Contains(entry.Key))
            {
                DefaultSortOrderKeys.Add(entry.Key);
            }
            
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
            if (!DefaultSortOrderKeys.Contains(newEntry.Key))
            {
                DefaultSortOrderKeys.Add(newEntry.Key);
            }

            if (RemovedKeys.IndexOf(key) > -1)
            {
                RemovedKeys.Remove(key);
            }

            OnMetaDataEntryChanged(new MetaDataEntryChangedEvent(MetaDataEntryChangedEvent.ENTRY_NEW, Values.Count - 1, newEntry));

            return Values.Count - 1;
        }

        public int Remove(String uid)
        {
            MetaDataEntry entry = EntryById(uid);
            
            if (entry != null)
            {
                int index = Values.IndexOf(entry);
                bool success = Values.Remove(entry);
                DefaultSortOrderKeys.Remove(entry.Key);

                if (success)
                {
                    if (RemovedKeys.IndexOf(entry.Key) == -1)
                    {
                        RemovedKeys.Add(entry.Key);
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
                DefaultSortOrderKeys.RemoveAt(index);

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
                DefaultSortOrderKeys.Remove(entry.Key);
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
                    throw new MetaDataValidationException(entry, null, "Metadata Value Error! Value with key ['" + entry.Key + "'] is not allowed!", true, true);
                }
                
                existing.Value = entry.Value;

                if (MetaDataFieldConfig.GetInstance().GetFieldConfigFor(entry.Key).Name.ToLower() == entry.Key.ToLower())
                {
                    var mapping = MetaDataFieldConfig.GetInstance().GetFieldConfigFor(entry.Key);

                    existing.Type = mapping;
                    if (entry.Value == null || entry.Value == "")
                    {
                        if (existing.Type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX)
                        {
                            existing.Value = mapping.OptionsAsList()[0] ?? "???";
                        }
                    }
                } else
                {
                    if (existing.Type.EditorConfig != null)
                    {
                        if (existing.Type.OptionsAsList().Length > 0)
                        {
                            existing.Type.Options = "";
                        }
                    } else
                    {
                        existing.Type.Options = "";
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
            List<String> itemsToMerge = new List<String>();
            string[] customDefaults = null;

            Defaults.Clear();
            if (custom == null) {
                if (Win_CBZSettings.Default.CustomDefaultProperties != null)
                {
                    customDefaults = new string[Win_CBZSettings.Default.CustomDefaultProperties.Count];
                    Win_CBZSettings.Default.CustomDefaultProperties.CopyTo(customDefaults, 0);
                }
            } else
            {
                customDefaults = new string[custom.Count];
                custom.CopyTo(customDefaults, 0);
            }


            if (customDefaults != null)
            { 
                foreach (String prop in customDefaults)
                {                 
                    MetaDataEntry defaultEntry = ParseDefaultProp(prop);

                    Defaults.Add(HandleNewEntry(defaultEntry.Key, defaultEntry.Value));
                       
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

            return result.ToString().TrimEnd('\r').TrimEnd('\n');
        }

        public List<MetaDataEntry> GetDefaultEntries()
        {
            return Defaults;
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

        public void ReorderProps()
        {
            List<MetaDataEntry> sortedProps = new List<MetaDataEntry>();

            bool valueAdded;

            foreach (MetaDataEntry entry in Defaults)
            {
                valueAdded = false;
                foreach (MetaDataEntry v in Values)
                {
                    if (v.Key.ToLower().Equals(entry.Key.ToLower()))
                    {
                        sortedProps.Add(v);
                        valueAdded = true;
                        break;
                    }
                }

                if (!valueAdded)
                {
                    sortedProps.Add(HandleNewEntry(entry.Key, entry.Value));
                   
                }

            }

            Values.Clear();

            sortedProps.ForEach(v => Values.Add(v));
        }

        public void Validate(MetaDataEntry entry, String newKey = null, String newVal = null, bool silent = true)
        {
            
            DataValidation validation = new DataValidation();

            if (ProtectedKeys.IndexOf(newKey?.ToLower()) != -1)
            {
                throw new MetaDataValidationException(entry, null, "Metadata Value Error! Key with name ['" + newKey + "'] is not allowed!", !silent, true);
            }

            //Regex re = Regex.("/[a-z]+$/gi");
       
            if (newKey != null && !Regex.IsMatch(newKey, @"^[a-z]+$", RegexOptions.IgnoreCase))
            {
                throw new MetaDataValidationException(entry, null, "Metadata Value Error! Key with name ['" + newKey + "'] must contain only values between ['a-zA-Z']!", !silent, true);
            }
            
            string[] duplicates = validation.ValidateDuplicateStrings(Values.ToArray().Select(s => s.Key).ToArray());
            string[] checkValues = Values.ToArray().Select(s => s.Key).ToArray();

            if (newKey != entry.Key && duplicates.Length == 0)
            {
                checkValues = checkValues.Append(newKey).ToArray();
            }

            if (validation.ValidateItemOccurence(newKey, checkValues) > 1)
            {
                throw new MetaDataValidationException(entry, null, "Duplicate keys ['" + newKey + "'] not allowed!", !silent, true);
            }

            if (duplicates.Length > 0 && Array.IndexOf(duplicates, newKey) > -1)
            {
                throw new MetaDataValidationException(entry, null, "Duplicate keys ['" + String.Join(",", duplicates) + "'] not allowed!", !silent, true);
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
                    throw new MetaDataValidationException(entryA, null, "Duplicate keys ['" + entryA.Key + "'] not allowed!");
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
            DefaultSortOrderKeys.Clear();
        }

        protected virtual void OnMetaDataEntryChanged(MetaDataEntryChangedEvent e)
        {
            MetaDataEntryChanged?.Invoke(this, e);
        }
    }
}
