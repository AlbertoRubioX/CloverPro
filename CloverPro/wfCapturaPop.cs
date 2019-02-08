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
    public partial class wfCapturaPop : Form
    {
        public string _lsProceso;
        public string _sClave;
        public wfCapturaPop(string asProceso)
        {
            InitializeComponent();
            _lsProceso = asProceso;
        }

        private void txtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _sClave = null;
                Close();
            }

            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtClave.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
                return;

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

            Close();
        }

        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void wfCapturaPop_Load(object sender, EventArgs e)
        {
            if(_lsProceso == "CAT050")
            {
                label1.Text = "FORMATO ESTANDAR";
                cbbClave.Visible = false;
                txtClave.Visible = true;
            }

            if(_lsProceso == "PRO050")
            {
                if(_sClave == "Controlada")
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
                    label1.Text = "NOMBRE DE LA ESTACION";
                
            }

            if (_lsProceso == "PRO060")
            {
                label1.Text = "CAPTURE EL NUEVO RPO";
            }

            if(_lsProceso == "EMP020")
            {
                label1.Text = "VENDOR BARCODE";
                cbbClave.Visible = false;
                txtClave.Visible = true;
            }
        }

        private void cbbClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _sClave = null;
                Close();
            }

        }

        private void cbbClave_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _sClave = cbbClave.SelectedValue.ToString();
            Close();
        }
    }
}
