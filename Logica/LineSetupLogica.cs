using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class LineSetupLogica
    {
        public long Folio { get; set; }
        public DateTime Fecha { get; set; }
        public string Planner { get; set; }
        public string Usuario { get; set; }
        public static int Guardar(LineSetupLogica set)
        {
            string[] parametros = { "@Folio", "@Fecha", "@Planner", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_lineset", parametros, set.Folio, set.Fecha, set.Planner, set.Usuario );
        }

        public static DataTable Consultar(LineSetupLogica set)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_lineset WHERE folio = "+set.Folio+" ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(LineSetupLogica set)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_lineset WHERE fecha = CAST('"+set.Fecha+"' AS date) AND planner='"+set.Planner+"' ");
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

        public static DataTable Listar(LineSetupLogica set)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT ls.folio,CAST(ls.fecha AS DATE) as Fecha,us.nombre as Planner "+
                "FROM t_lineset ls inner join t_usuario us on ls.planner = us.usuario "+
                "WHERE ls.folio = "+set.Folio+" ";
                datos = AccesoDatos.Consultar(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
    }
}
