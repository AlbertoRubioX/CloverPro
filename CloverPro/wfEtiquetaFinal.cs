using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Datos;
using Logica;

namespace CloverPro
{
    public partial class wfEtiquetaFinal : Form
    {
        public bool _lbCambio;
        private string _lsProceso = "PRO070";
        private string _lsTurno;
        private string _lsPlanta;
        private string _lsLine;
        private string _lsUsuario;
        private string _lsCodigo;
        public wfEtiquetaFinal()
        {
            InitializeComponent();
        }

        #region regInicio
        private void wfEtiquetaFinal_Activated(object sender, EventArgs e)
        {
            txtEtiqueta.Focus();
        }
        private void wfEtiquetaFinal_Load(object sender, EventArgs e)
        {
            Inicio();

            try
            {
                _lsTurno = GlobalVar.gsTurno;
                _lsPlanta = GlobalVar.gsPlanta;
                _lsUsuario = GlobalVar.gsUsuario;
                OperadorLogica Oper = new OperadorLogica();
                Oper.Operador = _lsUsuario;
                DataTable dtEmp = OperadorLogica.Consultar(Oper);
                if (dtEmp.Rows.Count != 0)
                {
                    if (!string.IsNullOrEmpty(dtEmp.Rows[0][5].ToString()))
                    {
                        _lsLine = dtEmp.Rows[0][5].ToString();
                        btnLine.Text = _lsLine;
                    }
                }
                else
                {
                    wfPlantaPop popUp = new wfPlantaPop(_lsPlanta);
                    popUp.ShowDialog();
                    string sPlanta = popUp._sClave;
                    if (string.IsNullOrEmpty(sPlanta))
                        return;

                    _lsLine = sPlanta.Substring(sPlanta.IndexOf(":") + 1);
                    sPlanta = sPlanta.Substring(0, sPlanta.IndexOf(":"));

                    _lsPlanta = sPlanta;

                    LineaLogica line = new LineaLogica();
                    line.Planta = _lsPlanta;
                    line.Linea = _lsLine;
                    DataTable dtL = LineaLogica.Consultar(line);
                    string sLinea = dtL.Rows[0]["nombre"].ToString();
                    btnLine.Text = sLinea;
                }

                DataTable dtC = ConfigLogica.Consultar();
                _lsCodigo = dtC.Rows[0]["eti_codigob"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            

        }

       
        private void Inicio()
        {
            _lbCambio = false;

            DataTable dt = DefectoLogica.Listar();

            txtEtiq2.Clear();
            txtRPO2.Clear();
            txtDefect2.Clear();

            //etiqueta anterior
            if (!string.IsNullOrEmpty(txtEtiqueta.Text))
                txtEtiq2.Text = txtEtiqueta.Text.ToString();
            if (!string.IsNullOrEmpty(txtRPO.Text))
                txtRPO2.Text = txtRPO.Text.ToString();
            if (!string.IsNullOrWhiteSpace(txtDefect.Text))
                txtDefect2.Text = txtDefect.Text.ToString();

            txtEtiqueta.Clear();
            txtRPO.Clear();
            txtDefect.Clear();
            cbbDefecto.ResetText();
            
            cbbDefecto.DataSource = dt;
            cbbDefecto.ValueMember = "defecto";
            cbbDefecto.DisplayMember = "descrip";
            cbbDefecto.SelectedIndex = -1;

            txtEtiqueta.Focus();
        }
        #endregion

        #region regGuardar
        private bool Valida()
        {
            bool bValida = false;
            if (string.IsNullOrEmpty(txtEtiqueta.Text))
            {
                txtEtiqueta.Focus();
                return bValida;
            }

            if (string.IsNullOrEmpty(txtRPO.Text))
            {
                MessageBox.Show("El RPO es incorrecto", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRPO.Focus();
                return bValida;
            }

            if (string.IsNullOrEmpty(txtDefect.Text) || string.IsNullOrWhiteSpace(txtDefect.Text))
            {
                MessageBox.Show("No se a especificado el Código de Salida", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDefect.Focus();
                return bValida;
            }

            if (cbbDefecto.SelectedIndex == -1 && string.IsNullOrEmpty(txtDefect.Text))
            {
                MessageBox.Show("No se a especificado el Defecto", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbDefecto.Focus();
                return bValida;
            }
           
            return true;
        }
        private bool Guardar()
        {
            if (Valida())
            {

                try
                {
                    EtiquetaDetLogica eti = new EtiquetaDetLogica();

                    eti.Folio = AccesoDatos.Consec(_lsProceso);
                    eti.FechaSale = dtFecha.Value;
                    eti.Planta = _lsPlanta;
                    eti.Linea = _lsLine;
                    eti.Turno = _lsTurno;
                    eti.Usuario = _lsUsuario;
                    eti.Barcode = txtEtiqueta.Text.ToString();
                    eti.RPO = txtRPO.Text.ToString();
                    if (txtDefect.Text.ToString() == _lsCodigo)
                        eti.Defecto = "N";
                    else
                        eti.Defecto = "S";
                    eti.Codigo = txtDefect.Text.ToString();

                    if (EtiquetaDetLogica.Guardar(eti) > 0)
                    {
                        Inicio();
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error al intentar guardar", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch (Exception ie)
                {
                    MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "DefectoLogica.Guardar(est)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
                return false;
        }

        #endregion

        #region regBoton
        private void btnLine_Click(object sender, EventArgs e)
        {
            wfPlantaPop popUp = new wfPlantaPop(_lsPlanta);
            popUp.ShowDialog();
            string sPlanta = popUp._sClave;
            if (string.IsNullOrEmpty(sPlanta))
                return;

            string sLinea = sPlanta.Substring(sPlanta.IndexOf(":") + 1);
            sPlanta = sPlanta.Substring(0, sPlanta.IndexOf(":"));

            _lsPlanta = sPlanta;
            LineaLogica line = new LineaLogica();
            line.Planta = sPlanta;
            line.Linea = sLinea;
            DataTable dtL = LineaLogica.Consultar(line);
            btnLine.Text = dtL.Rows[0]["nombre"].ToString();

            txtEtiqueta.Focus();
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            if (Guardar())
                Close();
            else
                Inicio();
        }

        
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

       
        private void btNew_Click(object sender, EventArgs e)
        {
            Inicio();
        }

        #endregion

        #region regCaptura

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
                    txtDefect.Text = datos.Rows[0]["descrip"].ToString();
                }
            }
            
            
        }

        private void cbbDefecto_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                DefectoLogica est = new DefectoLogica();
                est.Defecto = cbbDefecto.SelectedValue.ToString();
                DataTable datos = DefectoLogica.Consultar(est);
                if (datos.Rows.Count != 0)
                {
                    txtDefect.Text = datos.Rows[0]["defecto"].ToString();
                    Guardar();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
              
        }

        private void txtEtiqueta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtEtiqueta.Text))
                return;

            if (string.IsNullOrEmpty(_lsLine))
            {
                MessageBox.Show("No se ha especificado la Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEtiqueta.Clear();
                btnLine_Click(sender, e);
                return;
            }

            try
            {
                EtiquetaDetLogica eti = new EtiquetaDetLogica();
                eti.Planta = _lsPlanta;
                eti.Linea = _lsLine;
                eti.Turno = _lsTurno;
                eti.Barcode = txtEtiqueta.Text.ToString();

                //if(EtiquetaDetLogica.Verificar(eti))
                //{
                //    MessageBox.Show("La Etiqueta ya ha sido registrada anteriormente.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                string sRPO = txtEtiqueta.Text.ToString();
                int iPos = sRPO.IndexOf("-");
                int iIni = 0;
                if (iPos > 0)
                {
                    string sIni = sRPO.Substring(iPos + 2, 5);
                    Int32.TryParse(sIni, out iIni);
                    iIni++;

                    sRPO = "RPO" + sRPO.Substring(0, iPos);
                }
                else
                {
                    MessageBox.Show("Etiqueta Incorrecta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtEtiqueta.Clear();
                    txtEtiqueta.Focus();
                    return;
                }

                txtRPO.Text = sRPO;
                txtDefect.Focus();

            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }

        private void txtDefect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtDefect.Text))
                return;

            if (string.IsNullOrEmpty(txtEtiqueta.Text))
                return;

            try
            {
                bool bSave = false;

                if (txtDefect.Text.ToString() == _lsCodigo)
                    bSave = true;
                else
                {
                    DefectoLogica def = new DefectoLogica();
                    def.Defecto = txtDefect.Text.ToString().TrimEnd();
                    DataTable dtD = DefectoLogica.Consultar(def);
                    if (dtD.Rows.Count > 0)
                    {
                        cbbDefecto.SelectedValue = txtDefect.Text.ToString();
                        bSave = true;
                    }
                    else
                    {
                        MessageBox.Show("Código no encontrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtDefect.Clear();
                        return;
                    }
                }

                if (bSave)
                    Guardar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            

        }
        #endregion

        #region regColor
        private void txtDescrip_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtDefect, 0);
        }

        private void txtDescrip_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtDefect, 1);
        }

        private void cbbDefecto_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbDefecto, 0);
        }

        private void cbbDefecto_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbDefecto, 1);
        }

        private void txtEtiqueta_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtEtiqueta, 0);
        }

        private void txtEtiqueta_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtEtiqueta, 1);
        }
        #endregion
       
    }
}
