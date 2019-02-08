using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class RpoGlobLogica
    {
        public long Folio { get; set; }
        public DateTime Fecha { get; set; }
        public string RPO { get; set; }
        public string Modelo { get; set; }
        public string Planta { get; set; }
        public string Linea { get; set; }
        public int Cantidad { get; set; }
        public int CantGlobal { get; set; }
        public string HoraComp { get; set; }
        public string HoraEnt { get; set; }
        public string Estado { get; set; }
        public string Destino { get; set; }
        public string AlmHrIni { get; set; }
        public string AlmHrEnd { get; set; }
        public string ProHrIni { get; set; }
        public string ProHrEnd { get; set; }
        public string EnvHrIni { get; set; }
        public string EnvHrEnd { get; set; }
        public string TO { get; set; }
        public string Truck { get; set; }
        public string Nota { get; set; }
        public string IndRPO { get; set; }
        public string Cancelado { get; set; }
        public string Usuario { get; set; }
        /*
        public static int Guardar(LineSetActLogica det)
        {
            string[] parametros = { "@Folio", "@Consec", "@Actividad", "@Estatus", "@Comentario", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_lineseact", parametros, det.Folio, det.Consec, det.Actividad, det.Estatus, det.Comentario, det.Usuario);
        }*/

        public static DataTable MonitorGlobals(RpoGlobLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@Fecha", "@IndRPO", "@RPO", "@Cancelado" };
                datos = AccesoDatos.ConsultaSP("sp_mon_globals_rpo", parametros, rpo.Fecha, rpo.IndRPO, rpo.RPO, rpo.Cancelado);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Consultar(RpoGlobLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT rpo as RPO,CONVERT(varchar, fecha, 101) as Fecha,modelo as Modelo,cantidad as Cant,linea as Linea,hr_compromiso as [Hr Comp]," +
                "CASE destino WHEN 'VNS' THEN 'VAN NUYS' ELSE 'CALEXICO' END as Destino," +
                //"CASE estado WHEN 'ALM' THEN 'ALMACEN' WHEN 'PRO' THEN 'PRODUCCION' WHEN 'ENV' THEN 'ENVIOS' ELSE 'PLANEACION' END as Operando,"+
                "alm_hrent as [Entra ALM], alm_hrsal as [Sale ALM]," +
                "pro_hrent as [Entra PROD], pro_hrsal as [Sale PROD]," +
                "env_hrent as [Entra ENV], env_hrsal as [Sale ENV]," +
                "env_to as [No TO],truck_no as [No Camión],coments as Comentarios,estado,folio,'0' as ind,u_id,f_id " +
                "FROM t_rpo_glob " +
                "WHERE (cast(fecha as date) = cast('" + rpo.Fecha + "' as date) "+
                "OR ((cast(fecha as date) = cast('" + rpo.Fecha + "' as date) AND ( env_to is null or env_to = '') and fecha is not null))) " +
                "AND ('" + rpo.IndRPO + "' = '0' OR ('" + rpo.IndRPO+"' = '1' AND rpo = '"+rpo.RPO+"')) "+
                "ORDER BY hr_compromiso, folio";
                datos = AccesoDatos.Consultar(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaOrigen(RpoGlobLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_rpo WHERE rpo = '" + rpo.RPO + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool ActualizaWP(RpoGlobLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_rpo_glob SET modelo = '"+rpo.Modelo+"',hr_compromiso = '"+rpo.HoraComp+"',cantidad = "+rpo.Cantidad+", cant_global = "+rpo.CantGlobal+", env_hrent = '" + rpo.EnvHrIni + "', env_hrsal = '"+ rpo.EnvHrEnd + "' , env_to = '"+rpo.TO+"', truck_no = '"+rpo.Truck+"', coments = '"+rpo.Nota+"', u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + "";
                if (AccesoDatos.Borrar(sQuery) != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool ActualizaLinea(RpoGlobLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_rpo_glob SET planta = '" + rpo.Planta + "',linea = '" + rpo.Linea + "',u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + "";
                if (AccesoDatos.Borrar(sQuery) != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool ActualizaDestino(RpoGlobLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_rpo_glob SET destino = '" + rpo.Destino + "',u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + "";
                if (AccesoDatos.Borrar(sQuery) != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool CancelaRPO(RpoGlobLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_rpo_glob SET cancelado = '1', coments = '" + rpo.Nota + "', u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + "";
                if (AccesoDatos.Borrar(sQuery) != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool BorrarRPO(RpoGlobLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "DELETE FROM t_rpo_glob WHERE folio = " + rpo.Folio + "";
                if (AccesoDatos.Borrar(sQuery) != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
