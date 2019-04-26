using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Logica;
using Datos;
using System.IO;

namespace CloverPro
{
    public partial class wfLineUp_1t : Form
    {
        public bool _lbCambio;
        private bool _lbCambioDet;
        private bool _lbReqLine;
        private string _lsDuplicado;
        private string _lsFolioAnt;
        private string _lsModeloAnt;
        private string _lsRpoAnt;
        private string _lsModRev;
        private string _lsRevStd;
        private string _lsSinPPH;
        private string _lsProceso = "PRO050";
        public string _lsParam;
        private string _lsTurno;
        private string _lsPlanta;
        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;
        

        public wfLineUp_1t(string _asParam)
        {
            InitializeComponent();
            _lsParam = _asParam;

            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }

        #region regInicio
        private void wfLineUp_1t_Load(object sender, EventArgs e)
        {
            Inicio();

            WindowState = FormWindowState.Maximized;

            if (string.IsNullOrEmpty(_lsParam) || string.IsNullOrWhiteSpace(_lsParam))
                return;
        
            try
            {
                LineUpLogica line = new LineUpLogica();
                line.Folio = Convert.ToInt32(_lsParam);
                System.Data.DataTable data = LineUpLogica.Consultar(line);
                if (data.Rows.Count != 0)
                {
                    txtFolio.Text = _lsParam;
                    tstFolio.Text = _lsParam;
                    _lsPlanta = data.Rows[0]["planta"].ToString();
                    cbbLinea.SelectedValue = data.Rows[0]["linea"].ToString();
                    lblLinea.Text = data.Rows[0]["linea"].ToString();
                    txtModelo.Text = data.Rows[0]["modelo"].ToString();
                    if (!string.IsNullOrEmpty(data.Rows[0]["rpo"].ToString()))
                        txtRpo.Text = data.Rows[0]["rpo"].ToString();
                    dtpFecha.Value = Convert.ToDateTime(data.Rows[0]["fecha"].ToString());
                    if (data.Rows[0]["ind_controlada"].ToString() == "1")
                        chbControl.Checked = true;
                    if (!string.IsNullOrEmpty(data.Rows[0]["id_autorizo"].ToString()))
                        cbbAutorizo.SelectedValue = data.Rows[0]["id_autorizo"].ToString();
                    if (!string.IsNullOrEmpty(data.Rows[0]["modelo_ctrl"].ToString()))
                        txtNota.Text = data.Rows[0]["modelo_ctrl"].ToString();
                    _lsDuplicado = data.Rows[0]["ind_duplicado"].ToString();
                    if (!string.IsNullOrEmpty(data.Rows[0]["core"].ToString()))
                        cbbCore.SelectedValue = data.Rows[0]["core"].ToString();
                    _lsRevStd = data.Rows[0]["ind_revstd"].ToString();
                    _lsModRev = data.Rows[0]["modelo_rev"].ToString();
                    _lsSinPPH = data.Rows[0]["bloq_pph"].ToString();

                    txtRpo.Enabled = false;
                    txtModelo.Enabled = false;
                    cbbCore.Enabled = false;
                    cbbLinea.Enabled = false;
                    chbControl.Enabled = false;
                    txtNota.Enabled = false;
                        
                    //..AYUDA VISUAL PLANTA-LINEA..\\
                    PlantaLogica pl = new PlantaLogica();
                    pl.Planta = _lsPlanta;
                    System.Data.DataTable dtPl = PlantaLogica.Consultar(pl);
                    tssPlanta.Text = dtPl.Rows[0]["nombre"].ToString().ToUpper();

                    lblLineaHelp.Text = "L - " + data.Rows[0]["linea"].ToString();
                    tssLinea.Text = "LINEA " + data.Rows[0]["linea"].ToString();
                    //..\\

                    _lsFolioAnt = txtFolio.Text.ToString();

                    CargarDetalle(Convert.ToInt32(_lsParam));
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
            _lsTurno = GlobalVar.gsTurno;
            _lsPlanta = GlobalVar.gsPlanta;

            txtFolio.Clear();
            tstFolio.Clear();

            lblLinea.Visible = false;
            dtpFecha.Enabled = false;

            dtpFecha.ResetText();
            if(_lsTurno == "2")
            {
                int iHora = dtpFecha.Value.Hour;
                if(iHora >= 0 && iHora < 6)
                {
                    dtpFecha.Value = dtpFecha.Value.AddDays(-1);
                }
            }

            PlantaLogica pl = new PlantaLogica();
            pl.Planta = _lsPlanta;
            System.Data.DataTable dtPl = PlantaLogica.Consultar(pl);
            tssPlanta.Text = dtPl.Rows[0]["nombre"].ToString().ToUpper();


            txtRpo.Clear();
            txtRpo.Enabled = true;
            txtModelo.Clear();
            txtModelo.Enabled = true;

            lblCoreMod.Text = "";

            cbbLinea.Enabled = true;
            cbbLinea.ResetText();
            LineaLogica line = new LineaLogica();
            line.Planta = _lsPlanta;
            if (GlobalVar.gsArea == "PRO")
                cbbLinea.DataSource = LineaLogica.Listar(line);
            else
                cbbLinea.DataSource = LineaLogica.ListarTodas();

            cbbLinea.ValueMember = "linea";
            cbbLinea.DisplayMember = "linea";
            cbbLinea.SelectedIndex = -1;

            Dictionary<string, string> Tipo = new Dictionary<string, string>();
            Tipo.Add("VE", "VE");
            Tipo.Add("CVE", "CVE");
            Tipo.Add("WVE", "WVE");
            Tipo.Add("SVE", "SVE");
            Tipo.Add("NVE", "NVE");
            Tipo.Add("CNV", "CNVE");
            Tipo.Add("WNV", "WNVE");
            Tipo.Add("SNV", "SNVE");
            Tipo.Add("DAM", "DAMAGED");
            cbbCore.DataSource = new BindingSource(Tipo, null);
            cbbCore.DisplayMember = "Value";
            cbbCore.ValueMember = "Key";
            cbbCore.SelectedIndex = -1;

            lblFolio.Visible = false;
            txtFolio.Visible = false;

            chbControl.Checked = false;
            cbbAutorizo.Visible = false;
            UsuarioLogica user = new UsuarioLogica();
            user.Area = "ING";
            cbbAutorizo.DataSource = UsuarioLogica.ListarArea(user);
            cbbAutorizo.ValueMember = "usuario";
            cbbAutorizo.DisplayMember = "nombre";
            cbbAutorizo.SelectedIndex = -1;

            txtNota.Clear();
 
            lblLineaHelp.Text = string.Empty;
            tssLinea.Text = "LINEA";

            _lbReqLine = true; //captura linea obligatorio
            if (GlobalVar.gsArea == "COA")//Coating no maneja lineas ni RPO
                _lbReqLine = false;


            dgwEstaciones.DataSource = null;
            dgwOper.DataSource = null;
            CargarColOpe();
            CargarColumnasNew();

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "70") == true) //CAPTURA ESPECIAL
            {
                dtpFecha.Enabled = true;
            }

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == true) //MODIFICAR [SOLO DETALLE]
            {
                btnBack.Visible = true;
                btnFirst.Visible = true;
                btnNext.Visible = true;
                btnLast.Visible = true;

                lblFolio.Visible = true;
                tstFolio.Visible = true;
                cbbCore.Visible = true;

                toolStripSeparator2.Visible = false;
                
                
                //txtFolio.Visible = true;
                
                //cbbLinea.Location = new System.Drawing.Point(72, 53);
                //lblLinea.Location = new System.Drawing.Point(29,56);

                //txtModelo.Location = new System.Drawing.Point(268, 53);
                //lblModelo.Location = new System.Drawing.Point(210, 56);

                //txtRpo.Location = new System.Drawing.Point(518, 53);
                //lblRpo.Location = new System.Drawing.Point(475, 56);

                //cbbCore.Location = new System.Drawing.Point(450, 53);
                

                //dtpFecha.Location = new System.Drawing.Point(518, 26);
                //lblFecha.Location = new System.Drawing.Point(466, 30);

                //txtFolio.Location = new System.Drawing.Point(72, 26);
                //lblFolio.Location = new System.Drawing.Point(29, 30);

                
                //cbbAutorizo.Location = new System.Drawing.Point(731, 53);
                //chbControl.Location = new System.Drawing.Point(757, 26);

                //lblNota.Visible = false;
                //txtNota.Visible = false;
                //txtNota.Location = new System.Drawing.Point(753, 26);

                txtFolio.Focus();
            }
            else
            {
                btnBack.Visible = false;
                btnFirst.Visible = false;
                btnNext.Visible = false;
                btnLast.Visible = false;

                lblFolio.Visible = false;
                tstFolio.Visible = false;
                cbbCore.Visible = false;

                toolStripSeparator2.Visible = false;

                OperadorLogica Oper = new OperadorLogica();
                Oper.Operador = GlobalVar.gsUsuario;
                System.Data.DataTable dtEmp = OperadorLogica.Consultar(Oper);
                if(dtEmp.Rows.Count != 0)
                {
                    if (!string.IsNullOrEmpty(dtEmp.Rows[0][5].ToString()))
                    {
                        cbbLinea.SelectedValue = dtEmp.Rows[0][5].ToString();
                        lblLinea.Text = dtEmp.Rows[0][5].ToString();
                        //lblLineaHelp.Text = "L - " + dtEmp.Rows[0][5].ToString();
                        lblLineaHelp.Text = dtEmp.Rows[0][5].ToString();
                        tssLinea.Text = "LINEA " + dtEmp.Rows[0]["linea"].ToString();

                        CargarOperadores(_lsPlanta,dtEmp.Rows[0][5].ToString());
                    }
                }

                txtRpo.Focus();
            }

            if (!ConfigLogica.VerificaAgregaEstacion())
                btnF3.Enabled = false;
            else
                btnF3.Enabled = true;

            tssUsuario.Text = GlobalVar.gsNombreUs.ToUpper();

            _lsFolioAnt = string.Empty;
            _lsModeloAnt = string.Empty;
            _lsRpoAnt = string.Empty;
            _lsRpoAnt = string.Empty;
            _lsDuplicado = "0";
            _lsRevStd = "0";
            _lsModRev = string.Empty;

            _lbCambio = false;
            _lbCambioDet = false;

        }
        private void CargarTress(string _asCodigo)
        {
            try
            { 
                bool bExis = false;
                int iCodigo = int.Parse(_asCodigo);
                foreach(DataGridViewRow row in dgwOper.Rows)
                {
                    int iOper =int.Parse(row.Cells[0].Value.ToString());
                    if(iOper == iCodigo)
                    {
                        bExis = true;
                        break;
                    }
                }

                if (!bExis)
                {
                    TressLogica tres = new TressLogica();
                    tres.Codigo = int.Parse(_asCodigo);

                    System.Data.DataTable dt = TressLogica.OperadorLineUp(tres);
                    int iCont = dt.Rows.Count;
                    if (iCont > 0)
                    {
                        
                        string sCodigo = dt.Rows[0]["CODIGO"].ToString();
                        string sNombre = dt.Rows[0]["NOMBRE"].ToString();
                        string sLinea = dt.Rows[0]["LINEA"].ToString();
                        string sPuesto = dt.Rows[0]["NIVEL"].ToString();

                        dt = TressLogica.ConsultaImagen(tres);
                        Image img = byteArrayToImage((byte[])dt.Rows[0]["IM_BLOB"]);

                        string sDatos = sCodigo + Environment.NewLine + sNombre + Environment.NewLine + sLinea + Environment.NewLine + sPuesto;
                        AgregarOperador(sCodigo, img, sDatos);
                        
                        int iEdx = dgwOper.Rows.Count - 1;
                        if (dgwOper.Rows.Count > iEdx)
                            dgwOper[1, iEdx].Selected = true;

                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void CargarOperadores(string _asPlanta,string _asLinea)
        {
            try
            {
                dgwOper.DataSource = null;
                dgwOper.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgwEstaciones.DataSource = null;

                CargarColOpe();

                TressLogica tres = new TressLogica();
                tres.Turno = _lsTurno;
                string sPlanta;
                string sLinea = string.Empty;
                if (_asPlanta == "COL")
                {
                    sPlanta = "002";
                    sLinea = _asLinea.Substring(0, 1) + "TNR" + _asLinea.Substring(1);
                }
                    
                else
                {
                    if (_asPlanta == "FUS")
                    {
                        sPlanta = "003";
                        sLinea = _asLinea;
                    }
                    else
                    {
                        sPlanta = "005";//MONO
                        if(_asLinea.IndexOf("T") == -1)
                            sLinea = "TNR" + _asLinea.PadLeft(2, '0');
                        else
                            sLinea = _asLinea;
                    }
                        
                }

                tres.Planta = sPlanta;
                tres.Linea = sLinea;
                System.Data.DataTable dt = TressLogica.OperadoresLinea(tres);
                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    string sCodigo = dt.Rows[x][0].ToString();
                    string sNombre = dt.Rows[x][1].ToString();
                    string Linea = dt.Rows[x][2].ToString();
                    string sPuesto = dt.Rows[x][3].ToString();

                    Image img = byteArrayToImage((byte[])dt.Rows[x][4]);

                    string sDatos = sCodigo + Environment.NewLine + sNombre + Environment.NewLine + sLinea + Environment.NewLine + sPuesto;
                    AgregarOperador(sCodigo, img, sDatos);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

        }
        
        private void wfLineUp_1t_Activated(object sender, EventArgs e)
        {
            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == true)
            {
                if (dgwEstaciones.RowCount > 0)
                    dgwEstaciones.Focus();
                else
                    txtRpo.Focus();
            }
            else
            {
                if (dgwEstaciones.RowCount > 0)
                    dgwEstaciones.Focus();
                else
                    txtRpo.Focus();
            }
        }
        #endregion

        #region regGuardar
        private bool Valida()
        {
            bool bValida = false;

            if (!_lbCambio && !_lbCambioDet)
                return bValida;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "10") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bValida;
            }

            if (string.IsNullOrEmpty(txtModelo.Text))
            {
                MessageBox.Show("No se a especificado la clave del Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtModelo.Focus();
                return bValida;
            }

            if(_lbReqLine)
            {
                if (string.IsNullOrEmpty(txtRpo.Text) || string.IsNullOrWhiteSpace(txtRpo.Text))
                {
                    MessageBox.Show("No se a especificado el RPO", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtRpo.Focus();
                    return bValida;
                }
                else
                {
                    if(string.IsNullOrEmpty(tstFolio.Text))
                    {
                        if (!ConfigLogica.VerificaOmiteValidaLup())
                        {
                            LineUpLogica line = new LineUpLogica();
                            line.Linea = cbbLinea.SelectedValue.ToString();
                            line.Modelo = txtModelo.Text.ToString();
                            line.Fecha = dtpFecha.Value;
                            line.RPO = txtRpo.Text.ToString();
                            line.Turno = _lsTurno;

                            if (LineUpLogica.VerificaRPO(line))
                            {
                                MessageBox.Show("El RPO ya se encuentra cargado en un registro anterior. Favor de proporcionar el RPO correcto", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtRpo.Focus();
                                return bValida;
                            }
                        }
                    }
                }
            }
            
            if(chbControl.Checked)
            {
                if (cbbLinea.SelectedIndex == -1)
                {
                    MessageBox.Show("Favor de indicar quien Autoriza la Corrida Controlada", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbbAutorizo.Focus();
                    return bValida;
                }

                if(string.IsNullOrEmpty(txtNota.Text))
                {
                    MessageBox.Show("Favor de Registrar el Modelo que esta Corriendo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNota.Focus();
                    GlobalVar.CambiaColor(txtNota, 2);
                    return bValida;
                }

            }

            foreach (DataGridViewRow row in dgwEstaciones.Rows)
            {
                if (dgwEstaciones.IsCurrentRowDirty)
                    dgwEstaciones.CommitEdit(DataGridViewDataErrorContexts.Commit);

                if (string.IsNullOrEmpty(row.Cells[8].Value.ToString()))
                {
                    MessageBox.Show("No se ha especificado el No. del Empleado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgwEstaciones.CurrentCell = dgwEstaciones[8, row.Index];
                    dgwEstaciones.Focus();
                    return bValida;
                }

                string sLinea = row.Cells[11].Value.ToString();
                string sOpe = row.Cells[8].Value.ToString();
                if (string.IsNullOrEmpty(sLinea) && string.IsNullOrWhiteSpace(sLinea) && sOpe != "N/A")
                {
                    MessageBox.Show("La linea del operador no puede estar vacía", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    dgwEstaciones.CurrentCell = dgwEstaciones[8, row.Index];
                    dgwEstaciones.Focus();
                    return bValida;
                }
            }
            return true;    
        }
        private bool Guardar()
        {
            try
            {
                if (!Valida())
                    return false;

                Cursor = Cursors.WaitCursor;
                LineUpLogica line = new LineUpLogica();
                if (string.IsNullOrEmpty(txtFolio.Text))
                    line.Folio = AccesoDatos.Consec(_lsProceso);
                else
                    line.Folio = Convert.ToInt32(tstFolio.Text);

                //ESTACIONES
                foreach (DataGridViewRow row in dgwEstaciones.Rows)
                {
                    if (dgwEstaciones.IsCurrentRowDirty)
                        dgwEstaciones.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    if (!string.IsNullOrEmpty(row.Cells[13].Value.ToString()) && row.Cells[13].Value.ToString() == "A")
                    {
                        string sOper = row.Cells[8].Value.ToString();
                        int iCont = 0;
                        for (int i = 0; i < dgwEstaciones.RowCount; i++)
                        {
                            string sOperAnt = dgwEstaciones[8, i].Value.ToString().ToUpper();
                            if (sOper == sOperAnt)
                                iCont++;
                        }

                        if (iCont == 1)
                            row.Cells[13].Value = null;//NO AUSENCIA
                    }
                }
                int iContNivel = 0;
                foreach (DataGridViewRow row in dgwEstaciones.Rows)
                {
                    try
                    {
                        LineUpDetLogica lined = new LineUpDetLogica();
                        lined.Folio = line.Folio;
                        lined.Consec = Convert.ToInt32(row.Cells[1].Value);
                        lined.Linea = cbbLinea.SelectedValue.ToString();
                        lined.Modelo = txtModelo.Text.ToString();
                        lined.CveEstacion = Convert.ToInt32(row.Cells[4].Value);
                        if (!string.IsNullOrEmpty(row.Cells[5].Value.ToString()))
                            lined.Estacion = Convert.ToString(row.Cells[5].Value);
                        lined.Operacion = Convert.ToString(row.Cells[6].Value);
                        if (!string.IsNullOrEmpty(row.Cells[7].Value.ToString()))
                            lined.NivelReq = Convert.ToString(row.Cells[7].Value);
                        lined.Operador = Convert.ToString(row.Cells[8].Value);
                        lined.NombreOper = Convert.ToString(row.Cells[9].Value);
                        if (!string.IsNullOrEmpty(row.Cells[10].Value.ToString()))
                            lined.NivelOper = Convert.ToString(row.Cells[10].Value);
                        if (!string.IsNullOrEmpty(row.Cells[11].Value.ToString()))
                            lined.LineaOper = Convert.ToString(row.Cells[11].Value);
                        if (!string.IsNullOrEmpty(row.Cells[12].Value.ToString()))
                            lined.Tipo = Convert.ToString(row.Cells[12].Value);
                        if (!string.IsNullOrEmpty(row.Cells[13].Value.ToString()))
                            lined.Tipona = Convert.ToString(row.Cells[13].Value);
                        if (!string.IsNullOrEmpty(row.Cells[14].Value.ToString()))
                        {
                            lined.SinNivel = Convert.ToString(row.Cells[14].Value);
                            if (lined.SinNivel == "1")
                                iContNivel++;
                        }
                            
                        lined.Usuario = GlobalVar.gsUsuario;
                        
                        LineUpDetLogica.Guardar(lined);
                    }
                    catch (Exception ie)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "LineUpDetLogica.Guardar(lined)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                line.Planta = _lsPlanta;
                line.Linea = cbbLinea.SelectedValue.ToString();
                line.RPO = txtRpo.Text.ToString();
                line.Modelo = txtModelo.Text.ToString();
                
                if (cbbCore.SelectedIndex != -1)
                    line.Core = cbbCore.SelectedValue.ToString();

                line.Fecha = dtpFecha.Value;
                line.Turno = _lsTurno;
                line.Duplicado = _lsDuplicado;

                if (chbControl.Checked)
                    line.Controla = "1";
                else
                    line.Controla = "0";
                if (cbbAutorizo.SelectedIndex != -1)
                    line.Autoriza = cbbAutorizo.SelectedValue.ToString();
                if (!string.IsNullOrEmpty(txtNota.Text))
                    line.ModeloCtrl = txtNota.Text.ToString();

                line.RevStd = _lsRevStd;
                line.ModRev = _lsModRev;

                //guardar indicador de bloq_pph para bloquear impresion.
                if (iContNivel > 0)
                    line.BloqPPH = "1";
                else
                    line.BloqPPH = "0";
                   
                line.Usuario = GlobalVar.gsUsuario;

                if (LineUpLogica.Guardar(line) == 0)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("Error al intentar guardar el LineUp", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region regMail
        private bool EnviarMail(string _asFolio, string _asTipo)
        {
            try
            {
                string sMsg = string.Empty;
                System.Data.DataTable dtMail = new System.Data.DataTable();
                CorreoLogica mail = new CorreoLogica();
                mail.Folio = int.Parse(_asFolio);
                mail.FolioRef = _asFolio;
                mail.Proceso = _lsProceso;
                if (_asTipo == "G") // Despues de Guardar
                    dtMail = CorreoLogica.ConsultarRef(mail);
                else // Solicitar alta de operador
                    dtMail = CorreoLogica.ConsultarFolio(mail);

                if (dtMail.Rows.Count > 0)
                {
                    for (int ic = 0; ic < dtMail.Rows.Count; ic++)
                    {
                        int iFolio = Convert.ToInt32(dtMail.Rows[ic]["folio"].ToString());
                        EnviarCorreo envMail = new EnviarCorreo();
                        envMail.FolioMail = iFolio;
                        envMail.Asunto = dtMail.Rows[ic]["asunto"].ToString();

                        if (_asTipo == "G") // Despues de Guardar
                        {
                            sMsg = EnviarCorreo.EnviaAlerta(envMail);

                            if (sMsg == "OK")
                            {
                                mail.Estado = "E";
                                sMsg = "Se ha Enviado una Notificación por Correo electrónico al Supervisor.";
                            }
                            else
                            {
                                sMsg = "No se logro Enviar la Notificación por Correo electrónico al Supervisor.";
                                mail.Estado = "R";
                            }
                        }
                        else
                            EnviarCorreo.EnviaAlertaAsync(envMail);
                        
                        mail.Folio = Convert.ToInt32(dtMail.Rows[ic]["folio"].ToString());
                        mail.Asunto = dtMail.Rows[ic]["asunto"].ToString();

                        CorreoLogica.Guardar(mail);
                    }

                    if(!string.IsNullOrEmpty(sMsg))
                        MessageBox.Show(sMsg, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return true;
        }

        private int MailAltaOperador(string _asOperador)
        {
            try
            {
                string sAsunto = "CLOVER PRO - Alerta en Captura de LineUp";
                string sHtml = "<html><head><body><h2 style='color: #2e6c80;font:Calibri'>SOLICITUD DE REGISTRO DE OPERADOR EN LINEA " + cbbLinea.SelectedValue.ToString() + "</h2>";
                sHtml += "<p><h3 style='color: #FF0000;font:Calibri'>FAVOR DE AGREGAR AL CATALOGO DE OPERADORES AL EMPLEADO " + _asOperador + "</h3></p>";
                sHtml += "<br></br>";
                sHtml += "<p><h4>Clover Technologies Group | IT Software | Captura LineUp | " + tssPlanta.Text + "</h5></p>";
                sHtml += "</body></head></html>";

                
                int iFolio = AccesoDatos.Consec("MAIL");
                CorreoLogica mail = new CorreoLogica();
                mail.Folio = iFolio;
                mail.Mensaje = sHtml;
                CorreoLogica.GuardarBody(mail);

                //destinatarios
                System.Data.DataTable dtDest = CorreoLogica.ConsultaDestAux(_lsPlanta, cbbLinea.SelectedValue.ToString(), _lsTurno);
                for (int iC = 0; iC < dtDest.Rows.Count; iC++)
                {
                    mail.Destino = dtDest.Rows[iC][0].ToString();
                    mail.Tipo = dtDest.Rows[iC][1].ToString();

                    CorreoLogica.GuardarDest(mail);
                }

                mail.Estado = "P";
                mail.Asunto = sAsunto;
                mail.FolioRef = "";
                mail.Proceso = _lsProceso;
                if (CorreoLogica.Guardar(mail) == 0)
                    return 0;
                
                return iFolio;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
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
                Close();

        }

        private void btnCtrl_Click(object sender, EventArgs e)
        {
            chbControl.Checked = true;

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
                    sDirec += @"\CloverPRO_ManualLineUpV1.pdf";
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

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "80") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable dtCon = ConfigLogica.Consultar();
                string sSolRegistro = dtCon.Rows[0]["lineup_regoper"].ToString();
                if (sSolRegistro == "0")
                {
                    int iRow = dgwEstaciones.CurrentCell.RowIndex;
                    string sOper = dgwEstaciones[8, iRow].Value.ToString();
                    
                    OperadorLogica oper = new OperadorLogica();
                    oper.Operador = sOper;
                    System.Data.DataTable dt = OperadorLogica.Consultar(oper);
                    if (dt.Rows.Count > 0)
                    {
                        string sNombre = dt.Rows[0]["nombre"].ToString();
                        string sNivel = dt.Rows[0]["nivel"].ToString();

                        dgwEstaciones[9, iRow].Value = sNombre;
                        dgwEstaciones[10, iRow].Value = sNivel;
                        dgwEstaciones[11, iRow].Value = dt.Rows[0]["linea"].ToString();

                        // CERTIFICADO PPH
                        string sNivelReq = Convert.ToString(dgwEstaciones[7, iRow].Value);
                        if (ValidaNivel(sNivel, sNivelReq))
                        {
                            dgwEstaciones[14, iRow].Value = "0";
                            dgwEstaciones[10, iRow].Style.BackColor = Color.WhiteSmoke;
                        }
                        else
                        {
                            dgwEstaciones[14, iRow].Value = "1";
                            dgwEstaciones[10, iRow].Style.BackColor = Color.DarkRed;
                        }

                        if (dt.Rows[0]["linea"].ToString() == cbbLinea.SelectedValue.ToString())
                            dgwEstaciones[11, iRow].Style.BackColor = Color.LightGreen;
                        else
                            dgwEstaciones[11, iRow].Style.BackColor = Color.Yellow;
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in dgwEstaciones.Rows)
                    {
                        if (row.Cells[8].Value == null || row.Cells[8].Value == "")
                            return;

                        bool bActualiza = false;
                        if (row.Cells[9].Value == null || row.Cells[9].Value == "")
                            bActualiza = true;
                        if (row.Cells[10].Value == null || row.Cells[10].Value == "")
                            bActualiza = true;
                        if (row.Cells[11].Value == null || row.Cells[11].Value == "")
                            bActualiza = true;

                        if (bActualiza)
                        {
                            string sOper = row.Cells[8].Value.ToString();
                            OperadorLogica oper = new OperadorLogica();
                            oper.Operador = sOper;
                            System.Data.DataTable dt = OperadorLogica.Consultar(oper);
                            if (dt.Rows.Count > 0)
                            {
                                string sNombre = dt.Rows[0]["nombre"].ToString();
                                dgwEstaciones[9, row.Index].Value = sNombre;
                                dgwEstaciones[10, row.Index].Value = dt.Rows[0]["nivel"].ToString();
                                dgwEstaciones[11, row.Index].Value = dt.Rows[0]["linea"].ToString();

                                dgwEstaciones[8, row.Index].Style.BackColor = Color.White;
                                dgwEstaciones[9, row.Index].Style.BackColor = Color.White;
                                dgwEstaciones[10, row.Index].Style.BackColor = Color.White;

                                if (dt.Rows[0]["linea"].ToString() == cbbLinea.SelectedValue.ToString())
                                    dgwEstaciones[11, row.Index].Style.BackColor = Color.LightGreen;
                                else
                                    dgwEstaciones[11, row.Index].Style.BackColor = Color.Yellow;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #endregion

        #region regEstacion
        private void CargarDetalle(int _aiFolio)
        {
            dgwEstaciones.DataSource = null;
            LineUpDetLogica lined = new LineUpDetLogica();
            lined.Folio = _aiFolio;
            System.Data.DataTable dt = LineUpDetLogica.Listar(lined);
            dgwEstaciones.DataSource = dt;

            foreach (DataGridViewRow row in dgwEstaciones.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
                string sTipo = row.Cells[12].Value.ToString();
                string sSinNivel = row.Cells[14].Value.ToString();
                if (sTipo == "G")
                    row.DefaultCellStyle.BackColor = Color.Red;
                if (sTipo == "M")
                    row.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                if (sTipo == "Y")
                    row.DefaultCellStyle.BackColor = Color.LightSalmon;
                if (sTipo == "R")
                    row.DefaultCellStyle.BackColor = Color.Khaki;
                if (sTipo == "O")
                    row.Cells[11].Style.BackColor = Color.Yellow;
                if (sTipo == "X")
                    row.DefaultCellStyle.BackColor = Color.Green;
                if (sTipo == "X" || sTipo == "O")
                { 
                    if (row.Cells[11].Value.ToString() == cbbLinea.SelectedValue.ToString())
                        row.Cells[11].Style.BackColor = Color.LightGreen;
                    else
                        row.Cells[11].Style.BackColor = Color.Yellow;
                }
                    
                if(sSinNivel == "1")
                    row.Cells[10].Style.BackColor = Color.DarkRed;
            }

            CargarColumnasNew();

            dgwEstaciones.CurrentCell = dgwEstaciones[8, 0];
            dgwEstaciones.Focus();

        }
        private void dgwEstaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            //if ((iRow % 2) == 0)
            //    e.CellStyle.BackColor = Color.LightSkyBlue;
            //else
            //    e.CellStyle.BackColor = Color.White;

            if(e.Value != null)
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
        private void CargarColOpe()
        {
            int iRows = dgwOper.Rows.Count;
            if (iRows == 0)
            {
                System.Data.DataTable dtNew = new System.Data.DataTable("Oper");
                dtNew.Columns.Add("codigo", typeof(int));
                dtNew.Columns.Add("IMAGEN", typeof(Image));
                dtNew.Columns.Add("DATOS", typeof(string));
                dgwOper.DataSource = dtNew;
            }

            dgwOper.Columns[0].Visible = false;

            dgwOper.Columns[1].Width = ColumnWith(dgwEstaciones, 40);//ESTACION
            dgwOper.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwOper.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwOper.Columns[2].Width = ColumnWith(dgwEstaciones, 60);//ESTACION
            dgwOper.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwOper.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private void CargarColumnasNew()
        {
            int iRows = dgwEstaciones.Rows.Count;
            if (iRows == 0)
            {
                System.Data.DataTable dtNew = new System.Data.DataTable("Estacion");
                dtNew.Columns.Add("folio", typeof(int));
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("linea", typeof(string));
                dtNew.Columns.Add("modelo", typeof(string));
                dtNew.Columns.Add("cve_estacion", typeof(int));
                dtNew.Columns.Add("#", typeof(string));
                dtNew.Columns.Add("OPERACION", typeof(string));
                dtNew.Columns.Add("NIVEL REQ", typeof(string));
                dtNew.Columns.Add("# OPER", typeof(string));
                dtNew.Columns.Add("NOMBRE DEL EMPLEADO", typeof(string));
                dtNew.Columns.Add("NIVEL", typeof(string));
                dtNew.Columns.Add("LINEA", typeof(string));
                dtNew.Columns.Add("tipo_op", typeof(string));
                dtNew.Columns.Add("tipo_na", typeof(string));
                dtNew.Columns.Add("sin_nivel", typeof(string));
                dtNew.Columns.Add("u_id", typeof(string));
                dgwEstaciones.DataSource = dtNew;
            }

            dgwEstaciones.Columns[0].Visible = false;
            dgwEstaciones.Columns[1].Visible = false;
            dgwEstaciones.Columns[2].Visible = false;
            dgwEstaciones.Columns[3].Visible = false;
            dgwEstaciones.Columns[4].Visible = false;

            //dgwEstaciones.Columns[9].Visible = false;
            dgwEstaciones.Columns[10].Visible = false;
            dgwEstaciones.Columns[11].Visible = false;

            dgwEstaciones.Columns[12].Visible = false;
            dgwEstaciones.Columns[13].Visible = false;
            dgwEstaciones.Columns[14].Visible = false;
            dgwEstaciones.Columns[15].Visible = false;

            dgwEstaciones.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwEstaciones.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            dgwEstaciones.Columns[5].Width = ColumnWith(dgwEstaciones, 10);//ESTACION
            dgwEstaciones.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[5].ReadOnly = true;
            dgwEstaciones.Columns[6].Width = ColumnWith(dgwEstaciones, 65);
            dgwEstaciones.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[6].ReadOnly = true;

            dgwEstaciones.Columns[7].Width = ColumnWith(dgwEstaciones, 10);//NIVEL REQ
            dgwEstaciones.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[7].ReadOnly = true;

            dgwEstaciones.Columns[8].Width = ColumnWith(dgwEstaciones, 15);//NO EMPL
            dgwEstaciones.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[8].ReadOnly = false;

            dgwEstaciones.Columns[9].Width = ColumnWith(dgwEstaciones, 50);//NOMBRE
            dgwEstaciones.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[9].ReadOnly = true;
        }
       
        private void CargarColumnas()
        {
            int iRows = dgwEstaciones.Rows.Count;
            if (iRows == 0)
            {
                System.Data.DataTable dtNew = new System.Data.DataTable("Estacion");
                dtNew.Columns.Add("folio", typeof(int));
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("linea", typeof(string));
                dtNew.Columns.Add("modelo", typeof(string));
                dtNew.Columns.Add("cve_estacion", typeof(int));
                dtNew.Columns.Add("#", typeof(string));
                dtNew.Columns.Add("OPERACION", typeof(string));
                dtNew.Columns.Add("NIVEL REQ", typeof(string));
                dtNew.Columns.Add("NO. EMPLEADO", typeof(string));
                dtNew.Columns.Add("NOMBRE DEL EMPLEADO", typeof(string));
                dtNew.Columns.Add("NIVEL", typeof(string));
                dtNew.Columns.Add("LINEA", typeof(string));
                dtNew.Columns.Add("tipo_op", typeof(string));
                dtNew.Columns.Add("tipo_na", typeof(string));
                dtNew.Columns.Add("sin_nivel", typeof(string));
                dtNew.Columns.Add("u_id", typeof(string));
                dgwEstaciones.DataSource = dtNew;
            }
            
            dgwEstaciones.Columns[0].Visible = false;
            dgwEstaciones.Columns[1].Visible = false;
            dgwEstaciones.Columns[2].Visible = false;
            dgwEstaciones.Columns[3].Visible = false;
            dgwEstaciones.Columns[4].Visible = false;
            
            dgwEstaciones.Columns[12].Visible = false;
            dgwEstaciones.Columns[13].Visible = false;
            dgwEstaciones.Columns[14].Visible = false;
            dgwEstaciones.Columns[15].Visible = false;

            dgwEstaciones.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwEstaciones.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            dgwEstaciones.Columns[5].Width = ColumnWith(dgwEstaciones, 5);//ESTACION
            dgwEstaciones.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[5].ReadOnly = true;
            dgwEstaciones.Columns[6].Width = ColumnWith(dgwEstaciones, 33);
            dgwEstaciones.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[6].ReadOnly = true;

            dgwEstaciones.Columns[7].Width = ColumnWith(dgwEstaciones, 6);//NIVEL REQ
            dgwEstaciones.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[7].ReadOnly = true;

            dgwEstaciones.Columns[8].Width = ColumnWith(dgwEstaciones, 10);//NO EMPL
            dgwEstaciones.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[9].Width = ColumnWith(dgwEstaciones, 30);//NOMBRE
            dgwEstaciones.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[9].ReadOnly = true;
            dgwEstaciones.Columns[10].Width = ColumnWith(dgwEstaciones, 6);//NIVEL
            dgwEstaciones.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[10].ReadOnly = true;
            dgwEstaciones.Columns[11].Width = ColumnWith(dgwEstaciones, 10);//LINEA
            dgwEstaciones.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[11].ReadOnly = true;
        }
        private void dgwEstaciones_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            string sLinea = dgwEstaciones.Rows[e.RowIndex].Cells[11].Value.ToString().ToUpper();
            string sTipo = dgwEstaciones.Rows[e.RowIndex].Cells[12].Value.ToString();
            if(sTipo == "G")
                dgwEstaciones.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
            if (sTipo == "M")
                dgwEstaciones.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightSkyBlue;
            if (sTipo == "Y") //MYLAR
                dgwEstaciones.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightSalmon;
            if (sTipo == "R")
                dgwEstaciones.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Khaki;
            if (sTipo == "X") //EXTRA
                dgwEstaciones.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;
            if (sTipo == "O" || sTipo == "X") //OPERACION
                dgwEstaciones.Rows[e.RowIndex].Cells[11].Style.BackColor = Color.White;
            if (sTipo == "T") //ROBOT
                dgwEstaciones.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
        }
        //ESCANER SOBRE #OPERADOR && LINEA OP
        private bool ValidaNivel(string _asNivel,string _asNivelReq)
        {
            if (_asNivel == "GUIA")
                return true;

            if (!string.IsNullOrEmpty(_asNivelReq))
            {
                int iNivel = 0;
                int iNivelR = 0;
                if (_asNivel == "II")
                    iNivel = 2;
                if (_asNivel == "III" || _asNivel == "IIIQ")
                    iNivel = 3;
                if (_asNivel == "IV" || _asNivel == "IVQ")
                    iNivel = 4;
                if (_asNivel == "V" || _asNivel == "VQ")
                    iNivel = 5;

                if (_asNivelReq == "II")
                    iNivelR = 2;
                if (_asNivelReq == "III" | _asNivelReq == "IIIQ")
                    iNivelR = 3;
                if (_asNivelReq == "IV" || _asNivelReq == "IVQ")
                    iNivelR = 4;
                if (_asNivelReq == "V" || _asNivelReq == "VQ")
                    iNivelR = 5;

                if (iNivel < iNivelR)
                    return false;
            }
            return true;
        }
        private string TressNivel(string _asNivel)
        {
            string sValue = _asNivel;

            if (_asNivel.IndexOf("GUIA") != -1)
                return "GUIA";

            if (_asNivel.IndexOf("OPERADOR") != -1)
                return "OPG";

            if (_asNivel.Contains("PPH"))
            {
                //_asNivel = _asNivel.Substring(10);

                int iPos = _asNivel.IndexOf(" ");
                //if (iPos > 0)
                //    _asNivel = _asNivel.Substring(0, iPos);
                
                while (iPos > 0)
                {
                    _asNivel = _asNivel.Substring(iPos + 1);
                    iPos = _asNivel.IndexOf(" ");
                    if (_asNivel.IndexOf("NIV") != -1)
                    {
                        iPos++;
                        int iLen = _asNivel.Length - iPos;
                        if (iLen > 3)
                            iLen = 3;

                        if (_asNivel.IndexOf("4") != -1)//FUSORES
                            sValue = "IV";
                        else
                        {
                            if (_asNivel.IndexOf("3") != -1)//COATING
                                sValue = "III";
                            else
                            {
                                if (_asNivel.IndexOf("2") != -1)//COATING
                                    sValue = "II";
                                else
                                    sValue = _asNivel.Substring(iPos, iLen);
                            }
                        }
                        iPos = 0;
                    }
                }
                
                //PPH NIVEL IV Q PLANTA COLOR
                return sValue.Trim();
            }

            
            return sValue;
        }
        private string TressLinea(string _asPta,string _asLinea)
        {
            string sValue = _asLinea;

            if (sValue.IndexOf("TNR")!=-1)
                sValue = sValue.Replace("TNR", "");

            return sValue;
        }

        private void dgwEstaciones_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            bool bExiste = false;
            if (e.ColumnIndex == 8)
            {
                if (!string.IsNullOrEmpty(txtFolio.Text) && !string.IsNullOrWhiteSpace(txtFolio.Text))
                {
                    if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == false)
                    {
                        MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                
                _lbCambioDet = true;

                try
                {
                    string sOper = dgwEstaciones[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
                    if (!string.IsNullOrEmpty(sOper) && !string.IsNullOrWhiteSpace(sOper))
                    {
                        if(sOper == "N/A")
                            return;

                        if (sOper.IndexOf("A") > 0)
                            sOper = sOper.Substring(0, 6);

                        int iOper = 0;
                        if (!int.TryParse(sOper, out iOper))
                        {
                            MessageBox.Show("El Número de Empleado no se encuentra Registrado.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tssF7_Click(sender, e);
                            return;
                        }

                        if (sOper.Length < 6)
                            sOper = sOper.PadLeft(6, '0');

                        dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = sOper;

                        /*OperadorLogica oper = new OperadorLogica();
                        oper.Operador = sOper;
                        System.Data.DataTable dt = OperadorLogica.Consultar(oper);*/
                        TressLogica tOper = new TressLogica();
                        tOper.Codigo = int.Parse(sOper);
                        System.Data.DataTable dt = TressLogica.OperadorLineUp(tOper);
                        int iCont = dt.Rows.Count;
                        if (iCont > 0)
                        {
                            bExiste = true;
                            string sNombre = dt.Rows[0]["NOMBRE"].ToString();
                            string sNivel = TressNivel(dt.Rows[0]["NIVEL"].ToString());
                            string sLinea = dt.Rows[0]["LINEA"].ToString();
                            string sPta = dt.Rows[0]["PLANTA"].ToString();
                            sLinea = TressLinea(sPta,sLinea);

                            dgwEstaciones[9, e.RowIndex].Value = sNombre;
                            dgwEstaciones[10, e.RowIndex].Value = sNivel;
                            dgwEstaciones[11, e.RowIndex].Value = sLinea;

                            CargarTress(sOper);

                            /*
                            string sNombre = dt.Rows[0]["nombre"].ToString();
                            string sNivel = dt.Rows[0]["nivel"].ToString();
                            dgwEstaciones[9, e.RowIndex].Value = sNombre;
                            dgwEstaciones[10, e.RowIndex].Value = sNivel;
                            dgwEstaciones[11, e.RowIndex].Value = dt.Rows[0]["linea"].ToString();

                            */

                            //valida nivel del modelo vs nivel de operador
                            string sNivelReq = Convert.ToString(dgwEstaciones[7, e.RowIndex].Value);
                            if(ValidaNivel(sNivel,sNivelReq))
                                dgwEstaciones[14, e.RowIndex].Value = "0"; // CERTIFICADO PPH
                            else
                            {
                                dgwEstaciones[14, e.RowIndex].Value = "1";
                                MessageBox.Show("ALERTA DEL SISTEMA" + Environment.NewLine + "El Empleado no Cuenta con la Certificación Requerida para la Operación", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            
                            iCont = 0;
                            for(int i = 0; i < dgwEstaciones.RowCount; i ++)
                            {
                                if (dgwEstaciones[12, i].Value.ToString() == "G")
                                    continue;

                                string sOpRep = dgwEstaciones[9, i].Value.ToString().ToUpper();
                                if (sOper == sOpRep)
                                    iCont++;
                            }

                            if(iCont > 1)
                                dgwEstaciones[13, e.RowIndex].Value = "A";//AUSENCIA
                        }
                        else
                        {
                            //*BUSCAR EN ORBIS*\\                                
                            //System.Data.DataTable dtOrbis = OperadorLogica.ConsultarOrbis(oper);
                            //if (dtOrbis.Rows.Count != 0)
                            //{
                            //    bExiste = true;
                            //    string sNombre = dtOrbis.Rows[0]["nombre"].ToString();
                            //    string sApellido = dtOrbis.Rows[0]["apellidos"].ToString();
                                    
                            //    _lbCambio = true;

                            //    CargarTress(sOper);
                            //}
                        }
                        string sCod = dgwEstaciones[8, e.RowIndex].Value.ToString();
                        string sVal = dgwEstaciones[9, e.RowIndex].Value.ToString();
                        if (!bExiste && !string.IsNullOrEmpty(sCod) && string.IsNullOrEmpty(sVal))
                        {
                            System.Data.DataTable dtCon = ConfigLogica.Consultar();
                            string sSolRegistro = dtCon.Rows[0]["lineup_regoper"].ToString();
                            if(sSolRegistro == "0")
                            {
                                MessageBox.Show("El Número de Empleado no se encuentra Registrado.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                bExiste = true;
                                tssF7_Click(sender, e);
                                return;
                            }
                            else
                            {
                                DialogResult Result = MessageBox.Show("El Número de Empleado no se encuentra Registrado. Desea Solicitar el registro en el sistema?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                                if (Result == DialogResult.Yes)
                                {
                                    //mail a auxiliares, sup, sistemas
                                    int iFolio = MailAltaOperador(sOper);
                                    if (iFolio > 0)
                                        EnviarMail(iFolio.ToString(), "R");

                                    dgwEstaciones[9, e.RowIndex].Style.BackColor = Color.Yellow;
                                    dgwEstaciones[10, e.RowIndex].Style.BackColor = Color.Yellow;
                                    dgwEstaciones[11, e.RowIndex].Style.BackColor = Color.Yellow;
                                    dgwEstaciones[12, e.RowIndex].Style.BackColor = Color.Yellow;
                                    dgwEstaciones[14, e.RowIndex].Style.BackColor = Color.Yellow;


                                    bExiste = true;
                                }
                                else
                                {
                                    bExiste = true;
                                    dgwEstaciones[9, e.RowIndex].Value = string.Empty;
                                    dgwEstaciones[10, e.RowIndex].Value = string.Empty;
                                    dgwEstaciones[11, e.RowIndex].Value = string.Empty;
                                    dgwEstaciones[13, e.RowIndex].Value = string.Empty;
                                    dgwEstaciones[14, e.RowIndex].Value = string.Empty;
                                }
                            }
                            
                        }
                    }
                    else
                    {
                        tssF7_Click(sender, e);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "dgwEstaciones_CellValueChanged(6)" + Environment.NewLine + ex.ToString(), "Error en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
            }
        }
        
        private void dgwEstaciones_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (dgwEstaciones.Rows.Count > 0)
                {
                    MessageBox.Show("Favor de Limpiar pantalla antes de Cambiar de Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                wfPlantaPop popUp = new wfPlantaPop(_lsProceso);
                popUp.ShowDialog();
                string sPlanta = popUp._sClave;
                if (string.IsNullOrEmpty(sPlanta))
                    return;

                LineaLogica line = new LineaLogica();
                line.Planta = sPlanta;
                cbbLinea.DataSource = LineaLogica.LineaPlanta(line);
                cbbLinea.ValueMember = "linea";
                cbbLinea.DisplayMember = "linea";
                cbbLinea.SelectedIndex = -1;
            }

            try
            {
                int iRow = dgwEstaciones.CurrentCell.RowIndex;
                if (e.KeyCode == Keys.F3)
                {
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
                    if (cbbLinea.SelectedIndex != -1)
                        sLinea = cbbLinea.SelectedValue.ToString();
                    AgregaEstacion(sLinea, txtModelo.Text.ToString(), iClave, sEstacion, sOperacion, "Z", null,null,null);

                    dgwEstaciones.Rows[dgwEstaciones.Rows.Count - 1].Cells[8].Selected = true;
                }

                if (e.KeyCode == Keys.F4)
                {
                    if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "30") == false)
                    {
                        MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    string sTipo = dgwEstaciones[12, iRow].Value.ToString();
                    if (sTipo == "X" || sTipo == "Z" || sTipo == "G")
                        return;

                    dgwEstaciones[8, iRow].Value = "N/A";
                    dgwEstaciones[9, iRow].Value = "NO APLICA MODELO";
                    dgwEstaciones[10, iRow].Value = string.Empty;
                    dgwEstaciones[11, iRow].Value = string.Empty;
                    dgwEstaciones[13, iRow].Value = "M";

                    iRow++;
                    if (dgwEstaciones.RowCount > iRow)
                        dgwEstaciones.Rows[iRow].Cells[8].Selected = true;
                }

                if (e.KeyCode == Keys.F5) //DUPLICAR ESTACION DE OPERADOR
                {
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
                        if (cbbLinea.SelectedIndex != -1)
                            sLinea = cbbLinea.SelectedValue.ToString();
                        AgregaEstacion(sLinea, txtModelo.Text.ToString(), iClave, sEstacion, sOperacion + " (OP. EXTRA)", "X", null,sNivel,null);

                        dgwEstaciones.Rows[dgwEstaciones.Rows.Count - 1].Cells[8].Selected = true;
                    }
                }

                if (e.KeyCode == Keys.F6) //ELIMINAR ESTACION DUPLICADA
                {
                    if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "50") == false)
                    {
                        MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    string sTipo = dgwEstaciones[12, iRow].Value.ToString();
                    if (sTipo == "X" || sTipo == "Z")
                    {
                        //mandar al listado para borrar de bd al guardar cambios
                        int iFolio = 0;
                        int iCons = 0;
                        if(Int32.TryParse(dgwEstaciones[0, iRow].Value.ToString(),out iFolio))
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
                if(e.KeyCode == Keys.F7)
                {
                    if (dgwEstaciones.CurrentRow.Index == -1)
                        return;

                    dgwEstaciones[8, iRow].Value = string.Empty;
                    dgwEstaciones[9, iRow].Value = string.Empty;
                    dgwEstaciones[10, iRow].Value = string.Empty;
                    dgwEstaciones[11, iRow].Value = string.Empty;
                    dgwEstaciones[13, iRow].Value = string.Empty;
                }

                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    int iColumn = dgwEstaciones.CurrentCell.ColumnIndex;

                    if (iRow < dgwEstaciones.Rows.Count - 1)
                        dgwEstaciones.CurrentCell = dgwEstaciones[8, iRow + 1];
                    else
                        dgwEstaciones.CurrentCell = dgwEstaciones[8, 0];
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void dgwEstaciones_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string sLinea = dgwEstaciones[11, e.RowIndex].Value.ToString().ToUpper();
            string sTipo = dgwEstaciones[12, e.RowIndex].Value.ToString();
            string sNivel = dgwEstaciones[14, e.RowIndex].Value.ToString();
            if (sTipo == "O" || sTipo == "X" || sTipo == "Z")
            {
                if (sLinea == cbbLinea.SelectedValue.ToString())
                    dgwEstaciones[11, e.RowIndex].Style.BackColor = Color.LightGreen;
                else
                    dgwEstaciones[11, e.RowIndex].Style.BackColor = Color.Yellow;
            }

            if(sNivel == "1")
                dgwEstaciones[10, e.RowIndex].Style.BackColor = Color.DarkRed;
            else
            {
                if (sTipo == "G")
                    dgwEstaciones[10, e.RowIndex].Style.BackColor = Color.Red;
                if (sTipo == "M")
                    dgwEstaciones[10, e.RowIndex].Style.BackColor = Color.LightSkyBlue;
                if (sTipo == "Y") //MYLAR
                    dgwEstaciones[10, e.RowIndex].Style.BackColor = Color.LightSalmon;
                if (sTipo == "R")
                    dgwEstaciones[10, e.RowIndex].Style.BackColor = Color.Khaki;
                if (sTipo == "X") //EXTRA
                    dgwEstaciones[10, e.RowIndex].Style.BackColor = Color.Green;
                if (sTipo == "O" || sTipo == "X") //OPERACION
                    dgwEstaciones[10, e.RowIndex].Style.BackColor = Color.White;
                if (sTipo == "T") //ROBOT
                    dgwEstaciones[10, e.RowIndex].Style.BackColor = Color.LightGray;
            }
                
        }

        private void dgwEstaciones_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        #endregion

        #region regCambio
        private void txtModelo_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtRpo_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_lsRpoAnt) && _lsRpoAnt != txtRpo.Text)
                _lbCambio = true;
            _lsRpoAnt = txtRpo.Text.ToString();
        }

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

        private void cbbLinea_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbLinea, 0);
        }

        private void cbbLinea_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbLinea, 1);
        }
        private void txtModelo_Enter(object sender, EventArgs e)
        {
            if(_lbReqLine && cbbLinea.SelectedIndex == -1)
            {
                cbbLinea.Focus();
                return;
            }

            if(string.IsNullOrEmpty(txtRpo.Text))
            {
                txtRpo.Focus();
                return;
            }

            GlobalVar.CambiaColor(txtModelo, 1);
        }

        private void txtModelo_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtModelo, 0);
        }
        
        private void txtRpo_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRpo, 0);
        }

        private void txtRpo_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRpo, 1);
        }

        private void cbbAutorizo_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbAutorizo, 0);
        }

        private void cbbAutorizo_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbAutorizo, 1);
        }
        private void txtNota_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtNota, 0);
        }

        private void txtNota_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtNota, 1);
        }
        #endregion

        #region regCaptura
        private void txtFolio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtFolio.Text) || string.IsNullOrWhiteSpace(txtFolio.Text))
                return;

            try
            {
                LineUpLogica line = new LineUpLogica();
                line.Folio = Convert.ToInt32(txtFolio.Text.ToString());
                System.Data.DataTable data = LineUpLogica.Consultar(line);
                if(data.Rows.Count != 0)
                {
                    _lsPlanta = data.Rows[0]["planta"].ToString();
                    cbbLinea.SelectedValue = data.Rows[0]["linea"].ToString();
                    txtModelo.Text = data.Rows[0]["modelo"].ToString();
                    if (!string.IsNullOrEmpty(data.Rows[0]["rpo"].ToString()))
                        txtRpo.Text = data.Rows[0]["rpo"].ToString();
                    dtpFecha.Value = Convert.ToDateTime(data.Rows[0]["fecha"].ToString());
                    if (data.Rows[0]["ind_controlada"].ToString() == "1")
                        chbControl.Checked = true;
                    if (!string.IsNullOrEmpty(data.Rows[0]["id_autorizo"].ToString()))
                        cbbAutorizo.SelectedValue = data.Rows[0]["id_autorizo"].ToString();
                    if (!string.IsNullOrEmpty(data.Rows[0]["modelo_ctrl"].ToString()))
                        txtNota.Text = data.Rows[0]["modelo_ctrl"].ToString();
                    _lsDuplicado = data.Rows[0]["ind_duplicado"].ToString();
                    if (!string.IsNullOrEmpty(data.Rows[0]["core"].ToString()))
                        cbbCore.SelectedValue = data.Rows[0]["core"].ToString();
                    _lsRevStd = data.Rows[0]["ind_revstd"].ToString();
                    _lsModRev = data.Rows[0]["modelo_rev"].ToString();

                    //..AYUDA VISUAL..\\
                    PlantaLogica pl = new PlantaLogica();
                    pl.Planta = _lsPlanta;
                    System.Data.DataTable dtPl = PlantaLogica.Consultar(pl);
                    tssPlanta.Text = dtPl.Rows[0]["nombre"].ToString().ToUpper();
                    lblLineaHelp.Text = "L - " + data.Rows[0]["linea"].ToString();
                    tssLinea.Text = "LINEA " + data.Rows[0]["linea"].ToString();
                    //..\\

                    txtModelo.Enabled = false;
                    cbbLinea.Enabled = false;
                     
                    _lsFolioAnt = txtFolio.Text.ToString();

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
            else
            {
                txtModelo.Focus();
            }
            lblLineaHelp.Text = "L - " + cbbLinea.SelectedValue.ToString();
            tssLinea.Text = "LINEA " + cbbLinea.SelectedValue.ToString();
        }
        private void txtModelo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (cbbLinea.SelectedIndex == -1)
            {
                MessageBox.Show("No se ha especificado la Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(txtModelo.Text) || string.IsNullOrWhiteSpace(txtModelo.Text))
                return;

            if (cbbLinea.SelectedIndex == -1)
            {
                MessageBox.Show("Debe Indicar la Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbLinea.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtRpo.Text) && !string.IsNullOrWhiteSpace(txtRpo.Text))
            {
                try
                {
                    if (!ConfigLogica.VerificaOmiteValidaLup())
                    {
                        LineUpLogica line = new LineUpLogica();
                        line.Linea = cbbLinea.SelectedValue.ToString();
                        line.Modelo = txtModelo.Text.ToString().ToUpper();
                        line.RPO = txtRpo.Text.ToString();
                        line.Fecha = dtpFecha.Value;
                        line.Turno = _lsTurno;
                        
                        if (LineUpLogica.VerificaRPO(line))
                        {
                            MessageBox.Show("El RPO ya se encuentra cargado en un registro anterior. Favor de proporcionar el RPO correcto", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtRpo.Clear();
                            txtRpo.Focus();
                            return;
                        }

                        //if (LineUpLogica.VerificaRPO(line))
                        //{
                        //    //Ya se encuentra registrado, es un Nuevo RPO?
                        //    DialogResult Result = MessageBox.Show("Las Estaciones de la Linea ya han sido registradas. Desea registrar las Estaciones para un nuevo RPO?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        //    if (Result == DialogResult.Yes)
                        //        txtRpo.Focus();
                        //    else
                        //    {
                        //        txtModelo.Clear();
                        //        txtModelo.Focus();
                        //        return;
                        //    }
                        //}
                    }

                    CargarModelo(txtModelo.Text.ToString().ToUpper().Trim());
                    if (dgwEstaciones.RowCount > 0)
                    {
                        cbbLinea.Enabled = false;
                        txtRpo.Enabled = false;
                        txtModelo.Enabled = false;
                        cbbCore.Enabled = false;

                        if (string.IsNullOrEmpty(txtRpo.Text))
                            txtRpo.Focus();
                        else
                            dgwEstaciones.Focus();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }  
        }
        private void tstFolio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(tstFolio.Text) || string.IsNullOrWhiteSpace(tstFolio.Text))
                return;

            try
            {
                LineUpLogica line = new LineUpLogica();
                line.Folio = Convert.ToInt32(tstFolio.Text.ToString());
                System.Data.DataTable data = LineUpLogica.Consultar(line);
                if (data.Rows.Count != 0)
                {
                    txtFolio.Text = data.Rows[0]["folio"].ToString();
                    _lsPlanta = data.Rows[0]["planta"].ToString();
                    cbbLinea.SelectedValue = data.Rows[0]["linea"].ToString();
                    lblLinea.Text = data.Rows[0]["linea"].ToString();
                    txtModelo.Text = data.Rows[0]["modelo"].ToString();
                    if (!string.IsNullOrEmpty(data.Rows[0]["rpo"].ToString()))
                        txtRpo.Text = data.Rows[0]["rpo"].ToString();
                    dtpFecha.Value = Convert.ToDateTime(data.Rows[0]["fecha"].ToString());
                    if (data.Rows[0]["ind_controlada"].ToString() == "1")
                        chbControl.Checked = true;
                    if (!string.IsNullOrEmpty(data.Rows[0]["id_autorizo"].ToString()))
                        cbbAutorizo.SelectedValue = data.Rows[0]["id_autorizo"].ToString();
                    if (!string.IsNullOrEmpty(data.Rows[0]["modelo_ctrl"].ToString()))
                        txtNota.Text = data.Rows[0]["modelo_ctrl"].ToString();
                    _lsDuplicado = data.Rows[0]["ind_duplicado"].ToString();
                    if (!string.IsNullOrEmpty(data.Rows[0]["core"].ToString()))
                        cbbCore.SelectedValue = data.Rows[0]["core"].ToString();
                    _lsRevStd = data.Rows[0]["ind_revstd"].ToString();
                    _lsModRev = data.Rows[0]["modelo_rev"].ToString();

                    //..AYUDA VISUAL..\\
                    PlantaLogica pl = new PlantaLogica();
                    pl.Planta = _lsPlanta;
                    System.Data.DataTable dtPl = PlantaLogica.Consultar(pl);
                    tssPlanta.Text = dtPl.Rows[0]["nombre"].ToString().ToUpper();
                    lblLineaHelp.Text = "L - " + data.Rows[0]["linea"].ToString();
                    tssLinea.Text = "LINEA " + data.Rows[0]["linea"].ToString();
                    //..\\

                    txtRpo.Enabled = false;
                    txtModelo.Enabled = false;
                    cbbCore.Enabled = false;
                    cbbLinea.Enabled = false;
                    chbControl.Enabled = false;
                    txtNota.Enabled = false;

                    _lsFolioAnt = tstFolio.Text.ToString();

                    CargarDetalle(Convert.ToInt32(tstFolio.Text));
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
       
        private void txtRpo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtRpo.Text))
                return;

            if(cbbLinea.SelectedIndex == -1)
            {
                MessageBox.Show("No se ha especificado la Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnF1.Focus();
                return;
            }

            string sRPO = txtRpo.Text.ToString().Trim();
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
                        txtRpo.Clear();
                        txtRpo.Focus();
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

            txtRpo.Text = sRPO;
            
            try
            {
                if (!ConfigLogica.VerificaOmiteValidaLup())
                {
                    LineUpLogica line = new LineUpLogica();
                    line.Linea = cbbLinea.SelectedValue.ToString();
                    line.Fecha = dtpFecha.Value;
                    line.RPO = sRPO;
                    line.Turno = _lsTurno;

                    if (LineUpLogica.VerificaRPO(line))
                    {
                        MessageBox.Show("El RPO ya se encuentra cargado en un registro anterior. Favor de proporcionar el RPO correcto", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtRpo.Clear();
                        txtRpo.Focus();
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

                if (ConfigLogica.VerificaRpoOrbis())
                {
                    RpoLogica rpo = new RpoLogica();
                    rpo.RPO = sRPO;
                    System.Data.DataTable dtRpo = RpoLogica.ConsultarOrbis(rpo);
                    if (dtRpo.Rows.Count != 0)
                    {
                        string sModelo = dtRpo.Rows[0]["Producto"].ToString();
                        string sEstd = dtRpo.Rows[0]["Estandar"].ToString();
                        string sLinea = dtRpo.Rows[0]["Linea"].ToString();
                        if (sModelo.IndexOf("CONV") != -1)
                        {
                            sModelo = sModelo.TrimEnd();
                            sModelo = sModelo.Substring(0, sModelo.IndexOf("CONV"));
                            sModelo = sModelo.TrimEnd();
                        }

                        cbbCore.SelectedValue = sEstd;
                        txtModelo.Text = sModelo;
                        txtModelo_KeyDown(sender, e);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            

            txtModelo.Focus();
        }
        private void CargarModelo(string _asModelo)
        {
            try
            {
                dgwEstaciones.DataSource = null;
                CargarColumnasNew();

                System.Data.DataTable datos = new System.Data.DataTable();

                bool bExiste = false;
                string sBucaMod = ConfigLogica.VerificaBuscaModelo();
                
                if (sBucaMod == "R" || sBucaMod == "A")
                {
                    //MODELOS RELACIONADOS
                    ModerelaLogica modre = new ModerelaLogica();
                    modre.Moderela = _asModelo;
                    System.Data.DataTable dtRel = ModerelaLogica.ConsultaRelacionado(modre);
                    if (dtRel.Rows.Count != 0)
                    {
                        if (dtRel.Rows.Count > 0)
                        {
                            wfCapturaPopGrid wfMods = new wfCapturaPopGrid(_asModelo);
                            wfMods.ShowDialog();
                            _asModelo = wfMods._sClave;
                            _lsRevStd = wfMods._sIndStd;
                            _lsModRev = wfMods._sClave;
                        }
                        else
                        {
                            _asModelo = dtRel.Rows[0][1].ToString(); //LAYOUT
                            _lsRevStd = dtRel.Rows[0][3].ToString(); //IND. REVISION ESTANDAR
                            _lsModRev = _asModelo;
                        }
                                

                        ModeloLogica mode = new ModeloLogica();
                        mode.Modelo = _asModelo;
                        datos = ModeloLogica.Consultar(mode);
                        if (datos.Rows.Count != 0)
                            bExiste = true;
                    }
                }

                if (sBucaMod == "L" || sBucaMod == "A")
                {
                    if (!bExiste)
                    {
                        ModeloLogica mode = new ModeloLogica();
                        mode.Modelo = _asModelo;
                        datos = ModeloLogica.Consultar(mode);
                        if (datos.Rows.Count != 0)
                            bExiste = true;
                    }
                }

                if (bExiste)
                {
                    string sLinea = string.Empty;
                    if (cbbLinea.SelectedIndex != -1)
                        sLinea = cbbLinea.SelectedValue.ToString();

                    int iCant = 0;
                    if (!string.IsNullOrEmpty(datos.Rows[0]["tipo"].ToString()))
                        cbbCore.SelectedValue = datos.Rows[0]["tipo"].ToString();

                    if (cbbCore.SelectedIndex != -1)
                        lblCoreMod.Text = cbbCore.SelectedValue.ToString();

                    if (datos.Rows[0]["cant_guia"].ToString() != "0")
                    {
                        iCant = Convert.ToInt32(datos.Rows[0]["cant_guia"].ToString());
                        for (int i = 0; i < iCant; i++)
                        {
                            AgregaEstacion(sLinea, txtModelo.Text.ToString(), 0, null, "GUIA DE LINEA","G",null,"IV",null);
                        }
                    }

                    if (datos.Rows[0]["cant_multi"].ToString() != "0")
                    {
                        iCant = Convert.ToInt32(datos.Rows[0]["cant_multi"].ToString());
                        for (int i = 0; i < iCant; i++)
                        {
                            AgregaEstacion(sLinea, txtModelo.Text.ToString(), 0, null, "MULTIFUNCIONAL", "M", null, "III", null);
                        }
                    }

                    if (datos.Rows[0]["cant_retra"].ToString() != "0")
                    {
                        iCant = Convert.ToInt32(datos.Rows[0]["cant_retra"].ToString());
                        for (int i = 0; i < iCant; i++)
                        {
                            AgregaEstacion(sLinea, txtModelo.Text.ToString(), 0, null, "RETRABAJADOR","R",null,"III",null);
                        }
                    }

                    if (datos.Rows[0]["cant_mylar"].ToString() != "0")
                    {
                        iCant = Convert.ToInt32(datos.Rows[0]["cant_mylar"].ToString());
                        for (int i = 0; i < iCant; i++)
                        {
                            AgregaEstacion(sLinea, txtModelo.Text.ToString(), 0, null, "INSPECTOR MYLAR", "Y", null,"III",null);
                        }
                    }


                    if (datos.Rows[0]["ind_robot"].ToString() == "1")
                    {
                        AgregaEstacion(sLinea, txtModelo.Text.ToString(), 0, null, "ROBOT", "T", null, "III",null);
                    }

                    ModelosDataBound(_asModelo);
                    dgwEstaciones.Rows[0].Cells[8].Selected = true;

                }
                else
                {
                    MessageBox.Show("El Modelo no se encuentra Registrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtModelo.Clear();
                }

                _lbCambio = false;
                _lbCambioDet = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
        private void AgregaEstacion(string _asLinea, string _asModelo, int _aiCveEstacion, string _asEstacion, string _asOperacion, string _asTipo, string _asTipona, string _asNivel,string _asSinNivel)
        {
            System.Data.DataTable dt = dgwEstaciones.DataSource as System.Data.DataTable;
            dt.Rows.Add(null, 0, _asLinea, _asModelo, _aiCveEstacion, _asEstacion, _asOperacion,_asNivel, null, null, null, null, _asTipo, _asTipona,_asSinNivel);
        }
        
        private void ModelosDataBound(string _asModelo)
        {
            ModestaLogica mod = new ModestaLogica();
            mod.Modelo = _asModelo;
            System.Data.DataTable data = ModestaLogica.Listar(mod);
            if (data.Rows.Count > 0)
            {
                string sLinea = string.Empty;
                if (cbbLinea.SelectedIndex != -1)
                    sLinea = cbbLinea.SelectedValue.ToString();

                for(int i = 0; i < data.Rows.Count; i ++)
                {
                    int iCve = Convert.ToInt32(data.Rows[i][1].ToString());
                    string sEstacion  = data.Rows[i][2].ToString();

                    if (string.IsNullOrEmpty(data.Rows[i][3].ToString()))
                        continue;

                    string sNombre = data.Rows[i][3].ToString();
                    int iCant = Convert.ToInt32(data.Rows[i][4].ToString());
                    string sNivel = data.Rows[i][7].ToString();
                    for (int e = 0; e < iCant; e++)
                    {
                        AgregaEstacion(sLinea, txtModelo.Text.ToString(), iCve, sEstacion, sNombre, "O", null,sNivel,null);
                    }
                }
            }
            else
            {
                MessageBox.Show("El Formato Estandar del Modelo no se encuentra registrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private void chbControl_CheckedChanged(object sender, EventArgs e)
        {
            if (chbControl.Checked)
            {
                wfCapturaPop_1t wfPop = new wfCapturaPop_1t(_lsProceso);
                wfPop._sClave = "Controlada";
                wfPop.ShowDialog();
                string sAut = wfPop._sClave;
                if (!string.IsNullOrEmpty(sAut))
                {
                    cbbAutorizo.SelectedValue = sAut;
                    //Capturar Modelo del RPO
                    wfCapturaPop_1t wfPop2 = new wfCapturaPop_1t(_lsProceso);
                    wfPop2._sClave = "Modelo";
                    wfPop2.ShowDialog();
                    string sModRpo = wfPop2._sClave;
                    if (!string.IsNullOrEmpty(sModRpo))
                        txtNota.Text = sModRpo;
                    else
                    {
                        MessageBox.Show("No se ha especificado el Modelo del RPO", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        chbControl.Checked = false;
                    }
                }
                else
                    chbControl.Checked = false;
            }
            else
                cbbAutorizo.SelectedIndex = -1;
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
            if (cbbLinea.SelectedIndex != -1)
                sLinea = cbbLinea.SelectedValue.ToString();
            AgregaEstacion(sLinea, txtModelo.Text.ToString(), iClave, sEstacion, sOperacion, "Z", null,null,null);

            //* EL ING SE TOMA DE LA ASIGNACION POR LINEA *//
            //string sAut = string.Empty;
            //if(cbbAutorizo.SelectedIndex != -1)
            //    sAut = cbbAutorizo.SelectedValue.ToString();
            //else
            //{
            //    wfCapturaPop wfPop = new wfCapturaPop(_lsProceso);
            //    wfPop._sClave = "Controlada";
            //    wfPop.ShowDialog();
            //    sAut = wfPop._sClave;
            //}
            
            //if (!string.IsNullOrEmpty(sAut))
            //{
            //    cbbAutorizo.SelectedValue = sAut;
            //    AgregaEstacion(sLinea, txtModelo.Text.ToString(), iClave, sEstacion, sOperacion, "Z", null, null, null);    
            //}

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
                if (cbbLinea.SelectedIndex != -1)
                    sLinea = cbbLinea.SelectedValue.ToString();
                AgregaEstacion(sLinea, txtModelo.Text.ToString(), iClave, sEstacion, sOperacion + " (OP. EXTRA)", "X", null,sNivel,null);

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
        private bool ModificarLinea()
        {
            try
            {
                wfPlantaPop_1t popUp = new wfPlantaPop_1t(_lsPlanta);
                popUp.ShowDialog();
                string sPlanta = popUp._sClave;
                if (string.IsNullOrEmpty(sPlanta))
                    return false;

                string sLinea = sPlanta.Substring(sPlanta.IndexOf(":") + 1);
                sPlanta = sPlanta.Substring(0, sPlanta.IndexOf(":"));

                LineUpLogica lineup = new LineUpLogica();
                lineup.Folio = long.Parse(txtFolio.Text.ToString());
                lineup.Linea = sLinea;
                lineup.Planta = sPlanta;
                lineup.Usuario = GlobalVar.gsUsuario;
                return LineUpLogica.ModificaLinea(lineup);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
                
        }
        private void btnF1_Click(object sender, EventArgs e)
        {
            if (GlobalVar.gsUsuario == "ADMINPRO" && !string.IsNullOrEmpty(txtFolio.Text.ToString()))
            {
                DialogResult Result = MessageBox.Show("Desea Modificar la Linea del LineUp?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (ModificarLinea())
                        Close();
                }
                else
                    return;
            }
            else
            {
                if (dgwEstaciones.Rows.Count > 0)
                {
                    MessageBox.Show("Favor de Limpiar pantalla antes de Cambiar de Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    wfPlantaPop_1t popUp = new wfPlantaPop_1t(_lsPlanta);
                    popUp.ShowDialog();
                    string sPlanta = popUp._sClave;
                    if (string.IsNullOrEmpty(sPlanta))
                        return;

                    string sLinea = sPlanta.Substring(sPlanta.IndexOf(":") + 1);
                    sPlanta = sPlanta.Substring(0, sPlanta.IndexOf(":"));

                    _lsPlanta = sPlanta;
                    PlantaLogica pl = new PlantaLogica();
                    pl.Planta = sPlanta;
                    System.Data.DataTable dtPl = PlantaLogica.Consultar(pl);
                    tssPlanta.Text = dtPl.Rows[0]["nombre"].ToString().ToUpper();

                    LineaLogica line = new LineaLogica();
                    line.Planta = sPlanta;
                    cbbLinea.DataSource = LineaLogica.LineaPlanta(line);
                    cbbLinea.ValueMember = "linea";
                    cbbLinea.DisplayMember = "linea";
                    cbbLinea.SelectedValue = sLinea;

                    lblLinea.Text = sLinea;
                    //lblLineaHelp.Text = "L - " + sLinea;
                    lblLineaHelp.Text = sLinea;
                    tssLinea.Text = "LINEA " + sLinea;

                    CargarOperadores(sPlanta, sLinea);

                    if (txtRpo.Enabled)
                        txtRpo.Focus();
                    else
                        txtModelo.Focus();
                }
            }
            
        }
        #endregion

        #region regFlechas
        private void btnFirst_Click(object sender, EventArgs e)
        {
            string sValor = GlobalVar.Navegacion("F", "t_lineup", "folio", txtFolio.Text);
            _lsParam = sValor;

            if (_lbCambio)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (Guardar())
                        wfLineUp_1t_Load(sender, e);
                }
                else
                {
                    if (Result == DialogResult.No)
                        wfLineUp_1t_Load(sender, e);
                }
            }
            else
                wfLineUp_1t_Load(sender, e);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            string sValor = "";
            if (string.IsNullOrEmpty(txtFolio.Text))
                sValor = GlobalVar.Navegacion("F", "t_lineup", "folio", txtFolio.Text);
            else
                sValor = GlobalVar.Navegacion("B", "t_lineup", "folio", txtFolio.Text);

            _lsParam = sValor;

            if (_lbCambio)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (Guardar())
                        wfLineUp_1t_Load(sender, e);
                }
                else
                {
                    if (Result == DialogResult.No)
                        wfLineUp_1t_Load(sender, e);
                }
            }
            else
                wfLineUp_1t_Load(sender, e);
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
                sValor = GlobalVar.Navegacion("F", "t_lineup", "folio", txtFolio.Text);
            else
                sValor = GlobalVar.Navegacion("N", "t_lineup", "folio", txtFolio.Text);
            _lsParam = sValor;
            wfLineUp_1t_Load(sender, e);
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

            string sValor = GlobalVar.Navegacion("L", "t_lineup", "folio", txtFolio.Text);
            _lsParam = sValor;
            wfLineUp_1t_Load(sender, e);
        }

        #endregion

        #region regRes

        private void wfLineUp_1t_Resize(object sender, EventArgs e)
        {
            if (WindowState != _WindowStateAnt && WindowState != FormWindowState.Minimized)
            {
                _WindowStateAnt = WindowState;
                ResizeControl(panel1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(panel3, 1, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(dgwOper, 1, ref _iWidthAnt, ref _iHeightAnt, 0);
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

        #region regFotoTress
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            returnImage = resizeImage(returnImage, new Size(160, 100));

            return returnImage;
        }
        private void AgregarOperador(string _asCodigo, Image img, string _asDatos)
        {
            System.Data.DataTable dt = dgwOper.DataSource as System.Data.DataTable;
            dt.Rows.Add(_asCodigo, img, _asDatos);
        }
        private void dgwOper_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dgwOper.Rows.Count == 0)
                return;

            if (dgwEstaciones.Rows.Count == 0)
                return;

            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex == 2) //DATOS
            {

                int iEdx = dgwEstaciones.CurrentCell.RowIndex;
                if (iEdx < 0)
                {
                    MessageBox.Show("Seleecione la estación asignada", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    string sCodigo = dgwOper[0, e.RowIndex].Value.ToString();

                    dgwEstaciones[8, iEdx].Value = sCodigo;
                    iEdx++;
                    if (dgwEstaciones.Rows.Count > iEdx)
                        dgwEstaciones[8,iEdx].Selected = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        private void dgwEstaciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgwOper.Rows.Count == 0)
                return;

            if (dgwEstaciones.Rows.Count == 0)
                return;

            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex == 8) // # operador
            {

                try
                {
                    string sOper = dgwEstaciones[8, e.RowIndex].Value.ToString();
                    if (!string.IsNullOrEmpty(sOper))
                    {
                        foreach (DataGridViewRow row in dgwOper.Rows)
                        {
                            string sCodigo = row.Cells[0].Value.ToString();
                            sCodigo = sCodigo.PadLeft(6, '0');
                            if (sCodigo == sOper)
                            {
                                dgwOper[1, row.Index].Selected = true;
                                dgwOper.CurrentCell = dgwOper.Rows[row.Index].Cells[1];
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        #endregion
    }
}
