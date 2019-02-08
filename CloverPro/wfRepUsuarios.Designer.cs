namespace CloverPro
{
    partial class wfRepUsuarios
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
            this.chbTurno = new System.Windows.Forms.CheckBox();
            this.rbtTuno2 = new System.Windows.Forms.RadioButton();
            this.rbtTuno1 = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chbUser = new System.Windows.Forms.CheckBox();
            this.txtEmpleado = new System.Windows.Forms.TextBox();
            this.rbtNombre = new System.Windows.Forms.RadioButton();
            this.rbtEmp = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chbArea = new System.Windows.Forms.CheckBox();
            this.cbbArea = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbPlanta = new System.Windows.Forms.CheckBox();
            this.cbbPlanta = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
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
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 203);
            this.panel1.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.chbTurno);
            this.groupBox7.Controls.Add(this.rbtTuno2);
            this.groupBox7.Controls.Add(this.rbtTuno1);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.Location = new System.Drawing.Point(317, 16);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(141, 65);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            // 
            // chbTurno
            // 
            this.chbTurno.AutoSize = true;
            this.chbTurno.Checked = true;
            this.chbTurno.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbTurno.Location = new System.Drawing.Point(18, 0);
            this.chbTurno.Name = "chbTurno";
            this.chbTurno.Size = new System.Drawing.Size(117, 17);
            this.chbTurno.TabIndex = 3;
            this.chbTurno.Text = "Filtrar por Turno";
            this.chbTurno.UseVisualStyleBackColor = true;
            this.chbTurno.CheckedChanged += new System.EventHandler(this.chbTurno_CheckedChanged);
            // 
            // rbtTuno2
            // 
            this.rbtTuno2.AutoSize = true;
            this.rbtTuno2.Location = new System.Drawing.Point(79, 29);
            this.rbtTuno2.Name = "rbtTuno2";
            this.rbtTuno2.Size = new System.Drawing.Size(32, 17);
            this.rbtTuno2.TabIndex = 4;
            this.rbtTuno2.Text = "2";
            this.rbtTuno2.UseVisualStyleBackColor = true;
            this.rbtTuno2.CheckedChanged += new System.EventHandler(this.rbtTuno2_CheckedChanged);
            // 
            // rbtTuno1
            // 
            this.rbtTuno1.AutoSize = true;
            this.rbtTuno1.Checked = true;
            this.rbtTuno1.Location = new System.Drawing.Point(25, 29);
            this.rbtTuno1.Name = "rbtTuno1";
            this.rbtTuno1.Size = new System.Drawing.Size(32, 17);
            this.rbtTuno1.TabIndex = 3;
            this.rbtTuno1.TabStop = true;
            this.rbtTuno1.Text = "1";
            this.rbtTuno1.UseVisualStyleBackColor = true;
            this.rbtTuno1.CheckedChanged += new System.EventHandler(this.rbtTuno1_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chbUser);
            this.groupBox6.Controls.Add(this.txtEmpleado);
            this.groupBox6.Controls.Add(this.rbtNombre);
            this.groupBox6.Controls.Add(this.rbtEmp);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(21, 87);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(271, 85);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            // 
            // chbUser
            // 
            this.chbUser.AutoSize = true;
            this.chbUser.Location = new System.Drawing.Point(17, 0);
            this.chbUser.Name = "chbUser";
            this.chbUser.Size = new System.Drawing.Size(127, 17);
            this.chbUser.TabIndex = 4;
            this.chbUser.Text = "Filtrar por Usuario";
            this.chbUser.UseVisualStyleBackColor = true;
            this.chbUser.CheckedChanged += new System.EventHandler(this.chbUser_CheckedChanged);
            // 
            // txtEmpleado
            // 
            this.txtEmpleado.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtEmpleado.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpleado.Location = new System.Drawing.Point(24, 47);
            this.txtEmpleado.MaxLength = 150;
            this.txtEmpleado.Name = "txtEmpleado";
            this.txtEmpleado.Size = new System.Drawing.Size(228, 25);
            this.txtEmpleado.TabIndex = 2;
            this.txtEmpleado.Enter += new System.EventHandler(this.txtEmpleado_Enter);
            this.txtEmpleado.Leave += new System.EventHandler(this.txtEmpleado_Leave);
            // 
            // rbtNombre
            // 
            this.rbtNombre.AutoSize = true;
            this.rbtNombre.Location = new System.Drawing.Point(184, 19);
            this.rbtNombre.Name = "rbtNombre";
            this.rbtNombre.Size = new System.Drawing.Size(68, 17);
            this.rbtNombre.TabIndex = 1;
            this.rbtNombre.TabStop = true;
            this.rbtNombre.Text = "Nombre";
            this.rbtNombre.UseVisualStyleBackColor = true;
            this.rbtNombre.CheckedChanged += new System.EventHandler(this.rbtLineaU_CheckedChanged);
            // 
            // rbtEmp
            // 
            this.rbtEmp.AutoSize = true;
            this.rbtEmp.Checked = true;
            this.rbtEmp.Location = new System.Drawing.Point(27, 19);
            this.rbtEmp.Name = "rbtEmp";
            this.rbtEmp.Size = new System.Drawing.Size(104, 17);
            this.rbtEmp.TabIndex = 0;
            this.rbtEmp.TabStop = true;
            this.rbtEmp.Text = "No. Empleado";
            this.rbtEmp.UseVisualStyleBackColor = true;
            this.rbtEmp.CheckedChanged += new System.EventHandler(this.rbtLineaT_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chbArea);
            this.groupBox4.Controls.Add(this.cbbArea);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(296, 87);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(162, 85);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            // 
            // chbArea
            // 
            this.chbArea.AutoSize = true;
            this.chbArea.Location = new System.Drawing.Point(11, 0);
            this.chbArea.Name = "chbArea";
            this.chbArea.Size = new System.Drawing.Size(110, 17);
            this.chbArea.TabIndex = 3;
            this.chbArea.Text = "Filtrar por Area";
            this.chbArea.UseVisualStyleBackColor = true;
            this.chbArea.CheckedChanged += new System.EventHandler(this.chbArea_CheckedChanged);
            // 
            // cbbArea
            // 
            this.cbbArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbArea.Enabled = false;
            this.cbbArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbArea.FormattingEnabled = true;
            this.cbbArea.Location = new System.Drawing.Point(11, 47);
            this.cbbArea.Name = "cbbArea";
            this.cbbArea.Size = new System.Drawing.Size(138, 21);
            this.cbbArea.TabIndex = 1;
            this.cbbArea.Enter += new System.EventHandler(this.cbbModelo_Enter);
            this.cbbArea.Leave += new System.EventHandler(this.cbbModelo_Leave);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chbPlanta);
            this.groupBox2.Controls.Add(this.cbbPlanta);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(21, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(290, 65);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // chbPlanta
            // 
            this.chbPlanta.AutoSize = true;
            this.chbPlanta.Checked = true;
            this.chbPlanta.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbPlanta.Location = new System.Drawing.Point(11, 0);
            this.chbPlanta.Name = "chbPlanta";
            this.chbPlanta.Size = new System.Drawing.Size(120, 17);
            this.chbPlanta.TabIndex = 0;
            this.chbPlanta.Text = "Filtrar por Planta";
            this.chbPlanta.UseVisualStyleBackColor = true;
            this.chbPlanta.CheckedChanged += new System.EventHandler(this.chbPlanta_CheckedChanged);
            // 
            // cbbPlanta
            // 
            this.cbbPlanta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbPlanta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbPlanta.FormattingEnabled = true;
            this.cbbPlanta.Location = new System.Drawing.Point(11, 29);
            this.cbbPlanta.Name = "cbbPlanta";
            this.cbbPlanta.Size = new System.Drawing.Size(260, 21);
            this.cbbPlanta.TabIndex = 2;
            this.cbbPlanta.Enter += new System.EventHandler(this.cbbPlanta_Enter);
            this.cbbPlanta.Leave += new System.EventHandler(this.cbbPlanta_Leave);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 237);
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
            // wfRepUsuarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 259);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "wfRepUsuarios";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte de Usuarios";
            this.Activated += new System.EventHandler(this.wfRepEtiquetas_Activated);
            this.Load += new System.EventHandler(this.wfRepEtiquetas_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cbbArea;
        private System.Windows.Forms.CheckBox chbPlanta;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton rbtNombre;
        private System.Windows.Forms.RadioButton rbtEmp;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton rbtTuno2;
        private System.Windows.Forms.RadioButton rbtTuno1;
        private System.Windows.Forms.TextBox txtEmpleado;
        private System.Windows.Forms.CheckBox chbTurno;
        private System.Windows.Forms.CheckBox chbUser;
        private System.Windows.Forms.CheckBox chbArea;
    }
}