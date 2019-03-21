namespace CloverPro
{
    partial class wfRepGraficaRPO
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.rbtAmbos = new System.Windows.Forms.RadioButton();
            this.rbtTuno2 = new System.Windows.Forms.RadioButton();
            this.rbtTuno1 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbbPlanta = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtCantidad = new System.Windows.Forms.RadioButton();
            this.rbtRPO = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbtEnEsperaDet = new System.Windows.Forms.RadioButton();
            this.rbtArmaNoArma = new System.Windows.Forms.RadioButton();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
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
            this.toolStrip1.Size = new System.Drawing.Size(491, 25);
            this.toolStrip1.TabIndex = 1;
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
            this.btnNew.Visible = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpFecha);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 53);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fecha";
            // 
            // dtpFecha
            // 
            this.dtpFecha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFecha.CustomFormat = "dd/MM/yyyy";
            this.dtpFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFecha.Location = new System.Drawing.Point(37, 19);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(152, 20);
            this.dtpFecha.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.rbtAmbos);
            this.groupBox7.Controls.Add(this.rbtTuno2);
            this.groupBox7.Controls.Add(this.rbtTuno1);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.Location = new System.Drawing.Point(253, 40);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(216, 53);
            this.groupBox7.TabIndex = 5;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Turno de la programación";
            // 
            // rbtAmbos
            // 
            this.rbtAmbos.AutoSize = true;
            this.rbtAmbos.Location = new System.Drawing.Point(134, 19);
            this.rbtAmbos.Name = "rbtAmbos";
            this.rbtAmbos.Size = new System.Drawing.Size(62, 17);
            this.rbtAmbos.TabIndex = 5;
            this.rbtAmbos.Text = "Ambos";
            this.rbtAmbos.UseVisualStyleBackColor = true;
            // 
            // rbtTuno2
            // 
            this.rbtTuno2.AutoSize = true;
            this.rbtTuno2.Location = new System.Drawing.Point(84, 19);
            this.rbtTuno2.Name = "rbtTuno2";
            this.rbtTuno2.Size = new System.Drawing.Size(32, 17);
            this.rbtTuno2.TabIndex = 4;
            this.rbtTuno2.Text = "2";
            this.rbtTuno2.UseVisualStyleBackColor = true;
            // 
            // rbtTuno1
            // 
            this.rbtTuno1.AutoSize = true;
            this.rbtTuno1.Checked = true;
            this.rbtTuno1.Location = new System.Drawing.Point(31, 19);
            this.rbtTuno1.Name = "rbtTuno1";
            this.rbtTuno1.Size = new System.Drawing.Size(32, 17);
            this.rbtTuno1.TabIndex = 3;
            this.rbtTuno1.TabStop = true;
            this.rbtTuno1.Text = "1";
            this.rbtTuno1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbbPlanta);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 108);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(220, 53);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Planta";
            // 
            // cbbPlanta
            // 
            this.cbbPlanta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbPlanta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbPlanta.FormattingEnabled = true;
            this.cbbPlanta.Location = new System.Drawing.Point(22, 19);
            this.cbbPlanta.Name = "cbbPlanta";
            this.cbbPlanta.Size = new System.Drawing.Size(151, 21);
            this.cbbPlanta.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 275);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(491, 22);
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtCantidad);
            this.groupBox2.Controls.Add(this.rbtRPO);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(253, 108);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(216, 53);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tipo de medición";
            // 
            // rbtCantidad
            // 
            this.rbtCantidad.AutoSize = true;
            this.rbtCantidad.Location = new System.Drawing.Point(97, 23);
            this.rbtCantidad.Name = "rbtCantidad";
            this.rbtCantidad.Size = new System.Drawing.Size(75, 17);
            this.rbtCantidad.TabIndex = 7;
            this.rbtCantidad.Text = "Cantidad";
            this.rbtCantidad.UseVisualStyleBackColor = true;
            // 
            // rbtRPO
            // 
            this.rbtRPO.AutoSize = true;
            this.rbtRPO.Checked = true;
            this.rbtRPO.Location = new System.Drawing.Point(17, 23);
            this.rbtRPO.Name = "rbtRPO";
            this.rbtRPO.Size = new System.Drawing.Size(60, 17);
            this.rbtRPO.TabIndex = 6;
            this.rbtRPO.TabStop = true;
            this.rbtRPO.Text = "RPO\'s";
            this.rbtRPO.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbtEnEsperaDet);
            this.groupBox4.Controls.Add(this.rbtArmaNoArma);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(0, 167);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(469, 81);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Tipo de gráfica";
            // 
            // rbtEnEsperaDet
            // 
            this.rbtEnEsperaDet.AutoSize = true;
            this.rbtEnEsperaDet.Location = new System.Drawing.Point(253, 30);
            this.rbtEnEsperaDet.Name = "rbtEnEsperaDet";
            this.rbtEnEsperaDet.Size = new System.Drawing.Size(183, 17);
            this.rbtEnEsperaDet.TabIndex = 4;
            this.rbtEnEsperaDet.Text = "EN ESPERA Y DETENIDOS";
            this.rbtEnEsperaDet.UseVisualStyleBackColor = true;
            // 
            // rbtArmaNoArma
            // 
            this.rbtArmaNoArma.AutoSize = true;
            this.rbtArmaNoArma.Checked = true;
            this.rbtArmaNoArma.Location = new System.Drawing.Point(34, 30);
            this.rbtArmaNoArma.Name = "rbtArmaNoArma";
            this.rbtArmaNoArma.Size = new System.Drawing.Size(194, 17);
            this.rbtArmaNoArma.TabIndex = 3;
            this.rbtArmaNoArma.TabStop = true;
            this.rbtArmaNoArma.Text = "ARMANDOS Y NO ARMADOS";
            this.rbtArmaNoArma.UseVisualStyleBackColor = true;
            // 
            // wfRepGraficaRPO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 297);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "wfRepGraficaRPO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GraficaRPO";
            this.Load += new System.EventHandler(this.wfRepGraficaRPO_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton rbtTuno2;
        private System.Windows.Forms.RadioButton rbtTuno1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ComboBox cbbPlanta;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.RadioButton rbtAmbos;
        private System.Windows.Forms.RadioButton rbtCantidad;
        private System.Windows.Forms.RadioButton rbtRPO;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbtEnEsperaDet;
        private System.Windows.Forms.RadioButton rbtArmaNoArma;
    }
}