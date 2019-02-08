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
    public partial class wfRepGlobalRPO : Form
    {
        private string _lsProceso = "REP120";
        public bool _lbCambio;
        public wfRepGlobalRPO()
        {
            InitializeComponent();
        }

        #region regInicio
        private void wfRepEtiquetas_Load(object sender, EventArgs e)
        {
            Inicio();
            
        }
        private void wfRepEtiquetas_Activated(object sender, EventArgs e)
        {
            btPrint.Focus();
        }
        private void Inicio()
        {
            _lbCambio = false;

           

            dtpInicio.ResetText();
            dtpFinal.ResetText();
            

            chbModelo.Checked = false;
            txtModelo.Enabled = false;
            txtModelo.Clear();

            chbRPO.Checked = false;
            txtRPO.Enabled = false;
            txtRPO.Clear();

            chbTO.Checked = false;
            txtTO.Enabled = false;
            txtTO.Clear();

            btPrint.Focus();

        }

       
        #endregion

        private bool Valida()
        {
            bool bValida = false;

            if (string.IsNullOrEmpty(dtpInicio.Text) || string.IsNullOrEmpty(dtpFinal.Text))
            {
                MessageBox.Show("Favor de especificar el Periódo del reporte", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpInicio.Focus();
                return bValida;
            }
            else
            {
                if(dtpInicio.Value > dtpFinal.Value)
                {
                    MessageBox.Show("La Fecha Inicial no debe ser mayor a la Fecha Final", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtpFinal.Focus();
                    return bValida;
                }
            }

            if (chbModelo.Checked && string.IsNullOrEmpty(txtModelo.Text))
            {
                MessageBox.Show("No se a especificado el Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtModelo.Focus();
                return bValida;
            }

            if (chbRPO.Checked && string.IsNullOrEmpty(txtRPO.Text))
            {
                MessageBox.Show("No se a especificado el RPO", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRPO.Focus();
                return bValida;
            }

            if (chbTO.Checked && string.IsNullOrEmpty(txtTO.Text))
            {
                MessageBox.Show("No se a especificado el T.O.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTO.Focus();
                return bValida;
            }


            return true;

        }



        #region regBotones
        private void btPrint_Click(object sender, EventArgs e)
        {
            btnPrint_Click(sender, e);
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Inicio();
        }
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            wfImpresor Impresor = new wfImpresor();
            Impresor._lsProceso = _lsProceso;

           
            Impresor._ldtFinal = dtpFinal.Value;
            Impresor._ldtInicio = dtpInicio.Value;
            
            if (chbModelo.Checked)
            {
                Impresor._lsIndModelo = "1";
                Impresor._lsModelo = txtModelo.Text.ToString();
            }
            else
                Impresor._lsIndModelo = "0";

            if (chbRPO.Checked)
            {
                Impresor._lsIndRPO = "1";
                Impresor._lsRPO = txtRPO.Text.ToString();
            }
            else
                Impresor._lsIndRPO = "0";

            if (chbTO.Checked)
            {
                Impresor._lsIndTurno = "1";
                Impresor._lsTurno = txtTO.Text.ToString();
            }
            else
                Impresor._lsIndTurno = "0";

            if (chbSolos.Checked)
                Impresor._lsIndEst = "1";
            else
                Impresor._lsIndEst = "0";

            Impresor.Show();
        }

        #endregion

        #region regCaptura
        private void dtpInicio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
       
        private void chbModelo_CheckedChanged(object sender, EventArgs e)
        {
            if (chbModelo.Checked)
                txtModelo.Enabled = true;
            else
                txtModelo.Enabled = false;
        }

        private void chbRPO_CheckedChanged(object sender, EventArgs e)
        {
            if (chbRPO.Checked)
                txtRPO.Enabled = true;
            else
                txtRPO.Enabled = false;
        }

        private void txtFolioIni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtFolioFin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

      
        private void txtRPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtRPO.Text))
            {
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
            }
        }
        private void txtRPO_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRPO, 0);
        }

        private void txtRPO_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRPO, 1);
        }

        private void chbTO_CheckedChanged(object sender, EventArgs e)
        {

            txtTO.Enabled = chbTO.Checked;
        }

        private void txtModelo_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtModelo, 0);
        }

        private void txtModelo_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtModelo, 1);
        }

        private void txtTO_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtTO, 0);
        }

        private void txtTO_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtTO, 1);
        }

        #endregion

        
    }
}
