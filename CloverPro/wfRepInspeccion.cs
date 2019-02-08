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
    public partial class wfRepInspeccion : Form
    {
        private string _lsProceso = "REP020";
        public bool _lbCambio;
        public wfRepInspeccion()
        {
            InitializeComponent();
        }

        private void wfRepInspeccion_Load(object sender, EventArgs e)
        {
            Inicio();
            
        }

       
        private void Inicio()
        {
            _lbCambio = false;

            dtpInicio.ResetText();
            dtpFinal.ResetText();

            txtRPO.Clear();

            cbbTurno.ResetText();
            Dictionary<string, string> Turno = new Dictionary<string, string>();
            //Turno.Add("0", "Ambos");
            Turno.Add("1", "1er");
            Turno.Add("2", "2do");
            cbbTurno.DataSource = new BindingSource(Turno, null);
            cbbTurno.DisplayMember = "Value";
            cbbTurno.ValueMember = "Key";
            cbbTurno.SelectedIndex = 0;

            txtRPO.Focus();

        }
        private Boolean BuscarRPO(string asRPO)
        {
            DataTable dtEx = new DataTable();

            try
            {
                if (asRPO.IndexOf("RPO") == -1)
                    asRPO = "RPO" + asRPO;

                int iPos = asRPO.IndexOf("-");
                if (iPos > 0)
                    asRPO = asRPO.Substring(0, iPos);
                
                txtRPO.Text = asRPO;

                RpoLogica rpo = new RpoLogica();
                rpo.RPO = asRPO;
                if (!RpoLogica.Verificar(rpo))
                {
                    MessageBox.Show("No se encontró información del RPO", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error... " + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

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
                if (dtpInicio.Value > dtpFinal.Value)
                {
                    MessageBox.Show("La Fecha Inicial no debe ser mayor a la Fecha Final", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtpFinal.Focus();
                    return bValida;
                }
            }

            if (string.IsNullOrEmpty(txtRPO.Text) || string.IsNullOrWhiteSpace(txtRPO.Text))
            {
                MessageBox.Show("No se a especificado el RPO", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRPO.Focus();
                return bValida;
            }
            else
            {
                if (!BuscarRPO(txtRPO.Text.ToString().Trim()))
                    return false;
            }
            
            return true;
        }

        private void wfRepInspeccion_Activated(object sender, EventArgs e)
        {
            txtRPO.Focus();
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
        
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            wfImpresor Impresor = new wfImpresor();
            Impresor._lsProceso = _lsProceso;
            Impresor._ldtInicio = dtpInicio.Value;
            Impresor._ldtFinal = dtpFinal.Value;
            Impresor._lsRPO = txtRPO.Text.ToString();
            Impresor._lsTurno = cbbTurno.SelectedValue.ToString();
            Impresor.Show();
        }

        private void txtRPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtRPO.Text))
            {
                //FIND SKU IN DATABASE
                if(!BuscarRPO(txtRPO.Text.ToString()))
                    txtRPO.Clear();
            }
        }

        private void txtRPO_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRPO, 1);
        }

        private void txtRPO_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRPO, 0);
        }

        private void cbbTurno_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbTurno, 1);
        }

        private void cbbTurno_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbTurno, 0);
        }
    }
}
