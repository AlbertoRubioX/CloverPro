using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class RpoLogica
    {
        public string RPO { get; set; }
        public string Descrip { get; set; }
        public string Modelo { get; set; }
        public string Planta { get; set; }
        public string Linea { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public int UltCons { get; set; }
        public string Turno { get; set; }
        public string Origen { get; set; }
        public string Nota { get; set; }
        public string Parcial { get; set; }
        public int OrdenCarga { get; set; }
        public static int Guardar(RpoLogica rpo)
        {
            string[] parametros = { "@RPO", "@Descrip", "@Modelo", "@Planta", "@Linea", "@Cantidad", "@Fecha", "@UltCons", "@Origen", "@Turno","@Nota", "@Parcial","@Orden"};
            return AccesoDatos.Actualizar("sp_mant_rpo", parametros, rpo.RPO, rpo.Descrip, rpo.Modelo, rpo.Planta, rpo.Linea, rpo.Cantidad, rpo.Fecha, rpo.UltCons, rpo.Origen, rpo.Turno, rpo.Nota, rpo.Parcial,rpo.OrdenCarga);
        }

        public static DataTable Consultar(RpoLogica rpo)
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

        public static DataTable ConsultaOrigen(RpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_rpo WHERE origen = '" + rpo.Origen + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(RpoLogica rpo)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_rpo WHERE rpo = '" + rpo.RPO + "'";
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

        public static DataTable ConsultarOrbis(RpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.ConsultarMySql("SELECT * FROM rpo2 WHERE numRpo = '" + rpo.RPO + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarOrbisLinea(RpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                string sFecha = rpo.Fecha.ToString("yyyy-MM-dd");
                string sSql = "SELECT * FROM rpo2 WHERE Linea = '" + rpo.Linea + "' and fechaProgramada  = '"+sFecha+"' order by Producto ";
                datos = AccesoDatos.ConsultarMySql(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }


        public static DataTable ConsultaProdOrbis(RpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.ConsultarMySql("SELECT FechaRegistro,NumRPO,Producto,Estandar FROM produccion WHERE FechaRegistro = cast('" + rpo.Fecha + "' as date) AND idPlanta= '" + rpo.Planta + "' AND Linea='" + rpo.Linea + "' AND idTurno='" + rpo.Turno + "' ORDER BY NumRPO DESC");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return datos;
        }
    }
}
