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
    public partial class wfRepEmpaqueProc : Form
    {
        private string _lsProceso = "REP110";
        public bool _lbCambio;
        public wfRepEmpaqueProc()
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
            dtpInicio.Focus();
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

            rbtPlantaT.Checked = true;
            rbtPlantaU.Checked = false;
            lblPlanta.Visible = false;
            cbbPlanta.Visible = false;

            //if (GlobalVar.gsArea == "SUP" && !string.IsNullOrEmpty(GlobalVar.gsPlanta))
            //{
            //    rbtPlantaU.Checked = true;
            //    cbbPlanta.SelectedValue = GlobalVar.gsPlanta;
            //}

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


            dtpInicio.ResetText();
            dtpFinal.ResetText();

            //rbtLineaT.Checked = false;
            //rbtLineaU.Checked = false;
            
       
            cbbLineaIni.Visible = false;
                     
            chbRPO.Checked = false;
            txtRPO.Enabled = false;

            chbEstatus.Checked = false;
            cbbEstatus.Enabled = false;
            Dictionary<string, string> Est = new Dictionary<string, string>();
            Est.Add("L", "LISTO");
            Est.Add("D", "DETENIDO");
            Est.Add("E", "EN ESPERA");
            Est.Add("P", "EN PROCESO");
            cbbEstatus.DataSource = new BindingSource(Est, null);
            cbbEstatus.DisplayMember = "Value";
            cbbEstatus.ValueMember = "Key";
            cbbEstatus.SelectedIndex = -1;

            CargarLineas();

            dtpInicio.Focus();

        }

        private void CargarLineas()
        {
            DataTable dtL1 = new DataTable();
            DataTable dtL2 = new DataTable();
            if (rbtPlantaU.Checked && cbbPlanta.SelectedIndex != -1)
            {
                LineaLogica line = new LineaLogica();
                line.Planta = cbbPlanta.SelectedValue.ToString();
                dtL1 = LineaLogica.LineaPlanta(line);
                dtL2 = LineaLogica.LineaPlanta(line);
            }
            else
            {
                dtL1 = LineaLogica.ListarTodas();
                dtL2 = LineaLogica.ListarTodas();
            }
                

            cbbLineaIni.DataSource = dtL1;
            cbbLineaIni.ValueMember = "linea";
            cbbLineaIni.DisplayMember = "linea";
            cbbLineaIni.SelectedIndex = -1;

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

            if (rbtPlantaU.Checked && cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado la planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }

            

            if (rbtLineaU.Checked && cbbLineaIni.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de especificar la Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return bValida;
            }

            if (chbRPO.Checked && string.IsNullOrEmpty(txtRPO.Text))
            {
                MessageBox.Show("No se a especificado el Código del Proveedor", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRPO.Focus();
                return bValida;
            }

            return true;

        }



        #region regBotones
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

            string sIndP = "0";
            if (rbtPlantaU.Checked)
                sIndP = "1";

          
            string sIndL = "0";
            if (rbtLineaU.Checked)
                sIndL = "1";
            

            Impresor._ldtFinal = dtpFinal.Value;
            Impresor._ldtInicio = dtpInicio.Value;
            Impresor._lsIndPlanta = sIndP;
            if(cbbPlanta.SelectedIndex != -1)
                Impresor._lsPlanta = cbbPlanta.SelectedValue.ToString();

            Impresor._lsIndLinea = sIndL;
            if (rbtAmbos.Checked)
                Impresor._lsTurno = "A";
            else
            {
                if (rbtTuno1.Checked)
                    Impresor._lsTurno = "1";
                else
                    Impresor._lsTurno = "2";
            }

            if (cbbLineaIni.SelectedIndex != -1)
                Impresor._lsLineaIni = cbbLineaIni.SelectedValue.ToString();
 
           
            if (chbRPO.Checked)
            {
                Impresor._lsIndRPO = "1";
                Impresor._lsRPO = txtRPO.Text.ToString();
            }
            else
                Impresor._lsIndRPO = "0";

            if (chbEstatus.Checked)
            {
                Impresor._lsIndEst = "1";
                Impresor._lsEstatus = cbbEstatus.SelectedValue.ToString();
            }
            else
                Impresor._lsIndEst = "0";
            Impresor._lsTipoEmp = "A";
            if (chbPrio.Checked)
                Impresor._lsTipoEmp = "P";
            

            Impresor.Show();
        }

        #endregion

    
        private void dtpInicio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
        private void cbbPlanta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbbPlanta.Text.Length >= 9)
                e.Handled = true;
        }

        

        private void rbtPlantaT_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtPlantaT.Checked)
            {
                rbtPlantaU.Checked = false;
                lblPlanta.Visible = false;
                cbbPlanta.Visible = false;

                CargarLineas();
            }
        }

        private void rbtPlantaU_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtPlantaU.Checked)
            {
                rbtPlantaT.Checked = false;
                lblPlanta.Visible = true;
                cbbPlanta.Visible = true;

                CargarLineas();
            }
        }
       
        private void rbtLineaT_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtLineaT.Checked)
            {
                rbtLineaU.Checked = false;
                cbbLineaIni.Visible = false;
                
            }
        }

        private void rbtLineaU_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtLineaU.Checked)
            {
                rbtLineaT.Checked = false;
                cbbLineaIni.Visible = true;
                
            }
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

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CargarLineas();
        }

        private void txtRPO_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void rbtTuno1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtTuno1.Checked)
            {
                rbtTuno2.Checked = false;
                rbtAmbos.Checked = false;
            }
                
        }

        private void rbtTuno2_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtTuno2.Checked)
            {
                rbtTuno1.Checked = false;
                rbtAmbos.Checked = false;
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

        private void chbEstatus_CheckedChanged(object sender, EventArgs e)
        {
            cbbEstatus.Enabled = chbEstatus.Checked;
        }

        private void rbtAmbos_CheckedChanged(object sender, EventArgs e)
        {
            rbtTuno1.Checked = !rbtAmbos.Checked;
            rbtTuno2.Checked = !rbtAmbos.Checked;
        }
    }
}
