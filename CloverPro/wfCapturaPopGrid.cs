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
    public partial class wfCapturaPopGrid : Form
    {
        public string _lsProceso;
        public string _sClave;
        public string _sIndStd;
        public string _sTipoCore;
        
        public wfCapturaPopGrid(string asProceso)
        {
            InitializeComponent();
            _lsProceso = asProceso;
        }

        private void txtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _sClave = null;
                Close();
            }

            if (e.KeyCode != Keys.Enter)
                return;
            
        }

        private void wfCapturaPopGrid_Load(object sender, EventArgs e)
        {
            CargarModelos(_lsProceso);

        }

        private void CargarModelos(string _asModelo)
        {
            try
            {
                ModerelaLogica modre = new ModerelaLogica();
                modre.Moderela = _asModelo;
                DataTable dtRel = ModerelaLogica.ConsultaRelacionado(modre);
                dgwData.DataSource = dtRel;
                ColumnasGrid();

                if (dtRel.Rows.Count == 1)
                    Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            
        }

        private void ColumnasGrid()
        {
            int iRows = dgwData.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new System.Data.DataTable("Moderela");
                dtNew.Columns.Add("CORE", typeof(string));
                dtNew.Columns.Add("LAYOUT", typeof(int));
                dtNew.Columns.Add("MODELO", typeof(string));
                dtNew.Columns.Add("ind_formatostd", typeof(string));
                dgwData.DataSource = dtNew;
            }

            dgwData.Columns[3].Visible = false;

            dgwData.Columns[0].Width = ColumnWith(dgwData, 20);
            dgwData.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[1].Width = ColumnWith(dgwData, 35);
            dgwData.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[2].Width = ColumnWith(dgwData, 45);
            dgwData.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

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

        private void dgwData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /*
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightSkyBlue;
            else
                e.CellStyle.BackColor = Color.White;

            if (e.Value != null)
            {
                e.Value = e.Value.ToString().ToUpper();
                e.FormattingApplied = true;
            }*/
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _sTipoCore = string.Empty;
            _sClave = string.Empty;
            _sIndStd = string.Empty;
            
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgwData_SelectionChanged(object sender, EventArgs e)
        {
            int iRow = dgwData.CurrentRow.Index;
            DataGridViewRow row = dgwData.Rows[iRow];

            if (!string.IsNullOrEmpty(row.Cells[1].Value.ToString()))
            {
                _sTipoCore = row.Cells[0].Value.ToString();
                _sClave = row.Cells[1].Value.ToString();
                _sIndStd = row.Cells[3].Value.ToString();
            }
            else
            {
                _sTipoCore = string.Empty;
                _sClave = string.Empty;
                _sIndStd = string.Empty;
            }
        }
    }
}
