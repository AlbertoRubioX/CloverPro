namespace CloverPro
{
    partial class wfCapturaPop3
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtTest = new System.Windows.Forms.RadioButton();
            this.rbtEti = new System.Windows.Forms.RadioButton();
            this.rbtCaja = new System.Windows.Forms.RadioButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(365, 162);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtTest);
            this.groupBox1.Controls.Add(this.rbtEti);
            this.groupBox1.Controls.Add(this.rbtCaja);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(19, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(319, 129);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Motivo del Bloqueo";
            // 
            // rbtTest
            // 
            this.rbtTest.AutoSize = true;
            this.rbtTest.Font = new System.Drawing.Font("Marlett", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtTest.Location = new System.Drawing.Point(88, 97);
            this.rbtTest.Name = "rbtTest";
            this.rbtTest.Size = new System.Drawing.Size(191, 20);
            this.rbtTest.TabIndex = 3;
            this.rbtTest.TabStop = true;
            this.rbtTest.Text = "Prueba / Entrenamiento";
            this.rbtTest.UseVisualStyleBackColor = true;
            this.rbtTest.CheckedChanged += new System.EventHandler(this.rbtTest_CheckedChanged);
            // 
            // rbtEti
            // 
            this.rbtEti.AutoSize = true;
            this.rbtEti.Font = new System.Drawing.Font("Mangal", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtEti.Location = new System.Drawing.Point(88, 31);
            this.rbtEti.Name = "rbtEti";
            this.rbtEti.Size = new System.Drawing.Size(213, 29);
            this.rbtEti.TabIndex = 1;
            this.rbtEti.TabStop = true;
            this.rbtEti.Text = "Error al Escanear Etiqueta";
            this.rbtEti.UseVisualStyleBackColor = true;
            this.rbtEti.CheckedChanged += new System.EventHandler(this.rbtEti_CheckedChanged);
            // 
            // rbtCaja
            // 
            this.rbtCaja.AutoSize = true;
            this.rbtCaja.Font = new System.Drawing.Font("Marlett", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtCaja.Location = new System.Drawing.Point(88, 65);
            this.rbtCaja.Name = "rbtCaja";
            this.rbtCaja.Size = new System.Drawing.Size(132, 20);
            this.rbtCaja.TabIndex = 2;
            this.rbtCaja.TabStop = true;
            this.rbtCaja.Text = "Caja Incorrecta";
            this.rbtCaja.UseVisualStyleBackColor = true;
            this.rbtCaja.CheckedChanged += new System.EventHandler(this.rbtCaja_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 184);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(388, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(329, 17);
            this.toolStripStatusLabel1.Text = "Seleccione Una de las 3 Opciones como el Motivo del Bloqueo";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 212);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(365, 23);
            this.button1.TabIndex = 0;
            this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // wfCapturaPop3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(388, 206);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "wfCapturaPop3";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wfCapturaPop3_FormClosing);
            this.Load += new System.EventHandler(this.wfCapturaPop3_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtTest;
        private System.Windows.Forms.RadioButton rbtEti;
        private System.Windows.Forms.RadioButton rbtCaja;
        private System.Windows.Forms.Button button1;
    }
}