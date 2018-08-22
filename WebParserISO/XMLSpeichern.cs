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
            DataTable MappingTabelle = Form1.MappingTable;

            foreach (DataRow row in EndTabelle)
            {
                IsoKnoten(row, MappingTabelle, doc);
            }

            //for(int i =0; i<12;i++)
            //{
            //    IsoKnoten(EndTabelle[i],MappingTabelle,doc);
            //}

            doc.PreserveWhitespace = true;
            doc.Save("ISO9999new.xml");

        }

        private static void IsoKnoten(DataRow ISOrow, DataTable MappingTable , XmlDocument Datei)
        {
            // Erschaffe neue Kategorie und Finde Überkategorie, an welche sie angehängt wird.
            XmlElement Kategorie = Datei.CreateElement("IsoKategorie");
            //Datei.DocumentElement.AppendChild(Kategorie);
            Find_Parent_Cat(ISOrow, Datei).AppendChild(Kategorie);

            string[] ISOSpalten = { " Nummer ", " Titel ", " Beschreibung ", " ICF-Codes " };

            string[] MappingSpalten = { "ISO Nummer", "ISO Title", "ISO Description", "ICF Code", "ICF Title" };

            string[] AttributeEbene1 = { "Nummer", "Lang" };
            string[,] AttributeEbene2 = { { "Titel", "Title" },{ "Beschreibung", "Description" },{"ICF-Codes","ICF-Codes" } };
            string[] AttributEbene3 = { "ICF-Code", "Beschreibung", "Description"};

            //Erschaffe erste Ebene:

            XmlElement StandardElement = Datei.CreateElement("Nummer");
            StandardElement.InnerText = ISOrow[ISOSpalten[0]].ToString();
            Kategorie.AppendChild(StandardElement);

            StandardElement = Datei.CreateElement("Lang");
            Kategorie.AppendChild(StandardElement);

            string Nummer = (string)ISOrow[" Nummer "];
            if (Nummer.Length <= 6)
            {
                XmlElement newElem = Datei.CreateElement("Unterkategorien");
                Kategorie.AppendChild(newElem);
            }

            //Erschaffe zweite Ebene
            XmlNode Aktives = Kategorie.FirstChild.NextSibling;

            StandardElement = Datei.CreateElement("Deu");
            Aktives.AppendChild(StandardElement);

            StandardElement = Datei.CreateElement("Eng");
            Aktives.AppendChild(StandardElement);

            //Erschaffe dritte Ebene
            
            DataRowCollection Korrespondierende = CorrespondingRows(ISOrow, MappingTable);
            XmlNode Laufnode;

            for (int Sprachnummer = 0; Sprachnummer < 2; Sprachnummer++)
            {
                    for (int i = 0; i < 2; i++)
                    {
                        StandardElement = Datei.CreateElement(AttributeEbene2[i, Sprachnummer]);

                        if (Sprachnummer == 0)
                        {
                            StandardElement.InnerText = ISOrow[ISOSpalten[i + 1]].ToString();
                            Aktives.FirstChild.AppendChild(StandardElement);
                        }
                        else
                        {
                            if (Korrespondierende.Count > 0)
                            {
                                StandardElement.InnerText = Korrespondierende[0][MappingSpalten[i + 1]].ToString();
                                Aktives.LastChild.AppendChild(StandardElement);
                            }
                        }
                    }

                    if (ISOrow[ISOSpalten[0]].ToString().Length == 8)
                    {
                        if (Sprachnummer == 0)
                        {
                            Laufnode = Aktives.FirstChild;
                        }
                        else
                        {
                            Laufnode = Aktives.LastChild;
                        }

                        StandardElement = Datei.CreateElement("ICF-Codes");
                        Laufnode.AppendChild(StandardElement);

                        foreach (DataRow row in Korrespondierende)
                        {
                        StandardElement = Datei.CreateElement("ICF-Code");
                        Laufnode.LastChild.AppendChild(StandardElement);
                            
                            StandardElement = Datei.CreateElement(AttributEbene3[0]);
                            StandardElement.InnerText = (string)row[MappingSpalten[3]];
                            Laufnode.LastChild.LastChild.AppendChild(StandardElement);

                            StandardElement = Datei.CreateElement(AttributEbene3[1]);
                            StandardElement.InnerText = (string)row[MappingSpalten[4]];
                            Laufnode.LastChild.LastChild.AppendChild(StandardElement);

                        }
                    }
                
            }
        }

        /// <summary>
        /// Findet die zu einer in der ISO Tabelle hinterlegten ISO Nummer gehörigen Zeilen aus der Mappingtabelle
        /// </summary>
        /// <param name="ISOrow"></param>
        /// <param name="MappingTable"></param>
        /// <returns></returns>
        public static DataRowCollection CorrespondingRows(DataRow ISOrow, DataTable MappingTable)        
        {
            DataRowCollection MappingTColl = MappingTable.Rows;
            DataTable Korrespondierende = MappingTable.Clone();
            string Key = (string)ISOrow[" Nummer "];

            foreach(DataRow row in MappingTColl)
            {
                if((string)row["ISO Nummer"]==Key)
                {
                    Korrespondierende.ImportRow(row);
                }
            }
            return Korrespondierende.Rows;
        }

        /// <summary>
        /// Findet den Knoten der übergeordneten Kategorie der in der Zeile gespeicherten Kategorie
        /// </summary>
        /// <param name="row"></param>
        /// <param name="Datei"></param>
        /// <returns></returns>
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
