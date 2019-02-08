using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Datos;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CloverPro
{
    public class GlobalVar
    {
        public static string gsUsuario;
        public static string gsVersion;
        public static string gsEstacion;
        public static string gsPlanta;
        public static string gsDepto; //Prouduccion - Emplaque - Recepcion
        public static string gsArea; //Produccion - Etiquetas - Sistemas - Supervisor
        public static string gsTurno; //Reporte de LineUp
        public static string gsNombreUs; //Nombre del Usuario
        public static bool gbCambio;

        public static void CambiaColor(Control _Control, int _iEvent)
        {
            if (_iEvent == 0)
                _Control.BackColor = Color.White;
            if (_iEvent == 1)
                _Control.BackColor = Color.FromArgb(255, 255, 170);
            if (_iEvent == 2)
                _Control.BackColor = Color.FromArgb(255, 110, 110);
        }
        public static DateTime FechaTurno()
        {
            
            DateTime dtFecha = DateTime.Now;
            if (dtFecha.Hour >= 0 && dtFecha.Hour < 6)
            {
                dtFecha = DateTime.Today.AddDays(-1);
            }
            
            return dtFecha;
        }

        public static string TurnoGlobal()
        {

            string sTurno = "2";
            DateTime dtFecha = DateTime.Now;
            if (dtFecha.Hour >= 5 && dtFecha.Hour < 16)
            {
                sTurno = "1";
            }
            else
            {
                if (dtFecha.Hour == 16 && dtFecha.Minute <= 10)
                    sTurno = "1";
            }

            return sTurno;
        }
        public static string Navegacion(string _sTipo, string _sTabla, string _sCampo, string _sValor)
        {
            string sValor = "";
            string sQuery = "";
            switch (_sTipo)
            {
                case "F":
                    sQuery = "SELECT MIN(" + _sCampo + ") FROM " + _sTabla + " ";
                    break;
                case "B":
                    sQuery = "SELECT MAX(" + _sCampo + ") FROM " + _sTabla + " WHERE " + _sCampo + " < '" + _sValor + "'";
                    break;
                case "N":
                    sQuery = "SELECT MIN(" + _sCampo + ") FROM " + _sTabla + " WHERE " + _sCampo + " > '" + _sValor + "'";
                    break;
                case "L":
                    sQuery = "SELECT MAX(" + _sCampo + ") FROM " + _sTabla + " ";
                    break;

            }

            DataTable dato = new DataTable();
            dato = AccesoDatos.Consultar(sQuery);
            if (!string.IsNullOrEmpty(dato.Rows[0][0].ToString()))
                sValor = dato.Rows[0][0].ToString();
            else
                sValor = _sValor;

            return sValor;
        }
    }
}
