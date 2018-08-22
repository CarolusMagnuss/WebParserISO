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
    class RehadatParser
    {
        static string Basisadresse = "https://www.rehadat-hilfsmittel.de/de/suche/index.html";
        static string startpage = "https://www.rehadat-hilfsmittel.de/de/suche/index.html?connectdb=tecisolevel1&infobox=/infobox1.html&serviceCounter=1";
        static DataTable Iso9999 = new DataTable("ISO 9999");

        public static DataTable CreateISOTable()
        {
            Create_TableStructure();
            Get_Categories_from_Site(startpage);
            for (int i = 0; i < 2; i++)
            {
                Get_SubCategories(Iso9999);
            }
            return Iso9999;
        }

        public static void Get_Categories_from_Site(string URL)
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

        public static void Get_SubCategories(DataTable CurrentTable)
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

        private static void Split_String_To_Entry(string WebTableEntry)
        {
            DataRow row = Iso9999.NewRow();
            string Titeleintrag= getBetween(WebTableEntry, "title=\"", "\">");
            if (Titeleintrag.Length>=9 && Titeleintrag.Substring(0,8)=="Produkte")
            {
                int TitelStart = WebTableEntry.IndexOf('>');
                int TitelEnde = WebTableEntry.IndexOf("</a>")-1;
                char[] CharsToTrim = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ' ' };
                Titeleintrag = WebTableEntry.Substring(TitelStart + 1, WebTableEntry.Length - TitelStart - 2);
                TitelEnde = Titeleintrag.IndexOf('<') - 1;
                Titeleintrag = Titeleintrag.Substring(0, TitelEnde);
                Titeleintrag = Titeleintrag.Trim(CharsToTrim);
            }

            row[" Titel "] = Titeleintrag  ;
            
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
            if (Beschreibung.Contains("siehe)") == true)
            {
                Beschreibung = Beschreibung.Substring(0, WebTableEntry.IndexOf("siehe", 0 - 1));
            }
            if (Beschreibung.Length>=5 && Beschreibung.Substring(0,4)=="href")
            {
                Beschreibung = "";
            }
            row[" Beschreibung "] = Beschreibung;
            Iso9999.Rows.Add(row);
            
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

        public static string Remove_Ampersands(string link)
        {
            while (link.Contains("amp;") == true)
            {
                int start = link.IndexOf("amp;", 0);
                link = link.Remove(start, 4);

            }
            return link;
        }

        private static string NummernKorrektur(string ErsterSchurf, string Quelllink)
        {
            string ZweiterSchurf;
            ZweiterSchurf = getBetween(Quelllink, "nr=", "*");
            ZweiterSchurf = ZweiterSchurf.Replace('+', '.');
            return ZweiterSchurf;

        }

        private static HtmlAgilityPack.HtmlDocument HtmlStruct(string URL)
        {
            HtmlAgilityPack.HtmlWeb Webpage = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument htmldoc = Webpage.Load(URL);

            return htmldoc;

        }

        private static void Create_TableStructure()
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

        }
    }
}
