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
using Spire.Barcode.Forms;
using Spire.Barcode;
using Neodynamic.SDK.Printing;
using System.IO;
using Logica;
using Datos;

namespace CloverPro
{
    public partial class wfCodBarras : Form
    {
        private string _lsProceso = "PRO010";
        public wfCodBarras()
        {
            InitializeComponent();
        }

        #region rgeInicio
        private void Inicio()
        {
            txtRPO.Clear();
            txtSKU.Clear();
            txtSKU.Enabled = false;
            txtCant.Clear();
            txtInicio.Clear();
            txtFin.Clear();

        }
        private void wfCodBarras_Activated(object sender, EventArgs e)
        {
            //txtRPO.Focus();
        }

        private void wfCodBarras_Load(object sender, EventArgs e)
        {
            if (!AccesoLogica.verificaAdmin(GlobalVar.gsUsuario))
            {
                wfCodigo Codigo = new wfCodigo();
                Codigo.ShowDialog();
                string sCodigo = Codigo._sCodigo;
                if (string.IsNullOrEmpty(sCodigo))
                {
                    Close();
                }
            }

            Inicio();
            txtRPO.Focus();
        }
        #endregion

        #region regCaptura
        private void txtRPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtRPO.Text))
            {
                //FIND SKU IN DATABASE
                string sModelo = BuscarRPO(txtRPO.Text.ToString());

                if (!string.IsNullOrEmpty(sModelo))
                {
                    txtSKU.Text = sModelo;
                    txtInicio.Focus();
                }

                else
                {
                    txtSKU.Clear();
                    txtSKU.Enabled = true;
                    txtCant.Clear();
                    txtInicio.Clear();
                    txtFin.Clear();

                    txtSKU.Focus();
                }

            }
        }

        private string BuscarRPO(string asRPO)
        {
            string sModelo = "";
            DataTable dtEx = new DataTable();

            try
            {
                if (asRPO.IndexOf("RPO") == -1)
                    asRPO = "RPO" + asRPO;

                int iPos = asRPO.IndexOf("-");
                int iIni = 0;
                int iIni2 = 0;
                if (iPos > 0)
                {
                    string sIni = asRPO.Substring(iPos + 2,5);
                    Int32.TryParse(sIni,out iIni);
                    iIni++;

                    asRPO = asRPO.Substring(0, iPos);
                }

                txtRPO.Text = asRPO;

                RpoLogica rpo = new RpoLogica();
                rpo.RPO = asRPO;
                if(!RpoLogica.Verificar(rpo))
                    MessageBox.Show("No se encontró información del RPO, favor de capturar ó escanear Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    DataTable data = RpoLogica.Consultar(rpo);
                    sModelo = data.Rows[0]["modelo"].ToString();
                    iIni2 = Convert.ToInt32(data.Rows[0]["ult_cons"].ToString());
                    iIni2++;
                    if (iIni2 > iIni)
                        iIni = iIni2;
                    
                    txtInicio.Text = iIni.ToString();

                    txtCant.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error... " + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return sModelo;
        }

        private void txtSKU_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtSKU.Text))
                txtInicio.Focus();
        }

        private void txtInicio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtInicio.Text) && txtInicio.Text != "0")
                btnGenerar.Focus();
        }

        private void txtInicio_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCant.Text))
            {
                if (string.IsNullOrEmpty(txtSKU.Text))
                {
                    if (txtSKU.Enabled)
                        txtSKU.Focus();
                    else
                        txtRPO.Focus();
                }
                else
                    txtCant.Focus();
            }
        }

        private void txtInicio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtCant_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSKU.Text))
            {
                if (txtSKU.Enabled)
                    txtSKU.Focus();
                else
                    txtRPO.Focus();
            }
        }

        private void txtCant_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtCant.Text) && txtCant.Text != "0")
                txtInicio.Focus();
        }

        private void txtCant_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtCant_TextChanged(object sender, EventArgs e)
        {
            int iCant = 0;
            if (int.TryParse(txtCant.Text, out iCant))
            {
                if (iCant <= 0)
                    return;

                int iIni = 0;
                int iFin = 0;
                if (!int.TryParse(txtInicio.Text, out iIni))
                {
                    iIni = 1;
                    txtInicio.Text = "1";
                }

                iCant--;
                iFin = iIni + iCant;
                txtFin.Text = Convert.ToString(iFin);
            }
        }

        private void txtInicio_TextChanged(object sender, EventArgs e)
        {
            int iIni = 0;
            if (int.TryParse(txtInicio.Text, out iIni))
            {
                if (iIni <= 0)
                    return;
            }

            int iCant = 0;
            if (int.TryParse(txtCant.Text, out iCant))
            {
                if (iCant <= 0)
                    return;
            }

            int iFin = 0;
            iCant--;

            iFin = iIni + iCant;
            txtFin.Text = Convert.ToString(iFin);
        }

        #endregion

        #region regGuardar
        private bool Valida()
        {

            int iIni = 0, iFin = 0;

            if (string.IsNullOrEmpty(txtRPO.Text) || string.IsNullOrWhiteSpace(txtRPO.Text))
                return false;

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

            if (!Int32.TryParse(txtInicio.Text, out iIni) && iIni <= 0)
                return false;

            if (!Int32.TryParse(txtFin.Text, out iFin) && iFin <= 0)
                return false;

            if (iIni > iFin)
            {
                MessageBox.Show("El Rango de Consecutivos es Incorrecto", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            string sRPO = txtRPO.Text.ToString();

            int iPos = sRPO.IndexOf("-");
            if (iPos > 0)
            {
                sRPO = sRPO.Substring(0, iPos);
            }
            int iIni = Int32.Parse(txtInicio.Text);
            int iFin = Int32.Parse(txtFin.Text);

            if(!ConfigLogica.VerificaOmiteValidaEti())
            {
                if (!ValidaRangoImp(sRPO, iIni))
                {
                    MessageBox.Show("El rango de impresión ya se ha generado para el mismo RPO anteriormente" + Environment.NewLine + "Consulte el Historial de RPO", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Cursor = Cursors.Arrow;
                    return;
                }
            }
            

            if (sRPO.IndexOf("RPO") != -1)
                sRPO = sRPO.Replace("RPO", "");

            if (ImprimeEtiquetas(sRPO, iIni, iFin))
            {
                this.Cursor = Cursors.WaitCursor;
                if (Guardar(sRPO))
                    EnviarMail();

                this.Cursor = Cursors.Arrow;

                Inicio();

                txtRPO.Focus();
            }
        }
        private bool EnviarMail()
        {

            DataTable dtMail = new DataTable();
            dtMail = CorreoLogica.ConsultarPend();
            if(dtMail.Rows.Count != 0)
            {
                try
                {
                    int iFolio = Convert.ToInt32(dtMail.Rows[0]["folio"].ToString());
                    EnviarCorreo envMail = new EnviarCorreo();
                    envMail.FolioMail = iFolio;
                    envMail.Asunto = dtMail.Rows[0]["asunto"].ToString();

                    string sMsg = EnviarCorreo.EnviaAlerta(envMail);

                    CorreoLogica correo = new CorreoLogica();

                    if (sMsg == "OK")
                    {
                        sMsg = "Se ha Enviado una Notificación por Correo electrónico al Supervisor.";
                        correo.Estado = "E";
                    }
                    else
                        correo.Estado = "R";

                    MessageBox.Show(sMsg, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    correo.Folio = Convert.ToInt32(dtMail.Rows[0]["folio"].ToString());
                    correo.Asunto = dtMail.Rows[0]["asunto"].ToString();
                    correo.Proceso = _lsProceso;
                    CorreoLogica.Guardar(correo);
                }
                catch(Exception ex)
                {
                    throw (ex);
                }
                
            }
            
            return true;
        }
        private bool Guardar(string asRPO)
        {
            try
            {
                bool bReturn;

                if (asRPO.IndexOf("RPO") == -1)
                    asRPO = "RPO" + asRPO;

                EtiquetaLogica eti = new EtiquetaLogica();

                eti.RPO = asRPO;
                eti.Modelo = txtSKU.Text.ToString();
                eti.Cantidad = Convert.ToInt32(txtCant.Text);
                eti.Inicio = Convert.ToInt32(txtInicio.Text);
                eti.Fin = Convert.ToInt32(txtFin.Text);
                eti.Planta = GlobalVar.gsPlanta;
                eti.Estacion = GlobalVar.gsEstacion;
                eti.Usuario = GlobalVar.gsUsuario;
                eti.Tipo = "P";

                if (EtiquetaLogica.Guardar(eti) == 0)
                    bReturn = false;
                else
                    bReturn = true;

                return bReturn;
            }
            catch(Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show("EtiquetaLogica.Guardar(eti)" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        private bool ValidaRangoImp(string asRPO, int aiIni)
        {
            try
            {
                EtiquetaLogica eti = new EtiquetaLogica();
                eti.RPO = asRPO;
                eti.Inicio = aiIni;

                if (EtiquetaLogica.VerificarRango(eti))
                    return false;
                else
                    return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
        }
        private bool ImprimeEtiquetas(string asRPO, int aiIni, int aiFin)
        {
            try
            {
                int iCont = 0;
                iCont = aiFin - aiIni;
                if (iCont < 0)
                    return false;

                iCont++;

                this.Cursor = Cursors.WaitCursor;

                dtsBarcodes ds = new dtsBarcodes();
                DataTable t = ds.Tables.Add("dtBarcodes");
                t.Columns.Add("rpo", Type.GetType("System.String"));
                t.Columns.Add("modelo", Type.GetType("System.String"));
                t.Columns.Add("consec", Type.GetType("System.String"));
                t.Columns.Add("Barcode", Type.GetType("System.Byte[]"));

                BarCodeControl cont = new BarCodeControl();

                cont.Type = BarCodeType.Code128;
                cont.Height = 50;
                cont.ShowText = false;
                cont.ShowCheckSumChar = false;
                cont.Code128SetMode = Code128SetMode.Auto;

                DataRow r;
                for (int i = 0; i < iCont; i++)
                {
                    string sCons = string.Format("{0:D5}", aiIni);
                    string sCode = asRPO + "-M" + sCons;
                    string sLine = sCode + " " + txtSKU.Text.ToString();
                    aiIni++;

                    r = t.NewRow();
                    r["rpo"] = asRPO;
                    r["modelo"] = sLine;
                    r["consec"] = sCons;

                    cont.Data = sCode;
                    BarCodeGenerator bar = new BarCodeGenerator(cont);
                    Image im = bar.GenerateImage();

                    r["Barcode"] = imageToByteArray(im);

                    t.Rows.Add(r);
                }

           
                ReportDocument rptAgenda = new ReportDocument();

                DataTable dtConf = ConfigLogica.Consultar();
                string sDir = dtConf.Rows[0]["directorio"].ToString();
                string sArchivo = sDir + @"\Reportes\ImpCodBarras.rpt";
                rptAgenda.Load(sArchivo);
                rptAgenda.SetDataSource(t);

                PrintDialog dialog = new PrintDialog();
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    rptAgenda.PrintOptions.PrinterName = dialog.PrinterSettings.PrinterName;
                    rptAgenda.PrintToPrinter(1, false, 0, 0);
                    rptAgenda.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
                    this.Cursor = Cursors.Arrow;
                    return true;
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    return false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Arrow;
                return false;
            }
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
                    sDirec += @"\CloverPRO_AyudaVisual_p.pdf";
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
    }
}
