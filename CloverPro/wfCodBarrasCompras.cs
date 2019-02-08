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
    public partial class wfCodBarrasCompras : Form
    {
        private string _lsProceso = "EMP020";
        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;
        public wfCodBarrasCompras()
        {
            InitializeComponent();
            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }

        private void Inicio()
        {
            lblFecha.Text = Convert.ToString(DateTime.Now);
            lblLinea.Text = "";
            txtRPO.Clear();
            txtLote.Clear();
            txtModelo.Clear();

            EstacionLogica est = new EstacionLogica();
            est.Estacion = GlobalVar.gsEstacion;
            DataTable data = EstacionLogica.Consultar(est);
            if(data.Rows.Count != 0)
            {
                string sPlanta = data.Rows[0]["planta"].ToString();
                sslLinea.Text = data.Rows[0]["linea"].ToString();
                lblLinea.Text += data.Rows[0]["linea"].ToString();
                sslEstacion.Text = data.Rows[0]["estacion"].ToString();

                PlantaLogica pla = new PlantaLogica();
                pla.Planta = sPlanta;
                DataTable dt = PlantaLogica.Consultar(pla);
                sslPlanta.Text = dt.Rows[0]["nombre"].ToString();
            }

            txtRPO.Focus();

            
        }
        private void wfCodBarrasCompras_Activated(object sender, EventArgs e)
        {
            txtModelo.Focus();
        }

        private void wfCodBarrasCompras_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            Inicio();
            txtModelo.Focus();
        }

        private void txtModelo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtModelo.Text) || string.IsNullOrWhiteSpace(txtModelo.Text))
                return;

            if (!string.IsNullOrEmpty(txtModelo.Text))
                txtRPO.Focus();
        }

        private void txtModeloNext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtModelo.Text) || string.IsNullOrWhiteSpace(txtModelo.Text))
                return;

            if (string.IsNullOrEmpty(txtRPO.Text) || string.IsNullOrWhiteSpace(txtRPO.Text))
                return;

          
            string sRPO = txtRPO.Text.ToString().Trim();
            if (sRPO.IndexOf("RPO") == -1)
                sRPO = "RPO" + sRPO;

            int iPos = sRPO.IndexOf("-");
            int iIni = 0;
            if (iPos > 0)
            {
                string sIni = sRPO.Substring(iPos + 2, 5);
                Int32.TryParse(sIni, out iIni);
                iIni++;

                sRPO = sRPO.Substring(0, iPos);
            }

            txtRPO.Text = sRPO;

            if (ConfigLogica.VerificaRpoOrbis())
            {
                RpoLogica rpo = new RpoLogica();
                rpo.RPO = sRPO;
                DataTable dtRpo = RpoLogica.ConsultarOrbis(rpo);
                if (dtRpo.Rows.Count != 0)
                {
                    string sModelo = dtRpo.Rows[0]["Producto"].ToString();
                    string sEstd = dtRpo.Rows[0]["Estandar"].ToString();
                    string sLinea = dtRpo.Rows[0]["Linea"].ToString();

                    txtLote.Text = dtRpo.Rows[0]["cantidadProducir"].ToString();
                    
                }
            }



            txtRPO.Focus();
        }

        private bool Valida()
        {
            if (txtModelo.Text.ToString().ToUpper().TrimEnd() != txtRPO.Text.ToString().ToUpper().TrimEnd())
            {
                txtModelo.Focus();
                return false;
            }

            if(string.IsNullOrEmpty(txtRPO.Text))
            {
                MessageBox.Show("El RPO no se ha registrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRPO.Focus();
                return false;
            }

            if(string.IsNullOrEmpty(txtLote.Text))
            {
                MessageBox.Show("Favor de especificar la Cantidad del Lote", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLote.Focus();
                return false;
            }

           return true;
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
                    
                    CorreoLogica.Guardar(correo);
                }
                catch(Exception ex)
                {
                    throw (ex);
                }
                
            }
            
            return true;
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

        private void txtModelo_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtModelo, 0);
        }

        private void txtModelo_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtModelo.Text) && string.IsNullOrEmpty(txtRPO.Text))
                txtRPO.Focus();
            else
                GlobalVar.CambiaColor(txtModelo, 1);
        }

        private void txtModeloNext_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRPO, 0);
        }

        private void txtModeloNext_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtModelo.Text))
                txtModelo.Focus();
            else
                GlobalVar.CambiaColor(txtRPO, 1);
        }

        private void wfCodBarrasCompras_Resize(object sender, EventArgs e)
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

        private void txtLote_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtLote, 1);
        }

        private void txtLote_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtLote, 0);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;


        }
    }
}
