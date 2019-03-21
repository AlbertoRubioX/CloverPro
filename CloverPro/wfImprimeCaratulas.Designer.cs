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
            ((System.ComponentModel.ISupportInitialize)(this.nud_Cantidad)).BeginInit();
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
            2000,
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
            // wfImprimeCaratulas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(242, 124);
            this.Controls.Add(this.btn_ImpTarima);
            this.Controls.Add(this.nud_Cantidad);
            this.Controls.Add(this.lbl_Tarimas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "wfImprimeCaratulas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Caratulas";
            this.Load += new System.EventHandler(this.wfImprimeCaratulas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nud_Cantidad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Tarimas;
        private System.Windows.Forms.NumericUpDown nud_Cantidad;
        private System.Windows.Forms.Button btn_ImpTarima;
    }
}