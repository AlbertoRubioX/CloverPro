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
using Datos;

namespace CloverPro
{
    public partial class wfLineCapacity : Form
    {
        public bool _lbCambio;
        private bool _lbCambioDet;
        public string _lsParam;
        private string _lsProceso = "PRO100";
        private string _lsFolioAnt;
        private string _lsTurno;
        private string _lsArea;
        private string _lsPlantaAnt;
        private string _lsLineaAnt;
        private int _liPartida = 0;
        private double _ldRampeo=0;

        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;
        
        public wfLineCapacity()
        {
            InitializeComponent();
            
            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }

        #region regInicio
        private void wfLineCapacity_Load(object sender, EventArgs e)
        {
            _lsTurno = GlobalVar.gsTurno;


            cbbPlanta.ResetText();
            DataTable dt = PlantaLogica.Listar();
            cbbPlanta.DataSource = dt;
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.SelectedValue = -1;

            Inicio();

            WindowState = FormWindowState.Maximized;
        }

        private void Inicio()
        {
            txtFolio.Clear();

            _liPartida = 0;

            DataTable dtC = ConfigLogica.Consultar();
            _ldRampeo = double.Parse(dtC.Rows[0]["rampeo_hc"].ToString());

            UsuarioLogica user = new UsuarioLogica();
            user.Usuario = GlobalVar.gsUsuario;
           
            dgwEstaciones.DataSource = null;
            dgwStd1.DataSource = null;
            dgwStd2.DataSource = null;
            dgwReal1.DataSource = null;
            dgwReal2.DataSource = null;

            CargarColumnas();
            CargarColumnasStd();

            _lsFolioAnt = string.Empty;
            _lsPlantaAnt = string.Empty;
            _lsLineaAnt = string.Empty;
            
            _lbCambio = false;
            _lbCambioDet = false;

        }

        private void wfLineCapacity_Activated(object sender, EventArgs e)
        {
            cbbPlanta.Focus();
        }
        #endregion

        #region regGuardar
        private bool Valida()
        {
            bool bValida = false;

            if (!_lbCambio && !_lbCambioDet)
                return bValida;

            if (cbbPlanta.SelectedIndex == -1)
                return bValida;

            if (dgwEstaciones.Rows.Count == 0)
                return bValida;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "10") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bValida;
            }
            
            return true;    
        }
        private bool Guardar()
        {
            try
            {
                if (!Valida())
                    return false;

                Cursor = Cursors.WaitCursor;
                LineCapacityLogica line = new LineCapacityLogica();
                line.Planta = cbbPlanta.SelectedValue.ToString();
                line.Fecha = dtpFecha.Value;
                /*
                if (string.IsNullOrEmpty(txtFolio.Text))
                {
                    long lFolio = LineCapacityLogica.VerificarPta(line);
                    if(lFolio > 0)
                    {
                        line.Folio = lFolio;
                        _liPartida = LineCapacityLogica.VerificarPartida(line);
                    }
                    else
                        line.Folio = AccesoDatos.Consec(_lsProceso);
                }
                else
                    line.Folio = Convert.ToInt32(txtFolio.Text);

                */

                if (string.IsNullOrEmpty(txtFolio.Text))
                    line.Folio = AccesoDatos.Consec(_lsProceso);
                else
                    line.Folio = Convert.ToInt32(txtFolio.Text);

                line.Partida = _liPartida;
                line.Rampeo = _ldRampeo;
                line.Usuario = GlobalVar.gsUsuario;

                
                if (LineCapacityLogica.Guardar(line) > 0)
                {
                    /*
                    dtNew.Columns.Add("LINEA"  //0
                    dtNew.Columns.Add("MODELO" //1
                    dtNew.Columns.Add("STD 1T" //2
                    dtNew.Columns.Add("STD 2T" //3
                    dtNew.Columns.Add("STD HC" //4
                    dtNew.Columns.Add("hc_tress1" //5
                    dtNew.Columns.Add("hc_tress2" //6
                    dtNew.Columns.Add("TURNO 1" //7
                    dtNew.Columns.Add("TURNO 2" //8
                    dtNew.Columns.Add("linea" //9
                    dtNew.Columns.Add("tipo_linea" //10
                    
                    */
                    //CAMBIOS POR RPO
                    foreach (DataGridViewRow row in dgwEstaciones.Rows)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(row.Cells[1].Value.ToString()))
                                continue;

                            LineCapaDetLogica lined = new LineCapaDetLogica();
                            lined.Folio = line.Folio;
                            lined.Partida = _liPartida;
                            int iCons = 0;/*
                            if (!int.TryParse(row.Cells[1].Value.ToString(), out iCons))
                                iCons = 0;*/
                            lined.Consec = iCons;
                            lined.Planta = cbbPlanta.SelectedValue.ToString();
                            lined.Linea = row.Cells[0].Value.ToString();
                            lined.Modelo = row.Cells[1].Value.ToString().ToUpper();
                            double dCant = 0;
                            if (!double.TryParse(row.Cells[2].Value.ToString(), out dCant))
                                dCant = 0;
                            lined.Std1 = dCant;

                            dCant = 0;
                            if (!double.TryParse(row.Cells[3].Value.ToString(), out dCant))
                                dCant = 0;
                            lined.Std2 = dCant;
                            dCant = 0;
                            if (!double.TryParse(row.Cells[4].Value.ToString(), out dCant))
                                dCant = 0;
                            lined.Hc1 = dCant;
                            lined.Hc2 = dCant;

                            dCant = 0;
                            if (!double.TryParse(row.Cells[5].Value.ToString(), out dCant))
                                dCant = 0;
                            lined.HcTress1 = dCant;
                            dCant = 0;
                            if (!double.TryParse(row.Cells[6].Value.ToString(), out dCant))
                                dCant = 0;
                            lined.HcTress2 = dCant;
                            string sActiva = "1";
                            if (!bool.Parse(row.Cells[7].Value.ToString()))
                                sActiva = "0";
                            lined.Turno1 = sActiva;
                            sActiva = "1";
                            if (!bool.Parse(row.Cells[8].Value.ToString()))
                                sActiva = "0";
                            lined.Turno2 = sActiva;
                            lined.TipoLinea = row.Cells[10].Value.ToString();
                            
                            lined.Usuario = GlobalVar.gsUsuario;

                            LineCapaDetLogica.Guardar(lined);
                        }
                        catch (Exception ie)
                        {
                            Cursor = Cursors.Default;
                            MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "LineCapaDetLogica.Guardar(lined)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
                else
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("Error al intentar guardar el Simulador de Head Count", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
               
                Cursor = Cursors.Default;
                return true;
            }
            catch (Exception ie)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "LineUpLogica.Guardar(line)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        #endregion

        #region regBotones

        private void btnSave2_Click(object sender, EventArgs e)
        {
            btSave_Click(sender, e);
        }
        private void btnExit2_Click(object sender, EventArgs e)
        {
            btExit_Click(sender, e);
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            if (Guardar())
                Inicio();

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

        private void btnHelp_Click(object sender, EventArgs e)
        {

            try
            {
                System.Data.DataTable dt = ConfigLogica.Consultar();
                string sDirec = dt.Rows[0]["directorio"].ToString();
                if (!string.IsNullOrEmpty(sDirec))
                {
                    sDirec += @"\CloverPRO_ManualSetUp.pdf";
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

        #region regDetalleCambios
        private void CargarDetalle(int _aiFolio)
        {
            dgwEstaciones.DataSource = null;
            LineSetupDetLogica sed = new LineSetupDetLogica();
            sed.Folio = _aiFolio;
            DataTable dt = LineSetupDetLogica.Listar(sed);
            dgwEstaciones.DataSource = dt;

            CargarColumnas();
            CargarColumnasStd();

            
            dgwEstaciones.Focus();
            
        }
        private void dgwEstaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightBlue;
            else
                e.CellStyle.BackColor = Color.White;
           
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

        private void CargarColumnasStd()
        {

            int iRows = dgwStd1.Rows.Count;

            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("std");
                dtNew.Columns.Add("Lineas Activas", typeof(string));                 //0
                dtNew.Columns.Add("Unidades", typeof(string));//1
                dtNew.Columns.Add("HC", typeof(string));
                
                dgwStd1.DataSource = dtNew;
            }
            else
                dgwStd1.Rows[0].Selected = false;

            //dgwStd1.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //dgwStd1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            
            dgwStd1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwStd1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwStd1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwStd1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwStd1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwStd1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            iRows = dgwStd2.Rows.Count;

            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("std2");
                dtNew.Columns.Add("Lineas Activas", typeof(string));                 //0
                dtNew.Columns.Add("Unidades", typeof(string));//1
                dtNew.Columns.Add("HC", typeof(string));

                dgwStd2.DataSource = dtNew;
            }
            else
                dgwStd2.Rows[0].Selected = false;

            dgwStd2.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwStd2.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwStd2.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwStd2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwStd2.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwStd2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            iRows = dgwReal1.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("real");
                dtNew.Columns.Add("Coating Room", typeof(string));                 //0
                dtNew.Columns.Add("HC", typeof(string));//1
                dtNew.Columns.Add("HC RH", typeof(string));

                dgwReal1.DataSource = dtNew;
            }
            else
                dgwReal1.Rows[0].Selected = false;

            dgwReal1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwReal1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwReal1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwReal1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwReal1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwReal1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            iRows = dgwReal2.Rows.Count;

            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("real2");
                dtNew.Columns.Add("Coating Room", typeof(string));                 //0
                dtNew.Columns.Add("HC", typeof(string));//1
                dtNew.Columns.Add("HC RH", typeof(string));

                dgwReal2.DataSource = dtNew;
            }
            else
                dgwReal2.Rows[0].Selected = false;

            dgwReal2.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwReal2.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwReal2.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwReal2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwReal2.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwReal2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void CargarColumnas()
        {

            int iRows = dgwEstaciones.Rows.Count;

            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("headcount");
                dtNew.Columns.Add("LINEA", typeof(string));      //0
                dtNew.Columns.Add("MODELO", typeof(string));     //1
                dtNew.Columns.Add("STD 1T", typeof(int));        //2
                dtNew.Columns.Add("STD 2T", typeof(int));        //3
                dtNew.Columns.Add("STD H.C.", typeof(int));        //4
                dtNew.Columns.Add("H.C. ACTUAL TRESS 1T", typeof(int));        //5
                dtNew.Columns.Add("H.C. ACTUAL TRESS 2T", typeof(int));        //6
                dtNew.Columns.Add("TURNO 1", typeof(bool));      //7
                dtNew.Columns.Add("TURNO 2", typeof(bool));      //8
                dtNew.Columns.Add("linea", typeof(string));      //9
                dtNew.Columns.Add("tipo_linea", typeof(string)); //10
                
                dgwEstaciones.DataSource = dtNew;
            }
            
            dgwEstaciones.Columns[9].Visible = false;
            dgwEstaciones.Columns[10].Visible = false;

            dgwEstaciones.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwEstaciones.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            dgwEstaciones.Columns[0].Width = ColumnWith(dgwEstaciones, 20);//LINEA
            dgwEstaciones.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[0].ReadOnly = true;

            dgwEstaciones.Columns[1].Width = ColumnWith(dgwEstaciones, 20);//modelo
            dgwEstaciones.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[2].Width = ColumnWith(dgwEstaciones, 8);
            dgwEstaciones.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[3].Width = ColumnWith(dgwEstaciones, 8);
            dgwEstaciones.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[4].Width = ColumnWith(dgwEstaciones, 5);
            dgwEstaciones.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[5].Width = ColumnWith(dgwEstaciones, 10);
            dgwEstaciones.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[5].ReadOnly = true;
            dgwEstaciones.Columns[6].Width = ColumnWith(dgwEstaciones, 10);
            dgwEstaciones.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[6].ReadOnly = true;

            dgwEstaciones.Columns[7].Width = ColumnWith(dgwEstaciones, 10);
            dgwEstaciones.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwEstaciones.Columns[8].Width = ColumnWith(dgwEstaciones, 10);
            dgwEstaciones.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEstaciones.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        
        //ESCANER SOBRE #OPERADOR && LINEA OP
        private void AgregaCoating(string asTurno,bool abTipo,string asLinea,double adHc,double adHcTress)
        {
            
            AgregaReal(asTurno, abTipo, asLinea, adHc,adHcTress);
            
        }
        private void dgwEstaciones_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                if (e.ColumnIndex == 7 || e.ColumnIndex == 8)
                {
                    _lbCambioDet = true;
                    
                    double dStd1 = double.Parse(dgwEstaciones[2, e.RowIndex].Value.ToString());
                    double dStd2 = double.Parse(dgwEstaciones[3, e.RowIndex].Value.ToString());
                    double dHc = double.Parse(dgwEstaciones[4, e.RowIndex].Value.ToString());
                    double dHct1 = double.Parse(dgwEstaciones[5, e.RowIndex].Value.ToString());
                    double dHct2 = double.Parse(dgwEstaciones[6, e.RowIndex].Value.ToString());
                    string sLinea = dgwEstaciones[9, e.RowIndex].Value.ToString().ToUpper();
                    string sTipo = dgwEstaciones[10, e.RowIndex].Value.ToString();

                    bool bInd = bool.Parse(dgwEstaciones[e.ColumnIndex, e.RowIndex].Value.ToString());
                    
                    if (e.ColumnIndex == 7) 
                    {
                       
                        if (sTipo == "C")
                            AgregaCoating("1", bInd, sLinea, dHc, dHct1);

                        if(sTipo == "L")
                        {
                            string sVal = dgwEstaciones[1, e.RowIndex].Value.ToString().ToUpper();

                            if (dgwStd1.Rows.Count > 0)
                            {
                                int iCant = int.Parse(dgwStd1[0, 0].Value.ToString());
                                double dStd = double.Parse(dgwStd1[1, 0].Value.ToString());
                                double dHct = double.Parse(dgwStd1[2, 0].Value.ToString());

                                if (bInd)
                                {
                                    dStd += dStd1;
                                    dHct += dHc;
                                    iCant++;
                                }
                                else
                                {
                                    dStd -= dStd1;
                                    dHct -= dHc;
                                    iCant--;
                                }


                                dgwStd1.Rows[0].Cells[0].Value = iCant;
                                dgwStd1.Rows[0].Cells[1].Value = dStd;
                                dgwStd1.Rows[0].Cells[2].Value = dHct;
                            }
                            else
                            {
                                if (bInd)
                                    AgregaStd("1", 1, dStd1, dHc);
                            }
                        } 
                    }
                    else
                    {
                        if (sTipo == "C")
                            AgregaCoating("2", bInd, sLinea, dHc,dHct2);

                        if(sTipo == "L")
                        {
                            string sVal = dgwEstaciones[1, e.RowIndex].Value.ToString().ToUpper();

                            if (dgwStd2.Rows.Count > 0)
                            {
                                int iCant = int.Parse(dgwStd2[0, 0].Value.ToString());
                                double dStd = double.Parse(dgwStd2[1, 0].Value.ToString());
                                double dHct = double.Parse(dgwStd2[2, 0].Value.ToString());
                                if (bInd)
                                {
                                    dStd += dStd1;
                                    dHct += dHc;
                                    iCant++;
                                }
                                else
                                {
                                    dStd -= dStd1;
                                    dHct -= dHc;
                                    iCant--;
                                }
                                dgwStd2.Rows[0].Cells[0].Value = iCant;
                                dgwStd2.Rows[0].Cells[1].Value = dStd;
                                dgwStd2.Rows[0].Cells[2].Value = dHct;
                            }
                            else
                            {
                                if (bInd)
                                    AgregaStd("2", 1, dStd1, dHc);
                            }
                        }   
                    }
                }
                
                if (e.ColumnIndex == 2)
                {
                    /*
                    _lbCambioDet = true;

                    string sVal = dgwEstaciones[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
                    if (string.IsNullOrEmpty(sVal) || string.IsNullOrWhiteSpace(sVal))
                        return;

                    string sRPO = sVal.Trim();
                    if (sRPO.IndexOf("RPO") == -1)
                    {
                        int iRpo = 0;
                        if (int.TryParse(sRPO, out iRpo))
                            sRPO = "RPO" + sRPO;
                        else
                        {
                            MessageBox.Show("El RPO es incorrecto", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = string.Empty;
                            return;
                        }
                    }
                    dgwEstaciones[e.ColumnIndex, e.RowIndex].Value = sRPO;
                    string sModelo = string.Empty;
                    if (ConfigLogica.VerificaRpoOrbis())
                    {
                        RpoLogica rpo = new RpoLogica();
                        rpo.RPO = sRPO;
                        System.Data.DataTable dtRpo = RpoLogica.ConsultarOrbis(rpo);
                        if (dtRpo.Rows.Count != 0)
                            sModelo = dtRpo.Rows[0]["Producto"].ToString();
                    }
                    dgwEstaciones[8, e.RowIndex].Value = sModelo;
                    */
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "dgwEstaciones_CellValueChanged(6)" + Environment.NewLine + ex.ToString(), "Error en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void dgwEstaciones_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                btExit_Click(sender, e);
            }

            if (e.KeyCode == Keys.F1)
            {
                btnLine_Click(sender, e);//LINEA
            }
           
            if (e.KeyCode == Keys.F5)//NUEVO
            {
                btNew_Click(sender, e);
            }

            if (e.KeyCode == Keys.F6)//GUARDAR
            {
                btnSave2_Click(sender, e);
            }

           
            
        }

       
        private void dgwEstaciones_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            //e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        #endregion

        #region regCambio
        
        
        #endregion

        #region regColor
        private void txtFolio_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtFolio, 0);
        }

        private void txtFolio_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtFolio, 1);
        }
        
        #endregion

        #region regCaptura
        private void txtFolio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtFolio.Text) || string.IsNullOrWhiteSpace(txtFolio.Text))
                return;

            try
            {
                //LineSetupLogica line = new LineSetupLogica();
                //line.Folio = Convert.ToInt32(txtFolio.Text.ToString());
                //System.Data.DataTable data = LineSetupLogica.Consultar(line);
                //if(data.Rows.Count != 0)
                //{
                //    cbbPlanta.SelectedValue = data.Rows[0]["planta"].ToString();
                //    dtpFecha.Value = Convert.ToDateTime(data.Rows[0]["fecha"].ToString());
                //    //_liPartida
                //    _lsFolioAnt = txtFolio.Text.ToString();
                //    CargarDetalle(Convert.ToInt32(txtFolio.Text));
                //}
                //else
                //{
                //    MessageBox.Show("El Folio no se encuentra Registrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    txtFolio.Text = _lsFolioAnt;
                //    txtFolio.Focus();
                //    return;
                //}
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void AgregaStd(string _asTipo,int _aiLin, double _adUnidades, double _adHc)
        {
            if(_asTipo == "1")
            {
                DataTable dt = dgwStd1.DataSource as DataTable;
                dt.Rows.Add(_aiLin, _adUnidades, _adHc);
            }
            else
            {
                DataTable dt = dgwStd2.DataSource as DataTable;
                dt.Rows.Add(_aiLin, _adUnidades, _adHc);
            }

        }

        private void AgregaReal(string _asTipo,bool abAdd,string _asLin, double _adUnidades, double _adHc)
        {
            if(_asTipo=="1")
            {
                if(abAdd)
                {
                    DataTable dt = dgwReal1.DataSource as DataTable;
                    dt.Rows.Add(_asLin, _adUnidades, _adHc);
                }
                else
                {
                    dgwReal1.Rows.RemoveAt(0);
                    
                    
                }
                
            }
            else
            {
                if (abAdd)
                {
                    DataTable dt = dgwReal2.DataSource as DataTable;
                    dt.Rows.Add(_asLin, _adUnidades, _adHc);
                }
                else
                {
                    dgwReal2.Rows.RemoveAt(0);

                    //lblHctotal = 0;
                }

            }
        }
        private void AgregaLinea(string _asLinea, string _asModelo, double _adStd1, double _adStd2, double _adHc, double _adHcTress1, double _adHcTress2, bool _abActiva1,bool _abActiva2,string _asLine,string _asTipo)
        {
           
            DataTable dt = dgwEstaciones.DataSource as DataTable;
            dt.Rows.Add(_asLinea, _asModelo, _adStd1,_adStd2,_adHc,_adHcTress1,_adHcTress2 ,_abActiva1,_abActiva2,_asLine,_asTipo);
        }
        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Inicio();

                ModelohcLogica line = new ModelohcLogica();
                line.Planta = cbbPlanta.SelectedValue.ToString();
                DataTable data = ModelohcLogica.ListarLinea(line);
                int iLineas = data.Rows.Count;
                if (iLineas > 0)
                {
                    double dTotT1 = 0;
                    double dTotT2 = 0;
                    for (int x = 0; x < data.Rows.Count; x++)
                    {
                        string sLinea = data.Rows[x][0].ToString();
                        string sLine = data.Rows[x][7].ToString();
                        string sTipo = "L";
                        if (cbbPlanta.SelectedValue.ToString() == "COL" && !sLine.StartsWith("C"))
                            sTipo = "C";

                        //get HC_tress
                        double dHcTress1 = 0;
                        double dHcTress2 = 0;
                        
                        TressLogica tres = new TressLogica();
                        string sPta = cbbPlanta.SelectedValue.ToString();
                        if (sPta == "COL")
                            sPta = "002";
                        string sLineTress = sLinea.Substring(4);
                        if (sTipo == "C")
                            sLineTress = "856CLR";
                        tres.Planta = sPta;
                        tres.Linea = sLineTress;
                        DataTable dt3 = TressLogica.LineHeadCount(tres);
                        if(dt3.Rows.Count > 0)
                        {
                            for(int c = 0; c < dt3.Rows.Count; c++)
                            {
                                string sTurno = dt3.Rows[c][0].ToString().Trim();
                               
                                double dHc = 0;
                                if (!double.TryParse(dt3.Rows[0][1].ToString(), out dHc))
                                    dHc = 0;
                                if (sTurno == "1")
                                    dHcTress1 = dHc;
                                else
                                    dHcTress2 = dHc;
                            }
                        }

                        AgregaLinea(sLinea, null, 0, 0, 0,  dHcTress1, dHcTress2, false, false,sLine, sTipo);
                        dTotT1 += dHcTress1;
                        dTotT2 += dHcTress2;
                    }

                    lblLines.Text = iLineas.ToString();
                    lblTress1.Text = dTotT1.ToString();
                    lblTress2.Text = dTotT2.ToString();

                    dgwEstaciones.Rows[0].Selected = true;
                    dgwEstaciones.Focus();
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
        private void CargaStandar()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                foreach (DataGridViewRow row in dgwEstaciones.Rows)
                {
                    string sLinea = row.Cells[0].Value.ToString();
                    string sLineOr = sLinea.Substring(4);
                    string sInd = "1";
                    string sModelo = string.Empty;
                    string sModeloAnt = string.Empty;
                    double dStd1 = 0;
                    double dStd2 = 0;
                    double dHC = 0;
                    double dFac = 0;
                    double dStd1Ant = 0;
                    sInd = "1";

                    DataTable dtO = new DataTable();
                    RpoLogica rpo = new RpoLogica();
                    rpo.Linea = sLineOr;
                    rpo.Fecha = dtpFecha.Value;

                    dtO = RpoLogica.ConsultarOrbisLinea(rpo);
                    if (dtO.Rows.Count != 0)
                    {
                        sModelo = dtO.Rows[0]["Producto"].ToString().ToUpper();
                        dStd1 = 0;
                        dStd2 = 0;
                        dHC = 0;
                        dFac = 0;

                        if (!string.IsNullOrEmpty(sModelo))
                        {
                            ModelohcLogica cap = new ModelohcLogica();
                            cap.Planta = cbbPlanta.SelectedValue.ToString();
                            cap.Linea = sLinea;
                            cap.Modelo = sModelo;
                            DataTable dtM = ModelohcLogica.ConsultarModelo(cap);
                            if (dtM.Rows.Count > 0)
                            {
                                dStd1 = double.Parse(dtM.Rows[0][4].ToString());

                                if (dStd1 > dStd1Ant)
                                {
                                    dHC = double.Parse(dtM.Rows[0][3].ToString());
                                    dStd2 = double.Parse(dtM.Rows[0][5].ToString());
                                    dFac = double.Parse(dtM.Rows[0][6].ToString());
                                }
                                else
                                    sModelo = sModeloAnt;
                            }
                        }

                        dStd1Ant = dStd1;
                        sModeloAnt = sModelo;
                    }
                    else
                        sInd = "0";

                    bool bAct = false;
                    if (sInd == "1")
                        bAct = true;
                    //resutlado de standar
                    row.Cells[1].Value = sModelo;
                    row.Cells[2].Value = dStd1;
                    row.Cells[3].Value = dStd2;
                    row.Cells[4].Value = dHC;

                    if (!string.IsNullOrEmpty(sModelo))
                    {
                        double dHcTress = 0;
                        bool bActiva = false;
                        if (!double.TryParse(row.Cells[5].Value.ToString(), out dHcTress))
                            dHcTress = 0;
                        if (dHcTress > 0)
                            bActiva = true;
                        row.Cells[7].Value = bActiva;

                        bActiva = false;
                        if (!double.TryParse(row.Cells[6].Value.ToString(), out dHcTress))
                            dHcTress = 0;
                        if (dHcTress > 0)
                            bActiva = true;
                        row.Cells[8].Value = bActiva;
                    }
                }
            }
            catch(Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            Cursor = Cursors.Default;
        }
       
        #endregion

        #region regFuncion
       
        
        private void tssF3_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "60") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            wfCapturaPop popUp = new wfCapturaPop(_lsProceso);
            popUp.ShowDialog();
            string sOperacion = popUp._sClave;
            if (string.IsNullOrEmpty(sOperacion))
                return;

            int iClave = Convert.ToInt32(dgwEstaciones[4, dgwEstaciones.RowCount - 1].Value);
            iClave++;
            string sEstacion = iClave.ToString();
            string sLinea = string.Empty;
           
            dgwEstaciones.Rows[dgwEstaciones.Rows.Count - 1].Cells[8].Selected = true;
        }

        private void tssF4_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "30") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            iRow = dgwEstaciones.CurrentCell.RowIndex;
            string sTipo = dgwEstaciones[11, iRow].Value.ToString();
            if (sTipo == "X" || sTipo == "Z" || sTipo == "G")
                return;
            
            dgwEstaciones[8, iRow].Value = "N/A";
            dgwEstaciones[9, iRow].Value = "NO APLICA MODELO";
            dgwEstaciones[10, iRow].Value = string.Empty;
            dgwEstaciones[11, iRow].Value = string.Empty;
            dgwEstaciones[13, iRow].Value = "M";
            dgwEstaciones[14, iRow].Value = "0";

            iRow++;
            if(dgwEstaciones.RowCount > iRow)
                dgwEstaciones.Rows[iRow].Cells[8].Selected = true;
        }

        private void tssF5_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "40") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (dgwEstaciones[12, iRow].Value.ToString() == "O")
            {
                int iClave = Convert.ToInt32(dgwEstaciones[4, iRow].Value);
                string sEstacion = dgwEstaciones[5, iRow].Value.ToString();
                string sOperacion = dgwEstaciones[6, iRow].Value.ToString();
                string sNivel = dgwEstaciones[7, iRow].Value.ToString();
                string sLinea = string.Empty;
                
                dgwEstaciones.Rows[dgwEstaciones.Rows.Count - 1].Cells[8].Selected = true;
            }
        }

        private void tssF6_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "50") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string sTipo = dgwEstaciones[12, iRow].Value.ToString();
            if (sTipo == "X" || sTipo == "Z")
            {
                int iFolio = 0;
                int iCons = 0;
                if (Int32.TryParse(dgwEstaciones[0, iRow].Value.ToString(), out iFolio))
                {
                    iCons = Convert.ToInt32(dgwEstaciones[1, iRow].Value);
                    LineUpDetLogica line = new LineUpDetLogica();
                    line.Folio = iFolio;
                    line.Consec = iCons;
                    LineUpDetLogica.Eliminar(line);
                }
                dgwEstaciones.Rows.Remove(dgwEstaciones.CurrentRow);
            }
        }

        private void tssF7_Click(object sender, EventArgs e)
        {
            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            dgwEstaciones[8, iRow].Value = string.Empty;
            dgwEstaciones[9, iRow].Value = string.Empty;
            dgwEstaciones[10, iRow].Value = string.Empty;
            dgwEstaciones[11, iRow].Value = string.Empty;
            dgwEstaciones[13, iRow].Value = string.Empty;
            dgwEstaciones[14, iRow].Value = string.Empty;
        }

        private void btnF4_Click(object sender, EventArgs e)
        {
            tssF4_Click(sender, e);
            dgwEstaciones.Focus();
        }

        private void btnF3_Click(object sender, EventArgs e)
        {
            tssF3_Click(sender, e);
            dgwEstaciones.Focus();
        }

        private void btnF5_Click(object sender, EventArgs e)
        {
            tssF5_Click(sender, e);
            dgwEstaciones.Focus();
        }

        private void btnF6_Click(object sender, EventArgs e)
        {
            tssF6_Click(sender, e);
            dgwEstaciones.Focus();
        }

        private void btnF7_Click(object sender, EventArgs e)
        {
            tssF7_Click(sender, e);
            dgwEstaciones.Focus();
        }

        
        #endregion

        #region regFlechas
        private void btnFirst_Click(object sender, EventArgs e)
        {
            string sValor = GlobalVar.Navegacion("F", "t_lineset", "folio", txtFolio.Text);
            _lsParam = sValor;

            if (_lbCambio)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (Guardar())
                        wfLineCapacity_Load(sender, e);
                }
                else
                {
                    if (Result == DialogResult.No)
                        wfLineCapacity_Load(sender, e);
                }
            }
            else
                wfLineCapacity_Load(sender, e);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            string sValor = "";
            if (string.IsNullOrEmpty(txtFolio.Text))
                sValor = GlobalVar.Navegacion("F", "t_lineset", "folio", txtFolio.Text);
            else
                sValor = GlobalVar.Navegacion("B", "t_lineset", "folio", txtFolio.Text);
            _lsParam = sValor;


            if (_lbCambio)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (Guardar())
                        wfLineCapacity_Load(sender, e);
                }
                else
                {
                    if (Result == DialogResult.No)
                        wfLineCapacity_Load(sender, e);
                }
            }
            else
                wfLineCapacity_Load(sender, e);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_lbCambio)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                    Guardar();
                _lbCambio = false;
            }

            string sValor = string.Empty;
            if (string.IsNullOrEmpty(txtFolio.Text))
                sValor = GlobalVar.Navegacion("F", "t_lineset", "folio", txtFolio.Text);
            else
                sValor = GlobalVar.Navegacion("N", "t_lineset", "folio", txtFolio.Text);

            _lsParam = sValor;
            wfLineCapacity_Load(sender, e);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (_lbCambio)
            {
                DialogResult Result = MessageBox.Show("Desea guardar los cambios?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                    Guardar();
                _lbCambio = false;
            }

            string sValor = GlobalVar.Navegacion("L", "t_lineset", "folio", txtFolio.Text);
            _lsParam = sValor;
            wfLineCapacity_Load(sender, e);
        }
        #endregion

        #region regRes

        private void wfLineCapacity_Resize(object sender, EventArgs e)
        {
            if (WindowState != _WindowStateAnt && WindowState != FormWindowState.Minimized)
            {
                _WindowStateAnt = WindowState;
                ResizeControl(panel1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(groupBox1, 1, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(pnlTotal, 4, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(panel3, 1, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(dgwEstaciones, 1, ref _iWidthAnt, ref _iHeightAnt, 1);

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
            if (ai_Hor == 4)
            {
                int iH = groupBox1.Height - groupBox1.Location.Y;
                ac_Control.Location = new Point(ac_Control.Location.X, iH + ac_Control.Height - 10);
                    // lblCant.Location = new Point(lblCant.Location.X, groupBox1.Height - iH);
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

        #region regBotones
        private void btnLine_Click(object sender, EventArgs e) //PLANTA Y LINEA
        {
            if(cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de Ingresar la Planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return;
            }

            if (dgwEstaciones.Rows.Count == 0)
                return;

            int iRow = dgwEstaciones.CurrentCell.RowIndex;
            if (dgwEstaciones.CurrentRow.Index == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "10") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {

                if (string.IsNullOrEmpty(dgwEstaciones[0, iRow].Value.ToString()))
                    return;
                
                string sPlanta = cbbPlanta.SelectedValue.ToString();
                string sLinea = dgwEstaciones[0, iRow].Value.ToString();

                wfBusquedaPop wfBuscar = new wfBusquedaPop();
                wfBuscar._sClave = sPlanta;
                wfBuscar._lsLinea = sLinea;
                wfBuscar.ShowDialog();
                string sModelo = wfBuscar._lsModelo;
                if (!string.IsNullOrEmpty(sModelo))
                {
                    dgwEstaciones[1, iRow].Value = sModelo;
                    dgwEstaciones[2, iRow].Value = wfBuscar._ldStd1;
                    dgwEstaciones[3, iRow].Value = wfBuscar._ldStd2;
                    dgwEstaciones[4, iRow].Value = wfBuscar._ldHc;

                    double dHcTress = 0;
                    bool bActiva = false;
                    if (!double.TryParse(dgwEstaciones[5, iRow].Value.ToString(), out dHcTress))
                        dHcTress = 0;
                    if (dHcTress > 0)
                        bActiva = true;
                    dgwEstaciones[7, iRow].Value = bActiva;
                    bActiva = false;
                    if (!double.TryParse(dgwEstaciones[6, iRow].Value.ToString(), out dHcTress))
                        dHcTress = 0;
                    if (dHcTress > 0)
                        bActiva = true;
                    dgwEstaciones[8, iRow].Value = bActiva;
                    
                    /*
                    ModelohcLogica cap = new ModelohcLogica();
                    cap.Planta = cbbPlanta.SelectedValue.ToString();
                    cap.Linea = sLinea;
                    cap.Modelo = sModelo;
                    DataTable dtM = ModelohcLogica.ConsultarModelo(cap);
                    if (dtM.Rows.Count > 0)
                    {
                        double dStd1 = double.Parse(dtM.Rows[0][4].ToString());
                        double dHC = double.Parse(dtM.Rows[0][3].ToString());
                            
                        double dStd2 = double.Parse(dtM.Rows[0][5].ToString());
                        double dFac = double.Parse(dtM.Rows[0][6].ToString());
                        
                        bool bAct = true;
                        dgwEstaciones[2, iRow].Value = dStd1;
                        dgwEstaciones[3, iRow].Value = dStd2;
                        dgwEstaciones[4, iRow].Value = dHC;
                        dgwEstaciones[5, iRow].Value = dFac;
                        dgwEstaciones[6, iRow].Value = bAct;
                        dgwEstaciones[7, iRow].Value = bAct;
                    }
                    */
                }
 
                dgwEstaciones.Focus();
                iRow++;
                if (iRow == dgwEstaciones.Rows.Count)
                    iRow--;
                dgwEstaciones.CurrentCell = dgwEstaciones[0, iRow];

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        
        private void btnF1_Click(object sender, EventArgs e) //NOTAS
        {
            if (string.IsNullOrEmpty(txtFolio.Text))
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "40") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                #region regCapturaNota

                long lFolio = long.Parse(txtFolio.Text.ToString());
               
                if(!string.IsNullOrEmpty(_lsArea))
                {
                    wfActividEstatusPop ActPop = new wfActividEstatusPop();
                    ActPop._lFolio = lFolio;
                    //ActPop._iConsec = iCons;
                    ActPop._sArea = _lsArea;
                    ActPop.ShowDialog();
                }
                
                #endregion
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(),"ERROR en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion

        private void btnUrgencia_Click(object sender, EventArgs e)
        {
            if (cbbPlanta.SelectedIndex == -1)
                return;

            CargaStandar();
        }

        
    }
}
