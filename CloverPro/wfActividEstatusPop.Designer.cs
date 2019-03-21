namespace CloverPro
{
    partial class wfActividEstatusPop
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
            this.btnStoped = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNota = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEspera = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnComplete = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.btnAsignar = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnStoped);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtNota);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnEspera);
            this.panel1.Controls.Add(this.btnProcess);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnComplete);
            this.panel1.Controls.Add(this.btnDone);
            this.panel1.Controls.Add(this.btnFinish);
            this.panel1.Controls.Add(this.btnAsignar);
            this.panel1.Location = new System.Drawing.Point(12, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(642, 245);
            this.panel1.TabIndex = 0;
            // 
            // btnStoped
            // 
            this.btnStoped.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnStoped.BackgroundImage = global::CloverPro.Properties.Resources.if_Process_Warning;
            this.btnStoped.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStoped.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStoped.FlatAppearance.BorderColor = System.Drawing.Color.IndianRed;
            this.btnStoped.FlatAppearance.BorderSize = 2;
            this.btnStoped.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStoped.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStoped.ForeColor = System.Drawing.Color.Brown;
            this.btnStoped.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStoped.Location = new System.Drawing.Point(229, 35);
            this.btnStoped.Name = "btnStoped";
            this.btnStoped.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnStoped.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnStoped.Size = new System.Drawing.Size(185, 113);
            this.btnStoped.TabIndex = 13;
            this.btnStoped.Text = "DETENIDO";
            this.btnStoped.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStoped.UseVisualStyleBackColor = false;
            this.btnStoped.Click += new System.EventHandler(this.btnStoped_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(94, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "NOTA:";
            // 
            // txtNota
            // 
            this.txtNota.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNota.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNota.Location = new System.Drawing.Point(161, 173);
            this.txtNota.MaxLength = 100;
            this.txtNota.Multiline = true;
            this.txtNota.Name = "txtNota";
            this.txtNota.Size = new System.Drawing.Size(352, 44);
            this.txtNota.TabIndex = 0;
            this.txtNota.Enter += new System.EventHandler(this.txtNota_Enter);
            this.txtNota.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNota_KeyDown);
            this.txtNota.Leave += new System.EventHandler(this.txtNota_Leave);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSave.BackgroundImage = global::CloverPro.Properties.Resources.listo;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.ForestGreen;
            this.btnSave.FlatAppearance.BorderSize = 2;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.MintCream;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.Location = new System.Drawing.Point(229, 35);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnSave.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnSave.Size = new System.Drawing.Size(185, 113);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "LISTO";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEspera
            // 
            this.btnEspera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnEspera.BackgroundImage = global::CloverPro.Properties.Resources.clock;
            this.btnEspera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEspera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEspera.FlatAppearance.BorderColor = System.Drawing.Color.MediumTurquoise;
            this.btnEspera.FlatAppearance.BorderSize = 2;
            this.btnEspera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEspera.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEspera.ForeColor = System.Drawing.Color.MintCream;
            this.btnEspera.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEspera.Location = new System.Drawing.Point(229, 35);
            this.btnEspera.Name = "btnEspera";
            this.btnEspera.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnEspera.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnEspera.Size = new System.Drawing.Size(185, 113);
            this.btnEspera.TabIndex = 10;
            this.btnEspera.Text = "EN ESPERA";
            this.btnEspera.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEspera.UseVisualStyleBackColor = false;
            this.btnEspera.Visible = false;
            this.btnEspera.Click += new System.EventHandler(this.btnEspera_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnProcess.BackgroundImage = global::CloverPro.Properties.Resources.if_Process_Info;
            this.btnProcess.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProcess.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProcess.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnProcess.FlatAppearance.BorderSize = 2;
            this.btnProcess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcess.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcess.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnProcess.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnProcess.Location = new System.Drawing.Point(27, 35);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnProcess.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnProcess.Size = new System.Drawing.Size(185, 113);
            this.btnProcess.TabIndex = 12;
            this.btnProcess.Text = "EN PROCESO";
            this.btnProcess.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnProcess.UseVisualStyleBackColor = false;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnExit.FlatAppearance.BorderSize = 2;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Calibri", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.Location = new System.Drawing.Point(27, 35);
            this.btnExit.Name = "btnExit";
            this.btnExit.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnExit.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnExit.Size = new System.Drawing.Size(185, 113);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "N / A";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnComplete
            // 
            this.btnComplete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnComplete.BackgroundImage = global::CloverPro.Properties.Resources.if_complete;
            this.btnComplete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnComplete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnComplete.FlatAppearance.BorderColor = System.Drawing.Color.ForestGreen;
            this.btnComplete.FlatAppearance.BorderSize = 2;
            this.btnComplete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComplete.Font = new System.Drawing.Font("Calibri", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComplete.ForeColor = System.Drawing.Color.ForestGreen;
            this.btnComplete.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnComplete.Location = new System.Drawing.Point(433, 34);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnComplete.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnComplete.Size = new System.Drawing.Size(185, 113);
            this.btnComplete.TabIndex = 14;
            this.btnComplete.Text = "ENTREGADO";
            this.btnComplete.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnComplete.UseVisualStyleBackColor = false;
            this.btnComplete.Visible = false;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // btnDone
            // 
            this.btnDone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDone.BackgroundImage = global::CloverPro.Properties.Resources.if_Process_Accept;
            this.btnDone.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDone.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDone.FlatAppearance.BorderColor = System.Drawing.Color.ForestGreen;
            this.btnDone.FlatAppearance.BorderSize = 2;
            this.btnDone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDone.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDone.ForeColor = System.Drawing.Color.ForestGreen;
            this.btnDone.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDone.Location = new System.Drawing.Point(433, 35);
            this.btnDone.Name = "btnDone";
            this.btnDone.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnDone.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnDone.Size = new System.Drawing.Size(185, 113);
            this.btnDone.TabIndex = 11;
            this.btnDone.Text = "LISTO";
            this.btnDone.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDone.UseVisualStyleBackColor = false;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnFinish.BackgroundImage = global::CloverPro.Properties.Resources.rechazar;
            this.btnFinish.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFinish.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFinish.FlatAppearance.BorderColor = System.Drawing.Color.IndianRed;
            this.btnFinish.FlatAppearance.BorderSize = 2;
            this.btnFinish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFinish.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinish.ForeColor = System.Drawing.Color.MistyRose;
            this.btnFinish.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFinish.Location = new System.Drawing.Point(433, 35);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnFinish.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnFinish.Size = new System.Drawing.Size(185, 113);
            this.btnFinish.TabIndex = 8;
            this.btnFinish.Text = "INSUFICIENTE";
            this.btnFinish.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFinish.UseVisualStyleBackColor = false;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // btnAsignar
            // 
            this.btnAsignar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnAsignar.BackgroundImage = global::CloverPro.Properties.Resources.contact_list;
            this.btnAsignar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAsignar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAsignar.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnAsignar.FlatAppearance.BorderSize = 2;
            this.btnAsignar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAsignar.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAsignar.ForeColor = System.Drawing.Color.MistyRose;
            this.btnAsignar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAsignar.Location = new System.Drawing.Point(433, 35);
            this.btnAsignar.Name = "btnAsignar";
            this.btnAsignar.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnAsignar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnAsignar.Size = new System.Drawing.Size(185, 113);
            this.btnAsignar.TabIndex = 9;
            this.btnAsignar.Text = "ASIGNAR";
            this.btnAsignar.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAsignar.UseVisualStyleBackColor = false;
            this.btnAsignar.Visible = false;
            this.btnAsignar.Click += new System.EventHandler(this.btnAsignar_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 272);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(665, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(306, 17);
            this.toolStripStatusLabel1.Text = "Seleccione la Acción Requerida para su Departamento";
            // 
            // wfActividEstatusPop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 294);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "wfActividEstatusPop";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Activated += new System.EventHandler(this.wfActividEstatusPop_Activated);
            this.Load += new System.EventHandler(this.wfActividEstatusPop_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.wfActividEstatusPop_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNota;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Button btnAsignar;
        private System.Windows.Forms.Button btnEspera;
        private System.Windows.Forms.Button btnStoped;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnComplete;
    }
}