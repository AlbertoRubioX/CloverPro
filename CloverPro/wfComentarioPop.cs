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
    public partial class wfComentarioPop : Form
    {
        public string _lsProceso;
        public string _sClave;
        private long _lFolio;
        private int _iConsec;
        public wfComentarioPop(string asProceso)
        {
            InitializeComponent();
            _sClave = asProceso;
        }
        private void wfComentarioPop_Load(object sender, EventArgs e)
        {
            txtClave.Clear();

            if (!string.IsNullOrEmpty(_sClave))
            {
                if (_sClave.IndexOf('-') != -1)
                {
                    int iPos = _sClave.IndexOf('-');
                    _lFolio = long.Parse(_sClave.Substring(0, iPos));

                    int iPos2 = _sClave.IndexOf('*');//18-1*NOTA
                    if (iPos2 != -1)
                    {
                        _iConsec = int.Parse(_sClave.Substring(iPos + 1, iPos2 - (iPos + 1)));
                        if (_sClave.Length > iPos2)
                            _sClave = _sClave.Substring(iPos2 + 1);
                        else
                            _sClave = string.Empty;
                    }
                    else
                    {
                        _iConsec = int.Parse(_sClave.Substring(iPos + 1));
                        _sClave = string.Empty;
                    }
                }
                else
                {
                    btnSave.Visible = false;
                    this.Height = this.Height - (int)(this.Height * .25);
                }

                if(_sClave == "CANCEL")
                {
                    this.Height = this.Height + (int)(this.Height * .1);
                    lblCancel.Visible = true;
                }
                else
                    txtClave.Text = _sClave;
            }
            else
            {
                btnSave.Visible = false;
                this.Height = this.Height - (int)(this.Height * .25);
            }
        }
        private void txtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (string.IsNullOrEmpty(txtClave.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
                    _sClave = null;
                else
                    _sClave = txtClave.Text.ToString();

                Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _sClave = txtClave.Text.ToString();
                LineSetupDetLogica det = new LineSetupDetLogica();
                det.Folio = _lFolio;
                det.Consec = _iConsec;
                det.Comentario = _sClave;
                det.Usuario = GlobalVar.gsUsuario;

                LineSetupDetLogica.ActualizaComent(det);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador " + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            Close();
        }
    }
}
