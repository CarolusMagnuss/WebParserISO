﻿namespace WebParserISO
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.WebtoText = new System.Windows.Forms.RichTextBox();
            this.ISOTable = new System.Windows.Forms.DataGridView();
            this.WebToString = new System.Windows.Forms.Button();
            this.LoadasTree = new System.Windows.Forms.Button();
            this.ConvertToTable = new System.Windows.Forms.Button();
            this.Textviewer = new System.Windows.Forms.Button();
            this.Tableviewer = new System.Windows.Forms.Button();
            this.LoadIsofromFile = new System.Windows.Forms.Button();
            this.Save_as_DataSet = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ISOTable)).BeginInit();
            this.SuspendLayout();
            // 
            // WebtoText
            // 
            this.WebtoText.Location = new System.Drawing.Point(26, 65);
            this.WebtoText.Name = "WebtoText";
            this.WebtoText.Size = new System.Drawing.Size(1131, 436);
            this.WebtoText.TabIndex = 0;
            this.WebtoText.Text = "";
            this.WebtoText.Visible = false;
            // 
            // ISOTable
            // 
            this.ISOTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ISOTable.Location = new System.Drawing.Point(26, 68);
            this.ISOTable.Name = "ISOTable";
            this.ISOTable.Size = new System.Drawing.Size(1131, 431);
            this.ISOTable.TabIndex = 2;
            // 
            // WebToString
            // 
            this.WebToString.Location = new System.Drawing.Point(26, 532);
            this.WebToString.Name = "WebToString";
            this.WebToString.Size = new System.Drawing.Size(199, 25);
            this.WebToString.TabIndex = 4;
            this.WebToString.Text = "Parse ISO9999 von Website";
            this.WebToString.UseVisualStyleBackColor = true;
            this.WebToString.Click += new System.EventHandler(this.WebToString_Click);
            // 
            // LoadasTree
            // 
            this.LoadasTree.Location = new System.Drawing.Point(231, 532);
            this.LoadasTree.Name = "LoadasTree";
            this.LoadasTree.Size = new System.Drawing.Size(199, 25);
            this.LoadasTree.TabIndex = 5;
            this.LoadasTree.Text = "Konvertiere Heerkensfile zu Tabelle";
            this.LoadasTree.UseVisualStyleBackColor = true;
            this.LoadasTree.Click += new System.EventHandler(this.LoadasTree_Click);
            // 
            // ConvertToTable
            // 
            this.ConvertToTable.Location = new System.Drawing.Point(436, 532);
            this.ConvertToTable.Name = "ConvertToTable";
            this.ConvertToTable.Size = new System.Drawing.Size(199, 25);
            this.ConvertToTable.TabIndex = 6;
            this.ConvertToTable.Text = "Speicher ISO-ICF Mapping in XML";
            this.ConvertToTable.UseVisualStyleBackColor = true;
            this.ConvertToTable.Click += new System.EventHandler(this.ConvertToTable_Click);
            // 
            // Textviewer
            // 
            this.Textviewer.Location = new System.Drawing.Point(26, 12);
            this.Textviewer.Name = "Textviewer";
            this.Textviewer.Size = new System.Drawing.Size(46, 24);
            this.Textviewer.TabIndex = 7;
            this.Textviewer.Text = "Text";
            this.Textviewer.UseVisualStyleBackColor = true;
            this.Textviewer.Click += new System.EventHandler(this.Textviewer_Click);
            // 
            // Tableviewer
            // 
            this.Tableviewer.Location = new System.Drawing.Point(78, 12);
            this.Tableviewer.Name = "Tableviewer";
            this.Tableviewer.Size = new System.Drawing.Size(46, 24);
            this.Tableviewer.TabIndex = 9;
            this.Tableviewer.Text = "Tabelle";
            this.Tableviewer.UseVisualStyleBackColor = true;
            this.Tableviewer.Click += new System.EventHandler(this.Tableviewer_Click);
            // 
            // LoadIsofromFile
            // 
            this.LoadIsofromFile.Location = new System.Drawing.Point(1057, 520);
            this.LoadIsofromFile.Name = "LoadIsofromFile";
            this.LoadIsofromFile.Size = new System.Drawing.Size(100, 37);
            this.LoadIsofromFile.TabIndex = 10;
            this.LoadIsofromFile.Text = "Lade Iso aus Datei (default)";
            this.LoadIsofromFile.UseVisualStyleBackColor = true;
            this.LoadIsofromFile.Click += new System.EventHandler(this.LoadIsofromFile_Click);
            // 
            // Save_as_DataSet
            // 
            this.Save_as_DataSet.Location = new System.Drawing.Point(950, 522);
            this.Save_as_DataSet.Name = "Save_as_DataSet";
            this.Save_as_DataSet.Size = new System.Drawing.Size(101, 35);
            this.Save_as_DataSet.TabIndex = 11;
            this.Save_as_DataSet.Text = "Speichere Isotabelle (default)";
            this.Save_as_DataSet.UseVisualStyleBackColor = true;
            this.Save_as_DataSet.Click += new System.EventHandler(this.Save_as_DataSet_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 569);
            this.Controls.Add(this.Save_as_DataSet);
            this.Controls.Add(this.LoadIsofromFile);
            this.Controls.Add(this.Tableviewer);
            this.Controls.Add(this.Textviewer);
            this.Controls.Add(this.ConvertToTable);
            this.Controls.Add(this.LoadasTree);
            this.Controls.Add(this.WebToString);
            this.Controls.Add(this.ISOTable);
            this.Controls.Add(this.WebtoText);
            this.Name = "Form1";
            this.Text = "Parse ISO9999 from WebPage";
            ((System.ComponentModel.ISupportInitialize)(this.ISOTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox WebtoText;
        private System.Windows.Forms.DataGridView ISOTable;
        private System.Windows.Forms.Button WebToString;
        private System.Windows.Forms.Button LoadasTree;
        private System.Windows.Forms.Button ConvertToTable;
        private System.Windows.Forms.Button Textviewer;
        private System.Windows.Forms.Button Tableviewer;
        private System.Windows.Forms.Button LoadIsofromFile;
        private System.Windows.Forms.Button Save_as_DataSet;
    }
}

