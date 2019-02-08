using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class NivelDetLogica
    {
        public string Planta { get; set; }
        public string Nivel { get; set; }
        public int Consec { get; set; }
        public string Codigo { get; set; }
        public string Operacion { get; set; }
        public string Usuario { get; set; }
        
        public static int Guardar(NivelDetLogica niv)
        {
            string[] parametros = { "@Nivel", "@Consec", "@Estacion", "@Nombre", "@Operadores", "@Nivel", "@Codigo", "@Usuario"};
            return AccesoDatos.Actualizar("sp_mant_niveldet", parametros, niv.Nivel, niv.Consec, niv.Nivel, niv.Codigo, niv.Usuario);
        }

        public static DataTable Consultar(NivelDetLogica niv)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_niveldet WHERE nivel = '" + niv.Nivel + "' and consec = " + niv.Consec + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Listar(NivelDetLogica niv)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT nivel,consec,codigo as CODIGO,operacion as OPERACION FROM t_niveldet WHERE nivel = '" + niv.Nivel + "' ORDER BY nivel,consec");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(NivelDetLogica niv)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_niveldet WHERE nivel = '" + niv.Nivel + "' and consec = " + niv.Consec + "";
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

        public static DataTable ConsultaOperacion(NivelDetLogica niv)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT nd.* FROM t_niveldet nd INNER JOIN t_niveles nv ON nd.nivel = nv.nivel WHERE nv.planta = '"+niv.Planta+"' AND nd.operacion = '" + niv.Operacion + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaOperaciones(NivelDetLogica niv)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT nd.* FROM t_niveldet nd INNER JOIN t_niveles nv ON nd.nivel = nv.nivel WHERE nv.planta = '" + niv.Planta + "' AND nd.operacion like '%" + niv.Operacion + "%'";
                datos = AccesoDatos.Consultar(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Eliminar(NivelDetLogica niv)
        {
            try
            {
                string sQuery = "DELETE FROM t_niveldet WHERE nivel = '" + niv.Nivel + "' and consec = " + niv.Consec + "";
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
