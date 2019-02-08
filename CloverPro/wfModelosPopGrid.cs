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
    public partial class wfModelosPopGrid : Form
    {
        public string _lsProceso;
        public string _sClave; //formato layout
        public string _sModelo;
        public string _sIndStd;
        public string _sTipoCore;
        public string _sFile;
        private string _lsPlanta;
        private string _lsStdFile;
        private string _lsStdPath;
        
        public System.Data.DataTable _dtLista;
        
        public wfModelosPopGrid(string asProceso)
        {
            InitializeComponent();
            _lsProceso = asProceso;
        }

        private void txtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _sClave = null;
                Close();
            }

            if (e.KeyCode != Keys.Enter)
                return;
            
        }

        private void wfModelosPopGrid_Load(object sender, EventArgs e)
        {
            if(_lsProceso == "CAT050")
            {
                ImportarFormato(_sClave);

                if (dgwData.Rows.Count == 0)
                    Close();

                foreach(DataGridViewRow row in dgwData.Rows)
                {
                    string sModelo = _sModelo;
                    string sEstacion = Convert.ToString(row.Cells[2].Value).ToUpper();
                    string sNombre = Convert.ToString(row.Cells[3].Value);
                    int iOper = Convert.ToInt32(row.Cells[4].Value);
                    string sNivel = string.Empty;
                    string sCodigo = string.Empty;
                    string sNivReq = string.Empty;
                    bool bNew = true;

                    for (int x = 0; x < _dtLista.Rows.Count; x++)
                    {
                        string sEst = _dtLista.Rows[x][2].ToString().ToUpper();
                        string sNom = _dtLista.Rows[x][3].ToString().ToUpper();
                        sNivel = _dtLista.Rows[x][5].ToString();
                        sCodigo = _dtLista.Rows[x][6].ToString();
                        sNivReq = _dtLista.Rows[x][7].ToString();

                        if (sNombre == sNom)
                        {
                            bNew = false;
                            x = _dtLista.Rows.Count;
                        }
                    }
                    if(bNew)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        row.Cells[5].Value = sNivel;
                        row.Cells[6].Value = sCodigo;
                        row.Cells[7].Value = sNivReq;
                    }
                }
            }
            
            
        }

        private void CargarModelos()
        {
            try
            {
                ModestaLogica mod = new ModestaLogica();
                mod.Modelo = _sModelo;
                _dtLista = ModestaLogica.Listar(mod);

                ModeloLogica mode = new ModeloLogica();
                mode.Modelo = _sModelo;
                System.Data.DataTable data = ModeloLogica.Consultar(mode);
                if(data.Rows.Count > 0)
                {
                    _lsPlanta = data.Rows[0]["planta"].ToString();
                    _lsStdFile = data.Rows[0]["formato_std"].ToString();
                    _lsStdPath = data.Rows[0]["formstd_path"].ToString();
                }

                ColumnasGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            
        }

        private void ImportarFormato(string _asModelo)
        {
            try
            {
                CargarModelos();

                System.Data.DataTable data = new System.Data.DataTable();

                Microsoft.Office.Interop.Excel.Application exApp = new Microsoft.Office.Interop.Excel.Application();
                Workbook exLibro;
                Worksheet exHoja;
                Range exRango;

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Archivos de Excel (*.xls;*.xlsx)|*.xls;*.xlsx";

                dialog.Title = "Seleccione el archivo de Excel";

                dialog.InitialDirectory = _lsStdPath;
                dialog.FileName = _lsStdFile;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    _sFile = dialog.FileName;

                    exLibro = exApp.Workbooks.Open(_sFile);
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
                            if (exRango.Cells[i, 3].Value == null)//ACTIVIDADES
                                break;
                            else
                            {
                                if (string.IsNullOrEmpty(sEstAnt))
                                    continue;
                            }
                            
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

                        string sDescrip = exRango.Cells[i, 2].Value.ToString();
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

                            AgregarFila(0, sEstacion, sDescrip.ToUpper(), iCantOp, null, null,null);
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
                                            AgregarFila(0, sEst, sDescrip.ToUpper(), 1, null, null,null);

                                        sEstacion = sEstacion.Substring(iPos + 1).TrimStart();
                                    }
                                }
                            }
                        }
                    }

                   
                    exLibro.Close(true);
                    exApp.Quit();

                    //ListarRelacionados(sNota);

                    Cursor = Cursors.Default;
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "Archivo sin formato Estandar" + Environment.NewLine + "ImportarFormato(" + _asModelo + ") ..." + ex.ToString(), "ERROR " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cursor = Cursors.Default;
                return;
            }
        }
        private void ColumnasGrid()
        {
            int iRows = dgwData.Rows.Count;
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
                
                dgwData.DataSource = dtNew;
            }

            dgwData.Columns[0].Visible = false;
            dgwData.Columns[1].Visible = false;
            dgwData.Columns[7].Visible = false;
            

            dgwData.Columns[2].Width = ColumnWith(dgwData, 12);
            dgwData.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[2].ReadOnly = true;

            dgwData.Columns[3].Width = ColumnWith(dgwData, 56);
            dgwData.Columns[3].ReadOnly = true;

            dgwData.Columns[4].Width = ColumnWith(dgwData, 9);
            dgwData.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[4].ReadOnly = true;

            dgwData.Columns[5].Width = ColumnWith(dgwData, 8);
            dgwData.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[6].Width = ColumnWith(dgwData, 12);
            
            
        }
        private void AgregarFila(int _aiCons, string _asEstacion, string _asNombre, int _aiCant, string _asNivel, string _asCodigo, string _asNivReq)
        {
            if (dgwData.RowCount == 0)
            {
                System.Data.DataTable dt = dgwData.DataSource as System.Data.DataTable;
                dt.Rows.Add(null, _aiCons, _asEstacion, _asNombre, _aiCant, _asNivel, _asCodigo, _asNivReq);
            }
            else
            {
                int iIdx = dgwData.RowCount - 1;
                if (string.IsNullOrEmpty(dgwData.Rows[iIdx].Cells[3].Value.ToString()))
                {
                    dgwData.Rows[iIdx].Cells[0].Value = null;
                    dgwData.Rows[iIdx].Cells[1].Value = 0;
                    dgwData.Rows[iIdx].Cells[2].Value = _asEstacion;
                    dgwData.Rows[iIdx].Cells[3].Value = _asNombre;
                    dgwData.Rows[iIdx].Cells[4].Value = _aiCant;
                    dgwData.Rows[iIdx].Cells[5].Value = _asNivel;
                    dgwData.Rows[iIdx].Cells[6].Value = _asCodigo;
                    dgwData.Rows[iIdx].Cells[7].Value = _asNivReq;
                    
                }
                else
                {
                    System.Data.DataTable dt = dgwData.DataSource as System.Data.DataTable;
                    dt.Rows.Add(null, _aiCons, _asEstacion, _asNombre, _aiCant, _asNivel, _asCodigo, _asNivReq);
                }
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

        private void dgwData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /*
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightSkyBlue;
            else
                e.CellStyle.BackColor = Color.White;

            if (e.Value != null)
            {
                e.Value = e.Value.ToString().ToUpper();
                e.FormattingApplied = true;
            }*/
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _dtLista.Reset();

            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (dgwData.Rows.Count > 0)
            {
                foreach(DataGridViewRow row in dgwData.Rows)
                {
                    string sEstacion = row.Cells[2].Value.ToString();
                    string sNivel = row.Cells[5].Value.ToString();

                    if (string.IsNullOrEmpty(sNivel))
                    {
                        MessageBox.Show("Favor de indicar el Nivel PPH requerdio para la estación " + sEstacion , Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgwData.CurrentCell = row.Cells[5];
                        return;
                    }
                }

                _dtLista = dgwData.DataSource as System.Data.DataTable;
            }
            else
                _dtLista.Reset();
            
            Close();
        }

        private void fbEsc_Click(object sender, EventArgs e)
        {
            btnCancel_Click(sender, e);

        }

        private void fbSave_Click(object sender, EventArgs e)
        {
            btnAceptar_Click(sender, e);

        }

        private void dgwData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(sender, e);
            }

            if (e.KeyCode == Keys.F4)
            {
                btnAceptar_Click(sender, e);
            }

            if (e.KeyCode == Keys.F5)
            {
                int iRow = dgwData.CurrentCell.RowIndex;
                if (dgwData.CurrentRow.Index == -1)
                    return;

                AsignarPPH(iRow);
            }
        }

        private void AsignarPPH(int _aiRow)
        {
            string sNivel = string.Empty;
            string sCodigo = string.Empty;

            string sOperacion = dgwData[3, _aiRow].Value.ToString();
            bool bExist = false;

            NivelDetLogica niv = new NivelDetLogica();
            niv.Planta = _lsPlanta;
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
                        niv.Operacion = sOpe1;
                        dtN = NivelDetLogica.ConsultaOperaciones(niv);
                        if (dtN.Rows.Count != 0)
                            bExist = true;
                    }

                }

            }

            if(bExist)
            {
                sNivel = dtN.Rows[0]["nivel"].ToString();
                sCodigo = dtN.Rows[0]["codigo"].ToString();

                dgwData[5, _aiRow].Value = sNivel;
                dgwData[6, _aiRow].Value = sCodigo;
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
                        if (_lsPlanta == "MON")
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
    }
}
