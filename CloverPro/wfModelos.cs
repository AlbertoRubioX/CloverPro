using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Logica;

namespace CloverPro
{
    public partial class wfModelos : Form
    {
        public bool _lbCambio;
        private bool _lbCambioDet;
        private bool _lbCopy;
        private bool _lbRevStd;
        private string _lsNombreAnt;
        private int _liEstatusAnt;
        private string _lsOperaAnt;
        private string _lsDuraAnt;
        private string _lsTaktAnt;
        private string _lsCoatAnt;
        private bool _lbGuiaAnt;
        private bool _lbRetrAnt;
        private bool _lbMylarAnt;
        private bool _lbMultiAnt;
        private bool _lbRobotAnt;
        private string _lsGuiaAnt;
        private string _lsRetrAnt;
        private string _lsMylarAnt;
        private string _lsMultiAnt;
        private string _lsFormatoStd;
        private string _lsFile;
        private string _lsArchivoStd;
        private string _lsArchivoPath;
        private string _lsVAidAnt;

        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;

        private string _lsProceso = "CAT050";

        public wfModelos()
        {
            InitializeComponent();
            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }

        #region regInicio
        private void wfModelos_Load(object sender, EventArgs e)
        {
            Inicio();
        }

        private void Inicio()
        {
            cbbModelo.ResetText();
            System.Data.DataTable data = ModeloLogica.Listar();
            cbbModelo.DataSource = data;
            cbbModelo.ValueMember = "modelo";
            cbbModelo.DisplayMember = "modelo";
            cbbModelo.SelectedIndex = -1;

            Dictionary<string, string> Tipo = new Dictionary<string, string>();
            Tipo.Add("VE", "VE");
            Tipo.Add("CVE", "CVE");
            Tipo.Add("WVE", "WVE");
            Tipo.Add("SVE", "SVE");
            Tipo.Add("NVE", "NVE");
            Tipo.Add("CNV", "CNVE");
            Tipo.Add("WNV", "WNVE");
            Tipo.Add("SNV", "SNVE");
            Tipo.Add("DAM", "DAMAGED");
            cbbTipo.DataSource = new BindingSource(Tipo, null);
            cbbTipo.DisplayMember = "Value";
            cbbTipo.ValueMember = "Key";
            cbbTipo.SelectedIndex = -1;

            cbbEstatus.ResetText();
            Dictionary<string, string> Est = new Dictionary<string, string>();
            Est.Add("A", "Activo");
            Est.Add("D", "Descontinuado");
            cbbEstatus.DataSource = new BindingSource(Est, null);
            cbbEstatus.DisplayMember = "Value";
            cbbEstatus.ValueMember = "Key";
            cbbEstatus.SelectedIndex = 0;

            cbbPlanta.ResetText();
            cbbPlanta.DataSource = PlantaLogica.Listar();
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.SelectedIndex = -1;

            txtNombre.Clear();

            chbRevStd.Checked = false;
            txtUltRev.Clear();

            if(GlobalVar.gsUsuario == "ADMINP")
            {
                chbRevStd.Enabled = true;
                txtUltRev.Enabled = true;
            }

            chbConversion.Enabled = true;
            chbConversion.Checked = false;
            txtCRC.Enabled = true;
            txtCRC.Clear();

            txtCantop.Clear();
            txtCantop.Enabled = false;
            txtTackt.Clear();
            txtDuracion.Clear();
            txtCoating.Clear();

            chbGuia.Checked = true;
            txtGuia.Text = "1";
            txtGuia.Enabled = true;

            chbRetra.Checked = false;
            txtRetra.Clear();
            txtRetra.Enabled = false;

            chbMylar.Checked = false;
            txtMylar.Clear();
            txtMylar.Enabled = false;

            chbMulti.Checked = false;
            txtMulti.Clear();
            txtMulti.Enabled = false;

            chbRobot.Checked = false;

            dgwEstaciones.Enabled = true;
            dgwEstaciones.DataSource = null;
            dgwRemove.DataSource = null;
            CargarEstaciones();

            dgwModelos.Enabled = true;
            dgwModelos.DataSource = null;
            dgwRemoveMod.DataSource = null;
            CargarRelacionMod();

            _lsNombreAnt = null;
            _liEstatusAnt = 0;
            _lsOperaAnt = null;
            _lsDuraAnt = null;
            _lsTaktAnt = null;
            _lsCoatAnt = null;
            _lbGuiaAnt = true;
            _lbRetrAnt = false;
            _lbMultiAnt = false;
            _lbRobotAnt = false;
            _lsGuiaAnt = "1";
            _lsRetrAnt = null;
            _lsMultiAnt = null;
            _lsFormatoStd = null;
            _lsVAidAnt = null;
            _lbCambio = false;
            _lbCambioDet = false;
            _lbCopy = false;
            _lbRevStd = false; // Formato importado de RevisionStd

            tabControl1.SelectedIndex = 0;
            cbbModelo.Focus();

        }

        private void wfModelos_Activated(object sender, EventArgs e)
        {
            cbbModelo.Focus();
        }
        #endregion

        #region regGuardar
        private bool Valida()
        {
            bool bValida = false;

            if (!_lbCambio && !_lbCambioDet && !_lbCopy)
                return bValida;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "10") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bValida;
            }

            if (string.IsNullOrEmpty(cbbModelo.Text))
            {
                MessageBox.Show("No se a especificado la clave del Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbModelo.Focus();
                return bValida;
            }

            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("No se a especificado el nombre del Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Focus();
                return bValida;
            }

            if(cbbTipo.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de Especificar del Core ", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbTipo.Focus();
                return bValida;
            }

            if (cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de Especificar la Planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }

            if (chbGuia.Checked && (string.IsNullOrEmpty(txtGuia.Text) || Convert.ToInt32(txtGuia.Text) == 0))
            {
                MessageBox.Show("Favor de Indicar la Cantidad", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGuia.Focus();
                return bValida;
            }
            if (chbRetra.Checked && (string.IsNullOrEmpty(txtRetra.Text) || Convert.ToInt32(txtRetra.Text) == 0))
            {
                MessageBox.Show("Favor de Indicar la Cantidad", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRetra.Focus();
                return bValida;
            }

            if (chbMylar.Checked && (string.IsNullOrEmpty(txtMylar.Text) || Convert.ToInt32(txtMylar.Text) == 0))
            {
                MessageBox.Show("Favor de Indicar la Cantidad", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMylar.Focus();
                return bValida;
            }

            if (chbMulti.Checked && (string.IsNullOrEmpty(txtMulti.Text) || Convert.ToInt32(txtMulti.Text) == 0))
            {
                MessageBox.Show("Favor de Indicar la Cantidad", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMulti.Focus();
                return bValida;
            }

            if(chbRevStd.Checked && string.IsNullOrEmpty(txtUltRev.Text))
            {

                MessageBox.Show("La Ultima Revision no puede estar vacía. Verificar el Formato", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUltRev.Focus();
                return bValida;
            }
            if (chbConversion.Checked && string.IsNullOrEmpty(txtCRC.Text))
            {

                MessageBox.Show("El CRC de Conversión no puede estar vacío. Verificar el Formato", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUltRev.Focus();
                return bValida;
            }

            if (dgwEstaciones.Rows.Count == 1)
            {
                MessageBox.Show("Favor de Especificar las Estaciones del Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return bValida;
            }
            else
            {
                int iCont = 0;
                int iPPh = 0;
                foreach (DataGridViewRow row in dgwEstaciones.Rows)
                {
                    if (row.Index == dgwEstaciones.Rows.Count - 1)
                        break;

                    if (dgwEstaciones.IsCurrentRowDirty)
                        dgwEstaciones.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    string sEstacion = row.Cells[2].Value.ToString();
                    string sNivel = row.Cells[5].Value.ToString();

                    if (string.IsNullOrEmpty(sNivel))
                        iPPh++;
                    

                    iCont += Convert.ToInt32(row.Cells[4].Value);
                }

                if (iCont == 0)
                {
                    MessageBox.Show("Favor de Especificar la Cantidad de Operadores por Estación", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return bValida;
                }
                if(iPPh > 0 && cbbPlanta.SelectedValue.ToString() == "MON")
                {
                    MessageBox.Show("Favor de indicar el Nivel PPH requerdio en cada Estación", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return bValida;
                }

                return true;
            }

            if (dgwModelos.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgwModelos.Rows)
                {
                    if (row.Index == dgwModelos.Rows.Count - 1)
                        break;

                    if (dgwModelos.IsCurrentRowDirty)
                        dgwModelos.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    if(string.IsNullOrEmpty(row.Cells[2].Value.ToString()))
                    {
                        ModeloRemove(dgwModelos.CurrentRow);
                    }

                }
                return true;
            }
        }
        private bool Guardar()
        {
            try
            {
                if (!Valida())
                    return false;

                //BORRA ESTACIONES ELIMINADAS
                foreach (DataGridViewRow row in dgwRemove.Rows)
                {
                    string sModelo = row.Cells[0].Value.ToString();
                    int iCons = Convert.ToInt32(row.Cells[1].Value);
                    if (!string.IsNullOrEmpty(sModelo))
                    {
                        ModestaLogica rmode = new ModestaLogica();
                        rmode.Modelo = sModelo;
                        rmode.Consec = iCons;
                        try
                        {
                            ModestaLogica.Eliminar(rmode);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "ModestaLogica.Eliminar(mode);" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                }

                ModeloLogica mod = new ModeloLogica();
                mod.Modelo = cbbModelo.Text.ToString().ToUpper();
                mod.Descrip = txtNombre.Text.ToString();
                mod.Estatus = cbbEstatus.SelectedValue.ToString();
                mod.Tipo = cbbTipo.SelectedValue.ToString();
                mod.Planta = cbbPlanta.SelectedValue.ToString();
                if (chbRevStd.Checked) // IMPORTADO DE REVISION STD'S \\
                    mod.RevStd = "1";
                else
                    mod.RevStd = "0";
                mod.UltRev = txtUltRev.Text.ToString().TrimEnd();

                if (!string.IsNullOrEmpty(_lsArchivoStd))
                    mod.FormatoStd = _lsArchivoStd;
                //if (!string.IsNullOrEmpty(_lsArchivoPath))
                //    mod.FormatoPath = _lsArchivoPath;

                if (chbConversion.Checked) // CONVERSION \\
                    mod.Conversion = "1";
                else
                    mod.Conversion = "0";
                mod.CRC = txtCRC.Text.ToString().TrimEnd();
                mod.FormatoPath = txtLayOutFolder.Text.ToString();
                mod.VisualAidFolder = txtVAFolder.Text.ToString();

                double dCant = 0;
                if (Double.TryParse(txtTackt.Text, out dCant))
                    mod.Tasktime = dCant;
                else
                    mod.Tasktime = 0;
                dCant = 0;
                if (Double.TryParse(txtDuracion.Text, out dCant))
                    mod.Duracion = dCant;
                else
                    mod.Duracion = 0;

                dCant = 0;
                if (Double.TryParse(txtCoating.Text, out dCant))
                    mod.Coating = dCant;
                else
                    mod.Coating = 0;

                if (chbGuia.Checked)
                    mod.CantGuia = Convert.ToInt32(txtGuia.Text);
                else
                    mod.CantGuia = 0;
                if (chbRetra.Checked)
                    mod.CantRetra = Convert.ToInt32(txtRetra.Text);
                else
                    mod.CantRetra = 0;
                if (chbMylar.Checked)
                    mod.CantMylar = Convert.ToInt32(txtMylar.Text);
                else
                    mod.CantMylar = 0;
                if (chbMulti.Checked)
                    mod.CantMulti = Convert.ToInt32(txtMulti.Text);
                else
                    mod.CantMulti = 0;
                if (chbRobot.Checked)
                    mod.Robot = "1";
                else
                    mod.Robot = "0";
                

                //ESTACIONES

                int iCantOp = 0;
                foreach (DataGridViewRow row in dgwEstaciones.Rows)
                {
                    //if (row.Index == dgwEstaciones.Rows.Count - 1)
                    //    break;

                    if (dgwEstaciones.IsCurrentRowDirty)
                        dgwEstaciones.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    try
                    {
                        if (string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
                            continue;

                        string sModelo = mod.Modelo;
                        int iCons = Convert.ToInt32(row.Cells[1].Value);
                        string sEstacion = Convert.ToString(row.Cells[2].Value);
                        string sNombre = Convert.ToString(row.Cells[3].Value);
                        int iOper = Convert.ToInt32(row.Cells[4].Value);
                        string sNivel = Convert.ToString(row.Cells[5].Value);
                        string sCodigo = Convert.ToString(row.Cells[6].Value);

                        ModestaLogica mode = new ModestaLogica();
                        mode.Modelo = sModelo;
                        mode.Consec = iCons;
                        mode.Estacion = sEstacion;
                        mode.Nombre = sNombre.ToUpper();
                        mode.Operadores = iOper;
                        mode.Nivel = sNivel;
                        mode.Codigo = sCodigo;
                        mode.Usuario = GlobalVar.gsUsuario;

                        iCantOp += iOper;
                        ModestaLogica.Guardar(mode);
                    }
                    catch (Exception ie)
                    {
                        MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "ModestaLogica.Guardar(mode)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                //BORRA MODELOS ELIMINADAS
                foreach (DataGridViewRow row in dgwRemoveMod.Rows)
                {
                    string sModelo = row.Cells[0].Value.ToString();
                    int iCons = Convert.ToInt32(row.Cells[1].Value);
                    if (!string.IsNullOrEmpty(sModelo))
                    {
                        ModerelaLogica rmode = new ModerelaLogica();
                        rmode.Modelo = sModelo;
                        rmode.Consec = iCons;
                        try
                        {
                            ModerelaLogica.Eliminar(rmode);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "ModestaLogica.Eliminar(mode);" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                }

                foreach (DataGridViewRow row in dgwModelos.Rows)
                {
                    if (row.Index == dgwModelos.Rows.Count - 1)
                        break;

                    if (dgwModelos.IsCurrentRowDirty)
                        dgwModelos.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    if (row.Cells[2].Value == null)
                        continue;

                    try
                    {
                        int iCons = 0;
                        if (int.TryParse(row.Cells[1].Value.ToString(), out iCons))
                            iCons = Convert.ToInt32(row.Cells[1].Value);

                        string sModelo = mod.Modelo;
                        string sModRel = Convert.ToString(row.Cells[2].Value);
                        string sNota = Convert.ToString(row.Cells[3].Value);

                        ModerelaLogica modre = new ModerelaLogica();
                        modre.Modelo = sModelo;
                        modre.Consec = iCons;
                        modre.Moderela = sModRel;
                        modre.Nota = sNota.ToUpper();
                        modre.Usuario = GlobalVar.gsUsuario;
                        ModerelaLogica.Guardar(modre);
                    }
                    catch (Exception ie)
                    {
                        MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "ModerelaLogica.Guardar(modre)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                //MODELO
                mod.CantOper = iCantOp;
                mod.TotalOper = iCantOp + mod.CantGuia + mod.CantRetra;
                mod.Usuario = GlobalVar.gsUsuario;

                if (ModeloLogica.Guardar(mod) == 0)
                {
                    MessageBox.Show("Error al intentar guardar el Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                return true;
            }
            catch (Exception ie)
            {
                MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "EstacionLogica.Guardar(mod)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region regBotones
        private void btSave_Click(object sender, EventArgs e)
        {
            if (Guardar())
                Inicio();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string sModelo = cbbModelo.Text.ToString().ToUpper();
            if (string.IsNullOrEmpty(sModelo) || string.IsNullOrWhiteSpace(sModelo))
                return;

            string sPermiso = _lsProceso + "20";
            if(chbRevStd.Checked)
                sPermiso = _lsProceso + "70";

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, sPermiso) == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            DialogResult Result = MessageBox.Show("No podra recuperar la información del Modelo Borrado. Desea Continuar ?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            if (Result == DialogResult.Yes)
            {
                ModeloLogica mod = new ModeloLogica();
                mod.Modelo = sModelo;
                try
                {
                    //if(ModeloLogica.AntesDeBorrar(mod))
                    //{
                    //    MessageBox.Show("No puede borrar el modelo debido a que se encuentra registrado en LineUp", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //    return;
                    //}

                    ModeloLogica.Borrar(mod);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "" + Environment.NewLine + ex.ToString(), "Error " + Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Inicio();
            }

        }
        private void btRemove_Click(object sender, EventArgs e)
        {
            if (chbRevStd.Checked)
            {
                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, "CAT05060") == false)
                {
                    MessageBox.Show("No se permite modificar la Revision Standar", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if (tabControl1.SelectedIndex == 1)
            {
                if (dgwModelos.SelectedRows.Count == 0)
                    return;
                else
                {
                    if (string.IsNullOrEmpty(dgwModelos.SelectedCells[2].Value.ToString()))
                        return;
                }

                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "60") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DialogResult Result = MessageBox.Show("Desea Eliminar el Modelo Relacionado " + dgwModelos.SelectedCells[2].Value.ToString() + " ?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    ModeloRemove(dgwModelos.CurrentRow);
                    CargarRelacionMod();
                }
            }
            else
            {
                if (dgwEstaciones.SelectedRows.Count == 0)
                    return;
                else
                {
                    if (string.IsNullOrEmpty(dgwEstaciones.SelectedCells[2].Value.ToString()))
                        return;
                }

                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "30") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DialogResult Result = MessageBox.Show("Desea Eliminar la Estación " + dgwEstaciones.SelectedCells[2].Value.ToString() + " ?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    EstacionRemove(dgwEstaciones.CurrentRow);
                    CargarEstaciones();
                }
            }

            
        }

        private void EstacionRecicla(DataGridViewRow _crow)
        {
            if (!string.IsNullOrEmpty(_crow.Cells[0].Value.ToString()))
            {
                //mandar al listado para borrar de bd al guardar cambios
                if (dgwRemove.Rows.Count == 0)
                {
                    System.Data.DataTable dtNew = new System.Data.DataTable("Eliminar");
                    dtNew.Columns.Add("modelo", typeof(string));
                    dtNew.Columns.Add("consec", typeof(int));
                    dgwRemove.DataSource = dtNew;
                }

                string sModelo = _crow.Cells[0].Value.ToString();
                int iCons = Convert.ToInt32(_crow.Cells[1].Value);

                System.Data.DataTable dt = dgwRemove.DataSource as System.Data.DataTable;
                dt.Rows.Add(sModelo, iCons);
            }
        }
        private void EstacionRemove(DataGridViewRow _crow)
        {

            EstacionRecicla(_crow);

            dgwEstaciones.Rows.Remove(_crow);
            _lbCambioDet = true;

            foreach (DataGridViewRow row in dgwEstaciones.Rows)
            {
                row.Cells[1].Value = row.Index + 1;
                if (row.Index == dgwEstaciones.Rows.Count - 1)
                    dgwEstaciones.Rows.Remove(row);
            }
        }

        private void btNew_Click(object sender, EventArgs e)
        {
            if (_lbCambio || _lbCambioDet)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios antes de limpiar pantalla?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (Guardar())
                        Inicio();
                }
                else
                {
                    if (Result == DialogResult.No)
                        Inicio();
                }
            }
            else
                Inicio();

        }
        private void btExit_Click(object sender, EventArgs e)
        {
            if (_lbCambio || _lbCambioDet)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios antes de salir?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (Guardar())
                        Close();
                }
                else
                {
                    if (Result == DialogResult.No)
                        Close();
                }
            }
            else
                Close();
            Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (_lbCambio || _lbCambioDet)
            {

                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "40") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (chbRevStd.Checked)
                {
                    MessageBox.Show("No se permite modificar la Revision Standar", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DialogResult Result = MessageBox.Show("Desea Guardar los Cambios Antes de Duplicar el Modelo?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (Guardar())
                        Duplicar();
                }
                else
                {
                    if (Result == DialogResult.No)
                        Duplicar();
                }
            }
            else
                Duplicar();
        }

        private void Duplicar()
        {
            _lbCopy = true;
            cbbModelo.ResetText();
            cbbModelo.Focus();
        }

        private void ActualizarLayout(string _asModelo)
        {
            wfCapturaPop wfCap = new wfCapturaPop(_lsProceso);
            wfCap.ShowDialog();
            string sFormato = wfCap._sClave;

            if (string.IsNullOrEmpty(sFormato))
                return;

            string sModelo = CargarNombre(sFormato);
            wfModelosPopGrid wfLayoutPop = new wfModelosPopGrid(_lsProceso);
            wfLayoutPop._sClave = sFormato;
            wfLayoutPop._sModelo = sModelo;
            wfLayoutPop.ShowDialog();

            System.Data.DataTable dtData = wfLayoutPop._dtLista;
            if (dtData.Rows.Count > 0)
            {
                DialogResult Result = MessageBox.Show("Desea actualizar el layout del Modelo?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result != DialogResult.Yes)
                    return;

                Cursor = Cursors.WaitCursor;

                foreach(DataGridViewRow row in dgwEstaciones.Rows)
                {
                    
                    EstacionRecicla(row);
                }

                dgwEstaciones.DataSource = null;
                dgwEstaciones.DataSource = dtData;
                CargarEstaciones();

                int iPos = sFormato.LastIndexOf("(");
                //formato mm-dd-yyyy
                string sFecha = sFormato.Substring(iPos + 1);
                sFecha = sFecha.Replace(")", "");
                sFecha = sFecha.Replace("NVE", "");
                sFecha = sFecha.Replace("VE", "");
                DateTime dtFecha = Convert.ToDateTime(sFecha);
                if(txtUltRev.Text != sFecha)
                {
                    txtUltRev.Text = sFecha;
                    _lbCambio = true;
                }

                string sArchivo = wfLayoutPop._sFile;
                if(!string.IsNullOrEmpty(sArchivo))
                {
                    iPos = sArchivo.LastIndexOf(@"\");
                    if (iPos != -1)
                    {
                        _lsArchivoPath = sArchivo.Substring(0, iPos);
                        _lsArchivoStd = sArchivo.Substring(iPos + 1);
                        txtLayOutFolder.Text = _lsArchivoPath;
                    }
                }

                _lbCambioDet = true;

                Cursor = Cursors.Default;
            }
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            string sModelo = cbbModelo.Text.ToString().ToUpper();
            if (string.IsNullOrEmpty(sModelo) || string.IsNullOrWhiteSpace(sModelo))
                return;

            if (cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de seleccionar la planta del modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string sModAnt = CargarNombre(sModelo);
            ModeloLogica mod = new ModeloLogica();
            mod.Modelo = sModAnt;
            if(ModeloLogica.Verificar(mod))
            {
                DialogResult Result = MessageBox.Show("Desea actualizar el layout del Modelo?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "50") == false)
                    {
                        MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    ActualizarLayout(sModelo);
                }
                
                return;
            }

            if (_lbCambio || _lbCambioDet)
            {
                

                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "50") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DialogResult Result = MessageBox.Show("Desea Guardar los Cambios Antes de Cargar el Formato ?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (Guardar())
                    {
                        if (cbbPlanta.SelectedValue.ToString() == "COL")
                            ImportarFormatoColor(sModelo);
                        else
                        {
                            if (cbbPlanta.SelectedValue.ToString() == "FUS")
                                ImportarFormatoFusor(sModelo);
                            else
                                ImportarFormato(sModelo);
                        }
                    }
                }
                else
                {
                    if (Result == DialogResult.No)
                    {
                        if (cbbPlanta.SelectedValue.ToString() == "COL")
                            ImportarFormatoColor(sModelo);
                        else
                        {
                            if (cbbPlanta.SelectedValue.ToString() == "FUS")
                                ImportarFormatoFusor(sModelo);
                            else
                                ImportarFormato(sModelo);
                        }
                    }
                }
            }
            else
            {
                if (cbbPlanta.SelectedValue.ToString() == "COL")
                    ImportarFormatoColor(sModelo);
                else
                {
                    if (cbbPlanta.SelectedValue.ToString() == "FUS")
                        ImportarFormatoFusor(sModelo);
                    else
                        ImportarFormato(sModelo);
                }
            }

        }

        #endregion

        #region regImportarLayoutColor
        private void ImportarFormatoColor(string _asModelo)
        {
            //Inicio();
            cbbModelo.Text = _asModelo;
             
            _lbRevStd = true;
            ModelosDataBound(_asModelo);

            try
            {
                System.Data.DataTable data = new System.Data.DataTable();

                Microsoft.Office.Interop.Excel.Application exApp = new Microsoft.Office.Interop.Excel.Application();
                Workbook exLibro;
                Worksheet exHoja;
                Range exRango;

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Archivos de Excel (*.xls;*.xlsx)|*.xls;*.xlsx";

                dialog.Title = "Seleccione el archivo de Excel";

                dialog.FileName = string.Empty;


                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    string sFile = dialog.FileName;

                    exLibro = exApp.Workbooks.Open(sFile);
                    exHoja = exLibro.Sheets["Concentrado"];

                    //ENCAVEZADO
                    int iCol = 1;
                    int iCol2 = 2;
                    if (exHoja.Cells.Item[iCol, 5].Value == null)
                    {
                        iCol++;
                        iCol2++;
                    }
                        

                    string sNombre = exHoja.Cells.Item[iCol, 5].Value.ToString();
                    string sLinea = exHoja.Cells.Item[iCol2, 5].Value.ToString();
                    string sFecha = exHoja.Cells.Item[iCol2, 7].Value.ToString();

                    if (sNombre.IndexOf("Modelo") != -1)
                    {
                        sNombre = sNombre.Substring(7);
                    }
                    else
                    {
                        sNombre = exHoja.Cells.Item[iCol2, 5].Value.ToString();
                        if (sNombre.IndexOf("Modelo") != -1)
                            sNombre = sNombre.Substring(sNombre.IndexOf(":") + 1);

                        sLinea = exHoja.Cells.Item[iCol, 5].Value.ToString();
                    }

                    sNombre = sNombre.TrimStart().TrimEnd().ToUpper();

                    if (sLinea.IndexOf(":") != -1)
                        sLinea = sLinea.Substring(sLinea.IndexOf(":") + 1);
                    sLinea = sLinea.TrimStart().TrimEnd();

                    if (sFecha.IndexOf(":") != -1)
                        sFecha = sFecha.Substring(sFecha.IndexOf(":") + 1);
                    sFecha = sFecha.TrimStart().TrimEnd();

                    //END ENC
                    
                    //** CONFIGURACION **\\
                    string sTakt = "0";
                    string sCantop = "0";
                    double dDura = 0;
                    double dCoat = 0;

                    iCol = 6;
                    iCol2 = 12;
                    if (exHoja.Cells.Item[iCol, 11].Value != null)
                    {
                        string sRecurso = exHoja.Cells.Item[iCol, 11].Value;
                        sRecurso = sRecurso.ToUpper();
                        if (sRecurso.IndexOf("GUIA") != -1)
                            sCantop = exHoja.Cells.Item[iCol, iCol2].Value.ToString();
                        else
                        {
                            iCol++;
                            sRecurso = exHoja.Cells.Item[iCol, 11].Value;
                            sRecurso = sRecurso.ToUpper();
                            if (sRecurso.IndexOf("GUIA") != -1)
                                sCantop = exHoja.Cells.Item[iCol, iCol2].Value.ToString();
                            else
                            {
                                iCol++;
                                sRecurso = exHoja.Cells.Item[iCol, 11].Value;
                                sRecurso = sRecurso.ToUpper();
                                if (sRecurso.IndexOf("GUIA") != -1)
                                    sCantop = exHoja.Cells.Item[iCol, iCol2].Value.ToString();
                            }
                        }

                    }

                    txtCantop.Text = sCantop;
                    txtDuracion.Text = dDura.ToString();
                    txtTackt.Text = sTakt;
                    txtCoating.Text = dCoat.ToString();
                    
                    //* ESTACIONES *//
                    exHoja = exLibro.Sheets["Concentrado"];
                    exHoja.Select();
                    exRango = exHoja.get_Range("B5", "I35");

                    int iRows = exRango.Rows.Count;
                    int iCols = exRango.Columns.Count;
                    string sEstAnt = string.Empty;
                    for (int i = 1; i <= iRows; i++)
                    {
                        // AGREGA ESTACIONES \\
                        string sEstacion = string.Empty;
                        if (exRango.Cells[i, 1].Value == null)
                            continue;
                        
                        sEstacion = exRango.Cells[i, 1].Value.ToString();

                        if (sEstacion == "821")
                            continue;

                        if (exRango.Cells[i, 2].Value == null)
                            continue;

                        string sDescrip = exRango.Cells[i, 2].Value.ToString();
                        string sCantidad = "0";
                        
                        int iCant = 0;
                        int iColCt = 8;
                        if (exRango.Cells[i, 8].Value == null)
                            iColCt++;

                        if (exRango.Cells[i, iColCt].Value == null)
                            sCantidad = "1";
                        else
                        {
                            if (int.TryParse(exRango.Cells[i, iColCt].Value.ToString(), out iCant))
                                sCantidad = Convert.ToString(iCant);
                        }

                        int iEst = 0;
                        int iEstacion = 0;
                        int iCantOp = 0;
                        if (Int32.TryParse(sEstacion, out iEst))
                        {
                            iEstacion = iEst;
                            if (!int.TryParse(sCantidad, out iCantOp))
                                iCantOp = 0;

                            AgregarFila(0, sEstacion, sDescrip.ToUpper(), iCantOp,null,null);
                        }
                        else
                        {
                            if (sEstacion.ToUpper().IndexOf("Y") != -1)//16 y 17
                            {
                                while (!string.IsNullOrEmpty(sEstacion))
                                {
                                    string sEst = string.Empty;
                                    int iPos = sEstacion.ToUpper().IndexOf("Y");
                                    if (iPos != -1)
                                    {
                                        sEst = sEstacion.Substring(0, iPos).TrimEnd();
                                    }
                                    else
                                    {
                                        sEst = sEstacion;
                                        sEstacion = string.Empty;
                                    }

                                    if (!string.IsNullOrEmpty(sEst))
                                    {
                                        if (!string.IsNullOrEmpty(sEst))
                                            AgregarFila(0, sEst, sDescrip.ToUpper(), 1, null, null);

                                        sEstacion = sEstacion.Substring(iPos + 1).TrimStart();
                                    }
                                }
                            }
                            else
                                continue;
                        }
                    }

                    string sClave = cbbModelo.Text;
                    sClave = sClave.ToUpper().TrimEnd();
                    sClave = sClave.Replace(" ", "_");
                    cbbModelo.Text = sClave;

                    txtNombre.Text = sNombre + "("+sLinea +")";
                    txtUltRev.Text = sFecha;

                    chbRevStd.Checked = true;

                    txtGuia.Text = "1";
                    chbGuia.Checked = true;
                    
                    exLibro.Close(true);
                    exApp.Quit();

                    DialogResult Result = MessageBox.Show("Desea Cargar los modelos del LayOut ?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (Result == DialogResult.Yes)
                    {
                        ListarRelacionadosColor();
                    }

                    Cursor = Cursors.Default;
                    _lbCambio = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "Archivo sin formato Estandar" + Environment.NewLine + "ImportarFormato(" + _asModelo + ") ..." + ex.ToString(), "ERROR " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cursor = Cursors.Default;
                return;
            }
        }

        private void ListarRelacionadosColor()
        {
            try
            {
                System.Data.DataTable data = new System.Data.DataTable();

                Microsoft.Office.Interop.Excel.Application exApp = new Microsoft.Office.Interop.Excel.Application();
                Workbook exLibro;
                Worksheet exHoja;
                Range exRango;

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Archivos de Excel (*.xls;*.xlsx)|*.xls;*.xlsx";

                dialog.Title = "Seleccione el archivo de Excel";

                dialog.FileName = string.Empty;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    string sFile = dialog.FileName;

                    exLibro = exApp.Workbooks.Open(sFile);
                    exHoja = exLibro.Sheets["Sheet1"];
                    exHoja.Select();
                    exRango = exHoja.get_Range("A1", "B100");

                    int iRows = exRango.Rows.Count;
                    int iCols = exRango.Columns.Count;
                    string sEstAnt = string.Empty;
                    for (int i = 1; i <= iRows; i++)
                    {
                        // AGREGA ESTACIONES \\
                        string sModelo = string.Empty;
                        string sNota = string.Empty;
                        if (exRango.Cells[i, 1].Value == null)
                            continue;
                        
                        sModelo = exRango.Cells[i, 1].Value.ToString();
                        sNota = exRango.Cells[i, 2].Value.ToString();

                        sModelo = sModelo.ToUpper().TrimStart().TrimEnd();
                        sNota = sNota.ToUpper().TrimStart().TrimEnd();

                        int iCont = 0;
                        foreach(DataGridViewRow row in dgwEstaciones.Rows)
                        {
                            if(row.Cells[2].Value != null)
                            {
                                if (sModelo == row.Cells[2].Value.ToString())
                                {
                                    iCont++;
                                    break;
                                }
                            }
                        }

                        if (iCont > 0)
                            continue;

                        System.Data.DataTable dt = dgwModelos.DataSource as System.Data.DataTable;
                        dt.Rows.Add(null, 0, sModelo, sNota);
                    }

                    exLibro.Close(true);
                    exApp.Quit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "Archivo sin formato Estandar" + Environment.NewLine + "ImportarFormatoColor() ..." + ex.ToString(), "ERROR " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cursor = Cursors.Default;
                return;
            }
        }
        #endregion

        #region regImportarLoayoutFusor
        private void ImportarFormatoFusor(string _asModelo)
        {

            Inicio();
            cbbModelo.Text = _asModelo;

            _lbRevStd = true;
            ModelosDataBound(_asModelo);

            try
            {
                System.Data.DataTable data = new System.Data.DataTable();

                Microsoft.Office.Interop.Excel.Application exApp = new Microsoft.Office.Interop.Excel.Application();
                Workbook exLibro;
                Worksheet exHoja;
                Range exRango;

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Archivos de Excel (*.xls;*.xlsx)|*.xls;*.xlsx";

                dialog.Title = "Seleccione el archivo de Excel";

                dialog.FileName = string.Empty;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    string sFile = dialog.FileName;

                    exLibro = exApp.Workbooks.Open(sFile);
                    exHoja = exLibro.Sheets[_asModelo];

                    int iCol = 12;
                    int iCol2 = 15;

                    string sNombre = exHoja.Cells.Item[2, 4].Value.ToString();
                    if (string.IsNullOrEmpty(exHoja.Cells.Item[2, 11].Value))
                    {
                        iCol++;
                        iCol2++;
                    }

                    //** CONFIGURACION **\\
                    string sTakt = "0";
                    if (exHoja.Cells.Item[2, iCol].Value != null)
                        sTakt = exHoja.Cells.Item[2, iCol].Value.ToString();

                    string sDuracion = "0";
                    if (exHoja.Cells.Item[2, iCol2].Value != null)
                        sDuracion = exHoja.Cells.Item[2, iCol2].Value.ToString();

                    string sCantop = "0";
                    if (exHoja.Cells.Item[4, iCol].Value != null)
                        sCantop = exHoja.Cells.Item[4, iCol].Value.ToString();

                    string sCoating = "0";
                    if (exHoja.Cells.Item[4, iCol2].Value != null)
                        sCoating = exHoja.Cells.Item[4, iCol2].Value.ToString();

                    int iOper = 0;
                    if (!int.TryParse(sCantop, out iOper))
                        iOper = 0;
                    double dDura = 0;
                    if (!double.TryParse(sDuracion, out dDura))
                        dDura = 0;
                    double dCoat = 0;
                    if (!double.TryParse(sCoating, out dCoat))
                        dCoat = 0;
                    dDura = Math.Round(dDura, 0);
                    dCoat = Math.Round(dCoat, 4);

                    txtCantop.Text = Convert.ToString(iOper);
                    txtDuracion.Text = dDura.ToString();
                    txtTackt.Text = sTakt;
                    txtCoating.Text = dCoat.ToString();


                    txtNombre.Text = sNombre.ToUpper();
                    //** TIPO CORE **//
                    cbbTipo.SelectedIndex = 0;
                    

                    //* MODELOS *//
                    string sNota = String.Empty;
                    if (!string.IsNullOrEmpty(exHoja.Cells.Item[3, 4].Value))
                        sNota = exHoja.Cells.Item[3, 4].Value.ToString();
                    else
                    {
                        if (!string.IsNullOrEmpty(exHoja.Cells.Item[3, 3].Value))
                            sNota = exHoja.Cells.Item[3, 3].Value.ToString();
                        else
                        {
                            if (!string.IsNullOrEmpty(exHoja.Cells.Item[3, 2].Value))
                                sNota = exHoja.Cells.Item[3, 2].Value.ToString();
                            else
                            {
                                sNota = sNombre;
                                sNota = sNota.TrimEnd();
                            }
                        }
                    }

                    //* ESTACIONES *//
                    exHoja = exLibro.Sheets[_asModelo];
                    exHoja.Select();
                    exRango = exHoja.get_Range("B5", "J60");

                    int iRows = exRango.Rows.Count;
                    int iCols = exRango.Columns.Count;
                    string sEstAnt = string.Empty;
                    for (int i = 1; i <= iRows; i++)
                    {
                        // AGREGA ESTACIONES \\
                        string sEstacion = string.Empty;
                        if (exRango.Cells[i, 1].Value == null)
                        {
                            if (string.IsNullOrEmpty(sEstAnt))
                                continue;
                        }
                        else
                            sEstacion = exRango.Cells[i, 2].Value.ToString();



                        if (exRango.Cells[i, 2].Value == null)
                        {
                            sEstAnt = sEstacion;
                            continue;
                        }

                        if (string.IsNullOrEmpty(sEstacion))
                            sEstacion = sEstAnt;

                        string sDescrip = exRango.Cells[i, 2].Value.ToString();
                        string sCantidad = "0";
                        
                        int iCant = 0;
                        int iColCt = 6;
                        if (exRango.Cells[i, 6].Value == null)
                            iColCt++;


                        if (exRango.Cells[i, iColCt].Value == null)
                            sCantidad = "1";
                        else
                        {
                            if (int.TryParse(exRango.Cells[i, iColCt].Value.ToString(), out iCant))
                                sCantidad = Convert.ToString(iCant);
                        }

                        int iEst = 0;
                        int iEstacion = 0;
                        int iCantOp = 0;
                        if (Int32.TryParse(sEstacion, out iEst))
                        {
                            iEstacion = iEst;
                            if (!int.TryParse(sCantidad, out iCantOp))
                                iCantOp = 0;

                            AgregarFila(0, sEstacion, sDescrip.ToUpper(), iCantOp, null, null);
                        }
                        else
                        {
                            if (sEstacion.ToUpper().IndexOf("Y") != -1)//16 y 17
                            {
                                while (!string.IsNullOrEmpty(sEstacion))
                                {
                                    string sEst = string.Empty;
                                    int iPos = sEstacion.ToUpper().IndexOf("Y");
                                    if (iPos != -1)
                                    {
                                        sEst = sEstacion.Substring(0, iPos).TrimEnd();
                                    }
                                    else
                                    {
                                        sEst = sEstacion;
                                        sEstacion = string.Empty;
                                    }

                                    if (!string.IsNullOrEmpty(sEst))
                                    {
                                        if (!string.IsNullOrEmpty(sEst))
                                            AgregarFila(0, sEst, sDescrip.ToUpper(), 1, null, null);

                                        sEstacion = sEstacion.Substring(iPos + 1).TrimStart();
                                    }
                                }
                            }
                        }
                    }

                    //CONFIG CANT OP \\
                    string sRwk = string.Empty;
                    string sGuia = string.Empty;
                    string sMylar = string.Empty;
                    for (int i = 1; i <= iRows; i++)
                    {
                        if (exRango.Cells[i, 1].Value == null)
                        {
                            if (exRango.Cells[i, 7].Value != null || exRango.Cells[i, 8].Value != null)
                            { // CONFIGURACION STD
                                int iColRw = 8;
                                int iColCt = 7;
                                if (exRango.Cells[i, 8].Value == null)
                                {
                                    iColRw--;
                                    iColCt--;
                                }

                                string sTipo = exRango.Cells[i, iColRw].Value.ToString();
                                if (sTipo.ToUpper() == "RETRABAJADOR" || sTipo.ToUpper() == "RETRABAJO" || sTipo.ToUpper() == "RWK")
                                    sRwk = exRango.Cells[i, iColCt].Value.ToString();
                                if (sTipo.ToUpper() == "GUIA" || sTipo.ToUpper() == "GUIDE")
                                    sGuia = exRango.Cells[i, iColCt].Value.ToString();
                                if (sTipo.ToUpper().IndexOf("MYLAR") != -1)
                                    sMylar = exRango.Cells[i, iColCt].Value.ToString();
                            }
                            continue;
                        }
                    }

                    int iGuia = 0;
                    if (Int32.TryParse(sGuia, out iGuia))
                    {
                        chbGuia.Checked = true;
                        txtGuia.Text = iGuia.ToString();
                    }
                    else
                    {
                        chbGuia.Checked = false;
                        txtGuia.Text = "0";
                    }

                    int iRwk = 0;
                    if (Int32.TryParse(sRwk, out iRwk))
                    {
                        chbRetra.Checked = true;
                        txtRetra.Text = iRwk.ToString();
                    }

                    int iMylar = 0;
                    if (Int32.TryParse(sMylar, out iMylar))
                    {
                        chbMylar.Checked = true;
                        txtMylar.Text = iMylar.ToString();
                    }

                    exLibro.Close(true);
                    exApp.Quit();

                    CargarNombre(_asModelo);

                    ListarRelacionadosFusor(sNota);

                    chbRevStd.Checked = true;
                    cbbPlanta.SelectedValue = "FUS";

                    Cursor = Cursors.Default;
                    _lbCambio = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "Archivo sin formato Estandar" + Environment.NewLine + "ImportarFormato(" + _asModelo + ") ..." + ex.ToString(), "ERROR " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cursor = Cursors.Default;
                return;
            }
        }
        private void ListarRelacionadosFusor(string _asListado)
        {
            string sConver = string.Empty;
            string sModRel = string.Empty;
            int iPos = 0;
            

            while (!string.IsNullOrEmpty(_asListado))
            {
                sModRel = string.Empty;
                iPos = _asListado.IndexOf("/");

                if (iPos != -1)
                {
                    sModRel = _asListado.Substring(0, iPos).TrimEnd();

                }
                else
                {
                    sModRel = _asListado;
                    _asListado = string.Empty;
                }

                if (!string.IsNullOrEmpty(sModRel))
                {
                    string sNota = String.Empty;
                    int iPosN = sModRel.IndexOf("(");
                    if (iPosN != -1)
                    {
                        sNota = sModRel.Substring(iPosN).TrimStart().TrimEnd();
                        sModRel = sModRel.Substring(0, iPosN).TrimEnd();
                    }

                    if (!string.IsNullOrEmpty(sConver))
                        sNota = sConver;

                    System.Data.DataTable dt = dgwModelos.DataSource as System.Data.DataTable;
                    dt.Rows.Add(null, 0, sModRel, sNota);

                    _asListado = _asListado.Substring(iPos + 1).TrimStart();
                }
            }
        }
        #endregion

        #region regImportarLayout
        private void ObtenerCore(string _asNombre)
        {
            /* TIPO CORE
            Tipo.Add("VE", "VE");
            Tipo.Add("CVE", "CVE");
            Tipo.Add("WVE", "WVE");
            Tipo.Add("SVE", "SVE");
            Tipo.Add("NVE", "NVE");
            Tipo.Add("CNV", "CNVE");
            Tipo.Add("WNV", "WNVE");
            Tipo.Add("SNV", "SNVE");
            Tipo.Add("DAM", "DAMAGED");
            */
            if (_asNombre.IndexOf("DAM") != -1)
            {
                cbbTipo.SelectedIndex = 8;
                return;
            }
                
            if (_asNombre.IndexOf("SNVE") != -1)
            {
                cbbTipo.SelectedIndex = 7;
                return;
            }

            if (_asNombre.IndexOf("WNVE") != -1)
            {
                cbbTipo.SelectedIndex = 6;
                return;
            }

            if (_asNombre.IndexOf("CNVE") != -1)
            {
                cbbTipo.SelectedIndex = 5;
                return;
            }

            if (_asNombre.IndexOf("NVE") != -1)
            {
                cbbTipo.SelectedIndex = 4;
                return;
            }

            if (_asNombre.IndexOf("SVE") != -1)
            {
                cbbTipo.SelectedIndex = 3;
                return;
            }

            if (_asNombre.IndexOf("WVE") != -1)
            {
                cbbTipo.SelectedIndex = 2;
                return;
            }

            if (_asNombre.IndexOf("CVE") != -1)
            {
                cbbTipo.SelectedIndex = 1;
                return;
            }    
                        
            if (_asNombre.IndexOf("VE") != -1)
                cbbTipo.SelectedIndex = 0;
        }
        private void ListarRelacionados(string _asListado)
        {
            string sConver = string.Empty;
            string sModRel = string.Empty;
            int iPos = 0;
            //..OBTENER CONVERSION..\\
            if (_asListado.IndexOf("CRC") != -1)
            {
                if (_asListado.IndexOf("a") != -1)
                {
                    iPos = _asListado.IndexOf("a");
                    sConver = _asListado.Substring(0, iPos).TrimEnd();
                }
                if (_asListado.IndexOf(">") != -1)
                {
                    iPos = _asListado.IndexOf(">");
                    sConver = _asListado.Substring(0, iPos - 1).TrimEnd();
                }

                
                _asListado = _asListado.Substring(iPos + 1).TrimStart();

                iPos = sConver.IndexOf("CRC");
                if(iPos != -1)
                {
                    sConver = sConver.Substring(iPos);
                    sConver = sConver.TrimEnd();

                    chbConversion.Checked = true;
                    txtCRC.Text = sConver;

                    _asListado = _asListado.Replace("/", "-");
                }
                
            }
            else
            {
                if (_asListado.ToUpper().IndexOf("CONVERSION") != -1)
                {
                    iPos = _asListado.ToUpper().IndexOf("CONVERSION");
                    if(iPos == 0)
                        _asListado = _asListado.Substring(11).TrimStart();
                    else
                        _asListado = _asListado.Substring(0,iPos - 1).TrimEnd();

                    if (_asListado.ToUpper().IndexOf("()") != -1)
                        _asListado = _asListado.Replace("()", "");

                    chbConversion.Checked = true;

                }
            }


            while (!string.IsNullOrEmpty(_asListado))
            {
                sModRel = string.Empty;
                iPos = _asListado.IndexOf("-");

                if (iPos != -1)
                {
                    sModRel = _asListado.Substring(0, iPos).TrimEnd();
                    
                }
                else
                {
                    sModRel = _asListado;
                    _asListado = string.Empty;
                }

                if (!string.IsNullOrEmpty(sModRel))//BCMSE36A/V36ANPO/XRX36A
                {
                    if(sModRel.IndexOf("/") != -1)
                    {
                        string sModelo = string.Empty;
                        sModRel = sModRel + "/";

                        while(sModRel.IndexOf("/") != -1) //*CAMBIAR CICLO*// BCMSE36A/V36ANPO/XRX36A
                        {
                            int iPos2 = sModRel.IndexOf("/");
                            if (iPos2 != -1)
                            {
                                //*CAMBIA ULTIMOS DIGITOS DEL MODELO  *// BC55AP/XP
                                sModelo = sModRel.Substring(0, iPos2); // BC55AP
                            }
                            else
                            {
                                sModelo = sModRel;
                                sModRel = string.Empty;
                            }

                            if (!string.IsNullOrEmpty(sModelo))
                            {
                                System.Data.DataTable dt2 = dgwModelos.DataSource as System.Data.DataTable;
                                dt2.Rows.Add(null, 0, sModelo, sConver);

                                string sExp = sModRel.Substring(iPos2 + 1).TrimEnd(); // XP
                                if (!string.IsNullOrEmpty(sExp))
                                {
                                    string sCve = sModRel.Substring(0, 2);
                                    string sModcve = sModRel.Substring(2, iPos2);
                                    if (sExp.Any(char.IsNumber)) // MSE36A/V36ANPO
                                    {
                                        sModRel = sCve;//BC
                                    }
                                    else
                                        sModRel = sModelo.Substring(0, sModelo.Length - sExp.Length);


                                    sModRel += sExp; // BCV36ANPO
                                }
                                else
                                    sModRel = string.Empty;
                            }
                        }
                    }
                    else
                    {   
                        string sNota = String.Empty;
                        int iPosN = sModRel.IndexOf("(");
                        if (iPosN != -1)
                        {
                            sNota = sModRel.Substring(iPosN).TrimStart().TrimEnd();
                            sModRel = sModRel.Substring(0, iPosN).TrimEnd();
                        }

                        if (!string.IsNullOrEmpty(sConver))
                            sNota = sConver;

                        System.Data.DataTable dt = dgwModelos.DataSource as System.Data.DataTable;
                        dt.Rows.Add(null, 0, sModRel, sNota);
                    }
                    _asListado = _asListado.Substring(iPos + 1).TrimStart();
                }
            }
        }
        private void ImportarFormato(string _asModelo)
        {
            
            Inicio();
            cbbModelo.Text = _asModelo;

            _lbRevStd = true;
            ModelosDataBound(_asModelo);

            try
            {
                System.Data.DataTable data = new System.Data.DataTable();

                Microsoft.Office.Interop.Excel.Application exApp = new Microsoft.Office.Interop.Excel.Application();
                Workbook exLibro;
                Worksheet exHoja;
                Range exRango;

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Archivos de Excel (*.xls;*.xlsx)|*.xls;*.xlsx";

                dialog.Title = "Seleccione el archivo de Excel";

                dialog.FileName = string.Empty;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    string sFile = dialog.FileName;

                    exLibro = exApp.Workbooks.Open(sFile);
                    exHoja = exLibro.Sheets[_asModelo];

                    int iCol = 12;
                    int iCol2 = 15;
                    int iRow = 2;
                    if (string.IsNullOrEmpty(exHoja.Cells.Item[2, 4].Value))
                    {
                        iRow = iRow + 2;
                    }
                    string sNombre = exHoja.Cells.Item[iRow, 4].Value.ToString();

                    if (string.IsNullOrEmpty(exHoja.Cells.Item[iRow, 11].Value))
                    {
                        iCol++;
                        iCol2++;
                    }

                    //** CONFIGURACION **\\
                    string sTakt = "0";
                    if (exHoja.Cells.Item[iRow, iCol].Value != null)
                        sTakt = exHoja.Cells.Item[iRow, iCol].Value.ToString();

                    string sDuracion = "0";
                    if (exHoja.Cells.Item[iRow, iCol2].Value != null)
                        sDuracion = exHoja.Cells.Item[iRow, iCol2].Value.ToString();

                    string sCantop = "0";
                    if (exHoja.Cells.Item[iRow + 2, iCol].Value != null)
                        sCantop = exHoja.Cells.Item[iRow + 2, iCol].Value.ToString();

                    string sCoating = "0";
                    if (exHoja.Cells.Item[iRow + 2, iCol2].Value != null)
                        sCoating = exHoja.Cells.Item[iRow + 2, iCol2].Value.ToString();

                    int iOper = 0;
                    if (!int.TryParse(sCantop, out iOper))
                        iOper = 0;
                    double dDura = 0;
                    if (!double.TryParse(sDuracion, out dDura))
                        dDura = 0;
                    double dCoat = 0;
                    if (!double.TryParse(sCoating, out dCoat))
                        dCoat = 0;
                    dDura = Math.Round(dDura, 0);
                    dCoat = Math.Round(dCoat, 4);

                    txtCantop.Text = Convert.ToString(iOper);
                    txtDuracion.Text = dDura.ToString();
                    txtTackt.Text = sTakt;
                    txtCoating.Text = dCoat.ToString();


                    txtNombre.Text = sNombre.ToUpper();
                    //** TIPO CORE **//
                    ObtenerCore(_asModelo);

                    //* MODELOS *//
                    int iPos = 0;
                    string sNota = String.Empty;
                    iRow++;
                    if (!string.IsNullOrEmpty(exHoja.Cells.Item[iRow, 4].Value))
                        sNota = exHoja.Cells.Item[iRow, 4].Value.ToString();
                    else
                    {
                        if (!string.IsNullOrEmpty(exHoja.Cells.Item[iRow, 3].Value))
                            sNota = exHoja.Cells.Item[iRow, 3].Value.ToString();
                        else
                        {
                            if (!string.IsNullOrEmpty(exHoja.Cells.Item[iRow, 2].Value))
                                sNota = exHoja.Cells.Item[iRow, 2].Value.ToString();
                            else
                            {
                                sNota = sNombre;
                                if (sNota.IndexOf("NVE") != -1)
                                    sNota = sNota.Replace("NVE", "");
                                if (sNota.IndexOf("VE") != -1)
                                    sNota = sNota.Replace("VE", "");
                                if (sNota.IndexOf("DAM") != -1)
                                    sNota = sNota.Replace("DAM", "");
                                sNota = sNota.TrimEnd();
                            }
                        }
                    }    

                    //* CRC DE CONVERSION FUERA DE NOTA *//
                    if(chbConversion.Checked && string.IsNullOrEmpty(txtCRC.Text))
                    {
                        string sCRC = string.Empty;
                        if (!string.IsNullOrEmpty(exHoja.Cells.Item[iRow, 8].Value)) //*H*//
                            sCRC = exHoja.Cells.Item[iRow, 8].Value.ToString();
                        else
                        {
                            if (!string.IsNullOrEmpty(exHoja.Cells.Item[iRow, 7].Value)) //*G*//
                                sCRC = exHoja.Cells.Item[iRow, 7].Value.ToString();
                        }

                        if (!string.IsNullOrEmpty(sCRC))
                            if(sCRC.IndexOf("-") != -1)
                            {
                                txtNombre.Text += " " + sCRC;
                                sCRC = sCRC.Substring(0, sCRC.IndexOf("-")).TrimEnd();
                            }

                            txtCRC.Text = sCRC;
                    }

                    //* ESTACIONES *//
                    exHoja = exLibro.Sheets[_asModelo];
                    exHoja.Select();
                    exRango = exHoja.get_Range("B5", "J80");

                    int iRows = exRango.Rows.Count;
                    int iCols = exRango.Columns.Count;
                    string sEstAnt = string.Empty;
                    for (int i = 1; i <= iRows; i++)
                    {
                        // AGREGA ESTACIONES \\
                        string sEstacion = string.Empty;
                        if (exRango.Cells[i, 1].Value == null)
                        {
                            if(string.IsNullOrEmpty(sEstAnt))
                                continue;
                        }
                        else
                            sEstacion = exRango.Cells[i, 1].Value.ToString();

                        

                        if (exRango.Cells[i, 1].Value == null)
                        {
                            sEstAnt = sEstacion;
                            continue;
                        }

                        if (string.IsNullOrEmpty(sEstacion))
                            sEstacion = sEstAnt;

                        string sDescrip = string.Empty;
                        if (exRango.Cells[i, 2].Value != null)
                            sDescrip = exRango.Cells[i, 2].Value.ToString();

                        string sCantidad = "0";
                        if (sDescrip.ToUpper() == "ROBOT")
                            continue;
                        else
                        {
                            int iCant = 0;
                            int iColCt = 6;
                            if (exRango.Cells[i, 6].Value == null)
                                iColCt++;


                            if (exRango.Cells[i, iColCt].Value == null)
                                sCantidad = "1";
                            else
                            {
                                if (int.TryParse(exRango.Cells[i, iColCt].Value.ToString(), out iCant))
                                    sCantidad = Convert.ToString(iCant);
                            }

                        }
                        
                        int iEst = 0;
                        int iEstacion = 0;
                        int iCantOp = 0;
                        if (Int32.TryParse(sEstacion, out iEst))
                        {
                            iEstacion = iEst;
                            if (!int.TryParse(sCantidad, out iCantOp))
                                iCantOp = 0;

                            AgregarFila(0, sEstacion, sDescrip.ToUpper(), iCantOp, null, null);
                        }
                        else
                        {
                            if (sEstacion.ToUpper().IndexOf("Y") != -1)//16 y 17
                            {
                                while (!string.IsNullOrEmpty(sEstacion))
                                {
                                    string sEst = string.Empty;
                                    iPos = sEstacion.ToUpper().IndexOf("Y");
                                    if (iPos != -1)
                                    {
                                        sEst = sEstacion.Substring(0, iPos).TrimEnd();
                                    }
                                    else
                                    {
                                        sEst = sEstacion;
                                        sEstacion = string.Empty;
                                    }

                                    if (!string.IsNullOrEmpty(sEst))
                                    {
                                        if (!string.IsNullOrEmpty(sEst))
                                            AgregarFila(0, sEst, sDescrip.ToUpper(), 1, null, null);

                                        sEstacion = sEstacion.Substring(iPos + 1).TrimStart();
                                    }
                                }
                            }
                        }
                    }

                    //CONFIG CANT OP \\
                    string sRwk = string.Empty;
                    string sGuia = string.Empty;
                    string sMylar = string.Empty;
                    string sRobot = "0";
                    for (int i = 1; i <= iRows; i++)
                    {
                        if (exRango.Cells[i, 1].Value == null)
                        {
                            if (exRango.Cells[i, 7].Value != null || exRango.Cells[i, 8].Value != null)
                            { // CONFIGURACION STD
                                int iColRw = 8;
                                int iColCt = 7;
                                if (exRango.Cells[i, 8].Value == null)
                                {
                                    iColRw--;
                                    iColCt--;
                                }

                                string sTipo = exRango.Cells[i, iColRw].Value.ToString();
                                if (sTipo.ToUpper() == "RETRABAJO" || sTipo.ToUpper() == "RWK")
                                    sRwk = exRango.Cells[i, iColCt].Value.ToString();
                                if (sTipo.ToUpper() == "GUIA" || sTipo.ToUpper() == "GUIDE")
                                    sGuia = exRango.Cells[i, iColCt].Value.ToString();
                                if (sTipo.ToUpper().IndexOf("MYLAR") != -1)
                                    sMylar = exRango.Cells[i, iColCt].Value.ToString();
                            }
                            continue;
                        }
                        //BUSCA ROBOT ENTRE ESTACIONES\\
                        if (exRango.Cells[i, 2].Value == null)
                            continue;

                        string sDescrip = exRango.Cells[i, 2].Value.ToString();
                        if (sDescrip.ToUpper() == "ROBOT")
                            sRobot = "1";
                    }

                    int iGuia = 0;
                    if (Int32.TryParse(sGuia, out iGuia))
                    {
                        chbGuia.Checked = true;
                        txtGuia.Text = iGuia.ToString();
                    }
                    else
                    {
                        chbGuia.Checked = false;
                        txtGuia.Text = "0";
                    }

                    int iRwk = 0;
                    if (Int32.TryParse(sRwk, out iRwk))
                    {
                        chbRetra.Checked = true;
                        txtRetra.Text = iRwk.ToString();
                    }

                    int iMylar = 0;
                    if (Int32.TryParse(sMylar, out iMylar))
                    {
                        chbMylar.Checked = true;
                        txtMylar.Text = iMylar.ToString();
                    }

                    if (sRobot == "1")
                        chbRobot.Checked = true;

                    exLibro.Close(true);
                    exApp.Quit();

                    CargarNombre(_asModelo);

                    
                    iPos = sFile.LastIndexOf(@"\");
                    if(iPos != -1)
                    {
                        _lsArchivoPath = sFile.Substring(0, iPos);
                        _lsArchivoStd = sFile.Substring(iPos + 1);
                        txtLayOutFolder.Text = _lsArchivoPath;
                    }
                        

                    ListarRelacionados(sNota);

                    chbRevStd.Checked = true;
                    

                    Cursor = Cursors.Default;
                    _lbCambio = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "Archivo sin formato Estandar" + Environment.NewLine + "ImportarFormato(" + _asModelo + ") ..." + ex.ToString(), "ERROR " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cursor = Cursors.Default;
                return;
            }
        }
        private string CargarNombre(string _asModelo)
        {
            try
            {
                string sClave = _asModelo;
                //IMPORTAR PESTAÑA [BC11XP VE (06-10-17)]
                //IMPORTAR PESTAÑA [BC11XP VE (Conv) (06-10-17)] para conversiones
                int iPos = _asModelo.IndexOf("(");
                int iLen = _asModelo.Length;
                if (iPos != -1)
                {
                    string sTipo = _asModelo.Substring(iPos + 1, 4);
                    if (sTipo == "CONV") //formato Conversion
                        _asModelo = _asModelo.Replace("(CONV)", "CONV");
                    else
                    {
                        sTipo = _asModelo.Substring(iPos + 1, 3);
                        if (sTipo == "NVE")
                            _asModelo = _asModelo.Replace("(NVE)", "NVE");
                        else
                        {
                            sTipo = _asModelo.Substring(iPos + 1, 2);
                            if (sTipo == "VE")
                                _asModelo = _asModelo.Replace("(VE)", "VE");
                        }
                    }
                    if(sTipo.IndexOf("VE") != -1)
                    {
                        if (_asModelo.IndexOf("NVE") != -1)
                            sTipo = "NVE";
                        else
                        {
                            if (_asModelo.IndexOf("VE") != -1)
                                sTipo = "VE";
                        }
                    }

                    iPos = _asModelo.LastIndexOf("(");
                    //formato mm-dd-yyyy
                    string sFecha = _asModelo.Substring(iPos + 1);
                    sFecha = sFecha.Replace(")", "");
                    sFecha = sFecha.Replace("NVE", "");
                    sFecha = sFecha.Replace("VE", "");
                    DateTime dtFecha = Convert.ToDateTime(sFecha);
                    txtUltRev.Text = sFecha;

                    //ind formato revision std
                    sClave = _asModelo.Substring(0, iPos);
                    sClave = sClave.TrimEnd();
                    sClave = sClave.Replace(" ", "_");

                    if(sClave.Length > 20)
                        sClave = sClave.Substring(0, 20);

                    cbbModelo.Text = sClave;

                    return sClave;
                }
                return sClave;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
            
        }
        #endregion

        #region regCaptura
        private void CargarModelo(string _asModelo)
        {
            try
            {
                Inicio();
                bool bExiste = false;
                ModeloLogica mode = new ModeloLogica();
                System.Data.DataTable datos = new System.Data.DataTable();

                //MODELOS RELACIONADOS
                ModerelaLogica modre = new ModerelaLogica();
                modre.Moderela = _asModelo;
                System.Data.DataTable dtRel = ModerelaLogica.ConsultaRelacionado(modre);
                if (dtRel.Rows.Count != 0)
                {
                    if(dtRel.Rows.Count > 0)
                    {
                        wfCapturaPopGrid wfMods = new wfCapturaPopGrid(_asModelo);
                        wfMods.ShowDialog();
                        _asModelo = wfMods._sClave;
                        if (string.IsNullOrEmpty(_asModelo))
                            return;
                    }
                    else
                        _asModelo = dtRel.Rows[0]["modelo"].ToString();

                    mode.Modelo = _asModelo;
                    datos = ModeloLogica.Consultar(mode);
                    if (datos.Rows.Count != 0)
                        bExiste = true;
                }

                if (!bExiste)
                {
                    mode.Modelo = _asModelo;
                    datos = ModeloLogica.Consultar(mode);
                    if (datos.Rows.Count != 0)
                        bExiste = true;
                }

                if (!bExiste)
                {//REVISION ESTANDAR 
                    string sModelo = CargarNombre(_asModelo);
                    mode.Modelo = sModelo;
                    datos = ModeloLogica.Consultar(mode);
                    if (datos.Rows.Count != 0)
                        bExiste = true;
                }

                if (bExiste)
                {
                    cbbModelo.SelectedValue = datos.Rows[0]["modelo"].ToString();
                    txtNombre.Text = datos.Rows[0]["descrip"].ToString();
                    cbbEstatus.SelectedValue = datos.Rows[0]["estatus"].ToString();
                    if (!string.IsNullOrEmpty(datos.Rows[0]["tipo"].ToString()))
                        cbbTipo.SelectedValue = datos.Rows[0]["tipo"].ToString();
                    if (!string.IsNullOrEmpty(datos.Rows[0]["planta"].ToString()))
                        cbbPlanta.SelectedValue = datos.Rows[0]["planta"].ToString();
                    txtCantop.Text = datos.Rows[0]["total_op"].ToString();
                    txtDuracion.Text = datos.Rows[0]["duracion"].ToString();
                    txtTackt.Text = datos.Rows[0]["takt_time"].ToString();
                    txtCoating.Text = datos.Rows[0]["op_coating"].ToString();
                    //FORMATO STD
                    if (!string.IsNullOrEmpty(datos.Rows[0]["ind_formatostd"].ToString()))
                    {
                        if (datos.Rows[0]["ind_formatostd"].ToString() == "1")
                        {
                            chbRevStd.Checked = true;
                            _lbRevStd = true;
                        }
                        else
                        {
                            chbRevStd.Checked = false;
                            _lbRevStd = false;
                        }
                            
                        txtUltRev.Text = datos.Rows[0]["ult_revstd"].ToString();
                    }
                    if (!string.IsNullOrEmpty(datos.Rows[0]["formato_std"].ToString()))
                        _lsArchivoStd = datos.Rows[0]["formato_std"].ToString();
                    if (!string.IsNullOrEmpty(datos.Rows[0]["formstd_path"].ToString()))
                        _lsArchivoPath = datos.Rows[0]["formstd_path"].ToString();

                    if (datos.Rows[0]["ind_conversion"].ToString() == "1")
                    {
                        chbConversion.Checked = true;
                        chbConversion.Enabled = false;
                        txtCRC.Enabled = false;
                    }
                    else
                    {
                        chbConversion.Checked = false;
                        chbConversion.Enabled = true;
                        txtCRC.Enabled = true;
                    }

                    txtCRC.Text = datos.Rows[0]["crc_conv"].ToString();
                    txtLayOutFolder.Text = datos.Rows[0]["formstd_path"].ToString();
                    txtVAFolder.Text = datos.Rows[0]["va_folder"].ToString();

                    int iCant = 0;
                    if (Int32.TryParse(datos.Rows[0]["cant_guia"].ToString(), out iCant))
                    {
                        if (iCant > 0)
                            chbGuia.Checked = true;
                        else
                            chbGuia.Checked = false;

                        txtGuia.Text = datos.Rows[0]["cant_guia"].ToString();
                    }

                    if (Int32.TryParse(datos.Rows[0]["cant_retra"].ToString(), out iCant))
                    {
                        if (iCant > 0)
                            chbRetra.Checked = true;
                        else
                            chbRetra.Checked = false;

                        txtRetra.Text = datos.Rows[0]["cant_retra"].ToString();
                    }

                    if (Int32.TryParse(datos.Rows[0]["cant_mylar"].ToString(), out iCant))
                    {
                        if (iCant > 0)
                            chbMylar.Checked = true;
                        else
                            chbMylar.Checked = false;

                        txtMylar.Text = datos.Rows[0]["cant_mylar"].ToString();
                    }

                    if (Int32.TryParse(datos.Rows[0]["cant_multi"].ToString(), out iCant))
                    {
                        if (iCant > 0)
                            chbMulti.Checked = true;
                        else
                            chbMulti.Checked = false;

                        txtMulti.Text = datos.Rows[0]["cant_multi"].ToString();
                    }

                    if (datos.Rows[0]["ind_robot"].ToString() == "1")
                        chbRobot.Checked = true;
                    else
                        chbRobot.Checked = false;                    
                }
                else
                {
                    cbbModelo.Text = _asModelo;
                    txtNombre.Clear();
                    txtNombre.Text = _asModelo;       
                }

                ModelosDataBound(_asModelo);

                _lbCambio = false;
                _lbCambioDet = false;

                txtNombre.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
        private void ModelosDataBound(string _asModelo)
        {
            //nombre pestaña revision std
            _asModelo = CargarNombre(_asModelo);

            ModestaLogica mod = new ModestaLogica();
            mod.Modelo = _asModelo;
            System.Data.DataTable Lista = ModestaLogica.Listar(mod);
            dgwEstaciones.DataSource = Lista;
            CargarEstaciones();

            ModerelaLogica modre = new ModerelaLogica();
            modre.Modelo = _asModelo;
            System.Data.DataTable dtRel = ModerelaLogica.Listar(modre);
            dgwModelos.DataSource = dtRel;
            CargarRelacionMod();
        }
            
        private void cbbModelo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(cbbModelo.SelectedIndex != -1)
                CargarModelo(cbbModelo.SelectedValue.ToString());
        }

        private void cbbModelo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(cbbModelo.Text) && !string.IsNullOrWhiteSpace(cbbModelo.Text))
            {
                
                ModeloLogica mode = new ModeloLogica();
                cbbModelo.Text = cbbModelo.Text.ToUpper().Trim().ToString();
                if (!_lbCopy)
                    CargarModelo(cbbModelo.Text.ToString());
                
            }
            else
                cbbModelo.SelectedIndex = -1;
        }

        private void cbbModelo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbbModelo.Text.Length >= 15 && !char.IsControl(e.KeyChar))
                e.Handled = true;
            
            if (e.KeyChar >= 'a' && e.KeyChar <= 'z')
                e.KeyChar = Convert.ToChar(e.KeyChar.ToString().ToUpper());
        }

        private void cbbEstatus_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void chbGuia_CheckedChanged(object sender, EventArgs e)
        {
            if (!_lbGuiaAnt != chbGuia.Checked)
                _lbCambio = true;
            _lbGuiaAnt = chbGuia.Checked;

            if (chbGuia.Checked)
            {
                if (string.IsNullOrEmpty(txtGuia.Text) || Convert.ToInt32(txtGuia.Text) == 0)
                    txtGuia.Text = "1";
                txtGuia.Enabled = true;
            }
            else
            {
                txtGuia.Text = "0";
                txtGuia.Enabled = false;
            }
        }

        private void chbRetra_CheckedChanged(object sender, EventArgs e)
        {
            if (!_lbRetrAnt != chbRetra.Checked)
                _lbCambio = true;
            _lbRetrAnt = chbRetra.Checked;

            if (chbRetra.Checked)
            {
                if (string.IsNullOrEmpty(txtRetra.Text) || Convert.ToInt32(txtRetra.Text) == 0)
                    txtRetra.Text = "1";
                txtRetra.Enabled = true;
            }
            else
            {
                txtRetra.Text = "0";
                txtRetra.Enabled = false;
            }
        }

        private void chbMulti_CheckedChanged(object sender, EventArgs e)
        {
            if (!_lbMultiAnt != chbMulti.Checked)
                _lbCambio = true;
            _lbMultiAnt = chbMulti.Checked;

            if (chbMulti.Checked)
            {
                if (string.IsNullOrEmpty(txtMulti.Text) || Convert.ToInt32(txtMulti.Text) == 0)
                    txtMulti.Text = "1";
                txtMulti.Enabled = true;
            }
            else
            {
                txtMulti.Text = "0";
                txtMulti.Enabled = false;
            }
        }

        private void txtCantop_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtDuracion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == ',')
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtTackt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == ',')
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtCoating_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == ',')
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void chbConversion_CheckedChanged(object sender, EventArgs e)
        {
            if (chbConversion.Checked)
            {
                if (string.IsNullOrEmpty(txtCRC.Text))
                    txtCRC.Enabled = true;
            }
        }
        private void txtGuia_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtRetra_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtMulti_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        #endregion

        #region regEstacion
        private void dgwEstaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightSkyBlue;
            else
                e.CellStyle.BackColor = Color.White;

            if(e.Value != null)
            {
                e.Value = e.Value.ToString().ToUpper();
                e.FormattingApplied = true;
            }
        }
        private int ColumnWith(DataGridView _dtGrid, double _dColWith)
        {

            double dW = _dtGrid.Width - 10;
            double dTam = _dColWith;
            double dPor = dTam / 100;
            dTam = dW * dPor;
            dTam = Math.Truncate(dTam);

            return Convert.ToInt32(dTam);
        }
        private void CargarEstaciones()
        {
            
            int iRows = dgwEstaciones.Rows.Count;
            if (iRows == 0)
            {
                System.Data.DataTable dtNew = new System.Data.DataTable("Estacion");
                dtNew.Columns.Add("modelo", typeof(string));
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("ESTACION", typeof(string));
                dtNew.Columns.Add("NOMBRE", typeof(string));
                dtNew.Columns.Add("CANT", typeof(int));
                dtNew.Columns.Add("PPH", typeof(string));
                dtNew.Columns.Add("CODIGO", typeof(string));
                dtNew.Columns.Add("nivel_req", typeof(string));
                dgwEstaciones.DataSource = dtNew;
            }
            
            dgwEstaciones.Columns[0].Visible = false;
            dgwEstaciones.Columns[1].Visible = false;
            dgwEstaciones.Columns[7].Visible = false;

            //dgwEstaciones.Columns[2].ReadOnly = _lbRevStd;
            dgwEstaciones.Columns[2].Width = ColumnWith(dgwEstaciones, 12);
            dgwEstaciones.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[3].ReadOnly = _lbRevStd;
            dgwEstaciones.Columns[3].Width = ColumnWith(dgwEstaciones, 56);
            dgwEstaciones.Columns[4].Width = ColumnWith(dgwEstaciones, 9);
            //dgwEstaciones.Columns[4].ReadOnly = chbRevStd.Checked;
            dgwEstaciones.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[5].Width = ColumnWith(dgwEstaciones, 8);
            dgwEstaciones.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[6].Width = ColumnWith(dgwEstaciones, 12);

            if (!string.IsNullOrEmpty(cbbModelo.Text.ToString()))
            {
                string sEstacion = "0";
                if(dgwEstaciones.Rows.Count > 0)
                    sEstacion = Convert.ToString(dgwEstaciones.Rows[iRows - 1].Cells[2].Value);

                sEstacion = Convert.ToString(Convert.ToInt16(sEstacion) + 1);

                AgregarFila(0,sEstacion,null,1,null,null);
            }

        }
        private void AgregarFila(int _aiCons, string _asEstacion, string _asNombre, int _aiCant, string _asNivel,string _asCodigo)
        {
            if(dgwEstaciones.RowCount == 0)
            {
                System.Data.DataTable dt = dgwEstaciones.DataSource as System.Data.DataTable;
                dt.Rows.Add(null, _aiCons, _asEstacion, _asNombre, _aiCant,_asNivel,_asCodigo);
            }
            else
            {
                int iIdx = dgwEstaciones.RowCount - 1;
                if (string.IsNullOrEmpty(dgwEstaciones.Rows[iIdx].Cells[3].Value.ToString()))
                {
                    dgwEstaciones.Rows[iIdx].Cells[0].Value = null;
                    dgwEstaciones.Rows[iIdx].Cells[1].Value = 0;
                    dgwEstaciones.Rows[iIdx].Cells[2].Value = _asEstacion;
                    dgwEstaciones.Rows[iIdx].Cells[3].Value = _asNombre;
                    dgwEstaciones.Rows[iIdx].Cells[4].Value = _aiCant;
                    dgwEstaciones.Rows[iIdx].Cells[5].Value = _asNivel;
                    dgwEstaciones.Rows[iIdx].Cells[6].Value = _asCodigo;
                }
                else
                {
                    System.Data.DataTable dt = dgwEstaciones.DataSource as System.Data.DataTable;
                    dt.Rows.Add(null, _aiCons, _asEstacion, _asNombre, _aiCant,_asNivel,_asCodigo);
                }
            }
        }
        private void CargarRelacionMod()
        {

            int iRows = dgwModelos.Rows.Count;
            if (iRows == 0)
            {
                System.Data.DataTable dtNew = new System.Data.DataTable("Moderela");
                dtNew.Columns.Add("modelo", typeof(string));
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("Modelos", typeof(string));
                dtNew.Columns.Add("Nota", typeof(string));
                dgwModelos.DataSource = dtNew;
            }

            dgwModelos.Columns[0].Visible = false;
            dgwModelos.Columns[1].Visible = false;
            dgwModelos.Columns[2].Width = ColumnWith(dgwModelos, 40);


            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "55") == false)
            {
                dgwModelos.Columns[0].ReadOnly = chbRevStd.Checked;
                dgwModelos.Columns[1].ReadOnly = chbRevStd.Checked;
                dgwModelos.Columns[2].ReadOnly = chbRevStd.Checked;
            }



        }
        private void dgwEstaciones_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                int iIdx = dgwEstaciones.CurrentRow.Index;
                int iRows = dgwEstaciones.Rows.Count;
                int iRdx = e.RowIndex;
                int iNrow = iRdx + 1;
                

                if (e.ColumnIndex == 4)
                {
                    int iCant;
                    if (!int.TryParse(Convert.ToString(e.FormattedValue), out iCant))
                    {
                        e.Cancel = true;
                        MessageBox.Show("Valor invalido en el campo Operadores", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            
        }

        private void dgwEstaciones_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //MessageBox.Show(e.RowIndex.ToString());
            dgwEstaciones.Rows[e.RowIndex].Cells[3].Selected = true;
        }

        private void dgwEstaciones_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5)
                _lbCambioDet = true;
            
            if (e.ColumnIndex == 3)
            {
                string sNombre = dgwEstaciones[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
                if (!string.IsNullOrEmpty(sNombre) && !string.IsNullOrWhiteSpace(sNombre))
                {
                    foreach(DataGridViewRow row in dgwEstaciones.Rows)
                    {
                        if (row.Index == e.RowIndex)
                            continue;

                        string sEstacion = row.Cells[3].Value.ToString().ToUpper();
                        if(sNombre == sEstacion)
                        {
                            MessageBox.Show(string.Format("La Estación {0} ya se encuentra en el listado",sEstacion), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = string.Empty;
                            break;
                        }
                    }
                }
            }
        }

        private void dgwEstaciones_KeyDown(object sender, KeyEventArgs e)
        {
            int iRow = dgwEstaciones.CurrentCell.RowIndex;

            if (e.KeyCode == Keys.F5)
            {
                if (dgwEstaciones.CurrentRow.Index == -1)
                    return;

                AsignarPPH(iRow);
            }

            if (e.KeyCode != Keys.Enter)
                return;

            e.SuppressKeyPress = true;
            int iColumn = dgwEstaciones.CurrentCell.ColumnIndex;
            if (iColumn == dgwEstaciones.Columns.Count - 3)
            {
                if (iRow + 1 < dgwEstaciones.Rows.Count - 3)
                    dgwEstaciones.CurrentCell = dgwEstaciones[3, iRow + 2];
                else
                    dgwEstaciones.CurrentCell = dgwEstaciones[iColumn - 3, iRow];
            }
            else
                dgwEstaciones.CurrentCell = dgwEstaciones[iColumn + 1, iRow];

            
        }

        private void dgwEstaciones_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < dgwEstaciones.Rows.Count - 1)
                return;

            if (e.ColumnIndex != 3)
                return;

            if (_lbRevStd)
                return;

            if (!string.IsNullOrEmpty(dgwEstaciones[e.ColumnIndex, e.RowIndex].Value.ToString()))
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt = dgwEstaciones.DataSource as System.Data.DataTable;
                dt.Rows.Add(null, 0, dgwEstaciones.Rows.Count + 1, null, 1);
            }
        }
        #endregion

        #region regCambio
        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_lsNombreAnt) && txtNombre.Text != _lsNombreAnt)
                _lbCambio = true;
            _lsNombreAnt = txtNombre.Text.ToString();
        }

        private void cbbEstatus_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_liEstatusAnt != -1 && cbbEstatus.SelectedIndex != _liEstatusAnt)
                _lbCambio = true;
            _liEstatusAnt = cbbEstatus.SelectedIndex;
        }

        private void txtCantop_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_lsOperaAnt) && txtCantop.Text != _lsOperaAnt)
                _lbCambio = true;
            _lsOperaAnt = txtCantop.Text.ToString();
        }

        private void txtTackt_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_lsTaktAnt) && txtTackt.Text != _lsTaktAnt)
                _lbCambio = true;
            _lsTaktAnt = txtTackt.Text.ToString();
        }

        private void txtCoating_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_lsCoatAnt) && txtCoating.Text != _lsCoatAnt)
                _lbCambio = true;
            _lsCoatAnt = txtCoating.Text.ToString();
        }

        private void txtDuracion_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_lsDuraAnt) && txtDuracion.Text != _lsDuraAnt)
                _lbCambio = true;
            _lsDuraAnt = txtDuracion.Text.ToString();
        }

        private void txtGuia_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_lsGuiaAnt) && txtGuia.Text != _lsGuiaAnt)
                _lbCambio = true;
            _lsGuiaAnt = txtGuia.Text.ToString();
        }

        private void txtRetra_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_lsRetrAnt) && txtRetra.Text != _lsRetrAnt)
                _lbCambio = true;
            _lsRetrAnt = txtRetra.Text.ToString();
        }

        private void txtMulti_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_lsMultiAnt) && txtMulti.Text != _lsMultiAnt)
                _lbCambio = true;
            _lsMultiAnt = txtMulti.Text.ToString();
        }
        private void chbRobot_CheckedChanged(object sender, EventArgs e)
        {
            if (_lbRobotAnt != chbRobot.Checked)
                _lbCambio = true;
            _lbRobotAnt = chbRobot.Checked;
        }
        private void txtMylar_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_lsMylarAnt) && txtRetra.Text != _lsMylarAnt)
                _lbCambio = true;
            _lsMylarAnt = txtMylar.Text.ToString();
        }

        private void txtMylar_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void chbMylar_CheckedChanged(object sender, EventArgs e)
        {
            if (!_lbMylarAnt != chbMylar.Checked)
                _lbCambio = true;
            _lbMylarAnt = chbMylar.Checked;

            if (chbMylar.Checked)
            {
                if (string.IsNullOrEmpty(txtMylar.Text) || Convert.ToInt32(txtMylar.Text) == 0)
                    txtMylar.Text = "1";
                txtMylar.Enabled = true;
            }
            else
            {
                txtMylar.Text = "0";
                txtMylar.Enabled = false;
            }
        }

        private void txtVAFolder_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_lsVAidAnt) && txtVAFolder.Text != _lsVAidAnt)
                _lbCambio = true;
            _lsVAidAnt = txtVAFolder.Text.ToString();
        }
        #endregion

        #region regColor
        private void txtNombre_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtNombre, 0);
        }

        private void txtNombre_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtNombre, 1);
        }

        private void txtCantop_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtCantop, 1);
        }

        private void txtCantop_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtCantop, 0);
        }

        private void txtDuracion_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtDuracion, 0);
        }

        private void txtDuracion_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtDuracion, 1);
        }

        private void txtTackt_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtTackt, 1);
        }

        private void txtTackt_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtTackt, 0);
        }

        private void txtCoating_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtCoating, 0);
        }

        private void txtCoating_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtCoating, 1);
        }

        private void txtGuia_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtGuia, 0);
        }

        private void txtGuia_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtGuia, 1);
        }

        private void txtRetra_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRetra, 0);
        }

        private void txtRetra_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRetra, 1);
        }

        private void txtMulti_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtMulti, 0);
        }

        private void txtMulti_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtMulti, 1);
        }

        private void cbbModelo_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbModelo, 0);
        }

        private void cbbModelo_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbModelo, 1);
        }
        private void txtMylar_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtMylar, 0);
        }

        private void txtMylar_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtMylar, 1);
        }
        private void txtLayOutFolder_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtLayOutFolder, 1);
        }

        private void txtLayOutFolder_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtLayOutFolder, 0);
        }

        private void txtVAFolder_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtVAFolder, 1);
        }

        private void txtVAFolder_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtVAFolder, 0);
        }

        #endregion

        #region regResize
        private void wfModelos_Resize(object sender, EventArgs e)
        {
            if (WindowState != _WindowStateAnt && WindowState != FormWindowState.Minimized)
            {
                _WindowStateAnt = WindowState;
                ResizeControl(panel1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(tabControl1, 1, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(dgwEstaciones, 1, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(dgwModelos, 1, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(groupBox1, 1, ref _iWidthAnt, ref _iHeightAnt, 1);
            }
        }
        public void ResizeControl(Control ac_Control, int ai_Hor, ref int ai_WidthAnt, ref int ai_HegihtAnt, int ai_Retorna)
        {
            if (ai_WidthAnt == 0)
                ai_WidthAnt = ac_Control.Width;
            if (ai_WidthAnt == ac_Control.Width)
                return;

            int _dif = ai_WidthAnt - ac_Control.Width;
            int _difh = ai_HegihtAnt - ac_Control.Height;

            if (ai_Hor == 1)
                ac_Control.Height = this.Height - _difh;
            if (ai_Hor == 2)
                ac_Control.Width = this.Width - _dif;
            if (ai_Hor == 3)
            {
                ac_Control.Width = this.Width - _dif;
                ac_Control.Height = this.Height - _difh;
            }
            if (ai_Retorna == 1)
            {
                ai_WidthAnt = this.Width;
                ai_HegihtAnt = this.Height;
            }
        }

        public void ResizeGrid(DataGridView dataGrid, ref int prevWidth, ref int ai_HegihtAnt)
        {
            if (prevWidth == 0)
                prevWidth = dataGrid.Width;
            if (prevWidth == dataGrid.Width)
                return;

            int _dif = prevWidth - dataGrid.Width;
            int _difh = ai_HegihtAnt - dataGrid.Height;

            int fixedWidth = SystemInformation.VerticalScrollBarWidth + dataGrid.RowHeadersWidth + 2;
            int mul = 100 * (dataGrid.Width - fixedWidth) / (prevWidth - fixedWidth);
            int columnWidth;
            int total = 0;
            DataGridViewColumn lastVisibleCol = null;

            for (int i = 0; i < dataGrid.ColumnCount; i++)
                if (dataGrid.Columns[i].Visible)
                {
                    columnWidth = (dataGrid.Columns[i].Width * mul + 50) / 100;
                    dataGrid.Columns[i].Width = Math.Max(columnWidth, dataGrid.Columns[i].MinimumWidth);
                    total += dataGrid.Columns[i].Width;
                    lastVisibleCol = dataGrid.Columns[i];
                }
            if (lastVisibleCol == null)
                return;
            columnWidth = dataGrid.Width - total + lastVisibleCol.Width - fixedWidth;
            lastVisibleCol.Width = Math.Max(columnWidth, lastVisibleCol.MinimumWidth);

        }

        #endregion

        #region regRelacionMod
        private void dgwModelos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                _lbCambioDet = true;

                string sNombre = dgwModelos[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
                if (!string.IsNullOrEmpty(sNombre) && !string.IsNullOrWhiteSpace(sNombre))
                {
                    foreach (DataGridViewRow row in dgwModelos.Rows)
                    {
                        if (row.Index == e.RowIndex)
                            continue;

                        if (row.Cells[2].Value == null)
                            continue;

                        string sModelo = row.Cells[2].Value.ToString().ToUpper();
                        if (sNombre == sModelo)
                        {
                            MessageBox.Show(string.Format("El Modelo Relacionado {0} ya se encuentra en el listado", sModelo), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgwModelos[e.ColumnIndex, e.RowIndex].Value = string.Empty;
                            break;
                        }
                    }
                }
            }
        }
        private void dgwModelos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightGreen;
            else
                e.CellStyle.BackColor = Color.White;

            if (e.Value != null)
            {
                e.Value = e.Value.ToString().ToUpper();
                e.FormattingApplied = true;
            }
        }

        private void ModeloRemove(DataGridViewRow _crow)
        {
            try
            {
                if (!string.IsNullOrEmpty(_crow.Cells[0].Value.ToString()))
                {
                    //mandar al listado para borrar de bd al guardar cambios
                    if (dgwRemoveMod.Rows.Count == 0)
                    {
                        System.Data.DataTable dtNew = new System.Data.DataTable("Eliminar");
                        dtNew.Columns.Add("modelo", typeof(string));
                        dtNew.Columns.Add("consec", typeof(int));
                        dgwRemoveMod.DataSource = dtNew;
                    }

                    string sModelo = _crow.Cells[0].Value.ToString();
                    int iCons = Convert.ToInt32(_crow.Cells[1].Value);

                    System.Data.DataTable dt = dgwRemoveMod.DataSource as System.Data.DataTable;
                    dt.Rows.Add(sModelo, iCons);
                }

                dgwModelos.Rows.Remove(_crow);
                _lbCambioDet = true;

                //foreach (DataGridViewRow row in dgwModelos.Rows)
                //{
                //    row.Cells[1].Value = row.Index + 1;
                //    if (row.Index == dgwModelos.Rows.Count - 1)
                //        dgwModelos.Rows.Remove(row);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }            
        }


        #endregion

        #region regPPH
        private void AsignarPPH(int _aiRow)
        {
            string sNivel = string.Empty;
            string sCodigo = string.Empty;

            if(cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se ha especificado la planta del modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sOperacion = dgwEstaciones[3, _aiRow].Value.ToString();
            bool bExist = false;

            NivelDetLogica niv = new NivelDetLogica();
            niv.Planta = cbbPlanta.SelectedValue.ToString();
            niv.Operacion = sOperacion;
            System.Data.DataTable dtN = NivelDetLogica.ConsultaOperacion(niv);
            if (dtN.Rows.Count != 0)
                bExist = true;
            else
            {
                dtN = NivelDetLogica.ConsultaOperaciones(niv);
                if (dtN.Rows.Count != 0)
                    bExist = true;
                else
                {
                    string sOpe1 = string.Empty;
                    string sChar = "/";
                    int iPos = sOperacion.IndexOf(sChar);
                    if (iPos != -1)
                    {
                        sOpe1 = sOperacion.Substring(0, iPos).TrimEnd();

                        sOperacion = sOperacion.Substring(iPos + 1).TrimStart();
                    }
                    else
                    {
                        iPos = sOperacion.IndexOf(" ");
                        if (iPos != -1)
                        {
                            sOpe1 = sOperacion.Substring(0, iPos).TrimEnd();

                            sOperacion = sOperacion.Substring(iPos + 1).TrimStart();
                        }
                    }
                    niv.Operacion = sOperacion;

                    dtN = NivelDetLogica.ConsultaOperaciones(niv);
                    if (dtN.Rows.Count != 0)
                        bExist = true;
                    else
                    {
                        if(!string.IsNullOrEmpty(sOpe1))
                        {
                            niv.Operacion = sOpe1;
                            dtN = NivelDetLogica.ConsultaOperaciones(niv);
                            if (dtN.Rows.Count != 0)
                                bExist = true;
                        }
                        
                    }

                }

            }

            if (bExist)
            {
                sNivel = dtN.Rows[0]["nivel"].ToString();
                sCodigo = dtN.Rows[0]["codigo"].ToString();

                dgwEstaciones[5, _aiRow].Value = sNivel;
                dgwEstaciones[6, _aiRow].Value = sCodigo;
                _lbCambioDet = true;
            }
            else
            {
                //buscar en archivo excel de mejora continua
                //open from t_config
                try
                {
                    System.Data.DataTable dt = ConfigLogica.Consultar();
                    string sDirec = dt.Rows[0]["directorio"].ToString();
                    if (!string.IsNullOrEmpty(sDirec))
                    {
                        if(cbbPlanta.SelectedValue.ToString() == "MON")
                            sDirec += @"\FormatoPPH\LineUpNivelPPH-TONER.xlsx";
                        else
                            sDirec += @"\FormatoPPH\LineUpNivelPPH-COLOR.xlsx";
                        System.IO.FileInfo fileObj = new System.IO.FileInfo(sDirec);
                        fileObj.Attributes = System.IO.FileAttributes.ReadOnly;
                        System.Diagnostics.Process.Start(fileObj.FullName);
                    }
                    else
                        MessageBox.Show("No se ha especificado la ubicacion del Formato", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error... " + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        #endregion

        
        
    }
}
