using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class ConfigDestLogica
    {
        public string Planta { get; set; }
        public int Consec { get; set; }
        public string Correo { get; set; }
        public string Tipo { get; set; }
        public string Turno { get; set; }

        public static int Guardar(ConfigDestLogica config)
        {
            string[] parametros = { "@Planta", "@Consec", "@Correo", "@Turno", "@Tipo"};
            return AccesoDatos.Actualizar("sp_mant_conf_dest", parametros, config.Planta, config.Consec, config.Correo, config.Turno, config.Tipo);
        }

        public static DataTable Consultar(ConfigDestLogica config)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_conf_dest where correo = '" + config.Correo + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Listar(ConfigDestLogica config)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT planta,consec,correo as Correo,case tipo when 'T' then 'To' else 'Cc' end as Tipo,turno as Turno FROM t_conf_dest where planta = '"+config.Planta+ "' ORDER BY turno asc,tipo desc");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Eliminar(ConfigDestLogica config)
        {
            try
            {
                string sQuery = "DELETE FROM t_conf_dest WHERE planta = '"+config.Planta+"' and consec = " + config.Consec + "";
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
