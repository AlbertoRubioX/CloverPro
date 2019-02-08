using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class EtiquetaLogica
    {
        public string Planta { get; set; }
        public string RPO { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public int Inicio { get; set; }
        public int Fin { get; set; }
        public string Estacion { get; set; }
        public string Usuario { get; set; }
        public string Leyenda { get; set; }
        public string Tipo { get; set; }
        public static int Guardar(EtiquetaLogica eti)
        {
            string[] parametros = { "@Planta", "@RPO", "@Modelo", "@Cantidad", "@Inicio", "@Fin", "@Estacion", "@Usuario", "@Leyenda", "@Tipo" };
            return AccesoDatos.Actualizar("sp_mant_etiqueta", parametros, eti.Planta, eti.RPO, eti.Modelo, eti.Cantidad, eti.Inicio, eti.Fin, eti.Estacion, eti.Usuario, eti.Leyenda, eti.Tipo);
        }

        public static DataTable Consultar()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_etiqueta");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Listar(EtiquetaLogica eti)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("select pl.nombre as Planta,et.usuario as Usuario,et.folio as Folio,et.fecha as Fecha,et.cantidad as Cantidad,et.f_ini as Inicio,et.f_fin as Fin,et.modelo from t_etiqueta et inner join t_plantas pl on et.planta = pl.planta where et.rpo = '" + eti.RPO + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool VerificarRango(EtiquetaLogica eti)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_etiqueta WHERE rpo = '" + eti.RPO + "' and f_fin >= " + Convert.ToString(eti.Inicio) + "";
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
