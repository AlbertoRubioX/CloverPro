using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using Logica;
using Datos;

namespace CloverPro
{
    public partial class wfLineSetUp : Form
    {
        public bool _lbCambio;
        private bool _lbCambioDet;
        public string _lsParam;
        private string _lsProceso = "PLA020";
        private string _lsFolioAnt;
        private string _lsTurno;
        private string _lsArea;
        private string _lsPlantaAnt;
        private string _lsLineaAnt;

        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;
        
        public wfLineSetUp(string _asParam)
        {
            InitializeComponent();
            _lsParam = _asParam;

            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }

        #region regInicio
        private void wfLineSetUp_Load(object sender, EventArgs e)
        {
            _lsTurno = GlobalVar.gsTurno;

            Inicio();

            WindowState = FormWindowState.Maximized;

            if (string.IsNullOrEmpty(_lsParam) || string.IsNullOrWhiteSpace(_lsParam))
                return;
            try
            {
                LineSetupLogica line = new LineSetupLogica();
                line.Folio = Convert.ToInt16(_lsParam);
                DataTable data = LineSetupLogica.Consultar(line);
                if (data.Rows.Count != 0)
                {
                    txtFolio.Text = data.Rows[0]["folio"].ToString();
                    cbbPlanta.SelectedValue = data.Rows[0]["planner"].ToString();
                    dtpFecha.Value = Convert.ToDateTime(data.Rows[0]["fecha"].ToString());

                    _lsFolioAnt = txtFolio.Text.ToString();
                    //ESTATUS
                    CargarDetalle(Convert.ToInt16(_lsParam));
                }
                else
                {
                    MessageBox.Show("El Folio no se encuentra Registrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtFolio.Text = _lsFolioAnt;
                    txtFolio.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void Inicio()
        {
            txtFolio.Clear();
            

            UsuarioLogica user = new UsuarioLogica();
            user.Usuario = GlobalVar.gsUsuario;
            DataTable dtU = UsuarioLogica.Consultar(user);
            _lsArea = dtU.Rows[0]["area"].ToString();

            cbbPlanta.ResetText();
            UsuarioLogica usua = new UsuarioLogica();
            usua.Area = "PLA";
            usua.Turno = "1";
            DataTable dtPl = UsuarioLogica.ListarPlanners(usua);
            cbbPlanta.DataSource = dtPl;
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.ValueMember = "usuario";
            if (_lsArea == "PLA")
                cbbPlanta.SelectedValue = GlobalVar.gsUsuario;
            else
                cbbPlanta.SelectedIndex = -1;


            AreasLogica area = new AreasLogica();
            area.Area = _lsArea;
            DataTable dtA = AreasLogica.Consultar(area);
            if(dtA.Rows.Count > 0)
            {
                string sActPrev = dtA.Rows[0]["ind_actprev"].ToString();
                if (sActPrev == "0")
                    _lsArea = string.Empty;
            }
            else
                _lsArea = string.Empty;

            
            dgwEstaciones.DataSource = null;
            CargarColumnas();

            _lsFolioAnt = string.Empty;
            _lsPlantaAnt = string.Empty;
            _lsLineaAnt = string.Empty;
            
            _lbCambio = false;
            _lbCambioDet = false;

        }

        private void wfLineSetUp_Activated(object sender, EventArgs e)
        {
            //txtFolio.Focus();
        }
        #endregion

        #region regGuardar
        private bool Valida()
        {
            bool bValida = false;

            if (!_lbCambio && !_lbCambioDet)
                return bValida;

            if (cbbPlanta.SelectedIndex == -1)
                return bValida;

            
            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "10") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bValida;
            }

            foreach (DataGridViewRow row in dgwEstaciones.Rows)
            {
                if (row.Index == dgwEstaciones.Rows.Count - 1)
                    continue;

                if (dgwEstaciones.IsCurrentRowDirty)
                    dgwEstaciones.CommitEdit(DataGridViewDataErrorContexts.Commit);

                if (string.IsNullOrEmpty(row.Cells[2].Value.ToString()))
                {
                    MessageBox.Show("No se ha especificado la Planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgwEstaciones.CurrentCell = dgwEstaciones[3, row.Index];
                    dgwEstaciones.Focus();
                    return bValida;
                }

                if (string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
                {
                    MessageBox.Show("No se ha especificado la Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgwEstaciones.CurrentCell = dgwEstaciones[3, row.Index];
                    dgwEstaciones.Focus();
                    return bValida;
                }

                if (string.IsNullOrEmpty(row.Cells[4].Value.ToString()))
                {
                    MessageBox.Show("No se ha especificado el RPO Actual", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgwEstaciones.CurrentCell = dgwEstaciones[4, row.Index];
                    dgwEstaciones.Focus();
                    return bValida;
                }

                if (string.IsNullOrEmpty(row.Cells[5].Value.ToString()))
                {
                    MessageBox.Show("No se ha especificado el Modelo Actual", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgwEstaciones.CurrentCell = dgwEstaciones[5, row.Index];
                    dgwEstaciones.Focus();
                    return bValida;
                }

                string sRpo = row.Cells[4].Value.ToString();
                string sMod = row.Cells[5].Value.ToString();
                string sRpoS = row.Cells[7].Value.ToString();
                string sModS = row.Cells[8].Value.ToString();

                if (string.IsNullOrEmpty(row.Cells[0].Value.ToString()))
                {
                    LineSetupDetLogica det = new LineSetupDetLogica();
                    det.Fecha = dtpFecha.Value;
                    det.Planta = row.Cells[2].Value.ToString();
                    det.Linea = row.Cells[3].Value.ToString();
                    det.RPO = sRpo;
                    det.Modelo = sMod;
                    det.RPOSig = sRpoS;
                    det.ModeloSig = sModS;
                    if(LineSetupDetLogica.Verificar(det))
                    {
                        MessageBox.Show("El Cambio de Modelo ya ha sido Registrado anteriormente para la Linea." + Environment.NewLine + "Favor de Corregir para evitar una captura duplicada.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dgwEstaciones.CurrentCell = dgwEstaciones[4, row.Index];
                        dgwEstaciones.Focus();
                        return bValida;
                    }
                }
                
            }

            if (dgwEstaciones.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgwEstaciones.Rows)
                {
                    if (row.Index == dgwEstaciones.Rows.Count - 1)
                        break;

                    if (dgwEstaciones.IsCurrentRowDirty)
                        dgwEstaciones.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    if (string.IsNullOrEmpty(row.Cells[2].Value.ToString()))
                    {
                        RowRemove(dgwEstaciones.CurrentRow);
                    }

                }
                return true;
            }

            return true;    
        }
        private void RowRemove(DataGridViewRow _crow)
        {
            try
            {
                if (!string.IsNullOrEmpty(_crow.Cells[0].Value.ToString()))
                {
                    //mandar al listado para borrar de bd al guardar cambios
                    //if (dgwRemoveMod.Rows.Count == 0)
                    //{
                    //    System.Data.DataTable dtNew = new System.Data.DataTable("Eliminar");
                    //    dtNew.Columns.Add("modelo", typeof(string));
                    //    dtNew.Columns.Add("consec", typeof(int));
                    //    dgwRemoveMod.DataSource = dtNew;
                    //}

                    //string sModelo = _crow.Cells[0].Value.ToString();
                    //int iCons = Convert.ToInt32(_crow.Cells[1].Value);

                    //System.Data.DataTable dt = dgwRemoveMod.DataSource as System.Data.DataTable;
                    //dt.Rows.Add(sModelo, iCons);
                }

                dgwEstaciones.Rows.Remove(_crow);
                _lbCambioDet = true;

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private bool Guardar()
        {
            try
            {
                
                
                if (!Valida())
                    return false;

                Cursor = Cursors.WaitCursor;
                LineSetupLogica line = new LineSetupLogica();
                if (string.IsNullOrEmpty(txtFolio.Text))
                    line.Folio = AccesoDatos.Consec(_lsProceso);
                else
                    line.Folio = Convert.ToInt32(txtFolio.Text);

                line.Fecha = dtpFecha.Value;
                line.Planner = cbbPlanta.SelectedValue.ToString();
                line.Usuario = GlobalVar.gsUsuario;
                
                //CAMBIOS POR RPO
                foreach (DataGridViewRow row in dgwEstaciones.Rows)
                {
                    try
                    {
                        if (row.Index == dgwEstaciones.Rows.Count - 1)
                            break;

                        if (string.IsNullOrEmpty(row.Cells[4].Value.ToString()))
                            continue;

                        if (row.Cells[14].Value.ToString() == "C")
                            continue;

                        LineSetupDetLogica lined = new LineSetupDetLogica();
                        lined.Folio = line.Folio;
                        int iCons = 0;
                        if (!int.TryParse(row.Cells[1].Value.ToString(), out iCons))
                            iCons = 0;
                        lined.Consec = iCons;
                        lined.Planner = cbbPlanta.SelectedValue.ToString();
                        lined.Planta = row.Cells[2].Value.ToString();
                        lined.Linea = row.Cells[3].Value.ToString();
                        lined.RPO = row.Cells[4].Value.ToString().ToUpper();
                        lined.Modelo = row.Cells[5].Value.ToString().ToUpper();
                        double dCant = 0;
                        if (!double.TryParse(row.Cells[6].Value.ToString(), out dCant))
                            dCant = 0;
                        lined.Cantidad = dCant;
                        lined.RPOSig = row.Cells[7].Value.ToString().ToUpper();
                        lined.ModeloSig = row.Cells[8].Value.ToString().ToUpper();
                        lined.IniProg = row.Cells[10].Value.ToString();
                        lined.FinProg = row.Cells[11].Value.ToString();
                        lined.IniReal = string.Empty;
                        lined.FinReal = string.Empty;
                        lined.Duracion = 0;
                        lined.Retraso = 0;
                        if(string.IsNullOrEmpty(row.Cells[14].Value.ToString()))
                            lined.Estatus = "E";
                        else
                            lined.Estatus = row.Cells[14].Value.ToString();
                        lined.Comentario = row.Cells[15].Value.ToString();
                        lined.Urgente = row.Cells[16].Value.ToString();
                        lined.Turno = row.Cells[17].Value.ToString();
                        lined.CancelComent = string.Empty;
                        lined.Usuario = GlobalVar.gsUsuario;

                        LineSetupDetLogica.Guardar(lined);
                    }
                    catch (Exception ie)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "LineUpDetLogica.Guardar(lined)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (LineSetupLogica.Guardar(line) == 0)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("Error al intentar guardar el Cambio de SetUp", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                    
                Cursor = Cursors.Default;
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

        private void btNew_Click(object sender, EventArgs e)
        {
            if (_lbCambio || _lbCambioDet)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios antes de limpiar pantalla?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (Guardar())
                        Inicio();
                }
                else
                {
                    if (Result == DialogResult.No)
                        Inicio();
                }
            }
            else
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

        private void btnHelp_Click(object sender, EventArgs e)
        {

            try
            {
                System.Data.DataTable dt = ConfigLogica.Consultar();
                string sDirec = dt.Rows[0]["directorio"].ToString();
                if (!string.IsNullOrEmpty(sDirec))
                {
                    sDirec += @"\CloverPRO_ManualSetUp.pdf";
                    System.Diagnostics.Process.Start(sDirec);
                }
                else
                    MessageBox.Show("No se ha especificado la ubicacion del Manual", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error... " + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

         
        #endregion

        #region regDetalleCambios
        private void CargarDetalle(int _aiFolio)
        {
            dgwEstaciones.DataSource = null;
            LineSetupDetLogica sed = new LineSetupDetLogica();
            sed.Folio = _aiFolio;
            DataTable dt = LineSetupDetLogica.Listar(sed);
            dgwEstaciones.DataSource = dt;

            CargarColumnas();

            dgwEstaciones.CurrentCell = dgwEstaciones[3, 0];
            dgwEstaciones.Focus();
            
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
                dtNew.Columns.Add("folio", typeof(int));                    //0
                dtNew.Columns.Add("consec", typeof(int));                   //1
                dtNew.Columns.Add("planta", typeof(string));                //2 
                dtNew.Columns.Add("LINEA", typeof(string));                 //3
                dtNew.Columns.Add("RPO ACTUAL", typeof(string));            //4
                dtNew.Columns.Add("MODELO ACTUAL", typeof(string));         //5
                dtNew.Columns.Add("CANTIDAD TOTAL", typeof(int));           //6
                dtNew.Columns.Add("RPO SIGUIENTE", typeof(string));         //7
                dtNew.Columns.Add("MODELO SIGUIENTE", typeof(string));      //8
                dtNew.Columns.Add("HORA PROGRAMADA", typeof(string));      //9
                dtNew.Columns.Add("FINALIZA SETUP TEORICO", typeof(string));//10
                dtNew.Columns.Add("INICIO SET-UP", typeof(string));           //11
                dtNew.Columns.Add("FINAL SET-UP", typeof(string));            //12
                dtNew.Columns.Add("ESTATUS", typeof(string));               //13
                dtNew.Columns.Add("cve_estatus", typeof(string));           //14
                dtNew.Columns.Add("comentario", typeof(string));            //15
                dtNew.Columns.Add("ind_urgente", typeof(string));           //16
                dtNew.Columns.Add("turno", typeof(string));           //17
                dgwEstaciones.DataSource = dtNew;
            }

            try
            {
                foreach(DataGridViewRow row in dgwEstaciones.Rows)
                {
                    if(row.Cells[0].Value != null)
                    {
                        string sInd = Convert.ToString(row.Cells[16].Value);
                        if(sInd == "1")
                            row.DefaultCellStyle.ForeColor = Color.Green;

                        string sEstatus = Convert.ToString(row.Cells[14].Value);
                        if (sEstatus == "C")
                            row.DefaultCellStyle.ForeColor = Color.Red;
                        
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador " + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            dgwEstaciones.Columns[0].Visible = false;
            dgwEstaciones.Columns[1].Visible = false;
            dgwEstaciones.Columns[2].Visible = false;
            dgwEstaciones.Columns[6].Visible = false;
            dgwEstaciones.Columns[10].Visible = false;
            dgwEstaciones.Columns[12].Visible = false;
            dgwEstaciones.Columns[14].Visible = false;
            dgwEstaciones.Columns[15].Visible = false;
            dgwEstaciones.Columns[16].Visible = false;
            dgwEstaciones.Columns[17].Visible = false;

            dgwEstaciones.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwEstaciones.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            dgwEstaciones.Columns[3].Width = ColumnWith(dgwEstaciones, 7);//LINEA
            dgwEstaciones.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[3].ReadOnly = true;

            dgwEstaciones.Columns[4].Width = ColumnWith(dgwEstaciones, 13);//RPO
            dgwEstaciones.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[5].Width = ColumnWith(dgwEstaciones, 15);//MODELO
            dgwEstaciones.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //dgwEstaciones.Columns[6].Width = ColumnWith(dgwEstaciones, 7);//CANTIDAD
            //dgwEstaciones.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgwEstaciones.Columns[7].Width = ColumnWith(dgwEstaciones, 13);//RPO
            dgwEstaciones.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[8].Width = ColumnWith(dgwEstaciones, 15);//MODELO
            dgwEstaciones.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[9].Width = ColumnWith(dgwEstaciones, 12);//INICIO
            dgwEstaciones.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //dgwEstaciones.Columns[10].Width = ColumnWith(dgwEstaciones, 15);//FINAL
            //dgwEstaciones.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[11].Width = ColumnWith(dgwEstaciones, 12);//INI REAL
            dgwEstaciones.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[11].ReadOnly = true;

            //dgwEstaciones.Columns[12].Width = ColumnWith(dgwEstaciones, 10);//FIN REAL
            //dgwEstaciones.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[12].ReadOnly = true;
            
            dgwEstaciones.Columns[13].Width = ColumnWith(dgwEstaciones, 14);//ESTATUS
            dgwEstaciones.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[13].ReadOnly = true;
        }
        private void dgwEstaciones_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
        }
        //ESCANER SOBRE #OPERADOR && LINEA OP
        private void dgwEstaciones_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 16)
                {
                    _lbCambioDet = true;
                    //BUSCAR LINEA DEL VALOR CAPTURADO Y ASIGNAR PLANTA Y LINEA DEL CATALOGO

                }

                if (e.ColumnIndex == 9)
                {
                    _lbCambioDet = true;
                    string sVal = dgwEstaciones[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
                    if (string.IsNullOrEmpty(sVal) || string.IsNullOrWhiteSpace(sVal))
                        return;

                    string sHora = sVal.Trim();
                    if (sHora.IndexOf("AM") == -1 && sHora.IndexOf("PM") == -1)
                    {
                        MessageBox.Show("Formato de Hora Incorrecto. (00:00 AM/PM)", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = string.Empty;
                        return;
                    }
                    else
                    {
                        if(sHora.IndexOf(":") == -1)
                        {
                            MessageBox.Show("Formato de Hora Incorrecto. (00:00 AM/PM)", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = string.Empty;
                            return;
                        }
                    }
                }

                if (e.ColumnIndex == 4)
                {
                    _lbCambioDet = true;

                    string sVal = dgwEstaciones[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
                    if (string.IsNullOrEmpty(sVal) || string.IsNullOrWhiteSpace(sVal))
                        return;

                    string sRPO = sVal.Trim();
                    if (sRPO.IndexOf("RPO") == -1)
                    {
                        int iRpo = 0;
                        if (int.TryParse(sRPO, out iRpo))
                            sRPO = "RPO" + sRPO;
                        else
                        {
                            MessageBox.Show("El RPO es incorrecto", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = string.Empty;
                            return;
                        }
                    }
                    dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = sRPO;
                    string sModelo = string.Empty;
                    int iCant = 0;
                    if (ConfigLogica.VerificaRpoOrbis())
                    {
                        RpoLogica rpo = new RpoLogica();
                        rpo.RPO = sRPO;
                        DataTable dtRpo = RpoLogica.ConsultarOrbis(rpo);
                        if (dtRpo.Rows.Count != 0)
                        {
                            sModelo = dtRpo.Rows[0]["Producto"].ToString();
                            if (!int.TryParse(dtRpo.Rows[0]["cantidadProducir"].ToString(), out iCant))
                                iCant = 0;
                        }
                    }

                    dgwEstaciones[5, e.RowIndex].Value = sModelo;
                    dgwEstaciones[6, e.RowIndex].Value = iCant;
                }

                if (e.ColumnIndex == 5 || e.ColumnIndex == 8)
                {
                    _lbCambioDet = true;
                }
                if (e.ColumnIndex == 7)
                {
                    _lbCambioDet = true;

                    string sVal = dgwEstaciones[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
                    if (string.IsNullOrEmpty(sVal) || string.IsNullOrWhiteSpace(sVal))
                        return;

                    string sRPO = sVal.Trim();
                    if (sRPO.IndexOf("RPO") == -1)
                    {
                        int iRpo = 0;
                        if (int.TryParse(sRPO, out iRpo))
                            sRPO = "RPO" + sRPO;
                        else
                        {
                            MessageBox.Show("El RPO es incorrecto", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = string.Empty;
                            return;
                        }
                    }
                    dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = sRPO;
                    string sModelo = string.Empty;
                    if (ConfigLogica.VerificaRpoOrbis())
                    {
                        RpoLogica rpo = new RpoLogica();
                        rpo.RPO = sRPO;
                        System.Data.DataTable dtRpo = RpoLogica.ConsultarOrbis(rpo);
                        if (dtRpo.Rows.Count != 0)
                            sModelo = dtRpo.Rows[0]["Producto"].ToString();
                    }
                    dgwEstaciones[8, e.RowIndex].Value = sModelo;
                    
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

            if (e.KeyCode == Keys.F10)
            {
                btnHelp_Click(sender, e);
            }

            if (e.KeyCode == Keys.F1)
            {
                btnLine_Click(sender, e);//LINEA
            }

            if (e.KeyCode == Keys.F2)
            {
                btnTime_Click(sender, e);//HORA
            }

            if (e.KeyCode == Keys.F3)
            {
                btnF1_Click(sender, e);//ACTIVIDADES
            }

            if (e.KeyCode == Keys.F4)
            {
                btnNots_Click(sender, e);//NOTAS
            }

            if (e.KeyCode == Keys.F5)//NUEVO
            {
                btNew_Click(sender, e);
            }

            if (e.KeyCode == Keys.F6)//GUARDAR
            {
                btnSave2_Click(sender, e);
            }

            if (e.KeyCode == Keys.F8)//COPIAR RPO SIG A LINEA ANTERIOR
            {
                CopiarLineaAnt();
            }
            
        }

        private void dgwEstaciones_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgwEstaciones_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        #endregion

        #region regCambio
        
        
        #endregion

        #region regColor
        private void txtFolio_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtFolio, 0);
        }

        private void txtFolio_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtFolio, 1);
        }
        
        #endregion

        #region regCaptura
        private void txtFolio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtFolio.Text) || string.IsNullOrWhiteSpace(txtFolio.Text))
                return;

            try
            {
                LineSetupLogica line = new LineSetupLogica();
                line.Folio = Convert.ToInt32(txtFolio.Text.ToString());
                System.Data.DataTable data = LineSetupLogica.Consultar(line);
                if(data.Rows.Count != 0)
                {
                    cbbPlanta.SelectedValue = data.Rows[0]["planner"].ToString();
                    dtpFecha.Value = Convert.ToDateTime(data.Rows[0]["fecha"].ToString());

                    _lsFolioAnt = txtFolio.Text.ToString();
                    //ESTATUS
                    CargarDetalle(Convert.ToInt32(txtFolio.Text));
                }
                else
                {
                    MessageBox.Show("El Folio no se encuentra Registrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtFolio.Text = _lsFolioAnt;
                    txtFolio.Focus();
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }
        private void cbbLinea_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(_lbCambio)
            {
                DialogResult Result = MessageBox.Show("Desea Guardar los Cambios?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (Guardar())
                        Inicio();
                }
                else
                {
                    if (Result == DialogResult.No)
                        Inicio();
                }
            }
             
        }
         
        private void tstFolio_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode != Keys.Enter)
            //    return;

            //if (string.IsNullOrEmpty(tstFolio.Text) || string.IsNullOrWhiteSpace(tstFolio.Text))
            //    return;

            //try
            //{
            //    LineUpLogica line = new LineUpLogica();
            //    line.Folio = Convert.ToInt32(tstFolio.Text.ToString());
            //    System.Data.DataTable data = LineUpLogica.Consultar(line);
            //    if (data.Rows.Count != 0)
            //    {
            //        txtFolio.Text = data.Rows[0]["folio"].ToString();
            //        cbbPlanta.SelectedValue = data.Rows[0]["planta"].ToString();
            //        cbbLinea.SelectedValue = data.Rows[0]["linea"].ToString();
                     

            //        _lsFolioAnt = tstFolio.Text.ToString();

            //        CargarDetalle(Convert.ToInt32(tstFolio.Text));
            //    }
            //    else
            //    {
            //        MessageBox.Show("El Folio no se encuentra Registrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        txtFolio.Text = _lsFolioAnt;
            //        txtFolio.Focus();
            //        return;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
        }
       
        private void txtRpo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            
            

            string sRPO = txtFolio.Text.ToString().Trim();
            if (sRPO.IndexOf("RPO") == -1)
            {
                int iRpo = 0;
                if(int.TryParse(sRPO, out iRpo))
                    sRPO = "RPO" + sRPO;
                else
                {
                    if(sRPO != lblLinea.Text)
                    {
                        MessageBox.Show("El RPO es incorrecto", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                         
                        return;
                    }
                }
                 
            }

            int iPos = sRPO.IndexOf("-");
            int iIni = 0;
            if (iPos > 0)
            {
                string sIni = sRPO.Substring(iPos + 2, 5);
                Int32.TryParse(sIni, out iIni);
                iIni++;

                sRPO = sRPO.Substring(0, iPos);
            }

          
            
            try
            {
                if (!ConfigLogica.VerificaOmiteValidaLup())
                {
                    LineUpLogica line = new LineUpLogica();
                    
                    line.Fecha = dtpFecha.Value;
                    line.RPO = sRPO;
                    line.Turno = _lsTurno;

                    if (LineUpLogica.VerificaRPO(line))
                    {
                        MessageBox.Show("El RPO ya se encuentra cargado en un registro anterior. Favor de proporcionar el RPO correcto", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                         
                        return;
                    }
                }


                //..CARGAR MODELO DESDE RPO DE ORBIS..\\
                /*
                 *LEER PARAMETRO
                 * CARGAR RPO DESDE ORBIS
                 * OBTENER MODELO - CORE - CRC[no esta en rpo2 de orbis]
                 * CARGAR LAYOUT
                 * BLOQUEAR CAPTURA DE MODELO Y CORE
                 */
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            

            //txtModelo.Focus();
        }
      
        private void AgregaEstacion(string _asLinea, string _asModelo, int _aiCveEstacion, string _asEstacion, string _asOperacion, string _asTipo, string _asTipona, string _asNivel,string _asSinNivel)
        {
            System.Data.DataTable dt = dgwEstaciones.DataSource as System.Data.DataTable;
            dt.Rows.Add(null, 0, _asLinea, _asModelo, _aiCveEstacion, _asEstacion, _asOperacion,_asNivel, null, null, null, null, _asTipo, _asTipona,_asSinNivel);
        }
      
        
        private void cbbCore_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
        }
        
        #endregion

        #region regFuncion
        private void tssF1_Click(object sender, EventArgs e)
        {
            
            
        }
        
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

        #region regFlechas
        private void btnFirst_Click(object sender, EventArgs e)
        {
            string sValor = GlobalVar.Navegacion("F", "t_lineset", "folio", txtFolio.Text);
            _lsParam = sValor;

            if (_lbCambio)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (Guardar())
                        wfLineSetUp_Load(sender, e);
                }
                else
                {
                    if (Result == DialogResult.No)
                        wfLineSetUp_Load(sender, e);
                }
            }
            else
                wfLineSetUp_Load(sender, e);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            string sValor = "";
            if (string.IsNullOrEmpty(txtFolio.Text))
                sValor = GlobalVar.Navegacion("F", "t_lineset", "folio", txtFolio.Text);
            else
                sValor = GlobalVar.Navegacion("B", "t_lineset", "folio", txtFolio.Text);
            _lsParam = sValor;


            if (_lbCambio)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (Guardar())
                        wfLineSetUp_Load(sender, e);
                }
                else
                {
                    if (Result == DialogResult.No)
                        wfLineSetUp_Load(sender, e);
                }
            }
            else
                wfLineSetUp_Load(sender, e);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_lbCambio)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                    Guardar();
                _lbCambio = false;
            }

            string sValor = string.Empty;
            if (string.IsNullOrEmpty(txtFolio.Text))
                sValor = GlobalVar.Navegacion("F", "t_lineset", "folio", txtFolio.Text);
            else
                sValor = GlobalVar.Navegacion("N", "t_lineset", "folio", txtFolio.Text);

            _lsParam = sValor;
            wfLineSetUp_Load(sender, e);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (_lbCambio)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                    Guardar();
                _lbCambio = false;
            }

            string sValor = GlobalVar.Navegacion("L", "t_lineset", "folio", txtFolio.Text);
            _lsParam = sValor;
            wfLineSetUp_Load(sender, e);
        }
        #endregion

        #region regRes

        private void wfLineSetUp_Resize(object sender, EventArgs e)
        {
            if (WindowState != _WindowStateAnt && WindowState != FormWindowState.Minimized)
            {
                _WindowStateAnt = WindowState;
                ResizeControl(panel1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(groupBox1, 1, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(panel3, 1, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(dgwEstaciones, 1, ref _iWidthAnt, ref _iHeightAnt, 1);

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

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFolio.Text))
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "25") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                string sPlanner = string.Empty;

                if (cbbPlanta.SelectedIndex != -1)
                    sPlanner = cbbPlanta.SelectedValue.ToString();
                DateTime dtFecha = dtpFecha.Value;

                Inicio();

                if (!string.IsNullOrEmpty(sPlanner))
                {
                    cbbPlanta.SelectedValue = sPlanner;
                    dtpFecha.Value = dtFecha;
                }



                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Archivos de Excel (*.xls;*.xlsx)|*.xls;*.xlsx";

                dialog.Title = "Seleccione el archivo de Excel";

                dialog.FileName = string.Empty;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    string sFile = dialog.FileName;
                    string sHoja = "Sheet1";
                    DataTable dt = LlenarGrid(sFile, sHoja);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontró infomrmación para importar desde el archivo Excel", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Cursor = Cursors.Default;
                        return;
                    }

                    string sLineaAnt = string.Empty;
                    string sLinea = string.Empty;
                    for (int x = 0; x <= dt.Rows.Count; x++)
                    {
                        if (string.IsNullOrEmpty(dt.Rows[x][1].ToString()))
                            break;

                        if (string.IsNullOrEmpty(dt.Rows[x][0].ToString()))
                            sLinea = sLineaAnt;
                        else
                            sLinea = dt.Rows[x][0].ToString();

                        string sPlanta = string.Empty;
                        LineaLogica line = new LineaLogica();
                        line.Linea = sLinea;
                        DataTable dtP = LineaLogica.ConsultarPlanta(line);
                        if (dtP.Rows.Count != 0)
                            sPlanta = dtP.Rows[0]["planta"].ToString();

                        if (string.IsNullOrEmpty(sPlanta))
                        {
                            MessageBox.Show("La linea " + sLinea + " no se encuentra registrada en el sistema. Favor de verificar el listado.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            continue;
                        }

                        string sRPO = dt.Rows[x][1].ToString();
                        string sModelo = dt.Rows[x][2].ToString();
                        string sRPO2 = dt.Rows[x][3].ToString();
                        string sModelo2 = dt.Rows[x][4].ToString();

                        int iRow = dgwEstaciones.RowCount;

                        DataTable dtAdd = dgwEstaciones.DataSource as DataTable;
                        dtAdd.Rows.Add(null,null,sPlanta, sLinea,sRPO,sModelo,0,sRPO2,sModelo2,null,null,null,null,null,null,null,null,null);
                        

                        sLineaAnt = sLinea;

                    }

                    _lbCambioDet = true;

                    Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        private DataTable LlenarGrid(string archivo, string hoja)
        {
            DataTable table = new DataTable();
            //declaramos las variables         
            OleDbConnection conexion = null;
            DataSet dataSet = null;
            OleDbDataAdapter dataAdapter = null;
            string consultaHojaExcel = "Select * from [" + hoja + "$]";

            //esta cadena es para archivos excel 2007 y 2010
            string cadenaConexionArchivoExcel = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Mode=ReadWrite;Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";
            //Validamos que el usuario ingrese el nombre de la hoja del archivo de excel a leer
            
            try
            {
                conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
                conexion.Open(); //abrimos la conexion
                dataAdapter = new OleDbDataAdapter(consultaHojaExcel, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
                dataSet = new DataSet(); // creamos la instancia del objeto DataSet
                dataAdapter.TableMappings.Add("tbl", "Table");
                dataAdapter.Fill(dataSet);//llenamos el dataset
                table = dataSet.Tables[0];

                conexion.Close();//cerramos la conexion
            }
            catch (Exception ex)
            {
                //en caso de haber una excepcion que nos mande un mensaje de error
                MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja", ex.Message);
            }

            return table;
        }
        
        private void btnUrgencia_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "60") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string sInd = dgwEstaciones[16, iRow].Value.ToString();
            if (string.IsNullOrEmpty(sInd))
                sInd = "0";

            DataGridViewRow row = dgwEstaciones.Rows[iRow];
            if (sInd == "0")
            {
                dgwEstaciones[16, iRow].Value = "1";//-- INDICADOR * URGENTE --\\    
                row.DefaultCellStyle.ForeColor = Color.Green;
            }
            else
            {
                dgwEstaciones[16, iRow].Value = "0";
                row.DefaultCellStyle.ForeColor = Color.Black;
            }
                

            dgwEstaciones.Focus();
            dgwEstaciones.CurrentCell = dgwEstaciones[4, iRow];
        }
        private void btnLine_Click(object sender, EventArgs e) //PLANTA Y LINEA
        {
            if(cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de Ingresar el Planner", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return;
            }

            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "10") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {

                long lFolio = 0;
                if (long.TryParse(dgwEstaciones[0, iRow].Value.ToString(), out lFolio))
                    return;

                //int iCons = int.Parse(dgwEstaciones[1, iRow].Value.ToString());
                
                string sPlanner = cbbPlanta.SelectedValue.ToString();
                UsuarioLogica user = new UsuarioLogica();
                user.Usuario = sPlanner;
                DataTable dtU = UsuarioLogica.Consultar(user);
                string sPlanta = dtU.Rows[0]["planta"].ToString();

                wfPlantaPop_1t PlantaPop = new wfPlantaPop_1t(sPlanta);
                PlantaPop.ShowDialog();
                if(!string.IsNullOrEmpty(PlantaPop._lsPlanta))
                    dgwEstaciones[2, iRow].Value = PlantaPop._lsPlanta;
                if(!string.IsNullOrEmpty(PlantaPop._lsLinea))
                    dgwEstaciones[3, iRow].Value = PlantaPop._lsLinea;

                dgwEstaciones.Focus();
                dgwEstaciones.CurrentCell = dgwEstaciones[4, iRow];

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnTime_Click(object sender, EventArgs e) //HORA INICIO
        {
            if (string.IsNullOrEmpty(txtFolio.Text))
                return;

            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            string sEst = dgwEstaciones[14, iRow].Value.ToString();
            if (sEst == "C")
            {
                MessageBox.Show("No se permite modificar registros Cancelados", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "35") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                long lFolio = long.Parse(dgwEstaciones[0, iRow].Value.ToString());
                int iCons = int.Parse(dgwEstaciones[1, iRow].Value.ToString());

                wfActividadesPop ActPop = new wfActividadesPop(lFolio.ToString() + "-" + iCons.ToString());
                ActPop.ShowDialog();
                if(!string.IsNullOrEmpty(ActPop._sClave))
                    dgwEstaciones[9, iRow].Value = ActPop._sClave;

                dgwEstaciones.Focus();
                dgwEstaciones.CurrentCell = dgwEstaciones[9, iRow];

            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnF1_Click(object sender, EventArgs e) //NOTAS
        {
            if (string.IsNullOrEmpty(txtFolio.Text))
                return;

            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            string sEst = dgwEstaciones[14, iRow].Value.ToString();
            if (sEst == "C")
            {
                MessageBox.Show("No se permite modificar registros Cancelados", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            iRow = dgwEstaciones.CurrentCell.RowIndex;
            string sRPO = dgwEstaciones[2, iRow].Value.ToString();
            if(string.IsNullOrEmpty(sRPO))
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "40") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                #region regCapturaNota

                long lFolio = long.Parse(dgwEstaciones[0, iRow].Value.ToString());
                int iCons = int.Parse(dgwEstaciones[1, iRow].Value.ToString());
                if(!string.IsNullOrEmpty(_lsArea))
                {
                    wfActividEstatusPop ActPop = new wfActividEstatusPop();
                    ActPop._lFolio = lFolio;
                    ActPop._iConsec = iCons;
                    ActPop._sArea = _lsArea;
                    ActPop.ShowDialog();
                }
                else
                {
                    wfActividadesPop2 ActPop2 = new wfActividadesPop2(lFolio.ToString() + "-" + iCons.ToString());
                    ActPop2.ShowDialog();
                }
                

                dgwEstaciones.Focus();
                dgwEstaciones.CurrentCell = dgwEstaciones[8, 0];

                #endregion
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(),"ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;
            else
            {
                if (!string.IsNullOrEmpty(dgwEstaciones[0, iRow].Value.ToString()))
                    return;
            }

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            
            try
            {
                if (iRow < dgwEstaciones.Rows.Count - 1)
                    dgwEstaciones.Rows.Remove(dgwEstaciones.CurrentRow);
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
                
            
            
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;
            else
            {
                if (string.IsNullOrEmpty(dgwEstaciones[2, iRow].Value.ToString()))
                    return;

                if (string.IsNullOrEmpty(dgwEstaciones[0, iRow].Value.ToString()))
                    return;
            }

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "30") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult Result = MessageBox.Show("Desea Cancelar el Cambio de Setup de la Linea?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                try
                {
                    long lFolio = long.Parse(dgwEstaciones[0, iRow].Value.ToString());
                    int iCons = Convert.ToInt16(dgwEstaciones[1, iRow].Value.ToString());

                    string sComent = string.Empty;

                    wfComentarioPop Coment = new wfComentarioPop("CANCEL");
                    Coment._lsProceso = _lsProceso;
                    Coment.ShowDialog();
                    sComent = Coment._sClave;
                    
                    if(!string.IsNullOrEmpty(sComent))
                    {
                        LineSetupDetLogica sed = new LineSetupDetLogica();
                        sed.Folio = lFolio;
                        sed.Consec = iCons;
                        sed.CancelComent = sComent;
                        LineSetupDetLogica.Cancelar(sed);
                        CancelRow(iRow);
                    }
                    else
                    {
                        MessageBox.Show("Se Requiere el Motivo de la Cancelación para Proceder",Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
        }

        private void CancelRow(int _aiRow)
        {
            DataGridViewRow row = dgwEstaciones.Rows[_aiRow];
            row.DefaultCellStyle.ForeColor = Color.Red;
        }

        private void btnNots_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            iRow = dgwEstaciones.CurrentCell.RowIndex;
            string sRPO = dgwEstaciones[2, iRow].Value.ToString();
            if (string.IsNullOrEmpty(sRPO))
                return;

            string sEst = dgwEstaciones[14, iRow].Value.ToString();
            if(sEst == "C")
            {
                MessageBox.Show("No se permite modificar registros Cancelados",Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "50") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string sComent = dgwEstaciones[15, iRow].Value.ToString();
            string sParam = "";
            if(!string.IsNullOrEmpty(txtFolio.Text))
                sParam = dgwEstaciones[0, iRow].Value.ToString() + "-" + dgwEstaciones[1, iRow].Value.ToString();

            if (!string.IsNullOrEmpty(sComent))
                sParam += "*" + sComent;

            wfComentarioPop Coment = new wfComentarioPop(sParam);
            Coment._lsProceso = _lsProceso;
            Coment.ShowDialog();
            //Coment._sClave = sComent;
            if (Coment._sClave != sComent)
                _lbCambioDet = true;

            sComent = Coment._sClave;

            dgwEstaciones[15, iRow].Value = sComent;

            dgwEstaciones.Focus();
            dgwEstaciones.CurrentCell = dgwEstaciones[3, 0];
        }
        
        private void CopiarLineaAnt()
        {
            if (dgwEstaciones.Rows.Count <= 1)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            int iRowAnt = iRow - 1;

            if (iRowAnt == -1)
                return;

            string sPlanta = dgwEstaciones[2, iRowAnt].Value.ToString();
            string sLinea = dgwEstaciones[3, iRowAnt].Value.ToString();

            string sRPO = dgwEstaciones[7, iRowAnt].Value.ToString();
            string sModelo = dgwEstaciones[8, iRowAnt].Value.ToString();
            if (string.IsNullOrEmpty(sRPO))
                return;

            dgwEstaciones[2, iRow].Value = sPlanta;
            dgwEstaciones[3, iRow].Value = sLinea;
            dgwEstaciones[4, iRow].Value = sRPO;
            dgwEstaciones[5, iRow].Value = sModelo;

            dgwEstaciones.Focus();
            dgwEstaciones.CurrentCell = dgwEstaciones[7, iRow];
        }

        #endregion

        private void dgwEstaciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            if (e.ColumnIndex == 3)
            {
                btnLine_Click(sender, e);
            }
        }

        private void dgwEstaciones_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            
        }

        
    }
}
