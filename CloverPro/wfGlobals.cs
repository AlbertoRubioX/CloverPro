using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using Logica;
using Datos;

namespace CloverPro
{
    public partial class wfGlobals : Form
    {
        public bool _lbCambio;
        private bool _lbCambioDet;
        public string _lsParam;
        private string _lsProceso = "EMP050";
        private string _lsFolioAnt;
        private string _lsTurno;
        private string _lsArea;
        private string _lsPlantaAnt;
        private string _lsLineaAnt;

        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;
        
        public wfGlobals()
        {
            InitializeComponent();
            
            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }

        #region regInicio
        private void wfGlobals_Load(object sender, EventArgs e)
        {
            dtpFecha.Value = DateTime.Today;

            WindowState = FormWindowState.Maximized;

            Inicio();
        }

        private void Inicio()
        {
            lblCant.Text = "0";

            Dictionary<string, string> tipo = new Dictionary<string, string>();
            tipo.Add("R", "RPO");
            tipo.Add("M", "MODELO");
            cbbTipo.DataSource = new BindingSource(tipo, null);
            cbbTipo.DisplayMember = "Value";
            cbbTipo.ValueMember = "Key";
            cbbTipo.SelectedIndex = 0;
            txtRPO.Clear();

            dgwEstaciones.DataSource = null;
            CargarDetalle();
            CargarColumnas();

            _lbCambio = false;
            _lbCambioDet = false;

            //timer1.Start();

        }

        private void wfGlobals_Activated(object sender, EventArgs e)
        {
            dtpFecha.Focus();
        }

        private void dtpFecha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            CargarDetalle();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(!_lbCambioDet)
                CargarDetalle();
        }

        #endregion

        #region regGuardar
        private bool Valida()
        {
            bool bValida = false;

            if (!_lbCambioDet)
                return bValida;

            
            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "10") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bValida;
            }

            foreach (DataGridViewRow row in dgwEstaciones.Rows)
            {
                
                if (dgwEstaciones.IsCurrentRowDirty)
                    dgwEstaciones.CommitEdit(DataGridViewDataErrorContexts.Commit);
                
                string sInd = row.Cells[19].Value.ToString();
                if (sInd == "0")
                    continue;

                if (row.Cells[22].Value.ToString() == "1")
                {
                    MessageBox.Show("No se permite modificar registros cancelados", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgwEstaciones.CurrentCell = dgwEstaciones[0, row.Index];
                    dgwEstaciones.Focus();
                    return bValida;
                }

                int iCant = 0;
                int iCantG = 0;
                if(int.TryParse(row.Cells[3].Value.ToString(), out iCant))
                {
                    if (int.TryParse(row.Cells[4].Value.ToString(), out iCantG))
                    {
                        if(iCant < iCantG)
                        {
                            MessageBox.Show("La cantidad del RPO no puede ser menor a la cantidad Global", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgwEstaciones.CurrentCell = dgwEstaciones[3, row.Index];
                            dgwEstaciones.Focus();
                            return bValida;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(row.Cells[14].Value.ToString()))
                {
                    if (string.IsNullOrEmpty(row.Cells[12].Value.ToString()))
                    {
                        MessageBox.Show("No se ha especificado la Hora de Entrada da Envios", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgwEstaciones.CurrentCell = dgwEstaciones[12, row.Index];
                        dgwEstaciones.Focus();
                        return bValida;
                    }
                    if (string.IsNullOrEmpty(row.Cells[13].Value.ToString()))
                    {
                        MessageBox.Show("No se ha especificado la Hora de Salida de Envios", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgwEstaciones.CurrentCell = dgwEstaciones[13, row.Index];
                        dgwEstaciones.Focus();
                        return bValida;
                    }

                    if (string.IsNullOrEmpty(row.Cells[15].Value.ToString()))
                    {
                        MessageBox.Show("No se ha especificado Truck del TO", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgwEstaciones.CurrentCell = dgwEstaciones[15, row.Index];
                        dgwEstaciones.Focus();
                        return bValida;
                    }
                }
                bValida = true;
            }
            return bValida;    
        }
       
        private bool Guardar()
        {
            try
            {
                if (!Valida())
                    return false;

                foreach (DataGridViewRow row in dgwEstaciones.Rows)
                {
                    string sInd = row.Cells[19].Value.ToString();
                    if (sInd == "0")
                        continue;

                    long lFolio = long.Parse(row.Cells[18].Value.ToString());
                    string sModelo = row.Cells[2].Value.ToString();
                    int iCant = int.Parse(row.Cells[3].Value.ToString());
                    int iCantGlob = int.Parse(row.Cells[4].Value.ToString());
                    string sHrComp = row.Cells[6].Value.ToString();
                    string sHrIni = row.Cells[12].Value.ToString();
                    string sHrEnd = row.Cells[13].Value.ToString();
                    string sTO = row.Cells[14].Value.ToString();
                    string sTruck = row.Cells[15].Value.ToString();
                    string sNota = row.Cells[16].Value.ToString();

                    RpoGlobLogica rpo = new RpoGlobLogica();
                    rpo.Folio = lFolio;
                    rpo.Modelo = sModelo;
                    rpo.Cantidad = iCant;
                    rpo.CantGlobal = iCantGlob;
                    rpo.HoraComp = sHrComp;
                    rpo.EnvHrIni = sHrIni;
                    if(string.IsNullOrEmpty(sHrEnd) && !string.IsNullOrEmpty(sTO))
                    {
                        int iHora = DateTime.Now.Hour;
                        int iMin = DateTime.Now.Minute;

                        sHrEnd = iHora.ToString();
                        if (sHrEnd.Length == 1)
                            sHrEnd = "0" + sHrEnd;
                        string sMin = iMin.ToString();
                        if (sMin.Length == 1)
                            sMin = "0" + sMin;
                        sHrEnd += ":" + sMin;
                    }
                    rpo.EnvHrEnd = sHrEnd;
                    rpo.TO = sTO;
                    rpo.Truck = sTruck;
                    rpo.Nota = sNota.Replace("'","");
                    rpo.Usuario = GlobalVar.gsUsuario;

                    RpoGlobLogica.ActualizaWP(rpo);
                    
                }

                return true;
                
            }
            catch (Exception ie)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "LineUpLogica.Guardar(line)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        #endregion

        #region regBotones

        private void btnSave2_Click(object sender, EventArgs e)
        {
            btSave_Click(sender, e);
        }
        private void btnExit2_Click(object sender, EventArgs e)
        {
            btExit_Click(sender, e);
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            if (Guardar())
                Inicio();

        }

      
        private void btExit_Click(object sender, EventArgs e)
        {
            if (_lbCambio || _lbCambioDet)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios antes de salir?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (Guardar())
                        Close();
                }
                else
                {
                    if (Result == DialogResult.No)
                        Close();
                }
            }
            else
                Close();
            Close();
        }

       
        #endregion

        #region regDetalleCambios
        private void CargarDetalle()
        {
            dgwEstaciones.DataSource = null;
            RpoGlobLogica rpo = new RpoGlobLogica();
            rpo.Fecha = dtpFecha.Value;
            if (string.IsNullOrEmpty(txtRPO.Text))
                rpo.IndRPO = "0";
            else
            {
                rpo.IndRPO = cbbTipo.SelectedValue.ToString();
                rpo.RPO = txtRPO.Text.ToString();
            }
            if (chbCancel.Checked)
                rpo.Cancelado = "1";
            else
                rpo.Cancelado = "0";
                   
            DataTable dt = RpoGlobLogica.MonitorGlobals(rpo);
            dgwEstaciones.DataSource = dt;

            CargarColumnas();
            int iCant = dt.Rows.Count;
            if (iCant > 0)
            {
                dgwEstaciones.CurrentCell = dgwEstaciones[0, 0];
                dgwEstaciones.Focus();
            }
            lblCant.Text = iCant.ToString();
            
        }
        private void dgwEstaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightBlue;
            else
                e.CellStyle.BackColor = Color.White;

            if (e.Value != null)
            {
                e.Value = e.Value.ToString().ToUpper();
                e.FormattingApplied = true;
            }

            //if (e.ColumnIndex >= 0 && e.ColumnIndex <= 17)
            //{
            //    string sValue = dgwEstaciones[22, e.RowIndex].Value.ToString();
            //    if (sValue == "1")
            //        e.CellStyle.ForeColor = Color.Red;
            //}
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
            
            int iRows = dgwEstaciones.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("Estacion");
                dtNew.Columns.Add("RPO", typeof(int));                    //0
                dtNew.Columns.Add("Fecha", typeof(int));                   //1
                dtNew.Columns.Add("Modelo", typeof(string));         //2
                dtNew.Columns.Add("Cant", typeof(int));           //3
                dtNew.Columns.Add("Cant Global", typeof(int));           //4
                dtNew.Columns.Add("Linea", typeof(string));      //5
                dtNew.Columns.Add("Hora Comp", typeof(string));      //6
                dtNew.Columns.Add("Destino", typeof(string));//7
                dtNew.Columns.Add("Entra ALM", typeof(string));           //8
                dtNew.Columns.Add("Sale ALM", typeof(string));            //9
                dtNew.Columns.Add("Entra PRO", typeof(string));               //10
                dtNew.Columns.Add("Sale PRO", typeof(string));            //11
                dtNew.Columns.Add("Entra ENV", typeof(string));            //12
                dtNew.Columns.Add("Sale ENV", typeof(string));           //13
                dtNew.Columns.Add("No TO", typeof(string));           //14
                dtNew.Columns.Add("No Truck", typeof(string));           //15
                dtNew.Columns.Add("Comentarios", typeof(string));//16
                dtNew.Columns.Add("estado", typeof(string));           //17
                dtNew.Columns.Add("folio", typeof(long));           //18
                dtNew.Columns.Add("ind", typeof(string));           //19
                dtNew.Columns.Add("u_id", typeof(string));           //20
                dtNew.Columns.Add("f_id", typeof(string));           //21
                dtNew.Columns.Add("cancelado", typeof(string));           //22

                dgwEstaciones.DataSource = dtNew;
            }

            try
            {
                foreach(DataGridViewRow row in dgwEstaciones.Rows)
                {
                    if(row.Cells[0].Value != null)
                    {
                        string sEstatus = Convert.ToString(row.Cells[16].Value);
                        if (sEstatus == "ENV")
                            row.DefaultCellStyle.BackColor = Color.Yellow;
                        
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador " + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            
            dgwEstaciones.Columns[8].Visible = false;
            dgwEstaciones.Columns[9].Visible = false;
            dgwEstaciones.Columns[10].Visible = false;
            dgwEstaciones.Columns[11].Visible = false;
            dgwEstaciones.Columns[17].Visible = false;
            dgwEstaciones.Columns[18].Visible = false;
            dgwEstaciones.Columns[19].Visible = false;
            dgwEstaciones.Columns[20].Visible = false;
            dgwEstaciones.Columns[21].Visible = false;
            dgwEstaciones.Columns[22].Visible = false;

            dgwEstaciones.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwEstaciones.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            dgwEstaciones.Columns[0].Width = ColumnWith(dgwEstaciones, 8);//RPO
            dgwEstaciones.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[0].ReadOnly = true;

            dgwEstaciones.Columns[1].Width = ColumnWith(dgwEstaciones, 6);//Fecha
            dgwEstaciones.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[1].ReadOnly = true;

            dgwEstaciones.Columns[2].Width = ColumnWith(dgwEstaciones, 10);//MODELO
            dgwEstaciones.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[2].ReadOnly = false;
            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "25"))
                dgwEstaciones.Columns[2].ReadOnly = false;

            dgwEstaciones.Columns[3].Width = ColumnWith(dgwEstaciones, 4);//CANT
            dgwEstaciones.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgwEstaciones.Columns[3].ReadOnly = false;

            dgwEstaciones.Columns[4].Width = ColumnWith(dgwEstaciones, 5);//CANT GLOBAL
            dgwEstaciones.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[4].ReadOnly = false;

            dgwEstaciones.Columns[5].Width = ColumnWith(dgwEstaciones, 8);//LINEA
            dgwEstaciones.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[5].ReadOnly = true;

            dgwEstaciones.Columns[6].Width = ColumnWith(dgwEstaciones, 5);//HR COM
            dgwEstaciones.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[6].ReadOnly = true;
            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "25"))
                dgwEstaciones.Columns[6].ReadOnly = false;

            dgwEstaciones.Columns[7].Width = ColumnWith(dgwEstaciones, 9);//DESTINO
            dgwEstaciones.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[7].ReadOnly = true;

            //dgwEstaciones.Columns[9].Width = ColumnWith(dgwEstaciones, 6);//PROD ENTRA
            //dgwEstaciones.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[9].ReadOnly = true;

            //dgwEstaciones.Columns[10].Width = ColumnWith(dgwEstaciones, 6);//PROD SALE
            //dgwEstaciones.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[10].ReadOnly = true;

            dgwEstaciones.Columns[12].Width = ColumnWith(dgwEstaciones, 6);//ENV ENTRA
            dgwEstaciones.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[13].Width = ColumnWith(dgwEstaciones, 6);//ENV SALE
            dgwEstaciones.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            dgwEstaciones.Columns[14].Width = ColumnWith(dgwEstaciones, 11);//TO
            dgwEstaciones.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[15].Width = ColumnWith(dgwEstaciones, 7);//TRUCK
            dgwEstaciones.Columns[15].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[16].Width = ColumnWith(dgwEstaciones, 12);//COMENT
            dgwEstaciones.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[16].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }
       
        //ESCANER SOBRE #OPERADOR && LINEA OP
        private void dgwEstaciones_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.ColumnIndex >= 2 && e.ColumnIndex <= 6)
                {
                    if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "25") == false)
                    {
                        MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    _lbCambioDet = true;
                    dgwEstaciones[19, e.RowIndex].Value = "1";

                }

                if (e.ColumnIndex >= 12 && e.ColumnIndex <= 16 )
                {
                    if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "50") == false)
                    {
                        MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    _lbCambioDet = true;
                    dgwEstaciones[19, e.RowIndex].Value = "1";

                }

                if (e.ColumnIndex == 12 || e.ColumnIndex == 13)
                {
                    _lbCambioDet = true;
                    string sVal = dgwEstaciones[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
                    if (string.IsNullOrEmpty(sVal) || string.IsNullOrWhiteSpace(sVal))
                        return;

                    string sHora = sVal.Trim();
                    if(sHora.IndexOf(":") == -1)
                    {
                        int iHora = 0;
                        if (!int.TryParse(sHora,out iHora))
                        {
                            MessageBox.Show("Formato de Hora Incorrecto. (00:00)", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = string.Empty;
                            return;
                        }

                        if (sHora.Length == 4)
                        {
                            sHora = sHora.Substring(0, 2) + ":" + sHora.Substring(2, 2);
                            dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = sHora;
                        }
                        else
                        {
                            if (sHora.Length == 2)
                            {
                                sHora = sHora + ":00";
                                dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = sHora;
                            }
                            else
                            {
                                MessageBox.Show("Formato de Hora Incorrecto. (00:00)", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = string.Empty;
                                return;
                            }
                        }
                        
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "dgwEstaciones_CellValueChanged(6)" + Environment.NewLine + ex.ToString(), "Error en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void dgwEstaciones_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                btExit_Click(sender, e);
            }

          
           
        }

        private void dgwEstaciones_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void dgwEstaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex == 5 || e.ColumnIndex == 7) //LINEA
            {

                if (dgwEstaciones[22, e.RowIndex].Value.ToString() == "1")
                {
                    MessageBox.Show("No se permite modificar registros cancelados", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "25") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string sValue = dgwEstaciones[e.ColumnIndex, e.RowIndex].Value.ToString();
                
                try
                {
                    long lFolio = long.Parse(dgwEstaciones[18, e.RowIndex].Value.ToString());

                    wfCapturaPop_1t CapPop = new wfCapturaPop_1t("");
                    CapPop._lsProceso = _lsProceso;
                    CapPop._llFolio = lFolio;
                    CapPop._lsPlanta = GlobalVar.gsPlanta;
                    if (e.ColumnIndex == 5)
                        CapPop._sClave = "LINEA";
                    else
                        CapPop._sClave = "DESTINO";
                    CapPop.ShowDialog();
                    Inicio();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        #endregion

        #region regFuncion


        private void tssF3_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "60") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            wfCapturaPop popUp = new wfCapturaPop(_lsProceso);
            popUp.ShowDialog();
            string sOperacion = popUp._sClave;
            if (string.IsNullOrEmpty(sOperacion))
                return;

            int iClave = Convert.ToInt32(dgwEstaciones[4, dgwEstaciones.RowCount - 1].Value);
            iClave++;
            string sEstacion = iClave.ToString();
            string sLinea = string.Empty;
           
            dgwEstaciones.Rows[dgwEstaciones.Rows.Count - 1].Cells[8].Selected = true;
        }

        private void tssF4_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "30") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            iRow = dgwEstaciones.CurrentCell.RowIndex;
            string sTipo = dgwEstaciones[11, iRow].Value.ToString();
            if (sTipo == "X" || sTipo == "Z" || sTipo == "G")
                return;
            
            dgwEstaciones[8, iRow].Value = "N/A";
            dgwEstaciones[9, iRow].Value = "NO APLICA MODELO";
            dgwEstaciones[10, iRow].Value = string.Empty;
            dgwEstaciones[11, iRow].Value = string.Empty;
            dgwEstaciones[13, iRow].Value = "M";
            dgwEstaciones[14, iRow].Value = "0";

            iRow++;
            if(dgwEstaciones.RowCount > iRow)
                dgwEstaciones.Rows[iRow].Cells[8].Selected = true;
        }

        private void tssF5_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "40") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (dgwEstaciones[12, iRow].Value.ToString() == "O")
            {
                int iClave = Convert.ToInt32(dgwEstaciones[4, iRow].Value);
                string sEstacion = dgwEstaciones[5, iRow].Value.ToString();
                string sOperacion = dgwEstaciones[6, iRow].Value.ToString();
                string sNivel = dgwEstaciones[7, iRow].Value.ToString();
                string sLinea = string.Empty;
                
                dgwEstaciones.Rows[dgwEstaciones.Rows.Count - 1].Cells[8].Selected = true;
            }
        }

        private void tssF6_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "50") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string sTipo = dgwEstaciones[12, iRow].Value.ToString();
            if (sTipo == "X" || sTipo == "Z")
            {
                int iFolio = 0;
                int iCons = 0;
                if (Int32.TryParse(dgwEstaciones[0, iRow].Value.ToString(), out iFolio))
                {
                    iCons = Convert.ToInt32(dgwEstaciones[1, iRow].Value);
                    LineUpDetLogica line = new LineUpDetLogica();
                    line.Folio = iFolio;
                    line.Consec = iCons;
                    LineUpDetLogica.Eliminar(line);
                }
                dgwEstaciones.Rows.Remove(dgwEstaciones.CurrentRow);
            }
        }

        private void tssF7_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            dgwEstaciones[8, iRow].Value = string.Empty;
            dgwEstaciones[9, iRow].Value = string.Empty;
            dgwEstaciones[10, iRow].Value = string.Empty;
            dgwEstaciones[11, iRow].Value = string.Empty;
            dgwEstaciones[13, iRow].Value = string.Empty;
            dgwEstaciones[14, iRow].Value = string.Empty;
        }

        private void btnF4_Click(object sender, EventArgs e)
        {
            tssF4_Click(sender, e);
            dgwEstaciones.Focus();
        }

        private void btnF3_Click(object sender, EventArgs e)
        {
            tssF3_Click(sender, e);
            dgwEstaciones.Focus();
        }

        private void btnF5_Click(object sender, EventArgs e)
        {
            tssF5_Click(sender, e);
            dgwEstaciones.Focus();
        }

        private void btnF6_Click(object sender, EventArgs e)
        {
            tssF6_Click(sender, e);
            dgwEstaciones.Focus();
        }

        private void btnF7_Click(object sender, EventArgs e)
        {
            tssF7_Click(sender, e);
            dgwEstaciones.Focus();
        }

        
        #endregion

        #region regRes

        private void wfGlobals_Resize(object sender, EventArgs e)
        {
            if (WindowState != _WindowStateAnt && WindowState != FormWindowState.Minimized)
            {
                int iX = panel2.Width - btRefresh.Location.X;
                int iH = groupBox1.Height - lblCant.Location.Y;

                _WindowStateAnt = WindowState;
                ResizeControl(panel2, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(panel1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(groupBox1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(dgwEstaciones, 3, ref _iWidthAnt, ref _iHeightAnt, 1);

                btRefresh.Location = new Point(panel2.Width - iX, btRefresh.Location.Y);
                lblCant.Location = new Point(lblCant.Location.X, groupBox1.Height - iH);

            }
        }
        
        public void ResizeControl(Control ac_Control, int ai_Hor, ref int ai_WidthAnt, ref int ai_HegihtAnt, int ai_Retorna)
        {
            if (ai_WidthAnt == 0)
                ai_WidthAnt = ac_Control.Width;
            if (ai_WidthAnt == ac_Control.Width)
                return;

            int _dif = ai_WidthAnt - ac_Control.Width;
            int _difh = ai_HegihtAnt - ac_Control.Height;

            if (ai_Hor == 1)
                ac_Control.Height = this.Height - _difh;
            if (ai_Hor == 2)
                ac_Control.Width = this.Width - _dif;
            if (ai_Hor == 3)
            {
                ac_Control.Width = this.Width - _dif;
                ac_Control.Height = this.Height - _difh;
            }
            if (ai_Retorna == 1)
            {
                ai_WidthAnt = this.Width;
                ai_HegihtAnt = this.Height;
            }
        }

        public void ResizeGrid(DataGridView dataGrid, ref int prevWidth, ref int ai_HegihtAnt)
        {
            if (prevWidth == 0)
                prevWidth = dataGrid.Width;
            if (prevWidth == dataGrid.Width)
                return;

            int _dif = prevWidth - dataGrid.Width;
            int _difh = ai_HegihtAnt - dataGrid.Height;

            int fixedWidth = SystemInformation.VerticalScrollBarWidth + dataGrid.RowHeadersWidth + 2;
            int mul = 100 * (dataGrid.Width - fixedWidth) / (prevWidth - fixedWidth);
            int columnWidth;
            int total = 0;
            DataGridViewColumn lastVisibleCol = null;

            for (int i = 0; i < dataGrid.ColumnCount; i++)
                if (dataGrid.Columns[i].Visible)
                {
                    columnWidth = (dataGrid.Columns[i].Width * mul + 50) / 100;
                    dataGrid.Columns[i].Width = Math.Max(columnWidth, dataGrid.Columns[i].MinimumWidth);
                    total += dataGrid.Columns[i].Width;
                    lastVisibleCol = dataGrid.Columns[i];
                }
            if (lastVisibleCol == null)
                return;
            columnWidth = dataGrid.Width - total + lastVisibleCol.Width - fixedWidth;
            lastVisibleCol.Width = Math.Max(columnWidth, lastVisibleCol.MinimumWidth);

        }
        #endregion

        #region regBotones

       
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;
            

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            
            try
            {
                
                string sRPO = dgwEstaciones[0, iRow].Value.ToString();
                string sModelo = dgwEstaciones[2, iRow].Value.ToString();

                DialogResult Result = MessageBox.Show(String.Format("Desea Eliminar el registro {0} {1} ?",sRPO,sModelo), this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    long lFolio = long.Parse(dgwEstaciones[18, iRow].Value.ToString());
                    

                    RpoGlobLogica rpo = new RpoGlobLogica();
                    rpo.Folio = lFolio;
                    if (RpoGlobLogica.BorrarRPO(rpo))
                        CargarDetalle();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        #endregion

        private void txtRPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (cbbTipo.SelectedValue.ToString() == "R")
            {
                string sRPO = txtRPO.Text.ToString().Trim();
                if (sRPO.IndexOf("RPO") == -1)
                {
                    int iRpo = 0;
                    if (int.TryParse(sRPO, out iRpo))
                        txtRPO.Text = "RPO" + sRPO;
                }
            }

            CargarDetalle();

            txtRPO.Focus();
            if (!string.IsNullOrEmpty(txtRPO.Text))
            {
                txtRPO.SelectionStart = 0;
                txtRPO.SelectionLength = txtRPO.Text.Length;
            }
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            CargarDetalle();
        }

        private void btExcel_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "60") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ExportarFormato();

        }

        private void ExportarFormato()
        {
            Cursor = Cursors.WaitCursor;

            string sDirec = @"C:\CloverPRO\Formatos";
            string sFile = "GlobalsEmpaque";

            bool bExists = Directory.Exists(sDirec);
            if (!bExists)
                Directory.CreateDirectory(sDirec);

            sFile = sDirec + "\\" + sFile + ".xlsx";

            Excel.Application oXL = null;
            Excel._Workbook oWB = null;
            Excel._Worksheet oSheet = null;

            oXL = new Excel.Application();
            oWB = oXL.Workbooks.Open(sFile);
            oSheet = String.IsNullOrEmpty("Sheet1") ? (Excel._Worksheet)oWB.ActiveSheet : (Excel._Worksheet)oWB.Worksheets["Sheet1"];
            oWB.Worksheets["Sheet1"].Range("A1:S100").Clear();

            try
            {
                
                int iRow = 2;

                oSheet.Cells[1, 1] = "RPO";
                oSheet.Cells[1, 2] = "Fecha";
                oSheet.Cells[1, 3] = "Modelo";
                oSheet.Cells[1, 4] = "Cant";
                oSheet.Cells[1, 5] = "Cant Global";
                oSheet.Cells[1, 6] = "Linea";
                oSheet.Cells[1, 7] = "HrComp";
                oSheet.Cells[1, 8] = "Destino";
                oSheet.Cells[1, 9] = "Entra ALM";
                oSheet.Cells[1, 10] = "Sale ALM";
                oSheet.Cells[1, 11] = "Entra PROD";
                oSheet.Cells[1, 12] = "Sale PROD";
                oSheet.Cells[1, 13] = "Entra ENV";
                oSheet.Cells[1, 14] = "Sale ENV";
                oSheet.Cells[1, 15] = "No TO";
                oSheet.Cells[1, 16] = "No Tuck";
                oSheet.Cells[1, 17] = "Comentarios";
                oSheet.Cells[1, 18] = "Usuario id";
                oSheet.Cells[1, 19] = "Fecha id";


                foreach (DataGridViewRow row in dgwEstaciones.Rows)
                {
                    string sRPO = row.Cells[0].Value.ToString();
                    string sFecha = row.Cells[1].Value.ToString();
                    string sModelo = row.Cells[2].Value.ToString();
                    string sCant = row.Cells[3].Value.ToString();
                    string sCantGlob = row.Cells[4].Value.ToString();
                    string sLinea = row.Cells[5].Value.ToString();
                    string sHrCom = row.Cells[6].Value.ToString();
                    string sDestino = row.Cells[7].Value.ToString();
                    string sAlmEnt = row.Cells[8].Value.ToString();
                    string sAlmSal = row.Cells[9].Value.ToString();
                    string sProEnt = row.Cells[10].Value.ToString();
                    string sProSal = row.Cells[11].Value.ToString();
                    string sEnvEnt = row.Cells[12].Value.ToString();
                    string sEnvSal = row.Cells[13].Value.ToString();
                    string sTO = row.Cells[14].Value.ToString();
                    string sTruck = row.Cells[15].Value.ToString();
                    string sComent = row.Cells[16].Value.ToString();
                    string sUser = row.Cells[20].Value.ToString();
                    string sUid = row.Cells[21].Value.ToString();

                    oSheet.Cells[iRow, 1] = sRPO;
                    oSheet.Cells[iRow, 2] = sFecha;
                    oSheet.Cells[iRow, 3] = sModelo;
                    oSheet.Cells[iRow, 4] = sCant;
                    oSheet.Cells[iRow, 5] = sCantGlob;
                    oSheet.Cells[iRow, 6] = sLinea;
                    oSheet.Cells[iRow, 7] = sHrCom;
                    oSheet.Cells[iRow, 8] = sDestino;
                    oSheet.Cells[iRow, 9] = sAlmEnt;
                    oSheet.Cells[iRow, 10] = sAlmSal;
                    oSheet.Cells[iRow, 11] = sProEnt;
                    oSheet.Cells[iRow, 12] = sProSal;
                    oSheet.Cells[iRow, 13] = sEnvEnt;
                    oSheet.Cells[iRow, 14] = sEnvSal;
                    oSheet.Cells[iRow, 15] = sTO;
                    oSheet.Cells[iRow, 16] = sTruck;
                    oSheet.Cells[iRow, 17] = sComent;
                    oSheet.Cells[iRow, 18] = sUser;
                    oSheet.Cells[iRow, 19] = sUid;


                    iRow++;
                }

                oXL.DisplayAlerts = false;
                oWB.SaveAs(sFile, Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlShared);

                DialogResult Result = MessageBox.Show("Se ha exportado la captura." + Environment.NewLine + "Desea abrir el reporte en excel?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(sDirec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Cursor = Cursors.Default;
            }
            finally
            {
                if (oWB != null)
                {
                    oWB.Close();
                    oXL.Quit();
                    Cursor = Cursors.Default;
                }
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;


            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "15") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            try
            {

                string sCancel = dgwEstaciones[22, iRow].Value.ToString();
                if(sCancel == "1")
                {
                    MessageBox.Show("El RPO ya se encuentra Cancelado", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string sRPO = dgwEstaciones[0, iRow].Value.ToString();
                string sModelo = dgwEstaciones[2, iRow].Value.ToString();
                string sNota = dgwEstaciones[16, iRow].Value.ToString();

                DialogResult Result = MessageBox.Show(String.Format("Desea Cancelar el registro {0} {1} ?", sRPO, sModelo), this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    long lFolio = long.Parse(dgwEstaciones[18, iRow].Value.ToString());
                    
                    string sComent = string.Empty;

                    wfComentarioPop Coment = new wfComentarioPop("CANCEL");
                    Coment._lsProceso = _lsProceso;
                    Coment.ShowDialog();
                    sComent = Coment._sClave;

                    if (!string.IsNullOrEmpty(sComent))
                    {
                        if (!string.IsNullOrEmpty(sNota))
                            sComent += Environment.NewLine + sNota;

                        RpoGlobLogica rpo = new RpoGlobLogica();
                        rpo.Folio = lFolio;
                        rpo.Nota = sComent;
                        rpo.Usuario = GlobalVar.gsUsuario;

                        if (RpoGlobLogica.CancelaRPO(rpo))
                            CargarDetalle();
                    }
                    else
                    {
                        MessageBox.Show("Se requieren el motivo de la cancelación", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dgwEstaciones.Focus();
                        return;
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cbbTipo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtRPO.Text))
            {
                CargarDetalle();
            }
        }

        private void chbCancel_CheckedChanged(object sender, EventArgs e)
        {
            CargarDetalle();
        }

        private void dgwEstaciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
