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
using System.IO;
using Logica;
using Datos;

namespace CloverPro
{
    public partial class wfCodBarrasRec : Form
    {
        private string _lsProceso = "CES010";public wfCodBarrasRec()
        {
            InitializeComponent();
        }

        private void Inicio()
        {
            txtModelo.Clear();
            txtCant.Clear();
            

        }
        private void wfCodBarrasRec_Activated(object sender, EventArgs e)
        {
            txtModelo.Focus();
        }

        private void wfCodBarrasRec_Load(object sender, EventArgs e)
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
            txtModelo.Focus();
        }

        private void txtRPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtModelo.Text))
                txtCant.Focus();
        }
        

        private bool Valida()
        {

            int iIni = 0;

            if (string.IsNullOrEmpty(txtModelo.Text) || string.IsNullOrWhiteSpace(txtModelo.Text))
                return false;

            

            if (!Int32.TryParse(txtCant.Text, out iIni) && iIni <= 0)
                return false;
            

            return true;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            if (ImprimeEtiquetas())
            {
                this.Cursor = Cursors.WaitCursor;
                if (Guardar())
                    EnviarMail();

                this.Cursor = Cursors.Arrow;

                Inicio();

                txtModelo.Focus();
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
        private bool Guardar()
        {
            try
            {
                bool bReturn;

                EtiquetaLogica eti = new EtiquetaLogica();

                eti.Modelo = txtModelo.Text.ToString();
                eti.Cantidad = Convert.ToInt32(txtCant.Text);
                
                eti.Inicio = 0;
                eti.Fin = 0;
                eti.Planta = GlobalVar.gsPlanta;
                eti.Estacion = GlobalVar.gsEstacion;
                eti.Usuario = GlobalVar.gsUsuario;
                eti.Tipo = "R"; //Recepcion [CES]

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
       
        private bool ImprimeEtiquetas()
        {
           
            this.Cursor = Cursors.WaitCursor;

            DataTable dtConf = ConfigLogica.Consultar();

            int iCant = Int32.Parse(txtCant.Text);
            string sModelo = txtModelo.Text.ToString().Trim();

            dtsBarcodes ds = new dtsBarcodes();
            DataTable t = ds.Tables.Add("dtBarcodes");
            t.Columns.Add("fecha", Type.GetType("System.DateTime"));
            t.Columns.Add("modelo", Type.GetType("System.String"));
            t.Columns.Add("cantidad", Type.GetType("System.Int32"));
            t.Columns.Add("Barcode", Type.GetType("System.Byte[]"));
            t.Columns.Add("Barcode2", Type.GetType("System.Byte[]"));
            t.Columns.Add("leyenda", Type.GetType("System.String"));

            DataRow r;
            
            r = t.NewRow();

            r["modelo"] = sModelo;
            r["cantidad"] = iCant;
            r["fecha"] = dtpFecha.Value.ToString();
            r["leyenda"] = dtConf.Rows[0]["leyenda_rec"].ToString();

            BarCodeControl cont = new BarCodeControl();
            cont.Type = BarCodeType.Code128;
            cont.Height = 50;
            cont.ShowText = false;
            cont.ShowCheckSumChar = false;
            cont.Code128SetMode = Code128SetMode.Auto;
            cont.Data = sModelo;
            BarCodeGenerator bar = new BarCodeGenerator(cont);
            Image im = bar.GenerateImage();

            r["Barcode"] = imageToByteArray(im);

            cont.Data = iCant.ToString();
            Image im2 = bar.GenerateImage();

            r["Barcode2"] = imageToByteArray(im2);

            t.Rows.Add(r);
            

            try
            {
                ReportDocument rptAgenda = new ReportDocument();

                string sDir = dtConf.Rows[0]["directorio"].ToString();
                string sArchivo = sDir + @"\Reportes\impCodBarRec.rpt";
                rptAgenda.Load(sArchivo);
                rptAgenda.SetDataSource(t);

                PrintDialog dialog = new PrintDialog();
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    rptAgenda.PrintOptions.PrinterName = dialog.PrinterSettings.PrinterName;
                    rptAgenda.PrintToPrinter(1, false, 0, 0);
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
                this.Cursor = Cursors.Arrow;
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        private void txtCant_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtModelo.Text))
                txtModelo.Focus();
        }

        private void txtCant_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtCant.Text) && txtCant.Text != "0")
                btnGenerar.Focus();
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
            
        }

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
                    sDirec += @"\CloverPRO_AyudaVisual_c.pdf";
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
    }
}
