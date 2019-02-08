using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class ItemsLogica
    {
        public string Item { get; set; }
        public string Descrip { get; set; }
        public string Modelo { get; set; }
        public string Usuario { get; set; }
        public static int Guardar(ItemsLogica Item)
        {
            string[] parametros = { "@Item", "@SKU", "@Descrip", "@Usuario"};
            return AccesoDatos.Actualizar("sp_mant_items", parametros, Item.Item, Item.Modelo, Item.Descrip, Item.Usuario );
        }

        public static DataTable Consultar(ItemsLogica Item)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_items WHERE item = '" + Item.Item + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaSKU(ItemsLogica Item)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_items WHERE sku = '" + Item.Modelo + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(ItemsLogica Item)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_items WHERE item = '" + Item.Item + "'";
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
        public static bool VerificarSKU(ItemsLogica Item)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_items WHERE sku = '" + Item.Modelo + "'";
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
