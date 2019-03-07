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
    public partial class wfCapturaPop_1det : Form
    {
        public string _lsProceso;
        public string _sClave;
        public string _lsTipo;
        public string _lsArea;
        public string _lsParte;
        public double _ldCant;
        private bool bChange;

        public wfCapturaPop_1det()
        {
            InitializeComponent();
        }

        private void wfCapturaPop_1t_Load(object sender, EventArgs e)
        {
            #region regEmpaque

            
            if (_lsProceso == "EMP040" && _sClave == "DETENIDO")
            {
                
                Dictionary<string, string> Data = new Dictionary<string, string>();
                Data.Add("INS", "INSUFICIENTE");
                Data.Add("CAL", "PROBLEMAS DE CALIDAD");
                //Data.Add("INV", "PROBLEMAS DE INVENTARIO");
                            
                if (_lsArea == "E") //etiquetas
                    Data.Add("DOC", "PROBLEMAS CON DOCUMENTACION");
                            
                if(_lsArea == "A")
                {
                    Data.Add("EST", "PROBLEMAS DE ESTRUCTURA EN RPO");
                    if(_lsTipo == "P")
                        Data.Add("CAP", "CAPACIDAD"); // SOLO LINEAS PONY
                }

                Data.Add("CAN", "RPO CANCELADO");
                Data.Add("OTR", "OTRO");

                cbbClave.DataSource = new BindingSource(Data, null);
                cbbClave.DisplayMember = "Value";
                cbbClave.ValueMember = "Key";
                cbbClave.SelectedIndex = -1;   
            }
            #endregion         
        }

        private void txtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _sClave = null;
                Close();
            }

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtCant.Text))
                txtCant.Focus();
         
        }

        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cbbClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _sClave = null;
                Close();
            }
            
            if(e.KeyCode == Keys.Enter)
                bChange = true;
        }

        private void cbbClave_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(_lsProceso == "EMP040")
            {
                if (cbbClave.SelectedIndex == -1)
                    return;

                string sCode = cbbClave.SelectedValue.ToString();
                if(sCode == "INS" || sCode=="CAL" || sCode =="EST")
                {
                    txtClave.Enabled = true;
                    txtCant.Enabled = true;
                    txtClave.Focus();
                }
                else
                {
                    txtClave.Enabled = false;
                    txtCant.Enabled = false;
                    btnAceptar.Focus();
                }
            }
            
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (_lsProceso == "EMP040")
                {
                    if (cbbClave.SelectedIndex == -1)
                    {
                        MessageBox.Show("Favor de seleccionar del listado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbbClave.Focus();
                        return;
                    }

                    string sCode = cbbClave.SelectedValue.ToString();
                    double dCant = 0;
                    if (!double.TryParse(txtCant.Text, out dCant))
                        dCant = 0;

                    if (sCode == "CAL" || sCode == "INS" || sCode == "EST")
                    {
                        if (string.IsNullOrEmpty(txtClave.Text))
                        {
                            MessageBox.Show("Favor de capturar el No. de Parte afectado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClave.Focus();
                            return;
                        }

                        if (txtClave.Text.Length <= 2)
                        {
                            MessageBox.Show("Favor de capturar la información requerida", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClave.Focus();
                            return;
                        }

                        if (txtClave.Text.ToString().IndexOf("RPO") != -1)
                        {
                            MessageBox.Show("Favor de capturar el No. de Parte afectado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClave.Focus();
                            return;
                        }

                        if (dCant <= 0)
                        {
                            MessageBox.Show("Favor de capturar la cantidad detenida/faltante", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtCant.Focus();
                            return;
                        }

                    }
                    else
                    {
                        if (sCode == "OTR")
                        {
                            if (string.IsNullOrEmpty(txtClave.Text))
                            {
                                MessageBox.Show("Favor de capturar el Motivo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtClave.Focus();
                                return;
                            }
                        }
                    }

                    _sClave = cbbClave.SelectedValue.ToString();
                    _lsParte = txtClave.Text.ToString();
                    _ldCant = dCant;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

       
        private void cbbClave_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void cbbClave_DropDown(object sender, EventArgs e)
        {
            cbbClave.AutoCompleteMode = AutoCompleteMode.None;
        }

        private void wfCapturaPop_1det_Activated(object sender, EventArgs e)
        {
            cbbClave.Focus();
        }
    }
}
