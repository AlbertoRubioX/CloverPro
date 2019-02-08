namespace CloverPro
{
    partial class wfRepSupline
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
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btExit = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.chbTurno2 = new System.Windows.Forms.CheckBox();
            this.chbTurno1 = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtLineas = new System.Windows.Forms.TextBox();
            this.cbbLineaFin = new System.Windows.Forms.ComboBox();
            this.cbbLineaIni = new System.Windows.Forms.ComboBox();
            this.lblLFin = new System.Windows.Forms.Label();
            this.lblLIni = new System.Windows.Forms.Label();
            this.rbtLineaA = new System.Windows.Forms.RadioButton();
            this.rbtLineaU = new System.Windows.Forms.RadioButton();
            this.rbtLineaT = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbbSupervisor = new System.Windows.Forms.ComboBox();
            this.rbtSupA = new System.Windows.Forms.RadioButton();
            this.rbtSupT = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtPlantaU = new System.Windows.Forms.RadioButton();
            this.rbtPlantaT = new System.Windows.Forms.RadioButton();
            this.cbbPlanta = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnPrint,
            this.toolStripSeparator1,
            this.btExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(504, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.Image = global::CloverPro.Properties.Resources.New;
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(23, 22);
            this.btnNew.Text = "Nuevo";
            this.btnNew.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrint.Image = global::CloverPro.Properties.Resources.print_preview;
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(23, 22);
            this.btnPrint.Tag = "Vista Previa";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
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
            this.panel1.Controls.Add(this.groupBox7);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 304);
            this.panel1.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.chbTurno2);
            this.groupBox7.Controls.Add(this.chbTurno1);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.Location = new System.Drawing.Point(358, 18);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(100, 82);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Turno";
            // 
            // chbTurno2
            // 
            this.chbTurno2.AutoSize = true;
            this.chbTurno2.Location = new System.Drawing.Point(37, 52);
            this.chbTurno2.Name = "chbTurno2";
            this.chbTurno2.Size = new System.Drawing.Size(33, 17);
            this.chbTurno2.TabIndex = 1;
            this.chbTurno2.Text = "2";
            this.chbTurno2.UseVisualStyleBackColor = true;
            // 
            // chbTurno1
            // 
            this.chbTurno1.AutoSize = true;
            this.chbTurno1.Checked = true;
            this.chbTurno1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbTurno1.Location = new System.Drawing.Point(37, 29);
            this.chbTurno1.Name = "chbTurno1";
            this.chbTurno1.Size = new System.Drawing.Size(33, 17);
            this.chbTurno1.TabIndex = 0;
            this.chbTurno1.Text = "1";
            this.chbTurno1.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtLineas);
            this.groupBox6.Controls.Add(this.cbbLineaFin);
            this.groupBox6.Controls.Add(this.cbbLineaIni);
            this.groupBox6.Controls.Add(this.lblLFin);
            this.groupBox6.Controls.Add(this.lblLIni);
            this.groupBox6.Controls.Add(this.rbtLineaA);
            this.groupBox6.Controls.Add(this.rbtLineaU);
            this.groupBox6.Controls.Add(this.rbtLineaT);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(21, 106);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(437, 85);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Lineas";
            // 
            // txtLineas
            // 
            this.txtLineas.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLineas.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLineas.Location = new System.Drawing.Point(115, 50);
            this.txtLineas.MaxLength = 100;
            this.txtLineas.Name = "txtLineas";
            this.txtLineas.Size = new System.Drawing.Size(293, 25);
            this.txtLineas.TabIndex = 2;
            this.txtLineas.Visible = false;
            // 
            // cbbLineaFin
            // 
            this.cbbLineaFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbLineaFin.FormattingEnabled = true;
            this.cbbLineaFin.Location = new System.Drawing.Point(337, 50);
            this.cbbLineaFin.Name = "cbbLineaFin";
            this.cbbLineaFin.Size = new System.Drawing.Size(70, 21);
            this.cbbLineaFin.TabIndex = 4;
            // 
            // cbbLineaIni
            // 
            this.cbbLineaIni.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbLineaIni.FormattingEnabled = true;
            this.cbbLineaIni.Location = new System.Drawing.Point(115, 50);
            this.cbbLineaIni.Name = "cbbLineaIni";
            this.cbbLineaIni.Size = new System.Drawing.Size(66, 21);
            this.cbbLineaIni.TabIndex = 3;
            // 
            // lblLFin
            // 
            this.lblLFin.AutoSize = true;
            this.lblLFin.Location = new System.Drawing.Point(255, 53);
            this.lblLFin.Name = "lblLFin";
            this.lblLFin.Size = new System.Drawing.Size(73, 13);
            this.lblLFin.TabIndex = 6;
            this.lblLFin.Text = "Linea Final:";
            // 
            // lblLIni
            // 
            this.lblLIni.AutoSize = true;
            this.lblLIni.Location = new System.Drawing.Point(29, 53);
            this.lblLIni.Name = "lblLIni";
            this.lblLIni.Size = new System.Drawing.Size(48, 13);
            this.lblLIni.TabIndex = 4;
            this.lblLIni.Text = "Lineas:";
            this.lblLIni.Visible = false;
            // 
            // rbtLineaA
            // 
            this.rbtLineaA.AutoSize = true;
            this.rbtLineaA.Location = new System.Drawing.Point(337, 19);
            this.rbtLineaA.Name = "rbtLineaA";
            this.rbtLineaA.Size = new System.Drawing.Size(70, 17);
            this.rbtLineaA.TabIndex = 2;
            this.rbtLineaA.Text = "Algunas";
            this.rbtLineaA.UseVisualStyleBackColor = true;
            this.rbtLineaA.CheckedChanged += new System.EventHandler(this.rbtLineaA_CheckedChanged);
            // 
            // rbtLineaU
            // 
            this.rbtLineaU.AutoSize = true;
            this.rbtLineaU.Location = new System.Drawing.Point(204, 19);
            this.rbtLineaU.Name = "rbtLineaU";
            this.rbtLineaU.Size = new System.Drawing.Size(48, 17);
            this.rbtLineaU.TabIndex = 1;
            this.rbtLineaU.Text = "Una";
            this.rbtLineaU.UseVisualStyleBackColor = true;
            this.rbtLineaU.CheckedChanged += new System.EventHandler(this.rbtLineaU_CheckedChanged);
            // 
            // rbtLineaT
            // 
            this.rbtLineaT.AutoSize = true;
            this.rbtLineaT.Checked = true;
            this.rbtLineaT.Location = new System.Drawing.Point(40, 19);
            this.rbtLineaT.Name = "rbtLineaT";
            this.rbtLineaT.Size = new System.Drawing.Size(60, 17);
            this.rbtLineaT.TabIndex = 0;
            this.rbtLineaT.TabStop = true;
            this.rbtLineaT.Text = "Todas";
            this.rbtLineaT.UseVisualStyleBackColor = true;
            this.rbtLineaT.CheckedChanged += new System.EventHandler(this.rbtLineaT_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbbSupervisor);
            this.groupBox3.Controls.Add(this.rbtSupA);
            this.groupBox3.Controls.Add(this.rbtSupT);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(21, 197);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(437, 85);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Supervisor";
            // 
            // cbbSupervisor
            // 
            this.cbbSupervisor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbSupervisor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbSupervisor.FormattingEnabled = true;
            this.cbbSupervisor.Location = new System.Drawing.Point(21, 49);
            this.cbbSupervisor.Name = "cbbSupervisor";
            this.cbbSupervisor.Size = new System.Drawing.Size(292, 21);
            this.cbbSupervisor.TabIndex = 3;
            // 
            // rbtSupA
            // 
            this.rbtSupA.AutoSize = true;
            this.rbtSupA.Location = new System.Drawing.Point(204, 19);
            this.rbtSupA.Name = "rbtSupA";
            this.rbtSupA.Size = new System.Drawing.Size(48, 17);
            this.rbtSupA.TabIndex = 1;
            this.rbtSupA.Text = "Uno";
            this.rbtSupA.UseVisualStyleBackColor = true;
            this.rbtSupA.CheckedChanged += new System.EventHandler(this.rbtSupA_CheckedChanged);
            // 
            // rbtSupT
            // 
            this.rbtSupT.AutoSize = true;
            this.rbtSupT.Checked = true;
            this.rbtSupT.Location = new System.Drawing.Point(40, 19);
            this.rbtSupT.Name = "rbtSupT";
            this.rbtSupT.Size = new System.Drawing.Size(60, 17);
            this.rbtSupT.TabIndex = 0;
            this.rbtSupT.TabStop = true;
            this.rbtSupT.Text = "Todos";
            this.rbtSupT.UseVisualStyleBackColor = true;
            this.rbtSupT.CheckedChanged += new System.EventHandler(this.rbtSupT_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtPlantaU);
            this.groupBox2.Controls.Add(this.rbtPlantaT);
            this.groupBox2.Controls.Add(this.cbbPlanta);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(21, 18);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(331, 82);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Plantas";
            // 
            // rbtPlantaU
            // 
            this.rbtPlantaU.AutoSize = true;
            this.rbtPlantaU.Location = new System.Drawing.Point(204, 19);
            this.rbtPlantaU.Name = "rbtPlantaU";
            this.rbtPlantaU.Size = new System.Drawing.Size(48, 17);
            this.rbtPlantaU.TabIndex = 1;
            this.rbtPlantaU.Text = "Una";
            this.rbtPlantaU.UseVisualStyleBackColor = true;
            this.rbtPlantaU.CheckedChanged += new System.EventHandler(this.rbtPlantaU_CheckedChanged);
            // 
            // rbtPlantaT
            // 
            this.rbtPlantaT.AutoSize = true;
            this.rbtPlantaT.Checked = true;
            this.rbtPlantaT.Location = new System.Drawing.Point(40, 19);
            this.rbtPlantaT.Name = "rbtPlantaT";
            this.rbtPlantaT.Size = new System.Drawing.Size(60, 17);
            this.rbtPlantaT.TabIndex = 0;
            this.rbtPlantaT.TabStop = true;
            this.rbtPlantaT.Text = "Todas";
            this.rbtPlantaT.UseVisualStyleBackColor = true;
            this.rbtPlantaT.CheckedChanged += new System.EventHandler(this.rbtPlantaT_CheckedChanged);
            // 
            // cbbPlanta
            // 
            this.cbbPlanta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbPlanta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbPlanta.FormattingEnabled = true;
            this.cbbPlanta.Location = new System.Drawing.Point(21, 48);
            this.cbbPlanta.Name = "cbbPlanta";
            this.cbbPlanta.Size = new System.Drawing.Size(292, 21);
            this.cbbPlanta.TabIndex = 2;
            this.cbbPlanta.SelectionChangeCommitted += new System.EventHandler(this.cbbPlanta_SelectionChangeCommitted);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 340);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(504, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(125, 17);
            this.toolStripStatusLabel1.Text = "Presione ESC para Salir";
            // 
            // wfRepSupline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 362);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "wfRepSupline";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte de Lineas por Supervisor";
            this.Activated += new System.EventHandler(this.wfRepEtiquetas_Activated);
            this.Load += new System.EventHandler(this.wfRepEtiquetas_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbbPlanta;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtPlantaU;
        private System.Windows.Forms.RadioButton rbtPlantaT;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox cbbLineaFin;
        private System.Windows.Forms.ComboBox cbbLineaIni;
        private System.Windows.Forms.Label lblLFin;
        private System.Windows.Forms.Label lblLIni;
        private System.Windows.Forms.RadioButton rbtLineaA;
        private System.Windows.Forms.RadioButton rbtLineaU;
        private System.Windows.Forms.RadioButton rbtLineaT;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox txtLineas;
        private System.Windows.Forms.CheckBox chbTurno2;
        private System.Windows.Forms.CheckBox chbTurno1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbbSupervisor;
        private System.Windows.Forms.RadioButton rbtSupA;
        private System.Windows.Forms.RadioButton rbtSupT;
    }
}