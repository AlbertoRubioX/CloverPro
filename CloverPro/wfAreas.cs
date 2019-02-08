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
    public partial class wfAreas : Form
    {
        public bool _lbCambio;
        private string _lsProceso = "PLA010";
        public wfAreas()
        {
            InitializeComponent();
        }
        #region regInicio
        private void wfAreas_Load(object sender, EventArgs e)
        {
            Inicio();
        }

       
        private void Inicio()
        {
            _lbCambio = false;

            cbbDefecto.ResetText();
            DataTable dt = AreasLogica.Listar();
            cbbDefecto.DataSource = dt;
            cbbDefecto.ValueMember = "area";
            cbbDefecto.DisplayMember = "area";
            cbbDefecto.SelectedIndex = -1;

            txtDescrip.Clear();

            cbbDefecto.Focus();
            dgwContacto.DataSource = null;
            CargarColumnas();
        }
        

        private void wfAreas_Activated(object sender, EventArgs e)
        {
            cbbDefecto.Focus();
        }

        #endregion

        #region regSave
        private bool Valida()
        {
            bool bValida = false;
            if (string.IsNullOrEmpty(cbbDefecto.Text))
            {
                return bValida;
            }
            if (string.IsNullOrEmpty(txtDescrip.Text))
            {
                MessageBox.Show("No se a especificado la Descripción del Area", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbDefecto.Focus();
                return bValida;
            }

            return true;
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            if (Guardar())
                Close();
            else
                Inicio();
        }

        private bool Guardar()
        {
            if (Valida())
            {

                try
                {
                    AreasLogica area = new AreasLogica();
                    area.Area = cbbDefecto.Text.ToString();
                    area.Descrip = txtDescrip.Text.ToString().TrimEnd();
                    if (chbAct.Checked)
                        area.ActPrev = "1";
                    else
                        area.ActPrev = "0";
                    area.Orden = txtOrden.Text.ToString();
                    area.Usuario = GlobalVar.gsUsuario;
                    
                    if (AreasLogica.Guardar(area) > 0)
                    {
                        foreach (DataGridViewRow row in dgwContacto.Rows)
                        {
                            if (row.Index == dgwContacto.Rows.Count - 1)
                                break;

                            if (dgwContacto.IsCurrentRowDirty)
                                dgwContacto.CommitEdit(DataGridViewDataErrorContexts.Commit);

                            if (row.Cells[2].Value == null)
                                continue;

                            
                            int iCons = 0;
                            if (int.TryParse(row.Cells[1].Value.ToString(), out iCons))
                                iCons = Convert.ToInt32(row.Cells[1].Value);

                            string sCorreo = Convert.ToString(row.Cells[2].Value);
                            string sTurno = Convert.ToString(row.Cells[3].Value);
                            string sTipo = Convert.ToString(row.Cells[4].Value);

                            AreasDestLogica mail = new AreasDestLogica();
                            mail.Area = area.Area;
                            mail.Consec = iCons;
                            mail.Correo = sCorreo;
                            mail.Turno = sTurno;
                            mail.Tipo = sTipo;
                            mail.Envio = "1";
                            mail.Usuario = GlobalVar.gsUsuario;
                            AreasDestLogica.Guardar(mail);
                        }

                        foreach (DataGridViewRow row in dgwAsignar.Rows)
                        {
                            if (row.Index == dgwAsignar.Rows.Count - 1)
                                break;

                            if (dgwAsignar.IsCurrentRowDirty)
                                dgwAsignar.CommitEdit(DataGridViewDataErrorContexts.Commit);

                            if (row.Cells[2].Value == null)
                                continue;


                            int iCons = 0;
                            if (int.TryParse(row.Cells[1].Value.ToString(), out iCons))
                                iCons = Convert.ToInt32(row.Cells[1].Value);

                            string sCorreo = Convert.ToString(row.Cells[2].Value);
                            string sTurno = Convert.ToString(row.Cells[3].Value);

                            AreasDestLogica mail = new AreasDestLogica();
                            mail.Area = area.Area;
                            mail.Consec = iCons;
                            mail.Correo = sCorreo;
                            mail.Turno = sTurno;
                            mail.Tipo = "T";
                            mail.Envio = "0";
                            mail.Usuario = GlobalVar.gsUsuario;
                            AreasDestLogica.Guardar(mail);
                        }

                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error al intentar guardar el area", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch(Exception ie)
                {
                    MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "AreasLogica.Guardar(est)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
                return false;
        }
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region regCaptura

        private void cbbDefecto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(cbbDefecto.Text) && !string.IsNullOrWhiteSpace(cbbDefecto.Text))
            {
                AreasLogica area = new AreasLogica();
                cbbDefecto.Text = cbbDefecto.Text.ToUpper().ToString();
                area.Area = cbbDefecto.Text.ToString();
                DataTable datos = AreasLogica.Consultar(area);
                if (datos.Rows.Count != 0)
                {
                    txtDescrip.Text = datos.Rows[0]["descrip"].ToString();
                    if (datos.Rows[0]["ind_actprev"].ToString() == "1")
                        chbAct.Checked = true;
                    else
                        chbAct.Checked = false;
                    txtOrden.Text = datos.Rows[0]["orden"].ToString();

                    AreasDestLogica dest = new AreasDestLogica();
                    dest.Area = area.Area;
                    dest.Envio = "1";
                    dgwContacto.DataSource = AreasDestLogica.Listar(dest);
                    dest.Envio = "0";
                    dgwAsignar.DataSource = AreasDestLogica.Listar(dest);
                    CargarColumnas();
                }
                else
                {
                    Inicio();
                    cbbDefecto.Text = area.Area;
                }
            }
            
            
        }

        private void cbbDefecto_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AreasLogica area = new AreasLogica();
            area.Area = cbbDefecto.SelectedValue.ToString();
            DataTable datos = AreasLogica.Consultar(area);
            if (datos.Rows.Count != 0)
            {
                txtDescrip.Text = datos.Rows[0]["descrip"].ToString();
                if (datos.Rows[0]["ind_actprev"].ToString() == "1")
                    chbAct.Checked = true;
                else
                    chbAct.Checked = false;
                txtOrden.Text = datos.Rows[0]["orden"].ToString();

                AreasDestLogica dest = new AreasDestLogica();
                dest.Area = area.Area;
                dgwContacto.DataSource = AreasDestLogica.Listar(dest);
                CargarColumnas();
            }
            else
            {
                Inicio();
                cbbDefecto.Text = area.Area;
            }
        }

        #endregion

        #region regBoton
        private void btNew_Click(object sender, EventArgs e)
        {
            Inicio();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cbbDefecto.SelectedIndex == -1)
                return;

            if (tabControl1.SelectedIndex == 1)
            {
                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (dgwContacto.CurrentCell == null)
                    return;
                else
                {
                    int iRow = dgwContacto.CurrentCell.RowIndex;
                    if (string.IsNullOrEmpty(dgwContacto[2, iRow].Value.ToString()))
                        return;

                    try
                    {
                        int iConsec = 0;
                        if(int.TryParse(dgwContacto[1, iRow].Value.ToString(), out iConsec))
                        {
                            AreasDestLogica dest = new AreasDestLogica();
                            dest.Area = cbbDefecto.SelectedValue.ToString();
                            dest.Consec = iConsec;
                            AreasDestLogica.Eliminar(dest);
                        }
                        
                        dgwContacto.Rows.Remove(dgwContacto.CurrentRow);
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "AreasLogica.Eliminar(est)" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cbbDefecto.SelectedIndex == -1)
                return;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "20") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult Result = MessageBox.Show("Desea borrar el Area ?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            if (Result == DialogResult.Yes)
            {
                AreasLogica area = new AreasLogica();
                area.Area = cbbDefecto.SelectedValue.ToString();
                try
                {
                    if (AreasLogica.AntesDeBorrar(area))
                    {
                        MessageBox.Show("No puede borrar el Area debido a que se encuentra registrado realacionado en el sistema", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    AreasLogica.Eliminar(area);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "" + Environment.NewLine + ex.ToString(), "Error " + Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Inicio();
            }

        }

        #endregion

        #region regColor
        private void txtDescrip_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtDescrip, 0);
        }

        private void txtDescrip_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtDescrip, 1);
        }

        private void cbbDefecto_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbDefecto, 0);
        }

        private void cbbDefecto_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(cbbDefecto, 1);
        }
        private void txtOrden_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtOrden, 1);
        }

        private void txtOrden_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtOrden, 0);
        }

        #endregion

        #region regContactos
        private void dgwContacto_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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
        private void CargarColumnas()
        {
            DataTable dtNew = new DataTable("Mail");
            dtNew.Columns.Add("area", typeof(int));
            dtNew.Columns.Add("consec", typeof(int));
            dtNew.Columns.Add("CORREO", typeof(string));
            dtNew.Columns.Add("TURNO", typeof(string));
            dtNew.Columns.Add("TIPO", typeof(string));
            dtNew.Columns.Add("envia_auto", typeof(string));

            int iRows = dgwContacto.Rows.Count;
            if (iRows == 0)
                dgwContacto.DataSource = dtNew;

            iRows = dgwAsignar.Rows.Count;
            if (iRows == 0)
                dgwAsignar.DataSource = dtNew;
            

            dgwContacto.Columns[0].Visible = false;
            dgwContacto.Columns[1].Visible = false;
            dgwContacto.Columns[5].Visible = false;

            dgwContacto.Columns[2].Width = ColumnWith(dgwContacto, 70);
            dgwContacto.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwContacto.Columns[3].Width = ColumnWith(dgwContacto, 15);
            dgwContacto.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwContacto.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwContacto.Columns[4].Width = ColumnWith(dgwContacto, 15);
            dgwContacto.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwContacto.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwAsignar.Columns[0].Visible = false;
            dgwAsignar.Columns[1].Visible = false;
            dgwAsignar.Columns[4].Visible = false;
            dgwAsignar.Columns[5].Visible = false;

            dgwAsignar.Columns[2].Width = ColumnWith(dgwContacto, 80);
            dgwAsignar.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwAsignar.Columns[3].Width = ColumnWith(dgwContacto, 20);
            dgwAsignar.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwAsignar.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            


        }

        private void dgwAsignar_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightSkyBlue;
            else
                e.CellStyle.BackColor = Color.White;
        }

        #endregion


    }
}
