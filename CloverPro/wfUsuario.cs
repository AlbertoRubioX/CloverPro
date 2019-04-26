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
    public partial class wfUsuario : Form
    {
        public bool _lbCambio;
        private bool _lbEntra;
        private string _lsProceso = "CAT010";

        public wfUsuario()
        {
            InitializeComponent();
        }

        private void wfUsuario_Load(object sender, EventArgs e)
        {
            Inicio();
        }

        private void Inicio()
        {
            _lbCambio = false;

            cbbUsuario.ResetText();
            DataTable dtU = UsuarioLogica.Listar();
            cbbUsuario.DataSource = dtU;
            cbbUsuario.ValueMember = "usuario";
            cbbUsuario.DisplayMember = "usuario";
            cbbUsuario.SelectedIndex = -1;

            chbActivo.Checked = true;
            txtNombre.Clear();
            txtCorreo.Clear();

            cbbTurno.ResetText();
            Dictionary<string, string> Turno = new Dictionary<string, string>();
            Turno.Add("1", "1");
            Turno.Add("2", "2");
            cbbTurno.DataSource = new BindingSource(Turno, null);
            cbbTurno.DisplayMember = "Value";
            cbbTurno.ValueMember = "Key";
            cbbTurno.SelectedIndex = 0;

            cbbPlanta.ResetText();
            DataTable data = PlantaLogica.Listar();
            cbbPlanta.DataSource = data;
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.SelectedIndex = -1;

            cbbArea.ResetText();
            DataTable dtA = AreasLogica.Listar();
            cbbArea.DataSource = dtA;
            cbbArea.DisplayMember = "descrip";
            cbbArea.ValueMember = "Area";
            cbbArea.SelectedIndex = -1;

            cbbSup.ResetText();
            DataTable dtSp = UsuarioLogica.ListarSupGral();
            cbbSup.DataSource = dtSp;
            cbbSup.ValueMember = "usuario";
            cbbSup.DisplayMember = "nombre";
            cbbSup.SelectedIndex = -1;


            cbxAcceso.Checked = false;
            ListarPermisos(null);

            cbbUsuario.Focus();

        }

        public void ListarPermisos(string as_usuario)
        {
            string sProceso;
            int i = 0;

            trvPermisos.Nodes.Clear();

            DataTable permisos = new DataTable();

            permisos = UsuarioLogica.ListarAccesos(2);

            foreach (DataRow dr in permisos.Rows)
            {
                sProceso = dr["proceso"].ToString() + "00";
                trvPermisos.Nodes.Add(dr["descrip"].ToString());
                trvPermisos.Nodes[i].Name = sProceso;

                if (UsuarioLogica.VerificarPermiso(as_usuario, sProceso))
                    trvPermisos.Nodes[i].Checked = true;

                DataTable usuaper = new DataTable();

                usuaper = UsuarioLogica.ListarPermisos(dr["proceso"].ToString());

                int ni = 0;
                foreach (DataRow dp in usuaper.Rows)
                {
                    trvPermisos.Nodes[i].Nodes.Add(dp["descrip"].ToString());
                    trvPermisos.Nodes[i].Nodes[ni].Name = dp["operacion"].ToString();

                    if (UsuarioLogica.VerificarPermiso(as_usuario, dp["operacion"].ToString()))
                        trvPermisos.Nodes[i].Nodes[ni].Checked = true;
                    ni++;
                }
                i++;
            }
        }
        private bool Valida()
        {
            bool bValida = false;

            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "10") == false)
            {
                MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bValida;
            }

            if (string.IsNullOrEmpty(cbbUsuario.Text))
            {
                MessageBox.Show("No se a especificado la clave del Usuario", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbUsuario.Focus();
                return bValida;
            }
            else
            {
                if(cbbArea.SelectedValue.ToString() != "SPG")
                {
                    string sUsuario = cbbUsuario.Text.ToUpper().Trim().ToString();
                    if (sUsuario.Length > 6 && sUsuario!="ADMINPRO")
                    {
                        MessageBox.Show("La clave del Usuario no coincide con el número de empleado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbbUsuario.Focus();
                        return bValida;
                    }
                }
                
            }

            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("No se a especificado el Nombre del Usuario", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Focus();
                return bValida;
            }

            if (cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado la Planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }

            if (cbbArea.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado el Area", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbArea.Focus();
                return bValida;
            }

            return true;
        }

        private void wfUsuario_Activated(object sender, EventArgs e)
        {
            cbbUsuario.Focus();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (Guardar())
                Inicio();
        }

        private bool Guardar()
        {
            if (Valida())
            {

                try
                {
                    UsuarioLogica user = new UsuarioLogica();
                    user.Usuario = cbbUsuario.Text.ToString().ToUpper();
                    user.Nombre = txtNombre.Text.ToString();
                    if (chbActivo.Checked)
                        user.Activo = "1";
                    else
                        user.Activo = "0";
                    user.Correo = txtCorreo.Text.ToString();
                    user.Planta = cbbPlanta.SelectedValue.ToString();
                    user.Area = cbbArea.SelectedValue.ToString();
                    if (user.Planta == "CES")
                        user.Modulo = "CES";
                    if (user.Planta == "FUS" || user.Planta == "COL" || user.Planta == "TON1" || user.Planta == "NIC2" || user.Planta == "NIC3")
                        user.Modulo = "PRO";
                    if (user.Planta == "EMP" || user.Planta == "EMPN")
                        user.Modulo = "EMP";
                    user.Turno = cbbTurno.SelectedValue.ToString();
                    if(cbbSup.SelectedIndex != -1)
                        user.SupGral = cbbSup.SelectedValue.ToString();
                    user.UserId = GlobalVar.gsUsuario;

                    if (!GuardarPermisos(user))
                    {
                        MessageBox.Show("Error al intentar guardar permisos de usuario", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (UsuarioLogica.Guardar(user) > 0)
                        return true;
                    else
                    {
                        MessageBox.Show("Error al intentar guardar el Usuario", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch(Exception ie)
                {
                    MessageBox.Show("Favor de notificar al Administrador" + Environment.NewLine + "UsuarioLogica.Guardar(user)" + Environment.NewLine + ie.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
                return false;
        }

        public bool GuardarPermisos(UsuarioLogica userP)
        {
            string sProceso = "";
            string sOperacion = "";
            string sPermiso = "";

            for (int i = 0; i < trvPermisos.Nodes.Count; i++)
            {
                sOperacion = trvPermisos.Nodes[i].Name;
                sProceso = sOperacion.Substring(0, 6);

                if (trvPermisos.Nodes[i].Checked)
                    sPermiso = "1";
                else
                    sPermiso = "0";

                userP.Proceso = sProceso;
                userP.Operacion = sOperacion;
                userP.Permiso = sPermiso;
                if (UsuarioLogica.GuardarPermiso(userP) == 0)
                    return false;

                for (int ni = 0; ni < trvPermisos.Nodes[i].Nodes.Count; ni++)
                {
                    sOperacion = trvPermisos.Nodes[i].Nodes[ni].Name;
                    if (trvPermisos.Nodes[i].Nodes[ni].Checked)
                        sPermiso = "1";
                    else
                        sPermiso = "0";

                    userP.Proceso = sProceso;
                    userP.Operacion = sOperacion;
                    userP.Permiso = sPermiso;
                    if (UsuarioLogica.GuardarPermiso(userP) == 0)
                        return false;
                }
            }
            return true;
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            if (cbbPlanta.SelectedIndex == -1)
                return;

            DialogResult Result = MessageBox.Show("Desea borrar el Usuario?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.Yes)
            {
                try
                {
                    UsuarioLogica user = new UsuarioLogica();
                    user.Usuario = cbbUsuario.SelectedValue.ToString();

                    if (UsuarioLogica.AntesDeEliminar(user))
                    {
                        MessageBox.Show("El Usuario no se puede borrar, debido a que cuenta con movimientos en el Sistema", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if(UsuarioLogica.Eliminar(user))
                    {
                        MessageBox.Show("El Usuario ha sido borrado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Inicio();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Inicio();
        }

        private void cbbUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(cbbUsuario.Text) && !string.IsNullOrWhiteSpace(cbbUsuario.Text))
            {
                UsuarioLogica user = new UsuarioLogica();
                string sUsuario = cbbUsuario.Text.ToUpper().Trim().ToString();

                if (sUsuario.Length < 6)
                    sUsuario = sUsuario.PadLeft(6, '0');

                user.Usuario = sUsuario;
                DataTable datos = UsuarioLogica.Consultar(user);
                if (datos.Rows.Count != 0)
                {
                    cbbUsuario.SelectedValue = datos.Rows[0]["usuario"].ToString();
                    txtNombre.Text = datos.Rows[0]["nombre"].ToString();
                    if (datos.Rows[0]["activo"].ToString() == "1")
                        chbActivo.Checked = true;
                    else
                        chbActivo.Checked = false;
                    txtCorreo.Text = datos.Rows[0]["correo"].ToString();
                    cbbPlanta.SelectedValue = datos.Rows[0]["planta"].ToString();
                    cbbArea.SelectedValue = datos.Rows[0]["area"].ToString();
                    cbbTurno.SelectedValue = datos.Rows[0]["turno"].ToString();
                    if (!string.IsNullOrEmpty(datos.Rows[0]["sup_gral"].ToString()))
                        cbbSup.SelectedValue = datos.Rows[0]["sup_gral"].ToString();

                    ListarPermisos(sUsuario);
                }
                else
                {
                    Inicio();
                    cbbUsuario.Text = sUsuario;

                    OperadorLogica oper = new OperadorLogica();
                    oper.Operador = sUsuario;
                    DataTable dtOp = OperadorLogica.Consultar(oper);
                    if(dtOp.Rows.Count > 0)
                    {
                        txtNombre.Text = dtOp.Rows[0]["nombre"].ToString();
                        if (dtOp.Rows[0]["activo"].ToString() == "1")
                            chbActivo.Checked = true;
                        else
                            chbActivo.Checked = false;
                        cbbPlanta.SelectedValue = dtOp.Rows[0]["planta"].ToString();
                        cbbArea.SelectedValue = "PRO";
                        cbbTurno.SelectedValue = dtOp.Rows[0]["turno"].ToString();
                        cbbSup.SelectedIndex = -1;
                    }                    
                }
                txtNombre.Focus();
            }
            else
                cbbUsuario.SelectedIndex = -1;
        }

        private void cbbUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void cbbUsuario_SelectionChangeCommitted(object sender, EventArgs e)
        {
            UsuarioLogica user = new UsuarioLogica();
            user.Usuario = cbbUsuario.SelectedValue.ToString();

            DataTable datos = UsuarioLogica.Consultar(user);
            if (datos.Rows.Count != 0)
            {
                txtNombre.Text = datos.Rows[0]["nombre"].ToString();
                if (datos.Rows[0]["activo"].ToString() == "1")
                    chbActivo.Checked = true;
                else
                    chbActivo.Checked = false;
                txtCorreo.Text = datos.Rows[0]["correo"].ToString();
                cbbPlanta.SelectedValue = datos.Rows[0]["planta"].ToString();
                cbbArea.SelectedValue = datos.Rows[0]["area"].ToString();
                cbbTurno.SelectedValue = datos.Rows[0]["turno"].ToString();
                if (!string.IsNullOrEmpty(datos.Rows[0]["sup_gral"].ToString()))
                    cbbSup.SelectedValue = datos.Rows[0]["sup_gral"].ToString();

                ListarPermisos(user.Usuario);

                txtNombre.Focus();
            }
        }

        private void trvPermisos_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_lbEntra) return;
            _lbEntra = true;
            try
            {
                checkNodes(e.Node, e.Node.Checked);
            }
            finally
            {
                _lbEntra = false;
            }

            _lbCambio = true;
        }

        private void checkNodes(TreeNode node, bool check)
        {
            foreach (TreeNode child in node.Nodes)
            {
                child.Checked = check;
                checkNodes(child, check);
            }
        }

        private void cbxAcceso_CheckedChanged(object sender, EventArgs e)
        {
            _lbCambio = true;

            foreach (TreeNode node in trvPermisos.Nodes)
            {
                node.Checked = cbxAcceso.Checked;
            }
        }

        private void cbbArea_SelectionChangeCommitted(object sender, EventArgs e)
        {
            bool bNuevo = false;
            foreach (TreeNode node in trvPermisos.Nodes)
            {
                if(node.Checked)
                {
                    bNuevo = true;
                    break;
                }
            }

            if (!bNuevo)
                return;

            if (cbbArea.SelectedValue.ToString() == "IT")
            {
                cbxAcceso.Checked = true;
                return;
            }

            if(cbbArea.SelectedValue.ToString() == "SUP")
            {
                string sModulo = string.Empty;
                string sPlanta = cbbPlanta.SelectedValue.ToString();
                if (sPlanta == "CES")
                    sModulo = "CES";

                if (sPlanta == "COL" || sPlanta == "TON1" || sPlanta == "TON2")
                    sModulo = "PRO";

                if (sPlanta == "EMP" || sPlanta == "EMPN")
                    sModulo = "EMP";

                foreach (TreeNode node in trvPermisos.Nodes)
                {
                    if (node.Name.Substring(0,3) == sModulo)
                    {
                        node.Checked = true;
                    }
                }
            }

        }
    }
}