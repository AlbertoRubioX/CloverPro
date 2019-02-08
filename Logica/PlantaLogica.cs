using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class PlantaLogica
    {
        public string Planta { get; set; }
        public string Nombre { get; set; }

        public static int Guardar(PlantaLogica plan)
        {
            string[] parametros = { "@Planta", "@Nombre" };
            return AccesoDatos.Actualizar("sp_mant_planta", parametros, plan.Planta, plan.Nombre);
        }

        public static DataTable Listar()
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_plantas";
                datos = AccesoDatos.Consultar(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Consultar(PlantaLogica plan)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_plantas WHERE planta = '" + plan.Planta + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(PlantaLogica plan)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_plantas WHERE planta = '" + plan.Planta + "'";
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

        public static bool Eliminar(PlantaLogica plan)
        {
            try
            {
                string sQuery = "DELETE FROM t_lineas WHERE planta = '" + plan.Planta + "'";
                if (AccesoDatos.Borrar(sQuery) == 0)
                    return false;

                sQuery = "DELETE FROM t_ptasuper WHERE planta = '" + plan.Planta + "'";
                if (AccesoDatos.Borrar(sQuery) == 0)
                    return false;

                sQuery = "DELETE FROM t_plantas WHERE planta = '" + plan.Planta + "'";
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

        public static bool AntesDeEliminar(PlantaLogica plan)
        {
            try
            {
                string sQuery;

                DataTable datos = new DataTable();

                sQuery = "SELECT * FROM t_conf_dest WHERE planta = '" + plan.Planta + "'";
                datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    return true;

                sQuery = "SELECT * FROM t_linea WHERE planta = '" + plan.Planta + "'";
                datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    return true;

                sQuery = "SELECT * FROM t_usuario WHERE planta = '" + plan.Planta + "'";
                datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    return true;

                sQuery = "SELECT * FROM t_operador WHERE planta = '" + plan.Planta + "'";
                datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    return true;

                sQuery = "SELECT * FROM t_etiqueta WHERE planta = '" + plan.Planta + "'";
                datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    return true;

                sQuery = "SELECT * FROM t_lineup WHERE planta = '" + plan.Planta + "'";
                datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    return true;

                sQuery = "SELECT * FROM t_estacion WHERE planta = '" + plan.Planta + "'";
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
