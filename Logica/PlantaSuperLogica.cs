using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class PlantaSuperLogica
    {
        public string Planta { get; set; }
        public int Consec { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Turno { get; set; }
        public string Usuario { get; set; }

        public static int Guardar(PlantaSuperLogica pta)
        {
            string[] parametros = { "@Planta", "@Consec", "@Supervisor", "@Correo", "@Turno", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_ptasuper", parametros, pta.Planta, pta.Consec, pta.Nombre, pta.Correo, pta.Turno, pta.Usuario);
        }

        public static DataTable Listar(PlantaSuperLogica pta)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery;
                sQuery = "SELECT planta,consec,supervisor as Supervisor,correo as Correo,turno as Turno FROM t_ptasuper where planta = '"+pta.Planta+"'";
                datos = AccesoDatos.Consultar(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Consultar(PlantaSuperLogica pta)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_ptasuper WHERE planta = '"+pta.Planta+"' and consec = " + pta.Consec + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(PlantaSuperLogica pta)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_ptaea WHERE planta = '"+pta.Planta+"' and consec = " + pta.Consec + "";
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
     

        public static bool Eliminar(PlantaSuperLogica pta)
        {
            try
            {
                string sQuery = "DELETE FROM t_ptasuper WHERE planta = '"+pta.Planta+"' and consec = " + pta.Consec + "";
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
