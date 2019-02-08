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
    public partial class wfBusquedaPop : Form
    {
        public string _sClave;
        public string _lsLinea;
        public string _lsModelo;
        public double _ldStd1;
        public double _ldStd2;
        public double _ldHc;

        private bool _bExit;
        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;
        DataTable dtNew = new DataTable();
        public wfBusquedaPop()
        {
            InitializeComponent();
            
            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;

        }

        private void wfBusquedaPop_Load(object sender, EventArgs e)
        {
            //WindowState = FormWindowState.Maximized;
            btnLineUp.Text += " " + _lsLinea;
            CargarModelos();
            
        }
        private void CargarColumnas()
        {

            int iRows = dgwData.Rows.Count;

            if (iRows == 0)
            {
                dtNew.Columns.Add("MODELO", typeof(string));//0
                dtNew.Columns.Add("STD 1T", typeof(int));           //1
                dtNew.Columns.Add("STD 2T", typeof(int));         //2
                dtNew.Columns.Add("STD HC", typeof(int));//3
                dtNew.Columns.Add("FACTOR", typeof(double));//4
                dgwData.DataSource = dtNew;
            }
            else
                dgwData.Rows[0].Selected = false;

            dgwData.Columns[0].Width = ColumnWith(dgwData, 40);//INICIO
            dgwData.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[1].Width = ColumnWith(dgwData, 15);//INICIO
            dgwData.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[2].Width = ColumnWith(dgwData, 15);//INICIO
            dgwData.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[3].Width = ColumnWith(dgwData, 15);//INICIO
            dgwData.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgwData.Columns[4].Width = ColumnWith(dgwData, 15);//INICIO
            dgwData.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgwData.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //dgwData.ClearSelection();
            
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
        private void CargarModelos()
        {
            ModelohcLogica line = new ModelohcLogica();
            line.Planta = _sClave;
            line.Linea = _lsLinea;
            dgwData.DataSource = ModelohcLogica.ListarModelos(line);
            CargarColumnas();
        }

        #region regResize
        private void wfBusquedaPop_Resize(object sender, EventArgs e)
        {
            //CenterControl();

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

        private void wfBusquedaPop_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            //by line
            if (string.IsNullOrEmpty(_lsLinea))
                return;
            
            CargarModelos();

       
        }

        private void btnEndScan_Click(object sender, EventArgs e)
        {
            //all models by plant
            if (string.IsNullOrEmpty(_sClave)) 
                return;

            ModelohcLogica line = new ModelohcLogica();
            line.Planta = _sClave;
            dgwData.DataSource = ModelohcLogica.ListarModelosPta(line);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //dtNew.DefaultView.RowFilter = string.Format("MODELO LIKE '%{0}%'", textBox1.Text.ToString());
            (dgwData.DataSource as DataTable).DefaultView.RowFilter = string.Format("Modelo LIKE '{0}%' OR Modelo LIKE '% {0}%'", textBox1.Text);
        }

        private void dgwData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int iRow = e.RowIndex;
            if ((iRow % 2) == 0)
                e.CellStyle.BackColor = Color.LightGray;
            else
                e.CellStyle.BackColor = Color.White;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            _sClave = string.Empty;
            _lsLinea = string.Empty;
            _ldHc = 0;
            _ldStd1 = 0;
            _ldStd2 = 0;

            Close();
        }

        private void dgwData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string cellValue = dgwData.Rows[e.RowIndex].Cells[0].Value.ToString();
            if(!string.IsNullOrEmpty(cellValue))
            {
                _lsModelo = cellValue;
                
                if (!double.TryParse(dgwData.Rows[e.RowIndex].Cells[1].Value.ToString(), out _ldStd1))
                    _ldStd1 = 0;

                if (!double.TryParse(dgwData.Rows[e.RowIndex].Cells[2].Value.ToString(), out _ldStd2))
                    _ldStd2 = 0;

                if (!double.TryParse(dgwData.Rows[e.RowIndex].Cells[3].Value.ToString(), out _ldHc))
                    _ldHc = 0;

                Close();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (dgwData.Rows.Count == 1)
            {
                string cellValue = dgwData[0,0].Value.ToString();
                if (!string.IsNullOrEmpty(cellValue))
                {
                    _lsModelo = cellValue;

                    if (!double.TryParse(dgwData.Rows[0].Cells[1].Value.ToString(), out _ldStd1))
                        _ldStd1 = 0;

                    if (!double.TryParse(dgwData.Rows[0].Cells[2].Value.ToString(), out _ldStd2))
                        _ldStd2 = 0;

                    if (!double.TryParse(dgwData.Rows[0].Cells[3].Value.ToString(), out _ldHc))
                        _ldHc = 0;

                    Close();
                }
            }
        }

        private void wfBusquedaPop_Activated(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void dgwData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            

        }

        private void dgwData_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
        
            
        }
    }
}
