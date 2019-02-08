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
    public partial class wfPlantas : Form
    {
        public bool _lbCambio;
        public bool _lbCambioDet;
        private string _lsProceso = "CAT030";
        private string _lsNomAnt;
        public wfPlantas()
        {
            InitializeComponent();
        }

        #region regInicio
        private void wfPlantas_Load(object sender, EventArgs e)
        {
            Inicio();
            
        }
        private void Inicio()
        {
            _lbCambio = false;
            _lbCambioDet = false;

            cbbPlanta.ResetText();
            DataTable data = PlantaLogica.Listar();
            cbbPlanta.DataSource = data;
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "planta";
            cbbPlanta.SelectedIndex = -1;

            txtNombre.Clear();
            _lsNomAnt = string.Empty;
            CargarSuper();
            CargarLineas();
            tabControl1.SelectedIndex = 0;

            cbbPlanta.Focus();

        }
        

        private void wfPlantas_Activated(object sender, EventArgs e)
        {
            cbbPlanta.Focus();
        }

        #endregion

        #region regGuardar
        private bool Guardar()
        {
            if (Valida())
            {

                try
                {
                    PlantaLogica plan = new PlantaLogica();
                    plan.Planta = cbbPlanta.Text.ToString().ToUpper();
                    plan.Nombre = txtNombre.Text.ToString();
                    if (PlantaLogica.Guardar(plan) == 0)
                    {
                        MessageBox.Show("Error al intentar guardar la Planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    foreach(DataGridViewRow row in dgwSuper.Rows)
                    {
                        if (row.Index == dgwSuper.Rows.Count - 1)
                            break;

                        if (dgwLineas.IsCurrentRowDirty)
                            dgwLineas.CommitEdit(DataGridViewDataErrorContexts.Commit);

                        int iCons = 0;
                        if (!int.TryParse(row.Cells[1].Value.ToString(), out iCons))
                            iCons = 0;
                        
                        string sSuper = row.Cells[2].Value.ToString();
                        string sMail = row.Cells[3].Value.ToString();
                        string sTurno = row.Cells[4].Value.ToString();

                        PlantaSuperLogica ptas = new PlantaSuperLogica();
                        ptas.Planta = cbbPlanta.SelectedValue.ToString();
                        ptas.Consec = iCons;
                        ptas.Nombre = sSuper;
                        ptas.Correo= sMail;
                        ptas.Turno = sTurno;
                        ptas.Usuario = GlobalVar.gsUsuario;

                        if(PlantaSuperLogica.Guardar(ptas) == -1)
                        {
                            MessageBox.Show("Error al intentar guardar el supervisor " + sSuper, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                    }

                    foreach(DataGridViewRow row in dgwLineas.Rows)
                    {
                        if (row.Index == dgwLineas.Rows.Count - 1)
                            break;

                        if (dgwLineas.IsCurrentRowDirty)
                            dgwLineas.CommitEdit(DataGridViewDataErrorContexts.Commit);

                        string sPlanta = cbbPlanta.SelectedValue.ToString();
                        string sLine = Convert.ToString(row.Cells[1].Value);
                        string sNombre = Convert.ToString(row.Cells[2].Value);
                        string sLineaNav = Convert.ToString(row.Cells[3].Value);
                        string sTipo = Convert.ToString(row.Cells[4].Value);

                        LineaLogica line = new LineaLogica();
                        line.Planta = sPlanta;
                        line.Linea = sLine;
                        line.Nombre = sNombre;
                        line.LineaNav = sLineaNav;
                        line.Tipo = sTipo;

                        if(LineaLogica.Guardar(line) == -1)
                        { 
                            MessageBox.Show("Error al intentar guardar la linea " + sLine, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                    }
                    return true;
                }
                catch(Exception ie)
                {
                    MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "PlantaLogica.Guardar(plan)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
                return false;
        }

        private bool Valida()
        {
            bool bValida = false;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "10") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bValida;
            }

            if (string.IsNullOrEmpty(cbbPlanta.Text))
            {
                MessageBox.Show("No se a especificado la clave de la planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }

            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("No se a especificado el nombre de la planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Focus();
                return bValida;
            }

            foreach(DataGridViewRow row in dgwSuper.Rows)
            {
                if (row.Index == dgwSuper.Rows.Count - 1)
                    break;

                int iCons = 0;
                if (!int.TryParse(row.Cells[1].Value.ToString(), out iCons))
                {
                    row.Cells[1].Value = 0;
                }

                if (string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
                {
                    MessageBox.Show("Favor de proporcionar el correo del Supervisor", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgwSuper.Rows[row.Index].Cells[3].Selected = true;
                    return bValida;
                }

                int iTurno = 0;
                if (!int.TryParse(row.Cells[4].Value.ToString(), out iTurno))
                {
                    MessageBox.Show("El Turno es Incorrecto", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgwSuper.Rows[row.Index].Cells[4].Selected = true;
                    return bValida;
                }
                
            }

            return true;
        }
        #endregion

        #region regCaptura
        private void cbbPlanta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbbPlanta.Text.Length >= 9)
                e.Handled = true;
        }
        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                PlantaLogica plan = new PlantaLogica();
                plan.Planta = cbbPlanta.SelectedValue.ToString();
                DataTable datos = PlantaLogica.Consultar(plan);
                if (datos.Rows.Count != 0)
                {
                    txtNombre.Text = datos.Rows[0]["nombre"].ToString();

                    PlantaSuperLogica ptas = new PlantaSuperLogica();
                    ptas.Planta = cbbPlanta.SelectedValue.ToString();
                    dgwSuper.DataSource = PlantaSuperLogica.Listar(ptas);

                    LineaLogica lin = new LineaLogica();
                    lin.Planta = cbbPlanta.SelectedValue.ToString();
                    DataTable Lista = LineaLogica.Listar(lin);
                    dgwLineas.DataSource = Lista;

                    txtNombre.Focus();
                }
                CargarSuper();
                CargarLineas();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void cbbPlanta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            try
            {
                if (!string.IsNullOrEmpty(cbbPlanta.Text) && !string.IsNullOrWhiteSpace(cbbPlanta.Text))
                {
                    PlantaLogica plan = new PlantaLogica();
                    cbbPlanta.Text = cbbPlanta.Text.ToUpper().Trim().ToString();
                    plan.Planta = cbbPlanta.Text.ToString();
                    DataTable datos = PlantaLogica.Consultar(plan);
                    if (datos.Rows.Count != 0)
                    {
                        cbbPlanta.SelectedValue = datos.Rows[0]["planta"].ToString();
                        txtNombre.Text = datos.Rows[0]["nombre"].ToString();

                        PlantaSuperLogica ptas = new PlantaSuperLogica();
                        ptas.Planta = cbbPlanta.SelectedValue.ToString();
                        dgwSuper.DataSource = PlantaSuperLogica.Listar(ptas);

                        LineaLogica lin = new LineaLogica();
                        lin.Planta = cbbPlanta.SelectedValue.ToString();
                        DataTable Lista = LineaLogica.Listar(lin);
                        dgwLineas.DataSource = Lista;
                    }
                    else
                    {
                        txtNombre.Clear();
                        CargarSuper();
                        CargarLineas();
                    }
                    txtNombre.Focus();
                }
                else
                    cbbPlanta.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }
        #endregion

        #region regDatagrids
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
            
            if(dgwLineas.Rows.Count == 0)
            {
                DataTable dtNew = new DataTable("Lineas");
                dtNew.Columns.Add("planta", typeof(string));
                dtNew.Columns.Add("Linea", typeof(string));
                dtNew.Columns.Add("Nombre", typeof(string));
                dtNew.Columns.Add("Linea NAV", typeof(string));
                dtNew.Columns.Add("Tipo", typeof(string));
                dgwLineas.DataSource = dtNew;
            }
            
            dgwLineas.Columns[0].Visible = false;

            dgwLineas.Columns[1].Width = ColumnWith(dgwLineas, 20);
            dgwLineas.Columns[2].Width = ColumnWith(dgwLineas, 30);
            dgwLineas.Columns[3].Width = ColumnWith(dgwLineas, 30);
            dgwLineas.Columns[4].Width = ColumnWith(dgwLineas, 10);

        }

        private void dgwSuper_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightSkyBlue;
            else
                e.CellStyle.BackColor = Color.White;
        }

        private void CargarSuper()
        {

            if (dgwSuper.Rows.Count == 0)
            {
                DataTable dtNew = new DataTable("Super");
                dtNew.Columns.Add("planta", typeof(string));
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("Supevisor", typeof(string));
                dtNew.Columns.Add("Correo", typeof(string));
                dtNew.Columns.Add("Turno", typeof(string));
                dgwSuper.DataSource = dtNew;
            }

            dgwSuper.Columns[0].Visible = false;
            dgwSuper.Columns[1].Visible = false;

            dgwSuper.Columns[2].Width = ColumnWith(dgwSuper, 50);
            dgwSuper.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwSuper.Columns[3].Width = ColumnWith(dgwSuper, 30);
            dgwSuper.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwSuper.Columns[4].Width = ColumnWith(dgwSuper, 10);
            dgwSuper.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void dgwSuper_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void dgwSuper_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int iRow = dgwSuper.CurrentCell.RowIndex;
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    int iColumn = dgwSuper.CurrentCell.ColumnIndex;

                    if (iRow < dgwSuper.Rows.Count - 1)
                        dgwSuper.CurrentCell = dgwSuper[2, iRow + 1];
                    else
                        dgwSuper.CurrentCell = dgwSuper[2, 0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void dgwSuper_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    string sSuper = dgwSuper[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
                    if (!string.IsNullOrEmpty(sSuper) && !string.IsNullOrWhiteSpace(sSuper))
                    {
                        int iCont = 0;
                        for (int i = 0; i < dgwSuper.RowCount; i++)
                        {
                            if (i == dgwSuper.Rows.Count - 1)
                                break;

                            string sCve = dgwSuper[2, i].Value.ToString().ToUpper();
                            if (sSuper == sCve)
                                iCont++;
                        }

                        if (iCont > 1)
                        {
                            MessageBox.Show("Supervisor Repetido", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgwSuper[2, e.RowIndex].Value = string.Empty;
                            return;
                        }
                    }
                    else
                    {
                        dgwSuper[2, e.RowIndex].Value = string.Empty;
                        dgwSuper[3, e.RowIndex].Value = string.Empty;
                        dgwSuper[4, e.RowIndex].Value = string.Empty;
                    }
                }
                _lbCambioDet = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #endregion

        #region regBotones
        private void btSave_Click(object sender, EventArgs e)
        {
            if (!_lbCambio && !_lbCambioDet)
                return;

            if (Guardar())
                Close();
            else
                Inicio();
        }
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Inicio();
        }
        private void btRemove_Click(object sender, EventArgs e)
        {
            if (cbbPlanta.SelectedIndex == -1)
                return;

            if (dgwLineas.SelectedRows.Count == 0)
                return;

            if(tabControl1.SelectedIndex == 0)
            {
                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "30") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DialogResult Result = MessageBox.Show("Desea Eliminar la Linea?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    string sPlanta = dgwLineas.SelectedCells[0].Value.ToString();
                    string sLinea = dgwLineas.SelectedCells[1].Value.ToString();
                    if (!string.IsNullOrEmpty(sPlanta))
                    {
                        LineaLogica line = new LineaLogica();
                        line.Planta = sPlanta;
                        line.Linea = sLinea;

                        try
                        {
                            LineaLogica.Eliminar(line);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "LineaLogica.Eliminar(line);" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        dgwLineas.Rows.Remove(dgwLineas.CurrentRow);
                    }
                }
            }
            //Supervisores
            if (tabControl1.SelectedIndex == 1)
            {
                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "40") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DialogResult Result = MessageBox.Show("Desea Eliminar el Supervisor?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    string sPlanta = dgwSuper.SelectedCells[0].Value.ToString();
                    string sConsec  = dgwSuper.SelectedCells[1].Value.ToString();
                    if (!string.IsNullOrEmpty(sPlanta))
                    {
                        PlantaSuperLogica ptas = new PlantaSuperLogica();
                        ptas.Planta = sPlanta;
                        ptas.Consec = Convert.ToInt16(sConsec);

                        try
                        {
                            PlantaSuperLogica.Eliminar(ptas);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "LineaLogica.Eliminar(line);" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        dgwSuper.Rows.Remove(dgwSuper.CurrentRow);
                    }
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cbbPlanta.SelectedIndex == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult Result = MessageBox.Show("Desea borrar la Planta?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                try
                {
                    PlantaLogica plan = new PlantaLogica();
                    plan.Planta = cbbPlanta.SelectedValue.ToString();

                    if (PlantaLogica.AntesDeEliminar(plan))
                    {
                        MessageBox.Show("La Planta no se puede borrar, debido a que cuenta con movimientos en el Sistema", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (PlantaLogica.Eliminar(plan))
                    {
                        MessageBox.Show("La Planta ha sido borrada", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Inicio();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

            }
        }


        #endregion

        private void dgwLineas_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1)
                {
                    string sCve = dgwLineas[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
                    if (!string.IsNullOrEmpty(sCve) && !string.IsNullOrWhiteSpace(sCve))
                    {
                        int iCont = 0;
                        for (int i = 0; i < dgwLineas.RowCount; i++)
                        {
                            if (i == dgwLineas.Rows.Count - 1)
                                break;

                            string sCveAnt = dgwLineas[1, i].Value.ToString().ToUpper();
                            if (sCve == sCveAnt)
                                iCont++;
                        }

                        if (iCont > 1)
                        {
                            MessageBox.Show("Linea Repetida", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgwLineas[1, e.RowIndex].Value = string.Empty;
                            return;
                        }
                    }
                    else
                    {
                        dgwLineas[1, e.RowIndex].Value = string.Empty;
                        dgwLineas[2, e.RowIndex].Value = string.Empty;
                        dgwLineas[3, e.RowIndex].Value = string.Empty;
                    }
                }
                _lbCambioDet = true;
            }   
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_lsNomAnt) && _lsNomAnt != txtNombre.Text)
                _lbCambio = true;
            _lsNomAnt = txtNombre.Text.ToString();
        }
    }
}
