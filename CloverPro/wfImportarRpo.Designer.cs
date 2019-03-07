namespace CloverPro
{
    partial class wfImportarRpo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btExit = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chbParcial = new System.Windows.Forms.CheckBox();
            this.lblCant = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbbOrigen = new System.Windows.Forms.ComboBox();
            this.lblOrigen = new System.Windows.Forms.Label();
            this.cbbPlanta = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chbEdit = new System.Windows.Forms.CheckBox();
            this.cbbCatalogo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgwRpo = new System.Windows.Forms.DataGridView();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.txtArchivo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwRpo)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.btExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(593, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btExit
            // 
            this.btExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btExit.Image = global::CloverPro.Properties.Resources.Exit;
            this.btExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(23, 22);
            this.btExit.Text = "Salir";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chbParcial);
            this.panel1.Controls.Add(this.lblCant);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cbbOrigen);
            this.panel1.Controls.Add(this.lblOrigen);
            this.panel1.Controls.Add(this.cbbPlanta);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.chbEdit);
            this.panel1.Controls.Add(this.cbbCatalogo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dgwRpo);
            this.panel1.Controls.Add(this.btnImportar);
            this.panel1.Controls.Add(this.btnFile);
            this.panel1.Controls.Add(this.txtArchivo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panel1.Size = new System.Drawing.Size(569, 535);
            this.panel1.TabIndex = 0;
            // 
            // chbParcial
            // 
            this.chbParcial.AutoSize = true;
            this.chbParcial.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbParcial.Location = new System.Drawing.Point(336, 68);
            this.chbParcial.Name = "chbParcial";
            this.chbParcial.Size = new System.Drawing.Size(169, 17);
            this.chbParcial.TabIndex = 3;
            this.chbParcial.Text = "Subir como Carga Parcial";
            this.chbParcial.UseVisualStyleBackColor = true;
            this.chbParcial.CheckedChanged += new System.EventHandler(this.chbParcial_CheckedChanged);
            // 
            // lblCant
            // 
            this.lblCant.AutoSize = true;
            this.lblCant.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCant.ForeColor = System.Drawing.Color.Green;
            this.lblCant.Location = new System.Drawing.Point(172, 130);
            this.lblCant.Name = "lblCant";
            this.lblCant.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblCant.Size = new System.Drawing.Size(20, 23);
            this.lblCant.TabIndex = 10;
            this.lblCant.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.SteelBlue;
            this.label4.Location = new System.Drawing.Point(14, 130);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(161, 22);
            this.label4.TabIndex = 9;
            this.label4.Text = "Registros Cargados: ";
            // 
            // cbbOrigen
            // 
            this.cbbOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbOrigen.FormattingEnabled = true;
            this.cbbOrigen.Location = new System.Drawing.Point(123, 64);
            this.cbbOrigen.Name = "cbbOrigen";
            this.cbbOrigen.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbbOrigen.Size = new System.Drawing.Size(121, 21);
            this.cbbOrigen.TabIndex = 2;
            this.cbbOrigen.Visible = false;
            // 
            // lblOrigen
            // 
            this.lblOrigen.AutoSize = true;
            this.lblOrigen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrigen.Location = new System.Drawing.Point(65, 67);
            this.lblOrigen.Name = "lblOrigen";
            this.lblOrigen.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblOrigen.Size = new System.Drawing.Size(52, 13);
            this.lblOrigen.TabIndex = 11;
            this.lblOrigen.Text = "Origen :";
            this.lblOrigen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblOrigen.Visible = false;
            // 
            // cbbPlanta
            // 
            this.cbbPlanta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbPlanta.FormattingEnabled = true;
            this.cbbPlanta.Location = new System.Drawing.Point(347, 31);
            this.cbbPlanta.Name = "cbbPlanta";
            this.cbbPlanta.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbbPlanta.Size = new System.Drawing.Size(158, 21);
            this.cbbPlanta.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(290, 35);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Planta :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chbEdit
            // 
            this.chbEdit.AutoSize = true;
            this.chbEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbEdit.Location = new System.Drawing.Point(308, 134);
            this.chbEdit.Name = "chbEdit";
            this.chbEdit.Size = new System.Drawing.Size(197, 17);
            this.chbEdit.TabIndex = 7;
            this.chbEdit.Text = "Modificar Registros Existentes";
            this.chbEdit.UseVisualStyleBackColor = true;
            // 
            // cbbCatalogo
            // 
            this.cbbCatalogo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbCatalogo.FormattingEnabled = true;
            this.cbbCatalogo.Location = new System.Drawing.Point(123, 30);
            this.cbbCatalogo.Name = "cbbCatalogo";
            this.cbbCatalogo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbbCatalogo.Size = new System.Drawing.Size(121, 21);
            this.cbbCatalogo.TabIndex = 0;
            this.cbbCatalogo.SelectedIndexChanged += new System.EventHandler(this.cbbCatalogo_SelectedIndexChanged);
            this.cbbCatalogo.SelectionChangeCommitted += new System.EventHandler(this.cbbCatalogo_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(48, 33);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Catálogo :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgwRpo
            // 
            this.dgwRpo.AllowUserToAddRows = false;
            this.dgwRpo.AllowUserToDeleteRows = false;
            this.dgwRpo.AllowUserToResizeRows = false;
            this.dgwRpo.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgwRpo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgwRpo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgwRpo.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgwRpo.Location = new System.Drawing.Point(18, 157);
            this.dgwRpo.MultiSelect = false;
            this.dgwRpo.Name = "dgwRpo";
            this.dgwRpo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgwRpo.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgwRpo.RowHeadersVisible = false;
            this.dgwRpo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgwRpo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.SteelBlue;
            this.dgwRpo.RowTemplate.Height = 18;
            this.dgwRpo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwRpo.Size = new System.Drawing.Size(535, 317);
            this.dgwRpo.TabIndex = 9;
            // 
            // btnImportar
            // 
            this.btnImportar.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportar.Location = new System.Drawing.Point(150, 480);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(269, 41);
            this.btnImportar.TabIndex = 8;
            this.btnImportar.Text = "Importar Base de Datos";
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // btnFile
            // 
            this.btnFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFile.Location = new System.Drawing.Point(511, 103);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(27, 20);
            this.btnFile.TabIndex = 6;
            this.btnFile.Text = "...";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // txtArchivo
            // 
            this.txtArchivo.BackColor = System.Drawing.Color.White;
            this.txtArchivo.Location = new System.Drawing.Point(123, 103);
            this.txtArchivo.Name = "txtArchivo";
            this.txtArchivo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtArchivo.Size = new System.Drawing.Size(382, 20);
            this.txtArchivo.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 106);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Archivo Excel:";
            // 
            // wfImportarRpo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 575);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "wfImportarRpo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Importar Catálogo";
            this.Activated += new System.EventHandler(this.wfImportarRpo_Activated);
            this.Load += new System.EventHandler(this.wfImportarRpo_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwRpo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.TextBox txtArchivo;
        private System.Windows.Forms.DataGridView dgwRpo;
        private System.Windows.Forms.CheckBox chbEdit;
        private System.Windows.Forms.ComboBox cbbCatalogo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbbPlanta;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbbOrigen;
        private System.Windows.Forms.Label lblOrigen;
        private System.Windows.Forms.Label lblCant;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chbParcial;
    }
}