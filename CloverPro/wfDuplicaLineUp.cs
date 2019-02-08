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
using Datos;

namespace CloverPro
{
    public partial class wfDuplicaLineUp : Form
    {
        private string _lsProceso = "PRO060";
        public bool _lbCambio;
        private string _lsFolio;
        private string _lsTurno;
        private string _lsRevStd;
        private string _lsCoreOrig;
        private string _lsModeOrig;
        private string _lsCoreNew;
        private string _lsModeNew;
        private DateTime _dtFecha;
        public wfDuplicaLineUp()
        {
            InitializeComponent();
        }

        #region regInicio
        private void wfRepEtiquetas_Load(object sender, EventArgs e)
        {
            Inicio();
            
        }
        private void wfRepEtiquetas_Activated(object sender, EventArgs e)
        {
            txtRPO.Focus();
        }
        private void Inicio()
        {
            _lbCambio = false;
            _lsFolio = string.Empty;

            cbbPlanta.ResetText();
            DataTable data = PlantaLogica.Listar();
            cbbPlanta.DataSource = data;
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.SelectedIndex = -1;

            if (GlobalVar.gsArea == "SUP" && !string.IsNullOrEmpty(GlobalVar.gsPlanta))
            {
                cbbPlanta.SelectedValue = GlobalVar.gsPlanta;
            }

            _lsTurno = GlobalVar.gsTurno;
            if (_lsTurno == "1")
                _dtFecha = DateTime.Today;
            else
                _dtFecha = GlobalVar.FechaTurno();

            txtModelo.Clear();
            txtRPO.Clear();
            txtRPO2.Clear();
            txtModelo2.Clear();

            lblPlanta.Text = "";
            lblLIni.Text = "";

            _lsRevStd = string.Empty;;
            _lsCoreOrig = string.Empty;;
            _lsModeOrig = string.Empty;;
            _lsCoreNew = string.Empty;
            _lsModeNew = string.Empty;

            CargarLineas();

            tslSuper.Text = GlobalVar.gsUsuario;
            txtRPO.Focus();

        }

        private void CargarLineas()
        {
            DataTable dtL1 = new DataTable();
            if (cbbPlanta.SelectedIndex != -1)
            {
                LineaLogica line = new LineaLogica();
                line.Planta = cbbPlanta.SelectedValue.ToString();
                dtL1 = LineaLogica.LineaPlanta(line);
            }
            else
            {
                dtL1 = LineaLogica.ListarTodas();
            }
                

            cbbLineaIni.DataSource = dtL1;
            cbbLineaIni.ValueMember = "linea";
            cbbLineaIni.DisplayMember = "linea";
            cbbLineaIni.SelectedIndex = -1;
        }
        #endregion

        private bool Valida()
        {
            bool bValida = false;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "10") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bValida;
            }


            if (cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado la planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }

            if (cbbLineaIni.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de espeficicar la Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbLineaIni.Focus();
                return bValida;
            }

            if (string.IsNullOrEmpty(txtModelo.Text))
            {
                MessageBox.Show("No se a especificado el Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtModelo.Focus();
                return bValida;
            }

            if (string.IsNullOrEmpty(txtRPO.Text))
            {
                MessageBox.Show("No se a especificado el RPO", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRPO.Focus();
                return bValida;
            }

            if (string.IsNullOrEmpty(txtRPO2.Text))
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Debe Especifiacar el Nuevo RPO", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRPO2.Focus();
                return bValida;
            }

            if (txtRPO2.Text.ToString() == txtRPO.Text.ToString())
            {
                Cursor = Cursors.Default;
                MessageBox.Show("El Nuevo RPO debe ser Diferente al RPO de la Captura Original.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return bValida;
            }

            if (string.IsNullOrEmpty(txtModelo2.Text))
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Debe Especifiacar el Nuevo Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtModelo2.Focus();
                return bValida;
            }

            
            LineUpLogica lup = new LineUpLogica();
            if (GlobalVar.gsTurno == "1")
                lup.Fecha = DateTime.Today;
            else
                lup.Fecha = GlobalVar.FechaTurno();
            lup.Linea = cbbLineaIni.SelectedValue.ToString();
            lup.Modelo = txtModelo2.Text.ToString();
            lup.RPO = txtRPO2.Text.ToString();
            lup.Turno = GlobalVar.gsTurno;
            if (!ConfigLogica.VerificaOmiteValidaLup()) //PERMITE DUPLICAR LINE UP CON FECHA ANTERIOR
            {
                if (LineUpLogica.VerificaRPO(lup))
                {
                    MessageBox.Show("El LineUp del RPO ya se Encuentra Registrado. Verifique de Nuevo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return bValida;
                }
            }

            //** VERIFICAR LAYOUT DE MODELO ANT Y NUEVO **//
            if (!ConfigLogica.VerificaDupModelo())
            {
                if (_lsRevStd == "1")
                {
                    if (_lsModeNew != _lsModeOrig) // buscar por familia
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("El Estandar del modelo no coincide con el Original", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtModelo2.Focus();
                        return bValida;
                    }
                }
                else
                {
                    if (_lsModeNew != _lsModeOrig)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("El modelo no coincide con el Original", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtModelo2.Focus();
                        return bValida;
                    }
                }
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

            Imprimir();
            
        }

        private void Imprimir()
        {
            try
            {
                wfImpresor Impresor = new wfImpresor();
                Impresor._lsProceso = _lsProceso;

                DateTime dtFecha = DateTime.Today;
                if (GlobalVar.gsTurno == "2")
                    dtFecha = GlobalVar.FechaTurno();

                Impresor._ldtFinal = dtFecha;
                Impresor._ldtInicio = dtFecha;
                Impresor._lsIndPlanta = "U";
                Impresor._lsPlanta = cbbPlanta.SelectedValue.ToString();

                Impresor._lsIndFolio = "T";

                Impresor._lsIndLinea = "U";
                Impresor._lsLineaIni = cbbLineaIni.SelectedValue.ToString();
                
                Impresor._lsIndModelo = "1";
                Impresor._lsModelo = txtModelo.Text.ToString();

                Impresor._lsIndRPO = "1";
                Impresor._lsRPO = txtRPO.Text.ToString();
                Impresor._lsTurno = _lsTurno;

                Impresor.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Norificar al Administrador " + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            Cursor = Cursors.WaitCursor;

            try
            {
                LineUpLogica line = new LineUpLogica();
                if (GlobalVar.gsTurno == "1")
                    line.Fecha = DateTime.Today;
                else
                    line.Fecha = GlobalVar.FechaTurno();
                line.RPO = txtRPO.Text.ToString().Trim();
                line.Turno = _lsTurno;

                DataTable dt = new DataTable();

                if (ConfigLogica.VerificaOmiteValidaLup()) //PERMITE DUPLICAR LINE UP CON FECHA ANTERIOR
                    dt = LineUpLogica.ConsultaRPO(line);
                else
                    dt = LineUpLogica.ConsultarRPOxFecha(line);

                if (dt.Rows.Count != 0)
                {
                    _lsFolio = dt.Rows[0][0].ToString();

                    CargarDetalle(_lsFolio);

                    if (dgwEstaciones.RowCount != 0)
                    {
                        if (Guardar(txtRPO2.Text.ToString(), txtModelo2.Text.ToString()))
                        {
                            DialogResult Result = MessageBox.Show("La Caputra de LineUp del Modelo " + txtModelo2.Text.ToString() + " para el RPO " + txtRPO2.Text.ToString().Substring(3) + " se ha Registrado." + Environment.NewLine + "Desea Imprimir el Formato?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (Result == DialogResult.Yes)
                            {
                                Imprimir();
                            }
                            Close();
                        }
                    }
                }
                else
                    MessageBox.Show("La informacion del RPO no se encontró", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                Cursor = Cursors.Default;
            }
            catch(Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "Error " + Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }
      
        private void CargarDetalle(string _asFolio)
        {
            dgwEstaciones.DataSource = null;
            LineUpDetLogica lined = new LineUpDetLogica();
            lined.Folio = long.Parse(_asFolio);
            DataTable dt = LineUpDetLogica.Listar(lined);
            dgwEstaciones.DataSource = dt;
            
        }

        private bool Guardar(string _asRPO,string _asModelo)
        { 
            long lFolio = AccesoDatos.Consec("PRO050");
           
            foreach (DataGridViewRow row in dgwEstaciones.Rows)
            {
                try
                {
                    LineUpDetLogica lined = new LineUpDetLogica();
                    lined.Folio = lFolio;
                    lined.Consec = Convert.ToInt32(row.Cells[1].Value);
                    lined.Linea = cbbLineaIni.SelectedValue.ToString();
                    lined.Modelo = _asModelo;
                    lined.CveEstacion = Convert.ToInt32(row.Cells[4].Value);
                    if (!string.IsNullOrEmpty(row.Cells[5].Value.ToString()))
                        lined.Estacion = Convert.ToString(row.Cells[5].Value);
                    lined.Operacion = Convert.ToString(row.Cells[6].Value);
                    if (!string.IsNullOrEmpty(row.Cells[7].Value.ToString()))
                        lined.NivelReq = Convert.ToString(row.Cells[7].Value);
                    lined.Operador = Convert.ToString(row.Cells[8].Value);
                    lined.NombreOper = Convert.ToString(row.Cells[9].Value);
                    if (!string.IsNullOrEmpty(row.Cells[10].Value.ToString()))
                        lined.NivelOper = Convert.ToString(row.Cells[10].Value);
                    if (!string.IsNullOrEmpty(row.Cells[11].Value.ToString()))
                        lined.LineaOper = Convert.ToString(row.Cells[11].Value);
                    if (!string.IsNullOrEmpty(row.Cells[12].Value.ToString()))
                        lined.Tipo = Convert.ToString(row.Cells[12].Value);
                    if (!string.IsNullOrEmpty(row.Cells[13].Value.ToString()))
                        lined.Tipona = Convert.ToString(row.Cells[13].Value);
                    if (!string.IsNullOrEmpty(row.Cells[14].Value.ToString()))
                        lined.SinNivel = Convert.ToString(row.Cells[14].Value);    
                    lined.Usuario = Convert.ToString(row.Cells[15].Value);

                    LineUpDetLogica.Guardar(lined);
                }
                catch (Exception ie)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "LineUpDetLogica.Guardar(lined)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            LineUpLogica line = new LineUpLogica();
            line.Folio = long.Parse(_lsFolio);
            DataTable dtLn = LineUpLogica.Consultar(line);

            line.Folio = lFolio;
            //if (dtLn.Rows[0]["turno"].ToString() == "1")
            //    line.Fecha = DateTime.Today;
            //else
            //    line.Fecha = GlobalVar.FechaTurno();
            line.Fecha = DateTime.Parse(dtLn.Rows[0]["fecha"].ToString());
            line.Planta = cbbPlanta.SelectedValue.ToString();
            line.Linea = cbbLineaIni.SelectedValue.ToString();
            line.Modelo = _asModelo;
            line.RPO = _asRPO;
            line.Turno = dtLn.Rows[0]["turno"].ToString();
            line.Controla = dtLn.Rows[0]["ind_controlada"].ToString();
            line.Autoriza = dtLn.Rows[0]["id_autorizo"].ToString();
            line.ModeloCtrl = dtLn.Rows[0]["modelo_ctrl"].ToString();
            line.Turno = dtLn.Rows[0]["turno"].ToString();
            line.Usuario = dtLn.Rows[0]["u_id"].ToString();
            line.Duplicado = "1";
            line.FolioDup = long.Parse(dtLn.Rows[0]["folio"].ToString());
            line.UsuarioDup = GlobalVar.gsUsuario;
            line.ModeloDup = dtLn.Rows[0]["modelo"].ToString();
            line.RpoDup = dtLn.Rows[0]["rpo"].ToString();
            line.Core = dtLn.Rows[0]["core"].ToString();
            line.RevStd = dtLn.Rows[0]["ind_revstd"].ToString();
            line.ModRev = dtLn.Rows[0]["modelo_rev"].ToString();
            line.BloqPPH = dtLn.Rows[0]["bloq_pph"].ToString();

            if (LineUpLogica.Guardar(line) == 0)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Error al intentar guardar el Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            txtModelo.Clear();
            txtModelo.Text = _asModelo;
            txtRPO.Clear();
            txtRPO.Text = _asRPO;
            

            Cursor = Cursors.Default;
            return true;
            
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable dt = ConfigLogica.Consultar();
                string sDirec = dt.Rows[0]["directorio"].ToString();
                if (!string.IsNullOrEmpty(sDirec))
                {
                    sDirec += @"\CloverPRO_ManualDuplicaLineUp.pdf";
                    System.Diagnostics.Process.Start(sDirec);
                }
                else
                    MessageBox.Show("No se ha especificado la ubicacion del Manual", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error... " + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion

        #region regCaptura
        private void cbbPlanta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbbPlanta.Text.Length >= 9)
                e.Handled = true;
        }

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CargarLineas();
        }

        private bool ValidaRPO()
        {
            if (!string.IsNullOrEmpty(txtRPO.Text.ToString()))
            {
                string sRPO = txtRPO.Text.ToString().Trim();
                if (sRPO.IndexOf("RPO") == -1)
                    sRPO = "RPO" + sRPO;

                int iPos = sRPO.IndexOf("-");
                int iIni = 0;
                if (iPos > 0)
                {
                    string sIni = sRPO.Substring(iPos + 2, 5);
                    Int32.TryParse(sIni, out iIni);
                    iIni++;

                    sRPO = sRPO.Substring(0, iPos);
                }

                txtRPO.Text = sRPO;

                LineUpLogica lup = new LineUpLogica();
                if (GlobalVar.gsTurno == "1")
                    lup.Fecha = DateTime.Today;
                else
                    lup.Fecha = GlobalVar.FechaTurno();
                lup.RPO = sRPO;
                lup.Turno = GlobalVar.gsTurno;

                DataTable dtRpo = new DataTable();

                if (ConfigLogica.VerificaOmiteValidaLup()) //PERMITE DUPLICAR LINE UP CON FECHA ANTERIOR
                    dtRpo = LineUpLogica.ConsultaRPO(lup); //CONSULTA DATOS DEL ULTIMO RPO DEL TURNO
                else
                    dtRpo = LineUpLogica.ConsultarRPOxFecha(lup);

                if (dtRpo.Rows.Count != 0)
                {
                   
                    lup.Linea = dtRpo.Rows[0]["linea"].ToString();
                    lup.Modelo = dtRpo.Rows[0]["modelo"].ToString();

                    if (!ConfigLogica.VerificaOmiteValidaLup()) //PERMITE DUPLICAR LINE UP CON FECHA ANTERIOR
                    {
                        if (!LineUpLogica.VerificaRPO(lup)) // Valida RPO del Dia
                        {
                            MessageBox.Show("La captura del RPO no pertenece a la fecha actual", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtRPO.Focus();
                            return false;
                        }
                    }

                    cbbPlanta.SelectedValue = dtRpo.Rows[0]["planta"].ToString();
                    CargarLineas();
                    cbbLineaIni.SelectedValue = dtRpo.Rows[0]["linea"].ToString();

                    lblPlanta.Text = "PLANTA " + cbbPlanta.SelectedValue.ToString();
                    lblLIni.Text = "LINEA " + dtRpo.Rows[0]["linea"].ToString();

                    txtModelo.Text = dtRpo.Rows[0]["modelo"].ToString();
                    if(string.IsNullOrEmpty(dtRpo.Rows[0]["modelo_rev"].ToString()))
                        _lsModeOrig = dtRpo.Rows[0]["modelo"].ToString();
                    else
                        _lsModeOrig = dtRpo.Rows[0]["modelo_rev"].ToString();

                    _lsCoreOrig = dtRpo.Rows[0]["core"].ToString();
                    _lsRevStd = dtRpo.Rows[0]["ind_revstd"].ToString();

                    return true;
                }
                else
                {
                    MessageBox.Show("El RPO no se encuentra registrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtRPO.Clear();
                    txtRPO.Focus();
                    return false;
                }
            }
            else
                return false;
            
        }
        private void txtRPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                return;

            if (e.KeyCode != Keys.Enter)
                return;


            if (ValidaRPO())
                txtRPO2.Focus();
            else
                txtRPO.Focus();
        }

      
        private void txtRPO_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRPO, 0);
        }
        private void txtRPO_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRPO, 1);
        }



        #endregion

        private void txtRPO2_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRPO2, 1);
        }

        private void txtRPO2_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtRPO2, 0);
        }

        private void txtRPO2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtRPO2.Text.ToString()))
                return;

            string sRPO = txtRPO2.Text.ToString().Trim();
            if (sRPO.IndexOf("RPO") == -1)
                sRPO = "RPO" + sRPO;

            int iPos = sRPO.IndexOf("-");
            int iIni = 0;
            if (iPos > 0)
            {
                string sIni = sRPO.Substring(iPos + 2, 5);
                Int32.TryParse(sIni, out iIni);
                iIni++;

                sRPO = sRPO.Substring(0, iPos);
            }

            txtRPO2.Text = sRPO;

            if (ConfigLogica.VerificaRpoOrbis())
            {
                RpoLogica rpo = new RpoLogica();
                rpo.RPO = sRPO;
                DataTable dtRpo = RpoLogica.ConsultarOrbis(rpo);
                if (dtRpo.Rows.Count != 0)
                {
                    string sModelo = dtRpo.Rows[0]["Producto"].ToString().ToUpper();
                    string sEstd = dtRpo.Rows[0]["Estandar"].ToString();
                    string sLinea = dtRpo.Rows[0]["Linea"].ToString();
                    string sConv = string.Empty;
                    if (sModelo.IndexOf("CONV") != -1)
                    {
                        sModelo = sModelo.TrimEnd();
                        sModelo = sModelo.Substring(0, sModelo.IndexOf("CONV"));
                        sModelo = sModelo.TrimEnd();
                        sConv = "1";
                    }

                    txtModelo2.Text = sModelo;
                    _lsCoreNew = sEstd;
                    if (_lsRevStd == "1")
                    {
                        //MODELOS RELACIONADOS
                        ModerelaLogica modre = new ModerelaLogica();
                        modre.Moderela = sModelo;
                        DataTable dtRel = ModerelaLogica.ConsultaRelacionado(modre);
                        if (dtRel.Rows.Count != 0)
                        {
                            if (dtRel.Rows.Count > 0)
                            {
                                wfCapturaPopGrid wfMods = new wfCapturaPopGrid(sModelo);
                                wfMods.ShowDialog();
                                _lsModeNew = wfMods._sClave;
                                _lsCoreNew = wfMods._sTipoCore;
                            }
                            else
                            {
                                _lsModeNew = dtRel.Rows[0][1].ToString(); //LAYOUT
                                _lsCoreNew = dtRel.Rows[0][0].ToString();
                            }
                            if (sEstd != _lsCoreNew)
                            {
                                MessageBox.Show("El Estandar seleccionado no corresponde al del RPO", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtRPO2.Clear();
                                txtModelo2.Clear();
                                return;
                            }

                        }
                    }
                    else
                        _lsModeNew = sModelo;

                    //txtModelo_KeyDown(sender, e);
                }
                else
                {
                    MessageBox.Show("El RPO no se encuentra registrado en Orbis", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtModelo2.Clear();
                    txtRPO2.Focus();
                    return;
                }
            }
            else
                txtModelo2.Enabled = true;
        }

        private void txtModelo2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtModelo2.Text) && !string.IsNullOrWhiteSpace(txtModelo2.Text))
            {
                ModeloLogica mod = new ModeloLogica();
                string sMod = txtModelo2.Text.ToString().ToUpper().Trim();

                if(!BuscaModelo(sMod))
                {
                    MessageBox.Show("El Modelo no se encuentra registrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtModelo2.Clear();
                    txtModelo2.Focus();
                    return;
                }

                
                //mod.Modelo = sMod;
                //DataTable datos = ModeloLogica.Consultar(mod);
                
            }
        }

        private bool BuscaModelo(string _asModelo)
        {
            bool bExiste = false;

            DataTable datos = new DataTable();

            string sBucaMod = ConfigLogica.VerificaBuscaModelo();
            
            if (sBucaMod == "R" || sBucaMod == "A")
            {
                
                ModerelaLogica modre = new ModerelaLogica();
                modre.Moderela = _asModelo;
                DataTable dtRel = ModerelaLogica.ConsultaRelacionado(modre);
                if (dtRel.Rows.Count != 0)
                {
                    if (dtRel.Rows.Count > 0)
                    {
                        wfCapturaPopGrid wfMods = new wfCapturaPopGrid(_asModelo);
                        wfMods.ShowDialog();
                        _asModelo = wfMods._sClave;
                        _lsModeNew = wfMods._sClave;
                        _lsCoreNew = wfMods._sTipoCore;
                            
                    }
                    else
                    {
                        _asModelo = dtRel.Rows[0][1].ToString(); //LAYOUT
                        _lsCoreNew = dtRel.Rows[0][0].ToString(); // VE / NVE
                        _lsModeNew = dtRel.Rows[0][1].ToString(); //LAYOUT
                    }


                    ModeloLogica mode = new ModeloLogica();
                    mode.Modelo = _asModelo;
                    datos = ModeloLogica.Consultar(mode);
                    if (datos.Rows.Count != 0)
                        bExiste = true;
                }
            }
            
            if(!bExiste)
            {
                if (sBucaMod == "L" || sBucaMod == "A")
                {
                    ModeloLogica mode = new ModeloLogica();
                    mode.Modelo = _asModelo;
                    datos = ModeloLogica.Consultar(mode);
                    if (datos.Rows.Count != 0)
                    {
                        bExiste = true;
                    }
                }
            }
            
            return bExiste; 
        }
    }
}
