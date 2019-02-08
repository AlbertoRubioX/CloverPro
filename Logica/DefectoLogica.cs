using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class DefectoLogica
    {
        public string Defecto { get; set; }
        public string Descrip { get; set; }
        public string Usuario { get; set; }

        public static int Guardar(DefectoLogica def)
        {
            string[] parametros = { "@Defecto", "@Descrip", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_defecto", parametros, def.Defecto, def.Descrip, def.Usuario );
        }

        public static DataTable Consultar(DefectoLogica def)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_defectos WHERE defecto = '" + def.Defecto + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(DefectoLogica def)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_defectos WHERE defecto = '" + def.Defecto + "'";
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
                datos = AccesoDatos.Consultar("SELECT * FROM t_defectos");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Eliminar(DefectoLogica def)
        {
            try
            {
                string sQuery = "DELETE FROM t_defecto WHERE defecto = '" + def.Defecto + "'";
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
