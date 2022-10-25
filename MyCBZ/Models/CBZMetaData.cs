using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MyCBZ
{
    internal class CBZMetaData
    {
        protected static readonly String[] DefaultProperties = { "Series", "Number", "Web",
        "Summary", "Notes", "Publisher", "Imprint", "Genre", "PageCount", "LanguageISO",
        "Artist", "Title", "Year", "Month", "Day", "AgeRating", "Characters"};

        public String MetaDataFileName { get; set; }

        public XmlNode Root { get; set; }

        private XmlDocument MetaData { get; set; }

        private XmlReader MetaDataReader { get; set; }

        private XmlWriter MetaDataWriter { get; set; }

        private List<CBZMetaDataEntry> Defaults { get; set; }

        public BindingList<CBZMetaDataEntry> Values { get; set; }

        public BindingList<CBZMetaDataEntryPage> PageMetaData { get; set; }


        private readonly Stream InputStream;

       
        public CBZMetaData(bool createDefault = false)
        {
            Defaults = new List<CBZMetaDataEntry>();
            Values = new BindingList<CBZMetaDataEntry>();
            PageMetaData = new BindingList<CBZMetaDataEntryPage>();
            MetaData = new XmlDocument();

            MakeDefaultKeys();

            if (createDefault)
            {
                foreach (var key in Defaults)
                {
                    Values.Add(key);
                }
            }
        }

        public CBZMetaData(Stream fileInputStream, String name)
        {
            InputStream = fileInputStream;

            Defaults = new List<CBZMetaDataEntry>();
            Values = new BindingList<CBZMetaDataEntry>();
            PageMetaData = new BindingList<CBZMetaDataEntryPage>();

            MakeDefaultKeys();

            MetaData = new XmlDocument();
            MetaDataReader = XmlReader.Create(InputStream);
            MetaDataReader.Read();
            MetaData.Load(MetaDataReader);

            foreach (XmlNode node in MetaData.ChildNodes)
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
                                Values.Add(new CBZMetaDataEntry(subNode.Name, subNode.InnerText));
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
            MemoryStream ms = BuildComicInfoXMLStream();
            using (FileStream fs = File.Create(path, 4096, FileOptions.WriteThrough))
            {
                ms.CopyTo(fs);
                fs.Close();
            }
        }


        public MemoryStream BuildComicInfoXMLStream()
        {
            MemoryStream ms = new MemoryStream();
            XmlWriter xmlWriter = XmlWriter.Create(ms);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("ComicInfo");
            foreach (CBZMetaDataEntry entry in Values)
            {
                xmlWriter.WriteElementString(entry.Key, entry.Value);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteStartElement("Pages");
            foreach (CBZMetaDataEntryPage page in PageMetaData)
            {
                xmlWriter.WriteStartElement("Page");
                foreach (KeyValuePair<String, String> attibute in page.Attributes)
                {
                    xmlWriter.WriteAttributeString(attibute.Key, attibute.Value);
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            return ms;
        }


        protected void HandlePageMetaData(XmlNode pageNodes)
        {
            CBZMetaDataEntryPage pageMeta;

            if (pageNodes != null)
            {
                foreach (XmlNode subNode in pageNodes.ChildNodes)
                {
                    if (subNode.Attributes.Count > 0)
                    {
                        pageMeta = new CBZMetaDataEntryPage();

                        foreach (XmlAttribute attrib in subNode.Attributes)
                        {
                            try
                            {
                                pageMeta.SetAttribute(attrib.Name, attrib.Value);
                            } catch (Exception) {
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to read page->" + attrib.Name + " from ComicInfo.xml");
                            }
                        }

                        PageMetaData.Add(pageMeta);
                    }
                }
            }
        }

        protected void MakeDefaultKeys()
        {
            foreach (String prop in DefaultProperties)
            {
                Defaults.Add(new CBZMetaDataEntry(prop, ""));
            }
        }

        public int FillMissingDefaultProps()
        {
            int i = 0;
            bool valueExists;

            foreach (CBZMetaDataEntry entry in Defaults)
            {
                valueExists = false;
                foreach (CBZMetaDataEntry v in Values)
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

        public void Validate(CBZMetaDataEntry entry, String newKey)
        {
            int occurence = 0;
            foreach (CBZMetaDataEntry entryA in Values)
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

        public void Free()
        {
            if (InputStream != null) { 
                if (InputStream.CanRead) {
                    InputStream.Close();
                }
            }

            MetaData.RemoveAll();
            if (MetaDataReader != null)
            {
                MetaDataReader.Dispose();
            }
        }
    }
}
