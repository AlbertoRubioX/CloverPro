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
    public partial class wfCoating : Form
    {
        public bool _lbCambio;
        private string _lsProceso = "CAT070";
        public wfCoating()
        {
            InitializeComponent();
        }
        #region regInicio
        private void wfCoating_Load(object sender, EventArgs e)
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

            Dictionary<string, string> Turno = new Dictionary<string, string>();
            Turno.Add("1", "1");
            Turno.Add("2", "2");
            cbbTurno.DataSource = new BindingSource(Turno, null);
            cbbTurno.DisplayMember = "Value";
            cbbTurno.ValueMember = "Key";
            cbbTurno.SelectedIndex = 0;

            cbbLinea.ResetText();
            
            dgwLineas.Columns.Clear();
            CargarLineas();

            cbbPlanta.Focus();
        }

        private void wfCoating_Activated(object sender, EventArgs e)
        {
            cbbPlanta.Focus();
        }
        #endregion

        #region regLinea
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
            if (dgwLineas.Rows.Count == 0)
            {
                DataTable dtNew = new DataTable("Lineas");
                dtNew.Columns.Add("supevisor", typeof(string));
                dtNew.Columns.Add("consec", typeof(int));
                dtNew.Columns.Add("Modelo", typeof(string));
                dtNew.Columns.Add("RPO", typeof(string));
                dtNew.Columns.Add("%RPO", typeof(string));
                dtNew.Columns.Add("WIPER RPO", typeof(string));
                dtNew.Columns.Add("ENTRADA", typeof(string));
                dtNew.Columns.Add("BALANCE RPO INI", typeof(string));
                dtNew.Columns.Add("SALIDA DE MAT. NUEVO", typeof(string));
                dtNew.Columns.Add("SALIDA DE MAT. DEM", typeof(string));
                dtNew.Columns.Add("TOTAL", typeof(string));
                dtNew.Columns.Add("BALANCE FINAL", typeof(string));
                dgwLineas.DataSource = dtNew;
            }

            dgwLineas.Columns[0].Visible = false;
            dgwLineas.Columns[1].Visible = false;
            

            //dgwLineas.Columns[2].Width = ColumnWith(dgwLineas, 40);
            //dgwLineas.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwLineas.Columns[3].Width = ColumnWith(dgwLineas, 45);
            //dgwLineas.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgwLineas.Columns[4].Width = ColumnWith(dgwLineas, 10);
            //dgwLineas.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        #endregion

        #region regGuardar
        private bool Guardar()
        {
            if (Valida())
            {

                try
                {
                    foreach(DataGridViewRow row in dgwLineas.Rows)
                    {
                        SuperLineaLogica sup = new SuperLineaLogica();
                       
                        sup.Consec = Convert.ToInt16(row.Cells[1].Value.ToString());
                        sup.Planta = row.Cells[2].Value.ToString();
                        sup.Linea = row.Cells[3].Value.ToString();
                        sup.Turno = row.Cells[4].Value.ToString();
                        sup.Area = row.Cells[5].Value.ToString();
                        SuperLineaLogica.Guardar(sup);
                    }
                    return true;
                }
                catch (Exception ie)
                {
                    MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "EstacionLogica.Guardar(est)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
                return false;
        }
        private bool Valida()
        {
            bool bValida = false;

            if (!_lbCambio)
                return bValida;

           

            if(dgwLineas.RowCount == 0)
            {
                MessageBox.Show("No se han cargado Lineas al Supervisor", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgwLineas.Focus();
                return bValida;
            }
            
            return true;
        }
        #endregion

        #region regBotones
        private void btSave_Click(object sender, EventArgs e)
        {
            if (Guardar())
                CargarLineas();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void btNew_Click(object sender, EventArgs e)
        {
            Inicio();
        }
        private void btRemove_Click(object sender, EventArgs e)
        {
           

            if (dgwLineas.SelectedRows.Count == 0)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "30") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult Result = MessageBox.Show("Desea Eliminar la Linea del Supervisor?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                try
                {
                    string sSuper = dgwLineas.SelectedCells[0].Value.ToString();
                    if (!string.IsNullOrEmpty(sSuper))
                    {
                        SuperLineaLogica sup = new SuperLineaLogica();
                        sup.Supervisor = sSuper;
                        sup.Consec = Convert.ToInt16(dgwLineas.SelectedCells[1].Value.ToString());

                        SuperLineaLogica.Eliminar(sup);
                    }
                    dgwLineas.Rows.Remove(dgwLineas.CurrentRow);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           
            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult Result = MessageBox.Show("Desea borrar al Supervisor?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                try
                {
               
               

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

            }
        }
        #endregion
       
        #region regCaptura
      

        private void cbbEstacion_KeyDown(object sender, KeyEventArgs e)
        {
        
        }

        private void cbbEstacion_SelectionChangeCommitted(object sender, EventArgs e)
        {
       
            
            CargarLineas();
        }
        
        private void AgregaLinea(string _asPlanta, string _asLinea, string _asTurno, string _asArea)
        {
            //DataTable dt = dgwLineas.DataSource as DataTable;
            //dt.Rows.Add(cbbSuper.SelectedValue.ToString(), 0, _asPlanta, _asLinea, _asTurno, _asArea);
            //_lbCambio = true;
        }
        private void btnLineas_Click(object sender, EventArgs e)
        {
            

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "40") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

           
        }
        #endregion

        private void cbbPlanta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbbPlanta.SelectedIndex != -1)
            {
                

                LineaLogica line = new LineaLogica();
                line.Planta = cbbPlanta.SelectedValue.ToString();
                DataTable dtL1 = LineaLogica.LineaPlanta(line);
          
                cbbLinea.DataSource = dtL1;
                cbbLinea.ValueMember = "linea";
                cbbLinea.DisplayMember = "linea";
                cbbLinea.SelectedIndex = -1;
            }
            
            
        }

    

        private void cbbTurno_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(cbbPlanta.SelectedIndex != -1 && cbbLinea.SelectedIndex != -1)
            {
                
            }
        }

        private void cbbArea_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(cbbPlanta.SelectedIndex != -1)
            {
               
            }
        }

        private void cbbPlanta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
