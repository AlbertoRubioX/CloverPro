using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Logica;
using Datos;
using System.Diagnostics;

namespace CloverPro
{
    public partial class wfVisorLineSetUp : Form
    {
        public bool _lbCambio;
        private bool _lbCambioDet;
        public string _lsEstacion;
        public string _lsPlanta;
        private string _lsProceso = "PRO090";
        private string _lsFolioAnt;
        private string _lsTurno;
        private string _lsTipoMonitor;
        private string _lsLineaAnt;
        private string _lsVisorExtd;

        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;
        
        public wfVisorLineSetUp()
        {
            InitializeComponent();
            
            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }

        #region regInicio

        static bool FileInUse(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);

                    
                }
                return true;
            }
            catch (IOException ex)
            {
                return true;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {//MOSTRAR 2 PLANTAS POR MONITOR [ AREA DE SOPORTE ]

            if (_lsPlanta == "MON")
                _lsPlanta = "COL";
            else
                _lsPlanta = "MON";

            cbbPlanta.SelectedValue = _lsPlanta;
            CargarImagen();
            CargarDetalle();

        }
        private void VerificaArea()
        {
            EstacionLogica est = new EstacionLogica();
            est.Estacion = GlobalVar.gsEstacion;
            string sMonitor;
            if (EstacionLogica.VerificarMon(est))
            {
                DataTable data = EstacionLogica.Consultar(est);
                sMonitor = data.Rows[0]["ind_monitor"].ToString();
                _lsTipoMonitor = data.Rows[0]["area"].ToString();
                
                if (sMonitor == "1" && _lsTipoMonitor == "G")
                {
                    GlobalVar.gsArea = "SIS";
                    OpenExcel();
                    DobleMonitor();
                    
                }
                    
            }
        }
        private void DobleMonitor()
        {
            timer2.Start();
            timer3.Start();
        }
        private void CargarImagen()
        {
            if (_lsPlanta == "COL")
            {
                BackgroundImage = Properties.Resources.CAMBIO_DE_MODELO_color;
                if(_lsVisorExtd == "1")
                {
                    panel1.Height = panel1.Height + 118;
                    dgwEstaciones.Height = dgwEstaciones.Height + 118;
                }
            }
            else
                BackgroundImage = Properties.Resources.CAMBIO_DE_MODELO;
            
        }
        private void wfVisorLineSetUp_Load(object sender, EventArgs e)
        {
            _lsTurno = GlobalVar.gsTurno;
            _lsPlanta = GlobalVar.gsPlanta;

            DataTable dt = ConfigLogica.Consultar();
            _lsVisorExtd = dt.Rows[0]["mon_setupxtd"].ToString();//Datagrid pantalla completa (oculta nombre planta)

            VerificaArea();

            if(!string.IsNullOrEmpty(_lsPlanta))
                CargarImagen();
                
            Inicio();

            WindowState = FormWindowState.Maximized;

        }

        private void Inicio()
        {
           

            dtpFechaIni.ResetText();
            dtpFechaFin.ResetText();

            chbPlanta.Checked = false;
            cbbPlanta.Enabled = false;
            cbbPlanta.ResetText();
            DataTable dtPl = PlantaLogica.Listar();
            cbbPlanta.DataSource = dtPl;
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.SelectedIndex = -1;

            chbLinea.Checked = false;
            cbbLinea.Enabled = false;
            cbbLinea.ResetText();

            cbbTurno.Enabled = true;
            Dictionary<string, string> Turno = new Dictionary<string, string>();
            Turno.Add("1", "1");
            Turno.Add("2", "2");
            cbbTurno.DataSource = new BindingSource(Turno, null);
            cbbTurno.DisplayMember = "Value";
            cbbTurno.ValueMember = "Key";
            cbbTurno.SelectedIndex = -1;
            chbTurno.Checked = false;

            chbEstatus.Checked = false;
            cbbEstatus.Enabled = false;
            Dictionary<string, string> Est = new Dictionary<string, string>();
            Est.Add("E", "EN ESPERA");
            Est.Add("T", "EN TIEMPO");
            Est.Add("C", "CANCELADO");
            Est.Add("F", "FUERA DE TIEMPO");
            cbbEstatus.DataSource = new BindingSource(Est, null);
            cbbEstatus.DisplayMember = "Value";
            cbbEstatus.ValueMember = "Key";
            cbbEstatus.SelectedIndex = -1;

            if (!string.IsNullOrEmpty(_lsEstacion) && !string.IsNullOrEmpty(_lsPlanta))
            {
                _lsPlanta = GlobalVar.gsPlanta;
                chbPlanta.Checked = true;
                cbbPlanta.SelectedValue = _lsPlanta;
                _lsTurno = "1";

            }

            dgwEstaciones.DataSource = null;
            CargarDetalle();

            timer1.Start();
            timer3.Start();

        }

        private void wfVisorLineSetUp_Activated(object sender, EventArgs e)
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
                    sDirec += @"\CloverPRO_ManualSetUp.pdf";
                    //System.Diagnostics.Process.Start(sDirec);
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

        #region regEstacion
        private void CargarDetalle()
        {
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

            try
            {

                MonitorSetupLogica mon = new MonitorSetupLogica();
                mon.FechaIni = DateTime.Now;
                if (mon.FechaIni.Hour >= 6 && mon.FechaIni.Hour < 16)
                    _lsTurno = "1";
                else
                {
                    _lsTurno = "2";
                    if (mon.FechaIni.Hour >= 0 && mon.FechaIni.Hour < 6)
                        mon.FechaIni = DateTime.Today.AddDays(-1);
                }

                mon.FechaFin = DateTime.Today;
                

                if (chbPlanta.Checked)
                {
                    mon.IndPlanta = "1";
                    mon.Planta = cbbPlanta.SelectedValue.ToString();
                }
                else
                {
                    mon.IndPlanta = "0";
                    mon.Planta = "";
                }
                if (chbLinea.Checked)
                {
                    mon.IndLinea = "1";
                    mon.Linea = cbbLinea.SelectedValue.ToString();
                }
                else
                {
                    mon.IndLinea = "0";
                    mon.Linea = "";
                }
                /*
                if (chbTurno.Checked)
                {
                    mon.IndTurno = "1";
                    mon.Turno = cbbTurno.SelectedValue.ToString();
                }
                else
                {
                    mon.IndTurno = "0";
                    mon.Turno = "";
                }
                */

                mon.Turno = _lsTurno;
                if (chbEstatus.Checked)
                {
                    mon.IndEstatus = "1";
                    mon.Estatus = cbbEstatus.SelectedValue.ToString();
                }
                else
                {
                    mon.IndEstatus = "0";
                    mon.Estatus = "";
                }

                DataTable dt = MonitorSetupLogica.VisorSP(mon);
                dgwEstaciones.DataSource = dt;

                CargarColumnas();
                dgwEstaciones.Focus();
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Favor de Nificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                throw ex; 
            }
           
        }
        private void dgwEstaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            //if ((iRow % 2) == 0)
            //    e.CellStyle.BackColor = Color.LightGreen;
            //else
            //    e.CellStyle.BackColor = Color.White;

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
                System.Data.DataTable dtNew = new System.Data.DataTable("Estacion");
                dtNew.Columns.Add("FOLIO", typeof(int));
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("LINEA", typeof(string));
                dtNew.Columns.Add("RPO ACTUAL", typeof(string));
                dtNew.Columns.Add("MODELO ACTUAL", typeof(string));
                dtNew.Columns.Add("CANTIDAD TOTAL", typeof(int));
                dtNew.Columns.Add("RPO SIGUIENTE", typeof(string));
                dtNew.Columns.Add("MODELO SIGUIENTE", typeof(string));
                dtNew.Columns.Add("HORARIO DE CAMBIO", typeof(string));
                dtNew.Columns.Add("SIST", typeof(string));
                dtNew.Columns.Add("ALM", typeof(string));
                dtNew.Columns.Add("MTTO", typeof(string));
                dtNew.Columns.Add("IMP", typeof(string));
                dtNew.Columns.Add("CAL", typeof(string));
                dtNew.Columns.Add("INICIO SET-UP", typeof(string));
                dtNew.Columns.Add("DURACION", typeof(string));
                dtNew.Columns.Add("ESTATUS", typeof(string));
                dtNew.Columns.Add("COMENTARIOS", typeof(string));
                dtNew.Columns.Add("urgente", typeof(string));
                dtNew.Columns.Add("estatus", typeof(string));//19
                dtNew.Columns.Add("sis", typeof(string));//20
                dtNew.Columns.Add("alm", typeof(string));//21
                dtNew.Columns.Add("mto", typeof(string));//22
                dtNew.Columns.Add("imp", typeof(string));//23
                dgwEstaciones.DataSource = dtNew;
            }
            else
            {
                foreach(DataGridViewRow row in dgwEstaciones.Rows)
                {
                    string sInd = Convert.ToString(row.Cells[18].Value);
                    if (sInd == "1")
                    {
                        row.DefaultCellStyle.BackColor = Color.DarkRed;
                        row.DefaultCellStyle.ForeColor = Color.Yellow;
                    }

                    string sEst = row.Cells[16].Value.ToString();
                    if (sEst == "CANCELADO")
                    {
                        //row.DefaultCellStyle.BackColor = Color.Red;
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    }
                        

                    long lFolio = long.Parse(row.Cells[0].Value.ToString());
                    int iConsec = int.Parse(row.Cells[1].Value.ToString());

                    string sAct = string.Empty;
                    for (int x = 1; x <= 4; x++)
                    {
                        string sCve = row.Cells[8+x].Value.ToString();
                        if (sCve =="LISTO")
                            row.Cells[8+x].Style.BackColor = Color.LightGreen;
                        if (sCve == "INSUFICIENTE")
                            row.Cells[8 + x].Style.BackColor = Color.IndianRed;
                        if (sCve == "N/A")
                            row.Cells[8 + x].Style.BackColor = Color.Gainsboro;

                        if (x == 1)
                            sAct = "SIS";
                        if (x == 2)
                            sAct = "ALM";
                        if (x == 3)
                            sAct = "MTO";
                        if (x == 4)
                            sAct = "IMP";
                        //if (x == 5)
                        //    sAct = "CAL";

                        LineSetActLogica act = new LineSetActLogica();
                        act.Folio = lFolio;
                        act.Consec = iConsec;
                        act.Actividad = sAct;
                        DataTable dtA = LineSetActLogica.ConsultarArea(act);
                        if (dtA.Rows.Count > 0)
                        {
                            string sNota = string.Empty;

                            if (sAct == "SIS" && GlobalVar.gsArea == sAct)
                            {
                                sNota = dtA.Rows[0]["correo_dest"].ToString();
                                if (!string.IsNullOrEmpty(sNota) && sNota.IndexOf("@") != -1)
                                    sNota = sNota.Substring(0, sNota.IndexOf("@"));
                            }
                            else
                                sNota = dtA.Rows[0]["comentario"].ToString();
                            
                            if (!string.IsNullOrEmpty(sNota))
                                row.Cells[8+x].Value = row.Cells[8+x].Value.ToString() + " " + sNota;
                        }
                    }
                }
            }

            dgwEstaciones.Columns[0].Visible = false;
            dgwEstaciones.Columns[1].Visible = false;
            dgwEstaciones.Columns[5].Visible = false;
            dgwEstaciones.Columns[13].Visible = false;
            dgwEstaciones.Columns[18].Visible = false;
            dgwEstaciones.Columns[19].Visible = false;
            dgwEstaciones.Columns[20].Visible = false;
            dgwEstaciones.Columns[21].Visible = false;
            dgwEstaciones.Columns[22].Visible = false;
            dgwEstaciones.Columns[23].Visible = false;

            dgwEstaciones.DefaultCellStyle.SelectionBackColor = dgwEstaciones.DefaultCellStyle.BackColor;
            dgwEstaciones.DefaultCellStyle.SelectionForeColor = dgwEstaciones.DefaultCellStyle.ForeColor;

            dgwEstaciones.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwEstaciones.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            dgwEstaciones.Columns[0].Width = ColumnWith(dgwEstaciones, 3);//FOLIO
            dgwEstaciones.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[2].Width = ColumnWith(dgwEstaciones, 3);//LINEA
            dgwEstaciones.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[3].Width = ColumnWith(dgwEstaciones, 8);//RPO
            dgwEstaciones.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[4].Width = ColumnWith(dgwEstaciones, 8);//MODELO
            dgwEstaciones.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[5].Width = ColumnWith(dgwEstaciones, 6);//CANTIDAD
            //dgwEstaciones.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgwEstaciones.Columns[6].Width = ColumnWith(dgwEstaciones, 8);//RPO
            dgwEstaciones.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[7].Width = ColumnWith(dgwEstaciones, 8);//MODELO
            dgwEstaciones.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[8].Width = ColumnWith(dgwEstaciones, 5);//HORARIO INICIO
            dgwEstaciones.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Font = new Font("Verdana", 7F, FontStyle.Bold);
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            

            dgwEstaciones.Columns[9].Width = ColumnWith(dgwEstaciones, 6);//SIST
            dgwEstaciones.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[9].DefaultCellStyle = style;

            dgwEstaciones.Columns[10].Width = ColumnWith(dgwEstaciones, 6);//ALM
            dgwEstaciones.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[10].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.0F, FontStyle.Bold);

            dgwEstaciones.Columns[11].Width = ColumnWith(dgwEstaciones, 6);//MTTO
            dgwEstaciones.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[11].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.0F, FontStyle.Bold);

            dgwEstaciones.Columns[12].Width = ColumnWith(dgwEstaciones, 6);//IMP
            dgwEstaciones.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[12].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.0F, FontStyle.Bold);

            //dgwEstaciones.Columns[13].Width = ColumnWith(dgwEstaciones, 6);//CAL
            //dgwEstaciones.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwEstaciones.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[14].Width = ColumnWith(dgwEstaciones, 6);//INICIO SET-UP
            dgwEstaciones.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[15].Width = ColumnWith(dgwEstaciones, 4);//DURACION
            dgwEstaciones.Columns[15].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[16].Width = ColumnWith(dgwEstaciones, 8);//ESTATUS
            dgwEstaciones.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[16].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[17].Width = ColumnWith(dgwEstaciones, 15);//COMENTARIOS
            dgwEstaciones.Columns[17].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
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

           
        }
        private void dgwEstaciones_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        private void dgwEstaciones_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //string sEst = dgwEstaciones.Rows[e.RowIndex].Cells[15].Value.ToString();
            //if (sEst == "CANCELADO")
            //    dgwEstaciones.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
            //if (sSis == "TERMINADO")
            //    dgwEstaciones.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
        }
        private void dgwEstaciones_SelectionChanged(object sender, EventArgs e)
        {
            dgwEstaciones.ClearSelection();
        }

        //ESCANER SOBRE #OPERADOR && LINEA OP
        private void dgwEstaciones_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           
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
        private void wfVisorLineSetUp_Resize(object sender, EventArgs e)
        {
            if (WindowState != _WindowStateAnt && WindowState != FormWindowState.Minimized)
            {
                _WindowStateAnt = WindowState;
                ResizeControl(panel1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(panel2, 2, ref _iWidthAnt, ref _iHeightAnt, 0);
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
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
         
            CargarDetalle();
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

        private void wfVisorLineSetUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                CargarDetalle();
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            wfLogin Login = new wfLogin();
            Login.ShowDialog();
            
            
            string sUsuario = Login._sUsuario;
            if (string.IsNullOrEmpty(sUsuario))
                return;

            
            DataTable data = AccesoDatos.TraerUsuario(sUsuario);
            if (data.Rows.Count != 0)
            {
                GlobalVar.gsUsuario = sUsuario;
                GlobalVar.gsNombreUs = data.Rows[0][1].ToString();
                GlobalVar.gsPlanta = data.Rows[0][2].ToString();
                GlobalVar.gsDepto = data.Rows[0][4].ToString();
                GlobalVar.gsArea = data.Rows[0][5].ToString();
                GlobalVar.gsTurno = data.Rows[0][6].ToString();
            }

            int iForms = Application.OpenForms.Count;
            if(iForms == 2)
            {
                wfMonitorLineSetUp MonSetup = new wfMonitorLineSetUp();
                MonSetup.Show();
            }
            else
                Application.OpenForms[2].BringToFront();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            OpenExcel();
        }
        private void OpenExcel()
        {
            
            bool wbexists = File.Exists(@"C:\Users\agonz0\Desktop\JLOPEZ_Pendientes Sistemas.xlsx");
            if (!wbexists)
                return;

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook wbv = excel.Workbooks.Open(@"C:\Users\agonz0\Desktop\JLOPEZ_Pendientes Sistemas.xlsx");
            Microsoft.Office.Interop.Excel.Worksheet wx = excel.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
            
            //wbv.Close(true, Type.Missing, Type.Missing);
            //excel.Quit();
        }
    }
}
