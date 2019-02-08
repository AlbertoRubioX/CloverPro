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
    public partial class wfCapturaPopGridSetup : Form
    {
        public string _lsProceso;
        public string _sClave;
        public string _sCorreo;
        public string _sIndStd;
        public string _sTipoCore;
        public long _lFolio;
        public int _iCons;
        public string _sLinea;
        public string _sEstatus;

        public wfCapturaPopGridSetup(string asProceso)
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

        private void wfCapturaPopGridSetup_Load(object sender, EventArgs e)
        {
            if(_lsProceso == "PLA030")
            {
                CargarContactos();
            }
            if (_lsProceso == "EMP040")
            {
                panel1.BackColor = Color.Red;
                dgwData.Enabled = false;
                this.ControlBox = true;
                btnAceptar.Visible = false;
                btnCancel.Visible = false;
                label1.Visible = true;
                label1.Text = "No puede saltar RPO pendientes"+ Environment.NewLine +"Favor de procesar segun el Orden del Listado";
                toolstr1.Text = "Validación de Secuencia de RPO's";
                ListarRpoPendientes();
            }
                
            
        }

        private void ListarRpoPendientes()
        {
            try
            {
                ControlRpoLogica rpo = new ControlRpoLogica();
                rpo.Folio = _lFolio;
                rpo.Consec = _iCons;
                rpo.Linea = _sLinea;
                rpo.Estatus = _sEstatus;
                DataTable dt = ControlRpoLogica.ValidaPendienteSP(rpo);
                dgwData.DataSource = dt;
                ColumnasGridRpo();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void CargarContactos()
        {
            try
            {
                AreasDestLogica det = new AreasDestLogica();
                det.Area = _sClave;
                det.Envio = "0";
                DataTable dtRel = AreasDestLogica.Listar(det);

                if(dtRel.Rows.Count == 0)
                {
                    MessageBox.Show("No hay contactos configurados para Asignar", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }

                dgwData.DataSource = dtRel;
                ColumnasGrid();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            
        }
        private void ColumnasGridRpo()
        {
            int iRows = dgwData.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("rpo");
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("TURNO", typeof(string));
                dtNew.Columns.Add("RPO", typeof(string));
                dtNew.Columns.Add("MODELO", typeof(string));
                dtNew.Columns.Add("CANT", typeof(string));
                dtNew.Columns.Add("PRIORIDAD", typeof(string));
                dgwData.DataSource = dtNew;
            }
            dgwData.Columns[0].Visible = false;

            dgwData.Columns[1].Width = ColumnWith(dgwData, 10);
            dgwData.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[2].Width = ColumnWith(dgwData, 30);
            dgwData.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[3].Width = ColumnWith(dgwData, 30);
            dgwData.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[4].Width = ColumnWith(dgwData, 10);
            dgwData.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[5].Width = ColumnWith(dgwData, 20);
            dgwData.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private void ColumnasGrid()
        {
            int iRows = dgwData.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("Moderela");
                dtNew.Columns.Add("area", typeof(string));
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("CORREO", typeof(string));
                dtNew.Columns.Add("TURNO", typeof(string));
                dtNew.Columns.Add("tipo", typeof(string));
                dtNew.Columns.Add("asigna_auto", typeof(string));
                dgwData.DataSource = dtNew;
            }
            dgwData.Columns[0].Visible = false;
            dgwData.Columns[1].Visible = false;
            dgwData.Columns[4].Visible = false;
            dgwData.Columns[5].Visible = false;

            dgwData.Columns[2].Width = ColumnWith(dgwData, 80);
            dgwData.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[3].Width = ColumnWith(dgwData, 20);
            dgwData.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _sCorreo = string.Empty;

            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgwData_SelectionChanged(object sender, EventArgs e)
        {
            if (_lsProceso == "EMP040")
                return;

            int iRow = dgwData.CurrentRow.Index;
            DataGridViewRow row = dgwData.Rows[iRow];

            if (!string.IsNullOrEmpty(row.Cells[2].Value.ToString()))
            {
                _sCorreo = row.Cells[2].Value.ToString();
            }
            else
            {
                _sCorreo = string.Empty;
            }
        }

        private void wfCapturaPopGridSetup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void dgwData_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void dgwData_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
