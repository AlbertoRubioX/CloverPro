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

namespace CloverPro
{
    public partial class wfInspeccionManual : Form
    {
        private string _lsProceso = "PRO020";
        public bool _lbCambio;
        public wfInspeccionManual()
        {
            InitializeComponent();
        }

        private void wfInspeccionManual_Load(object sender, EventArgs e)
        {
            Inicio();
            
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

            if (!string.IsNullOrEmpty(GlobalVar.gsPlanta))
                cbbPlanta.SelectedValue = GlobalVar.gsPlanta;

            txtRPO.Clear();

            CargarColumnas();

            txtRPO.Focus();

        }

        private void CargarColumnas()
        {
            if (dgwEtiq.Rows.Count == 0)
            {

                DataTable dtNew = new DataTable("Etiqueta");
                dtNew.Columns.Add("planta", typeof(string));
                dtNew.Columns.Add("folio", typeof(int));
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("rpo", typeof(string));
                dtNew.Columns.Add("Etiqueta", typeof(string));
                dtNew.Columns.Add("Linea", typeof(string));
                dtNew.Columns.Add("Hora Inicia", typeof(string));
                dtNew.Columns.Add("Hora Termina", typeof(string));
                dtNew.Columns.Add("Sin Utilizar", typeof(bool));
                dtNew.Columns.Add("Defecto", typeof(bool));
                dtNew.Columns.Add("Codigo1", typeof(string));
                dtNew.Columns.Add("Código2", typeof(string));
                dtNew.Columns.Add("Código3", typeof(string));
                dgwEtiq.DataSource = dtNew;

            }
            /*
            DataGridViewComboBoxColumn cmb = new DataGridViewComboBoxColumn();
            cmb.HeaderText = "Código Defecto";
            cmb.Name = "codigodef";
            cmb.MaxDropDownItems = 5;
            cmb.Items.Add("4000");
            cmb.Items.Add("4001");
            dgwEtiq.Columns.Add(cmb);
            */

            dgwEtiq.Columns[0].Visible = false;
            dgwEtiq.Columns[1].Visible = false;
            dgwEtiq.Columns[2].Visible = false;
            dgwEtiq.Columns[3].Visible = false;

            dgwEtiq.Columns[4].Width = ColumnWith(dgwEtiq, 15);//etiqueta
            dgwEtiq.Columns[5].Width = ColumnWith(dgwEtiq, 10);//linea
            dgwEtiq.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEtiq.Columns[6].Width = ColumnWith(dgwEtiq, 13);//hora ini
            dgwEtiq.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEtiq.Columns[7].Width = ColumnWith(dgwEtiq, 13);//hora fin
            dgwEtiq.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEtiq.Columns[8].Width = ColumnWith(dgwEtiq, 10);//sin util
            dgwEtiq.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEtiq.Columns[9].Width = ColumnWith(dgwEtiq, 10);//con def
            dgwEtiq.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEtiq.Columns[10].Width = ColumnWith(dgwEtiq, 10);//codigo1
            dgwEtiq.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEtiq.Columns[11].Width = ColumnWith(dgwEtiq, 10);//codigo2
            dgwEtiq.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwEtiq.Columns[12].Width = ColumnWith(dgwEtiq, 10);//codigo3
            dgwEtiq.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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

        private Boolean BuscarRPO(string asRPO)
        {
            DataTable dtEx = new DataTable();

            try
            {
                if (asRPO.IndexOf("RPO") == -1)
                    asRPO = "RPO" + asRPO;

                int iPos = asRPO.IndexOf("-");
                if (iPos > 0)
                    asRPO = asRPO.Substring(0, iPos);
                
                txtRPO.Text = asRPO;

                RpoLogica rpo = new RpoLogica();
                rpo.RPO = asRPO;
                if (!RpoLogica.Verificar(rpo))
                {
                    MessageBox.Show("No se encontró información del RPO, favor de capturar ó escanear Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error... " + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        private bool Valida()
        {

            bool bValida = false;

            if(string.IsNullOrEmpty(cbbPlanta.Text))
            {
                MessageBox.Show("No se a especificado la planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }

            if(string.IsNullOrEmpty(txtRPO.Text) || string.IsNullOrWhiteSpace(txtRPO.Text))
            {
                MessageBox.Show("No se a especificado el RPO", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRPO.Focus();
                return bValida;
            }
            else
            {
                if (!BuscarRPO(txtRPO.Text.ToString().Trim()))
                    return false;
            }
            
            if(dgwEtiq.Rows.Count == 0)
            {
                MessageBox.Show("No se ha registrado la Inspección de Etiquetas", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRPO.Focus();
                return bValida;
            }

            return true;
        }

        #region regCaptura
        private void wfInspeccionManual_Activated(object sender, EventArgs e)
        {
            txtRPO.Focus();
        }
        
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

       
        private void cbbPlanta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

        }

        private void cbbPlanta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbbPlanta.Text.Length >= 9)
                e.Handled = true;
        }

        private void txtRPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtRPO.Text))
            {
                //FIND SKU IN DATABASE
                if(!BuscarRPO(txtRPO.Text.ToString().Trim()))
                    txtRPO.Clear();

                EtiquetaDetLogica etid = new EtiquetaDetLogica();
                etid.Planta = cbbPlanta.SelectedValue.ToString();
                etid.RPO = txtRPO.Text.ToString();
                try
                {
                    DataTable dtDet = EtiquetaDetLogica.Listar(etid);
                    foreach(DataRow row in dtDet.Rows)
                    {
                        string sPlanta = row[0].ToString();
                        long lFolio = long.Parse(row[1].ToString());
                        int iCons = Int16.Parse(row[2].ToString());
                        string sRPO = row[3].ToString();
                        string sCode = row[4].ToString();
                        string sLinea = row[5].ToString();
                        string sHoraE = row[6].ToString();
                        string sHoraS = row[7].ToString();
                        string sUtil = row[8].ToString();
                        bool bUtil = false;
                        if (sUtil == "1")
                            bUtil = true;

                        bool bDefec = false;
                        string sDefec = row[9].ToString();
                        if (sDefec == "1")
                            bDefec = true;
                       
                        string sCodigo = row[10].ToString();
                        

                        DataTable dt = dgwEtiq.DataSource as DataTable;
                        dt.Rows.Add(sPlanta,lFolio,iCons,sRPO,sCode,sLinea,sHoraE,sHoraS,bUtil,bDefec,sCodigo,sCodigo,sCodigo);
                        //dgwEtiq.DataSource = dtDet;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CargarColumnas();

            }
        }
        #endregion

        #region regBotones
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Inicio();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (Guardar())
                Inicio();

        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            wfImpresor Impresor = new wfImpresor();
            Impresor._lsProceso = _lsProceso;
            Impresor._lsPlanta = cbbPlanta.SelectedValue.ToString();
            Impresor._lsRPO = txtRPO.Text.ToString();
            Impresor.Show();
        }
        #endregion
        private bool Guardar()
        {
            if (!Valida())
                return false;

            try
            {
                Cursor = Cursors.WaitCursor;
                foreach (DataGridViewRow row in dgwEtiq.Rows)
                {

                    EtiquetaDetLogica eti = new EtiquetaDetLogica();
                    eti.Planta = row.Cells[0].Value.ToString();
                    eti.Folio = long.Parse(row.Cells[1].Value.ToString());
                    eti.Consec = Int16.Parse(row.Cells[2].Value.ToString());
                    eti.RPO = row.Cells[3].Value.ToString();
                    eti.Barcode = row.Cells[4].Value.ToString();
                    eti.Linea = row.Cells[5].Value.ToString();
                    eti.HoraEntra = row.Cells[6].Value.ToString();
                    eti.HoraSale = row.Cells[7].Value.ToString();

                    if(bool.Parse(row.Cells[8].Value.ToString()))
                        eti.SinUtil = "1";
                    else
                        eti.SinUtil = "0";

                    
                    if(bool.Parse(row.Cells[9].Value.ToString()))
                        eti.Defecto = "1";
                    else
                        eti.Defecto = "0";

                    eti.Codigo = row.Cells[10].Value.ToString();

                    if (EtiquetaDetLogica.Guardar(eti) == 0)
                    {
                        Cursor = Cursors.Default;
                        return false;
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + ex.ToString(), Text + " Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
                return false;
            }

            Cursor = Cursors.Default;
            return true;
        }
        
    }
}
