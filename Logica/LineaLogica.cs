using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class LineaLogica
    {
        public string Planta { get; set; }
        public string Linea { get; set; }
        public string Nombre { get; set; }
        public string LineaNav { get; set; }
        public string Tipo { get; set; }
        public static int Guardar(LineaLogica lin)
        {
            string[] parametros = { "@Planta", "@Linea", "@Nombre", "@LineaNav", "@Tipo" };
            return AccesoDatos.Actualizar("sp_mant_linea", parametros, lin.Planta, lin.Linea, lin.Nombre, lin.LineaNav, lin.Tipo);
        }

        public static DataTable Listar(LineaLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery;
                sQuery = "SELECT planta,linea as Linea,nombre as Nombre,linea_nav as [Linea NAV],tipo as Tipo FROM t_linea where planta = '"+linea.Planta+"'";
                datos = AccesoDatos.Consultar(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarTodas()
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery;
                sQuery = "SELECT linea FROM t_linea order by linea";
                datos = AccesoDatos.Consultar(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarTodasNav()
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery;
                sQuery = "SELECT linea_nav FROM t_linea WHERE linea_nav is not null and linea_nav > '' order by linea_nav ";
                datos = AccesoDatos.Consultar(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Consultar(LineaLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_linea WHERE planta = '"+linea.Planta+"' and linea = '" + linea.Linea + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarPlanta(LineaLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_linea WHERE linea = '" + linea.Linea + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable LineaPlanta(LineaLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_linea WHERE planta = '" + linea.Planta + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;

        }

        public static DataTable LineasNav(LineaLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_linea WHERE planta = '" + linea.Planta + "' and linea_nav is not null and linea_nav > ''");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;

        }

        public static DataTable VistaLineaPlanta(LineaLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT linea as Linea, nombre as Nombre FROM t_linea WHERE planta = '" + linea.Planta + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;

        }
        public static bool Verificar(LineaLogica linea)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_linea WHERE planta = '"+linea.Planta+"' and linea = '" + linea.Linea + "'";
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
        public static bool VerificaLinea(LineaLogica linea)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_linea WHERE linea = '" + linea.Linea + "'";
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

        public static bool Eliminar(LineaLogica linea)
        {
            try
            {
                string sQuery = "DELETE FROM t_linea WHERE planta = '"+linea.Planta+"' and linea = '" + linea.Linea + "'";
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

        public static bool AntesDeEliminar(LineaLogica linea)
        {
            try
            {
                string sQuery;

                DataTable datos = new DataTable();

                sQuery = "SELECT * FROM t_lineup WHERE planta = '"+linea.Planta+"' and linea = '" + linea.Linea + "'";
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
