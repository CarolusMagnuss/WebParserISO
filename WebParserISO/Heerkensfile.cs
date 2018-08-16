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
            List<string> KapitelStrings= new List<string>();
            int start, ende;
            for(int i=0; i<=9; i++)
            {
                start = Kapitel.IndexOf(Oberkategorien[i]);
                ende = Kapitel.IndexOf(Oberkategorien[i + 1])-1;
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
                DefaultValue= ""
            };
            MappingTable.Columns.Add(column);

            return MappingTable;
        }

        public static DataTable FillMappingTable()
        {
            DataTable Mapping = TableSetup();
            List<string> Kapitel = KapitelTeilung();

            
            DataRow row = Mapping.NewRow();
            foreach(string Chapter in Kapitel)
            {
                string[] LineCollection = Chapter.Split("\r\n".ToCharArray());
                for (int i=0; i<=LineCollection.Length-2;i++)
                {
                    switch (CheckLineType(LineCollection[i]))
                    {
                        case 0:
                            List<string> Titelzeile = Kategoriezeile(LineCollection[i]);
                            row["ISO Nummer"] = Titelzeile[0];
                            row["ISO Title"] = Titelzeile[1];
                            if (Titelzeile[0].Length<=5)
                            {   if(CheckLineType(LineCollection[i+1])==1)
                                {
                                    row[2] = (string)row[2] + LineCollection[i+1];
                                }
                                Mapping.Rows.Add(row);
                                row = Mapping.NewRow();
                            }
                            break;
                        case 1:
                            row[2] = (string)row[2] + LineCollection[i];
                            break;
                        case 2:
                            string[] ICFZeile = new string[2];
                            ICFZeile = ICF_Zeile(LineCollection[i]);
                            row[3] = ICFZeile[1];
                            row[4] = ICFZeile[0].Replace('?','\'');
                            Mapping.Rows.Add(row);
                            if(CheckLineType(LineCollection[i+1])!=2)
                            {
                                row = Mapping.NewRow();
                            }
                            break;
                        default:
                            break;
                    }
                }    
            }
            return Mapping;
        }

        public static int CheckLineType(string Zeile)
        {
            int Zeilentyp;
            if (Zeile.Length>=12)
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

            if (Zeile[5]=='.')
            {
                 Nummer = Zeile.Substring(0, 8);
                Title = Zeile.Substring(8, Zeile.Length - 8);
            }
            else
            {
                if(Zeile[2]=='.')
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
            if(Zeile.Contains('(')==true && Zeile.Contains(')'))
            {
                int Trennung = Zeile.IndexOf('(');
                int Ende = Zeile.IndexOf(')');
                Code[0] = Zeile.Substring(0, Trennung - 1);
                Code[1] = Zeile.Substring(Trennung + 1, Ende - Trennung - 1);

            }
            else
            {
                Code[0] = Zeile;
                Code[1] = "";
            }
            return Code;
        } //
         
    }
}
