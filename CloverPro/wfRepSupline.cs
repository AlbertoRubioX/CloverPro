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
    public partial class wfRepSupline : Form
    {
        private string _lsProceso = "REP050";
        public bool _lbCambio;
        public wfRepSupline()
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

            rbtPlantaT.Checked = true;
            rbtPlantaU.Checked = false;
            cbbPlanta.Visible = false;

            if (!string.IsNullOrEmpty(GlobalVar.gsPlanta))
            {
                cbbPlanta.SelectedValue = GlobalVar.gsPlanta;
            }

            if (GlobalVar.gsTurno == "2")
            {
                chbTurno1.Checked = false;
                chbTurno2.Checked = true;
            }
            else
            {
                chbTurno1.Checked = true;
                chbTurno2.Checked = false;
            }


            rbtSupT.Checked = true;
            rbtSupA.Checked = false;
            cbbSupervisor.Visible = false;

            cbbSupervisor.ResetText();

            DataTable dt = UsuarioLogica.ListarSuper();
            cbbSupervisor.DataSource = dt;
            cbbSupervisor.ValueMember = "usuario";
            cbbSupervisor.DisplayMember = "nombre";
            cbbSupervisor.SelectedIndex = -1;

            rbtLineaT.Checked = true;
            rbtLineaU.Checked = false;
            rbtLineaA.Checked = false;
            
            cbbLineaIni.Visible = false;
            lblLFin.Visible = false;
            cbbLineaFin.Visible = false;


            CargarLineas();

            cbbPlanta.Focus();

        }

        private void CargarLineas()
        {
            DataTable dtL1 = new DataTable();
            LineaLogica line = new LineaLogica();
            if (rbtPlantaU.Checked && cbbPlanta.SelectedIndex != -1)
            {
                line.Planta = cbbPlanta.SelectedValue.ToString();
                dtL1 = LineaLogica.LineaPlanta(line);

                string sLineas = string.Empty;
                for (int x = 0; x < dtL1.Rows.Count; x++)
                {
                    string sLine = dtL1.Rows[x][1].ToString();
                    if (string.IsNullOrEmpty(sLineas))
                        sLineas = sLine;
                    else
                        sLineas += "," + sLine;
                }

                if (!string.IsNullOrEmpty(sLineas))
                    txtLineas.Text = sLineas;
                else
                    txtLineas.Clear();
            }
            else
                dtL1 = LineaLogica.ListarTodas();
                
            
            cbbLineaIni.DataSource = dtL1;
            cbbLineaIni.ValueMember = "linea";
            cbbLineaIni.DisplayMember = "linea";
            cbbLineaIni.SelectedIndex = -1;
        }
        #endregion

        private bool Valida()
        {
            bool bValida = false;

            if (rbtPlantaU.Checked && cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado la planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }

            if (rbtLineaU.Checked && cbbLineaIni.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de espeficicar la Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbLineaIni.Focus();
                return bValida;
            }

            if (rbtLineaA.Checked && string.IsNullOrEmpty(txtLineas.Text))
            {
                MessageBox.Show("Favor de espeficicar las Lineas", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLineas.Focus();
                return bValida;
            }


            if (!chbTurno1.Checked && !chbTurno2.Checked)
            {
                MessageBox.Show("No se a especificado el Turno", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                chbTurno1.Focus();
                return bValida;
            }

            if (rbtSupA.Checked && cbbSupervisor.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado el Supervisor", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbSupervisor.Focus();
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

            Impresor._lsIndPlanta = sIndP;
            if(cbbPlanta.SelectedIndex != -1)
                Impresor._lsPlanta = cbbPlanta.SelectedValue.ToString();

            Impresor._lsIndLinea = sIndL;
            if (chbTurno1.Checked && chbTurno2.Checked)
                Impresor._lsTurno = "T";
            else
            {
                if (chbTurno1.Checked)
                    Impresor._lsTurno = "1";
                else
                    Impresor._lsTurno = "2";
            }
                
            if (cbbLineaIni.SelectedIndex != -1)
                Impresor._lsLineaIni = cbbLineaIni.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(txtLineas.Text))
                Impresor._lsLineas = "," + txtLineas.Text.ToString() + ",";

            if (rbtSupT.Checked)
                Impresor._lsIndEmp = "T";
            if (rbtSupA.Checked)
            {
                Impresor._lsIndEmp = "U";
                Impresor._lsEmpleado = cbbSupervisor.SelectedValue.ToString();
            }


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
            
                cbbPlanta.Visible = false;

                CargarLineas();
            }
        }

        private void rbtPlantaU_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtPlantaU.Checked)
            {
                rbtPlantaT.Checked = false;
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

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CargarLineas();
        }

        private void rbtSupT_CheckedChanged(object sender, EventArgs e)
        {
            cbbSupervisor.Visible = false;
            rbtSupA.Checked = false;
        }

        private void rbtSupA_CheckedChanged(object sender, EventArgs e)
        {
            cbbSupervisor.Visible = true;
            rbtSupT.Checked = false;
        }
    }
}
