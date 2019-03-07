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
    public partial class wfCapturaPop_1t : Form
    {
        public string _lsProceso;
        public string _sClave;
        public long _llFolio;
        public int _liConsec;
        public string _lsPlanta;
        public string _lsTipo;
        public string _lsArea;
        private bool bChange;

        DataTable dataTable = new DataTable();
        // Creates a DataView.
        DataView listDataView;
        bool isTextCleared = true;
        string rowFilterText = string.Empty;
        DataTable dtOper = new DataTable();

        public wfCapturaPop_1t(string asProceso)
        {
            InitializeComponent();
            _lsProceso = asProceso;
            
        }

        private void wfCapturaPop_1t_Load(object sender, EventArgs e)
        {
            this.Height = 161;
            panel1.Height = 103;
            #region regEmpaque

            if (_lsProceso == "EMP050")//GLOBALS
            {
                //REBOX PARA EMPAQUE
                this.Height = 210;
                btnAceptar.Visible = true;

                if (_sClave == "LINEA")
                {
                    label1.Text = "LINEA :";
                    LineaLogica lin = new LineaLogica();
                    lin.Planta = _lsPlanta;
                    DataTable dtL = LineaLogica.LineasNav(lin);
                    cbbClave.DataSource = dtL;
                    cbbClave.DisplayMember = "linea_nav";
                    cbbClave.ValueMember = "linea_nav";
                }
                else
                {
                    label1.Text = "DESTINO :";
                    Dictionary<string, string> list = new Dictionary<string, string>();
                    list.Add("CLX", "Calexico");
                    list.Add("VNS", "Van Nuys");
                    list.Add("C&V", "Calexico / Van Nuys");
                    cbbClave.DataSource = new BindingSource(list, null);
                    cbbClave.DisplayMember = "Value";
                    cbbClave.ValueMember = "Key";
                }
                    
                cbbClave.Visible = true;
                txtClave.Visible = false;

                cbbClave.SelectedIndex = -1;
            }

            if (_lsProceso == "EMP030")
            {
                //REBOX PARA EMPAQUE
                btnAceptar.Visible = false;
                label1.Text = "ITEM UPC BARCODE :";
                cbbClave.Visible = false;
                txtClave.Visible = true;

            }
            if (_lsProceso == "EMP040")
            {
                this.Height = 210;
                btnAceptar.Visible = true;

                ControlRpoLogica rpo = new ControlRpoLogica();
                rpo.Folio = _llFolio;
                rpo.Consec = _liConsec;
                DataTable dt = new DataTable();

                if (_sClave == "ALMACENISTA")
                {
                    label1.Text = "ALMACENISTA :";
                    cbbClave.Visible = true;
                    txtClave.Visible = false;

                    OperadorLogica op = new OperadorLogica();
                    op.Planta = _lsPlanta;
                    op.Nivel = "MAT";
                    op.Turno = GlobalVar.gsTurno;
                    dtOper = OperadorLogica.ConsultarPuesto(op);

                    cbbClave.DropDownStyle = ComboBoxStyle.DropDown;
                    cbbClave.DroppedDown = true;
                    cbbClave.DataSource = dtOper;
                    cbbClave.DisplayMember = "nombre";
                    cbbClave.ValueMember = "empleado";
                    cbbClave.SelectedIndex = -1;
                    
                }

                if (_sClave == "PRIORIDAD")
                {
                    txtClave.Visible = false;
                    cbbClave.Visible = true;
                    label1.Text = "PRIORIDAD :";

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
                    cbbClave.DataSource = new BindingSource(Data, null);
                    cbbClave.DisplayMember = "Value";
                    cbbClave.ValueMember = "Key";
                    cbbClave.SelectedIndex = -1;
                }
                else
                {
                    if (_sClave == "UBICACION")
                    {
                        txtClave.Visible = false;
                        cbbClave.Visible = true;
                        label1.Text = "UBICACION :";

                        RpoUbicaDetLogica ubi = new RpoUbicaDetLogica();
                        ubi.Planta = _lsPlanta;
                        dt = RpoUbicaDetLogica.ConsultaPlanta(ubi);
                        cbbClave.DataSource = dt;
                        cbbClave.DisplayMember = "ubica";
                        cbbClave.ValueMember = "ubica";
                        cbbClave.SelectedIndex = -1;
                    }
                    else
                    {
                        if (_sClave == "WP")
                        {
                            label1.Text = "PICK :";
                            cbbClave.Visible = false;
                            txtClave.Visible = true;
                        }

                        if (_sClave == "ENTREGA")
                        {
                            label1.Text = "ETIQUETA ENTREGADA A :";
                            cbbClave.Visible = false;
                            txtClave.Visible = true;
                            btnAceptar.Visible = false;
                        }

                        if (_sClave == "DETENIDO")
                        {
                            this.Height = 510;

                            label1.Text = "MOTIVO :";
                            /*
                            cbbClave.Visible = false;
                            txtClave.Visible = true;
                            */

                            txtClave.Visible = false;
                            cbbClave.Visible = true;

                            Dictionary<string, string> Data = new Dictionary<string, string>();
                            Data.Add("INS", "INSUFICIENTE");
                            Data.Add("CAL", "PROBLEMAS DE CALIDAD");
                            //Data.Add("INV", "PROBLEMAS DE INVENTARIO");
                            
                            if (_lsArea == "E") //etiquetas
                                Data.Add("DOC", "PROBLEMAS CON DOCUMENTACION");
                            
                            if(_lsArea == "A")
                            {
                                Data.Add("EST", "PROBLEMAS DE ESTRUCTURA EN RPO");
                                if(_lsTipo == "P")
                                    Data.Add("CAP", "CAPACIDAD"); // SOLO LINEAS PONY
                            }

                            Data.Add("CAN", "RPO CANCELADO");
                            Data.Add("OTR", "OTRO");

                            cbbClave.DataSource = new BindingSource(Data, null);
                            cbbClave.DisplayMember = "Value";
                            cbbClave.ValueMember = "Key";
                            cbbClave.SelectedIndex = -1;
                        }
                    }
                }
            }
            #endregion

            #region regSetUp
            if (_lsProceso == "PLA030")
            {
                this.Height = 210;

                if (_lsTipo == "E")
                {
                    label1.Text = "ESTATUS DEL SETUP:";
                    cbbClave.Visible = true;
                    txtClave.Visible = false;
                    btnAceptar.Visible = true;

                    Dictionary<string, string> Est = new Dictionary<string, string>();
                    Est.Add("E", "EN ESPERA");
                    Est.Add("P", "EN PROCESO");
                    Est.Add("R", "REALIZADO");
                    cbbClave.DataSource = new BindingSource(Est, null);
                    cbbClave.DisplayMember = "Value";
                    cbbClave.ValueMember = "Key";
                    cbbClave.SelectedIndex = -1;

                    if (!string.IsNullOrEmpty(_sClave))
                        cbbClave.SelectedValue = _sClave;
                }
                else
                {
                    label1.Text = "DURACION DEL SET-UP:";
                    cbbClave.Visible = false;
                    txtClave.Visible = true;
                    btnAceptar.Visible = true;

                    if (!string.IsNullOrEmpty(_sClave))
                        txtClave.Text = _sClave;
                }
            }
            #endregion

            if (_lsProceso == "PRO070")//ESCANEO FINAL DE ETIQUETAS / REEMPLAZA MP
            {
                //REBOX PARA EMPAQUE
                btnAceptar.Visible = false;
                label1.Text = "CODIGO DE AUTORIZACION :";
                cbbClave.Visible = false;
                txtClave.Visible = true;
                txtClave.PasswordChar = '*';

            }


            if (_lsProceso == "PRO050")
            {
                if (_sClave == "Controlada")
                {
                    txtClave.Visible = false;
                    cbbClave.Visible = true;
                    label1.Text = "AUTORIZACION DE INGENIERIA";

                    UsuarioLogica user = new UsuarioLogica();
                    user.Area = "ING";
                    cbbClave.DataSource = UsuarioLogica.ListarArea(user);
                    cbbClave.ValueMember = "usuario";
                    cbbClave.DisplayMember = "nombre";
                    cbbClave.SelectedIndex = -1;
                }
                else
                {
                    if (_sClave == "Modelo")
                    {
                        label1.Text = "NOTA / MODELO DEL RPO";
                        cbbClave.Visible = false;
                        txtClave.Visible = true;
                    }
                    else
                        label1.Text = "NOMBRE DE LA ESTACION";
                }
            }
        }

        private void txtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _sClave = null;
                Close();
            }

            if (_lsProceso == "PLA030")
                return;

            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtClave.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
                return;

            if (_lsProceso == "EMP040")
            {
                if(_sClave != "ENTREGA")
                    return;

                string sEmp = txtClave.Text.ToString().Trim();

                //# DE EMP QUE RECIBE LA ETIQUETA
                if (sEmp.IndexOf("A") != -1)
                    sEmp = sEmp.Substring(0,sEmp.IndexOf("A"));

                int iEmp = 0;
                if(!int.TryParse(sEmp,out iEmp))
                {
                    MessageBox.Show("Numero de Empleado incorrecto", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtClave.Clear();
                    return;
                }

                if (sEmp.Length < 6)
                {
                    sEmp = sEmp.PadLeft(6, '0');
                }

                OperadorLogica oper = new OperadorLogica();
                oper.Operador = sEmp;
                if(!OperadorLogica.Verificar(oper))
                {
                    TressLogica img = new TressLogica();
                    img.Codigo = int.Parse(sEmp);
                    DataTable datos = TressLogica.ConsultaOper(img);
                    if (datos.Rows.Count > 0)
                    {
                        string sName = datos.Rows[0]["PRETTYNAME"].ToString();
                        sEmp = sEmp + ":" + sName;
                    }
                    else
                    {
                        UsuarioLogica user = new UsuarioLogica();
                        user.Usuario = sEmp;
                        if (!UsuarioLogica.Verificar(user))
                        {
                            MessageBox.Show("El Materialista no se encuentra registrado en el sistema ", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtClave.Clear();
                            return;
                        }
                    }
                }

                _sClave = sEmp;
                txtClave.Text = _sClave;
                Close();
                
            }


            _sClave = txtClave.Text.ToString().Trim();

            if (_lsProceso == "EMP020")
            {
                if (_sClave.Substring(0, 1) == "P")
                    _sClave = _sClave.Substring(1);
            }

            if (_lsProceso == "PRO060")
            {
                if (_sClave.IndexOf("RPO") == -1)
                    _sClave = "RPO" + _sClave;

                int iPos = _sClave.IndexOf("-");
                int iIni = 0;
                if (iPos > 0)
                {
                    string sIni = _sClave.Substring(iPos + 2, 5);
                    Int32.TryParse(sIni, out iIni);
                    iIni++;

                    _sClave = _sClave.Substring(0, iPos);
                }
            }

            if (_lsProceso == "PRO070")
            {
                if (string.IsNullOrEmpty(txtClave.Text.ToString()))
                    return;

                _sClave = txtClave.Text.ToString().Trim();
            }

            if (_lsProceso == "EMP030")
            {
                if (string.IsNullOrEmpty(txtClave.Text.ToString()))
                    return;

                _sClave = txtClave.Text.ToString().Trim();
            }

            Close();
        }

        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cbbClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _sClave = null;
                Close();
            }
            
            if(e.KeyCode == Keys.Enter)
                bChange = true;


        }

        private void cbbClave_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(_lsProceso == "EMP040")
            {
                if (cbbClave.SelectedIndex == -1)
                    return;
                

                try
                {
                    if(_sClave == "ALMACENISTA" && bChange)
                    {
                        ControlRpoLogica rpo = new ControlRpoLogica();
                        rpo.Folio = _llFolio;
                        rpo.Consec = _liConsec;
                        rpo.Surte = cbbClave.SelectedValue.ToString();
                        rpo.Usuario = GlobalVar.gsUsuario;
                        ControlRpoLogica.ActualizaSurte(rpo);
                        Close();
                    }

                    if(_sClave == "UBICACION")
                    {
                        string sUbi = cbbClave.SelectedValue.ToString();
                        string sCelda = sUbi.Substring(3);//01-G
                        sUbi = sUbi.Substring(0, 2);

                        RpoUbicaDetLogica ubi = new RpoUbicaDetLogica();
                        ubi.Planta = _lsPlanta;
                        ubi.Ubicacion = sUbi;
                        ubi.Celda = sCelda;
                        
                        ControlRpoLogica rpo = new ControlRpoLogica();
                        rpo.Folio = _llFolio;
                        rpo.Planta = _lsPlanta;
                        rpo.Locacion = cbbClave.SelectedValue.ToString();
                        DataTable dt = ControlRpoLogica.ConsultarDisp(rpo);
                        if (dt.Rows.Count != 0)
                        {
                            DialogResult Result = MessageBox.Show("La ubicación seleccionada no cuenta con espacion disponible en el sistema" + Environment.NewLine + "Desea agregarla a la Ubicación?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (Result == DialogResult.No)
                            {
                                cbbClave.SelectedIndex = -1;
                                cbbClave.Focus();
                                return;
                            }
                        }

                        rpo.Consec = _liConsec;
                        rpo.Locacion = cbbClave.SelectedValue.ToString();
                        rpo.Usuario = GlobalVar.gsUsuario;
                        ControlRpoLogica.ActualizaLocal(rpo);
                        Close();
                    }
                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                
            }
            else
            {
                if (_lsProceso != "PLA030" && _lsProceso != "EMP050")
                {
                    _sClave = cbbClave.SelectedValue.ToString();
                    Close();
                }
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_lsProceso == "EMP050")//GLOBALS
                {
                    if (cbbClave.SelectedIndex == -1)
                        return;
                    else
                    {
                        RpoGlobLogica rpo = new RpoGlobLogica();
                        rpo.Folio = _llFolio;
                        rpo.Planta = _lsPlanta;
                        rpo.Usuario = GlobalVar.gsUsuario;
                        if (_sClave == "LINEA")
                        {
                            rpo.Linea = cbbClave.SelectedValue.ToString();
                            RpoGlobLogica.ActualizaLinea(rpo);
                        }
                        else
                        {
                            rpo.Destino = cbbClave.SelectedValue.ToString();
                            RpoGlobLogica.ActualizaDestino(rpo);
                        }
                        Close();
                    }
                }

                if (_lsProceso == "EMP040")
                {
                    //cbbClave.SelectedIndex = -1;
                    if(txtClave.Visible == true)
                    {
                        if (string.IsNullOrEmpty(txtClave.Text))
                        {
                            MessageBox.Show("Favor de capturar la información requerida", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClave.Focus();
                            return;
                        }

                        if(txtClave.Text.Length <=2)
                        {
                            MessageBox.Show("Favor de capturar la información requerida", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClave.Focus();
                            return;
                        }

                        if (!string.IsNullOrEmpty(txtClave.Text))
                        {

                            if (_sClave == "DETENIDO")
                            {
                                _sClave = txtClave.Text.ToString();
                                Close();
                            }
                            if (_sClave == "ENTREGA")
                                return;
                            else
                            {
                                ControlRpoLogica rpo = new ControlRpoLogica();
                                rpo.Folio = _llFolio;
                                rpo.Consec = _liConsec;
                                rpo.Usuario = GlobalVar.gsUsuario;
                                rpo.Surte = txtClave.Text.ToString();
                                rpo.WP = txtClave.Text.ToString();
                                rpo.Usuario = GlobalVar.gsUsuario;
                                if (_sClave == "WP")
                                    ControlRpoLogica.ActualizaWP(rpo);
                                else
                                    ControlRpoLogica.ActualizaSurte(rpo);

                                Close();
                            }
                        }
                    }
                    else
                    {
                        if (cbbClave.SelectedIndex == -1)
                        {
                            MessageBox.Show("Favor de seleccionar del listado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cbbClave.Focus();
                            return;
                        }
                        
                        if (_sClave == "DETENIDO")
                        {
                            string sClave = cbbClave.SelectedValue.ToString();
                            if (sClave == "OTR")
                            {
                                cbbClave.Visible = false;
                                txtClave.Visible = true;
                                txtClave.Focus();
                                return;
                            }
                            else
                            {
                                if(sClave == "INS" || sClave == "CAL" || sClave == "EST")
                                {
                                    cbbClave.Visible = false;
                                    txtClave.Visible = true;
                                    label1.Text = "NO. DE PARTE:";
                                    txtClave.Focus();
                                    return;
                                }
                                else
                                {
                                    _sClave = sClave;
                                    Close();
                                }
                            }
                        }

                        ControlRpoLogica rpo = new ControlRpoLogica();
                        rpo.Folio = _llFolio;
                        rpo.Consec = _liConsec;
                        rpo.Usuario = GlobalVar.gsUsuario;

                        if(_sClave == "PRIORIDAD")
                        {
                            rpo.Prioridad = cbbClave.SelectedValue.ToString();
                            ControlRpoLogica.ActualizaPrio(rpo);
                        }
                        if (_sClave == "UBICACION")
                        {
                            rpo.Locacion = cbbClave.SelectedValue.ToString();
                            ControlRpoLogica.ActualizaLocal(rpo);
                        }

                        if (_sClave == "ALMACENISTA")
                        {
                            //verifica si el cbbclave esta vacio para que no guarde datos no nesesarios.
                            if (cbbClave.Text == "")
                                cbbClave.SelectedIndex = -1;

                            if (cbbClave.SelectedIndex == -1)
                            {
                                return;
                                //rpo.Surte = cbbClave.SelectedValue.ToString();
                                //ControlRpoLogica.ActualizaSurte(rpo);
                            }
                            else {

                                //Actualiza y guarda al almacenista asignado  
                                rpo.Surte = cbbClave.SelectedValue.ToString();
                                ControlRpoLogica.ActualizaSurte(rpo);
                                rpo.Almacen = "P";//en proceso
                                rpo.Usuario = GlobalVar.gsUsuario;
                                ControlRpoLogica.ActualizaAlma(rpo);
                            }
                        }
                        
                        Close();
                        
                    }
                }

                if (_lsProceso == "PLA030")
                {
                    if(_lsTipo == "E")
                    {
                        if (cbbClave.SelectedIndex == -1)
                        {
                            MessageBox.Show("Favor de indicar el Estatus del Set-Up", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cbbClave.Focus();
                            return;
                        }

                        if(_sClave !=  cbbClave.SelectedValue.ToString())
                        {
                            LineSetupDetLogica det = new LineSetupDetLogica();
                            det.Folio = _llFolio;
                            det.Consec = _liConsec;
                            det.Estatus = cbbClave.SelectedValue.ToString();

                            LineSetupDetLogica.ActualizaEst(det);
                            Close();

                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txtClave.Text))
                        {
                            MessageBox.Show("Favor de indicar la Duración del Set-Up", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }


                        double dValor = 0;
                        if (double.TryParse(txtClave.Text, out dValor))
                        {
                            LineSetupDetLogica det = new LineSetupDetLogica();
                            det.Folio = _llFolio;
                            det.Consec = _liConsec;
                            det.Duracion = dValor;

                            LineSetupDetLogica.ActualizaDura(det);
                            Close();
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

       
        private void cbbClave_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string sFilter = cbbClave.Text.ToString().ToUpper();
               if (string.IsNullOrEmpty(sFilter.ToString()))
                    return;

                OperadorLogica op = new OperadorLogica();
                op.Planta = _lsPlanta;
                op.Nivel = "MAT";
                op.Turno = GlobalVar.gsTurno;
                op.Nombre = sFilter;
                DataTable dt = OperadorLogica.ConsultarPuestoFiltro(op);

                cbbClave.DataSource = dt;
                cbbClave.DisplayMember = "nombre";
                cbbClave.ValueMember = "empleado";
                cbbClave.SelectedIndex = -1;
                cbbClave.Text = sFilter;
                cbbClave.SelectionStart = cbbClave.Text.Length;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cbbClave_DropDown(object sender, EventArgs e)
        {
            cbbClave.AutoCompleteMode = AutoCompleteMode.None;
        }
    }
}
