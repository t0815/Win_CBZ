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


        public ObservableCollection<CBZMetaDataEntry> Values { get; set; }


        private Stream InputStream;

       
        public CBZMetaData(bool createDefault = false)
        {
            Values = new ObservableCollection<CBZMetaDataEntry>();
            MetaData = new XmlDocument();

            if (createDefault)
            {
                MakeDefaultKeys();
            }
        }

        public CBZMetaData(Stream fileInputStream, String name)
        {
            InputStream = fileInputStream;

            Values = new ObservableCollection<CBZMetaDataEntry>();
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

        public void MakeDefaultKeys()
        {
            Values.Add(new CBZMetaDataEntry("Seriew", ""));
            Values.Add(new CBZMetaDataEntry("Number", ""));
            Values.Add(new CBZMetaDataEntry("Web", ""));
            Values.Add(new CBZMetaDataEntry("Summary", ""));
            Values.Add(new CBZMetaDataEntry("Notes", ""));
            Values.Add(new CBZMetaDataEntry("Publisher", ""));
            Values.Add(new CBZMetaDataEntry("Imprint", ""));
            Values.Add(new CBZMetaDataEntry("Genre", ""));  
            Values.Add(new CBZMetaDataEntry("PageCount", ""));
            Values.Add(new CBZMetaDataEntry("LanguageISO", ""));
            Values.Add(new CBZMetaDataEntry("Artist", ""));
            Values.Add(new CBZMetaDataEntry("Title", ""));
            Values.Add(new CBZMetaDataEntry("Year", ""));
            Values.Add(new CBZMetaDataEntry("Month", ""));
            Values.Add(new CBZMetaDataEntry("Day", ""));
            Values.Add(new CBZMetaDataEntry("AgeRating", ""));
            Values.Add(new CBZMetaDataEntry("Characters", ""));
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
