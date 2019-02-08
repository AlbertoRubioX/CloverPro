using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Windows.Forms;
using System.Media;
using Spire.Barcode.Forms;
using Spire.Barcode;
using System.IO;
using Logica;
using Datos;

namespace CloverPro
{
    public partial class wfCodBarrasComFin : Form
    {
        private string _lsProceso = "EMP020";
        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;
        private string _lsPlanta;
        private string _lsLinea;
        private string _lsEstacion;
        private int _liFolio;
        private int _liCant;
        private int _liError;
        public wfCodBarrasComFin()
        {
            InitializeComponent();
            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }

        #region regInicio
        private void Inicio()
        {
            lblFecha.Text = Convert.ToString(DateTime.Now);
            lblScan.Text = "SCANNED  [ 0 ]";
            lblBloc.Text = "BLOCKED  [ 0 ]";

            txtModelo.Clear();
            txtModeloNext.Clear();
            GlobalVar.CambiaColor(txtModeloNext, 1);

            _lsEstacion= GlobalVar.gsEstacion;

            try
            {
                EstacionLogica est = new EstacionLogica();
                est.Estacion = _lsEstacion;
                DataTable data = EstacionLogica.Consultar(est);
                if (data.Rows.Count != 0)
                {
                    _lsPlanta = data.Rows[0]["planta"].ToString();
                    _lsLinea = data.Rows[0]["linea"].ToString();
                    _lsEstacion = data.Rows[0]["estacion"].ToString();

                    PlantaLogica pla = new PlantaLogica();
                    pla.Planta = _lsPlanta;
                    DataTable dt = PlantaLogica.Consultar(pla);
                    sslPlanta.Text = dt.Rows[0]["nombre"].ToString();
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Departamento de Sistemas" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sslLinea.Text = _lsLinea;
            sslEstacion.Text = _lsEstacion;

            txtModelo.Enabled = false;
            _liFolio = 0;
            _liCant = 0;
            _liError = 0;

            wfCapturaPop wfCap = new wfCapturaPop(_lsProceso);
            wfCap.ShowDialog();
            string sCodigo = wfCap._sClave;
            if (!string.IsNullOrEmpty(sCodigo))
            {
                txtModelo.Text = sCodigo;

                _liFolio = AccesoDatos.Consec(_lsProceso);
                Guardar("P");

                txtModeloNext.Focus();
            }
            else
                Close();
            
        }
        private void wfCodBarrasComIni_Activated(object sender, EventArgs e)
        {
            txtModelo.Focus();
        }

        private void wfCodBarrasComIni_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            Inicio();
            txtModelo.Focus();
        }

        #endregion

        #region regCaptura
        private void txtModelo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtModelo.Text) || string.IsNullOrWhiteSpace(txtModelo.Text))
                return;

            if (!string.IsNullOrEmpty(txtModelo.Text))
                txtModeloNext.Focus();
        }

        private void txtModeloNext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtModelo.Text) || string.IsNullOrWhiteSpace(txtModelo.Text))
                return;

            if (string.IsNullOrEmpty(txtModeloNext.Text) || string.IsNullOrWhiteSpace(txtModeloNext.Text))
                return;

            if (txtModeloNext.Text.Substring(0, 1) == "P")
                txtModeloNext.Text = txtModeloNext.Text.Substring(1);
            
            
            if (!Valida())
            {
                AgregarEr();
                EnviarMail();
                string sParam = _liFolio.ToString() + ":" + _lsProceso;

                Console.Beep(800, 1000);
                wfCapturaPop2 wfAlert = new wfCapturaPop2(sParam);
                wfAlert.ShowDialog();

                wfCapturaPop3 wfMotivo = new wfCapturaPop3(sParam);
                wfMotivo.ShowDialog();
                string sMotivo = wfMotivo._sClave;

                AgregarMotivo(sMotivo);
                EnviarMail();

                Contador(1);
                GlobalVar.CambiaColor(txtModeloNext, 2);
            }
            else
            {
                Contador(0);
                GlobalVar.CambiaColor(txtModeloNext, 1);
            }
            txtModeloNext.SelectAll();
            txtModeloNext.Focus();
        }
        private void txtModelo_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtModelo, 0);
        }

        private void txtModelo_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtModelo.Text) && string.IsNullOrEmpty(txtModeloNext.Text))
                txtModeloNext.Focus();
            else
                GlobalVar.CambiaColor(txtModelo, 1);
        }

        private void txtModeloNext_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtModeloNext, 0);
        }

        private void txtModeloNext_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtModelo.Text))
                txtModelo.Focus();
            else
                GlobalVar.CambiaColor(txtModeloNext, 1);
        }

        private void Contador(int _aiTipo)
        {
            int iP = 0;
            string sCant = "0";

            if (_aiTipo == 0)
            {
                _liCant++;
                iP = lblScan.Text.IndexOf("[");
                sCant = _liCant.ToString();
                lblScan.Text = lblScan.Text.Substring(0, iP + 2) + sCant + " ]";
            }
            else
            {
                
                _liError++;
                iP = lblBloc.Text.IndexOf("[");
                sCant = _liError.ToString();
                lblBloc.Text = lblBloc.Text.Substring(0, iP + 2) + sCant + " ]";
            }
        }

        public void AgregarEr()
        {
            Cursor = Cursors.WaitCursor;
            EtiquetaEmpdetLogica etid = new EtiquetaEmpdetLogica();
            etid.Folio = _liFolio;
            etid.Consec = 0;
            etid.VendorCode = txtModeloNext.Text.ToString();
            etid.ConsError = _liCant + 1;
            etid.IndAlerta = "S";
            etid.Usuario = "EMPAQUE";
            EtiquetaEmpdetLogica.Guardar(etid);

            Cursor = Cursors.Default;
        }
        public void AgregarMotivo(string _asMotivo)
        {
            Cursor = Cursors.WaitCursor;
            EtiquetaEmpdetLogica etid = new EtiquetaEmpdetLogica();
            etid.Folio = _liFolio;
            etid.ConsError = _liCant + 1;
            etid.Consec = EtiquetaEmpdetLogica.ConsultaCons(etid);
            etid.VendorCode = txtModeloNext.Text.ToString();
            etid.ConsError = _liCant + 1;
            etid.IndAlerta = "C";
            etid.MotivoBloq = _asMotivo;
            etid.Usuario = "EMPAQUE";
            EtiquetaEmpdetLogica.Guardar(etid);

            Cursor = Cursors.Default;
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

        private void wfCodBarrasComFin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Guardar("T");
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ConfigLogica.Consultar();
                string sDirec = dt.Rows[0]["directorio"].ToString();
                if (!string.IsNullOrEmpty(sDirec))
                {
                    sDirec += @"\CloverPRO_AyudaVisual_Empaque.pdf";
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

        #region regResize
        private void wfCodBarrasComIni_Resize(object sender, EventArgs e)
        {
            if (WindowState != _WindowStateAnt && WindowState != FormWindowState.Minimized)
            {
                _WindowStateAnt = WindowState;
                ResizeControl(panel1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);

                CenterControl(panel2);
                
            }
        }

        private void CenterControl(Control ac_Control)
        {
            int iW = this.Width / 2;
            int iH = this.Height / 2;

            int iW2 = ac_Control.Width / 2;
            int iH2 = ac_Control.Height / 2;

            int iX = iW - iW2;
            int iY = iH - iH2;

            ac_Control.Location = new Point(iX, iY);
            panel3.Location = new Point(iX, iY - 50);


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

        #endregion

        #region regGuardar
        private bool Valida()
        {
            if (txtModelo.Text.ToString().ToUpper().TrimEnd() != txtModeloNext.Text.ToString().ToUpper().TrimEnd())
                return false;

            return true;
        }

        private void Guardar(string _asEst)
        {   
            try
            {
                if (_liFolio > 0)
                {
                    EtiquetaEmpLogica eti = new EtiquetaEmpLogica();
                    eti.Folio = _liFolio;
                    eti.Planta = _lsPlanta;
                    eti.Linea = _lsLinea;
                    eti.Estacion = _lsEstacion;
                    eti.VendorCode = txtModelo.Text.ToString();
                    eti.Saldo = _liCant;
                    eti.CantError = _liError;
                    eti.Estatus = _asEst;
                    eti.Usuario = "EMPAQUE";

                    if (EtiquetaEmpLogica.Guardar(eti) == 0)
                    {
                        MessageBox.Show("Error al intentar guardar." + Environment.NewLine + "EtiquetaEmpLogica.Guardar(eti)", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private bool EnviarMail()
        {

            DataTable dtMail = new DataTable();
            CorreoLogica mail = new CorreoLogica();
            mail.FolioRef = _liFolio.ToString();
            mail.Proceso = _lsProceso;
            dtMail = CorreoLogica.ConsultarRef(mail);
            if (dtMail.Rows.Count != 0)
            {
                try
                {
                    int iFolio = Convert.ToInt32(dtMail.Rows[0]["folio"].ToString());
                    EnviarCorreo envMail = new EnviarCorreo();
                    envMail.FolioMail = iFolio;
                    envMail.Asunto = dtMail.Rows[0]["asunto"].ToString();

                    EnviarCorreo.EnviaAlertaAsync(envMail);

                    CorreoLogica correo = new CorreoLogica();

                    correo.Estado = "E";
                    correo.Folio = Convert.ToInt32(dtMail.Rows[0]["folio"].ToString());
                    correo.Asunto = dtMail.Rows[0]["asunto"].ToString();
                    correo.FolioRef = _liFolio.ToString();
                    correo.Proceso = _lsProceso;
                    CorreoLogica.Guardar(correo);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }

            return true;
        }



        #endregion

        private void lblBloc_Click(object sender, EventArgs e)
        {

        }
    }
}
