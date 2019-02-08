using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class MonitorSetupLogica
    {
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public string IndPlanta { get; set; }
        public string Planta { get; set; }
        public string IndLinea { get; set; }
        public string Linea { get; set; }
        public string IndTurno { get; set; }
        public string Turno { get; set; }
        public string IndEstatus { get; set; }
        public string Estatus { get; set; }
       
        public static DataTable ListarSP(MonitorSetupLogica set)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@IndPlanta", "@Planta", "@IndLinea", "@Linea", "@IndTurno", "@Turno", "@IndEst", "@Estatus" };
                datos = AccesoDatos.ConsultaSP("sp_mon_lineset", parametros, set.FechaIni, set.FechaFin, set.IndPlanta, set.Planta, set.IndLinea, set.Linea, set.IndTurno, set.Turno, set.IndEstatus, set.Estatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable VisorSP(MonitorSetupLogica set)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@IndPlanta", "@Planta", "@IndLinea", "@Linea", "@Turno", "@Estatus" };
                datos = AccesoDatos.ConsultaSP("sp_mon_lineset_visor", parametros, set.FechaIni, set.FechaFin, set.IndPlanta, set.Planta, set.IndLinea, set.Linea, set.Turno, set.Estatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable DuracionSP(MonitorSetupLogica set)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@IndPlanta", "@Planta", "@IndLinea", "@Linea"};
                datos = AccesoDatos.ConsultaSP("sp_rep_setup_dura", parametros, set.FechaIni, set.FechaFin, set.IndPlanta, set.Planta, set.IndLinea, set.Linea);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
    }
}
