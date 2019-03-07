using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using System.Windows.Forms;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
using System.Management;
using Logica;
using Datos;

namespace CloverPro
{
    public partial class wfCargarRPO : Form
    {
        private string _lsProceso = "CAT065";
        public string _lsPlanta = "";
        public wfCargarRPO()
        {
            InitializeComponent();
        }
        #region regInicio
        private void Inicio()
        {

            txtRPO.Clear();
            txtSKU.Clear();
            txtCant.Clear();
            cbbOrigen.ResetText();

            dtpFecha.ResetText();
            txtHrComp.Clear();
            txtHrEnt.Clear();
            txtCantGlob.ResetText();
            txtNota.ResetText();

            if(!string.IsNullOrEmpty(_lsPlanta))
            {
                cbbOrigen.ResetText();
                LineaLogica lin = new LineaLogica();
                lin.Planta = _lsPlanta;
                DataTable data = LineaLogica.LineaPlanta(lin);

                data = LineaLogica.LineasNav(lin);
                cbbOrigen.DataSource = data;
                cbbOrigen.DisplayMember = "linea_nav";
                cbbOrigen.ValueMember = "linea_nav";
               
                cbbOrigen.SelectedIndex = -1;
            }

            cbbPrioridad.ResetText();
            Dictionary<string, string> Data = new Dictionary<string, string>();
            Data.Add("0", "0");
            Data.Add("HOT", "HOT");
            Data.Add("GLO", "GLOBAL");
            Data.Add("RBX", "REBOX");
            Data.Add("BKO", "BACK ORDER");
            Data.Add("PDU", "PAST DUE");
            Data.Add("FUS", "FUSORES");
            Data.Add("PT1", "PTO 1/19");
            Data.Add("PT2", "PTO 2/08");
            Data.Add("C1B", "CAN01 BRND");
            Data.Add("XRX", "XRX CAN01");
            Data.Add("PSB", "PUSHBACK");
            cbbPrioridad.DataSource = new BindingSource(Data, null);
            cbbPrioridad.DisplayMember = "Value";
            cbbPrioridad.ValueMember = "Key";
            cbbPrioridad.SelectedIndex = -1;

            cbbTurno.ResetText();
            Dictionary<string, string> turno = new Dictionary<string, string>();
            turno.Add("1", "1");
            turno.Add("2", "2");
            cbbTurno.DataSource = new BindingSource(turno, null);
            cbbTurno.DisplayMember = "Value";
            cbbTurno.ValueMember = "Key";
            cbbTurno.SelectedIndex = 0;

            cbbDestino.ResetText();
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("CLX", "Calexico");
            list.Add("VNS", "Van Nuys");
            list.Add("C&V", "Calexico / Van Nuys");
            cbbDestino.DataSource = new BindingSource(list, null);
            cbbDestino.DisplayMember = "Value";
            cbbDestino.ValueMember = "Key";
            cbbDestino.SelectedIndex = -1;

            cbbOwner.ResetText();
            Dictionary<string, string> Owner = new Dictionary<string, string>();
            Owner.Add("ALM", "Almacén");
            Owner.Add("PRP", "Producción");
            Owner.Add("ENV", "Envios");
            cbbOwner.DataSource = new BindingSource(Owner, null);
            cbbOwner.DisplayMember = "Value";
            cbbOwner.ValueMember = "Key";
            cbbOwner.SelectedIndex = -1;

            cbbSupervisor.ResetText();
            UsuarioLogica user = new UsuarioLogica();
            user.Planta = "EMPN";
            user.Turno = "1";
            user.Area = "SPG";
            DataTable dtS = UsuarioLogica.ListarSuperParam(user);
            cbbSupervisor.DataSource = dtS;
            cbbSupervisor.ValueMember = "usuario";
            cbbSupervisor.DisplayMember = "nombre";
            cbbSupervisor.SelectedIndex = -1;

            tslPlanta.Text = _lsPlanta;

            txtRPO.Focus();
            
        }
        private void wfCargarRPO_Activated(object sender, EventArgs e)
        {
            txtRPO.Focus();
        }

        private void wfCargarRPO_Load(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(_lsPlanta))
            {
                UsuarioLogica user = new UsuarioLogica();
                user.Usuario = GlobalVar.gsUsuario;
                DataTable dtU = UsuarioLogica.Consultar(user);
                _lsPlanta = dtU.Rows[0]["planta"].ToString();
            }

            Inicio();
            txtRPO.Focus();
        }

        #endregion

        #region regGuardar 
        private bool Valida()
        {
            if (string.IsNullOrEmpty(txtRPO.Text) || string.IsNullOrWhiteSpace(txtRPO.Text))
            {
                if (cbbPrioridad.SelectedIndex == -1)
                    return false;
                else
                {
                    if (cbbPrioridad.SelectedValue.ToString() !="GLO")
                        return false;
                }
            }

            if (string.IsNullOrEmpty(txtSKU.Text) || string.IsNullOrWhiteSpace(txtSKU.Text))
                return false;
            else
            {
                if (txtSKU.Text.ToString() == txtRPO.Text.ToString())
                {
                    MessageBox.Show("El RPO debe ser direferente al Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            int iCant = 0;
            if(!int.TryParse(txtCant.Text.ToString(), out iCant))
            {
                MessageBox.Show("Favor de indicar la cantidad", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCant.Focus();
                return false;
            }


            if (cbbPrioridad.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de indicar la prioridad", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPrioridad.Focus();
                return false;
            }

            if (cbbPrioridad.SelectedValue.ToString() == "GLO")
            {
                if (cbbDestino.SelectedIndex == -1)
                {

                    MessageBox.Show("Favor de indicar el Destino", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbbDestino.Focus();
                    return false;
                }
                if(string.IsNullOrEmpty(txtHrComp.Text) || txtHrComp.Text == " :")
                {
                    MessageBox.Show("Favor de indicar la Hora compromiso", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtHrComp.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(txtCantGlob.Text) || txtCant.Text == "0")
                {
                    MessageBox.Show("Favor de indicar la Cantidad global", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCantGlob.Focus();
                    return false;
                }
            }
            else
            {
                if (cbbOrigen.SelectedIndex == -1)
                {

                    MessageBox.Show("Favor de indicar la linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbbOrigen.Focus();
                    return false;
                }
                if (chbNoto.Checked)
                {
                    MessageBox.Show("NO TO aplica solo para Globals", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbbPrioridad.Focus();
                    return false;
                }

            }
            
            return true;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            string sRPO = txtRPO.Text.ToString().Trim();
            if (!chbNoto.Checked)
            {
                if (sRPO.IndexOf("RPO") == -1)
                    sRPO = "RPO" + sRPO;
            }

            if (Guardar(sRPO))
                Inicio();
            
        }
        
        private bool Guardar(string asRPO)
        {
            try
            {
                bool bReturn;

                ControlRpoLogica rpo = new ControlRpoLogica();

                rpo.Fecha = DateTime.Today;
                rpo.Planta = _lsPlanta;

                long lFolio = ControlRpoLogica.ConsultarUltFolio(rpo);
                if (lFolio > 0)
                {
                    //long lFolio = long.Parse(data.Rows[0][0].ToString());
                    rpo.Folio = lFolio;
                    rpo.Consec = 0;
                    if(cbbOrigen.SelectedIndex != -1)
                        rpo.Linea = cbbOrigen.SelectedValue.ToString();
                    else
                        rpo.Linea = cbbOrigen.Text.ToString();
                    rpo.RPO = asRPO;
                    rpo.Modelo = txtSKU.Text.ToString().ToUpper().Trim();
                    rpo.Cantidad = int.Parse(txtCant.Text.ToString().Trim());
                    rpo.Surte = null;
                    rpo.Etiqueta = null;
                    rpo.Almacen = null;
                    rpo.Entrega = null;
                    rpo.Turno = cbbTurno.SelectedValue.ToString();
                    rpo.WP = null;
                    rpo.Prioridad = cbbPrioridad.SelectedValue.ToString();
                    rpo.CargaInd = "1";
                    rpo.Usuario = GlobalVar.gsUsuario;
                    if(rpo.Prioridad=="GLO")
                    {
                        rpo.FechaGlob = dtpFecha.Value;
                        if (txtHrComp.Text == "  :")
                            rpo.HoraComp = string.Empty;
                        else
                            rpo.HoraComp = txtHrComp.Text.ToString();
                        if(cbbDestino.SelectedIndex != -1)
                            rpo.Destino = cbbDestino.SelectedValue.ToString();
                        if (txtHrEnt.Text == "  :")
                            rpo.HoraEntrega = string.Empty;
                        else
                            rpo.HoraEntrega = txtHrEnt.Text.ToString();

                        rpo.CantGlobal = int.Parse(txtCantGlob.Text.ToString());

                        if (cbbOwner.SelectedIndex != -1)
                            rpo.Supervisor = cbbOwner.SelectedValue.ToString();

                        rpo.Nota = txtNota.Text.ToString();
                    }

                    if (ControlRpoLogica.Guardar(rpo) == 0)
                        bReturn = false;
                }
                else
                    return false;

                return true;
            }
            catch(Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show("Favor de Notificar al Administador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }
       
        #endregion

        #region Controles
        private void txtRPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtRPO.Text))
            {
                string sRPO = txtRPO.Text.ToString();

                Inicio();

                txtRPO.Text = sRPO;

                ControlRpoLogica rpo = new ControlRpoLogica();
                rpo.RPO = sRPO;
                long lFolio = ControlRpoLogica.ConsultarUltFolioRPO(rpo);
                rpo.Folio = lFolio;
                rpo.RPO = sRPO;
                DataTable data = ControlRpoLogica.ConsultaRPO(rpo);
                if(data.Rows.Count > 0)
                {
                    txtSKU.Text = data.Rows[0]["modelo"].ToString();
                    txtCant.Text = data.Rows[0]["cantidad"].ToString();
                    cbbTurno.SelectedValue = data.Rows[0]["turno"].ToString();
                    string sPrioridad = data.Rows[0]["prioridad"].ToString();
                    cbbPrioridad.SelectedValue = sPrioridad;
                    _lsPlanta = data.Rows[0]["planta"].ToString();

                    cbbOrigen.ResetText();
                    LineaLogica lin = new LineaLogica();
                    lin.Planta = _lsPlanta;
                    
                    DataTable dtP = LineaLogica.LineasNav(lin);
                    cbbOrigen.DataSource = dtP;
                    cbbOrigen.DisplayMember = "linea_nav";
                    cbbOrigen.ValueMember = "linea_nav";
                    cbbOrigen.SelectedIndex = -1;

                    if (!string.IsNullOrEmpty(data.Rows[0]["linea"].ToString()))
                    {
                        if (sPrioridad == "GLO")
                        {
                            string sLinea = data.Rows[0]["linea"].ToString();
                            lin.Linea = data.Rows[0]["linea"].ToString();
                            if (!LineaLogica.Verificar(lin))
                                cbbOrigen.Text = data.Rows[0]["linea"].ToString();
                            else
                                cbbOrigen.SelectedValue = data.Rows[0]["linea"].ToString();
                        }
                        else
                            cbbOrigen.SelectedValue = data.Rows[0]["linea"].ToString();
                    }
                            
                    if(sPrioridad == "GLO")
                    {
                        if (!string.IsNullOrEmpty(data.Rows[0]["fecha_glo"].ToString()))
                            dtpFecha.Value = Convert.ToDateTime(data.Rows[0]["fecha_glo"].ToString());
                        if (!string.IsNullOrEmpty(data.Rows[0]["destino"].ToString()))
                            cbbDestino.SelectedValue = data.Rows[0]["destino"].ToString();
                        txtHrComp.Text = data.Rows[0]["hr_compromiso"].ToString();
                        txtHrEnt.Text = data.Rows[0]["hr_entrega"].ToString();
                        txtCantGlob.Text = data.Rows[0]["cant_global"].ToString();
                        if (!string.IsNullOrEmpty(data.Rows[0]["supervisor"].ToString()))
                            cbbOwner.SelectedValue = data.Rows[0]["supervisor"].ToString();
                        txtNota.Text = data.Rows[0]["nota"].ToString();

                        groupBox1.Enabled = true;

                    }

                }
                else
                {
                    Inicio();
                    //FIND SKU IN DATABASE
                    txtRPO.Text = sRPO;
                    if(sRPO.IndexOf("NO TO") == -1)
                        BuscarRPO(sRPO);

                    if (string.IsNullOrEmpty(txtSKU.Text.ToString()))
                    {
                        txtSKU.Clear();
                        txtCant.Clear();
                        cbbOrigen.SelectedIndex = -1;
                        cbbPrioridad.SelectedIndex = -1;

                        txtSKU.Focus();
                    }
                }
            }
        }

        private void BuscarRPO(string asRPO)
        {
            try
            {
                if (asRPO.IndexOf("RPO") == -1)
                {
                    asRPO = "RPO" + asRPO;
                    txtRPO.Text = asRPO;
                }
                    
                RpoLogica rpo = new RpoLogica();
                rpo.RPO = asRPO;
                if (!RpoLogica.Verificar(rpo))
                {
                    DataTable dtRpo = RpoLogica.ConsultarOrbis(rpo);
                    if (dtRpo.Rows.Count != 0)
                    {
                        txtSKU.Text = dtRpo.Rows[0]["Producto"].ToString();
                        txtCant.Text = dtRpo.Rows[0]["cantidadProducir"].ToString();
                        cbbOrigen.SelectedValue = dtRpo.Rows[0]["Linea"].ToString();
                    }
                    else
                        MessageBox.Show("No se encontró información del RPO, favor de capturar ó escanear Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    DataTable data = RpoLogica.Consultar(rpo);
                    
                    txtSKU.Text = data.Rows[0]["modelo"].ToString();
                    txtCant.Text = data.Rows[0]["cantidad"].ToString();
                    cbbOrigen.SelectedValue = data.Rows[0]["linea"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error... " + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtSKU_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtSKU.Text))
                txtCant.Focus();
           
        }
        
        private void txtCant_Enter(object sender, EventArgs e)
        {
            
        }

        private void txtCant_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtCant.Text) || txtCant.Text == "0")
                txtCant.Focus();
            else
                cbbOrigen.Focus();
        }

        private void txtCant_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        #endregion

        #region regBotones
        private void btnNew_Click(object sender, EventArgs e)
        {
            Inicio();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnHelp_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ConfigLogica.Consultar();
                string sDirec = dt.Rows[0]["directorio"].ToString();
                if (!string.IsNullOrEmpty(sDirec))
                {
                    sDirec += @"\CloverPRO_AyudaVisual_e.pdf";
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

        private void cbbOrigen_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }

        private void cbbPrioridad_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(cbbPrioridad.SelectedValue.ToString() == "GLO")
            {
                int iHora = DateTime.Now.Hour;
                int iMin = DateTime.Now.Minute;
                txtHrComp.Text = "1300";
                if (iHora >= 10 && iMin >= 30 && iHora < 14)
                    txtHrComp.Text = "1500";

                cbbTurno.SelectedIndex = 0;
                groupBox1.Enabled = true;
                chbGlobal.Checked = true;
                txtCantGlob.Text = txtCant.Text.ToString();

                if (cbbDestino.SelectedIndex == -1)
                    cbbDestino.SelectedIndex = 0;
                    

                txtHrEnt.Focus();

            }
            else
            {
                groupBox1.Enabled = false;
                chbGlobal.Checked = false;
            }
                
        }

        private void txtHrEnt_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtHrEnt, 1);
        }

        private void txtHrEnt_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtHrEnt, 0);
        }

        private void txtHrComp_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtHrComp, 0);
        }

        private void txtHrComp_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtHrComp, 1);
        }

        private void chbNoto_CheckedChanged(object sender, EventArgs e)
        {
            if (chbNoto.Checked)
            {
                txtRPO.Enabled = false;
                txtRPO.Text = "NO TO";
                cbbPrioridad.SelectedValue = "GLO";
                cbbOwner.SelectedValue = "ENV";
                groupBox1.Enabled = true;
                txtHrComp.Text = "1300";

                if (cbbDestino.SelectedIndex == -1)
                    cbbDestino.SelectedIndex = 0;
            }
            else
                txtRPO.Enabled = true;

        }

        private void chbFuser_CheckedChanged(object sender, EventArgs e)
        {
            tslPlanta.Text = "EMPF";
            _lsPlanta = "EMPF";
        }

        private void btExcel_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archivos de Excel (*.xls;*.xlsx)|*.xls;*.xlsx";

            dialog.Title = "Seleccione el archivo de Excel";

            dialog.FileName = string.Empty;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;

                string sArchivo = dialog.FileName;
                
                DataTable  dt = LoadFile(sArchivo);
                int iRows = SaveRows(dt);
                MessageBox.Show("Registros Cargados: " + iRows.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                Cursor = Cursors.Arrow;
            }
        }
        private int SaveRows(DataTable _dtData)
        {
            int iCont = 0;
            try
            {
                
                ControlRpoLogica rpo = new ControlRpoLogica();

                rpo.Fecha = DateTime.Today;
                rpo.Planta = _lsPlanta;

                long lFolio = ControlRpoLogica.ConsultarUltFolio(rpo);
                int iRows = _dtData.Rows.Count;
                if (lFolio > 0 && iRows > 0)
                {
                    for (int i = 0; i <= iRows; i++)
                    {
                        DateTime dtFecha = DateTime.Today;
                        string sRPO = string.Empty;
                        string sModelo = string.Empty;
                        string sLine = string.Empty;
                        int iCant = 0;
                        int iCantGlob = 0;
                        string sTurno = string.Empty;
                        string sNota = string.Empty;
                        string sHrComp = string.Empty;
                        string sDestino = string.Empty;
                        string sHrEnt = string.Empty;
                        string sOwner = string.Empty;
                        string sComent = string.Empty;

                        string sValue = string.Empty;

                        if (string.IsNullOrEmpty(_dtData.Rows[i][0].ToString()))
                        {
                            i = iRows;
                            continue;
                        } 
                        
                        sValue = _dtData.Rows[i][0].ToString();
                        

                        if (sValue == "RPO")
                        {
                            sValue = string.Empty;
                            continue;
                        }

                        sTurno = "1";

                        sRPO = _dtData.Rows[i][0].ToString();
                        sRPO = sRPO.TrimStart().TrimEnd().ToUpper();

                        sModelo = string.Empty;

                        sModelo = _dtData.Rows[i][1].ToString();
                        sLine = _dtData.Rows[i][2].ToString();
                        if (!int.TryParse(_dtData.Rows[i][3].ToString(), out iCant))
                            iCant = 0;

                        sTurno = _dtData.Rows[i][4].ToString();
                        sNota = _dtData.Rows[i][5].ToString();

                        DateTime dtGlobal = DateTime.Today;
                        if (!DateTime.TryParse(_dtData.Rows[i][6].ToString(), out dtGlobal))
                            dtGlobal = DateTime.Today;
                        
                        if (!int.TryParse(_dtData.Rows[i][7].ToString(), out iCantGlob))
                            iCantGlob = 0;

                        sHrComp = _dtData.Rows[i][8].ToString();
                        DateTime dtHr = DateTime.Today;

                        if (!DateTime.TryParse(sHrComp, out dtHr))
                            continue;
                        sHrComp = dtHr.ToString("H:mm");

                        sDestino = _dtData.Rows[i][9].ToString();
                        sHrEnt = _dtData.Rows[i][10].ToString();
                        if (!DateTime.TryParse(sHrEnt, out dtHr))
                            continue;

                        sHrEnt = dtHr.ToString("H:mm");
                        sOwner = _dtData.Rows[i][11].ToString();
                        sComent = _dtData.Rows[i][12].ToString();

                        rpo.Folio = lFolio;
                        rpo.Consec = 0;
                        rpo.Linea = sLine;
                        rpo.RPO = sRPO;
                        rpo.Modelo = sModelo;
                        rpo.Cantidad = iCant;
                        rpo.Surte = null;
                        rpo.Etiqueta = null;
                        rpo.Almacen = null;
                        rpo.Entrega = null;
                        rpo.Turno = sTurno;
                        rpo.WP = null;
                        rpo.Prioridad = sNota;
                        rpo.CargaInd = "1";
                        rpo.Usuario = GlobalVar.gsUsuario;
                        if (rpo.Prioridad == "GLO")
                        {
                            rpo.FechaGlob = dtGlobal;
                            rpo.HoraComp = sHrComp;
                            rpo.Destino = sDestino;
                            rpo.HoraEntrega = sHrEnt;
                            rpo.CantGlobal = iCantGlob;
                            rpo.Supervisor = sOwner;
                            rpo.Nota = sComent;
                        }
                        else
                        {
                            rpo.HoraComp = null;
                            rpo.Destino = null;
                            rpo.HoraEntrega = null;
                            rpo.CantGlobal = iCant;
                            rpo.Supervisor = null;
                            rpo.Nota = null;
                        }

                        if (ControlRpoLogica.Guardar(rpo) > 0)
                            iCont++;
                    }

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja" + Environment.NewLine + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            return iCont;
        }
        private DataTable LoadFile(string _asFile)
        {
            //declaramos las variables         
            OleDbConnection conexion = null;
            DataSet dataSet = null;
            OleDbDataAdapter dataAdapter = null;

            string consultaHojaExcel = "Select * from [Sheet1$]";

            //esta cadena es para archivos excel 2007 y 2010
            string cadenaConexionArchivoExcel = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + _asFile + "';Mode=ReadWrite;Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";
            
            //Validamos que el usuario ingrese el nombre de la hoja del archivo de excel a leer
            DataTable dtData = new DataTable();
            try
            {
                conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
                conexion.Open(); //abrimos la conexion
                dataAdapter = new OleDbDataAdapter(consultaHojaExcel, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
                dataSet = new DataSet(); // creamos la instancia del objeto DataSet
                dataAdapter.TableMappings.Add("tbl", "Table");
                dataAdapter.Fill(dataSet);//llenamos el dataset
                dtData = dataSet.Tables[0];

                conexion.Close();//cerramos la conexion
            }
            catch (Exception ex)
            {
                //en caso de haber una excepcion que nos mande un mensaje de error
                MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja", ex.Message);
            }
            return dtData;
            
        }

        private void txtNota_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
