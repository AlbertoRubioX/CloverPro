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
    public partial class wfRepTransferencia : Form
    {
        private string _lsProceso = "REP060";
        public bool _lbCambio;
        public wfRepTransferencia()
        {
            InitializeComponent();
        }

        #region regInicio
        private void wfRepEtiquetas_Load(object sender, EventArgs e)
        {
            Inicio();
            
        }
        private void wfRepEtiquetas_Activated(object sender, EventArgs e)
        {
            dtpInicio.Focus();
        }
        private void Inicio()
        {
            _lbCambio = false;

            cbbSuper.ResetText();
            DataTable dtSup = UsuarioLogica.ListarSuper();
            cbbSuper.DataSource = dtSup;
            cbbSuper.DisplayMember = "nombre";
            cbbSuper.ValueMember = "usuario";
            cbbSuper.SelectedValue = GlobalVar.gsUsuario;

            cbbPlanta.ResetText();
            DataTable data = PlantaLogica.Listar();
            cbbPlanta.DataSource = data;
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.SelectedValue = GlobalVar.gsPlanta;


            chbTodos.Checked = false;

            if (GlobalVar.gsTurno == "2")
            {
                rbtTuno1.Checked = false;
                rbtTuno2.Checked = true;
            }
            else
            {
                rbtTuno1.Checked = true;
                rbtTuno2.Checked = false;
            }


            dtpInicio.ResetText();
            dtpFinal.ResetText();

            dgwLinea.DataSource = null;
            dgwOper.DataSource = null;

            ColumnasLinea();

            CargarLineas();

            dtpInicio.Focus();

        }
        private void ColumnasLinea()
        {
            int iRows = dgwLinea.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new System.Data.DataTable("Lineas");
                dtNew.Columns.Add("supervisor", typeof(string));
                dtNew.Columns.Add("consec", typeof(string));
                dtNew.Columns.Add("planta", typeof(string));
                dtNew.Columns.Add("LINEA", typeof(string));
                dtNew.Columns.Add("turno", typeof(string));
                dgwLinea.DataSource = dtNew;
            }

            dgwLinea.Columns[0].Visible = false;
            dgwLinea.Columns[1].Visible = false;
            dgwLinea.Columns[2].Visible = false;
            dgwLinea.Columns[4].Visible = false;

            dgwLinea.Columns[3].Width = ColumnWith(dgwLinea, 100);
            dgwLinea.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwLinea.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }
        private void ColumnasOper()
        {
            int iRows = dgwOper.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new System.Data.DataTable("Empleados");
                dtNew.Columns.Add("NUMERO", typeof(string));
                dtNew.Columns.Add("NOMBRE", typeof(string));
                dtNew.Columns.Add("activo", typeof(string));
                dtNew.Columns.Add("turno", typeof(string));
                dtNew.Columns.Add("planta", typeof(string));
                dtNew.Columns.Add("linea", typeof(string));
                dtNew.Columns.Add("nivel", typeof(string));
                dtNew.Columns.Add("u_id", typeof(string));
                dtNew.Columns.Add("f_id", typeof(DateTime));
                dtNew.Columns.Add("f_ingreso", typeof(DateTime));

                dgwOper.DataSource = dtNew;
            }

            dgwOper.Columns[2].Visible = false;
            dgwOper.Columns[3].Visible = false;
            dgwOper.Columns[4].Visible = false;
            dgwOper.Columns[5].Visible = false;
            dgwOper.Columns[7].Visible = false;
            dgwOper.Columns[8].Visible = false;
            dgwOper.Columns[9].Visible = false;

            dgwOper.Columns[0].Width = ColumnWith(dgwOper, 5);
            dgwOper.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwOper.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwOper.Columns[1].Width = ColumnWith(dgwOper, 10);
            dgwOper.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwOper.Columns[2].Width = ColumnWith(dgwOper, 80);
            dgwOper.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwOper.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
        private void CargarLineas()
        {
            try
            {
                if (cbbSuper.SelectedIndex == -1)
                    return;

                SuperLineaLogica sup = new SuperLineaLogica();
                sup.Supervisor = cbbSuper.SelectedValue.ToString();
                DataTable dtLs = SuperLineaLogica.Listar(sup);
                dgwLinea.DataSource = dtLs;
                if (dgwLinea.Rows.Count != 0)
                {
                    dgwLinea.Rows[0].Selected = true;
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador." + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }

        private void CargarOper(string _asLinea)
        {
            try
            {
                
                UsuarioLogica user = new UsuarioLogica();
                user.Usuario = cbbSuper.SelectedValue.ToString();
                DataTable dtUs = UsuarioLogica.Consultar(user);
                string sTurno = dtUs.Rows[0]["turno"].ToString();

                OperadorLogica oper = new OperadorLogica();
                oper.Planta = cbbPlanta.SelectedValue.ToString();
                oper.Linea = _asLinea;
                oper.Turno = sTurno;

                CargarDetalle(oper);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador." + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void CargarDetalle(OperadorLogica oper)
        {
            dgwOper.Columns.Clear();

            DataTable Lista = new DataTable();
            Lista = OperadorLogica.ConsultarLinea(oper);
            dgwOper.DataSource = Lista;

            DataGridViewCheckBoxColumn dgwChb = new DataGridViewCheckBoxColumn();
            dgwChb.ValueType = typeof(bool);
            dgwChb.Name = "ind";
            dgwChb.HeaderText = "";
            dgwOper.Columns.Insert(0, dgwChb);

            dgwOper.Columns[3].Visible = false;
            dgwOper.Columns[4].Visible = false;
            dgwOper.Columns[5].Visible = false;
            dgwOper.Columns[6].Visible = false;
            dgwOper.Columns[7].Visible = false;
            dgwOper.Columns[8].Visible = false;
            dgwOper.Columns[9].Visible = false;
            dgwOper.Columns[10].Visible = false;

            dgwOper.Columns[0].Width = ColumnWith(dgwOper, 5);
            dgwOper.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwOper.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwOper.Columns[1].Width = ColumnWith(dgwOper, 20);
            dgwOper.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwOper.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwOper.Columns[2].Width = ColumnWith(dgwOper, 75);
            dgwOper.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            for (int i = 1; i < dgwOper.Columns.Count; i++)
            {
                dgwOper.Columns[i].ReadOnly = true;
            }

            chbTodos.Checked = false;
            dgwOper.Focus();

        }
        #endregion

        private bool Valida()
        {
            bool bValida = false;

            if (string.IsNullOrEmpty(dtpInicio.Text) || string.IsNullOrEmpty(dtpFinal.Text))
            {
                MessageBox.Show("Favor de especificar el Periódo del reporte", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpInicio.Focus();
                return bValida;
            }
            else
            {
                if(dtpInicio.Value > dtpFinal.Value)
                {
                    MessageBox.Show("La Fecha Inicial no debe ser mayor a la Fecha Final", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtpFinal.Focus();
                    return bValida;
                }
            }
            return true;

        }



        #region regBotones
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Inicio();
        }
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            wfImpresor Impresor = new wfImpresor();
            Impresor._lsProceso = _lsProceso;

            Impresor._ldtFinal = dtpFinal.Value;
            Impresor._ldtInicio = dtpInicio.Value;
            if(cbbPlanta.SelectedIndex != -1)
                Impresor._lsPlanta = cbbPlanta.SelectedValue.ToString();
            if (rbtTuno1.Checked)
                Impresor._lsTurno = "1";
            else
                Impresor._lsTurno = "2";

            string sLinea = dgwLinea[3, dgwLinea.CurrentCell.RowIndex].Value.ToString();
            Impresor._lsLineaIni = sLinea;

            string sOper = CheckOper();
            if (!string.IsNullOrEmpty(sOper))
                Impresor._lsEmpleado = "," + sOper + ",";

            Impresor._lsSupervisor = cbbSuper.Text.ToString().ToUpper();

            Impresor.Show();
        }
        private string CheckOper()
        {
            string sOper = string.Empty;

            foreach(DataGridViewRow row in dgwOper.Rows)
            {
                bool bSelect = Convert.ToBoolean(row.Cells["ind"].Value);
                if (!bSelect)
                    continue;

                string sCve = Convert.ToString(row.Cells[1].Value);
                if (string.IsNullOrEmpty(sOper))
                    sOper = sCve;
                else
                    sOper += "," + sCve;
            }

            return sOper;
        }
        #endregion

    
        private void dtpInicio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
        private void cbbPlanta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbbPlanta.Text.Length >= 9)
                e.Handled = true;
        }

        private void txtFolioIni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtFolioFin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CargarLineas();
        }

        private void rbtTuno1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtTuno1.Checked)
                rbtTuno2.Checked = false;
        }

        private void rbtTuno2_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtTuno2.Checked)
                rbtTuno1.Checked = false;
        }

       

        private void dgwLinea_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.WhiteSmoke;
            else
                e.CellStyle.BackColor = Color.White;
        }

        private void dgwOper_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightGreen;
            else
                e.CellStyle.BackColor = Color.White;
        }

        private void cbbSuper_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                UsuarioLogica user = new UsuarioLogica();
                user.Usuario = cbbSuper.SelectedValue.ToString();
                DataTable data = UsuarioLogica.Consultar(user);
                if (data.Rows.Count != 0)
                    cbbPlanta.SelectedValue = data.Rows[0]["planta"].ToString();

                if (cbbPlanta.SelectedIndex == -1)
                    return;

                CargarLineas();


            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador." + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }

        private void dgwLinea_Click(object sender, EventArgs e)
        {
            if (dgwLinea.CurrentRow == null)
                return;

            int iRow = dgwLinea.CurrentRow.Index;
            DataGridViewRow row = dgwLinea.Rows[iRow];

            if (!string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
            {
                string sLinea = row.Cells[3].Value.ToString();
                CargarOper(sLinea);

            }
            else
            {
                dgwOper.DataSource = null;
            }
            
        }

        private void dgwLinea_SelectionChanged(object sender, EventArgs e)
        {
            dgwLinea_Click(sender, e);
        }

        private void chbTodos_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgwOper.Rows)
            {
                row.Cells["ind"].Value = chbTodos.Checked;
            }
        }
    }
}
