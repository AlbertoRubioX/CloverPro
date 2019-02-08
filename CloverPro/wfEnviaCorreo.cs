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
    public partial class wfEnviaCorreo : Form
    {
        public wfEnviaCorreo()
        {
            InitializeComponent();
        }

        private void wfEnviaCorreo_Load(object sender, EventArgs e)
        {
            txtMensaje.Text = "Este correo es una prueba de Configuración, favor de no responder.";

            DataTable dtUser = new DataTable();
            ConfigLogica conf = new ConfigLogica();

            dtUser = ConfigLogica.Consultar();
            if (dtUser.Rows.Count > 0)
            {
                txtDestino.Text = dtUser.Rows[0]["correo_dest"].ToString();
            }
        }

        private bool Valida()
        {
            bool bValida = false;

            if (string.IsNullOrEmpty(txtDestino.Text) || string.IsNullOrWhiteSpace(txtDestino.Text))
            {
                MessageBox.Show("Favor de especificar el Destinatario", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDestino.Focus();
                return bValida;
            }

            if (string.IsNullOrEmpty(txtMensaje.Text) || string.IsNullOrWhiteSpace(txtMensaje.Text))
            {
                DialogResult Result = MessageBox.Show("Desea enviar el Correo sin mensaje?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                    return true;
                else
                {
                    txtMensaje.Focus();
                    return bValida;
                }
            }

            return true;
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (Valida())
            {
                Cursor.Current = Cursors.WaitCursor;

                EnviarCorreo envMail = new EnviarCorreo();
                envMail.Para = txtDestino.Text.ToString();
                envMail.Asunto = "CloverPro - Confirmación de Correo Electrónico";
                envMail.Mensaje = txtMensaje.Text.ToString();

                string sMsg = EnviarCorreo.Enviar(envMail);
                if (sMsg == "OK")
                    sMsg = "Correo enviado exitosamente";
                MessageBox.Show(sMsg, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                Cursor.Current = Cursors.Default;
                Close();

            }
        }
    }
}
