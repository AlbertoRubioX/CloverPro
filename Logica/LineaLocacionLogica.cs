using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class LineaLocacionLogica
    {
        public string Planta { get; set; }
        public string Linea { get; set; }
        public string Nota { get; set; }
        public string Locacion { get; set; }
        public string Usuario { get; set; }
        public static int Guardar(LineaLocacionLogica lin)
        {
            string[] parametros = { "@Planta", "@Linea", "@Local", "@Nota", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_linelocal", parametros, lin.Planta, lin.Linea, lin.Locacion, lin.Nota, lin.Usuario);
        }

        public static DataTable Listar(LineaLocacionLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery;
                sQuery = "SELECT planta,linea ,locacion as LOCACIO,N nota as NOTA FROM t_linelocal where planta = '" + linea.Planta + "' AND linea = '"+linea.Linea+"'";
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
                sQuery = "SELECT linea FROM t_linelocal order by planta,linea,locacion";
                datos = AccesoDatos.Consultar(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Consultar(LineaLocacionLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_linelocal WHERE planta = '"+linea.Planta+"' and linea = '" + linea.Linea + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarPlanta(LineaLocacionLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_linelocal WHERE linea = '" + linea.Linea + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable LineaPlanta(LineaLocacionLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_linelocal WHERE planta = '" + linea.Planta + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;

        }

       

        public static DataTable VistaLineaPlanta(LineaLocacionLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT linea as Linea, nombre as Nota FROM t_linelocal WHERE planta = '" + linea.Planta + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;

        }
        public static bool Verificar(LineaLocacionLogica linea)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_linelocal WHERE planta = '"+linea.Planta+"' and linea = '" + linea.Linea + "' and locacion = '"+linea.Locacion+"'";
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
      
        public static bool Eliminar(LineaLocacionLogica linea)
        {
            try
            {
                string sQuery = "DELETE FROM t_linelocal WHERE planta = '"+linea.Planta+"' and linea = '" + linea.Linea + "' and locacion = '"+linea.Locacion+"'";
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

        public static bool AntesDeEliminar(LineaLocacionLogica linea)
        {
            try
            {
                string sQuery;

                DataTable datos = new DataTable();

                sQuery = "SELECT * FROM t_rpo_actdet WHERE planta = '"+linea.Planta+"' and linea = '" + linea.Linea + "' and locacion = '"+linea.Locacion+"'";
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
