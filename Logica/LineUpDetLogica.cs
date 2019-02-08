using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class LineUpDetLogica
    {
        public long Folio { get; set; }
        public int Consec { get; set; }
        public string Linea { get; set; }
        public string Modelo { get; set; }
        public int CveEstacion { get; set; }
        public string Estacion { get; set; }
        public string Operacion { get; set; }
        public string NivelReq { get; set; }
        public string Operador { get; set; }
        public string NombreOper { get; set; }
        public string NivelOper { get; set; }
        public string LineaOper { get; set; }
        public string Tipo { get; set; }
        public string Tipona { get; set; }
        public string SinNivel { get; set; }
        public string Usuario { get; set; }

        public static int Guardar(LineUpDetLogica lin)
        {
            string[] parametros = { "@Folio", "@Consec", "@Linea", "@Modelo", "@CveEstacion", "@Estacion", "@Operacion", "@Operador", "@NombreOp", "@NivelOp", "@LineaOp", "@Tipo", "@Tipona", "@NivelReq", "@SinNivel", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_lineupdet", parametros, lin.Folio, lin.Consec, lin.Linea, lin.Modelo, lin.CveEstacion, lin.Estacion, lin.Operacion, lin.Operador, lin.NombreOper, lin.NivelOper, lin.LineaOper, lin.Tipo, lin.Tipona, lin.NivelReq, lin.SinNivel, lin.Usuario);
        }

        public static DataTable Listar(LineUpDetLogica lin)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery;
                sQuery = "SELECT folio,consec,linea AS line,modelo,cve_estacion,estacion as #,operacion as OPERACION,nivel_req as [NIVEL REQ],operador as [NO. EMPLEADO],nombre_oper as [NOMBRE DEL EMPLEAO],nivel_oper as NIVEL,linea_oper as LINEA,tipo_op,tipo_na,sin_nivel,u_id FROM t_lineupdet where folio = " + lin.Folio+" ";
                datos = AccesoDatos.Consultar(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Consultar(LineUpDetLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_lineupdet WHERE folio = " + linea.Folio + " and consec = "+linea.Consec+"");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(LineUpDetLogica linea)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_lineupdet WHERE folio = " + linea.Folio + " and operacion = '"+linea.Operacion+"'";
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

        public static bool Eliminar(LineUpDetLogica linea)
        {
            try
            {
                string sQuery = "DELETE FROM t_lineupdet WHERE folio = " + linea.Folio + " and consec = " + linea.Consec + "";
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
