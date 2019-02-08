using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class ReempaqueLogica
    {
        public long Folio { get; set; }
        public string Estatus { get; set; }
        public string Usuario { get; set; }

        public static int Guardar(ReempaqueLogica eti)
        {
            string[] parametros = { "@Folio", "@Estatus", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_rmaemp", parametros, eti.Folio, eti.Estatus, eti.Usuario);
        }

        public static DataTable Consultar()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_rmaemp");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarDia()
        {
            DataTable datos = new DataTable();
            try
            {
                DateTime dtFecha = DateTime.Now;
                int iHora = dtFecha.Hour;
                if(iHora >= 0 && iHora < 6)
                    dtFecha = DateTime.Now.AddDays(-1);

                string sSql = "SELECT * FROM t_rmaemp WHERE CAST(fecha AS date) = CAST( '" + Convert.ToString(dtFecha) + "' AS date)";
                datos = AccesoDatos.Consultar(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarDiaTarima()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT rd.locacion AS LOCACION,rd.tarima AS TARIMA,sum(rd.cantidad) as CANT FROM t_rmaemp rm INNER JOIN t_rmaempdet rd on rm.folio = rd.folio WHERE CAST(rm.fecha AS date) = CAST( '"+Convert.ToString(DateTime.Today)+"' AS date) GROUP BY rd.locacion,rd.tarima ORDER BY 1,2");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarDiaRPO()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT rd.rpo AS RPO,rd.modelo AS BC,sum(rd.cantidad) as CANT FROM t_rmaemp rm INNER JOIN t_rmaempdet rd on rm.folio = rd.folio WHERE CAST(rm.fecha AS date) = CAST( '"+ Convert.ToString(DateTime.Today) + "' AS date) and rd.tipo = 'R' GROUP BY rd.rpo,rd.modelo ORDER BY 1,2");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarDiaModelo()
        {
            DataTable datos = new DataTable();
            try
            {
                string sSQL = "SELECT rd.sku AS [FINISH GOOD],sum(rd.cantidad) as CANT FROM t_rmaemp rm INNER JOIN t_rmaempdet rd on rm.folio = rd.folio " +
                "WHERE CAST(rm.fecha AS date) = CAST( '" + Convert.ToString(DateTime.Today) + "' AS date) and (rd.sku is not null AND rd.sku > '')" +
                "GROUP BY rd.sku ORDER BY 1";
                datos = AccesoDatos.Consultar(sSQL);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Listar(ReempaqueLogica eti)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("select * from t_rmaemp where folio = " + eti.Folio + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarDia()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT rd.folio,rd.consec,rd.planta AS PLANTA,rd.locacion AS LOCACION,rd.tarima AS TARIMA,rd.barcode AS ETIQUETA,rd.rpo AS RPO,rd.modelo AS MODELO,rd.cantidad as CANT,rd.sku as FG,rd.nota as NOTA,rd.tipo,rd.estado,rd.f_ingreso,rd.torder,rd.item FROM t_rmaemp rm INNER JOIN t_rmaempdet rd on rm.folio = rd.folio WHERE CAST(rm.fecha AS date) = CAST( '" + Convert.ToString(DateTime.Today) + "' AS date)");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static bool VerificarCapturaDia()
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_rmaemp WHERE CAST(fecha AS date) = CAST( " + Convert.ToString(DateTime.Now) + " AS date)";
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
    }
}
