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
    public partial class wfRepUsuarios : Form
    {
        private string _lsProceso = "REP040";
        public bool _lbCambio;
        public wfRepUsuarios()
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
            cbbPlanta.Focus();
        }
        private void Inicio()
        {
            _lbCambio = false;

            cbbPlanta.ResetText();
            DataTable data = PlantaLogica.Listar();
            cbbPlanta.DataSource = data;
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.SelectedIndex = -1;

            chbPlanta.Checked = true;

            if (!string.IsNullOrEmpty(GlobalVar.gsPlanta))
            {
                cbbPlanta.SelectedValue = GlobalVar.gsPlanta;
            }

            chbTurno.Checked = true;
            if (GlobalVar.gsTurno == "2")
            {
                rbtTuno1.Checked = false;
                rbtTuno2.Checked = true;
            }
            else
            {
                rbtTuno1.Checked = true;
                rbtTuno2.Checked = false;
            }

            chbUser.Checked = false;
            rbtEmp.Checked = true;
            rbtNombre.Checked = false;
            txtEmpleado.Clear();

            chbArea.Checked = false;
            cbbArea.ResetText();

            DataTable dtA = AreasLogica.Listar();
            cbbArea.DataSource = dtA;
            cbbArea.DisplayMember = "descrip";
            cbbArea.ValueMember = "area";
            cbbArea.SelectedIndex = 4;

            cbbPlanta.Focus();

        }

       
        #endregion

        #region regBotones
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Inicio();
        }
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private bool Valida()
        {
            bool bValida = false;


            if (chbPlanta.Checked && cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado la planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }


            if (chbUser.Checked && (string.IsNullOrEmpty(txtEmpleado.Text) || string.IsNullOrWhiteSpace(txtEmpleado.Text)))
            {
                MessageBox.Show("No se a especificado el Usuario", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmpleado.Focus();
                return bValida;
            }

            if (chbArea.Checked && cbbArea.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado el Area", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbArea.Focus();
                return bValida;
            }

            return true;

        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            wfImpresor Impresor = new wfImpresor();
            Impresor._lsProceso = _lsProceso;

            string sIndP = "0";
            if (chbPlanta.Checked)
                sIndP = "1";

            Impresor._lsIndPlanta = sIndP;
            if(cbbPlanta.SelectedIndex != -1)
                Impresor._lsPlanta = cbbPlanta.SelectedValue.ToString();

            if (chbTurno.Checked)
                Impresor._lsIndTurno = "1";
            else
                Impresor._lsIndTurno = "0";
            if (rbtTuno1.Checked)
                Impresor._lsTurno = "1";
            else
                Impresor._lsTurno = "2";

            if (chbUser.Checked)
            {
                Impresor._lsIndEmp = "1";
                if(rbtEmp.Checked)
                    Impresor._lsTipoEmp = "E";
                else
                    Impresor._lsTipoEmp = "N";
            }
            else
                Impresor._lsIndEmp = "0";
            if (!string.IsNullOrEmpty(txtEmpleado.Text))
                Impresor._lsEmpleado = txtEmpleado.Text.ToString().ToUpper().TrimEnd();

            if (chbArea.Checked)
                Impresor._lsIndArea = "1";
            else
                Impresor._lsIndArea = "0";
            if (cbbArea.SelectedIndex != -1)
                Impresor._lsArea = cbbArea.SelectedValue.ToString();

            Impresor.Show();
        }

        #endregion

    
        private void cbbPlanta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbbPlanta.Text.Length >= 9)
                e.Handled = true;
        }

       
        private void rbtLineaT_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtEmp.Checked)
            {
                rbtNombre.Checked = false;
            }
        }

        private void rbtLineaU_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtNombre.Checked)
            {
                rbtEmp.Checked = false;
            }
        }


        private void chbPlanta_CheckedChanged(object sender, EventArgs e)
        {
            cbbPlanta.Enabled = chbPlanta.Checked;
        }

        private void rbtTuno1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtTuno1.Checked)
                rbtTuno2.Checked = false;
        }

        private void rbtTuno2_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtTuno2.Checked)
                rbtTuno1.Checked = false;
        }

        
        private void cbbModelo_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbArea, 0);
        }

        private void cbbModelo_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbArea, 1);
        }

        
        private void chbTurno_CheckedChanged(object sender, EventArgs e)
        {
            rbtTuno1.Enabled = chbTurno.Checked;
            rbtTuno2.Enabled = chbTurno.Checked;
        }

        private void chbArea_CheckedChanged(object sender, EventArgs e)
        {
            cbbArea.Enabled = chbArea.Checked;
        }

        private void chbUser_CheckedChanged(object sender, EventArgs e)
        {
            rbtEmp.Enabled = chbUser.Checked;
            rbtNombre.Enabled = chbUser.Checked;
            txtEmpleado.Enabled = chbUser.Checked;
        }

        private void txtEmpleado_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtEmpleado, 0);
        }

        private void txtEmpleado_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtEmpleado, 1);
        }

        private void cbbPlanta_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbPlanta, 0);
        }

        private void cbbPlanta_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbPlanta, 1);
        }
    }
}
