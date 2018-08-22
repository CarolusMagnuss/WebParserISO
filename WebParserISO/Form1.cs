﻿using System;
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
        
        
        public static DataTable Iso9999 = new DataTable("ISO 9999");
        public static DataTable MappingTable = new DataTable("IsoToICFMapping");

        public Form1()
        {
            InitializeComponent();
        }

        private void WebToString_Click(object sender, EventArgs e)
        {   
            Iso9999= RehadatParser.CreateISOTable();
            ISOTable.DataSource = Iso9999;
        }

        private void ConvertToTable_Click(object sender, EventArgs e)
        {
            XMLSpeichern.SaveAsXML();
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
            MappingTable= Heerkensfile.FillMappingTable();
            ISOTable.DataSource = MappingTable;
            
        }

        private void LoadIsofromFile_Click(object sender, EventArgs e)
        {
            var DataSet = new DataSet();
            DataSet.ReadXml(Application.StartupPath + "/StandardWay.xml");
            Iso9999 = DataSet.Tables[0];
            ISOTable.DataSource = Iso9999;
        }

        private void Save_as_DataSet_Click(object sender, EventArgs e)
        {
            var dataSet = new DataSet();
            dataSet.Tables.Add(Iso9999);

            // Write dataset to xml file or stream
            dataSet.WriteXml("StandardWay.xml");
        }
    }
}