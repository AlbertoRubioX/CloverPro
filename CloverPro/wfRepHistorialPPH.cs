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
    public partial class wfRepHistorialPPH : Form
    {
        private string _lsProceso = "REP090";
        public bool _lbCambio;
        public wfRepHistorialPPH()
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

            cbbNivel.ResetText();
            Dictionary<string, string> Nivel = new Dictionary<string, string>();
            Nivel.Add("GUIA", "Guia");
            Nivel.Add("I", "I");
            Nivel.Add("II", "II");
            Nivel.Add("III", "III");
            Nivel.Add("IIIQ", "III Q");
            Nivel.Add("IV", "IV");
            Nivel.Add("IVQ", "IV Q");
            Nivel.Add("V", "V");
            Nivel.Add("NI", "Nuevo Ingreso");
            Nivel.Add("AUD", "Auditor Especializado");
            Nivel.Add("OPG", "Op. General");
            Nivel.Add("MAT", "Materialista");
            Nivel.Add("COM", "Componentes");
            Nivel.Add("TEM", "Temporal");
            cbbNivel.DataSource = new BindingSource(Nivel, null);
            cbbNivel.DisplayMember = "Value";
            cbbNivel.ValueMember = "Key";
            cbbNivel.SelectedIndex = 0;

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
            
            dtpInicio.ResetText();
            dtpFinal.ResetText();

            rbtEmpT.Checked = false;
            rbtEmpU.Checked = true;
            lblEmp.Visible = true;
            txtEmpleado.Visible = true;
            txtNombre.Visible = true;

            rbtNivelT.Checked = true;
            rbtNivelU.Checked = false;
            rbtNivelA.Checked = false;
            lblNivel.Visible = false;
            cbbNivel.Visible = false;
            txtNiveles.Visible = false;

            rbtLineaT.Checked = true;
            rbtLineaU.Checked = false;
            rbtLineaA.Checked = false;
            cbbLineaIni.Visible = false;
            lblLIni.Visible = false;
            txtLineas.Visible = false;

            dtpInicio.Focus();

        }

        private void CargarLineas()
        {
            DataTable dtL1 = new DataTable();
            
            if (rbtPlantaU.Checked && cbbPlanta.SelectedIndex != -1)
            {
                LineaLogica line = new LineaLogica();
                line.Planta = cbbPlanta.SelectedValue.ToString();
                dtL1 = LineaLogica.LineaPlanta(line);
             
            }
            else
            {
                dtL1 = LineaLogica.ListarTodas();
             
            }
                

            cbbLineaIni.DataSource = dtL1;
            cbbLineaIni.ValueMember = "linea";
            cbbLineaIni.DisplayMember = "linea";
            cbbLineaIni.SelectedIndex = -1;
            

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

            if (rbtEmpU.Checked && string.IsNullOrEmpty(txtEmpleado.Text))
            {
                MessageBox.Show("No se a especificado el No. de Empleado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmpleado.Focus();
                return bValida;
            }

            if (rbtNivelU.Checked && cbbNivel.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de espeficicar el Nivel PPH", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbNivel.Focus();
                return bValida;
            }

            if (rbtNivelA.Checked && string.IsNullOrEmpty(txtNiveles.Text))
            {
                MessageBox.Show("Favor de espeficicar el Nivel PPH", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNiveles.Focus();
                return bValida;
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
                cbbLineaIni.Focus();
                return bValida;
            }

            if (rbtLineaA.Checked && string.IsNullOrEmpty(txtLineas.Text))
            {
                MessageBox.Show("Favor de espeficicar las Lineas", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLineas.Focus();
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

            string sIndE = "T";
            if (rbtEmpU.Checked)
                sIndE = "U";

            string sIndN = "T";
            if (rbtNivelA.Checked)
                sIndN = "A";
            if (rbtNivelU.Checked)
                sIndN = "U";

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

            Impresor._lsIndEmp = sIndE;
            Impresor._lsEmpleado = txtEmpleado.Text.ToString();
            Impresor._lsIndNivel = sIndN;
            if (cbbNivel.SelectedIndex != -1)
                Impresor._lsNivel = cbbNivel.SelectedValue.ToString();

            if (!string.IsNullOrEmpty(txtNiveles.Text))
                Impresor._lsNivel = "," + txtNiveles.Text.ToString() + ",";

            Impresor._lsIndPlanta = sIndP;
            if(cbbPlanta.SelectedIndex != -1)
                Impresor._lsPlanta = cbbPlanta.SelectedValue.ToString();
            
            Impresor._lsIndLinea = sIndL;
            if (cbbLineaIni.SelectedIndex != -1)
                Impresor._lsLineaIni = cbbLineaIni.SelectedValue.ToString();
            
            if (!string.IsNullOrEmpty(txtLineas.Text))
                Impresor._lsLineas = "," + txtLineas.Text.ToString() + ",";
            

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
        private void rbtFolioT_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtEmpT.Checked)
            {
                rbtEmpU.Checked = false;
                lblEmp.Visible = false;
                txtEmpleado.Visible = false;
                txtNombre.Visible = false;
            }
        }

        private void rbtFolioU_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtEmpU.Checked)
            {
                rbtEmpT.Checked = false;
                lblEmp.Visible = true;
                txtEmpleado.Visible = true;
                txtNombre.Visible = true;
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
                txtLineas.Visible = true;
                cbbLineaIni.Visible = false;
            }
        }

        
        private void txtFolioIni_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtEmpleado_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtEmpleado, 1);
        }

        private void txtEmpleado_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtEmpleado, 0);
        }

        private void txtEmpleado_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtEmpleado.Text) && !string.IsNullOrWhiteSpace(txtEmpleado.Text))
            {
                OperadorLogica oper = new OperadorLogica();
                string sOperador = txtEmpleado.Text.ToUpper().Trim().ToString();

                if (sOperador.IndexOf("A") > 0)
                    sOperador = sOperador.Substring(0, 6);

                if (sOperador.Length < 6)
                    sOperador = sOperador.PadLeft(6, '0');

                oper.Operador = sOperador;
                DataTable datos = OperadorLogica.Consultar(oper);
                if (datos.Rows.Count != 0)
                {
                    txtEmpleado.Text = sOperador;
                    txtNombre.Text = datos.Rows[0]["nombre"].ToString();
                }
                else
                {
                    MessageBox.Show("No se encontró el No. de Empleado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtEmpleado.Clear();
                    txtNombre.Clear();
                }
                txtNombre.Focus();
            }
        }

        private void cbbNivel_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbNivel, 0);
        }

        private void cbbNivel_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbNivel, 1);
        }

        private void txtNiveles_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtNiveles, 0);
        }

        private void txtNiveles_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtNiveles, 1);
        }

        private void cbbLineaIni_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbLineaIni, 0);
        }

        private void cbbLineaIni_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbLineaIni, 1);
        }

        private void txtLineas_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtLineas, 0);
        }

        private void txtLineas_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtLineas, 1);
        }

        private void rbtNivelU_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtNivelU.Checked)
            {
                rbtNivelT.Checked = false;
                rbtNivelA.Checked = false;
                txtNiveles.Visible = false;
                lblNivel.Visible = true;
                cbbNivel.Visible = true;
            }
        }

        private void rbtNivelA_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtNivelA.Checked)
            {
                rbtNivelT.Checked = false;
                rbtNivelU.Checked = false;
                txtNiveles.Visible = true;
                lblNivel.Visible = true;
                cbbNivel.Visible = false;
            }
        }

        private void rbtNivelT_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtNivelT.Checked)
            {
                rbtNivelA.Checked = false;
                rbtNivelU.Checked = false;
                txtNiveles.Visible = false;
                lblNivel.Visible = false;
                cbbNivel.Visible = false;
            }
        }
    }
}
