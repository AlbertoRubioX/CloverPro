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
    public partial class wfVersionPop : Form
    {
        public string _lsProceso;
        public string _sLinea;
        public string _sModOrig;
        public string _sModelo;
        public string _sRPO;
        private bool _bExit;
        public wfVersionPop()
        {
            InitializeComponent();
            
        }


        private void wfCapturaPop2_Load(object sender, EventArgs e)
        {
            
        }

       
        private void wfCapturaPop2_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void wfCapturaPop2_SizeChanged(object sender, EventArgs e)
        {
           
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode != Keys.ControlKey)
            //    return;

            //if (string.IsNullOrEmpty(txtCodigo.Text.ToString()))
            //    return;

            if(txtCodigo.Text.ToString().ToUpper() == "X")
            {
                _bExit = true;
                Close();
            }
            
            
           
        }

        private void wfCapturaPop2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!_bExit)
                e.Cancel = true;
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
