using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class RpoUbicacionLogica
    {
        public string Planta { get; set; }
        public string Area { get; set; }
        public string Ubicacion { get; set; }
        public string Nota { get; set; }
        public string Usuario { get; set; }
        public static int Guardar(RpoUbicacionLogica ubi)
        {
            string[] parametros = { "@Planta", "@Ubicacion","@Area", "@Nota", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_rpo_ubi", parametros, ubi.Planta, ubi.Ubicacion,ubi.Area, ubi.Nota, ubi.Usuario);
        }

        public static DataTable Consultar(RpoUbicacionLogica ubi)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_rpo_ubi WHERE planta = '"+ubi.Planta+"' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarPlanta(RpoUbicacionLogica ubi)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_rpo_ubi WHERE ubicacion = '" + ubi.Ubicacion + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

       
        public static bool Verificar(RpoUbicacionLogica ubi)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_rpo_ubi WHERE planta = '"+ubi.Planta+"' and ubicacion = '" + ubi.Ubicacion + "'";
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
      
        public static bool Eliminar(RpoUbicacionLogica ubi)
        {
            try
            {
                string sQuery = "DELETE FROM t_rpo_ubi WHERE planta = '"+ubi.Planta+"' and ubicacion = '"+ubi.Ubicacion+"'";
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

        public static bool AntesDeEliminar(RpoUbicacionLogica ubi)
        {
            try
            {
                string sQuery;

                DataTable datos = new DataTable();

                sQuery = "SELECT * FROM t_rpo_actdet WHERE planta = '"+ubi.Planta+"' and ubicacion = '"+ubi.Ubicacion+"'";
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
