using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class RpoActDetLogica
    {
        public long Folio { get; set; }
        public int Consec { get; set; }
        public string Planta { get; set; }
        public string Linea { get; set; }
        public string RPO { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public string Surte { get; set; }
        public string Etiqueta { get; set; }
        public string Almacen { get; set; }
        public string Prioridad { get; set; }
        public string Entrega { get; set; }
        public string Turno { get; set; }
        public string WP { get; set; }
        public string Usuario { get; set; }
        public static int Guardar(RpoActDetLogica ubi)
        {
            string[] parametros = { "@Folio", "@Consec", "@Planta", "@Linea", "@RPO", "@Modelo", "@Cantidad", "@Surte", "@Etiqueta", "@Almacen", "@Prioridad", "@Entrega", "@Turno", "@WP", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_rpo_ubidet", parametros, ubi.Folio, ubi.Consec, ubi.Planta, ubi.Linea, ubi.RPO, ubi.Modelo, ubi.Cantidad, ubi.Surte, ubi.Etiqueta, ubi.Almacen, ubi.Prioridad, ubi.Entrega, ubi.Turno, ubi.WP, ubi.Usuario);
        }

        public static DataTable ConsultarRPO(RpoActDetLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_rpo_actdet WHERE folio = "+rpo.Folio+" and rpo = '" + rpo.RPO + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }


        public static bool Verificar(RpoActDetLogica ubi)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_rpo_ubidet WHERE planta = '"+ubi.Planta+"' ";
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
      
        public static bool Eliminar(RpoActDetLogica ubi)
        {
            try
            {
                string sQuery = "DELETE FROM t_rpo_ubidet WHERE planta = '"+ubi.Planta+"' ";
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
