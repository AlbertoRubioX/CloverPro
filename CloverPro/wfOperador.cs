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
using System.IO;

namespace CloverPro
{
    public partial class wfOperador : Form
    {
        public bool _lbCambio;
        private string _lsProceso = "CAT040";
        private string _lsFechaAnt;
        public wfOperador()
        {
            InitializeComponent();
        }

        #region regInicio
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        private void wfOperador_Load(object sender, EventArgs e)
        {
            Inicio();
        }

        private void wfOperador_Activated(object sender, EventArgs e)
        {
            cbbOperador.Focus();
        }

        private void Inicio()
        {
            _lbCambio = false;

            cbbOperador.ResetText();
            DataTable dt = OperadorLogica.Listar();
            cbbOperador.DataSource = dt;
            cbbOperador.ValueMember = "empleado";
            cbbOperador.DisplayMember = "empleado";
            cbbOperador.SelectedIndex = -1;

            chbActivo.Checked = true;
            txtNombre.Clear();

            txtFecha.Clear();
            dtpFecha.ResetText();

            cbbPlanta.ResetText();
            DataTable data = PlantaLogica.Listar();
            cbbPlanta.DataSource = data;
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.SelectedIndex = -1;

            cbbLinea.ResetText();
            cbbLinea.DataSource = null;

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

            cbbTurno.ResetText();
            Dictionary<string, string> Turno = new Dictionary<string, string>();
            Turno.Add("1", "1");
            Turno.Add("2", "2");
            
            cbbTurno.DataSource = new BindingSource(Turno, null);
            cbbTurno.DisplayMember = "Value";
            cbbTurno.ValueMember = "Key";
            cbbTurno.SelectedIndex = 0;

            pbxFoto.Image = null;
            _lsFechaAnt = string.Empty;

            cbbOperador.Focus();

        }

        #endregion

        #region regBotones
        private void btSave_Click(object sender, EventArgs e)
        {
            if (Guardar())
                Inicio();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            Inicio();
        }
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbbOperador.Text))
            {
                return;
            }

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                OperadorLogica oper = new OperadorLogica();
                oper.Operador = cbbOperador.Text.ToString().ToUpper();
                OperadorLogica.Eliminar(oper);
                Inicio();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "OperadorLogica.Eliminar(oper);" + Environment.NewLine + ex.ToString(), "Error " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
        #endregion

        #region regGuardar
        private bool Valida()
        {
            bool bValida = false;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "10") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bValida;
            }

            if (string.IsNullOrEmpty(cbbOperador.Text))
            {
                MessageBox.Show("No se a especificado la clave del Operador", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbOperador.Focus();
                return bValida;
            }
            else
            {
                string sUsuario = cbbOperador.Text.ToUpper().Trim().ToString();
                if (sUsuario.Length > 6)
                {
                    MessageBox.Show("La clave del Operador no coincide con el número de empleado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbbOperador.Focus();
                    return bValida;
                }
            }

            if (txtFecha.MaskFull)
            {
                DateTime dt;
                if (!DateTime.TryParseExact(txtFecha.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dt))
                {
                    MessageBox.Show("Favor de capturar la fecha con el formato debido ( dia/mes/año)", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtFecha.Focus();
                    return bValida;
                }
            }

            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("No se a especificado el Nombre", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Focus();
                return bValida;
            }

            if (cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado la Planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
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
                    OperadorLogica oper = new OperadorLogica();
                    oper.Operador = cbbOperador.Text.ToString().ToUpper();
                    oper.Nombre = txtNombre.Text.ToString();
                    
                    DateTime dtFecha;
                    if (DateTime.TryParseExact(txtFecha.Text.ToString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtFecha))
                        oper.FechaIngre = dtFecha;

                    if (chbActivo.Checked)
                        oper.Activo = "1";
                    else
                        oper.Activo = "0";

                    oper.Planta = cbbPlanta.SelectedValue.ToString();
                    if (cbbLinea.SelectedIndex != -1)
                        oper.Linea = cbbLinea.SelectedValue.ToString();
                    if(cbbNivel.SelectedIndex != -1)
                        oper.Nivel = cbbNivel.SelectedValue.ToString();
                    if(cbbTurno.SelectedIndex != -1)
                        oper.Turno = cbbTurno.SelectedValue.ToString();
                    oper.Usuario = GlobalVar.gsUsuario;

                    if (OperadorLogica.Guardar(oper) > 0)
                        return true;
                    else
                    {
                        MessageBox.Show("Error al intentar guardar el Operador", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch (Exception ie)
                {
                    MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "OperadorLogica.Guardar(oper)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
                return false;
        }

        #endregion

        #region regCaptura
        private void cbbOperador_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(cbbOperador.Text) && !string.IsNullOrWhiteSpace(cbbOperador.Text))
            {
                OperadorLogica oper = new OperadorLogica();
                string sOperador = cbbOperador.Text.ToUpper().Trim().ToString();

                if (sOperador.IndexOf("A") > 0)
                    sOperador = sOperador.Substring(0, 6);

                if (sOperador.Length < 6)
                    sOperador = sOperador.PadLeft(6, '0');

                oper.Operador = sOperador;
                cbbOperador.Text = sOperador;
                DataTable datos = OperadorLogica.Consultar(oper);
                if (datos.Rows.Count != 0)
                {
                    txtNombre.Text = datos.Rows[0]["nombre"].ToString();
                    if(!string.IsNullOrEmpty(datos.Rows[0]["f_ingreso"].ToString()))
                        txtFecha.Text = datos.Rows[0].Field<DateTime>(9).ToString("dd/MM/yyyy");
                    else
                    {
                        dtpFecha.ResetText();
                        txtFecha.Clear();
                    }
                    if (datos.Rows[0]["activo"].ToString() == "1")
                        chbActivo.Checked = true;
                    else
                        chbActivo.Checked = false;
                    cbbPlanta.SelectedValue = datos.Rows[0]["planta"].ToString();
                    if (!string.IsNullOrEmpty(datos.Rows[0]["planta"].ToString()))
                    {
                        cbbLinea.ResetText();
                        LineaLogica line = new LineaLogica();
                        line.Planta = datos.Rows[0]["planta"].ToString();
                        DataTable data = LineaLogica.LineaPlanta(line);
                        cbbLinea.DataSource = data;
                        cbbLinea.ValueMember = "linea";
                        cbbLinea.DisplayMember = "linea";
                        cbbLinea.SelectedIndex = -1;
                    }
                    
                    cbbLinea.SelectedValue = datos.Rows[0]["linea"].ToString();
                    cbbNivel.SelectedValue = datos.Rows[0]["nivel"].ToString();
                    cbbTurno.SelectedValue = datos.Rows[0]["turno"].ToString();

                    TressLogica img = new TressLogica();
                    img.Codigo = int.Parse(sOperador);
                    datos = TressLogica.ConsultaImagen(img);
                    if(datos.Rows.Count > 0)
                    {
                        pbxFoto.Image = byteArrayToImage((byte[])datos.Rows[0]["IM_BLOB"]);
                    }

                }
                else
                {
                    TressLogica img = new TressLogica();
                    img.Codigo = int.Parse(sOperador);
                    datos = TressLogica.ConsultaOper(img);
                    if (datos.Rows.Count > 0)
                    {
                        txtNombre.Text = datos.Rows[0]["PRETTYNAME"].ToString();
                        string sPuesto = datos.Rows[0]["CB_NIVEL2"].ToString();
                        if (sPuesto == "850EMP")
                        {
                            cbbPlanta.SelectedValue = "EMPN";
                            cbbNivel.SelectedValue = "MAT";
                        }

                        datos = TressLogica.ConsultaImagen(img);
                        if(datos.Rows.Count > 0)
                            pbxFoto.Image = byteArrayToImage((byte[])datos.Rows[0]["IM_BLOB"]);
                    }
                    else
                    {
                        Inicio();
                        cbbOperador.Text = sOperador;
                    }
                    
                }
                txtNombre.Focus();
            }
            else
                cbbOperador.SelectedIndex = -1;
        }
       
        private void cbbOperador_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void cbbOperador_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(cbbOperador.Text) && !string.IsNullOrWhiteSpace(cbbOperador.Text))
            if(cbbOperador.SelectedIndex != -1)
            {
                OperadorLogica oper = new OperadorLogica();
                string sOperador = cbbOperador.SelectedValue.ToString();

                if (sOperador.Length < 6)
                    sOperador = sOperador.PadLeft(6, '0');

                oper.Operador = sOperador;
                DataTable datos = OperadorLogica.Consultar(oper);
                if (datos.Rows.Count != 0)
                {
                    cbbOperador.SelectedValue = datos.Rows[0]["empleado"].ToString();
                    txtNombre.Text = datos.Rows[0]["nombre"].ToString();
                    if (!string.IsNullOrEmpty(datos.Rows[0]["f_ingreso"].ToString()))
                        txtFecha.Text = datos.Rows[0].Field<DateTime>(9).ToString("dd/MM/yyyy");
                    else
                    {
                        dtpFecha.ResetText();
                        txtFecha.Clear();
                    }
                    if (datos.Rows[0]["activo"].ToString() == "1")
                        chbActivo.Checked = true;
                    else
                        chbActivo.Checked = false;
                    cbbPlanta.SelectedValue = datos.Rows[0]["planta"].ToString();
                    if(!string.IsNullOrEmpty(datos.Rows[0]["planta"].ToString()))
                    {
                        cbbLinea.ResetText();
                        LineaLogica line = new LineaLogica();
                        line.Planta = datos.Rows[0]["planta"].ToString();
                        DataTable data = LineaLogica.LineaPlanta(line);
                        cbbLinea.DataSource = data;
                        cbbLinea.ValueMember = "linea";
                        cbbLinea.DisplayMember = "linea";
                        cbbLinea.SelectedIndex = -1;
                    }

                    cbbLinea.SelectedValue = datos.Rows[0]["linea"].ToString();
                    cbbNivel.SelectedValue = datos.Rows[0]["nivel"].ToString();
                    cbbTurno.SelectedValue = datos.Rows[0]["turno"].ToString();

                    TressLogica img = new TressLogica();
                    img.Codigo = int.Parse(sOperador);
                    datos = TressLogica.ConsultaImagen(img);
                    if (datos.Rows.Count > 0)
                    {
                        pbxFoto.Image = byteArrayToImage((byte[])datos.Rows[0]["IM_BLOB"]);
                    }
                }
                else
                {
                    Inicio();
                    cbbOperador.Text = sOperador;
                }
                txtNombre.Focus();
            }
        }

        #endregion

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cbbLinea.ResetText();
            LineaLogica line = new LineaLogica();
            line.Planta = cbbPlanta.SelectedValue.ToString();
            DataTable data = LineaLogica.LineaPlanta(line);
            cbbLinea.DataSource = data;
            cbbLinea.ValueMember = "linea";
            cbbLinea.DisplayMember = "linea";
            cbbLinea.SelectedIndex = -1;
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbbOperador.Text))
            {
                return;
            }
        
            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "30") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                OperadorLogica oper = new OperadorLogica();
                if (cbbOperador.SelectedIndex != -1)
                    oper.Operador = cbbOperador.SelectedValue.ToString();
                else
                    oper.Operador = cbbOperador.Text;

                DataTable dtOrbis = OperadorLogica.ConsultarOrbis(oper);
                if(dtOrbis.Rows.Count != 0)
                {
                    string sNombre = dtOrbis.Rows[0]["nombre"].ToString();
                    string sApellido = dtOrbis.Rows[0]["apellidos"].ToString();
                    string sPlanta = dtOrbis.Rows[0]["idPlanta"].ToString();
                    string sLinea = dtOrbis.Rows[0]["Linea"].ToString();
                    string sTurno = dtOrbis.Rows[0]["idTurno"].ToString();
                    string sEstatus = dtOrbis.Rows[0]["estatus"].ToString();

                    if(!string.IsNullOrEmpty(sNombre) && !string.IsNullOrEmpty(sApellido))
                    {
                        txtNombre.Text = sNombre.ToUpper().TrimStart().TrimEnd() + " " + sApellido.ToUpper().TrimStart().TrimEnd();
                    }

                    if (sLinea.IndexOf("TNR") != -1)
                    {
                        sLinea = sLinea.Substring(3);
                    }

                    if(!string.IsNullOrEmpty(sPlanta))
                    {
                        if (sPlanta == "1")
                            sPlanta = "TON1";
                        else
                        {
                            if (sLinea.IndexOf("A") != -1)
                                sLinea = sLinea.Substring(sLinea.IndexOf("A"));
                            if (sLinea.IndexOf("-A") != -1)
                                sLinea = sLinea.Substring(sLinea.IndexOf("-A"));

                            if (sLinea.IndexOf("TII") != -1)
                            {
                                sPlanta = "NIC3";
                            }
                            else
                            {
                                int iLinea = 0;
                                if (int.TryParse(sLinea, out iLinea))
                                {
                                    if (iLinea > 41)
                                        sPlanta = "NIC2";
                                    else
                                        sPlanta = "NIC3";
                                }
                                else
                                    sPlanta = "NIC3";
                            }   
                        }

                        cbbPlanta.SelectedValue = sPlanta;

                        LineaLogica line = new LineaLogica();
                        line.Planta = sPlanta;
                        DataTable data = LineaLogica.LineaPlanta(line);
                        cbbLinea.DataSource = data;
                        cbbLinea.ValueMember = "linea";
                        cbbLinea.DisplayMember = "linea";
                        cbbLinea.SelectedValue = sLinea;

                        if (sEstatus == "1" || sEstatus == "True")
                            chbActivo.Checked = true;
                        else
                            chbActivo.Checked = false;

                    }

                    cbbTurno.SelectedValue = sTurno;

                    _lbCambio = true;
                }
                else
                {
                    MessageBox.Show("El Empleado no se encuentra registrado en Orbis", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(),"Error " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(_lsFechaAnt))
                txtFecha.Text = dtpFecha.Text;
        }
    }
}
