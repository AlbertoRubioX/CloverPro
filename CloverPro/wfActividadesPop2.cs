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
    public partial class wfActividadesPop2 : Form
    {
        public string _lsProceso;
        public string _sClave;
        public int _iInd;
        public string _sClaves;
        private string _lsParam;
        private bool _lbCambio;
        private bool _lbClosing;
        private string _lsArea;
        public wfActividadesPop2(string _asFolio)
        {
            InitializeComponent();
            _lsParam = _asFolio;
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

        private void wfActividadesPop2_Load(object sender, EventArgs e)
        {
            //AREA DEL USUARIO / FILTRO DE ACTIVIDADES
            try
            {
                UsuarioLogica user = new UsuarioLogica();
                user.Usuario = GlobalVar.gsUsuario;
                DataTable dtU = UsuarioLogica.Consultar(user);
                _lsArea = dtU.Rows[0]["area"].ToString();

                AreasLogica area = new AreasLogica();
                area.Area = _lsArea;
                DataTable dtA = AreasLogica.Consultar(area);
                if (dtA.Rows.Count > 0)
                {
                    string sActPrev = dtA.Rows[0]["ind_actprev"].ToString();
                    if (sActPrev == "0")
                        _lsArea = string.Empty;
                }
                else
                    _lsArea = string.Empty;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            

            CargarModelos();

        }

        private void CargarModelos()
        {
            try
            {
                int iPos = _lsParam.IndexOf("-");
                string sFolio = _lsParam.Substring(0, iPos);
                long lFolio = long.Parse(sFolio);
                int iCons = int.Parse(_lsParam.Substring(iPos + 1));

                LineSetActLogica act = new LineSetActLogica();
                act.Folio = lFolio;
                act.Consec = iCons;

                DataTable dt = LineSetActLogica.Listar(act);
                dgwData.DataSource = dt;
                ColumnasGrid();

                
                dgwData.ClearSelection();
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
                DataTable dtNew = new DataTable("Act");
                dtNew.Columns.Add("folio", typeof(long));
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("CLAVE", typeof(string));
                dtNew.Columns.Add("ACTIVIDAD", typeof(string));
                dtNew.Columns.Add("estatus", typeof(string));
                dtNew.Columns.Add("ESTATUS", typeof(string));
                dtNew.Columns.Add("NOTA", typeof(string));
                dtNew.Columns.Add("u_id", typeof(string));
                dgwData.DataSource = dtNew;
            }
            dgwData.Columns[0].Visible = false;
            dgwData.Columns[1].Visible = false;
            dgwData.Columns[4].Visible = false;
            dgwData.Columns[7].Visible = false;

            
            dgwData.Columns[2].Width = ColumnWith(dgwData, 10);
            dgwData.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[2].ReadOnly = true;
            dgwData.Columns[3].Width = ColumnWith(dgwData, 30);
            dgwData.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[3].ReadOnly = true;
            dgwData.Columns[5].Width = ColumnWith(dgwData, 20);
            dgwData.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[5].ReadOnly = true;
            dgwData.Columns[6].Width = ColumnWith(dgwData, 40);
            dgwData.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

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

            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightBlue;
            else
                e.CellStyle.BackColor = Color.White;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_lbCambio)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "40") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {

                foreach (DataGridViewRow row in dgwData.Rows)
                {
                    LineSetActLogica act = new LineSetActLogica();
                    act.Folio = long.Parse(row.Cells[0].Value.ToString());
                    act.Consec = int.Parse(row.Cells[1].Value.ToString());
                    act.Actividad = row.Cells[2].Value.ToString();
                    act.Estatus = row.Cells[4].Value.ToString();
                    act.Comentario = row.Cells[6].Value.ToString();
                    act.Usuario = GlobalVar.gsUsuario;

                    if(LineSetActLogica.Guardar(act) == -1)
                    {
                        MessageBox.Show("Error al intentar guardar cambios en actividad " + act.Actividad, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "Error en LineSetActLogica.Guardar()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
    
            Close();
        }

        private void dgwData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "dgwEstaciones_CellValueChanged" + Environment.NewLine + ex.ToString(), "Error en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void dgwData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                //if (e.ColumnIndex == 0)
                //{
                //    if (dgwData[e.ColumnIndex, e.RowIndex].Value != null)
                //    {
                //        if((bool)dgwData[e.ColumnIndex, e.RowIndex].Value)
                //            dgwData[5, e.RowIndex].Value = "T";
                //        else
                //            dgwData[5, e.RowIndex].Value = "E";
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "dgwEstaciones_CellEndEdit" + Environment.NewLine + ex.ToString(), "Error en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void dgwData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgwData.Rows.Count == 0)
                return;

            if (e.ColumnIndex == 5)
            {
                long lFolio = long.Parse(dgwData[0, e.RowIndex].Value.ToString());
                int iCons = int.Parse(dgwData[1, e.RowIndex].Value.ToString());
                string sArea = dgwData[2, e.RowIndex].Value.ToString();

                if (_lsArea == "CTDOC")
                    _lsArea = "CAL";

                //if (sArea != _lsArea)
                //{
                //    MessageBox.Show("La Actividad " + sArea + " no pertenece a su Departamento", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                wfActividEstatusPop ActPop = new wfActividEstatusPop();
                ActPop._lFolio = lFolio;
                ActPop._iConsec = iCons;
                ActPop._sArea = _lsArea;
                ActPop.ShowDialog();

                CargarModelos();
            }
        }

        private void dgwData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode == Keys.F12)
                btnExit_Click(sender, e);
        }
    }
}
