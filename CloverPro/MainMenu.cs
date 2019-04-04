using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Logica;
using Datos;
using System.IO;

namespace CloverPro
{
    public partial class MainMenu : Form

    {
        

        private string _lsVersion = "1.1.2.27";
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIdex);
        private const int SM_TABLETPC = 86;
        private readonly bool tabletEnabled;
        private DateTime ultimafecha;
        private int pos;
        private string sVersion;


        public MainMenu()
        {
            InitializeComponent();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool Empaque()
        {
            bool bEmp = false;
            try
            {
                GlobalVar.gsEstacion = Environment.MachineName.ToString().ToUpper();
                EstacionLogica est = new EstacionLogica();
                est.Estacion = GlobalVar.gsEstacion;
                if (EstacionLogica.Verificar(est))
                {


                    DataTable data = EstacionLogica.Consultar(est);
                    GlobalVar.gsPlanta = data.Rows[0]["planta"].ToString();

                    tssUsuario.Text = GlobalVar.gsEstacion;
                    tssPlanta.Text = GlobalVar.gsPlanta;
                    tssVersionBD.Text = _lsVersion;
                    tssTurno.Text = GlobalVar.gsTurno;

                    string sPro = data.Rows[0]["proceso"].ToString();
                    string sArea = data.Rows[0]["area"].ToString();
                    GlobalVar.gsArea = sArea;

                    if(sPro == "EMP010" || sPro == "EMP020")
                    {
                        if (sArea == "I")
                        {
                            wfCapturaPop4 wfOpcion = new wfCapturaPop4();
                            wfOpcion.ShowDialog();

                            string sOpcion = wfOpcion._sClave;
                            if (sOpcion == "C")
                            {
                                wfCodBarrasComFin wfCodFinal = new wfCodBarrasComFin();
                                wfCodFinal.ShowDialog();
                                Close();
                            }
                            else
                            {
                                wfCodBarrasEmp wfEtiq = new wfCodBarrasEmp();
                                wfEtiq.ShowDialog();
                                Close();
                            }
                        }
                        else
                        {
                            wfCodBarrasComFin wfCodFinal = new wfCodBarrasComFin();
                            wfCodFinal.ShowDialog();
                            Close();
                        }
                    }
                    else
                    {
                        if (sPro == "EMP040" )
                        {
                            wfMonitorRPO wfRpo = new wfMonitorRPO();
                            wfRpo.ShowDialog();
                            Close();
                        }
                    }

                  
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
           
            return bEmp;
        }

        private bool Monitor()
        {
            bool bVal = false;
            try
            {
                GlobalVar.gsEstacion = Environment.MachineName.ToString().ToUpper();
                EstacionLogica est = new EstacionLogica();
                est.Estacion = GlobalVar.gsEstacion;
                if (EstacionLogica.VerificarMon(est))
                {
                    DataTable data = EstacionLogica.Consultar(est);
                    GlobalVar.gsPlanta = data.Rows[0]["planta"].ToString();
                    string sPlanta = GlobalVar.gsPlanta;

                    tssUsuario.Text = GlobalVar.gsEstacion;
                    tssPlanta.Text = GlobalVar.gsPlanta;
                    tssVersionBD.Text = _lsVersion;
                    
                    
                    wfVisorLineSetUp VisorSetUp = new wfVisorLineSetUp();
                    VisorSetUp._lsEstacion = est.Estacion;
                    VisorSetUp.Show();
                    VisorSetUp.BringToFront();
                    VisorSetUp.TopMost = true;

                    bVal = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return bVal;
        }

        private bool IsRunningOnTablet()
        {
            return (GetSystemMetrics(SM_TABLETPC) != 0);
        }
        private void MainMenu_Load(object sender, EventArgs e)
        {
            try
            {
                string sCal = Conexion.cadenaConexion;
                if(sCal.IndexOf("mxni3-app-08") == -1)
                {
                    wfVersionPop wfText = new wfVersionPop();
                    wfText.ShowDialog();
                    BackgroundImage = Properties.Resources.Wallpaper_debug;
                }

                tssVersion.Text = _lsVersion;
                OcultarMenu();

                if (Empaque())
                    return;

                if (Monitor())
                {
                    Application.OpenForms[1].BringToFront();
                    return;
                }
                    

                wfLogin Login = new wfLogin();
                Login.ShowDialog();
                string sUsuario = Login._sUsuario;
                if (string.IsNullOrEmpty(sUsuario))
                {
                    Close();
                }

                

                string sVersion = "";
                DataTable dtConf = ConfigLogica.Consultar();
                if (dtConf.Rows.Count != 0)
                    sVersion = dtConf.Rows[0]["version"].ToString();

                DataTable data = AccesoDatos.TraerUsuario(sUsuario);
                if(data.Rows.Count != 0)
                {
                    GlobalVar.gsUsuario = sUsuario;
                    GlobalVar.gsNombreUs = data.Rows[0][1].ToString();
                    GlobalVar.gsPlanta = data.Rows[0][2].ToString();
                    GlobalVar.gsDepto = data.Rows[0][4].ToString();
                    GlobalVar.gsArea = data.Rows[0][5].ToString();
                    GlobalVar.gsTurno = data.Rows[0][6].ToString();
                    
                    tssPlanta.Text = data.Rows[0][3].ToString();
                    tssVersionBD.Text = sVersion;
                    tssUsuario.Text = GlobalVar.gsNombreUs;
                    tssTurno.Text = data.Rows[0][6].ToString();

                    MostrarMenu(sUsuario);

                    if (!AccesoLogica.verificaAdmin(GlobalVar.gsUsuario))
                    {
                        ////// SUSTITUIR POR MODULO DEL USUARIO \\\\\\\\
                        if (GlobalVar.gsDepto == "PRO")
                        {
                            //PROCESO A MOSTRAR POR ESTACION DE TRABAJO
                            if(GlobalVar.gsArea == "ETI")
                            {
                                wfCodBarras CodBar = new wfCodBarras();
                                CodBar.ShowDialog();
                            }
                            else //AREA LINEA DE PRODUCCION
                            {
                                if (GlobalVar.gsArea == "PRO")
                                {
                                    //AREA DE TRABAJO X LINEA
                                    wfCapturaPop5 wfOpcion = new wfCapturaPop5();
                                    wfOpcion.ShowDialog();

                                    string sOpcion = wfOpcion._sClave;
                                    if (sOpcion == "C")
                                    {
                                        //if(IsRunningOnTablet())
                                        OperadorLogica Oper = new OperadorLogica();
                                        Oper.Operador = GlobalVar.gsUsuario;
                                        DataTable dt = OperadorLogica.Consultar(Oper);
                                        string sTipo = string.Empty;
                                        
                                        if (dt.Rows.Count != 0)
                                        {
                                            if (!string.IsNullOrEmpty(dt.Rows[0][5].ToString()))
                                            {
                                                string sPta = dt.Rows[0][4].ToString();
                                                string sLinea = dt.Rows[0][5].ToString();
                                                GlobalVar.gsPlanta = sPta;

                                                LineaLogica lin = new LineaLogica();
                                                lin.Planta = sPta;
                                                lin.Linea = sLinea;
                                                
                                                dt = LineaLogica.Consultar(lin);
                                                if(dt.Rows.Count > 0)
                                                    sTipo = dt.Rows[0]["tipo"].ToString();
                                            }
                                        }
                                        if (sTipo == "T")
                                        {
                                            wfLineUp_1t LineUpTouch = new wfLineUp_1t(string.Empty);
                                            LineUpTouch.ShowDialog();
                                            Close();
                                        }
                                        else
                                        {
                                            wfLineUp LineUp = new wfLineUp(string.Empty);
                                            LineUp.ShowDialog();
                                            Close();
                                        }
                                        
                                    }
                                    else
                                    {
                                        wfEtiquetaFinal wfEtiFin = new wfEtiquetaFinal();
                                        wfEtiFin.ShowDialog();
                                        Close();
                                    }
                                    
                                }
                            }

                        }
                        
                        if (GlobalVar.gsDepto == "CES")
                        {
                            wfCodBarrasRec CodBarR = new wfCodBarrasRec();
                            CodBarR.ShowDialog();
                        }
                        if (GlobalVar.gsDepto == "EMP")
                        {
                            
                            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, "EMP02000") == true) //CAPTURA ESPECIAL
                            {
                                wfCodBarrasCompras CodBarCom = new wfCodBarrasCompras();
                                CodBarCom.ShowDialog();
                                Close();
                            }
                            else
                            {
                                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, "EMP01000") == true) //ETIQUETA
                                {
                                    wfCodBarrasEmp CodBarEt = new wfCodBarrasEmp();
                                    CodBarEt.ShowDialog();
                                }
                                else
                                {
                                    if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, "EMP04000") == true) //ETIQUETA
                                    {
                                        if(GlobalVar.gsArea == "ETI" || GlobalVar.gsArea == "ALM")
                                        {
                                            wfMonitorRPO wfRpo = new wfMonitorRPO();
                                            wfRpo.ShowDialog();
                                            Close();
                                        }
                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        #region regPermisos
        private void OcultarMenu()
        {
            foreach (ToolStripMenuItem mitem in menuStrip1.Items) 
            {
                mitem.Visible = false;
                foreach(ToolStripMenuItem subitem in mitem.DropDownItems)
                {
                    subitem.Visible = false;
                }
            }   
        }

        private void MostrarMenu(string as_Usuario)
        {
        
            int iCon = 0;
            string[] sMod = new string[menuStrip1.Items.Count];

            DataTable dtPer = AccesoLogica.listarAccesos(as_Usuario);
            string[] sPer = new string[dtPer.Rows.Count];
            int iCont = 0;
            foreach (DataRow row in dtPer.Rows)
            {
                if (!sMod.Contains(row[0].ToString().Substring(0, 3)))
                {
                    sMod[iCon] = row[0].ToString().Substring(0, 3);
                    iCon++;
                }

                sPer[iCont] = row[0].ToString();
                iCont++;
            }

            foreach (ToolStripMenuItem mitem in menuStrip1.Items)
            {
                string sMenuItem = mitem.Name.Substring(3);

                if (sMenuItem == "Inicio")
                    mitem.Visible = true;

                if (sMod.Contains(sMenuItem) || GlobalVar.gsUsuario == "ADMINP")
                    mitem.Visible = true;

                foreach (ToolStripMenuItem subitem in mitem.DropDownItems)
                {
                    string sSubItem = subitem.Name.Substring(3);
                    if (sSubItem == "Exit")
                        subitem.Visible = true;
                    if (sSubItem == "Help")
                        subitem.Visible = true;

                    if (sPer.Contains(sSubItem) || GlobalVar.gsUsuario == "ADMINP")
                        subitem.Visible = true;
                }
            }

        }
        #endregion

        #region regOpenforms
        private void plantasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wfPlantas planta = new wfPlantas();
            planta.Show();
        }

        private void estacionDeTrabajoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wfEstaciones estacion = new wfEstaciones();
            estacion.Show();
        }

        private void configuraciónToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void etiquetasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            wfCodBarras barras = new wfCodBarras();
            barras.Show();
        }

        private void consultaDeRPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wfHistorico Hist = new wfHistorico();
            Hist.Show();
        }

        private void importarRPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wfUsuario User = new wfUsuario();
            User.Show();
        }

        private void tsmEtiquetaEmp_Click(object sender, EventArgs e)
        {
            wfCodBarrasEmp CodBarEmp = new wfCodBarrasEmp();
            CodBarEmp.Show();
        }

        private void tsmEtiquetaRec_Click(object sender, EventArgs e)
        {
            wfCodBarrasRec CodBarRec = new wfCodBarrasRec();
            CodBarRec.Show();
        }

        private void mitPRO050_Click(object sender, EventArgs e)
        {
            
            if (IsRunningOnTablet())
            {
                wfLineUp_1t LineUpTouch = new wfLineUp_1t(string.Empty);
                LineUpTouch.Show();
            }
            else
            {
                wfLineUp LineUp = new wfLineUp(string.Empty);
                LineUp.Show();
            }
            
        }

        private void mitSIS010_Click(object sender, EventArgs e)
        {
            wfConfig Conf = new wfConfig();
            Conf.Show();
        }

        private void listadoDeEtiquetasPorRPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wfRepEtiquetas RepEti = new wfRepEtiquetas();
            RepEti.Show();
        }
        private void mitCAT080_Click(object sender, EventArgs e)
        {
            wfActualizarOper monEmp = new wfActualizarOper();
            monEmp.Show();
        }


        private void mitEMP020_Click(object sender, EventArgs e)
        {
            wfCodBarrasComFin wfCodBarComp = new wfCodBarrasComFin();
            wfCodBarComp.ShowDialog();
        }

        private void mitCAT055_Click(object sender, EventArgs e)
        {
            wfDefectos wfDef = new wfDefectos();
            wfDef.Show();
        }

        private void mitPRO070_Click(object sender, EventArgs e)
        {
            if(GlobalVar.gsUsuario != "ADMINP")
            {
                DataTable dtConf = ConfigLogica.Consultar();
                string sItCod = dtConf.Rows[0]["codigo_bloq"].ToString();

                wfCapturaPop_1t wfPop = new wfCapturaPop_1t("PRO070");
                wfPop.ShowDialog();
                string sCve = wfPop._sClave;

                if (string.IsNullOrEmpty(sCve))
                    return;

                if (sCve != sItCod)
                    return;
            }
            

            wfEtiquetaFinal wfEtiFin = new wfEtiquetaFinal();
            wfEtiFin.Show();
        }

        private void coatingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //wfCoating Coating = new wfCoating();
            //Coating.Show();

        }

        private void mitPRO080_Click(object sender, EventArgs e)
        {
            wfLineSetUp SetUp = new wfLineSetUp(string.Empty);
            SetUp.Show();
        }

        private void mitCAT040_Click(object sender, EventArgs e)
        {
            wfOperador Oper = new wfOperador();
            Oper.Show();
        }

        private void mitCAT060_Click(object sender, EventArgs e)
        {
            wfImportarRpo Importar = new wfImportarRpo();
            Importar.Show();
        }

        private void mitCAT050_Click(object sender, EventArgs e)
        {
            wfModelos Modelo = new wfModelos();
            Modelo.Show();
        }
        private void mitPRO020_Click(object sender, EventArgs e)
        {
            wfInspeccionManual insp = new wfInspeccionManual();
            insp.Show();
        }
        private void mitCAT070_Click(object sender, EventArgs e)
        {
            wfSuperLinea SupLin = new wfSuperLinea();
            SupLin.Show();
        }
        private void mitPRO060_Click(object sender, EventArgs e)
        {
            wfDuplicaLineUp DupLineUp = new wfDuplicaLineUp();
            DupLineUp.Show();
        }

        private void mitPRO090_Click(object sender, EventArgs e)
        {
            wfMonitorLineSetUp MonSetup = new wfMonitorLineSetUp();
            MonSetup.Show();
        }

        private void mitPLA010_Click(object sender, EventArgs e)
        {
            wfAreas Areas = new wfAreas();
            Areas.Show();
        }

        private void mitPLA020_Click(object sender, EventArgs e)
        {
            wfLineSetUp LineSetUp = new wfLineSetUp("");
            LineSetUp.Show();
        }

        private void mitPLA030_Click(object sender, EventArgs e)
        {
            wfMonitorLineSetUp MonitorSetUp = new wfMonitorLineSetUp();
            MonitorSetUp.Show();
        }

        private void mitPLA040_Click(object sender, EventArgs e)
        {
            wfVisorLineSetUp VisorSetUp = new wfVisorLineSetUp();
            VisorSetUp.Show();
        }
        #endregion

        #region regReportes
        private void stmR020_Click(object sender, EventArgs e)
        {
            wfRepInspeccion repIns = new wfRepInspeccion();
            repIns.Show();
        }

        private void mitREP030_Click(object sender, EventArgs e)
        {
            wfRepLineUp repLineup = new wfRepLineUp();
            repLineup.Show();
        }

        private void mitREP070_Click(object sender, EventArgs e)
        {
            wfRepLineUpResumen repLineRes = new wfRepLineUpResumen();
            repLineRes.Show();
        }
        private void mitREP040_Click(object sender, EventArgs e)
        {
            wfRepUsuarios repUser = new wfRepUsuarios();
            repUser.Show();
        }

        private void mitREP050_Click(object sender, EventArgs e)
        {
            wfRepSupline repSup = new wfRepSupline();
            repSup.Show();
        }

        private void mitREP060_Click(object sender, EventArgs e)
        {
            wfRepTransferencia repTrans = new wfRepTransferencia();
            repTrans.Show();
        }

        private void mitREP080_Click(object sender, EventArgs e)
        {
            wfRepEtiquetaComp repEtiCom = new wfRepEtiquetaComp();
            repEtiCom.Show();
        }

        private void mitREP090_Click(object sender, EventArgs e)
        {
            wfRepHistorialPPH wfPPH = new wfRepHistorialPPH();
            wfPPH.Show();
        }

        private void mitREP100_Click(object sender, EventArgs e)
        {
            wfRepSetUp wfSetUp = new wfRepSetUp();
            wfSetUp.Show();
        }

        private void mitREP110_Click(object sender, EventArgs e)
        {
            wfRepEmpaqueProc wfRepProc = new wfRepEmpaqueProc();
            wfRepProc.Show();
        }

        private void mitREP120_Click(object sender, EventArgs e)
        {
            wfRepGlobalRPO wfGlobals = new wfRepGlobalRPO();
            wfGlobals.Show();
        }
        
        private void mitREP140_Click(object sender, EventArgs e)
        {
            wfRepEntregaDiario wfRepDiario = new wfRepEntregaDiario();
            wfRepDiario.Show();
        }
        #endregion

        private void mitEMP030_Click(object sender, EventArgs e)
        {
            wfEmpaqueRebox wfRebox = new wfEmpaqueRebox();
            wfRebox.Show();
        }

        private void mitEMP040_Click(object sender, EventArgs e)
        {
            wfMonitorRPO wfRPO = new wfMonitorRPO();
            wfRPO.Show();
        }

        private void mitCAT090_Click(object sender, EventArgs e)
        {
            wfUbicacionRpo wfUbic = new wfUbicacionRpo();
            wfUbic.Show();
        }

        private void mitCAT065_Click(object sender, EventArgs e)
        {
            wfCargarRPO wfRPO = new wfCargarRPO();
            wfRPO.Show();
        }

        private void rPODashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wfDashboardRPO wfGrafRPO = new wfDashboardRPO();
            wfGrafRPO.Show();
        }

        private void capturaDeLineUpV2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wfLineUp_1t LineUpTouch = new wfLineUp_1t(string.Empty);
            LineUpTouch.Show();
        }

        private void mitPRO090_Click_1(object sender, EventArgs e)
        {
            wfImpSetUpDuracion wfSetup = new wfImpSetUpDuracion();
            wfSetup.Show();
        }

        private void mitEMP050_Click(object sender, EventArgs e)
        {
            wfGlobals wfRpoGlob = new wfGlobals();
            wfRpoGlob.Show();
        }

        private void mitINV010_Click(object sender, EventArgs e)
        {
            wfInvCiclico wfCiclico = new wfInvCiclico();
            wfCiclico.Show();
        }

        private void mitPLA050_Click(object sender, EventArgs e)
        {
            
        }

        private void simuladorHeadCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wfLineCapacity wfCapacity = new wfLineCapacity();
            wfCapacity.Show();
        }

        private void tsmREP_Click(object sender, EventArgs e)
        {

        }

        private void reporteDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wfRepGraficaRPO wfGlobals = new wfRepGraficaRPO();
            wfGlobals.Show();
        }

        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
