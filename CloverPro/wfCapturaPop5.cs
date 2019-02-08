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
    public partial class wfCapturaPop5 : Form
    {
        public string _sClave;
        private bool _bExit;
        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;
        public wfCapturaPop5()
        {
            InitializeComponent();
            
            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;

        }

        private void wfCapturaPop5_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        #region regResize
        private void wfCapturaPop5_Resize(object sender, EventArgs e)
        {
            CenterControl();

            //if (WindowState != _WindowStateAnt && WindowState != FormWindowState.Minimized)
            //{
            //    _WindowStateAnt = WindowState;
            //    ResizeControl(panel1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                
            //}
        }
        private void CenterControl()
        {
            int iW = this.Width / 2;
            int iH = this.Height / 2;

            int iW2 = panel1.Width / 2;
            int iH2 = panel1.Height / 2;

            int iX = iW - iW2;
            int iY = iH - iH2;

            panel1.Location = new Point(iX, iY);
            
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

        #endregion

        private void wfCapturaPop5_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!_bExit)
                e.Cancel = true;
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            _sClave = "C"; //capturar LIneup
            _bExit = true;
            Close();
        }

        private void btnEndScan_Click(object sender, EventArgs e)
        {
            DataTable dtConf = ConfigLogica.Consultar();
            string sItCod = dtConf.Rows[0]["codigo_bloq"].ToString();

            wfCapturaPop_1t wfPop = new wfCapturaPop_1t("PRO070");
            wfPop.ShowDialog();
            string sCve = wfPop._sClave;

            if (string.IsNullOrEmpty(sCve))
                return;

            if (sCve != sItCod)
                return;

            _sClave = "E"; //escanear etiquetas
            _bExit = true;
            Close();
        }
    }
}
