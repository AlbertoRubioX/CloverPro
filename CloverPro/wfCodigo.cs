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
    public partial class wfCodigo : Form
    {
        public string _sCodigo;
        public wfCodigo()
        {
            InitializeComponent();
        }

        private void txtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _sCodigo = null;
                Close();
            }

            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtClave.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
                return;

            _sCodigo = txtClave.Text.ToString().Trim();
            if (_sCodigo.Length > 4)
                _sCodigo = _sCodigo.Substring(0, 4);

            if (_sCodigo.Length < 4)
                _sCodigo = _sCodigo.PadLeft(4, '0');

            if (!ConfigLogica.VerificaCodigoBloq(_sCodigo))
            {
                MessageBox.Show("El Código de desbloqueo no se encuentra registrado" + Environment.NewLine + "Favor de solicitar soporte al departamento de IT", "CloverPro | Sistema de Producción Local", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtClave.Clear();
                return;
            }
            else
                Close();
        }
    }
}
