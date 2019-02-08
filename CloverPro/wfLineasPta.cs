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
    public partial class wfLineasPta : Form
    {
        public bool _lbCambio;
        public bool _lbCambioDet;

        public string _lsArea;
        public string _lsSuper;

        public DataTable _dtReturn;
        private string _lsParam;
        private string _lsTurno;
        public wfLineasPta(string asParam)
        {
            InitializeComponent();
            _lsParam = asParam;
        }

        #region regInicio
        private void wfLineasPta_Load(object sender, EventArgs e)
        {
            Inicio();

            UsuarioLogica user = new UsuarioLogica();
            user.Usuario = _lsParam;
            DataTable dtU = UsuarioLogica.Consultar(user);
            string sPlanta = string.Empty;
            if (dtU.Rows.Count != 0)
            {
                sPlanta = dtU.Rows[0]["planta"].ToString();
                sslSup.Text = dtU.Rows[0]["nombre"].ToString().ToUpper();
                _lsArea = dtU.Rows[0]["area"].ToString();
                _lsTurno = dtU.Rows[0]["turno"].ToString().ToUpper();
            }
                

            if(!string.IsNullOrEmpty(sPlanta))
            {
                cbbPlanta.SelectedValue = sPlanta;
                
                LineaLogica line = new LineaLogica();
                line.Planta = sPlanta;
                DataTable data = LineaLogica.VistaLineaPlanta(line);
                if (data.Rows.Count != 0)
                {
                    dgwLineas.Columns.Clear();
                    dgwLineas.DataSource = data;
                    
                    DataGridViewCheckBoxColumn dgwChb = new DataGridViewCheckBoxColumn();
                    dgwChb.ValueType = typeof(bool);
                    dgwChb.Name = "ind";
                    dgwChb.HeaderText = "";
                    dgwLineas.Columns.Insert(0, dgwChb);

                    CargarLineas();
                }
                else
                {
                    dgwLineas.Columns.Clear();
                    CargarLineas();
                }

            }
        }
        private void Inicio()
        {
            _lbCambio = false;
            _lbCambioDet = false;
            _lsTurno = string.Empty;

            cbbPlanta.ResetText();
            DataTable data = PlantaLogica.Listar();
            cbbPlanta.DataSource = data;
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.SelectedIndex = -1;

            cbbPlanta.Focus();

            SuperLineaLogica sup = new SuperLineaLogica();
            sup.Supervisor = _lsParam;
            dgwData.DataSource = SuperLineaLogica.Listar(sup);
        }
        

        private void wfLineasPta_Activated(object sender, EventArgs e)
        {
            cbbPlanta.Focus();
        }

        #endregion

        #region regGuardar
        private bool Guardar()
        {
            if (Valida())
            {

                try
                {
                    PlantaLogica plan = new PlantaLogica();
                    plan.Planta = cbbPlanta.Text.ToString().ToUpper();
                    
                    if (PlantaLogica.Guardar(plan) == 0)
                    {
                        MessageBox.Show("Error al intentar guardar la Planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    
                    foreach(DataGridViewRow row in dgwLineas.Rows)
                    {
                        if (row.Index == dgwLineas.Rows.Count - 1)
                            break;

                        if (dgwLineas.IsCurrentRowDirty)
                            dgwLineas.CommitEdit(DataGridViewDataErrorContexts.Commit);

                        string sPlanta = cbbPlanta.SelectedValue.ToString();
                        string sLine = Convert.ToString(row.Cells[1].Value);
                        string sNombre = Convert.ToString(row.Cells[2].Value);

                        LineaLogica line = new LineaLogica();
                        line.Planta = sPlanta;
                        line.Linea = sLine;
                        line.Nombre = sNombre;

                        if(LineaLogica.Guardar(line) == -1)
                        { 
                            MessageBox.Show("Error al intentar guardar la linea " + sLine, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                    }
                    return true;
                }
                catch(Exception ie)
                {
                    MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "PlantaLogica.Guardar(plan)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
                return false;
        }

        private bool Valida()
        {
            bool bValida = false;

            if (cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado la Planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }

           

            foreach(DataGridViewRow row in dgwLineas.Rows)
            {
                int iCons = 0;
                if (row.Cells[1].Value.ToString() == "1")
                {
                    iCons++;
                }

                if (iCons == 0)
                    return bValida;
            }

            return true;
        }
        #endregion

        #region regCaptura
        private void cbbPlanta_KeyPress(object sender, KeyPressEventArgs e)
        {
       
        }
        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                LineaLogica line = new LineaLogica();
                line.Planta = cbbPlanta.SelectedValue.ToString();
                DataTable data = LineaLogica.VistaLineaPlanta(line);
                if (data.Rows.Count != 0)
                {
                    dgwLineas.Columns.Clear();

                    dgwLineas.DataSource = data;

                    DataGridViewCheckBoxColumn dgwChb = new DataGridViewCheckBoxColumn();
                    dgwChb.ValueType = typeof(bool);
                    dgwChb.Name = "ind";
                    dgwChb.HeaderText = "";
                    dgwLineas.Columns.Insert(0, dgwChb);

                    CargarLineas();
                }
                else
                {
                    dgwLineas.Columns.Clear();
                    CargarLineas();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void cbbPlanta_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
        #endregion

        #region regDatagrids
        private void dgwLineas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightSkyBlue;
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

        private void CargarLineas()
        {
            if(dgwLineas.RowCount > 0)
            {
                dgwLineas.Columns[0].Width = ColumnWith(dgwLineas, 10);
                dgwLineas.Columns[1].Width = ColumnWith(dgwLineas, 30);
                dgwLineas.Columns[2].Width = ColumnWith(dgwLineas, 55);

                dgwLineas.Columns[0].ReadOnly = false;
                dgwLineas.Columns[1].ReadOnly = true;
                dgwLineas.Columns[2].ReadOnly = true;
            }
            
        }
        private void dgwLineas_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }


        #endregion

        #region regBotones

        private void AgregaLinea(string _asLinea, string _asTurno, string _asArea)
        {
            DataTable dt = dgwData.DataSource as DataTable;
            dt.Rows.Add(_lsParam,0,cbbPlanta.SelectedValue.ToString(),_asLinea,_asTurno,_lsArea);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(dgwLineas.RowCount > 0)
            {
                SuperLineaLogica sup = new SuperLineaLogica();
                sup.Supervisor = _lsParam;
                sup.Planta = cbbPlanta.SelectedValue.ToString();
                sup.Area = _lsArea;
                sup.Turno = _lsTurno;

                foreach (DataGridViewRow row in dgwLineas.Rows)
                {
                    bool bSelect = Convert.ToBoolean(row.Cells["ind"].Value);
                    if (!bSelect)
                        continue;

                    
                    string sLinea = Convert.ToString(row.Cells["linea"].Value);
                    sup.Linea = sLinea;
                    if (SuperLineaLogica.VerificaLineaSup(sup))
                    {
                        DialogResult Result = MessageBox.Show("La Linea esta asignada a otro Supervisor. Desea Reemplazar al Supervisor de la linea?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (Result == DialogResult.No)
                            return;
                    }
                    
                    AgregaLinea(sLinea,_lsTurno,_lsArea);
                }

                Close();
            }
        }

        #endregion

        private void wfLineasPta_FormClosing(object sender, FormClosingEventArgs e)
        {
            _dtReturn = dgwData.DataSource as DataTable;
        }
    }
}
