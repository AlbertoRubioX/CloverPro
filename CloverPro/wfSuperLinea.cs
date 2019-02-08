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
    public partial class wfSuperLinea : Form
    {
        public bool _lbCambio;
        private string _lsProceso = "CAT070";
        public wfSuperLinea()
        {
            InitializeComponent();
        }
        #region regInicio
        private void wfSuperLinea_Load(object sender, EventArgs e)
        {
            Inicio();
        }


        private void Inicio()
        {
            _lbCambio = false;

            cbbPlanta.ResetText();
            DataTable data = PlantaLogica.Listar();
            cbbPlanta.DataSource = data;
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.SelectedIndex = 0;

            Dictionary<string, string> Turno = new Dictionary<string, string>();
            Turno.Add("1", "1");
            Turno.Add("2", "2");
            cbbTurno.DataSource = new BindingSource(Turno, null);
            cbbTurno.DisplayMember = "Value";
            cbbTurno.ValueMember = "Key";
            cbbTurno.SelectedIndex = 0;

            cbbArea.ResetText();
            Dictionary<string, string> Area = new Dictionary<string, string>();
            Area.Add("SUP", "Supervisor");
            Area.Add("ING", "Ingeniería");
            Area.Add("CAL", "Calidad");
            cbbArea.DataSource = new BindingSource(Area, null);
            cbbArea.DisplayMember = "Value";
            cbbArea.ValueMember = "Key";
            cbbArea.SelectedIndex = -1;

            cbbSuper.ResetText();

            //DataTable dt = UsuarioLogica.ListarSuper();
            //cbbSuper.DataSource = dt;
            //cbbSuper.ValueMember = "usuario";
            //cbbSuper.DisplayMember = "nombre";
            //cbbSuper.SelectedIndex = -1;

            dgwLineas.Columns.Clear();
            //CargarLineas();

            cbbPlanta.Focus();
        }

        private void wfSuperLinea_Activated(object sender, EventArgs e)
        {
            cbbPlanta.Focus();
        }
        #endregion

        #region regLinea
        private void dgwLineas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightSkyBlue;
            else
                e.CellStyle.BackColor = Color.White;
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
            if (dgwLineas.Rows.Count == 0)
            {
                DataTable dtNew = new DataTable("Lineas");
                dtNew.Columns.Add("supevisor", typeof(string));
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("Planta", typeof(string));
                dtNew.Columns.Add("Linea", typeof(string));
                dtNew.Columns.Add("Turno", typeof(string));
                dtNew.Columns.Add("area", typeof(string));
                dgwLineas.DataSource = dtNew;
            }

            dgwLineas.Columns[0].Visible = false;
            dgwLineas.Columns[1].Visible = false;
            dgwLineas.Columns[5].Visible = false;

            dgwLineas.Columns[2].Width = ColumnWith(dgwLineas, 40);
            dgwLineas.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwLineas.Columns[3].Width = ColumnWith(dgwLineas, 45);
            dgwLineas.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwLineas.Columns[4].Width = ColumnWith(dgwLineas, 10);
            dgwLineas.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        #endregion

        #region regGuardar
        private bool Guardar()
        {
            if (Valida())
            {

                try
                {
                    foreach(DataGridViewRow row in dgwLineas.Rows)
                    {
                        SuperLineaLogica sup = new SuperLineaLogica();
                        sup.Supervisor = cbbSuper.SelectedValue.ToString();
                        sup.Consec = Convert.ToInt16(row.Cells[1].Value.ToString());
                        sup.Planta = row.Cells[2].Value.ToString();
                        sup.Linea = row.Cells[3].Value.ToString();
                        sup.Turno = row.Cells[4].Value.ToString();
                        sup.Area = row.Cells[5].Value.ToString();
                        SuperLineaLogica.Guardar(sup);
                    }
                    return true;
                }
                catch (Exception ie)
                {
                    MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "EstacionLogica.Guardar(est)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
                return false;
        }
        private bool Valida()
        {
            bool bValida = false;

            if (!_lbCambio)
                return bValida;

            if (cbbSuper.SelectedIndex == -1)
            {
                cbbSuper.Focus();
                return bValida;
            }

            if(dgwLineas.RowCount == 0)
            {
                MessageBox.Show("No se han cargado Lineas al Supervisor", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgwLineas.Focus();
                return bValida;
            }
            
            return true;
        }
        #endregion

        #region regBotones
        private void btSave_Click(object sender, EventArgs e)
        {
            if (Guardar())
                CargarLineas();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void btNew_Click(object sender, EventArgs e)
        {
            Inicio();
        }
        private void btRemove_Click(object sender, EventArgs e)
        {
            if (cbbSuper.SelectedIndex == -1)
                return;

            if (dgwLineas.SelectedRows.Count == 0)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "30") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult Result = MessageBox.Show("Desea Eliminar la Linea del Supervisor?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                try
                {
                    string sSuper = dgwLineas.SelectedCells[0].Value.ToString();
                    if (!string.IsNullOrEmpty(sSuper))
                    {
                        SuperLineaLogica sup = new SuperLineaLogica();
                        sup.Supervisor = sSuper;
                        sup.Consec = Convert.ToInt16(dgwLineas.SelectedCells[1].Value.ToString());

                        SuperLineaLogica.Eliminar(sup);
                    }
                    dgwLineas.Rows.Remove(dgwLineas.CurrentRow);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cbbSuper.SelectedIndex == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult Result = MessageBox.Show("Desea borrar al Supervisor?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                try
                {
                    string sSuper = cbbSuper.SelectedValue.ToString();

                    SuperLineaLogica sup = new SuperLineaLogica();
                    sup.Supervisor = sSuper;

                    SuperLineaLogica.Borrar(sup);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

            }
        }
        #endregion
       
        #region regCaptura
        private void cbbSuper_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbSuper, 1);
        }

        private void cbbSuper_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbSuper, 0);
        }

        private void cbbEstacion_KeyDown(object sender, KeyEventArgs e)
        {
        
        }

        private void cbbEstacion_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SuperLineaLogica sup = new SuperLineaLogica();
            sup.Supervisor = cbbSuper.SelectedValue.ToString();
            DataTable datos = SuperLineaLogica.Listar(sup);
            if (datos.Rows.Count != 0)
                dgwLineas.DataSource = datos;
            else
                dgwLineas.Columns.Clear();
            
            CargarLineas();
        }
        
        private void AgregaLinea(string _asPlanta, string _asLinea, string _asTurno, string _asArea)
        {
            DataTable dt = dgwLineas.DataSource as DataTable;
            dt.Rows.Add(cbbSuper.SelectedValue.ToString(), 0, _asPlanta, _asLinea, _asTurno, _asArea);
            _lbCambio = true;
        }
        private void btnLineas_Click(object sender, EventArgs e)
        {
            if (cbbSuper.SelectedIndex == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "40") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            wfLineasPta LineP = new wfLineasPta(cbbSuper.SelectedValue.ToString());
            LineP.ShowDialog();
            if(LineP._dtReturn.Rows.Count > 0)
            {
                DataTable dtR = LineP._dtReturn;
                for(int x = 0; x < dtR.Rows.Count; x ++)
                {
                    bool bAdd = true;
                    string sPlanta = dtR.Rows[x][2].ToString();
                    string sLinea = dtR.Rows[x][3].ToString();
                    string sTurno = dtR.Rows[x][4].ToString();
                    string sArea = dtR.Rows[x][5].ToString();
                    foreach(DataGridViewRow row in dgwLineas.Rows)
                    {
                        if (sPlanta == row.Cells[2].Value.ToString() && sLinea == row.Cells[3].Value.ToString() && sTurno == row.Cells[4].Value.ToString())
                        {
                            bAdd = false;
                            break;
                        }
                    }

                    if(bAdd)
                        AgregaLinea(sPlanta, sLinea,sTurno,sArea);
                }
            }
        }
        #endregion

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbbArea.SelectedIndex != -1)
            {
                cbbSuper.ResetText();

                UsuarioLogica user = new UsuarioLogica();
                user.Planta = cbbPlanta.SelectedValue.ToString();
                user.Turno = cbbTurno.SelectedValue.ToString();
                user.Area = cbbArea.SelectedValue.ToString();

                DataTable dt = UsuarioLogica.ListarSuperParam(user);
                cbbSuper.DataSource = dt;
                cbbSuper.ValueMember = "usuario";
                cbbSuper.DisplayMember = "nombre";
                cbbSuper.SelectedIndex = -1;
            }
            else
                cbbSuper.SelectedIndex = -1;

            dgwLineas.Columns.Clear();
        }

        private void cbbTurno_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(cbbPlanta.SelectedIndex != -1 && cbbArea.SelectedIndex != -1)
            {
                cbbSuper.ResetText();

                UsuarioLogica user = new UsuarioLogica();
                user.Planta = cbbPlanta.SelectedValue.ToString();
                user.Turno = cbbTurno.SelectedValue.ToString();
                user.Area = cbbArea.SelectedValue.ToString();

                DataTable dt = UsuarioLogica.ListarSuperParam(user);
                cbbSuper.DataSource = dt;
                cbbSuper.ValueMember = "usuario";
                cbbSuper.DisplayMember = "nombre";
                cbbSuper.SelectedIndex = -1;

                dgwLineas.Columns.Clear();
            }
        }

        private void cbbArea_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(cbbPlanta.SelectedIndex != -1)
            {
                cbbSuper.ResetText();

                UsuarioLogica user = new UsuarioLogica();
                user.Planta = cbbPlanta.SelectedValue.ToString();
                user.Turno = cbbTurno.SelectedValue.ToString();
                user.Area = cbbArea.SelectedValue.ToString();

                DataTable dt = UsuarioLogica.ListarSuperParam(user);
                cbbSuper.DataSource = dt;
                cbbSuper.ValueMember = "usuario";
                cbbSuper.DisplayMember = "nombre";
                cbbSuper.SelectedIndex = -1;

                dgwLineas.Columns.Clear();
                
            }
        }

        private void cbbPlanta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void cbbPlanta_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
