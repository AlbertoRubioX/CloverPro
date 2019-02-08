using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class AreasDestLogica
    {
        public string Area { get; set; }
        public int Consec { get; set; }
        public string Correo { get; set; }
        public string Tipo { get; set; }
        public string Turno { get; set; }
        public string Envio { get; set; }
        public string Usuario { get; set; }

        public static int Guardar(AreasDestLogica config)
        {
            string[] parametros = { "@Area", "@Consec", "@Correo", "@Tipo", "@Turno", "@Envio", "@Usuario"};
            return AccesoDatos.Actualizar("sp_mant_areas_dest", parametros, config.Area, config.Consec, config.Correo, config.Tipo, config.Turno, config.Envio, config.Usuario);
        }

        public static DataTable Consultar(AreasDestLogica config)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_areas_dest where area = '" + config.Area + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Listar(AreasDestLogica config)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT area,consec,correo as CORREO,turno as TURNO,tipo as TIPO,envia_auto FROM t_areas_dest where area = '"+config.Area+ "' and envia_auto = '"+config.Envio+"' ORDER BY turno asc,correo");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarTurno(AreasDestLogica config)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT area,consec,correo as CORREO,turno as TURNO,tipo as TIPO FROM t_areas_dest where area = '" + config.Area + "' and turno = '"+config.Turno+"' and envia_auto = '" + config.Envio + "' ORDER BY turno asc,correo");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Eliminar(AreasDestLogica config)
        {
            try
            {
                string sQuery = "DELETE FROM t_areas_dest WHERE area = '"+config.Area+"' and consec = " + config.Consec + "";
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
