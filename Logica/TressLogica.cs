using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class TressLogica
    {
        public int Codigo { get; set; }
        public string Turno { get; set; }
        public string Planta { get; set; }
        public string Linea { get; set; }
        
        public static DataTable Consultar(TressLogica col)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT pta.TB_ELEMENT AS PLANTA,col.CB_NIVEL2 AS LINEA,col.CB_TURNO AS TURNO,col.CB_CODIGO,col.PRETTYNAME AS Nombre " +
                "FROM COLABORA col INNER JOIN NIVEL3 pta ON col.CB_NIVEL3 = pta.TB_CODIGO " +
                "WHERE col.CB_NIVEL0 = '1' AND col.CB_NIVEL1 = '001' AND col.CB_ACTIVO = 'S' ";
                datos = AccesoDatos.ConsultarTress(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaOper(TressLogica col)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT * FROM COLABORA WHERE CB_CODIGO =" + col.Codigo + "";
                datos = AccesoDatos.ConsultarTress(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaNivelOper(TressLogica col)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT col.CB_PUESTO as NIVEL,pu.PU_DESCRIP,col.* FROM COLABORA col INNER JOIN PUESTO pu ON col.CB_PUESTO = pu.PU_CODIGO WHERE col.CB_CODIGO = "+col.Codigo+"";
                datos = AccesoDatos.ConsultarTress(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaImagen(TressLogica img)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT * FROM IMAGEN WHERE CB_CODIGO ="+img.Codigo+" AND IM_TIPO = 'FOTO'";
                datos = AccesoDatos.ConsultarTress(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable OperadoresLinea(TressLogica img)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT co.CB_CODIGO,co.PRETTYNAME,co.CB_NIVEL2,pu.PU_DESCRIP,im.IM_BLOB "+
                "FROM COLABORA co INNER JOIN PUESTO pu ON co.CB_PUESTO = pu.PU_CODIGO INNER JOIN IMAGEN im ON co.CB_CODIGO = im.CB_CODIGO " +
                "WHERE co.CB_ACTIVO = 'S' AND co.CB_TURNO = '"+img.Turno+"' AND co.CB_NIVEL3 = '"+img.Planta+"' AND co.CB_NIVEL2 = '"+img.Linea+"' AND im.IM_TIPO = 'FOTO'" +
                "AND(select dbo.SP_CHECADAS(cast(getdate() as date), co.CB_CODIGO, 1)) > ''";
                datos = AccesoDatos.ConsultarTress(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable OperadorLineUp(TressLogica oper)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT col.CB_CODIGO as CODIGO,col.PRETTYNAME AS NOMBRE,pu.PU_DESCRIP AS NIVEL,pta.TB_ELEMENT AS PLANTA,col.CB_NIVEL2 AS LINEA " +
                "FROM COLABORA col  INNER JOIN NIVEL3 pta ON col.CB_NIVEL3 = pta.TB_CODIGO INNER JOIN PUESTO pu ON col.CB_PUESTO = pu.PU_CODIGO " +
                "WHERE col.CB_CODIGO = " + oper.Codigo + "";
                datos = AccesoDatos.ConsultarTress(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable LineHeadCount(TressLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT  co.CB_TURNO ,count( co.CB_CODIGO) as headcount FROM COLABORA co " +
                "WHERE co.CB_ACTIVO='S' AND co.CB_NIVEL1='001' and co.CB_NIVEL3 = '" + line.Planta + "'  AND co.CB_NIVEL2 = '" + line.Linea + "' group by co.CB_TURNO ";
                datos = AccesoDatos.ConsultarTress(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
    }
}
