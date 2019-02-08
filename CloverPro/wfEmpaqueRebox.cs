using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Windows.Forms;
using System.IO;
using System.Management;
using Microsoft.Office.Interop;
using Logica;
using Datos;

namespace CloverPro
{
    public partial class wfEmpaqueRebox : Form
    {

        private string _lsProceso = "EMP030";
        private bool _lbCambio;
        private long _llFolio;
        private string _lsDirec;
        private string _lsFile;
        public wfEmpaqueRebox()
        {
            InitializeComponent();
        }

        #region regInicio
        private void Inicio()
        {
            DataTable dtC = ConfigLogica.Consultar();
            _lsDirec = dtC.Rows[0]["direc_rma"].ToString();
            _lsFile = dtC.Rows[0]["filename_rma"].ToString();

            if (string.IsNullOrEmpty(_lsDirec))
                _lsDirec = @"C:\CloverPRO\Formatos";
            if (string.IsNullOrEmpty(_lsFile))
                _lsFile = "RMAEmpaque";

            UsuarioLogica us = new UsuarioLogica();
            us.Usuario = GlobalVar.gsUsuario;
            DataTable dt = UsuarioLogica.Consultar(us);
            string sPta = dt.Rows[0]["planta"].ToString();

            cbbPlanta.ResetText();
            dt = PlantaLogica.Listar();
            cbbPlanta.DataSource = dt;
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            if (!string.IsNullOrEmpty(sPta))
                cbbPlanta.SelectedValue = sPta;
            else
                cbbPlanta.SelectedIndex = -1;

            cbbOrigen.ResetText();
            Dictionary<string, string> Orig = new Dictionary<string, string>();
            Orig.Add("HOLD", "MX01AHOLD");
            Orig.Add("REWK", "REWORKMX06");
            cbbOrigen.DataSource = new BindingSource(Orig, null);
            cbbOrigen.DisplayMember = "Value";
            cbbOrigen.ValueMember = "Key";
            cbbOrigen.SelectedIndex = -1;

            txtTarima.Clear();
            txtRPO.Clear();
            txtSKU.Clear();
            txtFinishg.Clear();
            lblItem.Text = "";
            txtCant.Text = "1";

            ReempaqueLogica reb = new ReempaqueLogica();
            dt = ReempaqueLogica.ConsultarDia();
            if (dt.Rows.Count > 0)
                _llFolio = long.Parse(dt.Rows[0][0].ToString());
            else
                _llFolio = 0;

            dt = ReempaqueLogica.ListarDia();
            dgwData.DataSource = dt;
            CargarColumnas();

            dt = ReempaqueLogica.ConsultarDiaTarima();
            dgwDataT.DataSource = dt;
            CargarColumnasT();

            dt = ReempaqueLogica.ConsultarDiaRPO();
            dgwDataR.DataSource = dt;
            CargarColumnasR();

            dt = ReempaqueLogica.ConsultarDiaModelo();
            dgwDataM.DataSource = dt;
            CargarColumnasM();


        }

        private void wfEmpaqueRebox_Activated(object sender, EventArgs e)
        {
            cbbOrigen.Focus();
        }

        private void wfEmpaqueRebox_Load(object sender, EventArgs e)
        {

            Inicio();
            cbbOrigen.Focus();
        }

        #endregion

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
        private void CargarColumnas()
        {
            int iRows = dgwData.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("Reempaque");
                dtNew.Columns.Add("folio", typeof(long));//0
                dtNew.Columns.Add("consec", typeof(int));//1
                dtNew.Columns.Add("PLANTA", typeof(string));//2
                dtNew.Columns.Add("LOCACION", typeof(string));//3
                dtNew.Columns.Add("TARIMA", typeof(string));//4
                dtNew.Columns.Add("ETIQUETA", typeof(string));//5
                dtNew.Columns.Add("RPO", typeof(string));//6
                dtNew.Columns.Add("MODELO", typeof(string));//7
                dtNew.Columns.Add("CANT", typeof(int));//8
                dtNew.Columns.Add("FG", typeof(string));//9
                dtNew.Columns.Add("NOTA", typeof(int));//10
                dtNew.Columns.Add("tipo", typeof(string));//11
                dtNew.Columns.Add("estado", typeof(string));//12
                dtNew.Columns.Add("f_ingreso", typeof(DateTime));//13
                dtNew.Columns.Add("torder", typeof(string));//14
                dtNew.Columns.Add("item", typeof(string));//15
                dgwData.DataSource = dtNew;
            }
            else
            {
                foreach (DataGridViewRow row in dgwData.Rows)
                {
                    if (row.Cells[11].Value.ToString() == "M")
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }

            dgwData.Columns[0].Visible = false;
            dgwData.Columns[1].Visible = false;
            dgwData.Columns[11].Visible = false;
            dgwData.Columns[12].Visible = false;
            dgwData.Columns[13].Visible = false;
            dgwData.Columns[14].Visible = false;
            dgwData.Columns[15].Visible = false;

            dgwData.Columns[2].ReadOnly = true;
            dgwData.Columns[2].Width = ColumnWith(dgwData, 6);
            dgwData.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[3].ReadOnly = true;
            dgwData.Columns[3].Width = ColumnWith(dgwData, 8);
            dgwData.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[4].ReadOnly = true;
            dgwData.Columns[4].Width = ColumnWith(dgwData, 10);
            dgwData.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[5].ReadOnly = true;
            dgwData.Columns[5].Width = ColumnWith(dgwData, 13);
            dgwData.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[6].Width = ColumnWith(dgwData, 12);
            dgwData.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[7].Width = ColumnWith(dgwData, 12);
            dgwData.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[8].Width = ColumnWith(dgwData, 5);
            dgwData.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgwData.Columns[9].Width = ColumnWith(dgwData, 12);
            dgwData.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[10].Width = ColumnWith(dgwData, 23);
            dgwData.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void CargarColumnasT()
        {
            int iRows = dgwDataT.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("tarimas");
                dtNew.Columns.Add("LOCACION", typeof(string));
                dtNew.Columns.Add("TARIMA", typeof(string));
                dtNew.Columns.Add("CANT", typeof(int));
                dgwDataT.DataSource = dtNew;
            }

            dgwDataT.Columns[0].Width = ColumnWith(dgwDataT, 40);
            dgwDataT.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwDataT.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwDataT.Columns[1].Width = ColumnWith(dgwDataT, 40);
            dgwDataT.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwDataT.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwDataT.Columns[2].Width = ColumnWith(dgwDataT, 19);
            dgwDataT.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwDataT.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void CargarColumnasR()
        {
            int iRows = dgwDataR.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("rpo");
                dtNew.Columns.Add("RPO", typeof(string));
                dtNew.Columns.Add("BC", typeof(string));
                dtNew.Columns.Add("CANT", typeof(int));
                dgwDataR.DataSource = dtNew;
            }


            dgwDataR.Columns[0].Width = ColumnWith(dgwDataR, 40);
            dgwDataR.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwDataR.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwDataR.Columns[1].Width = ColumnWith(dgwDataR, 40);
            dgwDataR.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwDataR.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwDataR.Columns[2].Width = ColumnWith(dgwDataR, 19);
            dgwDataR.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwDataR.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void CargarColumnasM()
        {
            int iRows = dgwDataM.Rows.Count;
            if (iRows == 0)
            {
                DataTable dtNew = new DataTable("modelos");
                dtNew.Columns.Add("FINISH GOOD", typeof(string));
                dtNew.Columns.Add("CANT", typeof(int));
                dgwDataM.DataSource = dtNew;
            }

            dgwDataM.Columns[0].Width = ColumnWith(dgwDataM, 80);
            dgwDataM.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwDataM.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwDataM.Columns[1].Width = ColumnWith(dgwDataM, 19);
            dgwDataM.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwDataM.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void dgwData_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            string sTipo = dgwData.Rows[e.RowIndex].Cells[10].Value.ToString();
            if (sTipo == "M")
                dgwData.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        private void dgwDataT_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.WhiteSmoke;
            else
                e.CellStyle.BackColor = Color.White;
        }

        private void dgwDataR_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.WhiteSmoke;
            else
                e.CellStyle.BackColor = Color.White;
        }

        private void dgwDataM_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.WhiteSmoke;
            else
                e.CellStyle.BackColor = Color.White;
        }
        private void dgwData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (e.ColumnIndex == 8 || e.ColumnIndex == 9)
                {
                    _lbCambio = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "dgwEstaciones_CellValueChanged(6)" + Environment.NewLine + ex.ToString(), "Error en " + Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void dgwData_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {

                if (e.ColumnIndex == 8)
                {
                    int iCant = 0;
                    if (!int.TryParse(Convert.ToString(e.FormattedValue), out iCant))
                    {
                        e.Cancel = true;
                        MessageBox.Show("Valor numérico incorrecto.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (iCant == 0)
                        {
                            e.Cancel = true;
                            MessageBox.Show("La Cantidad debe ser mayor que cero", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region regGuardar 
        private bool Valida()
        {

            if (dgwData.Rows.Count == 0)
                return false;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "10") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private bool EnviarMail()
        {

            DataTable dtMail = new DataTable();
            dtMail = CorreoLogica.ConsultarPend();
            if (dtMail.Rows.Count != 0)
            {
                try
                {
                    int iFolio = Convert.ToInt32(dtMail.Rows[0]["folio"].ToString());
                    EnviarCorreo envMail = new EnviarCorreo();
                    envMail.FolioMail = iFolio;
                    envMail.Asunto = dtMail.Rows[0]["asunto"].ToString();

                    string sMsg = EnviarCorreo.EnviaAlerta(envMail);

                    CorreoLogica correo = new CorreoLogica();

                    if (sMsg == "OK")
                    {
                        sMsg = "Se ha Enviado una Notificación por Correo electrónico al Supervisor.";
                        correo.Estado = "E";
                    }
                    else
                        correo.Estado = "R";

                    MessageBox.Show(sMsg, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    correo.Folio = Convert.ToInt32(dtMail.Rows[0]["folio"].ToString());
                    correo.Asunto = dtMail.Rows[0]["asunto"].ToString();
                    correo.Proceso = _lsProceso;
                    CorreoLogica.Guardar(correo);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }

            return true;
        }
        private void Guardar()
        {
            try
            {
                if (!Valida())
                    return;

                if (_llFolio == 0)
                    _llFolio = AccesoDatos.Consec(_lsProceso);

                ReempDetLogica remd = new ReempDetLogica();

                foreach (DataGridViewRow row in dgwData.Rows)
                {
                    remd.Folio = _llFolio;
                    remd.Consec = int.Parse(row.Cells[1].Value.ToString());
                    remd.Planta = row.Cells[2].Value.ToString();
                    remd.Locacion = row.Cells[3].Value.ToString();
                    remd.Tarima = row.Cells[4].Value.ToString();
                    remd.Barcode = row.Cells[5].Value.ToString();
                    remd.RPO = row.Cells[6].Value.ToString();
                    remd.Modelo = row.Cells[7].Value.ToString();
                    remd.Cantidad = int.Parse(row.Cells[8].Value.ToString());
                    remd.SKU = row.Cells[9].Value.ToString();
                    remd.Nota = row.Cells[10].Value.ToString();
                    remd.Tipo = row.Cells[11].Value.ToString();
                    remd.Estado = row.Cells[12].Value.ToString();
                    remd.Fingreso = Convert.ToDateTime(row.Cells[13].Value.ToString());
                    remd.TO = row.Cells[14].Value.ToString();
                    remd.Item = row.Cells[15].Value.ToString();
                    remd.Usuario = GlobalVar.gsUsuario;

                    ReempDetLogica.Guardar(remd);

                }

                ReempaqueLogica rem = new ReempaqueLogica();
                rem.Folio = _llFolio;
                rem.Estatus = "P";
                rem.Usuario = GlobalVar.gsUsuario;

                ReempaqueLogica.Guardar(rem);

                Inicio();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show("Favor de Notificar al Administrador." + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #endregion

        #region Controles

        private void txtRPO_Enter(object sender, EventArgs e)
        {
            txtCant.Enabled = false;
        }

        private void cbbOrigen_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtTarima.Focus();
        }

        private void AgregaLinea(string _asBarcode, string _asRPO, string _asModelo, int _aiCant, string _asSKU, string _asTipo, string _asNota, string _asItem)
        {
            string sPlanta = cbbPlanta.SelectedValue.ToString();
            string sLocacion = cbbOrigen.SelectedValue.ToString();
            string sTarima = txtTarima.Text.ToString();
            string sTO = txtTO.Text.ToString().Trim();

            DataTable dt = dgwData.DataSource as DataTable;
            dt.Rows.Add(0, 0, sPlanta, sLocacion, sTarima, _asBarcode, _asRPO, _asModelo, _aiCant, _asSKU, null, _asTipo, null,DateTime.Now, sTO, _asItem);

            //TARIMAS
            bool bExis = false;
            foreach (DataGridViewRow row in dgwDataT.Rows)
            {
                string sLoc = row.Cells[0].Value.ToString();
                string sTar = row.Cells[1].Value.ToString();
                if (sLoc == sLocacion && sTar == sTarima)
                {
                    bExis = true;
                    int iCant = int.Parse(row.Cells[2].Value.ToString());
                    iCant += _aiCant;
                    row.Cells[2].Value = iCant;
                    break;
                }
            }
            if (!bExis)
            {
                dt = dgwDataT.DataSource as DataTable;
                dt.Rows.Add(sLocacion, sTarima, _aiCant);
            }

            dgwData.CurrentCell = dgwData[8, dgwData.Rows.Count - 1];
            //RPO
            if (_asTipo == "R")
            {
                bExis = false;
                foreach (DataGridViewRow row in dgwDataR.Rows)
                {
                    string sRpo = row.Cells[0].Value.ToString();
                    string sBC = row.Cells[1].Value.ToString();
                    if (sRpo == _asRPO && sBC == _asModelo)
                    {
                        bExis = true;
                        int iCant = int.Parse(row.Cells[2].Value.ToString());
                        iCant += _aiCant;
                        row.Cells[2].Value = iCant;
                        break;
                    }
                }
                if (!bExis)
                {
                    dt = dgwDataR.DataSource as DataTable;
                    dt.Rows.Add(_asRPO, _asModelo, _aiCant);
                }

            }

            //MODELOS
            if (!string.IsNullOrEmpty(_asSKU))
            {
                bExis = false;
                foreach (DataGridViewRow row in dgwDataM.Rows)
                {
                    string sModelo = row.Cells[0].Value.ToString();

                    if (sModelo == _asSKU)
                    {
                        bExis = true;
                        int iCant = int.Parse(row.Cells[1].Value.ToString());
                        iCant += _aiCant;
                        row.Cells[1].Value = iCant;
                        break;
                    }
                }
                if (!bExis)
                {
                    dt = dgwDataM.DataSource as DataTable;
                    dt.Rows.Add(_asSKU, _aiCant);
                }
            }
        }
        private void BuscarSKU()
        {
            ItemsLogica item = new ItemsLogica();
            item.Modelo = txtSKU.Text.ToString();
            if (ItemsLogica.VerificarSKU(item))
            {
                //EL DATECODE CONTIENE EL FINISHGOOD
                txtFinishg.Text = item.Modelo;
                return;
            }

            //OBTENER FINISHGOOD DE ITEM NO.
            string sFG = string.Empty;
            bool bExis = false;

            wfCapturaPop_1t wfPop = new wfCapturaPop_1t(_lsProceso);
            wfPop.ShowDialog();
            string sItem = wfPop._sClave;
            if (!string.IsNullOrEmpty(sItem))
            {
                if (sItem.IndexOf("0") == 0)
                    sItem = sItem.Substring(1);

                item.Item = sItem;
                lblItem.Text = sItem;
                if (ItemsLogica.Verificar(item))
                {
                    DataTable dt = ItemsLogica.Consultar(item);
                    sFG = dt.Rows[0][1].ToString();
                    if (!string.IsNullOrEmpty(sFG))
                    {
                        txtFinishg.Text = sFG;
                        bExis = true;
                    }
                }

            }

            if (!bExis)
            {
                txtFinishg.Enabled = true;
                txtFinishg.Focus();
            }
        }
        private void txtRPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (cbbPlanta.SelectedIndex == -1)
                return;

            if (cbbOrigen.SelectedIndex == -1)
                return;

            if (string.IsNullOrEmpty(txtTarima.Text.ToString()))
                return;

            string sPlanta = cbbPlanta.SelectedValue.ToString();
            string sLocacion = cbbOrigen.SelectedValue.ToString();
            string sTarima = txtTarima.Text.ToString();

            if (!string.IsNullOrEmpty(txtRPO.Text))
            {
                try
                {
                    string asRPO = txtRPO.Text.ToString();
                    bool bExis = false;
                    foreach (DataGridViewRow row in dgwData.Rows)
                    {
                        string sBarc = row.Cells[5].Value.ToString();

                        if (sBarc == asRPO)
                        {
                            bExis = true;
                            break;
                        }
                    }

                    if (bExis)
                    {
                        MessageBox.Show("Etiqueta Repetida", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtRPO.Clear();
                        txtSKU.Clear();
                        txtFinishg.Clear();
                        lblItem.Text = "";
                        return;
                    }

                    int iPos = asRPO.IndexOf("-");

                    if (iPos != -1)
                        asRPO = asRPO.Substring(0, iPos);

                    if (asRPO.IndexOf("RPO") == -1)
                        asRPO = "RPO" + asRPO;

                    //FIND SKU IN DATABASE
                    string sModelo = BuscarRPO(asRPO);

                    if (!string.IsNullOrEmpty(sModelo))
                    {
                        txtSKU.Text = sModelo;

                        BuscarSKU(); // FINISH GOOD
                    }
                    else
                    {
                        txtSKU.Clear();
                        txtSKU.Focus();
                    }

                    string sFG = txtFinishg.Text.ToString();
                    string sItem = lblItem.Text.ToString();
                    if (string.IsNullOrEmpty(sModelo) || string.IsNullOrEmpty(sFG))
                        return;

                    txtCant.Text = "1";
                    string sTipo = "R";

                    //buscar modelo en dgw
                    int iCant = 1;
                    foreach (DataGridViewRow row in dgwData.Rows)
                    {
                        string sPta = row.Cells[2].Value.ToString();
                        string sLoc = row.Cells[3].Value.ToString();
                        string sTar = row.Cells[4].Value.ToString();
                        string sBarc = row.Cells[5].Value.ToString();
                        string sRpo = row.Cells[6].Value.ToString();

                        if (sBarc == txtRPO.Text.ToString())
                        {
                            bExis = true;
                            break;
                        }
                    }

                    if (!bExis)
                        AgregaLinea(txtRPO.Text.ToString(), asRPO, sModelo, iCant, sFG, sTipo, null, sItem);

                    txtRPO.Clear();
                    txtSKU.Clear();
                    txtFinishg.Clear();
                    txtCant.Text = "1";

                    txtRPO.Focus();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador." + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
        }

        private string BuscarRPO(string asRPO)
        {
            string sModelo = "";
            DataTable dtEx = new DataTable();

            try
            {
                RpoLogica rpo = new RpoLogica();
                rpo.RPO = asRPO;
                if (!RpoLogica.Verificar(rpo))
                {
                    if (ConfigLogica.VerificaRpoOrbis())
                    {
                        DataTable dtRpo = RpoLogica.ConsultarOrbis(rpo);
                        if (dtRpo.Rows.Count != 0)
                            sModelo = dtRpo.Rows[0]["Producto"].ToString();
                        else
                        {
                            if (string.IsNullOrEmpty(txtFinishg.Text.ToString()))
                            {
                                using (OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\MXNI-FS-01\Temp\ARINCON\sistemas\QaLabApp\BaseDeDatos\QaLab2.accdb;Persist Security Info=False"))
                                using (OleDbCommand Command = new OleDbCommand("SELECT Source_No from RPOS where RPO = '" + rpo.RPO + "'", con))
                                {
                                    con.Open();
                                    OleDbDataReader DB_Reader = Command.ExecuteReader();
                                    if (DB_Reader.HasRows)
                                    {
                                        DB_Reader.Read();
                                        sModelo = DB_Reader.GetString(0);
                                    }
                                    else
                                        MessageBox.Show("No se encontró información del RPO, favor de capturar ó escanear Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }

                            }
                        }
                    }
                    else
                    {
                        // BUSCAR RPO DE BASE DE CONTROL DE DOCUMENTOS
                        if (string.IsNullOrEmpty(txtFinishg.Text.ToString()))
                        {
                            using (OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\MXNI-FS-01\Temp\ARINCON\sistemas\QaLabApp\BaseDeDatos\QaLab2.accdb;Persist Security Info=False"))
                            using (OleDbCommand Command = new OleDbCommand("SELECT Source_No from RPOS where RPO = '" + rpo.RPO + "'", con))
                            {
                                con.Open();
                                OleDbDataReader DB_Reader = Command.ExecuteReader();
                                if (DB_Reader.HasRows)
                                {
                                    DB_Reader.Read();
                                    sModelo = DB_Reader.GetString(0);
                                }
                            }
                        }
                    }
                }
                else
                {
                    DataTable data = RpoLogica.Consultar(rpo);
                    sModelo = data.Rows[0]["modelo"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error... " + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return sModelo;
        }

        private void txtSKU_Enter(object sender, EventArgs e)
        {
            txtCant.Enabled = true;
        }

        private void txtSKU_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (cbbPlanta.SelectedIndex == -1)
                return;

            if (cbbOrigen.SelectedIndex == -1)
                return;

            if (string.IsNullOrEmpty(txtTarima.Text.ToString()))
                return;

            if (!string.IsNullOrEmpty(txtSKU.Text))
            {
                try
                {
                    if (txtSKU.Text.ToString().IndexOf("-M") != -1)
                    {
                        MessageBox.Show("Formato del Modelo Incorrecto.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    string sModelo = txtSKU.Text.ToString();
                    string sTipo = "M";
                    string sBarcode = txtRPO.Text.ToString();
                    string sRPO = string.Empty;
                    if (!string.IsNullOrEmpty(sBarcode))
                    {
                        int iPos = sBarcode.IndexOf("-");

                        if (iPos != -1)
                            sRPO = sBarcode.Substring(0, iPos);

                        if (sRPO.IndexOf("RPO") == -1)
                            sRPO = "RPO" + sRPO;
                    }

                    string sFG = txtFinishg.Text.ToString();
                    if (string.IsNullOrEmpty(sFG))
                    {
                        //CARGAR FINISH GOOD DEL ITEM
                        BuscarSKU();
                    }

                    sFG = txtFinishg.Text.ToString();
                    string sItem = lblItem.Text.ToString();
                    if (string.IsNullOrEmpty(sFG))
                        return;

                    //buscar modelo en dgw
                    bool bExis = false;
                    int iCant = 0;
                    if (!int.TryParse(txtCant.Text.ToString(), out iCant))
                        return;

                    foreach (DataGridViewRow row in dgwData.Rows)
                    {
                        string sMod = row.Cells[7].Value.ToString();
                        string sTp = row.Cells[9].Value.ToString();

                        if (sMod == sModelo && sTipo == sTp)
                        {
                            bExis = true;
                            iCant += int.Parse(row.Cells[8].Value.ToString());
                            row.Cells[8].Value = iCant;
                            break;
                        }
                    }
                    if (!bExis)
                        AgregaLinea(sBarcode, sRPO, sModelo, iCant, sFG, sTipo, null, sItem);

                    txtRPO.Clear();
                    txtSKU.Clear();
                    txtFinishg.Clear();
                    txtCant.Text = "1";
                    lblItem.Text = "";

                    txtSKU.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador." + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
        }

        private void txtCant_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRPO.Text))
                return;

            if (string.IsNullOrEmpty(txtSKU.Text))
            {
                txtSKU.Focus();

            }
        }

        private void txtCant_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (e.KeyCode != Keys.Enter)
                return;

            if (cbbPlanta.SelectedIndex == -1)
                return;

            if (cbbOrigen.SelectedIndex == -1)
                return;

            if (string.IsNullOrEmpty(txtTarima.Text.ToString()))
                return;

            if (!string.IsNullOrEmpty(txtSKU.Text))
            {
                /*
                try
                {
                    string sModelo = txtSKU.Text.ToString();
                    string sTipo = "M";

                    //buscar modelo en dgw
                    bool bExis = false;
                    int iCant = 0;

                    if (!int.TryParse(txtCant.Text.ToString(), out iCant))
                        return;

                    if (iCant <= 0)
                        return;

                    foreach (DataGridViewRow row in dgwData.Rows)
                    {
                        string sMod = row.Cells[7].Value.ToString();
                        string sTp = row.Cells[9].Value.ToString();

                        if (sMod == sModelo && sTipo == sTp)
                        {
                            bExis = true;
                            iCant += int.Parse(row.Cells[8].Value.ToString());
                            row.Cells[8].Value = iCant;
                            break;
                        }
                    }
                    if (!bExis)
                        AgregaLinea(null, null, sModelo, iCant, null, sTipo, null, null);


                    txtSKU.Clear();
                    txtCant.Text = "1";

                    txtSKU.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador." + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }*/
            }


        }

        private void txtCant_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        #endregion


        #region regBotones
        private void btnSave_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgwData.Rows.Count == 0)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "30") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult Result = MessageBox.Show("Desea Eliminar el registro?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                string sFolio = dgwData.SelectedCells[0].Value.ToString();
                string sConsec = dgwData.SelectedCells[1].Value.ToString();
                if (!string.IsNullOrEmpty(sFolio) && sConsec != "0")
                {
                    long lFolio = long.Parse(sFolio);
                    int iConsec = int.Parse(sConsec);

                    ReempDetLogica remd = new ReempDetLogica();
                    remd.Folio = lFolio;
                    remd.Consec = iConsec;

                    try
                    {
                        ReempDetLogica.Eliminar(remd);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "LineaLogica.Eliminar(line);" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                dgwData.Rows.Remove(dgwData.CurrentRow);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgwData.Rows.Count == 0)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "40") == false)
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
                oWB = oXL.Workbooks.Open(@"\\mxni-fs-01\Temp\wrivera\agonz0\CloverPro\RMA\RMAEmpaque.xlsx");
                oSheet = String.IsNullOrEmpty("Sheet1") ? (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet : (Microsoft.Office.Interop.Excel._Worksheet)oWB.Worksheets["Sheet1"];

                int iRow = 5;
                foreach (DataGridViewRow row in dgwData.Rows)
                {
                    string sEtiqueta = row.Cells[5].Value.ToString();
                    string sTarima = row.Cells[4].Value.ToString();
                    string sRPO = row.Cells[6].Value.ToString();
                    string sModelo = row.Cells[7].Value.ToString();
                    int iCant = int.Parse(row.Cells[8].Value.ToString());
                    string sSKU = row.Cells[9].Value.ToString();
                    string sNota = row.Cells[10].Value.ToString();
                    string sTipo = row.Cells[11].Value.ToString();
                    DateTime dtIngreso = Convert.ToDateTime(row.Cells[13].Value.ToString());
                    string sTO = row.Cells[14].Value.ToString();

                    oSheet.Cells[iRow, 1] = sEtiqueta;
                    oSheet.Cells[iRow, 2] = sSKU;
                    oSheet.Cells[iRow, 3] = iCant;
                    oSheet.Cells[iRow, 5] = dtIngreso;
                    oSheet.Cells[iRow, 6] = sTO;
                    oSheet.Cells[iRow, 7] = sTarima;
                    oSheet.Cells[iRow, 8] = sModelo;
                    oSheet.Cells[iRow, 9] = sRPO;
                    oSheet.Cells[iRow, 10] = sNota;

                    iRow++;
                }

                oXL.DisplayAlerts = false;
                string sDirec = _lsDirec+@"\"+_lsFile+".xlsx"; //RMAEmpaque.xlsx
                oWB.SaveAs(sDirec, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared);

                DialogResult Result = MessageBox.Show("Se ha exportado la captura." + Environment.NewLine + "Desea abrir el reporte en excel?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
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


        private void btnNew_Click(object sender, EventArgs e)
        {
            Inicio();
        }

        private void btnExit_Click(object sender, EventArgs e)
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
                    //sDirec += @"\CloverPRO_AyudaVisual_e.pdf";
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

        private void txtFinishg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (cbbPlanta.SelectedIndex == -1)
                return;

            if (cbbOrigen.SelectedIndex == -1)
                return;

            if (string.IsNullOrEmpty(txtFinishg.Text.ToString()) || string.IsNullOrEmpty(txtSKU.Text.ToString()))
                return;

            string sPlanta = cbbPlanta.SelectedValue.ToString();
            string sLocacion = cbbOrigen.SelectedValue.ToString();
            string sTarima = txtTarima.Text.ToString();

            string sItem = txtFinishg.Text.ToString();
            string sFG = string.Empty;

            ItemsLogica item = new ItemsLogica();

            if (sItem.IndexOf("0") == 0)
                sItem = sItem.Substring(1);

            item.Item = sItem;
            if (ItemsLogica.Verificar(item))
            {
                DataTable dt = ItemsLogica.Consultar(item);
                sFG = dt.Rows[0][1].ToString();
                txtFinishg.Text = sFG;
            }
            sFG = txtFinishg.Text.ToString();
            string sTipo = "M";
            string sRPO = string.Empty;
            string sBarcode = txtRPO.Text.ToString();
            if(!string.IsNullOrEmpty(sBarcode))
            {
                sTipo = "R";
                sRPO = sBarcode;
                int iPos = sRPO.IndexOf("-");

                if (iPos != -1)
                    sRPO = sRPO.Substring(0, iPos);

                if (sRPO.IndexOf("RPO") == -1)
                    sRPO = "RPO" + sRPO;
            }

            string sModelo = txtSKU.Text.ToString();
            bool bExis = false;
            int iCant = 0;
            if (!int.TryParse(txtCant.Text.ToString(), out iCant))
                return;

            if (iCant <= 0)
                return;

            foreach (DataGridViewRow row in dgwData.Rows)
            {
                string sMod = row.Cells[7].Value.ToString();
                string sTp = row.Cells[9].Value.ToString();

                if (sMod == sModelo && sTipo == sTp)
                {
                    bExis = true;
                    iCant += int.Parse(row.Cells[8].Value.ToString());
                    row.Cells[8].Value = iCant;
                    break;
                }
            }
            if (!bExis)
                AgregaLinea(txtRPO.Text.ToString(), sRPO, sModelo, iCant, sFG, sTipo, null, sItem);

            txtRPO.Clear();
            txtSKU.Clear();
            txtFinishg.Clear();
            txtCant.Text = "1";
            lblItem.Text = "";
            txtSKU.Focus();
        }
    }
}
