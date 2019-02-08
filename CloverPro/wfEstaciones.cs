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
    public partial class wfEstaciones : Form
    {
        public bool _lbCambio;
        public wfEstaciones()
        {
            InitializeComponent();
        }

        private void wfEstaciones_Load(object sender, EventArgs e)
        {
            Inicio();
        }

       
        private void Inicio()
        {
            _lbCambio = false;

            cbbEstacion.ResetText();
            DataTable dt = EstacionLogica.Listar();
            cbbEstacion.DataSource = dt;
            cbbEstacion.ValueMember = "estacion";
            cbbEstacion.DisplayMember = "estacion";
            cbbEstacion.SelectedIndex = -1;

            chbMonitor.Checked = false;

            DataTable data = PlantaLogica.Listar();
            cbbPlanta.DataSource = data;
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.SelectedIndex = -1;

            cbbLinea.ResetText();
            cbbProceso.ResetText();
            DataTable dtPr = EstacionLogica.ListarProceso();
            cbbProceso.DataSource = dtPr;
            cbbProceso.ValueMember = "proceso";
            cbbProceso.DisplayMember = "descrip";
            cbbProceso.SelectedIndex = -1;

            Dictionary<string, string> Tipo = new Dictionary<string, string>();
            Tipo.Add("I", "Inicial");
            Tipo.Add("F", "Final");
            Tipo.Add("G", "Global");//Monitor de SetUP ambas plantas
            cbbArea.DataSource = new BindingSource(Tipo, null);
            cbbArea.DisplayMember = "Value";
            cbbArea.ValueMember = "Key";
            cbbArea.SelectedIndex = -1;

            cbbEstacion.Focus();
        }
        private bool Valida()
        {
            bool bValida = false;
            if (string.IsNullOrEmpty(cbbEstacion.Text))
            {
                MessageBox.Show("No se a especificado la Estación de Trabajo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbEstacion.Focus();
                return bValida;
            }

            if (cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado la Planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }

            if(!chbMonitor.Checked)
            {
                if (cbbLinea.SelectedIndex == -1)
                {
                    MessageBox.Show("No se a especificado la Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbbLinea.Focus();
                    return bValida;
                }


                if (cbbPlanta.SelectedValue.ToString() == "EMP")
                {
                    if (cbbProceso.SelectedIndex == -1)
                    {
                        MessageBox.Show("No se a especificado el Proceso", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbbProceso.Focus();
                        return bValida;
                    }

                    if (cbbArea.SelectedIndex == -1)
                    {
                        MessageBox.Show("No se a especificado el Area", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbbArea.Focus();
                        return bValida;
                    }
                }

            }

            return true;
        }

        private void wfEstaciones_Activated(object sender, EventArgs e)
        {
            cbbEstacion.Focus();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (Guardar())
                Inicio();
        }

        private bool Guardar()
        {
            if (Valida())
            {

                try
                {
                    EstacionLogica est = new EstacionLogica();
                    est.Estacion = cbbEstacion.Text.ToString();
                    est.Planta = cbbPlanta.SelectedValue.ToString();
                    if (chbMonitor.Checked)
                        est.Monitor = "1";
                    else
                        est.Monitor = "0";
                    if (cbbLinea.SelectedIndex == -1)
                        est.Linea = "";
                    else
                        est.Linea = cbbLinea.SelectedValue.ToString();
                    if (cbbProceso.SelectedIndex == -1)
                        est.Proceso = "";
                    else
                        est.Proceso = cbbProceso.SelectedValue.ToString();
                    if (cbbArea.SelectedIndex == -1)
                        est.Area = "";
                    else
                        est.Area = cbbArea.SelectedValue.ToString();

                    //if(chbMonitor.Checked)
                    //{
                    //    est.Monitor = "1";
                    //    est.Linea = "";
                    //    est.Proceso = "";
                    //    est.Area = "";
                    //}
                    //else
                    //{
                        //est.Monitor = "0";
                        //est.Linea = cbbLinea.SelectedValue.ToString();
                        //if (cbbProceso.SelectedIndex != -1)
                        //    est.Proceso = cbbProceso.SelectedValue.ToString();
                        //else
                        //    est.Proceso = "";
                        //if (cbbArea.SelectedIndex != -1)
                        //    est.Area = cbbArea.SelectedValue.ToString();
                        //else
                        //    est.Area = "";
                    //}
                    
                    est.Usuario = GlobalVar.gsUsuario;
                    
                    if (EstacionLogica.Guardar(est) > 0)
                        return true;
                    else
                    {
                        MessageBox.Show("Error al intentar guardar la Estación", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch(Exception ie)
                {
                    MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "EstacionLogica.Guardar(est)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void cbbEstacion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(cbbEstacion.Text) && !string.IsNullOrWhiteSpace(cbbEstacion.Text))
            {
                EstacionLogica est = new EstacionLogica();
                string sEst = cbbEstacion.Text.ToUpper().ToString();
                if(sEst.Length <= 4)
                    sEst = "MEXI" + sEst.PadLeft(4, '0');

                cbbEstacion.Text = sEst;
                est.Estacion = sEst;
                DataTable datos = EstacionLogica.Consultar(est);
                if (datos.Rows.Count != 0)
                {
                    cbbEstacion.SelectedValue = datos.Rows[0]["estacion"].ToString();
                    cbbPlanta.SelectedValue = datos.Rows[0]["planta"].ToString();
                    if (datos.Rows[0]["ind_monitor"].ToString() == "1")
                        chbMonitor.Checked = true;
                    else
                        chbMonitor.Checked = false;

                    CargarLineas();
                    cbbLinea.SelectedValue = datos.Rows[0]["linea"].ToString();
                    cbbProceso.ResetText();
                    cbbArea.ResetText();
                    if(!string.IsNullOrEmpty(datos.Rows[0]["proceso"].ToString()))
                        cbbProceso.SelectedValue = datos.Rows[0]["proceso"].ToString();
                    if (!string.IsNullOrEmpty(datos.Rows[0]["area"].ToString()))
                        cbbArea.SelectedValue  = datos.Rows[0]["area"].ToString();
                }
                else
                    cbbPlanta.SelectedIndex = -1;

                cbbPlanta.Focus();
            }
            
        }

        private void cbbEstacion_SelectionChangeCommitted(object sender, EventArgs e)
        {
            EstacionLogica est = new EstacionLogica();
            est.Estacion = cbbEstacion.SelectedValue.ToString();
            DataTable datos = EstacionLogica.Consultar(est);
            if (datos.Rows.Count != 0)
            {
                cbbPlanta.SelectedValue = datos.Rows[0]["planta"].ToString();
                if (datos.Rows[0]["ind_monitor"].ToString() == "1")
                    chbMonitor.Checked = true;
                else
                    chbMonitor.Checked = false;
                CargarLineas();
                cbbLinea.SelectedValue = datos.Rows[0]["linea"].ToString();
                cbbProceso.SelectedValue = datos.Rows[0]["proceso"].ToString();
                cbbArea.SelectedValue = datos.Rows[0]["area"].ToString();
            }
            else
                cbbPlanta.SelectedIndex = -1;

            cbbPlanta.Focus();
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            if (cbbEstacion.SelectedIndex == -1)
                return;

            DialogResult Result = MessageBox.Show("Desea borrar la Estación de Trabajo?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                try
                {
                    EstacionLogica est = new EstacionLogica();
                    est.Estacion = cbbEstacion.SelectedValue.ToString();
                    EstacionLogica.Eliminar(est);
                    Inicio();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "EstacionLogica.Eliminar(est)" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                
            }
        }
        private void CargarLineas()
        {
            if (cbbPlanta.SelectedIndex == -1)
                return;

            cbbLinea.ResetText();
            LineaLogica lin = new LineaLogica();
            lin.Planta = cbbPlanta.SelectedValue.ToString();
            DataTable data = LineaLogica.LineaPlanta(lin);
            cbbLinea.DataSource = data;
            cbbLinea.ValueMember = "linea";
            cbbLinea.DisplayMember = "linea";
            cbbLinea.SelectedIndex = -1;
        }
        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CargarLineas();
        }

        private void cbbProceso_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(cbbProceso.SelectedValue.ToString() == "EMP040")
            {
                cbbArea.ResetText();
                Dictionary<string, string> Tipo = new Dictionary<string, string>();
                Tipo.Add("L", "Normal");
                Tipo.Add("P", "Pony");
                cbbArea.DataSource = new BindingSource(Tipo, null);
                cbbArea.DisplayMember = "Value";
                cbbArea.ValueMember = "Key";
                cbbArea.SelectedIndex = -1;
            }

            if (cbbProceso.SelectedValue.ToString() == "ING010")
            {
                cbbArea.ResetText();
                Dictionary<string, string> Tipo = new Dictionary<string, string>();
                Tipo.Add("1", "1");
                Tipo.Add("2", "2");
                Tipo.Add("3", "3");
                Tipo.Add("4", "4");
                Tipo.Add("5", "5");
                Tipo.Add("6", "6");
                Tipo.Add("7", "7");
                Tipo.Add("8", "8");
                Tipo.Add("9", "9");
                Tipo.Add("10", "10");
                Tipo.Add("11", "11");
                Tipo.Add("12", "12");
                Tipo.Add("13", "13");
                Tipo.Add("14", "14");
                Tipo.Add("15", "15");
                Tipo.Add("16", "16");
                Tipo.Add("17", "17");
                Tipo.Add("18", "18");
                Tipo.Add("19", "19");
                Tipo.Add("20", "20");
                Tipo.Add("21", "21");
                Tipo.Add("22", "22");
                Tipo.Add("23", "23");
                Tipo.Add("24", "24");
                Tipo.Add("25", "25");
                cbbArea.DataSource = new BindingSource(Tipo, null);
                cbbArea.DisplayMember = "Value";
                cbbArea.ValueMember = "Key";
                cbbArea.SelectedIndex = -1;
            }
        }

        private void cbbProceso_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
