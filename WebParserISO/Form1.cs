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
    public partial class Form1 : Form
    {
        string startpage = "https://www.rehadat-hilfsmittel.de/de/suche/index.html?connectdb=tecisolevel1&infobox=/infobox1.html&serviceCounter=1";
        string Basisadresse = "https://www.rehadat-hilfsmittel.de/de/suche/index.html";
        DataTable Iso9999 = new DataTable("ISO 9999");

        public Form1()
        {
            InitializeComponent();
            Create_IsoTable();
        }

        private void WebToString_Click(object sender, EventArgs e)
        {
            Get_Categories_from_Site(startpage);
            for (int i = 0; i < 2; i++)
            {
                Get_SubCategories(Iso9999);
            }

        }

        private void ConvertToTable_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<IsoKategorien></IsoKategorien>");
            DataRowCollection EndTabelle = Iso9999.Rows;
            foreach (DataRow row in EndTabelle)
            {
                IsoKnoten(row, doc);
            }

            doc.PreserveWhitespace = true;
            doc.Save("ISO9999.xml");

        }

        private void IsoKnoten(DataRow row, XmlDocument Datei)
        {
            XmlElement Kategorie = Datei.CreateElement("IsoKategorie");
            Find_Parent_Cat(row,Datei).AppendChild(Kategorie);

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

        private XmlElement Find_Parent_Cat(DataRow row, XmlDocument Datei)
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

        private void Get_Categories_from_Site(string URL)
        {
            HtmlAgilityPack.HtmlDocument currentPage = HtmlStruct(URL);
            var StartNode = currentPage.DocumentNode.SelectSingleNode("//table");
            HtmlAgilityPack.HtmlNodeCollection Categories = StartNode.SelectNodes("./tr");

            int AnzahlInKat = Categories.Count();
            string[] FolgeLinks = new string[AnzahlInKat];

            int i = 0;
            foreach (HtmlAgilityPack.HtmlNode aktiver in Categories)
            {
                int k = 0;
                HtmlAgilityPack.HtmlNodeCollection CategorieElements = aktiver.ChildNodes;
                foreach (HtmlAgilityPack.HtmlNode SubAktiver in CategorieElements)
                {
                    k = k + 1;
                    int j = 0;
                    HtmlAgilityPack.HtmlNodeCollection WebZeile = SubAktiver.ChildNodes;
                    foreach (HtmlAgilityPack.HtmlNode Kind in WebZeile)
                    {
                        j = j + 1;
                        if (j == 1 && k == 2 && i >= 1)
                        {
                            Split_String_To_Entry(Kind.InnerHtml);
                        }
                    }
                }
                i = i + 1;
            }
        }

        private void Get_SubCategories(DataTable CurrentTable)
        {
            DataTable Buffer = CurrentTable.Copy();
            DataRowCollection RowCollection = Buffer.Rows;
            int rownumber = 0;
            foreach (DataRow row in RowCollection)
            {
                if ((bool)row[" Sub Gelesen "] == false)
                {
                    Get_Categories_from_Site(Basisadresse + (string)row[" Link "]);
                    DataRow actualRow = Iso9999.Rows[rownumber];
                    actualRow[" Sub Gelesen "] = true;
                }
                rownumber = rownumber + 1;
            }
        }

        private void Split_String_To_Entry(string WebTableEntry)
        {
            DataRow row = Iso9999.NewRow();

            row[" Titel "] = getBetween(WebTableEntry, "title=\"", "\">"); ;
            row[" Link "] = Remove_Ampersands(getBetween(WebTableEntry, "href=\"", "\" title"));

            string IsoNummer = getBetween(WebTableEntry, "Iso=", "\" title");
            if (IsoNummer.Length == 0)
            {
                row[" Nummer "] = NummernKorrektur(IsoNummer, WebTableEntry);
                row[" Sub Gelesen "] = true;
            }
            else
            {
                row[" Nummer "] = IsoNummer;
            }
            int start, end;
            start = WebTableEntry.IndexOf("<br>", 0) + 4;
            end = WebTableEntry.Length - start;
            string Beschreibung = WebTableEntry.Substring(start, end);
            if (Beschreibung.Contains("siehe)")==true)
            {
                Beschreibung = Beschreibung.Substring(0, WebTableEntry.IndexOf("siehe", 0 - 1));
            }
            row[" Beschreibung "] = Beschreibung;
            //if(IsoNummer.Length>=4)
            //{
            //DataRowCollection CurrentCats = Iso9999.Rows;
            //    DataRow InsertionRow = CurrentCats.Find(TopLevelKat(IsoNummer));
            //    int InsertionNumber = CurrentCats.IndexOf(InsertionRow);
            //   while (TopLevelKat((string)InsertionRow[" Nummer "]) == TopLevelKat(IsoNummer) && LowLevelKat((string)InsertionRow[" Nummer "]) < LowLevelKat(IsoNummer))
            //    {
            //        InsertionNumber = InsertionNumber + 1;
            //        InsertionRow = CurrentCats[InsertionNumber];
            //    }
            //    CurrentCats.InsertAt(row, InsertionNumber - 1);
            //}
            //else
            //{
            Iso9999.Rows.Add(row);
            //}
        }

        private string NummernKorrektur(string ErsterSchurf, string Quelllink)
        {
            string ZweiterSchurf;
            ZweiterSchurf = getBetween(Quelllink, "nr=", "*");
            ZweiterSchurf = ZweiterSchurf.Replace('+', '.');
            return ZweiterSchurf;

        }

        public string TopLevelKat(string Nummer)
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

        public int LowLevelKat(string Nummer)
        {
            string LowLevel;
            if (Nummer.Length >= 4)
            {
                LowLevel = Nummer.Substring(Nummer.Length - 2, 2);
                int LowLevelKat = Int32.Parse(LowLevel);
                return LowLevelKat;
            }
            else
            {
                return 0;
            }

        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        private string Remove_Ampersands(string link)
        {
            while (link.Contains("amp;") == true)
            {
                int start = link.IndexOf("amp;", 0);
                link = link.Remove(start, 4);

            }
            return link;
        }

        private void Create_IsoTable()
        {
            DataColumn column;

            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = " Nummer "

            };
            Iso9999.Columns.Add(column);

            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = " Titel "

            };
            Iso9999.Columns.Add(column);

            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = " Beschreibung "

            };
            Iso9999.Columns.Add(column);

            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = " ICF-Codes ",
                DefaultValue = ""

            };
            Iso9999.Columns.Add(column);

            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = " Link "

            };
            Iso9999.Columns.Add(column);

            column = new DataColumn("BoolProperty", typeof(bool))
            {
                DefaultValue = false,
                ColumnName = " Sub Gelesen "

            };
            Iso9999.Columns.Add(column);

            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = Iso9999.Columns[" Nummer "];
            Iso9999.PrimaryKey = PrimaryKeyColumns;

            ISOTable.DataSource = Iso9999;


        }

        private string WebToText(string URL)
        {

            WebClient client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            string PageText = client.DownloadString(URL);
            PageText.Replace("&nbsp;", "");
            PageText.Replace("K&ouml;ln", "Köln");
            //WebClient client = new WebClient();
            //{
            //    var htmlData = client.DownloadData(URL);
            //    PageText = Encoding.UTF8.GetString(htmlData);

            //}
            return PageText;
        }

        private HtmlAgilityPack.HtmlDocument HtmlStruct(string URL)
        {
            HtmlAgilityPack.HtmlWeb Webpage = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument htmldoc = Webpage.Load(URL);

            return htmldoc;

        }

        private XmlDocument TextToTree(string PageText)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(PageText);

            return doc;

        }

        private void Textviewer_Click(object sender, EventArgs e)
        {
            WebtoText.Visible = true;
            ISOTable.Visible = false;
        }
               
        private void Tableviewer_Click(object sender, EventArgs e)
        {
            WebtoText.Visible = false;
            ISOTable.Visible = true;
        }

        private void LoadasTree_Click(object sender, EventArgs e)
        {
            ISOTable.DataSource = Heerkensfile.FillMappingTable();
            
        }
        
    }
}