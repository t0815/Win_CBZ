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
using System.Runtime.Versioning;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    internal partial class MetaDataForm : Form
    {
        MetaData MetaData;

        MemoryStream ms;
        StringReader xsltManifest;
        XmlReader xslReader;
        XslCompiledTransform xTrans;
        XmlReader xReader;
        StringReader sr;
        XmlReaderSettings xmlReaderSettings;

        public MetaDataForm(MetaData metaData)
        {
            InitializeComponent();

            MetaData = metaData;

            MemoryStream fragmentStream = Program.ProjectModel.MetaData.BuildComicInfoXMLStream(true);
            
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            String metaDataString = utf8WithoutBom.GetString(fragmentStream.ToArray());

            //MetaData = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Test><mykey>test</mykey><value>omg</value></Test>";

            // Load the xslt used by IE to render the xml

            xsltManifest = new StringReader(Properties.Resources.ResourceManager.GetString(Uri.EscapeUriString("defaults").ToLowerInvariant()));
            xslReader = XmlReader.Create(xsltManifest);
            xTrans = new XslCompiledTransform();
            xTrans.Load(xslReader);

            // Read the xml string.
            xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
            sr = new StringReader(metaDataString);
            xReader = XmlReader.Create(sr, xmlReaderSettings);

            // Transform the XML data
            ms = new MemoryStream();
            //xTrans.OutputSettings.
            xTrans.Transform(xReader, null, ms);

            ms.Position = 0;
            // Set to the document stream
            metaDataView.DocumentStream = ms;

            fragmentStream.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MetaDataForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ms.Close();
            ms.Dispose();

            xsltManifest.Close();
            xslReader.Close();

            //xTrans.TemporaryFiles.Delete();
            
            xReader.Close();
            xReader.Dispose();

            sr.Close();
            sr.Dispose();
            xmlReaderSettings.CloseInput = false;
        }

        private void MetaDataForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MemoryStream copyMemStream = new MemoryStream();
                var utf8WithoutBom = new System.Text.UTF8Encoding(false);

                MemoryStream fullCopy = Program.ProjectModel.MetaData.BuildComicInfoXMLStream();

                XmlDocument doc = new XmlDocument
                {
                    PreserveWhitespace = true,
                };

                XmlReader MetaDataReader = XmlReader.Create(fullCopy);
                MetaDataReader.Read();
                doc.Load(MetaDataReader);

                //Create an XML declaration.
                //XmlDeclaration xmldecl;
                //xmldecl = doc.CreateXmlDeclaration("1.0", null, null);

                //Add the new node to the document.
                //XmlElement root = doc.DocumentElement;
                //doc.InsertBefore(xmldecl, root);
                doc.Save(copyMemStream);
                copyMemStream.Position = 0;

                byte[] encoded = new byte[copyMemStream.Length];
                copyMemStream.Read(encoded, 0, (int)copyMemStream.Length);

                DataObject data = new DataObject();
                data.SetData(DataFormats.UnicodeText, utf8WithoutBom.GetString(encoded));

                Clipboard.SetDataObject(data);

                copyMemStream.Close();
                fullCopy.Close();
            } catch (Exception ex) 
            {
                ApplicationMessage.ShowException(ex);
            }
            
        }
    }
}
