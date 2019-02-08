namespace CloverPro
{
    partial class wfVisorLineSetUp
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgwEstaciones = new System.Windows.Forms.DataGridView();
            this.lblLinea = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnFolio = new System.Windows.Forms.Button();
            this.btnAreas = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chbEstatus = new System.Windows.Forms.CheckBox();
            this.cbbEstatus = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFechaIni = new System.Windows.Forms.DateTimePicker();
            this.lblRpo = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chbPlanta = new System.Windows.Forms.CheckBox();
            this.lblModelo = new System.Windows.Forms.Label();
            this.cbbPlanta = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chbLinea = new System.Windows.Forms.CheckBox();
            this.cbbLinea = new System.Windows.Forms.ComboBox();
            this.lblCoreMod = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chbTurno = new System.Windows.Forms.CheckBox();
            this.cbbTurno = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnEdit = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwEstaciones)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dgwEstaciones);
            this.panel1.Controls.Add(this.lblLinea);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(25, 109);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1424, 464);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // dgwEstaciones
            // 
            this.dgwEstaciones.AllowUserToAddRows = false;
            this.dgwEstaciones.AllowUserToDeleteRows = false;
            this.dgwEstaciones.AllowUserToResizeColumns = false;
            this.dgwEstaciones.AllowUserToResizeRows = false;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.Black;
            this.dgwEstaciones.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dgwEstaciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgwEstaciones.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgwEstaciones.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgwEstaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgwEstaciones.DefaultCellStyle = dataGridViewCellStyle15;
            this.dgwEstaciones.Location = new System.Drawing.Point(3, 5);
            this.dgwEstaciones.MultiSelect = false;
            this.dgwEstaciones.Name = "dgwEstaciones";
            this.dgwEstaciones.ReadOnly = true;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgwEstaciones.RowHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.dgwEstaciones.RowHeadersVisible = false;
            this.dgwEstaciones.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dgwEstaciones.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgwEstaciones.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgwEstaciones.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgwEstaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgwEstaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwEstaciones.Size = new System.Drawing.Size(1416, 452);
            this.dgwEstaciones.TabIndex = 0;
            this.dgwEstaciones.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwEstaciones_CellClick);
            this.dgwEstaciones.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgwEstaciones_CellFormatting);
            this.dgwEstaciones.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwEstaciones_CellValueChanged);
            this.dgwEstaciones.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgwEstaciones_ColumnAdded);
            this.dgwEstaciones.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgwEstaciones_RowsAdded);
            this.dgwEstaciones.SelectionChanged += new System.EventHandler(this.dgwEstaciones_SelectionChanged);
            this.dgwEstaciones.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgwEstaciones_KeyDown);
            // 
            // lblLinea
            // 
            this.lblLinea.AutoSize = true;
            this.lblLinea.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLinea.Location = new System.Drawing.Point(90, 17);
            this.lblLinea.Name = "lblLinea";
            this.lblLinea.Size = new System.Drawing.Size(0, 31);
            this.lblLinea.TabIndex = 34;
            this.lblLinea.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnFolio);
            this.panel2.Controls.Add(this.btnAreas);
            this.panel2.Controls.Add(this.btnLoad);
            this.panel2.Controls.Add(this.groupBox6);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Controls.Add(this.groupBox5);
            this.panel2.Location = new System.Drawing.Point(22, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1283, 94);
            this.panel2.TabIndex = 0;
            this.panel2.Visible = false;
            // 
            // btnFolio
            // 
            this.btnFolio.Location = new System.Drawing.Point(1308, 36);
            this.btnFolio.Name = "btnFolio";
            this.btnFolio.Size = new System.Drawing.Size(75, 23);
            this.btnFolio.TabIndex = 33;
            this.btnFolio.Text = "Folio";
            this.btnFolio.UseVisualStyleBackColor = true;
            this.btnFolio.Visible = false;
            this.btnFolio.Click += new System.EventHandler(this.btnFolio_Click);
            // 
            // btnAreas
            // 
            this.btnAreas.Location = new System.Drawing.Point(1308, 11);
            this.btnAreas.Name = "btnAreas";
            this.btnAreas.Size = new System.Drawing.Size(75, 23);
            this.btnAreas.TabIndex = 32;
            this.btnAreas.Text = "Actividades";
            this.btnAreas.UseVisualStyleBackColor = true;
            this.btnAreas.Visible = false;
            this.btnAreas.Click += new System.EventHandler(this.btnAreas_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.Transparent;
            this.btnLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnLoad.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnLoad.FlatAppearance.BorderSize = 3;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.ForeColor = System.Drawing.Color.DodgerBlue;
            this.btnLoad.Image = global::CloverPro.Properties.Resources.Sync;
            this.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoad.Location = new System.Drawing.Point(1296, 22);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnLoad.Size = new System.Drawing.Size(100, 47);
            this.btnLoad.TabIndex = 31;
            this.btnLoad.Text = "CARGAR";
            this.btnLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chbEstatus);
            this.groupBox6.Controls.Add(this.cbbEstatus);
            this.groupBox6.Location = new System.Drawing.Point(1083, 11);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(188, 61);
            this.groupBox6.TabIndex = 30;
            this.groupBox6.TabStop = false;
            // 
            // chbEstatus
            // 
            this.chbEstatus.AutoSize = true;
            this.chbEstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbEstatus.ForeColor = System.Drawing.Color.White;
            this.chbEstatus.Location = new System.Drawing.Point(13, -2);
            this.chbEstatus.Name = "chbEstatus";
            this.chbEstatus.Size = new System.Drawing.Size(139, 21);
            this.chbEstatus.TabIndex = 0;
            this.chbEstatus.Text = "Filtrar por Estatus";
            this.chbEstatus.UseVisualStyleBackColor = true;
            this.chbEstatus.CheckedChanged += new System.EventHandler(this.chbEstatus_CheckedChanged);
            // 
            // cbbEstatus
            // 
            this.cbbEstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbEstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbEstatus.FormattingEnabled = true;
            this.cbbEstatus.Location = new System.Drawing.Point(13, 21);
            this.cbbEstatus.Name = "cbbEstatus";
            this.cbbEstatus.Size = new System.Drawing.Size(160, 28);
            this.cbbEstatus.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dtpFechaFin);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dtpFechaIni);
            this.groupBox2.Controls.Add(this.lblRpo);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(5, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(364, 61);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Periodo";
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFin.Location = new System.Drawing.Point(232, 22);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(117, 26);
            this.dtpFechaFin.TabIndex = 26;
            this.dtpFechaFin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtpFechaFin_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(191, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 28);
            this.label2.TabIndex = 25;
            this.label2.Text = "al";
            // 
            // dtpFechaIni
            // 
            this.dtpFechaIni.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaIni.Location = new System.Drawing.Point(68, 22);
            this.dtpFechaIni.Name = "dtpFechaIni";
            this.dtpFechaIni.Size = new System.Drawing.Size(117, 26);
            this.dtpFechaIni.TabIndex = 24;
            this.dtpFechaIni.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtpFechaIni_KeyDown);
            // 
            // lblRpo
            // 
            this.lblRpo.AutoSize = true;
            this.lblRpo.Font = new System.Drawing.Font("Meiryo UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRpo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblRpo.Location = new System.Drawing.Point(12, 20);
            this.lblRpo.Name = "lblRpo";
            this.lblRpo.Size = new System.Drawing.Size(50, 28);
            this.lblRpo.TabIndex = 20;
            this.lblRpo.Text = "Del";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chbPlanta);
            this.groupBox3.Controls.Add(this.lblModelo);
            this.groupBox3.Controls.Add(this.cbbPlanta);
            this.groupBox3.Location = new System.Drawing.Point(375, 11);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(277, 61);
            this.groupBox3.TabIndex = 27;
            this.groupBox3.TabStop = false;
            // 
            // chbPlanta
            // 
            this.chbPlanta.AutoSize = true;
            this.chbPlanta.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbPlanta.ForeColor = System.Drawing.Color.White;
            this.chbPlanta.Location = new System.Drawing.Point(13, -2);
            this.chbPlanta.Name = "chbPlanta";
            this.chbPlanta.Size = new System.Drawing.Size(132, 21);
            this.chbPlanta.TabIndex = 0;
            this.chbPlanta.Text = "Filtrar por Planta";
            this.chbPlanta.UseVisualStyleBackColor = true;
            this.chbPlanta.CheckedChanged += new System.EventHandler(this.chbPlanta_CheckedChanged);
            // 
            // lblModelo
            // 
            this.lblModelo.AutoSize = true;
            this.lblModelo.Font = new System.Drawing.Font("Meiryo UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModelo.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblModelo.Location = new System.Drawing.Point(8, 19);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.Size = new System.Drawing.Size(112, 28);
            this.lblModelo.TabIndex = 0;
            this.lblModelo.Text = "PLANTA:";
            // 
            // cbbPlanta
            // 
            this.cbbPlanta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbPlanta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbPlanta.FormattingEnabled = true;
            this.cbbPlanta.Location = new System.Drawing.Point(126, 21);
            this.cbbPlanta.Name = "cbbPlanta";
            this.cbbPlanta.Size = new System.Drawing.Size(132, 28);
            this.cbbPlanta.TabIndex = 1;
            this.cbbPlanta.SelectionChangeCommitted += new System.EventHandler(this.cbbPlanta_SelectionChangeCommitted);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chbLinea);
            this.groupBox4.Controls.Add(this.cbbLinea);
            this.groupBox4.Controls.Add(this.lblCoreMod);
            this.groupBox4.Location = new System.Drawing.Point(658, 11);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(225, 61);
            this.groupBox4.TabIndex = 28;
            this.groupBox4.TabStop = false;
            // 
            // chbLinea
            // 
            this.chbLinea.AutoSize = true;
            this.chbLinea.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbLinea.ForeColor = System.Drawing.Color.White;
            this.chbLinea.Location = new System.Drawing.Point(13, -2);
            this.chbLinea.Name = "chbLinea";
            this.chbLinea.Size = new System.Drawing.Size(127, 21);
            this.chbLinea.TabIndex = 0;
            this.chbLinea.Text = "Filtrar por Linea";
            this.chbLinea.UseVisualStyleBackColor = true;
            this.chbLinea.CheckedChanged += new System.EventHandler(this.chbLinea_CheckedChanged);
            // 
            // cbbLinea
            // 
            this.cbbLinea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbLinea.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbLinea.FormattingEnabled = true;
            this.cbbLinea.Location = new System.Drawing.Point(107, 21);
            this.cbbLinea.Name = "cbbLinea";
            this.cbbLinea.Size = new System.Drawing.Size(95, 28);
            this.cbbLinea.TabIndex = 2;
            this.cbbLinea.Enter += new System.EventHandler(this.cbbLinea_Enter);
            this.cbbLinea.Leave += new System.EventHandler(this.cbbLinea_Leave);
            // 
            // lblCoreMod
            // 
            this.lblCoreMod.AutoSize = true;
            this.lblCoreMod.Font = new System.Drawing.Font("Meiryo UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCoreMod.ForeColor = System.Drawing.Color.LightCoral;
            this.lblCoreMod.Location = new System.Drawing.Point(8, 19);
            this.lblCoreMod.Name = "lblCoreMod";
            this.lblCoreMod.Size = new System.Drawing.Size(93, 28);
            this.lblCoreMod.TabIndex = 21;
            this.lblCoreMod.Text = "LINEA:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.chbTurno);
            this.groupBox5.Controls.Add(this.cbbTurno);
            this.groupBox5.Location = new System.Drawing.Point(889, 11);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(188, 61);
            this.groupBox5.TabIndex = 29;
            this.groupBox5.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Gainsboro;
            this.label1.Location = new System.Drawing.Point(8, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 28);
            this.label1.TabIndex = 23;
            this.label1.Text = "TURNO:";
            // 
            // chbTurno
            // 
            this.chbTurno.AutoSize = true;
            this.chbTurno.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbTurno.ForeColor = System.Drawing.Color.White;
            this.chbTurno.Location = new System.Drawing.Point(13, -2);
            this.chbTurno.Name = "chbTurno";
            this.chbTurno.Size = new System.Drawing.Size(130, 21);
            this.chbTurno.TabIndex = 0;
            this.chbTurno.Text = "Filtrar por Turno";
            this.chbTurno.UseVisualStyleBackColor = true;
            this.chbTurno.CheckedChanged += new System.EventHandler(this.chbTurno_CheckedChanged);
            // 
            // cbbTurno
            // 
            this.cbbTurno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbTurno.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbTurno.FormattingEnabled = true;
            this.cbbTurno.Location = new System.Drawing.Point(119, 21);
            this.cbbTurno.Name = "cbbTurno";
            this.cbbTurno.Size = new System.Drawing.Size(54, 28);
            this.cbbTurno.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Interval = 30000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Image = global::CloverPro.Properties.Resources.if_arrow_back_outline_216436;
            this.btnEdit.Location = new System.Drawing.Point(54, 39);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 39);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // timer2
            // 
            this.timer2.Interval = 60000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 30000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // wfVisorLineSetUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CloverPro.Properties.Resources.CAMBIO_DE_MODELO;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1472, 703);
            this.ControlBox = false;
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.panel1);
            this.Name = "wfVisorLineSetUp";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CloverPRO - Visor de Cambios de Set Up";
            this.Activated += new System.EventHandler(this.wfVisorLineSetUp_Activated);
            this.Load += new System.EventHandler(this.wfVisorLineSetUp_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.wfVisorLineSetUp_KeyDown);
            this.Resize += new System.EventHandler(this.wfVisorLineSetUp_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwEstaciones)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblModelo;
        private System.Windows.Forms.DataGridView dgwEstaciones;
        private System.Windows.Forms.ComboBox cbbLinea;
        private System.Windows.Forms.Label lblLinea;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblCoreMod;
        private System.Windows.Forms.ComboBox cbbPlanta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbTurno;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFechaIni;
        private System.Windows.Forms.Label lblRpo;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chbPlanta;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chbLinea;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chbTurno;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox chbEstatus;
        private System.Windows.Forms.ComboBox cbbEstatus;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnFolio;
        private System.Windows.Forms.Button btnAreas;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
    }
}