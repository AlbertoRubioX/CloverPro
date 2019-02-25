using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using Microsoft.Office.Core;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Excel = Microsoft.Office.Interop.Excel;
using Logica;

namespace CloverPro
{
    public partial class wfImportarRpo : Form
    {
        public bool _lbCambio;
        public wfImportarRpo()
        {
            InitializeComponent();
        }

        private void wfImportarRpo_Load(object sender, EventArgs e)
        {

            chbEdit.Checked = false;

            cbbCatalogo.ResetText();
            Dictionary<string, string> Tipo = new Dictionary<string, string>();
            Tipo.Add("O", "Operadores");
            Tipo.Add("M", "Modelos");
            Tipo.Add("R", "RPO");
            Tipo.Add("I", "Items");
            cbbCatalogo.DataSource = new BindingSource(Tipo, null);
            cbbCatalogo.DisplayMember = "Value";
            cbbCatalogo.ValueMember = "Key";
            cbbCatalogo.SelectedIndex = 0;

            cbbOrigen.ResetText();
            Dictionary<string, string> Data = new Dictionary<string, string>();
            Data.Add("N", "Navision");
            Data.Add("P", "Planeación");
            Data.Add("M", "CloverMP");
            cbbOrigen.DataSource = new BindingSource(Data, null);
            cbbOrigen.DisplayMember = "Value";
            cbbOrigen.ValueMember = "Key";
            cbbOrigen.SelectedIndex = -1;

            cbbPlanta.ResetText();
            cbbPlanta.DataSource = PlantaLogica.Listar();
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.SelectedIndex = -1;

        }

        private void wfImportarRpo_Activated(object sender, EventArgs e)
        {
            txtArchivo.Focus();
        }

      
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

       
        private void btnFile_Click(object sender, EventArgs e)
        {
            
            if (cbbCatalogo.SelectedValue.ToString() == "R")
            {
                if(cbbOrigen.SelectedIndex == -1)
                {
                    MessageBox.Show("Favor de indicar el origen del listado a importar", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archivos de Excel (*.xls;*.xlsx)|*.xls;*.xlsx";

            dialog.Title = "Seleccione el archivo de Excel";

            dialog.FileName = string.Empty;
            
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                btnImportar.Enabled = false;
                Cursor = Cursors.WaitCursor;

                txtArchivo.Text = dialog.FileName;
                string sHoja = string.Empty;
                
                if (cbbCatalogo.SelectedValue.ToString() == "O")
                    sHoja = "PERSONAL LINE-UP";
                if (cbbCatalogo.SelectedValue.ToString() == "M")
                    sHoja = "MODELO";
                if (cbbCatalogo.SelectedValue.ToString() == "R")    
                    sHoja = "Production Order";
                if (cbbCatalogo.SelectedValue.ToString() == "I")
                    sHoja = "ItemUPC";

                LlenarGrid(txtArchivo.Text, sHoja);

                foreach (DataGridViewRow row in dgwRpo.Rows)
                {
                    int iRow = 0;
                    if (sHoja == "MODELO")
                        iRow = 1;
                    if (string.IsNullOrEmpty(Convert.ToString(row.Cells[iRow].Value)))
                        dgwRpo.Rows.Remove(row);
                }

                Cursor = Cursors.Arrow;
                //btnImportar.Enabled = true;

                if (dgwRpo.Rows.Count > 0)
                    btnImportar_Click(sender, e);
            }
        }
        private void CargarColFileGlob()
        {
            if (dgwRpo.Rows.Count == 0)
            {
                DataTable dtNew = new DataTable("Daily");
                dtNew.Columns.Add("RPO", typeof(string));
                dtNew.Columns.Add("descrip", typeof(string));
                dtNew.Columns.Add("MODELO", typeof(string));
                dtNew.Columns.Add("FECHA", typeof(DateTime));
                dtNew.Columns.Add("LINEA", typeof(string));
                dtNew.Columns.Add("CANT", typeof(int));
                dtNew.Columns.Add("TURNO", typeof(string));
                dtNew.Columns.Add("PLANTA", typeof(string));
                dtNew.Columns.Add("PRIORIDAD", typeof(string));
                dtNew.Columns.Add("FECHA_GLOB", typeof(DateTime));
                dtNew.Columns.Add("HR_COMP", typeof(string));
                dtNew.Columns.Add("DESTINO", typeof(string));
                dtNew.Columns.Add("ENTREGA", typeof(string));
                dtNew.Columns.Add("CANT_GLOB", typeof(string));
                dtNew.Columns.Add("NOTA", typeof(string));
                dgwRpo.DataSource = dtNew;

            }

            dgwRpo.Columns[1].Visible = false;

        }
        private void CargarColFile()
        {
            if (dgwRpo.Rows.Count == 0)
            {
                DataTable dtNew = new DataTable("Daily");
                dtNew.Columns.Add("RPO", typeof(string));
                dtNew.Columns.Add("descrip", typeof(string));
                dtNew.Columns.Add("MODELO", typeof(string));
                dtNew.Columns.Add("FECHA", typeof(DateTime));
                dtNew.Columns.Add("LINEA", typeof(string));
                dtNew.Columns.Add("CANT", typeof(int));
                dtNew.Columns.Add("TURNO", typeof(string));
                dtNew.Columns.Add("PLANTA", typeof(string));
                dtNew.Columns.Add("NOTA", typeof(string));
                dgwRpo.DataSource = dtNew;

            }

            dgwRpo.Columns[1].Visible = false;

        }
        private void LlenarGrid2(string _asArchivo)
        {
            try
            {
                dgwRpo.DataSource = null;
                CargarColFile();

                Excel.Application xlApp = new Excel.Application();
                Excel.Workbooks xlWorkbookS = xlApp.Workbooks;
                Excel.Workbook xlWorkbook = xlWorkbookS.Open(_asArchivo);

                Excel.Worksheet xlWorksheet = new Excel.Worksheet();

                string sValue = string.Empty;
                string sLine = string.Empty;
                string sTurno = string.Empty;

                int iSheets = xlWorkbook.Sheets.Count;
                

                string sShetName = string.Empty;
                
                if (cbbPlanta.SelectedValue.ToString() == "EMPF")
                {
                    while(sShetName.IndexOf("W") == -1)
                    {
                        xlWorksheet = xlWorkbook.Sheets[iSheets];
                        sShetName = xlWorksheet.Name;
                        if (sShetName.IndexOf("W") == -1)
                            iSheets--;
                        
                    }
                    
                    LineaLogica line = new LineaLogica();
                    line.Planta = cbbPlanta.SelectedValue.ToString();
                    DataTable dt = LineaLogica.LineaPlanta(line);
                    if (dt.Rows.Count > 0)
                        sLine = dt.Rows[0]["linea_nav"].ToString();
                    else
                        return;
                }
                else
                {
                    iSheets--;
                    xlWorksheet = xlWorkbook.Sheets[iSheets];
                }

                Excel.Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                int iCont = 0;

                for (int i = 1; i < rowCount; i++)
                {
                    DateTime dtFecha = DateTime.Today;
                    string sRPO = string.Empty;
                    string sModelo = string.Empty;
                    string sNota = string.Empty;
                    int iCant = 0;

                    if (cbbPlanta.SelectedValue.ToString() == "EMPF")
                    {
                        sRPO = string.Empty;
                        sModelo = string.Empty;
                        sNota = string.Empty;
                        iCant = 0;
                        sValue = string.Empty;

                        if (xlRange.Cells[i, 1].Value2 == null && xlRange.Cells[i, 2].Value2 == null)
                            continue;

                        if (xlRange.Cells[i, 1].Value2 != null)
                        {
                            sValue = Convert.ToString(xlRange.Cells[i, 1].Value2.ToString());
                        }

                        if (sValue == "RPO")
                        {
                            sValue = string.Empty;
                            continue;
                        }

                        if (sValue.IndexOf("RPO") == -1)
                        {
                            sValue = string.Empty;
                            continue;
                        }

                        sTurno = "1";

                        sRPO = Convert.ToString(xlRange.Cells[i, 1].Value2.ToString());
                        sRPO = sRPO.TrimStart().TrimEnd().ToUpper();

                        sModelo = string.Empty;
                        if (xlRange.Cells[i, 2].Value2 != null)
                            sModelo = Convert.ToString(xlRange.Cells[i, 2].Value2.ToString());

                        if (!int.TryParse(xlRange.Cells[i, 4].Value2.ToString(), out iCant))
                            iCant = 0;

                        if (xlRange.Cells[i, 20].Value2 != null)
                            sNota = Convert.ToString(xlRange.Cells[i, 20].Value2.ToString());
                        
                    }
                    else
                    {
                        if (xlRange.Cells[i, 2].Value2 == null && xlRange.Cells[i, 3].Value2 == null)
                            continue;

                        if (xlRange.Cells[i, 2].Value2 != null)
                        {
                            if (string.IsNullOrEmpty(sValue))
                                sValue = Convert.ToString(xlRange.Cells[i, 2].Value2.ToString());
                            
                        }


                        if (sValue.IndexOf("Turno") != -1)
                        {
                            sValue = string.Empty;
                            continue;
                        }


                        if (sValue.IndexOf("MX1APAC") != -1)
                        {
                            sLine = sValue;
                            sValue = string.Empty;
                            continue;
                        }

                        if (sValue == "FPP")
                        {
                            sValue = string.Empty;
                            continue;
                        }

                        if (sValue.IndexOf("FPP") != -1)
                        {
                            sValue = string.Empty;
                        }

                        if (xlRange.Cells[i, 3].Value2 == null)
                        {
                            
                            sValue = string.Empty;
                            continue;
                        }

                        sRPO = string.Empty;
                        sModelo = string.Empty;
                        sNota = string.Empty;
                        iCant = 0;

                        if (xlRange.Cells[i, 1].Value2 != null)
                            sTurno = Convert.ToString(xlRange.Cells[i, 1].Value2.ToString());

                        sRPO = Convert.ToString(xlRange.Cells[i, 3].Value2.ToString());
                        sRPO = sRPO.TrimStart().TrimEnd().ToUpper();

                        sModelo = string.Empty;
                        if (xlRange.Cells[i, 5].Value2 != null)
                            sModelo = Convert.ToString(xlRange.Cells[i, 5].Value2.ToString());

                        if (!int.TryParse(xlRange.Cells[i, 8].Value2.ToString(), out iCant))
                            iCant = 0;
                        if (iCant < 0)
                            iCant = 0;

                        if (xlRange.Cells[i, 12].Value2 != null)
                            sNota = Convert.ToString(xlRange.Cells[i, 12].Value2.ToString());
                    }

                    AgregaDato(sRPO, sModelo, dtFecha, sLine, iCant, sTurno, sNota);

                    iCont++;
                    lblCant.Text = iCont.ToString();
                }
                
                xlApp.DisplayAlerts = false;
                xlWorkbook.Close();
                xlApp.DisplayAlerts = true;
                xlApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja" + Environment.NewLine + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }
        private void LlenarGrid3(string _asArchivo)
        {
            try
            {
                dgwRpo.DataSource = null;
                CargarColFile();

                Excel.Application xlApp = new Excel.Application();
                Excel.Workbooks xlWorkbookS = xlApp.Workbooks;
                Excel.Workbook xlWorkbook = xlWorkbookS.Open(_asArchivo);

                Excel.Worksheet xlWorksheet = new Excel.Worksheet();
                int iSheets = xlWorkbook.Sheets.Count;
                xlWorksheet = xlWorkbook.Sheets[iSheets];

                string sValue = string.Empty;
                string sLine = string.Empty;
                string sTurno = string.Empty;

                

                if (cbbPlanta.SelectedValue.ToString() == "EMPF")
                {
                    LineaLogica line = new LineaLogica();
                    line.Planta = cbbPlanta.SelectedValue.ToString();
                    DataTable dt = LineaLogica.LineaPlanta(line);
                    if (dt.Rows.Count > 0)
                        sLine = dt.Rows[0]["linea_nav"].ToString();
                    else
                        return;
                }
                
                Excel.Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                int iCont = 0;

                for (int i = 1; i <= rowCount; i++)
                {
                    DateTime dtFecha = DateTime.Today;
                    string sRPO = string.Empty;
                    string sModelo = string.Empty;
                    string sNota = string.Empty;
                    int iCant = 0;

                  
                    sRPO = string.Empty;
                    sModelo = string.Empty;
                    sNota = string.Empty;
                    iCant = 0;
                    sValue = string.Empty;

                    if (xlRange.Cells[i, 1].Value2 == null && xlRange.Cells[i, 2].Value2 == null)
                        continue;

                    if (xlRange.Cells[i, 1].Value2 != null)
                    {
                        sValue = Convert.ToString(xlRange.Cells[i, 1].Value2.ToString());
                    }

                    if (sValue == "RPO")
                    {
                        sValue = string.Empty;
                        continue;
                    }

                    if (sValue.IndexOf("RPO") == -1)
                    {
                        sValue = string.Empty;
                        continue;
                    }

                    sTurno = "1";

                    sRPO = Convert.ToString(xlRange.Cells[i, 1].Value2.ToString());
                    sRPO = sRPO.TrimStart().TrimEnd().ToUpper();

                    if (!int.TryParse(xlRange.Cells[i, 4].Value2.ToString(), out iCant))
                        iCant = 0;

                    sModelo = string.Empty;
                    if (xlRange.Cells[i, 5].Value2 != null)
                        sModelo = Convert.ToString(xlRange.Cells[i, 5].Value2.ToString());

                    if (xlRange.Cells[i, 6].Value2 != null)
                        sLine = Convert.ToString(xlRange.Cells[i, 6].Value2.ToString());

                    if (xlRange.Cells[i, 7].Value2 != null)
                        sNota = Convert.ToString(xlRange.Cells[i, 7].Value2.ToString());

                    if (xlRange.Cells[i, 8].Value2 != null)
                        sTurno = Convert.ToString(xlRange.Cells[i, 8].Value2.ToString());

                    AgregaDato(sRPO, sModelo, dtFecha, sLine, iCant, sTurno, sNota);

                    iCont++;
                    lblCant.Text = iCont.ToString();
                }

                xlApp.DisplayAlerts = false;
                xlWorkbook.Close();
                xlApp.DisplayAlerts = true;
                xlApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja" + Environment.NewLine + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
       
        private void AgregaDato(string _asRPO, string _asModelo, DateTime _dtFecha,string _asLinea, int _aiCant, string _asTurno,string _asNota)
        {
            DataTable dt = dgwRpo.DataSource as DataTable;
            dt.Rows.Add(_asRPO,null,_asModelo,_dtFecha,_asLinea,_aiCant,_asTurno,cbbPlanta.SelectedValue.ToString(),_asNota);
        }
        private void LlenarGrid(string archivo, string hoja)
        {
            if (cbbCatalogo.SelectedValue.ToString() == "R" && cbbOrigen.SelectedValue.ToString() == "P")
            {
                if(chbParcial.Checked)
                {
                    LlenarGrid3(archivo);
                }
                else
                    LlenarGrid2(archivo);
                return;
            }
                
            
            //declaramos las variables         
            OleDbConnection conexion = null;
            DataSet dataSet = null;
            OleDbDataAdapter dataAdapter = null;

            string consultaHojaExcel = "Select * from [" + hoja + "$]";

            //esta cadena es para archivos excel 2007 y 2010
            string cadenaConexionArchivoExcel = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Mode=ReadWrite;Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";
            //Validamos que el usuario ingrese el nombre de la hoja del archivo de excel a leer
            if (string.IsNullOrEmpty(hoja))
            {
                MessageBox.Show("No hay una hoja para leer");
            }
            else
            {
                try
                {
                    conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
                    conexion.Open(); //abrimos la conexion
                    dataAdapter = new OleDbDataAdapter(consultaHojaExcel, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
                    dataSet = new DataSet(); // creamos la instancia del objeto DataSet
                    dataAdapter.TableMappings.Add("tbl", "Table");
                    dataAdapter.Fill(dataSet);//llenamos el dataset
                    DataTable table = dataSet.Tables[0];
                    dgwRpo.DataSource = table;

                    conexion.Close();//cerramos la conexion
                }
                catch (Exception ex)
                {
                    //en caso de haber una excepcion que nos mande un mensaje de error
                    MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja", ex.Message);
                }
            }
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtArchivo.Text) || string.IsNullOrWhiteSpace(txtArchivo.Text))
                return;

            if (dgwRpo.Rows.Count > 0)
            {
                Cursor = Cursors.WaitCursor;
                int iCont = 0;
                int iCant = 0;
                int iOmit = 0;
                int iMod = 0;

                try
                { 
                    string sModelo = string.Empty;
                    string sOperacion = string.Empty;

                    string sModeloAnt = string.Empty;
                    string sOperaAnt = string.Empty;
                    int iConsec = 0;
                    
                    foreach (DataGridViewRow row in dgwRpo.Rows)
                    {
                        #region regItems
                        if(cbbCatalogo.SelectedValue.ToString() =="I")
                        {
                            iCont++;
                            ItemsLogica item = new ItemsLogica();
                            item.Item = Convert.ToString(row.Cells[0].Value);
                            item.Modelo = Convert.ToString(row.Cells[1].Value);
                            item.Descrip = Convert.ToString(row.Cells[2].Value);

                            item.Usuario = GlobalVar.gsUsuario;
                            if (ItemsLogica.Guardar(item) == 0)
                            {
                                iOmit++;
                                MessageBox.Show("Error al intentar guardar el Item " + item.Item, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                continue;
                            }
                            iCant = iCont - iOmit;

                            lblCant.Text = iCont.ToString();
                        }
                        #endregion

                        #region regModelo
                        if (cbbCatalogo.SelectedValue.ToString() == "M")
                        {
                            if (row.Cells[1].Value == null || row.Cells[1].Value.ToString() == "" || row.Cells[1].Value.ToString() == "N/A")
                            {
                                sModelo = string.Empty;
                                sModeloAnt = string.Empty;

                                sOperacion = string.Empty;
                                sOperaAnt = string.Empty;

                                iConsec = 0;
                                iCant = 0;
                                continue;
                            }
                            //guarda modelo - encavezado -
                            if(string.IsNullOrEmpty(sModeloAnt) && row.Cells[0].Value != null)
                            {
                                ModeloLogica mod = new ModeloLogica();

                                sModelo = Convert.ToString(row.Cells[0].Value);

                                sModelo = sModelo.TrimStart();
                                sModelo = sModelo.TrimEnd();
                                sModelo = sModelo.ToUpper();

                                mod.Modelo = sModelo;
                                mod.Descrip = sModelo;
                                mod.Estatus = "A";
                                mod.CantGuia = 1;
                                mod.CantRetra = 1;
                                mod.Robot = "0";
                                mod.Usuario = GlobalVar.gsUsuario;
                                if (ModeloLogica.Verificar(mod))
                                    continue;
                                else
                                {
                                    ModeloLogica.Guardar(mod);
                                    iCont++;
                                }
                            }
                            sModeloAnt = sModelo;

                            if(!string.IsNullOrEmpty(row.Cells[1].Value.ToString()))
                            {
                                sOperacion = Convert.ToString(row.Cells[1].Value);
                                if (sOperacion == sOperaAnt)
                                {
                                    iCant++;
                                }
                                else
                                {
                                    iConsec++;
                                    iCant = 1;
                                }

                                sOperacion = sOperacion.TrimStart();
                                sOperacion = sOperacion.TrimEnd();
                                sOperacion = sOperacion.ToUpper();

                                ModestaLogica mode = new ModestaLogica();
                                mode.Modelo = sModelo;
                                mode.Consec = iConsec;
                                mode.Estacion = iConsec.ToString();
                                mode.Nombre = sOperacion;
                                mode.Operadores = iCant;
                                mode.Usuario = GlobalVar.gsUsuario;
                                ModestaLogica.Guardar(mode);

                                sOperaAnt = sOperacion;
                            }
                            
                        }
                        #endregion

                        #region regRPO
                        if (cbbCatalogo.SelectedValue.ToString() == "R")
                        {
                            string sRpo = Convert.ToString(row.Cells[0].Value);
                            if (string.IsNullOrEmpty(sRpo))
                                continue;

                            iConsec++;
                            RpoLogica rpo = new RpoLogica();
                            rpo.RPO = sRpo;
                            rpo.Descrip = Convert.ToString(row.Cells[1].Value);
                            rpo.Modelo = Convert.ToString(row.Cells[2].Value);
                            rpo.Origen = cbbOrigen.SelectedValue.ToString();

                            if (cbbOrigen.SelectedValue.ToString() == "P")
                            {
                                rpo.Planta = cbbPlanta.SelectedValue.ToString();
                                rpo.Fecha = Convert.ToDateTime(row.Cells[3].Value);
                                rpo.Linea = Convert.ToString(row.Cells[4].Value);
                                rpo.Cantidad = Convert.ToInt32(row.Cells[5].Value);
                                rpo.Turno = Convert.ToString(row.Cells[6].Value);
                                rpo.Nota = Convert.ToString(row.Cells[8].Value);
                            }
                            else
                            {
                                rpo.Linea = Convert.ToString(row.Cells[4].Value);
                                rpo.Cantidad = Convert.ToInt32(row.Cells[5].Value);
                                rpo.Planta = Convert.ToString(row.Cells[14].Value);

                                if (!string.IsNullOrEmpty(Convert.ToString(row.Cells[15].Value)))
                                    rpo.Fecha = Convert.ToDateTime(row.Cells[15].Value);
                            }

                            if (chbParcial.Checked)
                                rpo.Parcial = "1";
                            else
                                rpo.Parcial = "0";
                            rpo.OrdenCarga = iConsec;

                            if(rpo.Cantidad < 0)
                            {
                                MessageBox.Show("Error en la Cantidad del RPO " + sRpo, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                continue;
                            }
                            rpo.UltCons = 0;

                            if (RpoLogica.Guardar(rpo) == 0)
                            {
                                MessageBox.Show("Error al intentar guardar el RPO " + sRpo, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                continue;
                            }
                            else
                                iCont++;
                        }
                        #endregion

                        #region regOperador
                        if (cbbCatalogo.SelectedValue.ToString() == "O")
                        {
                            iCont++;

                            OperadorLogica oper = new OperadorLogica();
                            if (string.IsNullOrEmpty(Convert.ToString(row.Cells[0].Value)) || string.IsNullOrEmpty(Convert.ToString(row.Cells[1].Value)))
                                continue;

                            string sOperador = Convert.ToString(row.Cells[0].Value);
                            if (sOperador.IndexOf("A") > 0)
                                sOperador = sOperador.Substring(0, 6);

                            long lNoEmp = 0;
                            if (!long.TryParse(sOperador, out lNoEmp))
                                continue;

                            if (sOperador.Length < 6)
                                sOperador = sOperador.PadLeft(6, '0');

                            oper.Operador = sOperador;
                            oper.Nombre = Convert.ToString(row.Cells[1].Value);
                            if (!string.IsNullOrEmpty(Convert.ToString(row.Cells[2].Value)))
                            {
                                string sNivel = Convert.ToString(row.Cells[2].Value);
                                if (sNivel.IndexOf("PPH") != -1)
                                    sNivel = sNivel.Substring(10);
                                if (sNivel.IndexOf("GUIA") != -1)
                                    sNivel = "GUIA";
                                if (sNivel.IndexOf("NI") != -1) // NUEVO INGRESO - TRESS
                                    sNivel = "NI";
                                if (sNivel.IndexOf("AUX") != -1)
                                    sNivel = "AUX";
                                if (sNivel.IndexOf("MATERIALISTA") != -1)
                                    sNivel = "MAT";
                                if (sNivel.IndexOf("OP") != -1)
                                    sNivel = "OPG";
                                if (sNivel.IndexOf("TEMP") != -1)
                                    sNivel = "TEM";
                                if (sNivel.IndexOf("AUDITOR") != -1)
                                    sNivel = "AUD";
                                if (sNivel == "COMPONETES")
                                    sNivel = "COM";
                                if (sNivel.IndexOf("III Q") != -1)
                                    sNivel = "IIIQ";
                                if (sNivel.IndexOf("IV Q") != -1)
                                    sNivel = "IVQ";
                                oper.Nivel = sNivel;
                            }
                            
                            string sLinea = Convert.ToString(row.Cells[3].Value);
                            string sPlanta = string.Empty;

                            if (sLinea.IndexOf("CTNR") != -1)
                            {
                                sLinea = sLinea.Substring(4);

                                int iIdx = sLinea.IndexOf("A");
                                if (iIdx != -1)
                                    sLinea = sLinea.Substring(0, iIdx);

                                sLinea = "C" + sLinea;
                                sPlanta = "COL";
                            }

                            if (sLinea.IndexOf("TNR") != -1)
                            {
                                //FORMATO DE TRESS
                                sLinea = sLinea.Substring(3);

                                int iIdx = sLinea.IndexOf("A");
                                if (iIdx != -1)
                                    sLinea = sLinea.Substring(0, iIdx);

                                iIdx = sLinea.IndexOf("0"); // |- 01 -> 1
                                if (iIdx == 0)
                                    sLinea = sLinea.Substring(1);

                                int iLinea = 0;
                                if (Int32.TryParse(sLinea, out iLinea))
                                {
                                    if (iLinea < 47)
                                        sPlanta = "NIC3";
                                    else
                                        sPlanta = "NIC2";
                                }
                                else
                                {
                                    sPlanta = "NIC3"; //componentes 852-856

                                }
                            }  
                            else
                            {
                                //FORMATO ORBIS
                                if (sLinea.IndexOf("001") != -1)
                                    sPlanta = "TON1";
                                else
                                    sPlanta = "NIC3";
                            }

                            if (cbbPlanta.SelectedIndex != -1)
                                oper.Planta = cbbPlanta.SelectedValue.ToString();
                            else
                                oper.Planta = sPlanta;
                            oper.Linea = sLinea;
                            oper.Turno = Convert.ToString(row.Cells[4].Value);
                            string sActivo = Convert.ToString(row.Cells[5].Value);
                            if (sActivo == "S" || sActivo == "1")
                                oper.Activo = "1";
                            else
                                oper.Activo = "0";
                            DateTime dtFecha;
                            
                            if (!string.IsNullOrEmpty(Convert.ToString(row.Cells[6].Value)))
                            {
                                if (DateTime.TryParse(row.Cells[6].Value.ToString(), out dtFecha))
                                    oper.FechaIngre = Convert.ToDateTime(row.Cells[6].Value);
                            }
                                

                            oper.Usuario = GlobalVar.gsUsuario;

                            //MODIFICAR REG EXISTENTES
                            bool bExist = OperadorLogica.Verificar(oper);
                            if (!chbEdit.Checked && bExist)
                            {
                                iOmit++;
                                continue;
                            }
                            else
                            {
                                if (oper.Activo == "0")
                                {
                                    iOmit++;
                                    continue;
                                }    

                                DataTable dtOp = OperadorLogica.Consultar(oper);
                                if(dtOp.Rows.Count > 0)
                                {
                                    if (string.IsNullOrEmpty(oper.Planta))
                                        oper.Planta = dtOp.Rows[0]["planta"].ToString();
                                    if (string.IsNullOrEmpty(oper.Linea))
                                        oper.Linea = dtOp.Rows[0]["linea"].ToString();
                                    if (string.IsNullOrEmpty(oper.Nivel))
                                        oper.Nivel = dtOp.Rows[0]["nivel"].ToString();

                                    iMod++;
                                }
                                else
                                    iCant++;
                            }
                            
                            if (OperadorLogica.Guardar(oper) == 0)
                            {
                                MessageBox.Show("Error al intentar guardar el Operador " + oper.Operador, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                continue;
                            }
                            
                        }
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Cursor = Cursors.Default;
                    return;
                }

                Cursor = Cursors.Arrow;
                MessageBox.Show("La Importación se ha Completado. " + Environment.NewLine + "Registros Cargados: " + iCont.ToString() + Environment.NewLine + "Registros Omitidos: " + iOmit.ToString() + Environment.NewLine + "Registros Modificados: " + iMod.ToString() + Environment.NewLine + "Registros Nuevos: " + iCant.ToString() , Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                if(iCont > 0 && cbbCatalogo.SelectedValue.ToString() == "R" && cbbOrigen.SelectedValue.ToString() == "P")
                {
                    try
                    {
                        ControlRpoLogica rpo = new ControlRpoLogica();
                        rpo.Fecha = DateTime.Today;
                        rpo.Planta = cbbPlanta.SelectedValue.ToString();
                        rpo.Usuario = GlobalVar.gsUsuario;
                        if (chbParcial.Checked)
                            rpo.CargaParcial = "1";
                        else
                            rpo.CargaParcial = "0";

                        //CARGAR LOS RPO SUBIDOS POR PLANEACION
                        DataTable dt = ControlRpoLogica.CargarRPO(rpo);  
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Favor de Notificar al Administrador " + iCont.ToString() + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Cursor = Cursors.Default;
                        return;
                    }
                }
                Cursor = Cursors.Default;
                Close();
            }
            else
                MessageBox.Show("No se encontraron datos para importar", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbbCatalogo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(cbbCatalogo.SelectedValue.ToString() == "R")
            {
                lblOrigen.Visible = true;
                cbbOrigen.Visible = true;   
            }
            else
            {
                lblOrigen.Visible = false;
                cbbOrigen.Visible = false;
            }
                
        }

        private void chbParcial_CheckedChanged(object sender, EventArgs e)
        {
           
        }
    }
}
