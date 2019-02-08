namespace CloverPro
{
    partial class wfRepEmpaqueProc
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbbEstatus = new System.Windows.Forms.ComboBox();
            this.chbEstatus = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.rbtAmbos = new System.Windows.Forms.RadioButton();
            this.rbtTuno2 = new System.Windows.Forms.RadioButton();
            this.rbtTuno1 = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cbbLineaIni = new System.Windows.Forms.ComboBox();
            this.rbtLineaU = new System.Windows.Forms.RadioButton();
            this.rbtLineaT = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtRPO = new System.Windows.Forms.TextBox();
            this.chbRPO = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtPlantaU = new System.Windows.Forms.RadioButton();
            this.rbtPlantaT = new System.Windows.Forms.RadioButton();
            this.cbbPlanta = new System.Windows.Forms.ComboBox();
            this.lblPlanta = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpFinal = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpInicio = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.chbPrio = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.panel1.Controls.Add(this.chbPrio);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox7);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 338);
            this.panel1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbbEstatus);
            this.groupBox3.Controls.Add(this.chbEstatus);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(243, 232);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(215, 59);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            // 
            // cbbEstatus
            // 
            this.cbbEstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbEstatus.Enabled = false;
            this.cbbEstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbEstatus.FormattingEnabled = true;
            this.cbbEstatus.Location = new System.Drawing.Point(11, 27);
            this.cbbEstatus.Name = "cbbEstatus";
            this.cbbEstatus.Size = new System.Drawing.Size(177, 21);
            this.cbbEstatus.TabIndex = 4;
            // 
            // chbEstatus
            // 
            this.chbEstatus.AutoSize = true;
            this.chbEstatus.Location = new System.Drawing.Point(11, 0);
            this.chbEstatus.Name = "chbEstatus";
            this.chbEstatus.Size = new System.Drawing.Size(68, 17);
            this.chbEstatus.TabIndex = 0;
            this.chbEstatus.Text = "Estatus";
            this.chbEstatus.UseVisualStyleBackColor = true;
            this.chbEstatus.CheckedChanged += new System.EventHandler(this.chbEstatus_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.rbtAmbos);
            this.groupBox7.Controls.Add(this.rbtTuno2);
            this.groupBox7.Controls.Add(this.rbtTuno1);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.Location = new System.Drawing.Point(365, 74);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(93, 82);
            this.groupBox7.TabIndex = 4;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Turno";
            // 
            // rbtAmbos
            // 
            this.rbtAmbos.AutoSize = true;
            this.rbtAmbos.Location = new System.Drawing.Point(16, 47);
            this.rbtAmbos.Name = "rbtAmbos";
            this.rbtAmbos.Size = new System.Drawing.Size(62, 17);
            this.rbtAmbos.TabIndex = 2;
            this.rbtAmbos.Text = "Ambos";
            this.rbtAmbos.UseVisualStyleBackColor = true;
            this.rbtAmbos.CheckedChanged += new System.EventHandler(this.rbtAmbos_CheckedChanged);
            // 
            // rbtTuno2
            // 
            this.rbtTuno2.AutoSize = true;
            this.rbtTuno2.Location = new System.Drawing.Point(54, 19);
            this.rbtTuno2.Name = "rbtTuno2";
            this.rbtTuno2.Size = new System.Drawing.Size(32, 17);
            this.rbtTuno2.TabIndex = 1;
            this.rbtTuno2.Text = "2";
            this.rbtTuno2.UseVisualStyleBackColor = true;
            this.rbtTuno2.CheckedChanged += new System.EventHandler(this.rbtTuno2_CheckedChanged);
            // 
            // rbtTuno1
            // 
            this.rbtTuno1.AutoSize = true;
            this.rbtTuno1.Checked = true;
            this.rbtTuno1.Location = new System.Drawing.Point(16, 19);
            this.rbtTuno1.Name = "rbtTuno1";
            this.rbtTuno1.Size = new System.Drawing.Size(32, 17);
            this.rbtTuno1.TabIndex = 0;
            this.rbtTuno1.TabStop = true;
            this.rbtTuno1.Text = "1";
            this.rbtTuno1.UseVisualStyleBackColor = true;
            this.rbtTuno1.CheckedChanged += new System.EventHandler(this.rbtTuno1_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cbbLineaIni);
            this.groupBox6.Controls.Add(this.rbtLineaU);
            this.groupBox6.Controls.Add(this.rbtLineaT);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(21, 162);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(437, 61);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Lineas";
            // 
            // cbbLineaIni
            // 
            this.cbbLineaIni.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbLineaIni.FormattingEnabled = true;
            this.cbbLineaIni.Location = new System.Drawing.Point(278, 18);
            this.cbbLineaIni.Name = "cbbLineaIni";
            this.cbbLineaIni.Size = new System.Drawing.Size(129, 21);
            this.cbbLineaIni.TabIndex = 3;
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
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtRPO);
            this.groupBox5.Controls.Add(this.chbRPO);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(21, 232);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(215, 59);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            // 
            // txtRPO
            // 
            this.txtRPO.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRPO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRPO.Location = new System.Drawing.Point(10, 28);
            this.txtRPO.Name = "txtRPO";
            this.txtRPO.Size = new System.Drawing.Size(175, 20);
            this.txtRPO.TabIndex = 1;
            this.txtRPO.Enter += new System.EventHandler(this.txtRPO_Enter);
            this.txtRPO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRPO_KeyDown);
            this.txtRPO.Leave += new System.EventHandler(this.txtRPO_Leave);
            // 
            // chbRPO
            // 
            this.chbRPO.AutoSize = true;
            this.chbRPO.Location = new System.Drawing.Point(11, 0);
            this.chbRPO.Name = "chbRPO";
            this.chbRPO.Size = new System.Drawing.Size(52, 17);
            this.chbRPO.TabIndex = 0;
            this.chbRPO.Text = "RPO";
            this.chbRPO.UseVisualStyleBackColor = true;
            this.chbRPO.CheckedChanged += new System.EventHandler(this.chbRPO_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtPlantaU);
            this.groupBox2.Controls.Add(this.rbtPlantaT);
            this.groupBox2.Controls.Add(this.cbbPlanta);
            this.groupBox2.Controls.Add(this.lblPlanta);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(21, 74);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(338, 82);
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
            this.cbbPlanta.Location = new System.Drawing.Point(115, 47);
            this.cbbPlanta.Name = "cbbPlanta";
            this.cbbPlanta.Size = new System.Drawing.Size(217, 21);
            this.cbbPlanta.TabIndex = 2;
            this.cbbPlanta.SelectionChangeCommitted += new System.EventHandler(this.cbbPlanta_SelectionChangeCommitted);
            // 
            // lblPlanta
            // 
            this.lblPlanta.AutoSize = true;
            this.lblPlanta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlanta.Location = new System.Drawing.Point(62, 50);
            this.lblPlanta.Name = "lblPlanta";
            this.lblPlanta.Size = new System.Drawing.Size(47, 13);
            this.lblPlanta.TabIndex = 0;
            this.lblPlanta.Text = "Planta:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpFinal);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtpInicio);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(21, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 53);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Periódo";
            // 
            // dtpFinal
            // 
            this.dtpFinal.CustomFormat = "dd/MM/yyyy";
            this.dtpFinal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFinal.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFinal.Location = new System.Drawing.Point(307, 18);
            this.dtpFinal.Name = "dtpFinal";
            this.dtpFinal.Size = new System.Drawing.Size(100, 20);
            this.dtpFinal.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(229, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Fecha Final: ";
            // 
            // dtpInicio
            // 
            this.dtpInicio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpInicio.CustomFormat = "dd/MM/yyyy";
            this.dtpInicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpInicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpInicio.Location = new System.Drawing.Point(115, 19);
            this.dtpInicio.Name = "dtpInicio";
            this.dtpInicio.Size = new System.Drawing.Size(100, 20);
            this.dtpInicio.TabIndex = 0;
            this.dtpInicio.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtpInicio_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fecha Inicio: ";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 379);
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
            // chbPrio
            // 
            this.chbPrio.AutoSize = true;
            this.chbPrio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbPrio.Location = new System.Drawing.Point(31, 306);
            this.chbPrio.Name = "chbPrio";
            this.chbPrio.Size = new System.Drawing.Size(122, 17);
            this.chbPrio.TabIndex = 5;
            this.chbPrio.Text = "Mostrar Prioridad";
            this.chbPrio.UseVisualStyleBackColor = true;
            // 
            // wfRepEmpaqueProc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 401);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "wfRepEmpaqueProc";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte de Armado de RPO\'s";
            this.Activated += new System.EventHandler(this.wfRepEtiquetas_Activated);
            this.Load += new System.EventHandler(this.wfRepEtiquetas_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.Label lblPlanta;
        private System.Windows.Forms.ComboBox cbbPlanta;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpFinal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpInicio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtRPO;
        private System.Windows.Forms.CheckBox chbRPO;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtPlantaU;
        private System.Windows.Forms.RadioButton rbtPlantaT;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox cbbLineaIni;
        private System.Windows.Forms.RadioButton rbtLineaU;
        private System.Windows.Forms.RadioButton rbtLineaT;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton rbtTuno2;
        private System.Windows.Forms.RadioButton rbtTuno1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbbEstatus;
        private System.Windows.Forms.CheckBox chbEstatus;
        private System.Windows.Forms.RadioButton rbtAmbos;
        private System.Windows.Forms.CheckBox chbPrio;
    }
}