namespace CloverPro
{
    partial class wfRepEntregaDiario
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.rbtAmbos = new System.Windows.Forms.RadioButton();
            this.rbtTuno2 = new System.Windows.Forms.RadioButton();
            this.rbtTuno1 = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cbbLineaIni = new System.Windows.Forms.ComboBox();
            this.rbtLineaU = new System.Windows.Forms.RadioButton();
            this.rbtLineaT = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbbPlanta = new System.Windows.Forms.ComboBox();
            this.lblPlanta = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpInicio = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btExit = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 278);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(504, 22);
            this.statusStrip1.TabIndex = 6;
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
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox7);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 236);
            this.panel1.TabIndex = 4;
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
            this.cbbLineaIni.Location = new System.Drawing.Point(263, 18);
            this.cbbLineaIni.Name = "cbbLineaIni";
            this.cbbLineaIni.Size = new System.Drawing.Size(129, 21);
            this.cbbLineaIni.TabIndex = 3;
            // 
            // rbtLineaU
            // 
            this.rbtLineaU.AutoSize = true;
            this.rbtLineaU.Location = new System.Drawing.Point(167, 22);
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
            this.rbtLineaT.Location = new System.Drawing.Point(27, 22);
            this.rbtLineaT.Name = "rbtLineaT";
            this.rbtLineaT.Size = new System.Drawing.Size(60, 17);
            this.rbtLineaT.TabIndex = 0;
            this.rbtLineaT.TabStop = true;
            this.rbtLineaT.Text = "Todas";
            this.rbtLineaT.UseVisualStyleBackColor = true;
            this.rbtLineaT.CheckedChanged += new System.EventHandler(this.rbtLineaT_CheckedChanged);
            // 
            // groupBox2
            // 
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
            // cbbPlanta
            // 
            this.cbbPlanta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbPlanta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbPlanta.FormattingEnabled = true;
            this.cbbPlanta.Location = new System.Drawing.Point(77, 36);
            this.cbbPlanta.Name = "cbbPlanta";
            this.cbbPlanta.Size = new System.Drawing.Size(217, 21);
            this.cbbPlanta.TabIndex = 2;
            this.cbbPlanta.SelectionChangeCommitted += new System.EventHandler(this.cbbPlanta_SelectionChangeCommitted);
            // 
            // lblPlanta
            // 
            this.lblPlanta.AutoSize = true;
            this.lblPlanta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlanta.Location = new System.Drawing.Point(24, 36);
            this.lblPlanta.Name = "lblPlanta";
            this.lblPlanta.Size = new System.Drawing.Size(47, 13);
            this.lblPlanta.TabIndex = 0;
            this.lblPlanta.Text = "Planta:";
            // 
            // groupBox1
            // 
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
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fecha:";
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
            this.toolStrip1.TabIndex = 5;
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
            // wfRepEntregaDiario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 300);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "wfRepEntregaDiario";
            this.Text = "wfRepEntregaDiario";
            this.Load += new System.EventHandler(this.wfRepEntregaDiario_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton rbtAmbos;
        private System.Windows.Forms.RadioButton rbtTuno2;
        private System.Windows.Forms.RadioButton rbtTuno1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox cbbLineaIni;
        private System.Windows.Forms.RadioButton rbtLineaU;
        private System.Windows.Forms.RadioButton rbtLineaT;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbbPlanta;
        private System.Windows.Forms.Label lblPlanta;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpInicio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btExit;
    }
}