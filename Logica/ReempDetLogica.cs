using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class ReempDetLogica
    {
        public long Folio { get; set; }
        public int Consec { get; set; }
        public string Planta { get; set; }
        public string Locacion { get; set; }
        public string Tarima { get; set; }
        public string Barcode { get; set; }
        public string RPO { get; set; }
        public string Modelo { get; set; }
        public string Item { get; set; }
        public string SKU { get; set; }
        public int Cantidad { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public string Nota { get; set; }
        public DateTime Fingreso { get; set; }
        public string TO { get; set; }
        public string Usuario { get; set; }
        public static int Guardar(ReempDetLogica eti)
        {
            string[] parametros = { "@Folio", "@Consec", "@Planta", "@Locacion", "@Tarima", "@Barcode", "@RPO", "@Modelo", "@Item", "@SKU", "@Cantidad", "@Tipo", "@Estado", "@Nota", "@Fingreso", "@TO", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_rmaempdet", parametros, eti.Folio, eti.Consec, eti.Planta, eti.Locacion,eti.Tarima,eti.Barcode, eti.RPO, eti.Modelo, eti.Item, eti.SKU, eti.Cantidad, eti.Tipo, eti.Estado, eti.Nota, eti.Fingreso, eti.TO, eti.Usuario);
        }

        public static DataTable Consultar(ReempDetLogica eti)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_rmaempdet where folio = "+eti.Folio+" and consec = "+eti.Consec+"");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Listar(ReempDetLogica eti)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("select * from t_ramempdet where folio="+eti.Folio+"");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(ReempDetLogica eti)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_rmaempdet WHERE folio = " + eti.Folio + " and consec = " + eti.Consec + "";
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

        public static bool Eliminar(ReempDetLogica eti)
        {
            try
            {
                string sQuery = "DELETE FROM t_rmaempdet WHERE  folio = " + eti.Folio + " and consec = " + eti.Consec + "";
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
