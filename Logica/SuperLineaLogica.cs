using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class SuperLineaLogica
    {
        public string Supervisor { get; set; }
        public int Consec { get; set; }
        public string Planta { get; set; }
        public string Linea { get; set; }
        public string Turno { get; set; }
        public string Area { get; set; }
        public string Usuario { get; set; }

        public static int Guardar(SuperLineaLogica pta)
        {
            string[] parametros = { "@Super", "@Consec", "@Planta", "@Linea", "@Turno", "@Area", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_suplinea", parametros, pta.Supervisor, pta.Consec, pta.Planta, pta.Linea, pta.Turno, pta.Area, pta.Usuario);
        }

        public static DataTable Listar(SuperLineaLogica sup)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery;
                sQuery = "SELECT supervisor,consec,planta as Planta,linea as Linea,turno,area FROM t_suplinea where supervisor = '" + sup.Supervisor+"'";
                datos = AccesoDatos.Consultar(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Consultar(SuperLineaLogica sup)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_suplinea WHERE supervisor = '"+sup.Supervisor+"' and consec = " + sup.Consec + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(SuperLineaLogica sup)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_suplinea WHERE supervisor = '"+sup.Supervisor+"' and planta = '"+sup.Planta+"' and linea = '" + sup.Linea + "'";
                DataTable datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool VerificaLineaSup(SuperLineaLogica sup)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_suplinea WHERE supervisor <> '"+sup.Supervisor+"' and planta = '"+sup.Planta+"' and area = '"+sup.Area+"' and linea = '"+sup.Linea+"' and turno = '"+sup.Turno+"'";
                DataTable datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public static DataTable ConsultarLinea(SuperLineaLogica sup)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_suplinea WHERE planta = '" + sup.Planta + "' and linea = '"+sup.Linea+"' and turno = '"+sup.Turno+"' " );
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Eliminar(SuperLineaLogica sup)
        {
            try
            {
                string sQuery = "DELETE FROM t_suplinea WHERE supervisor = '"+sup.Supervisor+"' and consec = " + sup.Consec + "";
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

        public static bool Borrar(SuperLineaLogica sup)
        {
            try
            {
                string sQuery = "DELETE FROM t_suplinea WHERE supervisor = '" + sup.Supervisor + "'";
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
