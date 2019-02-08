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
    public partial class wfMonitorLineSetUp : Form
    {
        public bool _lbCambio;
        private bool _lbCambioDet;
        public string _lsParam;
        private string _lsProceso = "PLA030";
        private string _lsFolioAnt;
        private string _lsTurno;
        private string _lsArea;
        private string _lsIndDuracion;

        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;
        
        public wfMonitorLineSetUp()
        {
            InitializeComponent();
            
            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }
        
        #region regInicio
        private void wfMonitorLineSetUp_Load(object sender, EventArgs e)
        {
            _lsTurno = GlobalVar.gsTurno;

            Inicio();

            WindowState = FormWindowState.Maximized;

        }

        private void Inicio()
        {
            if (ConfigLogica.VerificaCapturaDuracion())
                _lsIndDuracion = "1";
            else
                _lsIndDuracion = "0";

            dtpFechaFin.ResetText();

            chbPlanta.Checked = false;
            cbbPlanta.Enabled = false;
            cbbPlanta.ResetText();
            DataTable dtPl = PlantaLogica.Listar();
            cbbPlanta.DataSource = dtPl;
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.SelectedIndex = -1;
            if (!string.IsNullOrEmpty(GlobalVar.gsPlanta) && GlobalVar.gsPlanta != "MON" && GlobalVar.gsArea == "SUP")
            {
                chbPlanta.Checked = true;
                cbbPlanta.SelectedValue = GlobalVar.gsPlanta;

                cbbLinea.ResetText();
                LineaLogica lin = new LineaLogica();
                lin.Planta = cbbPlanta.SelectedValue.ToString();
                DataTable data = LineaLogica.LineaPlanta(lin);
                cbbLinea.DataSource = data;
                cbbLinea.DisplayMember = "nombre";
                cbbLinea.ValueMember = "linea";
                cbbLinea.SelectedIndex = -1;
            }

            chbLinea.Checked = false;
            cbbLinea.Enabled = false;
            cbbLinea.ResetText();

            cbbTurno.Enabled = true;
            chbTurno.Checked = false;
            Dictionary<string, string> Turno = new Dictionary<string, string>();
            Turno.Add("1", "1");
            Turno.Add("2", "2");
            cbbTurno.DataSource = new BindingSource(Turno, null);
            cbbTurno.DisplayMember = "Value";
            cbbTurno.ValueMember = "Key";
            if(_lsTurno == "2")
                cbbTurno.SelectedIndex = 1;
            else
                cbbTurno.SelectedIndex = 0;
            cbbTurno.Enabled = false;

            chbEstatus.Checked = false;
            cbbEstatus.Enabled = false;
            Dictionary<string, string> Est = new Dictionary<string, string>();
            Est.Add("E", "EN ESPERA");
            Est.Add("P", "EN PROCESO");
            Est.Add("R", "REALIZADO");
            Est.Add("T", "EN TIEMPO");
            Est.Add("F", "FUERA DE TIEMPO");
            Est.Add("C", "CANCELADO");
            cbbEstatus.DataSource = new BindingSource(Est, null);
            cbbEstatus.DisplayMember = "Value";
            cbbEstatus.ValueMember = "Key";
            cbbEstatus.SelectedIndex = -1;

            dgwEstaciones.DataSource = null;
            CargarDetalle();

            timer1.Start();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CargarDetalle();
        }



        private void wfMonitorLineSetUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                CargarDetalle();
        }


        private void wfMonitorLineSetUp_Activated(object sender, EventArgs e)
        {
            dtpFechaIni.Focus();
        }
        #endregion

        #region regBotones

       
        private void btnExit2_Click(object sender, EventArgs e)
        {
            btExit_Click(sender, e);
        }
      

        private void btNew_Click(object sender, EventArgs e)
        {
            Inicio();
        }
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {

            try
            {
                System.Data.DataTable dt = ConfigLogica.Consultar();
                string sDirec = dt.Rows[0]["directorio"].ToString();
                if (!string.IsNullOrEmpty(sDirec))
                {
                    sDirec += @"\CloverPRO_ManualMonitorSetUp.pdf";
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
        private void btnFolio_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            try
            {

                int iRow = dgwEstaciones.CurrentCell.RowIndex;
                if (dgwEstaciones.CurrentRow.Index == -1)
                    return;

                long lFolio = long.Parse(dgwEstaciones[0, iRow].Value.ToString());
                wfLineSetUp SetUp = new wfLineSetUp(lFolio.ToString());
                SetUp.ShowDialog();

                CargarDetalle();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnAreas_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            try
            {

                int iRow = dgwEstaciones.CurrentCell.RowIndex;
                if (dgwEstaciones.CurrentRow.Index == -1)
                    return;

                long lFolio = long.Parse(dgwEstaciones[0, iRow].Value.ToString());
                int iCons = int.Parse(dgwEstaciones[1, iRow].Value.ToString());

                wfActividadesPop2 ActPop = new wfActividadesPop2(lFolio.ToString() + "-" + iCons.ToString());
                ActPop.ShowDialog();

                CargarDetalle();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion

        #region regEstacion
        private void CargarDetalle()
        {
            if(dtpFechaFin.Value < dtpFechaIni.Value)
            {
                MessageBox.Show("El periodo seleccionado es incorrecto", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dtpFechaFin.Focus();
                return;
            }

            if(chbPlanta.Checked && cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se ha seleccionado la planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbbPlanta.Focus();
                return;
            }

            if (chbLinea.Checked && cbbLinea.SelectedIndex == -1)
            {
                MessageBox.Show("No se ha seleccionado la linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbbLinea.Focus();
                return;
            }

            if (chbEstatus.Checked && cbbEstatus.SelectedIndex == -1)
            {
                MessageBox.Show("No se ha seleccionado el estatus", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbbEstatus.Focus();
                return;
            }

            MonitorSetupLogica mon = new MonitorSetupLogica();
            mon.FechaIni = dtpFechaIni.Value;
            mon.FechaFin = dtpFechaFin.Value;
            if(chbPlanta.Checked)
            {
                mon.IndPlanta = "1";
                mon.Planta = cbbPlanta.SelectedValue.ToString();
            }
            else
            {
                mon.IndPlanta = "0";
                mon.Planta = "";
            }
            if(chbLinea.Checked)
            {
                mon.IndLinea = "1";
                mon.Linea = cbbLinea.SelectedValue.ToString();
            }
            else
            {
                mon.IndLinea = "0";
                mon.Linea = "";
            }
            if(chbTurno.Checked)
            {
                mon.IndTurno = "1";
                mon.Turno = cbbTurno.SelectedValue.ToString();
            }
            else
            {
                mon.IndTurno = "0";
                mon.Turno = "";
            }
            if(chbEstatus.Checked)
            {
                mon.IndEstatus = "1";
                mon.Estatus = cbbEstatus.SelectedValue.ToString();
            }
            else
            {
                mon.IndEstatus = "0";
                mon.Estatus = "";
            }

            DataTable dt = MonitorSetupLogica.ListarSP(mon);
            dgwEstaciones.DataSource = dt;

            int iCant = dgwEstaciones.Rows.Count;
            tssCant.Text = iCant.ToString();

            CargarColumnas();
            dgwEstaciones.Focus();
        }
        private void dgwEstaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            string sValue = e.Value.ToString();


            if (e.ColumnIndex >= 0 && e.ColumnIndex <= 17)
            {
                sValue = dgwEstaciones[18, e.RowIndex].Value.ToString();
                if(sValue == "1")
                    e.CellStyle.BackColor = Color.DarkRed;

                sValue = dgwEstaciones[19, e.RowIndex].Value.ToString();
                if (sValue == "C")
                    e.CellStyle.ForeColor = Color.Red;

            }

            if (e.ColumnIndex >= 9 && e.ColumnIndex <= 12)
            {
                if (e.ColumnIndex == 9)
                    sValue = dgwEstaciones[20, e.RowIndex].Value.ToString();

                if (e.ColumnIndex == 10)
                    sValue = dgwEstaciones[21, e.RowIndex].Value.ToString();

                if (e.ColumnIndex == 11)
                    sValue = dgwEstaciones[22, e.RowIndex].Value.ToString();

                if (e.ColumnIndex == 12)
                    sValue = dgwEstaciones[23, e.RowIndex].Value.ToString();

                if (sValue == "L")
                    e.CellStyle.BackColor = Color.LightGreen;

                if (sValue == "I")
                    e.CellStyle.BackColor = Color.IndianRed;

                if (sValue== "N")
                    e.CellStyle.BackColor = Color.Gainsboro;

                if (sValue == "E")
                    e.CellStyle.BackColor = Color.WhiteSmoke;
            }

            if (e.Value != null)
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
        private void CargarColumnas()
        {
            
            int iRows = dgwEstaciones.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("Estacion");
                dtNew.Columns.Add("FOLIO", typeof(int));//0
                dtNew.Columns.Add("consec", typeof(int));//1
                dtNew.Columns.Add("LINEA", typeof(string));//2
                dtNew.Columns.Add("RPO ACTUAL", typeof(string));//3
                dtNew.Columns.Add("MODELO ACTUAL", typeof(string));//4
                dtNew.Columns.Add("CANTIDAD TOTAL", typeof(int));//5
                dtNew.Columns.Add("RPO SIGUIENTE", typeof(string));//6
                dtNew.Columns.Add("MODELO SIGUIENTE", typeof(string));//7
                dtNew.Columns.Add("HORARIO DE CAMBIO", typeof(string));//8
                dtNew.Columns.Add("SIST", typeof(string));//9
                dtNew.Columns.Add("ALM", typeof(string));//10
                dtNew.Columns.Add("MTTO", typeof(string));//11
                dtNew.Columns.Add("IMP", typeof(string));//12
                dtNew.Columns.Add("CAL", typeof(string));//13
                dtNew.Columns.Add("INICIO SET-UP", typeof(string));//14
                dtNew.Columns.Add("DURACION", typeof(string));//15
                dtNew.Columns.Add("ESTATUS", typeof(string));//16
                dtNew.Columns.Add("COMENTARIOS", typeof(string));//17
                dtNew.Columns.Add("urgente", typeof(string));//18
                dtNew.Columns.Add("estatus", typeof(string));//19
                dtNew.Columns.Add("sis", typeof(string));//20
                dtNew.Columns.Add("alm", typeof(string));//21
                dtNew.Columns.Add("mto", typeof(string));//22
                dtNew.Columns.Add("imp", typeof(string));//23
                dgwEstaciones.DataSource = dtNew;
            }
            else
            {
                #region regAreas
                foreach (DataGridViewRow row in dgwEstaciones.Rows)
                {
                    
                    long lFolio = long.Parse(row.Cells[0].Value.ToString());
                    int iConsec = int.Parse(row.Cells[1].Value.ToString());

                    string sAct = string.Empty;
                    for (int x = 1; x <= 4; x++)
                    {
                        string sCve = row.Cells[8+x].Value.ToString();
                        
                        if (x == 1)
                            sAct = "SIS";
                        if (x == 2)
                            sAct = "ALM";
                        if (x == 3)
                            sAct = "MTO";
                        if (x == 4)
                            sAct = "IMP";
                       
                        LineSetActLogica act = new LineSetActLogica();
                        act.Folio = lFolio;
                        act.Consec = iConsec;
                        act.Actividad = sAct;
                        DataTable dtA = LineSetActLogica.ConsultarArea(act);
                        if (dtA.Rows.Count > 0)
                        {
                            string sNota = string.Empty;

                            if(sAct == "SIS" && GlobalVar.gsArea == sAct)
                            {
                                sNota = dtA.Rows[0]["correo_dest"].ToString();
                                if(!string.IsNullOrEmpty(sNota) && sNota.IndexOf("@") != -1)
                                    sNota = sNota.Substring(0, sNota.IndexOf("@"));
                            }
                            else
                                sNota = dtA.Rows[0]["comentario"].ToString();

                            if (!string.IsNullOrEmpty(sNota) && !string.IsNullOrWhiteSpace(sNota))
                                row.Cells[8+x].Value = row.Cells[8+x].Value.ToString() + " " + sNota;
                        }
                    }
                }
                #endregion
            }


            dgwEstaciones.Columns[1].Visible = false;
            dgwEstaciones.Columns[5].Visible = false;
            dgwEstaciones.Columns[13].Visible = false;
            dgwEstaciones.Columns[18].Visible = false;
            dgwEstaciones.Columns[19].Visible = false;
            dgwEstaciones.Columns[20].Visible = false;
            dgwEstaciones.Columns[21].Visible = false;
            dgwEstaciones.Columns[22].Visible = false;
            dgwEstaciones.Columns[23].Visible = false;

            dgwEstaciones.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwEstaciones.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            DataGridViewCellStyle styleF = new DataGridViewCellStyle();
            styleF.Font = new Font("Verdana", 7F, FontStyle.Italic);
            styleF.ForeColor = Color.Blue;
            styleF.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[0].Width = ColumnWith(dgwEstaciones, 4);//FOLIO
            dgwEstaciones.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[0].ReadOnly = true;

            dgwEstaciones.Columns[2].Width = ColumnWith(dgwEstaciones, 4);//LINEA
            dgwEstaciones.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[2].ReadOnly = true;

            dgwEstaciones.Columns[3].Width = ColumnWith(dgwEstaciones, 8);//RPO
            dgwEstaciones.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[3].ReadOnly = false;

            dgwEstaciones.Columns[4].Width = ColumnWith(dgwEstaciones, 10);//MODELO
            dgwEstaciones.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[4].ReadOnly = false;

            //dgwEstaciones.Columns[5].Width = ColumnWith(dgwEstaciones, 6);//CANTIDAD
            //dgwEstaciones.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgwEstaciones.Columns[6].Width = ColumnWith(dgwEstaciones, 8);//RPO
            dgwEstaciones.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[6].ReadOnly = false;

            dgwEstaciones.Columns[7].Width = ColumnWith(dgwEstaciones, 10);//MODELO
            dgwEstaciones.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[7].ReadOnly = false;

            dgwEstaciones.Columns[8].Width = ColumnWith(dgwEstaciones, 7);//HORARIO INICIO
            dgwEstaciones.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[8].ReadOnly = true;

            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Font = new Font("Verdana", 7F, FontStyle.Bold);
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            

            dgwEstaciones.Columns[9].Width = ColumnWith(dgwEstaciones, 7);//SIST
            dgwEstaciones.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[9].DefaultCellStyle = style;
            dgwEstaciones.Columns[9].ReadOnly = true;

            dgwEstaciones.Columns[10].Width = ColumnWith(dgwEstaciones, 7);//ALM
            dgwEstaciones.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[10].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.0F, FontStyle.Bold);
            dgwEstaciones.Columns[10].ReadOnly = true;

            dgwEstaciones.Columns[11].Width = ColumnWith(dgwEstaciones, 7);//MTTO
            dgwEstaciones.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[11].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.0F, FontStyle.Bold);
            dgwEstaciones.Columns[11].ReadOnly = true;

            dgwEstaciones.Columns[12].Width = ColumnWith(dgwEstaciones, 7);//IMP
            dgwEstaciones.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[12].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.0F, FontStyle.Bold);
            dgwEstaciones.Columns[12].ReadOnly = true;

            //dgwEstaciones.Columns[13].Width = ColumnWith(dgwEstaciones, 7);//CAL
            //dgwEstaciones.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[13].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.0F, FontStyle.Bold);
            //dgwEstaciones.Columns[13].ReadOnly = true;

            dgwEstaciones.Columns[14].Width = ColumnWith(dgwEstaciones, 5);//INICIO SET-UP
            dgwEstaciones.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[14].ReadOnly = true;

            dgwEstaciones.Columns[15].Width = ColumnWith(dgwEstaciones, 5);//DURACION
            dgwEstaciones.Columns[15].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[15].ReadOnly = true;

            dgwEstaciones.Columns[16].Width = ColumnWith(dgwEstaciones, 8);//ESTATUS
            dgwEstaciones.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[16].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[16].ReadOnly = true;

            dgwEstaciones.Columns[17].Width = ColumnWith(dgwEstaciones, 11);//COMENTARIOS
            dgwEstaciones.Columns[17].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[17].ReadOnly = true;
        }
        
        //ESCANER SOBRE #OPERADOR && LINEA OP
        private void dgwEstaciones_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dgwEstaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex == 0)
            {
                long lFolio = long.Parse(dgwEstaciones[0, e.RowIndex].Value.ToString());
                wfLineSetUp SetUp = new wfLineSetUp(lFolio.ToString());
                SetUp.ShowDialog();
            }

            if (e.ColumnIndex == 8) //HORARIO INICIAL
            {

                string sEst = dgwEstaciones[16, e.RowIndex].Value.ToString();
                if (sEst == "CANCELADO")
                {
                    MessageBox.Show("No se permite modificar registros Cancelados", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "55") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                try
                {
                    long lFolio = long.Parse(dgwEstaciones[0, e.RowIndex].Value.ToString());
                    int iCons = int.Parse(dgwEstaciones[1, e.RowIndex].Value.ToString());

                    wfActividadesPop ActPop = new wfActividadesPop(lFolio.ToString() + "-" + iCons.ToString());
                    ActPop.ShowDialog();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            if (e.ColumnIndex >= 9 && e.ColumnIndex <= 13) //actividades
            {
                long lFolio = long.Parse(dgwEstaciones[0, e.RowIndex].Value.ToString());
                int iCons = int.Parse(dgwEstaciones[1, e.RowIndex].Value.ToString());
                string sArea = string.Empty;

                string sEst = dgwEstaciones[16, e.RowIndex].Value.ToString();
                if (sEst == "CANCELADO")
                {
                    MessageBox.Show("No se permite modificar registros Cancelados", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                if (e.ColumnIndex == 9)
                    sArea = "SIS";
                if (e.ColumnIndex == 10)
                    sArea = "ALM";
                if (e.ColumnIndex == 11)
                    sArea = "MTO";
                if (e.ColumnIndex == 12)
                    sArea = "IMP";
                if (e.ColumnIndex == 13)
                    sArea = "CAL";

                UsuarioLogica user = new UsuarioLogica();
                user.Usuario = GlobalVar.gsUsuario;
                DataTable dtU = UsuarioLogica.Consultar(user);
                _lsArea = dtU.Rows[0]["area"].ToString();

                if (_lsArea == "CTDOC")
                    _lsArea = "CAL";

                if (sArea != _lsArea && GlobalVar.gsUsuario != "ADMINP")
                {
                    MessageBox.Show("La Actividad " + sArea + " no pertenece a su Departamento", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                wfActividEstatusPop ActPop = new wfActividEstatusPop();
                ActPop._lsProceso = _lsProceso;
                ActPop._lFolio = lFolio;
                ActPop._iConsec = iCons;
                ActPop._sArea = sArea;
                ActPop.ShowDialog();

                CargarDetalle();
                dgwEstaciones.Rows[e.RowIndex].Selected = true;
            }

            if (e.ColumnIndex == 15 && _lsIndDuracion == "1") //DURACION
            {

                string sEst = dgwEstaciones[16, e.RowIndex].Value.ToString();
                if (sEst == "CANCELADO")
                {
                    MessageBox.Show("No se permite modificar registros Cancelados", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "70") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                try
                {
                    long lFolio = long.Parse(dgwEstaciones[0, e.RowIndex].Value.ToString());
                    int iCons = int.Parse(dgwEstaciones[1, e.RowIndex].Value.ToString());

                    string sDura = dgwEstaciones[15, e.RowIndex].Value.ToString();
                    
                    wfCapturaPop_1t CapPop = new wfCapturaPop_1t("");
                    CapPop._lsProceso = _lsProceso;
                    CapPop._llFolio = lFolio;
                    CapPop._liConsec = iCons;
                    if (!string.IsNullOrEmpty(sDura))
                        CapPop._sClave = sDura;
                    CapPop.ShowDialog();

                    CargarDetalle();
                    dgwEstaciones.Rows[e.RowIndex].Selected = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            if (e.ColumnIndex == 16) //ESTATUS
            {
                string sEst = dgwEstaciones[19, e.RowIndex].Value.ToString();
                if (sEst == "C")
                {
                    MessageBox.Show("No se permite modificar registros Cancelados", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "80") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                long lFolio = long.Parse(dgwEstaciones[0, e.RowIndex].Value.ToString());
                int iCons = int.Parse(dgwEstaciones[1, e.RowIndex].Value.ToString());

                wfCapturaPop_1t CapPop = new wfCapturaPop_1t("");
                CapPop._lsProceso = _lsProceso;
                CapPop._llFolio = lFolio;
                CapPop._liConsec = iCons;
                CapPop._sClave = sEst;
                CapPop._lsTipo = "E";
                CapPop.ShowDialog();

                CargarDetalle();
                dgwEstaciones.Rows[e.RowIndex].Selected = true;
            }

            if (e.ColumnIndex == 17) //COMENTARIOS
            {

                string sEst = dgwEstaciones[16, e.RowIndex].Value.ToString();
                if (sEst == "CANCELADO")
                {
                    MessageBox.Show("No se permite modificar registros Cancelados", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "60") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                try
                {
                    long lFolio = long.Parse(dgwEstaciones[0, e.RowIndex].Value.ToString());
                    int iCons = int.Parse(dgwEstaciones[1, e.RowIndex].Value.ToString());

                    string sComent = dgwEstaciones[17, e.RowIndex].Value.ToString();
                    string sParam = dgwEstaciones[0, e.RowIndex].Value.ToString() + "-" + dgwEstaciones[1, e.RowIndex].Value.ToString();

                    if (!string.IsNullOrEmpty(sComent))
                        sParam += "*" + sComent;

                    wfComentarioPop Coment = new wfComentarioPop(sParam);
                    Coment._lsProceso = _lsProceso;
                    Coment.ShowDialog();

                    CargarDetalle();
                    dgwEstaciones.Rows[e.RowIndex].Selected = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            if (e.ColumnIndex == 0 || e.ColumnIndex == 8 || e.ColumnIndex == 15 || e.ColumnIndex == 16 || e.ColumnIndex == 17 || (e.ColumnIndex >= 9 && e.ColumnIndex <= 13))
            {
                CargarDetalle();
                dgwEstaciones.CurrentCell = dgwEstaciones[e.ColumnIndex, e.RowIndex];
            }


        }
        private void dgwEstaciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgwEstaciones_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
                Close();

            if (e.KeyCode == Keys.F3)
            {
                CargarDetalle();
            }

            if (e.KeyCode == Keys.F4)
            {
                btnAreas_Click(sender, e);
            }

            if (e.KeyCode == Keys.F5)
            {
                btnFolio_Click(sender, e);
            }
        }

        private void dgwEstaciones_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            //e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        #endregion

        #region regColor

        private void cbbLinea_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbLinea, 0);
        }

        private void cbbLinea_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbLinea, 1);
        }




        #endregion

        #region regResize
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
        private void wfMonitorLineSetUp_Resize(object sender, EventArgs e)
        {
            if (WindowState != _WindowStateAnt && WindowState != FormWindowState.Minimized)
            {
                _WindowStateAnt = WindowState;
                ResizeControl(panel1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(panel2, 2, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(groupBox1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(dgwEstaciones, 3, ref _iWidthAnt, ref _iHeightAnt, 1);

                int iy = btnLoad.Location.Y;
                int iw = panel2.Width;
                int id = 118;
                int ix = iw - id;
                btnLoad.Location = new Point(ix, iy);
            }
        }
        #endregion

        #region regCaptura

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cbbLinea.ResetText();
            LineaLogica lin = new LineaLogica();
            lin.Planta = cbbPlanta.SelectedValue.ToString();
            System.Data.DataTable data = LineaLogica.LineaPlanta(lin);
            cbbLinea.DataSource = data;
            cbbLinea.DisplayMember = "nombre";
            cbbLinea.ValueMember = "linea";
            cbbLinea.SelectedIndex = -1;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            CargarDetalle();
        }

        private void chbPlanta_CheckedChanged(object sender, EventArgs e)
        {
            cbbPlanta.Enabled = chbPlanta.Checked;
        }

        private void chbLinea_CheckedChanged(object sender, EventArgs e)
        {
            cbbLinea.Enabled = chbLinea.Checked;
        }

        private void chbTurno_CheckedChanged(object sender, EventArgs e)
        {
            cbbTurno.Enabled = chbTurno.Checked;

        }

        private void chbEstatus_CheckedChanged(object sender, EventArgs e)
        {
            cbbEstatus.Enabled = chbEstatus.Checked;
        }

        private void dtpFechaIni_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode == Keys.F3)
                CargarDetalle();
        }

        private void dtpFechaFin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                CargarDetalle();
        }

        #endregion

    }
}
