namespace CloverPro
{
    partial class wfCargarRPO
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
            this.btExcel = new System.Windows.Forms.ToolStripButton();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chbFuser = new System.Windows.Forms.CheckBox();
            this.chbNoto = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCantGlob = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtNota = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtHrEnt = new System.Windows.Forms.MaskedTextBox();
            this.txtHrComp = new System.Windows.Forms.MaskedTextBox();
            this.cbbSupervisor = new System.Windows.Forms.ComboBox();
            this.cbbOwner = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cbbDestino = new System.Windows.Forms.ComboBox();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbbTurno = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbbPrioridad = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbbOrigen = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCant = new System.Windows.Forms.TextBox();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.txtRPO = new System.Windows.Forms.TextBox();
            this.txtSKU = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblPlanta = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslPlanta = new System.Windows.Forms.ToolStripStatusLabel();
            this.sp_rep_transferlineTableAdapter1 = new CloverPro.CloverproTableAdapters.sp_rep_transferlineTableAdapter();
            this.chbGlobal = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btExcel,
            this.btnExit,
            this.toolStripSeparator1,
            this.btnHelp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(827, 25);
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
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btExcel
            // 
            this.btExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btExcel.Image = global::CloverPro.Properties.Resources._1492140657_excel;
            this.btExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btExcel.Name = "btExcel";
            this.btExcel.Size = new System.Drawing.Size(23, 22);
            this.btExcel.Text = "Importar RPO";
            this.btExcel.Click += new System.EventHandler(this.btExcel_Click);
            // 
            // btnExit
            // 
            this.btnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExit.Image = global::CloverPro.Properties.Resources.Exit;
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(23, 22);
            this.btnExit.Text = "Salir";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnHelp
            // 
            this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHelp.Image = global::CloverPro.Properties.Resources.Manual;
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(23, 22);
            this.btnHelp.Text = "Consultar Manual";
            this.btnHelp.Visible = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.chbGlobal);
            this.panel1.Controls.Add(this.chbFuser);
            this.panel1.Controls.Add(this.chbNoto);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.cbbTurno);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.cbbPrioridad);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cbbOrigen);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtCant);
            this.panel1.Controls.Add(this.btnGenerar);
            this.panel1.Controls.Add(this.txtRPO);
            this.panel1.Controls.Add(this.txtSKU);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(803, 497);
            this.panel1.TabIndex = 0;
            // 
            // chbFuser
            // 
            this.chbFuser.AutoSize = true;
            this.chbFuser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbFuser.ForeColor = System.Drawing.Color.White;
            this.chbFuser.Location = new System.Drawing.Point(295, 15);
            this.chbFuser.Name = "chbFuser";
            this.chbFuser.Size = new System.Drawing.Size(89, 24);
            this.chbFuser.TabIndex = 27;
            this.chbFuser.Text = "FUSER";
            this.chbFuser.UseVisualStyleBackColor = true;
            this.chbFuser.CheckedChanged += new System.EventHandler(this.chbFuser_CheckedChanged);
            // 
            // chbNoto
            // 
            this.chbNoto.AutoSize = true;
            this.chbNoto.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbNoto.ForeColor = System.Drawing.Color.White;
            this.chbNoto.Location = new System.Drawing.Point(164, 15);
            this.chbNoto.Name = "chbNoto";
            this.chbNoto.Size = new System.Drawing.Size(81, 24);
            this.chbNoto.TabIndex = 26;
            this.chbNoto.Text = "NO TO";
            this.chbNoto.UseVisualStyleBackColor = true;
            this.chbNoto.CheckedChanged += new System.EventHandler(this.chbNoto_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtCantGlob);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txtNota);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtHrEnt);
            this.groupBox1.Controls.Add(this.txtHrComp);
            this.groupBox1.Controls.Add(this.cbbSupervisor);
            this.groupBox1.Controls.Add(this.cbbOwner);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.cbbDestino);
            this.groupBox1.Controls.Add(this.dtpFecha);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Enabled = false;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(30, 224);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(710, 169);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GLOBALS";
            // 
            // txtCantGlob
            // 
            this.txtCantGlob.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCantGlob.Location = new System.Drawing.Point(421, 83);
            this.txtCantGlob.MaxLength = 4;
            this.txtCantGlob.Name = "txtCantGlob";
            this.txtCantGlob.Size = new System.Drawing.Size(55, 20);
            this.txtCantGlob.TabIndex = 4;
            this.txtCantGlob.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.label15.Location = new System.Drawing.Point(329, 84);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(86, 20);
            this.label15.TabIndex = 38;
            this.label15.Text = "Cantidad:";
            // 
            // txtNota
            // 
            this.txtNota.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNota.Location = new System.Drawing.Point(112, 128);
            this.txtNota.MaxLength = 150;
            this.txtNota.Name = "txtNota";
            this.txtNota.Size = new System.Drawing.Size(578, 20);
            this.txtNota.TabIndex = 6;
            this.txtNota.TextChanged += new System.EventHandler(this.txtNota_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.label14.Location = new System.Drawing.Point(37, 126);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 20);
            this.label14.TabIndex = 36;
            this.label14.Text = "Nota:";
            // 
            // txtHrEnt
            // 
            this.txtHrEnt.Location = new System.Drawing.Point(185, 83);
            this.txtHrEnt.Mask = "00:00";
            this.txtHrEnt.Name = "txtHrEnt";
            this.txtHrEnt.Size = new System.Drawing.Size(55, 20);
            this.txtHrEnt.TabIndex = 3;
            this.txtHrEnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtHrComp
            // 
            this.txtHrComp.Location = new System.Drawing.Point(421, 38);
            this.txtHrComp.Mask = "00:00";
            this.txtHrComp.Name = "txtHrComp";
            this.txtHrComp.Size = new System.Drawing.Size(55, 20);
            this.txtHrComp.TabIndex = 1;
            this.txtHrComp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cbbSupervisor
            // 
            this.cbbSupervisor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbSupervisor.FormattingEnabled = true;
            this.cbbSupervisor.Location = new System.Drawing.Point(146, 180);
            this.cbbSupervisor.Name = "cbbSupervisor";
            this.cbbSupervisor.Size = new System.Drawing.Size(269, 21);
            this.cbbSupervisor.TabIndex = 8;
            this.cbbSupervisor.Visible = false;
            // 
            // cbbOwner
            // 
            this.cbbOwner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbOwner.FormattingEnabled = true;
            this.cbbOwner.Location = new System.Drawing.Point(569, 83);
            this.cbbOwner.Name = "cbbOwner";
            this.cbbOwner.Size = new System.Drawing.Size(121, 21);
            this.cbbOwner.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.label13.Location = new System.Drawing.Point(498, 83);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 20);
            this.label13.TabIndex = 33;
            this.label13.Text = "Owner:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.LightYellow;
            this.label12.Location = new System.Drawing.Point(246, 87);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 17);
            this.label12.TabIndex = 31;
            this.label12.Text = "(24hr)";
            // 
            // cbbDestino
            // 
            this.cbbDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbDestino.FormattingEnabled = true;
            this.cbbDestino.Location = new System.Drawing.Point(569, 34);
            this.cbbDestino.Name = "cbbDestino";
            this.cbbDestino.Size = new System.Drawing.Size(121, 21);
            this.cbbDestino.TabIndex = 2;
            // 
            // dtpFecha
            // 
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFecha.Location = new System.Drawing.Point(112, 35);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(128, 20);
            this.dtpFecha.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.label11.Location = new System.Drawing.Point(36, 178);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(104, 20);
            this.label11.TabIndex = 25;
            this.label11.Text = "Supervisor: ";
            this.label11.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.label10.Location = new System.Drawing.Point(37, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(145, 20);
            this.label10.TabIndex = 24;
            this.label10.Text = "RPO Entregado: ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.label9.Location = new System.Drawing.Point(482, 36);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 20);
            this.label9.TabIndex = 23;
            this.label9.Text = "Destino: ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.label8.Location = new System.Drawing.Point(261, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(162, 20);
            this.label8.TabIndex = 22;
            this.label8.Text = "Hora Compromiso: ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.label7.Location = new System.Drawing.Point(37, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 20);
            this.label7.TabIndex = 21;
            this.label7.Text = "Fecha: ";
            // 
            // cbbTurno
            // 
            this.cbbTurno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbTurno.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbTurno.FormattingEnabled = true;
            this.cbbTurno.Location = new System.Drawing.Point(565, 53);
            this.cbbTurno.Name = "cbbTurno";
            this.cbbTurno.Size = new System.Drawing.Size(175, 33);
            this.cbbTurno.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label6.Location = new System.Drawing.Point(458, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 26);
            this.label6.TabIndex = 19;
            this.label6.Text = "TURNO:";
            // 
            // cbbPrioridad
            // 
            this.cbbPrioridad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbPrioridad.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbPrioridad.FormattingEnabled = true;
            this.cbbPrioridad.Location = new System.Drawing.Point(565, 169);
            this.cbbPrioridad.Name = "cbbPrioridad";
            this.cbbPrioridad.Size = new System.Drawing.Size(175, 33);
            this.cbbPrioridad.TabIndex = 5;
            this.cbbPrioridad.SelectionChangeCommitted += new System.EventHandler(this.cbbPrioridad_SelectionChangeCommitted);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label5.Location = new System.Drawing.Point(408, 172);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 26);
            this.label5.TabIndex = 17;
            this.label5.Text = "PRIORIDAD:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label4.Location = new System.Drawing.Point(420, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 26);
            this.label4.TabIndex = 15;
            this.label4.Text = "CANTIDAD:";
            // 
            // cbbOrigen
            // 
            this.cbbOrigen.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbOrigen.FormattingEnabled = true;
            this.cbbOrigen.Location = new System.Drawing.Point(164, 169);
            this.cbbOrigen.Name = "cbbOrigen";
            this.cbbOrigen.Size = new System.Drawing.Size(218, 33);
            this.cbbOrigen.TabIndex = 2;
            this.cbbOrigen.SelectionChangeCommitted += new System.EventHandler(this.cbbOrigen_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label3.Location = new System.Drawing.Point(63, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 26);
            this.label3.TabIndex = 14;
            this.label3.Text = "LINEA :";
            // 
            // txtCant
            // 
            this.txtCant.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCant.Location = new System.Drawing.Point(565, 113);
            this.txtCant.MaxLength = 6;
            this.txtCant.Name = "txtCant";
            this.txtCant.Size = new System.Drawing.Size(175, 32);
            this.txtCant.TabIndex = 4;
            this.txtCant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCant.Enter += new System.EventHandler(this.txtCant_Enter);
            this.txtCant.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCant_KeyDown);
            this.txtCant.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCant_KeyPress);
            // 
            // btnGenerar
            // 
            this.btnGenerar.BackColor = System.Drawing.Color.Transparent;
            this.btnGenerar.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.btnGenerar.FlatAppearance.BorderSize = 2;
            this.btnGenerar.Font = new System.Drawing.Font("Calibri", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerar.ForeColor = System.Drawing.Color.DodgerBlue;
            this.btnGenerar.Image = global::CloverPro.Properties.Resources.if_Save_48;
            this.btnGenerar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGenerar.Location = new System.Drawing.Point(295, 411);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Padding = new System.Windows.Forms.Padding(0, 0, 30, 5);
            this.btnGenerar.Size = new System.Drawing.Size(203, 59);
            this.btnGenerar.TabIndex = 7;
            this.btnGenerar.Text = "GUARDAR";
            this.btnGenerar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGenerar.UseVisualStyleBackColor = false;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // txtRPO
            // 
            this.txtRPO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(110)))));
            this.txtRPO.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRPO.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRPO.Location = new System.Drawing.Point(164, 53);
            this.txtRPO.MaxLength = 20;
            this.txtRPO.Name = "txtRPO";
            this.txtRPO.Size = new System.Drawing.Size(218, 32);
            this.txtRPO.TabIndex = 0;
            this.txtRPO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRPO_KeyDown);
            // 
            // txtSKU
            // 
            this.txtSKU.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSKU.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSKU.Location = new System.Drawing.Point(164, 110);
            this.txtSKU.MaxLength = 20;
            this.txtSKU.Name = "txtSKU";
            this.txtSKU.Size = new System.Drawing.Size(218, 32);
            this.txtSKU.TabIndex = 1;
            this.txtSKU.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSKU_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label1.Location = new System.Drawing.Point(88, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "RPO:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label2.Location = new System.Drawing.Point(38, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "MODELO:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblPlanta,
            this.tslPlanta});
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(827, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblPlanta
            // 
            this.lblPlanta.Name = "lblPlanta";
            this.lblPlanta.Size = new System.Drawing.Size(46, 17);
            this.lblPlanta.Text = "Planta :";
            // 
            // tslPlanta
            // 
            this.tslPlanta.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.tslPlanta.Name = "tslPlanta";
            this.tslPlanta.Size = new System.Drawing.Size(48, 17);
            this.tslPlanta.Text = "NICOYA";
            // 
            // sp_rep_transferlineTableAdapter1
            // 
            this.sp_rep_transferlineTableAdapter1.ClearBeforeFill = true;
            // 
            // chbGlobal
            // 
            this.chbGlobal.AutoSize = true;
            this.chbGlobal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbGlobal.ForeColor = System.Drawing.Color.IndianRed;
            this.chbGlobal.Location = new System.Drawing.Point(586, 411);
            this.chbGlobal.Name = "chbGlobal";
            this.chbGlobal.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chbGlobal.Size = new System.Drawing.Size(134, 24);
            this.chbGlobal.TabIndex = 39;
            this.chbGlobal.Text = "Orden Global";
            this.chbGlobal.UseVisualStyleBackColor = true;
            this.chbGlobal.Visible = false;
            // 
            // wfCargarRPO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(827, 561);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "wfCargarRPO";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registro Individual de RPO";
            this.Activated += new System.EventHandler(this.wfCargarRPO_Activated);
            this.Load += new System.EventHandler(this.wfCargarRPO_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.TextBox txtRPO;
        private System.Windows.Forms.TextBox txtSKU;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox txtCant;
        private System.Windows.Forms.ComboBox cbbOrigen;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnHelp;
        private CloverproTableAdapters.sp_rep_transferlineTableAdapter sp_rep_transferlineTableAdapter1;
        private System.Windows.Forms.ComboBox cbbPrioridad;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbbTurno;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripStatusLabel lblPlanta;
        private System.Windows.Forms.ToolStripStatusLabel tslPlanta;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbbSupervisor;
        private System.Windows.Forms.ComboBox cbbDestino;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbbOwner;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.MaskedTextBox txtHrEnt;
        private System.Windows.Forms.MaskedTextBox txtHrComp;
        private System.Windows.Forms.CheckBox chbNoto;
        private System.Windows.Forms.TextBox txtNota;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtCantGlob;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ToolStripButton btExcel;
        private System.Windows.Forms.CheckBox chbFuser;
        private System.Windows.Forms.CheckBox chbGlobal;
    }
}