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
    public partial class wfActividadesPop : Form
    {
        public string _lsProceso;
        private long _lFolio;
        private int _iConsec;
        public string _sClave;
        
        public wfActividadesPop(string asFolio)
        {
            InitializeComponent();
            _sClave = asFolio;
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

        private void wfActividadesPop_Load(object sender, EventArgs e)
        {
            int iPos = _sClave.IndexOf("-");
            _lFolio = long.Parse(_sClave.Substring(0, iPos));
            _iConsec = int.Parse(_sClave.Substring(iPos + 1));

            Dictionary<string, string> L1 = new Dictionary<string, string>();
            for (int x = 1; x <= 12; x++)
            {
                string sVal = string.Format("{0:D2}", x);
                L1.Add(sVal, sVal);
            }
            cbbHora.DataSource = new BindingSource(L1, null);
            cbbHora.DisplayMember = "Value";
            cbbHora.ValueMember = "Key";
            cbbHora.SelectedIndex = -1;

            Dictionary<string, string> L2 = new Dictionary<string, string>();
            for(int x = 0; x < 60; x++)
            {
                string sMin = string.Format("{0:D2}",x);
                L2.Add(sMin, sMin);
            }
            
            cbbMin.DataSource = new BindingSource(L2, null);
            cbbMin.DisplayMember = "Value";
            cbbMin.ValueMember = "Key";
            cbbMin.SelectedIndex = 0;

            Dictionary<string, string> L3 = new Dictionary<string, string>();
            L3.Add("AM", "AM");
            L3.Add("PM", "PM");
            cbbTipo.DataSource = new BindingSource(L3, null);
            cbbTipo.DisplayMember = "Value";
            cbbTipo.ValueMember = "Key";
            cbbTipo.SelectedIndex = 0;

        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            _sClave = string.Empty;
            
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            try
            {
                if(GlobalVar.gsTurno == "2")
                {
                    int iHora = int.Parse(cbbHora.SelectedValue.ToString());
                    if (iHora >= 1 && iHora < 12 && cbbTipo.SelectedValue.ToString() == "AM")
                        return;
                    if (iHora == 12 && cbbTipo.SelectedValue.ToString() == "PM" || (iHora >= 1 && iHora <= 4 && cbbTipo.SelectedValue.ToString() == "PM"))
                        return;
                }
                _sClave = cbbHora.SelectedValue.ToString() + ":" + cbbMin.SelectedValue.ToString() + " " + cbbTipo.SelectedValue.ToString();

                if(_lsProceso == "EMP040")
                {

                    ControlRpoLogica rpo = new ControlRpoLogica();
                    rpo.Folio = _lFolio;
                    rpo.Consec = _iConsec;
                    rpo.Entrega = _sClave;
                    rpo.Usuario = GlobalVar.gsUsuario;
                    ControlRpoLogica.ActualizaEntrega(rpo);

                }
                else
                {
                    LineSetupDetLogica det = new LineSetupDetLogica();
                    det.Folio = _lFolio;
                    det.Consec = _iConsec;
                    det.IniProg = _sClave;
                    det.Usuario = GlobalVar.gsUsuario;
                    LineSetupDetLogica.ActualizaHoraIni(det);
                }

                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador " + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            Close();
        }

       private bool Valida()
        {
            bool bValida = false;

            if (cbbHora.SelectedIndex == -1)
            {
                cbbHora.Focus();
                return bValida;
            }

            if (cbbMin.SelectedIndex == -1)
            {
                cbbMin.Focus();
                return bValida;
            }

            return true;
        }

        private void cbbHora_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(cbbHora.SelectedIndex != -1)
            {
                int iHora = int.Parse(cbbHora.SelectedValue.ToString());
                if (iHora < 6)
                    cbbTipo.SelectedIndex = 1;
            }
        }
    }
}
