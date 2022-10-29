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
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Win_CBZ
{
    internal class MetaData
    {
        protected static readonly String[] DefaultProperties = { "Series", "Number", "Web",
        "Summary", "Notes", "Publisher", "Imprint", "Genre", "PageCount", "LanguageISO",
        "Author", "Title", "Year", "Month", "Day", "AgeRating", "Characters"};

        public List<String> CustomDefaultProperties { get; set; }

        public String MetaDataFileName { get; set; }

        public XmlNode Root { get; set; }

        private XmlDocument Document { get; set; }

        private XmlReader MetaDataReader { get; set; }

        private XmlWriter MetaDataWriter { get; set; }

        private List<MetaDataEntry> Defaults { get; set; }

        public BindingList<MetaDataEntry> Values { get; set; }

        public BindingList<MetaDataEntryPage> PageIndex { get; set; }


        private readonly Stream InputStream;

       
        public MetaData(bool createDefault = false)
        {
            Defaults = new List<MetaDataEntry>();
            Values = new BindingList<MetaDataEntry>();
            PageIndex = new BindingList<MetaDataEntryPage>();
            Document = new XmlDocument();

            MakeDefaultKeys();

            if (createDefault)
            {
                foreach (var key in Defaults)
                {
                    Values.Add(key);
                }
            }
        }

        public MetaData(Stream fileInputStream, String name)
        {
            InputStream = fileInputStream;

            Defaults = new List<MetaDataEntry>();
            Values = new BindingList<MetaDataEntry>();
            PageIndex = new BindingList<MetaDataEntryPage>();

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
                                Values.Add(new MetaDataEntry(subNode.Name, subNode.InnerText));
                                break;
                        }   
                    }
                }     
            }

            MetaDataFileName = name;

            FillMissingDefaultProps();
        }

        public void Save(String path)
        {
            try
            {
                MemoryStream ms = BuildComicInfoXMLStream();
                using (FileStream fs = File.Create(path, 4096, FileOptions.WriteThrough))
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


        public MemoryStream BuildComicInfoXMLStream()
        {
            MemoryStream ms = new MemoryStream();
            XmlWriter xmlWriter = XmlWriter.Create(ms);

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
            xmlWriter.WriteEndDocument();
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

            MetaDataEntryPage newPageEntry = null;
            foreach (Page page in pages)
            {
                try
                {
                    newPageEntry = new MetaDataEntryPage();
                    newPageEntry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE, page.Name)
                        .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_TYPE, page.ImageType)
                        .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_SIZE, page.Size.ToString());

                    PageIndex.Add(newPageEntry);
                }
                catch (Exception ex)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error rebuilding <pages> metadata for pagee->" + page.Name + "! [" + ex.Message + "]");
                    //throw new MetaDataPageEntryException(newPageEntry, "Error rebuilding <pages> metadata for pagee->" + page.Name + "! [" + ex.Message + "]");
                }
            }
        }

        public void UpdatePageIndexMetaDataEntry(String name, Page page)
        {
            foreach (MetaDataEntryPage entry in PageIndex)
            {
                if (entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE).Equals(page.Name))
                {
                    entry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE, page.Name)
                        .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_TYPE, page.ImageType)
                        .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_SIZE, page.Size.ToString());

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

        public bool HasValues()
        {
            foreach (MetaDataEntry entry in Values)
            {
                if (entry.Value != null && entry.Value != "")
                {
                    return true;
                }
            }

            return false;
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
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to read page->" + attrib.Name + " from ComicInfo.xml");
                            }
                        }

                        PageIndex.Add(pageMeta);
                    }
                }
            }
        }

        public void MakeDefaultKeys()
        {
            Defaults.Clear();
            if (Win_CBZSettings.Default.CustomDefaultProperties != null) {
                foreach (String prop in Win_CBZSettings.Default.CustomDefaultProperties)
                {
                    Defaults.Add(new MetaDataEntry(prop, ""));
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
                    Values.Add(entry);
                    i++;
                }

            }

            return i;
        }

        public void Validate(MetaDataEntry entry, String newKey)
        {
            int occurence = 0;
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
            int occurence = 0;
            foreach (MetaDataEntry entryA in Defaults)
            {
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

                occurence = 0;
            }  
        }

        public void Free()
        {
            if (InputStream != null) { 
                if (InputStream.CanRead) {
                    InputStream.Close();
                }
            }

            Document.RemoveAll();
            if (MetaDataReader != null)
            {
                MetaDataReader.Dispose();
            }
        }
    }
}
