﻿namespace CloverPro
{
    partial class wfSuperLinea
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btSave = new System.Windows.Forms.ToolStripButton();
            this.btNew = new System.Windows.Forms.ToolStripButton();
            this.btRemove = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btExit = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbbArea = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbbTurno = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbbPlanta = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLineas = new System.Windows.Forms.Button();
            this.dgwLineas = new System.Windows.Forms.DataGridView();
            this.cbbSuper = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwLineas)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btSave,
            this.btNew,
            this.btRemove,
            this.btnDelete,
            this.toolStripSeparator1,
            this.btExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(513, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btSave
            // 
            this.btSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSave.Image = global::CloverPro.Properties.Resources.Save;
            this.btSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(23, 22);
            this.btSave.Text = "Guardar";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btNew
            // 
            this.btNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btNew.Image = global::CloverPro.Properties.Resources.New;
            this.btNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btNew.Name = "btNew";
            this.btNew.Size = new System.Drawing.Size(23, 22);
            this.btNew.Text = "Nuevo";
            this.btNew.Click += new System.EventHandler(this.btNew_Click);
            // 
            // btRemove
            // 
            this.btRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btRemove.Image = global::CloverPro.Properties.Resources.remove_line;
            this.btRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(23, 22);
            this.btRemove.Text = "Borrar";
            this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = global::CloverPro.Properties.Resources.Trash;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.Text = "Borrar";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
            this.panel1.Controls.Add(this.cbbArea);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cbbTurno);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbbPlanta);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.cbbSuper);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(489, 455);
            this.panel1.TabIndex = 0;
            // 
            // cbbArea
            // 
            this.cbbArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbArea.DropDownWidth = 150;
            this.cbbArea.FormattingEnabled = true;
            this.cbbArea.Location = new System.Drawing.Point(102, 58);
            this.cbbArea.Name = "cbbArea";
            this.cbbArea.Size = new System.Drawing.Size(159, 21);
            this.cbbArea.TabIndex = 7;
            this.cbbArea.SelectionChangeCommitted += new System.EventHandler(this.cbbArea_SelectionChangeCommitted);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(59, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Area:";
            // 
            // cbbTurno
            // 
            this.cbbTurno.DropDownHeight = 100;
            this.cbbTurno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbTurno.DropDownWidth = 50;
            this.cbbTurno.FormattingEnabled = true;
            this.cbbTurno.IntegralHeight = false;
            this.cbbTurno.Location = new System.Drawing.Point(366, 31);
            this.cbbTurno.Name = "cbbTurno";
            this.cbbTurno.Size = new System.Drawing.Size(62, 21);
            this.cbbTurno.TabIndex = 5;
            this.cbbTurno.SelectionChangeCommitted += new System.EventHandler(this.cbbTurno_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(313, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Turno:";
            // 
            // cbbPlanta
            // 
            this.cbbPlanta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbPlanta.DropDownWidth = 150;
            this.cbbPlanta.FormattingEnabled = true;
            this.cbbPlanta.Location = new System.Drawing.Point(102, 31);
            this.cbbPlanta.Name = "cbbPlanta";
            this.cbbPlanta.Size = new System.Drawing.Size(159, 21);
            this.cbbPlanta.TabIndex = 3;
            this.cbbPlanta.SelectedIndexChanged += new System.EventHandler(this.cbbPlanta_SelectedIndexChanged);
            this.cbbPlanta.SelectionChangeCommitted += new System.EventHandler(this.cbbPlanta_SelectionChangeCommitted);
            this.cbbPlanta.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbbPlanta_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(49, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Planta:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLineas);
            this.groupBox1.Controls.Add(this.dgwLineas);
            this.groupBox1.Location = new System.Drawing.Point(28, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(426, 325);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lineas Asignadas";
            // 
            // btnLineas
            // 
            this.btnLineas.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLineas.Location = new System.Drawing.Point(134, 276);
            this.btnLineas.Name = "btnLineas";
            this.btnLineas.Size = new System.Drawing.Size(166, 30);
            this.btnLineas.TabIndex = 1;
            this.btnLineas.Text = "Asignar Lineas";
            this.btnLineas.UseVisualStyleBackColor = true;
            this.btnLineas.Click += new System.EventHandler(this.btnLineas_Click);
            // 
            // dgwLineas
            // 
            this.dgwLineas.AllowUserToAddRows = false;
            this.dgwLineas.AllowUserToDeleteRows = false;
            this.dgwLineas.AllowUserToResizeColumns = false;
            this.dgwLineas.AllowUserToResizeRows = false;
            this.dgwLineas.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgwLineas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwLineas.Location = new System.Drawing.Point(24, 19);
            this.dgwLineas.Name = "dgwLineas";
            this.dgwLineas.RowHeadersWidth = 25;
            this.dgwLineas.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgwLineas.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgwLineas.RowTemplate.Height = 20;
            this.dgwLineas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwLineas.Size = new System.Drawing.Size(376, 242);
            this.dgwLineas.TabIndex = 0;
            this.dgwLineas.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgwLineas_CellFormatting);
            // 
            // cbbSuper
            // 
            this.cbbSuper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbSuper.DropDownWidth = 150;
            this.cbbSuper.FormattingEnabled = true;
            this.cbbSuper.Location = new System.Drawing.Point(102, 85);
            this.cbbSuper.Name = "cbbSuper";
            this.cbbSuper.Size = new System.Drawing.Size(326, 21);
            this.cbbSuper.TabIndex = 0;
            this.cbbSuper.SelectionChangeCommitted += new System.EventHandler(this.cbbEstacion_SelectionChangeCommitted);
            this.cbbSuper.Enter += new System.EventHandler(this.cbbSuper_Enter);
            this.cbbSuper.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbbEstacion_KeyDown);
            this.cbbSuper.Leave += new System.EventHandler(this.cbbSuper_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Supervisor:";
            // 
            // wfSuperLinea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 496);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "wfSuperLinea";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Catálogo de Lineas por Supervisor";
            this.Activated += new System.EventHandler(this.wfSuperLinea_Activated);
            this.Load += new System.EventHandler(this.wfSuperLinea_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwLineas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btSave;
        private System.Windows.Forms.ToolStripButton btRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbSuper;
        private System.Windows.Forms.ToolStripButton btNew;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLineas;
        private System.Windows.Forms.DataGridView dgwLineas;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ComboBox cbbArea;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbbTurno;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbbPlanta;
        private System.Windows.Forms.Label label2;
    }
}