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
    public partial class wfDefectos : Form
    {
        public bool _lbCambio;
        private string _lsProceso = "CAT055";
        public wfDefectos()
        {
            InitializeComponent();
        }

        private void wfDefectos_Load(object sender, EventArgs e)
        {
            Inicio();
        }

       
        private void Inicio()
        {
            _lbCambio = false;

            cbbDefecto.ResetText();
            DataTable dt = DefectoLogica.Listar();
            cbbDefecto.DataSource = dt;
            cbbDefecto.ValueMember = "defecto";
            cbbDefecto.DisplayMember = "defecto";
            cbbDefecto.SelectedIndex = -1;

            txtDescrip.Clear();

            cbbDefecto.Focus();
        }
        private bool Valida()
        {
            bool bValida = false;
            if (string.IsNullOrEmpty(cbbDefecto.Text))
            {
                return bValida;
            }
            if(string.IsNullOrEmpty(txtDescrip.Text))
            {
                MessageBox.Show("No se a especificado la Descripción del Defecto", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbDefecto.Focus();
                return bValida;
            }
           


            return true;
        }

        private void wfDefectos_Activated(object sender, EventArgs e)
        {
            cbbDefecto.Focus();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (Guardar())
                Close();
            else
                Inicio();
        }

        private bool Guardar()
        {
            if (Valida())
            {

                try
                {
                    DefectoLogica def = new DefectoLogica();
                    def.Defecto = cbbDefecto.Text.ToString();
                    def.Descrip = txtDescrip.Text.ToString().TrimEnd();
                    def.Usuario = GlobalVar.gsUsuario;
                    
                    if (DefectoLogica.Guardar(def) > 0)
                        return true;
                    else
                    {
                        MessageBox.Show("Error al intentar guardar el Defecto", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch(Exception ie)
                {
                    MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "DefectoLogica.Guardar(est)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
                return false;
        }
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

       
        private void btNew_Click(object sender, EventArgs e)
        {
            Inicio();
        }

        private void cbbDefecto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(cbbDefecto.Text) && !string.IsNullOrWhiteSpace(cbbDefecto.Text))
            {
                DefectoLogica def = new DefectoLogica();
                cbbDefecto.Text = cbbDefecto.Text.ToUpper().ToString();
                def.Defecto = cbbDefecto.Text.ToString();
                DataTable datos = DefectoLogica.Consultar(def);
                if (datos.Rows.Count != 0)
                {
                    txtDescrip.Text = datos.Rows[0]["descrip"].ToString();
                }
            }
            
            
        }

        private void cbbDefecto_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DefectoLogica est = new DefectoLogica();
            est.Defecto = cbbDefecto.SelectedValue.ToString();
            DataTable datos = DefectoLogica.Consultar(est);
            if (datos.Rows.Count != 0)
            {
                txtDescrip.Text = datos.Rows[0]["descrip"].ToString();
            }
      
              
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            if (cbbDefecto.SelectedIndex == -1)
                return;

            DialogResult Result = MessageBox.Show("Desea borrar el Defecto?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                try
                {
                    DefectoLogica est = new DefectoLogica();
                    est.Defecto = cbbDefecto.SelectedValue.ToString();
                    DefectoLogica.Eliminar(est);
                    Inicio();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "DefectoLogica.Eliminar(est)" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                
            }
        }

        private void txtDescrip_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtDescrip, 0);
        }

        private void txtDescrip_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtDescrip, 1);
        }

        private void cbbDefecto_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbDefecto, 0);
        }

        private void cbbDefecto_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbDefecto, 1);
        }
    }
}
