using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class RpoUbicaDetLogica
    {
        public string Planta { get; set; }
        public string Area { get; set; }
        public string Ubicacion { get; set; }
        public string Celda { get; set; }
        public int Config { get; set; }
        public string Usuario { get; set; }
        public static int Guardar(RpoUbicaDetLogica ubi)
        {
            string[] parametros = { "@Planta", "@Ubicacion","@Area", "@Celda", "@Config", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_rpo_ubidet", parametros, ubi.Planta, ubi.Ubicacion,ubi.Area,ubi.Celda, ubi.Config, ubi.Usuario);
        }

        public static DataTable Listar(RpoUbicaDetLogica ubi)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery;
                sQuery = "SELECT planta,ubicacion,celda as CELDA,config as CONFIG FROM t_rpo_ubidet where planta = '" + ubi.Planta +"'AND area ='"+ubi.Area+ "' AND ubicacion = '"+ubi.Ubicacion+"'";
                datos = AccesoDatos.Consultar(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
       

        public static DataTable Consultar(RpoUbicaDetLogica ubi)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_rpo_ubidet WHERE planta = '"+ubi.Planta+"' and ubicacion = '" + ubi.Ubicacion + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarArea(RpoUbicaDetLogica ubi)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_rpo_ubidet WHERE planta = '" + ubi.Planta + "' and ubicacion = '" + ubi.Ubicacion + "' and celda = '"+ubi.Celda+"' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaPlanta(RpoUbicaDetLogica ubi)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT planta,ubicacion,celda,ubicacion+'-'+celda as ubica FROM t_rpo_ubidet WHERE planta = '" + ubi.Planta + "'"+" and " + " area = '" + ubi.Area + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(RpoUbicaDetLogica ubi)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_rpo_ubidet WHERE planta = '"+ubi.Planta+"' and ubicacion = '" + ubi.Ubicacion + "' and celda = '"+ubi.Celda+"'";
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
      
        public static bool Eliminar(RpoUbicaDetLogica ubi)
        {
            try
            {
                string sQuery = "DELETE FROM t_rpo_ubidet WHERE planta = '"+ubi.Planta+"' and ubicacion = '"+ubi.Ubicacion+"' and celda = '"+ubi.Celda+"' ";
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
