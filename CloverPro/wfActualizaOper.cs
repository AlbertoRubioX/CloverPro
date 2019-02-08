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
using Logica;

namespace CloverPro
{
    public partial class wfActualizarOper : Form
    {
        private string _lsProceso = "CAT080";
        private string _lsOperador;
        public bool _lbCambio;
        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;
        public wfActualizarOper()
        {
            InitializeComponent();

            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }

        private bool Valida()
        {
            bool bValida = false;

            if(chbPlanta.Checked && cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se ha especificado la Planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                //return bValida;
            }

            if (chbLinea.Checked)
            {
                if(cbbLinea.SelectedIndex == -1 && string.IsNullOrEmpty(cbbLinea.Text))
                {
                    MessageBox.Show("No se ha especificado la Linea", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbbLinea.Focus();
                    //return bValida;
                }
                
            }

            if (chbTurno.Checked && cbbTurno.SelectedIndex == -1)
            {
                MessageBox.Show("No se ha especificado el Turno", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbTurno.Focus();
                return bValida;
            }
            /*
            if (chbNivel.Checked && cbbNivel.SelectedIndex == -1)
            {
                MessageBox.Show("No se ha especificado el Nivel", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbNivel.Focus();
                return bValida;
            }*/

            if (chbEmpleado.Checked && string.IsNullOrEmpty(txtEmpleado.Text))
            {
                MessageBox.Show("No se ha especificado el Empleado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmpleado.Focus();
                return bValida;
            }
            return true;
        }

        private void wfActualizarOper_Activated(object sender, EventArgs e)
        {
            cbbPlanta.Focus();
        }
        private void wfActualizarOper_Load(object sender, EventArgs e)
        {
            Inicio();

            cbbPlanta.Focus();

            CargarColumnas();
        }

        private void Inicio()
        {
            chbPlanta.Checked = true;

            cbbPlanta.ResetText();
            cbbPlanta.DataSource = PlantaLogica.Listar();
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.SelectedValue = GlobalVar.gsPlanta;

            chbLinea.Checked = false;
            cbbLinea.ResetText();
            cbbLinea.Enabled = false;
            LineaLogica lin = new LineaLogica();
            lin.Planta = GlobalVar.gsPlanta;
            cbbLinea.DataSource = LineaLogica.Listar(lin);
            cbbLinea.ValueMember = "linea";
            cbbLinea.DisplayMember = "linea";
            cbbLinea.SelectedIndex = -1;

            chbTurno.Checked = true;
            cbbTurno.Enabled = true;
            cbbTurno.ResetText();
            Dictionary<string, string> Tipo = new Dictionary<string, string>();
            Tipo.Add("1", "1");
            Tipo.Add("2", "2");
            cbbTurno.DataSource = new BindingSource(Tipo, null);
            cbbTurno.DisplayMember = "Value";
            cbbTurno.ValueMember = "Key";
            cbbTurno.SelectedValue = GlobalVar.gsTurno;

            chbNivel.Checked = false;
            cbbNivel.Enabled = false;
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
            cbbNivel.SelectedIndex = -1;

            chbEmpleado.Checked = false;
            txtEmpleado.Enabled = false;
            txtEmpleado.Clear();
            rbtTress.Checked = false;
            txtArchivo.Clear();

            rbtOrbis.Checked = true;

            dgwData.DataSource = null;
            dgwBase.DataSource = null;
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Inicio();
        }
        private void btnFile_Click(object sender, EventArgs e)
        {

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

                sHoja = "PERSONAL LINE-UP";
                LlenarGrid(txtArchivo.Text, sHoja);


                Cursor = Cursors.Arrow;
                btnImportar.Enabled = true;
            }
        }

        #region regDetalle
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
            int iRows = dgwData.Rows.Count;
            if (iRows == 0)
            {
                System.Data.DataTable dtNew = new System.Data.DataTable("Empleados");
                dtNew.Columns.Add("NO. EMPLEADO", typeof(string));
                dtNew.Columns.Add("NOMBRE DEL EMPLEADO", typeof(string));
                dtNew.Columns.Add("PLANTA", typeof(string));
                dtNew.Columns.Add("NIVEL", typeof(string));
                dtNew.Columns.Add("LINEA", typeof(string));
                dtNew.Columns.Add("TURNO", typeof(string));
                dtNew.Columns.Add("activo", typeof(string));
                dtNew.Columns.Add("INGRESO", typeof(DateTime));
                dgwData.DataSource = dtNew;
            }

            dgwData.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            dgwData.Columns[6].Visible = false;

            dgwData.Columns[0].Width = ColumnWith(dgwData, 10);
            dgwData.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[1].Width = ColumnWith(dgwData, 40);
            dgwData.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgwData.Columns[2].Width = ColumnWith(dgwData, 10);
            dgwData.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[3].Width = ColumnWith(dgwData, 10);
            dgwData.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[4].Width = ColumnWith(dgwData, 10);
            dgwData.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[5].Width = ColumnWith(dgwData, 8);
            dgwData.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[7].Width = ColumnWith(dgwData, 10);
            dgwData.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[7].DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" };
        }
        #endregion   

        #region regActualizar
        private bool ObtenerNivelTress(int _aiIdx)
        {
            string sEmp = string.Empty;
            string sNivel = string.Empty;
            string sActivo = string.Empty;
            DateTime dtFecha = DateTime.Now;

            foreach (DataGridViewRow row in dgwBase.Rows)
            {
                if (string.IsNullOrEmpty(Convert.ToString(row.Cells[0].Value)))
                    continue;

                if (string.IsNullOrEmpty(Convert.ToString(row.Cells[2].Value)))
                    continue;

                string sOperador = Convert.ToString(row.Cells[0].Value);
                if (sOperador.IndexOf("A") > 0)
                    sOperador = sOperador.Substring(0, 6);

                long lNoEmp = 0;
                if (!long.TryParse(sOperador, out lNoEmp))
                    continue;

                if (sOperador.Length < 6)
                    sOperador = sOperador.PadLeft(6, '0');

                
                sEmp = dgwData[0, _aiIdx].Value.ToString();
                if (sEmp != sOperador)
                    continue;

                sNivel = Convert.ToString(row.Cells[2].Value);
                sActivo = Convert.ToString(row.Cells[3].Value);
                if (sActivo.ToUpper() == "S")
                    sActivo = "1";
                else
                    sActivo = "0";

                if (string.IsNullOrWhiteSpace(sNivel))
                    return false;
                else
                {
                    sNivel = sNivel.TrimStart().TrimEnd();
                    if (sNivel.IndexOf("PPH") != -1)
                        sNivel = sNivel.Substring(10).TrimStart();
                    if (sNivel.IndexOf("PTA") != -1)
                        sNivel = sNivel.Substring(0,sNivel.IndexOf("PTA") - 1);
                    if (sNivel.IndexOf("NI") != -1) // NUEVO INGRESO - TRESS
                        sNivel = "NI";
                    if (sNivel.IndexOf("AUX") != -1)
                        sNivel = "AUX";
                    if (sNivel.IndexOf("GUIA") != -1)
                        sNivel = "GUIA";
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

                    //CARGAR DATOS AL LISTADO
                    dgwData[3, _aiIdx].Value = sNivel;
                    dgwData[6, _aiIdx].Value = sActivo;
                    if (!string.IsNullOrEmpty(row.Cells[4].Value.ToString()))
                        dgwData[7, _aiIdx].Value = Convert.ToDateTime(row.Cells[4].Value);
                    
                    return true;
                }
            }
            return false;
        }

        private string ConvertNivel3(string _asNivel)
        {
            string sNivel = _asNivel;

            sNivel = sNivel.TrimStart().TrimEnd();
            if (sNivel.IndexOf("PPH") != -1)
                sNivel = sNivel.Substring(10).TrimStart();
            if (sNivel.IndexOf("PTA") != -1)
                sNivel = sNivel.Substring(0, sNivel.IndexOf("PTA") - 1);
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

            return sNivel;
        }
        private void ActualizaTress()
        {
            //if (string.IsNullOrEmpty(txtArchivo.Text) || string.IsNullOrWhiteSpace(txtArchivo.Text))
            //{
            //    MessageBox.Show("No se ha especificadao el Archivo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    btnFile.Focus();
            //    return;
            //}

            try
            {
                //if(dgwBase.Rows.Count == 0)
                //{
                //    MessageBox.Show("Archivo sin información", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                string sModelo = string.Empty;
                string sOperacion = string.Empty;

                string sModeloAnt = string.Empty;
                string sOperaAnt = string.Empty;
                int iCont = 0;
                string sLista = string.Empty;
                foreach(DataGridViewRow row in dgwData.Rows)
                {
                    OperadorLogica oper = new OperadorLogica();

                    
                    oper.Operador = row.Cells[0].Value.ToString();
                    oper.Nombre = row.Cells[1].Value.ToString();
                    oper.Planta = row.Cells[2].Value.ToString();       
                    oper.Linea = row.Cells[4].Value.ToString();
                    oper.Turno = row.Cells[5].Value.ToString();
                
                    //OBTENER NIVEL DESDE TRESS
                    //if(!ObtenerNivelTress(row.Index))
                    //    continue;

                    TressLogica tress = new TressLogica();
                    tress.Codigo = int.Parse(oper.Operador);
                    DataTable dt3 = TressLogica.ConsultaNivelOper(tress);
                    if (dt3.Rows.Count == 0)
                        continue;
                    else
                    {
                        string sNivel = dt3.Rows[0][1].ToString();

                        sNivel = ConvertNivel3(sNivel);

                        string sAct = dt3.Rows[0][3].ToString();
                        if (sAct == "S")
                            sAct = "1";
                        else
                            sAct = "0";

                        oper.Nivel = sNivel;
                        oper.Activo = sAct;
                        oper.FechaIngre = Convert.ToDateTime(dt3.Rows[0][31].ToString());

                        oper.Usuario = GlobalVar.gsUsuario;

                        _lsOperador = oper.Operador;

                        if (OperadorLogica.Guardar(oper) != 0)
                            iCont++;
                        else
                        {
                            MessageBox.Show("Error al intentar guardar el Nivel del Operador " + oper.Operador + Environment.NewLine + sLista, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            continue;
                        }
                    }
                    
                }
                if(iCont != 0)
                    MessageBox.Show("Cantidad de Registros Actualizados: " + iCont.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + _lsOperador + " - " +  ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
                return;
            }
        }

        private void ActualizarOrbis()
        {
            try
            {
                string sModelo = string.Empty;
                string sOperacion = string.Empty;

                string sModeloAnt = string.Empty;
                string sOperaAnt = string.Empty;
                int iCont = 0;
                foreach (DataGridViewRow row in dgwData.Rows)
                {
                    bool bCambio = false;
                    OperadorLogica oper = new OperadorLogica();

                    string sPlanta = string.Empty;
                    string sLinea = string.Empty;
                    string sTurno = string.Empty;
                    string sActivo = string.Empty;

                    oper.Operador = row.Cells[0].Value.ToString();
                    oper.Nombre = row.Cells[1].Value.ToString();
                    if (!string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
                        oper.Nivel = row.Cells[3].Value.ToString();
                    oper.Turno = row.Cells[5].Value.ToString();
                    if(!string.IsNullOrEmpty(row.Cells[7].Value.ToString()))
                        oper.FechaIngre = Convert.ToDateTime(row.Cells[7].Value.ToString());
                    if (!string.IsNullOrEmpty(row.Cells[2].Value.ToString()))
                        sPlanta = row.Cells[2].Value.ToString();
                    if (!string.IsNullOrEmpty(row.Cells[4].Value.ToString()))
                        sLinea = row.Cells[4].Value.ToString();
                    sTurno = row.Cells[5].Value.ToString();
                    sActivo = "1";
                    //OBTENER PLANTA - LINEA - TURNO DESDE ORBIS
                    string sPlantaOr = string.Empty;
                    string sLineaOr = string.Empty;
                    string sTurnoOr = string.Empty;
                    string sActivoOr = string.Empty;

                    DataTable dtOrbis = OperadorLogica.ConsultarOrbis(oper);
                    if (dtOrbis.Rows.Count == 0)
                        continue;
                    else
                    {

                        sPlantaOr = dtOrbis.Rows[0]["idPlanta"].ToString();
                        sLineaOr = dtOrbis.Rows[0]["Linea"].ToString();
                        sTurnoOr = dtOrbis.Rows[0]["idTurno"].ToString();
                        sActivoOr = dtOrbis.Rows[0]["estatus"].ToString();

                        if (sPlantaOr == "4")
                        {
                            sLineaOr = sLineaOr.Substring(4);
                            sLineaOr = sLineaOr.PadLeft(2, '0');
                            sLineaOr = "C" + sLineaOr;
                        }
                        if (sLineaOr.IndexOf("TNR") != -1)
                        {
                            sLineaOr = sLineaOr.Substring(3);
                            if (sLineaOr.IndexOf("A") != -1)
                                sLineaOr = sLineaOr.Substring(0,sLineaOr.IndexOf("A"));
                            if(sLineaOr.IndexOf("0") == 0)
                                sLineaOr = sLineaOr.Substring(1);
                        }

                        if (!string.IsNullOrEmpty(sPlantaOr))
                        {
                            if (sPlantaOr == "1")
                                sPlantaOr = "TON1";

                            if (sPlantaOr == "4")
                                sPlantaOr = "COL";
                            else
                            {
                                int iLinea = 0;
                                if(int.TryParse(sLineaOr, out iLinea))
                                {
                                    if (iLinea > 44)
                                        sPlantaOr = "NIC2";
                                    else
                                        sPlantaOr = "NIC3";
                                }
                                else
                                    sPlantaOr = "NIC3";
                            }
                        }

                        if (sActivoOr == "1" || sActivoOr == "True")
                            sActivoOr = "1";
                        else
                            sActivoOr = "0";
                    }

                    if (sPlanta != sPlantaOr)
                        bCambio = true;
                    if (sLinea != sLineaOr)
                        bCambio = true;
                    if (sTurno != sTurnoOr)
                        bCambio = true;
                    if (sActivo != sActivoOr)
                        bCambio = true;

                    if (!bCambio)
                        continue;

                    oper.Planta = sPlantaOr;
                    oper.Linea = sLineaOr;
                    oper.Turno = sTurnoOr;
                    oper.Activo = sActivoOr;

                    oper.Usuario = GlobalVar.gsUsuario;

                    if (OperadorLogica.Guardar(oper) != 0)
                        iCont++;
                    else
                    {
                        MessageBox.Show("Error al intentar guardar el Nivel del Operador " + Environment.NewLine + "Clave: " + oper.Operador, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        continue;
                    }
                }
                if(iCont != 0)
                    MessageBox.Show("Cantidad de Registros Actualizados: " + iCont.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "Error " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            //if (dgwData.Rows.Count == 0)
            //    return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (rbtTress.Checked)
                ActualizaTress();
            else
                ActualizarOrbis();
            /*
            BuscarListado();
            CargarColumnas();*/

            Cursor = Cursors.Default;
        }

        #endregion

        #region regCaptura
        private void txtEmpleado_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtEmpleado.Text) || string.IsNullOrWhiteSpace(txtEmpleado.Text))
                return;

            OperadorLogica oper = new OperadorLogica();
            string sOperador = txtEmpleado.Text.ToString();

            if (sOperador.IndexOf("A") > 0)
                sOperador = sOperador.Substring(0, 6);

            if (sOperador.Length < 6)
                sOperador = sOperador.PadLeft(6, '0');

            oper.Operador = sOperador;
            DataTable datos = OperadorLogica.Consultar(oper);
            if (datos.Rows.Count != 0)
            {
                txtEmpleado.Text = datos.Rows[0]["empleado"].ToString();
            }
            else
            {
                MessageBox.Show("El Empleado no se encuentra Registrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmpleado.Clear();
                txtEmpleado.Focus();
                return;
            }
        }
        private void chbPlanta_CheckedChanged(object sender, EventArgs e)
        {
            cbbPlanta.Enabled = chbPlanta.Checked;
        }

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cbbLinea.ResetText();
            LineaLogica lin = new LineaLogica();
            lin.Planta = cbbPlanta.SelectedValue.ToString();
            cbbLinea.DataSource = LineaLogica.Listar(lin);
            cbbLinea.ValueMember = "linea";
            cbbLinea.DisplayMember = "linea";
            cbbLinea.SelectedIndex = -1;
        }

        private void chbLinea_CheckedChanged(object sender, EventArgs e)
        {
            cbbLinea.Enabled = chbLinea.Checked;
        }

        private void chbTurno_CheckedChanged(object sender, EventArgs e)
        {
            cbbTurno.Enabled = chbTurno.Checked;
        }

        private void chbEmpleado_CheckedChanged(object sender, EventArgs e)
        {
            txtEmpleado.Enabled = chbEmpleado.Checked;
        }

        private void rbtTress_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtTress.Checked)
                rbtOrbis.Checked = false;
        }

        private void rbtOrbis_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtOrbis.Checked)
                rbtTress.Checked = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            cbbNivel.Enabled = chbNivel.Checked;
        }
        #endregion

        #region regBuscar
        private void LlenarGrid(string archivo, string hoja)
        {
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
                    dgwBase.DataSource = table;

                    conexion.Close();//cerramos la conexion
                }
                catch (Exception ex)
                {
                    //en caso de haber una excepcion que nos mande un mensaje de error
                    MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja", ex.Message);
                }
            }
        }
        private void BuscarListado()
        {
            ReportesLogica mon = new ReportesLogica();
            if (chbPlanta.Checked)
            {
                mon.IndPlanta = "1";
                if (cbbPlanta.SelectedIndex != -1)
                    mon.Planta = cbbPlanta.SelectedValue.ToString();
            }
            else
                mon.IndPlanta = "0";

            if (chbLinea.Checked)
            {
                mon.IndLinea = "1";
                if (cbbLinea.SelectedIndex != -1)
                    mon.LineaIni = cbbLinea.SelectedValue.ToString();
                else
                    mon.LineaIni = cbbLinea.Text.ToString();
            }
            else
                mon.IndLinea = "0";

            if (chbTurno.Checked)
            {
                mon.IndTurno = "1";
                mon.Turno = cbbTurno.SelectedValue.ToString();
            }
            else
                mon.IndTurno = "0";

            if (chbEmpleado.Checked)
            {
                mon.IndEmp = "1";
                mon.Empleado = txtEmpleado.Text.ToString();
            }
            else
                mon.IndEmp = "0";

            if (chbNivel.Checked)
            {
                mon.IndNivel = "1";
                if (cbbNivel.SelectedIndex != -1)
                    mon.Nivel = cbbNivel.SelectedValue.ToString();
                else
                    mon.Nivel = cbbNivel.Text.ToString();
            }
            else
                mon.IndNivel = "0";


            dgwData.DataSource = ReportesLogica.MonitorEmpleados(mon);
            lblCant.Text = dgwData.Rows.Count.ToString();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            BuscarListado();

            CargarColumnas();
        }
        #endregion

        #region regResize
        private void wfActualizarOper_Resize(object sender, EventArgs e)
        {
            if (WindowState != _WindowStateAnt && WindowState != FormWindowState.Minimized)
            {
                _WindowStateAnt = WindowState;
                ResizeControl(panel1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(groupBox9, 1, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(dgwData, 1, ref _iWidthAnt, ref _iHeightAnt, 1);

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

        private void btnHelp_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable dt = ConfigLogica.Consultar();
                string sDirec = dt.Rows[0]["directorio"].ToString();
                if (!string.IsNullOrEmpty(sDirec))
                {
                    sDirec += @"\CloverPRO_ManualActualizaEmp.pdf";
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

        private void cbbPlanta_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
