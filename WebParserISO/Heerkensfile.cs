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
                                                "30 ASSISTIVE PRODUCTS FOR RECREATION" };

        public static string LoadFiletoBox(string file)
        {
            string FileText = File.ReadAllText(file, Encoding.ASCII);

            return FileText;
        }

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
        }

        public static string GetChapters()
        {
            List<string> Kapitelliste = KapitelTeilung();
           
            string AllChaps = string.Join(" ", Kapitelliste);

            return AllChaps;
        }
    }
}
