using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Xsl;
using System.Xml;
using System.Data.SqlTypes;
using System.IO;

namespace Win_CBZ.Forms
{
    public partial class MetaDataForm : Form
    {
        String MetaData;

        public MetaDataForm(string metaData)
        {
            InitializeComponent();

            MetaData = metaData;

            //MetaData = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Test><mykey>test</mykey><value>omg</value></Test>";
            
            // Load the xslt used by IE to render the xml
            XslCompiledTransform xTrans = new XslCompiledTransform();
            xTrans.Load(Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName, @"resources\defaults.xsl"));

            // Read the xml string.
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
            StringReader sr = new StringReader(MetaData);
            XmlReader xReader = XmlReader.Create(sr, xmlReaderSettings);

            // Transform the XML data
            MemoryStream ms = new MemoryStream();
            //xTrans.OutputSettings.
            xTrans.Transform(xReader, null, ms);

            ms.Position = 0;
            // Set to the document stream
            metaDataView.DocumentStream = ms;

        }
    }
}
