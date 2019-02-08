using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using Logica;

namespace CloverPro
{
    public partial class wfDashboardRPO : Form
    {
        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;

        private string _lsArea = string.Empty;
        private string _lsPlanta = string.Empty;
        private string _lsTurno = string.Empty;
        private string _lsGlobal = string.Empty;
        private string _lsEstacion = string.Empty;
        public wfDashboardRPO()
        {
            InitializeComponent();

            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }

        private void wfDashboardRPO_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            _lsPlanta = GlobalVar.gsPlanta;
            
            _lsTurno = GlobalVar.TurnoGlobal();

            _lsArea = "A";
            CargarDatos(_lsArea);
            timer1.Start();
        }

        private void wfDashboardRPO_Resize(object sender, EventArgs e)
        {
            if (WindowState != _WindowStateAnt && WindowState != FormWindowState.Minimized)
            {
                _WindowStateAnt = WindowState;

                ResizeControl(panel1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(chtBarras, 3, ref _iWidthAnt, ref _iHeightAnt, 1);

                int iW = this.Width - 70;
                btnClose.Location = new Point(iW, btnClose.Location.Y);
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
            if (ai_Retorna == 1)
            {
                ai_WidthAnt = this.Width;
                ai_HegihtAnt = this.Height;
            }
        }

        private void CargarDatos(string _asTipo)
        {
            try
            {
                DataTable datos = new DataTable();

                string sMetaGlobal = string.Empty;
                string sRealGlobal = string.Empty;
                string sCumpTotal = string.Empty;

                ControlRpoLogica rpo = new ControlRpoLogica();
                rpo.Planta = _lsPlanta;
                string sFolio = string.Empty;

                datos = ControlRpoLogica.ConsultarFolio(rpo);
                if (datos.Rows.Count > 0)
                {
                    sFolio = datos.Rows[0][0].ToString();
                }
                long lFolio = 0;
                if (!long.TryParse(sFolio, out lFolio))
                {
                    Cursor = Cursors.Default;
                    return;
                }

                rpo.Fecha = DateTime.Today;
                rpo.Folio = lFolio;
                rpo.Turno = _lsTurno;
                if(_asTipo == "A")
                    datos = ControlRpoLogica.ConsultaVista(rpo);    
                else
                    datos = ControlRpoLogica.ConsultaVistaEti(rpo);

                if (datos.Rows.Count != 0)
                {
                    int iEsp = 0;
                    int iTotal = 0;

                    DataTable dtE = ControlRpoLogica.ConsultaEspera(rpo);
                    iEsp = int.Parse(dtE.Rows[0][0].ToString());

                    dtE = ControlRpoLogica.ConsultaTurno(rpo);
                    iTotal = int.Parse(dtE.Rows[0][0].ToString());

                    chtBarras.Series[0].Points.Clear();
                    
                    if(_asTipo == "A")
                        chtBarras.Series[0].LegendText = "ALMACEN";
                    else
                        chtBarras.Series[0].LegendText = "ETIQUETAS";

                    chtBarras.Palette = ChartColorPalette.SeaGreen;
                    chtBarras.Annotations.Clear();

                    chtBarras.ChartAreas["ChartArea1"].AxisX.Title = "ESTATUS DE ARMADO";//X
                    chtBarras.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font("Times New Roman", 14, FontStyle.Bold);
                    chtBarras.ChartAreas["ChartArea1"].AxisY.Title = "AVANCE DE ARMADO EN TURNO " + _lsTurno;//Y
                    chtBarras.ChartAreas["ChartArea1"].AxisY.Maximum = iTotal;
                    chtBarras.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font("Times New Roman", 14, FontStyle.Bold);

                    string sProc = datos.Rows[0][0].ToString();
                    string sListo = datos.Rows[0][1].ToString();
                    string sDet = datos.Rows[0][2].ToString();
                    string sEsp = string.Empty;
                    string sEnt = string.Empty;
                    int iRows = 4;
                    if (_asTipo == "A")
                        sEsp = datos.Rows[0][3].ToString();
                    else
                        sEnt = datos.Rows[0][3].ToString();
                    
                    for (int x = 0; x < iRows; x++)
                    {
                        string sSerie = datos.Columns[x].ColumnName.ToString();

                        if(_asTipo == "A" && x == 3)
                            datos.Rows[0][x] = iEsp;

                        chtBarras.Series[0].Points.AddXY(sSerie, datos.Rows[0][x].ToString());
                        
                        string sAnt = string.Empty;
                        double dPorc = double.Parse(datos.Rows[0][x].ToString());
                        double dPorcj = (dPorc / iTotal) * 100;
                        dPorcj = Math.Round(dPorcj, 2);

                        if (dPorcj > 0)
                        {
                            sAnt = dPorcj.ToString() + " %";
                            chtBarras.Series[0].Points[x].Label = sAnt;
                        }

                        chtBarras.Series[0].Font = new Font("Calibri", 16F, FontStyle.Bold);
                        chtBarras.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Calibri", 14F, FontStyle.Bold);
                        chtBarras.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                        chtBarras.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Calibri", 12F, FontStyle.Bold);

                        TextAnnotation tA = new TextAnnotation();
                        tA.Font = new Font("Calibri", 14F, FontStyle.Bold);
                        sAnt = datos.Rows[0][x].ToString();
                        tA.Text = sAnt;
                        tA.SetAnchor(chtBarras.Series[0].Points[x]);
                        chtBarras.Annotations.Add(tA);


                        if (x == 0)
                            chtBarras.Series[0].Points[x].Color = Color.DodgerBlue;
                        if (x == 1)
                            chtBarras.Series[0].Points[x].Color = Color.LightGreen;
                        if (x == 2)
                            chtBarras.Series[0].Points[x].Color = Color.IndianRed;
                        if (x == 3 && _asTipo == "A")
                            chtBarras.Series[0].Points[x].Color = Color.LightGray;
                        if (x == 3 && _asTipo == "E")
                            chtBarras.Series[0].Points[x].Color = Color.Yellow;
                        

                        chtBarras.Series[0].SmartLabelStyle.Enabled = true;
                        chtBarras.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                        chtBarras.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                    }
                    
                    chtBarras.Titles.Clear();
                    chtBarras.Titles.Add("RPO'S DEL TURNO: " + iTotal); //TITLE TOP
                    chtBarras.Titles[0].Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);

                    
                }
                else
                {
                    MessageBox.Show("No se encontro informacion para mostrar", "CloverCES Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + "Dashboard" + Environment.NewLine + "LoadChart.." + ex.ToString(), "ERROR " + Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_lsArea == "A")
                _lsArea = "E";
            else
                _lsArea = "A";

            CargarDatos(_lsArea);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
