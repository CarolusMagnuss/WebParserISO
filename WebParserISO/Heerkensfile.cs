using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Web;
using System.IO;
using System.Xml;

namespace WebParserISO
{
    class Heerkensfile
    {
        public static string[] Oberkategorien = {    "04 ASSISTIVE PRODUCTS FOR PERSONAL MEDICAL TREATMENT",
                                                "05 ASSISTIVE PRODUCTS FOR TRAINING IN SKILLS",
                                                "06 ORTHOSES AND PROSTHESES",
                                                "09 ASSISTIVE PRODUCTS FOR PERSONAL CARE AND PROTECTION",
                                                "12 ASSISTIVE PRODUCTS FOR PERSONAL MOBILITY ",
                                                "15 ASSISTIVE PRODUCTS FOR HOUSEKEEPING",
                                                "18 FURNISHINGS AND ADAPTATIONS TO HOMES AND OTHER PREMISES",
                                                "22 ASSISTIVE PRODUCTS FOR COMMUNICATION AND INFORMATION",
                                                "24 ASSISTIVE PRODUCTS FOR HANDLING OBJECTS AND DEVICES",
                                                "27 ASSISTIVE PRODUCTS FOR ENVIRONMENTAL IMPROVEMENT, TOOLS AND MACHINES",
                                                "30 ASSISTIVE PRODUCTS FOR RECREATION" }; //

        public static string LoadFiletoBox(string file)
        {
            string FileText = File.ReadAllText(file, Encoding.ASCII);

            return FileText;
        } //

        public static List<string> KapitelTeilung()
        {
            string Kapitel = LoadFiletoBox("ISO9999withICFreferences.txt");
            List<string> KapitelStrings = new List<string>();
            int start, ende;
            for (int i = 0; i <= 9; i++)
            {
                start = Kapitel.IndexOf(Oberkategorien[i]);
                ende = Kapitel.IndexOf(Oberkategorien[i + 1]) - 1;
                KapitelStrings.Add(Kapitel.Substring(start, ende - start));
            }
            start = Kapitel.IndexOf(Oberkategorien[10]);
            ende = Kapitel.Length;
            KapitelStrings.Add(Kapitel.Substring(start, ende - start));

            return KapitelStrings;
        } //

        public static string GetChapters()
        {
            List<string> Kapitelliste = KapitelTeilung();

            string AllChaps = string.Join(" ", Kapitelliste);

            return AllChaps;
        } //

        private static DataTable TableSetup() //
        {
            DataTable MappingTable = new DataTable();
            DataColumn column;

            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = "ISO Nummer"
            };
            MappingTable.Columns.Add(column);

            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = "ISO Title"
            };
            MappingTable.Columns.Add(column);
            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = "ISO Description",
                DefaultValue = ""
            };
            MappingTable.Columns.Add(column);
            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = "ICF Code",
                DefaultValue = ""
            };
            MappingTable.Columns.Add(column);
            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = "ICF Title",
                DefaultValue = ""
            };
            MappingTable.Columns.Add(column);

            return MappingTable;
        }

        public static DataTable FillMappingTable()
        {
            DataTable Mapping = TableSetup();
            List<string> Kapitel = KapitelTeilung();


            DataRow row = Mapping.NewRow();
            foreach (string Chapter in Kapitel)
            {
                string[] LineCollection = Get_Cleared_LineCollection(Chapter);

                for (int i = 0; i <= LineCollection.Length - 2; i++)
                {
                    string[] Entry = IsoMappingEntry(LineCollection, i);
                    int EntrySize = Entry.Length;
                    List<string> Titel = Kategoriezeile(Entry[0]);
                    row[0] = Titel[0];
                    row[1] = CorrectTitle(Titel[1]);
                    if(EntrySize>1)
                    {
                        for(int k=1;k<=EntrySize-1;k++)
                        {
                            if(CheckLineType(Entry[k])==1)
                            {
                                row[2] = (string)row[2] + Entry[k];
                            }
                            if(CheckLineType(Entry[k])==2)
                            {
                                string[] Code = ICF_Zeile(Entry[k]);
                                row[3] = Code[1];
                                row[4] = Code[0].Replace('?','\'');
                                if(k!=EntrySize-1)
                                {
                                    Mapping.Rows.Add(row);
                                    row = Mapping.NewRow();
                                    row[0] = Titel[0];
                                    row[1] = CorrectTitle(Titel[1]);
                                }
                            }
                        }

                    }
                    i = i + EntrySize-1;
                    Mapping.Rows.Add(row);
                    row = Mapping.NewRow();
                }
            }
            return Mapping;
        }

        public static int CheckLineType(string Zeile)
        {
            int Zeilentyp;
            if (Zeile.Length >= 12)
            {
                if (Zeile.Contains("ICF-reference:") == true)
                {
                    return Zeilentyp = 2; // ICF-Zeile
                }
                if (Int32.TryParse(Zeile.Substring(0, 2), out int Kat) == true)
                {
                    return Zeilentyp = 0; // Kategoriezeile
                }
                if (Int32.TryParse(Zeile.Substring(0, 2), out Kat) == false && Zeile.Contains("ICF-reference:") == false)
                {
                    return Zeilentyp = 1; // Beschreibungszeile
                }
            }
            return Zeilentyp = 3; //Leerzeile
        } //

        public static List<string> Kategoriezeile(string Zeile)
        {
            List<string> Eintrage = new List<string>();
            string Nummer, Title;

            if (Zeile[5] == '.')
            {
                Nummer = Zeile.Substring(0, 8);
                Title = Zeile.Substring(8, Zeile.Length - 8);
            }
            else
            {
                if (Zeile[2] == '.')
                {
                    Nummer = Zeile.Substring(0, 5);
                    Title = Zeile.Substring(5, Zeile.Length - 5);
                }
                else
                {
                    Nummer = Zeile.Substring(0, 2);
                    Title = Zeile.Substring(2, Zeile.Length - 2);

                }
            }
            Eintrage.Add(Nummer);
            Eintrage.Add(Title);
            return Eintrage;
        } //

        public static string[] ICF_Zeile(string Zeile)
        {
            string[] Code = new string[2];
            Zeile = Zeile.Remove(0, 15);
            Zeile = Zeile.Trim(' ');
            
            if (Zeile.Contains('(') == true && Zeile.Contains(')'))
            {
                int Trennung = Zeile.IndexOf('(');
                int Ende = Zeile.IndexOf(')');
                Code[1] = Zeile.Substring(Trennung + 1, Ende - Trennung - 1);
                if (Char.IsNumber(Code[1][Code[1].Length - 1]) == false)
                {
                    Code[0] = Zeile.Substring(0, Ende + 1);
                    Code[1] = Zeile.Substring(Ende + 2, Zeile.Length - 2 - Ende);
                    Code[1] = Code[1].Trim('(', ')');
                }
                else
                {
                    Code[1] = Zeile.Substring(Trennung + 1, Ende - Trennung - 1);
                    Code[0] = Zeile.Substring(0, Trennung - 1);
                }
            }
            else
            {
                Code[0] = Zeile;
                Code[1] = "";
            }
            return Code;
        } //

        public static string[] Get_Cleared_LineCollection(string Chapter)
        {
            string[] LineCollection = Chapter.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return LineCollection;

        }

        public static string[] IsoMappingEntry(string[] Linecollection, int StartingLine)

        {
            List<string> MappingEntry = new List<string>
            {
                Linecollection[StartingLine]
            };
            int i = 1;
            while (StartingLine+ i <= Linecollection.Length - 2 && CheckLineType(Linecollection[StartingLine + i]) != 0)
            {
                MappingEntry.Add(Linecollection[StartingLine + i]);
                i = i + 1;
            }
            
            return MappingEntry.ToArray();
        }

        public static string CorrectTitle(string Title)
        {
            Title = Title.Trim(' ');
            int TitleLength = Title.Length;
            if(Char.IsNumber(Title[TitleLength-1])==true)
            {
                Title = Title.Substring(0, TitleLength - 1);
            }
            return Title;
        }
        
    }
}