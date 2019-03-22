using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logica;

namespace CloverPro
{
    public partial class wfImprimeCaratulas : Form
    {
        public string sRpo;
        public string sPlanta;
        public string sLinea;
        public string sAlma;
        public string sModelo;
        public string sCant;
        public string sColor1;
        public string sColor2;
        public string diaesp;
        public string sPrioridad;

        public wfImprimeCaratulas()
        {
            
            InitializeComponent();
        }

        private void wfImprimeCaratulas_Load(object sender, EventArgs e)
        {
            nud_Cantidad.Select(0, nud_Cantidad.Value.ToString().Length);

            CultureInfo dia = new CultureInfo("es-Es");
            diaesp = dia.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek).ToUpper();
            
            if (!string.IsNullOrEmpty(sPrioridad.Trim())){
                diaesp = sPrioridad;
                if (sPrioridad.Equals("GLOBAL")|| sPrioridad.Equals("HOT"))
                 {
                    sColor1 = "#FF0000";
                    sColor2 = sColor1;
                }
                if (sPrioridad.Equals("BACK ORDER"))
                {
                    sColor1 = "#FF0000";
                    sColor2 = "#FFFF00";
                }
                if(sPrioridad.Equals("PAST DUE"))
                {
                    sColor1 = "#F4B084";
                    sColor2 = sColor1;
                }
            }
            else { 
                if (diaesp.Equals("LUNES"))
                {
                    sColor1 = "#FFFF00";
                    sColor2 = sColor1;
                }
                if (diaesp.Equals("MARTES"))
                {
                    sColor1 = "#92D050";
                    sColor2 = sColor1;
                }
                if (diaesp.Equals("MIÉRCOLES"))
                {
                    sColor1 = "#FFC000";
                    sColor2 = sColor1;
                }
                if (diaesp.Equals("JUEVES"))
                {
                    sColor1 = "#00B0F0";
                    sColor2 = sColor1;
                }
                if (diaesp.Equals("VIERNES"))
                {
                    sColor1 = "#FFFFFF";
                    sColor2 = sColor1;
                }

            }

        }

        private void btn_ImpTarima_Click(object sender, EventArgs e)
        {   DateTime hoy = DateTime.Today;
            string fecha = hoy.ToString("MM/dd/yyyy");

            string caratulas = nud_Cantidad.Value.ToString();
            int tarimas = int.Parse(caratulas);

            sRpo = sRpo.Substring(3);

            ImprimeCaratulas imprimir = new ImprimeCaratulas();
            imprimir.imprimeRotolos(sColor1, sColor2, diaesp, sRpo, sPlanta, sLinea, sAlma, sModelo, fecha, sCant,tarimas);
            Close();
            
            
        }

        private void nud_Cantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
     
                if ((int)e.KeyChar == (int)Keys.Enter)
                {
                    btn_ImpTarima_Click(sender, e);
                }

                if(e.KeyChar == (Char)Keys.Escape)
                this.Close();


        }

    }
}
