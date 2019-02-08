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
    public partial class wfRepEtiquetas : Form
    {
        private string _lsProceso = "REP010";
        public bool _lbCambio;
        public wfRepEtiquetas()
        {
            InitializeComponent();
        }

        private void wfRepEtiquetas_Load(object sender, EventArgs e)
        {
            Inicio();
            
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

            dtpInicio.ResetText();
            dtpFinal.ResetText();

            cbbPlanta.Focus();

        }
        private bool Valida()
        {
            bool bValida = false;

            if(string.IsNullOrEmpty(cbbPlanta.Text))
            {
                MessageBox.Show("No se a especificado la planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }

            if(string.IsNullOrEmpty(dtpInicio.Text) || string.IsNullOrEmpty(dtpFinal.Text))
            {
                MessageBox.Show("No se a especificado el periódo del reporte", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpInicio.Focus();
                return bValida;
            }
            
            return true;
        }

        private void wfRepEtiquetas_Activated(object sender, EventArgs e)
        {
            cbbPlanta.Focus();
        }

       
        
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

       
        private void cbbPlanta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Inicio();
        }

        private void cbbPlanta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbbPlanta.Text.Length >= 9)
                e.Handled = true;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            wfImpresor Impresor = new wfImpresor();
            Impresor._lsProceso = _lsProceso;
            Impresor._lsPlanta = cbbPlanta.SelectedValue.ToString();
            Impresor._ldtFinal = dtpFinal.Value;
            Impresor._ldtInicio = dtpInicio.Value;
            Impresor.Show();
        }
    }
}
