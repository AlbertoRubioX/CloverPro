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
    public partial class wfPlantaPop_1t : Form
    {
        public string _lsPlanta;
        public string _lsLinea;
        public string _sClave;
        public wfPlantaPop_1t(string asPlanta)
        {
            InitializeComponent();
            _lsPlanta = asPlanta;
        }

        private void wfPlantaPop_1t_Load(object sender, EventArgs e)
        {
            cbbPlanta.DataSource = PlantaLogica.Listar();
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            if (!string.IsNullOrEmpty(_lsPlanta))
            {
                cbbPlanta.SelectedValue = _lsPlanta;
                LineaLogica line = new LineaLogica();
                line.Planta = _lsPlanta;
                cbbLinea.DataSource = LineaLogica.LineaPlanta(line);
                cbbLinea.ValueMember = "linea";
                cbbLinea.DisplayMember = "linea";
                cbbLinea.SelectedIndex = -1;
                cbbLinea.Focus();

            }
            else
            {
                cbbPlanta.SelectedIndex = -1;
                cbbPlanta.Focus();
            }
                
        }

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LineaLogica line = new LineaLogica();
            line.Planta = cbbPlanta.SelectedValue.ToString();
            cbbLinea.DataSource = LineaLogica.LineaPlanta(line);
            cbbLinea.ValueMember = "linea";
            cbbLinea.DisplayMember = "linea";
            cbbLinea.SelectedIndex = -1;

            cbbLinea.Focus();
        }

        private void cbbPlanta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void cbbLinea_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _sClave = cbbPlanta.SelectedValue.ToString();
            _sClave += ":" + cbbLinea.SelectedValue.ToString();

            Close();
        }

        private void cbbLinea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void wfPlantaPop_1t_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (cbbPlanta.SelectedIndex != -1)
                _lsPlanta = cbbPlanta.SelectedValue.ToString();
            if (cbbLinea.SelectedIndex != -1)
                _lsLinea = cbbLinea.SelectedValue.ToString();
        }
    }
}
