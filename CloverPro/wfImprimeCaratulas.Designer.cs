namespace CloverPro
{
    partial class wfImprimeCaratulas
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
            this.lbl_Tarimas = new System.Windows.Forms.Label();
            this.nud_Cantidad = new System.Windows.Forms.NumericUpDown();
            this.btn_ImpTarima = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Cantidad)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_Tarimas
            // 
            this.lbl_Tarimas.AutoSize = true;
            this.lbl_Tarimas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Tarimas.Location = new System.Drawing.Point(47, 9);
            this.lbl_Tarimas.Name = "lbl_Tarimas";
            this.lbl_Tarimas.Size = new System.Drawing.Size(149, 20);
            this.lbl_Tarimas.TabIndex = 0;
            this.lbl_Tarimas.Text = "Cantidad Tarimas";
            // 
            // nud_Cantidad
            // 
            this.nud_Cantidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_Cantidad.Location = new System.Drawing.Point(60, 45);
            this.nud_Cantidad.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nud_Cantidad.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Cantidad.Name = "nud_Cantidad";
            this.nud_Cantidad.Size = new System.Drawing.Size(120, 22);
            this.nud_Cantidad.TabIndex = 1;
            this.nud_Cantidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_Cantidad.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Cantidad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nud_Cantidad_KeyPress);
            // 
            // btn_ImpTarima
            // 
            this.btn_ImpTarima.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ImpTarima.Location = new System.Drawing.Point(70, 82);
            this.btn_ImpTarima.Name = "btn_ImpTarima";
            this.btn_ImpTarima.Size = new System.Drawing.Size(100, 25);
            this.btn_ImpTarima.TabIndex = 2;
            this.btn_ImpTarima.Text = "Imprimir";
            this.btn_ImpTarima.UseVisualStyleBackColor = true;
            this.btn_ImpTarima.Click += new System.EventHandler(this.btn_ImpTarima_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 118);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(254, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Blue;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "Presione Esc Para Salir.";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(105, 17);
            this.toolStripStatusLabel2.Text = "Max 30 Etiquetas.";
            // 
            // wfImprimeCaratulas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(254, 140);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btn_ImpTarima);
            this.Controls.Add(this.nud_Cantidad);
            this.Controls.Add(this.lbl_Tarimas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "wfImprimeCaratulas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Caratulas";
            this.Load += new System.EventHandler(this.wfImprimeCaratulas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nud_Cantidad)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Tarimas;
        private System.Windows.Forms.NumericUpDown nud_Cantidad;
        private System.Windows.Forms.Button btn_ImpTarima;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
    }
}