using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Logica;

namespace CloverPro
{
    public partial class wfCapturaPop2 : Form
    {
        public string _lsProceso;
        public string _sLinea;
        public string _sModOrig;
        public string _sModelo;
        public string _sRPO;
        private bool _bExit;
        public wfCapturaPop2(string asProceso)
        {
            InitializeComponent();
            _lsProceso = asProceso;
        }


        private void wfCapturaPop2_Load(object sender, EventArgs e)
        {

            WindowState = FormWindowState.Maximized;
            
        }

       
        private void wfCapturaPop2_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void wfCapturaPop2_SizeChanged(object sender, EventArgs e)
        {
            int iW = this.Width/2;
            int iH = this.Height/2;

            int iW2 = panel2.Width/2;
            int iH2 = panel2.Height;

            int iX = iW - iW2;
            int iY = iH - iH2;

            panel2.Location = new Point(iX, iY);
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtCodigo.Text))
                return;

            if (ConfigLogica.VerificaCodigoEmp(txtCodigo.Text.ToString().ToUpper().TrimEnd()))
            {
                _bExit = true;
                Close();
            }
                
            else
                txtCodigo.Clear();
           
        }

        private void wfCapturaPop2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!_bExit)
                e.Cancel = true;
        }
    }
}
