using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class AlertaDestLogica
    {
        public string Alerta { get; set; }
        public int Consec { get; set; }
        public string Destino { get; set; }
        public string Tipo { get; set; }
        public string Turno { get; set; }

        public static int Guardar(AlertaDestLogica config)
        {
            string[] parametros = { "@Alerta", "@Consec", "@Destino", "@Tipo", "@Turno" };
            return AccesoDatos.Actualizar("sp_mant_alerta_dest", parametros, config.Alerta, config.Consec, config.Destino, config.Tipo, config.Turno);
        }

        public static DataTable Consultar(AlertaDestLogica config)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_alerta_dest where destino = '" + config.Destino + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Listar(AlertaDestLogica config)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT alerta,consec,destino as DESTINO,tipo as TIPO,turno as TURNO FROM t_alerta_dest where alerta = '"+config.Alerta+ "' ORDER BY turno asc,tipo desc");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Eliminar(AlertaDestLogica config)
        {
            try
            {
                string sQuery = "DELETE FROM t_alerta_dest WHERE alerta = '"+config.Alerta+"' and consec = " + config.Consec + "";
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
