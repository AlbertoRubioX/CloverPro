using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class ModerelaLogica
    {
        public string Modelo { get; set; }
        public int Consec { get; set; }
        public string Moderela { get; set; }
        public string Nota { get; set; }
        public string Usuario { get; set; }
        
        public static int Guardar(ModerelaLogica mode)
        {
            string[] parametros = { "@Modelo", "@Consec", "@Modrel", "@Nota", "@Usuario"};
            return AccesoDatos.Actualizar("sp_mant_moderela", parametros, mode.Modelo, mode.Consec, mode.Moderela, mode.Nota, mode.Usuario);
        }

        public static DataTable Consultar(ModerelaLogica mode)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_moderela WHERE modelo = '" + mode.Modelo + "' and consec = " + mode.Consec + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaRelacionado(ModerelaLogica mode)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT mo.tipo AS CORE,mr.modelo AS LAYOUT,mr.nota as NOTA,mo.ind_formatostd FROM t_moderela mr INNER JOIN t_modelo mo on mr.modelo = mo.modelo WHERE mr.modrel = '" + mode.Moderela + "' AND mo.estatus='A'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Listar(ModerelaLogica mode)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT modelo,consec,modrel as [Modelos],nota as Nota FROM t_moderela WHERE modelo = '" + mode.Modelo + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(ModerelaLogica mode)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_moderela WHERE modelo = '" + mode.Modelo + "' and consec = " + mode.Consec + "";
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

        public static bool Eliminar(ModerelaLogica mode)
        {
            try
            {
                string sQuery = "DELETE FROM t_moderela WHERE modelo = '" + mode.Modelo + "' and consec = " + mode.Consec + "";
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
