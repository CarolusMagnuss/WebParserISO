using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Web;
using System.IO;
using System.Xml;

namespace WebParserISO
{
    class XMLSpeichern
    {

        public static void SaveAsXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<IsoKategorien></IsoKategorien>");
            DataRowCollection EndTabelle = Form1.Iso9999.Rows;
            foreach (DataRow row in EndTabelle)
            {
                IsoKnoten(row, doc);
            }

            doc.PreserveWhitespace = true;
            doc.Save("ISO9999new.xml");

        }

        private static void IsoKnoten(DataRow row, XmlDocument Datei)
        {
            XmlElement Kategorie = Datei.CreateElement("IsoKategorie");
            Find_Parent_Cat(row, Datei).AppendChild(Kategorie);

            string[] KategorieAttribute = { "Nummer", "Titel", "Beschreibung", "ICF-Codes" };
            string[] Spalten = { " Nummer ", " Titel ", " Beschreibung ", " ICF-Codes " };

            for (int i = 0; i < 4; i++)
            {
                XmlElement StandardElement = Datei.CreateElement(KategorieAttribute[i]);
                StandardElement.InnerText = row[Spalten[i]].ToString();
                Kategorie.AppendChild(StandardElement);
            }
            string Nummer = (string)row[" Nummer "];
            if (Nummer.Length <= 6)
            {
                XmlElement newElem = Datei.CreateElement("Unterkategorien");
                Kategorie.AppendChild(newElem);
            }
        }

        private static XmlElement Find_Parent_Cat(DataRow row, XmlDocument Datei)
        {
            string Kategorienummer = (string)row[" Nummer "];
            XmlNode Parent = Datei.DocumentElement.FirstChild;

            XmlElement ParentNode;
            if (Kategorienummer.Length > 2)
            {
                string ParentNummer = TopLevelKat(Kategorienummer);
                if (ParentNummer.Length > 2)
                {
                    ParentNummer = TopLevelKat(ParentNummer);
                }

                while (Parent.FirstChild.InnerText != ParentNummer)
                {
                    Parent = Parent.NextSibling;
                }

                if (Kategorienummer.Length > 5)
                {
                    Parent = Parent.LastChild.FirstChild;
                    while (Parent.FirstChild.InnerText != TopLevelKat(Kategorienummer))
                    {
                        Parent = Parent.NextSibling;
                    }

                }

                return ParentNode = (XmlElement)Parent.LastChild;
            }
            else
            {
                return Datei.DocumentElement;
            }
        }

        public static string TopLevelKat(string Nummer)
        {
            string TopLevel;
            if (Nummer.Length >= 4)
            {
                TopLevel = Nummer.Substring(0, Nummer.Length - 3);
                return TopLevel;
            }
            else
            {
                return Nummer;
            }
        }
    }
}
