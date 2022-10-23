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
using System.Xml;
using System.Xml.Linq;

namespace MyCBZ
{
    internal class CBZMetaData
    {
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
                                switch (attrib.Name.ToLower())
                                {
                                    case "image":
                                        pageMeta.Image = int.Parse(attrib.Value);
                                        break;

                                    case "type":
                                        pageMeta.ImageType = attrib.Value;
                                        break;

                                    case "doublepage":
                                        pageMeta.DoublePage = Boolean.Parse(attrib.Value);
                                        break;

                                    case "imagesize":
                                        pageMeta.ImageSize = long.Parse(attrib.Value);
                                        break;

                                    case "key":
                                        pageMeta.Key = attrib.Value;
                                        break;

                                    case "imagewidth":
                                        pageMeta.ImageWidth = int.Parse(attrib.Value);
                                        break;

                                    case "imagewheight":
                                        pageMeta.ImageHeight = int.Parse(attrib.Value);
                                        break;
                                }
                            } catch (Exception) { }
                        }

                        PageMetaData.Add(pageMeta);

                    }
                }
            }

        }

        protected void MakeDefaultKeys()
        {
            Defaults.Add(new CBZMetaDataEntry("Series", ""));
            Defaults.Add(new CBZMetaDataEntry("Number", ""));
            Defaults.Add(new CBZMetaDataEntry("Web", ""));
            Defaults.Add(new CBZMetaDataEntry("Summary", ""));
            Defaults.Add(new CBZMetaDataEntry("Notes", ""));
            Defaults.Add(new CBZMetaDataEntry("Publisher", ""));
            Defaults.Add(new CBZMetaDataEntry("Imprint", ""));
            Defaults.Add(new CBZMetaDataEntry("Genre", ""));
            Defaults.Add(new CBZMetaDataEntry("PageCount", "", true));
            Defaults.Add(new CBZMetaDataEntry("LanguageISO", ""));
            Defaults.Add(new CBZMetaDataEntry("Artist", ""));
            Defaults.Add(new CBZMetaDataEntry("Title", ""));
            Defaults.Add(new CBZMetaDataEntry("Year", ""));
            Defaults.Add(new CBZMetaDataEntry("Month", ""));
            Defaults.Add(new CBZMetaDataEntry("Day", ""));
            Defaults.Add(new CBZMetaDataEntry("AgeRating", ""));
            Defaults.Add(new CBZMetaDataEntry("Characters", ""));
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
