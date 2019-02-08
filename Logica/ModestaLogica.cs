using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class ModestaLogica
    {
        public string Modelo { get; set; }
        public int Consec { get; set; }
        public string Estacion { get; set; }
        public string Nombre { get; set; }
        public int Operadores { get; set; }
        public string Nivel { get; set; }
        public string Codigo { get; set; }
        public string Usuario { get; set; }
        
        public static int Guardar(ModestaLogica mode)
        {
            string[] parametros = { "@Modelo", "@Consec", "@Estacion", "@Nombre", "@Operadores", "@Nivel", "@Codigo", "@Usuario"};
            return AccesoDatos.Actualizar("sp_mant_modesta", parametros, mode.Modelo, mode.Consec, mode.Estacion, mode.Nombre, mode.Operadores, mode.Nivel, mode.Codigo, mode.Usuario);
        }

        public static DataTable Consultar(ModestaLogica mode)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_modesta WHERE modelo = '" + mode.Modelo + "' and consec = " + mode.Consec + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Listar(ModestaLogica mode)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT modelo,consec,estacion as ESTACION,nombre as NOMBRE,cant_oper as CANT,nivel_req as PPH,nivel_cod as CODIGO,CASE nivel_req WHEN 'NIV1' THEN 'I' WHEN 'NIV2' THEN 'II' WHEN 'NIV3' THEN 'III' WHEN 'NIV4' THEN 'IV' WHEN 'NIV5' THEN 'V' ELSE '' END as nivel_req FROM t_modesta WHERE modelo = '" + mode.Modelo + "' ORDER BY modelo,consec");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(ModestaLogica mode)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_modesta WHERE modelo = '" + mode.Modelo + "' and consec = " + mode.Consec + "";
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

        public static bool Eliminar(ModestaLogica mode)
        {
            try
            {
                string sQuery = "DELETE FROM t_modesta WHERE modelo = '" + mode.Modelo + "' and consec = " + mode.Consec + "";
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
