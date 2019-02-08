using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logica;

namespace CloverPro
{
    public partial class wfHistorico : Form
    {
        public bool _lbCambio;
        public wfHistorico()
        {
            InitializeComponent();
        }

        private void wfHistorico_Load(object sender, EventArgs e)
        {
            Inicio();
        }

       
        private void Inicio()
        {
            txtRpo.Clear();
            txtModelo.Clear();
            dgwBarcode.DataSource = null;
            CargarColumnas();
        }

        private int ColumnWith(DataGridView _dtGrid, double _dColWith)
        {
            double dW = _dtGrid.Width - 10;
            double dTam = _dColWith;
            double dPor = dTam / 100;
            dTam = dW * dPor;
            dTam = Math.Truncate(dTam);

            return Convert.ToInt32(dTam);
        }
        private void CargarColumnas()
        {
            if (dgwBarcode.Rows.Count == 0)
            {

                DataTable dtNew = new DataTable("Serv");
                dtNew.Columns.Add("Planta", typeof(string));
                dtNew.Columns.Add("Usuario", typeof(string));
                dtNew.Columns.Add("Folio", typeof(int));
                dtNew.Columns.Add("Fecha", typeof(DateTime));
                dtNew.Columns.Add("Cantidad", typeof(int));
                dtNew.Columns.Add("Inicio", typeof(int));
                dtNew.Columns.Add("Fin", typeof(int));
                dtNew.Columns.Add("modelo", typeof(string));
                dgwBarcode.DataSource = dtNew;

            }

            dgwBarcode.Columns[0].Width = ColumnWith(dgwBarcode, 15);
            dgwBarcode.Columns[1].Width = ColumnWith(dgwBarcode, 15);
            dgwBarcode.Columns[2].Width = ColumnWith(dgwBarcode, 8);
            dgwBarcode.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwBarcode.Columns[3].Width = ColumnWith(dgwBarcode, 30);
            dgwBarcode.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwBarcode.Columns[4].Width = ColumnWith(dgwBarcode, 12);
            dgwBarcode.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwBarcode.Columns[5].Width = ColumnWith(dgwBarcode, 10);
            dgwBarcode.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwBarcode.Columns[6].Width = ColumnWith(dgwBarcode, 10);
            dgwBarcode.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwBarcode.Columns[7].Visible = false;
        }
        private void wfHistorico_Activated(object sender, EventArgs e)
        {
            txtRpo.Focus();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
          
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

    

     

        private void btRemove_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Inicio();
        }

      

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRpo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtRpo.Text) && !string.IsNullOrWhiteSpace(txtRpo.Text))
            {
                string sRPO = txtRpo.Text.ToString().Trim();

                if (sRPO.IndexOf("RPO") == -1)
                    sRPO = "RPO" + sRPO;

                int iPos = sRPO.IndexOf("-");
                if (iPos > 0)
                {
                    sRPO = sRPO.Substring(0, iPos);
                }

                txtRpo.Text = sRPO;

                EtiquetaLogica eti = new EtiquetaLogica();
                eti.RPO = sRPO;
                
                DataTable datos = EtiquetaLogica.Listar(eti);
                if (datos.Rows.Count != 0)
                {
                    txtModelo.Text = datos.Rows[0][7].ToString();
                    dgwBarcode.DataSource = datos;
                    CargarColumnas();
                }
                else
                {
                    MessageBox.Show("No se encuentra regisrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Inicio();
                    return;
                }
            }
        }
    }
}
