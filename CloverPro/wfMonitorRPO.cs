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
    public partial class wfMonitorRPO : Form
    {
        public bool _lbCambio;
        private bool _lbCambioDet;
        public string _lsParam;
        private string _lsProceso = "EMP040";
        private string _lsFolioAnt;
        private string _lsTurno;
        private string _lsArea;
        private string _lsIndDuracion;
        private bool _lbEti;

        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;
        
        public wfMonitorRPO()
        {
            InitializeComponent();
            
            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }
        
        #region regInicio
        private void wfMonitorRPO_Load(object sender, EventArgs e)
        {
            _lsTurno = GlobalVar.TurnoGlobal();
            string sVersion = "V. ";
            DataTable dtConf = ConfigLogica.Consultar();
            if (dtConf.Rows.Count != 0)
                tssVersion.Text = sVersion + dtConf.Rows[0]["version"].ToString();

            Inicio();

            WindowState = FormWindowState.Maximized;
            
        }

        private void Inicio()
        {

            _lbEti = false;
            dtpFecha.Enabled = false;
            dtpFecha.ResetText();

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "15") == true)
            {
                dtpFecha.Enabled = true;
            }

           

            chbPlanta.Checked = false;
            cbbPlanta.Enabled = false;
            cbbPlanta.ResetText();
            DataTable dtPl = PlantaLogica.Listar();
            cbbPlanta.DataSource = dtPl;
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.SelectedIndex = -1;
            if (!string.IsNullOrEmpty(GlobalVar.gsPlanta) )
            {
                chbPlanta.Checked = true;
                cbbPlanta.SelectedValue = GlobalVar.gsPlanta;

                cbbLinea.ResetText();
                LineaLogica lin = new LineaLogica();
                lin.Planta = cbbPlanta.SelectedValue.ToString();
                DataTable data = LineaLogica.LineaPlanta(lin);

                if (lin.Planta == "EMPN")
                {
                    data = LineaLogica.LineasNav(lin);
                    cbbLinea.DataSource = data;
                    cbbLinea.DisplayMember = "linea_nav";
                    cbbLinea.ValueMember = "linea_nav";
                }
                else
                {
                    cbbLinea.DisplayMember = "nombre";
                    cbbLinea.ValueMember = "linea";
                }
                
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
            
            // ESTATUS DE RPO
            chbEstatus.Checked = false;
            cbbEstatus.Enabled = false;
            Dictionary<string, string> Est = new Dictionary<string, string>();
            Est.Add("E", "ESPERA");
            Est.Add("P", "PROCESO");
            Est.Add("C", "COMPLETADO");
            Est.Add("T", "ENTREGADO");
            cbbEstatus.DataSource = new BindingSource(Est, null);
            cbbEstatus.DisplayMember = "Value";
            cbbEstatus.ValueMember = "Key";
            cbbEstatus.SelectedIndex = -1;
            
            chbPrioridad.Checked = false;
            cbbPrioridad.ResetText(); //FILTRO MAESTRO
            Dictionary<string, string> Data = new Dictionary<string, string>();
            Data.Add("P", "Prioridad");
            Data.Add("E", "Etiqueta");
            Data.Add("M", "Almacén");
            Data.Add("L", "Tipo Linea");
            Data.Add("A", "Almacenista");
            Data.Add("W", "W.P.");
            Data.Add("T", "Turno Procesado");
            cbbPrioridad.DataSource = new BindingSource(Data, null);
            cbbPrioridad.DisplayMember = "Value";
            cbbPrioridad.ValueMember = "Key";
            cbbPrioridad.SelectedIndex = -1;

            EstacionLogica est = new EstacionLogica();
            est.Estacion = GlobalVar.gsEstacion;
            if (EstacionLogica.Verificar(est))
            {
                _lbEti = true;
                chbPrioridad.Checked = true;
                cbbPrioridad.SelectedValue = "L";

                Dictionary<string, string> Data2 = new Dictionary<string, string>();
                Data2.Add("L", "Largos");
                Data2.Add("P", "Pony");
                cbbTipo.DataSource = new BindingSource(Data2, null);
                cbbTipo.DisplayMember = "Value";
                cbbTipo.ValueMember = "Key";
                cbbTipo.SelectedIndex = -1;
                cbbTipo.SelectedValue = GlobalVar.gsArea;
                //GlobalVar.gsArea = "E";//etiquetas para el filtro 
                chbEstatus.Checked = true;
                cbbEstatus.SelectedValue = "P";
            }

            dgwEstaciones.DataSource = null;
            CargarColumnas();
            AjustaColumnas();
            /* CARGADO DESDE IMPORTAR RPO POR PLANEACION
            ControlRpoLogica rpo = new ControlRpoLogica();
            rpo.Fecha = dtpFecha.Value;
            rpo.Planta = cbbPlanta.SelectedValue.ToString();
          
            //CARGAR LOS RPO SUBIDOS POR PLANEACION
            DataTable dt = ControlRpoLogica.CargarRPO(rpo);
            */


            timer1.Start();
            /*
            if (_lbEti)
            {
                CargarDetalle("1");
                timer2.Start();
            }
            else
            {
                CargarDetalle("0");
                timer1.Start();
            }
            */
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CargarDetalle("0");
        }



        private void wfMonitorRPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                CargarDetalle("0");
        }


        private void wfMonitorRPO_Activated(object sender, EventArgs e)
        {
            cbbPlanta.Focus();
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
                DataTable dt = ConfigLogica.Consultar();
                string sDirec = dt.Rows[0]["directorio"].ToString();
                if (!string.IsNullOrEmpty(sDirec))
                {
                    sDirec += @"\CloverPRO_ManualMonitorSetUp.pdf";
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
        private void CargarDetalle(string _asSort)
        {
            bool bSearchBack = false; //OPCION SOLO PARA BUSCAR RPO RESAGADO

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
            if (chbTurno.Checked && cbbTurno.SelectedIndex == -1)
            {
                MessageBox.Show("No se ha seleccionado el Turno", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbbTurno.Focus();
                return;
            }
            if (chbRPO.Checked)
            {
                if (string.IsNullOrEmpty(txtRPO.Text))
                {
                    MessageBox.Show("No se ha capturado el RPO", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtRPO.Focus();
                    return;
                }
                else
                    bSearchBack = true;
            }

            if (chbEstatus.Checked && cbbEstatus.SelectedIndex == -1)
            {
                MessageBox.Show("No se ha seleccionado el estatus", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbbEstatus.Focus();
                return;
            }

            if (chbPrioridad.Checked)
            {
                if (cbbPrioridad.SelectedIndex == -1 )
                {
                    if(cbbTipo.SelectedIndex == -1 && string.IsNullOrEmpty(cbbTipo.Text))
                    {
                        MessageBox.Show("No se ha seleccionado filtro", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        cbbPrioridad.Focus();
                        return;
                    }
                    
                }
                else
                {
                    if (cbbTipo.SelectedIndex == -1 && string.IsNullOrEmpty(cbbTipo.Text))
                    {
                        MessageBox.Show("No se ha seleccionado filtro", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        cbbTipo.Focus();
                        return;
                    }
                }
                
            }

            Cursor = Cursors.WaitCursor;
            try
            {
                ControlRpoLogica crpo = new ControlRpoLogica();
                crpo.Fecha = dtpFecha.Value;
                crpo.Planta = cbbPlanta.SelectedValue.ToString();
                crpo.Usuario = GlobalVar.gsUsuario;

                long lFolio = 0;
                string sFolio = string.Empty;
                DataTable data = ControlRpoLogica.ConsultarFolio(crpo);
                if (data.Rows.Count > 0)
                {
                    sFolio = data.Rows[0][0].ToString();
                }

                //CONSULTA FILTRADO POR DIA
                string sFecha1 = DateTime.Today.ToShortDateString();
                string sFecha2 = dtpFecha.Value.ToShortDateString();
                if(sFecha1 != sFecha2)
                {
                    data = ControlRpoLogica.ConsultarFolioAnt(crpo);
                    if (data.Rows.Count > 0)
                    {
                        sFolio = data.Rows[0][0].ToString();
                    }
                }

                ControlRpoLogica rpo = new ControlRpoLogica();
                if (!long.TryParse(sFolio, out lFolio))
                {
                    Cursor = Cursors.Default;
                    dgwEstaciones.DataSource = null;
                    CargarColumnas();
                    return;
                }

                rpo.Folio = lFolio;
                rpo.Fecha = dtpFecha.Value;

                if (chbPlanta.Checked)
                {
                    rpo.IndPlanta = "1";
                    rpo.Planta = cbbPlanta.SelectedValue.ToString();
                }
                else
                {
                    rpo.IndPlanta = "0";
                    rpo.Planta = "";
                }
                if (chbLinea.Checked)
                {
                    rpo.IndLinea = "1";
                    rpo.Linea = cbbLinea.SelectedValue.ToString();
                }
                else
                {
                    rpo.IndLinea = "0";
                    rpo.Linea = "";
                }
                if (chbTurno.Checked)
                {
                    rpo.IndTurno = "1";
                    rpo.Turno = cbbTurno.SelectedValue.ToString();
                }
                else
                {
                    rpo.IndTurno = "0";
                    rpo.Turno = "";
                }
                if (chbEstatus.Checked)
                {
                    rpo.IndEstatus = "1";
                    rpo.Estatus = cbbEstatus.SelectedValue.ToString();
                }
                else
                {
                    rpo.IndEstatus = "0";
                    rpo.Estatus = "";
                }

                if (chbRPO.Checked)
                {
                    rpo.IndRPO = "1";
                    rpo.RPO = txtRPO.Text.ToString();
                }
                else
                {
                    rpo.IndRPO = "0";
                    rpo.RPO = "";
                }

                if (chbPrioridad.Checked)
                {
                    rpo.IndFiltro = "1";
                    string sFiltro = cbbPrioridad.SelectedValue.ToString();
                    if (sFiltro == "P")
                    {
                        rpo.ValorFiltro = cbbTipo.SelectedValue.ToString();
                    }
                    if (sFiltro == "L")
                    {
                        rpo.ValorFiltro = cbbTipo.SelectedValue.ToString();
                    }
                    if (sFiltro == "A")
                    {
                        if (cbbTipo.SelectedIndex == -1)
                            rpo.ValorFiltro = cbbTipo.Text.ToString();
                        else
                            rpo.ValorFiltro = cbbTipo.SelectedValue.ToString();
                    }
                    if (sFiltro == "W")
                    {
                        rpo.ValorFiltro = cbbTipo.Text.ToString();
                    }
                    if (sFiltro == "E") // estatus de etiquetas
                    {
                        rpo.ValorFiltro = cbbTipo.SelectedValue.ToString();
                    }
                    if (sFiltro == "M") // estatus almacen
                    {
                        rpo.ValorFiltro = cbbTipo.SelectedValue.ToString();
                    }
                    if (sFiltro == "T") // turno procesado
                    {
                        rpo.ValorFiltro = cbbTipo.SelectedValue.ToString();
                    }
                    rpo.Filtro = sFiltro;

                }
                else
                {
                    rpo.IndFiltro = "0";
                    rpo.Filtro = "";
                    rpo.ValorFiltro = "";
                }

                //SORT DE ETIQUETAS
                rpo.Area = "X";
                /*
                if (_asSort == "1")
                    rpo.Area = GlobalVar.gsArea;
                */


                int iRow = 0;
                int iCol = 4;
                int iRowsB = dgwEstaciones.Rows.Count;
                if (iRowsB > 0 && dgwEstaciones.SelectedCells.Count > 0 )
                {
                    if (dgwEstaciones.CurrentCell.RowIndex != -1)
                    {
                        iRow = dgwEstaciones.CurrentCell.RowIndex;
                        iCol = dgwEstaciones.CurrentCell.ColumnIndex;
                    }
                }

                DataTable dt = ControlRpoLogica.ListarSP(rpo);
               
                dgwEstaciones.DataSource = dt;

                int iRowsA = dgwEstaciones.Rows.Count;

                if(iRowsA == 0 && bSearchBack)
                {
                    dt = ControlRpoLogica.ConsultarUltRPO(rpo);
                    if(dt.Rows.Count > 0)
                    {
                        lFolio = 0;
                        if(long.TryParse(dt.Rows[0][0].ToString(),out lFolio))
                        {
                            rpo.Folio = lFolio;
                            dt = ControlRpoLogica.ListarSP(rpo);
                            dgwEstaciones.DataSource = dt;
                            iRowsA = dgwEstaciones.Rows.Count;
                        }    
                    }
                }

                if (iRowsA == iRowsB && iRowsB > 0 && iRow != -1)
                {
                    dgwEstaciones.Rows[iRow].Selected = true;
                    dgwEstaciones.CurrentCell = dgwEstaciones[iCol, iRow];
                }

                double dCant=0;
                foreach(DataGridViewRow row in dgwEstaciones.Rows)
                {
                    double dCt = 0;
                    if(double.TryParse(row.Cells[7].Value.ToString(),out dCt))
                    {
                        if(dCt > 0)
                            dCant += dCt;
                    }
                }

                CargarColumnas();
                if(cbbPrioridad.SelectedIndex != -1 && cbbTipo.SelectedIndex != -1)
                {
                    if (cbbPrioridad.SelectedValue.ToString() == "L" && cbbTipo.SelectedValue.ToString() == "P")
                    {
                        ListSortDirection direction;
                        direction = ListSortDirection.Ascending;
                        dgwEstaciones.Sort(dgwEstaciones.Columns[22], direction);
                    }
                }

                Cursor = Cursors.Default;

                tssTotal.Text = iRowsA.ToString("N0");
                tssTotalUnid.Text = dCant.ToString("N0");

                dgwEstaciones.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cursor = Cursors.WaitCursor;
                return;
            }
        }
        private void dgwEstaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            string sValue = e.Value.ToString();
            
            if(e.ColumnIndex >= 4 && e.ColumnIndex <= 15)
            {
                sValue = dgwEstaciones[29, e.RowIndex].Value.ToString();
                string sValue2 = dgwEstaciones[19, e.RowIndex].Value.ToString();
                string sValue3 = dgwEstaciones[18, e.RowIndex].Value.ToString();
                if (!string.IsNullOrEmpty(sValue) && (sValue == "10" || sValue == "5") && (sValue2 == "E" || sValue2 == "P" || (string.IsNullOrEmpty(sValue3) || sValue3 == "P" || sValue3 == "D" )))
                {
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.ForeColor = Color.White;
                    /*
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.ForeColor = Color.White;
                    style.BackColor = Color.Red;
                    style.Font = new Font(dgwEstaciones.Font.FontFamily, 10, FontStyle.Bold);
                    e.CellStyle = style;
                    */
                }
            }

            sValue = e.Value.ToString();
            if (e.ColumnIndex == 10)
            {
                sValue = dgwEstaciones[17, e.RowIndex].Value.ToString();
                e.CellStyle.ForeColor = Color.Black;
                switch (sValue)
                {
                    case "L":
                        e.CellStyle.BackColor = Color.LightGreen;
                        break;
                    case "P":
                        e.CellStyle.BackColor = Color.DodgerBlue;
                        break;
                    case "D":
                        e.CellStyle.BackColor = Color.DarkRed;
                        e.CellStyle.ForeColor = Color.White;
                        break;
                    case "E":
                        e.CellStyle.BackColor = Color.LightGray;
                        e.CellStyle.ForeColor = Color.Blue;
                        break;
                    default:
                        e.CellStyle.BackColor = Color.WhiteSmoke;
                        break;
                }
                
            }
            if (e.ColumnIndex == 11)
            {
                if (!string.IsNullOrEmpty(sValue))
                {
                    e.CellStyle.ForeColor = Color.Black;
                    if (sValue.IndexOf("-") == -1)
                        e.CellStyle.BackColor = Color.WhiteSmoke;
                    else
                        e.CellStyle.BackColor = Color.Yellow;
                }
            }

            if(e.ColumnIndex == 12)
            {
                sValue = dgwEstaciones[18, e.RowIndex].Value.ToString();
                e.CellStyle.ForeColor = Color.Black;
                switch (sValue)
                {
                    case "L":
                        e.CellStyle.BackColor = Color.LightGreen;
                        break;
                    case "P":
                        e.CellStyle.BackColor = Color.DodgerBlue;
                        break;
                    case "D":
                        e.CellStyle.BackColor = Color.DarkRed;
                        e.CellStyle.ForeColor = Color.White;
                        break;
                    case "E":
                        e.CellStyle.BackColor = Color.LightGray;
                        e.CellStyle.ForeColor = Color.Blue;
                        break;
                    default:
                        e.CellStyle.BackColor = Color.WhiteSmoke;
                        break;
                }
            }

            if (e.ColumnIndex == 13)
            {
                sValue = dgwEstaciones[29, e.RowIndex].Value.ToString();
                if (!string.IsNullOrEmpty(sValue) && (sValue == "10" || sValue == "5"))
                {
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.ForeColor = Color.White;
                    style.BackColor = Color.Red;
                    style.Font = new Font(dgwEstaciones.Font.FontFamily, 10, FontStyle.Bold);
                    e.CellStyle = style;
                }
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
                dtNew.Columns.Add("folio", typeof(int));//0
                dtNew.Columns.Add("consec", typeof(int));//1
                dtNew.Columns.Add("fecha", typeof(DateTime));//2
                dtNew.Columns.Add("planta", typeof(string));//3
                dtNew.Columns.Add("TURNO", typeof(string));//4
                dtNew.Columns.Add("RPO", typeof(string));//5
                dtNew.Columns.Add("LINEA", typeof(string));//6
                dtNew.Columns.Add("CANT", typeof(int));//7
                dtNew.Columns.Add("MODELO", typeof(string));//8
                dtNew.Columns.Add("ALMACENISTA", typeof(string));//9
                dtNew.Columns.Add("ETIQUETAS", typeof(string));//10
                dtNew.Columns.Add("UBICACION", typeof(string));//11
                dtNew.Columns.Add("ALMACEN", typeof(string));//12
                dtNew.Columns.Add("PRIORIDAD", typeof(string));//13
                dtNew.Columns.Add("WP", typeof(string));//14
                dtNew.Columns.Add("ESTATUS", typeof(string));//15
                dtNew.Columns.Add("surte", typeof(string));//16
                dtNew.Columns.Add("etiqueta", typeof(string));//17
                dtNew.Columns.Add("almacen", typeof(string));//18
                dtNew.Columns.Add("estatus", typeof(string));//19
                dtNew.Columns.Add("eti_nota", typeof(string));//20
                dtNew.Columns.Add("alm_nota", typeof(string));//21
                dtNew.Columns.Add("f_pro", typeof(DateTime));//22
                dtNew.Columns.Add("turno_proc", typeof(string));//23
                dtNew.Columns.Add("entrega", typeof(string));//24
                dtNew.Columns.Add("nombre_oper", typeof(string));//25
                dtNew.Columns.Add("f_proceti", typeof(DateTime));//26
                dtNew.Columns.Add("turno_procet", typeof(string));//27
                dtNew.Columns.Add("prioridad", typeof(DateTime));//28
                dtNew.Columns.Add("ind_prio", typeof(string));//29
                dtNew.Columns.Add("tipo", typeof(string));//30 - tipo de linea [L/P]
                dtNew.Columns.Add("f_carga", typeof(DateTime));//31
                dtNew.Columns.Add("f_dete", typeof(DateTime));//32
                dtNew.Columns.Add("dete_nota", typeof(string));//33
                dtNew.Columns.Add("dete_cant", typeof(int));//34
                dtNew.Columns.Add("u_dete", typeof(string));//35
                dgwEstaciones.DataSource = dtNew;
            }
            else
            {
                #region regAreas
                
                foreach (DataGridViewRow row in dgwEstaciones.Rows)
                {

                    //if (string.IsNullOrEmpty(row.Cells[9].Value.ToString()) && row.Cells[16].Value != null)
                    //{
                    //    //ALMACENISTA | NOMBRE VS OPERADOR
                    //    row.Cells[9].Value = row.Cells[16].Value.ToString();
                    //}

                    //CARGAR METARIALISTA QUE RECIBE LA ETIQUETA
                    if (string.IsNullOrEmpty(row.Cells[12].Value.ToString()))
                    {
                        //chbTurno.Enabled = false;
                        //cbbTurno.Enabled = false;
                    }

                    if (row.Cells[0].Value != null && row.Cells[24].Value != null)
                    {
                        string sInd = Convert.ToString(row.Cells[24].Value);
                        if (!string.IsNullOrEmpty(sInd))
                        {
                            string sEntrega = Convert.ToString(row.Cells[25].Value);
                            row.Cells[11].Value = sEntrega;
                        }
                    }

                    if (row.Cells[0].Value != null && row.Cells[20].Value != null)
                    {
                        //NOTA DEL ESTATUS DETENIDO DE ETIQUETAS
                        string sInd = Convert.ToString(row.Cells[10].Value);
                        if (sInd == "DETENIDO")
                        {
                            string sNota = Convert.ToString(row.Cells[20].Value);
                            if(!string.IsNullOrEmpty(sNota))
                            {
                                switch(sNota)
                                {
                                    case "INS":
                                        sNota = "INSUFICIENTE";
                                        break;
                                    case "CAL":
                                        sNota = "PROBLEMAS DE CALIDAD";
                                        break;
                                    case "INV":
                                        sNota = "PROBLEMAS DE INVENTARIO";
                                        break;
                                    case "DOC":
                                        sNota = "PROBLEMAS CON DOCUMENTACION";
                                        break;
                                    case "CAP":
                                        sNota = "CAPACIDAD";
                                        break;
                                    case "CAN":
                                        sNota = "RPO CANCELADO";
                                        break;
                                    case "EST":
                                        sNota = "PROBLEMAS DE ESTRUCTURA EN RPO";
                                        break;
                                }
                                
                                sInd += Environment.NewLine + sNota;
                                row.Cells[10].Value = sInd;
                            }
                            
                        }
                    }

                    if (row.Cells[0].Value != null && row.Cells[21].Value != null)
                    {
                        //NOTA DEL ESTATUS DETENIDO DE ETIQUETAS
                        string sInd = Convert.ToString(row.Cells[12].Value);
                        if (sInd == "DETENIDO")
                        {
                            string sNota = Convert.ToString(row.Cells[21].Value);
                            if (!string.IsNullOrEmpty(sNota))
                            {
                                switch (sNota)
                                {
                                    case "INS":
                                        sNota = "INSUFICIENTE";
                                        break;
                                    case "CAL":
                                        sNota = "PROBLEMAS DE CALIDAD";
                                        break;
                                    case "INV":
                                        sNota = "PROBLEMAS DE INVENTARIO";
                                        break;
                                    case "DOC":
                                        sNota = "PROBLEMAS CON DOCUMENTACION";
                                        break;
                                    case "EST":
                                        sNota = "PROBLEMAS DE ESTRUCTURA EN RPO";
                                        break;
                                    case "CAP":
                                        sNota = "CAPACIDAD";
                                        break;
                                    case "CAN":
                                        sNota = "RPO CANCELADO";
                                        break;
                                }
                                sInd += Environment.NewLine + sNota;
                                row.Cells[12].Value = sInd;
                            }

                        }

                    }
                }
                
                #endregion
            }

            dgwEstaciones.Columns[0].Visible = false;
            dgwEstaciones.Columns[1].Visible = false;
            dgwEstaciones.Columns[2].Visible = false;
            dgwEstaciones.Columns[3].Visible = false;
            dgwEstaciones.Columns[16].Visible = false;
            dgwEstaciones.Columns[17].Visible = false;
            dgwEstaciones.Columns[18].Visible = false;
            dgwEstaciones.Columns[19].Visible = false;
            dgwEstaciones.Columns[20].Visible = false;
            dgwEstaciones.Columns[21].Visible = false;
            dgwEstaciones.Columns[22].Visible = false;
            dgwEstaciones.Columns[23].Visible = false;
            dgwEstaciones.Columns[24].Visible = false;
            dgwEstaciones.Columns[25].Visible = false;
            dgwEstaciones.Columns[26].Visible = false;
            dgwEstaciones.Columns[27].Visible = false;
            dgwEstaciones.Columns[28].Visible = false;
            dgwEstaciones.Columns[29].Visible = false;
            dgwEstaciones.Columns[30].Visible = false;
            dgwEstaciones.Columns[31].Visible = false;
            dgwEstaciones.Columns[32].Visible = false;
            dgwEstaciones.Columns[33].Visible = false;
            dgwEstaciones.Columns[34].Visible = false;
            dgwEstaciones.Columns[35].Visible = false;
        }

        //ESCANER SOBRE #OPERADOR && LINEA OP
        private void AjustaColumnas()
        {
            dgwEstaciones.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwEstaciones.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            dgwEstaciones.Columns[4].Width = ColumnWith(dgwEstaciones, 3);//TURNO
            dgwEstaciones.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[4].ReadOnly = true;

            dgwEstaciones.Columns[5].Width = ColumnWith(dgwEstaciones, 10);//RPO
            dgwEstaciones.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[5].ReadOnly = true;

            dgwEstaciones.Columns[6].Width = ColumnWith(dgwEstaciones, 10);//LINEA
            dgwEstaciones.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[7].Width = ColumnWith(dgwEstaciones, 4);//CANT
            dgwEstaciones.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgwEstaciones.Columns[7].ReadOnly = false;

            dgwEstaciones.Columns[8].Width = ColumnWith(dgwEstaciones, 10);//MODELO
            dgwEstaciones.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[8].ReadOnly = true;

            dgwEstaciones.Columns[9].Width = ColumnWith(dgwEstaciones, 9);//ALMACENISTA
            dgwEstaciones.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgwEstaciones.Columns[9].ReadOnly = true;

            dgwEstaciones.Columns[10].Width = ColumnWith(dgwEstaciones, 9);//ETIQUETA
            dgwEstaciones.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[10].ReadOnly = true;

            dgwEstaciones.Columns[11].Width = ColumnWith(dgwEstaciones, 8);//LOCACION
            dgwEstaciones.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[11].ReadOnly = true;

            dgwEstaciones.Columns[12].Width = ColumnWith(dgwEstaciones, 9);//ALMACEN
            dgwEstaciones.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[12].ReadOnly = true;

            dgwEstaciones.Columns[13].Width = ColumnWith(dgwEstaciones, 6);//PRIORIDAD
            dgwEstaciones.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[13].ReadOnly = true;

            dgwEstaciones.Columns[14].Width = ColumnWith(dgwEstaciones, 6);//WP
            dgwEstaciones.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[14].ReadOnly = true;

            dgwEstaciones.Columns[15].Width = ColumnWith(dgwEstaciones, 8);//ESTATUS
            dgwEstaciones.Columns[15].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[15].ReadOnly = true;
        }
        private void dgwEstaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex == 9) //ALMACENISTA
            {
                
                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "50") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string sAlm = dgwEstaciones[12, e.RowIndex].Value.ToString();
                if (string.IsNullOrEmpty(sAlm) )
                {
                    MessageBox.Show("No puede iniciar etiquetas que almacen no ha solicitado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    long lFolio = long.Parse(dgwEstaciones[0, e.RowIndex].Value.ToString());
                    int iCons = int.Parse(dgwEstaciones[1, e.RowIndex].Value.ToString());

                    wfCapturaPop_1t CapPop = new wfCapturaPop_1t("");
                    CapPop._lsProceso = _lsProceso;
                    CapPop._llFolio = lFolio;
                    CapPop._liConsec = iCons;
                    CapPop._lsPlanta = "EMPN";
                    CapPop._sClave = "ALMACENISTA";
                    CapPop.ShowDialog();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            if (e.ColumnIndex == 11)//UBICACION
            {
                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "70") == false )
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                
                long lFolio = long.Parse(dgwEstaciones[0, e.RowIndex].Value.ToString());
                int iCons = int.Parse(dgwEstaciones[1, e.RowIndex].Value.ToString());
                string sArea = string.Empty;

                string sEti = dgwEstaciones[17, e.RowIndex].Value.ToString();
                if (string.IsNullOrEmpty(sEti) || sEti != "L")
                    return;
                
                wfCapturaPop_1t CapPop = new wfCapturaPop_1t("");
                CapPop._lsProceso = _lsProceso;
                CapPop._llFolio = lFolio;
                CapPop._liConsec = iCons;
                CapPop._lsPlanta = "EMPN";
                CapPop._sClave = "UBICACION";
                CapPop.ShowDialog();
            }
            
            if (e.ColumnIndex == 10 || e.ColumnIndex == 12) //actividades-PRIORIDAD
            {
                long lFolio = long.Parse(dgwEstaciones[0, e.RowIndex].Value.ToString());
                int iCons = int.Parse(dgwEstaciones[1, e.RowIndex].Value.ToString());
                string sLinea = dgwEstaciones[6, e.RowIndex].Value.ToString();
                string sPrioridad = dgwEstaciones[28, e.RowIndex].Value.ToString();
                
                string sArea = string.Empty;
                string sValor = string.Empty;

                if (e.ColumnIndex == 10)//ETIQUETAS
                {
                    if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == false &&  GlobalVar.gsArea != "L" && GlobalVar.gsArea != "P")
                    {
                        MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    sValor = dgwEstaciones[10, e.RowIndex].Value.ToString();
                    string sAlm = dgwEstaciones[12, e.RowIndex].Value.ToString();
                    string sPrio = dgwEstaciones[13, e.RowIndex].Value.ToString();
                    if (string.IsNullOrEmpty(sAlm) && string.IsNullOrEmpty(sPrio))
                    {
                        MessageBox.Show("No puede iniciar etiquetas que almacen no ha solicitado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (e.ColumnIndex == 12) //ALMACEN
                {
                    if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "30") == false)
                    {
                        MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    sValor = dgwEstaciones[12, e.RowIndex].Value.ToString();
                    
                }

                if (e.ColumnIndex == 10)
                    sArea = "ETI";
                if (e.ColumnIndex == 12)
                    sArea = "ALM";

                /*VALIDA LA SECUENCIA DE RPO PARA ARMAR POR TURNO*/
                ControlRpoLogica rpo = new ControlRpoLogica();
                rpo.Folio = lFolio;
                rpo.Consec = iCons;
                rpo.Turno = _lsTurno;
                rpo.Estatus = sPrioridad;
                rpo.Linea = sLinea;
                bool bSecuencia = ConfigLogica.VerificaSecuenciaRPO();

                if (e.ColumnIndex == 12 && string.IsNullOrEmpty(sValor) && bSecuencia)                    
                {
                    string sTurnoRpo = dgwEstaciones[4, e.RowIndex].Value.ToString();
                    DataTable dt = ControlRpoLogica.ValidaPendienteSP(rpo);
                    if (dt.Rows.Count > 0)
                    {
                        wfCapturaPopGridSetup wfGrid = new wfCapturaPopGridSetup(_lsProceso);
                        wfGrid._lFolio = lFolio;
                        wfGrid._iCons = iCons;
                        wfGrid._sLinea = sLinea;
                        wfGrid._sEstatus = sPrioridad;
                        wfGrid.ShowDialog();
                        return;
                    }                   
                }

                wfActividEstatusPop ActPop = new wfActividEstatusPop();
                ActPop._lsProceso = _lsProceso;
                ActPop._lFolio = lFolio;
                ActPop._iConsec = iCons;
                ActPop._sTipo = dgwEstaciones[30, e.RowIndex].Value.ToString();
                
                if (e.ColumnIndex == 10)
                    ActPop._sArea = "E";
                else
                    ActPop._sArea = "A";
                ActPop.ShowDialog();

               
            }

            if (e.ColumnIndex == 13) //prioridad
            {
                
                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "40") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                try
                {
                    long lFolio = long.Parse(dgwEstaciones[0, e.RowIndex].Value.ToString());
                    int iCons = int.Parse(dgwEstaciones[1, e.RowIndex].Value.ToString());

                    wfCapturaPop_1t CapPop = new wfCapturaPop_1t("");
                    CapPop._lsProceso = _lsProceso;
                    CapPop._llFolio = lFolio;
                    CapPop._liConsec = iCons;
                    CapPop._lsPlanta = "EMPN";
                    CapPop._sClave = "PRIORIDAD";
                    CapPop.ShowDialog();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            if (e.ColumnIndex == 14)//WP
            {

                //string sEst = dgwEstaciones[16, e.RowIndex].Value.ToString();
                //if (sEst == "CANCELADO")
                //{
                //    MessageBox.Show("No se permite modificar registros Cancelados", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "60") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                try
                {
                    long lFolio = long.Parse(dgwEstaciones[0, e.RowIndex].Value.ToString());
                    int iCons = int.Parse(dgwEstaciones[1, e.RowIndex].Value.ToString());
                    string sEst = dgwEstaciones[19, e.RowIndex].Value.ToString();
                    if (sEst == "E")
                    {
                        MessageBox.Show("El estatus del RPO debe estar EN PROCESO antes de asignar W.P.",Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    wfCapturaPop_1t CapPop = new wfCapturaPop_1t("");
                    CapPop._lsProceso = _lsProceso;
                    CapPop._llFolio = lFolio;
                    CapPop._liConsec = iCons;
                    CapPop._lsPlanta = "EMPN";
                    CapPop._sClave = "WP";
                    CapPop.ShowDialog();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            if (e.ColumnIndex >= 9 || e.ColumnIndex <= 14)
            {
                CargarDetalle("0");
                if(dgwEstaciones.Rows.Count > 0)
                    dgwEstaciones.CurrentCell = dgwEstaciones[e.ColumnIndex, e.RowIndex];
            }


        }

        private void dgwEstaciones_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
                Close();

            if (e.KeyCode == Keys.F3)
            {
                CargarDetalle("0");
            }

            if (e.KeyCode == Keys.F5)
            {
                //RASTREAR LOCACION PARA RPO CON LOCACION BORRADA
                if (dgwEstaciones.Rows.Count == 0)
                    return;

                try
                {

                    int iRow = dgwEstaciones.CurrentCell.RowIndex;
                    if (dgwEstaciones.CurrentRow.Index == -1)
                        return;

                    long lFolio = long.Parse(dgwEstaciones[0, iRow].Value.ToString());
                    int iCons = int.Parse(dgwEstaciones[1, iRow].Value.ToString());

                    string sRPO = dgwEstaciones[5, iRow].Value.ToString();
                    string sLoc = dgwEstaciones[11, iRow].Value.ToString();

                    if(sLoc.IndexOf("-") == -1)
                    {
                        ControlRpoLogica rpo = new ControlRpoLogica();
                        rpo.RPO = sRPO;
                        string sLocAnt = ControlRpoLogica.RastreaLocacion(rpo);
                        if (!string.IsNullOrEmpty(sLocAnt))
                            dgwEstaciones[11, iRow].Value = sLocAnt;

                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                
            }
        }

        private void dgwEstaciones_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
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
        private void wfMonitorRPO_Resize(object sender, EventArgs e)
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
                int id = 60;
                int ix = iw - id;
                btnLoad.Location = new Point(ix, iy);
                ix -= id;
                btnSort.Location = new Point(ix, iy);
            }         
        }
        #endregion

        #region regCaptura

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cbbLinea.ResetText();
            LineaLogica lin = new LineaLogica();
            lin.Planta = cbbPlanta.SelectedValue.ToString();
            DataTable data = new DataTable();

            if(lin.Planta == "EMPN")
            {
                data = LineaLogica.LineasNav(lin);
                cbbLinea.DataSource = data;
                cbbLinea.DisplayMember = "linea_nav";
                cbbLinea.ValueMember = "linea_nav";
                cbbLinea.SelectedIndex = -1;
            }
            else
            {
                data = LineaLogica.LineaPlanta(lin);
                cbbLinea.DataSource = data;
                cbbLinea.DisplayMember = "nombre";
                cbbLinea.ValueMember = "linea";
                cbbLinea.SelectedIndex = -1;
            }
            
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            CargarDetalle("0");
            timer2.Stop();
            timer1.Start();
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

      
        private void chbPrioridad_CheckedChanged(object sender, EventArgs e)
        {
            cbbPrioridad.Enabled = chbPrioridad.Checked;
            cbbTipo.Enabled = chbPrioridad.Checked;
        }

        #endregion

        #region regFiltros
        private void txtRPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            string sRPO = txtRPO.Text.ToString().Trim();
            if (sRPO.IndexOf("RPO") == -1)
            {
                int iRpo = 0;
                if (int.TryParse(sRPO, out iRpo))
                    txtRPO.Text = "RPO" + sRPO;
            }

            CargarDetalle("0");

            txtRPO.Focus();
            if(!string.IsNullOrEmpty(txtRPO.Text))
            {
                txtRPO.SelectionStart = 0;
                txtRPO.SelectionLength = txtRPO.Text.Length;
            }
            
        }

       
        private void cbbPlanta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            CargarDetalle("0");
        }

        

        private void cbbPrioridad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            CargarDetalle("0");
        }

        private void cbbTipo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            CargarDetalle("0");
        }

        private void cbbEstatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            CargarDetalle("0");
        }

        private void cbbTurno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            CargarDetalle("0");
        }

        private void cbbLinea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            CargarDetalle("0");
        }
        #endregion

        private void btExcel_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "80") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ExportarFormato();
        }

        private void ExportarFormato()
        {
            Cursor = Cursors.WaitCursor;
            Microsoft.Office.Interop.Excel.Application oXL = null;
            Microsoft.Office.Interop.Excel._Workbook oWB = null;
            Microsoft.Office.Interop.Excel._Worksheet oSheet = null;

            try
            {
                oXL = new Microsoft.Office.Interop.Excel.Application();
                oWB = oXL.Workbooks.Open(@"\\mxni-fs-01\Temp\wrivera\agonz0\CloverPro\EMPAQUE\RPOEmpaque.xlsx");
                oSheet = String.IsNullOrEmpty("Sheet1") ? (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet : (Microsoft.Office.Interop.Excel._Worksheet)oWB.Worksheets["Sheet1"];

                int iRow = 2;
                foreach (DataGridViewRow row in dgwEstaciones.Rows)
                {
                    DateTime dtIngreso = Convert.ToDateTime(row.Cells[2].Value.ToString());
                    string sTurno = row.Cells[4].Value.ToString();
                    string sRPO = row.Cells[5].Value.ToString();
                    string sLinea = row.Cells[6].Value.ToString();
                    int iCant = int.Parse(row.Cells[7].Value.ToString());
                    string sModelo = row.Cells[8].Value.ToString();
                    string sAlmacenista = row.Cells[9].Value.ToString();
                    string sEtiqueta = row.Cells[10].Value.ToString();
                    string sLocacion = row.Cells[11].Value.ToString();
                    string sAlmacen = row.Cells[12].Value.ToString();
                    string sPrio = row.Cells[13].Value.ToString();
                    string sWP = row.Cells[14].Value.ToString();
                    string sEstatus = row.Cells[15].Value.ToString();

                    DateTime dtProceso = DateTime.Today;
                    if (!string.IsNullOrEmpty(row.Cells[22].Value.ToString()))
                        dtProceso = Convert.ToDateTime(row.Cells[22].Value.ToString());
                    string sTurnoPro = row.Cells[23].Value.ToString();
                    string sHora = string.Empty;

                    DateTime dtProceti = DateTime.Today;
                    if (!string.IsNullOrEmpty(row.Cells[26].Value.ToString()))
                        dtProceti = Convert.ToDateTime(row.Cells[26].Value.ToString());
                    string sTurnoProcet = row.Cells[27].Value.ToString();
                    string sHoraEti = string.Empty;

                    //DETENIDOS
       
                    DateTime dtfCarga = DateTime.Today;
                    if (!string.IsNullOrEmpty(row.Cells[31].Value.ToString()))
                        dtfCarga = Convert.ToDateTime(row.Cells[31].Value.ToString());
                    string sHoraCarga = string.Empty;

                    DateTime dtfDetenido = DateTime.Today;
                    if (!string.IsNullOrEmpty(row.Cells[32].Value.ToString()))
                        dtfDetenido = Convert.ToDateTime(row.Cells[32].Value.ToString());
                    string sHoraDetenido = string.Empty;

                    string sItemDetenido = row.Cells[33].Value.ToString();
                    string contDetenido = row.Cells[34].Value.ToString();
                    string UsDetenido = row.Cells[35].Value.ToString();
                    
                    oSheet.Cells[iRow, 1] = dtIngreso;
                    oSheet.Cells[iRow, 2] = sTurno;
                    oSheet.Cells[iRow, 3] = sRPO;
                    oSheet.Cells[iRow, 4] = sLinea;
                    oSheet.Cells[iRow, 5] = iCant;
                    oSheet.Cells[iRow, 6] = sModelo;
                    oSheet.Cells[iRow, 7] = sAlmacenista;
                    oSheet.Cells[iRow, 8] = sEtiqueta;
                    oSheet.Cells[iRow, 9] = sLocacion;
                    oSheet.Cells[iRow, 10] = sAlmacen;
                    oSheet.Cells[iRow, 11] = sPrio;
                    oSheet.Cells[iRow, 12] = sWP;
                    oSheet.Cells[iRow, 13] = sEstatus;

                    if (!string.IsNullOrEmpty(row.Cells[22].Value.ToString()))
                    {
                        sHora = Convert.ToString(row.Cells[22].Value);
                        int iPos = sHora.IndexOf(":");
                        sHora = sHora.Substring(iPos - 2);

                        oSheet.Cells[iRow, 14] = dtProceso;
                        oSheet.Cells[iRow, 15] = sHora.TrimStart();
                    }
                    
                    oSheet.Cells[iRow, 16] = sTurnoPro;

                    if (!string.IsNullOrEmpty(row.Cells[26].Value.ToString()))
                    {
                        sHoraEti = Convert.ToString(row.Cells[26].Value);
                        int iPos1 = sHoraEti.IndexOf(":");
                        sHoraEti = sHoraEti.Substring(iPos1 - 2);
                        
                        oSheet.Cells[iRow, 17] = dtProceti;
                        oSheet.Cells[iRow, 18] = sHoraEti.TrimStart();
                    }
                    oSheet.Cells[iRow, 19] = sTurnoProcet;

                    //DETENIDOS
                    if (!string.IsNullOrEmpty(row.Cells[31].Value.ToString()))
                    {
                        sHoraCarga = Convert.ToString(row.Cells[31].Value);
                        int iPos1 = sHoraCarga.IndexOf(":");
                        sHoraCarga = sHoraCarga.Substring(iPos1 - 2);

                        oSheet.Cells[iRow, 20] = dtfCarga;
                        oSheet.Cells[iRow, 21] = sHoraCarga.TrimStart();
                    }
                    if (!string.IsNullOrEmpty(row.Cells[32].Value.ToString()))
                    {
                        sHoraDetenido = Convert.ToString(row.Cells[32].Value);
                        int iPos1 = sHoraDetenido.IndexOf(":");
                        sHoraDetenido = sHoraDetenido.Substring(iPos1 - 2);

                        oSheet.Cells[iRow, 22] = dtfDetenido;
                        oSheet.Cells[iRow, 23] = sHoraDetenido.TrimStart();
                    }
                    oSheet.Cells[iRow, 24] = sItemDetenido;
                    oSheet.Cells[iRow, 25] = contDetenido;
                    oSheet.Cells[iRow, 26] = UsDetenido;
                    
                    iRow++;
                }

                oXL.DisplayAlerts = false;
                oWB.SaveAs(@"C:\CloverPRO\Formatos\RPOEmpaque.xlsx", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared);

                DialogResult Result = MessageBox.Show("Se ha exportado la consulta." + Environment.NewLine + "Desea abrir el reporte en excel?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    string sDirec = @"C:\CloverPRO\Formatos\RPOEmpaque.xlsx";
                    System.Diagnostics.Process.Start(sDirec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Cursor = Cursors.Default;
            }
            finally
            {
                if (oWB != null)
                {
                    oWB.Close();
                    oXL.Quit();
                    Cursor = Cursors.Default;
                }
            }
            Cursor = Cursors.Default;
        }

        private void chbRPO_CheckedChanged(object sender, EventArgs e)
        {
            txtRPO.Enabled = chbRPO.Checked;
        }

        private void cbbPrioridad_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cbbTipo.ResetText();

            if (cbbPrioridad.SelectedValue.ToString() == "P")
            {
                Dictionary<string, string> Data = new Dictionary<string, string>();
                Data.Add("0", "0");
                Data.Add("HOT", "HOT");
                Data.Add("GLO", "GLOBAL");
                Data.Add("RBX", "REBOX");
                Data.Add("BKO", "BACK ORDER");
                Data.Add("PDU", "PAST DUE");
                Data.Add("FUS", "FUSORES");
                Data.Add("PT1", "PTO 1/19");
                Data.Add("PT2", "PTO 2/08");
                Data.Add("C1B", "CAN01 BRND");
                Data.Add("XRX", "XRX CAN01");
                Data.Add("PSB", "PUSHBACK");
                cbbTipo.DataSource = new BindingSource(Data, null);
                cbbTipo.DisplayMember = "Value";
                cbbTipo.ValueMember = "Key";
                cbbTipo.SelectedIndex = -1;
            }

            if (cbbPrioridad.SelectedValue.ToString() == "L")
            {
                Dictionary<string, string> Data2 = new Dictionary<string, string>();
                Data2.Add("L", "Largos");
                Data2.Add("P", "Pony");
                cbbTipo.DataSource = new BindingSource(Data2, null);
                cbbTipo.DisplayMember = "Value";
                cbbTipo.ValueMember = "Key";
                cbbTipo.SelectedIndex = -1;
            }

            if(cbbPrioridad.SelectedValue.ToString() == "A")
            {
                OperadorLogica op = new OperadorLogica();
                op.Planta = cbbPlanta.SelectedValue.ToString();
                op.Nivel = "MAT";
                op.Turno = GlobalVar.gsTurno;
                DataTable dt = OperadorLogica.ConsultarPuesto(op);
                cbbTipo.DataSource = dt;
                cbbTipo.DisplayMember = "nombre";
                cbbTipo.ValueMember = "empleado";
                cbbTipo.SelectedIndex = -1;
            }

            if (cbbPrioridad.SelectedValue.ToString() == "E")
            {
                Dictionary<string, string> Data2 = new Dictionary<string, string>();
                Data2.Add("P", "Proceso");
                Data2.Add("D", "Detenido");
                Data2.Add("L", "Listo");
                Data2.Add("E", "Entregado");
                cbbTipo.DataSource = new BindingSource(Data2, null);
                cbbTipo.DisplayMember = "Value";
                cbbTipo.ValueMember = "Key";
                cbbTipo.SelectedIndex = -1;
            }

            if (cbbPrioridad.SelectedValue.ToString() == "M")
            {
                Dictionary<string, string> Data2 = new Dictionary<string, string>();
                Data2.Add("E", "En Espera");
                Data2.Add("P", "Proceso");
                Data2.Add("D", "Detenido");
                Data2.Add("L", "Listo");
                cbbTipo.DataSource = new BindingSource(Data2, null);
                cbbTipo.DisplayMember = "Value";
                cbbTipo.ValueMember = "Key";
                cbbTipo.SelectedIndex = -1;
            }

            if (cbbPrioridad.SelectedValue.ToString() == "T")
            {
                Dictionary<string, string> Data2 = new Dictionary<string, string>();
                Data2.Add("1", "1");
                Data2.Add("2", "2");
                cbbTipo.DataSource = new BindingSource(Data2, null);
                cbbTipo.DisplayMember = "Value";
                cbbTipo.ValueMember = "Key";
                cbbTipo.SelectedIndex = -1;
            }

        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            
            if (cbbPlanta.SelectedIndex == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, "CAT06500") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            wfCargarRPO wfRPO = new wfCargarRPO();
            wfRPO._lsPlanta = cbbPlanta.SelectedValue.ToString();
            wfRPO.ShowDialog();

            CargarDetalle("0");
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            ListSortDirection direction;
            direction = ListSortDirection.Ascending;
            dgwEstaciones.Sort(dgwEstaciones.Columns[22], direction);

            /*
            if(_lbEti)
            {
                CargarDetalle("1");
                timer1.Stop();
                timer2.Start();
            }
              */  
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            CargarDetalle("1");
        }

        private void cbbTipo_DropDown(object sender, EventArgs e)
        {
           
        }

        private void btPrint_Click(object sender, EventArgs e)
        {
            wfRepEmpaqueProc Report = new wfRepEmpaqueProc();
            Report.ShowDialog();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
