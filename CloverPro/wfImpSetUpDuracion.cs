using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using Logica;

namespace CloverPro
{
    public partial class wfImpSetUpDuracion : Form
    {
        private string _lsProceso = "PRO090";
        public bool _lbCambio;
        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;

        public wfImpSetUpDuracion()
        {
            InitializeComponent();
            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }

        #region regInicio
        private void wfRepEtiquetas_Load(object sender, EventArgs e)
        {
            Inicio();
            
        }
        private void wfRepEtiquetas_Activated(object sender, EventArgs e)
        {
            chbPlanta.Focus();
        }
        private void Inicio()
        {
            _lbCambio = false;

            cbbPlanta.ResetText();
            DataTable data = PlantaLogica.Listar();
            cbbPlanta.DataSource = data;
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.SelectedIndex = -1;

            chbPlanta.Checked = false;
            cbbPlanta.Enabled = false;

            chbLinea.Checked = false;
            cbbLinea.Enabled = false;

            dtpInicio.ResetText();
            dtpFinal.ResetText();
            
            CargarLineas();
            CargarColumnas();

            chbPlanta.Focus();

        }

        private void CargarLineas()
        {
            DataTable dtL1 = new DataTable();
            DataTable dtL2 = new DataTable();
            if (chbPlanta.Checked && cbbPlanta.SelectedIndex != -1)
            {
                LineaLogica line = new LineaLogica();
                line.Planta = cbbPlanta.SelectedValue.ToString();
                dtL1 = LineaLogica.LineaPlanta(line);
            }
            else
            {
                dtL1 = LineaLogica.ListarTodas();
            }
                

            cbbLinea.DataSource = dtL1;
            cbbLinea.ValueMember = "linea";
            cbbLinea.DisplayMember = "linea";
            cbbLinea.SelectedIndex = -1;

        }
        #endregion

        private bool Valida()
        {
            bool bValida = false;

            if (string.IsNullOrEmpty(dtpInicio.Text) || string.IsNullOrEmpty(dtpFinal.Text))
            {
                MessageBox.Show("Favor de especificar el Periódo del reporte", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpInicio.Focus();
                return bValida;
            }
            else
            {
                if(dtpInicio.Value > dtpFinal.Value)
                {
                    MessageBox.Show("La Fecha Inicial no debe ser mayor a la Fecha Final", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtpFinal.Focus();
                    return bValida;
                }
            }

            if (chbPlanta.Checked && cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado la planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }

            

            if (chbLinea.Checked && cbbLinea.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de espeficicar la Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return bValida;
            }

            return true;

        }

        #region regData
        private int ColumnWith(DataGridView _dtGrid, double _dColWith)
        {

            double dW = _dtGrid.Width - 10;
            double dTam = _dColWith;
            double dPor = dTam / 100;
            dTam = dW * dPor;
            dTam = Math.Truncate(dTam);

            return Convert.ToInt32(dTam);
        }
        private void CargarColumnas2()
        {

            int iRows = dgwExcel.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("datos");
                dtNew.Columns.Add("planta", typeof(string));//12
                dtNew.Columns.Add("linea", typeof(string));//4
                dtNew.Columns.Add("prev_rpo", typeof(string));//5
                dtNew.Columns.Add("next_rpo", typeof(string));//6
                dtNew.Columns.Add("prev_date", typeof(string));//9
                dtNew.Columns.Add("next_date", typeof(string));//10
                dtNew.Columns.Add("minutes", typeof(double));//11
                
                dgwExcel.DataSource = dtNew;
            }
        }
        private void CargarColumnas()
        {

            int iRows = dgwData.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("datos");
                dtNew.Columns.Add("fecha", typeof(int));//0
                dtNew.Columns.Add("folio", typeof(int));//1
                dtNew.Columns.Add("consec", typeof(int));//2
                dtNew.Columns.Add("TURNO", typeof(string));//3
                dtNew.Columns.Add("LINEA", typeof(string));//4
                dtNew.Columns.Add("RPO ANTERIOR", typeof(string));//5
                dtNew.Columns.Add("RPO SIGUIENTE", typeof(string));//6
                dtNew.Columns.Add("HORARIO DE CAMBIO", typeof(string));//7
                dtNew.Columns.Add("MINS", typeof(double));//8
                dtNew.Columns.Add("INICIA MP", typeof(string));//9
                dtNew.Columns.Add("TERMINA MP", typeof(string));//10
                dtNew.Columns.Add("MINS MP", typeof(double));//11
                dtNew.Columns.Add("RPO SIG MP", typeof(string));//12
                dtNew.Columns.Add("planta", typeof(string));//12
                dgwData.DataSource = dtNew;
            }

            dgwData.Columns[0].Visible = false;
            dgwData.Columns[1].Visible = false;
            dgwData.Columns[2].Visible = false;
            dgwData.Columns[13].Visible = false;

            //dgwData.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //dgwData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            dgwData.Columns[3].Width = ColumnWith(dgwData, 5);//TURNO
            dgwData.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[3].ReadOnly = true;

            dgwData.Columns[4].Width = ColumnWith(dgwData, 5);//LINEA
            dgwData.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[4].ReadOnly = true;
            

            dgwData.Columns[5].Width = ColumnWith(dgwData, 12);//RPO
            dgwData.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[5].ReadOnly = true;

            dgwData.Columns[6].Width = ColumnWith(dgwData, 12);//RPO
            dgwData.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[6].ReadOnly = true;

            dgwData.Columns[7].Width = ColumnWith(dgwData, 15);//HORARIO
            dgwData.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[7].ReadOnly = true;

            dgwData.Columns[8].Width = ColumnWith(dgwData, 5);//DURACION
            dgwData.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgwData.Columns[8].ReadOnly = true;

            dgwData.Columns[9].Width = ColumnWith(dgwData, 15);//INICIA MP
            dgwData.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[9].ReadOnly = true;

            dgwData.Columns[10].Width = ColumnWith(dgwData, 15);//TERMINA MP
            dgwData.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[10].ReadOnly = true;

            dgwData.Columns[11].Width = ColumnWith(dgwData, 5);//DURACION MP
            dgwData.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgwData.Columns[11].ReadOnly = true;
        
            dgwData.Columns[12].Width = ColumnWith(dgwData, 12);//RPO MP
            dgwData.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[12].ReadOnly = true;
        }

        #endregion

        #region regBotones
        private int getExcel2(string _asArchivo)
        {
            int iCant = 0;
            try
            {
                dgwExcel.DataSource = null;
                CargarColumnas2();

                Excel.Application xlApp = new Excel.Application();
                Excel.Workbooks xlWorkbookS = xlApp.Workbooks;
                Excel.Workbook xlWorkbook = xlWorkbookS.Open(_asArchivo);

                Excel.Worksheet xlWorksheet = new Excel.Worksheet();

                string sValue = string.Empty;
                
                int iSheets = xlWorkbook.Sheets.Count;
                
                xlWorksheet = xlWorkbook.Sheets[iSheets];
                
                Excel.Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                for (int i = 2; i < rowCount; i++)
                {
                    DateTime dtFecha = DateTime.Today;
                    string sRPO = string.Empty;
                    string sRPO2 = string.Empty;
                    string sPta = string.Empty;
                    string sLine = string.Empty;
                    string sFecha = string.Empty;
                    string sFecha2 = string.Empty;
                    string sMin = string.Empty;
                   
                    sValue = string.Empty;

                    if (xlRange.Cells[i, 8].Value2 == null)
                        continue;

                    if (xlRange.Cells[i, 1].Value2 != null)
                        sValue = Convert.ToString(xlRange.Cells[i, 1].Value2.ToString());
                    

                    if (sValue == "TimeDiffBetweenRPOs" || sValue == "Facility Name")
                    {
                        sValue = string.Empty;
                        continue;
                    }

                    if (string.IsNullOrEmpty(sValue))
                        continue;

                    sPta = Convert.ToString(xlRange.Cells[i, 1].Value2.ToString());
                    sLine = Convert.ToString(xlRange.Cells[i, 2].Value2.ToString());

                    //if (sPta != "MX01A" && sPta != "MX02" && sPta != "MX06" && sPta != "MXFSR")
                    //    continue;

                    int iLine = 0;
                    if (int.TryParse(sLine, out iLine))
                    {
                        if (iLine > 55)
                            continue;
                    }


                    if (cbbPlanta.SelectedIndex != -1)
                    {
                        string sPlanta = cbbPlanta.SelectedValue.ToString();
                        if (sPlanta == "COL")
                            sPlanta = "MX02";
                        if (sPlanta == "NIC2" || sPlanta == "NIC3" || sPlanta == "MON")
                            sPlanta = "MX01A";
                        if (sPlanta == "EMPN" || sPlanta == "EMPF")
                            sPlanta = "MX06";
                        if (sPlanta == "FUS")
                            sPlanta = "MXFSR";

                        if (sPlanta != sPta.ToUpper().Trim())
                            continue;
                    }

                    sRPO = Convert.ToString(xlRange.Cells[i, 3].Value2.ToString());
                    sRPO = sRPO.TrimStart().TrimEnd().ToUpper();
                    sRPO2 = Convert.ToString(xlRange.Cells[i, 4].Value2.ToString());
                    sRPO2 = sRPO2.TrimStart().TrimEnd().ToUpper();
                    if (xlRange.Cells[i, 5].Value2 != null)
                        sFecha = Convert.ToString(xlRange.Cells[i, 5].Value.ToString());
                    else
                        sFecha = Convert.ToString(xlRange.Cells[i, 6].Value.ToString());
                    sFecha2 = Convert.ToString(xlRange.Cells[i, 7].Value.ToString());
                    sMin = Convert.ToString(xlRange.Cells[i, 8].Value2.ToString());

                    AgregaDatoExc(sPta, sLine, sRPO, sRPO2, sFecha, sFecha2, sMin);
                    iCant++;

                }

                xlApp.DisplayAlerts = false;
                xlWorkbook.Close();
                xlApp.DisplayAlerts = true;
                xlApp.Quit();
            }
            catch(Exception ex)
            {
                ex.ToString();
            }

            return iCant;
        }
        private void AgregaDatoExc(string _asPta, string _asLine, string _asRPO, string _asRPO2, string _asFecha, string _asFecha2, string _asDura)
        {
            DataTable dt = dgwExcel.DataSource as DataTable;
            dt.Rows.Add(_asPta, _asLine, _asRPO, _asRPO2, _asFecha, _asFecha2, _asDura);
        }
        private DataTable getExcel(string _asArchivo)
        {
            DataTable dt = new DataTable();

            try
            {
                OleDbConnection conexion = null;
                DataSet dataSet = null;
                OleDbDataAdapter dataAdapter = null;

                string consultaHojaExcel = "Select * from [TimeDiffBetweenRPOs$]";

                //esta cadena es para archivos excel 2007 y 2010
                string cadenaConexionArchivoExcel = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + _asArchivo + "';Mode=ReadWrite;Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";
                //Validamos que el usuario ingrese el nombre de la hoja del archivo de excel a leer
                
                conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
                conexion.Open(); //abrimos la conexion
                dataAdapter = new OleDbDataAdapter(consultaHojaExcel, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
                dataSet = new DataSet(); // creamos la instancia del objeto DataSet
                dataAdapter.TableMappings.Add("tbl", "Table");
                dataAdapter.Fill(dataSet);//llenamos el dataset
                dt = dataSet.Tables[0];
                    
                conexion.Close();//cerramos la conexion
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja" + Environment.NewLine + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return dt;

        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (dgwData.Rows.Count == 0)
                return;

            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Archivos de Excel (*.xls;*.xlsx)|*.xls;*.xlsx";

                dialog.Title = "Seleccione el archivo de Excel";

                dialog.FileName = string.Empty;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    int iCant = getExcel2(dialog.FileName);

                    if (iCant == 0)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("There is no data to compare.",Text,MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                        return;
                    }
                        

                    foreach (DataGridViewRow row in dgwData.Rows)
                    {
                        string sPlanta = row.Cells[13].Value.ToString();
                        string sPta = string.Empty;
                        if (sPlanta == "COL")
                            sPta = "MX02";
                        if (sPlanta == "NIC2" || sPlanta == "NIC3")
                            sPta = "MX01A";
                        if (sPlanta == "EMPN" || sPlanta == "EMPF")
                            sPta = "MX06";
                        string sLinea = row.Cells[4].Value.ToString();
                        if (sPlanta == "COL")
                        {
                            sLinea = sLinea.Substring(1);
                            if (sLinea.IndexOf('0') == 0)
                                sLinea = sLinea.Substring(1);
                        }

                        string sRpo = row.Cells[5].Value.ToString();
                        string sRpo2 = row.Cells[6].Value.ToString();
                        string sHora = row.Cells[7].Value.ToString();

                        DateTime dtHora = DateTime.Now;
                        if (!string.IsNullOrEmpty(sHora))
                            dtHora = DateTime.Parse(sHora);
                        
                        for (int x = 0; x < dgwExcel.Rows.Count - 1; x++)
                        {

                            string sPtaMP = dgwExcel[0, x].Value.ToString().ToUpper();
                            if (sPtaMP != sPta)
                                continue;

                            string sLineMP = dgwExcel[1, x].Value.ToString();
                            if (sLinea != sLineMP)
                                continue;

                            string sRpoMP = dgwExcel[2, x].Value.ToString();
                            string sRpoNext = dgwExcel[3, x].Value.ToString();
                            string sIniMP = dgwExcel[4, x].Value.ToString();
                            string sFinMP = dgwExcel[5, x].Value.ToString();
                            string sDura = dgwExcel[6, x].Value.ToString();
                            if (sRpo == sRpoMP && sRpo2 ==  sRpoNext)//VALIDA RPO X LINEA
                            {
                                //VALIDA FECHA
                                if(!string.IsNullOrEmpty(sHora))
                                {
                                    DateTime dtIni = DateTime.Parse(sIniMP);
                                    int iDays = (dtIni - dtHora).Days;
                                    if (iDays > 0)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (dtIni.Day != dtHora.Day)
                                        {
                                            if (dtIni.Hour >= 6)
                                                continue;
                                        }
                                    }
                                }
                                
                                row.Cells[9].Value = sIniMP;
                                row.Cells[10].Value = sFinMP;
                                row.Cells[11].Value = Double.Parse(sDura);
                                row.Cells[12].Value = sRpoNext;
                                iCant++;
                            }
                        }
                    }

                    Cursor = Cursors.Arrow;
                    txtMP.Text = iCant.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }
        
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            wfImpresor Impresor = new wfImpresor();
            Impresor._lsProceso = _lsProceso;

            string sIndP = "0";
            if (chbPlanta.Checked)
                sIndP = "1";

          
            string sIndL = "0";
            if (chbLinea.Checked)
                sIndL = "1";
           
            Impresor._ldtFinal = dtpFinal.Value;
            Impresor._ldtInicio = dtpInicio.Value;
            Impresor._lsIndPlanta = sIndP;
            if (sIndP == "1")
                Impresor._lsPlanta = cbbPlanta.SelectedValue.ToString();
            else
                Impresor._lsPlanta = "";

            Impresor._lsIndLinea = sIndL;
            if (sIndL == "1")
                Impresor._lsLineaIni = cbbLinea.SelectedValue.ToString();
            else
                Impresor._lsLineaIni = "";

            Impresor.Show();
        }

        #endregion

        #region regCaptura
        private void dtpInicio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
        private void cbbPlanta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbbPlanta.Text.Length >= 9)
                e.Handled = true;
        }

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CargarLineas();
        }

       

        private void chbPlanta_CheckedChanged(object sender, EventArgs e)
        {
            cbbPlanta.Enabled = chbPlanta.Checked;
        }

        private void chbLinea_CheckedChanged(object sender, EventArgs e)
        {
            cbbLinea.Enabled = chbLinea.Checked;
        }
        #endregion
        private void CargarDatos()
        {
            try
            {
                MonitorSetupLogica mon = new MonitorSetupLogica();
                mon.FechaFin = dtpFinal.Value;
                mon.FechaIni = dtpInicio.Value;
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

                DataTable data = MonitorSetupLogica.DuracionSP(mon);
                dgwData.DataSource = data;
                CargarColumnas();
                int iCant = dgwData.Rows.Count;
                txtCant.Text = iCant.ToString();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
    
        private void button1_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            CargarDatos();
            
        }

        private void dgwData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightYellow;
            else
                e.CellStyle.BackColor = Color.White;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            if (dgwData.Rows.Count == 0)
                return;

            try
            {
                foreach (DataGridViewRow row in dgwData.Rows)
                {
                    if (string.IsNullOrEmpty(row.Cells[9].Value.ToString()))
                        continue;

                    long lFolio = long.Parse(row.Cells[1].Value.ToString());
                    int iCons = int.Parse(row.Cells[2].Value.ToString());
                    string sInicia = row.Cells[9].Value.ToString();
                    string sTermina = row.Cells[10].Value.ToString();
                    double dDura = double.Parse(row.Cells[11].Value.ToString());
                    string sRpo = row.Cells[12].Value.ToString();
                    string sPlanta = row.Cells[13].Value.ToString();
                    
                    LineSetupDetLogica set = new LineSetupDetLogica();
                    set.Folio = lFolio;
                    set.Consec = iCons;
                    set.IniciaMP = sInicia;
                    set.FinalMP = sTermina;
                    set.DuraMP = dDura;
                    set.RpoMP = sRpo;
                    LineSetupDetLogica.ActualizaDuraMP(set);
//                        CargarDatos();

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

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
        private void wfImpSetUpDuracion_Resize(object sender, EventArgs e)
        {
            if (WindowState != _WindowStateAnt && WindowState != FormWindowState.Minimized)
            {
                _WindowStateAnt = WindowState;
                ResizeControl(panel1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(groupBox3, 1, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(dgwData, 1, ref _iWidthAnt, ref _iHeightAnt, 1);

                int iy = dgwData.Location.Y;
                int ih = dgwData.Height;
                
                int ix = panel2.Location.X;
                int ipy = iy + ih;
                panel2.Location = new Point(ix, ipy);
            }
        }
        #endregion
    }
}
