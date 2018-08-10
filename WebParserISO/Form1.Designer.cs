namespace WebParserISO
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
            this.NodeForm = new System.Windows.Forms.TreeView();
            this.ISOTable = new System.Windows.Forms.DataGridView();
            this.URLadress = new System.Windows.Forms.TextBox();
            this.WebToString = new System.Windows.Forms.Button();
            this.LoadasTree = new System.Windows.Forms.Button();
            this.ConvertToTable = new System.Windows.Forms.Button();
            this.Textviewer = new System.Windows.Forms.Button();
            this.NodeViewer = new System.Windows.Forms.Button();
            this.Tableviewer = new System.Windows.Forms.Button();
            this.ISOBrowser = new System.Windows.Forms.WebBrowser();
            this.WebView = new System.Windows.Forms.Button();
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
            // 
            // NodeForm
            // 
            this.NodeForm.Location = new System.Drawing.Point(26, 67);
            this.NodeForm.Name = "NodeForm";
            this.NodeForm.Size = new System.Drawing.Size(1131, 433);
            this.NodeForm.TabIndex = 1;
            this.NodeForm.Visible = false;
            // 
            // ISOTable
            // 
            this.ISOTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ISOTable.Location = new System.Drawing.Point(26, 68);
            this.ISOTable.Name = "ISOTable";
            this.ISOTable.Size = new System.Drawing.Size(1131, 431);
            this.ISOTable.TabIndex = 2;
            this.ISOTable.Visible = false;
            // 
            // URLadress
            // 
            this.URLadress.Location = new System.Drawing.Point(26, 517);
            this.URLadress.Name = "URLadress";
            this.URLadress.Size = new System.Drawing.Size(247, 20);
            this.URLadress.TabIndex = 3;
            // 
            // WebToString
            // 
            this.WebToString.Location = new System.Drawing.Point(146, 543);
            this.WebToString.Name = "WebToString";
            this.WebToString.Size = new System.Drawing.Size(199, 25);
            this.WebToString.TabIndex = 4;
            this.WebToString.Text = "Lade ISO in Tabelle";
            this.WebToString.UseVisualStyleBackColor = true;
            this.WebToString.Click += new System.EventHandler(this.WebToString_Click);
            // 
            // LoadasTree
            // 
            this.LoadasTree.Location = new System.Drawing.Point(535, 543);
            this.LoadasTree.Name = "LoadasTree";
            this.LoadasTree.Size = new System.Drawing.Size(199, 25);
            this.LoadasTree.TabIndex = 5;
            this.LoadasTree.Text = "Lade XML";
            this.LoadasTree.UseVisualStyleBackColor = true;
            this.LoadasTree.Click += new System.EventHandler(this.LoadasTree_Click);
            // 
            // ConvertToTable
            // 
            this.ConvertToTable.Location = new System.Drawing.Point(901, 543);
            this.ConvertToTable.Name = "ConvertToTable";
            this.ConvertToTable.Size = new System.Drawing.Size(199, 25);
            this.ConvertToTable.TabIndex = 6;
            this.ConvertToTable.Text = "Speicher ISO in XML";
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
            // NodeViewer
            // 
            this.NodeViewer.Location = new System.Drawing.Point(78, 12);
            this.NodeViewer.Name = "NodeViewer";
            this.NodeViewer.Size = new System.Drawing.Size(46, 24);
            this.NodeViewer.TabIndex = 8;
            this.NodeViewer.Text = "Tree";
            this.NodeViewer.UseVisualStyleBackColor = true;
            this.NodeViewer.Click += new System.EventHandler(this.NodeViewer_Click);
            // 
            // Tableviewer
            // 
            this.Tableviewer.Location = new System.Drawing.Point(130, 12);
            this.Tableviewer.Name = "Tableviewer";
            this.Tableviewer.Size = new System.Drawing.Size(46, 24);
            this.Tableviewer.TabIndex = 9;
            this.Tableviewer.Text = "Tabelle";
            this.Tableviewer.UseVisualStyleBackColor = true;
            this.Tableviewer.Click += new System.EventHandler(this.Tableviewer_Click);
            // 
            // ISOBrowser
            // 
            this.ISOBrowser.Location = new System.Drawing.Point(26, 68);
            this.ISOBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.ISOBrowser.Name = "ISOBrowser";
            this.ISOBrowser.Size = new System.Drawing.Size(1131, 431);
            this.ISOBrowser.TabIndex = 10;
            this.ISOBrowser.Visible = false;
            // 
            // WebView
            // 
            this.WebView.Location = new System.Drawing.Point(182, 12);
            this.WebView.Name = "WebView";
            this.WebView.Size = new System.Drawing.Size(46, 24);
            this.WebView.TabIndex = 11;
            this.WebView.Text = "Web";
            this.WebView.UseVisualStyleBackColor = true;
            this.WebView.Click += new System.EventHandler(this.WebView_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 569);
            this.Controls.Add(this.WebView);
            this.Controls.Add(this.ISOBrowser);
            this.Controls.Add(this.Tableviewer);
            this.Controls.Add(this.NodeViewer);
            this.Controls.Add(this.Textviewer);
            this.Controls.Add(this.ConvertToTable);
            this.Controls.Add(this.LoadasTree);
            this.Controls.Add(this.WebToString);
            this.Controls.Add(this.URLadress);
            this.Controls.Add(this.ISOTable);
            this.Controls.Add(this.NodeForm);
            this.Controls.Add(this.WebtoText);
            this.Name = "Form1";
            this.Text = "Parse ISO9999 from WebPage";
            ((System.ComponentModel.ISupportInitialize)(this.ISOTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox WebtoText;
        private System.Windows.Forms.TreeView NodeForm;
        private System.Windows.Forms.DataGridView ISOTable;
        private System.Windows.Forms.TextBox URLadress;
        private System.Windows.Forms.Button WebToString;
        private System.Windows.Forms.Button LoadasTree;
        private System.Windows.Forms.Button ConvertToTable;
        private System.Windows.Forms.Button Textviewer;
        private System.Windows.Forms.Button NodeViewer;
        private System.Windows.Forms.Button Tableviewer;
        private System.Windows.Forms.WebBrowser ISOBrowser;
        private System.Windows.Forms.Button WebView;
    }
}

