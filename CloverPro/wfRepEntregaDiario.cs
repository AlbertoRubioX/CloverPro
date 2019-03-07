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
    public partial class wfRepEntregaDiario : Form
    {
        private string _lsProceso = "REP140";
        public bool _lbCambio;
        public wfRepEntregaDiario()
        {
            InitializeComponent();
        }

        #region regInicio
        private void wfRepEntregaDiario_Load(object sender, EventArgs e)
        {
            Inicio();
        }

        private void wfRepEntregaDiario_Activated(object sender, EventArgs e)
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

            cbbLineaIni.Visible = false;

            CargarLineas();

            dtpInicio.Focus();
        }

        private void CargarLineas()
        {
            DataTable dtL1 = new DataTable();
            if (cbbPlanta.SelectedIndex != -1)
            {
                LineaLogica line = new LineaLogica();
                line.Planta = cbbPlanta.SelectedValue.ToString();
                dtL1 = LineaLogica.LineasNav(line);
            }
            else
            {
                dtL1 = LineaLogica.ListarTodasNav();
            }


            cbbLineaIni.DataSource = dtL1;
            cbbLineaIni.ValueMember = "linea_nav";
            cbbLineaIni.DisplayMember = "linea_nav";
            cbbLineaIni.SelectedIndex = -1;

        }
        #endregion

        private bool Valida()
        {
            bool bValida = false;

            if (string.IsNullOrEmpty(dtpInicio.Text) )
            {
                MessageBox.Show("Favor de especificar el Periódo del reporte", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpInicio.Focus();
                return bValida;
            }


            if (cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado la planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }

            if(rbtLineaU.Checked && cbbLineaIni.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de especificar la Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            string sIndP = "1";

            string sIndL = "0";
            if (rbtLineaU.Checked)
                sIndL = "1";
            
            Impresor._ldtInicio = dtpInicio.Value;

            Impresor._lsIndPlanta = sIndP;
            if(cbbPlanta.SelectedIndex !=-1)
                Impresor._lsPlanta = cbbPlanta.SelectedValue.ToString();
            
                
            if (rbtAmbos.Checked)
                Impresor._lsTurno = "A";
            else
            {
                if (rbtTuno1.Checked)
                    Impresor._lsTurno = "1";
                else
                    Impresor._lsTurno = "2";
            }

            if (rbtLineaT.Checked)
                Impresor._lsLineaIni = "A";
            else 
            {
                Impresor._lsIndLinea = sIndL;
                if (cbbLineaIni.SelectedIndex != -1)
                    Impresor._lsLineaIni = cbbLineaIni.SelectedValue.ToString();
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
        
        private void rbtLineaT_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtLineaT.Checked)
            {
                rbtLineaU.Checked = false;
                cbbLineaIni.Visible = false;
            }
        }

        private void rbtLineaU_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtLineaU.Checked)
            {
                rbtLineaT.Checked = false;
                cbbLineaIni.Visible = true;
            }
        }
        
        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CargarLineas();
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

        private void rbtAmbos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtAmbos.Checked)
            {
                rbtTuno1.Checked = false;
                rbtTuno2.Checked = false;
            }
            //rbtTuno1.Checked = !rbtAmbos.Checked;
            //rbtTuno2.Checked = !rbtAmbos.Checked;
        }
    }
}