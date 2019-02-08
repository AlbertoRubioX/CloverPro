using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class ModelohcLogica
    {
        public string Planta { get; set; }
        public string Linea { get; set; }
        public string Modelo { get; set; }
        public string Area { get; set; }
        public string Descrip { get; set; }
        public string ActPrev { get; set; }
        public string Orden { get; set; }
        public string Usuario { get; set; }

        public static int Guardar(ModelohcLogica line)
        {
            string[] parametros = { "@Area", "@Descrip", "@Actprev", "@Orden", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_areas", parametros, line.Area, line.Descrip, line.ActPrev, line.Orden, line.Usuario);
        }

        public static DataTable Consultar(ModelohcLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_modelohc WHERE planta = '" + line.Planta + "' order by modelo");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarLinea(ModelohcLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_modelohc WHERE planta = '" + line.Planta + "' and linea = '"+ line.Linea +"' order by modelo");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarModelo(ModelohcLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_modelohc WHERE planta = '" + line.Planta + "' and linea = '" + line.Linea + "' and modelo = '"+line.Modelo+"'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarLinea(ModelohcLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT distinct linea_nav as Linea,'' as Modelo,0 as [STD 1 Turno],0 as [STD 2 Turno],0 as [STD HC],0 as factor,'1' as Activa,linea FROM t_linea WHERE planta = '" + line.Planta + "' and linea_nav is not null and linea_nav > ''");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static DataTable ListarLineaSP(ModelohcLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@Planta"};
                datos = AccesoDatos.ConsultaSP("sp_mon_lineas_hc", parametros, line.Planta);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarModelos(ModelohcLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT modelo as Modelo,std1er as [STD 1 Turno],std2do as [STD 2 Turno],headcount as [STD HC],factor as Factor FROM t_modelohc WHERE planta = '" + line.Planta + "' and linea = '"+line.Linea+"' order by modelo");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static DataTable ListarModelosPta(ModelohcLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT modelo as Modelo,std1er as [Std 1 Turno],std2do as [Std 2 Turno],headcount as [Head Count],factor as Factor FROM t_modelohc WHERE planta = '" + line.Planta + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(ModelohcLogica line)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_areas WHERE area = '" + line.Area + "'";
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

        public static bool Eliminar(ModelohcLogica line)
        {
            try
            {
                string sQuery = "DELETE FROM t_areas WHERE area = '" + line.Area + "'";
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

        public static bool AntesDeBorrar(ModelohcLogica area)
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
