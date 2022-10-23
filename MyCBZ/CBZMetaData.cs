using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


        public ObservableCollection<CBZMetaDataEntry> Values { get; set; }

        public ObservableCollection<CBZMetaDataEntryPage> PageMetaData { get; set; }

        private readonly Stream InputStream;

       
        public CBZMetaData(bool createDefault = false)
        {
            Defaults = new List<CBZMetaDataEntry>();
            Values = new ObservableCollection<CBZMetaDataEntry>();
            PageMetaData = new ObservableCollection<CBZMetaDataEntryPage>();
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
            Values = new ObservableCollection<CBZMetaDataEntry>();

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
                                handlePageMetaData(subNode);
                                break;
                            default:
                                Values.Add(new CBZMetaDataEntry(subNode.Name, subNode.InnerText));
                                break;
                        }   
                    }
                }     
            }

            MetaDataFileName = name;
        }

        protected void handlePageMetaData(XmlNode pageNodes)
        {
            foreach (XmlNode subNode in pageNodes.ChildNodes)
            {
            }

        }

        protected void MakeDefaultKeys()
        {
            Defaults.Add(new CBZMetaDataEntry("Seriew", ""));
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
