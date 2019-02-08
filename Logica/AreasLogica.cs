using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class AreasLogica
    {
        public string Area { get; set; }
        public string Descrip { get; set; }
        public string ActPrev { get; set; }
        public string Orden { get; set; }
        public string Usuario { get; set; }

        public static int Guardar(AreasLogica def)
        {
            string[] parametros = { "@Area", "@Descrip", "@Actprev", "@Orden", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_areas", parametros, def.Area, def.Descrip, def.ActPrev, def.Orden, def.Usuario );
        }

        public static DataTable Consultar(AreasLogica def)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_areas WHERE area = '" + def.Area + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(AreasLogica def)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_areas WHERE area = '" + def.Area + "'";
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

        public static DataTable Listar()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_areas");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarActPrev()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT area as CLAVE,descrip as ACTIVIDAD FROM t_areas where ind_actprev = '1' order by orden");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Eliminar(AreasLogica def)
        {
            try
            {
                string sQuery = "DELETE FROM t_areas WHERE area = '" + def.Area + "'";
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

        public static bool AntesDeBorrar(AreasLogica area)
        {
            try
            {
                string sQuery;

                DataTable datos = new DataTable();

                sQuery = "SELECT * FROM t_lineseact WHERE actividad = '" + area.Area + "'";
                datos = AccesoDatos.Consultar(sQuery);
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
    }
}
