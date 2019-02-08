using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO.Ports;
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
    public partial class wfConfig : Form
    {
        public bool _lbCambio;
        private bool _lbCambioMail;
        private bool _lbCambioAlerta;
        private int _liSDIndex;
        private int _liCons;
        private string _lsTipo;
        private string _lsTurno;
        public static SerialPort iSerialPort = new SerialPort();

        public wfConfig()
        {
            InitializeComponent();
        }
        #region inicio
        private void wfConfig_Activated(object sender, EventArgs e)
        {
            cbbPlanta.Focus();
        }


        private void wfConfig_Load(object sender, EventArgs e)
        {
            

            try
            {
                Inicio();

                DataTable data = new DataTable();
                data = ConfigLogica.Consultar();
                if (data.Rows.Count > 0)
                {
                    txtDirectorio.Text = data.Rows[0]["directorio"].ToString();
                    txtDirecRebox.Text = data.Rows[0]["direc_rma"].ToString();
                    txtFileRebox.Text = data.Rows[0]["filename_rma"].ToString();
                    txtServidor.Text = data.Rows[0]["servidor"].ToString();
                    txtPuerto.Text = data.Rows[0]["puerto"].ToString();
                    txtUsuario.Text = data.Rows[0]["usuario"].ToString();
                    txtContraUser.Text = data.Rows[0]["password"].ToString();
                    txtCorreoUser.Text = data.Rows[0]["correo_sal"].ToString();
                    txtCorreoAdm.Text = data.Rows[0]["correo_dest"].ToString();
                    if (data.Rows[0]["ind_ssl"].ToString() == "1")
                        chbSsl.Checked = true;
                    else
                        chbSsl.Checked = false;
                    if (data.Rows[0]["ind_html"].ToString() == "1")
                        chbHtml.Checked = true;
                    else
                        chbHtml.Checked = false;
                    txtCodBloq.Text = data.Rows[0]["codigo_bloq"].ToString();
                    if (data.Rows[0]["ind_correo"].ToString() == "1")
                        chbIndMail.Checked = true;
                    else
                        chbIndMail.Checked = false;
                    txtLeyendaR.Text = data.Rows[0]["leyenda_rec"].ToString();
                    txtImpEmp.Text = data.Rows[0]["emp_printer"].ToString();
                    txtSerial.Text = data.Rows[0]["emp_serial"].ToString();
                    //LINEUP
                    txtNotaLup.Text = data.Rows[0]["nota_lineup"].ToString();
                    txtFormatoLup.Text = data.Rows[0]["formato_lineup"].ToString();
                    if (data.Rows[0]["omiteval_lineup"].ToString() == "1")
                        chbOmiteLup.Checked = true;
                    else
                        chbOmiteLup.Checked = false;
                    if (data.Rows[0]["rpo_orbis"].ToString() == "1")
                        chbRpoOrb.Checked = true;
                    else
                        chbRpoOrb.Checked = false;
                    cbbBuscaMod.SelectedValue = data.Rows[0]["busca_modelo"].ToString();
                    if (data.Rows[0]["dup_rpomod"].ToString() == "1")
                        chbDupMod.Checked = true;
                    else
                        chbDupMod.Checked = false;
                    if (data.Rows[0]["agesta_lineup"].ToString() == "1")
                        chbAddEsta.Checked = true;
                    else
                        chbAddEsta.Checked = false;
                    if (data.Rows[0]["lineup_regoper"].ToString() == "1")
                        chbSolOper.Checked = true;
                    else
                        chbSolOper.Checked = false;
                    txtCodigoB.Text = data.Rows[0]["eti_codigob"].ToString();
                    txtVA.Text = data.Rows[0]["va_root"].ToString();
                    if (data.Rows[0]["va_lineup"].ToString() == "1")
                        chbVALineup.Checked = true;
                    else
                        chbVALineup.Checked = false;

                    //ETIQUETAS
                    if (data.Rows[0]["omiteval_eti"].ToString() == "1")
                        chbOmiteEti.Checked = true;
                    else
                        chbOmiteEti.Checked = false;

                    //PLANEACION
                    if (data.Rows[0]["aler_setupprev"].ToString() == "1")
                        chbPreSetup.Checked = true;
                    if (!string.IsNullOrEmpty(data.Rows[0]["min_setupprev"].ToString()))
                        txtMinSetup.Text = data.Rows[0]["min_setupprev"].ToString();
                    if (data.Rows[0]["aler_setuphora"].ToString() == "1")
                        chbAlerHoraSetup.Checked = true;
                    if (!string.IsNullOrEmpty(data.Rows[0]["min_setuphora"].ToString()))
                        txtHoraSetup.Text = data.Rows[0]["min_setuphora"].ToString();
                    if (data.Rows[0]["aler_setupdura"].ToString() == "1")
                        chbAlerDuraSetup.Checked = true;
                    if (!string.IsNullOrEmpty(data.Rows[0]["min_setupdura"].ToString()))
                        txtDuraSetup.Text = data.Rows[0]["min_setupdura"].ToString();
                    if (data.Rows[0]["aler_setupmax"].ToString() == "1")
                        chbAlerSetupMax.Checked = true;
                    if (!string.IsNullOrEmpty(data.Rows[0]["min_setupmax"].ToString()))
                        txtSetupMax.Text = data.Rows[0]["min_setupmax"].ToString();
                    if (data.Rows[0]["mon_setupxtd"].ToString() == "1")
                        chbSetupxtd.Checked = true;
                    else
                        chbSetupxtd.Checked = false;

                    if (data.Rows[0]["setup_duration"].ToString() == "1")
                        chbDuracion.Checked = true;
                    txtCantTurno.Text = data.Rows[0]["setup_durturno"].ToString();
                    txtMinCarg.Text = data.Rows[0]["setup_durmin"].ToString();
                    txtIniDura1t.Text = data.Rows[0]["setup_durini1t"].ToString();
                    txtIniDura2t.Text = data.Rows[0]["setup_durini2t"].ToString();
                    txtDurationSource.Text = data.Rows[0]["setup_datasource"].ToString();

                    //KANBAN
                    if (data.Rows[0]["ind_kanban"].ToString() == "1")
                        chbKanban.Checked = true;
                    else
                        chbKanban.Checked = false;
                    txtKanbanDir.Text = data.Rows[0]["kanban_direc"].ToString();
                    txtKanbanFile.Text = data.Rows[0]["kanban_file"].ToString();
                    txtKanStart.Text = data.Rows[0]["kanban_start"].ToString();
                    txtKanEnd.Text = data.Rows[0]["kanban_end"].ToString();
                    txtKanMin.Text = data.Rows[0]["kanban_minutes"].ToString();
                    //GLOBALS
                    if (data.Rows[0]["ind_globals"].ToString() == "1")
                        chbGlobals.Checked = true;
                    else
                        chbGlobals.Checked = false;
                    txtGlobStart.Text = data.Rows[0]["global_start"].ToString();
                    txtGlobEnd.Text = data.Rows[0]["global_end"].ToString();
                    txtGlobmin.Text = data.Rows[0]["global_min"].ToString();

                    /*
                    DataTable dtKp = KanbanPlanLogica.Listar();
                    dgwKanb.DataSource = dtKp;
                    ColumnasKanban(); */

                    //CONEXION
                    txtServer.Text = data.Rows[0]["server_cp"].ToString();
                    if(!string.IsNullOrEmpty(data.Rows[0]["tipo_cp"].ToString()))
                        cbbTipoSer.SelectedValue = data.Rows[0]["tipo_cp"].ToString();
                    txtBd.Text = data.Rows[0]["based_cp"].ToString();
                    txtUser.Text = data.Rows[0]["user_cp"].ToString();
                    txtClave.Text = data.Rows[0]["clave_cp"].ToString();

                    txtServerOrb.Text = data.Rows[0]["server_orb"].ToString();
                    if (!string.IsNullOrEmpty(data.Rows[0]["tipo_orb"].ToString()))
                        cbbTipoOrb.SelectedValue = data.Rows[0]["tipo_orb"].ToString();
                    txtBdOrb.Text = data.Rows[0]["based_orb"].ToString();
                    txtUserOrb.Text = data.Rows[0]["user_orb"].ToString();
                    txtClaveOrb.Text = data.Rows[0]["clave_orb"].ToString();
                    txtPuertoOrb.Text = data.Rows[0]["puerto_orb"].ToString();
                    txtCodigoEmp.Text = data.Rows[0]["cod_desbcomp"].ToString();

                    //CLOVERMP
                    txtDirTime.Text = data.Rows[0]["dirmp_timedif"].ToString();
                    txtFileTime.Text = data.Rows[0]["filemp_timedif"].ToString();
                    if(!string.IsNullOrEmpty(data.Rows[0]["ind_cargtimedif"].ToString()))
                    {
                        if (data.Rows[0]["ind_cargtimedif"].ToString() == "1")
                            chbAutoTime.Checked = true;
                        else
                            chbAutoTime.Checked = false;
                    }
                    txtHoraTime.Text = data.Rows[0]["hora_timedif"].ToString();
                    if (!string.IsNullOrEmpty(data.Rows[0]["tipo_timedif"].ToString()))
                        cbbTipoHrTime.SelectedValue = data.Rows[0]["tipo_timedif"].ToString();

                    //ALMACEN
                    if (!string.IsNullOrEmpty(data.Rows[0]["rpo_validasecuencia"].ToString()))
                    {
                        if (data.Rows[0]["rpo_validasecuencia"].ToString() == "1")
                            chbSecuRPO.Checked = true;
                        else
                            chbSecuRPO.Checked = false;
                    }
                    if (!string.IsNullOrEmpty(data.Rows[0]["omite_almestatus"].ToString()))
                    {
                        if (data.Rows[0]["omite_almestatus"].ToString() == "1")
                            chbOmiteEst.Checked = true;
                        else
                            chbOmiteEst.Checked = false;
                    }
                    if (!string.IsNullOrEmpty(data.Rows[0]["omite_almlinea"].ToString()))
                    {
                        if (data.Rows[0]["omite_almlinea"].ToString() == "1")
                            chbOmiteLineas.Checked = true;
                        else
                            chbOmiteLineas.Checked = false;
                    }

                    //KANBAN ALMACEN
                    txtAlmIni.Text = data.Rows[0]["alm_kbhrini"].ToString();
                    txtAlmFin.Text = data.Rows[0]["alm_kbhrfin"].ToString();
                    txtAlmCorte1.Text = data.Rows[0]["alm_corte1"].ToString();
                    txtAlmCorte2.Text = data.Rows[0]["alm_corte2"].ToString();
                    txtAlmCorte3.Text = data.Rows[0]["alm_corte3"].ToString();
                    txtAlmMeta1.Text = data.Rows[0]["alm_meta1"].ToString();
                    txtAlmMeta2.Text = data.Rows[0]["alm_meta2"].ToString();
                    txtAlmMeta3.Text = data.Rows[0]["alm_meta3"].ToString();
                    if (!string.IsNullOrEmpty(data.Rows[0]["alert_kanbalm"].ToString()))
                    {
                        if (data.Rows[0]["alert_kanbalm"].ToString() == "1")
                            chbAlmKanban.Checked = true;
                        else
                            chbAlmKanban.Checked = false;
                    }
                    if (data.Rows[0]["alm_indcte4"].ToString() == "1")
                        chbCorte4.Checked = true;
                    else
                        chbCorte4.Checked = false;
                    txtAlmCorte4.Text = data.Rows[0]["alm_corte4"].ToString();
                }
            }
            catch(Exception ie)
            {
                MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "ConfigLogica.Consultar()" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }            
        }

        private void Inicio()
        {
            _lbCambio = false;
            txtCorreo.Clear();

            DataTable data = PlantaLogica.Listar();
            cbbPlanta.DataSource = data;
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.SelectedIndex = -1;

            cbbTipo.ResetText();
            Dictionary<string, string> Tipo = new Dictionary<string, string>();
            Tipo.Add("T", "To");
            Tipo.Add("C", "Cc");
            cbbTipo.DataSource = new BindingSource(Tipo, null);
            cbbTipo.DisplayMember = "Value";
            cbbTipo.ValueMember = "Key";
            cbbTipo.SelectedIndex = 0;

            cbbTurno.ResetText();
            Dictionary<string, string> Turno = new Dictionary<string, string>();
            Turno.Add("1", "1");
            Turno.Add("2", "2");
            cbbTurno.DataSource = new BindingSource(Turno, null);
            cbbTurno.DisplayMember = "Value";
            cbbTurno.ValueMember = "Key";
            cbbTurno.SelectedIndex = 0;

            cbbBuscaMod.ResetText();
            Dictionary<string, string> Busca = new Dictionary<string, string>();
            Busca.Add("L", "Listado de Modelos");
            Busca.Add("R", "Revision Std");
            Busca.Add("A", "Ambas");
            cbbBuscaMod.DataSource = new BindingSource(Busca, null);
            cbbBuscaMod.DisplayMember = "Value";
            cbbBuscaMod.ValueMember = "Key";
            cbbBuscaMod.SelectedIndex = -1;

            txtServidor.Clear();
            txtPuerto.Clear();
            txtUsuario.Clear();
            txtContraUser.Clear();
            txtCorreoUser.Clear();
            txtImpEmp.Clear();

            chbSsl.Checked = false;
            chbHtml.Checked = false;
            chbIndMail.Checked = false;
            chbRpoOrb.Checked = false;

            cbbTipoSer.ResetText();
            Dictionary<string, string> TipoS = new Dictionary<string, string>();
            TipoS.Add("MS", "MSSQL");
            TipoS.Add("MY", "MySQL");
            cbbTipoSer.DataSource = new BindingSource(TipoS, null);
            cbbTipoSer.DisplayMember = "Value";
            cbbTipoSer.ValueMember = "Key";
            cbbTipoSer.SelectedIndex = 0;

            cbbTipoOrb.ResetText();
            Dictionary<string, string> TipoO = new Dictionary<string, string>();
            TipoO.Add("MS", "MSSQL");
            TipoO.Add("MY", "MySQL");
            cbbTipoOrb.DataSource = new BindingSource(TipoO, null);
            cbbTipoOrb.DisplayMember = "Value";
            cbbTipoOrb.ValueMember = "Key";
            cbbTipoOrb.SelectedIndex = -1;


            cbbTipoHrTime.ResetText();
            Dictionary<string, string> Tipohr = new Dictionary<string, string>();
            Tipohr.Add("AM", "AM");
            Tipohr.Add("PM", "PM");
            cbbTipoHrTime.DataSource = new BindingSource(Tipohr, null);
            cbbTipoHrTime.DisplayMember = "Value";
            cbbTipoHrTime.ValueMember = "Key";
            cbbTipoHrTime.SelectedIndex = 0;

            AlertaDestLogica aler = new AlertaDestLogica();
            aler.Alerta = "KANBALM";

            DataTable dt = AlertaDestLogica.Listar(aler);
            dgwAlertaAlm.DataSource = dt;
            AlertaColumnas();
        }
        #endregion

        #region Correos
        private void LastRow()
        {
            dgwCorreos.ClearSelection();
            int iRow = dgwCorreos.Rows.Count - 1;
            dgwCorreos.Rows[iRow].Selected = true;
            dgwCorreos.FirstDisplayedScrollingRowIndex = iRow;
        }
        private void CargarColumnas()
        {
            if (dgwCorreos.Rows.Count == 0)
            {

                DataTable dtNew = new DataTable("Serv");
                dtNew.Columns.Add("planta", typeof(string));
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("Correo", typeof(string));
                dtNew.Columns.Add("Tipo", typeof(string));
                dtNew.Columns.Add("Turno", typeof(string));

                dgwCorreos.DataSource = dtNew;

            }

            dgwCorreos.Columns[0].Visible = false;
            dgwCorreos.Columns[1].Visible = false;

            dgwCorreos.Columns[2].Width = ColumnWith(dgwCorreos, 80);
            dgwCorreos.Columns[3].Width = ColumnWith(dgwCorreos, 10);
            dgwCorreos.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwCorreos.Columns[4].Width = ColumnWith(dgwCorreos, 10);
            dgwCorreos.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            LastRow();

        }

        private void AlertaColumnas()
        {
            if (dgwAlertaAlm.Rows.Count == 0)
            {

                DataTable dtNew = new DataTable("data");
                dtNew.Columns.Add("alerta", typeof(string));
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("Destino", typeof(string));
                dtNew.Columns.Add("Tipo", typeof(string));
                dtNew.Columns.Add("Turno", typeof(string));

                dgwAlertaAlm.DataSource = dtNew;

            }

            dgwAlertaAlm.Columns[0].Visible = false;
            dgwAlertaAlm.Columns[1].Visible = false;

            dgwAlertaAlm.Columns[2].Width = ColumnWith(dgwAlertaAlm, 78);
            dgwAlertaAlm.Columns[3].Width = ColumnWith(dgwAlertaAlm, 10);
            dgwAlertaAlm.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwAlertaAlm.Columns[4].Width = ColumnWith(dgwAlertaAlm, 12);
            dgwAlertaAlm.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            

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


        private void dgwCorreos_Click(object sender, EventArgs e)
        {
            if (dgwCorreos.CurrentRow == null)
                return;

            _liSDIndex = dgwCorreos.CurrentRow.Index;
            DataGridViewRow row = dgwCorreos.Rows[_liSDIndex];
            if (!string.IsNullOrEmpty(row.Cells[2].Value.ToString()))
            {
                if (row.Cells[1].Value != null)
                    _liCons = int.Parse(row.Cells[1].Value.ToString());

                if (row.Cells[3].Value != null)
                {
                    if (row.Cells[3].Value.ToString() == "To")
                        _lsTipo = "T";
                    else
                        _lsTipo = "C";
                }
                if (row.Cells[4].Value != null)
                {
                    _lsTurno = row.Cells[4].Value.ToString();
                }

                string sCorreo = row.Cells[2].Value.ToString();
                int iPos = sCorreo.IndexOf("@");
                sCorreo = sCorreo.Substring(0, iPos);

                txtCorreo.Text = sCorreo;
                cbbTipo.SelectedValue = _lsTipo;
                cbbTurno.SelectedValue = _lsTurno;

            }
            else
            {
                _liCons = 0;
                _lsTipo = null;
                _lsTurno = null;

                txtCorreo.Clear();
                cbbTipo.SelectedIndex = 0;
                cbbTurno.SelectedIndex = 0;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de especificar la Planta a la que pertenece el estinatario", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return;
            }


            if (string.IsNullOrEmpty(txtCorreo.Text) || string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("Favor de especificar el correo del destinatario que desar agregar",Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
                txtCorreo.Focus();
                return;
            }
        
            if(cbbTipo.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de especificar el tipo de destinatario", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbTipo.Focus();
                return;
            }

            if (cbbTurno.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de especificar el turno del destinatario", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbTurno.Focus();
                return;
            }

            string sCorreo = txtCorreo.Text.ToString() + lblHost.Text.ToString();

            if (dgwCorreos.Rows.Count >= 0)
            {
                int iFind = -1;
                foreach (DataGridViewRow crow in dgwCorreos.Rows)
                {
                    if (crow.Cells[2].Value != null && crow.Cells[2].Value.ToString().Equals(sCorreo))
                    {
                        iFind = crow.Index;
                        break;
                    }
                }

                if (iFind != -1)
                {
                    DataGridViewRow crow = dgwCorreos.Rows[_liSDIndex];
                    if (crow.Cells[2].Value != null)
                    {
                        crow.Cells[2].Value = sCorreo;
                        crow.Cells[3].Value = cbbTipo.Text.ToString();
                        crow.Cells[4].Value = cbbTurno.Text.ToString();
                        _lbCambioMail = true;
                    }
                    //MessageBox.Show("El Destinatario ya se encuentra en la lista", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //txtCorreo.Clear();
                    //cbbTipo.SelectedIndex = 0;
                    //dgwCorreos.ClearSelection();
                }
                else
                {
                    DataTable dt = dgwCorreos.DataSource as DataTable;
                    dt.Rows.Add(cbbPlanta.SelectedValue.ToString(), 0, sCorreo, cbbTipo.Text.ToString(), cbbTurno.Text.ToString());
                    _lbCambioMail = true;
                }
            }
            else
            {
                DataTable dt = dgwCorreos.DataSource as DataTable;
                dt.Rows.Add(cbbPlanta.SelectedValue.ToString(), _liCons, sCorreo, cbbTipo.Text.ToString(), cbbTurno.Text.ToString());
                _lbCambioMail = true;
            }

            LastRow();

            txtCorreo.Clear();
            cbbTipo.SelectedIndex = 0;
            cbbTurno.SelectedIndex = 0;



        }
        private void dgwCorreos_SelectionChanged(object sender, EventArgs e)
        {
            dgwCorreos_Click(sender, e);
        }

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                DataTable dataD = new DataTable();
                ConfigDestLogica conf = new ConfigDestLogica();
                conf.Planta = cbbPlanta.SelectedValue.ToString();

                dataD = ConfigDestLogica.Listar(conf);
                dgwCorreos.DataSource = dataD;
                CargarColumnas();
                LastRow();

                txtCorreo.Clear();
                cbbTipo.SelectedIndex = 0;
                cbbTurno.SelectedIndex = 0;
            }
            catch (Exception ie)
            {
                MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "ConfigDestLogica.Listar()" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        #endregion

        #region Guardar
        private bool Valida()
        {
            bool bValida = false;

            if (string.IsNullOrEmpty(txtDirectorio.Text) || string.IsNullOrWhiteSpace(txtDirectorio.Text))
            {
                MessageBox.Show("Favor de especificar el Directorio", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.SelectedIndex = 0;
                txtDirectorio.Focus();
                return bValida;
            }

            if (string.IsNullOrEmpty(txtServidor.Text) || string.IsNullOrWhiteSpace(txtServidor.Text))
            {
                MessageBox.Show("Favor de especificar el Servidor de Correo", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.SelectedIndex = 1;
                txtServidor.Focus();
                return bValida;
            }

            if (string.IsNullOrEmpty(txtPuerto.Text) || string.IsNullOrWhiteSpace(txtPuerto.Text))
            {
                MessageBox.Show("Favor de especificar el Puerto de Salida", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.SelectedIndex = 1;
                txtPuerto.Focus();
                return bValida;
            }
            if (string.IsNullOrEmpty(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Favor de especificar el Usuario", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.SelectedIndex = 1;
                txtUsuario.Focus();
                return bValida;
            }
            if (string.IsNullOrEmpty(txtContraUser.Text) || string.IsNullOrWhiteSpace(txtContraUser.Text))
            {
                MessageBox.Show("Favor de especificar la contraseña del Usuario", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.SelectedIndex = 1;
                txtContraUser.Focus();
                return bValida;
            }
            if (string.IsNullOrEmpty(txtCorreoUser.Text) || string.IsNullOrWhiteSpace(txtCorreoUser.Text))
            {
                MessageBox.Show("Favor de especificar el Correo de Salida", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.SelectedIndex = 1;
                txtCorreoUser.Focus();
                return bValida;
            }

            if (!string.IsNullOrEmpty(txtCodBloq.Text) && !string.IsNullOrWhiteSpace(txtCodBloq.Text))
            {
                if (txtCodBloq.Text.ToString().Length < 4)
                    txtCodBloq.Text = txtCodBloq.Text.ToString().PadLeft(4, '0');
            }

            if (string.IsNullOrEmpty(txtCodigoB.Text) || string.IsNullOrWhiteSpace(txtCodigoB.Text))
            {
                MessageBox.Show("Favor de especificar el Código de Cartucho Bueno", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCodigoB.Focus();
                return bValida;
            }

            if(chbPreSetup.Checked && string.IsNullOrEmpty(txtMinSetup.Text))
            {
                MessageBox.Show("Favor de especificar el tiempo de tolerancia para la Alerta", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMinSetup.Focus();
                return bValida;
            }

            if (chbAlerHoraSetup.Checked && string.IsNullOrEmpty(txtHoraSetup.Text))
            {
                MessageBox.Show("Favor de especificar el horario limite para la Alerta", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtHoraSetup.Focus();
                return bValida;
            }

            if (chbCorte4.Checked && string.IsNullOrEmpty(txtAlmCorte4.Text))
            {
                MessageBox.Show("Favor de especificar el horario del Corte 4", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAlmCorte4.Focus();
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
                    ConfigLogica conf = new ConfigLogica();
                    conf.Directorio = txtDirectorio.Text.ToString();
                    conf.DirecRMA = txtDirecRebox.Text.ToString();
                    conf.FileRMA = txtFileRebox.Text.ToString();
                    conf.Servidor = txtServidor.Text.ToString();
                    if (!string.IsNullOrEmpty(txtPuerto.Text))
                        conf.Puerto = Int32.Parse(txtPuerto.Text);
                    else
                        conf.Puerto = 0;
                    conf.Usuario = txtUsuario.Text.ToString();
                    conf.Password = txtContraUser.Text.ToString();
                    conf.CorreoSal = txtCorreoUser.Text.ToString();
                    conf.CorreoDest = txtCorreoAdm.Text.ToString();
                    conf.CodigoBloq = txtCodBloq.Text.ToString();
                    if (chbIndMail.Checked)
                        conf.IndCorreo = "1";
                    else
                        conf.IndCorreo = "0";
                    if (chbSsl.Checked)
                        conf.SSL = "1";
                    else
                        conf.SSL = "0";
                    if (chbHtml.Checked)
                        conf.HTML = "1";
                    else
                        conf.HTML = "0";
                    conf.LeyendaRec = txtLeyendaR.Text.ToString();
                    conf.EmpPrinter = txtImpEmp.Text.ToString();
                    conf.EmpSerial = txtSerial.Text.ToString();
                    conf.NotaLup = txtNotaLup.Text.ToString();
                    conf.FormatoLup = txtFormatoLup.Text.ToString();
                    if (chbOmiteLup.Checked)
                        conf.OmiteLup = "1";
                    else
                        conf.OmiteLup = "0";
                    if (chbRpoOrb.Checked)
                        conf.RpoOrbis = "1";
                    else
                        conf.RpoOrbis = "0";
                    conf.BuscaModelo = cbbBuscaMod.SelectedValue.ToString();
                    if (chbDupMod.Checked)
                        conf.DupRpoMod = "1";
                    else
                        conf.DupRpoMod = "0";
                    if (chbOmiteEti.Checked)
                        conf.OmiteEti = "1";
                    else
                        conf.OmiteEti = "0";
                    if (chbAddEsta.Checked)
                        conf.AgregaEst = "1";
                    else
                        conf.AgregaEst = "0";
                    if (chbSolOper.Checked)
                        conf.SolicitaOper = "1";
                    else
                        conf.SolicitaOper = "0";
                    conf.CodigoBueno = txtCodigoB.Text.ToString();
                    conf.VisualAidRoot = txtVA.Text.ToString();
                    if (chbVALineup.Checked)
                        conf.VALineup = "1";
                    else
                        conf.VALineup = "0";

                    //CONEXION
                    conf.ServerCp = txtServer.Text.ToString();
                    conf.TipoCp = cbbTipoSer.SelectedValue.ToString();
                    conf.BasedCp = txtBd.Text.ToString();
                    conf.UserCp = txtUser.Text.ToString();
                    conf.ClaveCp = txtClave.Text.ToString();

                    conf.ServerOrb = txtServerOrb.Text.ToString();
                    if (cbbTipoOrb.SelectedIndex != -1)
                        conf.TipoOrb = cbbTipoOrb.SelectedValue.ToString();
                    conf.BasedOrb = txtBdOrb.Text.ToString();
                    conf.UserOrb = txtUserOrb.Text.ToString();
                    conf.ClaveOrb = txtClaveOrb.Text.ToString();
                    conf.PuertoOrb = txtPuertoOrb.Text.ToString();
                    conf.CodigoEmp = txtCodigoEmp.Text.ToString().Trim().TrimEnd();
                    //PLANEACION
                    if (chbPreSetup.Checked)
                    {
                        conf.AlerPreSetup = "1";
                        conf.MinPreSetup = int.Parse(txtMinSetup.Text);
                    }
                    else
                    {
                        conf.AlerPreSetup = "0";
                        conf.MinPreSetup = 0;
                    }
                    
                    if(chbAlerHoraSetup.Checked)
                    {
                        conf.AlerHoraSetup = "1";
                        conf.HoraSetup = txtHoraSetup.Text.ToString();
                    }
                    else
                    {
                        conf.AlerHoraSetup = "0";
                        conf.HoraSetup = string.Empty;
                    }

                    if(chbAlerDuraSetup.Checked)
                    {
                        conf.AlerDuraSetup = "1";
                        conf.MinDuraSetup = int.Parse(txtDuraSetup.Text);
                    }
                    else
                    {
                        conf.AlerDuraSetup = "0";
                        conf.MinDuraSetup = 0;
                    }
                    if(chbAlerSetupMax.Checked)
                    {
                        conf.SetupMax = "1";
                        conf.MinSetupMax = byte.Parse(txtSetupMax.Text);
                    }
                    else
                    {
                        conf.SetupMax = "0";
                        conf.MinSetupMax = 0;
                    }

                    if (chbSetupxtd.Checked)
                        conf.Setupxtd = "1";
                    else
                        conf.Setupxtd = "0";

                    if (chbDuracion.Checked)
                        conf.SetupDura = "1";
                    else
                        conf.SetupDura = "0";

                    conf.SetupTurno = int.Parse(txtCantTurno.Text.ToString());
                    conf.SetupMin = int.Parse(txtMinCarg.Text.ToString());
                    conf.SetupIni1t = txtIniDura1t.Text.ToString();
                    conf.SetupIni2t = txtIniDura2t.Text.ToString();
                    conf.SetupSoruce = txtDurationSource.Text.ToString();

                    //KANBAN
                    if (chbKanban.Checked)
                        conf.Kanban = "1";
                    else
                        conf.Kanban = "0";
                    conf.KanbanDiect = txtKanbanDir.Text.ToString();
                    conf.KanbanFile = txtKanbanFile.Text.ToString();
                    conf.KanbanMins = byte.Parse(txtKanMin.Text.ToString());
                    conf.KanbanStart = txtKanStart.Text.ToString();
                    conf.KanbanEnd = txtKanEnd.Text.ToString();

                    //GLOBALS
                    if (chbGlobals.Checked)
                        conf.Globals = "1";
                    else
                        conf.Globals = "0";
                    conf.GlobStart = txtGlobStart.Text.ToString();
                    conf.GlobEnd = txtGlobEnd.Text.ToString();
                    conf.GlobalsMins = byte.Parse(txtGlobmin.Text.ToString());

                    //MP
                    conf.DirectMP = txtDirTime.Text.ToString();
                    conf.FileTimeDif = txtFileTime.Text.ToString();
                    if (chbAutoTime.Checked)
                        conf.AutoTimeDif = "1";
                    else
                        conf.AutoTimeDif = "0";
                    conf.HoraTimeDif = txtHoraTime.Text.ToString();
                    conf.TipoHrTime = cbbTipoHrTime.SelectedValue.ToString();
                    //ALMACEN
                    if (chbSecuRPO.Checked)
                        conf.SecuenciaRPO = "1";
                    else
                        conf.SecuenciaRPO = "0";
                    if (chbOmiteEst.Checked)
                        conf.OmiteSecEst = "1";
                    else
                        conf.OmiteSecEst = "0";
                    if (chbOmiteLineas.Checked)
                        conf.OmiteSecLinea = "1";
                    else
                        conf.OmiteSecLinea = "0";
                    int iHora;
                    if (int.TryParse(txtAlmIni.Text, out iHora))
                        conf.AlmKanHrIni = iHora;
                    if (int.TryParse(txtAlmFin.Text, out iHora))
                        conf.AlmKanHrFin = iHora;
                    conf.AlmCorte1 = txtAlmCorte1.Text.ToString();
                    conf.AlmCorte2 = txtAlmCorte2.Text.ToString();
                    conf.AlmCorte3 = txtAlmCorte3.Text.ToString();
                    int iMeta = 0;
                    if(int.TryParse(txtAlmMeta1.Text,out iMeta))
                        conf.AlmMeta1 = int.Parse(txtAlmMeta1.Text.ToString());
                    if (int.TryParse(txtAlmMeta2.Text, out iMeta))
                        conf.AlmMeta2 = int.Parse(txtAlmMeta2.Text.ToString());
                    if (int.TryParse(txtAlmMeta3.Text, out iMeta))
                        conf.AlmMeta3 = int.Parse(txtAlmMeta3.Text.ToString());

                    if (chbAlmKanban.Checked)
                        conf.AlmKanban = "1";
                    else
                        conf.AlmKanban = "0";
                    if (chbCorte4.Checked)
                        conf.AlmIndCorte4 = "1";
                    else
                        conf.AlmIndCorte4 = "0";
                    conf.AlmCorte4 = txtAlmCorte4.Text.ToString();

                    if (ConfigLogica.Guardar(conf) > 0)
                    {
                        ConfigDestLogica confD = new ConfigDestLogica();

                        foreach (DataGridViewRow row in dgwCorreos.Rows)
                        {
                            if (row.Cells[2].Value == null)
                                continue;

                            if (row.Cells[1].Value != null)
                                confD.Consec = Int32.Parse(row.Cells[1].Value.ToString());
                            else
                                confD.Consec = 0;
                            confD.Planta = row.Cells[0].Value.ToString();
                            confD.Correo = row.Cells[2].Value.ToString();
                            confD.Tipo = row.Cells[3].Value.ToString().Substring(0,1);
                            confD.Turno = row.Cells[4].Value.ToString();

                            if (ConfigDestLogica.Guardar(confD) > 0)
                                continue;
                            else
                            {
                                MessageBox.Show("Error al intentar guardar la Configuración del Correo", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }

                        foreach(DataGridViewRow row in dgwAlertaAlm.Rows)
                        {
                            if (row.Index == dgwAlertaAlm.RowCount - 1)
                                continue;

                            AlertaDestLogica aler = new AlertaDestLogica();
                            
                            if (row.Cells[2].Value == null)
                                continue;
                            int iCons = 0;
                            if(int.TryParse(row.Cells[1].Value.ToString(),out iCons))
                                aler.Consec = iCons;
                            else
                                aler.Consec = 0;
                            /*
                            if (row.Cells[1].Value != null)
                                aler.Consec = Int32.Parse(row.Cells[1].Value.ToString());
                            else
                                aler.Consec = 0;
                                */
                            aler.Alerta = "KANBALM";
                            aler.Destino = row.Cells[2].Value.ToString();
                            aler.Tipo = row.Cells[3].Value.ToString().ToUpper();
                            aler.Turno = row.Cells[4].Value.ToString();

                            if (AlertaDestLogica.Guardar(aler) > 0)
                                continue;
                            else
                            {
                                MessageBox.Show("Error al intentar guardar los destinatarios de la Alerta", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }

                        return true;
                    }

                    else
                    {
                        MessageBox.Show("Error al intentar guardar la Configuración del Correo", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch(Exception ie)
                {
                    MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "ConfigLogica.Guardar(conf)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            else
                return false;
        }

        #endregion

        #region regBotones
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            if(Guardar())
            {
                if(cbbPlanta.SelectedIndex != -1)
                {
                    DataTable dataD = new DataTable();
                    ConfigDestLogica conf = new ConfigDestLogica();
                    conf.Planta = cbbPlanta.SelectedValue.ToString();

                    dataD = ConfigDestLogica.Listar(conf);
                    dgwCorreos.DataSource = dataD;
                    CargarColumnas();
                    LastRow();

                    txtCorreo.Clear();
                    cbbTipo.SelectedIndex = 0;
                    cbbTurno.SelectedIndex = 0;
                }
            }
        }
     

        private void btnDirectorio_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDirectorio.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnPrueba_Click(object sender, EventArgs e)
        {
            wfEnviaCorreo envMail = new wfEnviaCorreo();
            envMail.Show();
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            try
            {


                if (tabControl1.SelectedIndex == 1 && dgwCorreos.Rows.Count > 0)
                {
                    if (dgwCorreos.CurrentRow == null)
                        return;

                    if (!dgwCorreos.SelectedRows[0].IsNewRow)
                    {
                        string sCons = dgwCorreos.SelectedCells[1].Value.ToString();

                        if (!string.IsNullOrEmpty(sCons) && sCons != "0")
                        {
                            DialogResult Result = MessageBox.Show("Desea eliminar el destinatario " + dgwCorreos.SelectedCells[2].Value.ToString() + " ?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (Result == DialogResult.Yes)
                            {
                                ConfigDestLogica dest = new ConfigDestLogica();
                                dest.Planta = dgwCorreos.SelectedCells[0].Value.ToString();
                                dest.Consec = int.Parse(sCons);
                                ConfigDestLogica.Eliminar(dest);
                            }
                        }
                        dgwCorreos.Rows.RemoveAt(dgwCorreos.SelectedRows[0].Index);
                        _lbCambioMail = true;
                    }

                    LastRow();
                }
                else
                {
                    if (tabControl1.SelectedIndex == 8 && dgwAlertaAlm.Rows.Count > 0)
                    {
                        if (dgwAlertaAlm.CurrentRow == null)
                            return;
                        int iIdx = dgwAlertaAlm.SelectedCells[0].RowIndex;
                        if (iIdx != dgwAlertaAlm.RowCount - 1)
                        {
                            int iCons = 0;
                            if (int.TryParse(dgwAlertaAlm[1,iIdx].Value.ToString(), out iCons))
                            {
                                DialogResult Result = MessageBox.Show("Desea eliminar el destinatario de la Alerta ?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                                if (Result == DialogResult.Yes)
                                {
                                    AlertaDestLogica dest = new AlertaDestLogica();
                                    dest.Alerta = dgwAlertaAlm[0,iIdx].Value.ToString();
                                    dest.Consec = iCons;
                                    AlertaDestLogica.Eliminar(dest);
                                }
                            }
                            dgwAlertaAlm.Rows.RemoveAt(iIdx);
                            _lbCambioAlerta = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strException = string.Empty;
            string strComPort = txtSerial.Text.ToString();
            int nBaudrate = Convert.ToInt32(9600);

            int nRet = OpenCom(strComPort, nBaudrate, out strException);
            if (nRet != 0)
            {
                MessageBox.Show("Connect reader failed, due to: " + strException, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Reader connected " + strComPort + "@" + nBaudrate.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
            }

            iSerialPort.Close();

            return;
        }
        #endregion

        #region regCaptura
        private void cbbTipo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
        }
        private void cbbBuscaMod_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
        }
        private void cbbTurno_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
        }

        public static int OpenCom(string strPort, int nBaudrate, out string strException)
        {

            strException = string.Empty;

            if (iSerialPort.IsOpen)
            {
                iSerialPort.Close();
            }

            try
            {
                iSerialPort.PortName = strPort;
                iSerialPort.BaudRate = nBaudrate;
                iSerialPort.ReadTimeout = 200;
                iSerialPort.DataBits = 8;
                iSerialPort.Parity = Parity.None;
                iSerialPort.StopBits = StopBits.One;
                iSerialPort.Open();
            }
            catch (System.Exception ex)
            {
                strException = ex.Message;
                return -1;
            }
            return 0;
        }

        private void chbPreSetup_CheckedChanged(object sender, EventArgs e)
        {
            txtMinSetup.Enabled = chbPreSetup.Checked;
        }

        private void chbAlerHoraSetup_CheckedChanged(object sender, EventArgs e)
        {
            txtHoraSetup.Enabled = chbAlerHoraSetup.Checked;
        }

        private void chbAlerDuraSetup_CheckedChanged(object sender, EventArgs e)
        {
            txtDuraSetup.Enabled = chbAlerDuraSetup.Checked;
        }

        private void btnDirecRma_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDirecRebox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void chbAutoTime_CheckedChanged(object sender, EventArgs e)
        {
            txtHoraTime.Enabled = chbAutoTime.Checked;
            cbbTipoHrTime.Enabled = chbAutoTime.Checked;
        }

        private void btnDirMP_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDirTime.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnDirecKb_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtKanbanDir.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnDirecRma_Click_1(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDirecRebox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void chbDuracion_CheckedChanged(object sender, EventArgs e)
        {
            txtCantTurno.Enabled = chbDuracion.Checked;
            txtMinCarg.Enabled = chbDuracion.Checked;
            txtIniDura1t.Enabled = chbDuracion.Checked;
            txtIniDura2t.Enabled = chbDuracion.Checked;
            txtDurationSource.Enabled = chbDuracion.Checked;
        }

        private void chbAlerSetupMax_CheckedChanged(object sender, EventArgs e)
        {
            txtSetupMax.Enabled = chbAlerSetupMax.Checked;
        }

        #endregion



    }
}