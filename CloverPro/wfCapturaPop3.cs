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
    public partial class wfCapturaPop3 : Form
    {
        public string _lsProceso;
        public string _sClave;
        private bool _bExit;
        public wfCapturaPop3(string asProceso)
        {
            InitializeComponent();
            _lsProceso = asProceso;
        }

        private void wfCapturaPop3_Load(object sender, EventArgs e)
        {
            
        }

      
        private void wfCapturaPop3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!_bExit)
                e.Cancel = true;
        }

        private void rbtEti_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtEti.Checked)
            {
                _sClave = "E";
                _bExit = true;
                Close();
            }
            
        }

        private void rbtCaja_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtCaja.Checked)
            {
                _sClave = "C";
                _bExit = true;
                Close();
            }
            
        }

        private void rbtTest_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtTest.Checked)
            {
                _sClave = "P";
                _bExit = true;
                Close();
            }
            
        }
    }
}
