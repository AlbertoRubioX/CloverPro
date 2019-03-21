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
    public partial class wfUbicacionRpo : Form
    {
        public bool _lbCambio;
        private string _lsProceso = "CAT090";
        public wfUbicacionRpo()
        {
            InitializeComponent();
        }
        #region regInicio
        private void wfUbicacionRpo_Load(object sender, EventArgs e)
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
            cbbPlanta.SelectedIndex = -1;

            cbbUbica.ResetText();

            cbbArea.SelectedIndex = 0;

            dgwData.Columns.Clear();
            CargarLineas();

            cbbPlanta.Focus();
        }

        private void wfUbicacionRpo_Activated(object sender, EventArgs e)
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

            if (e.Value != null)
            {
                e.Value = e.Value.ToString().ToUpper();
                e.FormattingApplied = true;
            }
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
            if (dgwData.Rows.Count == 0)
            {
                DataTable dtNew = new DataTable("Lineas");
                dtNew.Columns.Add("planta", typeof(string));
                dtNew.Columns.Add("ubicacion", typeof(string));
                dtNew.Columns.Add("CELDA", typeof(string));
                dtNew.Columns.Add("CONFIG", typeof(string));
                dgwData.DataSource = dtNew;
            }
            else
            {
                dgwData.Focus();
                dgwData.CurrentCell = dgwData[2, dgwData.Rows.Count - 1];
            }

            dgwData.Columns[0].Visible = false;
            dgwData.Columns[1].Visible = false;

            dgwData.Columns[2].Width = ColumnWith(dgwData, 60);
            dgwData.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[3].Width = ColumnWith(dgwData, 30);
            dgwData.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        #endregion

        #region regGuardar
        private bool Guardar()
        {
            if (Valida())
            {
                try
                {
                    RpoUbicacionLogica ubi = new RpoUbicacionLogica();
                    ubi.Planta = cbbPlanta.SelectedValue.ToString();
                    if (cbbArea.SelectedIndex == 0)
                        ubi.Area = "PRO";
                    else
                        ubi.Area = "ALM";
                    ubi.Ubicacion = cbbUbica.Text.ToString().ToUpper();
                    ubi.Nota = string.Empty;
                    ubi.Usuario = GlobalVar.gsUsuario;

                    if (RpoUbicacionLogica.Guardar(ubi) != 0)
                    {
                        foreach (DataGridViewRow row in dgwData.Rows)
                        {
                            if (row.Index == dgwData.Rows.Count - 1)
                                continue;

                            if (string.IsNullOrEmpty(row.Cells[2].Value.ToString()))
                                continue;

                            RpoUbicaDetLogica rpod = new RpoUbicaDetLogica();
                            rpod.Planta = cbbPlanta.SelectedValue.ToString();
                            rpod.Ubicacion = cbbUbica.Text.ToString().ToUpper();
                            if (cbbArea.SelectedIndex == 0)
                                rpod.Area = "PRO";
                            else
                                rpod.Area = "ALM";
                            rpod.Celda = row.Cells[2].Value.ToString().ToUpper();
                            int iCant = 0;
                            if (!int.TryParse(row.Cells[3].Value.ToString(), out iCant))
                                iCant = 0;
                            rpod.Config = iCant;
                            rpod.Usuario = GlobalVar.gsUsuario;

                            RpoUbicaDetLogica.Guardar(rpod);
                        }
                    }
                    else
                        return false;
                    
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

            if (cbbPlanta.SelectedIndex == -1)
            {
                cbbPlanta.Focus();
                return bValida;
            }

            if (string.IsNullOrEmpty(cbbUbica.Text))
            {
                cbbUbica.Focus();
                return bValida;
            }

            if(dgwData.RowCount == 0)
            {
                dgwData.Focus();
                return bValida;
            }
            
            return true;
        }
        #endregion

        #region regBotones
        private void btSave_Click(object sender, EventArgs e)
        {
            if (Guardar())
            {
                string sPlanta = cbbPlanta.SelectedValue.ToString();
                Inicio();
                cbbPlanta.SelectedValue = sPlanta;
            }
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
            

            if (dgwData.SelectedRows.Count == 0)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                string sPlanta = dgwData.SelectedCells[0].Value.ToString();
                string sLocal = dgwData.SelectedCells[1].Value.ToString();
                string sCelda = dgwData.SelectedCells[2].Value.ToString();
                if (!string.IsNullOrEmpty(sLocal))
                {
                    RpoUbicaDetLogica rpo = new RpoUbicaDetLogica();
                    rpo.Planta = sPlanta;
                    rpo.Ubicacion = sLocal;
                    rpo.Celda = sCelda;
                        
                    RpoUbicaDetLogica.Eliminar(rpo);
                }

                dgwData.Rows.Remove(dgwData.CurrentRow);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        
        #endregion
       
        #region regCaptura
        
        private void AgregaLinea(string _asPlanta, string _asLinea, string _asTurno, string _asArea)
        {
            DataTable dt = dgwData.DataSource as DataTable;
            dt.Rows.Add(cbbPlanta.SelectedValue.ToString(), 0, _asPlanta, _asLinea, _asTurno, _asArea);
            _lbCambio = true;
        }
        
        #endregion

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RpoUbicacionLogica rpo = new RpoUbicacionLogica();
            rpo.Planta = cbbPlanta.SelectedValue.ToString();

            DataTable dt = RpoUbicacionLogica.Consultar(rpo);
            cbbUbica.DataSource = dt;
            cbbUbica.DisplayMember = "ubicacion";
            cbbUbica.ValueMember = "ubicacion";
            cbbUbica.SelectedIndex = -1;

            dgwData.Columns.Clear();
        }

        private void cbbUbica_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbbPlanta.SelectedIndex != -1)
            {
                RpoUbicaDetLogica rpo = new RpoUbicaDetLogica();
                rpo.Planta = cbbPlanta.SelectedValue.ToString();
                if (cbbArea.SelectedIndex == 0)
                    rpo.Area = "PRO";
                else
                    rpo.Area = "ALM";
                rpo.Ubicacion = cbbUbica.SelectedValue.ToString();

                DataTable datos = RpoUbicaDetLogica.Listar(rpo);

                if (datos.Rows.Count != 0)
                    dgwData.DataSource = datos;
                else
                    dgwData.Columns.Clear();

                CargarLineas();

            }
        }

        private void cbbPlanta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void cbbPlanta_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbUbica.ResetText();
            dgwData.DataSource = null;
        }

        private void cbbUbica_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(cbbUbica.Text) && !string.IsNullOrWhiteSpace(cbbUbica.Text))
            {

                RpoUbicaDetLogica ubi = new RpoUbicaDetLogica();
                ubi.Planta = cbbPlanta.SelectedValue.ToString();
                ubi.Ubicacion = cbbUbica.Text.ToUpper().Trim().ToString();
                cbbUbica.Text = cbbUbica.Text.ToUpper().Trim().ToString();
                DataTable data = RpoUbicaDetLogica.Listar(ubi);
                dgwData.DataSource = data;
                CargarLineas();

            }
            else
                cbbUbica.SelectedIndex = -1;
        }

        private void dgwData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
                _lbCambio = true;

            if (e.ColumnIndex == 2)
            {
                string sVal = dgwData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
                if(!string.IsNullOrEmpty(sVal) && !string.IsNullOrWhiteSpace(sVal))
                {
                    foreach (DataGridViewRow row in dgwData.Rows)
                    {
                        if (row.Index == e.RowIndex)
                            continue;

                        if (row.Cells[2].Value == null)
                            continue;

                        string sValor = row.Cells[2].Value.ToString().ToUpper();
                        if (sVal == sValor)
                        {
                            MessageBox.Show(string.Format("La ubicación {0} se encuentra en el listado", sValor), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgwData[e.ColumnIndex, e.RowIndex].Value = string.Empty;
                            break;
                        }
                    }
                }
            }
        }

        private void dgwData_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                
                if (e.ColumnIndex == 3)
                {
                    if (dgwData.Rows[e.RowIndex].Cells[2].FormattedValue == "")
                        return;

                    int iCant;
                    if (!int.TryParse(Convert.ToString(e.FormattedValue), out iCant))
                    {
                        e.Cancel = true;

                        MessageBox.Show("Valor incorrecto", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}