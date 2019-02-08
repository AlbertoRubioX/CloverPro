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
    public partial class wfLogin : Form
    {
        public string _sUsuario;
        public wfLogin()
        {
            InitializeComponent();
        }

        private void txtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _sUsuario = null;
                Close();
            }

            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtClave.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
                return;

            _sUsuario = txtClave.Text.ToString().Trim();
            if (_sUsuario != "ADMINP" && _sUsuario.IndexOf("A") > 0) //029621A
                _sUsuario = _sUsuario.Substring(0, 6);

            if (_sUsuario.Length < 6)
                _sUsuario = _sUsuario.PadLeft(6, '0');

            if (!AccesoLogica.verificaAcceso(_sUsuario))
            {
                MessageBox.Show("El Usuario no se encuentra registrado" + Environment.NewLine + "Favor de solicitar el registro del usuario nuevo al departamento de IT", "CloverPro | Sistema de Producción Local", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtClave.Clear();
                return;
            }
            else
                Close();
        }

        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtClave_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
