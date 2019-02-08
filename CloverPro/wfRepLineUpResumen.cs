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
    public partial class wfRepLineUpResumen : Form
    {
        private string _lsProceso = "REP070";
        public bool _lbCambio;
        public wfRepLineUpResumen()
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

            if (GlobalVar.gsArea == "SUP" && !string.IsNullOrEmpty(GlobalVar.gsPlanta))
            {
                rbtPlantaU.Checked = true;
                cbbPlanta.SelectedValue = GlobalVar.gsPlanta;
            }

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

            rbtLineaT.Checked = true;
            rbtLineaU.Checked = false;
            rbtLineaA.Checked = false;

            txtLineas.Visible = false;
            lblLIni.Visible = false;
            cbbLineaIni.Visible = false;
            lblLFin.Visible = false;
            cbbLineaFin.Visible = false;
            

            chbModelo.Checked = false;
            cbbModelo.Enabled = false;
            cbbModelo.DataSource = ModeloLogica.Listar();
            cbbModelo.ValueMember = "modelo";
            cbbModelo.DisplayMember = "modelo";
            cbbModelo.SelectedIndex = -1;

            chbRPO.Checked = false;
            txtRPO.Enabled = false;

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

            cbbLineaFin.DataSource = dtL2;
            cbbLineaFin.ValueMember = "linea";
            cbbLineaFin.DisplayMember = "linea";
            cbbLineaFin.SelectedIndex = -1;

            SuperLineaLogica sup = new SuperLineaLogica();
            sup.Supervisor = GlobalVar.gsUsuario;
            DataTable dtLs = SuperLineaLogica.Listar(sup);
            string sLineas = string.Empty;
            for(int x = 0; x < dtLs.Rows.Count; x++)
            {
                string sLine = dtLs.Rows[x][3].ToString();
                if (string.IsNullOrEmpty(sLineas))
                    sLineas = sLine;
                else
                    sLineas += "," + sLine;
            }

            if (!string.IsNullOrEmpty(sLineas))
                txtLineas.Text = sLineas;
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
                MessageBox.Show("Favor de espeficicar la Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return bValida;
            }

            if (rbtLineaA.Checked && string.IsNullOrEmpty(txtLineas.Text))
            {
                MessageBox.Show("Favor de espeficicar las Lineas", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return bValida;
            }


            if (chbModelo.Checked && cbbModelo.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado el Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbModelo.Focus();
                return bValida;
            }

            if (chbRPO.Checked && string.IsNullOrEmpty(txtRPO.Text))
            {
                MessageBox.Show("No se a especificado el RPO", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            string sIndP = "T";
            if (rbtPlantaU.Checked)
                sIndP = "U";

          
            string sIndL = "T";
            if (rbtLineaA.Checked)
                sIndL = "A";
            if (rbtLineaU.Checked)
                sIndL = "U";

            Impresor._ldtFinal = dtpFinal.Value;
            Impresor._ldtInicio = dtpInicio.Value;
            Impresor._lsIndPlanta = sIndP;
            if(cbbPlanta.SelectedIndex != -1)
                Impresor._lsPlanta = cbbPlanta.SelectedValue.ToString();

            Impresor._lsIndLinea = sIndL;
            if (rbtTuno1.Checked)
                Impresor._lsTurno = "1";
            else
                Impresor._lsTurno = "2";
            if (cbbLineaIni.SelectedIndex != -1)
                Impresor._lsLineaIni = cbbLineaIni.SelectedValue.ToString();
 
            if (!string.IsNullOrEmpty(txtLineas.Text))
                Impresor._lsLineas = "," + txtLineas.Text.ToString() + ",";
            if (chbModelo.Checked)
            {
                Impresor._lsIndModelo = "1";
                Impresor._lsModelo = cbbModelo.SelectedValue.ToString();
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
            if (chbSolos.Checked)
                Impresor._lsIndSolo = "1";
            else
                Impresor._lsIndSolo = "0";

            Impresor.Show();
        }

        #endregion

        #region regCaptura
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
                rbtLineaA.Checked = false;
                lblLIni.Visible = false;
                cbbLineaIni.Visible = false;
                lblLFin.Visible = false;
                cbbLineaFin.Visible = false;
                txtLineas.Visible = false;
            }
        }

        private void rbtLineaU_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtLineaU.Checked)
            {
                rbtLineaT.Checked = false;
                rbtLineaA.Checked = false;
                lblLIni.Text = "Linea:";
                lblLIni.Visible = true;
                cbbLineaIni.Visible = true;
                lblLFin.Visible = false;
                cbbLineaFin.Visible = false;
                txtLineas.Visible = false;
            }
        }

        private void rbtLineaA_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtLineaA.Checked)
            {
                rbtLineaT.Checked = false;
                rbtLineaU.Checked = false;
                lblLIni.Text = "Lineas:";
                lblLIni.Visible = true;
                cbbLineaIni.Visible = false;
                lblLFin.Visible = false;
                cbbLineaFin.Visible = false;
                txtLineas.Visible = true;
            }
        }

        private void chbModelo_CheckedChanged(object sender, EventArgs e)
        {
            if (chbModelo.Checked)
                cbbModelo.Enabled = true;
            else
                cbbModelo.Enabled = false;
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

        private void cbbModelo_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(cbbModelo.Text) && !string.IsNullOrWhiteSpace(cbbModelo.Text))
            {
                ModeloLogica mod = new ModeloLogica();
                string sMod = cbbModelo.Text.ToUpper().Trim().ToString();


                mod.Modelo = sMod;
                DataTable datos = ModeloLogica.Consultar(mod);
                if (datos.Rows.Count != 0)
                {
                    cbbModelo.SelectedValue = datos.Rows[0]["modelo"].ToString();
                }
                else
                {
                    MessageBox.Show("El Modelo no se encuentra en el listado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
                cbbModelo.SelectedIndex = -1;
        }

        private void cbbModelo_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbModelo, 0);
        }

        private void cbbModelo_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbModelo, 1);
        }

        private void txtRPO_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRPO, 0);
        }

        private void txtRPO_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRPO, 1);
        }
        
        #endregion
    }
}
