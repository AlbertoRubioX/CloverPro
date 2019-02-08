using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class ReportesLogica
    {
        public string Planta { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public string RPO { get; set; }
        public string IndRPO { get; set; }
        public string IndPlanta { get; set; }
        public string IndTurno { get; set; }
        public string Turno { get; set; }
        public string IndArea { get; set; }
        public string Area { get; set; }
        public string IndFolio { get; set; }
        public long FolioIni { get; set; }
        public long FolioFin { get; set; }
        public string IndLinea { get; set; }
        public string LineaIni { get; set; }
        public string LineaFin { get; set; }
        public string IndModelo { get; set; }
        public string Modelo { get; set; }
        public string IndEmp { get; set; }
        public string TipoEmp { get; set; }
        public string Empleado { get; set; }
        public string IndNivel { get; set; }
        public string Nivel { get; set; }
        public string BloqPPH { get; set; }
        public string IndEst { get; set; }
        public string Estatus { get; set; }

        public static DataTable Etiquetas(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@Planta", "@FechaIni", "@FechaFin" };
                datos = AccesoDatos.ConsultaSP("sp_rep_etiquetas", parametros, rep.Planta, rep.FechaIni, rep.FechaFin);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }

        public static DataTable InspeccionEtiquetas(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@RPO", "@Turno" };
                datos = AccesoDatos.ConsultaSP("sp_rep_etiquetasDet", parametros, rep.FechaIni, rep.FechaFin, rep.RPO, rep.Turno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }

        public static DataTable CapturaLineUp(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@IndPlanta", "@Planta", "@IndFolio", "@FolioIni", "@FolioFin", "@IndLinea", "@LineaIni", "@Lineas", "@IndModelo", "@Modelo", "@IndRpo", "@RPO", "@Turno", "@BloqImp" };
                datos = AccesoDatos.ConsultaSP("sp_rep_lineup", parametros, rep.FechaIni, rep.FechaFin, rep.IndPlanta, rep.Planta, rep.IndFolio, rep.FolioIni, rep.FolioFin, rep.IndLinea, rep.LineaIni, rep.LineaFin, rep.IndModelo, rep.Modelo, rep.IndRPO, rep.RPO, rep.Turno, rep.BloqPPH);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }
        public static DataTable CapturaLineUpBloq(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@IndPlanta", "@Planta", "@IndFolio", "@FolioIni", "@FolioFin", "@IndLinea", "@LineaIni", "@Lineas", "@IndModelo", "@Modelo", "@IndRpo", "@RPO", "@Turno", "@BloqImp" };
                datos = AccesoDatos.ConsultaSP("sp_rep_lineup_nivel", parametros, rep.FechaIni, rep.FechaFin, rep.IndPlanta, rep.Planta, rep.IndFolio, rep.FolioIni, rep.FolioFin, rep.IndLinea, rep.LineaIni, rep.LineaFin, rep.IndModelo, rep.Modelo, rep.IndRPO, rep.RPO, rep.Turno, rep.BloqPPH);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }
        public static DataTable HistorialPPH(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@IndEmp", "@Empleado", "@IndNivel", "@Nivel", "@Niveles", "@IndPlanta", "@Planta", "@IndLinea", "@LineaIni", "@Lineas"};
                datos = AccesoDatos.ConsultaSP("sp_rep_historico_pph", parametros, rep.FechaIni, rep.FechaFin, rep.IndEmp, rep.Empleado, rep.IndNivel, rep.Nivel, rep.Nivel, rep.IndPlanta, rep.Planta, rep.IndLinea, rep.LineaIni, rep.LineaFin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }

        public static DataTable CapturaLineUpRes(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@IndPlanta", "@Planta", "@IndLinea", "@LineaIni", "@Lineas", "@IndModelo", "@Modelo", "@IndRpo", "@RPO", "@Turno" };
                datos = AccesoDatos.ConsultaSP("sp_rep_lineup_resumen", parametros, rep.FechaIni, rep.FechaFin, rep.IndPlanta, rep.Planta, rep.IndLinea, rep.LineaIni, rep.LineaFin, rep.IndModelo, rep.Modelo, rep.IndRPO, rep.RPO, rep.Turno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }
        public static DataTable CapturaLineUpResOrbis(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT idProduccion,idPlanta,Linea,FechaRegistro,NumRPO,Producto,Estandar,idTurno,Usuario FROM produccion WHERE (FechaRegistro BETWEEN cast('" + Convert.ToDateTime(rep.FechaIni).ToString("yyyy-MM-dd") + "' as date) AND cast('" + Convert.ToDateTime(rep.FechaFin).ToString("yyyy-MM-dd") + "' as date)) ";
                sSql += "AND ('" + rep.IndPlanta + "' = 'T' OR ('" + rep.IndPlanta + "' = 'U' AND idPlanta = '" + rep.Planta + "')) ";
                sSql += "AND ('" + rep.IndLinea + "' = 'T' OR ('" + rep.IndLinea + "' = 'U' AND Linea = '" + rep.LineaIni + "') OR ('"+rep.IndLinea+"' = 'A' AND (LOCATE(Linea,'"+rep.LineaFin+"') > 0))) ";
                sSql += "AND('"+rep.IndModelo+ "' = '0' OR('" + rep.IndModelo + "' = '1' AND Producto = '"+rep.Modelo+"')) ";
                sSql += "AND('" + rep.IndRPO + "' = '0' OR('" + rep.IndRPO+"' = '1' AND NumRPO = '"+rep.RPO+"')) ";
                sSql += "AND idTurno=" + rep.Turno + " ORDER BY idPlanta,Linea,FechaRegistro,NumRPO DESC";
                datos = AccesoDatos.ConsultarMySql(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }
        public static DataTable ListadoUsuarios(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@IndPlanta", "@Planta", "@IndTurno", "@Turno", "@IndEmp", "@TipoEmp", "@Empleado", "@IndArea", "@Area" };
                datos = AccesoDatos.ConsultaSP("sp_rep_usuarios", parametros, rep.IndPlanta, rep.Planta, rep.IndTurno, rep.Turno, rep.IndEmp, rep.TipoEmp, rep.Empleado, rep.IndArea, rep.Area);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }
        public static DataTable ListadoSuperLine(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@IndPlanta", "@Planta", "@IndSup", "@Supervisor", "@IndLinea", "@LineaIni", "@Lineas", "@Turno" };
                datos = AccesoDatos.ConsultaSP("sp_rep_supline", parametros, rep.IndPlanta, rep.Planta, rep.IndEmp, rep.Empleado, rep.IndLinea, rep.LineaIni, rep.LineaFin, rep.Turno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }

        public static DataTable RepTransferLineas(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@Planta", "@Linea", "@Empleado", "@Turno" };
                datos = AccesoDatos.ConsultaSP("sp_rep_transferline", parametros, rep.FechaIni,rep.FechaFin, rep.Planta, rep.LineaIni,rep.Empleado, rep.Turno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }

        public static DataTable RepEtiEmpaque(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@IndPlanta", "@Planta", "@IndLinea", "@Linea", "@IndVend", "@Vendor", "@Turno" };
                datos = AccesoDatos.ConsultaSP("sp_rep_etiempaque", parametros, rep.FechaIni, rep.FechaFin, rep.IndPlanta, rep.Planta, rep.IndLinea, rep.LineaIni, rep.IndRPO, rep.RPO, rep.Turno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }
        public static DataTable RepEmpaqueRPOalm(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = {"@FechaIni", "@FechaFin", "@IndRPO", "@RPO", "@Turno" };
                datos = AccesoDatos.ConsultaSP("sp_rep_rpoact_almproc", parametros,rep.FechaIni, rep.FechaFin, rep.IndRPO, rep.RPO, rep.Turno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }

        public static DataTable RepGlobalRPO(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@IndRPO", "@RPO", "@IndItem", "@Item", "@IndTO", "@TO", "@Cancelado" };
                datos = AccesoDatos.ConsultaSP("sp_rep_globals_rpo", parametros, rep.FechaIni, rep.FechaFin, rep.IndRPO, rep.RPO, rep.IndModelo, rep.Modelo, rep.IndTurno, rep.Turno, rep.Estatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }
        public static DataTable MonitorEmpleados(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@IndPlanta", "@Planta", "@IndLinea", "@Linea", "@IndTurno", "@Turno", "@IndNivel", "@NiveL", "@IndEmp", "@Empleado" };
                datos = AccesoDatos.ConsultaSP("sp_mon_empleados", parametros, rep.IndPlanta, rep.Planta, rep.IndLinea, rep.LineaIni, rep.IndTurno, rep.Turno, rep.IndNivel, rep.Nivel, rep.IndEmp, rep.Empleado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }

        public static DataTable DiarioSetUp(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@IndPlanta", "@Planta", "@IndLinea", "@Linea", "@IndTurno", "@Turno", "@IndEst", "@Estatus" };
                datos = AccesoDatos.ConsultaSP("sp_rep_setup", parametros, rep.FechaIni, rep.FechaFin, rep.IndPlanta, rep.Planta, rep.IndLinea, rep.LineaIni, rep.IndTurno, rep.Turno, rep.IndEst, rep.Estatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static DataTable DiarioSetUp2(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@IndPlanta", "@Planta", "@IndLinea", "@Linea", "@IndTurno", "@Turno", "@IndEst", "@Estatus" };
                datos = AccesoDatos.ConsultaSP("sp_rep_setupDiario", parametros, rep.FechaIni, rep.FechaFin, rep.IndPlanta, rep.Planta, rep.IndLinea, rep.LineaIni, rep.IndTurno, rep.Turno, rep.IndEst, rep.Estatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable DuracionSetUpMP(ReportesLogica rep)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@FechaIni", "@FechaFin", "@IndPlanta", "@Planta", "@IndLinea", "@Linea"};
                datos = AccesoDatos.ConsultaSP("sp_rep_setup_dura", parametros, rep.FechaIni, rep.FechaFin, rep.IndPlanta, rep.Planta, rep.IndLinea, rep.LineaIni);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

    }
}
