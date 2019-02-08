namespace CloverPro
{
    partial class wfRepHistorialPPH
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtLineas = new System.Windows.Forms.TextBox();
            this.cbbLineaIni = new System.Windows.Forms.ComboBox();
            this.lblLIni = new System.Windows.Forms.Label();
            this.rbtLineaA = new System.Windows.Forms.RadioButton();
            this.rbtLineaU = new System.Windows.Forms.RadioButton();
            this.rbtLineaT = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtEmpleado = new System.Windows.Forms.TextBox();
            this.lblEmp = new System.Windows.Forms.Label();
            this.rbtEmpU = new System.Windows.Forms.RadioButton();
            this.rbtEmpT = new System.Windows.Forms.RadioButton();
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbbNivel = new System.Windows.Forms.ComboBox();
            this.lblNivel = new System.Windows.Forms.Label();
            this.rbtNivelA = new System.Windows.Forms.RadioButton();
            this.rbtNivelU = new System.Windows.Forms.RadioButton();
            this.rbtNivelT = new System.Windows.Forms.RadioButton();
            this.txtNiveles = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
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
            this.toolStrip1.Size = new System.Drawing.Size(513, 25);
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
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 434);
            this.panel1.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtLineas);
            this.groupBox6.Controls.Add(this.cbbLineaIni);
            this.groupBox6.Controls.Add(this.lblLIni);
            this.groupBox6.Controls.Add(this.rbtLineaA);
            this.groupBox6.Controls.Add(this.rbtLineaU);
            this.groupBox6.Controls.Add(this.rbtLineaT);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(20, 334);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(437, 85);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Lineas";
            // 
            // txtLineas
            // 
            this.txtLineas.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLineas.Location = new System.Drawing.Point(116, 51);
            this.txtLineas.MaxLength = 100;
            this.txtLineas.Name = "txtLineas";
            this.txtLineas.Size = new System.Drawing.Size(292, 20);
            this.txtLineas.TabIndex = 4;
            this.txtLineas.Visible = false;
            this.txtLineas.Enter += new System.EventHandler(this.txtLineas_Enter);
            this.txtLineas.Leave += new System.EventHandler(this.txtLineas_Leave);
            // 
            // cbbLineaIni
            // 
            this.cbbLineaIni.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbLineaIni.FormattingEnabled = true;
            this.cbbLineaIni.Location = new System.Drawing.Point(115, 50);
            this.cbbLineaIni.Name = "cbbLineaIni";
            this.cbbLineaIni.Size = new System.Drawing.Size(66, 21);
            this.cbbLineaIni.TabIndex = 3;
            this.cbbLineaIni.Visible = false;
            this.cbbLineaIni.Enter += new System.EventHandler(this.cbbLineaIni_Enter);
            this.cbbLineaIni.Leave += new System.EventHandler(this.cbbLineaIni_Leave);
            // 
            // lblLIni
            // 
            this.lblLIni.AutoSize = true;
            this.lblLIni.Location = new System.Drawing.Point(53, 53);
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
            this.groupBox3.Controls.Add(this.txtNombre);
            this.groupBox3.Controls.Add(this.txtEmpleado);
            this.groupBox3.Controls.Add(this.lblEmp);
            this.groupBox3.Controls.Add(this.rbtEmpU);
            this.groupBox3.Controls.Add(this.rbtEmpT);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(21, 74);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(437, 72);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Operadores";
            // 
            // txtNombre
            // 
            this.txtNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.Location = new System.Drawing.Point(187, 42);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.ReadOnly = true;
            this.txtNombre.Size = new System.Drawing.Size(220, 20);
            this.txtNombre.TabIndex = 4;
            // 
            // txtEmpleado
            // 
            this.txtEmpleado.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtEmpleado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpleado.Location = new System.Drawing.Point(115, 41);
            this.txtEmpleado.MaxLength = 6;
            this.txtEmpleado.Name = "txtEmpleado";
            this.txtEmpleado.Size = new System.Drawing.Size(66, 20);
            this.txtEmpleado.TabIndex = 2;
            this.txtEmpleado.Enter += new System.EventHandler(this.txtEmpleado_Enter);
            this.txtEmpleado.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEmpleado_KeyDown);
            this.txtEmpleado.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFolioIni_KeyPress);
            this.txtEmpleado.Leave += new System.EventHandler(this.txtEmpleado_Leave);
            // 
            // lblEmp
            // 
            this.lblEmp.AutoSize = true;
            this.lblEmp.Location = new System.Drawing.Point(24, 44);
            this.lblEmp.Name = "lblEmp";
            this.lblEmp.Size = new System.Drawing.Size(90, 13);
            this.lblEmp.TabIndex = 4;
            this.lblEmp.Text = "No. Empleado:";
            // 
            // rbtEmpU
            // 
            this.rbtEmpU.AutoSize = true;
            this.rbtEmpU.Checked = true;
            this.rbtEmpU.Location = new System.Drawing.Point(204, 19);
            this.rbtEmpU.Name = "rbtEmpU";
            this.rbtEmpU.Size = new System.Drawing.Size(48, 17);
            this.rbtEmpU.TabIndex = 1;
            this.rbtEmpU.TabStop = true;
            this.rbtEmpU.Text = "Uno";
            this.rbtEmpU.UseVisualStyleBackColor = true;
            this.rbtEmpU.CheckedChanged += new System.EventHandler(this.rbtFolioU_CheckedChanged);
            // 
            // rbtEmpT
            // 
            this.rbtEmpT.AutoSize = true;
            this.rbtEmpT.Location = new System.Drawing.Point(40, 19);
            this.rbtEmpT.Name = "rbtEmpT";
            this.rbtEmpT.Size = new System.Drawing.Size(60, 17);
            this.rbtEmpT.TabIndex = 0;
            this.rbtEmpT.Text = "Todos";
            this.rbtEmpT.UseVisualStyleBackColor = true;
            this.rbtEmpT.CheckedChanged += new System.EventHandler(this.rbtFolioT_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtPlantaU);
            this.groupBox2.Controls.Add(this.rbtPlantaT);
            this.groupBox2.Controls.Add(this.cbbPlanta);
            this.groupBox2.Controls.Add(this.lblPlanta);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(20, 246);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(437, 82);
            this.groupBox2.TabIndex = 3;
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
            this.cbbPlanta.Size = new System.Drawing.Size(292, 21);
            this.cbbPlanta.TabIndex = 2;
            this.cbbPlanta.Visible = false;
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
            this.lblPlanta.Visible = false;
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
            this.label3.Location = new System.Drawing.Point(230, 24);
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
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbbNivel);
            this.groupBox4.Controls.Add(this.lblNivel);
            this.groupBox4.Controls.Add(this.rbtNivelA);
            this.groupBox4.Controls.Add(this.rbtNivelU);
            this.groupBox4.Controls.Add(this.rbtNivelT);
            this.groupBox4.Controls.Add(this.txtNiveles);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(21, 152);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(437, 83);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Nivel PPH";
            // 
            // cbbNivel
            // 
            this.cbbNivel.DropDownWidth = 150;
            this.cbbNivel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbNivel.FormattingEnabled = true;
            this.cbbNivel.Location = new System.Drawing.Point(115, 43);
            this.cbbNivel.Name = "cbbNivel";
            this.cbbNivel.Size = new System.Drawing.Size(66, 21);
            this.cbbNivel.TabIndex = 3;
            this.cbbNivel.Visible = false;
            this.cbbNivel.Enter += new System.EventHandler(this.cbbNivel_Enter);
            this.cbbNivel.Leave += new System.EventHandler(this.cbbNivel_Leave);
            // 
            // lblNivel
            // 
            this.lblNivel.AutoSize = true;
            this.lblNivel.Location = new System.Drawing.Point(64, 46);
            this.lblNivel.Name = "lblNivel";
            this.lblNivel.Size = new System.Drawing.Size(36, 13);
            this.lblNivel.TabIndex = 4;
            this.lblNivel.Text = "PPH:";
            this.lblNivel.Visible = false;
            // 
            // rbtNivelA
            // 
            this.rbtNivelA.AutoSize = true;
            this.rbtNivelA.Location = new System.Drawing.Point(337, 19);
            this.rbtNivelA.Name = "rbtNivelA";
            this.rbtNivelA.Size = new System.Drawing.Size(70, 17);
            this.rbtNivelA.TabIndex = 2;
            this.rbtNivelA.Text = "Algunos";
            this.rbtNivelA.UseVisualStyleBackColor = true;
            this.rbtNivelA.CheckedChanged += new System.EventHandler(this.rbtNivelA_CheckedChanged);
            // 
            // rbtNivelU
            // 
            this.rbtNivelU.AutoSize = true;
            this.rbtNivelU.Location = new System.Drawing.Point(204, 19);
            this.rbtNivelU.Name = "rbtNivelU";
            this.rbtNivelU.Size = new System.Drawing.Size(48, 17);
            this.rbtNivelU.TabIndex = 1;
            this.rbtNivelU.Text = "Uno";
            this.rbtNivelU.UseVisualStyleBackColor = true;
            this.rbtNivelU.CheckedChanged += new System.EventHandler(this.rbtNivelU_CheckedChanged);
            // 
            // rbtNivelT
            // 
            this.rbtNivelT.AutoSize = true;
            this.rbtNivelT.Checked = true;
            this.rbtNivelT.Location = new System.Drawing.Point(40, 19);
            this.rbtNivelT.Name = "rbtNivelT";
            this.rbtNivelT.Size = new System.Drawing.Size(60, 17);
            this.rbtNivelT.TabIndex = 0;
            this.rbtNivelT.TabStop = true;
            this.rbtNivelT.Text = "Todos";
            this.rbtNivelT.UseVisualStyleBackColor = true;
            this.rbtNivelT.CheckedChanged += new System.EventHandler(this.rbtNivelT_CheckedChanged);
            // 
            // txtNiveles
            // 
            this.txtNiveles.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNiveles.Location = new System.Drawing.Point(114, 44);
            this.txtNiveles.MaxLength = 100;
            this.txtNiveles.Name = "txtNiveles";
            this.txtNiveles.Size = new System.Drawing.Size(292, 20);
            this.txtNiveles.TabIndex = 4;
            this.txtNiveles.Visible = false;
            this.txtNiveles.Enter += new System.EventHandler(this.txtNiveles_Enter);
            this.txtNiveles.Leave += new System.EventHandler(this.txtNiveles_Leave);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 474);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(513, 22);
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
            // wfRepHistorialPPH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 496);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "wfRepHistorialPPH";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte Histórico de PPH por Operador";
            this.Activated += new System.EventHandler(this.wfRepEtiquetas_Activated);
            this.Load += new System.EventHandler(this.wfRepEtiquetas_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtEmpleado;
        private System.Windows.Forms.Label lblEmp;
        private System.Windows.Forms.RadioButton rbtEmpU;
        private System.Windows.Forms.RadioButton rbtEmpT;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtPlantaU;
        private System.Windows.Forms.RadioButton rbtPlantaT;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txtLineas;
        private System.Windows.Forms.ComboBox cbbLineaIni;
        private System.Windows.Forms.Label lblLIni;
        private System.Windows.Forms.RadioButton rbtLineaA;
        private System.Windows.Forms.RadioButton rbtLineaU;
        private System.Windows.Forms.RadioButton rbtLineaT;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cbbNivel;
        private System.Windows.Forms.Label lblNivel;
        private System.Windows.Forms.RadioButton rbtNivelA;
        private System.Windows.Forms.RadioButton rbtNivelU;
        private System.Windows.Forms.RadioButton rbtNivelT;
        private System.Windows.Forms.TextBox txtNiveles;
    }
}