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

        MemoryStream ms;
        StringReader xsltManifest;
        XmlReader xslReader;
        XslCompiledTransform xTrans;
        XmlReader xReader;
        StringReader sr;
        XmlReaderSettings xmlReaderSettings;

        public MetaDataForm(string metaData)
        {
            InitializeComponent();

            MetaData = metaData;

            //MetaData = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Test><mykey>test</mykey><value>omg</value></Test>";

            // Load the xslt used by IE to render the xml

            xsltManifest = new StringReader(Properties.Resources.ResourceManager.GetString(Uri.EscapeUriString("defaults").ToLowerInvariant()));
            xslReader = XmlReader.Create(xsltManifest);
            xTrans = new XslCompiledTransform();
            xTrans.Load(xslReader);

            // Read the xml string.
            xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
            sr = new StringReader(MetaData);
            xReader = XmlReader.Create(sr, xmlReaderSettings);

            // Transform the XML data
            ms = new MemoryStream();
            //xTrans.OutputSettings.
            xTrans.Transform(xReader, null, ms);

            ms.Position = 0;
            // Set to the document stream
            metaDataView.DocumentStream = ms;

        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {

        }

        private void MetaDataForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ms.Close();
            ms.Dispose();

            xsltManifest.Close();
            xslReader.Close();

            xTrans.TemporaryFiles.Delete();
            
            xReader.Close();
            xReader.Dispose();

            sr.Close();
            sr.Dispose();
            xmlReaderSettings.CloseInput = false;
        }
    }
}
